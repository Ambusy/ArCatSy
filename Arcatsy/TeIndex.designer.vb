<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TeIndex
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
        Me.BSave = New System.Windows.Forms.Button
        Me.CollCodes = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.BCancel = New System.Windows.Forms.Button
        CType(Me.CollCodes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BSave
        '
        Me.BSave.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BSave.Location = New System.Drawing.Point(609, 28)
        Me.BSave.Name = "BSave"
        Me.BSave.Size = New System.Drawing.Size(103, 28)
        Me.BSave.TabIndex = 3
        Me.BSave.Text = "Save"
        Me.BSave.UseVisualStyleBackColor = True
        '
        'CollCodes
        '
        Me.CollCodes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CollCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.CollCodes.Location = New System.Drawing.Point(12, 28)
        Me.CollCodes.Name = "CollCodes"
        Me.CollCodes.Size = New System.Drawing.Size(247, 360)
        Me.CollCodes.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(149, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(188, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "0: de hele collectie opnieuw indexeren"
        '
        'BCancel
        '
        Me.BCancel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BCancel.Location = New System.Drawing.Point(609, 62)
        Me.BCancel.Name = "BCancel"
        Me.BCancel.Size = New System.Drawing.Size(103, 28)
        Me.BCancel.TabIndex = 5
        Me.BCancel.Text = "Cancel"
        Me.BCancel.UseVisualStyleBackColor = True
        '
        'TeIndex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(724, 384)
        Me.Controls.Add(Me.BCancel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BSave)
        Me.Controls.Add(Me.CollCodes)
        Me.Name = "TeIndex"
        Me.Text = "TeIndex"
        CType(Me.CollCodes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BSave As System.Windows.Forms.Button
    Friend WithEvents CollCodes As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BCancel As System.Windows.Forms.Button
End Class
