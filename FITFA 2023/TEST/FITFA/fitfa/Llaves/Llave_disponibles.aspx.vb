Public Class Llave_disponibles
    Inherits System.Web.UI.Page
    Dim DAllave As New Capa_de_datos.Llave
    Dim DAinscrip As New Capa_de_datos.Inscripciones
    Dim key_insc_ds As New Llaves_ds
    Dim DAevento As New Capa_de_datos.Eventos
    Dim DAinscripcion As New Capa_de_datos.Inscripciones
    Dim DAinstructor As New Capa_de_datos.Instructor
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("Tipo") = "administrador" Then
                div_Grid.Visible = True
                div_Volver.Visible = False
                Label1.Visible = True
                Lb_evento.Text = Session("evento_desc")
                Dim evento_id = Session("evento_id")
                HF_evento_id.Value = Session("evento_id") 'esto lo uso x el prolongado tiempo de una sesion
                Lb_fecha.Text = Session("fecha")
                Lb_fecha_cierre.Text = Session("fecha_cierre")
                'obtener_categorias(HF_evento_id.Value)
                obtener_llaves_generadas_info()
                'div_modalllaveOK.Visible = False
                'categorias_ObtenerInscriptos(evento_id)
                'div_Modal_err.Visible = False
                'div_Modal_error_generacion.Visible = False

                'busqueda() 'va a recuperar un listado de los inscriptos segun los combos seleccionados
            Else
                If Session("Area") = "" Then
                    div_Grid.Visible = False
                    div_Volver.Visible = True
                    Label1.Visible = True
                Else

                    div_Grid.Visible = True
                    div_Volver.Visible = True
                    Label1.Visible = False
                    Dim evento_id = Session("evento_id")
                    Dim ds_evento As DataSet = DAevento.Evento_ObetenerEvento_ID(evento_id)
                    Lb_evento.Text = ds_evento.Tables(0).Rows(0).Item("evento_descripcion") 'esto lo busco en sql
                    Lb_fecha.Text = ds_evento.Tables(0).Rows(0).Item("evento_fecha")
                    Lb_fecha_cierre.Text = ds_evento.Tables(0).Rows(0).Item("fechacierre")
                    HF_evento_id.Value = Session("evento_id") 'esto lo uso x el prolongado tiempo de una sesion
                    HF_area_id.Value = Session("Area")
                    'obtener_categorias(HF_evento_id.Value)
                    obtener_llaves_generadas_infoArea(CInt(HF_area_id.Value))
                End If
            End If
        End If
    End Sub


    Private Sub obtener_llaves_generadas_infoArea(ByVal area_id As Integer)
        key_insc_ds.Tables("Llaves_generadas").Rows.Clear()
        GridView2.DataSource = ""
        GridView2.DataBind()




        'se consulta en la bd las llaves generadas y se muestra en la grilla 2
        Dim ds_llave As DataSet = DAllave.Llave_obtener_llaves_generadas_infoArea(HF_evento_id.Value, area_id)
        If ds_llave.Tables(0).Rows.Count <> 0 Then

            'aqui lo relleno
            'Llaves_generadas
            Dim i As Integer = 0
            Dim item_nuevo As String = "no"
            While i < ds_llave.Tables(0).Rows.Count

                Dim ds_llave_filto As DataSet = DAllave.Llaves_Finalizadas_filtro(ds_llave.Tables(0).Rows(i).Item("ID"))
                If ds_llave_filto.Tables(0).Rows(0).Item("Llave_item_usuario_id") = 0 Then

                    'aqui lo agrego al primero.
                    'la categoria va concatenada en una var string
                    Dim tipo As String = ds_llave.Tables(0).Rows(i).Item("categoria_tipo")
                    'busco graduacion desde
                    Dim graduacion_desde As String = ""
                    Dim k As Integer = 0
                    While k < ds_llave.Tables(1).Rows.Count
                        If (ds_llave.Tables(1).Rows(k).Item("graduacion_id") = ds_llave.Tables(0).Rows(i).Item("categoria_gradinicial")) Then
                            graduacion_desde = ds_llave.Tables(1).Rows(k).Item("graduacion_desc")
                            k = ds_llave.Tables(1).Rows.Count
                        End If
                        k = k + 1
                    End While
                    'busco graduacion hasta
                    Dim graduacion_hasta As String = ""
                    k = 0
                    While k < ds_llave.Tables(1).Rows.Count
                        If ds_llave.Tables(1).Rows(k).Item("graduacion_id") = ds_llave.Tables(0).Rows(i).Item("categoria_gradfinal") Then
                            graduacion_hasta = ds_llave.Tables(1).Rows(k).Item("graduacion_desc")
                            k = ds_llave.Tables(1).Rows.Count
                        End If
                        k = k + 1
                    End While
                    Dim edad_desde As String = ds_llave.Tables(0).Rows(i).Item("categoria_edadinicial")
                    Dim edad_hasta As String = ds_llave.Tables(0).Rows(i).Item("categoria_edadfinal")
                    Dim peso_inicial As String = ds_llave.Tables(0).Rows(i).Item("categoria_peso_inical")
                    Dim peso_final As String = ds_llave.Tables(0).Rows(i).Item("categoria_peso_Final")
                    Dim sexo As String = ds_llave.Tables(0).Rows(i).Item("categoria_sexo")
                    'ahora junto todas las variables para mostrar en categoria
                    Dim categoria As String = sexo + ", " + graduacion_desde + "-" + graduacion_hasta + "(" + edad_desde + " a " + edad_hasta + " años)"
                    If tipo = "Lucha" Then
                        categoria = sexo + ", " + graduacion_desde + "-" + graduacion_hasta + "(" + edad_desde + " a " + edad_hasta + " años)" + " de " + peso_inicial + " a " + peso_final + " Kilos"
                    End If
                    Dim row_insc As DataRow = key_insc_ds.Tables("Llaves_generadas").NewRow()
                    'Dim Estado = "Pendiente"
                    row_insc("ID") = ds_llave.Tables(0).Rows(i).Item("ID")
                    row_insc("modalidad") = tipo
                    row_insc("categoria") = categoria
                    row_insc("inscriptos") = ds_llave.Tables(0).Rows(i).Item("inscriptos")
                    row_insc("Area") = ds_llave.Tables(0).Rows(i).Item("Area") 'choco: 19-07-2019 ahora recupera el area vinculada a la llave

                    key_insc_ds.Tables("Llaves_generadas").Rows.Add(row_insc)
                    i = i + 1

                Else
                    i = i + 1
                End If
            End While
            GridView2.DataSource = key_insc_ds.Tables("Llaves_generadas")
            GridView2.DataBind()

        End If
        If GridView2.Rows.Count = 0 Then
            Lab_no_llaves.Visible = True
            btn_Examinar.Visible = False 'es el boton de eliminar llave

        Else
            Lab_no_llaves.Visible = False
            btn_Examinar.Visible = True 'es el boton de eliminar llave
        End If




        ''------------LLAVES FINALIZADAS-----------------
        key_insc_ds.Tables("Llaves_generadas").Rows.Clear()
        GridView_LLF.DataSource = ""
        GridView_LLF.DataBind()

        'se consulta en la bd las llaves generadas y se muestra en la grilla 2

        If ds_llave.Tables(0).Rows.Count <> 0 Then

            'aqui lo relleno
            'Llaves_generadas
            Dim i As Integer = 0
            Dim item_nuevo As String = "no"
            While i < ds_llave.Tables(0).Rows.Count

                Dim ds_llave_filto As DataSet = DAllave.Llaves_Finalizadas_filtro(ds_llave.Tables(0).Rows(i).Item("ID"))
                If ds_llave_filto.Tables(0).Rows(0).Item("Llave_item_usuario_id") = 0 Then
                    i = i + 1
                Else

                    'aqui lo agrego al primero.
                    'la categoria va concatenada en una var string
                    Dim tipo As String = ds_llave.Tables(0).Rows(i).Item("categoria_tipo")
                    'busco graduacion desde
                    Dim graduacion_desde As String = ""
                    Dim k As Integer = 0
                    While k < ds_llave.Tables(1).Rows.Count
                        If (ds_llave.Tables(1).Rows(k).Item("graduacion_id") = ds_llave.Tables(0).Rows(i).Item("categoria_gradinicial")) Then
                            graduacion_desde = ds_llave.Tables(1).Rows(k).Item("graduacion_desc")
                            k = ds_llave.Tables(1).Rows.Count
                        End If
                        k = k + 1
                    End While
                    'busco graduacion hasta
                    Dim graduacion_hasta As String = ""
                    k = 0
                    While k < ds_llave.Tables(1).Rows.Count
                        If ds_llave.Tables(1).Rows(k).Item("graduacion_id") = ds_llave.Tables(0).Rows(i).Item("categoria_gradfinal") Then
                            graduacion_hasta = ds_llave.Tables(1).Rows(k).Item("graduacion_desc")
                            k = ds_llave.Tables(1).Rows.Count
                        End If
                        k = k + 1
                    End While
                    Dim edad_desde As String = ds_llave.Tables(0).Rows(i).Item("categoria_edadinicial")
                    Dim edad_hasta As String = ds_llave.Tables(0).Rows(i).Item("categoria_edadfinal")
                    Dim peso_inicial As String = ds_llave.Tables(0).Rows(i).Item("categoria_peso_inical")
                    Dim peso_final As String = ds_llave.Tables(0).Rows(i).Item("categoria_peso_Final")
                    Dim sexo As String = ds_llave.Tables(0).Rows(i).Item("categoria_sexo")
                    'ahora junto todas las variables para mostrar en categoria
                    Dim categoria As String = sexo + ", " + graduacion_desde + "-" + graduacion_hasta + "(" + edad_desde + " a " + edad_hasta + " años)"
                    If tipo = "Lucha" Then
                        categoria = sexo + ", " + graduacion_desde + "-" + graduacion_hasta + "(" + edad_desde + " a " + edad_hasta + " años)" + " de " + peso_inicial + " a " + peso_final + " Kilos"
                    End If
                    Dim row_insc As DataRow = key_insc_ds.Tables("Llaves_generadas").NewRow()
                    'Dim Estado = "Pendiente"
                    row_insc("ID") = ds_llave.Tables(0).Rows(i).Item("ID")
                    row_insc("modalidad") = tipo
                    row_insc("categoria") = categoria
                    row_insc("inscriptos") = ds_llave.Tables(0).Rows(i).Item("inscriptos")
                    row_insc("Area") = ds_llave.Tables(0).Rows(i).Item("Area") 'choco: 19-07-2019 ahora recupera el area vinculada a la llave

                    key_insc_ds.Tables("Llaves_generadas").Rows.Add(row_insc)
                    i = i + 1


                End If
            End While
            GridView_LLF.DataSource = key_insc_ds.Tables("Llaves_generadas")
            GridView_LLF.DataBind()

        End If
        If GridView_LLF.Rows.Count = 0 Then
            lbl_llf.Visible = False
        Else
            lbl_llf.Visible = True

        End If





    End Sub

    Private Sub obtener_llaves_generadas_info()
        key_insc_ds.Tables("Llaves_generadas").Rows.Clear()
        GridView2.DataSource = ""
        GridView2.DataBind()




        'se consulta en la bd las llaves generadas y se muestra en la grilla 2
        Dim ds_llave As DataSet = DAllave.Llave_obtener_llaves_generadas_info(HF_evento_id.Value)

        If ds_llave.Tables(0).Rows.Count <> 0 Then

            'aqui lo relleno
            'Llaves_generadas
            Dim i As Integer = 0
            Dim item_nuevo As String = "no"
            While i < ds_llave.Tables(0).Rows.Count



                'aqui lo agrego al primero.
                'la categoria va concatenada en una var string
                Dim tipo As String = ds_llave.Tables(0).Rows(i).Item("categoria_tipo")
                    'busco graduacion desde
                    Dim graduacion_desde As String = ""
                    Dim k As Integer = 0
                    While k < ds_llave.Tables(1).Rows.Count
                        If (ds_llave.Tables(1).Rows(k).Item("graduacion_id") = ds_llave.Tables(0).Rows(i).Item("categoria_gradinicial")) Then
                            graduacion_desde = ds_llave.Tables(1).Rows(k).Item("graduacion_desc")
                            k = ds_llave.Tables(1).Rows.Count
                        End If
                        k = k + 1
                    End While
                    'busco graduacion hasta
                    Dim graduacion_hasta As String = ""
                    k = 0
                    While k < ds_llave.Tables(1).Rows.Count
                        If ds_llave.Tables(1).Rows(k).Item("graduacion_id") = ds_llave.Tables(0).Rows(i).Item("categoria_gradfinal") Then
                            graduacion_hasta = ds_llave.Tables(1).Rows(k).Item("graduacion_desc")
                            k = ds_llave.Tables(1).Rows.Count
                        End If
                        k = k + 1
                    End While
                    Dim edad_desde As String = ds_llave.Tables(0).Rows(i).Item("categoria_edadinicial")
                    Dim edad_hasta As String = ds_llave.Tables(0).Rows(i).Item("categoria_edadfinal")
                    Dim peso_inicial As String = ds_llave.Tables(0).Rows(i).Item("categoria_peso_inical")
                    Dim peso_final As String = ds_llave.Tables(0).Rows(i).Item("categoria_peso_Final")
                    Dim sexo As String = ds_llave.Tables(0).Rows(i).Item("categoria_sexo")
                    'ahora junto todas las variables para mostrar en categoria
                    Dim categoria As String = sexo + ", " + graduacion_desde + "-" + graduacion_hasta + "(" + edad_desde + " a " + edad_hasta + " años)"
                    If tipo = "Lucha" Then
                        categoria = sexo + ", " + graduacion_desde + "-" + graduacion_hasta + "(" + edad_desde + " a " + edad_hasta + " años)" + " de " + peso_inicial + " a " + peso_final + " Kilos"
                    End If
                    Dim row_insc As DataRow = key_insc_ds.Tables("Llaves_generadas").NewRow()
                    'Dim Estado = "Pendiente"
                    row_insc("ID") = ds_llave.Tables(0).Rows(i).Item("ID")
                    row_insc("modalidad") = tipo
                    row_insc("categoria") = categoria
                    row_insc("inscriptos") = ds_llave.Tables(0).Rows(i).Item("inscriptos")
                    row_insc("Area") = ds_llave.Tables(0).Rows(i).Item("Area") 'choco: 19-07-2019 ahora recupera el area vinculada a la llave

                    key_insc_ds.Tables("Llaves_generadas").Rows.Add(row_insc)
                    i = i + 1


            End While
            GridView2.DataSource = key_insc_ds.Tables("Llaves_generadas")
            GridView2.DataBind()

        End If
        If GridView2.Rows.Count = 0 And GridView_LLF.Rows.Count = 0 Then
            Lab_no_llaves.Visible = True
            btn_Examinar.Visible = False 'es el boton de eliminar llave

        Else
            Lab_no_llaves.Visible = False
            btn_Examinar.Visible = True 'es el boton de eliminar llave
        End If

    End Sub

    Private Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        If (e.CommandName = "ID") Then
            ' Retrieve the row index stored in the CommandArgument property.
            'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
            'Session("usuario_id") = id
            'Response.Redirect("Mensaje_Datos_Personales.aspx")
            Session("categoria_id") = id
            Session("llave_id") = id
            'Session("evento_id")

            Dim i As Integer = 0
            Dim cantidad_inscriptos As Integer = 0
            While i < GridView2.Rows.Count
                Dim llave_id As Integer = CInt(GridView2.Rows(i).Cells(1).Text)
                If llave_id = id Then
                    cantidad_inscriptos = CInt(GridView2.Rows(i).Cells(4).Text) 'es la cantidad
                    'Session("evento_desc") = GridView1.Rows(i).Cells(1).Text
                    i = GridView2.Rows.Count
                Else
                    i = i + 1
                End If
            End While

            Dim c_inscri As Integer = cantidad_inscriptos



            'primero voy a borrar las llave q se haya creado si es necesario

            'DAllave.llave_eliminar(CInt(HF_evento_id.Value), id)
            'DAllave.llave_eliminar(Session("evento_id"), id) lo comento x q no quiero usar algo de sesion q se pueda perder si se prolonga el tiempo

            'aqui voy a poner la rutina para generar la llave.

            If (c_inscri = 2) Then
                Response.Redirect("Llave_2.aspx") 'este si va es IMPORTANTE
                'Response.Redirect("~/Visor_reporte_llave2.aspx") 'ESTE LO USO PARA VER EN CRISTAL
                'Response.Redirect("~/Reportes/Llaves/Visor_ejemplo.aspx")

            End If
            If (c_inscri > 2) And (c_inscri <= 4) Then
                Response.Redirect("Llave_4.aspx")
            End If

            If (c_inscri > 4) And (c_inscri <= 8) Then
                Response.Redirect("Llave_8.aspx")
            End If
            If (c_inscri > 8) And (c_inscri <= 16) Then
                Response.Redirect("Llave_16.aspx")
            End If
            If (c_inscri > 16) And (c_inscri <= 32) Then
                Response.Redirect("Llave_32.aspx")
            End If

            'Dim i As Integer = 0
            'While i < GridView1.Rows.Count
            '    Dim id_evento As Integer = CInt(GridView1.Rows(i).Cells(0).Text)
            '    If id_evento = index Then
            '        Session("evento_desc") = GridView1.Rows(i).Cells(1).Text
            '        i = GridView1.Rows.Count
            '        Response.Redirect("Llave_detalle_evento.aspx")
            '    Else
            '        i = i + 1
            '    End If
            'End While

            ''valido si el usuario no se ha inscripto ya.
            'Dim usuario_id As Integer = Session("Us_id")
            'Dim ds_inscripto As DataSet = DAinscripciones.Inscripcion_consultar_alumno_inscripto(id, usuario_id)

            'If ds_inscripto.Tables(0).Rows.Count = 0 Then
            '    Dim i As Integer = 0
            '    While i < GridView1.Rows.Count
            '        Dim id_evento As Integer = CInt(GridView1.Rows(i).Cells(1).Text)
            '        If id_evento = index Then
            '            Session("evento_desc") = GridView1.Rows(i).Cells(1).Text
            '            i = GridView1.Rows.Count
            '            Response.Redirect("Evento_datos.aspx")
            '        Else
            '            i = i + 1
            '        End If
            '    End While
            'Else
            '    'sino ya esta inscripto
            '    'div_Modal_error_inscripto.Visible = True
            '    'Modal_error_inscripto.Show()

            'End If
        End If
    End Sub

    Private Sub Eliminar_llave_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Eliminar_llave.ServerClick
        eliminar_llave_seleccionada()
    End Sub
    Private Sub eliminar_llave_seleccionada()
        Dim borrado As String = "no"
        'primero recorro la grilla 2 para ver si se selecciono algo
        Dim SELECCIONADO As CheckBox
        Dim i As Integer = 0
        While i < GridView2.Rows.Count
            SELECCIONADO = CType(GridView2.Rows(i).FindControl("CheckBox_item1"), CheckBox)
            If SELECCIONADO.Checked = True Then
                'como esta seleccionado procedo a borrar:
                Dim ds As DataSet = DAllave.LLave_obtener_llavegenerada_etc_2(GridView2.Rows(i).Cells(1).Text)
                Dim j As Integer = 0
                While j < ds.Tables(2).Rows.Count
                    'actualizo el estado en la tabla "inscripciones_x_torneo" pongo el campo en_llave='no'
                    Dim evento_id As Integer = ds.Tables(0).Rows(0).Item("evento_id")
                    Dim categoria_id As Integer = ds.Tables(0).Rows(0).Item("categoria_id")
                    Dim usuario_id As Integer = ds.Tables(2).Rows(j).Item("Llave_item_usuario_id")
                    DAllave.Llave_deshacer_llave(usuario_id, evento_id, categoria_id)
                    j = j + 1
                End While

                'aqui borro la llave
                DAllave.llave_eliminar(GridView2.Rows(i).Cells(1).Text)
                borrado = "si"
            End If
            i = i + 1
        End While
        If borrado = "si" Then
            'obtener_categorias(HF_evento_id.Value) no lo uso
            If Session("Tipo") = "administrador" Then
                obtener_llaves_generadas_info()
            Else
                obtener_llaves_generadas_infoArea(CInt(HF_area_id.Value))
            End If

        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Response.Redirect("../Torneo/Seleccion_Area.aspx")
    End Sub

    Dim categoria_id As Integer = 0
    Dim evento_id As Integer = 0
    Dim llave_id As Integer = 0

    Private Sub Btn_rptlucha_ServerClick(sender As Object, e As EventArgs) Handles Btn_rptlucha.ServerClick


        Dim modalidad As String = GridView2.Rows(0).Cells(2).Text
        llave_id = CInt(GridView2.Rows(0).Cells(1).Text)
        evento_id = HF_evento_id.Value


        If modalidad = "Lucha" Then

            Dim ds_llave As DataSet = DAllave.llave_obtenerinfo(CStr(llave_id))


            categoria_id = ds_llave.Tables(0).Rows(0).Item("categoria_id")

            Dim Llaves_ds As New Llaves_ds

            Dim B1 As String = ""
            Dim B2 As String = ""
            Dim B3 As String = ""
            Dim B4 As String = ""
            Dim B5 As String = ""
            Dim B6 As String = ""
            Dim B7 As String = ""

            llenar_encabezados(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7)

            crear_tabla_resultados(Llaves_ds)

            cargar_resultados_competencia

        End If




    End Sub



    Private Sub llenar_encabezados(ByVal evento_id As Integer, ByVal categoria_id As Integer, ByVal llave_id As Integer, ByRef Llaves_ds As DataSet, ByRef B1 As String, ByRef B2 As String, ByRef B3 As String, ByRef B4 As String, ByRef B5 As String, ByRef B6 As String, ByRef B7 As String)
        Dim ds_categorias As DataSet = DAllave.LLave_obtener_llavegenerada_etc_2(llave_id)

        Cargar_ListadoCompetidores(ds_categorias, Llaves_ds)

        Dim evento As String = ds_categorias.Tables(0).Rows(0).Item("evento_descripcion")
        Dim fecha As String = ds_categorias.Tables(0).Rows(0).Item("evento_fecha")
        Dim fecha_cierre As String = ds_categorias.Tables(0).Rows(0).Item("evento_fechacierre")



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
            categoria = "Categoria: " + categoria

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
                            'B1.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B1.ToolTip = usuario_id
                        B1 = tooltext + idtext
                    Case 2
                        If Llave_item_usuario_id <> 0 Then
                            'B2.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B2.ToolTip = usuario_id
                        B2 = tooltext + idtext
                    Case 3
                        If Llave_item_usuario_id <> 0 Then
                            'B3.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B3.ToolTip = usuario_id
                        B3 = tooltext + idtext
                    Case 4
                        If Llave_item_usuario_id <> 0 Then
                            'B4.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B4.ToolTip = usuario_id
                        B4 = tooltext + idtext
                    Case 5
                        If Llave_item_usuario_id <> 0 Then
                            'B5.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B5.ToolTip = usuario_id
                        B5 = tooltext + idtext
                    Case 6
                        If Llave_item_usuario_id <> 0 Then
                            'B6.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B6.ToolTip = usuario_id
                        B6 = tooltext + idtext
                    Case 7
                        If Llave_item_usuario_id <> 0 Then
                            'B7.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B7.ToolTip = usuario_id
                        B7 = tooltext + idtext
                End Select
                i = i + 1
            End While


        End If
    End Sub

    Private Sub colocar_tooltrip(ByVal ds As DataSet, ByVal item_nro As Integer, ByRef tooltext As String, ByRef idtext As String, ByRef usuario_id As Integer)
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

    Private Sub Cargar_ListadoCompetidores(ByVal ds_categorias As DataSet, ByRef Llaves_ds As DataSet)

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
        'GridView_COMPETIDORES.DataSource = Llaves_ds.Tables("Competidores")
        'GridView_COMPETIDORES.DataBind()

    End Sub


    Private Sub crear_tabla_resultados(ByRef Llaves_ds As DataSet)
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

        'GridView_RESULTADOS.DataSource = Llaves_ds.Tables("RESULTADOS")
        'GridView_RESULTADOS.DataBind()

    End Sub

    Private Sub cargar_resultados_competencia(ByRef B1 As String, ByRef B2 As String, ByRef B3 As String, ByRef B4 As String, ByRef B5 As String, ByRef B6 As String, ByRef B7 As String)
        'carga en la grilla
        If B7 <> "" Then
            LB_WINNER.Visible = True
            lb_1st.Text = B7
            Winners(B7, "1st")

            If B7 = B5 Then
                lb_2nd.Text = B6
                Winners(B6, "2nd")
            Else
                lb_2nd.Text = B5
                Winners(B5, "2nd")
            End If
            'aqui veo quien es el tercero
            If B1 <> "" And B2 <> "" Then
                If B1 = B5 Then
                    lb_3rd_a.Text = B2
                    Winners(B2, "3rd")
                End If
                If B2 = B5 Then
                    lb_3rd_a.Text = B1
                    Winners(B1, "3rd")
                End If
            End If
            If B3 <> "" And B4 <> "" Then
                If B3 = B6 Then
                    lb_3rd_b.Text = B4
                    Winners(B4, "4th")
                End If
                If B4 = B6 Then
                    lb_3rd_b.Text = B3
                    Winners(B3, "4th")
                End If
            End If
        End If
    End Sub



End Class