'
Public Class DelData
    Dim CurCollCodeMt As String
    Private Sub DelData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurCollCodeMt = CurCollCodeMtn
        Me.Text = SysMsg(951)
        Label1.Text = SysMsg(952) & CurCollCodeMt
        Button1.Text = SysMsg(951)
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DbConn As New OdbcConnection(SqlProv)
        Dim DeleteCursor As OdbcDataReader = Nothing
        Dim DbDeleteCursor As OdbcCommand
        Dim DeleteSql As String
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Try
            DbConn.Open()
        Catch e1 As Exception
            MsgBox(e1.ToString(), , SysMsg(66))
        End Try
        DeleteSql = "DELETE FROM data WHERE COLLCODE = '" & CurCollCodeMt & "'"
        DbDeleteCursor = New OdbcCommand(DeleteSql, DbConn)
        Try
            DeleteCursor = DbDeleteCursor.ExecuteReader()
        Catch e1 As Exception
            MsgBox(e1.ToString(), , SysMsg(953))
        Finally
            If Not (DeleteCursor Is Nothing) Then
                DeleteCursor.Close()
            End If
        End Try
        DeleteSql = "DELETE FROM searchterms WHERE COLLCODE = '" & CurCollCodeMt & "'"
        DbDeleteCursor = New OdbcCommand(DeleteSql, DbConn)
        Try
            DeleteCursor = DbDeleteCursor.ExecuteReader()
        Catch e1 As Exception
            MsgBox(e1.ToString(), , SysMsg(951))
        Finally
            If Not (DeleteCursor Is Nothing) Then
                DeleteCursor.Close()
            End If
        End Try
        Try
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        Catch ex As Exception
        End Try
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Close()
    End Sub
End Class
