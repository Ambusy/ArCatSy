<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.TCollCode = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.TCollName = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Collection code:"
        '
        'TCollCode
        '
        Me.TCollCode.Location = New System.Drawing.Point(118, 21)
        Me.TCollCode.Name = "TCollCode"
        Me.TCollCode.Size = New System.Drawing.Size(47, 20)
        Me.TCollCode.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(23, 66)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(105, 28)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Create"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'TCollName
        '
        Me.TCollName.Location = New System.Drawing.Point(185, 21)
        Me.TCollName.Name = "TCollName"
        Me.TCollName.Size = New System.Drawing.Size(272, 20)
        Me.TCollName.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(469, 116)
        Me.Controls.Add(Me.TCollName)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TCollCode)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Create New Collection"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TCollCode As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TCollName As System.Windows.Forms.TextBox

End Class
