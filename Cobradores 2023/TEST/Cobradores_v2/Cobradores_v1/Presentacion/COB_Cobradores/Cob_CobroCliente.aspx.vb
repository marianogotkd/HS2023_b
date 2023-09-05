Public Class Cob_CobroCliente
  Inherits System.Web.UI.Page

  Dim daTarifa As New Capa_Datos.Tarifa
  Dim daCtaCte As New Capa_Datos.CtaCte
  Dim daComprobante As New Capa_Datos.Comprobante
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos

      Obtener_info()


    End If
  End Sub

  Private Sub fechas_obtener(ByVal TARCLIE_ID As Integer, ByVal TableCopy As DataTable, ByRef fecha As String)
    'voy a ciclar para ver hasta donde llega el rango de fechas.
    Dim j As Integer = 0
    Dim fechadesde As String = ""
    Dim fechahasta As String = ""
    While j < TableCopy.Rows.Count
      If TARCLIE_ID = TableCopy.Rows(j).Item("TARCLIE_ID") Then
        If fechadesde = "" Then
          fechadesde = TableCopy.Rows(j).Item("Fecha")
        Else
          fechahasta = TableCopy.Rows(j).Item("Fecha")
        End If
      End If
      j = j + 1
    End While
    If fechahasta <> "" Then
      fecha = fechadesde + "-" + fechahasta
    Else
      fecha = fechadesde
    End If

  End Sub

  Private Sub Obtener_info()
    Dim DS_Cobradores As New DS_Cobradores
    HF_CLIE_ID.Value = Session("CLIE_ID")
    HF_LOCAL_ID.Value = Session("LOCAL_ID")

    Dim DS_info As DataSet = daTarifa.TarifaCliente_obtener(Session("CLIE_ID"), Session("LOCAL_ID"))

    Lb_ctacte.Text = "CTACTE: " + CStr(DS_info.Tables(0).Rows(0).Item("CTACTE_ID")) + "."
    Lb_cliente.Text = "CLIENTE: " + DS_info.Tables(0).Rows(0).Item("CLIE_ape") + ", " + DS_info.Tables(0).Rows(0).Item("CLIE_nom") + "."
    Lb_codlocal.Text = "COD.LOCAL: " + CStr(DS_info.Tables(1).Rows(0).Item("LOCAL_codigo")) + "."
    Lb_local.Text = "LOCAL: " + DS_info.Tables(1).Rows(0).Item("SECTOR_desc") + ", " + DS_info.Tables(1).Rows(0).Item("PASILLO_desc") + ", " + DS_info.Tables(1).Rows(0).Item("LOCAL_desc") + "."
    Lb_ctactesaldo.Text = "SALDO DEUDOR: $" + CStr(DS_info.Tables(0).Rows(0).Item("CTACTE_saldodeudor"))


    GridView1.DataSource = DS_info.Tables(2)
    GridView1.DataBind()
    GridView1.Columns(1).Visible = False   '0 es columna TARCLIE_ID

    'Txt_fecha01.Text = "13/07/2023"
    'Lb_fecha01info.Text = "13/07/2023"
    'Txt_fecha02.Text = "14/07/2023"
    'Lb_fecha02info.Text = "14/07/2023"

    'Txt_tarifa01.Text = "Alquiler diario"
    'Lb_tarifa01info.Text = "Alquiler diario"
    'Txt_tarifa02.Text = "Alquiler diario para todo el publico presente"
    'Lb_tarifa02info.Text = "Alquiler diario para todo el publico presente"


    'Txt_saldo01.Text = "12500,00"
    'Lb_saldo01info.Text = "12500,00"
    'Txt_saldo02.Text = "12500,00"
    'Lb_saldo02info.Text = "12500,00"


    'esto hago si agrupo las tarifas

    If DS_info.Tables(3).Rows.Count <> 0 Then

      Dim TableCopy As DataTable = DS_info.Tables(3).Copy 'copia de tabla y registros.

      Dim aux(0 To DS_info.Tables(3).Rows.Count)
      Dim add As Integer = 0
      Dim i As Integer = 0
      While i < DS_info.Tables(3).Rows.Count
        Dim TARCLIE_ID As Integer = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
        Dim existe = "no"
        Dim j As Integer = 0
        While j < aux.Count
          If aux(j) = TARCLIE_ID Then
            existe = "si"
            If i <> j Then
              DS_info.Tables(3).Rows.RemoveAt(i)
            End If
            i = 0
            Exit While
          End If
          j = j + 1
        End While
        If existe = "no" Then
          aux(add) = TARCLIE_ID
        End If
        i = i + 1
      End While

      i = 0

      If DS_info.Tables(3).Rows.Count <= 10 Then
        While i < DS_info.Tables(3).Rows.Count
          Select Case i
            Case 0
              Seccion01.Visible = True
              Lb_anular01.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF01_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa01info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo01info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")

              Dim fecha As String = ""
              fechas_obtener(HF01_TARCLIE_ID.Value, TableCopy, fecha)

              Lb_fecha01info.Text = fecha


            Case 1
              Seccion02.Visible = True
              Lb_anular02.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF02_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa02info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo02info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha02info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF02_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha02info.Text = fecha

            Case 2
              Seccion03.Visible = True
              Lb_anular03.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF03_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa03info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo03info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha03info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF03_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha03info.Text = fecha
            Case 3
              Seccion04.Visible = True
              Lb_anular04.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF04_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa04info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo04info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha04info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF04_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha04info.Text = fecha
            Case 4
              Seccion05.Visible = True
              Lb_anular05.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF05_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa05info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo05info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha05info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF05_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha05info.Text = fecha
            Case 5
              Seccion06.Visible = True
              Lb_anular06.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF06_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa06info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo06info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha06info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF06_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha06info.Text = fecha
            Case 6
              Seccion07.Visible = True
              Lb_anular07.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF07_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa07info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo07info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha07info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF07_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha07info.Text = fecha
            Case 7
              Seccion08.Visible = True
              Lb_anular08.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF08_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa08info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo08info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha08info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF08_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha08info.Text = fecha
            Case 8
              Seccion09.Visible = True
              Lb_anular09.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF09_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa09info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo09info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha09info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF09_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha09info.Text = fecha
            Case 9
              Seccion10.Visible = True
              Lb_anular10.Text = "Anular item " + CStr(i + 1)
              'HF02_CTACTEDET_ID.Value = DS_info.Tables(2).Rows(i).Item("CTACTEDET_ID")
              HF10_TARCLIE_ID.Value = DS_info.Tables(3).Rows(i).Item("TARCLIE_ID")
              Lb_tarifa10info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_desc")
              Lb_saldo10info.Text = DS_info.Tables(3).Rows(i).Item("TARCLIE_precio")
              'Lb_fecha10info.Text = DS_info.Tables(3).Rows(i).Item("Fecha")
              Dim fecha As String = ""
              fechas_obtener(HF10_TARCLIE_ID.Value, TableCopy, fecha)
              Lb_fecha10info.Text = fecha
          End Select
          i = i + 1
        End While
      Else
        '  'aqui mensaje "error, consulte al administrador."
        '  'modal_error_carga
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_error_carga", "$(document).ready(function () {$('#modal_error_carga').modal();});", True)
      End If
    End If




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
    Response.Redirect("~/COB_Cobradores/Cob_CobroListado.aspx")
  End Sub

  Private Sub Btn_ok_error_carga_ServerClick(sender As Object, e As EventArgs) Handles Btn_ok_error_carga.ServerClick
    Response.Redirect("~/COB_Cobradores/Cob_CobroListado.aspx")
  End Sub

  Private Sub btn_close_error_carga_ServerClick(sender As Object, e As EventArgs) Handles btn_close_error_carga.ServerClick
    Response.Redirect("~/COB_Cobradores/Cob_CobroListado.aspx")
  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick
    'Mdl_cobro
    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_cobro", "$(document).ready(function () {$('#Mdl_cobro').modal();});", True)


  End Sub

  Private Sub cobro(ByVal TARCLIE_ID As Integer, ByVal monto_paga As Decimal, ByVal nroComprobante As String, ByVal tarifainfo As String)
    'OJO VER QUE PASA SI SE ANULA O NO
    'validar que los montos sean menores o iguales a la tarifa que se cobra


    Dim ds_ctactedetalle As DataSet = daCtaCte.CtaCteDetalle_consulta(TARCLIE_ID, HF_CLIE_ID.Value, HF_LOCAL_ID.Value)


    Dim aux_monto_paga As Decimal = monto_paga 'PARA GENERAR LA ALTA EN CTACTEDET
    Dim aux_monto_total_a_descontar As Decimal = CDec(0)
    Dim i As Integer = 0
    Dim CTACTE_ID As Integer = ds_ctactedetalle.Tables(0).Rows(0).Item("CTACTE_ID") 'PARA CONSULTAR EL SALDO GENERAL
    While i < ds_ctactedetalle.Tables(0).Rows.Count
      Dim CTACTEDET_saldodeudor As Decimal = CDec(ds_ctactedetalle.Tables(0).Rows(i).Item("CTACTEDET_saldodeudor"))
      Dim CTACTEDET_ID As Integer = ds_ctactedetalle.Tables(0).Rows(i).Item("CTACTEDET_ID")
      If aux_monto_paga > CDec(0) Then

        Dim nuevo_saldodeudor As Decimal = 0
        If aux_monto_paga <= CTACTEDET_saldodeudor Then
          nuevo_saldodeudor = CTACTEDET_saldodeudor - aux_monto_paga
          daCtaCte.CtaCteDetalle_modifsaldodeudor(CInt(CTACTEDET_ID), nuevo_saldodeudor)


          '######################aqui alta en CTACTEDET###########

          Dim tipo As String = "haber"
          Dim ds_ctactedet As DataSet = daCtaCte.CtaCteDetalle_alta(Today, CTACTE_ID, aux_monto_paga, tipo, nroComprobante, TARCLIE_ID, CDec(0)) 'AQUI PONGO EN SALDODEUDOR LO QUE RESTA PAGAR, PERO NO ES MUY RELEVANTE YA QUE LO Q SE CONSULTA ES EL REGISTRO DONDE TIPO="DEBE", CON FECHA ANTERIOR.
          '#######################################################




          '######################aqui alta en tabla Comprobante#######################
          Dim Concepto_fecha As Date = ds_ctactedetalle.Tables(0).Rows(i).Item("Fecha") 'aqui va la fecha de la tarifa q cobro
          Dim Comprobante_fecha As Date = Today 'aqui va la fecha actual.
          Dim NEW_CTACTEDET_ID As Integer = ds_ctactedet.Tables(0).Rows(0).Item("CTACTEDET_ID")
          daComprobante.Comprobante_alta(nroComprobante, NEW_CTACTEDET_ID, tarifainfo, Concepto_fecha, Comprobante_fecha)
          '###########################################################################


          aux_monto_total_a_descontar = aux_monto_total_a_descontar + aux_monto_paga

          aux_monto_paga = CDec(0)

        Else
          nuevo_saldodeudor = CDec(0)
          aux_monto_paga = aux_monto_paga - CTACTEDET_saldodeudor

          daCtaCte.CtaCteDetalle_modifsaldodeudor(CInt(CTACTEDET_ID), nuevo_saldodeudor)

          '#####################aqui alta en CTACTEDET###########################

          Dim tipo As String = "haber"
          Dim ds_ctactedet As DataSet = daCtaCte.CtaCteDetalle_alta(Today, CTACTE_ID, CTACTEDET_saldodeudor, tipo, nroComprobante, TARCLIE_ID, CDec(0)) 'AQUI PONGO EN SALDODEUDOR LO QUE RESTA PAGAR, PERO NO ES MUY RELEVANTE YA QUE LO Q SE CONSULTA ES EL REGISTRO DONDE TIPO="DEBE", CON FECHA ANTERIOR.
          '######################################################################



          '######################aqui alta en tabla Comprobante#######################
          Dim Concepto_fecha As Date = ds_ctactedetalle.Tables(0).Rows(i).Item("Fecha") 'aqui va la fecha de la tarifa q cobro
          Dim Comprobante_fecha As Date = Today 'aqui va la fecha actual.
          Dim NEW_CTACTEDET_ID As Integer = ds_ctactedet.Tables(0).Rows(0).Item("CTACTEDET_ID")
          daComprobante.Comprobante_alta(nroComprobante, NEW_CTACTEDET_ID, tarifainfo, Concepto_fecha, Comprobante_fecha)
          '###########################################################################


          aux_monto_total_a_descontar = aux_monto_total_a_descontar + CTACTEDET_saldodeudor

        End If

      Else
        Exit While
      End If
      i = i + 1
    End While

    'Dim TARCLIE_ID As Integer = ds_ctactedetalle.Rows(0).Item("TARCLIE_ID") 'PARA GENERAR EL ALTA EN CTACTEDET

    'aqui modifico en CTACTEDET, EL REGISTRO Q TIENE EL "DEBE", ACTUALIZO EL CAMPO SALDODEUDOR



    'consulto tabla CtaCte para obtener saldodeudor
    Dim ds_infoctacte As DataSet = daCtaCte.CtaCte_obtenerID(CTACTE_ID)
    Dim CTACTE_saldodeudor As Decimal = ds_infoctacte.Tables(0).Rows(0).Item("CTACTE_saldodeudor")
    CTACTE_saldodeudor = CTACTE_saldodeudor - aux_monto_total_a_descontar

    'actualizo el saldodeudor en la tabla CtaCte
    daCtaCte.CtaCte_SaldodeudorModif(CTACTE_ID, CTACTE_saldodeudor)
  End Sub


  Private Sub btn_cobro_confirmar_ServerClick(sender As Object, e As EventArgs) Handles btn_cobro_confirmar.ServerClick
    'modal-sm_OKGRABADO

    Dim contcobros As Integer = 0
    Dim valido = "si"


    'validacion inicial:
    If Seccion01.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga01.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo01info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try
    End If

    If Seccion02.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga02.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo02info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try

    End If

    If Seccion03.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga03.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo03info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try

    End If

    If Seccion04.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga04.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo04info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try

    End If

    If Seccion05.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga05.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo05info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try

    End If

    If Seccion06.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga06.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo06info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try

    End If

    If Seccion07.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga07.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo07info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try
    End If

    If Seccion08.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga08.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo08info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try

    End If

    If Seccion09.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga09.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo09info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try

    End If

    If Seccion10.Visible = True Then
      Dim importe As Decimal
      Try
        importe = CDec(Txt_paga10.Text.Replace(".", ","))
        If importe > CDec(Lb_saldo10info.Text) Then
          valido = "no"
        End If
      Catch ex As Exception
        importe = "0"
      End Try

    End If



    If valido = "si" Then
      Dim NroComprobante As Integer = 1

      Dim ds_comprobante As DataSet = daCtaCte.CtaCteDetalle_ObtenerUltimoComprobante()
      Try
        NroComprobante = NroComprobante + CInt(ds_comprobante.Tables(0).Rows(0).Item("CTACTEDET_comprobante"))
      Catch ex As Exception
      End Try


      If Seccion01.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga01.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo01info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF01_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa01info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion02.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga02.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo02info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF02_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa02info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion03.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga03.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo03info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF03_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa03info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion04.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga04.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo04info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF04_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa04info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion05.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga05.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo05info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF05_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa05info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion06.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga06.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo06info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF06_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa06info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion07.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga07.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo07info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF07_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa07info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion08.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga08.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo08info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF08_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa08info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion09.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga09.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo09info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF09_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa09info.Text)
          contcobros = contcobros + 1
        End If
      End If

      If Seccion10.Visible = True Then
        Dim importe As Decimal
        Try
          importe = CDec(Txt_paga10.Text.Replace(".", ","))
          If importe > CDec(Lb_saldo10info.Text) Then
            valido = "no"
          End If
        Catch ex As Exception
          importe = "0"
        End Try

        If (importe <> 0) And (valido = "si") Then
          cobro(CInt(HF10_TARCLIE_ID.Value), importe, CStr(NroComprobante), Lb_tarifa10info.Text)
          contcobros = contcobros + 1
        End If
      End If


      '########PARA CREAR EL REPORTE DEL COMPROBANTE#########
      If NroComprobante <> 0 Then
        Dim ds_comprobanteinfo As DataSet = daComprobante.Comprobante_obtener(CStr(NroComprobante), CStr(Session("CLIE_ID")), CStr(Session("LOCAL_ID")))
        If ds_comprobanteinfo.Tables(2).Rows.Count <> 0 Then


          Dim DS_Cobradores As New DS_Cobradores
          Dim fila1 As DataRow = DS_Cobradores.Tables("Reporte_Comprobante_encabezado").NewRow
          fila1("nro_comprobante") = CStr(NroComprobante)
          fila1("Comprobante_fecha") = Today
          fila1("Cliente") = ds_comprobanteinfo.Tables(0).Rows(0).Item("CLIE_ape") + ", " + ds_comprobanteinfo.Tables(0).Rows(0).Item("CLIE_nom") + "."
          fila1("CtaCte") = CStr(ds_comprobanteinfo.Tables(0).Rows(0).Item("CTACTE_ID")) + "."
          fila1("Local") = ds_comprobanteinfo.Tables(1).Rows(0).Item("SECTOR_desc") + ", " + ds_comprobanteinfo.Tables(1).Rows(0).Item("PASILLO_desc") + ", " + ds_comprobanteinfo.Tables(1).Rows(0).Item("LOCAL_desc") + "."
          'Lb_codlocal.Text = "COD.LOCAL: " + CStr(DS_info.Tables(1).Rows(0).Item("LOCAL_codigo")) + "."
          DS_Cobradores.Tables("Reporte_Comprobante_encabezado").Rows.Add(fila1)

          Dim i As Integer = 0
          While i < ds_comprobanteinfo.Tables(2).Rows.Count
            Dim fila2 As DataRow = DS_Cobradores.Tables("Reporte_Comprobante_cobros").NewRow
            fila2("Concepto_Tarifa") = ds_comprobanteinfo.Tables(2).Rows(i).Item("Concepto_Tarifa")
            fila2("Concepto_fecha") = ds_comprobanteinfo.Tables(2).Rows(i).Item("Concepto_fecha")
            fila2("nro_comprobante") = CStr(NroComprobante)
            fila2("monto") = ds_comprobanteinfo.Tables(2).Rows(i).Item("CTACTEDET_importe")
            DS_Cobradores.Tables("Reporte_Comprobante_cobros").Rows.Add(fila2)
            i = i + 1
          End While

          Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
          CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
          CrReport.Load(Server.MapPath("~/COB_Cobradores/rpt_comprobante.rpt"))
          CrReport.Database.Tables("Reporte_Comprobante_encabezado").SetDataSource(DS_Cobradores.Tables("Reporte_Comprobante_encabezado"))
          CrReport.Database.Tables("Reporte_Comprobante_cobros").SetDataSource(DS_Cobradores.Tables("Reporte_Comprobante_cobros"))
          CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/COB_Cobradores/Comprobante.pdf"))



        End If





      End If


    End If

    If contcobros <> 0 Then
      'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
      'Response.Redirect("~/COB_Cobradores/Cob_Comprobante.aspx")

      BOTON_GRABA.Visible = False

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_reporte", "$(document).ready(function () {$('#modal_reporte').modal();});", True)


    Else
      'error, no se efectuo ningun cobro.verifique la info ingresada.
      'modal_error_validacion
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_error_validacion", "$(document).ready(function () {$('#modal_error_validacion').modal();});", True)
    End If

  End Sub

  Private Sub btn_grabado_ok_ServerClick(sender As Object, e As EventArgs) Handles btn_grabado_ok.ServerClick
    Response.Redirect("~/COB_Cobradores/Cob_Comprobante.aspx")
    'Response.Redirect("~/COB_Cobradores/Cob_CobroListado.aspx") 'con este retrocedo o continuo.
  End Sub

  Private Sub btn_grabado_close_ServerClick(sender As Object, e As EventArgs) Handles btn_grabado_close.ServerClick
    'Response.Redirect("~/COB_Cobradores/Cob_CobroListado.aspx") 'con este retrocedo o continuo
    Response.Redirect("~/COB_Cobradores/Cob_Comprobante.aspx")
  End Sub

  Private Sub BTN_IMPRIMIR_Click(sender As Object, e As EventArgs) Handles BTN_IMPRIMIR.Click

  End Sub
End Class
