Public Class Cob_usuariosAlta
  Inherits System.Web.UI.Page
  Dim DAusuario As New Capa_Datos.Usuarios
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos
      If Session("Usuarios_OP") = "MODIFICAR" Then
        HF_USU_ID.Value = Session("USU_ID")
        'Y RECUPERO TODA LA INFO DE LA BD
        RecuperarUsuario()
      Else
        'SI ES ALTA INGRESO 

      End If
    End If
  End Sub

  Private Sub RecuperarUsuario()

    Dim DsUsu As DataSet = DAusuario.Usuarios_buscarID(HF_USU_ID.Value)

    If DsUsu.Tables(0).Rows.Count <> 0 Then

      TxtUsuario.Text = DsUsu.Tables(0).Rows(0).Item("Usuario")
      TxtContrasena.Text = DsUsu.Tables(0).Rows(0).Item("Contraseña")
      DropDLJerarquia.SelectedValue = DsUsu.Tables(0).Rows(0).Item("Jerarquia")
      TxtApellido.Text = DsUsu.Tables(0).Rows(0).Item("USU_ape")
      TxtNombre.Text = DsUsu.Tables(0).Rows(0).Item("USU_nom")
      TxtDireccion.Text = DsUsu.Tables(0).Rows(0).Item("USU_direccion")
      TxtDni.Text = DsUsu.Tables(0).Rows(0).Item("USU_dni")
      TxtTelefono.Text = DsUsu.Tables(0).Rows(0).Item("USU_telefono")
      TxtMail.Text = DsUsu.Tables(0).Rows(0).Item("USU_mail")
      TxtObservacion.Text = DsUsu.Tables(0).Rows(0).Item("USU_obs")


    Else
      Mdl_label_errores.Text = "No se Encontro Informacion" 'dependiendo el error este msj va a cambiar
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_errores", "$(document).ready(function () {$('#Mdl_errores').modal();});", True)

    End If

  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/COB_Usuarios/Cob_usuarios.aspx")
  End Sub

#Region "GRABAR"
  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick

    If Session("Usuarios_OP") = "MODIFICAR" Then

      Try


        DAusuario.Usuario_Actualizar(TxtUsuario.Text, TxtContrasena.Text, DropDLJerarquia.SelectedValue, TxtApellido.Text, TxtNombre.Text, TxtDireccion.Text, TxtDni.Text, TxtTelefono.Text, TxtMail.Text, TxtObservacion.Text, HF_USU_ID.Value)
        'la sig linea muestra el modal con el mensaje "SE GUARDO CORRECTAMENTE"
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)

      Catch ex As Exception

      End Try

    Else



      Dim err As String = 0
      If TxtDni.Text = "" Then
        err = 1

      End If

      If TxtNombre.Text = "" Then
        err = 1
      End If

      If TxtApellido.Text = "" Then
        err = 1
      End If

      If TxtContrasena.Text = "" Then
        err = 1
      End If

      If TxtUsuario.Text = "" Then
        err = 1
      End If

      If err <> 1 Then
        Try
          DAusuario.Usuarios_alta(TxtUsuario.Text, TxtContrasena.Text, DropDLJerarquia.SelectedValue, TxtApellido.Text, TxtNombre.Text, TxtDireccion.Text, TxtDni.Text, TxtTelefono.Text, TxtMail.Text, TxtObservacion.Text, "Activo")
          'la sig linea muestra el modal con el mensaje "SE GUARDO CORRECTAMENTE"
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)

        Catch ex As Exception

        End Try
      Else
        'hacemos una validacion de dato ingresados y si falla mostramos este msj:
        Mdl_label_errores.Text = "Complete la info. Solicitada" 'dependiendo el error este msj va a cambiar
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_errores", "$(document).ready(function () {$('#Mdl_errores').modal();});", True)

      End If

      'la sig linea muestra el modal con el mensaje "SE GUARDO CORRECTAMENTE"
      'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)

      'AQUI DEJO COMENTADA SI QUIERES QUE EN VEZ DE GUARDAR DE UNA, MUESTRE UN MSJ DE CONFIRMACION.
      'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_altamodif", "$(document).ready(function () {$('#Mdl_altamodif').modal();});", True)
      '----si usas esta opcion, el codigo para hacer la alta/modif va en el click del boton: btn_altamodif_mdll
    End If

  End Sub
  Private Sub btn_altamodif_mdll_ServerClick(sender As Object, e As EventArgs) Handles btn_altamodif_mdll.ServerClick
    'aqui va todo el codigo para la alta o modificacion.


    'al final de la alta-modif se redirecciona al form anterior.
    Response.Redirect("~/COB_Usuarios/Cob_usuarios.aspx")
  End Sub


  Private Sub btn_graba_close_ServerClick(sender As Object, e As EventArgs) Handles btn_graba_close.ServerClick
    'VUELVE A FORM ANTERIOR
    Response.Redirect("~/COB_Usuarios/Cob_usuarios.aspx")
  End Sub

  Private Sub btn_ok_ServerClick(sender As Object, e As EventArgs) Handles btn_ok.ServerClick
    'VUELVE A FORM ANTERIOR
    Response.Redirect("~/COB_Usuarios/Cob_usuarios.aspx")
  End Sub


#End Region

#Region "DAR DE BAJA"

  Private Sub btn_baja_mdll_ServerClick(sender As Object, e As EventArgs) Handles btn_baja_mdll.ServerClick
    'aqui va todo el codigo para eliminar al usuario.


    'AQUI DEJO COMENTADA SI QUIERES QUE EN VEZ DE GUARDAR DE UNA, MUESTRE UN MSJ DE CONFIRMACION.
    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_altamodif", "$(document).ready(function () {$('#Mdl_altamodif').modal();});", True)
    '----si usas esta opcion, el codigo para hacer la alta/modif va en el click del boton: btn_altamodif_mdll

    DAusuario.Usuario_Eliminar("Inactivo", HF_USU_ID.Value)

    'al final de la eliminacion se redirecciona al form anterior:
    Response.Redirect("~/COB_Usuarios/Cob_usuarios.aspx")

  End Sub



#End Region



End Class
