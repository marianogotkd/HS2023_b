Public Class acl_gastos_alta
    Inherits System.Web.UI.Page
    Dim DAgastos As New Capa_Datos.WC_gastos
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      obtener_gastos_tipo()
      Txt_motivo.Text = ""
      limpiar()
      Txt_motivo.Focus()
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
            If (Menu = "D" And Opcion = "") Or (Menu = "D" And Opcion = "1") Then
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

  Private Sub obtener_gastos_tipo()
        Dim dataset1 As DataSet = DAgastos.GastosTipo_obtener_todos
        GridView1.DataSource = dataset1.Tables(0)
        GridView1.DataBind()
    End Sub

    Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ACL Gastos/acl_gastos.aspx")
  End Sub

    Private Sub btn_graba_mdl_cancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_mdl_cancelar.ServerClick
        Txt_motivo.Focus()
    End Sub

    Private Sub btn_graba_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_close.ServerClick
        Txt_motivo.Focus()
    End Sub

    Private Sub btn_graba_mdll_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_mdll.ServerClick

        'si confirma, valida y guarda
        Dim valido As String = "si"
        Dim ds_info As DataSet = DAgastos.GastosTipo_validar(Txt_motivo.Text)
        If ds_info.Tables(0).Rows.Count <> 0 Then
            valido = "no"
        End If
        If valido = "si" And Txt_motivo.Text <> "" Then
      DAgastos.GastosTipo_alta(Txt_motivo.Text)

      Session("op_ingreso") = "si"
      Response.Redirect("~/WC_ACL Gastos/acl_gastos.aspx")
    Else
            'modifique el motivo, ya existe.
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)
            lb_error_motivo.Visible = True
        End If
    End Sub

    Private Sub btn_close_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error.ServerClick
        Txt_motivo.Focus()
    End Sub

    Private Sub btn_ok_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error.ServerClick
        Txt_motivo.Focus()
    End Sub

    Private Sub limpiar()
        lb_error_motivo.Visible = False
    End Sub
End Class
