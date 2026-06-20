Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Xml

Public Class EditForm
    Private isNew As Boolean = True
    Private serviceData As New Dictionary(Of String, String)()

    ' Конструктор для добавления новой записи
    Public Sub New()
        InitializeComponent()
        isNew = True
        SetupControls()
        GenerateId()
    End Sub

    ' Конструктор для редактирования существующей записи
    Public Sub New(row As DataRowView)
        InitializeComponent()
        isNew = False
        SetupControls()

        ' Заполняем поля данными из выбранной строки
        txtId.Text = If(isNew, GenerateId(), row("Id").ToString())

        ' txtId.Text = row("Id").ToString()
        txtName.Text = row("Name").ToString()
        txtUrl.Text = row("Url").ToString()
        txtComment.Text = row("Comment").ToString()
        cmbMonthlyPayment.Text = row("MonthlyPayment").ToString()
        cmbPaymentDay.Text = row("PaymentDay").ToString()
        dtpFirstMonthPay.Text = row("LastPaymentDate").ToString()
        txtSumma.Text = row("itemPrice").ToString()
        chbCheckWeekly.Checked = row("WeeklyCheck").ToString()
    End Sub

    ' Настройка элементов управления при загрузке формы
    Private Sub SetupControls()
        ' Заполняем ComboBox для ежемесячной оплаты (1–12 месяцев)
        For i As Integer = 1 To 12
            cmbMonthlyPayment.Items.Add(i.ToString())
        Next

        ' Заполняем ComboBox для дня оплаты (1–31)
        For i As Integer = 1 To 31
            cmbPaymentDay.Items.Add(i.ToString())
        Next

        ' Если это редактирование, блокируем поле ID
        If Not isNew Then
            txtId.ReadOnly = True
            txtId.BackColor = SystemColors.Control
        End If
    End Sub

    ' Возвращает данные текущей записи в виде словаря
    Public Function GetServiceData() As Dictionary(Of String, String)
        serviceData.Clear()

        ' Генерируем ID для новой записи или берём существующий
        serviceData("Id") = If(isNew, GenerateId(), txtId.Text)
        serviceData("Name") = txtName.Text
        serviceData("Url") = txtUrl.Text
        serviceData("Comment") = txtComment.Text
        serviceData("MonthlyPayment") = cmbMonthlyPayment.Text
        serviceData("PaymentDay") = cmbPaymentDay.Text
        serviceData("LastPaymentDate") = dtpFirstMonthPay.Text
        serviceData("itemPrice") = txtSumma.Text
        serviceData("WeeklyCheck") = chbCheckWeekly.Checked
        Return serviceData
    End Function

    ' Генерирует уникальный ID (максимальный существующий + 1)
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
        txtId.Text = (maxId + 1).ToString()
        Return (maxId + 1).ToString()
    End Function

    ' Кнопка "Сохранить"
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Проверяем обязательные поля
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MessageBox.Show("Введите наименование услуги.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtUrl.Text) Then
            MessageBox.Show("Введите URL сайта оплаты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrWhiteSpace(cmbMonthlyPayment.Text) Then
            MessageBox.Show("Выберите период ежемесячной оплаты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrWhiteSpace(cmbPaymentDay.Text) Then
            MessageBox.Show("Выберите день оплаты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrWhiteSpace(dtpFirstMonthPay.Text) Then
            MessageBox.Show("Выберите первый месяц оплаты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtSumma.Text) Then
            MessageBox.Show("Укажите сумму оплаты корректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ' Кнопка "Закрыть"
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        'Form1.DataGridView1.CurrentCell = Form1.DataGridView1.Rows(Form1.R_position).Cells(2)
        'Form1.DataGridView1.Rows.Item(Form1.R_position).Selected = True
        DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' Обработка закрытия формы
    Private Sub EditForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If DialogResult <> DialogResult.OK Then
            DialogResult = DialogResult.Cancel

        Else

        End If

    End Sub


    Private Sub txtSumma_TextChanged(sender As Object, e As EventArgs) Handles txtSumma.TextChanged
        Dim text As String = txtSumma.Text
        Dim decimalSeparator As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator


        ' Если пусто — ничего не делаем
        If String.IsNullOrEmpty(text) Then Return


        ' Проверяем, что разделитель только один
        If text.IndexOf(decimalSeparator) <> text.LastIndexOf(decimalSeparator) Then
            txtSumma.Text = text.Substring(0, text.LastIndexOf(decimalSeparator))
            txtSumma.SelectionStart = txtSumma.Text.Length
            Return
        End If

        ' Разделяем на целую и дробную часть
        Dim parts() As String = text.Split(New Char() {decimalSeparator})


        If parts.Length > 1 Then
            ' Дробная часть: оставляем не более 2 цифр
            If parts(1).Length > 2 Then
                parts(1) = parts(1).Substring(0, 2)
            End If
            ' Собираем обратно
            txtSumma.Text = parts(0) & decimalSeparator & parts(1)
            txtSumma.SelectionStart = txtSumma.Text.Length
        End If


        ' Удаляем ведущие нули в целой части (кроме случая "0.XX")
        If parts(0).Length > 1 AndAlso parts(0)(0) = "0"c AndAlso parts(0)(1) <> decimalSeparator Then
            parts(0) = parts(0).TrimStart("0"c)
            If String.IsNullOrEmpty(parts(0)) Then parts(0) = "0"
            txtSumma.Text = If(parts.Length = 1, parts(0), parts(0) & decimalSeparator & parts(1))
            txtSumma.SelectionStart = txtSumma.Text.Length
        End If
    End Sub

    Private Sub txtSumma_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSumma.KeyPress
        ' Разрешаем: цифры, Backspace, точку или запятую (в зависимости от культуры)
        Dim decimalSeparator As Char = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator(0)


        If Not (Char.IsDigit(e.KeyChar) OrElse
          e.KeyChar = decimalSeparator OrElse
          e.KeyChar = ChrW(Keys.Back)) Then
            e.Handled = True  ' Запретить ввод
        End If

    End Sub

    Private Sub txtSumma_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSumma.KeyDown
        Dim text As String = txtSumma.Text
        Dim decimalSeparator As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator


        ' Если пусто — ничего не делаем
        If String.IsNullOrEmpty(text) Then Return


        ' Проверяем, что разделитель только один
        If text.IndexOf(decimalSeparator) <> text.LastIndexOf(decimalSeparator) Then
            txtSumma.Text = text.Substring(0, text.LastIndexOf(decimalSeparator))
            txtSumma.SelectionStart = txtSumma.Text.Length
            Return
        End If

        ' Разделяем на целую и дробную часть
        Dim parts() As String = text.Split(New Char() {decimalSeparator})


        If parts.Length > 1 Then
            ' Дробная часть: оставляем не более 2 цифр
            If parts(1).Length > 2 Then
                parts(1) = parts(1).Substring(0, 2)
            End If
            ' Собираем обратно
            txtSumma.Text = parts(0) & decimalSeparator & parts(1)
            txtSumma.SelectionStart = txtSumma.Text.Length
        End If


        ' Удаляем ведущие нули в целой части (кроме случая "0.XX")
        If parts(0).Length > 1 AndAlso parts(0)(0) = "0"c AndAlso parts(0)(1) <> decimalSeparator Then
            parts(0) = parts(0).TrimStart("0"c)
            If String.IsNullOrEmpty(parts(0)) Then parts(0) = "0"
            txtSumma.Text = If(parts.Length = 1, parts(0), parts(0) & decimalSeparator & parts(1))
            txtSumma.SelectionStart = txtSumma.Text.Length
        End If
    End Sub
End Class
