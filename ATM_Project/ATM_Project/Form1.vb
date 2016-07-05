Imports System.IO.Ports

Public Class Form1
    Dim SerialPort1 As New System.IO.Ports.SerialPort()

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SerialPort1.PortName = "COM6"
        SerialPort1.BaudRate = 9600
        SerialPort1.Parity = Parity.None
        SerialPort1.StopBits = StopBits.One
        SerialPort1.DataBits = 8
        SerialPort1.Handshake = Handshake.RequestToSend
        SerialPort1.DtrEnable = True
        SerialPort1.RtsEnable = True
        SerialPort1.NewLine = vbCrLf

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim message As String
        message = RichTextBox1.Text
        SerialPort1.Open()
        If SerialPort1.IsOpen() Then
            SerialPort1.Write("AT" & vbCrLf)
            SerialPort1.Write("AT+CMGF=1" & vbCrLf)
            SerialPort1.Write("AT+CMGS=" & Chr(34) & TextBox1.Text & Chr(34) & vbCrLf)
            SerialPort1.Write(message & Chr(26))
            MsgBox("sent")
        Else
            MsgBox("port not available")
        End If
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
        Form2.Show()

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Close()
    End Sub
End Class
