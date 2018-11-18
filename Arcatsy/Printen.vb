'
Imports RxLib.Rexx
'#Const CreLogFile = True
Public Class Printen
    Dim DbConn As New OdbcConnection(SqlProv)
    'Dim ControlNextPosY As Integer = 10, SizeLabelHeight As Integer = 10
    Dim wF As Object
    Dim FormisClosed As Boolean = False

    ' haal print specs op van een CollCode op
    Dim cSelectSQL As String = "SELECT PrtSpec FROM prtspecs WHERE CollCode = ? ORDER BY PrtSpec"
    Dim DRcSelect As OdbcDataReader = Nothing
    Dim DbCcSelect As OdbcCommand
    Dim cSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim tSelectSQL As String = "SELECT PrtOrder, FldCode, BeforeFirstField, BetweenFields, AfterLastField, IfFieldMissing, IfFieldDifferent FROM prtspeclines WHERE PrtSpec = ? ORDER BY PrtOrder"
    Dim DRtSelect As OdbcDataReader = Nothing
    Dim DbCtSelect As OdbcCommand
    Dim tSelP1 As New OdbcParameter("@PrtSpec", Odbc.OdbcType.VarChar, 50)

    Dim dSelectSQL As String = "SELECT FldSeqnr, FldText FROM data WHERE CollCode = ? AND CollSeq = ? AND FldCode = ? AND FldSeqnr >= ? AND FldSeqnr <= ? ORDER BY FldSeqnr"
    Dim DRdSelect As OdbcDataReader = Nothing
    Dim DbCdSelect As OdbcCommand
    Dim dSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim dSelP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    Dim dSelP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)
    Dim dSelP4 As New OdbcParameter("@FldSeqnrs", Odbc.OdbcType.Decimal, 16)
    Dim dSelP5 As New OdbcParameter("@FldSeqnre", Odbc.OdbcType.Decimal, 16)

    Dim rSelectSQL As String = "SELECT FldSeqnr, FldText FROM data WHERE CollCode = ? AND CollSeq = ? AND FldCode = ?"
    Dim DRrSelect As OdbcDataReader = Nothing
    Dim DbCrSelect As OdbcCommand
    Dim rSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim rSelP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    Dim rSelP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)

    Dim kSelectSQL As String = "SELECT FldCode, FldParent, FldPExit FROM collfields WHERE CollCode = ?"
    Dim DRkSelect As OdbcDataReader = Nothing
    Dim DbCkSelect As OdbcCommand
    Dim kSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim pSelectSQL As String = "SELECT DefPrtSpec FROM collections WHERE CollCode = ?"
    Dim DRpSelect As OdbcDataReader = Nothing
    Dim DbCpSelect As OdbcCommand
    Dim pSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim CollCodeNode, PrtNode As TreeNode
    Dim wrkFileName As String
    Dim PrIt As Collection, PrDef As Collection, PrtSpecLst As New Collection, PrtSpecLstColl As New Collection

    Dim WithEvents Rx As New  Rexx(New  RexxCompData)
    Dim RxCompiled As Boolean = False
    Dim execName As String = String.Empty, srcName As String = String.Empty
    Dim cvr As  DefVariable = Nothing
    Dim skippen As Boolean = False
    Dim StopPrint As Boolean ' change layout while printing 
    Private Sub Printen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = SysMsg(601)
        Me.DesktopLocation = New Point(500, 0)
        Me.Cursor = Cursors.WaitCursor
        BLaadlijst.Text = SysMsg(602)
        BTonen.Text = SysMsg(603)
        BPrinten.Text = SysMsg(604)
        BChaLayout.Text = SysMsg(605)
        Label1.Text = SysMsg(610)
        BHtml.Text = SysMsg(611)
        BBack.Visible = False

        DbConn.Open()

        DbCtSelect = New OdbcCommand(tSelectSQL, DbConn)
        DbCtSelect.Parameters.Add(tSelP1)

        DbCdSelect = New OdbcCommand(dSelectSQL, DbConn)
        DbCdSelect.Parameters.Add(dSelP1)
        DbCdSelect.Parameters.Add(dSelP2)
        DbCdSelect.Parameters.Add(dSelP3)
        DbCdSelect.Parameters.Add(dSelP4)
        DbCdSelect.Parameters.Add(dSelP5)

        DbCrSelect = New OdbcCommand(rSelectSQL, DbConn)
        DbCrSelect.Parameters.Add(rSelP1)
        DbCrSelect.Parameters.Add(rSelP2)
        DbCrSelect.Parameters.Add(rSelP3)

        DbCkSelect = New OdbcCommand(kSelectSQL, DbConn)
        DbCkSelect.Parameters.Add(kSelP1)

        DbCcSelect = New OdbcCommand(cSelectSQL, DbConn)
        DbCcSelect.Parameters.Add(cSelP1)

        DbCpSelect = New OdbcCommand(pSelectSQL, DbConn)
        DbCpSelect.Parameters.Add(pSelP1)

        DbConn.Close()
    End Sub
    Private Sub Printen_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not (DbConn Is Nothing) Then
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End If
        Try
            Kill(wrkFileName)
        Catch ex As Exception
        End Try
        wrkFileName = String.Empty
        Me.Hide()
        e.Cancel = True
        FormisClosed = True
    End Sub
    Private Sub GetTreeList()
        Dim i, j As Integer, CollCode As String, CollCodes As New Collection
        Dim op As Boolean = False
        If DbConn.State <> ConnectionState.Open Then
            op = True
            DbConn.Open()
        End If
        Label1.Visible = True
        PrintSpec.Visible = True
        BTonen.Visible = True
        PrintSpec.Nodes.Clear()
        CollCodes.Clear()
        For Each s As String In KeysSelectedToShow
            i = InStrRev(s, ","c)
            j = InStrRev(s, ","c, i - 1)
            CollCode = s.Substring(j, i - j - 1).TrimEnd
            If CollCodes.Contains(CollCode) Then
                Dim nr As CollNm
                nr = DirectCast(CollCodes.Item(CollCode), CollNm)
                nr.n += 1
            Else
                Dim nr As New CollNm
                nr.n = 1
                CollCodes.Add(nr, CollCode)
                CollCodeNode = New TreeNode(CollCode)
                CollCodeNode.Name = CollCode
                CollCodeNode.Checked = True
                PrintSpec.Nodes.Add(CollCodeNode)
                Dim defPrt As String = ""
                Try ' default prt spec
                    pSelP1.Value = CollCodeNode.Name
                    DRpSelect = DbCpSelect.ExecuteReader()
                    Do While (DRpSelect.Read())
                        defPrt = GetDbStringValue(DRpSelect, 0)
                    Loop
                Catch ex As Exception
                    MsgBox(ex.ToString())
                Finally
                    If Not (DRpSelect Is Nothing) Then
                        DRpSelect.Close()
                    End If
                End Try
                Try ' alle prtspecs van de Collection
                    cSelP1.Value = CollCode
                    DRcSelect = DbCcSelect.ExecuteReader()
                    Do While (DRcSelect.Read())
                        Dim prt As String = GetDbStringValue(DRcSelect, 0)
                        PrtNode = New TreeNode(prt)
                        PrtNode.Name = prt
                        PrtNode.Checked = (prt = defPrt)
                        CollCodeNode.Nodes.Add(PrtNode)
                    Loop
                    CollCodeNode.Expand()
                Catch ex As Exception
                    MsgBox(ex.ToString())
                Finally
                    If Not (DRcSelect Is Nothing) Then
                        DRcSelect.Close()
                    End If
                End Try
            End If
        Next
        If op Then DbConn.Close()
        For Each s As TreeNode In PrintSpec.Nodes
            Dim nr As CollNm
            nr = DirectCast(CollCodes.Item(s.Name), CollNm)
            s.Text = s.Text.Trim() & " (" & CStr(nr.n) & ")"
        Next
    End Sub
    Friend Sub SetPrSpecs(ByVal s As String)
        Dim i, j As Integer
        If s.Length() > 0 Then
            Dim w() As String = s.Split(" "c)
            For Each Me.CollCodeNode In PrintSpec.Nodes
                j = 0
                For i = 1 To w.Length Step 2
                    If w(i - 1) = CollCodeNode.Name.TrimEnd Then
                        j = i
                        Exit For
                    End If
                Next
                CollCodeNode.Checked = (j > 0)
                If CollCodeNode.Checked Then
                    Dim LeastOne As Boolean = False
                    For Each Me.PrtNode In CollCodeNode.Nodes
                        PrtNode.Checked = (w(j) = PrtNode.Name.TrimEnd)
                        If PrtNode.Checked Then LeastOne = True
                    Next
                    If Not LeastOne Then
                        MsgBox("Internal error: Prtspec " + w(j) + " for collection " + w(j - 1) + " is not defined")
                        If CollCodeNode.Nodes.Count > 0 Then
                            Dim t As TreeNode = DirectCast(CollCodeNode.Nodes.Item(0), TreeNode)
                            t.Checked = True
                        End If
                    End If
                End If
            Next
            ToonScherm()
        End If
    End Sub
    Friend Sub LoadFormData()
        Dim op As Boolean = False
        If DbConn.State <> ConnectionState.Open Then
            DbConn.Open()
            op = True
        End If
        If KeysSelectedToShow.Count > 0 Then
            GetTreeList()
            Scr.Visible = False
            ' BChaLayout.Visible = False
            StopPrint = False
            BPrinten.Visible = False
            BLaadlijst.Visible = False
            BTonen.Visible = False
            BHtml.Visible = False
            PrintSpec.Visible = False ' nuuuuuuu
            Label1.Visible = False ' nuuuuu
        Else
            BLaadlijst.Visible = True
            Label1.Visible = False
            PrintSpec.Visible = False
            BTonen.Visible = False
            BPrinten.Visible = False
        End If
        If op Then DbConn.Close()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub BTonen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTonen.Click
        BTonen.Visible = False
        ToonScherm()
    End Sub
    Public Sub ToonScherm()
        Dim op As Boolean
#If CreLogFile Then
        Loggen("start Toonscherm")
#End If
        If DbConn.State <> ConnectionState.Open Then
            op = True
            DbConn.Open()
        Else
            op = False
        End If
        ' file in which to write the HTML
        Dim fso As Object
        fso = CreateObject("Scripting.FileSystemObject")
        If wrkFileName = String.Empty Then
            wrkFileName = OpenWrkFile() & ".html"
            wF = fso.CreateTextFile(wrkFileName, True, True) ' unicode

            ' ook: object.OpenTextFile(filename[, iomode[, create[, format]]])
            '            filename         'Required. String expression that identifies the file to open.
            '            iomode           'Optional. Can be one of three constants: ForReading, ForWriting, or ForAppending.
            '            create           'Optional. Boolean value that indicates whether a new file can be created if the specified filename doesn't exist. The value is True if a new file is created, False if it isn't created. If omitted, a new file isn't created.
            '            Format           'Optional. One of three Tristate values used to indicate the format of the opened file. If omitted, the file is opened as ASCII. 

            '            ForReading         1:            ' Open a file for reading only. You can't write to this file.
            '            ForWriting         2:            ' Open a file for writing.
            '            ForAppending       8:            ' Open a file and write to the end of the file.
            '            Tristate UseDefault-2            ' Opens the file using the system default.
            '            Tristate True      -1            ' Opens the file as Unicode.
            '            Tristate False      0            ' Opens the file as ASCII.


            ' wF = File.Open(wrkFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)
        Else
            wF = fso.CreateTextFile(wrkFileName, True, True)
            ' wF = File.Open(wrkFileName, FileMode.Truncate, FileAccess.ReadWrite)
        End If
#If CreLogFile Then
        Loggen("wrk: " & wrkFileName)
#End If


        StopPrint = False
        ToonSchermInt()

        wF.Close()
        If op Then DbConn.Close()
#If CreLogFile Then
        Loggen("end Toonscherm")
#End If
        BPrinten.Visible = True
        BLaadlijst.Visible = True
        Me.Cursor = Cursors.Default

    End Sub
    Private Sub ToonSchermInt()
        Dim ns As Integer = 0, i, j As Integer
        Dim S0, S1 As Boolean
        Dim T0, T1 As String
        Dim p, ps As PrtData, kp, kpc As KenmDef, fiSf As Boolean = True
        Dim s As String, CurPrtSpec As String = ""
        FormisClosed = False
        ' printspec selected for each collection in the selectionlist?
        For Each Me.CollCodeNode In PrintSpec.Nodes
            If CollCodeNode.Checked Then
#If CreLogFile Then
                Loggen("sel: " & CollCodeNode.Name)
#End If
                ns += 1
                Dim n As Integer = 0
                For Each Me.PrtNode In CollCodeNode.Nodes
                    If PrtNode.Checked Then
                        n += 1
                    End If
                Next
                If n <> 1 Then
                    MsgBox(SysMsg1p(607, CollCodeNode.Name))
                    Exit Sub
                End If
            End If
        Next
        If ns = 0 Then Exit Sub
        Label1.Visible = False
        PrintSpec.Visible = False
        BChaLayout.Visible = True
        'BPrinten.Visible = True
        PrtSpecLst.Clear()
        PrtSpecLstColl.Clear()
        S0 = False
        T0 = ""
        S1 = False
        T1 = ""
        For Each Me.CollCodeNode In PrintSpec.Nodes
            If CollCodeNode.Checked Then
                PrDef = New Collection
                Try ' alle definities van searchterms met relatie
                    kSelP1.Value = CollCodeNode.Name.TrimEnd
                    DRkSelect = DbCkSelect.ExecuteReader()
                    Do While (DRkSelect.Read())
                        kp = New KenmDef
                        kp.FldCode = GetDbStringValue(DRkSelect, 0).TrimEnd
                        kp.FldParent = GetDbStringValue(DRkSelect, 1).TrimEnd
                        kp.FldHasExit = GetDbBooleanValue(DRkSelect, 2)
                        kp.FldHasDep = False
                        PrDef.Add(kp, kp.FldCode)
#If CreLogFile Then
                        Loggen("prdef: " & kp.FldCode & " " & kp.FldParent)
#End If
                    Loop
                Catch ex As Exception
                    MsgBox(ex.ToString())
                Finally
                    If Not (DRkSelect Is Nothing) Then
                        DRkSelect.Close()
                    End If
                End Try
                For Each kp In PrDef
                    If kp.FldParent <> "" Then
                        For Each kpc In PrDef
                            If kpc.FldCode = kp.FldParent Then
                                kpc.FldHasDep = True
                                Exit For
                            End If
                        Next
                    End If
                Next
                For Each Me.PrtNode In CollCodeNode.Nodes
                    If PrtNode.Checked Then
                        CurPrtSpec = PrtNode.Name.TrimEnd
                        Exit For
                    End If
                Next
#If CreLogFile Then
                Loggen("coll: " & CollCodeNode.Name)
                Loggen("contains coll?: " & CStr(PrtSpecLstColl.Contains(CollCodeNode.Name)))
#End If
                If Not PrtSpecLstColl.Contains(CollCodeNode.Name) Then
                    PrtSpecLstColl.Add(CurPrtSpec, CollCodeNode.Name)
#If CreLogFile Then
                    Loggen("add coll: " & CollCodeNode.Name)
#End If
                End If
#If CreLogFile Then
                Loggen("spec: " & CurPrtSpec)
                Loggen("contains spec?: " & CStr(PrtSpecLst.Contains(CurPrtSpec)))
#End If
                If Not PrtSpecLst.Contains(CurPrtSpec) Then
                    PrIt = New Collection
                    tSelP1.Value = CurPrtSpec
                    Try ' alle te printen searchterms
                        DRtSelect = DbCtSelect.ExecuteReader()
                        Do While (DRtSelect.Read())
                            p = New PrtData
                            p.Seq = GetDbIntegerValue(DRtSelect, 0)
                            p.FldCode = GetDbStringValue(DRtSelect, 1)
                            p.BeforeFirstField = GetDbStringValue(DRtSelect, 2)
                            p.BetweenFields = GetDbStringValue(DRtSelect, 3)
                            p.AfterLastField = GetDbStringValue(DRtSelect, 4)
                            p.IfFieldMissing = GetDbStringValue(DRtSelect, 5)
                            p.IfFieldDifferent = CBool(GetDbBooleanValue(DRtSelect, 6))
                            p.PrevCon = ""
                            p.Chld = False
                            p.HasChld = False
                            PrIt.Add(p, CStr(p.Seq))
                            Loggen("prt line: " & p.FldCode)
                            If p.Seq = 0 Then
                                T0 = TranUs(p.BeforeFirstField & p.BetweenFields & p.AfterLastField, New PrtFldText)
                                S0 = True
                            End If
                            If p.Seq = 1 Then
                                T1 = TranUs(p.BeforeFirstField & p.BetweenFields & p.AfterLastField, New PrtFldText)
                                S1 = True
                            End If
                            i = InStr(p.FldCode, "->")
                            If i > 0 Then
                                p.RefEnt = p.FldCode.Substring(i + 1)
                                p.FldCode = p.FldCode.Substring(0, i - 1).TrimEnd
                                i = InStr(p.RefEnt, ":")
                                p.RefFld = p.RefEnt.Substring(i)
                                p.RefEnt = p.RefEnt.Substring(0, i - 1).TrimEnd
                            End If
                        Loop
                    Catch ex As Exception
                        MsgBox(ex.ToString())
                    Finally
                        If Not (DRtSelect Is Nothing) Then
                            DRtSelect.Close()
                        End If
                    End Try
                    For Each p In PrIt ' relaties tussen te printen searchterms
                        If p.FldCode = "0" Or p.FldCode = "1" Or p.FldCode = "2" Or p.FldCode = "3" Then
                            p.HasExit = False
                        Else
                            If PrDef.Contains(p.FldCode) Then
                                kp = DirectCast(PrDef.Item(p.FldCode), KenmDef)
                                p.HasExit = kp.FldHasExit
                                If kp.FldParent <> "" Then
                                    For Each ps In PrIt
                                        If ps.FldCode = kp.FldParent Then
                                            p.Chld = True
                                            p.FldParent = kp.FldParent
                                            ps.HasChld = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            Else
                                MsgBox(SysMsg1p(608, p.FldCode))
                            End If
                        End If
                    Next
                    PrtSpecLst.Add(PrIt, CurPrtSpec)
                    Loggen("add spec: " & CurPrtSpec)
                End If
            End If
        Next
        If KeysSelectedToShow.Count > 25 Then
            PBar.Maximum = KeysSelectedToShow.Count
            PBar.Visible = True
            PBar.Value = 0
        End If
        Loggen("gen html")
        Dim CollCode As String = ""
        s = SysMsg(204) : WrText(wF, s & vbCrLf, Nothing, False, Nothing)
        s = SysMsg(205) : WrText(wF, s & vbCrLf, Nothing, False, Nothing)
        s = SysMsg(206) : WrText(wF, s & vbCrLf, Nothing, False, Nothing)
        s = SysMsg(207) : WrText(wF, s & vbCrLf, Nothing, False, Nothing)
        s = "<BODY>" & vbCrLf
        WrText(wF, s, Nothing, False, Nothing)
        If S0 Then
            WrText(wF, T0, Nothing, False, Nothing)
        End If
        CollCodeNode = PrintSpec.Nodes.Item(0)
        For Each selKey As String In KeysSelectedToShow ' van een Collection!
            Application.DoEvents()
            If FormisClosed Then Exit For
            If PBar.Visible Then PBar.Value += 1
            i = InStrRev(selKey, ","c)
            j = InStrRev(selKey, ","c, i - 1)
            CollCode = selKey.Substring(j, i - j - 1).TrimEnd
            If CollCodeNode.Name <> CollCode Then
                Do
                    For Each Me.CollCodeNode In PrintSpec.Nodes
                        If CollCodeNode.Name = CollCode Then
                            Exit Do
                        End If
                    Next
                    Exit Do
                Loop
            End If
            If CollCodeNode.Checked Then ' collection has to be printed?
                CurPrtSpec = CStr(PrtSpecLstColl.Item(CollCode))
                PrIt = DirectCast(PrtSpecLst.Item(CurPrtSpec), Collection)
                WrText(wF, vbCrLf, Nothing, False, Nothing)
                For Each p In PrIt
                    If p.Seq > 1 And Not p.Chld Then ' 0 en 1 zijn globale teksten, niet aan een kenmerk gebonden, children komen onder hun parent
                        ReadFldText(wF, selKey, p, 0, 99999999999999)
                        Application.DoEvents()
                        If StopPrint Then Exit Sub
                    End If
                Next
            End If
        Next
        If Not FormisClosed Then
            If S1 Then
                WrText(wF, T1, Nothing, False, Nothing)
            End If
            WrText(wF, "</BODY> </HTML> ", Nothing, False, Nothing)
            Scr.Url = New Uri(wrkFileName)
            If PBar.Visible Then PBar.Visible = False
            Scr.Visible = True
            BHtml.Visible = True
        End If
    End Sub
    Private Sub ReadRefText(ByVal sel As String, ByVal p As PrtData, ByVal pk As PrtFldText, ByRef pks As Collection)
        Try
            rSelP1.Value = p.RefEnt
            rSelP2.Value = CInt(sel)
            rSelP3.Value = p.RefFld
            DRrSelect = DbCrSelect.ExecuteReader()
            Do While (DRrSelect.Read())
                pk = New PrtFldText
                pk.FldSeqnrs = GetDbDecimalValue(DRrSelect, 0)
                pk.Txt = GetDbStringValue(DRrSelect, 1)
                pks.Add(pk)
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRrSelect Is Nothing) Then
                DRrSelect.Close()
            End If
        End Try
    End Sub
    Private Sub ReadFldText(ByVal wF As Object, ByVal sel As String, ByVal p As PrtData, ByVal strt As Decimal, ByVal endn As Decimal)
        Dim s As String = "", CollCode As String = "", CollSeq, i, j As Integer, txt As String = "", ixt As String = ""
        Dim pk As PrtFldText, pks As New Collection, FndOne As Boolean
        Dim na As String = "", FldSeqnr, ifs As Decimal
        i = InStrRev(sel, ","c)
        j = InStrRev(sel, ","c, i - 1)
        CollCode = sel.Substring(j, i - j - 1).TrimEnd
        CollSeq = CInt(sel.Substring(i))
        ixt = sel.Substring(0, j - 1)
        If p.FldCode = "0" Or p.FldCode = "1" Or p.FldCode = "2" Or p.FldCode = "3" Then
            FndOne = True
            pk = New PrtFldText
            pk.FldSeqnrs = 0
            If p.FldCode = "0" Then
                pk.Txt = CollCode
            ElseIf p.FldCode = "1" Then
                pk.Txt = CStr(CollSeq)
            ElseIf p.FldCode = "2" Then
                pk.Txt = ixt
            Else
                pk.Txt = ""
            End If
            pks.Add(pk)
        Else
            FndOne = False
            Dim NrErr As Integer = 0
            Try
ReTrySql1:
                dSelP1.Value = CollCode
                dSelP2.Value = CollSeq
                dSelP3.Value = p.FldCode
                dSelP4.Value = strt
                dSelP5.Value = endn
                DRdSelect = DbCdSelect.ExecuteReader()
                Do While (DRdSelect.Read())
                    pk = New PrtFldText
                    pk.FldSeqnrs = GetDbDecimalValue(DRdSelect, 0)
                    pk.Txt = GetDbStringValue(DRdSelect, 1)
                    If p.HasExit Then
                        If Not RxCompiled Then
                            If Rx.CompileRexxScript(My.Application.Info.DirectoryPath & "\PRINTEXIT") <> 0 Then
                                MsgBox(SysMsg(306))
                                Exit Sub
                            End If
                            RxCompiled = True
                        End If
                        na = ""
                        FldSeqnr = pk.FldSeqnrs
                        While FldSeqnr > 0
                            ifs = FldSeqnr Mod 1000D
                            FldSeqnr = Math.Floor(FldSeqnr / 1000D)
                            na = "." & CstrD(ifs) & na
                        End While
                        If Rx.ExecuteRexxScript(CollCode & " " & p.FldCode & " " & na.Substring(1) & " " & pk.Txt) = 0 Then
                            pk.Txt = Rx.GetVar(Rx.SourceNameIndexPosition("T", Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
                            pks.Add(pk)
                        End If
                    Else
                        If p.RefEnt Is Nothing Then
                            pks.Add(pk)
                        Else
                            ReadRefText(pk.Txt, p, pk, pks)
                        End If
                    End If
                    FndOne = True
                Loop
            Catch e As OdbcException
                If e.Message.Substring(0, 13) = "ERROR [07006]" Then ' ODBC error on mdb databases
                    If NrErr = 0 Then
                        NrErr += 1
                        GoTo ReTrySql1
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.ToString())
            Finally
                If Not (DRdSelect Is Nothing) Then
                    DRdSelect.Close()
                End If
            End Try
        End If
        i = 0
        For Each pk In pks
            i += 1
            If Not skippen AndAlso i > 1 AndAlso p.BetweenFields.Length() > 0 Then
                WrText(wF, TranUs(p.BetweenFields, pk), Nothing, False, pk)
            End If
            WrText(wF, pk.Txt, p, i = 1, pk)
            For Each ps As PrtData In PrIt
                If ps.FldParent = p.FldCode Then
                    ReadFldText(wF, sel, ps, pk.FldSeqnrs * 1000, pk.FldSeqnrs * 1000 + 999)
                End If
            Next
            If Not skippen AndAlso i = pks.Count AndAlso p.AfterLastField.Length() > 0 Then
                WrText(wF, TranUs(p.AfterLastField, pk), Nothing, False, pk)
            End If
        Next
        If Not FndOne Then
            If p.IfFieldMissing.Length() > 0 Then
                WrText(wF, TranUs(p.IfFieldMissing, New PrtFldText), Nothing, False, Nothing)
            End If
        End If
    End Sub
    Private Sub WrText(ByVal wF As Object, ByVal txt As String, ByVal p As PrtData, ByVal FldSeqFirst As Boolean, ByVal pk As PrtFldText)
        skippen = False
        If Not (p Is Nothing) Then
            If p.IfFieldDifferent Then ' check op dubbele teksten
                If txt = p.PrevCon Then
                    skippen = True
                    txt = ""
                Else
                    p.PrevCon = txt
                    ' level was dus gewijzigd: ook onderliggende
                    Dim fnd As Boolean = False
                    For Each pn As PrtData In PrIt
                        If pn.FldCode = p.FldCode AndAlso pn.BeforeFirstField = p.BeforeFirstField AndAlso pn.BetweenFields = p.BetweenFields AndAlso pn.AfterLastField = p.AfterLastField Then
                            fnd = True
                        ElseIf fnd Then
                            If pn.IfFieldDifferent Then
                                pn.PrevCon = "" ' onderliggende zijn nu ook "gewijzigd"
                            End If
                        End If
                    Next
                End If
            End If
        End If
        If Not skippen AndAlso Not (p Is Nothing) Then
            If FldSeqFirst Then txt = TranUs(p.BeforeFirstField, pk) & txt
        End If
        If txt.Length() > 0 Then
            wF.WriteLine(txt)
            'Dim SrcLength As Integer = txt.Length()
            'Dim buf(SrcLength + 1) As Byte
            'buf = System.Text.Encoding.Default.GetBytes(txt)
            'wF.Write(buf, 0, SrcLength)
        End If
    End Sub
    Private Function TranUs(ByVal txt As String, ByVal pk As PrtFldText) As String
        Dim i As Integer
        i = InStr(txt, "_"c)
        While i > 0
            If i > 2 AndAlso txt(i - 2) = "\"c Then
                txt = txt.Substring(0, i - 2) & "_" & txt.Substring(i)
            Else
                txt = txt.Substring(0, i - 1) & "&nbsp; " & txt.Substring(i)
            End If
            i = InStr(i, txt, "_"c)
        End While
        i = InStr(txt, "\#")
        While i > 0
            txt = txt.Substring(0, i - 1) & CstrD(pk.FldSeqnrs Mod 1000) & txt.Substring(i + 1)
            i = InStr(txt, "\#")
        End While
        Return txt
    End Function
    Private Function OpenWrkFile() As String
        'Dim fso, MyFile
        'fso = CreateObject("Scripting.FileSystemObject")
        'MyFile = fso.CreateTextFile("c:\testfile.txt", True)
        'MyFile.WriteLine("This is a test.")
        'MyFile.Close()
        Dim i As Integer, WrkfileName As String
        For i = 1 To 99
            WrkfileName = Path.GetTempFileName()
            If File.Exists(WrkfileName) Then
                Try
                    Kill(WrkfileName)
                Catch E As Exception ' might be allocated
                    i = i
                End Try
            End If
            If Not File.Exists(WrkfileName) Then
                Return WrkfileName
            End If
        Next
        MsgBox(SysMsg(609))
        Return Nothing
    End Function
    Private Sub BPrinten_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BPrinten.Click
        Scr.ShowPageSetupDialog()
        Scr.Print()
    End Sub
    Private Sub BLaadlijst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BLaadlijst.Click
        LoadKeyList()
        GetTreeList()
    End Sub
    Friend Sub LoadKeyList()
        Dim files() As String
        Scr.Visible = False
        OpenFileDialog1.Title = SysMsg(606)
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Key files (*.bew)|*.bew|All files (*.*)|*.*"
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.ShowHelp = False
        OpenFileDialog1.ShowDialog()
        files = OpenFileDialog1.FileNames
        If files(0) > "" Then
            Dim st As Stream = File.Open(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Rdr = New StreamReader(st)
            KeysSelectedToShow.Clear()
            Do Until Rdr.Peek = -1
                KeysSelectedToShow.Add(Rdr.ReadLine)
            Loop
            Rdr.Close()
            st.Close()
            LoadFormData()
        End If
    End Sub
    Private Sub PrintSpec_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles PrintSpec.NodeMouseClick
        Dim s As String = e.Node.FullPath
        Dim i As Integer = InStr(1, s, "\")
        If i = 0 Then Exit Sub ' click op Collection
        Dim mn As String = s.Substring(0, i - 1)
        Dim ln As String = s.Substring(i)
        For Each n As TreeNode In PrintSpec.Nodes
            If n.Text = mn Then
                Dim nuch As Boolean = False
                For Each sn As TreeNode In n.Nodes
                    If sn.Text = ln AndAlso sn.Checked Then
                        nuch = True
                    End If
                Next
                If nuch Then
                    For Each sn As TreeNode In n.Nodes
                        If sn.Text <> ln AndAlso sn.Checked Then
                            sn.Checked = False
                        End If
                    Next
                End If
            End If
        Next
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BChaLayout.Click
        Label1.Visible = True
        PrintSpec.Visible = True
        BPrinten.Visible = False
        Scr.Visible = False
        BTonen.Visible = True
        StopPrint = True
    End Sub
    Private Sub BHtml_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BHtml.Click
        Dim myProcess As Process
        myProcess = Process.Start("NOTEPAD", """" & wrkFileName & """")
        myProcess.WaitForExit()
    End Sub
    Private Sub Scr_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles Scr.Navigated
        Dim i As Integer = 1
        If e.Url.OriginalString <> wrkFileName Then
            BTonen.Visible = False
            BBack.Visible = True
        End If
    End Sub
    Private Sub BBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BBack.Click
        Scr.GoBack()
        BTonen.Visible = True
        BBack.Visible = False
    End Sub

    Sub RexxCmd(ByVal env As String, ByVal s As String, ByVal e As  RexxEvent, ByRef RexxEnv As  Rexx) Handles Rx.doCmd
        Dim rc As Integer = 0
        Dim w() As String = s.Split(" "c)
        If w(0) = "GET" Then
            Dim gtSelectSQL As String = "SELECT FldText FROM data WHERE CollCode = ? AND CollSeq = ? AND FldCode = ? AND FldSeqnr = ?"
            Dim DRgtSelect As OdbcDataReader = Nothing
            Dim DbCgtSelect As OdbcCommand
            Dim gtSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
            Dim gtSelP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
            Dim gtSelP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)
            Dim gtSelP4 As New OdbcParameter("@FldSeqnrs", Odbc.OdbcType.Decimal, 16)
            DbCgtSelect = New OdbcCommand(gtSelectSQL, DbConn)
            DbCgtSelect.Parameters.Add(gtSelP1)
            gtSelP1.Value = w(1)
            DbCgtSelect.Parameters.Add(gtSelP2)
            gtSelP2.Value = w(2)
            DbCgtSelect.Parameters.Add(gtSelP3)
            gtSelP3.Value = w(3)
            DbCgtSelect.Parameters.Add(gtSelP4)
            gtSelP4.Value = w(4)
            Dim txt As String = ""
            Try
                DRgtSelect = DbCgtSelect.ExecuteReader()
                If (DRgtSelect.Read()) Then
                    txt = GetDbStringValue(DRgtSelect, 0)
                End If
            Catch ex As Exception
                MsgBox(ex.ToString())
            Finally
                If Not (DRgtSelect Is Nothing) Then
                    DRgtSelect.Close()
                End If
            End Try
            Dim n As String = String.Empty, execName As String = String.Empty, k As Integer, cvr As  DefVariable = Nothing
            RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition("T",  Rexx.tpSymbol.tpVariable, cvr), txt, k, execName, n)
        End If
        rc = 0
        e.rc = rc
    End Sub

End Class
Friend Class CollNm
    Friend n As Integer
End Class
