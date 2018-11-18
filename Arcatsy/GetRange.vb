Public Class GetRange
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        FromK.Text = FromK.Text.ToUpper(CultInf)
        ToK.Text = ToK.Text.ToUpper(CultInf)
        If FromK.Text.Trim = ToK.Text.Trim Then
            ToK.Text &= Chr(&HFF)
        End If
        Me.Close()
    End Sub

    Private Sub GetRange_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = SysMsg(101)
        Button1.Text = SysMsg(1)
        Label2.Text = SysMsg(102)
        Label3.Text = SysMsg(103)
    End Sub
End Class