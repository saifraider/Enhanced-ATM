Imports MySql.Data.MySqlClient
Imports System.Data.OleDb



Public Class Form2

    Dim str As String = "Server=localhost;User Id=root;Password=;database=ATM"

    Dim conn
    Dim myreader As MySqlDataReader
    Dim query As String

    Dim cmd As New MySqlCommand
    Dim adapter As New MySqlDataAdapter


    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListBox1.Items.Clear()
        Dim sqltable As New DataTable
        Dim i As Integer
        'b = "SELECT sum(Amount) FROM bill_product where BID='" & a & "'"
        'MessageBox.Show(b)
        conn = New MySqlConnection(str)
        query = "SELECT user_mob from user where user_pin='" & Start.TextBox1.Text & "'"
        ' MessageBox.Show("abcd")



        conn.Open()
        With cmd
            .CommandText = query
            .Connection = conn
        End With
        With adapter
            .SelectCommand = cmd
            .Fill(sqltable)
        End With
        ' MessageBox.Show(sqltable.Rows.Count)
        For i = 0 To sqltable.Rows.Count - 1
            With ListBox1
                .Items.Add(sqltable.Rows(i)("user_mob"))
                ' With .Items(.Items.Count - 1).SubItems
                '.Add(sqltable.Rows(i)("Quantity"))
                ' .Add(sqltable.Rows(i)("user_mob"))


                ' End With
            End With
        Next
       



        ' query = "INSERT into User values('" & user_name & "','" & user_mob & "')"

        'cmd = New MySqlCommand(query, conn)

        '  cmd.ExecuteNonQuery()
        ' MessageBox.Show("inserted")
        conn.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim user_name As String = TextBox1.Text
        ' Dim user_mob As String = TextBox2.Text

        'conn = New MySqlConnection(str)
        'Dim user_mob As String


    End Sub



  

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

  
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim display As String
        display = ListBox1.Text
        'MsgBox(display)
        TextBox4.Text = display

    End Sub
End Class