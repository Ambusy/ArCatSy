<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Sortin
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
        Me.BSrt = New System.Windows.Forms.Button
        Me.NaamLst = New System.Windows.Forms.TreeView
        Me.BBrws = New System.Windows.Forms.Button
        Me.TDsn = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.BPrt = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'BSrt
        '
        Me.BSrt.Location = New System.Drawing.Point(513, 66)
        Me.BSrt.Name = "BSrt"
        Me.BSrt.Size = New System.Drawing.Size(109, 32)
        Me.BSrt.TabIndex = 0
        Me.BSrt.Text = "Sorterte"
        Me.BSrt.UseVisualStyleBackColor = True
        Me.BSrt.Visible = False
        '
        'NaamLst
        '
        Me.NaamLst.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NaamLst.CheckBoxes = True
        Me.NaamLst.Location = New System.Drawing.Point(12, 50)
        Me.NaamLst.Name = "NaamLst"
        Me.NaamLst.Size = New System.Drawing.Size(434, 294)
        Me.NaamLst.TabIndex = 1
        '
        'BBrws
        '
        Me.BBrws.Location = New System.Drawing.Point(513, 24)
        Me.BBrws.Name = "BBrws"
        Me.BBrws.Size = New System.Drawing.Size(109, 20)
        Me.BBrws.TabIndex = 12
        Me.BBrws.Text = "Browsen"
        Me.BBrws.UseVisualStyleBackColor = True
        '
        'TDsn
        '
        Me.TDsn.Location = New System.Drawing.Point(12, 24)
        Me.TDsn.Name = "TDsn"
        Me.TDsn.Size = New System.Drawing.Size(495, 20)
        Me.TDsn.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Lijstnaam"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'BPrt
        '
        Me.BPrt.Location = New System.Drawing.Point(513, 124)
        Me.BPrt.Name = "BPrt"
        Me.BPrt.Size = New System.Drawing.Size(109, 32)
        Me.BPrt.TabIndex = 14
        Me.BPrt.Text = "Printen"
        Me.BPrt.UseVisualStyleBackColor = True
        Me.BPrt.Visible = False
        '
        'Sortin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(722, 356)
        Me.Controls.Add(Me.BPrt)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BBrws)
        Me.Controls.Add(Me.TDsn)
        Me.Controls.Add(Me.NaamLst)
        Me.Controls.Add(Me.BSrt)
        Me.Name = "Sortin"
        Me.Text = "Sortin"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BSrt As System.Windows.Forms.Button
    Friend WithEvents NaamLst As System.Windows.Forms.TreeView
    Friend WithEvents BBrws As System.Windows.Forms.Button
    Friend WithEvents TDsn As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BPrt As System.Windows.Forms.Button
End Class
