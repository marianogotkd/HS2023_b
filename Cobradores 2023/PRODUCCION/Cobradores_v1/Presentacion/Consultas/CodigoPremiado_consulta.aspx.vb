Public Class CodigoPremiado_consulta
  Inherits System.Web.UI.Page
  Dim Darecorridos_zonas As New Capa_Datos.WC_recorridos_zonas


  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      'Permisos()
      Dim DS_Consultas As New DS_Consultas

      HF_fecha.Value = Session("fecha_parametro")
      HF_dia_id.Value = Session("dia")

      TxtZona.Text = Session("Zona")
      TxtCodigo.Text = Session("Codigo")

      txtImporte1.Text = Session("unacifra")
      txtImporte2.Text = Session("doscifras")
      txtImporte3.Text = Session("trescifras")
      txtImporte4.Text = Session("cuatrocifras")

      txtImporte1.ReadOnly = True
      txtImporte2.ReadOnly = True
      txtImporte3.ReadOnly = True
      txtImporte4.ReadOnly = True

      TxtZona.ReadOnly = True
      TxtCodigo.ReadOnly = True

      DS_Consultas.Tables("ZonasHabilitadas_importes").Rows.Clear()
      DS_Consultas.Tables("ZonasHabilitadas_importes").Merge(Session("tabla_zonas"))

      '////////////VOY A CAMBIAR EL CODIGO DE ZONA POR LA REFERENCIA.////////////////////////////////
      'Zona1,2,3,4,5
      Dim ds_zonas As DataSet = Darecorridos_zonas.recorridos_zonas_consultar_dia(HF_dia_id.Value)
      Dim i As Integer = 0
      While i < DS_Consultas.Tables("ZonasHabilitadas_importes").Rows.Count
        Try
          Dim codigoZ1 As String = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(i).Item("Zona1"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ1, i, "Zona1")
        Catch ex As Exception
        End Try
        Try
          Dim codigoZ2 As String = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(i).Item("Zona2"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ2, i, "Zona2")
        Catch ex As Exception
        End Try
        Try
          Dim codigoZ3 As String = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(i).Item("Zona3"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ3, i, "Zona3")
        Catch ex As Exception
        End Try
        Try
          Dim codigoZ4 As String = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(i).Item("Zona4"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ4, i, "Zona4")
        Catch ex As Exception
        End Try
        Try
          Dim codigoZ5 As String = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(i).Item("Zona5"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ5, i, "Zona5")
        Catch ex As Exception
        End Try
        i = i + 1
      End While


      '/////////////////////////////////////////////////////////////////////////////////////////////


      GridView1.DataSource = DS_Consultas.Tables("ZonasHabilitadas_importes") 'Session("tabla_zonas")
      GridView1.DataBind()
      Txt_totalrecaudado.Text = Session("TotalRecaudado")
      Txt_totalrecaudado.ReadOnly = True


      '///////////////////GENERACION DEL REPORTE////////////////////////////////

      DS_Consultas.Tables("CodigoPremiado_reporte").Rows.Clear()
      Dim fila As DataRow = DS_Consultas.Tables("CodigoPremiado_reporte").NewRow
      'voy a buscar la referencia a partir del codigo ingresado.
      Dim ds_zona As DataSet = Darecorridos_zonas.recorridos_zonas_buscar_codigo(CStr(TxtZona.Text), HF_dia_id.Value)

      fila("Zona") = TxtZona.Text
      Try
        fila("Zona") = TxtZona.Text + " - " + ds_zona.Tables(0).Rows(0).Item("Referencia")
      Catch ex As Exception

      End Try

      fila("Codigo") = TxtCodigo.Text
      fila("unacifra") = txtImporte1.Text
      fila("doscifras") = txtImporte2.Text
      fila("trescifras") = txtImporte3.Text
      fila("cuatrocifras") = txtImporte4.Text
      fila("Fecha") = HF_fecha.Value
      fila("totalrecaudado") = Session("TotalRecaudado")

      Dim Dia_descripcion As String = ""
      Select Case HF_dia_id.Value
        Case 1
          Dia_descripcion = "DOMINGO."
        Case 2
          Dia_descripcion = "LUNES."
        Case 3
          Dia_descripcion = "MARTES."
        Case 4
          Dia_descripcion = "MIERCOLES."
        Case 5
          Dia_descripcion = "JUEVES."
        Case 6
          Dia_descripcion = "VIERNES."
        Case 7
          Dia_descripcion = "SABADO."
      End Select

      fila("Dia") = Dia_descripcion
      DS_Consultas.Tables("CodigoPremiado_reporte").Rows.Add(fila)


      Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
      CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
      CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/CodigoPremiado_informe01.rpt"))
      CrReport.Database.Tables("CodigoPremiado_reporte").SetDataSource(DS_Consultas.Tables("CodigoPremiado_reporte"))
      CrReport.Database.Tables("ZonasHabilitadas_importes").SetDataSource(DS_Consultas.Tables("ZonasHabilitadas_importes"))
      CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/CodigoPremiado.pdf"))


    End If

  End Sub

  Private Sub buscar_zona(ByRef DS_Consultas As DataSet, ByVal ds_zonas As DataSet, ByVal codigoZ As String, ByVal indice As Integer, ByVal item_zona As String)
    Dim i As Integer = 0
    While i < ds_zonas.Tables(0).Rows.Count
      If codigoZ = ds_zonas.Tables(0).Rows(i).Item("Codigo") Then
        DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(indice).Item(item_zona) = ds_zonas.Tables(0).Rows(i).Item("Referencia")
        Exit While
      End If
      i = i + 1
    End While
  End Sub




  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/CodigoMasPremiado_b.aspx")
  End Sub
End Class
