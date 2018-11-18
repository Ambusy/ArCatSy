<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Maintenance
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
        Me.BColl = New System.Windows.Forms.Button()
        Me.BCombos = New System.Windows.Forms.Button()
        Me.BIdx = New System.Windows.Forms.Button()
        Me.GenRex = New System.Windows.Forms.Button()
        Me.CollNames = New System.Windows.Forms.ListBox()
        Me.BPSpec = New System.Windows.Forms.Button()
        Me.BRegExp = New System.Windows.Forms.Button()
        Me.Bexp = New System.Windows.Forms.Button()
        Me.BImp = New System.Windows.Forms.Button()
        Me.LPrtSpec = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BImpPrt = New System.Windows.Forms.Button()
        Me.BExpPrt = New System.Windows.Forms.Button()
        Me.BDelData = New System.Windows.Forms.Button()
        Me.UserBt = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BColl
        '
        Me.BColl.Location = New System.Drawing.Point(20, 16)
        Me.BColl.Name = "BColl"
        Me.BColl.Size = New System.Drawing.Size(145, 27)
        Me.BColl.TabIndex = 0
        Me.BColl.Text = "Collecties"
        Me.BColl.UseVisualStyleBackColor = True
        '
        'BCombos
        '
        Me.BCombos.Location = New System.Drawing.Point(20, 82)
        Me.BCombos.Name = "BCombos"
        Me.BCombos.Size = New System.Drawing.Size(145, 27)
        Me.BCombos.TabIndex = 1
        Me.BCombos.Text = "Comboboxen"
        Me.BCombos.UseVisualStyleBackColor = True
        '
        'BIdx
        '
        Me.BIdx.Location = New System.Drawing.Point(20, 190)
        Me.BIdx.Name = "BIdx"
        Me.BIdx.Size = New System.Drawing.Size(145, 27)
        Me.BIdx.TabIndex = 2
        Me.BIdx.Text = "Te indexeren"
        Me.BIdx.UseVisualStyleBackColor = True
        '
        'GenRex
        '
        Me.GenRex.Location = New System.Drawing.Point(198, 138)
        Me.GenRex.Name = "GenRex"
        Me.GenRex.Size = New System.Drawing.Size(144, 27)
        Me.GenRex.TabIndex = 7
        Me.GenRex.Text = "Genereer data pgm"
        Me.GenRex.UseVisualStyleBackColor = True
        '
        'CollNames
        '
        Me.CollNames.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CollNames.FormattingEnabled = True
        Me.CollNames.Location = New System.Drawing.Point(363, 16)
        Me.CollNames.Name = "CollNames"
        Me.CollNames.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.CollNames.Size = New System.Drawing.Size(325, 225)
        Me.CollNames.TabIndex = 6
        '
        'BPSpec
        '
        Me.BPSpec.Location = New System.Drawing.Point(20, 49)
        Me.BPSpec.Name = "BPSpec"
        Me.BPSpec.Size = New System.Drawing.Size(145, 27)
        Me.BPSpec.TabIndex = 8
        Me.BPSpec.Text = "Printspecificaties"
        Me.BPSpec.UseVisualStyleBackColor = True
        '
        'BRegExp
        '
        Me.BRegExp.Location = New System.Drawing.Point(20, 223)
        Me.BRegExp.Name = "BRegExp"
        Me.BRegExp.Size = New System.Drawing.Size(145, 27)
        Me.BRegExp.TabIndex = 9
        Me.BRegExp.Text = "Regexp"
        Me.BRegExp.UseVisualStyleBackColor = True
        '
        'Bexp
        '
        Me.Bexp.Location = New System.Drawing.Point(197, 16)
        Me.Bexp.Name = "Bexp"
        Me.Bexp.Size = New System.Drawing.Size(145, 27)
        Me.Bexp.TabIndex = 10
        Me.Bexp.Text = "Export data"
        Me.Bexp.UseVisualStyleBackColor = True
        '
        'BImp
        '
        Me.BImp.Location = New System.Drawing.Point(197, 49)
        Me.BImp.Name = "BImp"
        Me.BImp.Size = New System.Drawing.Size(145, 27)
        Me.BImp.TabIndex = 11
        Me.BImp.Text = "Import data"
        Me.BImp.UseVisualStyleBackColor = True
        '
        'LPrtSpec
        '
        Me.LPrtSpec.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LPrtSpec.FormattingEnabled = True
        Me.LPrtSpec.Location = New System.Drawing.Point(363, 263)
        Me.LPrtSpec.Name = "LPrtSpec"
        Me.LPrtSpec.Size = New System.Drawing.Size(325, 121)
        Me.LPrtSpec.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(413, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Label1"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(413, 247)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Label2"
        '
        'BImpPrt
        '
        Me.BImpPrt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BImpPrt.Location = New System.Drawing.Point(197, 296)
        Me.BImpPrt.Name = "BImpPrt"
        Me.BImpPrt.Size = New System.Drawing.Size(145, 27)
        Me.BImpPrt.TabIndex = 16
        Me.BImpPrt.Text = "Import data"
        Me.BImpPrt.UseVisualStyleBackColor = True
        '
        'BExpPrt
        '
        Me.BExpPrt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BExpPrt.Location = New System.Drawing.Point(198, 263)
        Me.BExpPrt.Name = "BExpPrt"
        Me.BExpPrt.Size = New System.Drawing.Size(145, 27)
        Me.BExpPrt.TabIndex = 15
        Me.BExpPrt.Text = "Export data"
        Me.BExpPrt.UseVisualStyleBackColor = True
        '
        'BDelData
        '
        Me.BDelData.Location = New System.Drawing.Point(197, 82)
        Me.BDelData.Name = "BDelData"
        Me.BDelData.Size = New System.Drawing.Size(145, 27)
        Me.BDelData.TabIndex = 17
        Me.BDelData.Text = "delete data"
        Me.BDelData.UseVisualStyleBackColor = True
        '
        'UserBt
        '
        Me.UserBt.Location = New System.Drawing.Point(20, 296)
        Me.UserBt.Name = "UserBt"
        Me.UserBt.Size = New System.Drawing.Size(145, 27)
        Me.UserBt.TabIndex = 18
        Me.UserBt.Text = "Users"
        Me.UserBt.UseVisualStyleBackColor = True
        '
        'Maintenance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(695, 396)
        Me.Controls.Add(Me.UserBt)
        Me.Controls.Add(Me.BDelData)
        Me.Controls.Add(Me.BImpPrt)
        Me.Controls.Add(Me.BExpPrt)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LPrtSpec)
        Me.Controls.Add(Me.BImp)
        Me.Controls.Add(Me.Bexp)
        Me.Controls.Add(Me.BRegExp)
        Me.Controls.Add(Me.BPSpec)
        Me.Controls.Add(Me.GenRex)
        Me.Controls.Add(Me.CollNames)
        Me.Controls.Add(Me.BIdx)
        Me.Controls.Add(Me.BCombos)
        Me.Controls.Add(Me.BColl)
        Me.Name = "Maintenance"
        Me.Text = "Maintenance"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BColl As System.Windows.Forms.Button
    Friend WithEvents BCombos As System.Windows.Forms.Button
    Friend WithEvents BIdx As System.Windows.Forms.Button
    Friend WithEvents GenRex As System.Windows.Forms.Button
    Friend WithEvents CollNames As System.Windows.Forms.ListBox
    Friend WithEvents BPSpec As System.Windows.Forms.Button
    Friend WithEvents BRegExp As System.Windows.Forms.Button
    Friend WithEvents Bexp As System.Windows.Forms.Button
    Friend WithEvents BImp As System.Windows.Forms.Button
    Friend WithEvents LPrtSpec As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BImpPrt As System.Windows.Forms.Button
    Friend WithEvents BExpPrt As System.Windows.Forms.Button
    Friend WithEvents BDelData As System.Windows.Forms.Button
    Friend WithEvents UserBt As System.Windows.Forms.Button
End Class
