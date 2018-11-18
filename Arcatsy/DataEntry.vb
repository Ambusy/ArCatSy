Imports RxLib.Rexx
'
Public Class DataEntry
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim WithEvents Rx As New Rexx(New RexxCompData)
    Dim Ctls As New Collection, Defines As New Collection
    Dim SizeLabelHeight, SizeTextWidth, SizeTextHeight As Integer
    Dim LeaveField As String = String.Empty
    Dim EnterField As String = String.Empty, PrevEnteredField As Integer = 0
    Dim CurrentField As String = String.Empty, CurEnteredField As Integer = 0
    Dim FiFldOnScr, LaFldOnScr As Integer
    Dim FieldWithFocus As String = String.Empty, FirstVisField As String
    Dim FieldsAddedDeleted As Boolean = False, FieldsAddedDeletedIx As Integer = 0
    Dim DataChanged As Boolean = False
    Dim ProgrammedChange As Boolean = False
    Dim ProgrammedFocus As Boolean = False
    Dim FieldInError As Boolean = False, PrvMsg As String = ""
    Dim focusC As Control, RexxActive As Boolean = False
    Dim SelKeyWords() As String, SelKeyDbRes() As String
    Dim CurCollCode As String, CurCollSeq As Integer, TypeEntry As String
    Dim Hidden As New Collection, LayoutChangedIx As Integer = 0
    Friend nSuspend As Integer

    ' insert een kenmerk met data
    Dim insertSQL As String = "INSERT INTO data (CollCode, CollSeq, FldCode, FldSeqnr, FldText) VALUES (?, ?, ?, ?, ?)"
    Dim DRInsert As OdbcDataReader = Nothing
    Dim DbCInsert As OdbcCommand
    Dim InsP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim InsP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    Dim InsP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)
    Dim InsP4 As New OdbcParameter("@FldSeqnr", Odbc.OdbcType.Decimal, 16)
    Dim InsP5 As New OdbcParameter("@FldText", Odbc.OdbcType.VarChar, 255)
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
    ' haal kenmerk titel en regexp op
    Dim kSelectSQL As String = "SELECT FldCheckRegexp, FldCheckErrmsg FROM collfields WHERE CollCode = ? AND FldCode = ?"
    Dim DRkSelect As OdbcDataReader = Nothing
    Dim DbCkSelect As OdbcCommand
    Dim kSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim kSelP2 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)
    ' haal combo text op
    Dim cSelectSQL As String = "SELECT CmbText FROM combo WHERE CmbCode = ? ORDER BY CmbOrder"
    Dim DRcSelect As OdbcDataReader = Nothing
    Dim DbcCSelect As OdbcCommand
    Dim cSelP1 As New OdbcParameter("@CmbCode", Odbc.OdbcType.VarChar, 50)
    ' kijk of een kenmerk met data bestaat
    Dim ExistsSQL As String = "SELECT FldText FROM data WHERE CollCode = ? AND CollSeq = ? AND FldCode = ? AND FldSeqnr = ?"
    Dim DRExists As OdbcDataReader = Nothing
    Dim DbCExists As OdbcCommand
    Dim ExiP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim ExiP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    Dim ExiP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)
    Dim ExiP4 As New OdbcParameter("@FldSeqnr", Odbc.OdbcType.Decimal, 16)
    ' Update een kenmerk
    Dim UpdateSQL As String = "UPDATE data SET FldText = ? WHERE CollCode = ? AND CollSeq = ? AND FldCode = ? AND FldSeqnr = ?"
    Dim DRUpdate As OdbcDataReader = Nothing
    Dim DbCUpdate As OdbcCommand
    Dim UpdP1 As New OdbcParameter("@FldText", Odbc.OdbcType.VarChar, 255)
    Dim UpdP2 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim UpdP3 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    Dim UpdP4 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)
    Dim UpdP5 As New OdbcParameter("@FldSeqnr", Odbc.OdbcType.Decimal, 16)
    ' Delete een kenmerk
    Dim DeleteSQL As String = "DELETE FROM data WHERE CollCode = ? AND CollSeq = ? AND FldCode = ? AND FldSeqnr = ?"
    Dim DRDelete As OdbcDataReader = Nothing
    Dim DbCDelete As OdbcCommand
    Dim DelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
    Dim DelP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
    Dim DelP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 4)
    Dim DelP4 As New OdbcParameter("@FldSeqnr", Odbc.OdbcType.Decimal, 16)
    Private Sub CInsMod()
        If TypeEntry = "E" Or CurCollSeq = -1 Then
            InsMod.Text = SysMsg(63)
        Else
            InsMod.Text = SysMsg(64)
        End If
    End Sub
    Private Sub DataEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurCollCode = CollCodeMenuSel ' from menu
        CurCollSeq = CollSeqSel ' from menu
        TypeEntry = TypeEntryMenu
        Me.Text = SysMsg(324)
        Me.DesktopLocation = New Point(0, 0)
        BClear.Text = SysMsg(303)
        Bsave.Text = SysMsg(65)
        BSelKey.Text = SysMsg(304)
        BNext.Text = SysMsg(60)
        BSelKey.Visible = (TypeEntry <> "E")
        CInsMod()
        BNext.Visible = (TypeEntry <> "E" And CurCollSeq > -1)

        Form1.Progre.Text = SysMsg(305)
        Form1.Progre.Refresh()
        Dim RexxName As String = ""
        ' Dim dirs As String() = Directory.GetFiles(My.Application.Info.DirectoryPath & "\", "DE *.rex")
        Dim dirs As String() = Directory.GetFiles(My.Application.Info.DirectoryPath & "\", "DE " & CurCollCode & " *.rex")
        If dirs.Length() = 0 Then
            dirs = Directory.GetFiles(My.Application.Info.DirectoryPath & "\", "DE " & CurCollCode & ".rex")
        End If
        If dirs.Length() > 1 Then
            Dim m As String = "", i As Integer
            For i = 0 To dirs.Length() - 1
                m = m & CStr(i + 1) & " " & Path.GetFileName(dirs(i)) & vbCrLf
            Next
            m = m & vbCrLf & "select file"
l1:         Try
                i = CInt(InputBox(m, "", "1"))
                RexxName = dirs(i - 1)
            Catch
                GoTo l1
            End Try
        Else
            If dirs.Length() > 0 Then
                RexxName = dirs(0)
            Else
                RexxName = "????.rex"
            End If
        End If

        If Rx.CompileRexxScript(RexxName) <> 0 Then
            MsgBox(SysMsg(306))
            Me.Close()
            Exit Sub
        End If
        Form1.Progre.Text = SysMsg(307)
        Form1.Progre.Refresh()

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

        DbCkSelect = New OdbcCommand(kSelectSQL, DbConn)
        DbCkSelect.Parameters.Add(kSelP1)
        DbCkSelect.Parameters.Add(kSelP2)

        DbcCSelect = New OdbcCommand(cSelectSQL, DbConn)
        DbcCSelect.Parameters.Add(cSelP1)

        DbCUpdate = New OdbcCommand(UpdateSQL, DbConn)
        DbCUpdate.Parameters.Add(UpdP1)
        DbCUpdate.Parameters.Add(UpdP2)
        DbCUpdate.Parameters.Add(UpdP3)
        DbCUpdate.Parameters.Add(UpdP4)
        DbCUpdate.Parameters.Add(UpdP5)

        DbCDelete = New OdbcCommand(DeleteSQL, DbConn)
        DbCDelete.Parameters.Add(DelP1)
        DbCDelete.Parameters.Add(DelP2)
        DbCDelete.Parameters.Add(DelP3)
        DbCDelete.Parameters.Add(DelP4)

        DbCExists = New OdbcCommand(ExistsSQL, DbConn)
        DbCExists.Parameters.Add(ExiP1)
        DbCExists.Parameters.Add(ExiP2)
        DbCExists.Parameters.Add(ExiP3)
        DbCExists.Parameters.Add(ExiP4)

        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try

        Form1.Progre.Text = SysMsg(308)
        Form1.Progre.Refresh()
        RxExe(TypeEntry & " D")
        ProgrammedChange = True
        If TypeEntry = "E" Then
            Rx.ExecuteRexxScript(TypeEntry & " I") ' Init
        Else
            Rx.ExecuteRexxScript(TypeEntry & " K") ' select Key
        End If
        If LayoutChangedIx > 0 Then
            LayoutScreen(LayoutChangedIx)
            LayoutChangedIx = 0
        End If
        ProgrammedChange = False
        If DbConn.State = ConnectionState.Open Then DbConn.Close()
        Form1.Progre.Text = ""
        Form1.Progre.Refresh()
    End Sub
    Private Sub DataEntry_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        CheckChanged()
        If (DbConn.State = ConnectionState.Open) Then
            DbConn.Close()
        End If
        LayoutChangedIx = 0
    End Sub
    Private Sub DataEntry_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        ' velden staan al op de juiste plaats
        Gr.Location = New Point(Me.Location.X, Me.Location.Y + 31)
        Gr.Size = New Size(Me.Size.Width - 10, Me.Size.Height - 65)
    End Sub
    Private Sub ClearScreen()
        DoSuspendLayout()
        InitialScreen(False)
        LayoutScreen(0)
        SetFocus(FirstVisField)
        DataChanged = False
        DoResumeLayout()
    End Sub
    Private Sub CheckChanged()
        If DataChanged Then
            If MessageBox.Show(SysMsg(61), "", MessageBoxButtons.YesNo) = vbYes Then
                RxExe(TypeEntry & " S")
            End If
        End If
    End Sub
    Friend Sub DoSuspendLayout()
        nSuspend += 1
        If nSuspend = 1 Then
            Me.SuspendLayout()
        End If
    End Sub
    Friend Sub DoResumeLayout()
        nSuspend -= 1
        If nSuspend = 0 Then
            Me.ResumeLayout(True)
        End If
    End Sub
    Private Sub BClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BClear.Click
        DoSuspendLayout()
        CheckChanged()
        ClearScreen()
        DoResumeLayout()
    End Sub
    Private Sub SaveAll()
        DoSuspendLayout()
        RxExe(TypeEntry & " S")
        If TypeEntry = "E" Then
            RxExe(TypeEntry & " I")
        End If
        DoResumeLayout()
    End Sub
    Private Sub BSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bsave.Click
        SaveAll()
    End Sub
    Private Sub Control_Expand(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer
        Dim c As Control = DirectCast(sender, Control)
        Dim ct As CtlsData = DirectCast(Ctls.Item(c.Name.Substring(4)), CtlsData) ' button
        If ct.Expanded Then
            c.Text = "+"
            For i = ct.Ix + 1 To Ctls.Count
                Dim ctc As CtlsData = DirectCast(Ctls.Item(i), CtlsData)
                If ctc.CLevel > ct.CLevel Then
                    VisibleLabelButtonText(ctc, False)
                Else
                    Exit For
                End If
            Next
        Else
            Dim PrevValue As String = String.Empty
            c.Text = "-"
            For i = ct.Ix + 1 To Ctls.Count
                Dim ctc As CtlsData = DirectCast(Ctls.Item(i), CtlsData)
                If ctc.CLevel = ct.CLevel + 1 Then
                    Control_ExpandChild(ctc, PrevValue)
                Else
                    If ctc.CLevel <= ct.CLevel Then Exit For
                End If
            Next
        End If
        ct.Expanded = Not ct.Expanded
        LayoutScreen(ct.Ix)
    End Sub
    Private Sub VisibleLabelButtonText(ByVal ctc As CtlsData, ByVal hoe As Boolean)
        Dim cc As Control
        ctc.Visible = hoe
        If ctc.HasDep Then
            cc = DirectCast(Gr.Controls.Item(ctc.CtlsIx - 2), Control) ' label
        Else
            cc = DirectCast(Gr.Controls.Item(ctc.CtlsIx - 1), Control) ' label
        End If
        If cc.Visible <> hoe Then
            cc.Visible = hoe
        End If
        If ctc.HasDep Then
            cc = DirectCast(Gr.Controls.Item(ctc.CtlsIx - 1), Control) ' button
            If hoe And ctc.HasDep Then
                If cc.Visible <> hoe Then
                    cc.Visible = hoe
                End If
            Else
                cc.Visible = False
            End If
        End If
        cc = DirectCast(Gr.Controls.Item(ctc.CtlsIx), Control) ' text
        If cc.Visible <> hoe Then
            cc.Visible = hoe
        End If
        FieldsAddedDeleted = True
        FieldsAddedDeletedIx = ctc.Ix
    End Sub
    Private Sub Control_ExpandChild(ByVal ct As CtlsData, ByRef PrevValue As String)
        If ct.Seq(ct.CLevel) = 1 Or PrevValue <> String.Empty Then
            VisibleLabelButtonText(ct, True)
            PrevValue = DirectCast(Gr.Controls.Item(ct.CtlsIx), Control).Text
            Dim nPrevValue As String = String.Empty
            If ct.Expanded Then
                For i As Integer = ct.Ix + 1 To Ctls.Count
                    Dim ctc As CtlsData = DirectCast(Ctls.Item(i), CtlsData)
                    If ctc.CLevel = ct.CLevel + 1 Then
                        Control_ExpandChild(ctc, nPrevValue)
                    Else
                        If ctc.CLevel <= ct.CLevel Then Exit For
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub Control_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim c As Control, cf As CtlsData, cna, fna As String, i As Integer
        Dim cp As CtlsData
        ' l e a v e  field
        c = DirectCast(sender, Control)
        cna = c.Name.Substring(2)
        If ProgrammedFocus Or cna = FieldWithFocus Then
            Exit Sub ' ongewenst of 1e keer
        End If
        DoSuspendLayout()
        If FieldInError Then
            FieldInError = False
            Msg.Text = PrvMsg
            PrvMsg = ""
        End If
        cp = DirectCast(Ctls.Item(FieldWithFocus), CtlsData)
        c = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
        Dim d As DefData = DirectCast(Defines.Item(cp.Naam), DefData)
        If d.RegExpC <> "" AndAlso c.Text.Length() > 0 Then
            Dim mc As MatchCollection
            Dim regExpre As New Regex(d.RegExpC)
            mc = regExpre.Matches(c.Text)
            If mc.Count = 0 Then
                ProgrammedFocus = True
                PrvMsg = Msg.Text
                Msg.Text = d.RegExpErr
                FieldInError = True
                fna = FieldWithFocus
                FieldWithFocus = String.Empty
                SetFocus(fna)
                ProgrammedFocus = False
                DoResumeLayout()
                Exit Sub
            End If
        End If
        If cp.LeaveExit Then
            i = RxExe(TypeEntry & " L " & FieldWithFocus)
            If i > 0 Then
                ProgrammedFocus = True
                fna = FieldWithFocus
                FieldWithFocus = String.Empty
                SetFocus(fna)
                ProgrammedFocus = False
                DoResumeLayout()
                Exit Sub
            End If
        End If
        ' e n t e r  field
        Dim pna As String
        c = DirectCast(sender, Control)
        focusC = c
        cna = c.Name.Substring(2)
        cf = DirectCast(Ctls.Item(cna), CtlsData)
        CurEnteredField = cf.Ix
        cp = DirectCast(Ctls.Item(FieldWithFocus), CtlsData)
        PrevEnteredField = cp.Ix
        If c.Text = String.Empty Then ' we komen in een leeg veld terecht.
            d = DirectCast(Defines.Item(cf.Naam), DefData)
            If d.Parent <> String.Empty Then ' maar het veld heeft een parent 
                i = InStrRev(cf.NaamRst, "."c)
                pna = d.Parent & cf.NaamRst.Substring(0, i - 1)
                cp = DirectCast(Ctls.Item(pna), CtlsData)
                c = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
                If c.Text = String.Empty Then ' en de parent is leeg, dan skippen we: 
                    '                           omlaag naar het eerstvolgende veld op het parent nivo, 
                    '                           omhoog naar het parent veld
                    FieldWithFocus = String.Empty
                    If PrevEnteredField < CurEnteredField Then ' omlaag
                        Dim cc As CtlsData = DirectCast(Ctls.Item(CurEnteredField), CtlsData)
                        For i = CurEnteredField + 1 To Ctls.Count
                            Dim cn As CtlsData = DirectCast(Ctls.Item(i), CtlsData)
                            If cn.CLevel <= cc.CLevel Then
                                c = DirectCast(Gr.Controls.Item(cn.CtlsIx), Control)
                                If c.TabStop Then
                                    focusC = c
                                    Exit For
                                End If
                            End If
                        Next
                    Else ' omhoog
                        focusC = c
                    End If
                End If
            End If
        End If
        ProgrammedFocus = True
        If Not focusC.Visible Then
            focusC = FndNxtVisible(focusC)
        End If
        If FieldsAddedDeleted Then
            LayoutScreen(FieldsAddedDeletedIx)
        End If
        SetFocus(focusC.Name.Substring(2))
        ProgrammedFocus = False
        DoResumeLayout()
    End Sub
    Private Sub ShowField(ByVal c As Control)
        If (c.Bounds.Top + SizeTextHeight + 20) > Gr.Height Then
            MoveFields(Gr.Height - (c.Bounds.Top + SizeTextHeight + 20))
        ElseIf c.Bounds.Top < 0 Then
            MoveFields(-c.Bounds.Top + SizeTextHeight + 5)
        End If
    End Sub
    Private Sub MoveFields(ByVal delta As Integer)
        Dim c As Control, cc As CtlsData, CurFldOnScr As Integer
        DoSuspendLayout()
        FiFldOnScr = -1
        LaFldOnScr = -1
        CurFldOnScr = -1
        For Each cc In Ctls
            If cc.Visible Then
                cc.Top = cc.Top + delta
                If FiFldOnScr = -1 AndAlso cc.Top > -1 Then FiFldOnScr = cc.Ix
                If cc.Top <= (Gr.Height - SizeTextHeight - SizeLabelHeight - 5) Then LaFldOnScr = cc.Ix
                If FieldWithFocus = cc.NaamNm AndAlso cc.Top > 0 AndAlso cc.Top < (Gr.Height - 10) Then
                    CurFldOnScr = cc.Ix
                End If
                If cc.HasDep Then
                    c = DirectCast(Gr.Controls.Item(cc.CtlsIx - 2), Control)
                Else
                    c = DirectCast(Gr.Controls.Item(cc.CtlsIx - 1), Control)
                End If
                c.Location = New System.Drawing.Point(cc.CLevel * 7, cc.Top)
                If cc.HasDep Then
                    c = DirectCast(Gr.Controls.Item(cc.CtlsIx - 1), Control)
                    c.Location = New System.Drawing.Point(cc.CLevel * 7 + SizeTextWidth, cc.Top + SizeLabelHeight + 7)
                End If
                c = DirectCast(Gr.Controls.Item(cc.CtlsIx), Control)
                c.Location = New System.Drawing.Point(cc.CLevel * 7, cc.Top + SizeLabelHeight + 7)
            End If
        Next
        If FiFldOnScr = -1 Or LaFldOnScr = -1 Then
            MoveFields(-delta)
        Else
            cc = DirectCast(Ctls.Item(FiFldOnScr), CtlsData)
            If cc.Top > 2 * (SizeLabelHeight + SizeLabelHeight + 10) Then
                MoveFields(-cc.Top)
            End If
            If CurFldOnScr = -1 Then
                cc = DirectCast(Ctls.Item(FiFldOnScr), CtlsData)
                SetFocus(cc.NaamNm)
            End If
        End If
        DoResumeLayout()
    End Sub
    Private Sub DataEntry_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        DoSuspendLayout()
        If e.KeyCode = 83 And e.Control Then ' ctrl-S
            SaveAll()
            e.Handled = True
        ElseIf e.KeyCode = 69 And e.Control Then ' ctrl-E
            ClearScreen()
            e.Handled = True
        ElseIf e.KeyCode = 65 And e.Control Then ' ctrl-A
            SelKey()
            e.Handled = True
        ElseIf e.KeyCode = 90 And e.Control Then ' ctrl-Z
            SelKeyNxt()
            e.Handled = True
        ElseIf e.KeyCode = 13 Then ' enter
            My.Computer.Keyboard.SendKeys("{TAB}", True)
            e.Handled = True
        ElseIf e.KeyCode = 33 Then ' pgup
            MoveFields(Gr.Height - 18)
            e.Handled = True
        ElseIf e.KeyCode = 34 Then ' pgdown
            MoveFields(-Gr.Height + 18)
            e.Handled = True
        ElseIf e.KeyCode = 46 Then
            DataChanged = True
            Msg.Text = ""
        ElseIf e.KeyCode = 112 Then ' f1
            Dim cp As CtlsData = DirectCast(Ctls(FieldWithFocus), CtlsData)
            Dim c As Control = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
            If c.Text.TrimEnd().Length() = 0 Then
                OpenFileDialog1.Title = SysMsg(868)
                OpenFileDialog1.FileName = ""
                OpenFileDialog1.Filter = "All files (*.*)|*.*"
                OpenFileDialog1.CheckFileExists = True
                OpenFileDialog1.Multiselect = False
                OpenFileDialog1.ShowHelp = False
                OpenFileDialog1.ShowDialog()
                If OpenFileDialog1.FileNames(0) > "" Then
                    c.Text = OpenFileDialog1.FileNames(0)
                    DataChanged = True
                End If
            Else
                If File.Exists(c.Text) Then
                    Dim myProcess As Process = Process.Start(c.Text, "")
                End If
            End If
        End If
        DoResumeLayout()
    End Sub
    Private Sub DataEntry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        DataChanged = True
        Msg.Text = ""
    End Sub
    Private Function FndNxtVisible(ByVal sender As Control) As Control
        Dim cna As String, i, nextFocus As Integer, c As Control, cp As CtlsData, s As String
        Dim fnd As Boolean = False
        cna = DirectCast(sender, Control).Name.Substring(2)
        cp = DirectCast(Ctls(FirstVisField), CtlsData)
        FndNxtVisible = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
        nextFocus = 0
        i = -1 ' where is the actual focus?
        s = "V." & cna
        For Each c In Gr.Controls
            i += 1
            If c.Name = s Then
                nextFocus = i
                Exit For
            End If
        Next
        For Each cp In Ctls
            If cp.CtlsIx = nextFocus Then
                fnd = True
            ElseIf fnd Then
                If cp.Visible Then
                    FndNxtVisible = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
                    Exit For
                End If
            End If
        Next
    End Function
    Private Sub Combo_Changed(ByVal sender As Object, ByVal e As System.EventArgs)
        DataChanged = True
        Msg.Text = ""
    End Sub
    Private Sub Control_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ProgrammedChange Then
            Exit Sub
        End If
        DoSuspendLayout()
        If FieldInError Then
            FieldInError = False
            Msg.Text = PrvMsg
            PrvMsg = ""
        End If
        Dim cna As String = DirectCast(sender, Control).Name.Substring(2)
        Dim cf As CtlsData = DirectCast(Ctls.Item(cna), CtlsData)
        Dim c As Control = DirectCast(sender, Control)
        cf.HadText = cf.HasText
        cf.HasText = (c.Text.Length() > 0)
        If cf.HadText <> cf.HasText Then
            HideOrShowChildrenFields(sender, True)
            SetTabstops()
        End If
        If cf.ChaExit Then
            ProgrammedChange = True
            RxExe(TypeEntry & " C " & FieldWithFocus)
            ProgrammedChange = False
            If cf.IsCombo Then
                DirectCast(c, ComboBox).SelectionLength = 0
                DirectCast(c, ComboBox).SelectionStart = 9999
            Else
                DirectCast(c, TextBox).SelectionLength = 0
                DirectCast(c, TextBox).SelectionStart = 9999
            End If
        End If
        If cf.HadText <> cf.HasText Then
            If FieldsAddedDeleted Then
                LayoutScreen(FieldsAddedDeletedIx)
            End If
        End If
        DoResumeLayout()
    End Sub
    Private Sub HideOrShowChildrenFields(ByVal sender As Object, ByVal CalcPos As Boolean)
        Dim cna, cnn, na, cont As String, i, j, k, nrpa As Integer
        Dim c As Control, cf, cn, cm As CtlsData
        ' toon een verborgen kind, wanneer een parent een waarde heeft, poets de inhoud als ook de parent leeg is
        cna = DirectCast(sender, Control).Name.Substring(2)
        cf = DirectCast(Ctls.Item(cna), CtlsData)
        cont = DirectCast(sender, Control).Text ' inhoud van het veld, wat ik ga verlaten
        If cont <> String.Empty Then
            nrpa = DirectCast(Defines.Item(cf.Naam), DefData).MaxOccurs
            If nrpa > 1 AndAlso cf.Seq(cf.CLevel) < nrpa Then
                i = InStrRev(cna, "."c)
                na = cna.Substring(0, i) & CStr(cf.Seq(cf.CLevel) + 1)
                MakeVis(na)
                If CalcPos Then
                    LayoutScreen(DirectCast(Ctls.Item(na), CtlsData).Ix)
                End If
            End If
        Else
            c = DirectCast(Gr.Controls.Item(cf.CtlsIx), Control)
            If c.Text = String.Empty Then ' parent leeg: kinderen ook  leeg maken
                For j = cf.Ix + 1 To Ctls.Count
                    cn = DirectCast(Ctls.Item(j), CtlsData)
                    If cn.CLevel <= cf.CLevel Then
                        Exit For
                    End If
                    c = DirectCast(Gr.Controls.Item(cn.CtlsIx), Control)
                    If c.Text <> String.Empty Then
                        c.Text = String.Empty ' maak leeg
                        cn.HasText = False
                        cnn = c.Name.Substring(2)
                        k = InStrRev(cnn, "."c)
                        If CInt(cnn.Substring(k)) > 1 Then ' laatste seqnr > 1? mischien verbergen
                            For k = j To Ctls.Count
                                cm = DirectCast(Ctls.Item(k), CtlsData)
                                If cm.CLevel <= cn.CLevel And k > j Then
                                    Exit For
                                End If
                                'c = DirectCast(Gr.Controls.Item(cn.CtlsIx), Control)
                                If cn.Visible Then
                                    VisibleLabelButtonText(cm, False)
                                End If
                            Next
                        End If
                    End If
                Next
            End If
            For i = cf.Ix + 1 To Ctls.Count ' zelfde FldCode zichtbaar zonder tekst: verbergen
                cn = DirectCast(Ctls.Item(i), CtlsData)
                If cn.CLevel < cf.CLevel Then
                    Exit For
                End If
                If cn.CLevel = cf.CLevel Then
                    c = DirectCast(Gr.Controls.Item(cn.CtlsIx), Control)
                    If cf.Naam = cn.Naam Then ' zelfde FldCode  ?
                        If c.Visible AndAlso c.Text = String.Empty Then ' 2 lege searchterms achter elkaar, de 2e verberg ik
                            For j = i To Ctls.Count
                                cn = DirectCast(Ctls.Item(j), CtlsData)
                                If cn.CLevel < cf.CLevel Or (j > i And cn.CLevel = cf.CLevel) Then
                                    Exit For
                                End If
                                c = DirectCast(Gr.Controls.Item(cn.CtlsIx), Control)
                                If c.Text <> String.Empty Then c.Text = String.Empty ' maak leeg
                                If cn.Visible Then
                                    VisibleLabelButtonText(cn, False)
                                End If
                            Next
                        End If
                        Exit For
                    End If
                End If
            Next
            j = InStrRev(cna, "."c)
            If CInt(cna.Substring(j)) > 1 Then   ' laatste seqnr > 1? mischien verbergen
                For i = cf.Ix - 1 To 1 Step -1
                    cn = DirectCast(Ctls.Item(i), CtlsData)
                    If cn.CLevel < cf.CLevel Then
                        Exit For
                    End If
                    If cn.CLevel = cf.CLevel Then ' zelfde level? moet zelfde FldCode zijn
                        c = DirectCast(Gr.Controls.Item(cn.CtlsIx), Control)
                        If c.Text = String.Empty Then ' 2 lege searchterms achter elkaar
                            For j = cf.Ix To Ctls.Count
                                cn = DirectCast(Ctls.Item(j), CtlsData)
                                If cn.CLevel < cf.CLevel Then
                                    Exit For
                                End If
                                If cn.CLevel >= cf.CLevel Then
                                    c = DirectCast(Gr.Controls.Item(cn.CtlsIx), Control)
                                    If c.Text <> String.Empty Then c.Text = String.Empty ' maak leeg
                                    If c.Visible Then
                                        VisibleLabelButtonText(cn, False)
                                    End If
                                End If
                            Next
                        End If
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub SetTabstops()
        Dim c As Control, cf, cn As CtlsData, j As Integer
        For Each cf In Ctls
            c = DirectCast(Gr.Controls.Item(cf.CtlsIx), Control)
            c.TabStop = c.Visible
        Next
        For Each cf In Ctls
            c = DirectCast(Gr.Controls.Item(cf.CtlsIx), Control)
            If c.Visible AndAlso c.Text = String.Empty Then
                For j = cf.Ix + 1 To Ctls.Count
                    cn = DirectCast(Ctls.Item(j), CtlsData)
                    If cn.CLevel <= cf.CLevel Then
                        Exit For
                    End If
                    c = DirectCast(Gr.Controls.Item(cn.CtlsIx), Control)
                    If c.TabStop Then
                        c.TabStop = False
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub MakeVis(ByVal na As String)
        Dim cf As CtlsData, c As Control
        cf = DirectCast(Ctls.Item(na), CtlsData)
        cf.Visible = True
        If cf.HasDep Then
            c = DirectCast(Gr.Controls.Item(cf.CtlsIx - 2), Control)
        Else
            c = DirectCast(Gr.Controls.Item(cf.CtlsIx - 1), Control)
        End If
        c.Visible = True
        If cf.HasDep Then
            c = DirectCast(Gr.Controls.Item(cf.CtlsIx - 1), Control)
            c.Visible = True
        End If
        c = DirectCast(Gr.Controls.Item(cf.CtlsIx), Control)
        c.Visible = True
        For Each d As DefData In Defines
            If d.Parent = cf.Naam Then
                MakeVis(d.Naam & cf.NaamRst & ".1")
            End If
        Next
    End Sub
    Private Sub DoDefine(ByVal e As  RexxEvent, ByRef RexxEnv As  Rexx)
        For Each d As DefData In Defines
            If d.Parent = String.Empty Then
                For i As Integer = 1 To d.MaxOccurs
                    DefField(d, String.Empty, 1, i, e, RexxEnv, False, Nothing)
                Next
            End If
        Next
    End Sub
    Private Sub DefField(ByRef d As DefData, ByVal suffix As String, ByVal level As Integer, ByVal Occurence As Integer, ByVal e As  RexxEvent, ByRef RexxEnv As  Rexx, ByVal hPar As Boolean, ByVal cPar As CtlsData)
        Dim st As String = String.Empty
        Dim FldName As String, i As Integer, HasDep As Boolean
        HasDep = False
        For Each dc As DefData In Defines
            If dc.Parent = d.Naam Then
                HasDep = True
                Exit For
            End If
        Next
        Dim lb As New Label
        lb.AutoSize = True
        lb.Size = New System.Drawing.Size(1, 1)
        lb.TabIndex = 0
        FldName = d.Naam & suffix & "." & CStr(Occurence)
        lb.Name = "L." & FldName
        lb.Text = d.Caption
        kSelP1.Value = CurCollCode
        kSelP2.Value = d.Naam
        Try
            DRkSelect = DbCkSelect.ExecuteReader()
            If DRkSelect.Read() Then
                d.RegExpC = GetDbStringValue(DRkSelect, 0)
                d.RegExpErr = GetDbStringValue(DRkSelect, 1)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(325))
        Finally
            If Not (DRkSelect Is Nothing) Then
                DRkSelect.Close()
            End If
        End Try
        Gr.Controls.Add(lb)
        If HasDep Then
            Dim eb As New Button
            eb.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            eb.Name = "L.B." & FldName
            eb.Size = New System.Drawing.Size(17, 18)
            eb.Text = "+"
            eb.TabStop = False
            Gr.Controls.Add(eb)
            AddHandler eb.Click, AddressOf Control_Expand
        End If
        If d.Type = "T" Then
            Dim tb As New TextBox
            tb.Size = New System.Drawing.Size(SizeTextWidth, SizeTextHeight)
            tb.Name = "V." & FldName
            tb.Text = String.Empty
            tb.MaxLength = d.MaxLength
            Gr.Controls.Add(tb)
            AddHandler tb.Enter, AddressOf Control_Enter
            AddHandler tb.TextChanged, AddressOf Control_Changed
        Else
            Dim cb As New ComboBox
            cb.FormattingEnabled = True
            cb.Name = "V." & FldName
            cb.Size = New System.Drawing.Size(SizeTextWidth, SizeTextHeight)
            cb.Text = String.Empty
            cb.MaxLength = d.MaxLength
            Try
                cSelP1.Value = d.Combo
                DRcSelect = DbcCSelect.ExecuteReader()
                Do While (DRcSelect.Read())
                    cb.Items.Add(GetDbStringValue(DRcSelect, 0))
                Loop
            Catch ex As Exception
                MsgBox(ex.ToString(), , SysMsg(326))
            Finally
                If Not (DRcSelect Is Nothing) Then
                    DRcSelect.Close()
                End If
            End Try
            Gr.Controls.Add(cb)
            AddHandler cb.Enter, AddressOf Control_Enter
            AddHandler cb.TextChanged, AddressOf Control_Changed
            AddHandler cb.SelectedIndexChanged, AddressOf Combo_Changed
        End If
        If CurrentField = "" Then CurrentField = FldName
        Dim ct As New CtlsData
        ct.CtlsIx = Gr.Controls.Count - 1
        ct.CLevel = level
        ct.HasDep = HasDep
        ct.HasParent = hPar
        ct.Expanded = True
        ct.HasText = False
        ct.HadText = False
        ct.ChaExit = d.ChaExit
        ct.LeaveExit = d.LeaveExit
        ct.Naam = d.Naam
        ct.NaamNm = FldName
        ct.NaamNm1 = d.Naam & suffix & "."
        ct.NaamRst = suffix & "." & CStr(Occurence)
        If Not cPar Is Nothing Then
            For i = 0 To 5
                ct.Seq(i) = cPar.Seq(i)
            Next
        End If
        ct.Seq(level) = Occurence
        ct.OldData = String.Empty
        ct.IsCombo = (d.Type = "C")
        Ctls.Add(ct, FldName)
        ct.Ix = Ctls.Count
        If HasDep Then
            For Each dc As DefData In Defines
                If dc.Parent = d.Naam Then
                    For i = 1 To dc.MaxOccurs
                        DefField(dc, suffix & "." & CStr(Occurence), level + 1, i, e, RexxEnv, True, ct)
                    Next
                End If
            Next
        End If
    End Sub
    Sub RexxCmd(ByVal env As String, ByVal s As String, ByVal e As  RexxEvent, ByRef RexxEnv As  Rexx) Handles Rx.doCmd
        Dim execName As String = String.Empty, srcName As String = String.Empty, n As String = String.Empty, k As Integer
        Dim cvr As  DefVariable = Nothing
        Dim os As String = s
        Dim cf As CtlsData
        Dim FiW, na, st As String, eRc As Integer = 0, Ixn As Integer
        RexxActive = True
        Try
            FiW = NxtWordFromStr(s).ToUpper(CultInf)
            If FiW <> "SIZE" And FiW <> "DEFINE" And Defined Then
                Form1.Progre.Text = SysMsg(309)
                Form1.Progre.Refresh()
                DoDefine(e, RexxEnv)
                Defined = False
                Form1.Progre.Text = SysMsg(310)
                Form1.Progre.Refresh()
                InitialScreen(True)
                LayoutScreen(0)
                DataChanged = False
            End If
            Select Case FiW
                Case "SIZE"   ' SIZE HeightLabel HeightText WidthText
                    SizeLabelHeight = CInt(NxtWordFromStr(s))
                    SizeTextHeight = CInt(NxtWordFromStr(s))
                    SizeTextWidth = CInt(NxtWordFromStr(s))
                Case "DEFINE"   ' DEFINE kenmerk [parent] type aantalvelden maxlengte [combonaam] exitchanged exitleave  
                    Dim d As New DefData
                    d.Naam = NxtWordFromStr(s)
                    d.Type = NxtWordFromStr(s)
                    If d.Type <> "T" And d.Type <> "C" Then
                        d.Parent = d.Type
                        d.Type = NxtWordFromStr(s)
                    Else
                        d.Parent = String.Empty
                    End If
                    d.MaxOccurs = CInt(NxtWordFromStr(s))
                    d.MaxLength = CInt(NxtWordFromStr(s))
                    If d.Type = "C" Then
                        d.Combo = NxtWordFromStr(s)
                    Else
                        d.Combo = String.Empty
                    End If
                    d.ChaExit = (NxtWordFromStr(s).ToUpper(CultInf) = "Y")
                    d.LeaveExit = (NxtWordFromStr(s).ToUpper(CultInf) = "Y")
                    d.Caption = s
                    Defines.Add(d, d.Naam)
                    Defined = True
                Case "SYSMSG"
                    RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition("SYSMSG",  Rexx.tpSymbol.tpVariable, cvr), SysMsg(CInt(NxtWordFromStr(s))), k, execName, n)
                Case "HIDE"
                    na = NxtWordFromStr(s)
                    If Ctls.Contains(na) Then
                        If Not Hidden.Contains(na) Then
                            Hidden.Add(na, na)
                        End If
                        Ixn = DirectCast(Ctls.Item(na), CtlsData).Ix
                        If LayoutChangedIx < Ixn Then LayoutChangedIx = Ixn
                    Else
                        eRc = 4
                    End If
                Case "SHOW"
                    na = NxtWordFromStr(s)
                    If Ctls.Contains(na) Then
                        If Hidden.Contains(na) Then
                            Hidden.Remove(na)
                        End If
                        Ixn = DirectCast(Ctls.Item(na), CtlsData).Ix
                        If LayoutChangedIx < Ixn Then LayoutChangedIx = Ixn
                    Else
                        eRc = 4
                    End If
                Case "FOCUS"
                    na = NxtWordFromStr(s)
                    If Ctls.Contains(na) Then
                        cf = DirectCast(Ctls(na), CtlsData)
                        If cf.Visible Then
                            SetFocus(cf.Naam)
                            SetTabstops()
                        End If
                    Else
                        eRc = 4
                    End If
                Case "SELECTKEY"
                    Form1.Progre.Text = SysMsg(311)
                    Form1.Progre.Refresh()
                    GetOrAddSelection(s, 0)
                    If CurCollSeq = 0 Then
                        eRc = 4
                        Me.Close()
                    ElseIf CurCollSeq = -1 Then
                        eRc = 1 ' new entry
                        CInsMod()
                        BNext.Visible = False
                        ClearScreen()
                    Else
                        eRc = 0 ' existing entry
                        CInsMod()
                        BNext.Visible = True
                    End If
                Case "INPUTKEY"
                    Form1.Progre.Text = SysMsg(311)
                    Form1.Progre.Refresh()
                    GetDirectKey(s, 0)
                    If CurCollSeq = 0 Then
                        eRc = 4
                        Me.Close()
                    Else
                        eRc = 0 ' existing entry
                        CInsMod()
                        BNext.Visible = False
                    End If
                Case "INITDATA"
                    InitialScreen(True)
                    LayoutScreen(0)
                    SetFocus(FirstVisField)
                    Form1.Progre.Text = ""
                    Form1.Progre.Refresh()
                Case "READDATA"
                    If CurCollSeq > 0 Then
                        Form1.Progre.Text = SysMsg(312)
                        Form1.Progre.Refresh()
                        InitialScreen(True)
                        getSelectedData(RexxEnv)
                        LayoutScreen(0)
                        FieldWithFocus = String.Empty
                        SetFocus(FirstVisField)
                    Else
                        eRc = 4
                        Me.Close()
                    End If
                    DataChanged = False
                    Form1.Progre.Text = ""
                    Form1.Progre.Refresh()
                Case "GETDATA"
                    Form1.Progre.Text = SysMsg(313)
                    Form1.Progre.Refresh()
                    Dim w() As String = s.Trim.Split()
                    If w(0) = String.Empty Then
                        For Each cf In Ctls
                            GetDataC(cf, RexxEnv)
                        Next
                        st = "msg.Text"
                        RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition(st.ToUpper(CultInf),  Rexx.tpSymbol.tpVariable, cvr), Msg.Text, k, execName, n)
                    Else
                        For i As Integer = 0 To w.Length() - 1
                            If w(i).ToUpper() = "MSG" Then
                                st = "msg.Text"
                                RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition(st.ToUpper(CultInf),  Rexx.tpSymbol.tpVariable, cvr), Msg.Text, k, execName, n)
                            Else
                                For Each cf In Ctls
                                    If cf.Naam = w(i) Then
                                        GetDataC(cf, RexxEnv)
                                    End If
                                Next
                            End If
                        Next
                    End If
                    Form1.Progre.Text = ""
                    Form1.Progre.Refresh()
                Case "PUTDATA"
                    Form1.Progre.Text = SysMsg(314)
                    Form1.Progre.Refresh()
                    ProgrammedChange = True
                    Dim w() As String = s.Trim.Split()
                    If w(0) = String.Empty Then
                        For Each cf In Ctls
                            PutDataC(cf, RexxEnv)
                        Next
                        st = "msg.Text"
                        Msg.Text = RexxEnv.GetVar(RexxEnv.SourceNameIndexPosition(st.ToUpper(CultInf),  Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
                    Else
                        For i As Integer = 0 To w.Length() - 1
                            If w(i).ToUpper() = "MSG" Then
                                st = "msg.Text"
                                Msg.Text = RexxEnv.GetVar(RexxEnv.SourceNameIndexPosition(st.ToUpper(CultInf),  Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
                            Else
                                For Each cf In Ctls
                                    If cf.Naam = w(i) Then
                                        PutDataC(cf, RexxEnv)
                                    End If
                                Next
                            End If
                        Next
                    End If
                    ProgrammedChange = False
                    Form1.Progre.Text = ""
                    Form1.Progre.Refresh()
                Case "SAVE"
                    SaveScreen()
                    Msg.Text = SysMsg(315)
                    DataChanged = False
                Case "SQL"
                    SqlRexx(s, RexxEnv)
            End Select
            e.rc = eRc
        Catch ex As Exception
            MsgBox(ex.Message & " : " & os & " (" & s & ")")
        End Try
        RexxActive = False
    End Sub
    Private Sub SqlRexx(ByVal SelectSql As String, ByVal RexxEnv As  Rexx)
        Dim n As String = String.Empty, execName As String = String.Empty, k As Integer, cvr As  DefVariable = Nothing
        Dim DbConn As New OdbcConnection(SqlProv)
        Dim DbCursor As OdbcDataReader = Nothing
        Dim DbSelect As OdbcCommand
        DbSelect = New OdbcCommand(SelectSql, DbConn)
        Dim s1 As String, i As Integer
        Try
            DbConn.Open()
            DbCursor = DbSelect.ExecuteReader()
            i = 0
            Do While (DbCursor.Read())
                i += 1
                For j As Integer = 1 To DbCursor.FieldCount
                    Try
                        If DbCursor.Item(j - 1).GetType.Name = "Int32" Then
                            s1 = CStr(GetDbIntegerValue(DbCursor, j - 1))
                        ElseIf DbCursor.Item(j - 1).GetType.Name = "String" Then
                            s1 = GetDbStringValue(DbCursor, j - 1)
                        ElseIf DbCursor.Item(j - 1).GetType.Name = "Decimal" Then
                            s1 = CstrD(GetDbDecimalValue(DbCursor, j - 1))
                        ElseIf DbCursor.Item(j - 1).GetType.Name = "Boolean" Then
                            s1 = CStr(GetDbBooleanValue(DbCursor, j - 1))
                        Else
                            s1 = "Program limitation. UnKnown type: " & DbCursor.Item(0).GetType.Name
                        End If
                    Catch ex As Exception
                        s1 = "SQL error " & ex.ToString()
                    End Try
                    RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition("SQL." & CStr(j) & "." & CStr(i),  Rexx.tpSymbol.tpVariable, cvr), s1, k, execName, n)
                Next
            Loop
            RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition("SQL.0",  Rexx.tpSymbol.tpVariable, cvr), CStr(i), k, execName, n)
        Catch e1 As Exception
            MsgBox(e1.ToString())
        Finally
            If Not (DbCursor Is Nothing) Then
                DbCursor.Close()
            End If
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End Try
    End Sub
    Private Sub GetDataC(ByVal cf As CtlsData, ByVal RexxEnv As  Rexx)
        ' stores value/visible from screen textbox into REXX storage (Tells REXX what the user entered)
        Dim n As String = String.Empty, execName As String = String.Empty, k As Integer
        Dim cvr As  DefVariable = Nothing
        Dim c As Control
        Dim st As String = cf.NaamNm & ".visible"
        If cf.Visible Then
            RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition(st.ToUpper(CultInf),  Rexx.tpSymbol.tpVariable, cvr), "1", k, execName, n)
        Else
            RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition(st.ToUpper(CultInf),  Rexx.tpSymbol.tpVariable, cvr), "0", k, execName, n)
        End If
        c = DirectCast(Gr.Controls.Item(cf.CtlsIx), Control)
        st = cf.NaamNm & ".Text"
        ' Debug.WriteLine("GetData: " & cf.NaamNm & "=" & c.Text)
        RexxEnv.StoreVar(RexxEnv.SourceNameIndexPosition(st.ToUpper(CultInf),  Rexx.tpSymbol.tpVariable, cvr), c.Text, k, execName, n)
    End Sub
    Private Sub PutDataC(ByVal cf As CtlsData, ByVal rexxenv As Rexx)
        ' stores value from REXX storage into screen textbox (Tells the user what REXX has calculated)
        Dim n As String = String.Empty, execName As String = String.Empty, srcName As String = String.Empty
        Dim cvr As DefVariable = Nothing
        Dim c As Control, st As String
        c = DirectCast(Gr.Controls.Item(cf.CtlsIx), Control)
        st = c.Name.Substring(2) & ".Text"
        c.Text = rexxenv.GetVar(rexxenv.SourceNameIndexPosition(st.ToUpper(CultInf), Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
        ' Debug.WriteLine("PutData: " & cf.NaamNm & "=" & c.Text)
    End Sub
    Sub GetDirectKey(ByVal s As String, ByVal CollSeqReq As Integer)
        Dim SelectSql As String = String.Empty
        Dim DRSelectCursor As OdbcDataReader = Nothing
        Dim DbCSelectCursor As OdbcCommand
        Dim CollSeq As Integer
        Dim WasOpen As Boolean = (DbConn.State = ConnectionState.Open)
        Dim exists As Boolean = False
        While Not exists
            If DbConn.State = ConnectionState.Open Then DbConn.Close()
            Dim rnr As String = InputBox(SysMsg(328), SysMsg(328), "0")
            DbConn.Open()
            If rnr <> "0" Then
                SelectSql = "SELECT CollSeq FROM searchterms WHERE CollCode = '" & CurCollCode & "' AND TermType = '" & s & "' AND TermText = '" & rnr.Trim & "'"
                DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
                Try
                    DRSelectCursor = DbCSelectCursor.ExecuteReader()
                    If DRSelectCursor.Read() Then
                        CollSeq = GetDbIntegerValue(DRSelectCursor, 0)
                        exists = True
                    End If
                Catch e1 As Exception
                    MsgBox(e1.ToString())
                Finally
                    If Not (DRSelectCursor Is Nothing) Then
                        DRSelectCursor.Close()
                    End If
                End Try
                CurCollSeq = CollSeq
            Else
                exists = True
                CurCollSeq = 0
            End If
        End While
        If Not WasOpen Then DbConn.Close()
    End Sub
    Sub GetOrAddSelection(ByVal s As String, ByVal CollSeqReq As Integer)
        Dim wasClosed As Boolean = False
        If SelectKey.KeyList.Items.Count = 0 Or CollSeqReq > 0 Then
            CreateSelList(s, CollSeqReq)
        End If
        If CollSeqReq = 0 Then ' > 0: only add a new entry
            If DbConn.State = ConnectionState.Open Then DbConn.Close()
            SelectKey.ShowDialog()
            If CollSeqSel = -2 Then
                Do
                    If DbConn.State <> ConnectionState.Open Then DbConn.Open()
                    CollSeqSel = 0
                    CreateSelList(s, CollSeqSel)
                    If DbConn.State = ConnectionState.Open Then DbConn.Close()
                    SelectKey.ShowDialog()
                Loop Until (CollSeqSel <> -2)
            End If
            CurCollSeq = CollSeqSel
            If DbConn.State <> ConnectionState.Open Then DbConn.Open()
        End If
    End Sub
    Private Sub CreateSelList(ByVal s As String, ByRef CollSeqReq As Integer)
        Dim i As Integer
        Dim SelectSql As String = String.Empty
        Dim DRSelectCursor As OdbcDataReader = Nothing
        Dim DbCSelectCursor As OdbcCommand
        Dim CollSeq As Integer, FldCode As String, n As Integer
        Dim lKey As String = "", hKey As String = ""
        If CollSeqReq = 0 Then ' all entries requested,      >=: add a new entry
            s = s.Trim()
            i = s.LastIndexOf(" "c)
            If s.Substring(i + 1).ToUpper = "RANGE" Then
                s = s.Substring(0, i)
                Dim f As New GetRange
                f.ShowDialog()
                lKey = f.FromK.Text
                hKey = f.ToK.Text
            ElseIf s.Substring(i + 1).ToUpper = "ALL" Then
                s = s.Substring(0, i)
            End If
            ' s = s.Replace(","c, " "c)
            Dim t As String = ""
            For i = 0 To s.Length() - 1
                If s(i) = "," OrElse s(i) = " " Then
                    If t.Length() = 0 Then
                        t = s(i)
                    Else
                        If t(t.Length() - 1) <> " " Then
                            t = t + " "
                        End If
                    End If
                Else
                    t = t + s(i)
                End If
            Next
            SelKeyWords = t.Split()
            SelKeyDbRes = t.Split() ' alleen om juiste aantal in array te hebben!
        End If
        SelectSql = "SELECT CollSeq, FldCode, FldText FROM data WHERE CollCode = '" & CurCollCode & "' AND FldCode IN ("
        For i = 1 To SelKeyWords.Length()
            SelectSql = SelectSql & "'" & SelKeyWords(i - 1) & "'"
            If i < SelKeyWords.Length() Then
                SelectSql = SelectSql & ", "
            End If
        Next
        SelectSql = SelectSql & ")"
        If CollSeqReq > 0 Then ' add entry to existing list after insert
            SelectSql = SelectSql & " AND CollSeq = " & CStr(CollSeqReq)
        End If
        SelectSql = SelectSql & " ORDER BY CollSeq, Fldcode"
        DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
        Try
            DRSelectCursor = DbCSelectCursor.ExecuteReader()
            Do While (DRSelectCursor.Read())
                If CollSeq <> GetDbIntegerValue(DRSelectCursor, 0) Then
                    If n > 0 Then
                        addSelToList(CollSeq, lKey, hKey)
                    End If
                    For i = 1 To SelKeyWords.Length()
                        SelKeyDbRes(i - 1) = String.Empty
                    Next
                    n = 0
                End If
                CollSeq = GetDbIntegerValue(DRSelectCursor, 0)
                FldCode = GetDbStringValue(DRSelectCursor, 1)
                For i = 1 To SelKeyWords.Length()
                    If SelKeyWords(i - 1) = FldCode Then
                        SelKeyDbRes(i - 1) = GetDbStringValue(DRSelectCursor, 2)
                        n += 1
                        Exit For
                    End If
                Next
            Loop
            If n > 0 Then
                addSelToList(CollSeq, lKey, hKey)
                If CollSeqReq = 0 Then
                    CurCollSeq = 0
                    CollSeqSel = 0
                End If
            End If
        Catch e1 As Exception
            MsgBox(e1.ToString())
        Finally
            If Not (DRSelectCursor Is Nothing) Then
                DRSelectCursor.Close()
            End If
        End Try
    End Sub
    Private Sub addSelToList(ByVal collSeq As Integer, ByVal lKey As String, ByVal hKey As String)
        Dim bl As String = New String(" "c, 200)
        Dim s As String = String.Empty
        Dim selct As Boolean = True
        If lKey <> "" AndAlso SelKeyDbRes(0).ToUpper(CultInf) < lKey Then selct = False
        If hKey <> "" AndAlso SelKeyDbRes(0).ToUpper(CultInf) > hKey Then selct = False
        If selct Then
            For i As Integer = 1 To SelKeyWords.Length()
                s = s & SelKeyDbRes(i - 1) & " "
                SelKeyDbRes(i - 1) = String.Empty
            Next
            SelectKey.KeyList.Items.Add(s & bl & "|" & CStr(collSeq))
        End If
    End Sub
    Private Function SqlQuoted(ByVal s As String) As String
        Dim i As Integer
        i = InStr(s, "'")
        While i > 0
            s = s.Substring(0, i) & "'" & s.Substring(i)
            i = InStr(i + 2, s, "'")
        End While
        Return "'" & s & "'"
    End Function
    Sub getSelectedData(ByRef RexxEnv As Rexx) ' leest data in field, en maakt onderliggende zichtbaar
        Dim i, ns As Integer, na As String, FldSeqnrs(5) As Decimal
        Dim SelectSql As String = String.Empty, d As DefData, FldCode As String, FldSeqnr As Decimal
        Dim DRSelectCursor As OdbcDataReader = Nothing
        Dim DbCSelectCursor As OdbcCommand
        Dim op As Boolean = False
        SelectSql = "SELECT FldCode, FldSeqnr, FldText FROM data WHERE CollCode = '" & CurCollCode & "' AND CollSeq = " & CStr(CurCollSeq) & " AND FldCode IN ("
        For i = 1 To Defines.Count
            d = DirectCast(Defines.Item(i), DefData)
            SelectSql = SelectSql & "'" & d.Naam & "'"
            If i < Defines.Count Then
                SelectSql = SelectSql & ", "
            End If
        Next
        SelectSql = SelectSql & ") ORDER BY FldCode, FldSeqnr"
        DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
        ProgrammedChange = True
        Try
            If DbConn.State = ConnectionState.Closed Then
                op = True
                DbConn.Open()
            End If
            DRSelectCursor = DbCSelectCursor.ExecuteReader()
            Do While (DRSelectCursor.Read())
                FldCode = GetDbStringValue(DRSelectCursor, 0)
                For Each d In Defines
                    If d.Naam.Length() >= FldCode.Length() Then
                        If d.Naam.Substring(d.Naam.Length() - FldCode.Length()) = FldCode Then
                            FldCode = d.Naam
                            Exit For
                        End If
                    End If
                Next
                d = DirectCast(Defines.Item(FldCode), DefData)
                FldSeqnr = GetDbDecimalValue(DRSelectCursor, 1)
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
                If Ctls.Contains(na) Then
                    Dim cp As CtlsData = DirectCast(Ctls(na), CtlsData)
                    Dim c As Control = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
                    VisibleLabelButtonText(cp, True)
                    c.Text = GetDbStringValue(DRSelectCursor, 2)
                    cp.OldData = c.Text
                    cp.HasText = True
                    HideOrShowChildrenFields(c, False)
                Else
                    MsgBox(na & " " & SysMsg(316))
                End If
            Loop
        Catch e1 As Exception
            MsgBox(e1.ToString(), , SysMsg(327))
        Finally
            If Not (DRSelectCursor Is Nothing) Then
                DRSelectCursor.Close()
            End If
            If op Then
                DbConn.Close()
            End If
        End Try
        ProgrammedChange = False
    End Sub
#If DEBUG Then
    Sub xxx()
        Dim i, j As Integer, c As Control
        'Debug.WriteLine("_______")
        'i = 0
        'For Each c In Gr.Controls
        '    Debug.WriteLine(CStr(i) & " " & c.Name)
        '    i += 1
        'Next
        Debug.WriteLine("-- ix XinC Lev name hPar hDep Top")
        For i = 1 To Ctls.Count
            Dim cp As CtlsData = DirectCast(Ctls(i), CtlsData)
            j = cp.CtlsIx
            c = DirectCast(Gr.Controls.Item(j), Control)
            If cp.Visible Then Debug.WriteLine(CStr(cp.Ix) & " " & CStr(cp.CtlsIx) & " " & cp.CLevel & " " & c.Name & " " & cp.HasParent & " " & cp.HasDep & " " & cp.Top)
        Next
    End Sub
#End If
    Sub InitialScreen(ByVal ClrOld As Boolean) ' maakt leeg en visible: alle initieel zichtbare velden
        Dim j As Integer, ok As Boolean, cf As CtlsData, c As Control
        DoSuspendLayout()
        ' 1e op lev 0 visible; alles op lev >=1 niet, behalve alle eentjes
        For Each cf In Ctls
            cf.HadText = False
            cf.HasText = False
            cf.Expanded = True
            If ClrOld Then cf.OldData = ""
            c = DirectCast(Gr.Controls.Item(cf.CtlsIx), Control)
            If c.Text.Length() > 0 Then c.Text = ""
            ok = True
            For j = 1 To cf.CLevel ' allemaal indexen 1?
                If cf.Seq(j) > 1 Then
                    ok = False
                    Exit For
                End If
            Next
            VisibleLabelButtonText(cf, ok)
        Next
        DoResumeLayout()
    End Sub
    Sub LayoutScreen(ByVal fiFldIx As Integer) ' (her)plaatst alle visible fields op de juiste plaats van het scherm
        Dim ControlNextPosY As Integer, c As Control, cp As CtlsData
        DoSuspendLayout()
        For Each cp In Ctls
            If cp.Visible AndAlso Hidden.Contains(cp.NaamNm) Then
                VisibleLabelButtonText(cp, False)
                If fiFldIx > 0 AndAlso cp.Ix < fiFldIx Then
                    fiFldIx = cp.Ix
                End If
            End If
        Next
        If fiFldIx = 0 Then ' initial layout
            ControlNextPosY = SizeLabelHeight + 5
            FieldWithFocus = ""
        Else
            For Each cp In Ctls ' first part stays where it was
                If cp.Visible Then
                    ControlNextPosY = cp.Top
                    Exit For
                End If
            Next
        End If
        Dim j As Integer, ti As Integer = 0
        FieldsAddedDeleted = False
        FieldsAddedDeletedIx = 0
        FirstVisField = String.Empty
        For Each cp In Ctls
            If cp.Visible Then
                If cp.Ix >= fiFldIx Then
                    j = cp.CtlsIx
                    If cp.HasDep Then
                        c = DirectCast(Gr.Controls.Item(j - 2), Control) ' label
                    Else
                        c = DirectCast(Gr.Controls.Item(j - 1), Control) ' label
                    End If
                    c.Visible = True
                    cp.Top = ControlNextPosY
                    If cp.HasDep Then
                        c = DirectCast(Gr.Controls.Item(j - 1), Control) ' button
                        c.Visible = cp.HasDep
                        If cp.Expanded Then
                            c.Text = "-"
                        Else
                            c.Text = "+"
                        End If
                    End If
                    c = DirectCast(Gr.Controls.Item(j), Control) ' textbox
                    c.TabIndex = ti
                End If
                ControlNextPosY += (SizeLabelHeight + SizeTextHeight + 10)
                ti += 1
                If FirstVisField = String.Empty Then
                    FirstVisField = cp.NaamNm
                End If
                If FieldWithFocus = String.Empty Then
                    FieldWithFocus = FirstVisField
                End If
            End If
        Next
        SetTabstops()
        MoveFields(0) ' leave at their initial locations
        ProgrammedFocus = True
        cp = DirectCast(Ctls(FieldWithFocus), CtlsData)
        c = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
        If Not c.Visible Then
            c = FndNxtVisible(c)
            FieldWithFocus = c.Name.Substring(2)
        End If
        Dim s As String = FieldWithFocus
        FieldWithFocus = String.Empty
        SetFocus(s)
        ProgrammedFocus = False
        DoResumeLayout()
    End Sub
    Private Sub SetFocus(ByVal FieldWithNewFocus As String)
        Dim c As Control, cp As CtlsData
        cp = DirectCast(Ctls(FieldWithNewFocus), CtlsData)
        c = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
        If FieldWithNewFocus <> FieldWithFocus Then
            FieldWithFocus = FieldWithNewFocus
            c.Focus()
            ShowField(c)
        End If
    End Sub
    Sub SaveScreen()
        Dim CollSeq As Integer
        If DbConn.State = ConnectionState.Closed Then
            Try
                DbConn.Open()
            Catch ex As Exception
                MsgBox(ex.ToString(), , SysMsg(66))
            End Try
        End If
        Form1.Idxen.Visible = True
        If TypeEntry = "E" Or CurCollSeq = -1 Then
            Try
                mSelP1.Value = CurCollCode
                DRmSelect = DbCmSelect.ExecuteReader()
                If DRmSelect.Read() Then
                    Try
                        CollSeq = GetDbIntegerValue(DRmSelect, 0) + 1
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
        Else
            CollSeq = CurCollSeq
        End If
        InsXP1.Value = CurCollCode
        InsXP2.Value = CollSeq
        Try
            DRXInsert = DbCXInsert.ExecuteReader()
        Catch e As OdbcException
            Dim i As Integer = 0 ' ignore , probably duplicate
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(318))
        Finally
            If Not (DRXInsert Is Nothing) Then
                DRXInsert.Close()
            End If
        End Try
        For ic As Integer = 1 To Ctls.Count
            Dim cp As CtlsData = DirectCast(Ctls(ic), CtlsData)
            Dim c As Control = DirectCast(Gr.Controls.Item(cp.CtlsIx), Control)
            If cp.Naam(0) <> "#"c AndAlso c.Text <> cp.OldData Then
                Dim FldCode As String, FldCodeExi As Boolean
                Dim FldSeqnr As Decimal, i As Integer
                Dim s As String = cp.NaamNm
                i = InStr(s, ".")
                FldCode = s.Substring(0, i - 1).TrimEnd
                s = s.Substring(i)
                FldSeqnr = 0
                i = InStr(s, ".")
                While i > 0
                    FldSeqnr = FldSeqnr * 1000 + CDec(s.Substring(0, i))
                    s = s.Substring(i)
                    i = InStr(s, ".")
                End While
                FldSeqnr = FldSeqnr * 1000 + CDec(s.Substring(i))
                If TypeEntry <> "E" Then ' in correction mode
                    Dim NrErr As Integer = 0
ReTrySql1:          ExiP1.Value = CurCollCode
                    ExiP2.Value = CollSeq
                    ExiP3.Value = FldCode
                    ExiP4.Value = FldSeqnr
                    Try
                        DRExists = DbCExists.ExecuteReader()
                        FldCodeExi = DRExists.Read()
                        If FldCodeExi Then
                            UpdP1.Value = GetDbStringValue(DRExists, 0)
                        End If
                    Catch e As OdbcException
                        If e.Message.Substring(0, 13) = "ERROR [07006]" Then ' ODBC error on mdb databases
                            If NrErr = 0 Then
                                NrErr += 1
                                GoTo ReTrySql1
                            End If
                        End If
                    Catch ex As Exception
                        MsgBox(ex.ToString(), , SysMsg(319))
                    Finally
                        If Not (DRExists Is Nothing) Then
                            DRExists.Close()
                        End If
                    End Try
                Else
                    FldCodeExi = False
                End If
                If TypeEntry = "E" Or Not FldCodeExi Then ' in input mode or new entity
                    Dim NrErr As Integer = 0
ReTrySql2:          InsP1.Value = CurCollCode
                    InsP2.Value = CollSeq
                    InsP3.Value = FldCode
                    InsP4.Value = FldSeqnr
                    InsP5.Value = c.Text
                    Try
                        DRInsert = DbCInsert.ExecuteReader()
                    Catch e As OdbcException
                        If e.Message.Substring(0, 13) = "ERROR [07006]" Then
                            If NrErr = 0 Then
                                NrErr += 1
                                GoTo ReTrySql2
                            End If
                        End If
                    Catch ex As Exception
                        MsgBox(ex.ToString(), , SysMsg(320))
                    Finally
                        If Not (DRInsert Is Nothing) Then
                            DRInsert.Close()
                        End If
                    End Try
                Else
                    If CStr(UpdP1.Value) <> c.Text Then
                        If c.Text.Length() > 0 Then
                            Dim NrErr As Integer = 0
ReTrySql3:                  UpdP1.Value = c.Text
                            UpdP2.Value = CurCollCode
                            UpdP3.Value = CollSeq
                            UpdP4.Value = FldCode
                            UpdP5.Value = FldSeqnr
                            Try
                                DRUpdate = DbCUpdate.ExecuteReader()
                            Catch e As OdbcException
                                If e.Message.Substring(0, 13) = "ERROR [07006]" Then
                                    If NrErr = 0 Then
                                        NrErr += 1
                                        GoTo ReTrySql3
                                    End If
                                End If
                            Catch ex As Exception
                                MsgBox(ex.ToString(), , SysMsg(321))
                            Finally
                                If Not (DRUpdate Is Nothing) Then
                                    DRUpdate.Close()
                                End If
                            End Try
                        Else
                            DelP1.Value = CurCollCode
                            DelP2.Value = CollSeq
                            DelP3.Value = FldCode
                            DelP4.Value = FldSeqnr
                            Try
                                DRDelete = DbCDelete.ExecuteReader()
                            Catch ex As Exception
                                MsgBox(ex.ToString(), , SysMsg(322))
                            Finally
                                If Not (DRDelete Is Nothing) Then
                                    DRDelete.Close()
                                End If
                            End Try
                        End If
                    End If
                End If
            End If
        Next
        If CurCollSeq = -1 Then
            GetOrAddSelection("", CollSeq)
        End If
        DbConn.Close()
    End Sub
    Private Function RxExe(ByVal s As String) As Integer
        If Not RexxActive Then
            Dim r As Integer = Rx.ExecuteRexxScript(s)
            If LayoutChangedIx > 0 Then
                LayoutScreen(LayoutChangedIx)
                LayoutChangedIx = 0
            End If
            Return r
        Else
            MsgBox(SysMsg1p(323, s), MsgBoxStyle.OkOnly)
            Return 255
        End If
    End Function
    Private Sub SelKey()
        ProgrammedChange = True
        Rx.ExecuteRexxScript(TypeEntry & " K")
        ProgrammedChange = False
    End Sub
    Private Sub BSelKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSelKey.Click
        DoSuspendLayout()
        CheckChanged()
        SelKey()
        DoResumeLayout()
    End Sub
    Private Sub SelKeyNxt()
        SelectKey.KeyList_Next()
        CurCollSeq = CollSeqSel
        ProgrammedChange = True
        Rx.ExecuteRexxScript(TypeEntry & " N")
        ProgrammedChange = False
    End Sub
    Private Sub BNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BNext.Click
        DoSuspendLayout()
        CheckChanged()
        SelKeyNxt()
        DoResumeLayout()
    End Sub
End Class

