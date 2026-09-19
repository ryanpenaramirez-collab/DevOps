Imports System.Globalization

Public Class Form1

    Private txtPantalla As TextBox
    Private primerNumero As Double = 0
    Private operacion As String = ""
    Private nuevaEntrada As Boolean = True

    ' ---------- Construcción de la interfaz ----------
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Calculadora"
        Me.ClientSize = New Size(286, 385)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(245, 245, 245)

        ' Pantalla
        txtPantalla = New TextBox()
        txtPantalla.Location = New Point(10, 12)
        txtPantalla.Width = 266
        txtPantalla.Font = New Font("Segoe UI", 20)
        txtPantalla.TextAlign = HorizontalAlignment.Right
        txtPantalla.ReadOnly = True
        txtPantalla.BackColor = Color.White
        txtPantalla.BorderStyle = BorderStyle.FixedSingle
        txtPantalla.Text = "0"
        Me.Controls.Add(txtPantalla)

        ' Botones
        Dim filas As String()() = {
            New String() {"C", "±", "←", "÷"},
            New String() {"7", "8", "9", "×"},
            New String() {"4", "5", "6", "-"},
            New String() {"1", "2", "3", "+"},
            New String() {"0", ".", "="}
        }

        Const ancho As Integer = 62
        Const alto As Integer = 55
        Const espacio As Integer = 6

        For i As Integer = 0 To filas.Length - 1
            Dim x As Integer = 10
            Dim y As Integer = 70 + i * (alto + espacio)

            For Each texto As String In filas(i)
                Dim btn As New Button()
                btn.Text = texto
                btn.Font = New Font("Segoe UI", 14)
                btn.Height = alto
                btn.Width = If(texto = "0", ancho * 2 + espacio, ancho)
                btn.Location = New Point(x, y)
                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderColor = Color.Silver
                btn.Cursor = Cursors.Hand

                ' Colores sencillos según el tipo de botón
                If texto = "=" Then
                    btn.BackColor = Color.FromArgb(0, 120, 215)
                    btn.ForeColor = Color.White
                ElseIf texto = "C" Then
                    btn.BackColor = Color.FromArgb(255, 205, 205)
                ElseIf "÷×-+±←".Contains(texto) Then
                    btn.BackColor = Color.FromArgb(230, 230, 230)
                Else
                    btn.BackColor = Color.White
                End If

                AddHandler btn.Click, AddressOf Boton_Click
                Me.Controls.Add(btn)

                x += btn.Width + espacio
            Next
        Next
    End Sub

    ' ---------- Lógica ----------
    Private Sub Boton_Click(sender As Object, e As EventArgs)
        Dim t As String = DirectCast(sender, Button).Text

        If Char.IsDigit(t(0)) Then
            ' Números
            If nuevaEntrada OrElse txtPantalla.Text = "0" Then
                txtPantalla.Text = t
            Else
                txtPantalla.Text &= t
            End If
            nuevaEntrada = False

        ElseIf t = "." Then
            If nuevaEntrada Then
                txtPantalla.Text = "0."
                nuevaEntrada = False
            ElseIf Not txtPantalla.Text.Contains(".") Then
                txtPantalla.Text &= "."
            End If

        ElseIf t = "C" Then
            txtPantalla.Text = "0"
            primerNumero = 0
            operacion = ""
            nuevaEntrada = True

        ElseIf t = "←" Then
            If nuevaEntrada Then Return
            Dim s As String = txtPantalla.Text
            s = s.Substring(0, s.Length - 1)
            If s = "" OrElse s = "-" Then s = "0"
            txtPantalla.Text = s

        ElseIf t = "±" Then
            If txtPantalla.Text = "0" OrElse txtPantalla.Text = "Error" Then Return
            If txtPantalla.Text.StartsWith("-") Then
                txtPantalla.Text = txtPantalla.Text.Substring(1)
            Else
                txtPantalla.Text = "-" & txtPantalla.Text
            End If

        ElseIf t = "=" Then
            If operacion <> "" Then
                Calcular()
                operacion = ""
                nuevaEntrada = True
            End If

        Else
            ' Operadores: ÷ × - +
            If operacion <> "" AndAlso Not nuevaEntrada Then Calcular()
            primerNumero = ObtenerValor()
            operacion = t
            nuevaEntrada = True
        End If
    End Sub

    Private Sub Calcular()
        Dim segundo As Double = ObtenerValor()
        Dim resultado As Double = 0

        Select Case operacion
            Case "+"
                resultado = primerNumero + segundo
            Case "-"
                resultado = primerNumero - segundo
            Case "×"
                resultado = primerNumero * segundo
            Case "÷"
                If segundo = 0 Then
                    txtPantalla.Text = "Error"
                    operacion = ""
                    nuevaEntrada = True
                    Return
                End If
                resultado = primerNumero / segundo
        End Select

        txtPantalla.Text = resultado.ToString(CultureInfo.InvariantCulture)
        primerNumero = resultado
    End Sub

    Private Function ObtenerValor() As Double
        Dim valor As Double
        If Double.TryParse(txtPantalla.Text, NumberStyles.Float, CultureInfo.InvariantCulture, valor) Then
            Return valor
        End If
        Return 0
    End Function

End Class