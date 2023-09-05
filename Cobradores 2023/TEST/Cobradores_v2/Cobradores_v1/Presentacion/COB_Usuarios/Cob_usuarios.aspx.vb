Public Class Cob_usuarios
  Inherits System.Web.UI.Page
  Dim DAusuario As New Capa_Datos.Usuarios



  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos

      Obtener_usuarios()


    End If
  End Sub

  Dim DS_Cob_usuarios As New DS_Cob_usuarios
  Private Sub Obtener_usuarios()
    DS_Cob_usuarios.Usuarios.Rows.Clear()

    GridView1.Columns(0).Visible = True 'columna ID

    Dim ds_info As DataSet = DAusuario.Usuarios_obtenertodos

    If ds_info.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_info.Tables(0).Rows.Count
        If ds_info.Tables(0).Rows(i).Item("USU_jerarquia") <> "1" Then 'el 1 es admin gral, no lo muestro.
          Dim fila As DataRow = DS_Cob_usuarios.Usuarios.NewRow
          fila("USU_ID") = ds_info.Tables(0).Rows(i).Item("USU_ID")
          fila("USU_usuario") = ds_info.Tables(0).Rows(i).Item("USU_usuario")
          fila("USU_dni") = ds_info.Tables(0).Rows(i).Item("USU_dni")
          fila("ApeNomb") = ds_info.Tables(0).Rows(i).Item("USU_ape") + ", " + ds_info.Tables(0).Rows(i).Item("USU_nom")
          Try
            If ds_info.Tables(0).Rows(i).Item("USU_jerarquia") = "2" Then
              fila("Jerarquia") = "Administrador"
            End If
            If ds_info.Tables(0).Rows(i).Item("USU_jerarquia") = "3" Then
              fila("Jerarquia") = "Cobrador"
            End If
          Catch ex As Exception
          End Try
          DS_Cob_usuarios.Usuarios.Rows.Add(fila)
        End If
        i = i + 1
      End While
    End If

    GridView1.DataSource = DS_Cob_usuarios.Usuarios
    GridView1.DataBind()

    GridView1.Columns(0).Visible = False '0 es columna ID


  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_nuevo_ServerClick(sender As Object, e As EventArgs) Handles btn_nuevo.ServerClick
    Session("Usuarios_OP") = "ALTA"
    Response.Redirect("~/COB_Usuarios/Cob_usuariosAlta.aspx")
  End Sub

  Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
    Try
      If (e.CommandName = "ID") Then
        ' Retrieve the row index stored in the CommandArgument property.
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
        'Session("usuario_id") = id
        'Response.Redirect("Mensaje_Datos_Personales.aspx")

        Session("USU_ID") = id 'CON ESTA VARIABLE VOY A PODER RECUPERAR LA INFO EN EL FORM Cob_usuariosAlta.aspx
        Session("Usuarios_OP") = "MODIFICAR"
        Response.Redirect("~/COB_Usuarios/Cob_usuariosAlta.aspx")


      End If
    Catch ex As Exception

    End Try
  End Sub
End Class
