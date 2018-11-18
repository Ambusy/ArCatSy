'
Imports RxLib.Rexx
Public Class Selectie
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim fiSql As String
    Dim WithEvents Rx As New Rexx(New RexxCompData)
    Dim WithEvents Rxq As New Rexx(New RexxCompData)
    Dim RexxActive As Boolean = False
    Dim compiledQ As Boolean = False, compiled As Boolean = False
    Dim ExitCodes As New Collection, LastTfld As Object
    Private Function SqlQuoted(ByVal s As String) As String
        Dim i As Integer
        i = InStr(s, "'")
        While i > 0
            s = s.Substring(0, i) & "'" & s.Substring(i)
            i = InStr(i + 2, s, "'")
        End While
        Return "'" & s & "'"
    End Function
    Private Function QueryPart(ByVal O2 As Control, ByVal C2 As Control, ByVal T2 As Control, ByVal S2 As CheckBox, ByVal N2 As CheckBox, ByVal V2 As Control) As String
        Dim T2T As String = T2.Text.Trim().ToUpper ' field REGEXP '^-?[0-9]+$'
        Dim T2TO As String = T2T
        Dim O2T As String = O2.Text.Trim()
        Dim C2T As String = SqlQuoted(C2.Text.Trim().ToUpper)
        Dim s As String = "", ffiSql As String
        If T2T <> "" Then
            Dim qu As String
            If Not S2.Checked AndAlso Not N2.Checked AndAlso V2.Text <> "=" AndAlso IsNumeric(T2TO) Then
                T2T = " TermText REGEXP '^-?[0-9]+$' AND " + "TermText " & V2.Text & " " + T2T
            Else
                If V2.Text = "" Then V2.Text = "="
                If S2.Checked Then
                    qu = "TermText LIKE "
                    T2T = T2T & "%"
                ElseIf N2.Checked Then
                    qu = "TermText LIKE "
                    T2T = "%" & T2T & "%"
                Else
                    qu = "TermText " & V2.Text & " "
                End If
                T2T = qu & SqlQuoted(T2T)
            End If
            ffiSql = " " & fiSql
            If O2T = "&" Then
                s = ffiSql & " EXISTS(SELECT '1' FROM searchterms AS T2 "
                s = s & "WHERE T1.CollCode = T2.CollCode AND T1.CollSeq = T2.CollSeq "
                If C2T <> "''" Then
                    s = s & "AND T2.TermType = " & C2T & " AND T2." & T2T
                Else
                    s = s & "AND T2." & T2T
                End If
                s = s & ")"
            ElseIf O2T = "|" Or O2T = "" Then
                If fiSql = "AND" Then ffiSql = " OR" ' where stays where!
                If C2T <> "''" Then
                    s = ffiSql & " TermType = " & C2T & " AND " & T2T
                Else
                    s = ffiSql & " " & T2T
                End If
            Else
                s = ffiSql & " NOT EXISTS(SELECT '1' FROM searchterms AS T2 "
                s = s & "WHERE T1.CollCode = T2.CollCode AND T1.CollSeq = T2.CollSeq "
                If C2T <> "''" Then
                    s = s & "AND T2.TermType = " & C2T & " AND T2." & T2T
                Else
                    s = s & "AND T2." & T2T
                End If
                s = s & ")"
            End If
        End If
        fiSql = "AND"
        Return s
    End Function
    Private Sub BZoek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BZoek.Click
        DoeZoek()
    End Sub
    Private Sub DoeZoek()
        Dim s, t, cl As String, CollCode As String, i As Integer
        Me.Cursor = Cursors.WaitCursor
        Dim tSelectSQL As String = "SELECT DISTINCT CollCode, CollSeq FROM searchterms AS T1"
        cl = TCollCode.Text.Trim()
        If cl = "" Then
            fiSql = "WHERE"
        Else
            fiSql = "WHERE ("
        End If
        tSelectSQL = tSelectSQL & QueryPart(O1, C1, T1, S1, N1, V1)
        tSelectSQL = tSelectSQL & QueryPart(O2, C2, T2, S2, N2, V2)
        tSelectSQL = tSelectSQL & QueryPart(O3, C3, T3, S3, N3, V3)
        tSelectSQL = tSelectSQL & QueryPart(O4, C4, T4, S4, N4, V4)
        tSelectSQL = tSelectSQL & QueryPart(O5, C5, T5, S5, N5, V5)
        tSelectSQL = tSelectSQL & QueryPart(O6, C6, T6, S6, N6, V6)
        tSelectSQL = tSelectSQL & QueryPart(O7, C7, T7, S7, N7, V7)
        tSelectSQL = tSelectSQL & QueryPart(O8, C8, T8, S8, N8, V8)
        tSelectSQL = tSelectSQL & QueryPart(O9, C9, T9, S9, N9, V9)
        s = cl
        If s.Length() > 0 Then
            i = InStr(s, ",")
            t = ""
            While i > 0
                t = t & SqlQuoted(s.Substring(0, i - 1).Trim()) & ", "
                s = s.Substring(i)
                i = InStr(s, ",")
            End While
            t = t & SqlQuoted(s)
            tSelectSQL = tSelectSQL & ") AND CollCode in (" & t & ")"
            If TCollSeqnrV.Text.Trim().Length() > 0 Then
                If TCollSeqnrT.Text.Trim().Length() = 0 Then
                    tSelectSQL = tSelectSQL & " AND CollSeq = " & TCollSeqnrV.Text
                Else
                    tSelectSQL = tSelectSQL & " AND CollSeq >= " & TCollSeqnrV.Text & " AND CollSeq <= " & TCollSeqnrT.Text
                End If
            End If
        End If
        Dim DRtSelect As OdbcDataReader = Nothing
        Dim DbCtSelect As OdbcCommand
        DbConn.Open()
        DbCtSelect = New OdbcCommand(tSelectSQL, DbConn)
        KeysSelectedToShow.Clear()
        Try ' alle enties in trefwoorden
            DRtSelect = DbCtSelect.ExecuteReader()
            Do While (DRtSelect.Read())
                CollCode = GetDbStringValue(DRtSelect, 0)
                s = "," & CollCode.PadRight(4) & "," & Format(GetDbIntegerValue(DRtSelect, 1), "00000000")
                KeysSelectedToShow.Add(s)
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRtSelect Is Nothing) Then
                DRtSelect.Close()
            End If
        End Try
        DbConn.Close()
        If KeysSelectedToShow.Count() > 0 Then
            Reslt.Text = SysMsg1p(656, CStr(KeysSelectedToShow.Count()))
        Else
            Reslt.Text = SysMsg(657)
        End If
        BToon.Visible = (KeysSelectedToShow.Count() > 0)
        BLijst.Visible = BToon.Visible
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub O1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles O2.TextChanged, O3.TextChanged, O4.TextChanged, O5.TextChanged, O6.TextChanged, O7.TextChanged, O8.TextChanged, O9.TextChanged
        Dim c As Control
        c = DirectCast(sender, Control)
        If c.Name = "O1" Then
            If c.Text.Length() > 0 Then c.Text = ""
        Else
            If c.Text <> "" And c.Text <> "&" And c.Text <> "!" And c.Text <> "|" Then
                c.Text = ""
            End If
        End If
    End Sub
    Private Sub V1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles V1.TextChanged, V2.TextChanged, V3.TextChanged, V4.TextChanged, V5.TextChanged, V6.TextChanged, V7.TextChanged, V8.TextChanged, V9.TextChanged
        Dim c As Control, n As Integer, LOp As String = ""
        c = DirectCast(sender, Control)
        n = CInt(c.Name.Substring(1))
        Select Case n
            Case 1 : LOp = O1.Text
            Case 2 : LOp = O2.Text
            Case 3 : LOp = O3.Text
            Case 4 : LOp = O4.Text
            Case 5 : LOp = O5.Text
            Case 6 : LOp = O6.Text
            Case 7 : LOp = O7.Text
            Case 8 : LOp = O8.Text
            Case 9 : LOp = O9.Text
        End Select
        If LOp = "!" Then
            If c.Text <> "" And c.Text <> "=" Then
                c.Text = "="
            End If
        Else
            If c.Text <> "" And c.Text <> "=" And c.Text <> ">" And c.Text <> ">=" And c.Text <> "<" And c.Text <> "<=" Then
                c.Text = "="
            End If
        End If
    End Sub

    Private Sub T1_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles T1.KeyPress, T2.KeyPress, T3.KeyPress, T4.KeyPress, T5.KeyPress, T6.KeyPress, T7.KeyPress, T8.KeyPress, T9.KeyPress
        If e.KeyChar = " " Then
            Dim s As String = SysMsg(210).TrimEnd
            If s.Substring(s.Length - 1, 1) = "y" Then
                Dim c As TextBox, n As Integer
                c = DirectCast(sender, TextBox)
                n = CInt(c.Name.Substring(1))
                If c.Text.Trim.Length() > 0 Then
                    NextT(n + 1, True)
                End If
            End If
        End If
    End Sub
    Sub NextT(n As Integer, SetF As Boolean)
        Dim DConn As String = SysMsg(4)
        Select Case n
            Case 2 : O2.Visible = True : O2.Text = DConn : C2.Visible = True : T2.Visible = True : S2.Visible = True : N2.Visible = True : V2.Visible = True : If SetF Then T2.Focus()
            Case 3 : O3.Visible = True : O3.Text = DConn : C3.Visible = True : T3.Visible = True : S3.Visible = True : N3.Visible = True : V3.Visible = True : If SetF Then T3.Focus()
            Case 4 : O4.Visible = True : O4.Text = DConn : C4.Visible = True : T4.Visible = True : S4.Visible = True : N4.Visible = True : V4.Visible = True : If SetF Then T4.Focus()
            Case 5 : O5.Visible = True : O5.Text = DConn : C5.Visible = True : T5.Visible = True : S5.Visible = True : N5.Visible = True : V5.Visible = True : If SetF Then T5.Focus()
            Case 6 : O6.Visible = True : O6.Text = DConn : C6.Visible = True : T6.Visible = True : S6.Visible = True : N6.Visible = True : V6.Visible = True : If SetF Then T6.Focus()
            Case 7 : O7.Visible = True : O7.Text = DConn : C7.Visible = True : T7.Visible = True : S7.Visible = True : N7.Visible = True : V7.Visible = True : If SetF Then T7.Focus()
            Case 8 : O8.Visible = True : O8.Text = DConn : C8.Visible = True : T8.Visible = True : S8.Visible = True : N8.Visible = True : V8.Visible = True : If SetF Then T8.Focus()
            Case 9 : O9.Visible = True : O9.Text = DConn : C9.Visible = True : T9.Visible = True : S9.Visible = True : N9.Visible = True : V9.Visible = True : If SetF Then T9.Focus()
        End Select
        LO.Visible = True
        LO2.Visible = True
        LO3.Visible = True
    End Sub
    Private Sub T_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles T1.TextChanged, T2.TextChanged, T3.TextChanged, T4.TextChanged, T5.TextChanged, T6.TextChanged, T7.TextChanged, T8.TextChanged, T9.TextChanged
        Dim c As Control, n As Integer
        c = DirectCast(sender, Control)
        n = CInt(c.Name.Substring(1))
        If n = 1 Then BZoek.Visible = (c.Text.Trim.Length() > 0)
        LastTfld = sender
        If c.Text.Trim.Length() > 0 Then
            n += 1
            NextT(n, False)
        End If
    End Sub
    Private Sub BLijst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BLijst.Click
        SaveFileDialog1.FileName = ""
        SaveFileDialog1.CheckFileExists = False
        SaveFileDialog1.Title = SysMsg(557)
        SaveFileDialog1.Filter = "Key files (*.bew)|*.bew|All files (*.*)|*.*"
        SaveFileDialog1.ShowDialog()
        Dim Filename As String = SaveFileDialog1.FileName
        If Filename = "" Then
            Exit Sub
        End If
        If Not WritableFile(Filename, "SAVE") Then
            Exit Sub
        End If
FileDeleteErrorRes:
        Try
            If File.Exists(Filename) Then File.Delete(Filename)
        Catch ex As Exception
            Dim Msg As String = SysMsg1p(558, Filename)
            Dim Response As Integer = MsgBox(Msg, MsgBoxStyle.RetryCancel, Msg)
            If Response = MsgBoxResult.Retry Then ' user requests RETRY.
                GoTo FileDeleteErrorRes
            Else
                Exit Sub
            End If
        End Try
        Dim st As Stream = File.Open(Filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
        Wrtr = New StreamWriter(st)
        For Each s As String In KeysSelectedToShow
            Wrt(s)
        Next
        Wrtr.Close()
        Wrtr.Dispose()
        st.Close()
        st.Dispose()
    End Sub
    Private Sub BToon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BToon.Click
        DoeToon("")
    End Sub
    Private Sub DoeToon(ByVal s As String)
        Me.Cursor = Cursors.WaitCursor
        fPrinten.Show()
        fPrinten.LoadFormData()
        fPrinten.SetPrSpecs(s)
        fPrinten.ToonScherm()
        fPrinten.BringToFront()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub Selectie_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not (DbConn Is Nothing) Then
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End If
    End Sub
    Private Sub Selectie_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = vbCr Then
            C1T1_Leave(LastTfld, e)
            BZoek_Click(sender, New EventArgs)
            My.Computer.Keyboard.SendKeys("{TAB}")
        End If
    End Sub
    Private Sub Selectie_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = SysMsg(505)
        Me.DesktopLocation = New Point(0, 0)
        BZoek.Text = SysMsg(651)
        BLijst.Text = SysMsg(652)
        BToon.Text = SysMsg(653)
        BClear.Text = SysMsg(661)
        BRexx.Text = SysMsg(660)
        Label2.Text = SysMsg(654)
        Label3.Text = SysMsg(655)
        Label4.Text = SysMsg(658)
        Label1.Text = SysMsg(659)
        Label8.Text = SysMsg(662)
        Label9.Text = SysMsg(663)
        Label10.Text = SysMsg(664)
        Reslt.Text = ""
        TCollCode.Text = ""
        For Each s As String In Form1.CollNames.SelectedItems
            s = s
            TCollCode.Text = TCollCode.Text & s.Substring(1 + InStr(s, "|")) & ","
        Next
        If TCollCode.Text <> "" Then TCollCode.Text = TCollCode.Text.Substring(0, TCollCode.Text.Length() - 1)
        T1.Select()
        BRexx.Visible = File.Exists(My.Application.Info.DirectoryPath & "\SEARCH.rex")
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        Dim ExitSQL As String = "SELECT DISTINCT FldTermType FROM collfields WHERE FldQExit = true"
        Dim DRExit As OdbcDataReader = Nothing
        Dim DbCExit As OdbcCommand, TermCode As String
        DbCExit = New OdbcCommand(ExitSQL, DbConn)
        Try
            DRExit = DbCExit.ExecuteReader()
            Do While (DRExit.Read())
                TermCode = GetDbStringValue(DRExit, 0)
                ExitCodes.Add(TermCode, TermCode)
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRExit Is Nothing) Then
                DRExit.Close()
            End If
        End Try
        Try
            DbConn.Close()
        Catch ex As Exception
        End Try
        If ExitCodes.Count() > 0 Then
            If Rxq.CompileRexxScript(My.Application.Info.DirectoryPath & "\QUERYEXIT") <> 0 Then
                MsgBox(SysMsg(306))
                Exit Sub
            End If
            compiledQ = True
        End If
    End Sub

    Private Sub C1T1_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles T1.Leave, T2.Leave, T3.Leave, T4.Leave, T5.Leave, T6.Leave, T7.Leave, T8.Leave, T9.Leave, C1.Leave, C2.Leave, C3.Leave, C4.Leave, C5.Leave, C6.Leave, C7.Leave, C8.Leave, C9.Leave
        If compiledQ Then
            Dim c As Control, n As Integer
            c = DirectCast(sender, Control)
            n = CInt(c.Name.Substring(1))
            Select Case n
                Case 1 : If T1.Text.Length() > 0 AndAlso ExitCodes.Contains(C1.Text) Then QExit(C1.Text & " " & T1.Text, 1)
                Case 2 : If T2.Text.Length() > 0 AndAlso ExitCodes.Contains(C2.Text) Then QExit(C2.Text & " " & T2.Text, 2)
                Case 3 : If T3.Text.Length() > 0 AndAlso ExitCodes.Contains(C3.Text) Then QExit(C3.Text & " " & T3.Text, 3)
                Case 4 : If T4.Text.Length() > 0 AndAlso ExitCodes.Contains(C4.Text) Then QExit(C4.Text & " " & T4.Text, 4)
                Case 5 : If T5.Text.Length() > 0 AndAlso ExitCodes.Contains(C5.Text) Then QExit(C5.Text & " " & T5.Text, 5)
                Case 6 : If T6.Text.Length() > 0 AndAlso ExitCodes.Contains(C6.Text) Then QExit(C6.Text & " " & T6.Text, 6)
                Case 7 : If T7.Text.Length() > 0 AndAlso ExitCodes.Contains(C7.Text) Then QExit(C7.Text & " " & T7.Text, 7)
                Case 8 : If T8.Text.Length() > 0 AndAlso ExitCodes.Contains(C8.Text) Then QExit(C8.Text & " " & T8.Text, 8)
                Case 9 : If T9.Text.Length() > 0 AndAlso ExitCodes.Contains(C9.Text) Then QExit(C9.Text & " " & T9.Text, 9)
            End Select
        End If
    End Sub
    Private Sub QExit(ByVal p As String, ByVal n As Integer)
        Dim i As Integer = Rxq.ExecuteRexxScript(p)
        If i = 0 Then
            Dim execName As String = String.Empty, srcName As String = String.Empty
            Dim cvr As  DefVariable = Nothing
            p = Rxq.GetVar(Rxq.SourceNameIndexPosition("T",  Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
            Select Case n
                Case 1 : T1.Text = p
                Case 2 : T2.Text = p
                Case 3 : T3.Text = p
                Case 4 : T4.Text = p
                Case 5 : T5.Text = p
                Case 6 : T6.Text = p
                Case 7 : T7.Text = p
                Case 8 : T8.Text = p
                Case 9 : T9.Text = p
            End Select
        End If
    End Sub
    Private Sub S1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S1.CheckedChanged, S2.CheckedChanged, S3.CheckedChanged, S4.CheckedChanged, S5.CheckedChanged, S6.CheckedChanged, S7.CheckedChanged, S8.CheckedChanged, S9.CheckedChanged
        Dim c As Control, n As Integer
        c = DirectCast(sender, Control)
        n = CInt(c.Name.Substring(1))
        Select Case n
            Case 1 : If S1.Checked Then N1.Checked = False : V1.Text = "="
            Case 2 : If S2.Checked Then N2.Checked = False : V2.Text = "="
            Case 3 : If S3.Checked Then N3.Checked = False : V3.Text = "="
            Case 4 : If S4.Checked Then N4.Checked = False : V4.Text = "="
            Case 5 : If S5.Checked Then N5.Checked = False : V5.Text = "="
            Case 6 : If S6.Checked Then N6.Checked = False : V6.Text = "="
            Case 7 : If S7.Checked Then N7.Checked = False : V7.Text = "="
            Case 8 : If S8.Checked Then N8.Checked = False : V8.Text = "="
            Case 9 : If S9.Checked Then N9.Checked = False : V9.Text = "="
        End Select
    End Sub
    Private Sub N1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles N1.CheckedChanged, N2.CheckedChanged, N3.CheckedChanged, N4.CheckedChanged, N5.CheckedChanged, N6.CheckedChanged, N7.CheckedChanged, N8.CheckedChanged, N9.CheckedChanged
        Dim c As Control, n As Integer
        c = DirectCast(sender, Control)
        n = CInt(c.Name.Substring(1))
        Select Case n
            Case 1 : If N1.Checked Then S1.Checked = False : V1.Text = "="
            Case 2 : If N2.Checked Then S2.Checked = False : V2.Text = "="
            Case 3 : If N3.Checked Then S3.Checked = False : V3.Text = "="
            Case 4 : If N4.Checked Then S4.Checked = False : V4.Text = "="
            Case 5 : If N5.Checked Then S5.Checked = False : V5.Text = "="
            Case 6 : If N6.Checked Then S6.Checked = False : V6.Text = "="
            Case 7 : If N7.Checked Then S7.Checked = False : V7.Text = "="
            Case 8 : If N8.Checked Then S8.Checked = False : V8.Text = "="
            Case 9 : If N9.Checked Then S9.Checked = False : V9.Text = "="
        End Select
    End Sub
    Sub RexxCmd(ByVal env As String, ByVal s As String, ByVal e As  RexxEvent, ByRef RexxEnv As  Rexx) Handles Rx.doCmd
        Dim FiW, wO, wV, wC, wT As String, eRc As Integer = 0, i As Integer
        Dim os As String = s
        RexxActive = True
        Try
            FiW = NxtWordFromStr(s).ToUpper(CultInf)
            Select Case FiW
                Case "S"
                    i = CInt(NxtWordFromStr(s))
                    wO = NxtWordFromStr(s)
                    wV = NxtWordFromStr(s)
                    wC = NxtWordFromStr(s)
                    If wC = "*" Then wC = ""
                    wT = s
                    Select Case i
                        Case 1 : O1.Text = wO : V1.Text = wV : C1.Text = wC : T1.Text = wT
                        Case 2 : O2.Text = wO : V2.Text = wV : C2.Text = wC : T2.Text = wT
                        Case 3 : O3.Text = wO : V3.Text = wV : C3.Text = wC : T3.Text = wT
                        Case 4 : O4.Text = wO : V4.Text = wV : C4.Text = wC : T4.Text = wT
                        Case 5 : O5.Text = wO : V5.Text = wV : C5.Text = wC : T5.Text = wT
                        Case 6 : O6.Text = wO : V6.Text = wV : C6.Text = wC : T6.Text = wT
                        Case 7 : O7.Text = wO : V7.Text = wV : C7.Text = wC : T7.Text = wT
                        Case 8 : O8.Text = wO : V8.Text = wV : C8.Text = wC : T8.Text = wT
                        Case 9 : O9.Text = wO : V9.Text = wV : C9.Text = wC : T9.Text = wT
                    End Select
                Case "COLL"
                    TCollCode.Text = s
                Case "COLLSEQ"
                    TCollSeqnrV.Text = NxtWordFromStr(s)
                    TCollSeqnrT.Text = s
                Case "SEARCH"
                    DoeZoek()
                    If KeysSelectedToShow.Count() = 0 Then eRc = 4
                Case "SHOW"
                    If KeysSelectedToShow.Count() > 0 Then
                        DoeToon(s)
                        Me.Close()
                    End If
            End Select
            e.rc = eRc
        Catch ex As Exception
            MsgBox(ex.Message & " : " & os & " (" & s & ")")
        End Try
        RexxActive = False
    End Sub
    Private Sub BRexx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BRexx.Click
        If Not compiled Then
            If Rx.CompileRexxScript(My.Application.Info.DirectoryPath & "\SEARCH") <> 0 Then
                MsgBox(SysMsg(306))
                Exit Sub
            End If
            compiled = True
        End If
        Rx.ExecuteRexxScript("I") ' Initial call
    End Sub
    Private Sub BClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BClear.Click
        O1.Text = "|" : V1.Text = "=" : C1.Text = "" : T1.Text = "" : N1.Checked = False : S1.Checked = False
        O2.Text = "" : V2.Text = "=" : C2.Text = "" : T2.Text = "" : N2.Checked = False : S2.Checked = False
        O3.Text = "" : V3.Text = "=" : C3.Text = "" : T3.Text = "" : N3.Checked = False : S3.Checked = False
        O4.Text = "" : V4.Text = "=" : C4.Text = "" : T4.Text = "" : N4.Checked = False : S4.Checked = False
        O5.Text = "" : V5.Text = "=" : C5.Text = "" : T5.Text = "" : N5.Checked = False : S5.Checked = False
        O6.Text = "" : V6.Text = "=" : C6.Text = "" : T6.Text = "" : N6.Checked = False : S6.Checked = False
        O7.Text = "" : V7.Text = "=" : C7.Text = "" : T7.Text = "" : N7.Checked = False : S7.Checked = False
        O8.Text = "" : V8.Text = "=" : C8.Text = "" : T8.Text = "" : N8.Checked = False : S8.Checked = False
        O9.Text = "" : V9.Text = "=" : C9.Text = "" : T9.Text = "" : N9.Checked = False : S9.Checked = False
        '
        O2.Visible = False : V2.Visible = False : C2.Visible = False : T2.Visible = False : N2.Visible = False : S2.Visible = False
        O3.Visible = False : V3.Visible = False : C3.Visible = False : T3.Visible = False : N3.Visible = False : S3.Visible = False
        O4.Visible = False : V4.Visible = False : C4.Visible = False : T4.Visible = False : N4.Visible = False : S4.Visible = False
        O5.Visible = False : V5.Visible = False : C5.Visible = False : T5.Visible = False : N5.Visible = False : S5.Visible = False
        O6.Visible = False : V6.Visible = False : C6.Visible = False : T6.Visible = False : N6.Visible = False : S6.Visible = False
        O7.Visible = False : V7.Visible = False : C7.Visible = False : T7.Visible = False : N7.Visible = False : S7.Visible = False
        O8.Visible = False : V8.Visible = False : C8.Visible = False : T8.Visible = False : N8.Visible = False : S8.Visible = False
        O9.Visible = False : V9.Visible = False : C9.Visible = False : T9.Visible = False : N9.Visible = False : S9.Visible = False
        BLijst.Visible = False
        BToon.Visible = False
    End Sub
End Class
