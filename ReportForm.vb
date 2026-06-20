Imports System
Imports System.Data
Imports System.IO
Imports System.Text
Imports System.Xml
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports PdfSharp.Drawing
Imports PdfSharp.Fonts
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports PdfSharp.Printing
Imports Pm

Public Class ReportForm

    Private servicesTable As Data.DataTable

    Private xmlPath As String = My.Application.Info.DirectoryPath & "\data.xml"


    Private Function GetThursdayOfWeek(date1 As Date) As Date
        Dim daysToThursday As Integer = (4 - date1.DayOfWeek + 7) Mod 7
        If daysToThursday = 0 Then Return date1
        Return date1.AddDays(-daysToThursday)  ' Назад к четвергу этой недели
    End Function

    Private Function GetLastThursdayOfYear(lastDay As Date) As Date
        Dim current As Date = lastDay
        While current.DayOfWeek <> DayOfWeek.Thursday
            current = current.AddDays(-1)
        End While
        Return current
    End Function

    Private Function GetFirstThursdayAfter(date1 As Date) As Date
        Dim d As Date = date1
        While d.DayOfWeek <> DayOfWeek.Thursday
            d = d.AddDays(1)
        End While
        Return d
    End Function

    Private Function GetFirstThursdayOnOrAfter(date1 As Date) As Date
        Dim d As Date = date1
        While d.DayOfWeek <> DayOfWeek.Thursday
            d = d.AddDays(1)
        End While
        Return d
    End Function

    Private Function GetLastThursdayOnOrBefore(date1 As Date) As Date
        Dim d As Date = date1
        While d.DayOfWeek <> DayOfWeek.Thursday
            d = d.AddDays(-1)
        End While
        Return d
    End Function

    Private Sub ReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Инициализация таблицы данных
        servicesTable = New Data.DataTable("Services")
        servicesTable.Columns.Add("Id", GetType(Integer))
        servicesTable.Columns.Add("Name", GetType(String))
        servicesTable.Columns.Add("Url", GetType(String))
        servicesTable.Columns.Add("Comment", GetType(String))
        servicesTable.Columns.Add("MonthlyPayment", GetType(Integer))
        servicesTable.Columns.Add("PaymentDay", GetType(Integer))
        servicesTable.Columns.Add("LastPaymentDate", GetType(Date))
        servicesTable.Columns.Add("itemPrice", GetType(Decimal))
        servicesTable.Columns.Add("WeeklyCheck", GetType(Boolean))
        'Загрузка данных из XML
        LoadServicesFromXml(xmlPath)
        GridDataChanged()
    End Sub

    Private Sub GridDataChanged()

        If dgvServices.Rows.Count <= 0 Then

            btnExportToExcel.Enabled = False
            btnExportPdf.Enabled = False


        Else
            btnExportToExcel.Enabled = True
            btnExportPdf.Enabled = True

        End If
    End Sub


    Private Sub LoadServicesFromXml(filePath As String)
        If Not File.Exists(filePath) Then
            MessageBox.Show("Файл XML не найден: " & filePath, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim doc As New XmlDocument()
        doc.Load(filePath)

        For Each node As XmlNode In doc.SelectNodes("/Services/Service")
            Dim row As DataRow = servicesTable.NewRow()

            row("Id") = Convert.ToInt32(node("Id").InnerText)
            row("Name") = node("Name").InnerText
            row("Url") = node("Url").InnerText
            row("Comment") = node("Comment").InnerText
            row("MonthlyPayment") = Convert.ToInt32(node("MonthlyPayment").InnerText)
            row("PaymentDay") = Convert.ToInt32(node("PaymentDay").InnerText)
            row("LastPaymentDate") = Date.Parse(node("LastPaymentDate").InnerText)
            row("itemPrice") = Decimal.Parse(node("itemPrice").InnerText)
            row("WeeklyCheck") = Boolean.Parse(node("WeeklyCheck").InnerText)

            servicesTable.Rows.Add(row)
        Next

        For i = 0 To dgvServices.Columns.Count - 1
            dgvServices.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

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
            Dim excelApp = New Application()
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



    Private Sub btnGenerateReport1_Click(sender As Object, e As EventArgs) Handles btnGenerateReport1.Click
        ' 1. Проверка периода
        If dtpStartDate.Value.Date > dtpEndDate.Value.Date Then
            MsgBox("Начальная дата не может быть позже конечной.", vbExclamation)
            GridDataChanged()
            Return
        End If

        Dim reportStartDate As Date = dtpStartDate.Value.Date
        Dim reportEndDate As Date = dtpEndDate.Value.Date

        ' 2. Проверка источника данных
        If servicesTable Is Nothing OrElse servicesTable.Rows.Count = 0 Then
            MsgBox("Нет данных для формирования отчёта.", vbInformation)
            GridDataChanged()
            Return
        End If

        ' 3. Подготовка DataGridView
        dgvServices.SuspendLayout()
        dgvServices.Rows.Clear()
        dgvServices.Columns.Clear()


        For i = 0 To dgvServices.Columns.Count - 1
            dgvServices.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        ' 4. Создание столбцов
        Dim colNum As New DataGridViewTextBoxColumn()
        colNum.Name = "NumCol"
        colNum.HeaderText = "№"
        colNum.Width = 50
        colNum.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        Dim colName As New DataGridViewTextBoxColumn()
        colName.Name = "LinkCol"
        colName.HeaderText = "Название услуги"
        colName.Width = 200


        Dim colComment As New DataGridViewTextBoxColumn()
        colComment.Name = "CommentCol"
        colComment.HeaderText = "Комментарий"
        colComment.Width = 400

        Dim colDate As New DataGridViewTextBoxColumn()
        colDate.Name = "DateCol"
        colDate.HeaderText = "Дата платежа"
        colDate.Width = 100
        colDate.DefaultCellStyle.Format = "dd.MM.yyyy"

        Dim colSum As New DataGridViewTextBoxColumn()
        colSum.Name = "SumCol"
        colSum.HeaderText = "Сумма платежа"
        colSum.DefaultCellStyle.Format = "N2"
        colSum.Width = 120
        colSum.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


        dgvServices.Columns.AddRange({colNum, colName, colComment, colDate, colSum})


        Dim globalRowNumber As Integer = 1

        ' 5. Собираем четверги (начала недель)
        Dim allWeekStarts As New List(Of Date)
        Try
            Dim firstThursday As Date = GetFirstThursdayOnOrAfter(reportStartDate)
            Dim lastThursday As Date = GetLastThursdayOnOrBefore(reportEndDate)

            If firstThursday > reportEndDate Then
                MsgBox("Нет недель в выбранном периоде.", vbInformation)
                dgvServices.ResumeLayout()
                Return
            End If

            Dim currentThursday As Date = firstThursday
            While currentThursday <= lastThursday
                allWeekStarts.Add(currentThursday)
                currentThursday = currentThursday.AddDays(7)
            End While
        Catch ex As Exception
            MsgBox($"Ошибка при расчёте недель: {ex.Message}", vbExclamation)
            dgvServices.ResumeLayout()
            GridDataChanged()
            Return
        End Try



        ' 6. Заполняем DataGridView по неделям
        For Each weekStart As Date In allWeekStarts
            Dim weekEnd As Date = weekStart.AddDays(6)
            Dim weeklyTotal As Decimal = 0D
            Dim paymentsThisWeek As New List(Of DataRow)

            Dim headerIndex As Integer = dgvServices.Rows.Add()
            dgvServices.Rows(headerIndex).Cells("NumCol").Value = Nothing
            dgvServices.Rows(headerIndex).Cells("LinkCol").Value = $"Неделя с {weekStart:dd.MM.yyyy} по {weekEnd:dd.MM.yyyy}"
            dgvServices.Rows(headerIndex).Cells("CommentCol").Value = Nothing
            dgvServices.Rows(headerIndex).Cells("DateCol").Value = Nothing
            dgvServices.Rows(headerIndex).Cells("SumCol").Value = Nothing

            'dgvServices.Rows(headerIndex).DefaultCellStyle.Font = New Font(dgvServices.Font, FontStyle.Bold)
            dgvServices.Rows(headerIndex).DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
            globalRowNumber = 1

            For Each row As DataRow In servicesTable.Rows
                Try
                    Dim lastPayment As Date = CDate(row("LastPaymentDate"))
                    Dim monthlyInterval As Integer = CInt(row("MonthlyPayment"))
                    Dim paymentDay As Integer = CInt(row("PaymentDay"))
                    Dim itemPrice As Decimal = CDec(row("ItemPrice"))
                    Dim url As String = If(row("Url") IsNot Nothing, CStr(row("Url")), "")
                    Dim serviceName As String = If(row("Name") IsNot Nothing, CStr(row("Name")), "Не указано")
                    Dim comment As String = If(row("Comment") IsNot Nothing, CStr(row("Comment")), "")
                    '***добавлена переменная на еженедельную проверку****
                    Dim WeekCheck As Boolean = CBool(row("WeeklyCheck"))
                    ' MsgBox(row("WeeklyCheck").ToString())
                    Dim nextPayment As Date = lastPayment



                    Do

                        nextPayment = nextPayment.AddMonths(monthlyInterval)
                        If nextPayment > reportEndDate Then Exit Do

                        Dim correctedDay As Integer = Math.Min(paymentDay, Date.DaysInMonth(nextPayment.Year, nextPayment.Month))
                        nextPayment = New Date(nextPayment.Year, nextPayment.Month, correctedDay)

                        If nextPayment >= weekStart AndAlso nextPayment <= weekEnd AndAlso
                           nextPayment >= reportStartDate AndAlso nextPayment <= reportEndDate AndAlso
                           nextPayment >= lastPayment Then

                            Dim newRowIndex As Integer = dgvServices.Rows.Add()

                            'If WeekCheck = True Then


                            '    dgvServices.Rows(newRowIndex).Cells("NumCol").Value = globalRowNumber
                            '    dgvServices.Rows(newRowIndex).Cells("LinkCol").Value = serviceName
                            '    dgvServices.Rows(newRowIndex).Cells("LinkCol").Tag = url
                            '    dgvServices.Rows(newRowIndex).Cells("CommentCol").Value = comment
                            '    dgvServices.Rows(newRowIndex).Cells("DateCol").Value = nextPayment
                            '    dgvServices.Rows(newRowIndex).Cells("SumCol").Value = itemPrice

                            'End If


                            ' Dim newRowIndex As Integer = dgvServices.Rows.Add()
                            dgvServices.Rows(newRowIndex).Cells("NumCol").Value = globalRowNumber
                            dgvServices.Rows(newRowIndex).Cells("LinkCol").Value = serviceName
                            dgvServices.Rows(newRowIndex).Cells("LinkCol").Tag = url
                            dgvServices.Rows(newRowIndex).Cells("CommentCol").Value = comment
                            dgvServices.Rows(newRowIndex).Cells("DateCol").Value = nextPayment
                            dgvServices.Rows(newRowIndex).Cells("SumCol").Value = itemPrice


                            globalRowNumber += 1
                            weeklyTotal += itemPrice




                            paymentsThisWeek.Add(row)



                        End If
                    Loop
                Catch ex As Exception

                    MsgBox($"Ошибка обработки услуги '{""}': {ex.Message}", vbExclamation)
                    GridDataChanged()
                End Try
            Next

            ' Добавляем заголовок и итог недели, если есть платежи
            If paymentsThisWeek.Count > 0 Then


                ' Итог за неделю
                Dim totalIndex As Integer = dgvServices.Rows.Add()
                dgvServices.Rows(totalIndex).Cells("NumCol").Value = Nothing
                dgvServices.Rows(totalIndex).Cells("LinkCol").Value = Nothing
                dgvServices.Rows(totalIndex).Cells("CommentCol").Value = Nothing
                dgvServices.Rows(totalIndex).Cells("DateCol").Value = "Итого за неделю:"
                dgvServices.Rows(totalIndex).Cells("SumCol").Value = weeklyTotal


                '  dgvServices.Rows(totalIndex).DefaultCellStyle.Font = New Font(dgvServices.Font, FontStyle.Bold)
                dgvServices.Rows(totalIndex).DefaultCellStyle.BackColor = Color.LightGray
            End If
        Next


        ' 7. Обработчик клика по ссылке
        'RemoveHandler dgvServices.CellContentClick, AddressOf dgvServices_CellContentClick
        'AddHandler dgvServices.CellContentClick, AddressOf dgvServices_CellContentClick


        ' 8. Завершаем обновление интерфейса
        ' dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        '  dgvServices.ResizeColumns()
        dgvServices.ResumeLayout()
        GridDataChanged()

        ' 9. Статус
        MsgBox("Отчёт сформирован!", vbInformation, "Finish")
    End Sub

    Private Sub dgvServices_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvServices.CellContentClick
        ' 1. Проверяем корректность индекса
        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        ' 2. Определяем, кликнули ли по колонке-ссылке
        Dim colName As String = dgvServices.Columns(e.ColumnIndex).Name
        If colName <> "LinkColumn! " Then Exit Sub  ' Не наша колонка — выходим

        ' 3. Получаем URL из скрытого столбца "Url"
        Dim urlCell As DataGridViewCell = dgvServices.Rows(e.RowIndex).Cells("Url")
        If urlCell.Value Is Nothing OrElse String.IsNullOrWhiteSpace(urlCell.Value.ToString()) Then
            MsgBox("Ссылка не указана.", vbExclamation, "Ошибка")
            Exit Sub
        End If

        Dim url As String = urlCell.Value.ToString().Trim()

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
    End Sub



    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        If dgvServices.RowCount = 0 Then
            MsgBox("Нет данных для экспорта.", vbExclamation)
            Return
        End If

        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "PDF файлы (*.pdf)|*.pdf|Все файлы (*.*)|*.*"
        saveDialog.FileName = "Отчёт_по_платежам_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
        saveDialog.Title = "Сохранить отчёт в PDF"


        If saveDialog.ShowDialog() <> DialogResult.OK Then Exit Sub


        Try
            ExportDataGridViewToPdf(dgvServices, saveDialog.FileName)
            ' Process.Start(saveDialog.FileName)
        Catch ex As Exception
            MsgBox($"Ошибка при экспорте: {ex.Message}", vbCritical)
        End Try
    End Sub

    'Private Function GetRelativeWidths(dgv As DataGridView) As Single()
    '    Dim totalWidth As Integer = dgv.DisplayRectangle.Width  ' Общая ширина видимой части
    '    Dim relWidths As New List(Of Single)


    '    For Each col As DataGridViewColumn In dgv.Columns
    '        Dim rel As Single = CSng(col.Width) / totalWidth
    '        relWidths.Add(rel)
    '    Next

    '    Return relWidths.ToArray()
    'End Function

    Private Sub ExportDataGridViewToPdf(dgv As DataGridView, filePath As String)
        ShowProgressForm("Экспорт в PDF (iTextSharp)...", 0)

        Try



            ' 1. Создаём документ и writer
            Dim document As New Document(PageSize.A4, 40, 40, 30, 30)
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))
            document.Open()

            ' 2. Настраиваем шрифты
            Dim baseFont As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, False)
            Dim fontNormal As New iTextSharp.text.Font(baseFont, 9, 0, BaseColor.BLACK)
            Dim fontBold As New iTextSharp.text.Font(baseFont, 9, 1, BaseColor.BLACK)

            '            Dim baseFont As BaseFont = BaseFont.CreateFont(
            '    "C:\Windows\Fonts\arial.ttf",  ' путь к шрифту
            '    BaseFont.WINANSI,                ' или BaseFont.IDENTITY_H для Unicode
            '    True                               ' встраивать шрифт в PDF
            ')
            Dim font As New iTextSharp.text.Font(baseFont, 9, 0, BaseColor.BLACK)


            ' 3. Создаём таблицу с количеством колонок как в DataGridView
            Dim table As New PdfPTable(dgv.Columns.Count)
            'table.WidthPercentage = 100

            'Dim relWidths As Single() = GetRelativeWidths(dgv)
            'table.SetWidths(relWidths)


            '  table.SetWidths(Enumerable.Repeat(1.0F, dgv.Columns.Count).ToArray())  ' Равномерное распределение


            ' 4. Добавляем заголовки столбцов (первая строка)
            For colIndex As Integer = 0 To dgv.Columns.Count - 1
                Dim cell As New PdfPCell(New Phrase(dgv.Columns(colIndex).HeaderText, fontBold))
                cell.BackgroundColor = New BaseColor(220, 220, 220)  ' Светло‑серый фон
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                cell.Border = iTextSharp.text.Rectangle.BOX
                cell.BorderWidth = 1
                table.AddCell(cell)
            Next

            ' 5. Добавляем строки данных
            Dim totalRows As Integer = dgv.Rows.Count
            For rowIndex As Integer = 0 To totalRows - 1
                For colIndex As Integer = 0 To dgv.Columns.Count - 1
                    Dim cellValue = dgv.Rows(rowIndex).Cells(colIndex).Value
                    Dim displayText As String


                    ' Форматируем дату в коротком формате (если это дата и колонка 3)
                    If colIndex = 3 AndAlso TypeOf cellValue Is Date Then
                        displayText = CDate(cellValue).ToShortDateString()
                    Else
                        displayText = If(cellValue?.ToString(), "")
                    End If

                    ' Определяем цвет фона и текста
                    Dim bgColor As BaseColor
                    Dim textColor As BaseColor = BaseColor.BLACK


                    If dgv.Rows(rowIndex).Cells(1).Value?.ToString().StartsWith("Неделя с ") Then
                        bgColor = New BaseColor(50, 50, 50)    ' Тёмно‑серый
                        textColor = BaseColor.WHITE
                    ElseIf dgv.Rows(rowIndex).Cells(3).Value?.ToString().StartsWith("Итого за ") Then
                        bgColor = New BaseColor(220, 220, 220) ' Светло‑серый
                        textColor = BaseColor.BLACK
                    Else
                        bgColor = BaseColor.WHITE                    ' Белый (по умолчанию)
                        textColor = BaseColor.BLACK
                    End If

                    ' Создаём ячейку
                    Dim cell As New pdf.PdfPCell(New Phrase(displayText, New iTextSharp.text.Font(baseFont, 9, 0, textColor)))
                    cell.BackgroundColor = bgColor
                    cell.HorizontalAlignment = Element.ALIGN_CENTER
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.Border = iTextSharp.text.Rectangle.BOX
                    cell.BorderWidth = 1

                    cell.Padding = 5


                    table.AddCell(cell)
                Next

                UpdateProgress((rowIndex + 1) / totalRows * 100)
            Next

            ' 6. Добавляем таблицу в документ
            document.Add(table)

            ' 7. Закрываем документ
            document.Close()
            writer.Close()

            MessageBox.Show("PDF‑файл создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Shell(na)
        Catch ex As Exception
            MessageBox.Show($"Ошибка при экспорте в PDF: {ex.Message}", "Ошибка",
                       MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CloseProgressForm()
        End Try
    End Sub




    Function EncodingHack(ByVal str As String) As String
        Dim encoding = System.Text.Encoding.BigEndianUnicode
        Dim bytes = encoding.GetBytes(str)
        Dim sb = New StringBuilder()
        sb.Append(ChrW(254))
        sb.Append(ChrW(255))

        For i As Integer = 0 To bytes.Length - 1
            sb.Append(ChrW(bytes(i)))
        Next

        Return sb.ToString()
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*"
        saveDialog.FileName = "Отчёт.xlsx"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            ExportDataGridViewToExcel(dgvServices, saveDialog.FileName)
        End If
    End Sub
End Class

