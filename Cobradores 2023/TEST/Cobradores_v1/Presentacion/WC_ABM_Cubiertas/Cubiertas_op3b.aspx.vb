Public Class Cubiertas_op3b
  Inherits System.Web.UI.Page
  Dim daCubiertas As New Capa_Datos.Cubiertas
  Dim daClientes As New Capa_Datos.WB_clientes
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()

      HF_grupo_id.Value = CInt(Session("Grupo_id"))
      HF_IdCubGrupo.Value = 0
      Txt_grupo_codigo.Text = Session("Codigo")
      Txt_grupo_nombre.Text = Session("Nombre")

      'recupero info de la cubierta si existe en GruposCub, donde tipo = 1
      Dim ds_info As DataSet = daCubiertas.GruposCub_BuscarOp3(CStr(Session("Codigo")))
      If ds_info.Tables(0).Rows.Count <> 0 Then
        Txt_exc1.Text = CDec(ds_info.Tables(0).Rows(0).Item("UnaCifra"))
        Txt_exc2.Text = CDec(ds_info.Tables(0).Rows(0).Item("DosCifras"))
        Txt_exc3.Text = CDec(ds_info.Tables(0).Rows(0).Item("TresCifras"))
        Txt_exc4.Text = CDec(ds_info.Tables(0).Rows(0).Item("CuatroCifras"))
        DropDownList_considerar.SelectedValue = ds_info.Tables(0).Rows(0).Item("Consideracion")
        Txt_cliente_codigo.Text = ds_info.Tables(1).Rows(0).Item("Clie_Codigo")
        HF_grupo_id.Value = ds_info.Tables(0).Rows(0).Item("Grupo_id")

        HF_IdCubGrupo.Value = ds_info.Tables(0).Rows(0).Item("IdCubGrupo")
        Txt_cliente_codigo.ReadOnly = True
      Else
        Txt_exc1.Text = "0,00"
        Txt_exc2.Text = "0,00"
        Txt_exc3.Text = "0,00"
        Txt_exc4.Text = "0,00"
      End If
      Txt_grupo_codigo.ReadOnly = True
      Txt_grupo_nombre.ReadOnly = True
      Txt_exc1.Focus()


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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 3 or null

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
            If (Menu = "J" And Opcion = "") Or (Menu = "J" And Opcion = "3") Then
              valido = "si"
              Exit While
            End If
            i = i + 1
          End While
          If valido = "si" Then
            'se accede sin problemas
          Else
            'no tiene permiso, se redirige a menu.
            Response.Redirect("~/WC_ABM_Cubiertas/Cubiertas_op.aspx")
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

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABM_Cubiertas/Cubiertas_op3a.aspx")
  End Sub
  Private Sub Txt_grupo_codigo_Init(sender As Object, e As EventArgs) Handles Txt_grupo_codigo.Init
    Txt_grupo_codigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_grupo_nombre_Init(sender As Object, e As EventArgs) Handles Txt_grupo_nombre.Init
    Txt_grupo_nombre.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_exc1_Init(sender As Object, e As EventArgs) Handles Txt_exc1.Init
    Txt_exc1.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_exc2_Init(sender As Object, e As EventArgs) Handles Txt_exc2.Init
    Txt_exc2.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_exc3_Init(sender As Object, e As EventArgs) Handles Txt_exc3.Init
    Txt_exc3.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_exc4_Init(sender As Object, e As EventArgs) Handles Txt_exc4.Init
    Txt_exc4.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_cliente_codigo_Init(sender As Object, e As EventArgs) Handles Txt_cliente_codigo.Init
    Txt_cliente_codigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick
    Dim valido_ingreso As String = "si"
    Dim exc1 As Decimal
    Try
      exc1 = CDec(Txt_exc1.Text.Replace(".", ","))
    Catch ex As Exception
      exc1 = CDec(0)
    End Try

    Dim exc2 As Decimal
    Try
      exc2 = CDec(Txt_exc2.Text.Replace(".", ","))
    Catch ex As Exception
      exc2 = CDec(0)
    End Try

    Dim exc3 As Decimal
    Try
      exc3 = CDec(Txt_exc3.Text.Replace(".", ","))
    Catch ex As Exception
      exc3 = CDec(0)
    End Try

    Dim exc4 As Decimal
    Try
      exc4 = CDec(Txt_exc4.Text.Replace(".", ","))
    Catch ex As Exception
      exc4 = CDec(0)
    End Try
    Try
      Txt_cliente_codigo.Text = CInt(Txt_cliente_codigo.Text)
    Catch ex As Exception
      valido_ingreso = "no"
    End Try
    If valido_ingreso = "si" Then
      If HF_IdCubGrupo.Value = 0 Then
        'ALTA EN TABLA GRUPOSCUB
        '1)VALIDAMOS SI EL CLIENTE PERTENECE AL GRUPO
        Dim ds_clie As DataSet = daClientes.Clientes_buscar_codigo(CInt(Txt_cliente_codigo.Text))
        If ds_clie.Tables(0).Rows.Count <> 0 Then
          If HF_grupo_id.Value = ds_clie.Tables(0).Rows(0).Item("Grupo_id") Then
            'AQUI ALTA
            Dim Cliente_id As Integer = ds_clie.Tables(0).Rows(0).Item("Cliente")
            daCubiertas.GruposCub_alta_op3(Txt_grupo_codigo.Text, exc1, exc2, exc3, exc4, Cliente_id, DropDownList_considerar.SelectedValue)
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
          Else
            'msj error: el cliente no pertenece al grupo.
            Lb_error_valid.Text = "El cliente no pertenece al grupo."
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerror_validacion", "$(document).ready(function () {$('#modal_sn_okerror_validacion').modal();});", True)
          End If
        Else
          'msj error: ingrese cod cliente valido
          Lb_error_valid.Text = "Ingrese Cod. Cliente valido."
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerror_validacion", "$(document).ready(function () {$('#modal_sn_okerror_validacion').modal();});", True)
        End If
      Else
        'MODIFICO REGISTRO EN TABLA GRUPOSCUB
        daCubiertas.GruposCub_modificar_op3(HF_IdCubGrupo.Value, exc1, exc2, exc3, exc4, DropDownList_considerar.SelectedValue)
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
      End If
    Else
      If Txt_cliente_codigo.Text = "" Then
        'msj error: ingrese cod cliente valido
        Lb_error_valid.Text = "Ingrese Cod. Cliente valido."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerror_validacion", "$(document).ready(function () {$('#modal_sn_okerror_validacion').modal();});", True)
      End If
    End If
  End Sub

  Private Sub btn_graba_close_ServerClick(sender As Object, e As EventArgs) Handles btn_graba_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABM_Cubiertas/Cubiertas_op3a.aspx")
  End Sub

  Private Sub btn_ok_ServerClick(sender As Object, e As EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABM_Cubiertas/Cubiertas_op3a.aspx")
  End Sub

  Private Sub btn_ok_errorvalidacion_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_errorvalidacion.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_error_validacion_close_ServerClick(sender As Object, e As EventArgs) Handles btn_error_validacion_close.ServerClick
    Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_baja_mdll_ServerClick(sender As Object, e As EventArgs) Handles btn_baja_mdll.ServerClick
    Try
      If HF_IdCubGrupo.Value <> "0" Then
        daCubiertas.GruposCub_eliminar_op3(CInt(HF_IdCubGrupo.Value))
        Session("op_ingreso") = "si"
        Response.Redirect("~/WC_ABM_Cubiertas/Cubiertas_op3a.aspx")
      Else
        'mensaje error.
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerror_eliminar", "$(document).ready(function () {$('#modal_sn_okerror_eliminar').modal();});", True)
      End If
    Catch ex As Exception
    End Try
  End Sub

  Private Sub btn_ok_erroreliminar_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_erroreliminar.ServerClick
    Txt_exc1.Focus()
  End Sub

  Private Sub btn_error_eliminar_close_ServerClick(sender As Object, e As EventArgs) Handles btn_error_eliminar_close.ServerClick
    Txt_exc1.Focus()
  End Sub

End Class
