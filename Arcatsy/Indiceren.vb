'
Imports RxLib.Rexx
Public Class Indiceren
    Dim WithEvents Rx As New Rexx(New RexxCompData)
    Dim RxCompiled As Boolean = False
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim DbConn2 As New OdbcConnection(SqlProv)
    Dim CurCollCode As String
    Private Sub Indiceren_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not (DbConn Is Nothing) Then
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End If
    End Sub
    Private Sub Indiceren_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = SysMsg(410)
        Me.DesktopLocation = New Point(0, 0)
        Me.Show()
        indx()
    End Sub
    Sub indx()
        Dim RegExpKenmerkC As New Collection
        Dim Src As New Collection
        Dim TwOldC As New Collection
        Dim TwNewC As New Collection
        Dim CollSeqToIdxC As New Collection
        Dim FldTextDataC As New Collection
        Dim mc As MatchCollection
        Dim i As Integer, s As String = "", CollSeq As Integer, Sqs As String
        Dim execName As String = String.Empty, srcName As String = String.Empty
        Dim cvr As DefVariable = Nothing

        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try

        ' haal regexp text op
        Dim SQLreSelect As String = "SELECT FldCode, FldTermType, FldTermRegexp, FldXExit FROM collfields WHERE CollCode = ? AND (FldTermRegexp <> '' OR FldXExit = true)"
        Dim DRreSelect As OdbcDataReader = Nothing
        Dim DCreSelect As OdbcCommand
        Dim reSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        DCreSelect = New OdbcCommand(SQLreSelect, DbConn)
        DCreSelect.Parameters.Add(reSelP1)
        Dim rData As RexExpRec

        ' haal CollSeq op van te indexeren records
        Dim SQLsfSelect As String = "SELECT CollSeq FROM tobeindexed WHERE CollCode = ?"
        Dim DRsfSelect As OdbcDataReader = Nothing
        Dim DCsfSelect As OdbcCommand
        Dim sfSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        DCsfSelect = New OdbcCommand(SQLsfSelect, DbConn)
        DCsfSelect.Parameters.Add(sfSelP1)

        ' haal alle CollSeq op van een Collection
        Dim SQLasSelect As String = "SELECT DISTINCT CollSeq FROM data WHERE CollCode = ? ORDER BY CollSeq"
        Dim DRasSelect As OdbcDataReader = Nothing
        Dim DCasSelect As OdbcCommand
        Dim asSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        DCasSelect = New OdbcCommand(SQLasSelect, DbConn)
        DCasSelect.Parameters.Add(asSelP1)

        ' haal alle CollSeq op van een Collection uit indexen
        Dim SQLaxSelect As String = "SELECT DISTINCT CollSeq FROM searchterms WHERE CollCode = ? ORDER BY CollSeq"
        Dim DRaxSelect As OdbcDataReader = Nothing
        Dim DCaxSelect As OdbcCommand
        Dim axSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        DCaxSelect = New OdbcCommand(SQLaxSelect, DbConn)
        DCaxSelect.Parameters.Add(axSelP1)

        ' haal tw oud op van een CollSeq
        Dim SQLtoSelect As String = "SELECT TermType, TermText FROM searchterms WHERE CollCode = ? AND CollSeq = ?"
        Dim DRtoSelect As OdbcDataReader = Nothing
        Dim DCtoSelect As OdbcCommand
        Dim toSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        Dim toSelP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
        DCtoSelect = New OdbcCommand(SQLtoSelect, DbConn)
        DCtoSelect.Parameters.Add(toSelP1)
        DCtoSelect.Parameters.Add(toSelP2)
        Dim toData As TwInDb

        ' haal alle kenmerkteksten op van een CollSeq
        Dim SQLkmSelect As String = "SELECT FldCode, FldText FROM data WHERE CollCode = ? AND CollSeq = ?"
        Dim DRkmSelect As OdbcDataReader = Nothing
        Dim DCkmSelect As OdbcCommand
        Dim kmSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        Dim kmSelP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
        DCkmSelect = New OdbcCommand(SQLkmSelect, DbConn)
        DCkmSelect.Parameters.Add(kmSelP1)
        DCkmSelect.Parameters.Add(kmSelP2)
        Dim kmData As TextInDb

        ' insert een trefwoord referentie
        Dim SQLTwInsert As String = "INSERT INTO searchterms (CollCode, CollSeq, TermType, TermText) VALUES (?, ?, ?, ?)"
        Dim DRTwInsert As OdbcDataReader = Nothing
        Dim DCCTwInsert As OdbcCommand
        Dim TwInsP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        Dim TwInsP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
        Dim TwInsP3 As New OdbcParameter("@TermType", Odbc.OdbcType.VarChar, 4)
        Dim TwInsP4 As New OdbcParameter("@TermText", Odbc.OdbcType.VarChar, 255)
        DCCTwInsert = New OdbcCommand(SQLTwInsert, DbConn)
        DCCTwInsert.Parameters.Add(TwInsP1)
        DCCTwInsert.Parameters.Add(TwInsP2)
        DCCTwInsert.Parameters.Add(TwInsP3)
        DCCTwInsert.Parameters.Add(TwInsP4)

        ' Delete een trefwoord referentie
        Dim SQLTwDelete As String = "DELETE FROM searchterms WHERE CollCode = ? AND CollSeq = ? AND TermType = ? AND TermText = ?"
        Dim DRTwDelete As OdbcDataReader = Nothing
        Dim DCCTwDelete As OdbcCommand
        Dim TwDelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        Dim TwDelP2 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
        Dim TwDelP3 As New OdbcParameter("@TermType", Odbc.OdbcType.VarChar, 4)
        Dim TwDelP4 As New OdbcParameter("@TermText", Odbc.OdbcType.VarChar, 255)
        DCCTwDelete = New OdbcCommand(SQLTwDelete, DbConn)
        DCCTwDelete.Parameters.Add(TwDelP1)
        DCCTwDelete.Parameters.Add(TwDelP2)
        DCCTwDelete.Parameters.Add(TwDelP3)
        DCCTwDelete.Parameters.Add(TwDelP4)

        ' Update een trefwoord referentie
        Dim SQLTwUpdate As String = "UPDATE searchterms SET TermType = ?, TermText = ? WHERE CollCode = ? AND CollSeq = ? AND TermType = ? AND TermText = ?"
        Dim DRTwUpdate As OdbcDataReader = Nothing
        Dim DCCTwUpdate As OdbcCommand
        Dim TwUpdP1 As New OdbcParameter("@TermType", Odbc.OdbcType.VarChar, 4)
        Dim TwUpdP2 As New OdbcParameter("@TermText", Odbc.OdbcType.VarChar, 255)
        Dim TwUpdP3 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        Dim TwUpdP4 As New OdbcParameter("@CollSeq", Odbc.OdbcType.Int, 4)
        Dim TwUpdP5 As New OdbcParameter("@oldTermType", Odbc.OdbcType.VarChar, 4)
        Dim TwUpdP6 As New OdbcParameter("@oldTermText", Odbc.OdbcType.VarChar, 255)
        DCCTwUpdate = New OdbcCommand(SQLTwUpdate, DbConn)
        DCCTwUpdate.Parameters.Add(TwUpdP1)
        DCCTwUpdate.Parameters.Add(TwUpdP2)
        DCCTwUpdate.Parameters.Add(TwUpdP3)
        DCCTwUpdate.Parameters.Add(TwUpdP4)
        DCCTwUpdate.Parameters.Add(TwUpdP5)
        DCCTwUpdate.Parameters.Add(TwUpdP6)

        ' Delete de "indiceren" opdracht
        Dim SQLIxDelete As String = "DELETE FROM tobeindexed WHERE CollCode = ? "
        Dim DRIxDelete As OdbcDataReader = Nothing
        Dim DCCIxDelete As OdbcCommand
        Dim IxDelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)
        DCCIxDelete = New OdbcCommand(SQLIxDelete, DbConn)
        DCCIxDelete.Parameters.Add(IxDelP1)

        Me.Cursor = Cursors.WaitCursor
        Dim nCollsToDo As Integer = 0
        For Each Sfile As String In Form1.CollNames.SelectedItems
            nCollsToDo += 1
        Next
        For Each Sfile As String In Form1.CollNames.SelectedItems
            Label1.Text = Sfile.Split("|"c)(0).Trim
            CurCollCode = Sfile.Split("|"c)(1).Trim
            Label2.Text = Sfile
            Me.Refresh()
            ' haal regexp voor tw voor Collection
            reSelP1.Value = CurCollCode
            RegExpKenmerkC.Clear()
            Try
                DRreSelect = DCreSelect.ExecuteReader()
                Do While (DRreSelect.Read())
                    rData = New RexExpRec()
                    rData.FldTermType = GetDbStringValue(DRreSelect, 1)
                    rData.kmRegExp = GetDbStringValue(DRreSelect, 2)
                    rData.kmRexx = GetDbBooleanValue(DRreSelect, 3)
                    RegExpKenmerkC.Add(rData, GetDbStringValue(DRreSelect, 0))
                Loop
            Catch ex As Exception
                MsgBox(ex.ToString(), , SysMsg(401))
            Finally
                If Not (DRreSelect Is Nothing) Then
                    DRreSelect.Close()
                End If
            End Try
            ' haal alle te indiceren sf nrs op
            sfSelP1.Value = CurCollCode
            CollSeqToIdxC.Clear()
            Try
                DRsfSelect = DCsfSelect.ExecuteReader()
                Do While (DRsfSelect.Read())
                    CollSeq = GetDbIntegerValue(DRsfSelect, 0)
                    If CollSeq > 0 Then
                        CollSeqToIdxC.Add(CollSeq, CStr(CollSeq))
                    Else ' alle nummers van de Collection
                        DbConn2.Open()
                        asSelP1.Value = CurCollCode
                        Try
                            DRasSelect = DCasSelect.ExecuteReader()
                            Do While (DRasSelect.Read())
                                CollSeq = GetDbIntegerValue(DRasSelect, 0)
                                CollSeqToIdxC.Add(CollSeq, CStr(CollSeq))
                            Loop
                        Catch ex As Exception
                            MsgBox(ex.ToString(), , SysMsg1p(402, CurCollCode))
                        Finally
                            If Not (DRasSelect Is Nothing) Then
                                DRasSelect.Close()
                            End If
                        End Try
                        axSelP1.Value = CurCollCode ' en uit bestaande indexen
                        Try
                            DRaxSelect = DCaxSelect.ExecuteReader()
                            Do While (DRaxSelect.Read())
                                CollSeq = GetDbIntegerValue(DRaxSelect, 0)
                                Sqs = CStr(CollSeq)
                                If Not CollSeqToIdxC.Contains(Sqs) Then
                                    CollSeqToIdxC.Add(CollSeq, Sqs)
                                End If
                            Loop
                        Catch ex As Exception
                            MsgBox(ex.ToString(), , SysMsg1p(402, CurCollCode))
                        Finally
                            If Not (DRaxSelect Is Nothing) Then
                                DRaxSelect.Close()
                            End If
                        End Try
                        DbConn2.Close()
                    End If
                Loop
            Catch ex As Exception
                MsgBox(ex.ToString(), , SysMsg1p(403, CurCollCode))
            Finally
                If Not (DRsfSelect Is Nothing) Then
                    DRsfSelect.Close()
                End If
            End Try
            Form1.Vltd.Visible = True
            Form1.Vltd.Maximum = CollSeqToIdxC.Count
            Form1.Vltd.Value = 0
            For Each CollSeq In CollSeqToIdxC
                Form1.Vltd.Value += 1
                ' Lees alle bestaande trefwoorden van een CollSeq 
                toSelP1.Value = CurCollCode
                toSelP2.Value = CollSeq
                TwOldC.Clear()
                Try
                    DRtoSelect = DCtoSelect.ExecuteReader()
                    Do While (DRtoSelect.Read())
                        toData = New TwInDb
                        toData.FldTermTypec = GetDbStringValue(DRtoSelect, 0)
                        toData.FldTermTypet = GetDbStringValue(DRtoSelect, 1)
                        TwOldC.Add(toData, toData.ToString)
                    Loop
                Catch ex As Exception
                    MsgBox(ex.ToString(), , SysMsg1p(404, CurCollCode & " " & CStr(CollSeq)))
                Finally
                    If Not (DRtoSelect Is Nothing) Then
                        DRtoSelect.Close()
                    End If
                End Try
                ' lees alle bestaande data-teksten te indiceren
                kmSelP1.Value = CurCollCode
                kmSelP2.Value = CollSeq
                FldTextDataC.Clear()
                Try
                    DRkmSelect = DCkmSelect.ExecuteReader()
                    Do While (DRkmSelect.Read())
                        kmData = New TextInDb
                        kmData.FldCode = GetDbStringValue(DRkmSelect, 0)
                        kmData.FldText = GetDbStringValue(DRkmSelect, 1)
                        FldTextDataC.Add(kmData)
                    Loop
                Catch ex As Exception
                    MsgBox(ex.ToString(), , SysMsg1p(405, CurCollCode & " " & CStr(CollSeq)))
                Finally
                    If Not (DRkmSelect Is Nothing) Then
                        DRkmSelect.Close()
                    End If
                End Try
                TwNewC.Clear()
                For Each km As TextInDb In FldTextDataC
                    ' match tekst in data tegen regexp
                    If RegExpKenmerkC.Contains(km.FldCode) Then
                        rData = DirectCast(RegExpKenmerkC.Item(km.FldCode), RexExpRec)
                        If Not rData.kmRexx Then
                            Dim regExpre As New Regex(rData.kmRegExp)
                            mc = regExpre.Matches(km.FldText)
                            For Each m As Match In mc
                                ' nieuw trefwoord opslaan
                                If m.Length() > 0 AndAlso m.Value.Trim().Length() > 0 Then
                                    toData = New TwInDb
                                    toData.FldTermTypec = rData.FldTermType
                                    toData.FldTermTypet = m.Value.Trim().ToUpper(CultInf)
                                    s = toData.ToString
                                    If Not TwNewC.Contains(s) Then
                                        TwNewC.Add(toData, s)
                                    End If
                                End If
                            Next
                        Else
                            If Not RxCompiled Then
                                If Rx.CompileRexxScript(My.Application.Info.DirectoryPath & "\TERMS") <> 0 Then
                                    MsgBox(SysMsg(306))
                                    Exit Sub
                                End If
                                RxCompiled = True
                            End If
                            If Rx.ExecuteRexxScript("X " & CurCollCode & " " & km.FldCode & " " & km.FldText) = 0 Then
                                Dim nrt As Integer = CInt(Rx.GetVar(Rx.SourceNameIndexPosition("T.0", Rexx.tpSymbol.tpVariable, cvr), execName, srcName))
                                For i = 1 To nrt
                                    Dim rt As String = Rx.GetVar(Rx.SourceNameIndexPosition("T." & CStr(i), Rexx.tpSymbol.tpVariable, cvr), execName, srcName)
                                    If rt.Length() > 0 Then
                                        toData = New TwInDb
                                        toData.FldTermTypec = rData.FldTermType
                                        toData.FldTermTypet = rt.ToUpper(CultInf)
                                        s = toData.ToString
                                        If Not TwNewC.Contains(s) Then
                                            TwNewC.Add(toData, s)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
                For i = TwNewC.Count To 1 Step -1
                    If TwOldC.Contains(TwNewC.Item(i).ToString) Then
                        TwOldC.Remove(TwNewC.Item(i).ToString)
                        TwNewC.Remove(i)
                    End If
                Next
                For i = 1 To TwNewC.Count
                    If i <= TwOldC.Count Then
                        toData = DirectCast(TwNewC.Item(i), TwInDb)
                        TwUpdP1.Value = toData.FldTermTypec
                        TwUpdP2.Value = toData.FldTermTypet
                        TwUpdP3.Value = CurCollCode
                        TwUpdP4.Value = CollSeq
                        toData = DirectCast(TwOldC.Item(i), TwInDb)
                        TwUpdP5.Value = toData.FldTermTypec
                        TwUpdP6.Value = toData.FldTermTypet
                        Try
                            DRTwUpdate = DCCTwUpdate.ExecuteReader()
                        Catch ex As Exception
                            MsgBox(ex.ToString(), , SysMsg(406))
                        Finally
                            If Not (DRTwUpdate Is Nothing) Then
                                DRTwUpdate.Close()
                            End If
                        End Try
                    Else
                        TwInsP1.Value = CurCollCode
                        TwInsP2.Value = CollSeq
                        toData = DirectCast(TwNewC.Item(i), TwInDb)
                        TwInsP3.Value = toData.FldTermTypec
                        TwInsP4.Value = toData.FldTermTypet
                        Try
                            DRTwInsert = DCCTwInsert.ExecuteReader()
                        Catch ex As Exception
                            MsgBox(ex.ToString(), , SysMsg(407))
                        Finally
                            If Not (DRTwInsert Is Nothing) Then
                                DRTwInsert.Close()
                            End If
                        End Try
                    End If
                Next
                For i = 1 To TwOldC.Count
                    If i > TwNewC.Count Then
                        TwDelP1.Value = CurCollCode
                        TwDelP2.Value = CollSeq
                        toData = DirectCast(TwOldC.Item(i), TwInDb)
                        TwDelP3.Value = toData.FldTermTypec
                        TwDelP4.Value = toData.FldTermTypet
                        Try
                            DRTwDelete = DCCTwDelete.ExecuteReader()
                        Catch ex As Exception
                            MsgBox(ex.ToString(), , SysMsg(408))
                        Finally
                            If Not (DRTwDelete Is Nothing) Then
                                DRTwDelete.Close()
                            End If
                        End Try
                    End If
                Next
            Next
            'delete de Collection nu net afgehandeld in "te indiceren"
            IxDelP1.Value = CurCollCode
            Try
                DRIxDelete = DCCIxDelete.ExecuteReader()
            Catch ex As Exception
                MsgBox(ex.ToString(), , SysMsg1p(409, CurCollCode))
            Finally
                If Not (DRIxDelete Is Nothing) Then
                    DRIxDelete.Close()
                End If
            End Try
        Next
        DbConn.Close()
        Form1.Vltd.Visible = False
        Form1.Vltd.Value = 0
        Label1.Text = ""
        Label2.Text = ""
        Me.Cursor = Cursors.Default
        Me.Close()
    End Sub
End Class
Friend Class RexExpRec
    Friend FldTermType As String
    Friend kmRegExp As String
    Friend kmRexx As Boolean
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()> _
    Public Overrides Function ToString() As String
        Return FldTermType & "|" & kmRegExp
    End Function
End Class
Friend Class TwInDb
    Friend FldTermTypec As String
    Friend FldTermTypet As String
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()> _
    Public Overrides Function ToString() As String
        Return FldTermTypec & "|" & FldTermTypet
    End Function
End Class
Friend Class TextInDb
    Friend FldCode As String
    Friend FldText As String
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()> _
    Public Overrides Function ToString() As String
        Return FldCode & "|" & FldText
    End Function
End Class
