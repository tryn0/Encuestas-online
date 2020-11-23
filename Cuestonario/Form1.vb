Public Class Form1
    Dim path As String = "C:\Users\Guillermo\Desktop\encuesta.txt" 'Dirección del archivo txt de encuestas
    Dim pathR As String = "C:\Users\Guillermo\Desktop\respuestas.txt" 'Dirección del archivo txt de respuestas
    Dim kv As New Dictionary(Of String, List(Of String))
    Dim kv2 As New Dictionary(Of String, List(Of String))
    Dim encuesta As New Dictionary(Of String, List(Of String))
    Dim respuestas As New Dictionary(Of String, String)
    Dim nEncuestas As Integer = 0
    Dim nPreguntas As Integer = 0
    Dim nEncuestasLeidas As Integer = 0
    Dim nSi As Integer = 0
    Dim nNo As Integer = 0
    Dim nNs As Integer = 0
    Dim posicionActualPreguntas As Integer = 0
    Dim encuestaActual As String
    Dim grabado As Boolean = False
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If CheckBox1.Checked Then
            MsgBox("Vas a salir de la app")
        ElseIf CheckBox2.Checked Then
            MsgBox("Antes de salir se van a grabar las encuestas")
            If respuestas.Keys.Count > 0 Then
                For Each line As KeyValuePair(Of String, String) In respuestas
                    grabarRespuestas(pathR, line.Key, line.Value)
                Next
            End If
        End If
        Me.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Bloquea todos los controls, menos el boton salir
        'Lee el archivo de encuestas, y carga dichas preguntas en memoria para poder usarlas en la app
        For Each ctrl In Me.Controls
            disable(ctrl)
        Next
        Button10.Enabled = True
        'Carga de archivo de encuestas
        leerTxt()
    End Sub

    Private Sub disable(ctrl As Control)
        'Función para bloquear controls
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        ComboBox1.Items.Clear()
        If TypeOf ctrl Is GroupBox Then
            Dim ctrl2 As Control
            For Each ctrl2 In ctrl.Controls
                disable(ctrl2)
            Next
        ElseIf TypeOf ctrl Is Button Then
            ctrl.Enabled = False
        ElseIf TypeOf ctrl Is RadioButton Then
            ctrl.Enabled = False
        ElseIf TypeOf ctrl Is CheckBox Then
            ctrl.Enabled = False
        ElseIf TypeOf ctrl Is TextBox Then
            ctrl.Enabled = False
            ctrl.Text = ""
        ElseIf TypeOf ctrl Is ComboBox Then
            ctrl.Enabled = False
        End If
    End Sub

    Private Sub enableGB(gb As GroupBox)
        'Función para habilitar groupbox
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        ComboBox1.Items.Clear()
        For Each ctrl In Me.Controls
            disable(ctrl)
        Next
        Button10.Enabled = True
        For Each ctrl In gb.Controls
            If TypeOf ctrl Is GroupBox Then
                Dim ctrl2 As Control
                For Each ctrl2 In ctrl.Controls
                    ctrl2.Enabled = True
                Next
            Else
                ctrl.Enabled = True
            End If
        Next
        If grabado = True Then
            Button9.Enabled = False
        End If
    End Sub
    Private Sub PreguntasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreguntasToolStripMenuItem.Click
        enableGB(GroupBox1)
        For Each preguntas As KeyValuePair(Of String, List(Of String)) In kv
            ComboBox1.Items.Add(preguntas.Key)
            ComboBox1.SelectedIndex = 0
        Next
    End Sub

    Private Sub AvisoSalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AvisoSalirToolStripMenuItem.Click
        enableGB(GroupBox2)
    End Sub

    Private Sub IniciarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IniciarToolStripMenuItem.Click 'Iniciar encuesta
        If kv.Values.Count > 0 Then
            Label4.Text = "Total encuestas " + kv.Keys.Count.ToString()
            enableGB(GroupBox3)
            ListBox1.Items.Clear()
            For Each encuesta As KeyValuePair(Of String, List(Of String)) In kv
                If encuesta.Value.Count > 0 Then
                    ListBox1.Items.Add(encuesta.Key)
                End If
            Next
        End If
    End Sub

    Private Sub leerTxt()
        'Función para leer archivo de encuestas
        If My.Computer.FileSystem.FileExists(path) Then
            For Each line As String In IO.File.ReadAllLines(path)
                Dim lista As New List(Of String)
                Dim parts() As String = line.Split("-")
                For Each preguntas As String In parts(1).Split(",")
                    lista.Add(preguntas)
                Next
                kv.Add(parts(0), lista)
            Next
            For Each line As KeyValuePair(Of String, List(Of String)) In kv
                nEncuestas = nEncuestas + 1
                For Each pregunta As String In line.Value
                    nPreguntas = nPreguntas + 1
                Next
            Next
        Else
            MsgBox("No existe el archivo en la ruta:" & vbLf & path & vbLf & "Por lo que no cargará las encuestas, revíselo")
        End If
    End Sub

    Private Sub cargarUltimaPregunta(encuesta As String)
        Label3.Text = "Total preguntas " + (kv.Item(encuesta).Count).ToString()
        TextBox1.Text = ""
        If kv.Item(encuesta).Count > 0 Then
            TextBox1.Text = kv.Item(encuesta)(kv.Item(encuesta).Count - 1)
        Else
            TextBox1.Text = "Escriba una pregunta"
        End If
        posicionActualPreguntas = kv.Item(encuesta).Count - 1
        encuestaActual = encuesta
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'Alta
        If TextBox1.TextLength < 1 Then
            MsgBox("Escribe pregunta")
        Else
            If kv.Item(encuestaActual).Count < 14 Then
                kv.Item(encuestaActual).Add(TextBox1.Text)
            Else
                MsgBox("Máximo de preguntas alcanzado")
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click 'Anterior
        If posicionActualPreguntas > 0 And kv.Item(encuestaActual).Count > posicionActualPreguntas - 1 Then
            posicionActualPreguntas = posicionActualPreguntas - 1
            TextBox1.Text = kv.Item(encuestaActual)(posicionActualPreguntas)
        Else
            MsgBox("No hay mas arriba")
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click 'Siguiente
        If posicionActualPreguntas < kv.Item(encuestaActual).Count And kv.Item(encuestaActual).Count > posicionActualPreguntas + 1 Then
            posicionActualPreguntas = posicionActualPreguntas + 1
            TextBox1.Text = kv.Item(encuestaActual)(posicionActualPreguntas)
        Else
            MsgBox("No hay mas abajo")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click 'Baja
        If kv.Item(encuestaActual).Count > 0 Then
            kv.Item(encuestaActual).RemoveAt(posicionActualPreguntas)
            If kv.Item(encuestaActual).Count > 0 Then
                If posicionActualPreguntas < kv.Item(encuestaActual).Count - 1 Then
                    posicionActualPreguntas = posicionActualPreguntas + 1
                ElseIf posicionActualPreguntas > kv.Item(encuestaActual).Count - 1 Then
                    posicionActualPreguntas = posicionActualPreguntas - 1
                End If
                TextBox1.Text = kv.Item(encuestaActual)(posicionActualPreguntas)
            Else
                    TextBox1.Text = ""
            End If
        Else
            MsgBox("No hay preguntas que dar de baja")
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        ListBox2.Items.Clear()
        Label5.Text = "Encuesta actual " + ListBox1.SelectedItem.ToString()
        Dim key As String = ListBox1.SelectedItem.ToString()
        For Each preguntas As String In kv.Item(key)
            ListBox2.Items.Add(preguntas)
        Next
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        cargarUltimaPregunta(ComboBox1.SelectedItem.ToString())
    End Sub

    Private Sub TextBox1_GotFocus(sender As Object, e As EventArgs) Handles TextBox1.GotFocus
        If TextBox1.Text = "Escriba una pregunta" Then
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox1_LostFocus(sender As Object, e As EventArgs) Handles TextBox1.LostFocus
        If TextBox1.Text = "" Then
            TextBox1.Text = "Escriba una pregunta"
        End If
    End Sub

    Function radioButton(rb As RadioButton)
        If rb.Checked = True Then
            If respuestas.ContainsKey(ListBox2.SelectedItem.ToString()) Then
                ' Si existe actualizar respuesta
                respuestas.Item(ListBox2.SelectedItem.ToString()) = rb.Text
            Else
                ' Si no existe la pregunta en el txt la introduzco y añado la respuesta
                respuestas.Add(ListBox2.SelectedItem.ToString(), rb.Text)
            End If
        End If
    End Function
    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        radioButton(RadioButton1)
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        radioButton(RadioButton2)
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        radioButton(RadioButton3)
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged ' Seteo de radiobutton si item seleccionado ya tiene respuesta
        RadioButton1.Checked = False
        RadioButton2.Checked = False
        RadioButton3.Checked = False
        If respuestas.ContainsKey(ListBox2.SelectedItem.ToString()) Then
            Select Case respuestas.Item(ListBox2.SelectedItem.ToString())
                Case "Sí"
                    RadioButton1.Checked = True
                Case "No"
                    RadioButton2.Checked = True
                Case "NS"
                    RadioButton3.Checked = True
            End Select
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click ' Grabar
        For Each line As KeyValuePair(Of String, String) In respuestas
            grabarRespuestas(pathR, line.Key, line.Value)
        Next
    End Sub
    Function grabarRespuestas(ruta As String, pregunta As String, respuesta As String) ' Graba en txt las respuestas
        If Not My.Computer.FileSystem.FileExists(pathR) Then ' Si existe el archivo
            IO.File.Create(pathR).Dispose()
        End If
        If IO.File.ReadAllLines(pathR).Length > 0 Then ' Si hay algo escrito
            ' Si existe la pregunta añadir respuesta
            Dim contador As Integer = 0
            Dim preguntaExiste As Boolean = False
            Dim lines() As String = IO.File.ReadAllLines(pathR)
            For Each line As String In lines
                Dim parts() As String = line.Split("-")
                Dim preguntaEscrita As String = parts(0)
                If preguntaEscrita = pregunta Then
                    preguntaExiste = True
                    parts(1) = parts(1) + "," + respuesta
                    lines(contador) = parts(0) + "-" + parts(1)
                    IO.File.WriteAllLines(pathR, lines)
                End If
                contador = contador + 1
            Next
            If preguntaExiste = False Then
                Using sw As IO.StreamWriter = IO.File.AppendText(pathR)
                    sw.Write(vbLf + pregunta + "-" + respuesta)
                End Using
            End If
        Else ' Si no hay NADA escrito, solo añadir la pregunta y la respuesta
            Using sw As IO.StreamWriter = IO.File.AppendText(pathR)
                sw.Write(vbLf + pregunta + "-" + respuesta)
            End Using
        End If
        grabado = True
        Button9.Enabled = False
    End Function

    Private Sub ResultadoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResultadoToolStripMenuItem.Click ' Resultado
        ' Muestra las estadísticas de las encuestas respondidas
        If respuestas.Keys.Count > 0 Then
            For Each line As KeyValuePair(Of String, String) In respuestas
                nEncuestasLeidas = nEncuestasLeidas + 1
                If line.Value = "Sí" Then
                    nSi = nSi + 1
                ElseIf line.Value = "No" Then
                    nNo = nNo + 1
                ElseIf line.Value = "NS" Then
                    nNs = nNs + 1
                End If
            Next
            Label6.Text = Label6.Text + nEncuestasLeidas.ToString()
            Label7.Text = Label7.Text + nSi.ToString() + " " + String.Format("{0:0.00}", ((nSi / (nSi + nNo + nNs)) * 100)) + "%"
            Label8.Text = Label8.Text + nNo.ToString() + " " + String.Format("{0:0.00}", ((nNo / (nSi + nNo + nNs)) * 100)) + "%"
            Label9.Text = Label9.Text + nNs.ToString() + " " + String.Format("{0:0.00}", ((nNs / (nSi + nNo + nNs)) * 100)) + "%"
        End If
    End Sub

    Private Sub GrabarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GrabarToolStripMenuItem.Click
        If grabado = False Then
            For Each line As KeyValuePair(Of String, String) In respuestas
                grabarRespuestas(pathR, line.Key, line.Value)
            Next
        End If
    End Sub
End Class