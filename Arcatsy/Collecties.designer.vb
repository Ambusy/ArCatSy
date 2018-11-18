<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Collecties
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
        Me.CollCodes = New System.Windows.Forms.DataGridView
        Me.BSave = New System.Windows.Forms.Button
        Me.FldCodes = New System.Windows.Forms.DataGridView
        Me.BCancel = New System.Windows.Forms.Button
        CType(Me.CollCodes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FldCodes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CollCodes
        '
        Me.CollCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.CollCodes.Location = New System.Drawing.Point(12, 12)
        Me.CollCodes.Name = "CollCodes"
        Me.CollCodes.Size = New System.Drawing.Size(444, 150)
        Me.CollCodes.TabIndex = 0
        '
        'BSave
        '
        Me.BSave.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BSave.Location = New System.Drawing.Point(621, 29)
        Me.BSave.Name = "BSave"
        Me.BSave.Size = New System.Drawing.Size(103, 24)
        Me.BSave.TabIndex = 1
        Me.BSave.Text = "Saven"
        Me.BSave.UseVisualStyleBackColor = True
        '
        'FldCodes
        '
        Me.FldCodes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FldCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.FldCodes.Location = New System.Drawing.Point(12, 168)
        Me.FldCodes.Name = "FldCodes"
        Me.FldCodes.Size = New System.Drawing.Size(766, 326)
        Me.FldCodes.TabIndex = 2
        '
        'BCancel
        '
        Me.BCancel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BCancel.Location = New System.Drawing.Point(621, 69)
        Me.BCancel.Name = "BCancel"
        Me.BCancel.Size = New System.Drawing.Size(103, 24)
        Me.BCancel.TabIndex = 3
        Me.BCancel.Text = "Cancellen"
        Me.BCancel.UseVisualStyleBackColor = True
        '
        'Collecties
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(783, 494)
        Me.Controls.Add(Me.BCancel)
        Me.Controls.Add(Me.FldCodes)
        Me.Controls.Add(Me.BSave)
        Me.Controls.Add(Me.CollCodes)
        Me.Name = "Collecties"
        Me.Text = "Collecties"
        CType(Me.CollCodes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FldCodes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CollCodes As System.Windows.Forms.DataGridView
    Friend WithEvents BSave As System.Windows.Forms.Button
    Friend WithEvents FldCodes As System.Windows.Forms.DataGridView
    Friend WithEvents BCancel As System.Windows.Forms.Button
End Class
