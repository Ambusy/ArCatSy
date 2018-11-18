Imports System.Globalization
'
Public Class Form1
    Dim DbConn As OdbcConnection
    Friend CollNamesC As New Collection
    Private Sub Form1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        CollNamesC.Clear()
        ReadCursorCollection(CollNamesC, "SELECT CollCode, CollName FROM collections", "S1 S0", " | ", True)
        'CollNames.Items.Clear()
        For Each s As String In CollNamesC
            Dim w As String = s.Substring(InStr(s, "|")).Trim()
            If Not HideColl.Contains(w) Then
                If Not CollNames.Items.Contains(s) Then
                    CollNames.Items.Add(s)
                End If
            End If
        Next
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If logFile Is Nothing Then
        Else
            logFile.Dispose()
            logFile.Close()
        End If
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim s, w As String
        ReadSysMsg(My.Application.Info.DirectoryPath & "\" & "system messages.txt")
        ' system constants
        CultInf = New CultureInfo(SysMsg(202), False)
        SqlProv = SysMsg(208)
        DbConn = New OdbcConnection(SqlProv)
        Dim retr As MsgBoxResult
        retr = MsgBoxResult.Retry
        While retr = MsgBoxResult.Retry
            Try
                DbConn.Open()
                DbConn.Close()
                retr = MsgBoxResult.Ok
            Catch ex As Exception
                retr = MsgBox(SysMsg(211), MsgBoxStyle.RetryCancel)
            End Try
        End While
        If retr = MsgBoxResult.Cancel Then
            Me.Close()
            Exit Sub
        End If
        ' security
        Dim login As String = SysMsg(212)
        If login.Substring(6) = "YES" Then
            If Not DoLogin() Then
                Me.Close()
                Exit Sub
            End If
        End If

        'screen constants
        DataEntry.Text = SysMsg(51)
        DataChange.Text = SysMsg(52)
        Idxen.Text = SysMsg(53)
        BZoeken.Text = SysMsg(54)
        BMaakReg.Text = SysMsg(55)
        BPrint.Text = SysMsg(56)
        BMaint.Text = SysMsg(57)
        BSort.Text = SysMsg(59)
        s = SysMsg(203)
        s = s.Trim()
        While s.Length() > 0
            Dim i As Integer = InStr(s, ",")
            If i > 0 Then
                w = s.Substring(0, i - 1)
                s = s.Substring(i).TrimEnd()
            Else
                w = s
                s = ""
            End If
            HideColl.Add(w, w)
        End While
        fPrinten = New Printen()

        Idxen.Visible = RecsToIndex()
        Dim AllowUpdate As Boolean = True
        s = SysMsg(210).TrimEnd
        If s.Substring(s.Length - 3, 1) = "n" Then
            AllowUpdate = False
            DataEntry.Visible = False
            BMaint.Visible = False
            DataChange.Visible = False
            DataChange.Visible = False
            Idxen.Visible = False
            If s.Substring(s.Length - 2, 1) = "y" Then
                Timer1.Enabled = True
            End If
        End If
    End Sub
    Private Function RecsToIndex() As Boolean
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        ' haal CollSeq op van te indexeren records
        Dim SQLsfSelect As String = "SELECT TOP 1 CollSeq FROM tobeindexed"
        Dim DRsfSelect As OdbcDataReader = Nothing
        Dim DCsfSelect As OdbcCommand
        DCsfSelect = New OdbcCommand(SQLsfSelect, DbConn)
        Dim r As Boolean = False
        Try
            DRsfSelect = DCsfSelect.ExecuteReader()
            Do While (DRsfSelect.Read())
                r = True
                Exit Do
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(58))
        Finally
            If Not (DRsfSelect Is Nothing) Then
                DRsfSelect.Close()
            End If
        End Try
        DbConn.Close()
        Return r
    End Function
    Private Sub CollNames_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollNames.SelectedIndexChanged
        If CollNames.SelectedIndex > -1 Then
            CollCodeMenuSel = CStr(CollNames.Items(CollNames.SelectedIndex)).Split("|"c)(1).Trim
            SelectKey.KeyList.Items.Clear()
        End If
    End Sub
    Private Sub DataEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataEntry.Click
        If CollNames.SelectedIndex >= 0 AndAlso CollNames.SelectedItems.Count = 1 Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim f As New DataEntry
            TypeEntryMenu = "E"
            f.nSuspend = 0
            f.DoSuspendLayout()
            f.Show()
            f.DoResumeLayout()
            Me.Cursor = System.Windows.Forms.Cursors.Default
        Else
            MsgBox(SysMsg(301))
        End If
    End Sub
    Private Sub DataChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataChange.Click
        If CollNames.SelectedIndex >= 0 AndAlso CollNames.SelectedItems.Count = 1 Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim f As New DataEntry
            TypeEntryMenu = "C"
            f.Show()
            Me.Cursor = System.Windows.Forms.Cursors.Default
        Else
            MsgBox(SysMsg(301))
        End If
    End Sub
    Private Sub Idxen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Idxen.Click
        If CollNames.SelectedIndex >= 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim f As New Indiceren
            f.Show()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Idxen.Visible = False
        Else
            SelectColls()
            If CollNames.SelectedIndex < 0 Then
                MsgBox(SysMsg(302))
            End If
        End If
    End Sub
    Private Sub SelectColls()
        ' haal CollSeq op van te indexeren records
        Dim SQLsfSelect As String = "SELECT t.CollCode, CollName FROM tobeindexed t, collections c WHERE t.CollCode = c.CollCode"
        Dim DRsfSelect As OdbcDataReader = Nothing
        Dim DCsfSelect As OdbcCommand
        DCsfSelect = New OdbcCommand(SQLsfSelect, DbConn)
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        Try
            DRsfSelect = DCsfSelect.ExecuteReader()
            Do While (DRsfSelect.Read())
                Dim s As String = GetDbStringValue(DRsfSelect, 1) & " | " & GetDbStringValue(DRsfSelect, 0)
                For i As Integer = 0 To CollNames.Items.Count - 1
                    If CStr(CollNames.Items(i)) = s Then
                        CollNames.SelectedIndices.Add(i)
                    End If
                Next
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRsfSelect Is Nothing) Then
                DRsfSelect.Close()
            End If
        End Try
        DbConn.Close()
    End Sub
    Private Sub DefSfn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BMaint.Click
        Dim f As New Maintenance
        f.Show()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BZoeken.Click
        Dim f As New Selectie
        f.Show()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BPrint.Click
        fPrinten.Show()
        fPrinten.LoadKeyList()
        fPrinten.LoadFormData()
        fPrinten.SetPrSpecs("")
        fPrinten.ToonScherm()
        fPrinten.BringToFront()
    End Sub
    Private Sub GenRex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If CollNames.SelectedIndex >= 0 Then
            Dim f As New GenRexx
            f.Show()
        Else
            MsgBox(SysMsg(301))
        End If
    End Sub
    Private Sub BMaakReg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BMaakReg.Click
        Dim f As New MkRegister
        f.Show()
    End Sub
    Friend Sub ReadSysMsg(ByVal Filename As String)
        Dim line As String
        Dim w As String
        Try
            Using sr As StreamReader = New StreamReader(Filename)
                ' Read and display the lines from the file until the end 
                ' of the file is reached.
                line = sr.ReadLine()
                While Not (line Is Nothing)
                    w = NxtWordFromStr(line)
                    If w.Length() > 5 AndAlso w.Substring(0, 5) = "ARCAT" Then
                        SysConst.Add(line, w)
                    End If
                    line = sr.ReadLine()
                End While
                sr.Close()
            End Using
        Catch E As Exception
            MsgBox("File: " & Filename & " not found or corrupted, cancelling program. Reinstall systemfiles", MsgBoxStyle.Exclamation)
            Me.Close()
        End Try
    End Sub
    Private Sub BSort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSort.Click
        Dim f As New Sortin
        f.Show()
    End Sub
    Function DoLogin() As Boolean
        Dim f As New Login()
        f.ShowDialog()
        Return f.correct
    End Function
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Dim f As New Selectie
        f.Show()
        Timer1.Enabled = False
    End Sub
End Class
