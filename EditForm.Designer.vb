<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditForm
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EditForm))
        TableLayoutPanel1 = New TableLayoutPanel()
        OK_Button = New Button()
        Cancel_Button = New Button()
        txtId = New TextBox()
        txtName = New TextBox()
        txtUrl = New TextBox()
        txtComment = New TextBox()
        cmbMonthlyPayment = New ComboBox()
        cmbPaymentDay = New ComboBox()
        btnSave = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label7 = New Label()
        dtpFirstMonthPay = New DateTimePicker()
        Label8 = New Label()
        txtSumma = New TextBox()
        PictureBox1 = New PictureBox()
        PictureBox2 = New PictureBox()
        PictureBox3 = New PictureBox()
        Label9 = New Label()
        chbCheckWeekly = New CheckBox()
        TableLayoutPanel1.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Controls.Add(OK_Button, 0, 0)
        TableLayoutPanel1.Controls.Add(Cancel_Button, 1, 0)
        TableLayoutPanel1.Location = New Point(323, 465)
        TableLayoutPanel1.Margin = New Padding(4, 3, 4, 3)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Size = New Size(170, 33)
        TableLayoutPanel1.TabIndex = 0
        ' 
        ' OK_Button
        ' 
        OK_Button.Anchor = AnchorStyles.None
        OK_Button.Location = New Point(4, 3)
        OK_Button.Margin = New Padding(4, 3, 4, 3)
        OK_Button.Name = "OK_Button"
        OK_Button.Size = New Size(77, 27)
        OK_Button.TabIndex = 0
        OK_Button.Text = "ОК"
        ' 
        ' Cancel_Button
        ' 
        Cancel_Button.Anchor = AnchorStyles.None
        Cancel_Button.Location = New Point(89, 3)
        Cancel_Button.Margin = New Padding(4, 3, 4, 3)
        Cancel_Button.Name = "Cancel_Button"
        Cancel_Button.Size = New Size(77, 27)
        Cancel_Button.TabIndex = 1
        Cancel_Button.Text = "Закрыть"
        ' 
        ' txtId
        ' 
        txtId.Location = New Point(160, 119)
        txtId.Name = "txtId"
        txtId.ReadOnly = True
        txtId.Size = New Size(100, 23)
        txtId.TabIndex = 1
        ' 
        ' txtName
        ' 
        txtName.Location = New Point(160, 157)
        txtName.Multiline = True
        txtName.Name = "txtName"
        txtName.Size = New Size(327, 61)
        txtName.TabIndex = 2
        ' 
        ' txtUrl
        ' 
        txtUrl.Location = New Point(160, 233)
        txtUrl.Name = "txtUrl"
        txtUrl.Size = New Size(327, 23)
        txtUrl.TabIndex = 3
        ' 
        ' txtComment
        ' 
        txtComment.Location = New Point(160, 269)
        txtComment.Multiline = True
        txtComment.Name = "txtComment"
        txtComment.Size = New Size(329, 43)
        txtComment.TabIndex = 4
        ' 
        ' cmbMonthlyPayment
        ' 
        cmbMonthlyPayment.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMonthlyPayment.FormattingEnabled = True
        cmbMonthlyPayment.Location = New Point(160, 336)
        cmbMonthlyPayment.Name = "cmbMonthlyPayment"
        cmbMonthlyPayment.Size = New Size(59, 23)
        cmbMonthlyPayment.TabIndex = 5
        ' 
        ' cmbPaymentDay
        ' 
        cmbPaymentDay.DropDownStyle = ComboBoxStyle.DropDownList
        cmbPaymentDay.FormattingEnabled = True
        cmbPaymentDay.Location = New Point(416, 336)
        cmbPaymentDay.Name = "cmbPaymentDay"
        cmbPaymentDay.Size = New Size(72, 23)
        cmbPaymentDay.TabIndex = 6
        ' 
        ' btnSave
        ' 
        btnSave.Location = New Point(32, 465)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(87, 30)
        btnSave.TabIndex = 7
        btnSave.Text = "Сохранить"
        btnSave.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(124, 122)
        Label1.Name = "Label1"
        Label1.Size = New Size(30, 15)
        Label1.TabIndex = 8
        Label1.Text = "П/П"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(17, 160)
        Label2.Name = "Label2"
        Label2.Size = New Size(137, 15)
        Label2.TabIndex = 9
        Label2.Text = "Наименование оплаты:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(12, 236)
        Label3.Name = "Label3"
        Label3.Size = New Size(142, 15)
        Label3.TabIndex = 10
        Label3.Text = "Ссылка на сайт сервиса:"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(76, 272)
        Label4.Name = "Label4"
        Label4.Size = New Size(78, 15)
        Label4.TabIndex = 11
        Label4.Text = "Коментарии:"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(38, 339)
        Label5.Name = "Label5"
        Label5.Size = New Size(116, 15)
        Label5.TabIndex = 12
        Label5.Text = "Оплата в месяц(ах):"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(268, 339)
        Label6.Name = "Label6"
        Label6.Size = New Size(142, 15)
        Label6.TabIndex = 13
        Label6.Text = "Оплата до какого числа:"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(32, 388)
        Label7.Name = "Label7"
        Label7.Size = New Size(122, 15)
        Label7.TabIndex = 14
        Label7.Text = "Дата первой оплаты:"
        ' 
        ' dtpFirstMonthPay
        ' 
        dtpFirstMonthPay.Format = DateTimePickerFormat.Short
        dtpFirstMonthPay.Location = New Point(160, 382)
        dtpFirstMonthPay.Name = "dtpFirstMonthPay"
        dtpFirstMonthPay.Size = New Size(95, 23)
        dtpFirstMonthPay.TabIndex = 15
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(280, 388)
        Label8.Name = "Label8"
        Label8.Size = New Size(92, 15)
        Label8.TabIndex = 16
        Label8.Text = "Сумма оплаты:"
        ' 
        ' txtSumma
        ' 
        txtSumma.Location = New Point(388, 384)
        txtSumma.Name = "txtSumma"
        txtSumma.Size = New Size(100, 23)
        txtSumma.TabIndex = 17
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = Color.White
        PictureBox1.BorderStyle = BorderStyle.FixedSingle
        PictureBox1.Location = New Point(0, 1)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(506, 84)
        PictureBox1.TabIndex = 18
        PictureBox1.TabStop = False
        ' 
        ' PictureBox2
        ' 
        PictureBox2.BorderStyle = BorderStyle.FixedSingle
        PictureBox2.Location = New Point(0, 448)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(506, 63)
        PictureBox2.TabIndex = 19
        PictureBox2.TabStop = False
        ' 
        ' PictureBox3
        ' 
        PictureBox3.BackColor = Color.White
        PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), Image)
        PictureBox3.Location = New Point(17, 12)
        PictureBox3.Name = "PictureBox3"
        PictureBox3.Size = New Size(56, 55)
        PictureBox3.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox3.TabIndex = 21
        PictureBox3.TabStop = False
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.BackColor = Color.White
        Label9.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(204))
        Label9.Location = New Point(94, 28)
        Label9.Name = "Label9"
        Label9.Size = New Size(177, 25)
        Label9.TabIndex = 20
        Label9.Text = "Карточка сервиса"
        ' 
        ' chbCheckWeekly
        ' 
        chbCheckWeekly.AutoSize = True
        chbCheckWeekly.Location = New Point(160, 423)
        chbCheckWeekly.Name = "chbCheckWeekly"
        chbCheckWeekly.Size = New Size(218, 19)
        chbCheckWeekly.TabIndex = 22
        chbCheckWeekly.Text = "Проверять каждую неделю баланс"
        chbCheckWeekly.UseVisualStyleBackColor = True
        ' 
        ' EditForm
        ' 
        AcceptButton = OK_Button
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = Cancel_Button
        ClientSize = New Size(507, 512)
        Controls.Add(chbCheckWeekly)
        Controls.Add(PictureBox3)
        Controls.Add(Label9)
        Controls.Add(btnSave)
        Controls.Add(TableLayoutPanel1)
        Controls.Add(PictureBox2)
        Controls.Add(PictureBox1)
        Controls.Add(txtSumma)
        Controls.Add(Label8)
        Controls.Add(dtpFirstMonthPay)
        Controls.Add(Label7)
        Controls.Add(Label6)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(cmbPaymentDay)
        Controls.Add(cmbMonthlyPayment)
        Controls.Add(txtComment)
        Controls.Add(txtUrl)
        Controls.Add(txtName)
        Controls.Add(txtId)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Margin = New Padding(4, 3, 4, 3)
        MaximizeBox = False
        MinimizeBox = False
        Name = "EditForm"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Карточка сервиса"
        TableLayoutPanel1.ResumeLayout(False)
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents txtId As TextBox
    Friend WithEvents txtName As TextBox
    Friend WithEvents txtUrl As TextBox
    Friend WithEvents txtComment As TextBox
    Friend WithEvents cmbMonthlyPayment As ComboBox
    Friend WithEvents cmbPaymentDay As ComboBox
    Friend WithEvents btnSave As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents dtpFirstMonthPay As DateTimePicker
    Friend WithEvents Label8 As Label
    Friend WithEvents txtSumma As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents Label9 As Label
    Friend WithEvents chbCheckWeekly As CheckBox

End Class
