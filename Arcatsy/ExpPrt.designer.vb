<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExpPrt
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
        Me.BCanc = New System.Windows.Forms.Button
        Me.BImpExp = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.BBrws = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.TDsn = New System.Windows.Forms.TextBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.SuspendLayout()
        '
        'BCanc
        '
        Me.BCanc.Location = New System.Drawing.Point(140, 111)
        Me.BCanc.Name = "BCanc"
        Me.BCanc.Size = New System.Drawing.Size(67, 22)
        Me.BCanc.TabIndex = 13
        Me.BCanc.Text = "Cancel"
        Me.BCanc.UseVisualStyleBackColor = True
        '
        'BImpExp
        '
        Me.BImpExp.Location = New System.Drawing.Point(15, 111)
        Me.BImpExp.Name = "BImpExp"
        Me.BImpExp.Size = New System.Drawing.Size(67, 22)
        Me.BImpExp.TabIndex = 12
        Me.BImpExp.Text = "Import"
        Me.BImpExp.UseVisualStyleBackColor = True
        Me.BImpExp.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "In collection ......"
        '
        'BBrws
        '
        Me.BBrws.Location = New System.Drawing.Point(449, 26)
        Me.BBrws.Name = "BBrws"
        Me.BBrws.Size = New System.Drawing.Size(75, 23)
        Me.BBrws.TabIndex = 10
        Me.BBrws.Text = "Browsen"
        Me.BBrws.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "DataFileNaam"
        '
        'TDsn
        '
        Me.TDsn.Location = New System.Drawing.Point(13, 29)
        Me.TDsn.Name = "TDsn"
        Me.TDsn.Size = New System.Drawing.Size(430, 20)
        Me.TDsn.TabIndex = 8
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ExpPrt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(735, 266)
        Me.Controls.Add(Me.BCanc)
        Me.Controls.Add(Me.BImpExp)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BBrws)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TDsn)
        Me.Name = "ExpPrt"
        Me.Text = "ExpPrt"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BCanc As System.Windows.Forms.Button
    Friend WithEvents BImpExp As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BBrws As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TDsn As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
