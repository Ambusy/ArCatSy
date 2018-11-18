'
Public Class ExpData
    Dim ProgChange As Boolean = False
    Dim fso As Object, WrtrUc As Object
    Dim CollSeq As Integer
    Dim CurCollCodeMt As String
    Dim TypeExpInp As String
    Dim FldSep As String
    Dim DbConn As New OdbcConnection(SqlProv)
    ' insert key in te indexeren
    Dim insertXSQL As String = "INSERT INTO tobeindexed (CollCode,CollSeq) VALUES (?, ?)"
    Dim DRXInsert As OdbcDataReader = Nothing
    Dim DbCXInsert As OdbcCommand
    Dim InsXP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim InsXP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    ' haal hoogste CollSeq op
    Dim mSelectSQL As String = "SELECT MAX(CollSeq) FROM data WHERE CollCode = ?"
    Dim DRmSelect As OdbcDataReader = Nothing
    Dim DbCmSelect As OdbcCommand
    Dim mSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    ' insert een kenmerk met data
    Dim insertSQL As String = "INSERT INTO data (CollCode, CollSeq, FldCode, FldSeqnr, FldText) VALUES (?, ?, ?, ?, ?)"
    Dim DRInsert As OdbcDataReader = Nothing
    Dim DbCInsert As OdbcCommand
    Dim InsP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim InsP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    Dim InsP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 255)
    Dim InsP4 As New OdbcParameter("@FldSeqnr", Odbc.OdbcType.Decimal, 16)
    Dim InsP5 As New OdbcParameter("@FldText", Odbc.OdbcType.VarChar, 255)
    ' definities van velden
    Dim kSelectSQL As String = "SELECT FldCode, FldParent FROM collfields WHERE CollCode = ?"
    Dim DRkSelect As OdbcDataReader = Nothing
    Dim DbCkSelect As OdbcCommand
    Dim kSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    ' insert een collectie
    Dim CInsertSQL As String = "INSERT INTO collections(CollCode, CollName, DefPrtSpec, EntitySelFields) VALUES(?, ?, ?, ?)"
    Dim DRCInsert As OdbcDataReader = Nothing
    Dim DbCCInsert As OdbcCommand
    Dim CInsP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
    Dim CInsP2 As New OdbcParameter("@CollName", Odbc.OdbcType.VarChar, 100, "CollName")
    Dim CInsP3 As New OdbcParameter("@DefPrtSpec", Odbc.OdbcType.VarChar, 50, "DefPrtSpec")
    Dim CInsP4 As New OdbcParameter("@EntitySelFields", Odbc.OdbcType.VarChar, 50, "EntitySelFields")
    ' update een collectie
    Dim CUpdateSQL As String = "UPDATE collections SET DefPrtSpec = ?, EntitySelFields = ? WHERE CollCode = ?"
    Dim DRCUpdate As OdbcDataReader = Nothing
    Dim DbCCUpdate As OdbcCommand
    Dim CUpdP1 As New OdbcParameter("@DefPrtSpec", Odbc.OdbcType.VarChar, 50, "DefPrtSpec")
    Dim CUpdP2 As New OdbcParameter("@EntitySelFields", Odbc.OdbcType.VarChar, 50, "EntitySelFields")
    Dim CUpdP3 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
    ' insert velden van een collectie
    Dim FInsertSQL As String = "INSERT INTO collfields(CollCode, FldCode, FldCaption, FldParent, FldInputOrder, FldMaxLength, FldMaxOccurs, FldCheckRegexp, FldCheckErrmsg, FldTermRegexp, FldTermType, FldCombo, FldPExit, FldSExit, FldXExit, FldQExit) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
    Dim DRFInsert As OdbcDataReader = Nothing
    Dim DbCFInsert As OdbcCommand
    Dim FInsP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
    Dim FInsP2 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4, "FldCode")
    Dim FInsP3 As New OdbcParameter("@FldCaption", Odbc.OdbcType.VarChar, 50, "FldCaption")
    Dim FInsP4 As New OdbcParameter("@FldParent", Odbc.OdbcType.VarChar, 4, "FldParent")
    Dim FInsP5 As New OdbcParameter("FldInputOrder", Odbc.OdbcType.Int, 4, "FldInputOrder")
    Dim FInsP6 As New OdbcParameter("@FldMaxLength", Odbc.OdbcType.Int, 4, "FldMaxLength")
    Dim FInsP7 As New OdbcParameter("@FldMaxOccurs", Odbc.OdbcType.Int, 4, "FldMaxOccurs")
    Dim FInsP8 As New OdbcParameter("@FldCheckRegexp", Odbc.OdbcType.VarChar, 255, "FldCheckRegexp")
    Dim FInsP9 As New OdbcParameter("@FldCheckErrmsg", Odbc.OdbcType.VarChar, 80, "FldCheckErrmsg")
    Dim FInsP10 As New OdbcParameter("@FldTermRegexp", Odbc.OdbcType.VarChar, 255, "FldTermRegexp")
    Dim FInsP11 As New OdbcParameter("@FldTermType", Odbc.OdbcType.VarChar, 4, "FldTermType")
    Dim FInsP12 As New OdbcParameter("@FldCombo", Odbc.OdbcType.VarChar, 50, "FldCombo")
    Dim FInsP13 As New OdbcParameter("@FldPExit", Odbc.OdbcType.Bit, 4, "FldPExit")
    Dim FInsP14 As New OdbcParameter("@FldSExit", Odbc.OdbcType.Bit, 4, "FldSExit")
    Dim FInsP15 As New OdbcParameter("@FldXExit", Odbc.OdbcType.Bit, 4, "FldXExit")
    Dim FInsP16 As New OdbcParameter("@FldQExit", Odbc.OdbcType.Bit, 4, "FldQExit")


    Private Sub BBrws_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BBrws.Click
        OpenFileDialog1.Title = SysMsg(868)
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Datafiles (*.txt)|*.txt|All files (*.*)|*.*"
        OpenFileDialog1.CheckFileExists = (TypeExpInp = "Imp")
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.ShowHelp = False
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.FileNames(0) > "" Then
            TDsn.Text = OpenFileDialog1.FileNames(0)
            BImpExp.Visible = True
        End If
    End Sub
    Private Sub BCanc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCanc.Click
        Me.Close()
    End Sub
    Private Sub BImpExp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BImpExp.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        If CheckData.Checked And CheckDef.Checked Then
            MsgBox(SysMsg(861))
        ElseIf CheckData.Checked Then
            If TypeExpInp = "Imp" Then
                If CurCollCodeMt <> "" Then
                    doImp()
                Else
                    MsgBox(SysMsg(862))
                    Me.Close()
                End If
            Else
                doExp()
            End If
        ElseIf CheckDef.Checked Then
            If TypeExpInp = "Imp" Then
                doImpDef()
            Else
                doExpDef()
            End If
        Else
            MsgBox(SysMsg(861))
        End If
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Close()
    End Sub
    Private Sub doImpDef()
        Dim s As String = "", i, i1 As Integer
        If Seps.SelectedIndex = 0 Then
            FldSep = vbTab
        Else
            FldSep = CStr(Seps.Items(Seps.SelectedIndex))
        End If
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        Form1.Idxen.Visible = True

        Dim st As Stream = File.Open(TDsn.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Rdr = New StreamReader(st)
        Dim CollCode, CollName, DefPrtSpec, EntitySelFields As String
        s = Rdr.ReadLine
        i1 = 0
        i = InStr(1, s, FldSep)
        If CurCollCodeMt = "" Then
            CurCollCodeMt = InputBox("naam collectie:")
        End If
        CollCode = CurCollCodeMt
        CollName = s.Substring(i1, i - i1 - 1).TrimEnd
        i1 = i
        i = InStr(i + 1, s, FldSep)
        DefPrtSpec = s.Substring(i1, i - i1 - 1).TrimEnd
        EntitySelFields = s.Substring(i)
        Dim NrErr As Integer = 0
ReTrySql1: Try ' collectie
            CInsP1.Value = CollCode
            CInsP2.Value = CollName
            CInsP3.Value = DefPrtSpec
            CInsP4.Value = EntitySelFields
            DRCInsert = DbCCInsert.ExecuteReader()
        Catch ex As Exception
            If ex.Message.Substring(0, 13) = "ERROR [07006]" Then
                If NrErr = 0 Then
                    NrErr += 1
                    GoTo ReTrySql1
                End If
            End If
            NrErr = 0
ReTrySql2:  Try ' collectie updaten
                CUpdP1.Value = DefPrtSpec
                CUpdP2.Value = EntitySelFields
                CUpdP3.Value = CollCode
                DRCUpdate = DbCCUpdate.ExecuteReader()
            Catch ex1 As Exception
                If ex1.Message.Substring(0, 13) = "ERROR [07006]" Then
                    If NrErr = 0 Then
                        NrErr += 1
                        GoTo ReTrySql2
                    End If
                End If
                MsgBox(ex1.ToString(), , SysMsg(506))
            Finally
                If Not (DRCUpdate Is Nothing) Then
                    DRCUpdate.Close()
                End If
            End Try
        Finally
            If Not (DRCInsert Is Nothing) Then
                DRCInsert.Close()
            End If
        End Try
        Do Until Rdr.Peek = -1
            Dim FldCode, FldCaption, FldParent, FldCheckRegexp, FldCheckErrmsg, FldTermRegexp, FldTermType, FldCombo As String
            Dim FldInputOrder, FldMaxLength, FldMaxOccurs As Integer, FldPExit, FldSExit, FldXExit, FldQExit As Boolean
            s = Rdr.ReadLine
            CollCode = CurCollCodeMt
            i1 = 0
            i = InStr(1, s, FldSep)
            FldCode = s.Substring(i1, i - i1 - 1).TrimEnd
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldCaption = s.Substring(i1, i - i1 - 1).TrimEnd
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldParent = s.Substring(i1, i - i1 - 1).TrimEnd
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldInputOrder = CInt(s.Substring(i1, i - i1 - 1))
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldMaxLength = CInt(s.Substring(i1, i - i1 - 1))
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldMaxOccurs = CInt(s.Substring(i1, i - i1 - 1))
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldCheckRegexp = s.Substring(i1, i - i1 - 1)
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldCheckErrmsg = s.Substring(i1, i - i1 - 1)
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldTermRegexp = s.Substring(i1, i - i1 - 1)
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldTermType = s.Substring(i1, i - i1 - 1).TrimEnd
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldCombo = s.Substring(i1, i - i1 - 1)
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldPExit = CBool(s.Substring(i1, i - i1 - 1))
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldSExit = CBool(s.Substring(i1, i - i1 - 1))
            i1 = i
            i = InStr(i + 1, s, FldSep)
            FldXExit = CBool(s.Substring(i1, i - i1 - 1))
            FldQExit = CBool(s.Substring(i))
            NrErr = 0
ReTrySql3:  Try
                FInsP1.Value = CollCode
                FInsP2.Value = FldCode
                FInsP3.Value = FldCaption
                FInsP4.Value = FldParent
                FInsP5.Value = FldInputOrder
                FInsP6.Value = FldMaxLength
                FInsP7.Value = FldMaxOccurs
                FInsP8.Value = FldCheckRegexp
                FInsP9.Value = FldCheckErrmsg
                FInsP10.Value = FldTermRegexp
                FInsP11.Value = FldTermType
                FInsP12.Value = FldCombo
                FInsP13.Value = FldPExit
                FInsP14.Value = FldSExit
                FInsP15.Value = FldXExit
                FInsP16.Value = FldQExit
                DRFInsert = DbCFInsert.ExecuteReader()
            Catch ex As Exception
                If ex.Message.Substring(0, 13) = "ERROR [07006]" Then
                    If NrErr = 0 Then
                        NrErr += 1
                        GoTo ReTrySql3
                    End If
                End If
                MsgBox(ex.ToString())
            Finally
                If Not (DRFInsert Is Nothing) Then
                    DRFInsert.Close()
                End If
            End Try
        Loop
        Rdr.Close()
        st.Close()
        DbConn.Close()
    End Sub
    Private Sub doExpDef()
        Dim s As String
        Dim SelectSql As String
        Dim DRSelectCursor As OdbcDataReader = Nothing
        Dim DbCSelectCursor As OdbcCommand
        If Seps.SelectedIndex = 0 Then
            FldSep = vbTab
        Else
            FldSep = CStr(Seps.Items(Seps.SelectedIndex))
        End If
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        ' alles ophalen en schrijven
        fso = CreateObject("Scripting.FileSystemObject")
        WrtrUc = fso.CreateTextFile(TDsn.Text, True, True) ' unicode
        SelectSql = "SELECT CollCode, CollName, DefPrtSpec, EntitySelFields FROM collections WHERE CollCode = '" & CurCollCodeMt & "'"
        DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
        Try
            DRSelectCursor = DbCSelectCursor.ExecuteReader()
            DRSelectCursor.Read()
            s = GetDbStringValue(DRSelectCursor, 1) & FldSep & GetDbStringValue(DRSelectCursor, 2) & FldSep & GetDbStringValue(DRSelectCursor, 3)
            WrtrUc.writeline(s) ' collection
        Catch e1 As Exception
            MsgBox(e1.ToString())
        Finally
            If Not (DRSelectCursor Is Nothing) Then
                DRSelectCursor.Close()
            End If
        End Try
        SelectSql = "SELECT CollCode, FldCode, FldCaption, FldParent, FldInputOrder, FldMaxLength, FldMaxOccurs, FldCheckRegexp, FldCheckErrmsg, FldTermRegexp, FldTermType, FldCombo, FldPExit, FldSExit, FldXExit, FldQExit FROM collfields WHERE CollCode = '" & CurCollCodeMt & "'"
        DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
        Try
            DRSelectCursor = DbCSelectCursor.ExecuteReader()
            Do While (DRSelectCursor.Read())
                s = GetDbStringValue(DRSelectCursor, 1) & FldSep & GetDbStringValue(DRSelectCursor, 2) & FldSep & GetDbStringValue(DRSelectCursor, 3) & FldSep _
                & CStr(GetDbIntegerValue(DRSelectCursor, 4)) & FldSep & CStr(GetDbIntegerValue(DRSelectCursor, 5)) & FldSep & CStr(GetDbIntegerValue(DRSelectCursor, 6)) & FldSep & GetDbStringValue(DRSelectCursor, 7) & FldSep _
                & GetDbStringValue(DRSelectCursor, 8) & FldSep & GetDbStringValue(DRSelectCursor, 9) & FldSep & GetDbStringValue(DRSelectCursor, 10) & FldSep & GetDbStringValue(DRSelectCursor, 11) & FldSep _
                & GetDbBooleanValue(DRSelectCursor, 12) & FldSep & GetDbBooleanValue(DRSelectCursor, 13) & FldSep & GetDbBooleanValue(DRSelectCursor, 14) & FldSep & GetDbBooleanValue(DRSelectCursor, 15)
                WrtrUc.WriteLine(s) ' collection
            Loop
        Catch e1 As Exception
            MsgBox(e1.ToString())
        Finally
            If Not (DRSelectCursor Is Nothing) Then
                DRSelectCursor.Close()
            End If
        End Try
        WrtrUc.Close()
        DbConn.Close()
    End Sub
    Private Sub doExp()
        Dim i, ns, nl As Integer, na, s As String, FldSeqnrs(5) As Decimal, vCollSeq As Integer
        Dim FldDef As New Collection, kp As KenmDefin, DataPresent As Boolean
        Dim SelectSql As String, FldCode As String, FldSeqnr As Decimal
        Dim DRSelectCursor As OdbcDataReader = Nothing
        Dim DbCSelectCursor As OdbcCommand
        If Seps.SelectedIndex = 0 Then
            FldSep = vbTab
        Else
            FldSep = CStr(Seps.Items(Seps.SelectedIndex))
        End If
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        'verzamel alle veldcodes
        SelectSql = "SELECT CollSeq, FldCode, FldSeqnr FROM DATA WHERE CollCode = '" & CurCollCodeMt & "' ORDER BY CollSeq, FldCode, FldSeqnr"
        DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
        If ExpSq.Checked Then
            s = "#" & FldSep
        Else
            s = ""
        End If
        DataPresent = False
        Try
            DRSelectCursor = DbCSelectCursor.ExecuteReader()
            Do While (DRSelectCursor.Read())
                CollSeq = GetDbIntegerValue(DRSelectCursor, 0)
                FldCode = GetDbStringValue(DRSelectCursor, 1)
                FldSeqnr = GetDbDecimalValue(DRSelectCursor, 2)
                ns = 0
                While FldSeqnr > 0
                    ns += 1
                    FldSeqnrs(ns) = FldSeqnr Mod 1000D
                    FldSeqnr = Math.Floor(FldSeqnr / 1000D)
                End While
                na = FldCode
                For i = ns To 1 Step -1
                    na = na & "." & CstrD(FldSeqnrs(i))
                Next
                If Not FldDef.Contains(na) Then
                    kp = New KenmDefin
                    kp.FldCode = na
                    kp.FldText = ""
                    FldDef.Add(kp, na)
                    s = s & na & FldSep
                    DataPresent = True
                End If
            Loop
        Catch e1 As Exception
            MsgBox(e1.ToString())
        Finally
            If Not (DRSelectCursor Is Nothing) Then
                DRSelectCursor.Close()
            End If
        End Try
        If DataPresent Then
            ' alles ophalen en schrijven
            SelectSql = "SELECT CollSeq, FldCode, FldSeqnr, FldText FROM DATA WHERE CollCode = '" & CurCollCodeMt & "' ORDER BY CollSeq, FldCode, FldSeqnr"
            DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
            fso = CreateObject("Scripting.FileSystemObject")
            WrtrUc = fso.CreateTextFile(TDsn.Text, True, True) ' unicode
            WrtrUc.WriteLine(s.Substring(0, s.Length() - 1)) ' heading
            nl = 0
            Try
                DRSelectCursor = DbCSelectCursor.ExecuteReader()
                Do While (DRSelectCursor.Read())
                    CollSeq = GetDbIntegerValue(DRSelectCursor, 0)
                    If CollSeq <> vCollSeq Then
                        If nl > 0 Then
                            DoWrt(FldDef, vCollSeq)
                            s = ""
                        End If
                        vCollSeq = CollSeq
                        nl += 1
                    End If
                    FldCode = GetDbStringValue(DRSelectCursor, 1)
                    FldSeqnr = GetDbDecimalValue(DRSelectCursor, 2)
                    ns = 0
                    While FldSeqnr > 0
                        ns += 1
                        FldSeqnrs(ns) = FldSeqnr Mod 1000D
                        FldSeqnr = Math.Floor(FldSeqnr / 1000D)
                    End While
                    na = FldCode
                    For i = ns To 1 Step -1
                        na = na & "." & CstrD(FldSeqnrs(i))
                    Next
                    kp = DirectCast(FldDef.Item(na), KenmDefin)
                    kp.FldText = GetDbStringValue(DRSelectCursor, 3)
                Loop
                DoWrt(FldDef, vCollSeq)
            Catch e1 As Exception
                MsgBox(e1.ToString())
            Finally
                If Not (DRSelectCursor Is Nothing) Then
                    DRSelectCursor.Close()
                End If
            End Try
            WrtrUc.Close()
        End If
        DbConn.Close()
    End Sub
    Private Sub DoWrt(ByVal FldDef As Collection, ByVal CollSeq As Integer)
        Dim s As String
        If ExpSq.Checked Then
            s = CStr(CollSeq) & FldSep
        Else
            s = ""
        End If
        Dim kp As KenmDefin
        For Each kp In FldDef
            s = s & kp.FldText & FldSep
            kp.FldText = ""
        Next
        WrtrUc.WriteLine(s.Substring(0, s.Length() - 1))
    End Sub
    Private Sub doImp()
        Dim s As String = "", s1 As String, nw, i, j, i1, ip As Integer
        Dim CollSeq As Integer, ImpSeq As Boolean
        If Seps.SelectedIndex = 0 Then
            FldSep = vbTab
        Else
            FldSep = CStr(Seps.Items(Seps.SelectedIndex))
        End If
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        Form1.Idxen.Visible = True
        s1 = ""
        Dim st As Stream = File.Open(TDsn.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Rdr = New StreamReader(st)
        Dim nRecs As Integer = 0
        While Rdr.Peek <> -1
            s = Rdr.ReadLine
            nRecs += 1
        End While
        Rdr.Close()
        st.Close()
        Form1.Vltd.Visible = True
        st = File.Open(TDsn.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Rdr = New StreamReader(st)
        Form1.Vltd.Minimum = 0
        Form1.Vltd.Value = 0
        Form1.Vltd.Maximum = nRecs + 1
        Dim wrds(5) As String
        If Rdr.Peek > -1 Then
            If Not cbRaw.Checked Then ' names and indexes line
                s = Rdr.ReadLine
                nw = 1
                i = InStr(1, s, FldSep)
                ImpSeq = False
                If i > 0 Then
                    If s.Substring(0, i - 1) = "#" Then
                        ImpSeq = True
                        s = s.Substring(2)
                        i = InStr(1, s, FldSep)
                    End If
                End If
                While i > 0
                    i = InStr(i + 1, s, FldSep)
                    nw += 1
                End While
                ReDim wrds(nw)
                i = InStr(1, s, FldSep)
                i1 = 0
                For j = 1 To nw - 1
                    wrds(j) = s.Substring(i1, i - i1 - 1)
                    i1 = i
                    i = InStr(i + 1, s, FldSep)
                Next
                wrds(nw) = s.Substring(i1)
                If Not ImpSeq Then
                    Try ' hoogste key
                        mSelP1.Value = CurCollCodeMt
                        DRmSelect = DbCmSelect.ExecuteReader()
                        If DRmSelect.Read() Then
                            Try
                                CollSeq = GetDbIntegerValue(DRmSelect, 0)
                            Catch ex As Exception
                                CollSeq = 1
                            End Try
                        End If
                    Catch ex As Exception
                        MsgBox(ex.ToString(), , SysMsg(317))
                    Finally
                        If Not (DRmSelect Is Nothing) Then
                            DRmSelect.Close()
                        End If
                    End Try
                End If
            End If
            Dim iRecs As Integer = 1
            Do Until Rdr.Peek = -1
                iRecs += 1
                If iRecs Mod 10 = 0 Then
                    Form1.Vltd.Value = iRecs
                    Form1.Vltd.Refresh()
                End If
                s = Rdr.ReadLine
                If s.Length() > 0 Then
                    If Not cbRaw.Checked Then ' formatted data (result of export by ArCarSy)
                        Try
                            If ImpSeq Then
                                i = InStr(1, s, FldSep)
                                CollSeq = CInt(s.Substring(0, i - 1))
                                s = s.Substring(i)
                            Else
                                CollSeq += 1
                            End If
                            Dim NrErr As Integer = 0
ReTrySql4:                  InsXP1.Value = CurCollCodeMt
                            InsXP2.Value = CollSeq
                            Try ' in te indexeren
                                DRXInsert = DbCXInsert.ExecuteReader()
                            Catch e As OdbcException
                                Dim j9 As Integer = 0 ' duplicate
                            Catch ex As Exception
                                If ex.Message.Substring(0, 13) = "ERROR [07006]" Then
                                    If NrErr = 0 Then
                                        NrErr += 1
                                        GoTo ReTrySql4
                                    End If
                                End If
                                If ex.Message.Substring(0, 13) <> "ERROR [23000]" AndAlso MsgBox(ex.ToString(), MsgBoxStyle.OkCancel, SysMsg(318)) = MsgBoxResult.Cancel Then
                                    Exit Do
                                End If
                            Finally
                                If Not (DRXInsert Is Nothing) Then
                                    DRXInsert.Close()
                                End If
                            End Try
                            i = InStr(1, s, FldSep)
                            i1 = 0
                            For j = 1 To nw
                                If i1 >= s.Length() Then
                                    s1 = ""
                                ElseIf j < nw Then
                                    s1 = s.Substring(i1, i - i1 - 1)
                                Else
                                    s1 = s.Substring(i1)
                                End If
                                If s1.Length > 0 Then
                                    Dim FldCode As String
                                    Dim FldSeqnr As Integer
                                    Dim sn As String = wrds(j)
                                    ip = InStr(sn, ".")
                                    FldCode = sn.Substring(0, ip - 1).TrimEnd
                                    sn = sn.Substring(ip)
                                    FldSeqnr = 0
                                    ip = InStr(sn, ".")
                                    While ip > 0
                                        FldSeqnr = FldSeqnr * 1000 + CInt(sn.Substring(0, ip))
                                        sn = sn.Substring(ip)
                                        ip = InStr(sn, ".")
                                    End While
                                    FldSeqnr = FldSeqnr * 1000 + CInt(sn.Substring(ip))
                                    InsP1.Value = CurCollCodeMt
                                    InsP2.Value = CollSeq
                                    InsP3.Value = FldCode
                                    InsP4.Value = FldSeqnr
                                    InsP5.Value = s1
                                    Try
                                        DRInsert = DbCInsert.ExecuteReader()
                                    Catch ex As Exception
                                        If MsgBox(CurCollCodeMt & " " & CollSeq & " " & FldCode & " " & FldSeqnr & " " & s1 & vbCrLf & ex.ToString(), MsgBoxStyle.OkCancel, SysMsg(320)) = MsgBoxResult.Cancel Then
                                            Exit Do
                                        End If
                                    Finally
                                        If Not (DRInsert Is Nothing) Then
                                            DRInsert.Close()
                                        End If
                                    End Try
                                End If
                                If i1 < s.Length() Then
                                    i1 = i
                                    i = InStr(i + 1, s, FldSep)
                                    If i = 0 Then i = s.Length() + 1
                                End If
                            Next
                        Catch ex As Exception
                            MsgBox(CurCollCodeMt & " " & s & vbCrLf & ex.ToString(), , "E R R O R")
                        End Try
                    Else
                        nw = 5 ' collcode collseq, fldcode, fldseq, data (Result of DB utility that dumps a table)
                        i = InStr(1, s, FldSep)
                        i1 = 0
                        For j = 1 To nw - 1
                            wrds(j) = s.Substring(i1, i - i1 - 1)
                            i1 = i
                            i = InStr(i + 1, s, FldSep)
                        Next
                        wrds(nw) = s.Substring(i1)
                        CurCollCodeMt = wrds(1).TrimEnd
                        CollSeq = CInt(wrds(2))
                        Dim NrErr As Integer = 0
ReTrySql4a:             InsXP1.Value = CurCollCodeMt
                        InsXP2.Value = CollSeq
                        Try ' in te indexeren
                            DRXInsert = DbCXInsert.ExecuteReader()
                        Catch e As OdbcException
                            Dim j9 As Integer = 0 ' duplicate
                        Catch ex As Exception
                            If ex.Message.Substring(0, 13) = "ERROR [07006]" Then
                                If NrErr = 0 Then
                                    NrErr += 1
                                    GoTo ReTrySql4a
                                End If
                            End If
                            If ex.Message.Substring(0, 13) <> "ERROR [23000]" AndAlso MsgBox(ex.ToString(), MsgBoxStyle.OkCancel, SysMsg(318)) = MsgBoxResult.Cancel Then
                                Exit Do
                            End If
                        Finally
                            If Not (DRXInsert Is Nothing) Then
                                DRXInsert.Close()
                            End If
                        End Try
                        Dim FldCode As String
                        Dim FldSeqnr As Integer
                        FldCode = wrds(3).TrimEnd
                        FldSeqnr = CInt(wrds(4))
                        NrErr = 0
ReTrySql5:              InsP1.Value = CurCollCodeMt
                        InsP2.Value = CollSeq
                        InsP3.Value = FldCode
                        InsP4.Value = FldSeqnr
                        InsP5.Value = wrds(5).TrimEnd
                        Try
                            DRInsert = DbCInsert.ExecuteReader()
                        Catch ex As Exception
                            If ex.Message.Substring(0, 13) = "ERROR [07006]" Then
                                If NrErr = 0 Then
                                    NrErr += 1
                                    GoTo ReTrySql5
                                End If
                            End If
                            If MsgBox(CurCollCodeMt & " " & CollSeq & " " & FldCode & " " & FldSeqnr & " " & s1 & vbCrLf & ex.ToString(), MsgBoxStyle.OkCancel, SysMsg(320)) = MsgBoxResult.Cancel Then
                                Exit Do
                            End If
                        Finally
                            If Not (DRInsert Is Nothing) Then
                                DRInsert.Close()
                            End If
                        End Try
                    End If
                End If
            Loop
        End If
        Rdr.Close()
        st.Close()
        DbConn.Close()
        Form1.Vltd.Visible = False
    End Sub
    Private Sub ExpData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurCollCodeMt = CurCollCodeMtn
        TypeExpInp = TypeExpInpMenu
        DbCCInsert = New OdbcCommand(CInsertSQL, DbConn)
        DbCCInsert.Parameters.Add(CInsP1)
        DbCCInsert.Parameters.Add(CInsP2)
        DbCCInsert.Parameters.Add(CInsP3)
        DbCCInsert.Parameters.Add(CInsP4)

        DbCCUpdate = New OdbcCommand(CUpdateSQL, DbConn)
        DbCCUpdate.Parameters.Add(CUpdP1)
        DbCCUpdate.Parameters.Add(CUpdP2)
        DbCCUpdate.Parameters.Add(CUpdP3)

        DbCFInsert = New OdbcCommand(FInsertSQL, DbConn)
        DbCFInsert.Parameters.Add(FInsP1)
        DbCFInsert.Parameters.Add(FInsP2)
        DbCFInsert.Parameters.Add(FInsP3)
        DbCFInsert.Parameters.Add(FInsP4)
        DbCFInsert.Parameters.Add(FInsP5)
        DbCFInsert.Parameters.Add(FInsP6)
        DbCFInsert.Parameters.Add(FInsP7)
        DbCFInsert.Parameters.Add(FInsP8)
        DbCFInsert.Parameters.Add(FInsP9)
        DbCFInsert.Parameters.Add(FInsP10)
        DbCFInsert.Parameters.Add(FInsP11)
        DbCFInsert.Parameters.Add(FInsP12)
        DbCFInsert.Parameters.Add(FInsP13)
        DbCFInsert.Parameters.Add(FInsP14)
        DbCFInsert.Parameters.Add(FInsP15)
        DbCFInsert.Parameters.Add(FInsP16)

        DbCInsert = New OdbcCommand(insertSQL, DbConn)
        DbCInsert.Parameters.Add(InsP1)
        DbCInsert.Parameters.Add(InsP2)
        DbCInsert.Parameters.Add(InsP3)
        DbCInsert.Parameters.Add(InsP4)
        DbCInsert.Parameters.Add(InsP5)

        DbCXInsert = New OdbcCommand(insertXSQL, DbConn)
        DbCXInsert.Parameters.Add(InsXP1)
        DbCXInsert.Parameters.Add(InsXP2)

        DbCmSelect = New OdbcCommand(mSelectSQL, DbConn)
        DbCmSelect.Parameters.Add(mSelP1)

        Me.DesktopLocation = New Point(0, 0)
        If TypeExpInp = "Imp" Then
            Me.Text = SysMsg(851)
            BImpExp.Text = SysMsg(853)
            Label1.Text = SysMsg(857)
            Label2.Text = SysMsg1p(855, CurCollCodeMt)
            ExpSq.Visible = False
        Else
            Me.Text = SysMsg(852)
            BImpExp.Text = SysMsg(854)
            Label1.Text = SysMsg(858)
            Label2.Text = SysMsg1p(856, CurCollCodeMt)
            ExpSq.Text = SysMsg(869)
        End If
        Seps.SelectedIndex = 0
        BBrws.Text = SysMsg(804)
        BCanc.Text = SysMsg(2)
        CheckDef.Text = SysMsg(859)
        CheckData.Text = SysMsg(860)
    End Sub
    Private Sub TDsn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TDsn.TextChanged
        BImpExp.Visible = True
    End Sub
    Private Sub CheckDef_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckDef.CheckedChanged
        If Not ProgChange Then
            ProgChange = True
            CheckData.Checked = False
            ProgChange = False
        End If
    End Sub
    Private Sub CheckData_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckData.CheckedChanged
        If Not ProgChange Then
            ProgChange = True
            CheckDef.Checked = False
            ProgChange = False
        End If
    End Sub
End Class
