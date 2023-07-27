Public Class ListadoxCategorias_view
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim dasf As DataTable = Session("dataset_cred_seleccionadas")
        Dim inscriptos_reporte As DataTable = Session("datatable_inscriptos_reporte")
        Dim inscriptos_info_reporte As DataTable = Session("datatble_inscriptos_info_reporte")


        If inscriptos_reporte.Rows.Count <> 0 Then
            'Dim cant As Integer = dasf.Rows.Count
            Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
            CrReport.Load(Server.MapPath("~/Llaves/Reporte_listadoxcategorias/ListadoxCategorias_reporte.rpt"))
            CrReport.Database.Tables("inscriptos_reporte").SetDataSource(inscriptos_reporte)
            CrReport.Database.Tables("inscriptos_info_reporte").SetDataSource(inscriptos_info_reporte)
            CrystalReportViewer1.ReportSource = CrReport

            'CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,  )
            'CrReport.Dispose()
            'EventLog.WriteEntry(sSource, "XML y PDF Creados")


            'CrReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, "Crystal")
            'Response.End()

            'CrReport.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)



        Else
            'Dim ds_inscripciones As DataSet = DAinscripciones.Inscripciones_credenciales_obtener(Session("evento_id"))
            'credenciales_ds.Tables("Credenciales").Merge(ds_inscripciones.Tables(0))
            'Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            'CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
            'CrReport.Load(Server.MapPath("~/Credenciales/reportes/Credenciales_1.rpt"))
            'CrReport.Database.Tables("Credenciales").SetDataSource(credenciales_ds.Tables("Credenciales"))
            'CrystalReportViewer1.ReportSource = CrReport
        End If
    End Sub

End Class