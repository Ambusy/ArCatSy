Public Class Maintenance
    Friend PrtSpecC As New Collection
    Private Sub BColl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BColl.Click
        Dim f As New Collecties
        f.Show()
    End Sub
    Private Sub BCombos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCombos.Click
        Dim f As New Combos
        f.Show()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BIdx.Click
        Dim f As New TeIndex
        f.Show()
    End Sub
    Private Sub Maintenance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Form1.CollNamesC.Clear()
        ReadCursorCollection(Form1.CollNamesC, "SELECT CollCode, CollName FROM collections", "S1 S0", " | ", True)
        CollNames.Items.Clear()
        LPrtSpec.Items.Clear()
        For Each s As String In Form1.CollNamesC
            CollNames.Items.Add(s)
        Next
        ReadCursorCollection(PrtSpecC, "SELECT DISTINCT PrtSpec FROM prtspecs", "S0", " ", True)
        For Each s As String In PrtSpecC
            LPrtSpec.Items.Add(s)
        Next
    End Sub
    Private Sub Maintenance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = SysMsg(456)
        Me.DesktopLocation = New Point(0, 0)
        BColl.Text = SysMsg(451)
        Label1.Text = SysMsg(451)
        BCombos.Text = SysMsg(452)
        BIdx.Text = SysMsg(453)
        BPSpec.Text = SysMsg(454)
        Label2.Text = SysMsg(454)
        GenRex.Text = SysMsg(455)
        BImp.Text = SysMsg(457)
        Bexp.Text = SysMsg(458)
        BImpPrt.Text = SysMsg(459)
        BExpPrt.Text = SysMsg(460)
        BDelData.Text = SysMsg(951)
        Dim login As String = SysMsg(212)
        If login.Substring(6) = "YES" Then
            UserBt.Visible = True
        Else
            UserBt.Visible = False
        End If

        'For Each s As String In Form1.CollNamesC
        '    CollNames.Items.Add(s)
        'Next
        'ReadCursorCollection(PrtSpecC, "SELECT DISTINCT PrtSpec FROM prtspecs", "S0", " ", True)
        'For Each s As String In PrtSpecC
        '    LPrtSpec.Items.Add(s)
        'Next
        CurCollCodeMtn = ""
        CurPrtSpecMtn = ""
    End Sub
    Private Sub GenRex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GenRex.Click
        If CollNames.SelectedIndex >= 0 Then
            Dim f As New GenRexx
            f.Show()
        Else
            MsgBox(SysMsg(301))
        End If
    End Sub
    Private Sub CollNames_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollNames.SelectedIndexChanged
        If CollNames.SelectedIndex >= 0 Then CurCollCodeMtn = CStr(CollNames.Items(CollNames.SelectedIndex)).Split("|"c)(1).Trim
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BPSpec.Click
        Dim f As New PrtSpc
        f.Show()
    End Sub
    Private Sub BRegExp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BRegExp.Click
        Dim f As New Regexp
        f.Show()
    End Sub
    Private Sub Bexp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bexp.Click
        If CollNames.SelectedIndex >= 0 Then
            TypeExpInpMenu = "Exp"
            Dim f As New ExpData
            f.Show()
        Else
            MsgBox(SysMsg(301))
        End If
    End Sub
    Private Sub BImp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BImp.Click
        '  If CollNames.SelectedIndex >= 0 Then
        TypeExpInpMenu = "Imp"
        Dim f As New ExpData
        f.Show()
        '  Else
        '   MsgBox(SysMsg(301))
        '  End If
    End Sub
    Private Sub BExpPrt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BExpPrt.Click
        If LPrtSpec.SelectedIndex >= 0 Then
            TypeExpInpMenu = "Exp"
            Dim f As New ExpPrt
            f.Show()
        Else
            MsgBox(SysMsg(461))
        End If
    End Sub
    Private Sub BImpPrt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BImpPrt.Click
        If LPrtSpec.SelectedIndex >= 0 Then
            TypeExpInpMenu = "Imp"
            Dim f As New ExpPrt
            f.Show()
        Else
            MsgBox(SysMsg(461))
        End If
    End Sub
    Private Sub LPrtSpec_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LPrtSpec.SelectedIndexChanged
        CurPrtSpecMtn = CStr(LPrtSpec.Items(LPrtSpec.SelectedIndex))
    End Sub
    Private Sub BDelData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BDelData.Click
        If CollNames.SelectedIndex >= 0 Then
            Dim f As New DelData
            f.Show()
        Else
            MsgBox(SysMsg(461))
        End If
    End Sub

    Private Sub UserBt_Click(sender As Object, e As EventArgs) Handles UserBt.Click
        Dim f As New Users
        f.Show()
    End Sub
End Class
