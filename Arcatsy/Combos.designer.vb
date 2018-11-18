<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Combos
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
        Me.CmbDt = New System.Windows.Forms.DataGridView
        Me.BSave = New System.Windows.Forms.Button
        Me.SCmb = New System.Windows.Forms.DataGridView
        Me.BCancel = New System.Windows.Forms.Button
        CType(Me.CmbDt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SCmb, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CmbDt
        '
        Me.CmbDt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CmbDt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.CmbDt.Location = New System.Drawing.Point(12, 168)
        Me.CmbDt.Name = "CmbDt"
        Me.CmbDt.Size = New System.Drawing.Size(766, 308)
        Me.CmbDt.TabIndex = 5
        '
        'BSave
        '
        Me.BSave.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.BSave.Location = New System.Drawing.Point(612, 12)
        Me.BSave.Name = "BSave"
        Me.BSave.Size = New System.Drawing.Size(103, 24)
        Me.BSave.TabIndex = 4
        Me.BSave.Text = "Save"
        Me.BSave.UseVisualStyleBackColor = True
        '
        'SCmb
        '
        Me.SCmb.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SCmb.Location = New System.Drawing.Point(12, 12)
        Me.SCmb.Name = "SCmb"
        Me.SCmb.Size = New System.Drawing.Size(444, 150)
        Me.SCmb.TabIndex = 3
        '
        'BCancel
        '
        Me.BCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.BCancel.Location = New System.Drawing.Point(612, 52)
        Me.BCancel.Name = "BCancel"
        Me.BCancel.Size = New System.Drawing.Size(103, 24)
        Me.BCancel.TabIndex = 6
        Me.BCancel.Text = "Cancel"
        Me.BCancel.UseVisualStyleBackColor = True
        '
        'Combos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(783, 479)
        Me.Controls.Add(Me.BCancel)
        Me.Controls.Add(Me.CmbDt)
        Me.Controls.Add(Me.BSave)
        Me.Controls.Add(Me.SCmb)
        Me.Name = "Combos"
        Me.Text = "Combos"
        CType(Me.CmbDt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SCmb, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CmbDt As System.Windows.Forms.DataGridView
    Friend WithEvents BSave As System.Windows.Forms.Button
    Friend WithEvents SCmb As System.Windows.Forms.DataGridView
    Friend WithEvents BCancel As System.Windows.Forms.Button
End Class
