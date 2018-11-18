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
        Me.components = New System.ComponentModel.Container()
        Me.DataEntry = New System.Windows.Forms.Button()
        Me.CollNames = New System.Windows.Forms.ListBox()
        Me.Idxen = New System.Windows.Forms.Button()
        Me.DataChange = New System.Windows.Forms.Button()
        Me.BMaint = New System.Windows.Forms.Button()
        Me.BPrint = New System.Windows.Forms.Button()
        Me.BZoeken = New System.Windows.Forms.Button()
        Me.BMaakReg = New System.Windows.Forms.Button()
        Me.Progre = New System.Windows.Forms.Label()
        Me.BSort = New System.Windows.Forms.Button()
        Me.Vltd = New System.Windows.Forms.ProgressBar()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'DataEntry
        '
        Me.DataEntry.Location = New System.Drawing.Point(10, 13)
        Me.DataEntry.Name = "DataEntry"
        Me.DataEntry.Size = New System.Drawing.Size(106, 40)
        Me.DataEntry.TabIndex = 0
        Me.DataEntry.Text = "Data Entry"
        Me.DataEntry.UseVisualStyleBackColor = True
        '
        'CollNames
        '
        Me.CollNames.FormattingEnabled = True
        Me.CollNames.Location = New System.Drawing.Point(262, 13)
        Me.CollNames.Name = "CollNames"
        Me.CollNames.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.CollNames.Size = New System.Drawing.Size(264, 329)
        Me.CollNames.Sorted = True
        Me.CollNames.TabIndex = 1
        '
        'Idxen
        '
        Me.Idxen.Location = New System.Drawing.Point(73, 59)
        Me.Idxen.Name = "Idxen"
        Me.Idxen.Size = New System.Drawing.Size(105, 39)
        Me.Idxen.TabIndex = 2
        Me.Idxen.Text = "Indiceren"
        Me.Idxen.UseVisualStyleBackColor = True
        '
        'DataChange
        '
        Me.DataChange.Location = New System.Drawing.Point(135, 13)
        Me.DataChange.Name = "DataChange"
        Me.DataChange.Size = New System.Drawing.Size(106, 40)
        Me.DataChange.TabIndex = 3
        Me.DataChange.Text = "Data Change"
        Me.DataChange.UseVisualStyleBackColor = True
        '
        'BMaint
        '
        Me.BMaint.Location = New System.Drawing.Point(73, 307)
        Me.BMaint.Name = "BMaint"
        Me.BMaint.Size = New System.Drawing.Size(104, 40)
        Me.BMaint.TabIndex = 4
        Me.BMaint.Text = "Maintenance"
        Me.BMaint.UseVisualStyleBackColor = True
        '
        'BPrint
        '
        Me.BPrint.Location = New System.Drawing.Point(135, 193)
        Me.BPrint.Name = "BPrint"
        Me.BPrint.Size = New System.Drawing.Size(106, 38)
        Me.BPrint.TabIndex = 6
        Me.BPrint.Text = "Printen"
        Me.BPrint.UseVisualStyleBackColor = True
        '
        'BZoeken
        '
        Me.BZoeken.Location = New System.Drawing.Point(11, 149)
        Me.BZoeken.Name = "BZoeken"
        Me.BZoeken.Size = New System.Drawing.Size(106, 38)
        Me.BZoeken.TabIndex = 7
        Me.BZoeken.Text = "Zoeken"
        Me.BZoeken.UseVisualStyleBackColor = True
        '
        'BMaakReg
        '
        Me.BMaakReg.Location = New System.Drawing.Point(11, 193)
        Me.BMaakReg.Name = "BMaakReg"
        Me.BMaakReg.Size = New System.Drawing.Size(106, 38)
        Me.BMaakReg.TabIndex = 8
        Me.BMaakReg.Text = "Register maken"
        Me.BMaakReg.UseVisualStyleBackColor = True
        '
        'Progre
        '
        Me.Progre.AutoSize = True
        Me.Progre.Location = New System.Drawing.Point(8, 361)
        Me.Progre.Name = "Progre"
        Me.Progre.Size = New System.Drawing.Size(0, 13)
        Me.Progre.TabIndex = 9
        '
        'BSort
        '
        Me.BSort.Location = New System.Drawing.Point(135, 149)
        Me.BSort.Name = "BSort"
        Me.BSort.Size = New System.Drawing.Size(106, 38)
        Me.BSort.TabIndex = 10
        Me.BSort.Text = "Lijst sorteren"
        Me.BSort.UseVisualStyleBackColor = True
        '
        'Vltd
        '
        Me.Vltd.Location = New System.Drawing.Point(10, 361)
        Me.Vltd.Name = "Vltd"
        Me.Vltd.Size = New System.Drawing.Size(516, 17)
        Me.Vltd.TabIndex = 11
        Me.Vltd.Visible = False
        '
        'Timer1
        '
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(531, 381)
        Me.Controls.Add(Me.Vltd)
        Me.Controls.Add(Me.BSort)
        Me.Controls.Add(Me.Progre)
        Me.Controls.Add(Me.BMaakReg)
        Me.Controls.Add(Me.BZoeken)
        Me.Controls.Add(Me.BPrint)
        Me.Controls.Add(Me.BMaint)
        Me.Controls.Add(Me.DataChange)
        Me.Controls.Add(Me.Idxen)
        Me.Controls.Add(Me.CollNames)
        Me.Controls.Add(Me.DataEntry)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "A R C A T S Y"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataEntry As System.Windows.Forms.Button
    Friend WithEvents CollNames As System.Windows.Forms.ListBox
    Friend WithEvents Idxen As System.Windows.Forms.Button
    Friend WithEvents DataChange As System.Windows.Forms.Button
    Friend WithEvents BMaint As System.Windows.Forms.Button
    Friend WithEvents BPrint As System.Windows.Forms.Button
    Friend WithEvents BZoeken As System.Windows.Forms.Button
    Friend WithEvents BMaakReg As System.Windows.Forms.Button
    Friend WithEvents Progre As System.Windows.Forms.Label
    Friend WithEvents BSort As System.Windows.Forms.Button
    Friend WithEvents Vltd As System.Windows.Forms.ProgressBar
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
