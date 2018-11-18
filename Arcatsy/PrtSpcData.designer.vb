<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrtSpcData
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
        Me.Specs = New System.Windows.Forms.DataGridView
        Me.BCancel = New System.Windows.Forms.Button
        Me.BSave = New System.Windows.Forms.Button
        Me.PrtNm = New System.Windows.Forms.DataGridView
        CType(Me.Specs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PrtNm, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Specs
        '
        Me.Specs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Specs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Specs.Location = New System.Drawing.Point(12, 83)
        Me.Specs.Name = "Specs"
        Me.Specs.Size = New System.Drawing.Size(692, 263)
        Me.Specs.TabIndex = 13
        '
        'BCancel
        '
        Me.BCancel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BCancel.Location = New System.Drawing.Point(601, 53)
        Me.BCancel.Name = "BCancel"
        Me.BCancel.Size = New System.Drawing.Size(103, 24)
        Me.BCancel.TabIndex = 12
        Me.BCancel.Text = "Cancel"
        Me.BCancel.UseVisualStyleBackColor = True
        '
        'BSave
        '
        Me.BSave.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BSave.Location = New System.Drawing.Point(601, 13)
        Me.BSave.Name = "BSave"
        Me.BSave.Size = New System.Drawing.Size(103, 24)
        Me.BSave.TabIndex = 11
        Me.BSave.Text = "Save"
        Me.BSave.UseVisualStyleBackColor = True
        '
        'PrtNm
        '
        Me.PrtNm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PrtNm.Location = New System.Drawing.Point(12, 3)
        Me.PrtNm.Name = "PrtNm"
        Me.PrtNm.Size = New System.Drawing.Size(291, 59)
        Me.PrtNm.TabIndex = 10
        '
        'PrtSpcData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(716, 358)
        Me.Controls.Add(Me.Specs)
        Me.Controls.Add(Me.BCancel)
        Me.Controls.Add(Me.BSave)
        Me.Controls.Add(Me.PrtNm)
        Me.Name = "PrtSpcData"
        Me.Text = "PrtSpcData"
        CType(Me.Specs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PrtNm, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Specs As System.Windows.Forms.DataGridView
    Friend WithEvents BCancel As System.Windows.Forms.Button
    Friend WithEvents BSave As System.Windows.Forms.Button
    Friend WithEvents PrtNm As System.Windows.Forms.DataGridView
End Class
