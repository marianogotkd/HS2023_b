Public Class Cubiertas_op2a
  Inherits System.Web.UI.Page
  Dim DAGrupos As New Capa_Datos.WC_grupos
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      Permisos()
      Txt_GrupoCod.Focus()
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
            If (Menu = "J" And Opcion = "") Or (Menu = "J" And Opcion = "2") Then
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



  Private Sub Txt_GrupoCod_Init(sender As Object, e As EventArgs) Handles Txt_GrupoCod.Init
    Txt_GrupoCod.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub


  Private Sub btn_ok_error2_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error2.ServerClick
    Txt_GrupoCod.Focus()
  End Sub

  Private Sub btn_error_close2_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close2.ServerClick
    Txt_GrupoCod.Focus()
  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABM_Cubiertas/Cubiertas_op.aspx")
  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick

    Dim ds_grupo As DataSet = DAGrupos.Grupos_buscar_codigo(Txt_GrupoCod.Text)
    If ds_grupo.Tables(0).Rows.Count <> 0 Then
      'enviar el codigo del grupo y nombre.
      Session("Codigo") = ds_grupo.Tables(0).Rows(0).Item("Codigo")
      Session("Nombre") = ds_grupo.Tables(0).Rows(0).Item("Nombre")
      Session("Grupo_id") = ds_grupo.Tables(0).Rows(0).Item("Grupo_id")
      Session("op_ingreso") = "si"
      Response.Redirect("~/WC_ABM_Cubiertas/Cubiertas_op2b.aspx")
    Else
      'msj la busqueda no arrojo resultados.
      'modal-ok_error2  
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error2", "$(document).ready(function () {$('#modal-ok_error2').modal();});", True)
    End If

  End Sub

End Class
