Imports System.Windows.Forms
Public Class SelectKey
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        CollSeqSel = 0
        Me.Hide()
    End Sub
    Private Sub KeyList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles KeyList.Click
        Dim s As String = CStr(KeyList.SelectedItem)
        Dim i As Integer = InStrRev(s, "|")
        s = s.Substring(i)
        CollSeqSel = CInt(s)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub
    Friend Sub KeyList_Prev()
        If KeyList.SelectedIndex > 0 Then
            KeyList.SelectedIndex -= 1
            Dim sn As String = CStr(KeyList.SelectedItem)
            Dim i As Integer = InStrRev(sn, "|")
            sn = sn.Substring(i)
            CollSeqSel = CInt(sn)
        End If
    End Sub
    Friend Sub KeyList_Next()
        If (KeyList.SelectedIndex + 1) < KeyList.Items.Count Then
            KeyList.SelectedIndex += 1
            Dim sn As String = CStr(KeyList.SelectedItem)
            Dim i As Integer = InStrRev(sn, "|")
            sn = sn.Substring(i)
            CollSeqSel = CInt(sn)
        End If
    End Sub
    Private Sub SelectKey_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = SysMsg(325)
        OK_Button.Text = SysMsg(1)
        Cancel_Button.Text = SysMsg(2)
        BNew.Text = SysMsg(62)
        KeyList.Select()
    End Sub
    Private Sub BNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BNew.Click
        CollSeqSel = -1 ' new entry
        Me.Hide()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        KeyList.Items.Clear()
        CollSeqSel = -2 ' new list
        Me.Hide()
    End Sub
End Class
