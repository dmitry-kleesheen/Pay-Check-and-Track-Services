<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReport))
        dgvReport = New DataGridView()
        btnGenerateReport = New Button()
        dtpStartDate = New DateTimePicker()
        dtpEndDate = New DateTimePicker()
        PictureBox2 = New PictureBox()
        PictureBox1 = New PictureBox()
        btnExportPdf = New Button()
        btnExportToExcel = New Button()
        btnClose = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        CType(dgvReport, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvReport
        ' 
        dgvReport.AllowUserToAddRows = False
        dgvReport.AllowUserToDeleteRows = False
        dgvReport.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvReport.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable
        dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvReport.EditMode = DataGridViewEditMode.EditProgrammatically
        dgvReport.Location = New Point(12, 165)
        dgvReport.MultiSelect = False
        dgvReport.Name = "dgvReport"
        dgvReport.ReadOnly = True
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvReport.ShowRowErrors = False
        dgvReport.Size = New Size(900, 388)
        dgvReport.TabIndex = 5
        ' 
        ' btnGenerateReport
        ' 
        btnGenerateReport.Location = New Point(325, 108)
        btnGenerateReport.Name = "btnGenerateReport"
        btnGenerateReport.Size = New Size(124, 31)
        btnGenerateReport.TabIndex = 6
        btnGenerateReport.Text = "Сформировать"
        btnGenerateReport.UseVisualStyleBackColor = True
        ' 
        ' dtpStartDate
        ' 
        dtpStartDate.Format = DateTimePickerFormat.Short
        dtpStartDate.Location = New Point(66, 110)
        dtpStartDate.Name = "dtpStartDate"
        dtpStartDate.Size = New Size(94, 23)
        dtpStartDate.TabIndex = 7
        ' 
        ' dtpEndDate
        ' 
        dtpEndDate.Format = DateTimePickerFormat.Short
        dtpEndDate.Location = New Point(192, 110)
        dtpEndDate.Name = "dtpEndDate"
        dtpEndDate.Size = New Size(99, 23)
        dtpEndDate.TabIndex = 8
        ' 
        ' PictureBox2
        ' 
        PictureBox2.BackColor = Color.White
        PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), Image)
        PictureBox2.Location = New Point(17, 8)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(64, 66)
        PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox2.TabIndex = 18
        PictureBox2.TabStop = False
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        PictureBox1.BackColor = Color.White
        PictureBox1.BorderStyle = BorderStyle.FixedSingle
        PictureBox1.Location = New Point(-3, -3)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(928, 84)
        PictureBox1.TabIndex = 17
        PictureBox1.TabStop = False
        ' 
        ' btnExportPdf
        ' 
        btnExportPdf.Location = New Point(613, 108)
        btnExportPdf.Name = "btnExportPdf"
        btnExportPdf.Size = New Size(135, 30)
        btnExportPdf.TabIndex = 21
        btnExportPdf.Text = "Экспорт в PDF"
        btnExportPdf.UseVisualStyleBackColor = True
        ' 
        ' btnExportToExcel
        ' 
        btnExportToExcel.Location = New Point(472, 108)
        btnExportToExcel.Name = "btnExportToExcel"
        btnExportToExcel.Size = New Size(135, 31)
        btnExportToExcel.TabIndex = 20
        btnExportToExcel.Text = "Выгрузить в Эксэль"
        btnExportToExcel.UseVisualStyleBackColor = True
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClose.Location = New Point(807, 108)
        btnClose.Name = "btnClose"
        btnClose.Size = New Size(100, 31)
        btnClose.TabIndex = 19
        btnClose.Text = "Закрыть"
        btnClose.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = Color.White
        Label1.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(204))
        Label1.Location = New Point(107, 25)
        Label1.Name = "Label1"
        Label1.Size = New Size(224, 25)
        Label1.TabIndex = 22
        Label1.Text = "Формирование отчёта"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(44, 114)
        Label2.Name = "Label2"
        Label2.Size = New Size(16, 15)
        Label2.TabIndex = 23
        Label2.Text = "с:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(162, 114)
        Label3.Name = "Label3"
        Label3.Size = New Size(24, 15)
        Label3.TabIndex = 23
        Label3.Text = "по:"
        ' 
        ' frmReport
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(924, 565)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(btnExportPdf)
        Controls.Add(btnExportToExcel)
        Controls.Add(btnClose)
        Controls.Add(PictureBox2)
        Controls.Add(PictureBox1)
        Controls.Add(dtpEndDate)
        Controls.Add(dtpStartDate)
        Controls.Add(btnGenerateReport)
        Controls.Add(dgvReport)
        Name = "frmReport"
        Text = "Формирование отчёта"
        CType(dgvReport, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dgvReport As DataGridView
    Friend WithEvents btnGenerateReport As Button
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnExportPdf As Button
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
End Class
