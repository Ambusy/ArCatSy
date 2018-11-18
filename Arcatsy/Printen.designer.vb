<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Printen
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Printen))
        Me.BTonen = New System.Windows.Forms.Button()
        Me.PrintSpec = New System.Windows.Forms.TreeView()
        Me.Scr = New System.Windows.Forms.WebBrowser()
        Me.BPrinten = New System.Windows.Forms.Button()
        Me.BLaadlijst = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.BChaLayout = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BHtml = New System.Windows.Forms.Button()
        Me.BBack = New System.Windows.Forms.Button()
        Me.PBar = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'BTonen
        '
        Me.BTonen.Location = New System.Drawing.Point(323, 1)
        Me.BTonen.Name = "BTonen"
        Me.BTonen.Size = New System.Drawing.Size(110, 31)
        Me.BTonen.TabIndex = 1
        Me.BTonen.Text = "Tonen"
        Me.BTonen.UseVisualStyleBackColor = True
        '
        'PrintSpec
        '
        Me.PrintSpec.BackColor = System.Drawing.SystemColors.Control
        Me.PrintSpec.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.PrintSpec.CheckBoxes = True
        Me.PrintSpec.Location = New System.Drawing.Point(158, 17)
        Me.PrintSpec.Name = "PrintSpec"
        Me.PrintSpec.Size = New System.Drawing.Size(148, 353)
        Me.PrintSpec.TabIndex = 2
        '
        'Scr
        '
        Me.Scr.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Scr.CausesValidation = False
        Me.Scr.Location = New System.Drawing.Point(1, 38)
        Me.Scr.MinimumSize = New System.Drawing.Size(20, 20)
        Me.Scr.Name = "Scr"
        Me.Scr.Size = New System.Drawing.Size(780, 455)
        Me.Scr.TabIndex = 3
        Me.Scr.Url = New System.Uri("", System.UriKind.Relative)
        Me.Scr.Visible = False
        '
        'BPrinten
        '
        Me.BPrinten.Location = New System.Drawing.Point(439, 1)
        Me.BPrinten.Name = "BPrinten"
        Me.BPrinten.Size = New System.Drawing.Size(110, 31)
        Me.BPrinten.TabIndex = 4
        Me.BPrinten.Text = "Printen"
        Me.BPrinten.UseVisualStyleBackColor = True
        '
        'BLaadlijst
        '
        Me.BLaadlijst.Location = New System.Drawing.Point(555, 1)
        Me.BLaadlijst.Name = "BLaadlijst"
        Me.BLaadlijst.Size = New System.Drawing.Size(110, 31)
        Me.BLaadlijst.TabIndex = 5
        Me.BLaadlijst.Text = "Haal lijst"
        Me.BLaadlijst.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'BChaLayout
        '
        Me.BChaLayout.AutoSize = True
        Me.BChaLayout.Location = New System.Drawing.Point(1, 1)
        Me.BChaLayout.Name = "BChaLayout"
        Me.BChaLayout.Size = New System.Drawing.Size(151, 31)
        Me.BChaLayout.TabIndex = 6
        Me.BChaLayout.Text = "Andere layout"
        Me.BChaLayout.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(158, 1)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Label1"
        '
        'BHtml
        '
        Me.BHtml.Location = New System.Drawing.Point(671, 1)
        Me.BHtml.Name = "BHtml"
        Me.BHtml.Size = New System.Drawing.Size(110, 31)
        Me.BHtml.TabIndex = 8
        Me.BHtml.Text = "html"
        Me.BHtml.UseVisualStyleBackColor = True
        Me.BHtml.Visible = False
        '
        'BBack
        '
        Me.BBack.Image = CType(resources.GetObject("BBack.Image"), System.Drawing.Image)
        Me.BBack.Location = New System.Drawing.Point(323, 1)
        Me.BBack.Name = "BBack"
        Me.BBack.Size = New System.Drawing.Size(99, 31)
        Me.BBack.TabIndex = 9
        Me.BBack.UseVisualStyleBackColor = True
        Me.BBack.Visible = False
        '
        'PBar
        '
        Me.PBar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PBar.Location = New System.Drawing.Point(0, 472)
        Me.PBar.Name = "PBar"
        Me.PBar.Size = New System.Drawing.Size(783, 13)
        Me.PBar.TabIndex = 10
        Me.PBar.Visible = False
        '
        'Printen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(783, 485)
        Me.Controls.Add(Me.PBar)
        Me.Controls.Add(Me.BBack)
        Me.Controls.Add(Me.BHtml)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BChaLayout)
        Me.Controls.Add(Me.BLaadlijst)
        Me.Controls.Add(Me.BPrinten)
        Me.Controls.Add(Me.Scr)
        Me.Controls.Add(Me.PrintSpec)
        Me.Controls.Add(Me.BTonen)
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.Name = "Printen"
        Me.Text = "Printen"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BTonen As System.Windows.Forms.Button
    Friend WithEvents PrintSpec As System.Windows.Forms.TreeView
    Friend WithEvents Scr As System.Windows.Forms.WebBrowser
    Friend WithEvents BPrinten As System.Windows.Forms.Button
    Friend WithEvents BLaadlijst As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BChaLayout As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BHtml As System.Windows.Forms.Button
    Friend WithEvents BBack As System.Windows.Forms.Button
    Friend WithEvents PBar As System.Windows.Forms.ProgressBar
End Class
