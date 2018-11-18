<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MkRegister
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
        Me.NaamLst = New System.Windows.Forms.TreeView
        Me.BMkReg = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.TDsn = New System.Windows.Forms.TextBox
        Me.BBrws = New System.Windows.Forms.Button
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.BPrt = New System.Windows.Forms.Button
        Me.CheckRegExp = New System.Windows.Forms.CheckBox
        Me.CheckRexx = New System.Windows.Forms.CheckBox
        Me.GrRegExp = New System.Windows.Forms.GroupBox
        Me.Lowerc = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TRegEx = New System.Windows.Forms.TextBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.BRexx = New System.Windows.Forms.Button
        Me.GrRegExp.SuspendLayout()
        Me.SuspendLayout()
        '
        'NaamLst
        '
        Me.NaamLst.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NaamLst.CheckBoxes = True
        Me.NaamLst.Location = New System.Drawing.Point(9, 163)
        Me.NaamLst.Name = "NaamLst"
        Me.NaamLst.Size = New System.Drawing.Size(434, 345)
        Me.NaamLst.TabIndex = 0
        '
        'BMkReg
        '
        Me.BMkReg.Location = New System.Drawing.Point(459, 181)
        Me.BMkReg.Name = "BMkReg"
        Me.BMkReg.Size = New System.Drawing.Size(126, 38)
        Me.BMkReg.TabIndex = 3
        Me.BMkReg.Text = "Maak register "
        Me.BMkReg.UseVisualStyleBackColor = True
        Me.BMkReg.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 108)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Naam van de lijst"
        '
        'TDsn
        '
        Me.TDsn.Location = New System.Drawing.Point(9, 124)
        Me.TDsn.Name = "TDsn"
        Me.TDsn.Size = New System.Drawing.Size(495, 20)
        Me.TDsn.TabIndex = 4
        '
        'BBrws
        '
        Me.BBrws.Location = New System.Drawing.Point(510, 124)
        Me.BBrws.Name = "BBrws"
        Me.BBrws.Size = New System.Drawing.Size(72, 20)
        Me.BBrws.TabIndex = 6
        Me.BBrws.Text = "Browse"
        Me.BBrws.UseVisualStyleBackColor = True
        '
        'BPrt
        '
        Me.BPrt.Location = New System.Drawing.Point(459, 245)
        Me.BPrt.Name = "BPrt"
        Me.BPrt.Size = New System.Drawing.Size(126, 40)
        Me.BPrt.TabIndex = 9
        Me.BPrt.Text = "Print"
        Me.BPrt.UseVisualStyleBackColor = True
        Me.BPrt.Visible = False
        '
        'CheckRegExp
        '
        Me.CheckRegExp.AutoSize = True
        Me.CheckRegExp.Checked = True
        Me.CheckRegExp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckRegExp.Location = New System.Drawing.Point(2, 23)
        Me.CheckRegExp.Name = "CheckRegExp"
        Me.CheckRegExp.Size = New System.Drawing.Size(64, 17)
        Me.CheckRegExp.TabIndex = 11
        Me.CheckRegExp.Text = "RegExp"
        Me.CheckRegExp.UseVisualStyleBackColor = True
        '
        'CheckRexx
        '
        Me.CheckRexx.AutoSize = True
        Me.CheckRexx.Location = New System.Drawing.Point(2, 72)
        Me.CheckRexx.Name = "CheckRexx"
        Me.CheckRexx.Size = New System.Drawing.Size(54, 17)
        Me.CheckRexx.TabIndex = 12
        Me.CheckRexx.Text = "ReXX"
        Me.CheckRexx.UseVisualStyleBackColor = True
        '
        'GrRegExp
        '
        Me.GrRegExp.Controls.Add(Me.Lowerc)
        Me.GrRegExp.Controls.Add(Me.Label1)
        Me.GrRegExp.Controls.Add(Me.TRegEx)
        Me.GrRegExp.Location = New System.Drawing.Point(60, 2)
        Me.GrRegExp.Name = "GrRegExp"
        Me.GrRegExp.Size = New System.Drawing.Size(683, 58)
        Me.GrRegExp.TabIndex = 16
        Me.GrRegExp.TabStop = False
        '
        'Lowerc
        '
        Me.Lowerc.AutoSize = True
        Me.Lowerc.Checked = True
        Me.Lowerc.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Lowerc.Location = New System.Drawing.Point(439, 35)
        Me.Lowerc.Name = "Lowerc"
        Me.Lowerc.Size = New System.Drawing.Size(117, 17)
        Me.Lowerc.TabIndex = 13
        Me.Lowerc.Text = "Alleen kleine letters"
        Me.Lowerc.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(230, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Regular expression voor indexering van register"
        '
        'TRegEx
        '
        Me.TRegEx.Location = New System.Drawing.Point(9, 33)
        Me.TRegEx.Name = "TRegEx"
        Me.TRegEx.Size = New System.Drawing.Size(424, 20)
        Me.TRegEx.TabIndex = 11
        Me.TRegEx.Text = "\w*"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'BRexx
        '
        Me.BRexx.Location = New System.Drawing.Point(591, 181)
        Me.BRexx.Name = "BRexx"
        Me.BRexx.Size = New System.Drawing.Size(126, 38)
        Me.BRexx.TabIndex = 17
        Me.BRexx.Text = "predefined"
        Me.BRexx.UseVisualStyleBackColor = True
        '
        'MkRegister
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(745, 510)
        Me.Controls.Add(Me.BRexx)
        Me.Controls.Add(Me.GrRegExp)
        Me.Controls.Add(Me.CheckRexx)
        Me.Controls.Add(Me.CheckRegExp)
        Me.Controls.Add(Me.BPrt)
        Me.Controls.Add(Me.BBrws)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TDsn)
        Me.Controls.Add(Me.BMkReg)
        Me.Controls.Add(Me.NaamLst)
        Me.Name = "MkRegister"
        Me.Text = "MkRegister"
        Me.GrRegExp.ResumeLayout(False)
        Me.GrRegExp.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NaamLst As System.Windows.Forms.TreeView
    Friend WithEvents BMkReg As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TDsn As System.Windows.Forms.TextBox
    Friend WithEvents BBrws As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents BPrt As System.Windows.Forms.Button
    Friend WithEvents CheckRegExp As System.Windows.Forms.CheckBox
    Friend WithEvents CheckRexx As System.Windows.Forms.CheckBox
    Friend WithEvents GrRegExp As System.Windows.Forms.GroupBox
    Friend WithEvents Lowerc As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TRegEx As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BRexx As System.Windows.Forms.Button
End Class
