Public Class PagosCobrosReclamos_resumen
  Inherits System.Web.UI.Page
  Dim DAanticipados As New Capa_Datos.WC_anticipados
  Dim Daparametro As New Capa_Datos.WC_parametro
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DS_pagoscobrosreclamos As New DS_pagoscobrosreclamos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos
      'Txt_cliente_codigo.Focus()
      'recuperar fecha de tabla parametro.
      Dim ds_fecha As DataSet = Daparametro.Parametro_obtener_dia
      If ds_fecha.Tables(0).Rows.Count <> 0 Then
        Dim FECHA As Date = CDate(ds_fecha.Tables(0).Rows(0).Item("Fecha"))
        Hf_FECHA.Value = FECHA
        txt_fecha.Text = FECHA.ToString("yyyy-MM-dd")
        obtener_resumen(FECHA)

        txt_fecha.Focus()
      End If

    End If
  End Sub

  Private Sub Permisos()
    'validamos permisos del login
    Dim Idusuario As Integer = CInt(Request.Cookies("Token_Idusuario").Value)
    Dim ds_usu As DataSet = DAusuario.Usuarios_buscarID(Idusuario)
    If ds_usu.Tables(0).Rows.Count <> 0 Then
      Dim Jerarquia As String = ""
      Try
        Jerarquia = ds_usu.Tables(0).Rows(0).Item("Jerarquia")
      Catch ex As Exception
      End Try

      Select Case Jerarquia
        Case "1"
          'se accede sin problemas.

        Case "2"
          'se verifica que permisos estan habilitados.
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 4 or null

          Dim ds_permisos As DataSet = DApermisos.Permisos_buscar(Idusuario)
          Dim i As Integer = 0
          Dim valido As String = "no"
          While i < ds_permisos.Tables(0).Rows.Count
            Dim Menu As String = ""
            Try
              Menu = ds_permisos.Tables(0).Rows(i).Item("Menu")
            Catch ex As Exception
            End Try
            Dim Opcion As String = ""
            Try
              Opcion = ds_permisos.Tables(0).Rows(i).Item("Opcion")
            Catch ex As Exception
            End Try
            If (Menu = "3" And Opcion = "") Or (Menu = "3" And Opcion = "4") Then
              valido = "si"
              Exit While
            End If
            i = i + 1
          End While
          If valido = "si" Then
            'se accede sin problemas
          Else
            'no tiene permiso, se redirige a menu.
            Response.Redirect("~/Inicio.aspx")
          End If
      End Select
    End If

    If Session("op_ingreso") = "si" Then
      Session("op_ingreso") = ""
    Else
      Session("op_ingreso") = ""
      Response.Redirect("~/Inicio.aspx")
    End If

  End Sub

  'Dim DS_pagoscobrosreclamos As New DS_pagoscobrosreclamos
  Private Sub obtener_resumen(ByVal Fecha As Date)
    DS_pagoscobrosreclamos.Tabla1.Rows.Clear()

    GridView1.Columns(0).Visible = True 'columna ID


    Dim ds_info As DataSet = DAanticipados.Anticipados_resumen(Fecha)
    If ds_info.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_info.Tables(0).Rows.Count
        Dim tipo As String = ds_info.Tables(0).Rows(i).Item("Tipo")
        Dim fila As DataRow = DS_pagoscobrosreclamos.Tabla1.NewRow
        fila("Anticipados_id") = ds_info.Tables(0).Rows(i).Item("Anticipados_id")
        fila("Tipo") = tipo
        fila("Codigo") = ds_info.Tables(0).Rows(i).Item("Codigo")
        fila("Nombre") = ds_info.Tables(0).Rows(i).Item("Nombre")
        fila("Importe") = ds_info.Tables(0).Rows(i).Item("Importe")
        Select Case tipo
          Case "RECLAMO"
            Dim sincalculo = ds_info.Tables(0).Rows(i).Item("Sincalculo")
            Select Case sincalculo
              Case 0
                fila("sincalculo") = "NO"
              Case -1
                fila("sincalculo") = "SI"
            End Select
            Dim prescred = ds_info.Tables(0).Rows(i).Item("Pres/Cred")
            Select Case prescred
              Case 0
                fila("Pres/Cred") = "PRESTAMO"
              Case -1
                fila("Pres/Cred") = "CREDITO"
            End Select
            fila("Descripcion") = ds_info.Tables(0).Rows(i).Item("Descripcion")
          Case Else
            'AQUI HAGO PAGOS Y COBROS
            fila("Sincalculo") = ""
            fila("Pres/Cred") = ""
            fila("Descripcion") = ""
        End Select
        fila("Fecha") = ds_info.Tables(0).Rows(i).Item("Fecha")
        DS_pagoscobrosreclamos.Tabla1.Rows.Add(fila)
        i = i + 1
      End While
    End If



    '------------------------------------------------------------------------------------
    'MODIF: 22-09-08---AGREGAR CALCULO DE TOTALES DE PAGOS, COBROS Y RECLAMOS

    If DS_pagoscobrosreclamos.Tabla1.Rows.Count <> 0 Then
      Dim TOTAL_PAGOS As Decimal = 0
      Dim TOTAL_COBROS As Decimal = 0
      Dim TOTAL_RECLAMOS As Decimal = 0

      Dim i As Integer = 0
      While i < DS_pagoscobrosreclamos.Tabla1.Rows.Count
        Dim tipo As String = DS_pagoscobrosreclamos.Tabla1.Rows(i).Item("Tipo").ToString
        Select Case tipo
          Case "RECLAMO"
            TOTAL_RECLAMOS = TOTAL_RECLAMOS + CDec(DS_pagoscobrosreclamos.Tabla1.Rows(i).Item("Importe"))
          Case "COBRO"
            TOTAL_COBROS = TOTAL_COBROS + CDec(DS_pagoscobrosreclamos.Tabla1.Rows(i).Item("Importe"))
          Case "PAGO"
            TOTAL_PAGOS = TOTAL_PAGOS + CDec(DS_pagoscobrosreclamos.Tabla1.Rows(i).Item("Importe"))
        End Select
        i = i + 1
      End While
      If TOTAL_RECLAMOS <> CDec(0) Then
        Dim fila_reclamo As DataRow = DS_pagoscobrosreclamos.Tabla1.NewRow
        fila_reclamo("Tipo") = "TOTAL RECLAMOS"
        fila_reclamo("Importe") = (Math.Round(TOTAL_RECLAMOS, 2).ToString("N2"))
        DS_pagoscobrosreclamos.Tabla1.Rows.Add(fila_reclamo)
      End If
      If TOTAL_COBROS <> CDec(0) Then
        Dim fila_cobros As DataRow = DS_pagoscobrosreclamos.Tabla1.NewRow
        fila_cobros("Tipo") = "TOTAL COBROS"
        fila_cobros("Importe") = (Math.Round(TOTAL_COBROS, 2).ToString("N2"))
        DS_pagoscobrosreclamos.Tabla1.Rows.Add(fila_cobros)
      End If
      If TOTAL_PAGOS <> CDec(0) Then
        Dim fila_pagos As DataRow = DS_pagoscobrosreclamos.Tabla1.NewRow
        fila_pagos("Tipo") = "TOTAL PAGOS"
        fila_pagos("Importe") = (Math.Round(TOTAL_PAGOS, 2).ToString("N2"))
        DS_pagoscobrosreclamos.Tabla1.Rows.Add(fila_pagos)
      End If
    End If
    '------------------------------------------------------------------------------------


    GridView1.DataSource = DS_pagoscobrosreclamos.Tabla1
    GridView1.DataBind()

    GridView1.Columns(0).Visible = False '0 es columna ID

    Try
      If CDate(Hf_FECHA.Value) <> CDate(txt_fecha.Text) Then
        GridView1.Columns(9).Visible = False '10 es columna eliminar
      End If
    Catch ex As Exception

    End Try
  End Sub

  Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_Pagos Cobros Reclamos/PagosCobrosReclamos.aspx")
  End Sub

  Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
    Try
      If (e.CommandName = "ID") Then
        ' Retrieve the row index stored in the CommandArgument property.
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
        'Session("usuario_id") = id
        'Response.Redirect("Mensaje_Datos_Personales.aspx")
        Session("ID") = id

        'aqui pregunto si estoy seguro de eliminar.
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_baja", "$(document).ready(function () {$('#Mdl_baja').modal();});", True)

      End If
    Catch ex As Exception

    End Try

  End Sub
  Private Sub btn_baja_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_baja_close.ServerClick
    'nada
  End Sub

  Private Sub btn_baja_mdl_cancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_baja_mdl_cancelar.ServerClick
    'nada
  End Sub

  Private Sub btn_baja_mdll_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_baja_mdll.ServerClick
    Try
      'aqui codigo para eliminar.
      DAanticipados.Anticipados_eliminar(Session("ID"))
      Session("ID") = ""
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKELIMINADO", "$(document).ready(function () {$('#modal-sm_OKELIMINADO').modal();});", True)

    Catch ex As Exception

    End Try

  End Sub

  Private Sub btn_ELIMINAR_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ELIMINAR_close.ServerClick
    Try
      'cargo la grilla nuevamente con la info actualizada.
      obtener_resumen(txt_fecha.Text)
      txt_fecha.Focus()
    Catch ex As Exception

    End Try
  End Sub

  Private Sub btn_ok_elimnar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_elimnar.ServerClick
    Try
      'cargo la grilla nuevamente con la info actualizada.
      obtener_resumen(txt_fecha.Text)
      txt_fecha.Focus()
    Catch ex As Exception

    End Try

  End Sub





  Private Sub btn_buscar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.ServerClick
    Try
      DS_pagoscobrosreclamos.Tabla1.Rows.Clear()
      GridView1.DataSource = ""
      GridView1.Columns(0).Visible = True '0 es columna ID
      GridView1.Columns(9).Visible = True '9 es columna eliminar

      Dim ds_info As DataSet = DAanticipados.Anticipados_resumen(txt_fecha.Text)
      If ds_info.Tables(0).Rows.Count <> 0 Then
        Dim i As Integer = 0
        While i < ds_info.Tables(0).Rows.Count
          Dim tipo As String = ds_info.Tables(0).Rows(i).Item("Tipo")
          Dim fila As DataRow = DS_pagoscobrosreclamos.Tabla1.NewRow
          fila("Anticipados_id") = ds_info.Tables(0).Rows(i).Item("Anticipados_id")
          fila("Tipo") = tipo
          fila("Codigo") = ds_info.Tables(0).Rows(i).Item("Codigo")
          fila("Nombre") = ds_info.Tables(0).Rows(i).Item("Nombre")
          fila("Importe") = ds_info.Tables(0).Rows(i).Item("Importe")
          Select Case tipo
            Case "RECLAMO"
              Dim sincalculo = ds_info.Tables(0).Rows(i).Item("Sincalculo")
              Select Case sincalculo
                Case 0
                  fila("sincalculo") = "NO"
                Case -1
                  fila("sincalculo") = "SI"
              End Select
              Dim prescred = ds_info.Tables(0).Rows(i).Item("Pres/Cred")
              Select Case prescred
                Case 0
                  fila("Pres/Cred") = "PRESTAMO"
                Case -1
                  fila("Pres/Cred") = "CREDITO"
              End Select
              fila("Descripcion") = ds_info.Tables(0).Rows(i).Item("Descripcion")
            Case Else
              'AQUI HAGO PAGOS Y COBROS
              fila("Sincalculo") = ""
              fila("Pres/Cred") = ""
              fila("Descripcion") = ""
          End Select
          fila("Fecha") = ds_info.Tables(0).Rows(i).Item("Fecha")
          DS_pagoscobrosreclamos.Tabla1.Rows.Add(fila)
          i = i + 1
        End While

        '------------------------------------------------------------------------------------
        'MODIF: 22-09-08---AGREGAR CALCULO DE TOTALES DE PAGOS, COBROS Y RECLAMOS

        If DS_pagoscobrosreclamos.Tabla1.Rows.Count <> 0 Then
          Dim TOTAL_PAGOS As Decimal = 0
          Dim TOTAL_COBROS As Decimal = 0
          Dim TOTAL_RECLAMOS As Decimal = 0

          i = 0
          While i < DS_pagoscobrosreclamos.Tabla1.Rows.Count
            Dim tipo As String = DS_pagoscobrosreclamos.Tabla1.Rows(i).Item("Tipo").ToString
            Select Case tipo
              Case "RECLAMO"
                TOTAL_RECLAMOS = TOTAL_RECLAMOS + CDec(DS_pagoscobrosreclamos.Tabla1.Rows(i).Item("Importe"))
              Case "COBRO"
                TOTAL_COBROS = TOTAL_COBROS + CDec(DS_pagoscobrosreclamos.Tabla1.Rows(i).Item("Importe"))
              Case "PAGO"
                TOTAL_PAGOS = TOTAL_PAGOS + CDec(DS_pagoscobrosreclamos.Tabla1.Rows(i).Item("Importe"))
            End Select
            i = i + 1
          End While
          If TOTAL_RECLAMOS <> CDec(0) Then
            Dim fila_reclamo As DataRow = DS_pagoscobrosreclamos.Tabla1.NewRow
            fila_reclamo("Tipo") = "TOTAL RECLAMOS"
            fila_reclamo("Importe") = (Math.Round(TOTAL_RECLAMOS, 2).ToString("N2"))
            DS_pagoscobrosreclamos.Tabla1.Rows.Add(fila_reclamo)
          End If
          If TOTAL_COBROS <> CDec(0) Then
            Dim fila_cobros As DataRow = DS_pagoscobrosreclamos.Tabla1.NewRow
            fila_cobros("Tipo") = "TOTAL COBROS"
            fila_cobros("Importe") = (Math.Round(TOTAL_COBROS, 2).ToString("N2"))
            DS_pagoscobrosreclamos.Tabla1.Rows.Add(fila_cobros)
          End If
          If TOTAL_PAGOS <> CDec(0) Then
            Dim fila_pagos As DataRow = DS_pagoscobrosreclamos.Tabla1.NewRow
            fila_pagos("Tipo") = "TOTAL PAGOS"
            fila_pagos("Importe") = (Math.Round(TOTAL_PAGOS, 2).ToString("N2"))
            DS_pagoscobrosreclamos.Tabla1.Rows.Add(fila_pagos)
          End If
        End If
        '------------------------------------------------------------------------------------





        GridView1.DataSource = DS_pagoscobrosreclamos.Tabla1
        GridView1.DataBind()
        GridView1.Columns(0).Visible = False '10 es columna eliminar
        Try
          If CDate(Hf_FECHA.Value) <> CDate(txt_fecha.Text) Then
            GridView1.Columns(9).Visible = False '10 es columna eliminar
          End If
        Catch ex As Exception

        End Try

        GridView1.Focus()
      Else
        GridView1.DataSource = DS_pagoscobrosreclamos.Tabla1
        GridView1.DataBind()
        GridView1.Columns(0).Visible = False '0 es columna ID
        GridView1.Columns(9).Visible = False '9 es columna eliminar

        'la busqueda no arrojo resultados.
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_error_busqueda", "$(document).ready(function () {$('#modal_error_busqueda').modal();});", True)
      End If
    Catch ex As Exception
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_error_busqueda", "$(document).ready(function () {$('#modal_error_busqueda').modal();});", True)
    End Try
  End Sub

#Region "modal_error_busqueda"
  Private Sub btn_ok_error_busqueda_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_busqueda.ServerClick
    txt_fecha.Focus()
  End Sub

  Private Sub btn_close_error_busqueda_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_busqueda.ServerClick
    txt_fecha.Focus()
  End Sub
#End Region


#Region "INIT"
  'AQUI agrego el atributo onfocus y asocio a la rutina js seleccionartexto para que cuando se ponga el foco en un textbox se seleccione todo el contenido
  Private Sub txt_fecha_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_fecha.Init
    txt_fecha.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub
#End Region

End Class
