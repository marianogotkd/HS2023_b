Public Class acl_gastos_carga
    Inherits System.Web.UI.Page
    Dim DAgastos As New Capa_Datos.WC_gastos
    Dim Daparametro As New Capa_Datos.WC_parametro
    Dim DAgrupos As New Capa_Datos.WC_grupos
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos

#Region "INIT"
  Private Sub Txt_fecha_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_fecha.Init
        Txt_fecha.Attributes.Add("onfocus", "seleccionarTexto(this);")
    End Sub

    Private Sub Txt_grupo_codigo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_grupo_codigo.Init
        Txt_grupo_codigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
    End Sub

    Private Sub Txt_importe_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_importe.Init
        Txt_importe.Attributes.Add("onfocus", "seleccionarTexto(this);")
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      motivos_obtener()
      'la fecha la recupero de la tabla parametro.
      Dim ds_fecha As DataSet = Daparametro.Parametro_obtener_dia
      If ds_fecha.Tables(0).Rows.Count <> 0 Then
        Dim FECHA As Date = CDate(ds_fecha.Tables(0).Rows(0).Item("Fecha"))
        Txt_fecha.Text = FECHA.ToString("yyyy-MM-dd")
      End If
      Txt_fecha.Focus()
    End If
  End Sub

  Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ACL Gastos/acl_gastos.aspx")
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
            If (Menu = "D" And Opcion = "") Or (Menu = "D" And Opcion = "2") Then
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

  Private Sub motivos_obtener()
    Dim ds_motivos As DataSet = DAgastos.GastosTipo_obtener_todos
    If ds_motivos.Tables(0).Rows.Count <> 0 Then
      DropDownList_motivo.DataSource = ds_motivos.Tables(0)
      DropDownList_motivo.DataTextField = "Motivo"
      DropDownList_motivo.DataValueField = "Gastotipo_id"
      DropDownList_motivo.DataBind()
    End If


  End Sub

#Region "Mdl_graba"
  Private Sub btn_graba_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_close.ServerClick
        Txt_fecha.Focus()
    End Sub

    Private Sub btn_graba_cancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_cancelar.ServerClick
        Txt_fecha.Focus()
    End Sub

    Private Sub btn_graba_confirmar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_confirmar.ServerClick
        lb_error_fecha.Visible = False
        lb_error_grupocodigo.Visible = False
        lb_error_importe.Visible = False

        'aqui codigo para guardar.
        Dim valido_ingreso As String = "si"

        If Txt_fecha.Text = "" Then
            valido_ingreso = "no"
            lb_error_fecha.Visible = True
        Else
            Try
                Dim fecha As Date = CDate(Txt_fecha.Text)
            Catch ex As Exception
                valido_ingreso = "no"
                lb_error_fecha.Visible = True
            End Try
        End If

        Dim importe As Decimal
        Try
            importe = CDec(Txt_importe.Text.Replace(".", ","))
            If importe = 0 Then
                valido_ingreso = "no"
                lb_error_importe.Visible = True
            End If
        Catch ex As Exception
            valido_ingreso = "no"
            lb_error_importe.Visible = True
        End Try

        If Txt_grupo_codigo.Text = "" Then
            valido_ingreso = "no"
            lb_error_grupocodigo.Visible = True
        End If

        If valido_ingreso = "si" Then
            Dim ds_grupo As DataSet = DAgrupos.Grupos_buscar_codigo(Txt_grupo_codigo.Text)
            If ds_grupo.Tables(0).Rows.Count <> 0 Then
                Dim Grupo_id As Integer = ds_grupo.Tables(0).Rows(0).Item("Grupo_id")
                DAgastos.Gastos_alta(Txt_fecha.Text, Grupo_id, DropDownList_motivo.SelectedValue, importe)
                'mensaje que confirma el guardado y vuelve al menu anterior.
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
            Else
                'el grupo ingresado no existe.
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_grupo", "$(document).ready(function () {$('#modal-sm_error_grupo').modal();});", True)
            End If
        Else
            'mensaje ingrese la informacion solicitada.
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_ingreso", "$(document).ready(function () {$('#modal-sm_error_ingreso').modal();});", True)
        End If



    End Sub
#End Region

#Region "modal-sm_error_ingreso"
    Private Sub btn_close_error_ingreso_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_ingreso.ServerClick
        Txt_fecha.Focus()
    End Sub

    Private Sub btn_ok_error_ingreso_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_ingreso.ServerClick
        Txt_fecha.Focus()
    End Sub
#End Region

#Region "modal-sm_error_grupo"
    Private Sub btn_close_error_grupo_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_grupo.ServerClick
        Txt_grupo_codigo.Focus()
    End Sub

    Private Sub btn_ok_error_grupo_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_grupo.ServerClick
        Txt_grupo_codigo.Focus()
    End Sub
#End Region

#Region "modal-sm_OKGRABADO"
    Private Sub btn_ok_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ACL Gastos/acl_gastos.aspx")
  End Sub

    Private Sub btn_ok_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ACL Gastos/acl_gastos.aspx")
  End Sub
#End Region

    Private Sub BOTON_GRABAR_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BOTON_GRABAR.ServerClick
        'Mdl_graba
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_graba", "$(document).ready(function () {$('#Mdl_graba').modal();});", True)
    End Sub


End Class
