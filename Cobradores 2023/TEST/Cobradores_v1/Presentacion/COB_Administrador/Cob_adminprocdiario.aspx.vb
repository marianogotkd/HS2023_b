Public Class Cob_adminprocdiario
  Inherits System.Web.UI.Page
  Dim daParametro As New Capa_Datos.Parametro
  Dim daTarifa As New Capa_Datos.Tarifa
  Dim daCtaCte As New Capa_Datos.CtaCte
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then

      Txt_asig_fechaDesde.Text = Today
      Txt_asig_fechaHasta.Text = Today

      'validar_fecha()

      'Txt_OP.Focus()



    End If
  End Sub

  Private Sub validar_fecha()

    Dim ds_parametro As DataSet = daParametro.Parametro_obtenerultimoproc
    If ds_parametro.Tables(0).Rows.Count <> 0 Then
      Dim FECHA As Date = CDate(ds_parametro.Tables(0).Rows(0).Item("PAR_fecha"))
      HF_fechaDesde.Value = ds_parametro.Tables(0).Rows(0).Item("PAR_fecha")
      Txt_asig_fechaDesde.Text = FECHA.ToString("dd-MM-yyyy")


    End If


  End Sub


  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_modificar_ServerClick(sender As Object, e As EventArgs) Handles btn_modificar.ServerClick

    Dim valido As String = "si"
    Dim FechaIngresoDesde As Date
    Try
      FechaIngresoDesde = CDate(Txt_asig_fechaDesde.Text)
    Catch ex As Exception
      valido = "no"
    End Try

    Dim FechaIngresoHasta As Date
    Try
      FechaIngresoHasta = CDate(Txt_asig_fechaHasta.Text)
    Catch ex As Exception
      valido = "no"
    End Try


    If valido = "si" Then
      valido = "si"
      'ahora verifico que el intervalo de fechas sea correcto

      If FechaIngresoHasta >= FechaIngresoDesde Then
        'continuo
        ProcesoDiario2(FechaIngresoDesde, FechaIngresoHasta)

      Else
        'ingrese un intervalo valido.
        'modal-ErrorValidacion
        Label_ErrorValidacion.Text = "Ingrese un intervalo de fechas valido."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ErrorValidacion", "$(document).ready(function () {$('#modal-ErrorValidacion').modal();});", True)

      End If

    Else
      'ingrese fechas validas.
      'modal-ErrorValidacion
      Label_ErrorValidacion.Text = "Ingrese fechas validas."
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ErrorValidacion", "$(document).ready(function () {$('#modal-ErrorValidacion').modal();});", True)

    End If





    'verificar que la fecha ingresada sea mayor a la fecha que se registro en Parametro.PAR_fecha,
    'puede ocurrir qe parametro este vacia, lo cual tambien es correcto.

    'If valido = "si" Then
    '  valido = "si"
    '  Dim valido_mj_error = ""
    '  Dim ds_parametro As DataSet = daParametro.Parametro_obtenerultimoproc
    '  If ds_parametro.Tables(0).Rows.Count <> 0 Then
    '    Dim FechaParametro As Date = CDate(ds_parametro.Tables(0).Rows(0).Item("PAR_fecha"))

    '    If FechaIngreso > FechaParametro Then
    '      'es valido

    '      ProcesoDiario(FechaIngreso)


    '    Else
    '      'no es valido
    '      valido = "no"
    '      If FechaIngreso = FechaParametro Then
    '        valido_mj_error = "fecha igual" 'ya se ejecuto el proceso para la fecha ingresada.
    '      Else
    '        valido_mj_error = "fecha anterior"
    '      End If
    '    End If

    '  Else
    '    'entonces es valido
    '  End If

    '  If valido = "si" Then
    '  Else
    '    'aqi mensaje de error dependiendo el valor en valido_mj_error
    '    'hay 2 opcione: q sean iguales las fechas o bien se ingreso una fecha inferior al ultimo proc ejecutado en el servidor.
    '  End If
    'Else
    '  'aqui msj, ingrese fecha valida.

    'End If
  End Sub


  Private Sub ProcesoDiario2(ByVal FechaIngresoDesde As Date, ByVal FechaIngresoHasta As Date)
    'hay 2 tipos de carga:
    '1 - si FechaIngresoDesde es igual a FechaIngresoHasta, se hace 1 consulta.
    '2 - si son varios dias son 1 consulta x fecha y hoy avanzando hasta q fechadesde sea igual a fechahasta


    While FechaIngresoDesde <= FechaIngresoHasta
      'consulto la tabla TarifaCliente, donde TARCLIE_estado='activo' y TARCLIE_fechainicio <= a FechaIngreso
      Dim ds_tarifacliente As DataSet = daTarifa.TarifaCliente_procdiario001(FechaIngresoDesde)
      'ciclo y voy ingresando
      Dim i As Integer = 0
      While i < ds_tarifacliente.Tables(0).Rows.Count
        Dim TARCLIE_tipo As String = ds_tarifacliente.Tables(0).Rows(i).Item("TARCLIE_tipo")
        Select Case TARCLIE_tipo
          Case "UNICA"
            TarifaUnica(ds_tarifacliente.Tables(0).Rows(i), FechaIngresoDesde)
          Case "PERIODICA"
            'TODAVIA NO
            TarifaPeriodica2(ds_tarifacliente.Tables(0).Rows(i), FechaIngresoDesde)
        End Select
        i = i + 1
      End While
      '/////////////////////////////////////////////////////////////////
      '/////////////////////////////////////////////////////////////////
      FechaIngresoDesde = FechaIngresoDesde.AddDays(1)
      '/////////////////////////////////////////////////////////////////
      '/////////////////////////////////////////////////////////////////
    End While

    'daParametro.Parametro_alta(FechaIngreso, "proc.diario generado")


    'aqui llamo al modal para indicar que el proceso termino.




    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok-complete", "$(document).ready(function () {$('#modal-ok-complete').modal();});", True)


    'If FechaIngresoDesde = FechaIngresoHasta Then
    '  'consulto la tabla TarifaCliente, donde TARCLIE_estado='activo' y TARCLIE_fechainicio <= a FechaIngreso
    '  Dim ds_tarifacliente As DataSet = daTarifa.TarifaCliente_procdiario001(FechaIngreso)
    '  'ciclo y voy ingresando






    'Else


    'End If



  End Sub

  Private Sub ProcesoDiario(ByVal FechaIngreso As Date)
    'consulto la tabla TarifaCliente, donde TARCLIE_estado='activo' y TARCLIE_fechainicio <= a FechaIngreso


    Dim ds_tarifacliente As DataSet = daTarifa.TarifaCliente_procdiario001(FechaIngreso)
    'ciclo y voy ingresando

    Dim i As Integer = 0
    While i < ds_tarifacliente.Tables(0).Rows.Count
      Dim TARCLIE_tipo As String = ds_tarifacliente.Tables(0).Rows(i).Item("TARCLIE_tipo")
      Select Case TARCLIE_tipo
        Case "UNICA"
          TarifaUnica(ds_tarifacliente.Tables(0).Rows(i), FechaIngreso)
        Case "PERIODICA"
          'TODAVIA NO
          TarifaPeriodica(ds_tarifacliente.Tables(0).Rows(i), FechaIngreso)
      End Select
      i = i + 1
    End While

    daParametro.Parametro_alta(FechaIngreso, "proc.diario generado")


    'aqui llamo al modal para indicar que el proceso termino.




    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok-complete", "$(document).ready(function () {$('#modal-ok-complete').modal();});", True)


  End Sub

  Private Sub TarifaUnica(ByVal fila As DataRow, ByVal FechaIngreso As Date)
    'como es una tarifa unica y ademas activa, se va a insertar directamente en el detalle de la ctacte.
    Dim TARCLIE_ID As Integer = fila.Item("TARCLIE_ID")
    Dim CTACTE_ID As Integer = fila.Item("CTACTE_ID")
    Dim Importe As Decimal = fila.Item("TARCLIE_precio")

    Dim TARCLIE_fechainicio As Date = CDate(fila.Item("TARCLIE_fechainicio"))

    If TARCLIE_fechainicio <= FechaIngreso Then 'solo se inserta si es valida la fecha de inicio de la tarifa

      daCtaCte.CtaCteDetalle_alta(TARCLIE_fechainicio, CTACTE_ID, Importe, "debe", "", TARCLIE_ID, Importe) 'nota: el saldodeudor=importe de la tarifa.
      Dim ds_ctacteinfo As DataSet = daCtaCte.CtaCte_obtenerID(CTACTE_ID)
      Dim CTACTE_saldodeudor As Decimal = ds_ctacteinfo.Tables(0).Rows(0).Item("CTACTE_saldodeudor")

      'SE MODIFICA EL SALDO DEUDOR EN CTACTE.CTACTE_saldodedor = CTACTE.CTACTE_saldodedor + TarifaCliente.TARCLIE_precio 
      CTACTE_saldodeudor = CTACTE_saldodeudor + Importe
      daCtaCte.CtaCte_SaldodeudorModif(CTACTE_ID, CTACTE_saldodeudor)

      'luego se va a modificar el estado de dicha tarifa en la tabla TarifaCliente.TARCLIE_estado='inactivo'
      daTarifa.TarifaCliente_EstadoModificar(TARCLIE_ID, "inactivo generado")


    End If


  End Sub


  Private Sub TarifaPeriodica2(ByVal fila As DataRow, ByVal FechaIngreso As Date)
    Dim TARCLIE_fechainicio As Date = CDate(fila.Item("TARCLIE_fechainicio"))
    If TARCLIE_fechainicio <= FechaIngreso Then 'solo se inserta si es valida la fecha de inicio de la tarifa
      'para las tarifas periodicas, aqui tengo que ver en el detalle de la ctacte si ya hay una registrada con este id (TARCLIE_ID)....

      'tambien tengo que recuperar la config de dias para la tarifa periodica...ejemplo: Lu,Ma,Mi,Ju,Vi,Sa,Do
      Dim TARCLIE_ID As Integer = fila.Item("TARCLIE_ID")

      Dim ds_InfoDias As DataSet = daTarifa.TarifaCliente_procdiario002(TARCLIE_ID)
      If ds_InfoDias.Tables(0).Rows.Count <> 0 Then
        Dim valido As String = "si"
        Dim cont_altas As Integer = 0
        Dim CTACTE_ID As Integer = fila.Item("CTACTE_ID")
        Dim Importe As Decimal = fila.Item("TARCLIE_precio")
        Dim ds_CtaCteDetalle As DataSet = daCtaCte.CtaCteDetalle_obtenerxTARCLIE_ID(CTACTE_ID, TARCLIE_ID)
        'Dim TARCLIE_dias As Integer = fila.Item("TARCLIE_dias")



        Dim fechaaux As Date

        If ds_CtaCteDetalle.Tables(0).Rows.Count = 0 Then
          'es valido
          'como nunca se inserto la tarifa veo cuantas veces la inserto en base a los dias habilitados..ejemplo Lu,Ma,Mi,Ju,Vi,Sa,Do

          Dim aux1 As Date = TARCLIE_fechainicio
          While aux1 <= FechaIngreso
            Dim ii As Integer = 0
            While ii < ds_InfoDias.Tables(0).Rows.Count
              Dim dia_semana As String = ds_InfoDias.Tables(0).Rows(ii).Item("TARCLIEDIA_desc")

              Select Case dia_semana
                Case "Lu"
                  dia_semana = "lunes"
                Case "Ma"
                  dia_semana = "martes"
                Case "Mi"
                  dia_semana = "miércoles"
                Case "Ju"
                  dia_semana = "jueves"
                Case "Vi"
                  dia_semana = "viernes"
                Case "Sa"
                  dia_semana = "sábado"
                Case "Do"
                  dia_semana = "domingo"
              End Select
              Dim dia_aux As String = Format(aux1, "dddd").ToString
              If dia_semana = dia_aux Then
                '////////////////////////////////////////////////////////////////////////////////////////////////
                daCtaCte.CtaCteDetalle_alta(aux1, CTACTE_ID, Importe, "debe", "", TARCLIE_ID, Importe) 'nota: el saldodeudor=importe de la tarifa.
                Dim ds_ctacteinfo As DataSet = daCtaCte.CtaCte_obtenerID(CTACTE_ID)
                Dim CTACTE_saldodeudor As Decimal = ds_ctacteinfo.Tables(0).Rows(0).Item("CTACTE_saldodeudor")

                '  'SE MODIFICA EL SALDO DEUDOR EN CTACTE.CTACTE_saldodedor = CTACTE.CTACTE_saldodedor + TarifaCliente.TARCLIE_precio 
                CTACTE_saldodeudor = CTACTE_saldodeudor + Importe
                daCtaCte.CtaCte_SaldodeudorModif(CTACTE_ID, CTACTE_saldodeudor)

                '  'NO SE MODIFICA EL ESTADO EN TARIFACLIENTE, ya q esto va a depender del admin.
                '////////////////////////////////////////////////////////////////////////////////////////////////

                cont_altas = cont_altas + 1
                Exit While
              End If

              ii = ii + 1
            End While
            aux1 = aux1.AddDays(1)
          End While

          fechaaux = TARCLIE_fechainicio

        Else
          '...obtener la fecha de ese ultimo registro donde CtaCteDetalle.TARCLIE_ID = TarifaCliente.TARCLIE_ID y validar si es correcto agregar o no un nuevo registro.
          Dim FechaRegistro As Date = CDate(ds_CtaCteDetalle.Tables(0).Rows(0).Item("CTACTEDET_fecha"))

          'Dim aux1 As Date = FechaRegistro.AddDays(TARCLIE_dias)
          Dim aux1 As Date = FechaRegistro.AddDays(1)

          While aux1 <= FechaIngreso
            Dim ii As Integer = 0
            While ii < ds_InfoDias.Tables(0).Rows.Count
              Dim dia_semana As String = ds_InfoDias.Tables(0).Rows(ii).Item("TARCLIEDIA_desc")

              Select Case dia_semana
                Case "Lu"
                  dia_semana = "lunes"
                Case "Ma"
                  dia_semana = "martes"
                Case "Mi"
                  dia_semana = "miércoles"
                Case "Ju"
                  dia_semana = "jueves"
                Case "Vi"
                  dia_semana = "viernes"
                Case "Sa"
                  dia_semana = "sábado"
                Case "Do"
                  dia_semana = "domingo"
              End Select
              Dim dia_aux As String = Format(aux1, "dddd").ToString
              If dia_semana = dia_aux Then
                '////////////////////////////////////////////////////////////////////////////////////////////////
                daCtaCte.CtaCteDetalle_alta(aux1, CTACTE_ID, Importe, "debe", "", TARCLIE_ID, Importe) 'nota: el saldodeudor=importe de la tarifa.
                Dim ds_ctacteinfo As DataSet = daCtaCte.CtaCte_obtenerID(CTACTE_ID)
                Dim CTACTE_saldodeudor As Decimal = ds_ctacteinfo.Tables(0).Rows(0).Item("CTACTE_saldodeudor")

                '  'SE MODIFICA EL SALDO DEUDOR EN CTACTE.CTACTE_saldodedor = CTACTE.CTACTE_saldodedor + TarifaCliente.TARCLIE_precio 
                CTACTE_saldodeudor = CTACTE_saldodeudor + Importe
                daCtaCte.CtaCte_SaldodeudorModif(CTACTE_ID, CTACTE_saldodeudor)

                '  'NO SE MODIFICA EL ESTADO EN TARIFACLIENTE, ya q esto va a depender del admin.
                '////////////////////////////////////////////////////////////////////////////////////////////////


                cont_altas = cont_altas + 1
                Exit While
              End If

              ii = ii + 1
            End While
            aux1 = aux1.AddDays(1)
          End While





          'fechaaux = FechaRegistro.AddDays(TARCLIE_dias)


        End If

        If cont_altas <> 0 Then

          'Dim i As Integer = 0
          'While i < cont_altas

          '  daCtaCte.CtaCteDetalle_alta(fechaaux, CTACTE_ID, Importe, "debe", "", TARCLIE_ID, Importe) 'nota: el saldodeudor=importe de la tarifa.
          '  Dim ds_ctacteinfo As DataSet = daCtaCte.CtaCte_obtenerID(CTACTE_ID)
          '  Dim CTACTE_saldodeudor As Decimal = ds_ctacteinfo.Tables(0).Rows(0).Item("CTACTE_saldodeudor")

          '  'SE MODIFICA EL SALDO DEUDOR EN CTACTE.CTACTE_saldodedor = CTACTE.CTACTE_saldodedor + TarifaCliente.TARCLIE_precio 
          '  CTACTE_saldodeudor = CTACTE_saldodeudor + Importe
          '  daCtaCte.CtaCte_SaldodeudorModif(CTACTE_ID, CTACTE_saldodeudor)

          '  'NO SE MODIFICA EL ESTADO EN TARIFACLIENTE, ya q esto va a depender del admin.

          '  fechaaux = fechaaux.AddDays(TARCLIE_dias) - --comento choco 223

          '  i = i + 1
          'End While

        End If

      End If
    End If

  End Sub

  Private Sub TarifaPeriodica(ByVal fila As DataRow, ByVal FechaIngreso As Date)

    Dim TARCLIE_fechainicio As Date = CDate(fila.Item("TARCLIE_fechainicio"))
    If TARCLIE_fechainicio <= FechaIngreso Then 'solo se inserta si es valida la fecha de inicio de la tarifa
      'para las tarifas periodicas, aqui tengo que ver en el detalle de la ctacte si ya hay una registrada con este id (TARCLIE_ID)....

      Dim valido As String = "si"
      Dim cont_altas As Integer = 0

      Dim TARCLIE_ID As Integer = fila.Item("TARCLIE_ID")
      Dim CTACTE_ID As Integer = fila.Item("CTACTE_ID")
      Dim Importe As Decimal = fila.Item("TARCLIE_precio")
      Dim ds_CtaCteDetalle As DataSet = daCtaCte.CtaCteDetalle_obtenerxTARCLIE_ID(CTACTE_ID, TARCLIE_ID)
      Dim TARCLIE_dias As Integer = fila.Item("TARCLIE_dias")


      Dim fechaaux As Date

      If ds_CtaCteDetalle.Tables(0).Rows.Count = 0 Then
        'es valido
        'como nunca se inserto la tarifa veo cuantas veces la inserto en base a los dias

        Dim aux1 As Date = TARCLIE_fechainicio
        While aux1 <= FechaIngreso
          cont_altas = cont_altas + 1
          aux1 = aux1.AddDays(TARCLIE_dias)
        End While

        fechaaux = TARCLIE_fechainicio

      Else
        '...obtener la fecha de ese ultimo registro donde CtaCteDetalle.TARCLIE_ID = TarifaCliente.TARCLIE_ID y validar si es correcto agregar o no un nuevo registro.
        Dim FechaRegistro As Date = CDate(ds_CtaCteDetalle.Tables(0).Rows(0).Item("CTACTEDET_fecha"))

        Dim aux1 As Date = FechaRegistro.AddDays(TARCLIE_dias)
        While aux1 <= FechaIngreso
          cont_altas = cont_altas + 1
          aux1 = aux1.AddDays(TARCLIE_dias)
        End While

        fechaaux = FechaRegistro.AddDays(TARCLIE_dias)


      End If

      If cont_altas <> 0 Then

        Dim i As Integer = 0
        While i < cont_altas

          daCtaCte.CtaCteDetalle_alta(fechaaux, CTACTE_ID, Importe, "debe", "", TARCLIE_ID, Importe) 'nota: el saldodeudor=importe de la tarifa.
          Dim ds_ctacteinfo As DataSet = daCtaCte.CtaCte_obtenerID(CTACTE_ID)
          Dim CTACTE_saldodeudor As Decimal = ds_ctacteinfo.Tables(0).Rows(0).Item("CTACTE_saldodeudor")

          'SE MODIFICA EL SALDO DEUDOR EN CTACTE.CTACTE_saldodedor = CTACTE.CTACTE_saldodedor + TarifaCliente.TARCLIE_precio 
          CTACTE_saldodeudor = CTACTE_saldodeudor + Importe
          daCtaCte.CtaCte_SaldodeudorModif(CTACTE_ID, CTACTE_saldodeudor)

          'NO SE MODIFICA EL ESTADO EN TARIFACLIENTE, ya q esto va a depender del admin.

          fechaaux = fechaaux.AddDays(TARCLIE_dias)

          i = i + 1
        End While

      End If


    End If





    '.donde se van a ingresar tantas tarifas hasta llegar a la FechaIngreso. para eso validar contra TARCLIE_dias (me va a dar la periodicidad.)




    'insertamo en ctactedetalle

    'actalizamos saldodeudor en ctacte.



  End Sub


  Private Sub Btn_ok_complete_ServerClick(sender As Object, e As EventArgs) Handles Btn_ok_complete.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Btn_ok_complete_close_ServerClick(sender As Object, e As EventArgs) Handles Btn_ok_complete_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Inicio.aspx")
  End Sub
End Class
