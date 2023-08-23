Public Class MenuConsultas
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
      txt_opcion.Focus()
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
          Div_Op1.Visible = True
          Div_Op2.Visible = True
          Div_Op3.Visible = True
          Div_Op4.Visible = True
          Div_Op5.Visible = True
        Case "2"
          'se verifica que permisos estan habilitados.
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 1,2,3,4 or null

          Dim ds_permisos As DataSet = DApermisos.Permisos_buscar(Idusuario)
          Dim i As Integer = 0
          Dim valido As String = "no"
          While i < ds_permisos.Tables(0).Rows.Count
            Dim Menu As String = ""
            Try
              Menu = ds_permisos.Tables(0).Rows(i).Item("Menu")
            Catch ex As Exception
            End Try
            Dim Opcion As String = ""
            Try
              Opcion = ds_permisos.Tables(0).Rows(i).Item("Opcion")
            Catch ex As Exception
            End Try
            If Menu = "5" Then
              If Opcion = "" Then
                valido = "si"
                Div_Op1.Visible = True
                Div_Op2.Visible = True
                Div_Op3.Visible = True
                Div_Op4.Visible = True
                Div_Op5.Visible = True
              End If
              If Opcion = "1" Then
                valido = "si"
                Div_Op1.Visible = True
              End If
              If Opcion = "2" Then
                valido = "si"
                Div_Op2.Visible = True
              End If
              If Opcion = "3" Then
                valido = "si"
                Div_Op3.Visible = True
              End If
              If Opcion = "4" Then
                valido = "si"
                Div_Op4.Visible = True
              End If
              If Opcion = "5" Then
                valido = "si"
                Div_Op4.Visible = True
              End If
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

      If Session("op_ingreso") = "si" Then
        Session("op_ingreso") = ""
      Else
        Session("op_ingreso") = ""
        Response.Redirect("~/Inicio.aspx")
      End If


    End If


  End Sub


  Private Sub btn_ok_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error.ServerClick
    txt_opcion.Focus()
  End Sub

  Private Sub btn_close_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error.ServerClick
    txt_opcion.Focus()
  End Sub


  'AQUI agrego el atributo onfocus y asocio a la rutina js seleccionartexto para que cuando se ponga el foco en un textbox se seleccione todo el contenido
  Private Sub txt_opcion_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_opcion.Init
    txt_opcion.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub LinkButton_CodigoPremiado_Click(sender As Object, e As EventArgs) Handles LinkButton_CodigoPremiado.Click
    'opcion 2
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/CodigoMasPremiadoRecorridos_b.aspx")
  End Sub

  Private Sub LinkButton_CodigosMasCargados_Click(sender As Object, e As EventArgs) Handles LinkButton_CodigosMasCargados.Click
    'opcion 1
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/CodigoMasPremiadoRecorridos.aspx")
  End Sub

  Private Sub LinkButton_IngresoDeTerminales_Click(sender As Object, e As EventArgs) Handles LinkButton_IngresoDeTerminales.Click
    'opcion 3
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/IngresoTerminales.aspx")
  End Sub

  Private Sub LinkButton_ConsultarModificar_Click(sender As Object, e As EventArgs) Handles LinkButton_ConsultarModificar.Click
    'opcion 4
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/ConsultarModificar_xcargas_a.aspx")
  End Sub

  Private Sub LinkButton_RecaudacionxCliente_Click(sender As Object, e As EventArgs) Handles LinkButton_RecaudacionxCliente.Click
    'opcion 5
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/RecaudacionxCliente.aspx")
  End Sub

  Private Sub BOTON_GRABAR_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABAR.ServerClick
    Select Case txt_opcion.Text.ToUpper
      Case "1"
        If Div_Op1.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/Consultas/CodigoMasPremiadoRecorridos.aspx")
          'Response.Redirect("~/Consultas/CodigoMasPremiado.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "2"
        If Div_Op2.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/Consultas/CodigoMasPremiadoRecorridos_b.aspx")
        Else
          txt_opcion.Focus()
        End If


      Case "3"
        If Div_Op3.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/Consultas/IngresoTerminales.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "4"
        If Div_Op4.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/Consultas/ConsultarModificar_xcargas_a.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "5"
        If Div_Op5.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/Consultas/RecaudacionxCliente.aspx")
        Else
          txt_opcion.Focus()
        End If
      Case Else
        ''aqui va mensaje de error.
        'no existe
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)

    End Select

  End Sub




End Class
