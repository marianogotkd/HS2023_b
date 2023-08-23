Public Class CodigosMasCargados_consulta
  Inherits System.Web.UI.Page

  Dim Daparametro As New Capa_Datos.WC_parametro
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      permisos
      HF_fecha.Value = Session("fecha_parametro")
      HF_dia_id.Value = Session("dia")

      Dim ds_fecha As DataSet = Daparametro.Parametro_obtener_dia
      If ds_fecha.Tables(0).Rows.Count <> 0 Then
        Dim FECHA As Date = CDate(ds_fecha.Tables(0).Rows(0).Item("Fecha"))
        'txt_fecha.Text = FECHA.ToString("yyyy-MM-dd")

        Txt_fecha.Text = FECHA.ToString("yyyy-MM-dd")
        Dim dia As Integer = Session("dia")
        Select Case dia
          Case 1
            Label_dia.Text = "DIA: DOMINGO."
          Case 2
            Label_dia.Text = "DIA: LUNES."
          Case 3
            Label_dia.Text = "DIA: MARTES."
          Case 4
            Label_dia.Text = "DIA: MIERCOLES."
          Case 5
            Label_dia.Text = "DIA: JUEVES."
          Case 6
            Label_dia.Text = "DIA: VIERNES."
          Case 7
            Label_dia.Text = "DIA: SABADO."
        End Select
      End If

      txtClienteDesde.Text = Session("cliente_desde")
      txtClienteHasta.Text = Session("cliente_hasta")
      txtImporte1.Text = Session("importe1")
      txtImporte2.Text = Session("importe2")
      txtImporte3.Text = Session("importe3")
      txtImporte4.Text = Session("importe4")


      Txt_fecha.ReadOnly = True

      txtClienteDesde.ReadOnly = True
      txtClienteHasta.ReadOnly = True
      txtImporte1.ReadOnly = True
      txtImporte2.ReadOnly = True
      txtImporte3.ReadOnly = True
      txtImporte4.ReadOnly = True


      GridView1.DataSource = Session("tabla_consulta")
      GridView1.DataBind()



      '//////////////////ARMADO DEL REPORTE//////////////
      Dim DS_Consultas As New DS_Consultas
      DS_Consultas.CodigosMasPremiados_reporte.Rows.Clear()
      Dim fila As DataRow = DS_Consultas.CodigosMasPremiados_reporte.NewRow
      fila("Fecha") = HF_fecha.Value
      Select Case HF_dia_id.Value
        Case 1
          fila("Dia") = "DOMINGO."
        Case 2
          fila("Dia") = "LUNES."
        Case 3
          fila("Dia") = "MARTES."
        Case 4
          fila("Dia") = "MIERCOLES."
        Case 5
          fila("Dia") = "JUEVES."
        Case 6
          fila("Dia") = "VIERNES."
        Case 7
          fila("Dia") = "SABADO."
      End Select
      fila("Cliente_desde") = txtClienteDesde.Text
      fila("Cliente_hasta") = txtClienteHasta.Text
      fila("importe1") = txtImporte1.Text
      fila("importe2") = txtImporte2.Text
      fila("importe3") = txtImporte3.Text
      fila("importe4") = txtImporte4.Text
      DS_Consultas.CodigosMasPremiados_reporte.Rows.Add(fila)
      DS_Consultas.CodigoMasPremiado_importes.Merge(Session("tabla_consulta"))

      Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
      CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
      CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/CodigosMasCargados_informe01.rpt"))
      CrReport.Database.Tables("CodigosMasPremiados_reporte").SetDataSource(DS_Consultas.Tables("CodigosMasPremiados_reporte"))
      CrReport.Database.Tables("CodigoMasPremiado_importes").SetDataSource(DS_Consultas.Tables("CodigoMasPremiado_importes"))
      CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/CodigosMasCargados.pdf"))


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
            If (Menu = "5" And Opcion = "") Or (Menu = "5" And Opcion = "1") Then
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
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/CodigoMasPremiado.aspx")

  End Sub
End Class
