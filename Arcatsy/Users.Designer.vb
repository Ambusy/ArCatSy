<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Users
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.BCancel = New System.Windows.Forms.Button()
        Me.BSave = New System.Windows.Forms.Button()
        Me.CollCodes = New System.Windows.Forms.DataGridView()
        CType(Me.CollCodes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BCancel
        '
        Me.BCancel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BCancel.Location = New System.Drawing.Point(583, 74)
        Me.BCancel.Name = "BCancel"
        Me.BCancel.Size = New System.Drawing.Size(103, 24)
        Me.BCancel.TabIndex = 6
        Me.BCancel.Text = "Cancellen"
        Me.BCancel.UseVisualStyleBackColor = True
        '
        'BSave
        '
        Me.BSave.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BSave.Location = New System.Drawing.Point(583, 34)
        Me.BSave.Name = "BSave"
        Me.BSave.Size = New System.Drawing.Size(103, 24)
        Me.BSave.TabIndex = 5
        Me.BSave.Text = "Saven"
        Me.BSave.UseVisualStyleBackColor = True
        '
        'CollCodes
        '
        Me.CollCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.CollCodes.Location = New System.Drawing.Point(12, 21)
        Me.CollCodes.Name = "CollCodes"
        Me.CollCodes.Size = New System.Drawing.Size(444, 150)
        Me.CollCodes.TabIndex = 4
        '
        'Users
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(698, 201)
        Me.Controls.Add(Me.BCancel)
        Me.Controls.Add(Me.BSave)
        Me.Controls.Add(Me.CollCodes)
        Me.Name = "Users"
        Me.Text = "Users"
        CType(Me.CollCodes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BCancel As System.Windows.Forms.Button
    Friend WithEvents BSave As System.Windows.Forms.Button
    Friend WithEvents CollCodes As System.Windows.Forms.DataGridView
End Class
