Imports System.Globalization
'
'#Const CreLogFile = True
Module Module1
#If CreLogFile Then
    Friend logFile As StreamWriter = File.CreateText("ArCatSy.Log.txt")
    Friend nInsp As Integer
#Else
    Friend logFile As StreamWriter = Nothing
    Friend nInsp As Integer
#End If
    Friend Defined As Boolean = False
    Friend CollCodeMenuSel As String = ""
    Friend CurCollCodeMtn As String = "" ' in maintenance
    Friend CurPrtSpecMtn As String = "" ' in maintenance
    Friend SelCollCodeCl As String = "" ' in selectie prtspec
    Friend SelPrtCl As String = "" ' in selectie prtspec
    Friend CollSeqSel As Integer = 0 ' in key selectie form
    Friend TypeEntryMenu As String = "" ' keuze op menu
    Friend TypeExpInpMenu As String = "" ' type imp/exp op menu
    'next 2 will be overwritten in form1
    Public CultInf As New CultureInfo("en-US", False)
    Public SqlProv As String = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=ArCatSy.mdb"
    Public SqlServ As String = "Server=Machine\Engine;Initial Catalog=ArCatSy;Integrated Security=SSPI;"
    Public mySqlServ As String = "server=127.0.0.1;uid=root;pwd=;database=ArCatSy;"

    Friend KeysSelectedToShow As New Collection
    Friend Wrtr As StreamWriter, Rdr As StreamReader
    Friend SysConst As New Collection
    Friend HideColl As New Collection
    Friend fPrinten As Printen

    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Friend Function SysMsg2p(ByVal i As Integer, ByVal n1 As String, ByVal n2 As String) As String
        Dim s As String
        s = SysMsg(i)
        i = InStr(s, "%1")
        If i > 0 Then
            s = s.Substring(0, i - 1) & n1 & s.Substring(i + 1)
        End If
        i = InStr(s, "%2")
        If i > 0 Then
            s = s.Substring(0, i - 1) & n2 & s.Substring(i + 1)
        End If
        Return s
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Friend Function SysMsg1p(ByVal i As Integer, ByVal n1 As String) As String
        Dim s As String
        s = SysMsg(i)
        i = InStr(s, "%1")
        If i > 0 Then
            s = s.Substring(0, i - 1) & n1 & s.Substring(i + 1)
        End If
        Return s
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Friend Function SysMsg(ByVal i As Integer) As String
        Dim s As String = "ARCAT" & CStr(i)
        If SysConst.Contains(s) Then
            SysMsg = CStr(SysConst.Item(s))
        Else
            SysMsg = s & " not defined in 'system messages.txt'"
        End If
    End Function
    ' <Global.System.Diagnostics.DebuggerStepThroughAttribute()> _
    <Conditional("CreLogFile")>
    Friend Sub Loggen(ByVal s As String)
        Dim i As Integer
        i = s.IndexOf(" "c)
        If i > 0 Then
            If s.Length() > i + 5 Then
                If s.Substring(i + 1, 5) = "start" Then nInsp += 2
            End If
        End If
        logFile.WriteLine(" ".PadRight(nInsp) & s)
        If i > 0 Then
            If s.Length() > i + 3 Then
                If s.Substring(i + 1, 3) = "end" Then
                    nInsp -= 2
                    If nInsp < 0 Then nInsp = 0
                End If
            End If
        End If
    End Sub
    Friend Sub ReadCursorCollection(ByRef DestCollection As Collection, ByVal SelectSql As String, ByVal SeqNrsFields As String, ByVal FieldSeparator As String, ByVal KeyC As Boolean)
        Dim DbConn As New OdbcConnection(SqlProv)
        Dim DRSelectCursor As OdbcDataReader = Nothing
        Dim DbCSelectCursor As OdbcCommand
        Dim s, s1, kc As String, words() As String, t As Char, n As Integer
        DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
        DestCollection.Clear()
        Try
            DbConn.Open()
            DRSelectCursor = DbCSelectCursor.ExecuteReader()
            words = SeqNrsFields.Split()
            s1 = ""
            Do While (DRSelectCursor.Read())
                s = ""
                kc = ""
                For i As Integer = 1 To words.Length()
                    If i > 1 Then s = s & FieldSeparator
                    t = words(i - 1)(0)
                    n = CInt(words(i - 1).Substring(1))
                    Select Case t
                        Case "S"c
                            s1 = GetDbStringValue(DRSelectCursor, n)
                        Case "I"c
                            s1 = CStr(GetDbIntegerValue(DRSelectCursor, n))
                    End Select
                    s = s & s1
                    If n = 0 Then kc = s1
                Next
                If KeyC Then
                    DestCollection.Add(s, kc)
                Else
                    DestCollection.Add(s)
                End If
            Loop
        Catch e1 As Exception
            MsgBox(e1.ToString())
        Finally
            If Not (DRSelectCursor Is Nothing) Then
                DRSelectCursor.Close()
            End If
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End Try
    End Sub
    Friend Sub Wrt(ByVal s As String)
        Wrtr.Write(s & vbCrLf)
    End Sub
    Friend Function CstrD(d As Decimal) As String
        Dim i As Integer
        i = d
        Return (CStr(i))
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Friend Function WritableFile(ByVal fn As String, ByRef orig As String) As Boolean
        Dim f As String
        WritableFile = False
        f = Path.GetFullPath(fn)
        Dim dr As New DirectoryInfo(f.Substring(0, 2))
        If dr.Attributes = -1 Then
            MsgBox(SysMsg1p(701, f.Substring(0, 2)), , orig)
            Exit Function
        End If
        If CBool(dr.Attributes And FileAttributes.ReadOnly) Then ' R/O drive
            MsgBox(SysMsg1p(702, f.Substring(0, 2)), , orig)
            Exit Function
        End If
        If File.Exists(f) Then
            Dim fi As New FileInfo(f)
            If CBool(fi.IsReadOnly) Then
                MsgBox(SysMsg1p(703, f), , orig)
                Exit Function
            End If
        End If
        WritableFile = True
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Friend Function NxtWordFromStr(ByRef s As String, Optional ByVal Def As String = "", Optional ByVal sep As String = " ") As String ' modifies s .............
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
    Friend Function GetDbStringValue(ByVal Dr As OdbcDataReader, ByVal nr As Integer) As String
        If IsDBNull(Dr.Item(nr)) Then
            Return ""
        Else
            Return Dr.GetString(nr).TrimEnd
        End If
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Friend Function GetDbBooleanValue(ByVal Dr As OdbcDataReader, ByVal nr As Integer) As Boolean
        If IsDBNull(Dr.Item(nr)) Then
            Return False
        Else
            Return Dr.GetBoolean(nr)
        End If
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Friend Function GetDbDecimalValue(ByVal Dr As OdbcDataReader, ByVal nr As Integer) As Decimal
        If IsDBNull(Dr.Item(nr)) Then
            Return 0
        Else
            Return Dr.GetDecimal(nr)
        End If
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()>
    Friend Function GetDbIntegerValue(ByVal Dr As OdbcDataReader, ByVal nr As Integer) As Integer
        If IsDBNull(Dr.Item(nr)) Then
            Return 0
        Else
            Return Dr.GetInt32(nr)
        End If
    End Function
    <Global.System.Diagnostics.DebuggerStepThroughAttribute()> _
    Friend Function EmptyToBlank(ByVal s As String) As String
        ' If s.Length() = 0 Then s = " "
        Return s
    End Function
    Public Sub RemoveNullFromGrid(ByVal gr As DataGridView)
        For j As Integer = 1 To gr.CurrentRow.Cells.Count - 1
            If IsDBNull(gr.CurrentRow.Cells.Item(j).Value) Then
                Dim vt As String = gr.CurrentRow.Cells.Item(j).ValueType.ToString
                Try
                    If vt = "System.String" Then
                        gr.CurrentRow.Cells.Item(j).Value = ""
                    ElseIf vt = "System.Boolean" Then
                        gr.CurrentRow.Cells.Item(j).Value = False
                    Else
                        gr.CurrentRow.Cells.Item(j).Value = 0
                    End If
                Catch ex As Exception

                End Try
            End If
        Next
    End Sub
End Module
Friend Class CtlsData
        Friend Ix As Integer ' own index (to access after access with key)
        Friend CtlsIx As Integer ' idx in controls (V.)
        Friend CLevel As Integer ' level of parentage en aantal indexen in Seq
        Friend Naam As String ' naam veld (v)
        Friend NaamNm As String ' naam compleet (v.1.2.3)
        Friend NaamNm1 As String ' naam tot laatste index (v.1.2.)
        Friend NaamRst As String ' naam vanaf indexen (.1.2.3)
        Friend Seq(5) As Integer ' indexen numeric
        Friend HasDep As Boolean ' has dependents?
        Friend HasParent As Boolean ' has parent?
        Friend Visible As Boolean ' op scherm
        Friend Expanded As Boolean ' op scherm 
        Friend HasText As Boolean ' op scherm 
        Friend HadText As Boolean ' vorige op scherm 
        Friend ChaExit As Boolean
        Friend LeaveExit As Boolean
        Friend OldData As String ' actual data in database
        Friend Top As Integer ' van labelbox zonder "zichtbaar maken"
        Friend IsCombo As Boolean
End Class
    Friend Class DefData
        Friend Naam As String
        Friend Parent As String
        Friend Type As String
        Friend Caption As String
        Friend MaxLength As Integer
        Friend MaxOccurs As Integer
        Friend Combo As String
        Friend ChaExit As Boolean
        Friend LeaveExit As Boolean
        Friend RegExpC As String
        Friend RegExpErr As String
        Friend RegExpX As String
    End Class
    Friend Class PrtData
        Friend Seq As Integer
        Friend FldCode As String
        Friend RefEnt As String
        Friend RefFld As String
        Friend BeforeFirstField As String
        Friend BetweenFields As String
        Friend AfterLastField As String
        Friend IfFieldMissing As String
        Friend IfFieldDifferent As Boolean
        Friend IxTxt As Integer
        Friend Chld As Boolean
        Friend HasChld As Boolean
        Friend FldParent As String
        Friend PrevCon As String
        'Friend FldSeq As Integer
        Friend HasExit As Boolean
    End Class
    Friend Class KenmDef
        Friend FldCode As String
        Friend FldParent As String
        Friend FldHasDep As Boolean
        Friend FldHasExit As Boolean
    End Class
    Friend Class KenmDefin
        Friend FldCode As String
        Friend FldText As String
    End Class
    Friend Class PrtFldText
        Friend FldSeqnrs As Decimal
        Friend Txt As String
    End Class
    Friend Class GenDef
        Friend FldCode As String
        Friend FldParent As String
        Friend FldCaption As String
        Friend FldMaxLength As Integer
        Friend FldMaxOccurs As Integer
        Friend FldCheckRegexp As String
        Friend FldCheckErrmsg As String
        Friend FldCombo As String
        Friend FldTitle As String
    End Class
