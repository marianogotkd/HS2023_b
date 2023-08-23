Public Class abml_creditos


  '######## Choco Puto 25-05-2022 18:59 ###########
  Inherits System.Web.UI.Page
  Dim Daparametro As New Capa_Datos.WC_parametro
  Dim DAprestamoscreditos As New Capa_Datos.WC_prestamoscreditos
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      Txt_cliente_codigo.Focus()
      'recuperar fecha de tabla parametro.
      Dim ds_fecha As DataSet = Daparametro.Parametro_obtener_dia
      If ds_fecha.Tables(0).Rows.Count <> 0 Then
        Dim FECHA As Date = CDate(ds_fecha.Tables(0).Rows(0).Item("Fecha"))
        txt_fecha.Text = FECHA.ToString("yyyy-MM-dd")
      End If
    End If
  End Sub

  Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABML Prestamos_Creditos/abml_prestamoscreditos.aspx")
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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 2 or null

          Dim ds_permisos As DataSet = DApermisos.Permisos_buscar(Idusuario)
          Dim i As Integer = 0
          Dim valido As String = "no"
          While i < ds_permisos.Tables(0).Rows.Count
            Dim Menu As String = ""
            Try
              Menu = ds_permisos.Tables(0).Rows(i).Item("Menu").ToString.ToUpper
            Catch ex As Exception
            End Try
            Dim Opcion As String = ""
            Try
              Opcion = ds_permisos.Tables(0).Rows(i).Item("Opcion")
            Catch ex As Exception
            End Try
            If (Menu = "C" And Opcion = "") Or (Menu = "C" And Opcion = "2") Then
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

  Private Sub btn_buscar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.ServerClick
    lb_cliente_nomb.InnerText = "Nombre:"
    Txt_importe.Text = ""
    Txt_diasacobrar.Text = ""
    Txt_porcentaje.Text = ""

    Try
      Dim Fecha As Date = CDate(txt_fecha.Text)

      If Txt_cliente_codigo.Text <> "" Then
        Dim ds_info As DataSet = DAprestamoscreditos.Creditos_buscar_cliente_info(Txt_cliente_codigo.Text, txt_fecha.Text)
        If ds_info.Tables(2).Rows.Count <> 0 Then
          'cargo la info del credito que se recupero para esa fecha.
          lb_cliente_nomb.InnerText = "Nombre: " + ds_info.Tables(2).Rows(0).Item("Nombre")
          Txt_importe.Text = CDec(ds_info.Tables(2).Rows(0).Item("Importe"))
          Txt_porcentaje.Text = CDec(ds_info.Tables(2).Rows(0).Item("Porcentaje"))
          Txt_diasacobrar.Text = ds_info.Tables(2).Rows(0).Item("Dias")
          Txt_importe.Focus()
        Else
          If ds_info.Tables(0).Rows.Count <> 0 Then
            lb_cliente_nomb.InnerText = "Nombre: " + ds_info.Tables(0).Rows(0).Item("Nombre")
            Txt_importe.Focus()
          Else
            'no existe, emitir un mensaje.
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_noexiste", "$(document).ready(function () {$('#modal-sm_error_noexiste').modal();});", True)
          End If


        End If
      Else
        'no existe, emitir un mensaje.
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_noexiste", "$(document).ready(function () {$('#modal-sm_error_noexiste').modal();});", True)
      End If


    Catch ex As Exception
      'mensaje ingrese fecha para buscar.
      'no existe, emitir un mensaje.
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_fecha", "$(document).ready(function () {$('#modal-sm_error_fecha').modal();});", True)
    End Try


  End Sub

#Region "INIT"
  'AQUI agrego el atributo onfocus y asocio a la rutina js seleccionartexto para que cuando se ponga el foco en un textbox se seleccione todo el contenido
  Private Sub Txt_cliente_codigo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_cliente_codigo.Init
    Txt_cliente_codigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub txt_fecha_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_fecha.Init
    txt_fecha.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_importe_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_importe.Init
    Txt_importe.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_porcentaje_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_porcentaje.Init
    Txt_porcentaje.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_diasacobrar_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_diasacobrar.Init
    Txt_diasacobrar.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

#End Region

#Region "modal-sm_error_noexiste"
  Private Sub btn_close_error_noexiste_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_noexiste.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_ok_error_noexiste_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_noexiste.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub
#End Region

#Region "modal-sm_error_fecha"
  Private Sub btn_close_error_fecha_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_fecha.ServerClick
    txt_fecha.Focus()
  End Sub

  Private Sub btn_ok_error_fecha_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_fecha.ServerClick
    txt_fecha.Focus()
  End Sub
#End Region

  Private Sub limpiar_label_error()
    lb_error_codigo.Visible = False
    lb_error_fecha.Visible = False
    lb_error_importe.Visible = False
    lb_error_porcentaje.Visible = False
    lb_error_dias.Visible = False
  End Sub

  Private Sub BOTON_GRABA_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BOTON_GRABA.ServerClick
    limpiar_label_error()
    Dim valido_ingreso As String = "si"

    If Txt_cliente_codigo.Text = "" Then
      valido_ingreso = "no"
      lb_error_codigo.Visible = True
    End If

    If txt_fecha.Text = "" Then
      valido_ingreso = "no"
      lb_error_fecha.Visible = True
    Else
      Try
        Dim fecha As Date = CDate(txt_fecha.Text)
      Catch ex As Exception
        valido_ingreso = "no"
        lb_error_fecha.Visible = True
      End Try
    End If

    Dim importe As Decimal
    Try
      importe = CDec(Txt_importe.Text.Replace(".", ","))
    Catch ex As Exception
      valido_ingreso = "no"
      lb_error_importe.Visible = True
    End Try

    Dim porcentaje As Decimal
    Try
      porcentaje = CDec(Txt_porcentaje.Text.Replace(".", ","))
    Catch ex As Exception
      porcentaje = CDec(0)
      lb_error_porcentaje.Visible = True
      valido_ingreso = "no"
    End Try

    Dim dias As Integer = 0
    If Txt_diasacobrar.Text = "" Or Txt_diasacobrar.Text = "0" Then
      valido_ingreso = "no"
      lb_error_dias.Visible = True
    End If
    Try
      dias = CInt(Txt_diasacobrar.Text)
    Catch ex As Exception
      valido_ingreso = "no"
      lb_error_dias.Visible = True
    End Try

    If valido_ingreso = "si" Then
      Try
        Dim ds_info As DataSet = DAprestamoscreditos.Creditos_buscar_cliente_info(Txt_cliente_codigo.Text, txt_fecha.Text)
        If ds_info.Tables(0).Rows.Count <> 0 Then 'verifico que existe el cliente
          Session("Cliente") = ds_info.Tables(0).Rows(0).Item("Cliente")
          If ds_info.Tables(2).Rows.Count <> 0 Then
            'entonces pregunto si quiero modificar el prestamo actual.
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_graba_modif", "$(document).ready(function () {$('#Mdl_graba_modif').modal();});", True)
          Else
            'entonces es un alta para ello primero valido si tengo margen para pedir creditos

            Dim cant_pc As Integer = CInt(ds_info.Tables(0).Rows(0).Item("Cantidadpc"))

            If ds_info.Tables(1).Rows.Count < cant_pc Then
              'entonces pregunto si deseo dar de alta el prestamo.
              ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_graba_alta", "$(document).ready(function () {$('#Mdl_graba_alta').modal();});", True)
            Else
              'mensaje: Cantidad máxima. No puede solicitar un prestamo.

              ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_limite", "$(document).ready(function () {$('#modal-sm_error_limite').modal();});", True)
            End If

          End If
        End If


      Catch ex As Exception

      End Try
    Else
      'mensaje ingrese la informacion solicitada.
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_ingreso", "$(document).ready(function () {$('#modal-sm_error_ingreso').modal();});", True)
    End If


  End Sub


#Region "Mdl_graba_alta"
  Private Sub btn_graba_alta_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_alta_close.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_graba_alta_cancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_alta_cancelar.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_graba_alta_confirmar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_alta_confirmar.ServerClick
    'aqui codigo de alta.
    Dim importe As Decimal
    Try
      importe = CDec(Txt_importe.Text.Replace(".", ","))

    Catch ex As Exception
      importe = CDec(0)
    End Try
    Dim porcentaje As Decimal

    Try
      porcentaje = CDec(Txt_porcentaje.Text.Replace(".", ","))

    Catch ex As Exception
      porcentaje = CDec(0)
    End Try

    Dim Interes As Decimal = porcentaje / 100

    Dim MontoInteres As Decimal = importe * Interes

    Dim Saldo As Decimal = CDec(importe) + MontoInteres
    '/////Se actualiza??

    Dim Cuota_valor As Decimal = (importe + MontoInteres) / CInt(Txt_diasacobrar.Text)

    DAprestamoscreditos.Creditos_alta(CInt(Session("Cliente")), txt_fecha.Text, importe, porcentaje, Txt_diasacobrar.Text, Saldo, Cuota_valor)

    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
  End Sub
#End Region

#Region "modal-sm_OKGRABADO"
  Private Sub btn_graba_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABML Prestamos_Creditos/abml_prestamoscreditos.aspx")
  End Sub

  Private Sub btn_ok_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABML Prestamos_Creditos/abml_prestamoscreditos.aspx")
  End Sub
#End Region

#Region "modal-sm_error_ingreso"
  Private Sub btn_close_error_ingreso_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_ingreso.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_ok_error_ingreso_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_ingreso.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub
#End Region


#Region "Mdl_graba_modif"
  Private Sub btn_graba_modif_cancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_modif_cancelar.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_graba_modif_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_modif_close.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_graba_modif_confirmar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_modif_confirmar.ServerClick
    'aqui codigo de alta.
    Dim importe As Decimal
    Try
      importe = CDec(Txt_importe.Text.Replace(".", ","))
    Catch ex As Exception
      importe = CDec(0)
    End Try

    Dim porcentaje As Decimal
    Try
      porcentaje = CDec(Txt_porcentaje.Text.Replace(".", ","))
    Catch ex As Exception
      porcentaje = CDec(0)
    End Try

    Dim Interes As Decimal = porcentaje / 100

    Dim MontoInteres As Decimal = importe * Interes

    Dim Saldo As Decimal = CDec(importe) + MontoInteres
    '/////Se actualiza??

    Dim Cuota_valor As Decimal = (importe + MontoInteres) / CInt(Txt_diasacobrar.Text)

    'Dim Saldo As Decimal = CDec(importe) * CDec(porcentaje)
    'Dim Cuota_valor As Decimal = (importe * porcentaje) / CInt(Txt_diasacobrar.Text)
    DAprestamoscreditos.Creditos_modificar(CInt(Session("Cliente")), txt_fecha.Text, importe, porcentaje, Txt_diasacobrar.Text, Saldo, 1, Cuota_valor)
    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
  End Sub
#End Region

#Region "modal-sm_error_limite"
  Private Sub btn_close_error_limite_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_limite.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_ok_error_limite_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_limite.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub
#End Region



End Class
