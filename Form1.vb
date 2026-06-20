Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports System.Net
Imports System.Text.Encoding
Imports System.ComponentModel
Imports System.Data.Common
Imports System.DirectoryServices.ActiveDirectory

Public Class Form1
    Private xmlPath As String = My.Application.Info.DirectoryPath & "\data.xml"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDataGridView()
        LoadDataFromXml()
        DataGridView1.Columns.Item(0).Width = 53
        DataGridView1.Columns.Item(1).Width = 250
        DataGridView1.Columns.Item(2).Width = 250
        DataGridView1.Columns.Item(3).Width = 103
        DataGridView1.Columns.Item(4).Width = 100
        DataGridView1.Columns.Item(5).Width = 100
        DataGridView1.Columns.Item(6).Width = 100
        DataGridView1.Columns.Item(7).Width = 250

        DataGridView1.ContextMenuStrip = ContextMenuStrip1
        GridDataChanged()
    End Sub

    ' Инициализация DataGridView
    Private Sub InitializeDataGridView()



        DataGridView1.AutoGenerateColumns = False
        DataGridView1.ColumnCount = 7

        ' Столбец "П/П"
        DataGridView1.Columns(0).Name = "Id"
        DataGridView1.Columns(0).HeaderText = "П/П"
        DataGridView1.Columns(0).DataPropertyName = "Id"

        DataGridView1.Columns("Id").DisplayIndex = 0

        ' Столбец "Наименование услуги" (LinkLabel)
        Dim linkColumn As New DataGridViewLinkColumn()
        linkColumn.Name = "NameColumn"
        linkColumn.HeaderText = "Наименование услуги"
        linkColumn.DataPropertyName = "Name"
        linkColumn.UseColumnTextForLinkValue = False
        linkColumn.LinkBehavior = LinkBehavior.HoverUnderline
        DataGridView1.Columns.Add(linkColumn)

        DataGridView1.Columns("NameColumn").DisplayIndex = 1

        ' Столбец "Комментарии"
        DataGridView1.Columns(2).Name = "Comment"
        DataGridView1.Columns(2).HeaderText = "Комментарии"
        DataGridView1.Columns(2).DataPropertyName = "Comment"
        DataGridView1.Columns("Comment").DisplayIndex = 2

        ' Столбец "Ежемесячная оплата"
        DataGridView1.Columns(3).Name = "MonthlyPayment"
        DataGridView1.Columns(3).HeaderText = "Ежемесячная оплата (мес)"
        DataGridView1.Columns(3).DataPropertyName = "MonthlyPayment"
        DataGridView1.Columns("MonthlyPayment").DisplayIndex = 3

        ' Столбец "Оплата каждого числа"
        DataGridView1.Columns(4).Name = "PaymentDay"
        DataGridView1.Columns(4).HeaderText = "Оплата каждого числа"
        DataGridView1.Columns(4).DataPropertyName = "PaymentDay"
        DataGridView1.Columns("PaymentDay").DisplayIndex = 4

        ' Столбец "Дата последней оплаты"
        DataGridView1.Columns(5).Name = "LastPaymentDate"
        DataGridView1.Columns(5).HeaderText = "Дата последней оплаты"
        DataGridView1.Columns(5).DataPropertyName = "LastPaymentDate"
        DataGridView1.Columns("LastPaymentDate").DisplayIndex = 5

        ' Столбец "П/П"
        DataGridView1.Columns(6).Name = "itemPrice"
        DataGridView1.Columns(6).HeaderText = "Цена"
        DataGridView1.Columns(6).DataPropertyName = "itemPrice"

        DataGridView1.Columns("itemPrice").DisplayIndex = 6


        '' Скрытый столбец с URL
        DataGridView1.Columns(1).Name = "Url"
        DataGridView1.Columns(1).Visible = False
        DataGridView1.Columns(1).DataPropertyName = "Url"

        For i = 0 To DataGridView1.ColumnCount - 1
            DataGridView1.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next


        AddHandler DataGridView1.CellContentClick, AddressOf DataGridView1_CellContentClick
    End Sub

    ' Загрузка данных из XML
    Private Sub LoadDataFromXml()
        Try


            If File.Exists(xmlPath) = False Then
                CreateDefaultXml()
            End If

            Dim ds As New DataSet()
            ds.ReadXml(xmlPath)
            DataGridView1.DataSource = ds.Tables(0)
        Catch ex As Exception

            DataGridView1.DataSource = Nothing
        End Try
    End Sub

    ' Создание пустого XML
    Private Sub CreateDefaultXml()
        Dim doc As New XmlDocument()
        Dim root As XmlElement = doc.CreateElement("Services")
        doc.AppendChild(root)
        doc.Save(xmlPath)
    End Sub

    ' Обработчик клика по LinkLabel в DataGridView
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        ' Проверяем: клик по столбцу LinkLabel и валидная строка
        If e.ColumnIndex = DataGridView1.Columns("NameColumn").Index AndAlso e.RowIndex >= 0 Then
            Try
                ' Получаем URL из скрытого столбца "Url" текущей строки
                Dim url = DataGridView1.Rows(e.RowIndex).Cells("Url").Value?.ToString

                If Not String.IsNullOrEmpty(url) Then
                    ' Открываем ссылку в браузере по умолчанию
                    Process.Start(New ProcessStartInfo(url) With {
                        .UseShellExecute = True
                    })
                Else
                    MessageBox.Show("URL не указан.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            Catch ex As Exception
                MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' Кнопка "Добавить"
    Private Sub tsbAdd_Click_1(sender As Object, e As EventArgs) Handles tsbAdd.Click
        Dim editForm As New EditForm
        If editForm.ShowDialog = DialogResult.OK Then
            SaveToXml(editForm.GetServiceData)
            LoadDataFromXml()

            If DataGridView1.CurrentRow.Index > 0 Then
                R_position = DataGridView1.CurrentRow.Index
            End If

            DataGridView1.CurrentCell = DataGridView1.Rows(R_position).Cells(2)
            DataGridView1.Rows.Item(R_position).Selected = True
        End If

    End Sub

    Public R_position As Integer

    ' Кнопка "Редактировать"
    Private Sub tsbEdit_Click(sender As Object, e As EventArgs) Handles TsbEdit.Click
        If DataGridView1.CurrentRow Is Nothing Then
            MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        R_position = DataGridView1.CurrentRow.Index
        Dim row = DirectCast(DataGridView1.CurrentRow.DataBoundItem, DataRowView)
        Dim editForm As New EditForm(row)
        If editForm.ShowDialog = DialogResult.OK Then
            UpdateXml(editForm.GetServiceData, row("Id").ToString)
            LoadDataFromXml()
            DataGridView1.CurrentCell = DataGridView1.Rows(R_position).Cells(2)
            DataGridView1.Rows.Item(R_position).Selected = True
        End If

    End Sub

    ' Кнопка "Удалить"
    Private Sub tsbDelete_Click(sender As Object, e As EventArgs) Handles tsbDelete.Click
        Dim currentRow As DataGridViewRow = DataGridView1.CurrentRow

        If DataGridView1.CurrentRow Is Nothing Then
            MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim ant = DataGridView1.CurrentRow.Index
        Dim id = DataGridView1.CurrentRow.Cells("Id").Value.ToString

        Dim cellValue As Object = DataGridView1.CurrentCell.Value
        Dim txt As String

        'txt = DirectCast(cellValue, DataRowView)("Name").ToString
        txt = DataGridView1.CurrentRow.Cells(7).Value
        Dim result = MessageBox.Show($"Удалить запись: {txt}?", "Подтверждение удаления",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Exclamation
)

        If result = DialogResult.Yes Then

            ' R_position = DataGridView1.CurrentRow.Index
            DeleteFromXml(id)


            If DataGridView1.CurrentCell.RowIndex >= 2 Then
                DataGridView1.CurrentCell = DataGridView1.Rows(ant - 1).Cells(2)
                DataGridView1.Rows.Item(ant - 1).Selected = True
            End If

            LoadDataFromXml()
            DataGridView1.Refresh()
            ' Код для удаления записи (вставьте сюда вашу логику удаления)
            MessageBox.Show("Запись удалена.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            ' Действие при нажатии "Нет" (можно оставить пустым)
            MessageBox.Show("Удаление отменено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    ' Сохранение новой записи
    Private Sub SaveToXml(serviceData As Dictionary(Of String, String))
        Dim doc As New XmlDocument()
        doc.Load(xmlPath)

        Dim serviceNode As XmlNode = doc.CreateElement("Service")

        For Each kvp In serviceData
            Dim node As XmlNode = doc.CreateElement(kvp.Key)
            node.InnerText = kvp.Value
            serviceNode.AppendChild(node)
        Next

        doc.DocumentElement.AppendChild(serviceNode)
        doc.Save(xmlPath)
    End Sub

    ' Обновление записи
    Private Sub UpdateXml(serviceData As Dictionary(Of String, String), id As String)
        Dim doc As New XmlDocument()
        doc.Load(xmlPath)

        Dim node As XmlNode = doc.SelectSingleNode($"//Service[Id='{id}']")
        If node IsNot Nothing Then
            For Each kvp In serviceData
                Dim child As XmlNode = node.SelectSingleNode(kvp.Key)
                If child IsNot Nothing Then
                    child.InnerText = kvp.Value
                Else
                    Dim newChild As XmlNode = doc.CreateElement(kvp.Key)
                    newChild.InnerText = kvp.Value
                    node.AppendChild(newChild)
                End If
            Next
            doc.Save(xmlPath)
        End If
    End Sub

    ' Удаление записи
    Private Sub DeleteFromXml(id As String)
        Dim doc As New XmlDocument()
        doc.Load(xmlPath)

        Dim node As XmlNode = doc.SelectSingleNode($"//Service[Id='{id}']")
        If node IsNot Nothing Then
            node.ParentNode.RemoveChild(node)
            doc.Save(xmlPath)
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        TsbEdit.PerformClick()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        ' Проверяем: клик по столбцу LinkLabel и валидная строка
        If e.ColumnIndex = DataGridView1.Columns("NameColumn").Index AndAlso e.RowIndex >= 0 Then
            Try
                ' Получаем URL из скрытого столбца "Url" текущей строки
                Dim url = DataGridView1.Rows(e.RowIndex).Cells("Url").Value?.ToString

                If Not String.IsNullOrEmpty(url) Then
                    ' Открываем ссылку в браузере по умолчанию
                    Process.Start(New ProcessStartInfo(url) With {
                        .UseShellExecute = True
                    })
                Else
                    MessageBox.Show("URL не указан.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            Catch ex As Exception
                MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub tsbImport_Click_1(sender As Object, e As EventArgs) Handles tsbImport.Click

        ' Шаг 1. Диалоговое окно выбора CSV-файла
        Using ofd As New OpenFileDialog
            ofd.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*"
            ofd.Title = "Выберите CSV-файл для импорта"

            If ofd.ShowDialog <> DialogResult.OK Then
                MessageBox.Show("Файл не выбран.", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim csvPath = ofd.FileName

            ' Шаг 3. Читаем CSV и формируем DataTable
            Dim dt = ReadCsvToDataTable(csvPath, ","c) ' разделитель — запятая

            If dt.Rows.Count = 0 Then
                MessageBox.Show("CSV-файл пуст или не содержит данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Шаг 4. Определяем последний Id из существующего XML (если есть)
            Dim lastId = 0
            If File.Exists(xmlPath) Then
                Dim doc = XDocument.Load(xmlPath)
                Dim maxId = doc.Descendants("Id").Select(Function(x) CInt(x.Value)).DefaultIfEmpty(0).Max
                lastId = maxId
            End If

            ' Шаг 5. Создаём новый XML или дополняем существующий
            Dim root As XElement
            If Not File.Exists(xmlPath) Then
                root = <Services></Services>
            Else
                root = XDocument.Load(xmlPath).Root
            End If

            ' Шаг 6. Добавляем строки из CSV с новым Id
            For Each row As DataRow In dt.Rows
                Dim newId = lastId + 1
                lastId += 1

                Dim service =
                    <Service>
                        <Id><%= newId %></Id>
                        <Name><%= row("Name").ToString %></Name>
                        <Url><%= row("Url").ToString %></Url>
                        <Comment><%= row("Comment").ToString %></Comment>
                        <MonthlyPayment><%= row("MonthlyPayment").ToString %></MonthlyPayment>
                        <PaymentDay><%= row("PaymentDay").ToString %></PaymentDay>
                        <LastPaymentDate><%= row("LastPaymentDate").ToString %></LastPaymentDate>
                        <itemPrice><%= row("itemPrice").ToString %></itemPrice>
                    </Service>

                root.Add(service)
            Next

            ' Шаг 7. Сохраняем XML
            Dim xdoc As New XDocument(root)
            xdoc.Declaration = New XDeclaration("1.0", "windows-1251", Nothing)
            xdoc.Save(xmlPath)
            LoadDataFromXml()
            MessageBox.Show($"Данные успешно импортированы в {xmlPath}", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Using
        ' End Using
    End Sub

    Function ReadCsvToDataTable(filePath As String, delim As Char) As DataTable
        Dim dt As New DataTable()

        Using reader As New StreamReader(filePath, System.Text.Encoding.UTF8)
            ' Читаем первую строку — заголовки
            Dim headerLine As String = reader.ReadLine()
            If String.IsNullOrWhiteSpace(headerLine) Then Return dt

            Dim headers As String() = headerLine.Split(delim).Select(Function(s) s.Trim()).ToArray()
            For Each h In headers
                dt.Columns.Add(h, GetType(String))
            Next

            ' Читаем остальные строки
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine().Trim()
                If String.IsNullOrWhiteSpace(line) Then Continue While

                Dim cells As String() = line.Split(delim)
                Dim row As DataRow = dt.NewRow()

                For i = 0 To headers.Length - 1
                    If i < cells.Length Then
                        row(i) = cells(i).Trim()
                    Else
                        row(i) = String.Empty
                    End If
                Next

                dt.Rows.Add(row)
            End While
        End Using

        Return dt
    End Function




    Dim maxId As Integer = 0

    Private Function GenerateId() As String
        If Not File.Exists("data.xml") Then Return "1"

        Dim doc As New XmlDocument()
        doc.Load("data.xml")

        Dim nodes As XmlNodeList = doc.SelectNodes("//Service/Id")
        If nodes.Count = 0 Then Return "1"


        For Each node As XmlNode In nodes
            Dim id As Integer = Integer.Parse(node.InnerText)
            If id > maxId Then maxId = id
        Next
        'txtId.Text = (maxId + 1).ToString()
        Return (maxId + 1).ToString()
    End Function


    Private Sub DataGridView1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Call tsbEdit_Click(e, e)
    End Sub


    Private Sub tsbReport_Click(sender As Object, e As EventArgs) Handles tsbReport.Click
        frmReport.ShowDialog()

    End Sub

    Private Sub tsbHelp_Click(sender As Object, e As EventArgs) Handles tsbHelp.Click
        AboutBox1.ShowDialog()
    End Sub


    Private Sub DataGridView1_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved
        GridDataChanged()
    End Sub

    Private Sub GridDataChanged()

        If DataGridView1.Rows.Count <= 0 Then

            TsbEdit.Enabled = False
            tsbDelete.Enabled = False
            txtFind.Enabled = False
            tsbFind.Enabled = False
            tsbReport.Enabled = False

        Else
            TsbEdit.Enabled = True
            tsbDelete.Enabled = True
            txtFind.Enabled = True
            tsbFind.Enabled = True
            tsbReport.Enabled = True
        End If
    End Sub

    Private Sub DataGridView1_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        GridDataChanged()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub tsbFind_Click(sender As Object, e As EventArgs) Handles tsbFind.Click
        Dim searchText As String = txtFind.Text

        ' Иначе — поиск подстроки во всех столбцах
        FindAndHighlightPartial(searchText)

    End Sub

    ''' <summary>
    ''' Поиск строк по частичному совпадению во всех столбцах
    ''' </summary>
    ''' <param name="searchText">Текст для поиска (подстрока)</param>
    Private Sub FindAndHighlightPartial(searchText As String)
        If String.IsNullOrWhiteSpace(searchText) Then
            MsgBox("Введите текст для поиска.", vbExclamation)
            Return
        End If


        ' Снимаем выделение со всех строк
        DataGridView1.ClearSelection()


        Dim found As Boolean = False


        For Each row As DataGridViewRow In DataGridView1.Rows
            If Not row.IsNewRow Then
                For Each cell As DataGridViewCell In row.Cells
                    If cell.Value IsNot Nothing Then
                        Dim cellValue As String = cell.Value.ToString()
                        If cellValue.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 Then
                            ' Выделяем строку и переходим к следующей строке
                            row.Selected = True
                            found = True
                            Exit For
                        End If
                    End If
                Next
            End If
        Next

        If Not found Then
            MsgBox("Совпадений не найдено.", vbInformation)
        Else
            ' Делаем первую найденную строку видимой
            If DataGridView1.SelectedRows.Count > 0 Then
                DataGridView1.FirstDisplayedScrollingRowIndex = DataGridView1.SelectedRows(0).Index
            End If
        End If
    End Sub

    ' Обработчик для "Редактировать"
    Private Sub editToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles editToolStripMenuItem.Click
        Dim currentRow As DataGridViewRow = DataGridView1.CurrentRow
        If currentRow Is Nothing Then
            MessageBox.Show("Выберите строку для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        ' Пример: открываем форму редактирования (замените на свою логику)
        Call tsbEdit_Click(e, e)
    End Sub

    ' Обработчик для "Удалить"
    Private Sub deleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim currentRow As DataGridViewRow = DataGridView1.CurrentRow
        If currentRow Is Nothing Then
            MessageBox.Show("Выберите строку для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Call tsbDelete_Click(e, e)


    End Sub


End Class

