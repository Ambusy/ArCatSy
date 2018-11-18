Public Class Regexp
    Private Sub Regexp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = SysMsg(751)
        Label2.Text = SysMsg(752)
        Label3.Text = SysMsg(753)
        Button1.Text = SysMsg(754)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim s As String
        Res.Items.Clear()
        Try
            Dim regExpre As New Regex(Tregexp.Text.Trim)
            Dim mc As MatchCollection
            Try
                mc = regExpre.Matches(TText.Text)
                For Each m As Match In mc
                    s = m.Value.Trim().ToUpper(CultInf)
                    If s.Length() > 0 Then
                        Res.Items.Add(s)
                    End If
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
