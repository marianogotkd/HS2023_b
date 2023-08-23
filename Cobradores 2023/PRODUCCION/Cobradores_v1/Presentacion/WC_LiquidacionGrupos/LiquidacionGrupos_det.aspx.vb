Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Public Class LiquidacionGrupos_det
  Inherits System.Web.UI.Page
  Dim DAParametro As New Capa_Datos.WC_parametro
  Dim DAliquidacion As New Capa_Datos.WC_Liquidacion
  Dim DAReliquidacion As New Capa_Datos.WC_Reliquidacion
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DAweb As New Capa_Datos.WC_Web
  Dim DAconfiguracion As New Capa_Datos.Configuracion
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos
      Dim DS_liqgrupos As New DS_liqgrupos
      DS_liqgrupos.Tables("LiqGrupos").Merge(Session("Tabla_LiqGrupos"))
      GridView1.DataSource = DS_liqgrupos.Tables("LiqGrupos")
      GridView1.DataBind()

      '----AQUI GENERO REPORTE-------

      DS_liqgrupos.Tables("LiqGrupos_rpt").Merge(Session("Tabla_LiqGrupos_rpt"))
      Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
      CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
      CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/LiquidacionGrupos_informe01.rpt"))
      CrReport.Database.Tables("LiqGrupos_rpt").SetDataSource(DS_liqgrupos.Tables("LiqGrupos_rpt"))
      CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/LiqGrupos.pdf"))
      '------------------------------

      Crear_BKP()
      btn_continuar.Focus()




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
            If (Menu = "10" And Opcion = "") Or (Menu = "10" And Opcion = "1") Then
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

  Private Sub btn_continuar_ServerClick(sender As Object, e As EventArgs) Handles btn_continuar.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Crear_BKP()
    '//////////borrar back T y crear uno nuevo.//////////////////////////////////
    'Modif 22-10-18 se intenta eliminar un bkp con la misma fecha y estado... ejemplo: "C:\BKPWC\WC_20220314A.bak"
    Dim ds_parametro As DataSet = DAParametro.Parametro_consultar_ultliq
    If ds_parametro.Tables(0).Rows.Count <> 0 Then
      Dim fecha_año As String = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha")).Year.ToString
      Dim fecha_mes As String = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha")).Month.ToString
      Dim fecha_dia As String = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha")).Day.ToString
      If fecha_dia.ToString.Length = 1 Then
        fecha_dia = "0" + fecha_dia
      End If
      If fecha_mes.ToString.Length = 1 Then
        fecha_mes = "0" + fecha_mes
      End If
      Dim archivo As String = "C:\BKPWC\" + "WC_" + fecha_año + fecha_mes + fecha_dia + "T.bak"
      DAReliquidacion.Reliquidacion_DeleteBkp(archivo)

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

      '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
      'nota: se creará un backup de la bd posterior a la liquidacion solo se almacenara la fecha y una letra al final...en este caso "T" para indicar que es una copia POSTERIOR a la liquidacion final
      DAliquidacion.BACKUP("T", CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha")))
      '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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

      Try

        Session("OP") = "5" 'la opcion 5 es para generar un .zip con las 2 bd, WebCentral posterior a la Liq. y la BD Copy.
        Session("BKP_fecha") = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha"))

        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx','_blank','height=600px,width=600px,scrollbars=1');", True)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx');", True)


      Catch ex As Exception

      End Try



    End If
  End Sub

End Class
