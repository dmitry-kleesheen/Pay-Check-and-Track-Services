<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        PictureBox1 = New PictureBox()
        Panel1 = New Panel()
        ToolStrip1 = New ToolStrip()
        tsbAdd = New ToolStripButton()
        TsbEdit = New ToolStripButton()
        ToolStripSeparator1 = New ToolStripSeparator()
        tsbDelete = New ToolStripButton()
        ToolStripSeparator2 = New ToolStripSeparator()
        ToolStripLabel1 = New ToolStripLabel()
        txtFind = New ToolStripTextBox()
        tsbFind = New ToolStripButton()
        ToolStripSeparator5 = New ToolStripSeparator()
        tsbReport = New ToolStripButton()
        tsbImport = New ToolStripButton()
        ToolStripSeparator3 = New ToolStripSeparator()
        tsbHelp = New ToolStripButton()
        DataGridView1 = New DataGridView()
        Label1 = New Label()
        PictureBox2 = New PictureBox()
        ContextMenuStrip1 = New ContextMenuStrip(components)
        editToolStripMenuItem = New ToolStripMenuItem()
        DeleteToolStripMenuItem = New ToolStripMenuItem()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        ToolStrip1.SuspendLayout()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        PictureBox1.BackColor = Color.White
        PictureBox1.BorderStyle = BorderStyle.FixedSingle
        PictureBox1.Location = New Point(0, 2)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(800, 84)
        PictureBox1.TabIndex = 6
        PictureBox1.TabStop = False
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Panel1.Controls.Add(ToolStrip1)
        Panel1.Controls.Add(DataGridView1)
        Panel1.Location = New Point(0, 92)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(800, 358)
        Panel1.TabIndex = 11
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.Dock = DockStyle.None
        ToolStrip1.Items.AddRange(New ToolStripItem() {tsbAdd, TsbEdit, ToolStripSeparator1, tsbDelete, ToolStripSeparator2, ToolStripLabel1, txtFind, tsbFind, ToolStripSeparator5, tsbReport, tsbImport, ToolStripSeparator3, tsbHelp})
        ToolStrip1.Location = New Point(0, 0)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Size = New Size(393, 25)
        ToolStrip1.TabIndex = 6
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' tsbAdd
        ' 
        tsbAdd.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbAdd.Image = CType(resources.GetObject("tsbAdd.Image"), Image)
        tsbAdd.ImageTransparentColor = Color.Magenta
        tsbAdd.Name = "tsbAdd"
        tsbAdd.Size = New Size(23, 22)
        tsbAdd.Text = "Добавить"
        ' 
        ' TsbEdit
        ' 
        TsbEdit.DisplayStyle = ToolStripItemDisplayStyle.Image
        TsbEdit.Image = CType(resources.GetObject("TsbEdit.Image"), Image)
        TsbEdit.ImageTransparentColor = Color.Magenta
        TsbEdit.Name = "TsbEdit"
        TsbEdit.Size = New Size(23, 22)
        TsbEdit.Tag = "DisableGroup1"
        TsbEdit.Text = "Редактировать"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(6, 25)
        ' 
        ' tsbDelete
        ' 
        tsbDelete.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbDelete.Image = CType(resources.GetObject("tsbDelete.Image"), Image)
        tsbDelete.ImageTransparentColor = Color.Magenta
        tsbDelete.Name = "tsbDelete"
        tsbDelete.Size = New Size(23, 22)
        tsbDelete.Tag = "DisableGroup1"
        tsbDelete.Text = "Удалить"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(6, 25)
        ' 
        ' ToolStripLabel1
        ' 
        ToolStripLabel1.Name = "ToolStripLabel1"
        ToolStripLabel1.Size = New Size(44, 22)
        ToolStripLabel1.Text = "Найти:"
        ' 
        ' txtFind
        ' 
        txtFind.Name = "txtFind"
        txtFind.Size = New Size(150, 25)
        txtFind.Tag = "DisableGroup1"
        ' 
        ' tsbFind
        ' 
        tsbFind.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbFind.Image = CType(resources.GetObject("tsbFind.Image"), Image)
        tsbFind.ImageTransparentColor = Color.Magenta
        tsbFind.Name = "tsbFind"
        tsbFind.Size = New Size(23, 22)
        tsbFind.Tag = "DisableGroup1"
        tsbFind.Text = "Найти далее"
        ' 
        ' ToolStripSeparator5
        ' 
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New Size(6, 25)
        ' 
        ' tsbReport
        ' 
        tsbReport.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbReport.Image = CType(resources.GetObject("tsbReport.Image"), Image)
        tsbReport.ImageTransparentColor = Color.Magenta
        tsbReport.Name = "tsbReport"
        tsbReport.Size = New Size(23, 22)
        tsbReport.Tag = "DisableGroup1"
        tsbReport.Text = "Сформировать заявку"
        ' 
        ' tsbImport
        ' 
        tsbImport.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbImport.Image = CType(resources.GetObject("tsbImport.Image"), Image)
        tsbImport.ImageTransparentColor = Color.Magenta
        tsbImport.Name = "tsbImport"
        tsbImport.Size = New Size(23, 22)
        tsbImport.Text = "Импорт из CSV файла..."
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(6, 25)
        ' 
        ' tsbHelp
        ' 
        tsbHelp.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbHelp.Image = CType(resources.GetObject("tsbHelp.Image"), Image)
        tsbHelp.ImageTransparentColor = Color.Magenta
        tsbHelp.Name = "tsbHelp"
        tsbHelp.Size = New Size(23, 22)
        tsbHelp.Text = "Помощь"
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
        DataGridView1.Location = New Point(3, 28)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(797, 327)
        DataGridView1.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = Color.White
        Label1.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(204))
        Label1.Location = New Point(96, 28)
        Label1.Name = "Label1"
        Label1.Size = New Size(313, 25)
        Label1.TabIndex = 12
        Label1.Text = "Учёт оплаты ""Онлайн сервисов"""
        ' 
        ' PictureBox2
        ' 
        PictureBox2.BackColor = Color.White
        PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), Image)
        PictureBox2.Location = New Point(21, 12)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(56, 55)
        PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox2.TabIndex = 13
        PictureBox2.TabStop = False
        ' 
        ' ContextMenuStrip1
        ' 
        ContextMenuStrip1.Items.AddRange(New ToolStripItem() {editToolStripMenuItem, DeleteToolStripMenuItem})
        ContextMenuStrip1.Name = "ContextMenuStrip1"
        ContextMenuStrip1.Size = New Size(155, 48)
        ' 
        ' editToolStripMenuItem
        ' 
        editToolStripMenuItem.Image = CType(resources.GetObject("editToolStripMenuItem.Image"), Image)
        editToolStripMenuItem.ImageTransparentColor = Color.Magenta
        editToolStripMenuItem.Name = "editToolStripMenuItem"
        editToolStripMenuItem.Size = New Size(154, 22)
        editToolStripMenuItem.Text = "Редактировать"
        ' 
        ' DeleteToolStripMenuItem
        ' 
        DeleteToolStripMenuItem.Image = CType(resources.GetObject("DeleteToolStripMenuItem.Image"), Image)
        DeleteToolStripMenuItem.ImageTransparentColor = Color.Magenta
        DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        DeleteToolStripMenuItem.Size = New Size(154, 22)
        DeleteToolStripMenuItem.Text = "Удалить"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(PictureBox2)
        Controls.Add(Label1)
        Controls.Add(Panel1)
        Controls.Add(PictureBox1)
        Name = "Form1"
        Text = "Заявка на оплату услуг по неделям"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ToolStrip1.ResumeLayout(False)
        ToolStrip1.PerformLayout()
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        ContextMenuStrip1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents tsbAdd As ToolStripButton
    Friend WithEvents TsbEdit As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents tsbDelete As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents tsbHelp As ToolStripButton
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents tsbReport As ToolStripButton
    Friend WithEvents tsbImport As ToolStripButton
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents txtFind As ToolStripTextBox
    Friend WithEvents tsbFind As ToolStripButton
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents editToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem

End Class
