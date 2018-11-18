'
Public Class GenRexx
    Dim DbConn As New OdbcConnection(SqlProv)
    Dim CurCollCodeMt As String
    ' haal hoogste CollSeq op
    Dim kSelectSQL As String = "SELECT FldCode, FldParent, FldCaption, FldMaxLength, FldMaxOccurs, FldCheckRegexp, FldCheckErrmsg, FldCombo, FldCaption FROM collfields WHERE CollCode = ? ORDER BY FldInputOrder"
    Dim DRkSelect As OdbcDataReader = Nothing
    Dim DbCkSelect As OdbcCommand
    Dim kSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim fSelectSQL As String = "SELECT EntitySelFields FROM collections WHERE CollCode = ?"
    Dim DRfSelect As OdbcDataReader = Nothing
    Dim DbCfSelect As OdbcCommand
    Dim fSelP1 As New OdbcParameter("@CollCode", Odbc.OdbcType.VarChar, 4)

    Dim EntitySelFields As String = ""
    Dim genFs As New Collection
    Private Sub GenRexx_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not (DbConn Is Nothing) Then
            If (DbConn.State = ConnectionState.Open) Then
                DbConn.Close()
            End If
        End If
    End Sub
    Private Sub GenRexx_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurCollCodeMt = CurCollCodeMtn
        Me.Text = SysMsg(359)
        Me.DesktopLocation = New Point(0, 0)
        Label1.Text = SysMsg(351)
        Label3.Text = SysMsg(352)
        Button1.Text = SysMsg(353)
        Button2.Text = SysMsg(354)

        Label2.Text = CurCollCodeMt
        Label4.Text = "DE " & CurCollCodeMt & ".rex"

        DbCkSelect = New OdbcCommand(kSelectSQL, DbConn)
        DbCkSelect.Parameters.Add(kSelP1)

        DbCfSelect = New OdbcCommand(fSelectSQL, DbConn)
        DbCfSelect.Parameters.Add(fSelP1)

    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            DbConn.Open()
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(66))
        End Try
        Try
            fSelP1.Value = CurCollCodeMt
            DRfSelect = DbCfSelect.ExecuteReader()
            While DRfSelect.Read()
                EntitySelFields = GetDbStringValue(DRfSelect, 0)
            End While
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(355))
        Finally
            If Not (DRfSelect Is Nothing) Then
                DRfSelect.Close()
            End If
        End Try
        genFs.Clear()
        Try
            kSelP1.Value = CurCollCodeMt
            DRkSelect = DbCkSelect.ExecuteReader()
            While DRkSelect.Read()
                Dim g As New GenDef
                g.FldCode = GetDbStringValue(DRkSelect, 0)
                g.FldParent = GetDbStringValue(DRkSelect, 1)
                g.FldCaption = GetDbStringValue(DRkSelect, 2)
                g.FldMaxLength = GetDbIntegerValue(DRkSelect, 3)
                g.FldMaxOccurs = GetDbIntegerValue(DRkSelect, 4)
                g.FldCheckRegexp = GetDbStringValue(DRkSelect, 5)
                g.FldCheckErrmsg = GetDbStringValue(DRkSelect, 6)
                g.FldCombo = GetDbStringValue(DRkSelect, 7)
                g.FldTitle = GetDbStringValue(DRkSelect, 8)
                genFs.Add(g)
            End While
        Catch ex As Exception
            MsgBox(ex.ToString(), , SysMsg(356))
        Finally
            If Not (DRkSelect Is Nothing) Then
                DRkSelect.Close()
            End If
        End Try
        DbConn.Close()
        DoDefine()
    End Sub
    Private Sub DoDefine()
        Dim st As Stream = File.Open(My.Application.Info.DirectoryPath & "\" & Label4.Text, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
        Wrtr = New StreamWriter(st)
        Wrt("/* DE " & CurCollCodeMt & " */")
        Wrt("trace n")
        Wrt("signal on novalue")
        Wrt("text=""TEXT""")
        Wrt("visible=""VISIBLE""")
        Wrt("erc = 0")
        Wrt("parse arg entry type fname")
        Wrt("select")
        Wrt("  when type = ""D"" then do")
        Wrt("    ""SIZE 10 20 700""")
        For Each d As GenDef In genFs
            If d.FldParent = String.Empty Then
                DefField(d, 1)
            End If
        Next
        Wrt("    ""SYSMSG 357""")
        Wrt("    imsg = SysMsg")
        Wrt("    ""SYSMSG 358""")
        Wrt("    mmsg = SysMsg")
        Wrt("  end")
        Wrt("  when type = ""I"" then do")
        Wrt("    ""INITDATA""")
        Wrt("    ""GETDATA msg""")
        Wrt("    msg.text = imsg")
        Wrt("    ""PUTDATA msg""")
        Wrt("  end")
        If EntitySelFields = "" Then EntitySelFields = DirectCast(genFs.Item(1), GenDef).FldCode
        Wrt("  when type = ""K"" then do")
        Wrt("    ""GETDATA msg""")
        Wrt("    ""SELECTKEY " & EntitySelFields & " ALL""")
        Wrt("    if rc = 1 then do")
        Wrt("      ""INITDATA""")
        Wrt("       msg.text = imsg")
        Wrt("    end")
        Wrt("    else if rc = 0 then do")
        Wrt("      ""READDATA""")
        Wrt("      msg.text = mmsg")
        Wrt("    end")
        Wrt("    ""PUTDATA msg""")
        Wrt("  end")
        Wrt("  when type = ""N"" then do ")
        Wrt("    ""READDATA""")
        Wrt("    msg.text = mmsg")
        Wrt("    ""PUTDATA msg""")
        Wrt("  end")
        Wrt("  when type = ""S"" then do")
        Wrt("    ""SAVE""")
        Wrt("  end")
        'Wrt("  when type = ""L"" then do")
        'Wrt("    if fname = ""????.?"" then do")
        'Wrt("      /* code statements for this field, if exit was activated*/")
        'Wrt("    end")
        'Wrt("  end")
        'Wrt("  when type = ""C"" then do")
        'Wrt("    if fname = ""????.?"" then do")
        'Wrt("      /* code statements for this field, if exit was activated */")
        'Wrt("    end")
        'Wrt("  end")
        Wrt("end")
        Wrt("exit erc")
        Wrtr.Close()
        st.Close()
        Me.Close()
    End Sub
    Private Sub DefField(ByRef d As GenDef, ByVal level As Integer)
        If d.FldCombo = "" Then
            If d.FldParent = "" Then
                Wrt("    ""DEFINE " & d.FldCode & " T " & CStr(d.FldMaxOccurs) & " " & CStr(d.FldMaxLength) & " n n " & d.FldCaption & """")
            Else
                Wrt("    ""DEFINE " & d.FldCode & " " & d.FldParent & " T " & CStr(d.FldMaxOccurs) & " " & CStr(d.FldMaxLength) & " n n " & d.FldCaption & """")
            End If
        Else
            If d.FldParent = "" Then
                Wrt("    ""DEFINE " & d.FldCode & " C " & CStr(d.FldMaxOccurs) & " " & CStr(d.FldMaxLength) & " " & d.FldCombo & " n n " & d.FldCaption & """")
            Else
                Wrt("    ""DEFINE " & d.FldCode & " " & d.FldParent & " C " & CStr(d.FldMaxOccurs) & " " & CStr(d.FldMaxLength) & " " & d.FldCombo & " n n " & d.FldCaption & """")
            End If
        End If
        For Each dc As GenDef In genFs
            If dc.FldParent = d.FldCode Then
                DefField(dc, level + 1)
            End If
        Next
    End Sub
End Class
