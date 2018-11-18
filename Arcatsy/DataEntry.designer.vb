<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataEntry
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
        Me.BClear = New System.Windows.Forms.Button()
        Me.Bsave = New System.Windows.Forms.Button()
        Me.Msg = New System.Windows.Forms.Label()
        Me.Gr = New System.Windows.Forms.GroupBox()
        Me.BSelKey = New System.Windows.Forms.Button()
        Me.BNext = New System.Windows.Forms.Button()
        Me.InsMod = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'BClear
        '
        Me.BClear.Location = New System.Drawing.Point(3, 1)
        Me.BClear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BClear.Name = "BClear"
        Me.BClear.Size = New System.Drawing.Size(151, 34)
        Me.BClear.TabIndex = 9998
        Me.BClear.Text = "Leeg"
        Me.BClear.UseVisualStyleBackColor = True
        '
        'Bsave
        '
        Me.Bsave.Location = New System.Drawing.Point(161, 1)
        Me.Bsave.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Bsave.Name = "Bsave"
        Me.Bsave.Size = New System.Drawing.Size(140, 34)
        Me.Bsave.TabIndex = 9999
        Me.Bsave.Text = "Opslaan"
        Me.Bsave.UseVisualStyleBackColor = True
        '
        'Msg
        '
        Me.Msg.AutoSize = True
        Me.Msg.BackColor = System.Drawing.SystemColors.Control
        Me.Msg.ForeColor = System.Drawing.Color.Red
        Me.Msg.Location = New System.Drawing.Point(656, 1)
        Me.Msg.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Msg.Name = "Msg"
        Me.Msg.Size = New System.Drawing.Size(12, 17)
        Me.Msg.TabIndex = 2
        Me.Msg.Text = "."
        '
        'Gr
        '
        Me.Gr.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Gr.Location = New System.Drawing.Point(3, 102)
        Me.Gr.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Gr.Name = "Gr"
        Me.Gr.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Gr.Size = New System.Drawing.Size(1039, 495)
        Me.Gr.TabIndex = 3
        Me.Gr.TabStop = False
        '
        'BSelKey
        '
        Me.BSelKey.Location = New System.Drawing.Point(309, 1)
        Me.BSelKey.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BSelKey.Name = "BSelKey"
        Me.BSelKey.Size = New System.Drawing.Size(199, 34)
        Me.BSelKey.TabIndex = 10000
        Me.BSelKey.Text = "Selecteer "
        Me.BSelKey.UseVisualStyleBackColor = True
        '
        'BNext
        '
        Me.BNext.Location = New System.Drawing.Point(516, 1)
        Me.BNext.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BNext.Name = "BNext"
        Me.BNext.Size = New System.Drawing.Size(132, 34)
        Me.BNext.TabIndex = 10001
        Me.BNext.Text = "next"
        Me.BNext.UseVisualStyleBackColor = True
        '
        'InsMod
        '
        Me.InsMod.AutoSize = True
        Me.InsMod.BackColor = System.Drawing.SystemColors.Control
        Me.InsMod.ForeColor = System.Drawing.Color.DarkGray
        Me.InsMod.Location = New System.Drawing.Point(656, 17)
        Me.InsMod.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.InsMod.Name = "InsMod"
        Me.InsMod.Size = New System.Drawing.Size(12, 17)
        Me.InsMod.TabIndex = 10002
        Me.InsMod.Text = "."
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'DataEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1041, 619)
        Me.Controls.Add(Me.InsMod)
        Me.Controls.Add(Me.BNext)
        Me.Controls.Add(Me.BSelKey)
        Me.Controls.Add(Me.Gr)
        Me.Controls.Add(Me.Msg)
        Me.Controls.Add(Me.Bsave)
        Me.Controls.Add(Me.BClear)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "DataEntry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "DataEntry"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BClear As System.Windows.Forms.Button
    Friend WithEvents Bsave As System.Windows.Forms.Button
    Friend WithEvents Msg As System.Windows.Forms.Label
    Friend WithEvents Gr As System.Windows.Forms.GroupBox
    Friend WithEvents BSelKey As System.Windows.Forms.Button
    Friend WithEvents BNext As System.Windows.Forms.Button
    Friend WithEvents InsMod As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
