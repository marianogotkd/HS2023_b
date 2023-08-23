Public Class ConsultarModificar_xcargas_a
  Inherits System.Web.UI.Page
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAConsultas As New Capa_Datos.WB_Consultas
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      'AQUI VALIDO, SI NO HAY NINGUNA FECHA EN LA TABLA PARAMETRO, PONGO UN MENSAJE MODAL QUE DIGA:
      'ERROR, PRIMERO DEBE INICIAR DIA.
      Dim ds_info As DataSet = DAparametro.Parametro_obtener_dia
      If ds_info.Tables(0).Rows.Count <> 0 Then
        'cargo la fecha y el dia en los textbox
        HF_parametro_id.Value = ds_info.Tables(0).Rows(0).Item("Parametro_id")
        Dim FECHA As Date = CDate(ds_info.Tables(0).Rows(0).Item("Fecha"))
        HF_fecha.Value = ds_info.Tables(0).Rows(0).Item("Fecha")
        'Label_fecha.Text = FECHA.ToString("yyyy-MM-dd")
        Label_fecha.Text = FECHA.ToString("dd-MM-yyyy")
        Dim dia As Integer = CInt(ds_info.Tables(0).Rows(0).Item("Dia"))
        HF_dia_id.Value = dia
        Select Case dia
          Case 1
            Label_dia.Text = "DOMINGO."
          Case 2
            Label_dia.Text = "LUNES."
          Case 3
            Label_dia.Text = "MARTES."
          Case 4
            Label_dia.Text = "MIERCOLES."
          Case 5
            Label_dia.Text = "JUEVES."
          Case 6
            Label_dia.Text = "VIERNES."
          Case 7
            Label_dia.Text = "SABADO."
        End Select
        'mostrar_zonas_habilitadas(dia)

      Else
        'AQUI MENSAJE Y QUE CON EL BOTON "OK" U "CLOSE" VUELVA AL MENU PRINCIPAL.
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
      End If

      Txt_ClienteCod.Focus()
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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 4 or null

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
            If (Menu = "5" And Opcion = "") Or (Menu = "5" And Opcion = "4") Then
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

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub
  Private Sub btn_error_close_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_ok_error_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Txt_ClienteCod_Init(sender As Object, e As EventArgs) Handles Txt_ClienteCod.Init
    Txt_ClienteCod.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub btn_modificar_ServerClick(sender As Object, e As EventArgs) Handles btn_modificar.ServerClick
    Dim Valido = "si"
    Try
      Txt_ClienteCod.Text = CInt(Txt_ClienteCod.Text)
    Catch ex As Exception
      Valido = "no"
    End Try

    If Valido = "si" Then
      Dim ds_tablas_disp As DataSet = DAConsultas.XCargas_ObtenerNombraTablas()

      If ds_tablas_disp.Tables(0).Rows.Count <> 0 Then
        Dim i As Integer = 0
        Dim cont_existe As Integer = 0

        While i < ds_tablas_disp.Tables(0).Rows.Count
          Dim Tabla As String = ds_tablas_disp.Tables(0).Rows(i).Item("nombre").ToString

          'hago una consulta x cada tabla buscando x cliente
          Dim DS_INFO As DataSet = DAConsultas.XCargas_obtenerTabla(Tabla, Txt_ClienteCod.Text)
          If DS_INFO.Tables(0).Rows.Count <> 0 Then
            cont_existe = cont_existe + 1
          End If
          i = i + 1
        End While
        If cont_existe <> 0 Then
          'paso al proximo formulario
          Session("cliente_codigo") = Txt_ClienteCod.Text
          Session("op_ingreso") = "si"
          Response.Redirect("~/Consultas/ConsultarModificar_xcargas_b.aspx")
        Else
          'MENSAJE: La busqueda no arrojo resultados

          Label_ErrorValidacion.Text = "La busqueda no arrojo resultados!"
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ErrorValidacion", "$(document).ready(function () {$('#modal-ErrorValidacion').modal();});", True)

        End If

      Else
        'no existe ninguna tabla "xcargas"

      End If
    Else
      Label_ErrorValidacion.Text = "La busqueda no arrojo resultados!"
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ErrorValidacion", "$(document).ready(function () {$('#modal-ErrorValidacion').modal();});", True)
    End If








  End Sub

  Private Sub Btn_ErrorValidacionClose_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorValidacionClose.ServerClick
    Txt_ClienteCod.Focus()
  End Sub

  Private Sub Btn_ErrorValidacionOk_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorValidacionOk.ServerClick
    Txt_ClienteCod.Focus()
  End Sub
End Class
