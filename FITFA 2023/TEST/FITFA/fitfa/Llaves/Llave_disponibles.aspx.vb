Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports Ionic.Zip
Public Class Llave_disponibles
    Inherits System.Web.UI.Page
    Dim DAllave As New Capa_de_datos.Llave
    Dim DAinscrip As New Capa_de_datos.Inscripciones
    Dim key_insc_ds As New Llaves_ds
    Dim DAevento As New Capa_de_datos.Eventos
    Dim DAinscripcion As New Capa_de_datos.Inscripciones
    Dim DAinstructor As New Capa_de_datos.Instructor
    'Dim Llaves_ds As New Llaves_ds
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

        Dim backupDestination As String = Server.MapPath("~/Archivos_pdf/Backup")
        If Not Directory.Exists(backupDestination) Then
            Directory.CreateDirectory(backupDestination)
        End If

        Dim Llaves_ds1 As New Llaves_ds

        evento_id = HF_evento_id.Value

        Dim i As Integer = 0
        While i < GridView2.Rows.Count
            Dim modalidad As String = GridView2.Rows(i).Cells(2).Text
            llave_id = CInt(GridView2.Rows(i).Cells(1).Text)
            Dim c_inscri As Integer = CInt(GridView2.Rows(i).Cells(4).Text) 'es la cantidad

            If (modalidad = "Lucha") Then

                Dim ds_llave As DataSet = DAllave.llave_obtenerinfo(CStr(llave_id))


                categoria_id = ds_llave.Tables(0).Rows(0).Item("categoria_id") 'tendria que ser 2

                Dim Llaves_ds As New Llaves_ds

                'LLAVE 2 ############################################################################################################## INICIO
                If (c_inscri = 2) Then

                    Dim B1 As String = ""
                    Dim B2 As String = ""
                    Dim B3 As String = ""

                    llenar_encabezados_llave2(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3)

                    crear_tabla_resultados(Llaves_ds)

                    cargar_resultados_competencia_llave2(B1, B2, B3, Llaves_ds)

                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                    Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                    fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                    fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                    fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                    fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                    fila2("ID") = 1
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                    CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_02.rpt"))

                    CrReport.Database.Tables("LLAVE_2").SetDataSource(Llaves_ds.Tables("LLAVE_2"))
                    CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                    CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                    Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                    Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                    CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                    Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                    filaruta("RUTA") = nombre_archivo + ".pdf"
                    Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)


                End If
                'LLAVE 2 ############################################################################################################## FIN

                'LLAVE 4 ############################################################################################################## INICIO
                If (c_inscri > 2) And (c_inscri <= 4) Then

                    Dim B1 As String = ""
                    Dim B2 As String = ""
                    Dim B3 As String = ""
                    Dim B4 As String = ""
                    Dim B5 As String = ""
                    Dim B6 As String = ""
                    Dim B7 As String = ""

                    llenar_encabezados(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7)

                    crear_tabla_resultados(Llaves_ds)

                    cargar_resultados_competencia(B1, B2, B3, B4, B5, B6, B7, Llaves_ds)

                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                    Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                    fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                    fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                    fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                    fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                    fila2("ID") = 1
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                    CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_04.rpt"))

                    CrReport.Database.Tables("LLAVE_4").SetDataSource(Llaves_ds.Tables("LLAVE_4"))
                    CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                    CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                    Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                    Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                    CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                    Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                    filaruta("RUTA") = nombre_archivo + ".pdf"
                    Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                End If
                'LLAVE 4 ############################################################################################################## FIN

                'LLAVE 8 ############################################################################################################## INICIO
                If (c_inscri > 4) And (c_inscri <= 8) Then
                    Dim B1 As String = ""
                    Dim B2 As String = ""
                    Dim B3 As String = ""
                    Dim B4 As String = ""
                    Dim B5 As String = ""
                    Dim B6 As String = ""
                    Dim B7 As String = ""
                    Dim B8 As String = ""
                    Dim B9 As String = ""
                    Dim B10 As String = ""
                    Dim B11 As String = ""
                    Dim B12 As String = ""
                    Dim B13 As String = ""
                    Dim B14 As String = ""
                    Dim B15 As String = ""

                    llenar_encabezados_llave8(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15)

                    crear_tabla_resultados(Llaves_ds)

                    cargar_resultados_competencia_llave8(B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, Llaves_ds)

                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                    Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                    fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                    fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                    fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                    fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                    fila2("ID") = 1
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                    CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_08.rpt"))

                    CrReport.Database.Tables("LLAVE_8").SetDataSource(Llaves_ds.Tables("LLAVE_8"))
                    CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                    CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                    Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                    Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                    CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                    Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                    filaruta("RUTA") = nombre_archivo + ".pdf"
                    Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                End If
                'LLAVE 8 ############################################################################################################## FIN

                If (c_inscri > 8) And (c_inscri <= 16) Then
                    Dim B1 As String = ""
                    Dim B2 As String = ""
                    Dim B3 As String = ""
                    Dim B4 As String = ""
                    Dim B5 As String = ""
                    Dim B6 As String = ""
                    Dim B7 As String = ""
                    Dim B8 As String = ""
                    Dim B9 As String = ""
                    Dim B10 As String = ""
                    Dim B11 As String = ""
                    Dim B12 As String = ""
                    Dim B13 As String = ""
                    Dim B14 As String = ""
                    Dim B15 As String = ""
                    Dim B16 As String = ""
                    Dim B17 As String = ""
                    Dim B18 As String = ""
                    Dim B19 As String = ""
                    Dim B20 As String = ""
                    Dim B21 As String = ""
                    Dim B22 As String = ""
                    Dim B23 As String = ""
                    Dim B24 As String = ""
                    Dim B25 As String = ""
                    Dim B26 As String = ""
                    Dim B27 As String = ""
                    Dim B28 As String = ""
                    Dim B29 As String = ""
                    Dim B30 As String = ""
                    Dim B31 As String = ""

                    llenar_encabezados_llave16(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15,
                            B16, B17, B18, B19, B20, B21, B22, B23, B24, B25, B26, B27, B28, B29, B30, B31)

                    crear_tabla_resultados(Llaves_ds)

                    cargar_resultados_competencia_llave16(B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15,
                                                          B16, B17, B18, B19, B20, B21, B22, B23, B24, B25, B26, B27, B28,
                                                          B29, B30, B31, Llaves_ds)

                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                    Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                    fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                    fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                    fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                    fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                    fila2("ID") = 1
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                    CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidor_16.rpt"))

                    CrReport.Database.Tables("LLAVE_16").SetDataSource(Llaves_ds.Tables("LLAVE_16"))
                    CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                    CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                    Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                    Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                    CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                    Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                    filaruta("RUTA") = nombre_archivo + ".pdf"
                    Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                End If
                If (c_inscri > 16) And (c_inscri <= 32) Then
                    Response.Redirect("Llave_32.aspx")
                End If





            End If
            i = i + 1
        End While

        If Llaves_ds1.Tables("LLAVES_PDF").Rows.Count <> 0 Then
            Session("tabla_pdf") = Llaves_ds1.Tables("LLAVES_PDF")

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../Archivos_pdf/Descargando.aspx');", True)

            'Response.Redirect("~/Archivos_pdf/Descargando.aspx")
        End If



        'llave_id = CInt(2)
        'evento_id = 105
    End Sub


    Private Sub llenar_encabezados_llave2(ByVal evento_id As Integer, ByVal categoria_id As Integer, ByVal llave_id As Integer, ByRef Llaves_ds As DataSet, ByRef B1 As String, ByRef B2 As String, ByRef B3 As String)
        Dim ds_categorias As DataSet = DAllave.LLave_obtener_llavegenerada_etc_2(llave_id)

        Cargar_ListadoCompetidores(ds_categorias, Llaves_ds)

        Dim evento As String = ds_categorias.Tables(0).Rows(0).Item("evento_descripcion")
        Dim fecha As String = ds_categorias.Tables(0).Rows(0).Item("evento_fecha")
        Dim fecha_cierre As String = ds_categorias.Tables(0).Rows(0).Item("evento_fechacierre")


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

            '####################################### LLENO TABLA LLAVE_DATOS PARA REPORTE ############################### INICIO

            Llaves_ds.Tables("LLAVE_DATOS").Rows.Clear()
            Dim fila1 As DataRow = Llaves_ds.Tables("LLAVE_DATOS").NewRow
            fila1("Evento") = evento
            fila1("Fecha") = CDate(fecha)
            fila1("Categoria") = categoria
            fila1("Cant_Inscriptos") = Llaves_ds.Tables("Competidores").Rows.Count
            fila1("ID") = 1
            Llaves_ds.Tables("LLAVE_DATOS").Rows.Add(fila1)


            '####################################### LLENO TABLA LLAVE_DATOS PARA REPORTE ############################### FIN







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

                End Select
                i = i + 1
            End While

            '####################################### LLENO TABLA LLAVE_2 PARA REPORTE ############################### INICIO
            Llaves_ds.Tables("LLAVE_2").Rows.Clear()

            Dim fila3 As DataRow = Llaves_ds.Tables("LLAVE_2").NewRow
            fila3("B1") = B1
            fila3("B2") = B2
            fila3("B3") = B3
            fila3("ID") = 1
            Llaves_ds.Tables("LLAVE_2").Rows.Add(fila3)
            '####################################### LLENO TABLA LLAVE_2 PARA REPORTE ############################### FIN


        End If


    End Sub

    Private Sub llenar_encabezados_llave8(ByVal evento_id As Integer, ByVal categoria_id As Integer, ByVal llave_id As Integer, ByRef Llaves_ds As DataSet, ByRef B1 As String, ByRef B2 As String, ByRef B3 As String, ByRef B4 As String, ByRef B5 As String, ByRef B6 As String, ByRef B7 As String, ByRef B8 As String, ByRef B9 As String, ByRef B10 As String, ByRef B11 As String, ByRef B12 As String, ByRef B13 As String, ByRef B14 As String, ByRef B15 As String)
        Dim ds_categorias As DataSet = DAllave.LLave_obtener_llavegenerada_etc_2(llave_id)

        Cargar_ListadoCompetidores(ds_categorias, Llaves_ds)

        Dim evento As String = ds_categorias.Tables(0).Rows(0).Item("evento_descripcion")
        Dim fecha As String = ds_categorias.Tables(0).Rows(0).Item("evento_fecha")
        Dim fecha_cierre As String = ds_categorias.Tables(0).Rows(0).Item("evento_fechacierre")


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

            '####################################### LLENO TABLA LLAVE_DATOS PARA REPORTE ############################### INICIO

            Llaves_ds.Tables("LLAVE_DATOS").Rows.Clear()
            Dim fila1 As DataRow = Llaves_ds.Tables("LLAVE_DATOS").NewRow
            fila1("Evento") = evento
            fila1("Fecha") = CDate(fecha)
            fila1("Categoria") = categoria
            fila1("Cant_Inscriptos") = Llaves_ds.Tables("Competidores").Rows.Count
            fila1("ID") = 1
            Llaves_ds.Tables("LLAVE_DATOS").Rows.Add(fila1)


            '####################################### LLENO TABLA LLAVE_DATOS PARA REPORTE ############################### FIN

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
                    Case 8
                        If Llave_item_usuario_id <> 0 Then
                            'B8.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B8.ToolTip = usuario_id
                        B8 = tooltext + idtext
                    Case 9
                        If Llave_item_usuario_id <> 0 Then
                            'B9.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B9.ToolTip = usuario_id
                        B9 = tooltext + idtext
                    Case 10
                        If Llave_item_usuario_id <> 0 Then
                            'B10.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B10.ToolTip = usuario_id
                        B10 = tooltext + idtext
                    Case 11
                        If Llave_item_usuario_id <> 0 Then
                            'B11.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B11.ToolTip = usuario_id
                        B11 = tooltext + idtext
                    Case 12
                        If Llave_item_usuario_id <> 0 Then
                            'B12.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B12.ToolTip = usuario_id
                        B12 = tooltext + idtext
                    Case 13
                        If Llave_item_usuario_id <> 0 Then
                            'B13.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B13.ToolTip = usuario_id
                        B13 = tooltext + idtext
                    Case 14
                        If Llave_item_usuario_id <> 0 Then
                            'B14.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B14.ToolTip = usuario_id
                        B14 = tooltext + idtext
                    Case 15
                        If Llave_item_usuario_id <> 0 Then
                            'B15.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B15.ToolTip = usuario_id
                        B15 = tooltext + idtext
                End Select
                i = i + 1
            End While

            '####################################### LLENO TABLA LLAVE_2 PARA REPORTE ############################### INICIO
            Llaves_ds.Tables("LLAVE_8").Rows.Clear()

            Dim fila3 As DataRow = Llaves_ds.Tables("LLAVE_8").NewRow
            fila3("B1") = B1
            fila3("B2") = B2
            fila3("B3") = B3
            fila3("B4") = B4
            fila3("B5") = B5
            fila3("B6") = B6
            fila3("B7") = B7
            fila3("B8") = B8
            fila3("B9") = B9
            fila3("B10") = B10
            fila3("B11") = B11
            fila3("B12") = B12
            fila3("B13") = B13
            fila3("B14") = B14
            fila3("B15") = B15
            fila3("ID") = 1
            Llaves_ds.Tables("LLAVE_8").Rows.Add(fila3)
            '####################################### LLENO TABLA LLAVE_2 PARA REPORTE ############################### FIN


        End If

    End Sub

    Private Sub llenar_encabezados_llave16(ByVal evento_id As Integer, ByVal categoria_id As Integer, ByVal llave_id As Integer, ByRef Llaves_ds As DataSet, ByRef B1 As String, ByRef B2 As String, ByRef B3 As String, ByRef B4 As String, ByRef B5 As String, ByRef B6 As String, ByRef B7 As String, ByRef B8 As String, ByRef B9 As String, ByRef B10 As String, ByRef B11 As String, ByRef B12 As String, ByRef B13 As String, ByRef B14 As String, ByRef B15 As String,
                                           ByRef B16 As String, ByRef B17 As String, ByRef B18 As String, ByRef B19 As String, ByRef B20 As String, ByRef B21 As String, ByRef B22 As String, ByRef B23 As String, ByRef B24 As String,
                                           ByRef B25 As String, ByRef B26 As String, ByRef B27 As String, ByRef B28 As String, ByRef B29 As String, ByRef B30 As String, ByRef B31 As String)

        Dim ds_categorias As DataSet = DAllave.LLave_obtener_llavegenerada_etc_2(llave_id)

        Cargar_ListadoCompetidores(ds_categorias, Llaves_ds)

        Dim evento As String = ds_categorias.Tables(0).Rows(0).Item("evento_descripcion")
        Dim fecha As String = ds_categorias.Tables(0).Rows(0).Item("evento_fecha")
        Dim fecha_cierre As String = ds_categorias.Tables(0).Rows(0).Item("evento_fechacierre")


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

            '####################################### LLENO TABLA LLAVE_DATOS PARA REPORTE ############################### INICIO

            Llaves_ds.Tables("LLAVE_DATOS").Rows.Clear()
            Dim fila1 As DataRow = Llaves_ds.Tables("LLAVE_DATOS").NewRow
            fila1("Evento") = evento
            fila1("Fecha") = CDate(fecha)
            fila1("Categoria") = categoria
            fila1("Cant_Inscriptos") = Llaves_ds.Tables("Competidores").Rows.Count
            fila1("ID") = 1
            Llaves_ds.Tables("LLAVE_DATOS").Rows.Add(fila1)


            '####################################### LLENO TABLA LLAVE_DATOS PARA REPORTE ############################### FIN

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
                    Case 8
                        If Llave_item_usuario_id <> 0 Then
                            'B8.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B8.ToolTip = usuario_id
                        B8 = tooltext + idtext
                    Case 9
                        If Llave_item_usuario_id <> 0 Then
                            'B9.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B9.ToolTip = usuario_id
                        B9 = tooltext + idtext
                    Case 10
                        If Llave_item_usuario_id <> 0 Then
                            'B10.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B10.ToolTip = usuario_id
                        B10 = tooltext + idtext
                    Case 11
                        If Llave_item_usuario_id <> 0 Then
                            'B11.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B11.ToolTip = usuario_id
                        B11 = tooltext + idtext
                    Case 12
                        If Llave_item_usuario_id <> 0 Then
                            'B12.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B12.ToolTip = usuario_id
                        B12 = tooltext + idtext
                    Case 13
                        If Llave_item_usuario_id <> 0 Then
                            'B13.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B13.ToolTip = usuario_id
                        B13 = tooltext + idtext
                    Case 14
                        If Llave_item_usuario_id <> 0 Then
                            'B14.Visible = True
                        End If

                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B14.ToolTip = usuario_id
                        B14 = tooltext + idtext
                    Case 15
                        If Llave_item_usuario_id <> 0 Then
                            'B15.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B15.ToolTip = usuario_id
                        B15 = tooltext + idtext
                    Case 16
                        If Llave_item_usuario_id <> 0 Then
                            'B16.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B16.ToolTip = usuario_id
                        B16 = tooltext + idtext
                    Case 17
                        If Llave_item_usuario_id <> 0 Then
                            'B17.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B17.ToolTip = usuario_id
                        B17 = tooltext + idtext
                    Case 18
                        If Llave_item_usuario_id <> 0 Then
                            'B18.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B18.ToolTip = usuario_id
                        B18 = tooltext + idtext
                    Case 19
                        If Llave_item_usuario_id <> 0 Then
                            'B19.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B19.ToolTip = usuario_id
                        B19 = tooltext + idtext
                    Case 20
                        If Llave_item_usuario_id <> 0 Then
                            'B20.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B20.ToolTip = usuario_id
                        B20 = tooltext + idtext
                    Case 21
                        If Llave_item_usuario_id <> 0 Then
                            'B21.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B21.ToolTip = usuario_id
                        B21 = tooltext + idtext
                    Case 22
                        If Llave_item_usuario_id <> 0 Then
                            'B22.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B22.ToolTip = usuario_id
                        B22 = tooltext + idtext
                    Case 23
                        If Llave_item_usuario_id <> 0 Then
                            'B23.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B23.ToolTip = usuario_id
                        B23 = tooltext + idtext
                    Case 24
                        If Llave_item_usuario_id <> 0 Then
                            'B24.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B24.ToolTip = usuario_id
                        B24 = tooltext + idtext
                    Case 25
                        If Llave_item_usuario_id <> 0 Then
                            'B25.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B25.ToolTip = usuario_id
                        B25 = tooltext + idtext
                    Case 26
                        If Llave_item_usuario_id <> 0 Then
                            'B26.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B26.ToolTip = usuario_id
                        B26 = tooltext + idtext
                    Case 27
                        If Llave_item_usuario_id <> 0 Then
                            'B27.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B27 = usuario_id
                        B27 = tooltext + idtext
                    Case 28
                        If Llave_item_usuario_id <> 0 Then
                            'B28.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B28.ToolTip = usuario_id
                        B28 = tooltext + idtext
                    Case 29
                        If Llave_item_usuario_id <> 0 Then
                            'B29.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B29.ToolTip = usuario_id
                        B29 = tooltext + idtext
                    Case 30
                        If Llave_item_usuario_id <> 0 Then
                            'B30.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B30.ToolTip = usuario_id
                        B30 = tooltext + idtext
                    Case 31
                        If Llave_item_usuario_id <> 0 Then
                            'B31.Visible = True
                        End If
                        colocar_tooltrip(ds_categorias, item_nro, tooltext, idtext, usuario_id)
                        'B31.ToolTip = usuario_id
                        B31 = tooltext + idtext
                End Select
                i = i + 1
            End While

            '####################################### LLENO TABLA LLAVE_2 PARA REPORTE ############################### INICIO
            Llaves_ds.Tables("LLAVE_16").Rows.Clear()

            Dim fila3 As DataRow = Llaves_ds.Tables("LLAVE_16").NewRow
            fila3("B1") = B1
            fila3("B2") = B2
            fila3("B3") = B3
            fila3("B4") = B4
            fila3("B5") = B5
            fila3("B6") = B6
            fila3("B7") = B7
            fila3("B8") = B8
            fila3("B9") = B9
            fila3("B10") = B10
            fila3("B11") = B11
            fila3("B12") = B12
            fila3("B13") = B13
            fila3("B14") = B14
            fila3("B15") = B15
            fila3("B16") = B16
            fila3("B17") = B17
            fila3("B18") = B18
            fila3("B19") = B19
            fila3("B20") = B20
            fila3("B21") = B21
            fila3("B22") = B22
            fila3("B23") = B23
            fila3("B24") = B24
            fila3("B25") = B25
            fila3("B26") = B26
            fila3("B27") = B27
            fila3("B28") = B28
            fila3("B29") = B29
            fila3("B30") = B30
            fila3("B31") = B31
            fila3("ID") = 1
            Llaves_ds.Tables("LLAVE_16").Rows.Add(fila3)
            '####################################### LLENO TABLA LLAVE_2 PARA REPORTE ############################### FIN


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

            '####################################### LLENO TABLA LLAVE_DATOS PARA REPORTE ############################### INICIO

            Llaves_ds.Tables("LLAVE_DATOS").Rows.Clear()
            Dim fila1 As DataRow = Llaves_ds.Tables("LLAVE_DATOS").NewRow
            fila1("Evento") = evento
            fila1("Fecha") = CDate(fecha)
            fila1("Categoria") = categoria
            fila1("Cant_Inscriptos") = Llaves_ds.Tables("Competidores").Rows.Count
            fila1("ID") = 1
            Llaves_ds.Tables("LLAVE_DATOS").Rows.Add(fila1)


            '####################################### LLENO TABLA LLAVE_DATOS PARA REPORTE ############################### FIN







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

            '####################################### LLENO TABLA LLAVE_4 PARA REPORTE ############################### INICIO
            Llaves_ds.Tables("LLAVE_4").Rows.Clear()

            Dim fila3 As DataRow = Llaves_ds.Tables("LLAVE_4").NewRow
            fila3("B1") = B1
            fila3("B2") = B2
            fila3("B3") = B3
            fila3("B4") = B4
            fila3("B5") = B5
            fila3("B6") = B6
            fila3("B7") = B7
            fila3("ID") = 1
            Llaves_ds.Tables("LLAVE_4").Rows.Add(fila3)
            '####################################### LLENO TABLA LLAVE_4 PARA REPORTE ############################### FIN


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
        'SE COLOCA EL COMPETIDOR Y EL INSTRUCTOR EN LA TABLA "COMPETIDORES"


        Llaves_ds.Tables("Competidores").Rows.Clear()

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
        Llaves_ds.Tables("RESULTADOS").Rows.Clear() '*******************************NUEVO

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

    Private Sub cargar_resultados_competencia_llave2(ByRef B1 As String, ByRef B2 As String, ByRef B3 As String, ByRef Llaves_ds As DataSet)
        If B3 <> "" Then
            'lb_1st.Text = B3.Text
            Winners(B3, "1st", Llaves_ds)

            'veo cual es el segundo
            If B3 = B1 Then
                'el segundo es b2
                'lb_2nd.Text = B2.Text
                Winners(B2, "2nd", Llaves_ds)
            Else
                'lb_2nd.Text = B1.Text
                Winners(B1, "2nd", Llaves_ds)
            End If
        End If
        'no hay 3ros, porque es una llave de 2
    End Sub

    Private Sub cargar_resultados_competencia_llave8(ByRef B1 As String, ByRef B2 As String, ByRef B3 As String, ByRef B4 As String, ByRef B5 As String,
                                                     ByRef B6 As String, ByRef B7 As String, ByRef B8 As String, ByRef B9 As String,
                                                     ByRef B10 As String, ByRef B11 As String, ByRef B12 As String, ByRef B13 As String,
                                                     ByRef B14 As String, ByRef B15 As String, ByRef Llaves_ds As DataSet)

        If B15 <> "" Then
            'LB_WINNER.Visible = True
            'lb_1st.Text = B15.Text
            Winners(B15, "1st", Llaves_ds)
            If B15 = B13 Then
                'lb_2nd.Text = B14.Text
                Winners(B14, "2nd", Llaves_ds)
            Else
                'lb_2nd.Text = B13.Text
                Winners(B13, "2nd", Llaves_ds)
            End If
            'aqui veo quien es el tercero
            If B9 <> "" And B10 <> "" Then
                If B9 = B13 Then
                    'lb_3rd_a.Text = B10.Text
                    Winners(B10, "3rd", Llaves_ds)
                End If
                If B10 = B13 Then
                    'lb_3rd_a.Text = B9.Text
                    Winners(B9, "3rd", Llaves_ds)
                End If
            End If
            If B11 <> "" And B12 <> "" Then
                If B11 = B14 Then
                    'lb_3rd_b.Text = B12.Text
                    Winners(B12, "4th", Llaves_ds)
                End If
                If B12 = B14 Then
                    'lb_3rd_b.Text = B11.Text
                    Winners(B11, "4th", Llaves_ds)
                End If
            End If
        End If

    End Sub
    Private Sub cargar_resultados_competencia_llave16(ByRef B1 As String, ByRef B2 As String, ByRef B3 As String, ByRef B4 As String, ByRef B5 As String,
                                                     ByRef B6 As String, ByRef B7 As String, ByRef B8 As String, ByRef B9 As String,
                                                     ByRef B10 As String, ByRef B11 As String, ByRef B12 As String, ByRef B13 As String,
                                                     ByRef B14 As String, ByRef B15 As String, ByRef B16 As String,
                                                     ByRef B17 As String, ByRef B18 As String, ByRef B19 As String,
                                                     ByRef B20 As String, ByRef B21 As String, ByRef B22 As String,
                                                     ByRef B23 As String, ByRef B24 As String, ByRef B25 As String,
                                                     ByRef B26 As String, ByRef B27 As String, ByRef B28 As String,
                                                     ByRef B29 As String, ByRef B30 As String, ByRef B31 As String, ByRef Llaves_ds As DataSet)
        If B31 <> "" Then
            'LB_WINNER.Visible = True
            'lb_1st.Text = B31.Text
            Winners(B31, "1st", Llaves_ds)
            If B31 = B29 Then
                'lb_2nd.Text = B30.Text
                Winners(B30, "2nd", Llaves_ds)
            Else
                'lb_2nd.Text = B29.Text
                Winners(B29, "2nd", Llaves_ds)
            End If
            'aqui veo quien es el tercero
            If B25 <> "" And B26 <> "" Then
                If B25 = B29 Then
                    'lb_3rd_a.Text = B26.Text
                    Winners(B26, "3rd", Llaves_ds)
                End If
                If B26 = B29 Then
                    'lb_3rd_a.Text = B25.Text
                    Winners(B25, "3rd", Llaves_ds)
                End If
            End If
            If B27 <> "" And B28 <> "" Then
                If B27 = B30 Then
                    'lb_3rd_b.Text = B28.Text
                    Winners(B28, "4th", Llaves_ds)
                End If
                If B28 = B30 Then
                    'lb_3rd_b.Text = B27.Text
                    Winners(B27, "4th", Llaves_ds)
                End If
            End If
        End If




    End Sub


    Private Sub cargar_resultados_competencia(ByRef B1 As String, ByRef B2 As String, ByRef B3 As String, ByRef B4 As String, ByRef B5 As String, ByRef B6 As String, ByRef B7 As String, ByRef Llaves_ds As DataSet)
        'carga en la grilla
        If B7 <> "" Then
            'LB_WINNER.Visible = True
            'lb_1st.Text = B7
            Winners(B7, "1st", Llaves_ds)

            If B7 = B5 Then
                'lb_2nd.Text = B6
                Winners(B6, "2nd", Llaves_ds)
            Else
                'lb_2nd.Text = B5
                Winners(B5, "2nd", Llaves_ds)
            End If
            'aqui veo quien es el tercero
            If B1 <> "" And B2 <> "" Then
                If B1 = B5 Then
                    'lb_3rd_a.Text = B2
                    Winners(B2, "3rd", Llaves_ds)
                End If
                If B2 = B5 Then
                    'lb_3rd_a.Text = B1
                    Winners(B1, "3rd", Llaves_ds)
                End If
            End If
            If B3 <> "" And B4 <> "" Then
                If B3 = B6 Then
                    'lb_3rd_b.Text = B4
                    Winners(B4, "4th", Llaves_ds)
                End If
                If B4 = B6 Then
                    'lb_3rd_b.Text = B3
                    Winners(B3, "4th", Llaves_ds)
                End If
            End If
        End If
    End Sub


    Private Sub Winners(ByVal competidor As String, ByVal puesto As String, ByRef Llaves_ds As DataSet)
        Select Case puesto
            Case "1st"
                Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor") = competidor
               'GridView_RESULTADOS.Rows(0).Cells(1).Text = competidor

            Case "2nd"
                Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor") = competidor
                'GridView_RESULTADOS.Rows(1).Cells(1).Text = competidor


            Case "3rd"
                Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor") = competidor
                'GridView_RESULTADOS.Rows(2).Cells(1).Text = competidor

            Case "4th"
                Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor") = competidor
                'GridView_RESULTADOS.Rows(3).Cells(1).Text = competidor

        End Select
        'NOTA: CUANDO QUIERO AGREGAR LA INFO DEL INSTRUCTOR, ESO SE RECUPERA DE LA TABLA COMPETIDORES...Y NOTE QUE CUANDO EL NOMBRE TIENE ALGUN ACENTO SE MUESTRAN CARACTERES RAROS, X ESTA MISMA RAZON CUANDO COMPARO CON LA TABLA COMPETIDORES, NO LO ENCUENTRO NUNCA AL COMPETIDOR...DECIDI QUITAR DE LA TABLA RESULTADOS LA COLUMNA INSTRUCTOR


    End Sub



    Private Sub Btn_rptforma_ServerClick(sender As Object, e As EventArgs) Handles Btn_rptforma.ServerClick


        Dim backupDestination As String = Server.MapPath("~/Archivos_pdf/Backup")
        If Not Directory.Exists(backupDestination) Then
            Directory.CreateDirectory(backupDestination)
        End If

        Dim Llaves_ds1 As New Llaves_ds

        evento_id = HF_evento_id.Value

        Dim i As Integer = 0
        While i < GridView2.Rows.Count
            Dim modalidad As String = GridView2.Rows(i).Cells(2).Text
            llave_id = CInt(GridView2.Rows(i).Cells(1).Text)
            Dim c_inscri As Integer = CInt(GridView2.Rows(i).Cells(4).Text) 'es la cantidad

            If (modalidad = "Forma") Then

                Dim ds_llave As DataSet = DAllave.llave_obtenerinfo(CStr(llave_id))


                categoria_id = ds_llave.Tables(0).Rows(0).Item("categoria_id") 'tendria que ser 2

                Dim Llaves_ds As New Llaves_ds

                'LLAVE 2 ############################################################################################################## INICIO
                If (c_inscri = 2) Then

                    Dim B1 As String = ""
                    Dim B2 As String = ""
                    Dim B3 As String = ""

                    llenar_encabezados_llave2(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3)

                    crear_tabla_resultados(Llaves_ds)

                    cargar_resultados_competencia_llave2(B1, B2, B3, Llaves_ds)

                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                    Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                    fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                    fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                    fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                    fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                    fila2("ID") = 1
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                    CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_02.rpt"))

                    CrReport.Database.Tables("LLAVE_2").SetDataSource(Llaves_ds.Tables("LLAVE_2"))
                    CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                    CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                    Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                    Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                    CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                    Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                    filaruta("RUTA") = nombre_archivo + ".pdf"
                    Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)


                End If
                'LLAVE 2 ############################################################################################################## FIN

                'LLAVE 4 ############################################################################################################## INICIO
                If (c_inscri > 2) And (c_inscri <= 4) Then

                    Dim B1 As String = ""
                    Dim B2 As String = ""
                    Dim B3 As String = ""
                    Dim B4 As String = ""
                    Dim B5 As String = ""
                    Dim B6 As String = ""
                    Dim B7 As String = ""

                    llenar_encabezados(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7)

                    crear_tabla_resultados(Llaves_ds)

                    cargar_resultados_competencia(B1, B2, B3, B4, B5, B6, B7, Llaves_ds)

                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                    Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                    fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                    fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                    fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                    fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                    fila2("ID") = 1
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                    CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_04.rpt"))

                    CrReport.Database.Tables("LLAVE_4").SetDataSource(Llaves_ds.Tables("LLAVE_4"))
                    CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                    CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                    Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                    Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                    CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                    Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                    filaruta("RUTA") = nombre_archivo + ".pdf"
                    Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                End If
                'LLAVE 4 ############################################################################################################## FIN

                'LLAVE 8 ############################################################################################################## INICIO
                If (c_inscri > 4) And (c_inscri <= 8) Then
                    Dim B1 As String = ""
                    Dim B2 As String = ""
                    Dim B3 As String = ""
                    Dim B4 As String = ""
                    Dim B5 As String = ""
                    Dim B6 As String = ""
                    Dim B7 As String = ""
                    Dim B8 As String = ""
                    Dim B9 As String = ""
                    Dim B10 As String = ""
                    Dim B11 As String = ""
                    Dim B12 As String = ""
                    Dim B13 As String = ""
                    Dim B14 As String = ""
                    Dim B15 As String = ""

                    llenar_encabezados_llave8(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15)

                    crear_tabla_resultados(Llaves_ds)

                    cargar_resultados_competencia_llave8(B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, Llaves_ds)

                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                    Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                    fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                    fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                    fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                    fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                    fila2("ID") = 1
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                    CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_08.rpt"))

                    CrReport.Database.Tables("LLAVE_8").SetDataSource(Llaves_ds.Tables("LLAVE_8"))
                    CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                    CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                    Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                    Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                    CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                    Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                    filaruta("RUTA") = nombre_archivo + ".pdf"
                    Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                End If
                'LLAVE 8 ############################################################################################################## FIN

                If (c_inscri > 8) And (c_inscri <= 16) Then
                    Dim B1 As String = ""
                    Dim B2 As String = ""
                    Dim B3 As String = ""
                    Dim B4 As String = ""
                    Dim B5 As String = ""
                    Dim B6 As String = ""
                    Dim B7 As String = ""
                    Dim B8 As String = ""
                    Dim B9 As String = ""
                    Dim B10 As String = ""
                    Dim B11 As String = ""
                    Dim B12 As String = ""
                    Dim B13 As String = ""
                    Dim B14 As String = ""
                    Dim B15 As String = ""
                    Dim B16 As String = ""
                    Dim B17 As String = ""
                    Dim B18 As String = ""
                    Dim B19 As String = ""
                    Dim B20 As String = ""
                    Dim B21 As String = ""
                    Dim B22 As String = ""
                    Dim B23 As String = ""
                    Dim B24 As String = ""
                    Dim B25 As String = ""
                    Dim B26 As String = ""
                    Dim B27 As String = ""
                    Dim B28 As String = ""
                    Dim B29 As String = ""
                    Dim B30 As String = ""
                    Dim B31 As String = ""

                    llenar_encabezados_llave16(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15,
                            B16, B17, B18, B19, B20, B21, B22, B23, B24, B25, B26, B27, B28, B29, B30, B31)

                    crear_tabla_resultados(Llaves_ds)

                    cargar_resultados_competencia_llave16(B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15,
                                                          B16, B17, B18, B19, B20, B21, B22, B23, B24, B25, B26, B27, B28,
                                                          B29, B30, B31, Llaves_ds)

                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                    Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                    fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                    fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                    fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                    fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                    fila2("ID") = 1
                    Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                    '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                    CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidor_16.rpt"))

                    CrReport.Database.Tables("LLAVE_16").SetDataSource(Llaves_ds.Tables("LLAVE_16"))
                    CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                    CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                    Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                    Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                    CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                    Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                    filaruta("RUTA") = nombre_archivo + ".pdf"
                    Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                End If
                If (c_inscri > 16) And (c_inscri <= 32) Then
                    Response.Redirect("Llave_32.aspx")
                End If





            End If
            i = i + 1
        End While

        If Llaves_ds1.Tables("LLAVES_PDF").Rows.Count <> 0 Then
            Session("tabla_pdf") = Llaves_ds1.Tables("LLAVES_PDF")

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../Archivos_pdf/Descargando.aspx');", True)

            'Response.Redirect("~/Archivos_pdf/Descargando.aspx")
        End If


    End Sub

    Private Sub Btn_rptseleccion_ServerClick(sender As Object, e As EventArgs) Handles Btn_rptseleccion.ServerClick


        Dim backupDestination As String = Server.MapPath("~/Archivos_pdf/Backup")
        If Not Directory.Exists(backupDestination) Then
            Directory.CreateDirectory(backupDestination)
        End If

        Dim Llaves_ds1 As New Llaves_ds

        evento_id = HF_evento_id.Value

        Dim i As Integer = 0
        While i < GridView2.Rows.Count
            Dim modalidad As String = GridView2.Rows(i).Cells(2).Text
            llave_id = CInt(GridView2.Rows(i).Cells(1).Text)
            Dim c_inscri As Integer = CInt(GridView2.Rows(i).Cells(4).Text) 'es la cantidad

                    Dim SELECCIONADO As CheckBox
                    SELECCIONADO = CType(GridView2.Rows(i).FindControl("CheckBox_item1"), CheckBox)
                    If SELECCIONADO.Checked = True Then

                        Dim ds_llave As DataSet = DAllave.llave_obtenerinfo(CStr(llave_id))


                        categoria_id = ds_llave.Tables(0).Rows(0).Item("categoria_id") 'tendria que ser 2

                        Dim Llaves_ds As New Llaves_ds

                        'LLAVE 2 ############################################################################################################## INICIO
                        If (c_inscri = 2) Then

                            Dim B1 As String = ""
                            Dim B2 As String = ""
                            Dim B3 As String = ""

                            llenar_encabezados_llave2(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3)

                            crear_tabla_resultados(Llaves_ds)

                            cargar_resultados_competencia_llave2(B1, B2, B3, Llaves_ds)

                            '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                            Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                            Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                            fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                            fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                            fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                            fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                            fila2("ID") = 1
                            Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                            '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                            Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                            CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                            CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_02.rpt"))

                            CrReport.Database.Tables("LLAVE_2").SetDataSource(Llaves_ds.Tables("LLAVE_2"))
                            CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                            CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                            Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                            Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                            CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                            CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                            Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                            filaruta("RUTA") = nombre_archivo + ".pdf"
                            Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)


                        End If
                        'LLAVE 2 ############################################################################################################## FIN

                        'LLAVE 4 ############################################################################################################## INICIO
                        If (c_inscri > 2) And (c_inscri <= 4) Then

                            Dim B1 As String = ""
                            Dim B2 As String = ""
                            Dim B3 As String = ""
                            Dim B4 As String = ""
                            Dim B5 As String = ""
                            Dim B6 As String = ""
                            Dim B7 As String = ""

                            llenar_encabezados(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7)

                            crear_tabla_resultados(Llaves_ds)

                            cargar_resultados_competencia(B1, B2, B3, B4, B5, B6, B7, Llaves_ds)

                            '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                            Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                            Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                            fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                            fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                            fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                            fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                            fila2("ID") = 1
                            Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                            '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                            Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                            CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                            CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_04.rpt"))

                            CrReport.Database.Tables("LLAVE_4").SetDataSource(Llaves_ds.Tables("LLAVE_4"))
                            CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                            CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                            Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                            Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                            CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                            CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                            Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                            filaruta("RUTA") = nombre_archivo + ".pdf"
                            Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                        End If
                        'LLAVE 4 ############################################################################################################## FIN

                        'LLAVE 8 ############################################################################################################## INICIO
                        If (c_inscri > 4) And (c_inscri <= 8) Then
                            Dim B1 As String = ""
                            Dim B2 As String = ""
                            Dim B3 As String = ""
                            Dim B4 As String = ""
                            Dim B5 As String = ""
                            Dim B6 As String = ""
                            Dim B7 As String = ""
                            Dim B8 As String = ""
                            Dim B9 As String = ""
                            Dim B10 As String = ""
                            Dim B11 As String = ""
                            Dim B12 As String = ""
                            Dim B13 As String = ""
                            Dim B14 As String = ""
                            Dim B15 As String = ""

                            llenar_encabezados_llave8(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15)

                            crear_tabla_resultados(Llaves_ds)

                            cargar_resultados_competencia_llave8(B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, Llaves_ds)

                            '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                            Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                            Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                            fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                            fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                            fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                            fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                            fila2("ID") = 1
                            Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                            '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                            Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                            CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                            CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidores_08.rpt"))

                            CrReport.Database.Tables("LLAVE_8").SetDataSource(Llaves_ds.Tables("LLAVE_8"))
                            CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                            CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                            Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                            Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                            CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                            CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                            Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                            filaruta("RUTA") = nombre_archivo + ".pdf"
                            Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                        End If
                        'LLAVE 8 ############################################################################################################## FIN

                        If (c_inscri > 8) And (c_inscri <= 16) Then
                            Dim B1 As String = ""
                            Dim B2 As String = ""
                            Dim B3 As String = ""
                            Dim B4 As String = ""
                            Dim B5 As String = ""
                            Dim B6 As String = ""
                            Dim B7 As String = ""
                            Dim B8 As String = ""
                            Dim B9 As String = ""
                            Dim B10 As String = ""
                            Dim B11 As String = ""
                            Dim B12 As String = ""
                            Dim B13 As String = ""
                            Dim B14 As String = ""
                            Dim B15 As String = ""
                            Dim B16 As String = ""
                            Dim B17 As String = ""
                            Dim B18 As String = ""
                            Dim B19 As String = ""
                            Dim B20 As String = ""
                            Dim B21 As String = ""
                            Dim B22 As String = ""
                            Dim B23 As String = ""
                            Dim B24 As String = ""
                            Dim B25 As String = ""
                            Dim B26 As String = ""
                            Dim B27 As String = ""
                            Dim B28 As String = ""
                            Dim B29 As String = ""
                            Dim B30 As String = ""
                            Dim B31 As String = ""

                            llenar_encabezados_llave16(evento_id, categoria_id, llave_id, Llaves_ds, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15,
                            B16, B17, B18, B19, B20, B21, B22, B23, B24, B25, B26, B27, B28, B29, B30, B31)

                            crear_tabla_resultados(Llaves_ds)

                            cargar_resultados_competencia_llave16(B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15,
                                                          B16, B17, B18, B19, B20, B21, B22, B23, B24, B25, B26, B27, B28,
                                                          B29, B30, B31, Llaves_ds)

                            '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### INICIO
                            Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Clear()
                            Dim fila2 As DataRow = Llaves_ds.Tables("LLAVE_RESULTADOS").NewRow
                            fila2("1st") = Llaves_ds.Tables("RESULTADOS").Rows(0).Item("Competidor")
                            fila2("2nd") = Llaves_ds.Tables("RESULTADOS").Rows(1).Item("Competidor")
                            fila2("3rd_a") = Llaves_ds.Tables("RESULTADOS").Rows(2).Item("Competidor")
                            fila2("3rd_b") = Llaves_ds.Tables("RESULTADOS").Rows(3).Item("Competidor")
                            fila2("ID") = 1
                            Llaves_ds.Tables("LLAVE_RESULTADOS").Rows.Add(fila2)
                            '####################################### LLENO TABLA LLAVE_RESULTADOS PARA REPORTE ############################### FIN

                            Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                            CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                            CrReport.Load(Server.MapPath("~/Llaves/Reporte_llaves/Competidor_16.rpt"))

                            CrReport.Database.Tables("LLAVE_16").SetDataSource(Llaves_ds.Tables("LLAVE_16"))
                            CrReport.Database.Tables("LLAVE_DATOS").SetDataSource(Llaves_ds.Tables("LLAVE_DATOS"))
                            CrReport.Database.Tables("LLAVE_RESULTADOS").SetDataSource(Llaves_ds.Tables("LLAVE_RESULTADOS"))

                            Dim nombre_archivo As String = Lb_evento.Text + "_" + modalidad + "_llave" + llave_id.ToString

                            Dim ruta As String = "/Archivos_pdf/Backup/" + nombre_archivo + ".pdf"

                            CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))

                            CrReport.Dispose() 'esto hago para que no me genere un desbordamiento cuando son muchos rpt q se crean.

                            Dim filaruta As DataRow = Llaves_ds1.Tables("LLAVES_PDF").NewRow
                            filaruta("RUTA") = nombre_archivo + ".pdf"
                            Llaves_ds1.Tables("LLAVES_PDF").Rows.Add(filaruta)

                        End If
                        If (c_inscri > 16) And (c_inscri <= 32) Then
                            Response.Redirect("Llave_32.aspx")
                        End If





                    End If
                    i = i + 1
        End While

        If Llaves_ds1.Tables("LLAVES_PDF").Rows.Count <> 0 Then
            Session("tabla_pdf") = Llaves_ds1.Tables("LLAVES_PDF")

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../Archivos_pdf/Descargando.aspx');", True)

            'Response.Redirect("~/Archivos_pdf/Descargando.aspx")
        End If




    End Sub
End Class