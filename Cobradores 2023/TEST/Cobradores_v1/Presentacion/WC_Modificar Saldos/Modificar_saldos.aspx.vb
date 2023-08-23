Public Class Modificar_saldos
    Inherits System.Web.UI.Page
    Dim DAClientes As New Capa_Datos.WB_clientes
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
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
            If (Menu = "F" And Opcion = "") Or (Menu = "F" And Opcion = "1") Then
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

  Private Sub btn_modificar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_modificar.ServerClick
        If Txt_cliente_id.Text <> "" Then
            Dim ds_clie As DataSet = DAClientes.Clientes_buscar_codigo(Txt_cliente_id.Text)

            If ds_clie.Tables(0).Rows.Count <> 0 Then
                Session("clientes_op") = "modificar"
        'pasar ademas el ID del grupo.
        Session("cliente_id") = CInt(ds_clie.Tables(0).Rows(0).Item("Cliente"))
        Session("op_ingreso") = "si"
        Response.Redirect("Modificar_saldos_detalle.aspx")
      Else
                'no existe
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)
                
            End If
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)
        End If
    End Sub

    Private Sub btn_close_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error.ServerClick
        Txt_cliente_id.Focus()
    End Sub

    Private Sub btn_ok_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error.ServerClick
        Txt_cliente_id.Focus()
    End Sub

    'AQUI agrego el atributo onfocus y asocio a la rutina js seleccionartexto para que cuando se ponga el foco en un textbox se seleccione todo el contenido
    Private Sub Txt_cliente_id_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_cliente_id.Init
        Txt_cliente_id.Attributes.Add("onfocus", "seleccionarTexto(this);")
    End Sub

    Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
        Response.Redirect("~/Inicio.aspx")
    End Sub



End Class
