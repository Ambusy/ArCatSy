'
Public Class ExpPrt
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim fso As Object, WrtrUc As Object
    Dim CurPrtSpecMt As String
    Dim TypeExpInp As String
    Private Sub BBrws_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BBrws.Click
        OpenFileDialog1.Title = SysMsg(867)
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Layoutfiles (*.txt)|*.txt|All files (*.*)|*.*"
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
        If TypeExpInp = "Imp" Then
            doImp()
        Else
            doExp()
        End If
        Me.Close()
    End Sub
    Private Sub doExp()
        Dim s As String
        Dim SelectSql As String
        Dim DRSelectCursor As OdbcDataReader = Nothing
        Dim DbCSelectCursor As OdbcCommand
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        'select printspec
        SelectSql = "SELECT PrtSpec, PrtOrder, FldCode, BeforeFirstField, BetweenFields, AfterLastField, IfFieldMissing, IfFieldDifferent FROM prtspeclines WHERE PrtSpec = '" & CurPrtSpecMt & "' ORDER BY PrtOrder"
        DbCSelectCursor = New OdbcCommand(SelectSql, DbConn)
        fso = CreateObject("Scripting.FileSystemObject")
        WrtrUc = fso.CreateTextFile(TDsn.Text, True, True) ' unicode
        Try
            DRSelectCursor = DbCSelectCursor.ExecuteReader()
            Do While (DRSelectCursor.Read())
                s = GetDbIntegerValue(DRSelectCursor, 1) & vbTab _
                & GetDbStringValue(DRSelectCursor, 2) & vbTab _
                & GetDbStringValue(DRSelectCursor, 3) & vbTab _
                & GetDbStringValue(DRSelectCursor, 4) & vbTab _
                & GetDbStringValue(DRSelectCursor, 5) & vbTab _
                & GetDbStringValue(DRSelectCursor, 6) & vbTab _
                & GetDbBooleanValue(DRSelectCursor, 7)
                WrtrUc.WriteLine(s)
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
    Private Sub doImp()
        Dim s As String = "", i, i1 As Integer
        'insert printspec
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        Dim deleteSQL As String = "DELETE FROM prtspeclines WHERE PrtSpec = '" & CurPrtSpecMt & "'"
        Dim DRDelete As OdbcDataReader = Nothing
        Dim DbCDelete As OdbcCommand
        DbCDelete = New OdbcCommand(deleteSQL, DbConn)
        Try
            DRDelete = DbCDelete.ExecuteReader()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(870))
        Finally
            If Not (DRDelete Is Nothing) Then
                DRDelete.Close()
            End If
        End Try
        Dim insertSQL As String = "INSERT INTO prtspeclines(PrtSpec, PrtOrder, FldCode, BeforeFirstField, BetweenFields, AfterLastField, IfFieldMissing, IfFieldDifferent) VALUES(?, ?, ?, ?, ?, ?, ?, ?)"
        Dim DRInsert As OdbcDataReader = Nothing
        Dim DbCInsert As OdbcCommand
        Dim InsP1 As New OdbcParameter("@PrtSpec", Odbc.OdbcType.VarChar, 50, "PrtSpec")
        Dim InsP2 As New OdbcParameter("@PrtOrder", Odbc.OdbcType.Int, 4, "PrtOrder")
        Dim InsP3 As New OdbcParameter("@FldCode", Odbc.OdbcType.VarChar, 15, "FldCode")
        Dim InsP4 As New OdbcParameter("@BeforeFirstField", Odbc.OdbcType.VarChar, 255, "BeforeFirstField")
        Dim InsP5 As New OdbcParameter("@BetweenFields", Odbc.OdbcType.VarChar, 255, "BetweenFields")
        Dim InsP6 As New OdbcParameter("@AfterLastField", Odbc.OdbcType.VarChar, 255, "AfterLastField")
        Dim InsP7 As New OdbcParameter("@IfFieldMissing", Odbc.OdbcType.VarChar, 255, "IfFieldMissing")
        Dim InsP8 As New OdbcParameter("@IfFieldDifferent", Odbc.OdbcType.Bit, 1, "IfFieldDifferent")

        DbCInsert = New OdbcCommand(insertSQL, DbConn)
        DbCInsert.Parameters.Add(InsP1)
        DbCInsert.Parameters.Add(InsP2)
        DbCInsert.Parameters.Add(InsP3)
        DbCInsert.Parameters.Add(InsP4)
        DbCInsert.Parameters.Add(InsP5)
        DbCInsert.Parameters.Add(InsP6)
        DbCInsert.Parameters.Add(InsP7)
        DbCInsert.Parameters.Add(InsP8)

        Form1.Idxen.Visible = True

        Dim st As Stream = File.Open(TDsn.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        Dim PrtSpec, FldCode, BeforeFirstField, BetweenFields, AfterLastField, IfFieldMissing As String
        Dim IfFieldDifferent As Boolean, PrtOrder As Integer
        Rdr = New StreamReader(st)
        Do Until Rdr.Peek = -1
            s = Rdr.ReadLine
            PrtSpec = CurPrtSpecMt
            i1 = 0
            i = InStr(1, s, vbTab)
            PrtOrder = CInt(s.Substring(i1, i - i1 - 1))
            i1 = i
            i = InStr(i + 1, s, vbTab)
            FldCode = s.Substring(i1, i - i1 - 1).TrimEnd
            i1 = i
            i = InStr(i + 1, s, vbTab)
            BeforeFirstField = s.Substring(i1, i - i1 - 1)
            i1 = i
            i = InStr(i + 1, s, vbTab)
            BetweenFields = s.Substring(i1, i - i1 - 1)
            i1 = i
            i = InStr(i + 1, s, vbTab)
            AfterLastField = s.Substring(i1, i - i1 - 1)
            i1 = i
            i = InStr(i + 1, s, vbTab)
            IfFieldMissing = s.Substring(i1, i - i1 - 1)
            IfFieldDifferent = s.Substring(i).ToUpper = "TRUE"
            InsP1.Value = PrtSpec
            InsP2.Value = PrtOrder
            InsP3.Value = FldCode
            InsP4.Value = EmptyToBlank(BeforeFirstField)
            InsP5.Value = EmptyToBlank(BetweenFields)
            InsP6.Value = EmptyToBlank(AfterLastField)
            InsP7.Value = EmptyToBlank(IfFieldMissing)
            InsP8.Value = IfFieldDifferent
            Try
                DRInsert = DbCInsert.ExecuteReader()
            Catch ex As Exception
                MsgBox(ex.ToString(), , SysMsg(871))
            Finally
                If Not (DRInsert Is Nothing) Then
                    DRInsert.Close()
                End If
            End Try
        Loop
        Rdr.Close()
        st.Close()
        DbConn.Close()
    End Sub
    Private Sub ExpSpec_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DesktopLocation = New Point(0, 0)
        CurPrtSpecMt = CurPrtSpecMtn
        TypeExpInp = TypeExpInpMenu
        If TypeExpInp = "Imp" Then
            Me.Text = SysMsg(864)
            BImpExp.Text = SysMsg(853)
            Label1.Text = SysMsg(857)
            Label2.Text = SysMsg1p(866, CurPrtSpecMt)
        Else
            Me.Text = SysMsg(865)
            BImpExp.Text = SysMsg(854)
            Label1.Text = SysMsg(858)
            Label2.Text = SysMsg1p(863, CurPrtSpecMt)
        End If
        BBrws.Text = SysMsg(804)
        BCanc.Text = SysMsg(2)
    End Sub
    Private Sub TDsn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TDsn.TextChanged
        BImpExp.Visible = True
    End Sub
End Class
