Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports Ionic.Zip

Public Class Descargando
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Llaves_ds1 As New Llaves_ds

        Llaves_ds1.Tables("LLAVES_PDF").Rows.Clear()
        Llaves_ds1.Tables("LLAVES_PDF").Merge(Session("tabla_pdf"))

        Dim backupDestination As String = Server.MapPath("~/Archivos_pdf/Backup")
        If Not Directory.Exists(backupDestination) Then
            Directory.CreateDirectory(backupDestination)
        End If

        Dim paquete_zip As New ZipFile()


        Dim i As Integer = 0
        While i < Llaves_ds1.Tables("LLAVES_PDF").Rows.Count
            Dim filename As String = Llaves_ds1.Tables("LLAVES_PDF").Rows(i).Item("RUTA")
            paquete_zip.AddFile(backupDestination + "\" + filename, "")
            i = i + 1
        End While
        Dim fileName_paquete As String = "LLAVESZIP" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".zip"
        paquete_zip.Save(Server.MapPath("~/Archivos_pdf/Backup/" + fileName_paquete))

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


    End Sub

End Class