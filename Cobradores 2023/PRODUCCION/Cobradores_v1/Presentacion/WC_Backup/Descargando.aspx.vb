Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports Ionic.Zip
Public Class Descargando
  Inherits System.Web.UI.Page
  Dim DABackup As New Capa_Datos.WC_Backup
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAconfiguracion As New Capa_Datos.Configuracion
  Dim DAweb As New Capa_Datos.WC_Web
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Select Case Session("OP")
      Case "1" 'copiar WebCentral
        Dim dbNAme As String = "WebCentral"
        Dim backupDestination As String = Server.MapPath("~/Backup")
        If Not Directory.Exists(backupDestination) Then
          Directory.CreateDirectory(backupDestination)
        End If
        'Dim fileName As String = dbNAme + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak"
        Dim fileName As String = dbNAme + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak"

        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & ".bak'"
        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".Bak'"
        Dim backupfile As String = backupDestination & "\" & "WebCentral" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".Bak'"


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

        DABackup.Backup(backupfile)

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

        Dim bytes As Byte() = File.ReadAllBytes(Path.Combine(backupDestination, fileName))
        ' Delete .bak file from server folder.
        If Directory.Exists(backupDestination) Then
          Directory.Delete(backupDestination, True)
        End If
        Response.Clear()
        Response.Buffer = True
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/octet-stream"
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & fileName)
        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.End()
      Case "2" 'Copiar WebCentral_copy
        Dim dbNAme As String = "WebCentral_copy"
        Dim backupDestination As String = Server.MapPath("~/Backup")
        If Not Directory.Exists(backupDestination) Then
          Directory.CreateDirectory(backupDestination)
        End If
        'Dim fileName As String = dbNAme + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak"
        Dim fileName As String = dbNAme + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak"

        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & ".bak'"
        'Dim backupfile As String = backupDestination & "\" & "WebCentral_copy" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".Bak'"
        Dim backupfile As String = backupDestination & "\" & "WebCentral_copy" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".Bak'"

        DABackup.Backup_copy(backupfile)

        Dim bytes As Byte() = File.ReadAllBytes(Path.Combine(backupDestination, fileName))
        ' Delete .bak file from server folder.
        If Directory.Exists(backupDestination) Then
          Directory.Delete(backupDestination, True)
        End If
        Response.Clear()
        Response.Buffer = True
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/octet-stream"
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & fileName)
        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.End()

      Case "3" 'copia de ambas bd

        Dim dbNAme As String = "WebCentral"
        Dim backupDestination As String = Server.MapPath("~/Backup")
        If Not Directory.Exists(backupDestination) Then
          Directory.CreateDirectory(backupDestination)
        End If
        'Dim fileName As String = dbNAme + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak"
        Dim fileName As String = dbNAme + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak"

        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & ".bak'"
        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".Bak'"
        Dim backupfile As String = backupDestination & "\" & "WebCentral" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".Bak'"

        DABackup.Backup(backupfile)


        '*****************Dim dbNAme As String = "WebCentral_copy"
        Dim dbNAme2 As String = "WebCentral_copy"
        'Dim backupDestination As String = Server.MapPath("~/Backup")
        If Not Directory.Exists(backupDestination) Then
          Directory.CreateDirectory(backupDestination)
        End If
        'Dim fileName As String = dbNAme + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak"
        Dim fileName2 As String = dbNAme2 + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak"

        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & ".bak'"
        'Dim backupfile As String = backupDestination & "\" & "WebCentral_copy" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".Bak'"
        Dim backupfile2 As String = backupDestination & "\" & "WebCentral_copy" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".Bak'"

        DABackup.Backup_copy(backupfile2)
        '*****************************
        Dim paquete_zip As New ZipFile()

        paquete_zip.AddFile(backupDestination + "\" + fileName, "")
        paquete_zip.AddFile(backupDestination + "\" + fileName2, "")
        Dim fileName_paquete As String = "WebCentral" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".zip"
        paquete_zip.Save(Server.MapPath("~/Backup/" + fileName_paquete))

        Dim bytes As Byte() = File.ReadAllBytes(Path.Combine(backupDestination, fileName_paquete))

        ' Delete .bak file from server folder.
        If Directory.Exists(backupDestination) Then
          Directory.Delete(backupDestination, True)
        End If
        Response.Clear()
        Response.Buffer = True
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/octet-stream"
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & fileName_paquete)
        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.End()
      Case 4
        'SE HACE UN BACK UP DE LA BD WEB CENTRAL PREVIA A LA LIQUIDACION. SE LA USA EN EL MODULO DE LIQUIDACION FINAL.
        Dim ds_info As DataSet = DAparametro.Parametro_obtener_dia
        If ds_info.Tables(0).Rows.Count <> 0 Then
          Dim FECHA As Date = CDate(ds_info.Tables(0).Rows(0).Item("Fecha"))

          Dim dbNAme As String = "WC_"
          Dim backupDestination As String = Server.MapPath("~/Backup")
          If Not Directory.Exists(backupDestination) Then
            Directory.CreateDirectory(backupDestination)
          End If
          'Dim fileName As String = dbNAme + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak"
          Dim fileName As String = dbNAme + FECHA.ToString("yyyyMMdd") + "A" + ".bak"

          'Dim backupfile As String = backupDestination & "\" & "WebCentral" & ".bak'"
          'Dim backupfile As String = backupDestination & "\" & "WebCentral" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".Bak'"
          Dim backupfile As String = backupDestination & "\" & "WC_" & FECHA.ToString("yyyyMMdd") & "A" & ".Bak'"


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



          DABackup.Backup(backupfile)



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




          '*****************Dim dbNAme As String = "WebCentral_copy"
          Dim dbNAme2 As String = "WebCentral_copy"
          'Dim backupDestination As String = Server.MapPath("~/Backup")
          If Not Directory.Exists(backupDestination) Then
            Directory.CreateDirectory(backupDestination)
          End If
          'Dim fileName As String = dbNAme + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak"
          Dim fileName2 As String = dbNAme2 + FECHA.ToString("yyyyMMdd") + ".bak"

          'Dim backupfile As String = backupDestination & "\" & "WebCentral" & ".bak'"
          'Dim backupfile As String = backupDestination & "\" & "WebCentral_copy" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".Bak'"
          Dim backupfile2 As String = backupDestination & "\" & "WebCentral_copy" & FECHA.ToString("yyyyMMdd") & ".Bak'"

          DABackup.Backup_copy(backupfile2)
          '*****************************
          Dim paquete_zip As New ZipFile()

          paquete_zip.AddFile(backupDestination + "\" + fileName, "")
          paquete_zip.AddFile(backupDestination + "\" + fileName2, "")
          Dim fileName_paquete As String = "WebCentral" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".zip"
          paquete_zip.Save(Server.MapPath("~/Backup/" + fileName_paquete))

          Dim bytes As Byte() = File.ReadAllBytes(Path.Combine(backupDestination, fileName_paquete))

          ' Delete .bak file from server folder.
          If Directory.Exists(backupDestination) Then
            Directory.Delete(backupDestination, True)
          End If
          Response.Clear()
          Response.Buffer = True
          Response.Charset = ""
          Response.Cache.SetCacheability(HttpCacheability.NoCache)
          Response.ContentType = "application/octet-stream"
          Response.AppendHeader("Content-Disposition", "attachment; filename=" & fileName_paquete)
          Response.BinaryWrite(bytes)
          Response.Flush()
          Response.End()

        End If
      Case 5
        Dim FECHA As Date = Session("BKP_fecha")

        Dim dbNAme As String = "WC_"
        Dim backupDestination As String = Server.MapPath("~/Backup")
        If Not Directory.Exists(backupDestination) Then
          Directory.CreateDirectory(backupDestination)
        End If
        'Dim fileName As String = dbNAme + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak"
        Dim fileName As String = dbNAme + FECHA.ToString("yyyyMMdd") + "T" + ".bak"

        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & ".bak'"
        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".Bak'"
        Dim backupfile As String = backupDestination & "\" & "WC_" & FECHA.ToString("yyyyMMdd") & "T" & ".Bak'"


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

        DABackup.Backup(backupfile)


        '///////////////////////SE EJECUTA EL SP WebHabilitar solo si Configuracion.Web=true/////////////////////////////////
        'Dim ds_config As DataSet = DAconfiguracion.Configuracion_obtenertodo()
        If ds_config.Tables(0).Rows.Count <> 0 Then
          If ds_config.Tables(0).Rows(0).Item("Web") = True Then
            Try
              DAweb.WebHabilitar()
            Catch ex As Exception
            End Try
          End If
        End If
        '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        '*****************Dim dbNAme As String = "WebCentral_copy"
        Dim dbNAme2 As String = "WebCentral_copy"
        'Dim backupDestination As String = Server.MapPath("~/Backup")
        If Not Directory.Exists(backupDestination) Then
          Directory.CreateDirectory(backupDestination)
        End If
        'Dim fileName As String = dbNAme + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak"
        Dim fileName2 As String = dbNAme2 + FECHA.ToString("yyyyMMdd") + ".bak"

        'Dim backupfile As String = backupDestination & "\" & "WebCentral" & ".bak'"
        'Dim backupfile As String = backupDestination & "\" & "WebCentral_copy" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".Bak'"
        Dim backupfile2 As String = backupDestination & "\" & "WebCentral_copy" & FECHA.ToString("yyyyMMdd") & ".Bak'"

        DABackup.Backup_copy(backupfile2)
        '*****************************
        Dim paquete_zip As New ZipFile()

        paquete_zip.AddFile(backupDestination + "\" + fileName, "")
        paquete_zip.AddFile(backupDestination + "\" + fileName2, "")
        Dim fileName_paquete As String = "WebCentral" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".zip"
        paquete_zip.Save(Server.MapPath("~/Backup/" + fileName_paquete))

        Dim bytes As Byte() = File.ReadAllBytes(Path.Combine(backupDestination, fileName_paquete))

        ' Delete .bak file from server folder.
        If Directory.Exists(backupDestination) Then
          Directory.Delete(backupDestination, True)
        End If
        Response.Clear()
        Response.Buffer = True
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/octet-stream"
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & fileName_paquete)
        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.End()

    End Select
    If Response.Buffer = True Then
      Dim estatus = "sigue lleno"
    Else
      Dim estatus = "vacio"
    End If

    'Response.Clear()
    'Response.Redirect("~/Inicio.aspx")

  End Sub

  Private Sub Descargando_SaveStateComplete(sender As Object, e As EventArgs) Handles Me.SaveStateComplete
    'Dim ahora = "me ejecuto"
    'Response.Redirect("~/Inicio.aspx")
  End Sub
End Class
