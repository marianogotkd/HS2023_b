Public Class TicketsClieReimprimir
  Inherits System.Web.UI.Page

#Region "DECLARACIONES"
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
#End Region

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      PERMISOS
      Txt_fecha.Focus()

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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 1 or null y subopcion 3 o null

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
            If (Menu = "G" And Opcion = "") Or (Menu = "G" And Opcion = "2") Then

              If (SubOpcion = "") Or (SubOpcion = "3") Then
                valido = "si"
                Exit While
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
    End If

    If Session("op_ingreso") = "si" Then
      Session("op_ingreso") = ""
    Else
      Session("op_ingreso") = ""
      Response.Redirect("~/Inicio.aspx")
    End If

  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_TicketsClientes/TicketsClientes_op2.aspx")
  End Sub


  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick
    Dim valido = "si"
    Try
      Txt_fecha.Text = CDate(Txt_fecha.Text)
      Dim fecha_base As Date = CDate("01/01/1900")
      If Txt_fecha.Text < fecha_base Then
        valido = "no"
      End If
    Catch ex As Exception
      valido = "no"
    End Try


    If valido = "si" Then
      Dim ds_liq As DataSet = DAparametro.Parametro_consultar_fecha(CDate(Txt_fecha.Text))
      If ds_liq.Tables(0).Rows.Count <> 0 Then
        'verifico que el registro recuperado tenga estado="Inactivo"
        Dim estado = ds_liq.Tables(0).Rows(0).Item("Estado")
        If estado = "Inactivo" Then
          'redirecciono al proximo formulario

          Session("Fecha_regenerar") = CDate(Txt_fecha.Text)
          Session("op_ingreso") = "si"
          Response.Redirect("~/WC_TicketsClientes/TicketsClieReimprimir_op.aspx")

        Else

          'no hay liquidacion para esa fecha.
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
        End If

      Else
        'no hay liquidacion para esa fecha.
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
      End If
    Else
      'error, ingrese fecha valida.
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error2", "$(document).ready(function () {$('#modal-ok_error2').modal();});", True)
    End If


  End Sub

  Private Sub btn_error_close_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close.ServerClick
    Txt_fecha.Focus()
  End Sub

  Private Sub btn_ok_error_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error.ServerClick
    Txt_fecha.Focus()
  End Sub

  Private Sub btn_error_close2_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close2.ServerClick
    Txt_fecha.Focus()
  End Sub

  Private Sub btn_ok_error2_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error2.ServerClick
    Txt_fecha.Focus()
  End Sub
End Class



