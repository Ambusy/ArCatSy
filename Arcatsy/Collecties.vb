'

Public Class Collecties
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim saven As Boolean
    Dim bmb As BindingManagerBase
    Dim dsCollectie As New DataSet()
    Dim daCollectie As New OdbcDataAdapter()
    Dim daCollFields As New OdbcDataAdapter()
    Private Sub Collecties_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If saven Then
                daCollectie.Update(dsCollectie, "collections")
                daCollFields.Update(dsCollectie, "collfields")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(506))
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
        Me.Text = SysMsg(501)
        Me.DesktopLocation = New Point(0, 0)
        BSave.Text = SysMsg(3)
        BCancel.Text = SysMsg(2)
        saven = True
        SqlCmnd = New OdbcCommand("SELECT CollCode, CollName, DefPrtSpec, EntitySelFields FROM collections", DbConn)
        daCollectie.SelectCommand = SqlCmnd
        SqlCmnd = New OdbcCommand("UPDATE collections SET CollName = ?, DefPrtSpec = ?, EntitySelFields = ? WHERE CollCode = ?", DbConn)
        SqlCmnd.Parameters.Add("@CollName", Odbc.OdbcType.VarChar, 100, "CollName")
        SqlCmnd.Parameters.Add("@DefPrtSpec", Odbc.OdbcType.VarChar, 50, "DefPrtSpec")
        SqlCmnd.Parameters.Add("@EntitySelFields", Odbc.OdbcType.VarChar, 50, "EntitySelFields")
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daCollectie.UpdateCommand = SqlCmnd
        SqlCmnd = New OdbcCommand("DELETE FROM collections WHERE CollCode = ?", DbConn)
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daCollectie.DeleteCommand = SqlCmnd
        SqlCmnd = New OdbcCommand("INSERT INTO collections(CollCode, CollName, DefPrtSpec, EntitySelFields) VALUES(?, ?, ?, ?)", DbConn)
        SqlCmnd.Parameters.Add("@CollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlCmnd.Parameters.Add("@CollName", Odbc.OdbcType.VarChar, 100, "CollName")
        SqlCmnd.Parameters.Add("@DefPrtSpec", Odbc.OdbcType.VarChar, 50, "DefPrtSpec")
        SqlCmnd.Parameters.Add("@EntitySelFields", Odbc.OdbcType.VarChar, 50, "EntitySelFields")
        daCollectie.InsertCommand = SqlCmnd
        SqlCmnd = New OdbcCommand("SELECT CollCode, FldCode, FldCaption, FldParent, FldInputOrder, FldMaxLength, FldMaxOccurs, FldCheckRegexp, FldCheckErrmsg, FldTermRegexp, FldTermType, FldCombo, FldPExit, FldSExit, FldXExit, FldQExit FROM collfields", DbConn)
        daCollFields.SelectCommand = SqlCmnd
        SqlCmnd = New OdbcCommand("UPDATE collfields SET FldCode = ?, FldCaption = ?, FldParent = ?, FldInputOrder = ?, FldMaxLength = ?, FldMaxOccurs = ?, FldCheckRegexp = ?, FldCheckErrmsg = ?, FldTermRegexp = ?, FldTermType = ?, FldCombo = ?, FldPExit = ?, FldSExit = ?, FldXExit = ?, FldQExit = ? WHERE CollCode = ? AND FldCode = ?", DbConn)
        SqlCmnd.Parameters.Add("@FldCode", Odbc.OdbcType.VarChar, 4, "FldCode")
        SqlCmnd.Parameters.Add("@FldCaption", Odbc.OdbcType.VarChar, 50, "FldCaption")
        SqlCmnd.Parameters.Add("@FldParent", Odbc.OdbcType.VarChar, 4, "FldParent")
        SqlCmnd.Parameters.Add("@FldInputOrder", Odbc.OdbcType.Int, 4, "FldInputOrder")
        SqlCmnd.Parameters.Add("@FldMaxLength", Odbc.OdbcType.Int, 4, "FldMaxLength")
        SqlCmnd.Parameters.Add("@FldMaxOccurs", Odbc.OdbcType.Int, 4, "FldMaxOccurs")
        SqlCmnd.Parameters.Add("@FldCheckRegexp", Odbc.OdbcType.VarChar, 255, "FldCheckRegexp")
        SqlCmnd.Parameters.Add("@FldCheckErrmsg", Odbc.OdbcType.VarChar, 80, "FldCheckErrmsg")
        SqlCmnd.Parameters.Add("@FldTermRegexp", Odbc.OdbcType.VarChar, 255, "FldTermRegexp")
        SqlCmnd.Parameters.Add("@FldTermType", Odbc.OdbcType.VarChar, 4, "FldTermType")
        SqlCmnd.Parameters.Add("@FldCombo", Odbc.OdbcType.VarChar, 50, "FldCombo")
        SqlCmnd.Parameters.Add("@FldPExit", Odbc.OdbcType.Bit, 4, "FldPExit")
        SqlCmnd.Parameters.Add("@FldSExit", Odbc.OdbcType.Bit, 4, "FldSExit")
        SqlCmnd.Parameters.Add("@FldXExit", Odbc.OdbcType.Bit, 4, "FldXExit")
        SqlCmnd.Parameters.Add("@FldQExit", Odbc.OdbcType.Bit, 4, "FldQExit")
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlDbParam = SqlCmnd.Parameters.Add("@oldFldCode", Odbc.OdbcType.VarChar, 4, "FldCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daCollFields.UpdateCommand = SqlCmnd
        SqlCmnd = New OdbcCommand("DELETE FROM collfields WHERE CollCode = ? AND FldCode = ?", DbConn)
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlDbParam = SqlCmnd.Parameters.Add("@oldFldCode", Odbc.OdbcType.VarChar, 4, "FldCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daCollFields.DeleteCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("INSERT INTO collfields(CollCode, FldCode, FldCaption, FldParent, FldInputOrder, FldMaxLength, FldMaxOccurs, FldCheckRegexp, FldCheckErrmsg, FldTermRegexp, FldTermType, FldCombo, FldPExit, FldSExit, FldXExit, FldQExit) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", DbConn)
        SqlCmnd.Parameters.Add("@CollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlCmnd.Parameters.Add("@FldCode", Odbc.OdbcType.VarChar, 4, "FldCode")
        SqlCmnd.Parameters.Add("@FldCaption", Odbc.OdbcType.VarChar, 50, "FldCaption")
        SqlCmnd.Parameters.Add("@FldParent", Odbc.OdbcType.VarChar, 4, "FldParent")
        SqlCmnd.Parameters.Add("@FldInputOrder", Odbc.OdbcType.Int, 4, "FldInputOrder")
        SqlCmnd.Parameters.Add("@FldMaxLength", Odbc.OdbcType.Int, 4, "FldMaxLength")
        SqlCmnd.Parameters.Add("@FldMaxOccurs", Odbc.OdbcType.Int, 4, "FldMaxOccurs")
        SqlCmnd.Parameters.Add("@FldCheckRegexp", Odbc.OdbcType.VarChar, 255, "FldCheckRegexp")
        SqlCmnd.Parameters.Add("@FldCheckErrmsg", Odbc.OdbcType.VarChar, 80, "FldCheckErrmsg")
        SqlCmnd.Parameters.Add("@FldTermRegexp", Odbc.OdbcType.VarChar, 255, "FldTermRegexp")
        SqlCmnd.Parameters.Add("@FldTermType", Odbc.OdbcType.VarChar, 4, "FldTermType")
        SqlCmnd.Parameters.Add("@FldCombo", Odbc.OdbcType.VarChar, 50, "FldCombo")
        SqlCmnd.Parameters.Add("@FldPExit", Odbc.OdbcType.Bit, 4, "FldPExit")
        SqlCmnd.Parameters.Add("@FldSExit", Odbc.OdbcType.Bit, 4, "FldSExit")
        SqlCmnd.Parameters.Add("@FldXExit", Odbc.OdbcType.Bit, 4, "FldXExit")
        SqlCmnd.Parameters.Add("@FldQExit", Odbc.OdbcType.Bit, 4, "FldQExit")
        daCollFields.InsertCommand = SqlCmnd

        DbConn.Open()
        daCollectie.Fill(dsCollectie, "collections")
        daCollFields.Fill(dsCollectie, "collfields")
        dsCollectie.Relations.Add("SfnKmc", dsCollectie.Tables("collections").Columns("CollCode"), dsCollectie.Tables("collfields").Columns("CollCode"))
        CollCodes.DataSource = dsCollectie
        CollCodes.DataMember = "collections"
        FldCodes.DataSource = dsCollectie
        FldCodes.DataMember = "collections.SfnKmc"
        bmb = Me.BindingContext(dsCollectie, "collections")
        bmb.Position = bmb.Count
        bmb.Position = 0
    End Sub
    Private Sub save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSave.Click
        Try
            daCollectie.Update(dsCollectie, "collections")
            daCollFields.Update(dsCollectie, "collfields")
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(506))
        End Try
    End Sub
    Private Sub cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCancel.Click
        saven = False
        Me.Close()
    End Sub
    Private Sub FldCodes_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles FldCodes.UserAddedRow
        Dim gr As DataGridView
        gr = DirectCast(sender, DataGridView)
        RemoveNullFromGrid(gr)
        ' seq nr: max + 1
        Dim maxn As Integer = 0
        For r As Integer = 0 To gr.CurrentRow.Index - 1
            If CInt(gr.Rows(r).Cells.Item(4).Value) > maxn Then
                maxn = CInt(gr.Rows(r).Cells.Item(4).Value)
            End If
        Next
        gr.CurrentRow.Cells.Item(4).Value = maxn + 1
        ' # entries: 1
        If CInt(gr.CurrentRow.Cells.Item(6).Value) = 0 Then
            gr.CurrentRow.Cells.Item(6).Value = 1
        End If
    End Sub
    Private Sub CollCodes_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles CollCodes.UserAddedRow
        Dim gr As DataGridView
        gr = DirectCast(sender, DataGridView)
        RemoveNullFromGrid(gr)
    End Sub
End Class