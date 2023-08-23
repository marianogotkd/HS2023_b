Public Class TicketsClientesPorOrden
  Inherits System.Web.UI.Page
#Region "DECLARACIONES"
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAtickets As New Capa_Datos.WC_tickets
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
#End Region

#Region "EVENTOS"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      Permisos
      'VALIDACION: Verificar en BD cual es el dia de la ultima liquidacion en tabla PARAMETRO, donde el campo Estado= "Inactivo"
      Dim ds_parametro As DataSet = DAparametro.Parametro_consultar_ultliq
      If ds_parametro.Tables(0).Rows.Count <> 0 Then

        '/////FECHA DE ULTIMA LIQUIDACION
        HF_parametro_id.Value = ds_parametro.Tables(0).Rows(0).Item("Parametro_id")
        HF_fecha.Value = ds_parametro.Tables(0).Rows(0).Item("Fecha")
        Dim FECHA As Date = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha"))
        Label_fecha.Text = FECHA.ToString("dd-MM-yyyy")

        '////DIA DE LA LIQUIDACION
        'aqui va un case dependiendo el nro de dia
        Dim Dia As Integer = CInt(ds_parametro.Tables(0).Rows(0).Item("Dia"))
        HF_dia_id.Value = CInt(ds_parametro.Tables(0).Rows(0).Item("Dia"))

        Select Case Dia
          Case 1 'Domingo
            Label_dia.Text = "DOMINGO"
          Case 2 'Lunes
            Label_dia.Text = "LUNES"
          Case 3 'Martes
            Label_dia.Text = "MARTES"
          Case 4 'Miercoles
            Label_dia.Text = "MIERCOLES"
          Case 5 'Jueves
            Label_dia.Text = "JUEVES"
          Case 6 'Viernes
            Label_dia.Text = "VIERNES"
          Case 7 'Sabado
            Label_dia.Text = "SABADO"
        End Select

        Txt_DesdeGrupoCodigo.Focus()
      Else
        'error, no hay liquidacion completada
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
      End If
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
            Dim SubOpcion As String = ""
            Try
              SubOpcion = ds_permisos.Tables(0).Rows(i).Item("SubOpcion")
            Catch ex As Exception
            End Try
            If (Menu = "G" And Opcion = "") Or (Menu = "G" And Opcion = "1") Then

              If (SubOpcion = "") Or (SubOpcion = "1") Then
                valido = "si"
                Exit While
              End If


            End If
            i = i + 1
          End While
          If valido = "si" Then
            'se accede sin problemas
          Else
            'no tiene permiso, se redirige a menu.
            Response.Redirect("~/WC_TicketsClientes/TicketsClientes_op1.aspx")
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
    Response.Redirect("~/WC_TicketsClientes/TicketsClientes_op1.aspx")
  End Sub

  Private Sub btn_error_close_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_ok_error_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Txt_DesdeGrupoCodigo_Init(sender As Object, e As EventArgs) Handles Txt_DesdeGrupoCodigo.Init
    Txt_DesdeGrupoCodigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_DesdeClienteCod_Init(sender As Object, e As EventArgs) Handles Txt_DesdeClienteCod.Init
    Txt_DesdeClienteCod.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_HastaGrupoCodigo_Init(sender As Object, e As EventArgs) Handles Txt_HastaGrupoCodigo.Init
    Txt_HastaGrupoCodigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_HastaClienteCod_Init(sender As Object, e As EventArgs) Handles Txt_HastaClienteCod.Init
    Txt_HastaClienteCod.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_msjgeneral_Init(sender As Object, e As EventArgs) Handles Txt_msjgeneral.Init
    Txt_msjgeneral.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick

    'valido que todos los campos tengas algo ingresado.

    Dim valido As String = "si"

    Try
      Txt_DesdeGrupoCodigo.Text = CInt(Txt_DesdeGrupoCodigo.Text)
    Catch ex As Exception
      valido = "no"
    End Try

    Try
      Txt_DesdeClienteCod.Text = CInt(Txt_DesdeClienteCod.Text)
    Catch ex As Exception
      valido = "no"
    End Try

    Try
      Txt_HastaGrupoCodigo.Text = CInt(Txt_HastaGrupoCodigo.Text)
    Catch ex As Exception
      valido = "no"
    End Try

    Try
      Txt_HastaClienteCod.Text = CInt(Txt_HastaClienteCod.Text)
    Catch ex As Exception
      valido = "no"
    End Try

    If valido = "si" Then
      'recupero todos los puntos y recorridos para la fecha indicada.

#Region "TABLA DE PUNTOS"

      'voy a cargar los 3 tables con las 5 zonas, en total son 3 tablas de 21 registros (encabezado+20 puntos).
      Dim DS_ticketsclientes As New DS_ticketsclientes
      Dim Fila_A As DataRow = DS_ticketsclientes.Tables("Puntos_A").NewRow
      Fila_A("ZON1") = "ZON1" '1A
      Fila_A("ZON2") = "ZON2" '1B
      Fila_A("ZON3") = "ZON3" '1C
      Fila_A("ZON4") = "ZON4" '1D
      Fila_A("ZON5") = "ZON5" '1E
      Fila_A("ZON6") = "ZON6" '1F
      Fila_A("ZON7") = "ZON7" '1G
      Fila_A("ZON8") = "ZON8" '1H
      Fila_A("ZON9") = "ZON9" '1I
      Fila_A("ZO10") = "ZO10" '1J
      Fila_A("ZO11") = "ZO11" '2A
      Fila_A("ZO12") = "ZO12" '2B
      Fila_A("ZO13") = "ZO13" '2C
      Fila_A("ZO14") = "ZO14" '2D
      Fila_A("ZO15") = "ZO15" '2E
      Fila_A("ZO16") = "ZO16" '2F
      Fila_A("ZO17") = "ZO17" '2G
      Fila_A("ZO18") = "ZO18" '2H
      Fila_A("ZO19") = "ZO19" '2I
      Fila_A("ZO20") = "ZO20" '2J
      Fila_A("Fecha") = CDate(HF_fecha.Value)
      DS_ticketsclientes.Tables("Puntos_A").Rows.Add(Fila_A)
      Dim i As Integer = 0
      While i < 20
        Dim Fila As DataRow = DS_ticketsclientes.Tables("Puntos_A").NewRow
        Fila("ITEM") = CStr(i + 1)
        Fila("Fecha") = CDate(HF_fecha.Value)
        DS_ticketsclientes.Tables("Puntos_A").Rows.Add(Fila)
        i = i + 1
      End While
      Dim Puntos_B As DataTable = DS_ticketsclientes.Tables("Puntos_A").Clone() 'copio solo la estructura de Puntos_A
      DS_ticketsclientes.Tables("Puntos_A").Rows.Add()
      'ahora creo registros para Puntos_B
      Dim Fila_B As DataRow = Puntos_B.NewRow
      Fila_B("ZON1") = "ZO21"
      Fila_B("ZON2") = "ZO22"
      Fila_B("ZON3") = "ZO23"
      Fila_B("ZON4") = "ZO24"
      Fila_B("ZON5") = "ZO25"
      Fila_B("ZON6") = "ZO26"
      Fila_B("ZON7") = "ZO27"
      Fila_B("ZON8") = "ZO28"
      Fila_B("ZON9") = "ZO28"
      Fila_B("ZO10") = "ZO30"
      Fila_B("ZO11") = "ZO31"
      Fila_B("ZO12") = "ZO32"
      Fila_B("ZO13") = "ZO33"
      Fila_B("ZO14") = "ZO34"
      Fila_B("ZO15") = "ZO35"
      Fila_B("ZO16") = "ZO36"
      Fila_B("ZO17") = "ZO37"
      Fila_B("ZO18") = "ZO38"
      Fila_B("ZO19") = "ZO39"
      Fila_B("ZO20") = "ZO40"
      Fila_B("Fecha") = CDate(HF_fecha.Value)
      Puntos_B.Rows.Add(Fila_B)
      Dim ii As Integer = 0
      While ii < 20
        Dim Fila As DataRow = Puntos_B.NewRow
        Fila("ITEM") = CStr(ii + 1)
        Fila("Fecha") = CDate(HF_fecha.Value)
        Puntos_B.Rows.Add(Fila)
        ii = ii + 1
      End While
      DS_ticketsclientes.Tables("Puntos_A").Merge(Puntos_B)
      DS_ticketsclientes.Tables("Puntos_A").Rows.Add()
      Dim Puntos_C As DataTable = DS_ticketsclientes.Tables("Puntos_A").Clone() 'copio solo la estructura de Puntos_A
      'ahora creo registros para Puntos_B
      Dim Fila_C As DataRow = Puntos_C.NewRow
      Fila_C("ZON1") = "ZO41"
      Fila_C("ZON2") = "ZO42"
      Fila_C("ZON3") = "ZO43"
      Fila_C("ZON4") = "ZO44"
      Fila_C("ZON5") = "ZO45"
      Fila_C("ZON6") = "ZO46"
      Fila_C("ZON7") = "ZO47"
      Fila_C("ZON8") = "ZO48"
      Fila_C("ZON9") = "ZO49"
      Fila_C("ZO10") = "ZO50"
      'Fila_C("ZO11") = "ZO
      'Fila_C("ZO12") = "ZO32"
      'Fila_C("ZO13") = "ZO33"
      'Fila_C("ZO14") = "ZO34"
      'Fila_C("ZO15") = "ZO35"
      'Fila_C("ZO16") = "ZO36"
      'Fila_C("ZO17") = "ZO37"
      'Fila_C("ZO18") = "ZO38"
      'Fila_C("ZO19") = "ZO39"
      'Fila_C("ZO20") = "ZO40"
      Fila_C("Fecha") = CDate(HF_fecha.Value)
      Puntos_C.Rows.Add(Fila_C)
      Dim J As Integer = 0
      While J < 20
        Dim Fila As DataRow = Puntos_C.NewRow
        Fila("ITEM") = CStr(J + 1)
        Fila("Fecha") = CDate(HF_fecha.Value)
        Puntos_C.Rows.Add(Fila)
        J = J + 1
      End While
      DS_ticketsclientes.Tables("Puntos_A").Merge(Puntos_C)


      Dim ds_puntos As DataSet = DAtickets.RecorridosPuntos_obtener_fecha(CDate(HF_fecha.Value))
      If ds_puntos.Tables(0).Rows.Count <> 0 Then
        'si recupero algo cargo
        Dim r As Integer = 0
        While r < ds_puntos.Tables(0).Rows.Count
          Dim Codigo As String = CStr(ds_puntos.Tables(0).Rows(r).Item("Codigo"))
          Select Case Codigo
            Case "1A"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON1", 1, r)
            Case "1B"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON2", 1, r)
            Case "1C"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON3", 1, r)
            Case "1D"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON4", 1, r)
            Case "1E"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON5", 1, r)
            Case "1F"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON6", 1, r)
            Case "1G"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON7", 1, r)
            Case "1H"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON8", 1, r)
            Case "1I"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON9", 1, r)
            Case "1J"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO10", 1, r)
            Case "2A"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO11", 1, r)
            Case "2B"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO12", 1, r)
            Case "2C"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO13", 1, r)
            Case "2D"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO14", 1, r)
            Case "2E"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO15", 1, r)
            Case "2F"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO16", 1, r)
            Case "2G"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO17", 1, r)
            Case "2H"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO18", 1, r)
            Case "2I"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO19", 1, r)
            Case "2J"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO20", 1, r)
            Case "3A"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON1", 23, r)
            Case "3B"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON2", 23, r)
            Case "3C"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON3", 23, r)
            Case "3D"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON4", 23, r)
            Case "3E"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON5", 23, r)
            Case "3F"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON6", 23, r)
            Case "3G"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON7", 23, r)
            Case "3H"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON8", 23, r)
            Case "3I"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON9", 23, r)
            Case "3J"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO10", 23, r)
            Case "4A"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO11", 23, r)
            Case "4B"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO12", 23, r)
            Case "4C"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO13", 23, r)
            Case "4D"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO14", 23, r)
            Case "4E"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO15", 23, r)
            Case "4F"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO16", 23, r)
            Case "4G"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO17", 23, r)
            Case "4H"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO18", 23, r)
            Case "4I"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO19", 23, r)
            Case "4J"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO20", 23, r)
            Case "5A"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON1", 45, r)
            Case "5B"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON2", 45, r)
            Case "5C"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON3", 45, r)
            Case "5D"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON4", 45, r)
            Case "5E"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON5", 45, r)
            Case "5F"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON6", 45, r)
            Case "5G"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON7", 45, r)
            Case "5H"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON8", 45, r)
            Case "5I"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZON9", 45, r)
            Case "5J"
              CARGA_1(DS_ticketsclientes, ds_puntos, "ZO10", 45, r)
          End Select


          r = r + 1
        End While


      Else
        'error. no hay puntos o recorridos para dicha fecha.
      End If

#End Region



      Dim ds_ctacte As DataSet = DAtickets.CtaCte_MovimientosBuscar(CDate(HF_fecha.Value), CInt(Txt_DesdeGrupoCodigo.Text), CInt(Txt_DesdeClienteCod.Text), CInt(Txt_HastaGrupoCodigo.Text), CInt(Txt_HastaClienteCod.Text))

      If ds_ctacte.Tables(0).Rows.Count <> 0 Then

        'vamos a cargar la info de la cta cta y premios para cada cliente.

        Dim indice As Integer = 0
        While indice < ds_ctacte.Tables(0).Rows.Count

          Dim fila As DataRow = DS_ticketsclientes.Tables("Cliente_CtacteInfo").NewRow
          fila("Grupo_id") = CInt(ds_ctacte.Tables(0).Rows(indice).Item("Grupo_id"))
          fila("Grupo_codigo") = ds_ctacte.Tables(0).Rows(indice).Item("Grupo_codigo")
          fila("Grupo_nombre") = ds_ctacte.Tables(0).Rows(indice).Item("Grupo_nombre")
          fila("Cliente") = CInt(ds_ctacte.Tables(0).Rows(indice).Item("Cliente"))
          fila("Cliente_codigo") = CInt(ds_ctacte.Tables(0).Rows(indice).Item("Cliente_codigo"))
          fila("Cliente_nombre") = ds_ctacte.Tables(0).Rows(indice).Item("Cliente_Nombre")
          fila("R") = ds_ctacte.Tables(0).Rows(indice).Item("R")
          fila("O") = ds_ctacte.Tables(0).Rows(indice).Item("O")
          fila("Recaudacion") = ds_ctacte.Tables(0).Rows(indice).Item("Recaudacion")
          fila("Comision") = ds_ctacte.Tables(0).Rows(indice).Item("Comision")
          fila("Premios") = ds_ctacte.Tables(0).Rows(indice).Item("Premios")
          fila("Reclamos") = ds_ctacte.Tables(0).Rows(indice).Item("Reclamos")
          fila("DejoGano") = ds_ctacte.Tables(0).Rows(indice).Item("DejoGano")
          If CDec(ds_ctacte.Tables(0).Rows(indice).Item("DejoGano")) > 0 Then
            fila("DejoGano_desc") = "DEJO:"
          Else
            fila("DejoGano_desc") = "GANO:"
          End If
          fila("RecaudacionSC") = ds_ctacte.Tables(0).Rows(indice).Item("RecaudacionSC")
          fila("ComisionSC") = ds_ctacte.Tables(0).Rows(indice).Item("ComisionSC")
          fila("PremiosSC") = ds_ctacte.Tables(0).Rows(indice).Item("PremiosSC")
          fila("ReclamosSC") = ds_ctacte.Tables(0).Rows(indice).Item("ReclamosSC")
          fila("DejoGanoSC") = ds_ctacte.Tables(0).Rows(indice).Item("DejoGanoSC")
          If CDec(ds_ctacte.Tables(0).Rows(indice).Item("DejoGanoSC")) > 0 Then
            fila("DejoGanoSC_desc") = "DEJO:"
          Else
            fila("DejoGanoSC_desc") = "GANO:"
          End If
          Dim DejoGano_sum As Decimal = CDec(ds_ctacte.Tables(0).Rows(indice).Item("DejoGano")) + CDec(ds_ctacte.Tables(0).Rows(indice).Item("DejoGanoSC"))
          fila("DejoGanoGeneral") = DejoGano_sum
          If DejoGano_sum > 0 Then
            fila("DejoGanoGeneral_desc") = "GENERAL DEJO:"
          Else
            fila("DejoGanoGeneral_desc") = "GENERAL GANO:"
          End If
          fila("RecaudacionB") = ds_ctacte.Tables(0).Rows(indice).Item("RecaudacionB")
          fila("ComisionB") = ds_ctacte.Tables(0).Rows(indice).Item("ComisionB")
          fila("PremiosB") = ds_ctacte.Tables(0).Rows(indice).Item("PremiosB")
          fila("ReclamosB") = ds_ctacte.Tables(0).Rows(indice).Item("ReclamosB")
          fila("DejoGanoB") = ds_ctacte.Tables(0).Rows(indice).Item("DejoGanoB")
          If CDec(ds_ctacte.Tables(0).Rows(indice).Item("DejoGanoB")) > 0 Then
            fila("DejoGanoB_desc") = "DEJO:"
          Else
            fila("DejoGanoB_desc") = "GANO:"
          End If
          DejoGano_sum = DejoGano_sum + CDec(ds_ctacte.Tables(0).Rows(indice).Item("DejoGanoB"))
          fila("DejoGanoGeneralDia") = DejoGano_sum
          If DejoGano_sum > 0 Then
            fila("DejoGanoGeneralDia_desc") = "GENERAL DEL DIA DEJO:"
          Else
            fila("DejoGanoGeneralDia_desc") = "GENERAL DEL DIA GANO:"
          End If
          fila("Saldoanterior") = ds_ctacte.Tables(0).Rows(indice).Item("Clientes_Saldoanterior")
          fila("Cobros") = ds_ctacte.Tables(0).Rows(indice).Item("Cobros")
          fila("Regalos") = ds_ctacte.Tables(0).Rows(indice).Item("Regalos")
          fila("Pagos") = ds_ctacte.Tables(0).Rows(indice).Item("Pagos")
          fila("Prestamo") = ds_ctacte.Tables(0).Rows(indice).Item("Prestamo")
          fila("CobPrestamo") = ds_ctacte.Tables(0).Rows(indice).Item("CobPrestamo")
          fila("Credito") = ds_ctacte.Tables(0).Rows(indice).Item("Credito")
          fila("CobCredito") = ds_ctacte.Tables(0).Rows(indice).Item("CobCredito")

          Dim ds_credi As DataSet = DAtickets.CobroCreditos_ClienteObtener(CDate(HF_fecha.Value), CInt(ds_ctacte.Tables(0).Rows(indice).Item("Cliente")))
          If ds_credi.Tables(0).Rows.Count <> 0 Then
            Dim nro_cta As Integer = CInt(ds_credi.Tables(0).Rows(0).Item("Cuota"))
            Dim dias As Integer = CInt(ds_credi.Tables(0).Rows(0).Item("Dias"))
            fila("Credito_Cuota") = "CREDITO CUOTA " + CStr(nro_cta) + "/" + CStr(dias) 'dbo.CobroPrestamosCreditos para saer que cuota se cobra
          Else
            fila("Credito_Cuota") = "CREDITO CUOTA"
          End If
          fila("Clientes_Saldo") = ds_ctacte.Tables(0).Rows(indice).Item("Clientes_Saldo")

          If ds_ctacte.Tables(0).Rows(indice).Item("Clientes_Imprime") = True Then
            Dim calculo_importe As Decimal = (100 * CDec(ds_ctacte.Tables(0).Rows(indice).Item("Clientes_SaldoRegalo"))) / CDec(ds_ctacte.Tables(0).Rows(indice).Item("Clientes_Regalo"))
            fila("Regalo_monto") = calculo_importe

            If calculo_importe > 0 Then
              fila("Regalo_desc") = "REGALO EN CONTRA % " + CStr(ds_ctacte.Tables(0).Rows(indice).Item("Clientes_Regalo")) + ":"
            Else
              fila("Regalo_desc") = "REGALO A FAVOR % " + CStr(ds_ctacte.Tables(0).Rows(indice).Item("Clientes_Regalo")) + ":"
            End If
            'Este dato solo es a nivel informativo y
            'puede tomar la referencia "A FAVOR" si el valor es negativo, o "EN CONTRA" Si es positivo.
            'Se muestra si dbo.Clientes.Imprime = true. El valor del porcentaje se obtiene del dbo.Clientes.Regalo.
            'El importe se obtiene de multiplicar (100 * dbo.Clientes.SaldoRegalo / dbo.Clientes.Regalo).
          End If

          fila("Fecha") = CDate(HF_fecha.Value)
          fila("mensaje_usuario") = Txt_msjgeneral.Text.ToString
          DS_ticketsclientes.Tables("Cliente_CtacteInfo").Rows.Add(fila)


          'OBTENGO PREMIOS

          Dim DS_Premios As DataSet = DAtickets.Premios_Cliente_Obtener(CDate(HF_fecha.Value), CInt(HF_dia_id.Value), CInt(ds_ctacte.Tables(0).Rows(indice).Item("Cliente")))
          If DS_Premios.Tables(0).Rows.Count <> 0 Then
            Dim indice2 As Integer = 0
            While indice2 < DS_Premios.Tables(0).Rows.Count
              Dim row1 As DataRow = DS_ticketsclientes.Tables("Cliente_PremiosInfo").NewRow
              row1("Cliente") = CInt(DS_Premios.Tables(0).Rows(indice2).Item("Cliente"))
              row1("Premios_id") = DS_Premios.Tables(0).Rows(indice2).Item("Premios_id")
              row1("Recorrido_codigo") = DS_Premios.Tables(0).Rows(indice2).Item("Recorrido_codigo")
              row1("Referencia") = DS_Premios.Tables(0).Rows(indice2).Item("Referencia")
              row1("Importe") = DS_Premios.Tables(0).Rows(indice2).Item("Importe")
              row1("Pid") = DS_Premios.Tables(0).Rows(indice2).Item("Pid")
              row1("Suc") = DS_Premios.Tables(0).Rows(indice2).Item("Suc")
              row1("Pid2") = DS_Premios.Tables(0).Rows(indice2).Item("Pid2")
              row1("Suc2") = DS_Premios.Tables(0).Rows(indice2).Item("Suc2")
              row1("Premio") = DS_Premios.Tables(0).Rows(indice2).Item("Premio")
              row1("Terminal") = DS_Premios.Tables(0).Rows(indice2).Item("Terminal")

              DS_ticketsclientes.Tables("Cliente_PremiosInfo").Rows.Add(row1)

              indice2 = indice2 + 1
            End While


          End If

          indice = indice + 1
        End While


        'Dim filab As DataRow = DS_ticketsclientes.Tables("TicketClieOrden_info1").NewRow
        'filab("Cliente_codigo") = "1"
        'filab("Cliente_nombre") = "CHOCOLONEA, PABLO"
        'filab("Fecha") = CDate(HF_fecha.Value)
        'filab("R") = "0000"
        'filab("O") = "0000"
        'filab("Dia") = "LUNES"
        'DS_ticketsclientes.Tables("TicketClieOrden_info1").Rows.Add(filab)

        '------------------AQUIREPORTE ------------------------------------------------

        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketsClientesPorOrden_informe01a.rpt"))
        CrReport.Database.Tables("TicketClieOrden_info1").SetDataSource(DS_ticketsclientes.Tables("TicketClieOrden_info1"))
        CrReport.Database.Tables("Puntos_A").SetDataSource(DS_ticketsclientes.Tables("Puntos_A"))

        CrReport.Database.Tables("Cliente_CtacteInfo").SetDataSource(DS_ticketsclientes.Tables("Cliente_CtacteInfo"))
        CrReport.Database.Tables("Cliente_PremiosInfo").SetDataSource(DS_ticketsclientes.Tables("Cliente_PremiosInfo"))

        'creo una cadena que voy a necesitar para el nombre del archivo a generar
        Dim grupo_longitud As Integer = 3
        Dim cliente_longitud As Integer = 4
        Dim grupo_dig As String = Txt_DesdeGrupoCodigo.Text
        While grupo_dig.Length < grupo_longitud
          grupo_dig = "0" + grupo_dig
        End While
        Dim cliente_dig As String = Txt_DesdeClienteCod.Text
        While cliente_dig.Length < cliente_longitud
          cliente_dig = "0" + cliente_dig
        End While

        Dim nombre_archivo As String = CDate(HF_fecha.Value).ToString("ddMMyy") + grupo_dig + cliente_dig
        'Dim nombre_archivo As String = CDate(HF_fecha.Value).ToString("ddMMyy") + Txt_DesdeGrupoCodigo.Text + Txt_DesdeClienteCod.Text
        Dim ruta As String = "/WC_Reportes/Rpt/" + nombre_archivo + ".pdf"



        'CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), ruta))
        'CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketClieOrden.pdf"))

        'CrReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, False, "Reporte")

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_RptGenerado", "$(document).ready(Function() {$('#modal-ok_RptGenerado').modal();});", True)

        '------------------------------------------------------------------------------
      Else
        'error, la busqueda no arrojo resultados. No hay movimientos para la fecha y los parametros ingresados.
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_ErrorConsulta", "$(document).ready(Function() {$('#modal-ok_ErrorConsulta').modal();});", True)
      End If







    Else
      'msj complete información solicitada.
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_ErrorValidacion", "$(document).ready(Function() {$('#modal-ok_ErrorValidacion').modal();});", True)

    End If


  End Sub

  Private Sub Btn_ErrorValidacion_close_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorValidacion_close.ServerClick
    focus_error()
  End Sub

  Private Sub Btn_ErrorValidacion_ok_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorValidacion_ok.ServerClick
    focus_error()
  End Sub

  Private Sub Btn_ErrorConsulta_close_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorConsulta_close.ServerClick
    Txt_DesdeGrupoCodigo.Focus()
  End Sub

  Private Sub Btn_ErrorConsulta_ok_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorConsulta_ok.ServerClick
    Txt_DesdeGrupoCodigo.Focus()
  End Sub
#End Region

#Region "METODOS"
  Private Sub focus_error()
    Try
      Txt_DesdeGrupoCodigo.Text = CInt(Txt_DesdeGrupoCodigo.Text)
      Try
        Txt_DesdeClienteCod.Text = CInt(Txt_DesdeClienteCod.Text)
        Try
          Txt_HastaGrupoCodigo.Text = CInt(Txt_HastaGrupoCodigo.Text)
          Try
            Txt_HastaClienteCod.Text = CInt(Txt_HastaClienteCod.Text)
          Catch ex As Exception
            Txt_HastaClienteCod.Focus()
          End Try
        Catch ex As Exception
          Txt_HastaGrupoCodigo.Focus()
        End Try
      Catch ex As Exception
        Txt_DesdeClienteCod.Focus()
      End Try
    Catch ex As Exception
      Txt_DesdeGrupoCodigo.Focus()
    End Try
  End Sub

  Private Sub CARGA_1(ByRef DS_ticketsclientes As DataSet, ByRef ds_puntos As DataSet, ByVal Zona As String, ByVal indice As Integer, ByVal r As Integer)
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P1"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 1).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P2"))
    Catch ex As Exception

    End Try

    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 2).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P3"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 3).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P4"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 4).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P5"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 5).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P6"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 6).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P7"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 7).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P8"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 8).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P9"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 9).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P10"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 10).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P11"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 11).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P12"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 12).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P13"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 13).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P14"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 14).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P15"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 15).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P16"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 16).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P17"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 17).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P18"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 18).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P19"))
    Catch ex As Exception

    End Try
    Try
      DS_ticketsclientes.Tables("Puntos_A").Rows(indice + 19).Item(Zona) = CStr(ds_puntos.Tables(0).Rows(r).Item("P20"))
    Catch ex As Exception

    End Try


  End Sub



#End Region



End Class
