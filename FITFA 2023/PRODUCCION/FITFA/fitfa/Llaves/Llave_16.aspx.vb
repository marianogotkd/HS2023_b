﻿Public Class Llave_16
    Inherits System.Web.UI.Page
    Dim DAllave As New Capa_de_datos.Llave
    Dim DAinscripcion As New Capa_de_datos.Inscripciones
    Dim categoria_id As Integer = 0
    Dim evento_id As Integer = 0
    Dim llave_id As Integer = 0
    Dim Llaves_ds As New Llaves_ds
    Dim DAinstructor As New Capa_de_datos.Instructor

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            categoria_id = Session("categoria_id")
            evento_id = Session("evento_id")
            llave_id = Session("llave_id")
            llenar_encabezados(evento_id, categoria_id, llave_id)
            seccion_competencia.Visible = True
            'primero genero la tabla resultados con 4 registros.
            crear_tabla_resultados()
            cargar_resultados_competencia()

            'REORDENAR_LLAVE_PRIORIDAD_PROFES(llave_id)
        End If
    End Sub

    Private Sub REORDENAR_LLAVE_PRIORIDAD_PROFES(ByVal llave_id As Integer)
        Dim ds_llave As DataSet = DAllave.Llave_para_reordenar(llave_id)
        If ds_llave.Tables(0).Rows.Count <> 0 Then
            'aqui me fijo primero cuales son pareja y los voy a meter en un dataset, donde 1 columna me dice que son pareja o single.
            'si tienen el mismo numero en la columna son pareja, si esta 1 sola vez es single.

            'Llaves_ds2.tables("Llave_pareja_single").
            Dim Llaves_ds2 As New Llaves_ds


            Dim pareja As String = "pareja2"
            Dim i As Integer = 0
            While i < ds_llave.Tables(0).Rows.Count
                Dim usuario_id As Integer = CInt(ds_llave.Tables(0).Rows(i).Item("Llave_item_usuario_id"))
                If usuario_id <> 0 Then
                    'es competidor
                    Dim Llave_item_id As Integer = CInt(ds_llave.Tables(0).Rows(i).Item("Llave_item_id"))
                    'busco la llave_item_id en otro registro, en el puntero derecho o izquierdo.
                    Dim j As Integer = 0
                    While j < ds_llave.Tables(1).Rows.Count
                        Dim Llave_item_PIzqp As Integer = CInt(ds_llave.Tables(1).Rows(j).Item("Llave_item_PIzq"))
                        Dim Llave_item_PDerecho As Integer = CInt(ds_llave.Tables(1).Rows(j).Item("Llave_item_PDerecho"))
                        If Llave_item_id = Llave_item_PIzqp Then
                            'me fijo que ocurre con el derecho. si no esta en table(0) es un competidor "single" o Libre
                            Dim k As Integer = 0
                            Dim existe = "no"
                            While k < ds_llave.Tables(0).Rows.Count
                                If Llave_item_PDerecho = ds_llave.Tables(0).Rows(k).Item("Llave_item_id") Then
                                    existe = "si"
                                    Exit While
                                End If
                                k = k + 1
                            End While

                            Dim fila As DataRow = Llaves_ds2.Tables("Llave_pareja_single").NewRow
                            fila("Llave_item_id") = Llave_item_id
                            fila("Llave_item_usuario_id") = usuario_id
                            fila("USUARIO") = ds_llave.Tables(0).Rows(i).Item("USUARIO")
                            fila("instructor_id") = ds_llave.Tables(0).Rows(i).Item("instructor_id")
                            If existe = "si" Then
                                If pareja = "pareja1" Then
                                    pareja = "pareja2"
                                Else
                                    pareja = "pareja1"
                                End If

                                'lo ingreso como pareja
                                fila("tipo") = pareja
                            Else
                                fila("tipo") = "single"
                            End If
                            Llaves_ds2.Tables("Llave_pareja_single").Rows.Add(fila)
                        Else
                            If Llave_item_id = Llave_item_PDerecho Then
                                'me fijo que ocurre con el izquierdo. si no esta en table(0) es un competidor "single" o Libre
                                Dim k As Integer = 0
                                Dim existe = "no"
                                While k < ds_llave.Tables(0).Rows.Count
                                    If Llave_item_PIzqp = ds_llave.Tables(0).Rows(k).Item("Llave_item_id") Then
                                        existe = "si"
                                        Exit While
                                    End If
                                    k = k + 1
                                End While

                                Dim fila As DataRow = Llaves_ds2.Tables("Llave_pareja_single").NewRow
                                fila("Llave_item_id") = Llave_item_id
                                fila("Llave_item_usuario_id") = usuario_id
                                fila("USUARIO") = ds_llave.Tables(0).Rows(i).Item("USUARIO")
                                fila("instructor_id") = ds_llave.Tables(0).Rows(i).Item("instructor_id")
                                If existe = "si" Then
                                    If pareja = "pareja1" Then
                                        pareja = "pareja2"
                                    Else
                                        pareja = "pareja1"
                                    End If
                                    'lo ingreso como pareja
                                    fila("tipo") = pareja
                                Else
                                    fila("tipo") = "single"
                                End If
                                Llaves_ds2.Tables("Llave_pareja_single").Rows.Add(fila)
                            End If
                        End If

                        j = j + 1
                    End While

                End If

                i = i + 1
            End While

            If Llaves_ds2.Tables("Llave_pareja_single").Rows.Count <> 0 Then
                'aqui comienza la reorganización
                '1) voy a contar la cant de registros por instructor
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////
                i = 0
                While i < Llaves_ds2.Tables("Llave_pareja_single").Rows.Count
                    Dim instructor_id As Integer = CInt(Llaves_ds2.Tables("Llave_pareja_single").Rows(i).Item("instructor_id"))
                    Dim j As Integer = 0
                    Dim existe = "no"
                    While j < Llaves_ds2.Tables("Inscriptos_x_instructor").Rows.Count
                        If instructor_id = Llaves_ds2.Tables("Inscriptos_x_instructor").Rows(j).Item("instructor_id") Then
                            Llaves_ds2.Tables("Inscriptos_x_instructor").Rows(j).Item("cantidad") = CInt(Llaves_ds2.Tables("Inscriptos_x_instructor").Rows(j).Item("cantidad")) + 1
                            existe = "si"
                            Exit While
                        End If
                        j = j + 1
                    End While
                    If existe = "no" Then
                        Dim fila As DataRow = Llaves_ds2.Tables("Inscriptos_x_instructor").NewRow
                        fila("instructor_id") = instructor_id
                        fila("cantidad") = 1
                        Llaves_ds2.Tables("Inscriptos_x_instructor").Rows.Add(fila)
                    End If
                    i = i + 1
                End While
                'ordeno la tabla Inscriptos_x_instructor  por la columna cantidad.
                Dim dtV As DataView = Llaves_ds2.Tables("Inscriptos_x_instructor").DefaultView
                dtV.Sort = "cantidad DESC"
                Dim dt_list As DataTable = dtV.ToTable
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////
                If dt_list.Rows.Count Then
                    Llaves_ds2.Tables("Llave_pareja_single1").Merge(Llaves_ds2.Tables("Llave_pareja_single"))
                    'o) voy a quitar de estos registros el usuario, instructor_id, llave_item_usuario_id
                    i = 0
                    While i < Llaves_ds2.Tables("Llave_pareja_single1").Rows.Count
                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(i).Item("Llave_item_usuario_id") = 0
                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(i).Item("USUARIO") = ""
                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(i).Item("instructor_id") = 0
                        i = i + 1
                    End While

                    'este dt_list tiene el conteo de registros de todos los instructores: ejemplo instructor 45, cant 3. instructor 10, cant 2. instructor 60, cant 1.
                    i = 0
                    'primero determino cuantos inscriptos son...x que voy a ir metiendo 1 arriba y otro abajo.
                    Dim cant_insc As Integer = Llaves_ds2.Tables("Llave_pareja_single").Rows.Count
                    Dim inicio As Integer = 0
                    Dim medio As Integer = 0
                    If cant_insc Mod (2) = 0 Then
                        'es par
                        medio = cant_insc / 2
                    Else
                        medio = CInt(cant_insc / 2) + 1
                    End If

                    Dim mov_inicio As Integer = inicio
                    Dim mov_medio As Integer = medio
                    Dim donde_poner As String = "arriba"
                    Dim ingreso_secuenal As String = ""
                    While i < dt_list.Rows.Count
                        Dim instructor_id = CInt(dt_list.Rows(i).Item(0))

                        Dim j As Integer = 0
                        While j < Llaves_ds2.Tables("Llave_pareja_single").Rows.Count
                            If instructor_id = Llaves_ds2.Tables("Llave_pareja_single").Rows(j).Item("instructor_id") Then
                                Dim usuario_id As Integer = Llaves_ds2.Tables("Llave_pareja_single").Rows(j).Item("Llave_item_usuario_id")
                                Dim USUARIO As String = Llaves_ds2.Tables("Llave_pareja_single").Rows(j).Item("USUARIO")
                                If ingreso_secuenal = "si" Then

                                    Dim h As Integer = 0
                                    While h < Llaves_ds2.Tables("Llave_pareja_single1").Rows.Count
                                        If Llaves_ds2.Tables("Llave_pareja_single1").Rows(h).Item("instructor_id") = 0 Then
                                            Llaves_ds2.Tables("Llave_pareja_single1").Rows(h).Item("Llave_item_usuario_id") = usuario_id
                                            Llaves_ds2.Tables("Llave_pareja_single1").Rows(h).Item("USUARIO") = USUARIO
                                            Llaves_ds2.Tables("Llave_pareja_single1").Rows(h).Item("instructor_id") = instructor_id
                                            Exit While
                                        End If
                                        h = h + 1
                                    End While

                                Else
                                    If donde_poner = "arriba" Then

                                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(mov_inicio).Item("Llave_item_usuario_id") = usuario_id
                                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(mov_inicio).Item("USUARIO") = USUARIO
                                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(mov_inicio).Item("instructor_id") = instructor_id
                                        If Llaves_ds2.Tables("Llave_pareja_single1").Rows(mov_inicio).Item("tipo") = "pareja1" Then
                                            mov_inicio = mov_inicio + 2
                                            donde_poner = "abajo"
                                        Else
                                            mov_inicio = mov_inicio + 1
                                            donde_poner = "abajo"
                                        End If


                                    Else
                                        'poner abajo
                                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(mov_medio).Item("Llave_item_usuario_id") = usuario_id
                                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(mov_medio).Item("USUARIO") = USUARIO
                                        Llaves_ds2.Tables("Llave_pareja_single1").Rows(mov_medio).Item("instructor_id") = instructor_id
                                        If Llaves_ds2.Tables("Llave_pareja_single1").Rows(mov_medio).Item("tipo") = "pareja1" Then
                                            mov_medio = mov_medio + 2
                                            donde_poner = "arriba"
                                        Else
                                            mov_medio = mov_medio + 1
                                            donde_poner = "arriba"
                                        End If

                                    End If

                                    If donde_poner = "abajo" Then
                                        'verifico si no estoy al final
                                        If (mov_medio = medio + 1) Or (mov_medio > medio + 1) Then
                                            'verifico si no estoy al final de parte de arriba
                                            If (mov_inicio = medio) Or (mov_inicio > medio) Then
                                                ingreso_secuenal = "si"
                                            End If
                                            ''ya no puedo ingresar mas...entonces voy colocando secuencial
                                            'ingreso_secuenal = "si"
                                        End If
                                    Else
                                        If donde_poner = "arriba" Then
                                            'verifico si no estoy al final de parte de arriba
                                            If (mov_inicio = medio) Or (mov_inicio > medio) Then
                                                'ya no puedo ingresar mas aqui...
                                                If (mov_medio = medio + 1) Or (mov_medio > medio + 1) Then
                                                    'ya no puedo ingresar mas...entonces voy colocando secuencial.
                                                    ingreso_secuenal = "si"
                                                Else
                                                    donde_poner = "abajo"
                                                End If
                                            End If
                                        End If
                                    End If
                                End If

                            End If
                            j = j + 1
                        End While

                        i = i + 1
                    End While


                End If
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////


                If Llaves_ds2.Tables("Llave_pareja_single1").Rows.Count <> 0 Then

                End If


            End If


        End If



    End Sub


    Private Sub crear_tabla_resultados()
        Dim fila1 As DataRow = Llaves_ds.Tables("RESULTADOS").NewRow
        fila1("Puesto") = "1st"
        fila1("Competidor") = "..."

        Llaves_ds.Tables("RESULTADOS").Rows.Add(fila1)

        Dim fila2 As DataRow = Llaves_ds.Tables("RESULTADOS").NewRow
        fila2("Puesto") = "2nd"
        fila2("Competidor") = "..."

        Llaves_ds.Tables("RESULTADOS").Rows.Add(fila2)

        Dim fila3 As DataRow = Llaves_ds.Tables("RESULTADOS").NewRow
        fila3("Puesto") = "3rd"
        fila3("Competidor") = "..."

        Llaves_ds.Tables("RESULTADOS").Rows.Add(fila3)

        Dim fila4 As DataRow = Llaves_ds.Tables("RESULTADOS").NewRow
        fila4("Puesto") = "3rd"
        fila4("Competidor") = "..."

        Llaves_ds.Tables("RESULTADOS").Rows.Add(fila4)

        GridView_RESULTADOS.DataSource = Llaves_ds.Tables("RESULTADOS")
        GridView_RESULTADOS.DataBind()
    End Sub

    Private Sub cargar_resultados_competencia()
        If B31.Text <> "" Then
            LB_WINNER.Visible = True
            lb_1st.Text = B31.Text
            Winners(B31.Text, "1st")
            If B31.Text = B29.Text Then
                lb_2nd.Text = B30.Text
                Winners(B30.Text, "2nd")
            Else
                lb_2nd.Text = B29.Text
                Winners(B29.Text, "2nd")
            End If
            'aqui veo quien es el tercero
            If B25.Text <> "" And B26.Text <> "" Then
                If B25.Text = B29.Text Then
                    lb_3rd_a.Text = B26.Text
                    Winners(B26.Text, "3rd")
                End If
                If B26.Text = B29.Text Then
                    lb_3rd_a.Text = B25.Text
                    Winners(B25.Text, "3rd")
                End If
            End If
            If B27.Text <> "" And B28.Text <> "" Then
                If B27.Text = B30.Text Then
                    lb_3rd_b.Text = B28.Text
                    Winners(B28.Text, "4th")
                End If
                If B28.Text = B30.Text Then
                    lb_3rd_b.Text = B27.Text
                    Winners(B27.Text, "4th")
                End If
            End If
        End If

    End Sub

    Private Sub Cargar_ListadoCompetidores(ByVal ds_categorias As DataSet)

        Dim i As Integer = 0
        While i < ds_categorias.Tables(3).Rows.Count

            Dim ds_inscripcion As DataSet = DAinscripcion.inscripcion_recuperar_ID(ds_categorias.Tables(3).Rows(i).Item("usuario_id"))
            Dim idtext As String = "(" + CStr(ds_inscripcion.Tables(0).Rows(0).Item("inscripcion_id")) + ")"
            Dim Competidor As String = ds_categorias.Tables(3).Rows(i).Item("apenom") + idtext


            Dim instructor_id As Integer = ds_categorias.Tables(3).Rows(i).Item("instructor_id")
            Dim ds_instr As DataSet = DAinstructor.Instructor_obtener_INFO(instructor_id)
            Dim Datos_Instructor As String = ds_instr.Tables(0).Rows(0).Item("ApellidoyNombre") + " (Dni:" + CStr(ds_instr.Tables(0).Rows(0).Item("usuario_doc")) + ")"

            'veo si ya existen en el table("Competidores")
            Dim j As Integer = 0
            Dim existe = "no"
            While j < Llaves_ds.Tables("Competidores").Rows.Count
                If Competidor = Llaves_ds.Tables("Competidores").Rows(j).Item("Competidor") Then
                    existe = "si"
                    Exit While
                End If
                j = j + 1
            End While
            If existe = "no" Then
                'agrego
                Dim fila As DataRow = Llaves_ds.Tables("Competidores").NewRow
                fila("Competidor") = Competidor
                fila("Instructor") = Datos_Instructor
                Llaves_ds.Tables("Competidores").Rows.Add(fila)
            End If

            i = i + 1
        End While
        GridView_COMPETIDORES.DataSource = Llaves_ds.Tables("Competidores")
        GridView_COMPETIDORES.DataBind()

    End Sub


    Private Sub llenar_encabezados(ByVal evento_id As Integer, ByVal categoria_id As Integer, ByVal llave_id As Integer)
        Dim ds_categorias As DataSet = DAllave.LLave_obtener_llavegenerada_etc_2(llave_id)

        Cargar_ListadoCompetidores(ds_categorias)

        Lb_evento.Text = ds_categorias.Tables(0).Rows(0).Item("evento_descripcion")
        Lb_fecha.Text = ds_categorias.Tables(0).Rows(0).Item("evento_fecha")
        Lb_fecha_cierre.Text = ds_categorias.Tables(0).Rows(0).Item("evento_fechacierre")
        'aqui cargo el label lb_categoria
        'Dim ds_categorias As DataSet = DAllave.LLave_obtener_inscriptos(evento_id)
        If ds_categorias.Tables(0).Rows.Count <> 0 Then
            Dim tipo As String = ds_categorias.Tables(0).Rows(0).Item("categoria_tipo")
            Dim graduacion_desde As String = ""
            Dim k As Integer = 0
            While k < ds_categorias.Tables(1).Rows.Count 'tabla q tiene las graduaciones
                If (ds_categorias.Tables(1).Rows(k).Item("graduacion_id") = ds_categorias.Tables(0).Rows(0).Item("categoria_gradinicial")) Then
                    graduacion_desde = ds_categorias.Tables(1).Rows(k).Item("graduacion_desc")
                    k = ds_categorias.Tables(1).Rows.Count
                End If
                k = k + 1
            End While
            Dim graduacion_hasta As String = ""
            k = 0
            While k < ds_categorias.Tables(1).Rows.Count 'tabla que tiene las graduaciones
                If ds_categorias.Tables(1).Rows(k).Item("graduacion_id") = ds_categorias.Tables(0).Rows(0).Item("categoria_gradfinal") Then
                    graduacion_hasta = ds_categorias.Tables(1).Rows(k).Item("graduacion_desc")
                    k = ds_categorias.Tables(1).Rows.Count
                End If
                k = k + 1
            End While
            Dim edad_desde As String = ds_categorias.Tables(0).Rows(0).Item("categoria_edadinicial")
            Dim edad_hasta As String = ds_categorias.Tables(0).Rows(0).Item("categoria_edadfinal")
            Dim sexo As String = ds_categorias.Tables(0).Rows(0).Item("categoria_sexo")
            Dim peso_inicial As String = ds_categorias.Tables(0).Rows(0).Item("categoria_peso_inical")
            Dim peso_final As String = ds_categorias.Tables(0).Rows(0).Item("categoria_peso_Final")
            'ahora junto todas las variables para mostrar en categoria
            Dim categoria As String = tipo + " " + sexo + ", " + graduacion_desde + "-" + graduacion_hasta + "(" + edad_desde + " a " + edad_hasta + " años)"
            If tipo = "Lucha" Then
                categoria = tipo + " " + sexo + ", " + graduacion_desde + "-" + graduacion_hasta + "(" + edad_desde + " a " + edad_hasta + " años)" + " de " + peso_inicial + " a " + peso_final + " Kilos"
            End If
            Lb_categoria.Text = "Categoria: " + categoria

            'ahora pongo en visible solo los botones dependiendo de los inscriptos
            Dim i As Integer = 0
            While i < ds_categorias.Tables(2).Rows.Count
                Dim item_nro As Integer = CInt(ds_categorias.Tables(2).Rows(i).Item("Llave_item_Numero"))
                Dim Llave_item_usuario_id As Integer = CInt(ds_categorias.Tables(2).Rows(i).Item("Llave_item_usuario_id"))

                Dim usuario_id As Integer = 0 'esta variable se va actualizando en la rutina: colocar_tooltrip
                Dim tooltext As String = ""
                Dim idtext As String = ""
                Select Case item_nro
                    Case 1
                        If Llave_item_usuario_id <> 0 Then
                            B1.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B1.ToolTip = usuario_id
                        B1.Text = tooltext + idtext
                    Case 2
                        If Llave_item_usuario_id <> 0 Then
                            B2.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B2.ToolTip = usuario_id
                        B2.Text = tooltext + idtext
                    Case 3
                        If Llave_item_usuario_id <> 0 Then
                            B3.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B3.ToolTip = usuario_id
                        B3.Text = tooltext + idtext
                    Case 4
                        If Llave_item_usuario_id <> 0 Then
                            B4.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B4.ToolTip = usuario_id
                        B4.Text = tooltext + idtext
                    Case 5
                        If Llave_item_usuario_id <> 0 Then
                            B5.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B5.ToolTip = usuario_id
                        B5.Text = tooltext + idtext
                    Case 6
                        If Llave_item_usuario_id <> 0 Then
                            B6.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B6.ToolTip = usuario_id
                        B6.Text = tooltext + idtext
                    Case 7
                        If Llave_item_usuario_id <> 0 Then
                            B7.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B7.ToolTip = usuario_id
                        B7.Text = tooltext + idtext
                    Case 8
                        If Llave_item_usuario_id <> 0 Then
                            B8.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B8.ToolTip = usuario_id
                        B8.Text = tooltext + idtext
                    Case 9
                        If Llave_item_usuario_id <> 0 Then
                            B9.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B9.ToolTip = usuario_id
                        B9.Text = tooltext + idtext
                    Case 10
                        If Llave_item_usuario_id <> 0 Then
                            B10.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B10.ToolTip = usuario_id
                        B10.Text = tooltext + idtext
                    Case 11
                        If Llave_item_usuario_id <> 0 Then
                            B11.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B11.ToolTip = usuario_id
                        B11.Text = tooltext + idtext
                    Case 12
                        If Llave_item_usuario_id <> 0 Then
                            B12.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B12.ToolTip = usuario_id
                        B12.Text = tooltext + idtext
                    Case 13
                        If Llave_item_usuario_id <> 0 Then
                            B13.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B13.ToolTip = usuario_id
                        B13.Text = tooltext + idtext
                    Case 14
                        If Llave_item_usuario_id <> 0 Then
                            B14.Visible = True
                        End If

                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B14.ToolTip = usuario_id
                        B14.Text = tooltext + idtext
                    Case 15
                        If Llave_item_usuario_id <> 0 Then
                            B15.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B15.ToolTip = usuario_id
                        B15.Text = tooltext + idtext
                    Case 16
                        If Llave_item_usuario_id <> 0 Then
                            B16.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B16.ToolTip = usuario_id
                        B16.Text = tooltext + idtext
                    Case 17
                        If Llave_item_usuario_id <> 0 Then
                            B17.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B17.ToolTip = usuario_id
                        B17.Text = tooltext + idtext
                    Case 18
                        If Llave_item_usuario_id <> 0 Then
                            B18.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B18.ToolTip = usuario_id
                        B18.Text = tooltext + idtext
                    Case 19
                        If Llave_item_usuario_id <> 0 Then
                            B19.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B19.ToolTip = usuario_id
                        B19.Text = tooltext + idtext
                    Case 20
                        If Llave_item_usuario_id <> 0 Then
                            B20.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B20.ToolTip = usuario_id
                        B20.Text = tooltext + idtext
                    Case 21
                        If Llave_item_usuario_id <> 0 Then
                            B21.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B21.ToolTip = usuario_id
                        B21.Text = tooltext + idtext
                    Case 22
                        If Llave_item_usuario_id <> 0 Then
                            B22.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B22.ToolTip = usuario_id
                        B22.Text = tooltext + idtext
                    Case 23
                        If Llave_item_usuario_id <> 0 Then
                            B23.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B23.ToolTip = usuario_id
                        B23.Text = tooltext + idtext
                    Case 24
                        If Llave_item_usuario_id <> 0 Then
                            B24.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B24.ToolTip = usuario_id
                        B24.Text = tooltext + idtext
                    Case 25
                        If Llave_item_usuario_id <> 0 Then
                            B25.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B25.ToolTip = usuario_id
                        B25.Text = tooltext + idtext
                    Case 26
                        If Llave_item_usuario_id <> 0 Then
                            B26.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B26.ToolTip = usuario_id
                        B26.Text = tooltext + idtext
                    Case 27
                        If Llave_item_usuario_id <> 0 Then
                            B27.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B27.ToolTip = usuario_id
                        B27.Text = tooltext + idtext
                    Case 28
                        If Llave_item_usuario_id <> 0 Then
                            B28.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B28.ToolTip = usuario_id
                        B28.Text = tooltext + idtext
                    Case 29
                        If Llave_item_usuario_id <> 0 Then
                            B29.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B29.ToolTip = usuario_id
                        B29.Text = tooltext + idtext
                    Case 30
                        If Llave_item_usuario_id <> 0 Then
                            B30.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B30.ToolTip = usuario_id
                        B30.Text = tooltext + idtext
                    Case 31
                        If Llave_item_usuario_id <> 0 Then
                            B31.Visible = True
                        End If
                        colocar_tooltrip(B1, ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        B31.ToolTip = usuario_id
                        B31.Text = tooltext + idtext
                End Select
                i = i + 1
            End While

        End If
    End Sub

    Private Sub recuper_nombre_participante(ByVal ds_categorias As DataSet, ByRef apenom As String, ByVal Llave_item_usuario_id As Integer)

        Dim i As Integer = 0
        While i < ds_categorias.Tables(2).Rows.Count
            If CInt(ds_categorias.Tables(2).Rows(i).Item("Llave_item_usuario_id")) = Llave_item_usuario_id Then
                apenom = ds_categorias.Tables(2).Rows(i).Item("apenom")
                i = ds_categorias.Tables(2).Rows.Count
            End If
            i = i + 1
        End While
    End Sub

    Private Sub colocar_tooltrip(ByVal Boton As Button, ByVal ds As DataSet, ByVal item_nro As Integer, ByRef tooltext As String, ByRef idtext As String, ByRef usuario_id As Integer)
        Dim i As Integer = 0
        While i < ds.Tables(3).Rows.Count
            If ds.Tables(3).Rows(i).Item("Llave_item_Numero") = item_nro Then
                tooltext = ds.Tables(3).Rows(i).Item("apenom")
                Dim ds_inscripcion As DataSet = DAinscripcion.inscripcion_recuperar_ID(ds.Tables(3).Rows(i).Item("usuario_id"))
                idtext = "(" + CStr(ds_inscripcion.Tables(0).Rows(0).Item("inscripcion_id")) + ")"
                usuario_id = ds.Tables(3).Rows(i).Item("usuario_id")
                i = ds.Tables(3).Rows.Count
            End If
            i = i + 1
        End While

    End Sub
    Private Sub actualizar_llave(ByRef boton_desde As Button, ByRef boton_hasta As Button, ByRef boton_versus As Button, ByVal nro As Integer)

        If boton_desde.Text <> "" And boton_versus.Text <> "" Then
            If boton_versus.Visible = True Then
                boton_hasta.Visible = True
                boton_hasta.Text = boton_desde.Text
                boton_hasta.ToolTip = boton_desde.ToolTip
                'actualizo en la bd, el tooltip me da el id del usuario.
                'nro es el numero de nodo
                Dim ds_categorias As DataSet = DAllave.LLave_obtener_llavegenerada_etc_2(Session("llave_id"))
                Dim i As Integer = 0
                While i < ds_categorias.Tables(2).Rows.Count
                    If ds_categorias.Tables(2).Rows(i).Item("Llave_item_Numero") = nro Then
                        'aqui actualizo en bd
                        Dim llave_item_id As Integer = ds_categorias.Tables(2).Rows(i).Item("Llave_item_id")
                        DAllave.Llave_item_actualizar_progreso(llave_item_id, CInt(boton_desde.ToolTip))
                        i = ds_categorias.Tables(2).Rows.Count
                    End If
                    i = i + 1
                End While
            End If
        End If


    End Sub



    Private Sub B1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B1.Click
        actualizar_llave(B1, B17, B2, 17)
    End Sub

    Private Sub B2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B2.Click
        actualizar_llave(B2, B17, B1, 17)
    End Sub

    Private Sub B3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B3.Click
        actualizar_llave(B3, B18, B4, 18)
    End Sub

    Private Sub B4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B4.Click
        actualizar_llave(B4, B18, B3, 18)
    End Sub

    Private Sub B5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B5.Click
        actualizar_llave(B5, B19, B6, 19)
    End Sub

    Private Sub B6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B6.Click
        actualizar_llave(B6, B19, B5, 19)
    End Sub

    Private Sub B7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B7.Click
        actualizar_llave(B7, B20, B8, 20)
    End Sub

    Private Sub B8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B8.Click
        actualizar_llave(B8, B20, B7, 20)
    End Sub

    Private Sub B9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B9.Click
        actualizar_llave(B9, B21, B10, 21)
    End Sub

    Private Sub B10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B10.Click
        actualizar_llave(B10, B21, B9, 21)
    End Sub

    Private Sub B11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B11.Click
        actualizar_llave(B11, B22, B12, 22)
    End Sub

    Private Sub B12_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B12.Click
        actualizar_llave(B12, B22, B11, 22)
    End Sub

    Private Sub B13_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B13.Click
        actualizar_llave(B13, B23, B14, 23)
    End Sub

    Private Sub B14_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B14.Click
        actualizar_llave(B14, B23, B13, 23)
    End Sub

    Private Sub B15_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B15.Click
        actualizar_llave(B15, B24, B16, 24)
    End Sub

    Private Sub B16_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B16.Click
        actualizar_llave(B16, B24, B15, 24)
    End Sub

    Private Sub B17_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B17.Click
        actualizar_llave(B17, B25, B18, 25)
    End Sub

    Private Sub B18_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B18.Click
        actualizar_llave(B18, B25, B17, 25)
    End Sub

    Private Sub B19_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B19.Click
        actualizar_llave(B19, B26, B20, 26)
    End Sub

    Private Sub B20_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B20.Click
        actualizar_llave(B20, B26, B19, 26)
    End Sub

    Private Sub B21_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B21.Click
        actualizar_llave(B21, B27, B22, 27)
    End Sub

    Private Sub B22_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B22.Click
        actualizar_llave(B22, B27, B21, 27)
    End Sub

    Private Sub B23_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B23.Click
        actualizar_llave(B23, B28, B24, 28)
    End Sub

    Private Sub B24_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B24.Click
        actualizar_llave(B24, B28, B23, 28)
    End Sub

    Private Sub B25_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B25.Click
        actualizar_llave(B25, B29, B26, 29)
    End Sub

    Private Sub B26_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B26.Click
        actualizar_llave(B26, B29, B25, 29)
    End Sub

    Private Sub B27_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B27.Click
        actualizar_llave(B27, B30, B28, 30)
    End Sub

    Private Sub B28_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B28.Click
        actualizar_llave(B28, B30, B27, 30)
    End Sub

    Private Sub B29_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B29.Click
        actualizar_llave(B29, B31, B30, 31)
        If B31.Visible = True And (B29.Text <> "") And (B30.Text <> "") Then
            LB_WINNER.Visible = True
            lb_1st.Text = B31.Text
            Winners(B31.Text, "1st")
            lb_2nd.Text = B30.Text
            Winners(B30.Text, "2nd")
            'aqui veo quien es el tercero
            If B25.Text <> "" And B26.Text <> "" Then
                If B25.Text = B29.Text Then
                    lb_3rd_a.Text = B26.Text
                    Winners(B26.Text, "3rd")
                End If
                If B26.Text = B29.Text Then
                    lb_3rd_a.Text = B25.Text
                    Winners(B25.Text, "3rd")
                End If
            End If
            If B27.Text <> "" And B28.Text <> "" Then
                If B27.Text = B30.Text Then
                    lb_3rd_b.Text = B28.Text
                    Winners(B28.Text, "4th")
                End If
                If B28.Text = B30.Text Then
                    lb_3rd_b.Text = B27.Text
                    Winners(B27.Text, "4th")
                End If
            End If
        End If
    End Sub

    Private Sub B30_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B30.Click
        actualizar_llave(B30, B31, B29, 31)
        If B31.Visible = True And (B29.Text <> "") And (B30.Text <> "") Then
            LB_WINNER.Visible = True
            lb_1st.Text = B31.Text
            Winners(B31.Text, "1st")
            lb_2nd.Text = B29.Text
            Winners(B29.Text, "2nd")
            'aqui veo quien es el tercero
            If B25.Text <> "" And B26.Text <> "" Then
                If B25.Text = B29.Text Then
                    lb_3rd_a.Text = B26.Text
                    Winners(B26.Text, "3rd")
                End If
                If B26.Text = B29.Text Then
                    lb_3rd_a.Text = B25.Text
                    Winners(B25.Text, "3rd")
                End If
            End If
            If B27.Text <> "" And B28.Text <> "" Then
                If B27.Text = B30.Text Then
                    lb_3rd_b.Text = B28.Text
                    Winners(B28.Text, "4th")
                End If
                If B28.Text = B30.Text Then
                    lb_3rd_b.Text = B27.Text
                    Winners(B27.Text, "4th")
                End If
            End If
        End If
    End Sub
    Dim Llaves_reporte_DS As New Llaves_reporte_DS
    Private Sub Btn_reporte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_reporte.Click
        Llaves_ds.Tables("LLAVE_DATOS").Rows.Clear()
        Dim fila1 As DataRow = Llaves_ds.Tables("LLAVE_DATOS").NewRow
        fila1("Evento") = Lb_evento.Text
        fila1("Fecha") = CDate(Lb_fecha.Text)
        fila1("Categoria") = Lb_categoria.Text
        fila1("Cant_Inscriptos") = GridView_COMPETIDORES.Rows.Count
        fila1("ID") = 1
        Llaves_ds.Tables("LLAVE_DATOS").Rows.Add(fila1)

        Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
        Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
        fila2("1st") = GridView_RESULTADOS.Rows(0).Cells(1).Text
        fila2("2nd") = GridView_RESULTADOS.Rows(1).Cells(1).Text
        fila2("3rd_a") = GridView_RESULTADOS.Rows(2).Cells(1).Text
        fila2("3rd_b") = GridView_RESULTADOS.Rows(3).Cells(1).Text
        fila2("ID") = 1
        Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)


        Llaves_ds.Tables("LLAVE_16").Rows.Clear()

        Dim fila3 As DataRow = Llaves_ds.Tables("LLAVE_16").NewRow
        fila3("B1") = B1.Text
        fila3("B2") = B2.Text
        fila3("B3") = B3.Text
        fila3("B4") = B4.Text
        fila3("B5") = B5.Text
        fila3("B6") = B6.Text
        fila3("B7") = B7.Text
        fila3("B8") = B8.Text
        fila3("B9") = B9.Text
        fila3("B10") = B10.Text
        fila3("B11") = B11.Text
        fila3("B12") = B12.Text
        fila3("B13") = B13.Text
        fila3("B14") = B14.Text
        fila3("B15") = B15.Text
        fila3("B16") = B16.Text
        fila3("B17") = B17.Text
        fila3("B18") = B18.Text
        fila3("B19") = B19.Text
        fila3("B20") = B20.Text
        fila3("B21") = B21.Text
        fila3("B22") = B22.Text
        fila3("B23") = B23.Text
        fila3("B24") = B24.Text
        fila3("B25") = B25.Text
        fila3("B26") = B26.Text
        fila3("B27") = B27.Text
        fila3("B28") = B28.Text
        fila3("B29") = B29.Text
        fila3("B30") = B30.Text
        fila3("B31") = B31.Text
        fila3("ID") = 1
        Llaves_ds.Tables("LLAVE_16").Rows.Add(fila3)

        Llaves_ds.Tables("Competidores").Rows.Clear()
        Dim i As Integer = 0
        While i < GridView_COMPETIDORES.Rows.Count
            Dim fila4 As DataRow = Llaves_ds.Tables("Competidores").NewRow
            fila4("Competidor") = GridView_COMPETIDORES.Rows(i).Cells(0).Text
            fila4("Instructor") = GridView_COMPETIDORES.Rows(i).Cells(1).Text
            Llaves_ds.Tables("Competidores").Rows.Add(fila4)
            i = i + 1
        End While


        Session("op_llave") = "llave 16"
        Session("datatable_LLAVE_DATOS") = Llaves_ds.Tables("LLAVE_DATOS")
        Session("datatable_LLAVE_RESULTADOS") = Llaves_ds.Tables("LLAVE_RESULTADOS")
        Session("datatable_LLAVE_16") = Llaves_ds.Tables("LLAVE_16")
        'Session("datatable_Competidores") = Llaves_ds.Tables("Competidores")

        Response.Redirect("RPTcompetidores16.aspx")




        'Llaves_reporte_DS.Tables("Llave16").Rows.Clear()
        'Dim row_competidores As DataRow = Llaves_reporte_DS.Tables("Llave16").NewRow

        'row_competidores("B1") = B1.Text
        'row_competidores("B2") = B2.Text
        'row_competidores("B3") = B3.Text
        'row_competidores("B4") = B4.Text
        'row_competidores("B5") = B5.Text
        'row_competidores("B6") = B6.Text
        'row_competidores("B7") = B7.Text
        'row_competidores("B8") = B8.Text
        'row_competidores("B9") = B9.Text
        'row_competidores("B10") = B10.Text
        'row_competidores("B11") = B11.Text
        'row_competidores("B12") = B12.Text
        'row_competidores("B13") = B13.Text
        'row_competidores("B14") = B14.Text
        'row_competidores("B15") = B15.Text
        'row_competidores("B16") = B16.Text
        'row_competidores("B17") = B17.Text
        'row_competidores("B18") = B18.Text
        'row_competidores("B19") = B19.Text
        'row_competidores("B20") = B20.Text
        'row_competidores("B21") = B21.Text
        'row_competidores("B22") = B22.Text
        'row_competidores("B23") = B23.Text
        'row_competidores("B24") = B24.Text
        'row_competidores("B25") = B25.Text
        'row_competidores("B26") = B26.Text
        'row_competidores("B27") = B27.Text
        'row_competidores("B28") = B28.Text
        'row_competidores("B29") = B29.Text
        'row_competidores("B30") = B30.Text
        'row_competidores("B31") = B31.Text
        'row_competidores("1st") = lb_1st.Text
        'row_competidores("2nd") = lb_2nd.Text
        'row_competidores("3rd_a") = lb_3rd_a.Text
        'row_competidores("3rd_b") = lb_3rd_b.Text
        'row_competidores("evento") = Lb_evento.Text
        'row_competidores("fecha_evento") = CDate(Lb_fecha.Text).ToShortDateString
        'row_competidores("categoria") = Lb_categoria.Text
        'Session("llave") = 16
        'Llaves_reporte_DS.Tables("Llave16").Rows.Add(row_competidores)
        'Session("dataset_competidores") = Llaves_reporte_DS.Tables("Llave16")
        'Response.Redirect("~/Llaves/Reporte_llaves/Visor_llaves_report.aspx")
    End Sub

#Region "WINNER"

    Private Sub Winners(ByVal competidor As String, ByVal puesto As String)
        Select Case puesto
            Case "1st"
                GridView_RESULTADOS.Rows(0).Cells(1).Text = competidor

            Case "2nd"
                GridView_RESULTADOS.Rows(1).Cells(1).Text = competidor


            Case "3rd"
                GridView_RESULTADOS.Rows(2).Cells(1).Text = competidor

            Case "4th"
                GridView_RESULTADOS.Rows(3).Cells(1).Text = competidor

        End Select
        'NOTA: CUANDO QUIERO AGREGAR LA INFO DEL INSTRUCTOR, ESO SE RECUPERA DE LA TABLA COMPETIDORES...Y NOTE QUE CUANDO EL NOMBRE TIENE ALGUN ACENTO SE MUESTRAN CARACTERES RAROS, X ESTA MISMA RAZON CUANDO COMPARO CON LA TABLA COMPETIDORES, NO LO ENCUENTRO NUNCA AL COMPETIDOR...DECIDI QUITAR DE LA TABLA RESULTADOS LA COLUMNA INSTRUCTOR


    End Sub

#End Region

End Class