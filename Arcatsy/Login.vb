'
Public Class Login
    Public correct As Boolean = False
    Dim DbConn As New OdbcConnection(SqlProv)
    ' haal user data op
    Dim cSelectSQL As String = "SELECT Pswd FROM Users WHERE Name = ?"
    Dim DRcSelect As OdbcDataReader = Nothing
    Dim DbCcSelect As OdbcCommand
    Dim cSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DbConn.Open()
        Try ' get user
            cSelP1.Value = Username.Text
            DRcSelect = DbCcSelect.ExecuteReader()
            correct = False
            Do While (DRcSelect.Read())
                Dim pswd As String = GetDbStringValue(DRcSelect, 0)
                If pswd = Password.Text Then
                    correct = True
                End If
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If Not (DRcSelect Is Nothing) Then
                DRcSelect.Close()
            End If
        End Try
        DbConn.Close()
        Me.Close()

    End Sub
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DbCcSelect = New OdbcCommand(cSelectSQL, DbConn)
        DbCcSelect.Parameters.Add(cSelP1)
    End Sub

    Private Sub Password_KeyUp(sender As Object, e As KeyEventArgs) Handles Password.KeyUp
        If e.KeyCode = 13 Then
            Button1_Click(sender, New EventArgs)
        End If
    End Sub

End Class