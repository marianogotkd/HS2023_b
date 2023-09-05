Public Class Cob_Local
  Inherits System.Web.UI.Page
  Dim daLocal As New Capa_Datos.Local

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos

      'Obtener_usuarios()
      Recuperar_Locales()

    End If
  End Sub

  Private Sub Recuperar_Locales()
    Dim DS_Cob_local As New DS_Cob_local

    DS_Cob_local.Tables("Local").Rows.Clear()

    GridView1.Columns(0).Visible = True 'columna ID

    Dim ds_local As DataSet = daLocal.consultarsoloactivos

    DS_Cob_local.Tables("Local").Merge(ds_local.Tables(0))

    GridView1.DataSource = DS_Cob_local.Tables("Local")
    GridView1.DataBind()
    GridView1.Columns(0).Visible = False '0 es columna ID

  End Sub


  Private Sub btn_nuevo_ServerClick(sender As Object, e As EventArgs) Handles btn_nuevo.ServerClick
    Session("Local_OP") = "ALTA"
    Response.Redirect("~/COB_Local/Cob_LocalAlta.aspx")
  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Inicio.aspx")



  End Sub

  Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
    If (e.CommandName = "ID") Then
      ' Retrieve the row index stored in the CommandArgument property.
      Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
      'Session("usuario_id") = id
      'Response.Redirect("Mensaje_Datos_Personales.aspx")


      Session("Local_OP") = "MODIFICAR"
      Session("LOCAL_ID") = id
      Response.Redirect("~/COB_Local/Cob_LocalAlta.aspx")

      'If id = "0" Then
      '  'lo elimino.
      '  GridView1.DeleteRow(index)

      'End If

      'Session("USU_ID") = id 'CON ESTA VARIABLE VOY A PODER RECUPERAR LA INFO EN EL FORM Cob_usuariosAlta.aspx
      'Session("Usuarios_OP") = "MODIFICAR"
      'Response.Redirect("~/COB_Usuarios/Cob_usuariosAlta.aspx")


    End If


  End Sub
End Class
