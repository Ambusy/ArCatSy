'
Public Class Combos

    Dim DbConn As New OdbcConnection(SqlProv)
    Dim saven As Boolean
    Dim bmb As BindingManagerBase
    Dim dsCombo As New DataSet()
    Dim daCombo As New OdbcDataAdapter()
    Dim daComboRegels As New OdbcDataAdapter()
    Private Sub Combos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If saven Then daComboRegels.Update(dsCombo, "LINES")
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(506))
        End Try
        If Not (DbConn Is Nothing) Then
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End If
    End Sub

    Private Sub Combos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim SqlCmnd As OdbcCommand
        Dim SqlDbParam As OdbcParameter
        Me.Text = SysMsg(502)
        Me.DesktopLocation = New Point(0, 0)
        BSave.Text = SysMsg(3)
        BCancel.Text = SysMsg(2)
        saven = True

        SqlCmnd = New OdbcCommand("SELECT DISTINCT CmbCode FROM combo WHERE CmbCode NOT IN (SELECT DISTINCT FldCombo FROM collfields WHERE FldCombo <> '')" _
          & "UNION SELECT DISTINCT FldCombo FROM collfields WHERE FldCombo <> '' ", DbConn)
        daCombo.SelectCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("SELECT CmbCode, CmbOrder, CmbText FROM combo", DbConn)
        daComboRegels.SelectCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("UPDATE combo SET CmbText = ? WHERE CmbCode = ? AND CmbOrder = ?", DbConn)
        SqlCmnd.Parameters.Add("@CmbText", Odbc.OdbcType.VarChar, 255, "CmbText")
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCmbCode", Odbc.OdbcType.VarChar, 50, "CmbCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlDbParam = SqlCmnd.Parameters.Add("@OldCmbOrder", Odbc.OdbcType.Int, 4, "CmbOrder")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daComboRegels.UpdateCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("DELETE FROM combo WHERE CmbCode = ? AND CmbOrder = ?", DbConn)
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCmbCode", Odbc.OdbcType.VarChar, 50, "CmbCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlDbParam = SqlCmnd.Parameters.Add("@OldCmbOrder", Odbc.OdbcType.Int, 4, "CmbOrder")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daComboRegels.DeleteCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("INSERT INTO combo(CmbCode, CmbOrder, CmbText) VALUES(?,?,?)", DbConn)
        SqlCmnd.Parameters.Add("@CmbCode", Odbc.OdbcType.VarChar, 50, "CmbCode")
        SqlCmnd.Parameters.Add("@CmbOrder", Odbc.OdbcType.Int, 4, "CmbOrder")
        SqlCmnd.Parameters.Add("@CmbText", Odbc.OdbcType.VarChar, 255, "CmbText")
        daComboRegels.InsertCommand = SqlCmnd

        DbConn.Open()
        daCombo.Fill(dsCombo, "COMBOS")
        daComboRegels.Fill(dsCombo, "LINES")
        dsCombo.Relations.Add("CmbR", dsCombo.Tables("COMBOS").Columns("CmbCode"), dsCombo.Tables("LINES").Columns("CmbCode"))
        SCmb.DataSource = dsCombo
        SCmb.DataMember = "COMBOS"
        If SCmb.RowCount = 1 Then
            MsgBox(SysMsg(507))
            Me.Close()
        Else
            SCmb.AllowUserToAddRows = False
            SCmb.AllowUserToDeleteRows = False
            CmbDt.DataSource = dsCombo
            CmbDt.DataMember = "COMBOS.CmbR"
            bmb = Me.BindingContext(dsCombo, "COMBOS")
            bmb.Position = bmb.Count
            bmb.Position = 0
        End If
    End Sub
    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSave.Click
        Try
            daComboRegels.Update(dsCombo, "LINES")
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(506))
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCancel.Click
        saven = False
        Me.Close()
    End Sub
    Private Sub SCmb_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles SCmb.UserAddedRow
        Dim gr As DataGridView
        gr = DirectCast(sender, DataGridView)
        RemoveNullFromGrid(gr)
    End Sub

    Private Sub CmbDt_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles CmbDt.UserAddedRow
        Dim gr As DataGridView
        gr = DirectCast(sender, DataGridView)
        RemoveNullFromGrid(gr)
    End Sub
End Class
