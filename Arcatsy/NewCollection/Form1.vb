Imports System.Data.Odbc

Public Class Form1
    Dim SysConst As New Collection
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim SqlProv As String
        SqlProv = SysMsg(208)
        Dim DBConn As New OdbcConnection(SqlProv)
        DBConn.Open()
        Dim i As Integer, m, n As String
        If TCollCode.Text.Trim() <> "" AndAlso TCollName.Text.Trim() <> "" Then
            TCollCode.Text = TCollCode.Text.Trim()
            Dim dirs As String() = Directory.GetFiles(My.Application.Info.DirectoryPath & "\", "DE TMPL*.rex")
            For i = 0 To dirs.Length() - 1
                m = Path.GetFileName(dirs(i))
                n = "DE " & TCollCode.Text & m.Substring(7)
                File.Copy(dirs(i), My.Application.Info.DirectoryPath & "\" & n, True)
            Next
            ' haal hoogste CollSeq op

            Dim mSelSQL As String = "SELECT MAX(CollSeq) FROM Data WHERE CollCode = ?"
            Dim DRmSel As OdbcDataReader = Nothing
            Dim DBCmSel As OdbcCommand
            Dim mSelP1 As New OdbcParameter("@CollCode", OdbcType.VarChar, 4)
            DBCmSel = New OdbcCommand(mSelSQL, DBConn)
            DBCmSel.Parameters.Add(mSelP1)
            Dim CollSeq As Integer
            Try
                mSelP1.Value = "SURV"
                DRmSel = DBCmSel.ExecuteReader()
                If DRmSel.Read() Then
                    Try
                        CollSeq = GetDbIntegerValue(DRmSel, 0) + 1
                    Catch ex As Exception
                        CollSeq = 1
                    End Try
                End If
            Catch ex As Exception
                MsgBox(ex.ToString())
            Finally
                If Not (DRmSel Is Nothing) Then
                    DRmSel.Close()
                End If
            End Try
            ' insert SURV entries voor code en naam

            Dim svInsSQL As String = "INSERT INTO data (CollCode, CollSeq, FldCode, FldSeqnr, FldText) VALUES (?, ?, ?, ?, ?)"
            Dim DRsvIns As OdbcDataReader = Nothing
            Dim DBCsvIns As OdbcCommand
            DBCsvIns = New OdbcCommand(svInsSQL, DBConn)
            Dim svInsP1 As New OdbcParameter("@CollCode", OdbcType.VarChar, 4)
            Dim svInsP2 As New OdbcParameter("@CollSeq", OdbcType.Int, 4)
            Dim svInsP3 As New OdbcParameter("@FldCode", OdbcType.VarChar, 4)
            Dim svInsP4 As New OdbcParameter("@FldSeqnr", OdbcType.Decimal, 16)
            Dim svInsP5 As New OdbcParameter("@FldText", OdbcType.VarChar, 255)
            DBCsvIns.Parameters.Add(svInsP1)
            DBCsvIns.Parameters.Add(svInsP2)
            DBCsvIns.Parameters.Add(svInsP3)
            DBCsvIns.Parameters.Add(svInsP4)
            DBCsvIns.Parameters.Add(svInsP5)
            svInsP1.Value = "SURV"
            svInsP2.Value = CollSeq
            svInsP3.Value = "sf"
            svInsP4.Value = 1
            svInsP5.Value = TCollCode.Text
            Try
                DRsvIns = DBCsvIns.ExecuteReader()
            Catch ex As Exception
                MsgBox(ex.ToString())
            Finally
                If Not (DRsvIns Is Nothing) Then
                    DRsvIns.Close()
                End If
            End Try
            svInsP1.Value = "SURV"
            svInsP2.Value = CollSeq
            svInsP3.Value = "na"
            svInsP4.Value = 1
            svInsP5.Value = TCollName.Text
            Try
                DRsvIns = DBCsvIns.ExecuteReader()
            Catch ex As Exception
                MsgBox(ex.ToString())
            Finally
                If Not (DRsvIns Is Nothing) Then
                    DRsvIns.Close()
                End If
            End Try
            ' insert collectie
            Dim clInsSQL As String = "INSERT INTO Collections(CollCode, CollName, DefPrtSpec, EntitySelFields) VALUES(?, ?, ?, ?)"
            Dim DRclIns As OdbcDataReader = Nothing
            Dim DBCclIns As OdbcCommand
            DBCclIns = New OdbcCommand(clInsSQL, DBConn)
            Dim clInsP1 As New OdbcParameter("@CollCode", OdbcType.VarChar, 4)
            Dim clInsP2 As New OdbcParameter("@CollName", OdbcType.VarChar, 100)
            Dim clInsP3 As New OdbcParameter("@DefPrtSpec", OdbcType.VarChar, 50)
            Dim clInsP4 As New OdbcParameter("@EntitySelFields", OdbcType.VarChar, 50)
            DBCclIns.Parameters.Add(clInsP1)
            DBCclIns.Parameters.Add(clInsP2)
            DBCclIns.Parameters.Add(clInsP3)
            DBCclIns.Parameters.Add(clInsP4)
            clInsP1.Value = TCollCode.Text
            clInsP2.Value = TCollName.Text
            clInsP3.Value = "Std"
            clInsP4.Value = "rv,in,nr"
            Try
                DRclIns = DBCclIns.ExecuteReader()
            Catch ex As Exception
                MsgBox(ex.ToString())
            Finally
                If Not (DRclIns Is Nothing) Then
                    DRclIns.Close()
                End If
            End Try
        End If
        ' copy definitie van collectie

        Dim cfSelSQL As String = "SELECT CollCode, FldCode, FldCaption, FldParent, FldInputOrder, FldMaxLength, FldMaxOccurs, FldCheckRegexp, FldCheckErrmsg, FldTermRegexp, FldTermType, FldCombo, FldPExit, FldSExit, FldXExit, FldQExit FROM CollFields WHERE CollCode = 'TMPL'"
        Dim DRcfSel As OdbcDataReader = Nothing
        Dim DBCcfSel As OdbcCommand
        DBCcfSel = New OdbcCommand(cfSelSQL, DBConn)
        Dim fs As New Collection
        Try
            DRcfSel = DBCcfSel.ExecuteReader()
            While DRcfSel.Read()
                Dim f As New flds
                f.P1 = TCollCode.Text
                f.P2 = GetDbStringValue(DRcfSel, 1)
                f.P3 = GetDbStringValue(DRcfSel, 2)
                f.P4 = GetDbStringValue(DRcfSel, 3)
                f.P5 = GetDbIntegerValue(DRcfSel, 4)
                f.P6 = GetDbIntegerValue(DRcfSel, 5)
                f.P7 = GetDbIntegerValue(DRcfSel, 6)
                f.P8 = GetDbStringValue(DRcfSel, 7)
                f.P9 = GetDbStringValue(DRcfSel, 8)
                f.P10 = GetDbStringValue(DRcfSel, 9)
                f.P11 = GetDbStringValue(DRcfSel, 10)
                f.P12 = GetDbStringValue(DRcfSel, 11)
                f.P13 = GetDbBooleanValue(DRcfSel, 12)
                f.P14 = GetDbBooleanValue(DRcfSel, 13)
                f.P15 = GetDbBooleanValue(DRcfSel, 14)
                f.P16 = GetDbBooleanValue(DRcfSel, 15)
                fs.Add(f)
            End While
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRcfSel Is Nothing) Then
                DRcfSel.Close()
            End If
        End Try
        Dim cfInsSQL As String = "INSERT INTO CollFields(CollCode, FldCode, FldCaption, FldParent, FldInputOrder, FldMaxLength, FldMaxOccurs, FldCheckRegexp, FldCheckErrmsg, FldTermRegexp, FldTermType, FldCombo, FldPExit, FldSExit, FldXExit, FldQExit) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
        Dim DRcfIns As OdbcDataReader = Nothing
        Dim DBCcfIns As OdbcCommand
        Dim cfInsP1 As New OdbcParameter("@CollCode", OdbcType.VarChar, 4)
        Dim cfInsP2 As New OdbcParameter("@FldCode", OdbcType.VarChar, 4)
        Dim cfInsP3 As New OdbcParameter("@FldCaption", OdbcType.VarChar, 50)
        Dim cfInsP4 As New OdbcParameter("@FldParent", OdbcType.VarChar, 4)
        Dim cfInsP5 As New OdbcParameter("@FldInputOrder", OdbcType.Int, 4)
        Dim cfInsP6 As New OdbcParameter("@FldMaxLength", OdbcType.Int, 4)
        Dim cfInsP7 As New OdbcParameter("@FldMaxOccurs", OdbcType.Int, 4)
        Dim cfInsP8 As New OdbcParameter("@FldCheckRegexp", OdbcType.VarChar, 255)
        Dim cfInsP9 As New OdbcParameter("@FldCheckErrmsg", OdbcType.VarChar, 80)
        Dim cfInsP10 As New OdbcParameter("@FldTermRegexp", OdbcType.VarChar, 255)
        Dim cfInsP11 As New OdbcParameter("@FldTermType", OdbcType.VarChar, 4)
        Dim cfInsP12 As New OdbcParameter("@FldCombo", OdbcType.VarChar, 4)
        Dim cfInsP13 As New OdbcParameter("@FldPExit", OdbcType.Bit, 4)
        Dim cfInsP14 As New OdbcParameter("@FldSExit", OdbcType.Bit, 4)
        Dim cfInsP15 As New OdbcParameter("@FldXExit", OdbcType.Bit, 4)
        Dim cfInsP16 As New OdbcParameter("@FldQExit", OdbcType.Bit, 4)
        DBCcfIns = New OdbcCommand(cfInsSQL, DBConn)
        DBCcfIns.Parameters.Add(cfInsP1)
        DBCcfIns.Parameters.Add(cfInsP2)
        DBCcfIns.Parameters.Add(cfInsP3)
        DBCcfIns.Parameters.Add(cfInsP4)
        DBCcfIns.Parameters.Add(cfInsP5)
        DBCcfIns.Parameters.Add(cfInsP6)
        DBCcfIns.Parameters.Add(cfInsP7)
        DBCcfIns.Parameters.Add(cfInsP8)
        DBCcfIns.Parameters.Add(cfInsP9)
        DBCcfIns.Parameters.Add(cfInsP10)
        DBCcfIns.Parameters.Add(cfInsP11)
        DBCcfIns.Parameters.Add(cfInsP12)
        DBCcfIns.Parameters.Add(cfInsP13)
        DBCcfIns.Parameters.Add(cfInsP14)
        DBCcfIns.Parameters.Add(cfInsP15)
        DBCcfIns.Parameters.Add(cfInsP16)
        For j As Integer = 1 To fs.Count
            Dim f As flds = DirectCast(fs.Item(j), flds)
            cfInsP1.Value = f.P1
            cfInsP2.Value = f.P2
            cfInsP3.Value = f.P3
            cfInsP4.Value = f.P4
            cfInsP5.Value = f.P5
            cfInsP6.Value = f.P6
            cfInsP7.Value = f.P7
            cfInsP8.Value = f.P8
            cfInsP9.Value = f.P9
            cfInsP10.Value = f.P10
            cfInsP11.Value = f.P11
            cfInsP12.Value = f.P12
            cfInsP13.Value = f.P13
            cfInsP14.Value = f.P14
            cfInsP15.Value = f.P15
            cfInsP16.Value = f.P16
            Try
                DRcfIns = DBCcfIns.ExecuteReader()
            Catch ex As Exception
                MsgBox(ex.ToString())
            Finally
                If Not (DRcfIns Is Nothing) Then
                    DRcfIns.Close()
                End If
            End Try
        Next
        ' copy printspecs
        Dim psSelSQL As String = "SELECT PrtSpec FROM PrtSpecs WHERE CollCode = 'TMPL'"
        Dim DRpsSel As OdbcDataReader = Nothing
        Dim DBCpsSel As OdbcCommand
        DBCpsSel = New OdbcCommand(psSelSQL, DBConn)
        Dim ps As New Collection
        Try
            DRpsSel = DBCpsSel.ExecuteReader()
            While DRpsSel.Read()
                Dim p As New prts
                p.P1 = GetDbStringValue(DRpsSel, 0)
                p.P2 = TCollCode.Text
                ps.Add(p)
            End While
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRpsSel Is Nothing) Then
                DRpsSel.Close()
            End If
        End Try

        Dim psInsSQL As String = "INSERT INTO PrtSpecs(PrtSpec, CollCode) VALUES(?,?)"
        Dim DRpsIns As OdbcDataReader = Nothing
        Dim DBCpsIns As OdbcCommand
        Dim psInsP1 As New OdbcParameter("@PrtSpec", OdbcType.VarChar, 50)
        Dim psInsP2 As New OdbcParameter("@CollCode", OdbcType.VarChar, 4)
        DBCpsIns = New OdbcCommand(psInsSQL, DBConn)
        DBCpsIns.Parameters.Add(psInsP1)
        DBCpsIns.Parameters.Add(psInsP2)
        For j As Integer = 1 To ps.Count
            Dim p As New prts
            p = DirectCast(ps.Item(j), prts)
            psInsP1.Value = p.P1
            psInsP2.Value = p.P2
            Try
                DRpsIns = DBCpsIns.ExecuteReader()
            Catch ex As Exception
                MsgBox(ex.ToString())
            Finally
                If Not (DRpsIns Is Nothing) Then
                    DRpsIns.Close()
                End If
            End Try
        Next
        DBConn.Close()
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ReadSysMsg(My.Application.Info.DirectoryPath & "\" & "system messages.txt")
        Me.Text = SysMsg(1000)
        Label1.Text = SysMsg(1001)
        Button1.Text = SysMsg(1002)
    End Sub
    Private Sub ReadSysMsg(ByVal Filename As String)
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
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Private Function NxtWordFromStr(ByRef s As String, Optional ByVal Def As String = "", Optional ByVal sep As String = " ") As String ' modifies s .............
        ' get next word from string, and strip from string s
        Dim i As Integer
        s = s.TrimStart()
        i = InStr(1, s, sep)
        If i = 0 Then
            NxtWordFromStr = s
            s = ""
        Else
            NxtWordFromStr = s.Substring(0, i - 1)
            s = s.Substring(i)
        End If
        If NxtWordFromStr = "" Then If Not IsNothing(Def) Then NxtWordFromStr = Def
        NxtWordFromStr = NxtWordFromStr
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Private Function SysMsg(ByVal i As Integer) As String
        Dim s As String = "ARCAT" & CStr(i)
        If SysConst.Contains(s) Then
            SysMsg = CStr(SysConst.Item(s))
        Else
            SysMsg = s & " not defined in 'system messages.txt'"
        End If
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()> Private Function GetDbIntegerValue(ByVal Dr As OdbcDataReader, ByVal nr As Integer) As Integer
        If IsDBNull(Dr.Item(nr)) Then
            Return 0
        Else
            Return Dr.GetInt32(nr)
        End If
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()> Friend Function GetDbStringValue(ByVal Dr As OdbcDataReader, ByVal nr As Integer) As String
        If IsDBNull(Dr.Item(nr)) Then
            Return ""
        Else
            Return Dr.GetString(nr).TrimEnd
        End If
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()> Friend Function GetDbBooleanValue(ByVal Dr As OdbcDataReader, ByVal nr As Integer) As Boolean
        If IsDBNull(Dr.Item(nr)) Then
            Return False
        Else
            Return Dr.GetBoolean(nr)
        End If
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()> Friend Function GetDbDecimalValue(ByVal Dr As OdbcDataReader, ByVal nr As Integer) As Decimal
        If IsDBNull(Dr.Item(nr)) Then
            Return 0
        Else
            Return Dr.GetDecimal(nr)
        End If
    End Function
    Private Sub TCollCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TCollCode.TextChanged
        If TCollName.Text.Trim.Length() > 0 And TCollName.Text.Trim.Length() > 0 Then
            Button1.Visible = True
        End If
    End Sub
    Private Sub TCollName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TCollName.TextChanged
        If TCollName.Text.Trim.Length() > 0 And TCollName.Text.Trim.Length() > 0 Then
            Button1.Visible = True
        End If
    End Sub
End Class
Class flds
    Public P1 As String
    Public P2 As String
    Public P3 As String
    Public P4 As String
    Public P5 As Integer
    Public P6 As Integer
    Public P7 As Integer
    Public P8 As String
    Public P9 As String
    Public P10 As String
    Public P11 As String
    Public P12 As String
    Public P13 As Boolean
    Public P14 As Boolean
    Public P15 As Boolean
    Public P16 As Boolean
End Class
Class prts
    Public P1 As String
    Public P2 As String
End Class
