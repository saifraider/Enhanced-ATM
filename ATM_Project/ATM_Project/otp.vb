Imports System.IO.Ports
Imports MySql.Data.MySqlClient
Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.Text

Public Class otp
    Dim SerialPort1 As New System.IO.Ports.SerialPort()
    Dim str As String = "Server=localhost;User Id=root;Password=;database=ATM"
    Dim conn
    Dim myreader As MySqlDataReader
    Dim query As String
    Dim cmd As New MySqlCommand
    Dim adapter As New MySqlDataAdapter
    Dim mobile_no As String
    Dim value As Integer
    Dim hash2 As String
    Dim hash1 As String
    Dim Second As Integer
    Dim count As Integer


    Public Shared Function getMD5Hash(ByVal strToHash As String) As String
        Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider()
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = md5Obj.ComputeHash(bytesToHash)

        Dim strResult As String = ""
        Dim b As Byte

        For Each b In bytesToHash
            strResult += b.ToString("x2")
        Next

        Return strResult
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("1")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("2")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("3")
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("4")
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("5")
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("6")
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("7")
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("8")
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("9")
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        If (TextBox1.Text.Length <= 5) And CheckBox1.Checked = True Then TextBox1.AppendText("0")
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        ListBox2.Items.Clear()
        Dim sqltable As New DataTable
        Dim i As Integer
        conn = New MySqlConnection(str)
        query = "SELECT otp_hash from user where user_pin='" & Start.TextBox1.Text & "'"
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
            With ListBox2
                .Items.Add(sqltable.Rows(i)("otp_hash"))
            End With
        Next
        conn.Close()
        Dim otp_hash As String = ListBox2.Items(0).ToString

        hash2 = getMD5Hash(TextBox1.Text)
        If otp_hash.Equals(hash2) Then
            transaction.Show()
            Me.Close()
        Else
            MessageBox.Show("Enter Correct OTP")
        End If


    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        If (TextBox1.Text.Length > 0) Then TextBox1.Text = TextBox1.Text.Remove(TextBox1.Text.Length - 1)
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        Button11.Visible = True
        Button11.Enabled = True
        Button13.Enabled = False
        Button13.Visible = False

        CheckBox1.Checked = True
        '------------------------------------RANDOM NUMBER------------------------------------------------------'
        Randomize()
        value = CInt(Int(900000 * Rnd() + 100000))

        hash1 = getMD5Hash(value)

        

        conn = New MySqlConnection(str)
        conn.Open()
        'query = "INSERT into User values('" & user_name & "')"
        query = "UPDATE user SET otp_hash = '" & hash1 & "' WHERE user_pin= '" & Start.TextBox1.Text & "' "
        cmd = New MySqlCommand(query, conn)

        cmd.ExecuteNonQuery()
        'MessageBox.Show(" hash inserted")
        conn.Close()


        SerialPort1.Open()
        If SerialPort1.IsOpen() Then
            SerialPort1.Write("AT" & vbCrLf)
            SerialPort1.Write("AT+CMGF=1" & vbCrLf)
            'SerialPort1.Write("AT+CMGS=" & Chr(34) & "'9820640601'" & Chr(34) & vbCrLf)
            SerialPort1.Write("AT+CMGS=" & Chr(34) & mobile_no & Chr(34) & vbCrLf) ' <------ check syntax of mobile no.
            SerialPort1.Write(value & Chr(26))
            MsgBox("sent")
            SerialPort1.Close()
        Else
            MsgBox("port not available")
        End If
        MessageBox.Show(value)
        Timer1.Interval = 1000
        Timer1.Start()
        'MsgBox("timer started")
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        count = count + 1
        If count.Equals(1) Then
            Label2.Visible = True
            Label3.Visible = True
            Label4.Visible = True
        End If
        If count.Equals(2) Then
            Label2.Visible = True
            Label3.Text = "1"
            Label4.Visible = True
        End If
        If count.Equals(3) Then
            Label2.Visible = True
            Label3.Text = "0"
            Label4.Visible = True
        End If
        Button14.Visible = False
        Button14.Enabled = False
        Button11.Enabled = True

        Randomize()
        value = CInt(Int((900000 * Rnd()) + 100000))
        MessageBox.Show(value)
        hash1 = getMD5Hash(value)
        'MessageBox.Show(hash)
        'Button13.Enabled = False


        conn = New MySqlConnection(str)
        conn.Open()
        'query = "INSERT into User values('" & user_name & "')"
        query = "UPDATE user SET otp_hash = '" & hash1 & "' WHERE user_pin= '" & Start.TextBox1.Text & "' "
        cmd = New MySqlCommand(query, conn)

        cmd.ExecuteNonQuery()
        'MessageBox.Show(" hash inserted")
        conn.Close()


        SerialPort1.Open()
        If SerialPort1.IsOpen() Then
            SerialPort1.Write("AT" & vbCrLf)
            SerialPort1.Write("AT+CMGF=1" & vbCrLf)
            SerialPort1.Write("AT+CMGS=" & Chr(34) & mobile_no & Chr(34) & vbCrLf)   ' <------ check syntax of mobile no.
            SerialPort1.Write(value & Chr(26))
            MsgBox("sent")
            SerialPort1.Close()
        Else
            MsgBox("port not available")
        End If

        Timer1.Interval = 1000
        Timer1.Start()


        ' Timer1.Interval = 1000
        'Timer1.Start()
        'MsgBox("timer started")
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub otp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        count = 0
        Label2.Visible = False
        Label3.Visible = False
        Label4.Visible = False
        Button11.Enabled = False
        SerialPort1.PortName = "COM6"
        SerialPort1.BaudRate = 9600
        SerialPort1.Parity = Parity.None
        SerialPort1.StopBits = StopBits.One
        SerialPort1.DataBits = 8
        SerialPort1.Handshake = Handshake.RequestToSend
        SerialPort1.DtrEnable = True
        SerialPort1.RtsEnable = True
        SerialPort1.NewLine = vbCrLf

        ListBox1.Items.Clear()
        Dim sqltable As New DataTable
        Dim i As Integer
        conn = New MySqlConnection(str)
        query = "SELECT user_mob from user where user_pin='" & Start.TextBox1.Text & "'"
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
                .Items.Add(sqltable.Rows(i)("user_mob"))
            End With
        Next
        conn.Close()
        mobile_no = ListBox1.Items(0).ToString



    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Second = Second + 1
        If Second >= 30 Then

            Button14.Enabled = True
            Button14.Visible = True
            Button11.Enabled = False
            Timer1.Stop() 'Timer stops functioning
            Second = 0

            If count.Equals(3) Then
                Button14.Enabled = False
                Button14.Visible = False
                Button11.Enabled = False
                Button15.Visible = True
            End If
            'If count.Equals(0) then 
            MsgBox("Timer Stopped....")
        End If
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        Start.TextBox1.Clear()
        Start.Show()
        Me.Close()
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        TextBox1.Clear()

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub
End Class