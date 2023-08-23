Public Class Cobro_prestamos_manuales_det
  Inherits System.Web.UI.Page
  Dim daprestamoscreditos As New Capa_Datos.WC_prestamoscreditos
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      Dim ds_prestamo As DataSet = daprestamoscreditos.Prestamos_buscar_x_id(CInt(Session("Idprestamocredito")))

      If ds_prestamo.Tables(0).Rows.Count <> 0 Then
        HF_Idpredtamocredito.Value = CInt(Session("Idprestamocredito"))
        Label_cliente_codigo.Text = ds_prestamo.Tables(0).Rows(0).Item("Codigo") + ", " + ds_prestamo.Tables(0).Rows(0).Item("Nombre")
        Label_fecha_prestamo.Text = ds_prestamo.Tables(0).Rows(0).Item("Fecha")
        Label_saldoprestamo.Text = ds_prestamo.Tables(0).Rows(0).Item("Saldo")
      End If

      Txt_importe_cobrar.Focus()

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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 2 or null

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
            If (Menu = "6" And Opcion = "") Or (Menu = "6" And Opcion = "1") Then
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

  Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_Cobro Prestamos Manuales/Cobro_prestamos_manuales.aspx")
  End Sub

  Private Sub BOTON_GRABA_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BOTON_GRABA.ServerClick
    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_graba", "$(document).ready(function () {$('#Mdl_graba').modal();});", True)
  End Sub

#Region "Mdl_graba"


#End Region


  Private Sub btn_graba_cancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_cancelar.ServerClick
    Txt_importe_cobrar.Focus()
  End Sub

  Private Sub btn_graba_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_close.ServerClick
    Txt_importe_cobrar.Focus()
  End Sub

  Private Sub btn_graba_confirmar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_confirmar.ServerClick
    Try
      'valido importe ingresado
      Dim valido_ingreso As String = "si"
      Dim ds_prestamo As DataSet = daprestamoscreditos.Prestamos_buscar_x_id(CInt(HF_Idpredtamocredito.Value))

      If ds_prestamo.Tables(0).Rows.Count <> 0 Then
        Dim saldo_prestamo As Decimal = CDec(ds_prestamo.Tables(0).Rows(0).Item("Saldo"))

        Dim importe_cobrar As Decimal = 0
        Try
          importe_cobrar = CDec(Txt_importe_cobrar.Text.Replace(".", ","))
        Catch ex As Exception
          valido_ingreso = "no"
        End Try

        If (importe_cobrar > saldo_prestamo) Or (importe_cobrar = CDec(0)) Then
          valido_ingreso = "no"
        End If

        If valido_ingreso = "si" Then
          'obtener fecha del dia de la tabla parametro
          Dim ds_fecha As DataSet = DAparametro.Parametro_obtener_dia
          Dim FECHA As Date = Today
          If ds_fecha.Tables(0).Rows.Count <> 0 Then
            FECHA = CDate(ds_fecha.Tables(0).Rows(0).Item("Fecha"))
          End If


          daprestamoscreditos.CobroPrestamosCreditos_alta(CInt(HF_Idpredtamocredito.Value), FECHA, importe_cobrar)
          'aqui modal con mensaje de operacion correcta.
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)

        Else
          'aqui va mensaje: "error, ingre un importe válido", colocando el foco en txt_importe_cobrar
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_ingreso", "$(document).ready(function () {$('#modal-sm_error_ingreso').modal();});", True)
        End If
      End If

    Catch ex As Exception

    End Try
  End Sub

#Region "modal-sm_OKGRABADO"
  Private Sub btn_ok_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_Cobro Prestamos Manuales/Cobro_prestamos_manuales.aspx")
  End Sub

  Private Sub btn_ok_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_Cobro Prestamos Manuales/Cobro_prestamos_manuales.aspx")
  End Sub
#End Region

#Region "modal_sm_error_ingreso"
  Private Sub btn_erroringreso_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_erroringreso_close.ServerClick
    Txt_importe_cobrar.Focus()
  End Sub

  Private Sub btn_erroringreso_ok_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_erroringreso_ok.ServerClick
    Txt_importe_cobrar.Focus()
  End Sub
#End Region


#Region "INIT"
  'se agrega un atributo que sirve para que se seleccione todo el contenido del textbox cuando recibe el foco.
  Private Sub Txt_importe_cobrar_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_importe_cobrar.Init
    Txt_importe_cobrar.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub
#End Region

End Class
