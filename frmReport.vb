Imports System.IO
Imports System.Xml
Imports System.Windows.Forms
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports DataTable = System.Data.DataTable
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO


Public Class frmReport
    Private servicesTable As DataTable
    Private xmlPath As String = My.Application.Info.DirectoryPath & "\data.xml"
    Private Sub frmReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Инициализация таблицы данных
        servicesTable = New System.Data.DataTable("Services")
        servicesTable.Columns.Add("Id", GetType(Integer))
        servicesTable.Columns.Add("Name", GetType(String))
        servicesTable.Columns.Add("Url", GetType(String))
        servicesTable.Columns.Add("Comment", GetType(String))
        servicesTable.Columns.Add("MonthlyPayment", GetType(Integer))
        servicesTable.Columns.Add("PaymentDay", GetType(Integer))
        servicesTable.Columns.Add("LastPaymentDate", GetType(Date))
        servicesTable.Columns.Add("ItemPrice", GetType(Decimal))
        servicesTable.Columns.Add("WeeklyCheck", GetType(Boolean))

        ' Загрузка данных из XML

        LoadServicesFromXml(xmlPath)  ' Укажите путь к вашему XML\
        For i = 0 To dgvReport.Columns.Count - 1
            dgvReport.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

    End Sub

    Private Sub LoadServicesFromXml(filePath As String)
        If Not File.Exists(filePath) Then
            MsgBox("Файл XML не найден: " & filePath, vbExclamation)
            Return
        End If

        Dim doc As New XmlDocument()
        doc.Load(filePath)

        For Each node As XmlNode In doc.SelectNodes("//Service")
            Dim row As DataRow = servicesTable.NewRow()

            row("Id") = Integer.Parse(node.SelectSingleNode("Id").InnerText)
            row("Name") = node.SelectSingleNode("Name").InnerText
            ' MsgBox(row("Name").ToString)
            row("Url") = node.SelectSingleNode("Url").InnerText
            row("Comment") = node.SelectSingleNode("Comment").InnerText
            row("MonthlyPayment") = Integer.Parse(node.SelectSingleNode("MonthlyPayment").InnerText)
            row("PaymentDay") = Integer.Parse(node.SelectSingleNode("PaymentDay").InnerText)
            row("LastPaymentDate") = Date.Parse(node.SelectSingleNode("LastPaymentDate").InnerText)
            'MsgBox(row("itemPrice").ToString)
            row("itemPrice") = Decimal.Parse(node.SelectSingleNode("itemPrice").InnerText)
            row("WeeklyCheck") = Boolean.Parse(node.SelectSingleNode("WeeklyCheck").InnerText)

            servicesTable.Rows.Add(row)
        Next

    End Sub

    Private Function GetFirstFridayOnOrAfter(startDate As Date) As Date
        Dim dayOfWeek As Integer = CInt(startDate.DayOfWeek)
        Dim daysToAdd As Integer = (5 - dayOfWeek) Mod 7  ' Пятница = 5 (в .NET: воскресенье=0)
        If daysToAdd = 0 Then daysToAdd = 7
        Return startDate.AddDays(daysToAdd)
    End Function

    Private Function GetLastFridayOnOrBefore(endDate As Date) As Date
        Dim dayOfWeek As Integer = CInt(endDate.DayOfWeek)
        Dim daysToSubtract As Integer = (dayOfWeek - 5 + 7) Mod 7
        If daysToSubtract = 0 Then Return endDate
        Return endDate.AddDays(-daysToSubtract)
    End Function

    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click



        ' 1. Проверка дат
        If dtpStartDate.Value.Date > dtpEndDate.Value.Date Then
            MsgBox("Начальная дата не может быть позже конечной.", vbExclamation)
            Return
        End If

        Dim reportStartDate As Date = dtpStartDate.Value.Date
        Dim reportEndDate As Date = dtpEndDate.Value.Date

        ' 2. Проверка данных
        If servicesTable Is Nothing OrElse servicesTable.Rows.Count = 0 Then
            MsgBox("Нет данных для формирования отчёта.", vbInformation)
            Return
        End If

        ' 3. Подготовка DataGridView
        dgvReport.SuspendLayout()
        dgvReport.Rows.Clear()
        dgvReport.Columns.Clear()



        ' Создание столбцов
        Dim colPeriod As New DataGridViewTextBoxColumn() With {
            .Name = "PeriodCol", .HeaderText = "Период (неделя)", .Width = 150
        }

        Dim colName As New DataGridViewTextBoxColumn() With {
            .Name = "NameCol", .HeaderText = "Название услуги", .Width = 200
        }

        Dim colComment As New DataGridViewTextBoxColumn() With {
            .Name = "CommentCol", .HeaderText = "Комментарий", .Width = 300
        }

        Dim colLastPayment As New DataGridViewTextBoxColumn() With {
            .Name = "LastPaymentCol", .HeaderText = "Последняя оплата", .Width = 100,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "dd.MM.yyyy"}
        }

        Dim colPrice As New DataGridViewTextBoxColumn() With {
            .Name = "PriceCol", .HeaderText = "Сумма", .Width = 100,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "N2"}
        }

        dgvReport.Columns.AddRange({colPeriod, colName, colComment, colLastPayment, colPrice})


        ' 4. Собираем пятницы (начала недель)
        Dim allWeekStarts As New List(Of Date)
        Try
            Dim firstFriday As Date = GetFirstFridayOnOrAfter(reportStartDate)
            Dim lastFriday As Date = GetLastFridayOnOrBefore(reportEndDate)

            If firstFriday > reportEndDate Then
                MsgBox("Нет недель в выбранном периоде.", vbInformation)
                dgvReport.ResumeLayout()
                Return
            End If

            Dim currentFriday As Date = firstFriday
            While currentFriday <= lastFriday
                allWeekStarts.Add(currentFriday)
                currentFriday = currentFriday.AddDays(7)
            End While
        Catch ex As Exception
            MsgBox($"Ошибка при расчёте недель: {ex.Message}", vbExclamation)
            dgvReport.ResumeLayout()
            Return
        End Try

        ' 5. Прогресс-бар и таймер (упрощённый вариант)
        Dim progressForm As New frmProgress()
        progressForm.Show()
        progressForm.ProgressValue = 0

        Dim totalWeeks As Integer = allWeekStarts.Count
        Dim processedWeeks As Integer = 0

        ' 6. Заполняем отчёт по неделям
        For Each weekStart As Date In allWeekStarts
            processedWeeks += 1
            progressForm.ProgressValue = CInt((processedWeeks / totalWeeks) * 100)
            progressForm.Update()

            Dim weekEnd As Date = weekStart.AddDays(6)
            Dim weeklyTotal As Decimal = 0D

            ' Заголовок недели (тёмно‑серый фон)
            Dim headerIndex As Integer = dgvReport.Rows.Add()
            dgvReport.Rows(headerIndex).Cells("PeriodCol").Value = $"с {weekStart:dd.MM.yyyy} по {weekEnd:dd.MM.yyyy}"
            dgvReport.Rows(headerIndex).DefaultCellStyle.BackColor = Color.DarkGray
            dgvReport.Rows(headerIndex).DefaultCellStyle.ForeColor = Color.White  ' Белый текст для контраста
            dgvReport.Rows(headerIndex).ReadOnly = True

            For Each row As DataRow In servicesTable.Rows
                Dim serviceName As String = row("Name").ToString()
                Dim comment As String = row("Comment").ToString()
                Dim lastPayment As Date = CDate(row("LastPaymentDate"))
                Dim monthlyInterval As Integer = CInt(row("MonthlyPayment"))
                Dim paymentDay As Integer = CInt(row("PaymentDay"))
                Dim itemPrice As Decimal = CDec(row("ItemPrice"))
                Dim url As String = If(row("Url") IsNot Nothing, row("Url").ToString(), "")
                Dim WeekCheck As Boolean = CBool(row("WeeklyCheck"))

                ' Для записей с WeeklyCheck = True — выводим КАЖДУЮ неделю (курсив + подчёркивание)
                If WeekCheck Then
                    Dim newRowIndex As Integer = dgvReport.Rows.Add()
                    dgvReport.Rows(newRowIndex).Cells("PeriodCol").Value = Nothing
                    dgvReport.Rows(newRowIndex).Cells("NameCol").Value = serviceName
                    dgvReport.Rows(newRowIndex).Cells("NameCol").Tag = url
                    dgvReport.Rows(newRowIndex).Cells("CommentCol").Value = comment
                    dgvReport.Rows(newRowIndex).Cells("LastPaymentCol").Value = lastPayment
                    dgvReport.Rows(newRowIndex).Cells("PriceCol").Value = itemPrice


                    ' Стилизация: курсив + подчёркивание
                    dgvReport.Rows(newRowIndex).DefaultCellStyle.Font = New System.Drawing.Font(dgvReport.Font, FontStyle.Italic Or FontStyle.Underline)
                    dgvReport.Rows(newRowIndex).DefaultCellStyle.ForeColor = Color.Blue  ' Дополнительный акцент

                    weeklyTotal += itemPrice
                Else
                    ' Для WeeklyCheck = False — рассчитываем даты платежей
                    Dim nextPayment As Date = lastPayment
                    Do
                        nextPayment = nextPayment.AddMonths(monthlyInterval)
                        If nextPayment > reportEndDate Then Exit Do


                        ' Корректировка дня платежа
                        Dim correctedDay As Integer = Math.Min(paymentDay, Date.DaysInMonth(nextPayment.Year, nextPayment.Month))
                        nextPayment = New Date(nextPayment.Year, nextPayment.Month, correctedDay)


                        If nextPayment >= weekStart AndAlso nextPayment <= weekEnd AndAlso
                   nextPayment >= reportStartDate AndAlso nextPayment <= reportEndDate AndAlso
                   nextPayment >= lastPayment Then


                            Dim newRowIndex As Integer = dgvReport.Rows.Add()
                            dgvReport.Rows(newRowIndex).Cells("PeriodCol").Value = Nothing
                            dgvReport.Rows(newRowIndex).Cells("NameCol").Value = serviceName
                            dgvReport.Rows(newRowIndex).Cells("NameCol").Tag = url
                            dgvReport.Rows(newRowIndex).Cells("CommentCol").Value = comment
                            dgvReport.Rows(newRowIndex).Cells("LastPaymentCol").Value = nextPayment
                            dgvReport.Rows(newRowIndex).Cells("PriceCol").Value = itemPrice


                            weeklyTotal += itemPrice
                        End If
                    Loop
                End If
            Next

            ' Итог за неделю (светло‑серый фон)
            If weeklyTotal > 0D Then
                Dim totalIndex As Integer = dgvReport.Rows.Add()
                dgvReport.Rows(totalIndex).Cells("LastPaymentCol").Value = "Сумма итого:"
                dgvReport.Rows(totalIndex).Cells("PriceCol").Value = weeklyTotal
                dgvReport.Rows(totalIndex).DefaultCellStyle.BackColor = Color.LightGray
                dgvReport.Rows(totalIndex).ReadOnly = True
            End If
        Next

        ' 7. Завершение
        dgvReport.ResumeLayout()
        progressForm.Close()

        For i = 0 To dgvReport.Columns.Count - 1
            dgvReport.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        MsgBox("Отчёт сформирован!", vbInformation, "Готово")
    End Sub

    ' Обработчик клика по ссылке в колонке "Название услуги"
    Private Sub dgvReport_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReport.CellContentClick

        Dim url As String = TryCast(dgvReport.Rows(e.RowIndex).Cells("NameCol").Tag, String)

        ' Dim url As String = urlCell.Value.ToString().Trim()

        ' 4. Валидируем и нормализуем URL
        If Not url.StartsWith("http://") And Not url.StartsWith("https://") Then
            ' Добавляем протокол (предпочитаем HTTPS)
            url = "https://" & url
        End If

        ' 5. Проверяем, что URL валидный
        Dim uriResult As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uriResult) Then
            MsgBox($"Некорректный URL: {url}", vbExclamation, "Ошибка ссылки")
            Exit Sub
        End If

        ' 6. Открываем ссылку в браузере
        Try
            Process.Start(New ProcessStartInfo With {
                .FileName = uriResult.ToString(),
                .UseShellExecute = True  ' Важно: открывает в системном браузере
            })
        Catch ex As Exception
            MsgBox($"Не удалось открыть ссылку:{vbCrLf}{uriResult.ToString()}{vbCrLf}Ошибка: {ex.Message}",
                  vbCritical, "Ошибка браузера")
        End Try
        'If e.ColumnIndex = dgvReport.Columns("NameCol").Index AndAlso e.RowIndex >= 0 Then

        '    If Not String.IsNullOrEmpty(url) Then
        '        Try
        '            System.Diagnostics.Process.Start(url)
        '        Catch ex As Exception
        '            MsgBox($"Не удалось открыть ссылку: {ex.Message}", vbExclamation)
        '        End Try
        '    End If
        'End If
    End Sub

    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*"
        saveDialog.FileName = "Отчёт.xlsx"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            ExportDataGridViewToExcel(dgvReport, saveDialog.FileName)
        End If
    End Sub


    Private Sub ReleaseExcelObject(obj As Object)
        Try
            If obj IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
                obj = Nothing
            End If
        Catch
            obj = Nothing
        End Try
    End Sub

    Private Sub ExportDataGridViewToExcel(dgv As DataGridView, filePath As String)
        ' Создаём форму прогресса
        ShowProgressForm("Экспорт в Excel...", 0)

        Try
            ' 1. Создаём приложение Excel
            Dim excelApp = New Excel.Application()
            excelApp.Visible = False

            ' 2. Добавляем рабочую книгу и лист
            Dim Workbook = excelApp.Workbooks.Add()
            Dim Worksheet = Workbook.ActiveSheet

            ' 3. Записываем заголовки столбцов
            For colIndex As Integer = 0 To dgv.Columns.Count - 1
                Worksheet.Cells(1, colIndex + 1) = dgv.Columns(colIndex).HeaderText
            Next

            ' 4. Записываем данные строк
            Dim totalRows As Integer = dgv.Rows.Count
            For rowIndex As Integer = 0 To totalRows - 1
                For colIndex As Integer = 0 To dgv.Columns.Count - 1
                    Dim cellValue = dgv.Rows(rowIndex).Cells(colIndex).Value

                    ' Форматируем дату в коротком формате (если это дата)
                    If colIndex = 3 AndAlso TypeOf cellValue Is Date Then  ' Предполагаем, что дата платежа в 4-м столбце (индекс 3)
                        Worksheet.Cells(rowIndex + 2, colIndex + 1) = CDate(cellValue).ToShortDateString()
                    Else
                        Worksheet.Cells(rowIndex + 2, colIndex + 1) = cellValue?.ToString()
                    End If
                Next

                ' Проверяем, является ли строка "Неделя с ... по ..."
                If dgv.Rows(rowIndex).Cells(1).Value?.ToString().StartsWith("Неделя с ") Then
                    Dim range As Excel.Range = Worksheet.Range("A" & (rowIndex + 2), Worksheet.Cells(rowIndex + 2, dgv.Columns.Count))
                    range.Interior.Color = XlRgbColor.rgbDarkGray  ' Тёмно-серый цвет
                    range.Font.Color = XlRgbColor.rgbWhite  ' Белый текст для контраста
                End If

                ' Проверяем, является ли строка "Неделя с ... по ..."
                If dgv.Rows(rowIndex).Cells(3).Value?.ToString().StartsWith("Итого за ") Then
                    Dim range As Excel.Range = Worksheet.Range("A" & (rowIndex + 2), Worksheet.Cells(rowIndex + 2, dgv.Columns.Count))
                    range.Interior.Color = XlRgbColor.rgbLightGrey ' Тёмно-серый цвет
                    range.Font.Color = XlRgbColor.rgbBlack  ' Белый текст для контраста
                End If

                UpdateProgress((rowIndex + 1) / totalRows * 100)
            Next

            ' 5. Форматируем заголовки
            Dim headerRange As Excel.Range = Worksheet.Range("A1", Worksheet.Cells(1, dgv.Columns.Count))
            headerRange.Font.Bold = True
            headerRange.Interior.Color = XlRgbColor.rgbLightGray
            headerRange.Borders.LineStyle = XlLineStyle.xlContinuous

            ' 6. Применяем границы ко всем данным
            Dim dataRange As Excel.Range = Worksheet.Range("A1", Worksheet.Cells(totalRows + 1, dgv.Columns.Count))
            dataRange.Borders.LineStyle = XlLineStyle.xlContinuous
            dataRange.Borders.Weight = XlBorderWeight.xlThin

            ' 7. Автоширина столбцов
            Worksheet.Columns.AutoFit()

            ' 8. Сохраняем файл
            Workbook.SaveAs(filePath)
            Workbook.Close(False)

            ' Освобождаем объекты
            ReleaseObject(Worksheet)
            ReleaseObject(Workbook)
            ReleaseObject(excelApp)

            MessageBox.Show("Excel‑файл создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CloseProgressForm()
        Catch ex As Exception
            MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                           MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CloseProgressForm()
        End Try
    End Sub

    ' Метод для освобождения COM-объектов
    Private Sub ReleaseObject(ByVal obj As Object)
        Try
            If obj IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
                obj = Nothing
            End If
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
    ' Создание формы прогресса
    Private ProgressForm = New Form()
    Private progressLabel = New System.Windows.Forms.Label()
    Private ProgressBar = New ProgressBar()
    Private Sub ShowProgressForm(title As String, initialValue As Integer)

        ' Если форма уже существует — закрываем её
        If ProgressForm IsNot Nothing Then
            Try
                ProgressForm.Close()
            Catch
                ' Игнорируем ошибки при закрытии (например, если форма уже закрыта)
            End Try
        End If
        ProgressForm = New Form()
        progressLabel = New System.Windows.Forms.Label()
        ProgressBar = New ProgressBar()

        ProgressForm.Text = title
        ProgressForm.StartPosition = FormStartPosition.CenterParent
        ProgressForm.FormBorderStyle = FormBorderStyle.FixedDialog
        ProgressForm.ControlBox = False
        ProgressForm.TopMost = True
        ProgressForm.Width = 400
        ProgressForm.Height = 120


        progressLabel.Text = "Выполняется экспорт..."
        progressLabel.Location = New System.Drawing.Point(10, 10)
        progressLabel.AutoSize = True


        ProgressBar.Minimum = 0
        ProgressBar.Maximum = 100
        ProgressBar.Value = initialValue
        ProgressBar.Location = New System.Drawing.Point(10, 40)
        ProgressBar.Width = ProgressForm.ClientSize.Width - 20

        ProgressForm.Controls.Add(progressLabel)
        ProgressForm.Controls.Add(ProgressBar)

        ProgressForm.Show()
    End Sub

    ' Обновление прогресса
    Private Sub UpdateProgress(value As Integer)
        ' Проверяем, что форма создана и имеет дескриптор
        If ProgressForm Is Nothing OrElse Not ProgressForm.IsHandleCreated Then
            Return  ' Выходим, если форма не готова
        End If

        ' Обновляем элементы через Invoke (потокобезопасно)
        If ProgressBar.InvokeRequired Then
            ProgressBar.Invoke(Sub() ProgressBar.Value = value)
        Else
            ProgressBar.Value = value
        End If

        If progressLabel.InvokeRequired Then
            progressLabel.Invoke(Sub() progressLabel.Text = $"Экспорт: {value}%")
        Else
            progressLabel.Text = $"Экспорт: {value}%"
        End If
    End Sub

    ' Закрытие формы прогресса
    Private Sub CloseProgressForm()
        If ProgressForm IsNot Nothing Then
            Try
                ProgressForm.Close()
                ProgressForm.Dispose()  ' Явно освобождаем ресурсы
            Catch
                ' Игнорируем ошибки при закрытии
            Finally
                ProgressForm = Nothing
                progressLabel = Nothing
                ProgressBar = Nothing
            End Try
        End If
    End Sub

    ' Вспомогательный метод: определяет номер столбца в Excel с учётом пропущенных ("Url")
    Private Function GetExcelColumnIndex(dgv As DataGridView, colIndex As Integer) As Integer
        Dim visibleIndex As Integer = 1  ' Начинаем с 1 (первый столбец Excel)
        For i As Integer = 0 To colIndex
            Dim col As DataGridViewColumn = dgv.Columns(i)
            If Not col.Name.Equals("Url", StringComparison.OrdinalIgnoreCase) Then
                visibleIndex += 1
            End If
        Next
        Return visibleIndex
    End Function

    Private Sub AddHyperlinksToServiceNames(
    worksheet As Microsoft.Office.Interop.Excel.Worksheet,
    grid As DataGridView,
    servicesData As Data.DataTable
)
        ' Находим индекс столбца "Название услуги" в DataGridView
        Dim serviceNameColIndex As Integer = -1
        For i As Integer = 0 To grid.Columns.Count - 1
            If String.Equals(grid.Columns(i).Name, "Название услуги", StringComparison.OrdinalIgnoreCase) OrElse
           String.Equals(grid.Columns(i).HeaderText, "Название услуги", StringComparison.OrdinalIgnoreCase) Then
                serviceNameColIndex = i
                Exit For
            End If
        Next

        If serviceNameColIndex = -1 Then Exit Sub  ' Столбец не найден


        ' Для каждой строки проверяем, является ли она платёжной (не заголовок недели и не итог)
        For rowIndex As Integer = 0 To grid.Rows.Count - 1
            Dim row As DataGridViewRow = grid.Rows(rowIndex)

            Dim weekCellValue As String = If(row.Cells("Неделя").Value?.ToString(), "")
            Dim numCellValue As String = If(row.Cells("№").Value?.ToString(), "")

            ' Пропускаем заголовки недель и итоговые строки
            If Not String.IsNullOrEmpty(weekCellValue) OrElse numCellValue = "-1" Then Continue For


            ' Получаем название услуги из текущей строки
            Dim serviceName As String = row.Cells(serviceNameColIndex).Value?.ToString()
            If String.IsNullOrEmpty(serviceName) Then Continue For


            ' Ищем URL в исходной таблице servicesTable
            Dim url As String = ""
            For Each dataRow As DataRow In servicesData.Rows
                If String.Equals(dataRow("Name").ToString(), serviceName, StringComparison.OrdinalIgnoreCase) Then
                    url = dataRow("Url").ToString()
                    Exit For
                End If
            Next

            If Not String.IsNullOrEmpty(url) Then
                Try
                    ' Добавляем гиперссылку
                    worksheet.Hyperlinks.Add(
                    Anchor:=worksheet.Cells(rowIndex + 2, serviceNameColIndex + 1),
                    Address:=url,
                    TextToDisplay:=serviceName
                )
                Catch ex As Exception
                    ' Если не удалось добавить гиперссылку — оставляем текст
                    worksheet.Cells(rowIndex + 2, serviceNameColIndex + 1) = serviceName
                End Try
            End If
        Next
    End Sub

    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        If dgvReport.Rows.Count = 0 Then
            MsgBox("Нет данных для экспорта.", vbExclamation)
            Return
        End If

        Using saveDialog As New SaveFileDialog()
            saveDialog.Filter = "PDF-файлы (*.pdf)|*.pdf"
            saveDialog.Title = "Сохранить PDF"
            saveDialog.FileName = "Отчёт_по_платежам.pdf"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Try
                    ExportWithPdfSharp(dgvReport, saveDialog.FileName)
                    MsgBox("Экспорт выполнен!", vbInformation, "Готово")
                Catch ex As Exception
                    MsgBox($"Ошибка: {ex.Message}", vbExclamation, "Ошибка экспорта")
                End Try
            End If
        End Using
    End Sub

    Private Sub ExportWithPdfSharp(grid As DataGridView, filePath As String)
        ' Создание PDF-документа
        Dim document As New PdfDocument()
        Dim page As PdfPage = document.Add(page)
        Dim gfx As XGraphics = XGraphics.FromPdfPage(page)

        ' Шрифты
        Dim titleFont As New XFont("Arial", 14, XFontStyle.Bold)
        Dim headerFont As New XFont("Arial", 10, XFontStyle.Bold)
        Dim normalFont As New XFont("Arial", 9, XFontStyle.Regular)
        Dim italicFont As New XFont("Arial", 9, XFontStyle.Italic)

        ' Цвета
        Dim darkGray As New XSolidBrush(XColor.FromArgb(64, 64, 64))
        Dim lightGray As New XSolidBrush(XColor.FromArgb(220, 220, 220))
        Dim blue As New XSolidBrush(XColors.Blue)
        Dim black As New XSolidBrush(XColors.Black)


        ' Отступы и позиции
        Dim margin As Double = 40
        Dim yPos As Double = margin
        Dim colWidths As Double() = {80, 120, 150, 80, 80}  ' Ширина колонок (мм)
        Dim totalWidth As Double = colWidths.Sum()


        ' --- Заголовок ---
        Dim title As String = "Отчёт по платежам"
        gfx.DrawString(title, titleFont, black, New XRect(margin, yPos, totalWidth, 30), XStringFormats.TopCenter)
        yPos += 40


        ' --- Таблица ---
        For Each row As DataGridViewRow In grid.Rows
            If row.IsNewRow Then Continue For


            ' Определение стиля строки
            Dim isHeader As Boolean = row.Cells("PeriodCol").Value?.ToString().StartsWith("с ")
            Dim isTotal As Boolean = row.Cells("PeriodCol").Value?.ToString() = "Итого за неделю:"
            Dim isWeekly As Boolean = CBool(grid.Rows(row.Index).Cells("NameCol").Tag)

            Dim currentFont As XFont = If(isWeekly, italicFont, normalFont)
            Dim textBrush As XSolidBrush = If(isWeekly, blue, black)


            For colIndex As Integer = 0 To grid.Columns.Count - 2
                Dim cellValue As String = If(
                row.Cells(colIndex).Value IsNot Nothing,
                row.Cells(colIndex).Value.ToString(),
                ""
            )

                ' Позиция ячейки
                Dim xPos As Double = margin + colWidths.Take(colIndex).Sum()
                Dim rect As New XRect(xPos, yPos, colWidths(colIndex), 25)

                ' Фон для заголовков недель и итогов
                If isHeader Then
                    gfx.DrawRectangle(darkGray, rect)
                    textBrush = New XSolidBrush(XColors.White)
                    currentFont = New XFont("Arial", 9, XFontStyle.Bold)
                ElseIf isTotal Then
                    gfx.DrawRectangle(lightGray, rect)
                End If

                ' Текст
                gfx.DrawString(cellValue, currentFont, textBrush, rect, XStringFormats.Center)
            Next

            yPos += 25  ' Переход на следующую строку
        Next

        ' Сохранение PDF
        document.Save(filePath)
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
