Imports System.Diagnostics
Imports MySql.Data.MySqlClient
Imports System.Data.OleDb

Public Class Start
    Dim str As String = "Server=localhost;User Id=root;Password=;database=ATM"
    Dim conn
    Dim myreader As MySqlDataReader
    Dim query As String
    Dim cmd As New MySqlCommand
    Dim adapter As New MySqlDataAdapter
    Dim name1 As String
    Dim pin1 As String
    Public Pin As String
   
    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListBox1.Visible = True
        ListBox2.Visible = True
        ListBox1.Items.Clear()
        Dim sqltable As New DataTable
        Dim i As Integer
        conn = New MySqlConnection(str)
        query = "SELECT face from store" 'where user_pin='" & TextBox1.Text & "'"
        conn.Open()
        With cmd
            .CommandText = query
            .Connection = conn
        End With
        With adapter
            .SelectCommand = cmd
            .Fill(sqltable)
        End With
        For i = 0 To sqltable.Rows.Count - 1
            With ListBox1
                .Items.Add(sqltable.Rows(i)("face"))
            End With
        Next
        conn.Close()
        name1 = ListBox1.Items(0).ToString

        ListBox2.Items.Clear()
        Dim sqltable1 As New DataTable
        Dim j As Integer
        conn = New MySqlConnection(str)
        query = "SELECT user_pin from user where user_name='" & name1 & "'"
        conn.Open()
        With cmd
            .CommandText = query
            .Connection = conn
        End With
        With adapter
            .SelectCommand = cmd
            .Fill(sqltable1)
        End With
        For j = 0 To sqltable1.Rows.Count - 1
            With ListBox2
                .Items.Add(sqltable1.Rows(j)("user_pin"))
            End With
        Next
        conn.Close()
        pin1 = ListBox2.Items(0).ToString

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("1")
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("2")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("3")
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("4")
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("5")
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("6")
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("7")
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("8")
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("9")
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        If (TextBox1.Text.Length <= 5) Then TextBox1.AppendText("0")
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click

        If (TextBox1.Text.Length > 0) Then TextBox1.Text = TextBox1.Text.Remove(TextBox1.Text.Length - 1)
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Pin = TextBox1.Text
        If TextBox1.Text.Length <= 5 Then
            MsgBox("Invalid PIN")
        ElseIf pin1.Equals(TextBox1.Text) Then
            Me.Hide()
            otp.Show()
        Else
            MessageBox.Show("enter correct PIN")
            ' Form4.Show()

        End If

    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        TextBox1.Clear()

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub
End Class