'
Public Class PrtSpc
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim saven As Boolean
    Dim bmb As BindingManagerBase
    Dim dsCollectie As New DataSet()
    Dim daCollections As New OdbcDataAdapter()
    Dim daSpcVal As New OdbcDataAdapter()
    Private Sub Collecties_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If saven Then
                daSpcVal.Update(dsCollectie, "SPCVAL")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
        If Not (DbConn Is Nothing) Then
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End If
    End Sub
    Private Sub Collecties_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim SqlCmnd As OdbcCommand
        Dim SqlDbParam As OdbcParameter
        Me.Text = SysMsg(503)
        Me.DesktopLocation = New Point(0, 0)
        BSave.Text = SysMsg(3)
        BCancel.Text = SysMsg(2)
        BToPrt.Text = SysMsg(600)
        saven = True

        DeleteNonExist()

        SqlCmnd = New OdbcCommand("SELECT CollCode, CollName FROM collections", DbConn)
        daCollections.SelectCommand = SqlCmnd
        CollCodes.AllowUserToAddRows = False
        CollCodes.AllowUserToDeleteRows = False

        SqlCmnd = New OdbcCommand("SELECT CollCode, PrtSpec FROM prtspecs", DbConn)
        daSpcVal.SelectCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("DELETE FROM prtspecs WHERE CollCode = ? AND PrtSpec = ?", DbConn)
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlCmnd.Parameters.Add("@oldPrtSpec", Odbc.OdbcType.VarChar, 50, "PrtSpec")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daSpcVal.DeleteCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("INSERT INTO prtspecs(CollCode, PrtSpec) VALUES(?, ?)", DbConn)
        SqlCmnd.Parameters.Add("@CollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlCmnd.Parameters.Add("@PrtSpec", Odbc.OdbcType.VarChar, 50, "PrtSpec")
        daSpcVal.InsertCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("UPDATE prtspecs SET PrtSpec = ? WHERE CollCode = ? AND PrtSpec = ?", DbConn)
        SqlCmnd.Parameters.Add("@PrtSpec", Odbc.OdbcType.VarChar, 50, "PrtSpec")
        SqlDbParam = SqlCmnd.Parameters.Add("@OldCollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlDbParam = SqlCmnd.Parameters.Add("@oldPrtSpec", Odbc.OdbcType.VarChar, 50, "PrtSpec")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daSpcVal.UpdateCommand = SqlCmnd

        DbConn.Open()
        daCollections.Fill(dsCollectie, "collections")
        daSpcVal.Fill(dsCollectie, "SPCVAL")
        dsCollectie.Relations.Add("SfnVal", dsCollectie.Tables("collections").Columns("CollCode"), dsCollectie.Tables("SPCVAL").Columns("CollCode"))
        CollCodes.DataSource = dsCollectie
        CollCodes.DataMember = "collections"
        SpcVal.DataSource = dsCollectie
        SpcVal.DataMember = "collections.SfnVal"
        bmb = Me.BindingContext(dsCollectie, "collections")
        bmb.Position = bmb.Count
        bmb.Position = 0
    End Sub
    Private Sub DeleteNonExist() ' delete specs, after user deleted collections
        Dim DbConn As New OdbcConnection(SqlProv)
        Dim DeleteCursor As OdbcDataReader = Nothing
        Dim DbDeleteCursor As OdbcCommand
        Dim DeleteSql As String
        Try
            DbConn.Open()
        Catch e1 As Exception
            MsgBox(e1.ToString(), , SysMsg(66))
        End Try
        DeleteSql = "DELETE FROM prtspecs WHERE COLLCODE NOT IN (SELECT COLLCODE FROM COLLECTIONS)"
        DbDeleteCursor = New OdbcCommand(DeleteSql, DbConn)
        Try
            DeleteCursor = DbDeleteCursor.ExecuteReader()
        Catch e1 As Exception
            MsgBox(e1.ToString())
        Finally
            If Not (DeleteCursor Is Nothing) Then
                DeleteCursor.Close()
            End If
        End Try
        DeleteSql = "DELETE FROM prtspeclines WHERE PrtSpec NOT IN (SELECT Distinct PrtSpec FROM prtspecs)"
        DbDeleteCursor = New OdbcCommand(DeleteSql, DbConn)
        Try
            DeleteCursor = DbDeleteCursor.ExecuteReader()
        Catch e1 As Exception
            MsgBox(e1.ToString())
        Finally
            If Not (DeleteCursor Is Nothing) Then
                DeleteCursor.Close()
            End If
        End Try
    End Sub
    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSave.Click
        Try
            daSpcVal.Update(dsCollectie, "SPCVAL")
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCancel.Click
        saven = False
        Me.Close()
    End Sub
    Private Sub SpcVal_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles SpcVal.CellEnter
        Dim gr As DataGridView = DirectCast(sender, DataGridView)
        SelCollCodeCl = CStr(gr.Item(0, e.RowIndex).Value)
        Try
            SelPrtCl = CStr(gr.Item(1, e.RowIndex).Value)
        Catch ex As Exception
            SelPrtCl = ""
        End Try
    End Sub

    Private Sub SpcVal_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles SpcVal.CellLeave
        Dim gr As DataGridView = DirectCast(sender, DataGridView)
        SelCollCodeCl = CStr(gr.Item(0, e.RowIndex).Value)
        Try
            SelPrtCl = CStr(gr.Item(1, e.RowIndex).Value)
        Catch ex As Exception
            SelPrtCl = ""
        End Try
    End Sub
    Private Sub SpcVal_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles SpcVal.CellValueChanged
        Dim gr As DataGridView = DirectCast(sender, DataGridView)
        SelCollCodeCl = CStr(gr.Item(0, e.RowIndex).Value)
        Try
            SelPrtCl = CStr(gr.Item(1, e.RowIndex).Value)
        Catch ex As Exception
            SelPrtCl = ""
        End Try
    End Sub
    Private Sub BToPrt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BToPrt.Click
        If SelPrtCl <> "" Then
            Try
                daSpcVal.Update(dsCollectie, "SPCVAL")
            Catch ex As Exception
                MsgBox(ex.ToString())
            End Try
            Dim f As New PrtSpcData
            f.Show()
        End If
    End Sub

    Private Sub CollCodes_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles CollCodes.UserAddedRow
        Dim gr As DataGridView
        gr = DirectCast(sender, DataGridView)
        RemoveNullFromGrid(gr)
    End Sub

    Private Sub SpcVal_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles SpcVal.UserAddedRow
        Dim gr As DataGridView
        gr = DirectCast(sender, DataGridView)
        RemoveNullFromGrid(gr)
    End Sub
End Class
