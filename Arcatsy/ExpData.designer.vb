<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExpData
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
        Me.TDsn = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BBrws = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BImpExp = New System.Windows.Forms.Button()
        Me.BCanc = New System.Windows.Forms.Button()
        Me.CheckData = New System.Windows.Forms.CheckBox()
        Me.CheckDef = New System.Windows.Forms.CheckBox()
        Me.ExpSq = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Seps = New System.Windows.Forms.ComboBox()
        Me.cbRaw = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'TDsn
        '
        Me.TDsn.Location = New System.Drawing.Point(24, 39)
        Me.TDsn.Margin = New System.Windows.Forms.Padding(4)
        Me.TDsn.Name = "TDsn"
        Me.TDsn.Size = New System.Drawing.Size(572, 22)
        Me.TDsn.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 15)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "DataFileNaam"
        '
        'BBrws
        '
        Me.BBrws.Location = New System.Drawing.Point(605, 36)
        Me.BBrws.Margin = New System.Windows.Forms.Padding(4)
        Me.BBrws.Name = "BBrws"
        Me.BBrws.Size = New System.Drawing.Size(100, 28)
        Me.BBrws.TabIndex = 2
        Me.BBrws.Text = "Browsen"
        Me.BBrws.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 95)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 17)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "In collection ......"
        '
        'BImpExp
        '
        Me.BImpExp.Location = New System.Drawing.Point(27, 197)
        Me.BImpExp.Margin = New System.Windows.Forms.Padding(4)
        Me.BImpExp.Name = "BImpExp"
        Me.BImpExp.Size = New System.Drawing.Size(89, 27)
        Me.BImpExp.TabIndex = 4
        Me.BImpExp.Text = "Import"
        Me.BImpExp.UseVisualStyleBackColor = True
        Me.BImpExp.Visible = False
        '
        'BCanc
        '
        Me.BCanc.Location = New System.Drawing.Point(193, 197)
        Me.BCanc.Margin = New System.Windows.Forms.Padding(4)
        Me.BCanc.Name = "BCanc"
        Me.BCanc.Size = New System.Drawing.Size(89, 27)
        Me.BCanc.TabIndex = 5
        Me.BCanc.Text = "Cancel"
        Me.BCanc.UseVisualStyleBackColor = True
        '
        'CheckData
        '
        Me.CheckData.AutoSize = True
        Me.CheckData.Location = New System.Drawing.Point(27, 154)
        Me.CheckData.Margin = New System.Windows.Forms.Padding(4)
        Me.CheckData.Name = "CheckData"
        Me.CheckData.Size = New System.Drawing.Size(58, 21)
        Me.CheckData.TabIndex = 6
        Me.CheckData.Text = "data"
        Me.CheckData.UseVisualStyleBackColor = True
        '
        'CheckDef
        '
        Me.CheckDef.AutoSize = True
        Me.CheckDef.Location = New System.Drawing.Point(27, 126)
        Me.CheckDef.Margin = New System.Windows.Forms.Padding(4)
        Me.CheckDef.Name = "CheckDef"
        Me.CheckDef.Size = New System.Drawing.Size(50, 21)
        Me.CheckDef.TabIndex = 7
        Me.CheckDef.Text = "def"
        Me.CheckDef.UseVisualStyleBackColor = True
        '
        'ExpSq
        '
        Me.ExpSq.AutoSize = True
        Me.ExpSq.Location = New System.Drawing.Point(193, 154)
        Me.ExpSq.Margin = New System.Windows.Forms.Padding(4)
        Me.ExpSq.Name = "ExpSq"
        Me.ExpSq.Size = New System.Drawing.Size(110, 21)
        Me.ExpSq.TabIndex = 8
        Me.ExpSq.Text = "Export seqnr"
        Me.ExpSq.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(401, 155)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 17)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Separator"
        '
        'Seps
        '
        Me.Seps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Seps.FormattingEnabled = True
        Me.Seps.Items.AddRange(New Object() {"Tab", ",", ";", "|"})
        Me.Seps.Location = New System.Drawing.Point(405, 175)
        Me.Seps.Margin = New System.Windows.Forms.Padding(4)
        Me.Seps.Name = "Seps"
        Me.Seps.Size = New System.Drawing.Size(115, 24)
        Me.Seps.TabIndex = 11
        '
        'cbRaw
        '
        Me.cbRaw.AutoSize = True
        Me.cbRaw.Location = New System.Drawing.Point(105, 168)
        Me.cbRaw.Margin = New System.Windows.Forms.Padding(4)
        Me.cbRaw.Name = "cbRaw"
        Me.cbRaw.Size = New System.Drawing.Size(52, 21)
        Me.cbRaw.TabIndex = 12
        Me.cbRaw.Text = "raw"
        Me.cbRaw.UseVisualStyleBackColor = True
        Me.cbRaw.Visible = False
        '
        'ExpData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 327)
        Me.Controls.Add(Me.cbRaw)
        Me.Controls.Add(Me.Seps)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ExpSq)
        Me.Controls.Add(Me.CheckDef)
        Me.Controls.Add(Me.CheckData)
        Me.Controls.Add(Me.BCanc)
        Me.Controls.Add(Me.BImpExp)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BBrws)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TDsn)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "ExpData"
        Me.Text = "ExpData"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TDsn As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BBrws As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BImpExp As System.Windows.Forms.Button
    Friend WithEvents BCanc As System.Windows.Forms.Button
    Friend WithEvents CheckData As System.Windows.Forms.CheckBox
    Friend WithEvents CheckDef As System.Windows.Forms.CheckBox
    Friend WithEvents ExpSq As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Seps As System.Windows.Forms.ComboBox
    Friend WithEvents cbRaw As CheckBox
End Class
