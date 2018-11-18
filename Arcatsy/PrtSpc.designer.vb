<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrtSpc
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
        Me.BCancel = New System.Windows.Forms.Button
        Me.BSave = New System.Windows.Forms.Button
        Me.CollCodes = New System.Windows.Forms.DataGridView
        Me.SpcVal = New System.Windows.Forms.DataGridView
        Me.BToPrt = New System.Windows.Forms.Button
        CType(Me.CollCodes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SpcVal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BCancel
        '
        Me.BCancel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BCancel.Location = New System.Drawing.Point(592, 52)
        Me.BCancel.Name = "BCancel"
        Me.BCancel.Size = New System.Drawing.Size(103, 24)
        Me.BCancel.TabIndex = 7
        Me.BCancel.Text = "Cancel"
        Me.BCancel.UseVisualStyleBackColor = True
        '
        'BSave
        '
        Me.BSave.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BSave.Location = New System.Drawing.Point(592, 12)
        Me.BSave.Name = "BSave"
        Me.BSave.Size = New System.Drawing.Size(103, 24)
        Me.BSave.TabIndex = 5
        Me.BSave.Text = "Save"
        Me.BSave.UseVisualStyleBackColor = True
        '
        'CollCodes
        '
        Me.CollCodes.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.CollCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.CollCodes.Location = New System.Drawing.Point(3, 2)
        Me.CollCodes.Name = "CollCodes"
        Me.CollCodes.Size = New System.Drawing.Size(539, 150)
        Me.CollCodes.TabIndex = 4
        '
        'SpcVal
        '
        Me.SpcVal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SpcVal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SpcVal.Location = New System.Drawing.Point(3, 158)
        Me.SpcVal.Name = "SpcVal"
        Me.SpcVal.Size = New System.Drawing.Size(539, 211)
        Me.SpcVal.TabIndex = 8
        '
        'BToPrt
        '
        Me.BToPrt.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BToPrt.Location = New System.Drawing.Point(592, 158)
        Me.BToPrt.Name = "BToPrt"
        Me.BToPrt.Size = New System.Drawing.Size(103, 39)
        Me.BToPrt.TabIndex = 9
        Me.BToPrt.Text = "Printspec"
        Me.BToPrt.UseVisualStyleBackColor = True
        '
        'PrtSpc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(707, 381)
        Me.Controls.Add(Me.BToPrt)
        Me.Controls.Add(Me.SpcVal)
        Me.Controls.Add(Me.BCancel)
        Me.Controls.Add(Me.BSave)
        Me.Controls.Add(Me.CollCodes)
        Me.Name = "PrtSpc"
        Me.Text = "PrtSpc"
        CType(Me.CollCodes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SpcVal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BCancel As System.Windows.Forms.Button
    Friend WithEvents BSave As System.Windows.Forms.Button
    Friend WithEvents CollCodes As System.Windows.Forms.DataGridView
    Friend WithEvents SpcVal As System.Windows.Forms.DataGridView
    Friend WithEvents BToPrt As System.Windows.Forms.Button
End Class
