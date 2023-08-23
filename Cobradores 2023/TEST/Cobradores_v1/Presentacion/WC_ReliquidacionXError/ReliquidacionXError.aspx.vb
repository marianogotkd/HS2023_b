Public Class ReliquidacionXError
  Inherits System.Web.UI.Page

#Region "DECLARACIONES"
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAliquidacion As New Capa_Datos.WC_Liquidacion
  Dim DAReliquidacion As New Capa_Datos.WC_Reliquidacion
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DAconfiguracion As New Capa_Datos.Configuracion
  Dim DAweb As New Capa_Datos.WC_Web
#End Region

#Region "EVENTOS"

#End Region

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      'AQUI VALIDO, SI NO HAY NINGUNA FECHA EN LA TABLA PARAMETRO, PONGO UN MENSAJE MODAL QUE DIGA:
      'ERROR, PRIMERO DEBE INICIAR DIA.

      Dim ds_diaactivo As DataSet = DAparametro.Parametro_obtener_dia

      Dim valido = "no"
      If ds_diaactivo.Tables(0).Rows.Count <> 0 Then

        Dim LiqFinal As String = ""
        Try
          LiqFinal = CStr(ds_diaactivo.Tables(0).Rows(0).Item("LiqFinal"))
        Catch ex As Exception
          LiqFinal = ""
        End Try
        If LiqFinal = "" Then
          'SI parametro.LiqFinal = "" se recupera dia ya liquidado y se retrocede a esa fecha.
          Dim ds_info As DataSet = DAparametro.Parametro_obtener_UltimoDiaLiq
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

            Session("Fecha_anterior") = "Retrocede a fecha anterior"

          Else
            'AQUI MENSAJE Y QUE CON EL BOTON "OK" U "CLOSE" VUELVA AL MENU PRINCIPAL.
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
          End If
        Else
          'si parametro.LiqFinal <> "" quiere decir q ocurrio un error durante la liq final y se debe liq nuevamente el dia actual...x lo tanto se restaura el .bak para la fecha del dia activo.
          'cargo la fecha y el dia en los textbox
          HF_parametro_id.Value = ds_diaactivo.Tables(0).Rows(0).Item("Parametro_id")
          Dim FECHA As Date = CDate(ds_diaactivo.Tables(0).Rows(0).Item("Fecha"))
          HF_fecha.Value = ds_diaactivo.Tables(0).Rows(0).Item("Fecha")
          'Label_fecha.Text = FECHA.ToString("yyyy-MM-dd")
          Label_fecha.Text = FECHA.ToString("dd-MM-yyyy")
          Dim dia As Integer = CInt(ds_diaactivo.Tables(0).Rows(0).Item("Dia"))
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

        End If

      Else
        'NO HAY DIA ACTIVO, ENTONCES RE RECUPERA DIA YA LIQUIDADO Y SE RETROCEDE A ESA FECHA
        Dim ds_info As DataSet = DAparametro.Parametro_obtener_UltimoDiaLiq
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
          Session("Fecha_anterior") = "Retrocede a fecha anterior"
        Else
          'AQUI MENSAJE Y QUE CON EL BOTON "OK" U "CLOSE" VUELVA AL MENU PRINCIPAL.
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
        End If

      End If




      Txt_OP.Focus()
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
              Menu = ds_permisos.Tables(0).Rows(i).Item("Menu")
            Catch ex As Exception
            End Try
            Dim Opcion As String = ""
            Try
              Opcion = ds_permisos.Tables(0).Rows(i).Item("Opcion")
            Catch ex As Exception
            End Try
            If (Menu = "99" And Opcion = "") Or (Menu = "99" And Opcion = "1") Then
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

  Private Sub Txt_OP_Init(sender As Object, e As EventArgs) Handles Txt_OP.Init
    Txt_OP.Attributes.Add("onfocus", "seleccionarTexto(this);")
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

  Private Sub btn_modificar_ServerClick(sender As Object, e As EventArgs) Handles btn_modificar.ServerClick
    If Txt_OP.Text.ToUpper = "OK" Then
      metodo1()
    Else
      'error
      Txt_OP.Focus()
    End If
  End Sub

  Private Sub metodo1()

    '///////////////////////SE EJECUTA EL SP WebDeshabilitar solo si Configuracion.Web=true/////////////////////////////////
    Dim ds_config As DataSet = DAconfiguracion.Configuracion_obtenertodo()
    If ds_config.Tables(0).Rows.Count <> 0 Then
      If ds_config.Tables(0).Rows(0).Item("Web") = True Then
        Try
          DAweb.WebDeshabilitar()
        Catch ex As Exception
        End Try
      End If
    End If
    '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Dim fecha_año As String = CDate(HF_fecha.Value).Year.ToString
    Dim fecha_mes As String = CDate(HF_fecha.Value).Month.ToString
    Dim fecha_dia As String = CDate(HF_fecha.Value).Day.ToString

    If fecha_dia.ToString.Length = 1 Then
      fecha_dia = "0" + fecha_dia
    End If
    If fecha_mes.ToString.Length = 1 Then
      fecha_mes = "0" + fecha_mes
    End If

    Dim base_datos As String = "WC_" + fecha_año.ToString + fecha_mes.ToString + fecha_dia.ToString + "A.bak"

    Dim directorio_bak As String = "C:\BKPWC\" + base_datos

    Dim prueba As String = base_datos.Substring(0, 10)

    '0) LLAMO A UN PROCEDIMIENTOS QUE ME RECUPERA LA RUTA DONDE SE ENCUENTRA LA BASE DE DATOS
    Dim ds_directorioBD As DataSet = DAReliquidacion.Reliquidacion_Obtener_directorioBD
    'quitar de cadena "WebCentral.mdf", 14 caracteres.
    Dim directorio As String = ds_directorioBD.Tables(0).Rows(0).Item("CurrentLocation").ToString


    '1a) ELIMINO SI EXISTE WC_D.bak del directorio "C:\BKPWC\"
    DAReliquidacion.Reliquidacion_DeleteBkp("C:\BKPWC\WC_D.bak")

    '1b) hacemos copia de la bd actual. en el parametro CONDICION VAMOS A PONER "D" DE MANERA SIMBOLICA, INDICANDO QUE ES UN BKP O RESPALDO DEL DIA DE TRABAJO.
    DAliquidacion.BACKUP_aux("D")
    Dim BD_ACTUAL_bak As String = "C:\BKPWC\WC_D.bak"

    '1c) ELIMINO SI EXISTE LA BASE DE DATOS "WebCentral_copy", ya que necesito que no exista para poder crear y restaurar en "2)"
    Try
      DAliquidacion.DROP_DATABASE_WEBCENTRAL_COPY()
    Catch ex As Exception

    End Try


    '2) RESTAURAMOS EL BACK "WC_D.bak" en una nueva bd auxiliar.
    DAliquidacion.restore_backup("WebCentral_copy", directorio.Substring(0, directorio.Length - 14), BD_ACTUAL_bak)    'EL PARAMETRO DEL MEDIO ES RUTA DESTINO
    'DAliquidacion.restore_backup("WebCentral_copy", "C:\BKPWC\", directorio_bak)

    '3)Cierro conexion de la BD Web Central, esto lo hago para poder restaurar a la fecha anterior
    DAReliquidacion.Reliquidacion_CerrarConexion()

    '4) RESTAURAMOS al dia anterior previa a la liquidacion.
    DAReliquidacion.restore_backup_WebCentral("WebCentral", directorio.Substring(0, directorio.Length - 14), directorio_bak)

    '5) Cambio el estado del campo Terminales en la tabla Parametro. Coloco el valor en false (0) para deshabilitar el ingreso de datos.
    DAReliquidacion.Reliquidacion_TerminalesEstado(0, CInt(HF_parametro_id.Value))

    If Session("Fecha_anterior") = "Retrocede a fecha anterior" Then
      DAparametro.Parametro_ReliquidacionModifEstado(CInt(HF_parametro_id.Value), "SI")
    End If
    Session("Fecha_anterior") = ""

    '///////////////////////SE EJECUTA EL SP WebHabilitar solo si Configuracion.Web=true/////////////////////////////////

    If ds_config.Tables(0).Rows.Count <> 0 Then
      If ds_config.Tables(0).Rows(0).Item("Web") = True Then
        Try
          DAweb.WebHabilitar()
        Catch ex As Exception
        End Try
      End If
    End If
    '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    'aqui va un cartel: "SE RESTAURO CORRECTAMENTE."
    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)



  End Sub

  Private Sub Btn_ok_close_ServerClick(sender As Object, e As EventArgs) Handles Btn_ok_close.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Btn_ok_ServerClick(sender As Object, e As EventArgs) Handles Btn_ok.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub
End Class
