Public Class Listados_EntradasSalidas
  Inherits System.Web.UI.Page
  Dim DAListados As New Capa_Datos.Listados
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()

      Txt_FechaDesde.Focus()

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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 2 or null

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
            If (Menu = "I" And Opcion = "") Or (Menu = "I" And Opcion = "3") Then
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

  Private Sub Txt_FechaDesde_Init(sender As Object, e As EventArgs) Handles Txt_FechaDesde.Init
    Txt_FechaDesde.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_FechaHasta_Init(sender As Object, e As EventArgs) Handles Txt_FechaHasta.Init
    Txt_FechaHasta.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_GrupoCodigo_Init(sender As Object, e As EventArgs) Handles Txt_GrupoCodigo.Init
    Txt_GrupoCodigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick
    Dim valido = "si"
    Try
      Txt_FechaDesde.Text = CDate(Txt_FechaDesde.Text)
      Dim fecha_base As Date = CDate("01/01/1900")
      If Txt_FechaDesde.Text < fecha_base Then
        valido = "no"
      End If
    Catch ex As Exception
      valido = "no"
    End Try

    Try
      Txt_FechaHasta.Text = CDate(Txt_FechaHasta.Text)

    Catch ex As Exception
      valido = "no"
    End Try

    If valido = "si" Then 'si hasta etapa estan bien las fechas, ahora controlo que el intervalo de fechas sea correcto
      If CDate(Txt_FechaHasta.Text) < CDate(Txt_FechaDesde.Text) Then
        valido = "no"
      End If
    End If

    If valido = "si" Then
      If (Txt_GrupoCodigo.Text = "999") Then
        'RECUPERAR TODOS LOS GRUPOS
        Dim ds_EntSalida As DataSet = DAListados.EntradaSalida_BuscarRangoFechasTodo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text))
        REPORTE(ds_EntSalida)
      Else
        'RECUPERAR SOLO 1 GRUPO
        Dim ds_EntSalida As DataSet = DAListados.EntradaSalida_BuscarRangoFechasUno(Txt_GrupoCodigo.Text, CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text))
        REPORTE(ds_EntSalida)


      End If

    Else
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error2", "$(document).ready(function () {$('#modal-ok_error2').modal();});", True)
    End If

  End Sub

  Private Sub Agregar_Entrada(ByRef DS_Listados As DataSet, ByVal ds_EntSalida As DataSet, ByVal Cliente_codigo As Integer, ByVal Grupo_nombre As String)
    'primero valido que no exista el Cliente_codigo en la tabla ES_Entradas
    Dim valido = "si"
    Dim filtro As String = "cliente = " + CStr(Cliente_codigo)
    Dim rows() As DataRow = DS_Listados.Tables("ES_Entradas").Select(filtro, "importe ASC")
    If rows.Count <> 0 Then
      valido = "no"
    End If

    If valido = "si" Then
      Dim i As Integer = 0
      Dim importe As Decimal = 0
      Dim codigo As Integer = 0

      While i < ds_EntSalida.Tables(0).Rows.Count
        If Cliente_codigo = CInt(ds_EntSalida.Tables(0).Rows(i).Item("Cliente_codigo")) Then
          codigo = CInt(ds_EntSalida.Tables(0).Rows(i).Item("Grupo_codigo"))
          importe = importe + CDec(ds_EntSalida.Tables(0).Rows(i).Item("Cobros"))
          importe = (Math.Round(importe, 2).ToString("N2"))
        End If
        i = i + 1
      End While
      If importe <> CDec(0) Then
        'lo agrego en ES_Entradas
        Dim fila As DataRow = DS_Listados.Tables("ES_Entradas").NewRow
        fila("codigo") = codigo
        fila("cliente") = Cliente_codigo
        fila("detalle") = "PAGO"
        fila("importe") = importe
        DS_Listados.Tables("ES_Entradas").Rows.Add(fila)
        Agregar_Grupos(DS_Listados, codigo, Grupo_nombre)
      End If
    End If


  End Sub

  Private Sub Agregar_Pagos(ByRef DS_Listados As DataSet, ByVal ds_EntSalida As DataSet, ByVal Cliente_codigo As Integer, ByVal Grupo_nombre As String)
    'primero valido que no exista el Cliente_codigo en la tabla ES_Pagos
    Dim valido = "si"
    Dim filtro As String = "cliente = " + CStr(Cliente_codigo)
    Dim rows() As DataRow = DS_Listados.Tables("ES_Pagos").Select(filtro, "importe ASC")
    If rows.Count <> 0 Then
      valido = "no"
    End If

    If valido = "si" Then
      Dim i As Integer = 0
      Dim importe As Decimal = 0
      Dim codigo As Integer = 0

      While i < ds_EntSalida.Tables(0).Rows.Count
        If Cliente_codigo = CInt(ds_EntSalida.Tables(0).Rows(i).Item("Cliente_codigo")) Then
          codigo = CInt(ds_EntSalida.Tables(0).Rows(i).Item("Grupo_codigo"))
          Try
            importe = importe + CDec(ds_EntSalida.Tables(0).Rows(i).Item("Salida"))
          Catch ex As Exception
          End Try
          importe = (Math.Round(importe, 2).ToString("N2"))
        End If
        i = i + 1
      End While
      If importe <> CDec(0) Then
        'lo agrego en ES_Entradas
        Dim fila As DataRow = DS_Listados.Tables("ES_Pagos").NewRow
        fila("codigo") = codigo
        fila("cliente") = Cliente_codigo
        fila("detalle") = "PAGUE"
        fila("importe") = importe
        DS_Listados.Tables("ES_Pagos").Rows.Add(fila)
        Agregar_Grupos(DS_Listados, codigo, Grupo_nombre)
      End If
    End If
  End Sub

  Private Sub Agregar_Gastos(ByRef DS_Listados As DataSet, ByVal ds_EntSalida As DataSet, ByVal detalle As String, ByVal Grupo_codigo As Integer, ByVal Grupo_nombre As String)
    'primero valido que no exista el Cliente_codigo en la tabla ES_Gastos
    Dim valido = "si"
    Dim i As Integer = 0
    While i < DS_Listados.Tables("ES_Gastos").Rows.Count
      If (Grupo_codigo = CInt(DS_Listados.Tables("ES_Gastos").Rows(i).Item("codigo"))) And (detalle = DS_Listados.Tables("ES_Gastos").Rows(i).Item("detalle")) Then
        valido = "no"
        Exit While
      End If
      i = i + 1
    End While
    If valido = "si" Then
      i = 0
      Dim importe As Decimal = 0
      While i < ds_EntSalida.Tables(1).Rows.Count
        If (Grupo_codigo = CInt(ds_EntSalida.Tables(1).Rows(i).Item("Codigo"))) And (detalle = ds_EntSalida.Tables(1).Rows(i).Item("Motivo")) Then
          importe = importe + CDec(ds_EntSalida.Tables(1).Rows(i).Item("Importe"))
          importe = (Math.Round(importe, 2).ToString("N2"))
        End If
        i = i + 1
      End While
      If importe <> CDec(0) Then
        'lo agrego en ES_Entradas
        Dim fila As DataRow = DS_Listados.Tables("ES_Gastos").NewRow
        fila("codigo") = Grupo_codigo

        fila("detalle") = detalle
        fila("importe") = importe
        DS_Listados.Tables("ES_Gastos").Rows.Add(fila)
        Agregar_Grupos(DS_Listados, Grupo_codigo, Grupo_nombre)
      End If
    End If
  End Sub

  Private Sub Agregar_Grupos(ByRef DS_Listados As DataSet, ByVal Grupo_codigo As Integer, ByVal Grupo_nombre As String)
    Dim valido = "si"
    Dim filtro As String = "codigo = " + CStr(Grupo_codigo)
    Dim rows() As DataRow = DS_Listados.Tables("ES_Grupos").Select(filtro, "SaldoDia ASC")
    If rows.Count <> 0 Then
      valido = "no"
    End If

    If valido = "si" Then
      Dim fila As DataRow = DS_Listados.Tables("ES_Grupos").NewRow
      fila("codigo") = Grupo_codigo
      fila("fecha_desde") = CDate(Txt_FechaDesde.Text)
      fila("fecha_hasta") = CDate(Txt_FechaHasta.Text)
      fila("Grupo_nombre") = Grupo_nombre
      fila("cont_E") = CInt(0)
      fila("cont_P") = CInt(0)
      fila("cont_G") = CInt(0)
      DS_Listados.Tables("ES_Grupos").Rows.Add(fila)
    End If
  End Sub


  Private Sub REPORTE(ByRef DS_EntSalida As DataSet)
    Dim DS_Listados As New DS_Listados
    'EntradaSalida_BuscarRangoFechas


    If ds_EntSalida.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_EntSalida.Tables(0).Rows.Count
        Dim Grupo_codigo As Integer = CInt(ds_EntSalida.Tables(0).Rows(i).Item("Grupo_codigo"))
        Dim Grupo_nombre As String = ds_EntSalida.Tables(0).Rows(i).Item("Grupo_nombre")
        Dim Cliente_codigo As String = CInt(ds_EntSalida.Tables(0).Rows(i).Item("Cliente_codigo"))

        Dim filtro As String = "cliente = " + CStr(Cliente_codigo)
        Dim rows() As DataRow = DS_Listados.Tables("ES_Entradas").Select(filtro, "importe ASC")
        If rows.Count = 0 Then
          Agregar_Entrada(DS_Listados, ds_EntSalida, Cliente_codigo, Grupo_nombre)
        End If

        Dim filtro2 As String = "cliente = " + CStr(Cliente_codigo)
        Dim rows2() As DataRow = DS_Listados.Tables("ES_Pagos").Select(filtro, "importe ASC")
        If rows2.Count = 0 Then
          Agregar_Pagos(DS_Listados, ds_EntSalida, Cliente_codigo, Grupo_nombre)
        End If
        i = i + 1
      End While
    End If

    If ds_EntSalida.Tables(1).Rows.Count Then
      Dim j As Integer = 0
      While j < ds_EntSalida.Tables(1).Rows.Count
        Dim Grupo_codigo As Integer = CInt(ds_EntSalida.Tables(1).Rows(j).Item("Codigo"))
        Dim Grupo_nombre As String = ds_EntSalida.Tables(1).Rows(j).Item("Grupo_nombre")
        Dim detalle As String = ds_EntSalida.Tables(1).Rows(j).Item("Motivo")

        Agregar_Gastos(DS_Listados, ds_EntSalida, detalle, Grupo_codigo, Grupo_nombre)

        j = j + 1
      End While
    End If

    'ahora vienen los calculos para ES_Grupos
    Dim k As Integer = 0
    While k < DS_Listados.Tables("ES_Grupos").Rows.Count
      Dim grupo_codigo As Integer = DS_Listados.Tables("ES_Grupos").Rows(k).Item("codigo")

      '*****************************************************************************************
      Dim Subtotal_Entradas As Decimal = 0
      Dim filtro_E As String = "codigo = " + CStr(grupo_codigo)
      Dim rows_E() As DataRow = DS_Listados.Tables("ES_Entradas").Select(filtro_E, "importe ASC")
      Dim kk As Integer = 0
      While kk < rows_E.Count
        Subtotal_Entradas = Subtotal_Entradas + CDec(rows_E(kk).Item("importe"))
        Subtotal_Entradas = (Math.Round(Subtotal_Entradas, 2).ToString("N2"))
        DS_Listados.Tables("ES_Grupos").Rows(k).Item("cont_E") = CInt(1)
        kk = kk + 1
      End While
      '*****************************************************************************************
      Dim Subtotal_Pagos As Decimal = 0
      Dim filtro_P As String = "codigo = " + CStr(grupo_codigo)
      Dim rows_P() As DataRow = DS_Listados.Tables("ES_Pagos").Select(filtro_P, "importe ASC")
      kk = 0
      While kk < rows_P.Count
        Subtotal_Pagos = Subtotal_Pagos + CDec(rows_P(kk).Item("importe"))
        Subtotal_Pagos = (Math.Round(Subtotal_Pagos, 2).ToString("N2"))
        DS_Listados.Tables("ES_Grupos").Rows(k).Item("cont_P") = CInt(1)
        kk = kk + 1
      End While
      '*****************************************************************************************
      Dim Subtotal_Gastos As Decimal = 0
      Dim filtro_G As String = "codigo = " + CStr(grupo_codigo)
      Dim rows_G() As DataRow = DS_Listados.Tables("ES_Gastos").Select(filtro_G, "importe ASC")
      kk = 0
      While kk < rows_G.Count
        Subtotal_Gastos = Subtotal_Gastos + CDec(rows_G(kk).Item("importe"))
        Subtotal_Gastos = (Math.Round(Subtotal_Gastos, 2).ToString("N2"))
        DS_Listados.Tables("ES_Grupos").Rows(k).Item("cont_G") = CInt(1)
        kk = kk + 1
      End While

      Dim SaldoDia As Decimal = Subtotal_Entradas - Subtotal_Pagos - Subtotal_Gastos
      SaldoDia = (Math.Round(SaldoDia, 2).ToString("N2"))
      DS_Listados.Tables("ES_Grupos").Rows(k).Item("SaldoDia") = SaldoDia

      If SaldoDia < CDec(0) Then
        DS_Listados.Tables("ES_Grupos").Rows(k).Item("SaldoDia_desc") = "SALDO DIA GANA"
      End If
      If SaldoDia > CDec(0) Then
        DS_Listados.Tables("ES_Grupos").Rows(k).Item("SaldoDia_desc") = "SALDO DIA DEJA "
      End If
      k = k + 1
    End While

    If DS_Listados.Tables("ES_Grupos").Rows.Count <> 0 Then
      'report
      Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
      CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
      CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/ListadoEntradaSalida_informe01.rpt"))
      CrReport.Database.Tables("ES_Grupos").SetDataSource(DS_Listados.Tables("ES_Grupos"))
      CrReport.Database.Tables("ES_Entradas").SetDataSource(DS_Listados.Tables("ES_Entradas"))
      CrReport.Database.Tables("ES_Pagos").SetDataSource(DS_Listados.Tables("ES_Pagos"))
      CrReport.Database.Tables("ES_Gastos").SetDataSource(DS_Listados.Tables("ES_Gastos"))


      CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/ListadoEntradaSalida.pdf"))
      CrReport.Dispose() 'esto para no sobrecargar crystal y generar desbordamientos
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
    Else
      'mensaje la busqueda no arrojo resultados.
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If



    ''esto es una prueba
    'Dim DS_Listados As New DS_Listados
    'Dim fila_1 As DataRow = DS_Listados.Tables("ES_Grupos").NewRow
    'fila_1("codigo") = CInt(1)
    'fila_1("fecha_desde") = Today
    'fila_1("fecha_hasta") = Today.AddDays(1)
    'fila_1("SaldoDia_desc") = "SALDO DIA GANA"
    'fila_1("SaldoDia") = CDec(-5135)
    'fila_1("Grupo_nombre") = "NOMBRE1"
    'fila_1("cont_E") = 1
    'fila_1("cont_P") = 0
    'fila_1("cont_G") = 0
    'DS_Listados.Tables("ES_Grupos").Rows.Add(fila_1)
    ''********************************************************************************
    'Dim fila_b As DataRow = DS_Listados.Tables("ES_Entradas").NewRow
    'fila_b("codigo") = CInt(1)
    'fila_b("cliente") = CInt(1)
    'fila_b("detalle") = "PAGO"
    'fila_b("importe") = CDec(500)
    'DS_Listados.Tables("ES_Entradas").Rows.Add(fila_b)

    'Dim fila_bb As DataRow = DS_Listados.Tables("ES_Entradas").NewRow
    'fila_bb("codigo") = CInt(1)
    'fila_bb("cliente") = CInt(2)
    'fila_bb("detalle") = "PAGO"
    'fila_bb("importe") = CDec(1300)
    'DS_Listados.Tables("ES_Entradas").Rows.Add(fila_bb)
    ''********************************************************************************
    'Dim fila_c As DataRow = DS_Listados.Tables("ES_Pagos").NewRow
    'fila_c("codigo") = CInt(1)
    'fila_c("cliente") = CInt(5)
    'fila_c("detalle") = "PAGUE"
    'fila_c("importe") = CDec(325)
    'DS_Listados.Tables("ES_Pagos").Rows.Add(fila_c)

    'Dim fila_cc As DataRow = DS_Listados.Tables("ES_Pagos").NewRow
    'fila_cc("codigo") = CInt(1)
    'fila_cc("cliente") = CInt(13)
    'fila_cc("detalle") = "PAGUE"
    'fila_cc("importe") = CDec(1110)
    'DS_Listados.Tables("ES_Pagos").Rows.Add(fila_cc)
    ''********************************************************************************
    'Dim fila_d As DataRow = DS_Listados.Tables("ES_Gastos").NewRow
    'fila_d("codigo") = CInt(1)
    'fila_d("detalle") = "NAFTA"
    'fila_d("importe") = CDec(3000)
    'DS_Listados.Tables("ES_Gastos").Rows.Add(fila_d)

    'Dim fila_dd As DataRow = DS_Listados.Tables("ES_Gastos").NewRow
    'fila_dd("codigo") = CInt(1)
    'fila_dd("detalle") = "LUZ"
    'fila_dd("importe") = CDec(2500)
    'DS_Listados.Tables("ES_Gastos").Rows.Add(fila_dd)

    ''********************************************************************************



    ''SEGUNDO GRUPO
    'Dim fila_2 As DataRow = DS_Listados.Tables("ES_Grupos").NewRow
    'fila_2("codigo") = CInt(2)
    'fila_2("fecha_desde") = Today
    'fila_2("fecha_hasta") = Today.AddDays(1)
    'fila_2("SaldoDia_desc") = "SALDO DIA GANA"
    'fila_2("SaldoDia") = CDec(-5135)
    'fila_2("Grupo_nombre") = "NOMBRE2"
    'fila_2("cont_E") = 1
    'fila_2("cont_P") = 1
    'fila_2("cont_G") = 1
    'DS_Listados.Tables("ES_Grupos").Rows.Add(fila_2)
    ''********************************************************************************
    'Dim fila_2b As DataRow = DS_Listados.Tables("ES_Entradas").NewRow
    'fila_2b("codigo") = CInt(2)
    'fila_2b("cliente") = CInt(3)
    'fila_2b("detalle") = "PAGO"
    'fila_2b("importe") = CDec(500)
    'DS_Listados.Tables("ES_Entradas").Rows.Add(fila_2b)

    'Dim fila_2bb As DataRow = DS_Listados.Tables("ES_Entradas").NewRow
    'fila_2bb("codigo") = CInt(2)
    'fila_2bb("cliente") = CInt(4)
    'fila_2bb("detalle") = "PAGO"
    'fila_2bb("importe") = CDec(1300)
    'DS_Listados.Tables("ES_Entradas").Rows.Add(fila_2bb)
    ''********************************************************************************
    ''Dim fila_2c As DataRow = DS_Listados.Tables("ES_Pagos").NewRow
    ''fila_2c("codigo") = CInt(2)
    ''fila_2c("cliente") = CInt(6)
    ''fila_2c("detalle") = "PAGUE"
    ''fila_2c("importe") = CDec(325)
    ''DS_Listados.Tables("ES_Pagos").Rows.Add(fila_2c)

    ''Dim fila_2cc As DataRow = DS_Listados.Tables("ES_Pagos").NewRow
    ''fila_2cc("codigo") = CInt(2)
    ''fila_2cc("cliente") = CInt(14)
    ''fila_2cc("detalle") = "PAGUE"
    ''fila_2cc("importe") = CDec(1110)
    ''DS_Listados.Tables("ES_Pagos").Rows.Add(fila_2cc)
    ''********************************************************************************
    'Dim fila_2d As DataRow = DS_Listados.Tables("ES_Gastos").NewRow
    'fila_2d("codigo") = CInt(2)
    'fila_2d("detalle") = "NAFTITA"
    'fila_2d("importe") = CDec(3000)
    'DS_Listados.Tables("ES_Gastos").Rows.Add(fila_2d)

    'Dim fila_2dd As DataRow = DS_Listados.Tables("ES_Gastos").NewRow
    'fila_2dd("codigo") = CInt(2)
    'fila_2dd("detalle") = "LUCECITA"
    'fila_2dd("importe") = CDec(2500)
    'DS_Listados.Tables("ES_Gastos").Rows.Add(fila_2dd)
    ''********************************************************************************




  End Sub

  Private Sub REPORTE_TODO()

  End Sub

End Class
