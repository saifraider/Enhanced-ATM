Imports MySql.Data.MySqlClient
Imports System.Data.OleDb
Public Class transaction
    Dim str As String = "Server=localhost;User Id=root;Password=;database=ATM"
    Dim conn
    Dim myreader As MySqlDataReader
    Dim query As String
    Dim cmd As New MySqlCommand
    Dim adapter As New MySqlDataAdapter
    Dim a As String
    Dim b As Integer
    Dim d As Integer
    Dim balance As Integer
    Dim final As Integer


    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListBox1.Items.Clear()
        Dim sqltable As New DataTable
        Dim i As Integer
        conn = New MySqlConnection(str)
        query = "SELECT Balance from user where user_pin='" & Start.TextBox1.Text & "'"
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
                .Items.Add(sqltable.Rows(i)("Balance"))
            End With
        Next
        conn.Close()
        Dim cur_balance As String = ListBox1.Items(0).ToString
        Label3.Text = cur_balance
        final = ListBox1.Items(0).ToString
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click






        b = CInt(a)

        'Convert.ToInt32(ListBox1.Items[0].Value)

        d = CInt(final)
        balance = d - b
        'MsgBox(balance)
        If balance < 0 Then
            MsgBox("enter valid value")
            balance = balance + b
        Else
            final = balance
            Label3.Text = balance
            TextBox1.Enabled = False
            conn = New MySqlConnection(str)
            conn.Open()
            'query = "INSERT into User values('" & user_name & "')"
            query = "UPDATE user SET balance = '" & balance & "' WHERE user_pin= '" & Start.TextBox1.Text & "' "
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()
            'MessageBox.Show(" hash inserted")
            conn.Close()
            Button1.Visible = False
            TextBox1.Clear()
            TextBox1.Visible = False
            Label1.Visible = False
            Button2.Visible = True
            Button3.Visible = True
            MessageBox.Show("Thank you for using our ATM")
        End If
        'conn = New MySqlConnection(str)
        'conn.Open()
        ' Dim selcmd As New OleDbCommand("SELECT balance FROM user WHERE user_pin = '" & Start.TextBox1.Text & "'")
        'Dim reader As OleDbDataReader = selcmd.ExecuteReader()
        ' Dim a As String
        ' a = reader("balance").ToString()
        ' conn.Close()


    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        a = TextBox1.Text
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Start.TextBox1.Clear()
        Start.Show()
        'Application.Restart()
        Me.Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Close()
        Application.Exit()
        'Process.Start("C:\\Users\\ilf\\Documents\\Visual Studio 2010\\Projects\\FaceRecProOV\\bin\\Debug\\MultiFaceRec.exe")
    End Sub
End Class