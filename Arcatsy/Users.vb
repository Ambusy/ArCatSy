'

Public Class Users
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim saven As Boolean
    Dim bmb As BindingManagerBase
    Dim dsCollectie As New DataSet()
    Dim daCollectie As New OdbcDataAdapter()
    Private Sub Collecties_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If saven Then
                Form1.Idxen.Visible = True
                daCollectie.Update(dsCollectie, "USER")
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
        Me.Text = "Users"
        Me.DesktopLocation = New Point(0, 0)
        BSave.Text = SysMsg(3)
        BCancel.Text = SysMsg(2)
        saven = True

        SqlCmnd = New OdbcCommand("SELECT name, pswd,  role FROM users", DbConn)
        daCollectie.SelectCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("DELETE FROM Users WHERE name = ?", DbConn)
        SqlDbParam = SqlCmnd.Parameters.Add("@oldname", Odbc.OdbcType.VarChar, 4, "name")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daCollectie.DeleteCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("INSERT INTO Users(name, pswd, access, role) VALUES(?, ?, NOW(), ?)", DbConn)
        SqlCmnd.Parameters.Add("@name", Odbc.OdbcType.VarChar, 32, "name")
        SqlCmnd.Parameters.Add("@pswd", Odbc.OdbcType.Int, 32, "pswd")
        SqlCmnd.Parameters.Add("@role", Odbc.OdbcType.Text, 1, "role")
        daCollectie.InsertCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("DELETE FROM Users WHERE name = ?", DbConn)
        SqlDbParam = SqlCmnd.Parameters.Add("@oldname", Odbc.OdbcType.VarChar, 4, "name")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daCollectie.UpdateCommand = SqlCmnd

        DbConn.Open()
        daCollectie.Fill(dsCollectie, "USER")
        CollCodes.DataSource = dsCollectie
        CollCodes.DataMember = "USER"
        bmb = Me.BindingContext(dsCollectie, "USER")
        bmb.Position = bmb.Count
        bmb.Position = 0
    End Sub
    Private Sub BSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSave.Click
        Try
            daCollectie.Update(dsCollectie, "USER")
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Sub BCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCancel.Click
        saven = False
        Me.Close()
    End Sub
End Class
