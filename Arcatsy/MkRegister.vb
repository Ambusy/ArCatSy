'
Imports RxLib.Rexx
Public Class MkRegister
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim pSelectSQL As String = "SELECT CollCode, CollName FROM collections"
    Dim DRpSelect As OdbcDataReader = Nothing
    Dim DbCpSelect As OdbcCommand

    Dim kSelectSQL As String = "SELECT FldCode, FldCaption FROM collfields WHERE CollCode = ? ORDER BY FldCode"
    Dim DRkSelect As OdbcDataReader = Nothing
    Dim DbCkSelect As OdbcCommand
    Dim kSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim dSelectSQL As String = "SELECT FldText, CollSeq FROM data WHERE CollCode = ? AND FldCode = ?"
    Dim DRdSelect As OdbcDataReader = Nothing
    Dim DbCdSelect As OdbcCommand
    Dim dSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim dSelP2 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)

    Dim dkSelectSQL As String = "SELECT DISTINCT CollSeq FROM data WHERE CollCode = ?"
    Dim DRdkSelect As OdbcDataReader = Nothing
    Dim DbCdkSelect As OdbcCommand
    Dim dkSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim CollCodeNode, FldlCodeNode As TreeNode
    Dim nodeClick As Boolean = False
    Dim WithEvents Rx As New  Rexx(New  RexxCompData)
    Dim RxCompiled As Boolean
    Dim WithEvents RxP As New  Rexx(New  RexxCompData)
    Dim RexxActive As Boolean = False
    Dim compiled As Boolean = False
    Private Sub MkRegister_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not (DbConn Is Nothing) Then
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End If
    End Sub
    Private Sub MkRegister_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = SysMsg(551)
        Me.DesktopLocation = New Point(0, 0)
        Label1.Text = SysMsg(552)
        Label2.Text = SysMsg(553)
        Lowerc.Text = SysMsg(554)
        BBrws.Text = SysMsg(555)
        BMkReg.Text = SysMsg(556)
        BRexx.Text = SysMsg(660)
        BPrt.Text = SysMsg(603)
        If File.Exists(My.Application.Info.DirectoryPath & "\REGISTER.rex") Then
            BRexx.Visible = True
        Else
            BRexx.Visible = False
        End If
        TDsn.Visible = True
        Label2.Visible = True
        BBrws.Visible = True

        DbCpSelect = New OdbcCommand(pSelectSQL, DbConn)
        DbCkSelect = New OdbcCommand(kSelectSQL, DbConn)
        DbCkSelect.Parameters.Add(kSelP1)
        DbCdSelect = New OdbcCommand(dSelectSQL, DbConn)
        DbCdSelect.Parameters.Add(dSelP1)
        DbCdSelect.Parameters.Add(dSelP2)

        DbCdkSelect = New OdbcCommand(dkSelectSQL, DbConn)
        DbCdkSelect.Parameters.Add(dkSelP1)

        BouwNaamLijst()
    End Sub
    Private Sub BouwNaamLijst()
        DbConn.Open()
        NaamLst.Nodes.Clear()
        Try ' collections
            DRpSelect = DbCpSelect.ExecuteReader()
            Do While (DRpSelect.Read())
                CollCodeNode = New TreeNode(GetDbStringValue(DRpSelect, 0) & ": " & GetDbStringValue(DRpSelect, 1))
                CollCodeNode.Name = GetDbStringValue(DRpSelect, 0)
                CollCodeNode.Checked = False
                NaamLst.Nodes.Add(CollCodeNode)
                Try ' alle searchterms
                    kSelP1.Value = CollCodeNode.Name
                    DRkSelect = DbCkSelect.ExecuteReader()
                    Do While (DRkSelect.Read())
                        FldlCodeNode = New TreeNode(GetDbStringValue(DRkSelect, 0) & ": " & GetDbStringValue(DRkSelect, 1))
                        FldlCodeNode.Name = GetDbStringValue(DRkSelect, 0)
                        FldlCodeNode.Checked = False
                        CollCodeNode.Nodes.Add(FldlCodeNode)
                    Loop
                Catch ex As Exception
                    MsgBox(ex.ToString())
                Finally
                    If Not (DRkSelect Is Nothing) Then
                        DRkSelect.Close()
                    End If
                End Try
                FldlCodeNode = New TreeNode("0: " & SysMsg(559))
                FldlCodeNode.Name = "0"
                FldlCodeNode.Checked = False
                CollCodeNode.Nodes.Add(FldlCodeNode)
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRpSelect Is Nothing) Then
                DRpSelect.Close()
            End If
        End Try
        DbConn.Close()
    End Sub
    Private Sub BrVis()
        If nodeClick Then BMkReg.Visible = True
    End Sub
    Private Sub NaamLst_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles NaamLst.NodeMouseClick
        Dim s As String = e.Node.FullPath
        Dim i As Integer = InStr(1, s, "\")
        If i = 0 Then Exit Sub ' primary node clicked
        Dim mn As String = s.Substring(0, i - 1)
        Dim ln As String = s.Substring(i)
        nodeClick = True
        BrVis()
        For Each n As TreeNode In NaamLst.Nodes
            If n.Text = mn Then
                For Each sn As TreeNode In n.Nodes
                    If sn.Text = ln AndAlso sn.Checked Then
                        n.Checked = True
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub BBrws_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BBrws.Click
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
        TDsn.Text = Filename
    End Sub
    Private Sub BMkReg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BMkReg.Click
        If TDsn.Text.Trim() = "" Then
            TDsn.Text = "Z automaticRegister.bew"
        End If
        MaakReg()
    End Sub
    Private Sub MaakReg()
        Dim mc As MatchCollection, rt As String, CollSeq, i As Integer
        Dim execName As String = String.Empty, srcName As String = String.Empty
        Dim cvr As  DefVariable = Nothing
        Me.Cursor = Cursors.WaitCursor
        If CheckRexx.Checked And Not RxCompiled Then
            If Rx.CompileRexxScript(My.Application.Info.DirectoryPath & "\TERMS") <> 0 Then
                MsgBox(SysMsg(306))
                Exit Sub
            End If
            RxCompiled = True
        End If
        DbConn.Open()
        Dim st As Stream = File.Open(TDsn.Text, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
        Wrtr = New StreamWriter(st)
        For Each Me.CollCodeNode In NaamLst.Nodes
            If CollCodeNode.Checked Then
                For Each Me.FldlCodeNode In CollCodeNode.Nodes
                    If FldlCodeNode.Checked Then
                        If FldlCodeNode.Name = "0" Then
                            Try
                                dkSelP1.Value = CollCodeNode.Name
                                DRdkSelect = DbCdkSelect.ExecuteReader()
                                Do While (DRdkSelect.Read())
                                    CollSeq = GetDbIntegerValue(DRdkSelect, 0)
                                    If CheckRegExp.Checked Then
                                        Wrt(CollCodeNode.Name & " " & Format(CollSeq, "00000000") & "," & CollCodeNode.Name.PadRight(4) & "," & Format(CollSeq, "00000000"))
                                    Else
                                        If Rx.ExecuteRexxScript("R " & CollCodeNode.Name & " " & FldlCodeNode.Name & " " & CollCodeNode.Name & " " & CStr(CollSeq)) = 0 Then
                                            Dim nrt As Integer = CInt(Rx.GetVar(Rx.SourceNameIndexPosition("T.0",  Rexx.tpSymbol.tpVariable, cvr), execName, srcName))
                                            For i = 1 To nrt
                                                rt = Rx.GetVar(Rx.SourceNameIndexPosition("T." & CStr(i),  Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
                                                Wrt(rt & "," & CollCodeNode.Name.PadRight(4) & "," & Format(CollSeq, "00000000"))
                                            Next
                                        End If
                                    End If
                                Loop
                            Catch ex As Exception
                                MsgBox(ex.ToString())
                            Finally
                                If Not (DRdkSelect Is Nothing) Then
                                    DRdkSelect.Close()
                                End If
                            End Try
                        Else
                            Try
                                dSelP1.Value = CollCodeNode.Name
                                dSelP2.Value = FldlCodeNode.Name
                                DRdSelect = DbCdSelect.ExecuteReader()
                                Do While (DRdSelect.Read())
                                    Dim Txt As String = GetDbStringValue(DRdSelect, 0)
                                    CollSeq = GetDbIntegerValue(DRdSelect, 1)
                                    If CheckRegExp.Checked Then
                                        ' match tekst in data tegen regexp
                                        Dim regExpre As New Regex(TRegEx.Text)
                                        mc = regExpre.Matches(Txt)
                                        For Each m As Match In mc
                                            ' nieuw trefwoord opslaan
                                            If m.Length() > 0 AndAlso m.Value.Trim().Length() > 0 Then
                                                If Lowerc.Checked Then
                                                    rt = m.Value.Trim().ToLower()
                                                Else
                                                    rt = m.Value.Trim()
                                                End If
                                                Wrt(rt & "," & CollCodeNode.Name.PadRight(4) & "," & Format(CollSeq, "00000000"))
                                            End If
                                        Next
                                    Else
                                        If Rx.ExecuteRexxScript("R " & CollCodeNode.Name & " " & FldlCodeNode.Name & " " & Txt) = 0 Then
                                            Dim nrt As Integer = CInt(Rx.GetVar(Rx.SourceNameIndexPosition("T.0",  Rexx.tpSymbol.tpVariable, cvr), execName, srcName))
                                            For i = 1 To nrt
                                                rt = Rx.GetVar(Rx.SourceNameIndexPosition("T." & CStr(i),  Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
                                                Wrt(rt & "," & CollCodeNode.Name.PadRight(4) & "," & Format(CollSeq, "00000000"))
                                            Next
                                        End If
                                    End If
                                Loop
                            Catch ex As Exception
                                MsgBox(ex.ToString())
                            Finally
                                If Not (DRdSelect Is Nothing) Then
                                    DRdSelect.Close()
                                End If
                            End Try
                        End If
                    End If
                Next
            End If
        Next
        DbConn.Close()
        Wrtr.Close()
        st.Close()
        Dim myProcess As Process
        myProcess = Process.Start("SORT", """" & TDsn.Text & """ /O """ & TDsn.Text & ".SORTED""")
        myProcess.WaitForExit()
        Dim sti As Stream = File.Open(TDsn.Text & ".SORTED", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Rdr = New StreamReader(sti)
        st = File.Open(TDsn.Text, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
        Wrtr = New StreamWriter(st)
        Dim vs As String = Nothing
        Dim s As String
        KeysSelectedToShow.Clear()
        Do Until Rdr.Peek = -1
            s = Rdr.ReadLine
            If s <> vs Then
                Wrt(s)
                KeysSelectedToShow.Add(s) ' to print directly
                vs = s
            End If
        Loop
        Wrtr.Close()
        Rdr.Close()
        sti.Close()
        st.Close()
        Kill(TDsn.Text & ".SORTED")
        BPrt.Visible = (KeysSelectedToShow.Count > 0)
        BMkReg.Visible = (KeysSelectedToShow.Count = 0)
        TDsn.Visible = False
        Label2.Visible = False
        BBrws.Visible = False
        BRexx.Visible = False
        Me.Cursor = Cursors.Default
        If KeysSelectedToShow.Count = 0 Then
            MsgBox(SysMsg(560))
        End If
    End Sub
    Private Sub BPrt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BPrt.Click
        fPrinten.Show()
        fPrinten.LoadFormData()
        fPrinten.SetPrSpecs("")
        fPrinten.ToonScherm()
        fPrinten.BringToFront()
        Me.Close()
    End Sub
    Private Sub CheckRegExp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckRegExp.CheckedChanged
        If CheckRegExp.Checked Then
            CheckRexx.Checked = False
            GrRegExp.Visible = True
        Else
            CheckRexx.Checked = True
            GrRegExp.Visible = False
        End If
        BMkReg.Visible = False
        BrVis()
    End Sub
    Private Sub CheckRexx_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckRexx.CheckedChanged
        If CheckRexx.Checked Then
            CheckRegExp.Checked = False
        Else
            CheckRegExp.Checked = True
        End If
        BMkReg.Visible = False
        BrVis()
    End Sub
    Sub RexxCmd(ByVal env As String, ByVal s As String, ByVal e As  RexxEvent, ByRef RexxEnv As  Rexx) Handles RxP.doCmd
        Dim FiW, CollCode, FldCode As String, eRc As Integer = 0
        Dim execName As String = String.Empty, srcName As String = String.Empty, n As String = String.Empty, k As Integer
        Dim cvr As  DefVariable = Nothing
        Dim os As String = s
        RexxActive = True
        Try
            FiW = NxtWordFromStr(s).ToUpper(CultInf)
            Select Case FiW
                Case "REGEXP"
                    CheckRegExp.Checked = True
                    TRegEx.Text = s
                Case "ACTCOLL"
                    Dim st As String = "", nt As String
                    For Each x As String In Form1.CollNames.SelectedItems
                        nt = x.Split("|"c)(1).Trim
                        st = st & ", " & nt
                    Next
                    If st.Length() > 0 Then st = st.Substring(2)
                    RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition("ACTCOLL.1",  Rexx.tpSymbol.tpVariable, cvr), st, k, execName, n)
                Case "REXX"
                    CheckRexx.Checked = True
                Case "R"
                    CollCode = NxtWordFromStr(s)
                    FldCode = NxtWordFromStr(s)
                    For Each Me.CollCodeNode In NaamLst.Nodes
                        If CollCodeNode.Name.TrimEnd = CollCode Then
                            For Each Me.FldlCodeNode In CollCodeNode.Nodes
                                If FldlCodeNode.Name.TrimEnd = FldCode Then
                                    CollCodeNode.Checked = True
                                    CollCodeNode.Expand()
                                    FldlCodeNode.Checked = True
                                End If
                            Next
                        End If
                    Next
                Case "CREATE"
                    If s = "" Then
                        s = "Z automaticRegister.bew"
                    End If
                    TDsn.Text = s
                    MaakReg()
                    If KeysSelectedToShow.Count() = 0 Then eRc = 4
                Case "SHOW"
                    If KeysSelectedToShow.Count() > 0 Then
                        DoeToon(s)
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
            If RxP.CompileRexxScript(My.Application.Info.DirectoryPath & "\REGISTER") <> 0 Then
                MsgBox(SysMsg(306))
                Exit Sub
            End If
            compiled = True
        End If
        RxP.ExecuteRexxScript("I") ' Initial call
    End Sub
    Private Sub DoeToon(ByVal s As String)
        fPrinten.Show()
        fPrinten.LoadFormData()
        fPrinten.SetPrSpecs(s)
        If s = "" Then fPrinten.ToonScherm()
        fPrinten.BringToFront()
        Me.Close()
    End Sub
End Class
