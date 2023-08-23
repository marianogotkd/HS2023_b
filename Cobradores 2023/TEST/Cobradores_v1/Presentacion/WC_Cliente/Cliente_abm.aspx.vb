Public Class Cliente_abm
  Inherits System.Web.UI.Page
  Dim DAClientes As New Capa_Datos.WB_clientes
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos

  Dim DS_cliente As New DS_cliente
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      'Dim ds_clie As DataSet = DAClientes.Clientes_obtenertodos
      'DS_cliente.Clientes.Rows.Clear()
      'GridView1.DataSource = ""
      'GridView1.DataBind()
      'If ds_clie.Tables(0).Rows.Count <> 0 Then
      'DS_cliente.Clientes.Merge(ds_clie.Tables(0))
      'GridView1.DataSource = DS_cliente.Clientes
      'GridView1.DataBind()
      'End If
      Txt_cliente_id.Focus()
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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 1 or null

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
            If (Menu = "B" And Opcion = "") Or (Menu = "B" And Opcion = "1") Then
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

  Private Sub recuperar_clientes()
    Dim ds_clie As DataSet = DAClientes.Clientes_obtenertodos
    DS_cliente.Clientes.Rows.Clear()

    GridView1.DataSource = ""
    'GridView1.DataBind()
    If ds_clie.Tables(0).Rows.Count <> 0 Then
      DS_cliente.Clientes.Merge(ds_clie.Tables(0))
      GridView1.DataSource = DS_cliente.Clientes
      GridView1.DataBind()
    End If
  End Sub


  'Private Sub btn_nuevo_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.ServerClick
  '    Session("clientes_op") = "alta"
  '    Response.Redirect("Cliente_alta_a.aspx")
  'End Sub

  Private Sub btn_modificar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_modificar.ServerClick
    If Txt_cliente_id.Text <> "" Then
      'vamos a convertir el codigo ingresado en entero, de esta forma si hay ceros mas a la izquierda los deberia eliminar.
      Try
        Txt_cliente_id.Text = CInt(Txt_cliente_id.Text)
      Catch ex As Exception

      End Try


      Dim ds_clie As DataSet = DAClientes.Clientes_buscar_codigo(Txt_cliente_id.Text)

      If ds_clie.Tables(0).Rows.Count <> 0 Then
        Session("clientes_op") = "modificar"
        'pasar ademas el ID del grupo.
        Session("cliente_id") = CInt(ds_clie.Tables(0).Rows(0).Item("Cliente"))

        Session("op_ingreso") = "si"
        Response.Redirect("Cliente_alta_b.aspx")
      Else
        'no existe
        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)
        'se redirecciona al modulo de alta
        Session("clientes_op") = "alta"
        Session("codigo_nuevo") = Txt_cliente_id.Text
        Session("op_ingreso") = "si"
        Response.Redirect("Cliente_alta_b.aspx")
      End If
    Else
      'se redirecciona al modulo de alta
      'Session("clientes_op") = "alta"
      'Response.Redirect("Cliente_alta_b.aspx")
      Txt_cliente_id.Focus()
    End If
  End Sub

  Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
    If (e.CommandName = "op_modificar") Then 'es mostrar info de alumnos
      ' Retrieve the row index stored in the CommandArgument property.
      Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
      Session("clientes_op") = "modificar"
      'pasar ademas el ID del grupo.
      Session("cliente_id") = id
      Session("op_ingreso") = "si"
      Response.Redirect("Cliente_alta.aspx")
    End If
    If (e.CommandName = "op_eliminar") Then 'es mostrar info de alumnos
      ' Retrieve the row index stored in the CommandArgument property.
      Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
      Session("id_eliminar") = id
    End If
  End Sub

  Private Sub btn_baja_modal_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_baja_modal.ServerClick
    DAClientes.Clientes_baja(Session("id_eliminar"))
    recuperar_clientes()
  End Sub

  Private Sub btn_close_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error.ServerClick

    Txt_cliente_id.Focus()
  End Sub

  Private Sub btn_ok_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error.ServerClick

    Txt_cliente_id.Focus()
  End Sub

  Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub




#Region "INIT"
  'AQUI agrego el atributo onfocus y asocio a la rutina js seleccionartexto para que cuando se ponga el foco en un textbox se seleccione todo el contenido
  Private Sub Txt_cliente_id_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_cliente_id.Init
    Txt_cliente_id.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub
#End Region


End Class
