'
Public Class PrtSpcData
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim saven As Boolean
    Dim bmb As BindingManagerBase
    Dim dsPrtSpec As New DataSet()
    Dim daPrtSpec As New OdbcDataAdapter()
    Dim daPrtFields As New OdbcDataAdapter()
    Dim SelCollCode, SelPrt As String
    Private Sub Collecties_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If saven Then
                DbConn.Open()
                daPrtFields.Update(dsPrtSpec, "PRTFLDS")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
        If (DbConn.State = ConnectionState.Open) Then
            DbConn.Close()
        End If
    End Sub
    Private Sub Collecties_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SelCollCode = SelCollCodeCl ' from calling form
        SelPrt = SelPrtCl
        Dim SqlCmnd As OdbcCommand
        Dim SqlDbParam As OdbcParameter
        Me.Text = SysMsg(504)
        Me.DesktopLocation = New Point(0, 0)
        BSave.Text = SysMsg(3)
        BCancel.Text = SysMsg(2)
        saven = True

        SqlCmnd = New OdbcCommand("SELECT CollCode, PrtSpec FROM prtspecs WHERE PrtSpec = '" & SelPrt & "' AND CollCode = '" & SelCollCode & "'", DbConn)
        daPrtSpec.SelectCommand = SqlCmnd
        PrtNm.AllowUserToAddRows = False
        PrtNm.AllowUserToDeleteRows = False

        SqlCmnd = New OdbcCommand("SELECT PrtSpec, PrtOrder, FldCode, BeforeFirstField, BetweenFields, AfterLastField, IfFieldMissing, IfFieldDifferent FROM prtspeclines WHERE PrtSpec = '" & SelPrt & "' ORDER BY PrtOrder", DbConn)
        daPrtFields.SelectCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("DELETE FROM prtspeclines WHERE PrtSpec = ? AND PrtOrder = ?", DbConn)
        SqlDbParam = SqlCmnd.Parameters.Add("@oldPrtSpec", Odbc.OdbcType.VarChar, 50, "PrtSpec")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlDbParam = SqlCmnd.Parameters.Add("@OldPrtOrder", Odbc.OdbcType.Int, 4, "PrtOrder")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daPrtFields.DeleteCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("INSERT INTO prtspeclines(PrtSpec, PrtOrder, FldCode, BeforeFirstField, BetweenFields, AfterLastField, IfFieldMissing, IfFieldDifferent) VALUES(?, ?, ?, ?, ?, ?, ?, ?)", DbConn)
        SqlCmnd.Parameters.Add("@PrtSpec", Odbc.OdbcType.VarChar, 50, "PrtSpec")
        SqlCmnd.Parameters.Add("@PrtOrder", Odbc.OdbcType.Int, 4, "PrtOrder")
        SqlCmnd.Parameters.Add("@FldCode", Odbc.OdbcType.VarChar, 15, "FldCode")
        SqlCmnd.Parameters.Add("@BeforeFirstField", Odbc.OdbcType.VarChar, 255, "BeforeFirstField")
        SqlCmnd.Parameters.Add("@BetweenFields", Odbc.OdbcType.VarChar, 255, "BetweenFields")
        SqlCmnd.Parameters.Add("@AfterLastField", Odbc.OdbcType.VarChar, 255, "AfterLastField")
        SqlCmnd.Parameters.Add("@IfFieldMissing", Odbc.OdbcType.VarChar, 255, "IfFieldMissing")
        SqlCmnd.Parameters.Add("@IfFieldDifferent", Odbc.OdbcType.Bit, 1, "IfFieldDifferent")
        daPrtFields.InsertCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("UPDATE prtspeclines SET PrtOrder = ?, FldCode = ?, BeforeFirstField = ?, BetweenFields = ?, AfterLastField = ?, IfFieldMissing = ?, IfFieldDifferent = ? WHERE PrtSpec = ? AND PrtOrder = ?", DbConn)
        SqlCmnd.Parameters.Add("@PrtOrder", Odbc.OdbcType.Int, 4, "PrtOrder")
        SqlCmnd.Parameters.Add("@FldCode", Odbc.OdbcType.VarChar, 15, "FldCode")
        SqlCmnd.Parameters.Add("@BeforeFirstField", Odbc.OdbcType.VarChar, 255, "BeforeFirstField")
        SqlCmnd.Parameters.Add("@BetweenFields", Odbc.OdbcType.VarChar, 255, "BetweenFields")
        SqlCmnd.Parameters.Add("@AfterLastField", Odbc.OdbcType.VarChar, 255, "AfterLastField")
        SqlCmnd.Parameters.Add("@IfFieldMissing", Odbc.OdbcType.VarChar, 255, "IfFieldMissing")
        SqlCmnd.Parameters.Add("@IfFieldDifferent", Odbc.OdbcType.Bit, 1, "IfFieldDifferent")
        SqlDbParam = SqlCmnd.Parameters.Add("@oldPrtSpec", Odbc.OdbcType.VarChar, 50, "PrtSpec")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlDbParam = SqlCmnd.Parameters.Add("@OldPrtOrder", Odbc.OdbcType.Int, 4, "PrtOrder")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daPrtFields.UpdateCommand = SqlCmnd

        DbConn.Open()
        daPrtSpec.Fill(dsPrtSpec, "PRTSPEC")
        daPrtFields.Fill(dsPrtSpec, "PRTFLDS")
        dsPrtSpec.Relations.Add("SpecFlds", dsPrtSpec.Tables("PRTSPEC").Columns("PrtSpec"), dsPrtSpec.Tables("PRTFLDS").Columns("PrtSpec"))
        PrtNm.DataSource = dsPrtSpec
        PrtNm.DataMember = "PRTSPEC"
        Specs.DataSource = dsPrtSpec
        Specs.DataMember = "PRTSPEC.SpecFlds"
        bmb = Me.BindingContext(dsPrtSpec, "PRTSPEC")
        bmb.Position = bmb.Count
        bmb.Position = 0
        DbConn.Close()
    End Sub
    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSave.Click
        DbConn.Open()
        Try
            daPrtFields.Update(dsPrtSpec, "PRTFLDS")
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
        DbConn.Close()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCancel.Click
        saven = False
        Me.Close()
    End Sub
    Private Sub PrtNm_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles PrtNm.UserAddedRow
        Dim gr As DataGridView
        gr = DirectCast(sender, DataGridView)
        RemoveNullFromGrid(gr)
    End Sub

    Private Sub Specs_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles Specs.UserAddedRow
        Dim gr As DataGridView
        gr = DirectCast(sender, DataGridView)
        RemoveNullFromGrid(gr)
    End Sub
End Class
