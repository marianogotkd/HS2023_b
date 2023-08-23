Public Class TicketsClieReimprimir_op
  Inherits System.Web.UI.Page

  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      Permisos
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


        Case "2"
          'se verifica que permisos estan habilitados.
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 1,2,3 or null

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
            Dim SubOpcion As String = ""
            Try
              SubOpcion = ds_permisos.Tables(0).Rows(i).Item("SubOpcion")
            Catch ex As Exception
            End Try
            Dim SSOpcion As String = ""
            Try
              SSOpcion = ds_permisos.Tables(0).Rows(i).Item("SSOpcion")
            Catch ex As Exception
            End Try

            If Menu = "G" Then
              If Opcion = "" Then 'tiene que ser 1 or null
                Select Case SubOpcion 'tiene q ser 3 o null
                  Case ""
                    Select Case SSOpcion'tiene que ser 1, 2 or null
                      Case ""
                        Div_Op1.Visible = True
                        Div_Op2.Visible = True
                        valido = "si"
                      Case "1"
                        Div_Op1.Visible = True
                        valido = "si"
                      Case = "2"
                        Div_Op2.Visible = True
                        valido = "si"
                    End Select

                  Case "3"
                    Select Case SSOpcion
                      Case ""
                        Div_Op1.Visible = True
                        Div_Op2.Visible = True
                        valido = "si"
                      Case "1"
                        Div_Op1.Visible = True
                        valido = "si"
                      Case = "2"
                        Div_Op2.Visible = True
                        valido = "si"
                    End Select

                End Select
              End If
              If Opcion = "2" Then
                Select Case SubOpcion 'tiene q ser 3 o null
                  Case ""
                    Select Case SSOpcion'tiene que ser 1, 2 or null
                      Case ""
                        Div_Op1.Visible = True
                        Div_Op2.Visible = True
                        valido = "si"
                      Case "1"
                        Div_Op1.Visible = True
                        valido = "si"
                      Case = "2"
                        Div_Op2.Visible = True
                        valido = "si"
                    End Select

                  Case "3"
                    Select Case SSOpcion
                      Case ""
                        Div_Op1.Visible = True
                        Div_Op2.Visible = True
                        valido = "si"
                      Case "1"
                        Div_Op1.Visible = True
                        valido = "si"
                      Case = "2"
                        Div_Op2.Visible = True
                        valido = "si"
                    End Select

                End Select
              End If
            End If
            i = i + 1
          End While
          If valido = "si" Then
            'se accede sin problemas
          Else
            'no tiene permiso, se redirige a menu.
            Response.Redirect("~/WC_TicketsClientes/TicketsClientes_op2.aspx")
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

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_TicketsClientes/TicketsClieReimprimir.aspx")
  End Sub
  Private Sub BOTON_GRABAR_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABAR.ServerClick
    Select Case txt_opcion.Text.ToUpper
      Case "1"
        If Div_Op1.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/WC_TicketsClientes/TicketsClieReimprimir_orden.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "2"
        If Div_Op2.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/WC_TicketsClientes/TicketsClieReimprimir_recorridos.aspx")
        Else
          txt_opcion.Focus()
        End If


      Case Else
        ''aqui va mensaje de error.
        'no existe
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)
    End Select
  End Sub

  Private Sub btn_close_error_ServerClick(sender As Object, e As EventArgs) Handles btn_close_error.ServerClick
    txt_opcion.Focus()
  End Sub

  Private Sub btn_ok_error_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error.ServerClick
    txt_opcion.Focus()
  End Sub

  Private Sub txt_opcion_Init(sender As Object, e As EventArgs) Handles txt_opcion.Init
    txt_opcion.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub LinkButton_PorOrden_Click(sender As Object, e As EventArgs) Handles LinkButton_PorOrden.Click
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_TicketsClientes/TicketsClieReimprimir_orden.aspx")
  End Sub

  Private Sub LinkButton_PorRecorrido_Click(sender As Object, e As EventArgs) Handles LinkButton_PorRecorrido.Click
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_TicketsClientes/TicketsClieReimprimir_recorridos.aspx")
  End Sub
End Class
