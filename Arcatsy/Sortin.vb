'
Imports RxLib.Rexx
Public Class Sortin
    Dim DbConn As New OdbcConnection(SqlProv)

    Dim pSelectSQL As String = "SELECT CollName FROM collections WHERE CollCode = ?"
    Dim DRpSelect As OdbcDataReader = Nothing
    Dim DbCpSelect As OdbcCommand
    Dim pSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim kSelectSQL As String = "SELECT FldCode, FldCaption, FldParent, FldSExit FROM collfields WHERE CollCode = ?"
    Dim DRkSelect As OdbcDataReader = Nothing
    Dim DbCkSelect As OdbcCommand
    Dim kSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim dSelectSQL As String = "SELECT FldText, FldSeqNr FROM data WHERE CollCode = ? AND CollSeq = ? AND FldCode = ? AND FldSeqnr >= ? AND FldSeqnr <= ?"
    Dim DRdSelect As OdbcDataReader = Nothing
    Dim DbCdSelect As OdbcCommand
    Dim dSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim dSelP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    Dim dSelP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)
    Dim dSelP4 As New OdbcParameter("@FldSeq1", Odbc.OdbcType.Decimal, 16)
    Dim dSelP5 As New OdbcParameter("@FldSeqn", Odbc.OdbcType.Decimal, 16)

    Dim CollCodeNode, FldCodeNode As TreeNode
    Dim nodeClick As Boolean = False
    Dim SrtDefs As Collection, SrtTermTmp, SrtNextTerm, SrtTermsCompl As New Collection

    Dim WithEvents Rx As New  Rexx(New  RexxCompData)
    Dim RxCompiled As Boolean = False
    Dim execName As String = String.Empty, srcName As String = String.Empty
    Dim cvr As  DefVariable = Nothing

    Private Sub Sortin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = SysMsg(801)
        Me.DesktopLocation = New Point(0, 0)
        BSrt.Text = SysMsg(802)
        Label1.Text = SysMsg(803)
        BBrws.Text = SysMsg(804)
        BPrt.Text = SysMsg(805)

        DbCpSelect = New OdbcCommand(pSelectSQL, DbConn)
        DbCpSelect.Parameters.Add(pSelP1)

        DbCkSelect = New OdbcCommand(kSelectSQL, DbConn)
        DbCkSelect.Parameters.Add(kSelP1)

        DbCdSelect = New OdbcCommand(dSelectSQL, DbConn)
        DbCdSelect.Parameters.Add(dSelP1)
        DbCdSelect.Parameters.Add(dSelP2)
        DbCdSelect.Parameters.Add(dSelP3)
        DbCdSelect.Parameters.Add(dSelP4)
        DbCdSelect.Parameters.Add(dSelP5)

    End Sub
    Private Sub Sortin_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not (DbConn Is Nothing) Then
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSrt.Click
        Dim s As String, i, j As Integer, CollCode, CollSeqs, FldCode As String, CollSeq As Integer
        Dim kpc As KenmDef, SrtDef As Collection, fnd As Boolean, kp As KenmDef
        BSrt.Visible = False ' een keer per scherm
        Me.Cursor = Cursors.WaitCursor
        For Each n As TreeNode In NaamLst.Nodes
            i = InStrRev(n.Text, ":")
            CollCode = n.Text.Substring(0, i - 1).TrimEnd
            SrtDef = DirectCast(SrtDefs.Item(CollCode), Collection)
            For Each kp In SrtDef
                fnd = False
                For Each sn As TreeNode In n.Nodes
                    i = InStrRev(sn.Text, ":")
                    FldCode = sn.Text.Substring(0, i - 1).TrimEnd
                    If kp.FldCode = FldCode Then
                        fnd = sn.Checked
                        Exit For
                    End If
                Next
                If Not fnd Then
                    SrtDef.Remove(kp.FldCode)
                End If
            Next
            fnd = True
            While fnd
                fnd = False
                For Each kp In SrtDef
                    If kp.FldParent <> String.Empty AndAlso Not SrtDef.Contains(kp.FldParent) Then
                        fnd = True
                        kp.FldParent = ""
                    End If
                Next
            End While
        Next
        DbConn.Open()
        Dim sti As Stream = File.Open(TDsn.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Rdr = New StreamReader(sti)
        KeysSelectedToShow.Clear()
        Do Until Rdr.Peek = -1
            s = Rdr.ReadLine()
            i = InStrRev(s, ","c)
            j = InStrRev(s, ","c, i - 1)
            CollCode = s.Substring(j, i - j - 1).TrimEnd
            CollSeqs = s.Substring(i)
            s = "," & CollCode & "," & CollSeqs ' alleen key gedeelte
            KeysSelectedToShow.Add(s)
        Loop
        Rdr.Close()
        sti.Close()
        Dim sto As Stream = File.Open(TDsn.Text & ".TMP", FileMode.Create, FileAccess.ReadWrite, FileShare.None)
        Wrtr = New StreamWriter(sto)
        For Each s In KeysSelectedToShow
            i = InStrRev(s, ","c)
            j = InStrRev(s, ","c, i - 1)
            CollCode = s.Substring(j, i - j - 1).TrimEnd
            CollSeq = CInt(s.Substring(i))
            For Each n As TreeNode In NaamLst.Nodes
                i = InStrRev(n.Text, ":")
                If n.Text.Substring(0, i - 1) = CollCode Then
                    Dim mxNr As Integer = 0
                    For Each sn As TreeNode In n.Nodes
                        i = InStrRev(sn.Text, "|")
                        If i > 0 Then
                            j = CInt(sn.Text.Substring(i + 1))
                            If j > mxNr Then mxNr = j
                        End If
                    Next
                    For nr As Integer = 1 To mxNr
                        For Each sn As TreeNode In n.Nodes
                            i = InStrRev(sn.Text, " | ")
                            If i > 0 Then
                                If CInt(sn.Text.Substring(i + 2)) = nr Then
                                    j = InStrRev(sn.Text, ":")
                                    FldCode = sn.Text.Substring(0, j - 1).TrimEnd
                                    kpc = DirectCast(DirectCast(SrtDefs.Item(CollCode), Collection).Item(FldCode), KenmDef)
                                    If kpc.FldParent = "" Then
                                        AddSortFld("", CollCode, CollSeq, FldCode, SrtNextTerm, 0, 999999999)
                                    End If
                                    If SrtTermsCompl.Count = 0 Then
                                        For Each ix As String In SrtNextTerm
                                            SrtTermsCompl.Add(ix)
                                        Next
                                    Else
                                        For Each ix As String In SrtTermsCompl
                                            If SrtNextTerm.Count = 0 Then
                                                SrtTermTmp.Add(ix)
                                            Else
                                                For Each ixn As String In SrtNextTerm
                                                    SrtTermTmp.Add(ix & ixn)
                                                Next
                                            End If
                                        Next
                                        SrtTermsCompl.Clear()
                                        For Each ix As String In SrtTermTmp
                                            SrtTermsCompl.Add(ix)
                                        Next
                                        SrtTermTmp.Clear()
                                    End If
                                    SrtNextTerm.Clear()
                                End If
                            End If
                        Next
                    Next
                End If
            Next
            For Each ix As String In SrtTermsCompl
                Wrt(ix & "," & CollCode.PadRight(4) & "," & Format(CollSeq, "00000000"))
            Next
            SrtTermsCompl.Clear()
        Next
        Rdr.Close()
        Wrtr.Close()
        sto.Close()
        sti.Close()
        DbConn.Close()
        If KeysSelectedToShow.Count > 0 Then
            Dim myProcess As Process
            myProcess = Process.Start("SORT", """" & TDsn.Text & ".TMP"" /O """ & TDsn.Text & ".SORTED""")
            myProcess.WaitForExit()
            Kill(TDsn.Text & ".TMP")
            sti = File.Open(TDsn.Text & ".SORTED", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Rdr = New StreamReader(sti)
            sto = File.Open(TDsn.Text, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
            Wrtr = New StreamWriter(sto)
            Dim vs As String = Nothing
            KeysSelectedToShow.Clear()
            Do Until Rdr.Peek = -1
                s = Rdr.ReadLine
                i = InStrRev(s, ","c)
                j = InStrRev(s, ","c, i - 1)
                s = s.Substring(j - 1)
                If s <> vs Then
                    Wrt(s)
                    KeysSelectedToShow.Add(s) ' to print directly
                    vs = s
                End If
            Loop
            Wrtr.Close()
            Rdr.Close()
            sti.Close()
            sto.Close()
            Kill(TDsn.Text & ".SORTED")
            BPrt.Visible = True
        Else
            MsgBox(SysMsg(806))
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Private Function AddSortFld(ByVal PrevTxt As String, ByVal CollCode As String, ByVal CollSeq As Integer, ByVal FldCode As String, ByVal SrtNextTerm As Collection, ByVal FromSeq As Decimal, ByVal ToSeq As Decimal) As Boolean
        Dim kp, kpc As KenmDef, rc As Boolean = False
        Dim pk As PrtFldText, pks As New Collection, SrtDef As Collection
        SrtDef = DirectCast(SrtDefs.Item(CollCode), Collection)
        kp = DirectCast(SrtDef.Item(FldCode), KenmDef)
        Try ' field
            dSelP1.Value = CollCode
            dSelP2.Value = CollSeq
            dSelP3.Value = FldCode
            dSelP4.Value = FromSeq
            dSelP5.Value = ToSeq
            DRdSelect = DbCdSelect.ExecuteReader()
            Do While (DRdSelect.Read())
                pk = New PrtFldText
                pk.Txt = GetDbStringValue(DRdSelect, 0)
                pk.FldSeqnrs = GetDbDecimalValue(DRdSelect, 1)
                If kp.FldHasExit Then
                    If Not RxCompiled Then
                        If Rx.CompileRexxScript(My.Application.Info.DirectoryPath & "\SORTEXIT") <> 0 Then
                            MsgBox(SysMsg(306))
                            Return rc
                        End If
                        RxCompiled = True
                    End If
                    If Rx.ExecuteRexxScript(CollCode & " " & FldCode & " " & pk.Txt) = 0 Then
                        pk.Txt = Rx.GetVar(Rx.SourceNameIndexPosition("T",  Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
                        pks.Add(pk)
                    End If
                Else
                    pks.Add(pk)
                End If
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRdSelect Is Nothing) Then
                DRdSelect.Close()
            End If
        End Try
        For Each pk In pks
            If Not kp.FldHasDep Then
                rc = True
                SrtNextTerm.Add(PrevTxt & pk.Txt) ' einde van reeks afhankelijken
            Else
                Dim added As Boolean = False
                For Each kpc In SrtDef
                    If kpc.FldParent = kp.FldCode Then
                        added = AddSortFld(PrevTxt & pk.Txt, CollCode, CollSeq, kpc.FldCode, SrtNextTerm, pk.FldSeqnrs * 1000, pk.FldSeqnrs * 1000 + 999)
                    End If
                Next
                If Not added Then
                    SrtNextTerm.Add(PrevTxt & pk.Txt) ' add alleen eigen text, zonder children
                End If
                rc = True
            End If
        Next
        Return rc
    End Function
    Private Sub BouwNaamLijst()
        Dim s As String, i, j As Integer, CollCode, CollSeq As String
        Dim Colls As New Collection, kp, kpc As KenmDef, SrtDef As Collection
        DbConn.Open()
        SrtDefs = New Collection
        NaamLst.Nodes.Clear()
        Dim st As Stream = File.Open(TDsn.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Rdr = New StreamReader(st)
        Do Until Rdr.Peek = -1
            s = Rdr.ReadLine()
            i = InStrRev(s, ","c)
            j = InStrRev(s, ","c, i - 1)
            CollCode = s.Substring(j, i - j - 1).TrimEnd
            If Not Colls.Contains(CollCode) Then
                Try ' once per Collection
                    pSelP1.Value = CollCode
                    DRpSelect = DbCpSelect.ExecuteReader()
                    If (DRpSelect.Read()) Then
                        CollCodeNode = New TreeNode(CollCode & ": " & GetDbStringValue(DRpSelect, 0))
                        CollCodeNode.Name = CollCode
                        CollCodeNode.Checked = False
                        NaamLst.Nodes.Add(CollCodeNode)
                        Colls.Add(" ", CollCode)
                    Else
                        MsgBox(SysMsg1p(807, CollCode))
                        TDsn.Text = ""
                        GoTo zz
                    End If
                Catch ex As Exception
                    MsgBox(ex.ToString())
                Finally
                    If Not (DRpSelect Is Nothing) Then
                        DRpSelect.Close()
                    End If
                End Try
                CollSeq = s.Substring(i)
                SrtDef = New Collection
                Try ' alle fields
                    kSelP1.Value = CollCode
                    DRkSelect = DbCkSelect.ExecuteReader()
                    Do While (DRkSelect.Read())
                        FldCodeNode = New TreeNode(GetDbStringValue(DRkSelect, 0) & ": " & GetDbStringValue(DRkSelect, 1))
                        FldCodeNode.Name = GetDbStringValue(DRkSelect, 0)
                        FldCodeNode.Checked = False
                        CollCodeNode.Nodes.Add(FldCodeNode)
                        kp = New KenmDef
                        kp.FldCode = GetDbStringValue(DRkSelect, 0)
                        kp.FldParent = GetDbStringValue(DRkSelect, 2)
                        kp.FldHasExit = GetDbBooleanValue(DRkSelect, 3)
                        SrtDef.Add(kp, kp.FldCode)
                    Loop
                Catch ex As Exception
                    MsgBox(ex.ToString())
                Finally
                    If Not (DRkSelect Is Nothing) Then
                        DRkSelect.Close()
                    End If
                End Try
                For Each kp In SrtDef
                    If kp.FldParent <> "" Then
                        For Each kpc In SrtDef
                            If kpc.FldCode = kp.FldParent Then
                                kpc.FldHasDep = True
                                Exit For
                            End If
                        Next
                    End If
                Next
                SrtDefs.Add(SrtDef, CollCode)
            End If
        Loop
        ' BSrt.Visible = True
zz:
        Rdr.Close()
        st.Close()
        DbConn.Close()
    End Sub
    Private Sub NaamLst_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles NaamLst.NodeMouseClick
        Dim j As Integer
        Dim s As String = e.Node.FullPath
        Dim i As Integer = InStr(1, s, "\")
        If i = 0 Then Exit Sub ' primary node clicked
        Dim mn As String = s.Substring(0, i - 1)
        Dim ln As String = s.Substring(i)
        BSrt.Visible = True
        For Each n As TreeNode In NaamLst.Nodes
            If n.Text = mn Then
                Dim mxNr As Integer = 0
                For Each sn As TreeNode In n.Nodes
                    Dim s1 As String = sn.Text
                    i = InStrRev(s1, "|")
                    If i > 0 Then
                        j = CInt(s1.Substring(i + 1))
                        If j > mxNr Then mxNr = j
                    End If
                Next
                For Each sn As TreeNode In n.Nodes
                    If sn.Text = ln Then
                        Dim s1 As String = sn.Text
                        i = InStrRev(s1, " | ")
                        If i > 0 Then
                            s1 = s1.Substring(0, i - 1)
                            sn.Text = s1
                        Else
                            If sn.Checked Then
                                sn.Text = sn.Text & " | " & CStr(mxNr + 1)
                            End If
                        End If
                        Exit For
                    End If
                Next
                Dim chRt As Boolean = False
                BSrt.Visible = False
                For Each sn As TreeNode In n.Nodes
                    If sn.Checked Then
                        chRt = True
                        BSrt.Visible = True
                        Exit For
                    End If
                Next
                n.Checked = chRt
            End If
        Next
    End Sub
    Private Sub TDsn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TDsn.TextChanged
        If File.Exists(TDsn.Text) Then
            BouwNaamLijst()
        End If
    End Sub
    Private Sub BBrws_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BBrws.Click
        BPrt.Visible = False
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.Title = SysMsg(557)
        OpenFileDialog1.Filter = "KeyRef files (*.bew)|*.bew|All files (*.*)|*.*"
        OpenFileDialog1.ShowDialog()
        Dim Filename As String = OpenFileDialog1.FileName
        If Filename = "" Then
            Exit Sub
        End If
        TDsn.Text = Filename
    End Sub
    Private Sub BPrt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BPrt.Click
        fPrinten.Show()
        fPrinten.LoadFormData()
        fPrinten.SetPrSpecs("")
        fPrinten.ToonScherm()
        fPrinten.BringToFront()
        Me.Close()
    End Sub
End Class
