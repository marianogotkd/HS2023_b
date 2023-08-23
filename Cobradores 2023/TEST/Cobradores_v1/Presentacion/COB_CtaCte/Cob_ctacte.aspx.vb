Public Class Cob_ctacte
  Inherits System.Web.UI.Page

  Dim daTarifa As New Capa_Datos.Tarifa
  Dim daCtaCte As New Capa_Datos.CtaCte

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos

      Obtener_info()


    End If

  End Sub

  Private Sub Obtener_info()
    'necesito que me pasen por session CLIE_ID Y CTACTE_ID




    Dim DS_ctactehistorial As New DS_ctactehistorial
    DS_ctactehistorial.Tables("LOCALES").Rows.Clear()

    HF_CLIE_ID.Value = Session("CLIE_ID")
    HF_CTACTE_ID.VALUE = Session("CTACTE_ID")
    'HF_LOCAL_ID.Value = Session("LOCAL_ID")


    Dim DS_info As DataSet = daCtaCte.CtaCte_historial(HF_CLIE_ID.Value, HF_CTACTE_ID.Value)

    'Dim DS_info As DataSet = daCtaCte.CtaCte_historial(7, 7)


    Lb_ctacte.Text = "CTACTE: " + CStr(DS_info.Tables(0).Rows(0).Item("CTACTE_ID")) + "."
    Lb_cliente.Text = "CLIENTE: " + DS_info.Tables(0).Rows(0).Item("CLIE_ape") + ", " + DS_info.Tables(0).Rows(0).Item("CLIE_nom") + "."
    Lb_ctactesaldo.Text = "SALDO DEUDOR: $" + CStr(DS_info.Tables(0).Rows(0).Item("CTACTE_saldodeudor"))

    Dim i As Integer = 0
    While i < DS_info.Tables(1).Rows.Count
      Dim fila As DataRow = DS_ctactehistorial.Tables("LOCALES").NewRow
      fila("codlocal") = "COD.LOCAL: " + CStr(DS_info.Tables(1).Rows(i).Item("LOCAL_codigo")) + "."
      fila("local") = "LOCAL: " + DS_info.Tables(1).Rows(i).Item("SECTOR_desc") + ", " + DS_info.Tables(1).Rows(i).Item("PASILLO_desc") + ", " + DS_info.Tables(1).Rows(i).Item("LOCAL_desc") + "."
      DS_ctactehistorial.Tables("LOCALES").Rows.Add(fila)
      i = i + 1
    End While
    If DS_ctactehistorial.Tables("LOCALES").Rows.Count <> 0 Then
      seccion_locales.Visible = True
      GridViewLocales.DataSource = DS_ctactehistorial.Tables("LOCALES")
      GridViewLocales.DataBind()
    End If

    i = 0
    While i < DS_info.Tables(2).Rows.Count
      Dim fila As DataRow = DS_ctactehistorial.Tables("MOVIMIENTOS").NewRow
      fila("TARCLIE_ID") = DS_info.Tables(2).Rows(i).Item("TARCLIE_ID")
      fila("Fecha") = DS_info.Tables(2).Rows(i).Item("Fecha")
      fila("TARCLIE_desc") = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
      If DS_info.Tables(2).Rows(i).Item("tipo") = "debe" Then
        fila("Debe") = DS_info.Tables(2).Rows(i).Item("importe")
      Else
        fila("Haber") = DS_info.Tables(2).Rows(i).Item("importe")
      End If
      fila("comprobante") = DS_info.Tables(2).Rows(i).Item("comprobante")
      fila("CTACTEDET_ID") = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
      fila("LOCAL_codigo") = DS_info.Tables(2).Rows(i).Item("LOCAL_codigo")
      DS_ctactehistorial.Tables("MOVIMIENTOS").Rows.Add(fila)
      i = i + 1
    End While



    If DS_ctactehistorial.Tables("MOVIMIENTOS").Rows.Count <> 0 Then
      GridView1.DataSource = DS_ctactehistorial.Tables("MOVIMIENTOS")
      GridView1.DataBind()
      GridView1.Columns(0).Visible = False '0 1 7
      GridView1.Columns(1).Visible = False '0 1 7
      GridView1.Columns(7).Visible = False '0 1 7

      secciongrid.Visible = True

    End If




    'GridView1.DataSource = DS_info.Tables(2)
    'GridView1.DataBind()
    'GridView1.Columns(1).Visible = False   '0 es columna TARCLIE_ID


    'esto hago si agrupo las tarifas

    'If DS_info.Tables(3).Rows.Count <> 0 Then

    '  Dim TableCopy As DataTable = DS_info.Tables(3).Copy 'copia de tabla y registros.

    '  Dim aux(0 To DS_info.Tables(3).Rows.Count)
    '  Dim add As Integer = 0
    '  Dim i As Integer = 0
    '  While i < DS_info.Tables(3).Rows.Count
    '    Dim TARCLIE_ID As Integer = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '    Dim existe = "no"
    '    Dim j As Integer = 0
    '    While j < aux.Count
    '      If aux(j) = TARCLIE_ID Then
    '        existe = "si"
    '        If i <> j Then
    '          DS_info.Tables(3).Rows.RemoveAt(i)
    '        End If
    '        i = 0
    '        Exit While
    '      End If
    '      j = j + 1
    '    End While
    '    If existe = "no" Then
    '      aux(add) = TARCLIE_ID
    '    End If
    '    i = i + 1
    '  End While

    '  i = 0

    '  If DS_info.Tables(3).Rows.Count <= 10 Then
    '    While i < DS_info.Tables(3).Rows.Count
    '      Select Case i
    '        Case 0
    '          Seccion01.Visible = True
    '          Lb_anular01.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF01_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa01info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo01info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")

    '          Dim fecha As String = ""
    '          fechas_obtener(HF01_TARCLIE_ID.Value, TableCopy, fecha)

    '          Lb_fecha01info.Text = fecha


    '        Case 1
    '          Seccion02.Visible = True
    '          Lb_anular02.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF02_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa02info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo02info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha02info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF02_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha02info.Text = fecha

    '        Case 2
    '          Seccion03.Visible = True
    '          Lb_anular03.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF03_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa03info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo03info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha03info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF03_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha03info.Text = fecha
    '        Case 3
    '          Seccion04.Visible = True
    '          Lb_anular04.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF04_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa04info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo04info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha04info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF04_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha04info.Text = fecha
    '        Case 4
    '          Seccion05.Visible = True
    '          Lb_anular05.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF05_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa05info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo05info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha05info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF05_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha05info.Text = fecha
    '        Case 5
    '          Seccion06.Visible = True
    '          Lb_anular06.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF06_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa06info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo06info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha06info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF06_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha06info.Text = fecha
    '        Case 6
    '          Seccion07.Visible = True
    '          Lb_anular07.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF07_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa07info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo07info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha07info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF07_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha07info.Text = fecha
    '        Case 7
    '          Seccion08.Visible = True
    '          Lb_anular08.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF08_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa08info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo08info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha08info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF08_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha08info.Text = fecha
    '        Case 8
    '          Seccion09.Visible = True
    '          Lb_anular09.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF09_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa09info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo09info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha09info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF09_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha09info.Text = fecha
    '        Case 9
    '          Seccion10.Visible = True
    '          Lb_anular10.Text = "Anular item " + CStr(i + 1)
    '          'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '          HF10_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
    '          Lb_tarifa10info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
    '          Lb_saldo10info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
    '          'Lb_fecha10info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
    '          Dim fecha As String = ""
    '          fechas_obtener(HF10_TARCLIE_ID.Value, TableCopy, fecha)
    '          Lb_fecha10info.Text = fecha
    '      End Select
    '      i = i + 1
    '    End While
    '  Else
    '    '  'aqui mensaje "error, consulte al administrador."
    '    '  'modal_error_carga
    '    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_error_carga", "$(document).ready(function () {$('#modal_error_carga').modal();});", True)
    '  End If
    'End If




    'If DS_info.Tables(3).Rows.Count <= 10 Then

    '  Dim i As Integer = 0
    '  While i < DS_info.Tables(3).Rows.Count
    '    Select Case i
    '      Case 0
    '        'aqui pongo en visible el div para el primer cobro.
    '        Seccion01.Visible = True
    '        Lb_anular01.Text = "Anular item " + CStr(i + 1)
    '        Dim J As Integer = 0
    '        Dim CadenaFecha As String = ""
    '        While J < DS_info.Tables(2).Rows.Count
    '          If J = 0 Then
    '            HF01_TARCLIE_ID.Value = DS_info.Tables(2).Rows(J).Item("TARCLIE_ID")
    '            Lb_tarifa01info.Text = DS_info.Tables(2).Rows(J).Item("TARCLIE_desc")

    '          End If


    '          J = J + 1
    '        End While




    '        'HF01_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")


    '        Lb_fecha01info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")

    '        Lb_saldo01info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 1
    '        Seccion02.Visible = True
    '        HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular02.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha02info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa02info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo02info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 2
    '        Seccion03.Visible = True
    '        HF03_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular03.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha03info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa03info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo03info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 3
    '        Seccion04.Visible = True
    '        HF04_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular04.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha04info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa04info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo04info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 4
    '        Seccion05.Visible = True

    '        Lb_anular05.Text = "Anular item " + CStr(i + 1)
    '        HF05_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_fecha05info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa05info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo05info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 5
    '        Seccion06.Visible = True
    '        HF06_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular06.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha06info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa06info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo06info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 6
    '        Seccion07.Visible = True
    '        HF07_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular07.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha07info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa07info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo07info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 7
    '        Seccion08.Visible = True
    '        HF08_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular08.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha08info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa08info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo08info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 8
    '        Seccion09.Visible = True
    '        HF09_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular09.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha09info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa09info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo09info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 9
    '        Seccion10.Visible = True
    '        HF10_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular10.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha10info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa10info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo10info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '    End Select
    '    i = i + 1
    '  End While
    'Else
    '  'aqui mensaje "error, consulte al administrador."
    '  'modal_error_carga
    '  ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_error_carga", "$(document).ready(function () {$('#modal_error_carga').modal();});", True)
    'End If








    ''esto funciona si hago un div por cada tarifa, sin agrupar

    'If DS_info.Tables(2).Rows.Count <= 10 Then

    '  Dim i As Integer = 0
    '  While i < DS_info.Tables(2).Rows.Count
    '    Select Case i
    '      Case 0
    '        'aqui pongo en visible el div para el primer cobro.
    '        Seccion01.Visible = True
    '        HF01_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular01.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha01info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa01info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo01info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 1
    '        Seccion02.Visible = True
    '        HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular02.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha02info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa02info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo02info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 2
    '        Seccion03.Visible = True
    '        HF03_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular03.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha03info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa03info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo03info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 3
    '        Seccion04.Visible = True
    '        HF04_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular04.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha04info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa04info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo04info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 4
    '        Seccion05.Visible = True

    '        Lb_anular05.Text = "Anular item " + CStr(i + 1)
    '        HF05_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_fecha05info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa05info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo05info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 5
    '        Seccion06.Visible = True
    '        HF06_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular06.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha06info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa06info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo06info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 6
    '        Seccion07.Visible = True
    '        HF07_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular07.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha07info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa07info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo07info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 7
    '        Seccion08.Visible = True
    '        HF08_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular08.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha08info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa08info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo08info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 8
    '        Seccion09.Visible = True
    '        HF09_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular09.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha09info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa09info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo09info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '      Case 9
    '        Seccion10.Visible = True
    '        HF10_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
    '        Lb_anular10.Text = "Anular item " + CStr(i + 1)
    '        Lb_fecha10info.Text = DS_info.Tables(2).Rows(i).Item("Fecha")
    '        Lb_tarifa10info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_desc")
    '        Lb_saldo10info.Text = DS_info.Tables(2).Rows(i).Item("TARCLIE_precio")
    '    End Select
    '    i = i + 1
    '  End While
    'Else
    '  'aqui mensaje "error, consulte al administrador."
    '  'modal_error_carga
    '  ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_error_carga", "$(document).ready(function () {$('#modal_error_carga').modal();});", True)
    'End If


  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("Clientes_OP") = "MODIFICAR"
    Response.Redirect("~/COB_Clientes/Cob_clientesAlta.aspx")
  End Sub
End Class
