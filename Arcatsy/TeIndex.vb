'
Public Class TeIndex
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim saven As Boolean
    Dim bmb As BindingManagerBase
    Dim dsCollectie As New DataSet()
    Dim daCollectie As New OdbcDataAdapter()
    Private Sub Collecties_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If saven Then
                Form1.Idxen.Visible = True
                daCollectie.Update(dsCollectie, "TEINDEX")
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
        Me.Text = SysMsg(901)
        Me.DesktopLocation = New Point(0, 0)
        Label1.Text = SysMsg(902)
        BSave.Text = SysMsg(3)
        BCancel.Text = SysMsg(2)
        saven = True

        SqlCmnd = New OdbcCommand("SELECT CollCode, CollSeq FROM tobeindexed", DbConn)
        daCollectie.SelectCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("DELETE FROM tobeindexed WHERE CollCode = ? AND CollSeq = ?", DbConn)
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        SqlDbParam = SqlCmnd.Parameters.Add("@oldCollSeq", Odbc.OdbcType.Int, 4, "CollSeq")
        SqlDbParam.SourceVersion = DataRowVersion.Original
        daCollectie.DeleteCommand = SqlCmnd

        SqlCmnd = New OdbcCommand("INSERT INTO tobeindexed(CollCode, CollSeq) VALUES(?, ?)", DbConn)
        SqlCmnd.Parameters.Add("@CollCode", Odbc.OdbcType.VarChar, 4, "CollCode")
        SqlCmnd.Parameters.Add("@CollSeq", Odbc.OdbcType.Int, 4, "CollSeq")
        daCollectie.InsertCommand = SqlCmnd

        DbConn.Open()
        daCollectie.Fill(dsCollectie, "TEINDEX")
        CollCodes.DataSource = dsCollectie
        CollCodes.DataMember = "TEINDEX"
        bmb = Me.BindingContext(dsCollectie, "TEINDEX")
        bmb.Position = bmb.Count
        bmb.Position = 0
    End Sub
    Private Sub BSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BSave.Click
        Try
            daCollectie.Update(dsCollectie, "TEINDEX")
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Sub BCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCancel.Click
        saven = False
        Me.Close()
    End Sub
End Class
