Public Class ab_recorridos_zonas
    Inherits System.Web.UI.Page
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      txt_dia.Focus()

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
            If (Menu = "E" And Opcion = "") Or (Menu = "E" And Opcion = "1") Then
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


  Private Sub BOTON_GRABAR_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BOTON_GRABAR.ServerClick
        Try
            Dim opcion As Integer = CInt(txt_dia.Text)
            If opcion > 0 And opcion < 8 Then
                Session("dia_seleccionado") = opcion

        Session("op_ingreso") = "si"
        Response.Redirect("~/WC_AB Recorridos_Zonas/ab_recorridos_zonas_activacion.aspx")

      Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)
            End If





        Catch ex As Exception
            'aqui mensaje de que la opcion es incorrecta.
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)
        End Try
    End Sub

    Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
        Response.Redirect("~/Inicio.aspx")
    End Sub

    Private Sub btn_ok_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error.ServerClick
        txt_dia.Focus()
    End Sub


    'AQUI agrego el atributo onfocus y asocio a la rutina js seleccionartexto para que cuando se ponga el foco en un textbox se seleccione todo el contenido
    Private Sub txt_dia_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_dia.Init
        txt_dia.Attributes.Add("onfocus", "seleccionarTexto(this);")
    End Sub

    Private Sub LinkButton_domingo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton_domingo.Click
        txt_dia.Text = 1
        txt_dia.Focus()
    End Sub

    Private Sub LinkButton_lunes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton_lunes.Click
        txt_dia.Text = 2
        txt_dia.Focus()
    End Sub

    Private Sub LinkButton_martes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton_martes.Click
        txt_dia.Text = 3
        txt_dia.Focus()
    End Sub

    Private Sub LinkButton_miercoles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton_miercoles.Click
        txt_dia.Text = 4
        txt_dia.Focus()
    End Sub

    Private Sub LinkButton_Jueves_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton_Jueves.Click
        txt_dia.Text = 5
        txt_dia.Focus()
    End Sub

    Private Sub LinkButton_viernes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton_viernes.Click
        txt_dia.Text = 6
        txt_dia.Focus()
    End Sub

    Private Sub LinkButton_sabado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton_sabado.Click
        txt_dia.Text = 7
        txt_dia.Focus()
    End Sub
End Class
