<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReportForm
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
        btnClose = New Button()
        dgvServices = New DataGridView()
        dtpEndDate = New DateTimePicker()
        btnGenerateReport1 = New Button()
        dtpStartDate = New DateTimePicker()
        Label2 = New Label()
        btnExportToExcel = New Button()
        btnExportPdf = New Button()
        CType(dgvServices, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClose.Location = New Point(852, 99)
        btnClose.Name = "btnClose"
        btnClose.Size = New Size(88, 27)
        btnClose.TabIndex = 3
        btnClose.Text = "Закрыть"
        btnClose.UseVisualStyleBackColor = True
        ' 
        ' dgvServices
        ' 
        dgvServices.AllowUserToAddRows = False
        dgvServices.AllowUserToDeleteRows = False
        dgvServices.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvServices.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable
        dgvServices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvServices.EditMode = DataGridViewEditMode.EditProgrammatically
        dgvServices.Location = New Point(12, 139)
        dgvServices.MultiSelect = False
        dgvServices.Name = "dgvServices"
        dgvServices.ReadOnly = True
        dgvServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvServices.ShowRowErrors = False
        dgvServices.Size = New Size(928, 319)
        dgvServices.TabIndex = 4
        ' 
        ' dtpEndDate
        ' 
        dtpEndDate.Format = DateTimePickerFormat.Short
        dtpEndDate.Location = New Point(188, 100)
        dtpEndDate.Name = "dtpEndDate"
        dtpEndDate.Size = New Size(91, 23)
        dtpEndDate.TabIndex = 6
        ' 
        ' btnGenerateReport1
        ' 
        btnGenerateReport1.Location = New Point(296, 99)
        btnGenerateReport1.Name = "btnGenerateReport1"
        btnGenerateReport1.Size = New Size(152, 27)
        btnGenerateReport1.TabIndex = 8
        btnGenerateReport1.Text = "Сформировать отчёт"
        btnGenerateReport1.UseVisualStyleBackColor = True
        ' 
        ' dtpStartDate
        ' 
        dtpStartDate.Format = DateTimePickerFormat.Short
        dtpStartDate.Location = New Point(55, 100)
        dtpStartDate.Name = "dtpStartDate"
        dtpStartDate.Size = New Size(97, 23)
        dtpStartDate.TabIndex = 10
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(158, 105)
        Label2.Name = "Label2"
        Label2.Size = New Size(24, 15)
        Label2.TabIndex = 11
        Label2.Text = "по:"
        ' 
        ' btnExportToExcel
        ' 
        btnExportToExcel.Location = New Point(465, 99)
        btnExportToExcel.Name = "btnExportToExcel"
        btnExportToExcel.Size = New Size(152, 27)
        btnExportToExcel.TabIndex = 12
        btnExportToExcel.Text = "Выгрузить в Эксэль"
        btnExportToExcel.UseVisualStyleBackColor = True
        ' 
        ' btnExportPdf
        ' 
        btnExportPdf.Location = New Point(632, 99)
        btnExportPdf.Name = "btnExportPdf"
        btnExportPdf.Size = New Size(152, 27)
        btnExportPdf.TabIndex = 13
        btnExportPdf.Text = "Экспорт в PDF"
        btnExportPdf.UseVisualStyleBackColor = True
        ' 
        ' ReportForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(952, 470)
        Controls.Add(btnExportPdf)
        Controls.Add(btnExportToExcel)
        Controls.Add(Label2)
        Controls.Add(dtpStartDate)
        Controls.Add(btnGenerateReport1)
        Controls.Add(dtpEndDate)
        Controls.Add(dgvServices)
        Controls.Add(btnClose)
        Name = "ReportForm"
        Text = "Заявка на оплату сервисов"
        CType(dgvServices, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents btnClose As Button
    Friend WithEvents dgvServices As DataGridView
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents btnGenerateReport1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents btnExportToExcel As Button
    Friend WithEvents btnExportPdf As Button
    'Friend WithEvents Report1 As FastReport.Report
    'Friend WithEvents Report2 As FastReport.Report
End Class
