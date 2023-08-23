Public Class LiquidacionFinal_Creditos
  Inherits System.Web.UI.Page
#Region "DECLARACIONES"
  Dim DaPrestamosCreditos As New Capa_Datos.WC_prestamoscreditos
  Dim DACtaCte As New Capa_Datos.WC_CtaCte
  Dim DACliente As New Capa_Datos.WB_clientes
  Dim DAParametro As New Capa_Datos.WC_parametro
  Dim DALiquidacion As New Capa_Datos.WC_Liquidacion
  Dim DAReliquidacion As New Capa_Datos.WC_Reliquidacion
  Dim DAconfiguracion As New Capa_Datos.Configuracion
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DAweb As New Capa_Datos.WC_Web
  Dim DAgrupos As New Capa_Datos.WC_grupos

#End Region

#Region "METODOS"
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
            If (Menu = "7" And Opcion = "") Or (Menu = "7" And Opcion = "1") Then
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
#End Region

#Region "EVENTOS"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      HF_fecha.Value = Session("fecha_parametro")
      Dim FECHA As Date = CDate(HF_fecha.Value)
      'LABEL_fecha_parametro.Text = FECHA.ToString("yyyy-MM-dd")
      LABEL_fecha_parametro.Text = FECHA.ToString("dd-MM-yyyy")

      HF_parametro_id.Value = Session("parametro_id")
      DAParametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal_Creditos.aspx") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.


      Dim DS_liqfinal As New DS_liqfinal
      DS_liqfinal.Tables("Creditos").Rows.Clear()

      '1) El proceso de cobro de creditos consiste en buscar los
      'creditos activos dbo.PrestamosCreditos.Estado = "A" y en donde dbo.PrestamosCreditos.Tipo = C
      Dim DS_Creditos As DataSet = DaPrestamosCreditos.Creditos_obtener
      If DS_Creditos.Tables(0).Rows.Count <> 0 Then
        Dim i As Integer = 0
        While i < DS_Creditos.Tables(0).Rows.Count
          Dim Cliente_ID As Integer = DS_Creditos.Tables(0).Rows(i).Item("Cliente_ID")
          Dim Cliente_Codigo As String = DS_Creditos.Tables(0).Rows(i).Item("Cliente_Codigo")

          Dim fecha_credito As Date = DS_Creditos.Tables(0).Rows(i).Item("Fecha")
          Dim ds_ctacte As DataSet = DACtaCte.CtaCte_obtener(CInt(Cliente_Codigo), HF_fecha.Value)
          '-------------------------------------------------------------------------------------------------------------------------
          '-------------------------------------------------------------------------------------------------------------------------
          '------------------------FECHA: 09-12-2022  SE REGISTRA EL CREDITO EN CTACTE---------------------------------------------
          If fecha_credito = HF_fecha.Value Then
            If ds_ctacte.Tables(0).Rows.Count <> 0 Then
              'NO modifico. Si existe ctacte quiere decir que se lo agrego en la etapa anterior de la liq fina. en el form "LiquidacionFinal.aspx"
              'Dim CreditoImporte As Decimal = DS_Creditos.Tables(0).Rows(i).Item("Saldo")
              'Dim IDCtaCte As Integer = ds_ctacte.Tables(0).Rows(0).Item("IDCtacte")
              'DALiquidacion.Credito_InsertCtaCte(IDCtaCte, CStr(CreditoImporte).Replace(",", "."))
              ''Actualizar el saldo dbo.Clientes.Saldo = dbo.Clientes.Saldo + credito
              'DACliente.Cliente_ActualizarSaldo_ctacte(Cliente_ID, CreditoImporte)
            Else
              'inserto
              Dim CreditoImporte As Decimal = DS_Creditos.Tables(0).Rows(i).Item("Saldo")
              Dim Grupo_id As String = CStr(DS_Creditos.Tables(0).Rows(i).Item("Grupo_id"))
              'vamos a crear un registro con todo en cero salvo SaldoAnterior.
              Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_id, CStr(Cliente_Codigo), HF_fecha.Value)
              Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
              'Dim CtaCte_Regalos As Decimal = SaldoRegalo
              DALiquidacion.Credito_InsertCtaCte(IdCtaCte_a, CStr(CreditoImporte).Replace(",", "."))
              'Actualizar el saldo dbo.Clientes.Saldo = dbo.Clientes.Saldo + credito
              DACliente.Cliente_ActualizarSaldo_ctacte(Cliente_ID, CreditoImporte)
              '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
              'FECHA: 2022-12-29.
              'NOTA: GUARDAR EN CTACTE.SALIDA EL TOTAL DEL SALDO FINAL CUANDO EL CLIENTE GANA, ES DECIR CUANDO EL MONTO ES NEGATIVO.
              'recupero saldo del cliente.  
              Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(CStr(Cliente_Codigo))
              Dim Clie_saldo As Decimal = 0
              Try
                Clie_saldo = ds_clie.Tables(0).Rows(0).Item("Saldo")
              Catch ex As Exception
              End Try
              If Clie_saldo < CDec(0) Then
                'recupero el id de la ctacta para la fecha a liquidar.
                Dim DS_CTACTE1 As DataSet = DACtaCte.CtaCte_obtener(CInt(Cliente_Codigo), HF_fecha.Value)
                If DS_CTACTE1.Tables(0).Rows.Count <> 0 Then
                  'Dim IdCtaCte As Integer = CInt(DS_CTACTE1.Tables(0).Rows(0).Item("IdCtaCte"))
                  DACtaCte.CtaCte_ActualizarSalida(CInt(Cliente_Codigo), HF_fecha.Value, CDec(Clie_saldo))
                End If
              End If
              '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            End If
          End If
          '-------------------------------------------------------------------------------------------------------------------------
          '-------------------------------------------------------------------------------------------------------------------------




          If fecha_credito <> HF_fecha.Value Then 'Si el credito fue dado de alta en la fecha del dia a liquidar no deberia ejecutarse ningun cobro.

            If ds_ctacte.Tables(0).Rows.Count <> 0 Then
              Dim IdCtaCte As Integer = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")
              Dim PrestamosCreditos_Saldo As Decimal = DS_Creditos.Tables(0).Rows(i).Item("Saldo")
              Dim PrestamosCreditos_IdPrestamoCredito As Integer = DS_Creditos.Tables(0).Rows(i).Item("Idprestamocredito")
              Dim PrestamosCreditos_cuota As Decimal = DS_Creditos.Tables(0).Rows(i).Item("Cuota_valor") 'ojo el valor de la cuota es 0 en la tabla
              'RECUPERO NRO ULTIMA CUOTA
              Dim DS_cuota As DataSet = DaPrestamosCreditos.CobroPrestamosCreditos_obtener_cuota(PrestamosCreditos_IdPrestamoCredito)
              Dim nro_cta_cobrada As Integer = 0 '---de donde lo saco...hago un select de todos los cobrosprestamoscreditos para ese id?
              If DS_cuota.Tables(0).Rows.Count <> 0 Then
                nro_cta_cobrada = CDec(DS_cuota.Tables(0).Rows(0).Item("Cuota")) + 1
              Else
                nro_cta_cobrada = CDec(1)
              End If

              Dim importe As Decimal = PrestamosCreditos_cuota
              'AQUI GUARDO EN BD
              DaPrestamosCreditos.CobroPrestamosCreditos_altaCredito(PrestamosCreditos_IdPrestamoCredito, HF_fecha.Value, importe, nro_cta_cobrada)

              '----------------------------------------------------------------------------------------------------------------------------------------------------
              'Actualizar el saldo del credito dbo.PrestamosCreditos.Saldo = dbo.PrestamosCreditos.Saldo - dbo.CobroPrestamosCreditos.Importe
              '(si el cobro del credito es de la ultima cuota se deberia marcar este credito como "Cancelado" dbo.PrestamosCreditos.Estado = C)
              PrestamosCreditos_Saldo = PrestamosCreditos_Saldo - PrestamosCreditos_cuota
              Dim Estado_id As Integer = 1 '1 activo, 2 cancelado, 3 baja
              If (PrestamosCreditos_Saldo = 0) Or (PrestamosCreditos_Saldo < 0) Then
                PrestamosCreditos_Saldo = 0
                'esta cancelado el prestamo
                Estado_id = 2
              End If

              'AQUI ACTUALIZO EN BD
              DaPrestamosCreditos.PrestamosCreditos_ActualizarSaldo(PrestamosCreditos_IdPrestamoCredito, PrestamosCreditos_Saldo, Estado_id)
              '----------------------------------------------------------------------------------------------------------------------------------------------------

              '----------------------------------------------------------------------------------------------------------------------------------------------------
              'Actualizar el campo dbo.CtaCte.CobCredito = dbo.CtaCte.CobCredito + dbo.CobroPrestamosCreditos.Importe
              Dim CobCredito As Decimal = 0
              Try 'el try lo uso x que el campo recuperado de ctacte es null
                CobCredito = ds_ctacte.Tables(0).Rows(0).Item("CobCredito") + importe
              Catch ex As Exception
                CobCredito = importe
              End Try


              DACtaCte.CtaCte_actualizarCobCredito(IdCtaCte, CobCredito)
              '----------------------------------------------------------------------------------------------------------------------------------------------------

              '----------------------------------------------------------------------------------------------------------------------------------------------------
              'Actualizar el saldo dbo.Clientes.Saldo = dbo.Clientes.Saldo + dbo.CtaCte.CobCredito
              DACliente.Cliente_ActualizarSaldo_ctacte2(Cliente_ID, importe)
              '----------------------------------------------------------------------------------------------------------------------------------------------------
              '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
              'FECHA: 2022-12-29.
              'NOTA: GUARDAR EN CTACTE.SALIDA EL TOTAL DEL SALDO FINAL CUANDO EL CLIENTE GANA, ES DECIR CUANDO EL MONTO ES NEGATIVO.
              'recupero saldo del cliente.  
              Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(CStr(Cliente_Codigo))
              Dim Clie_saldo As Decimal = 0
              Try
                Clie_saldo = ds_clie.Tables(0).Rows(0).Item("Saldo")
              Catch ex As Exception
              End Try
              If Clie_saldo < CDec(0) Then
                'recupero el id de la ctacta para la fecha a liquidar.
                Dim DS_CTACTE1 As DataSet = DACtaCte.CtaCte_obtener(CInt(Cliente_Codigo), HF_fecha.Value)
                If DS_CTACTE1.Tables(0).Rows.Count <> 0 Then
                  'Dim IdCtaCte As Integer = CInt(DS_CTACTE1.Tables(0).Rows(0).Item("IdCtaCte"))
                  DACtaCte.CtaCte_ActualizarSalida(CInt(Cliente_Codigo), HF_fecha.Value, CDec(Clie_saldo))
                End If
              End If
              '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
              ''---------------FECHA: 2022-11-11--------------SE AGREGA TABLA CONFIGURACION----------------------------------------------------------
              'Dim SaldosACero As Integer = 0 'de momento hay que considerar que el dbo.Configuracion.SaldosACero = 0. 
              'Try
              '  Dim ds_config As DataSet = DAconfiguracion.Configuracion_obtenertodo
              '  SaldosACero = CInt(ds_config.Tables(0).Rows(0).Item("SaldosACero"))
              'Catch ex As Exception
              'End Try
              'If SaldosACero = 1 Then
              '  'recupero info del cliente.
              '  Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(Cliente_Codigo)
              '  Dim Saldo As Decimal = CDec(ds_clie.Tables(0).Rows(0).Item("Saldo"))
              '  If Saldo < 0 Then 'si el saldo es negativo el Cliente Gana.
              '    DACtaCte.CtaCte_ActualizarSalida(CInt(Cliente_Codigo), CDate(HF_fecha.Value), Saldo) 'Guardar en el campo dbo.CtaCte.Salida el saldo final del cliente cuando el cliente gana, 
              '    DACliente.Clientes_ActualizarSaldo(Cliente_Codigo, Saldo, CDec(0)) 'y poner en 0 el saldo del cliente dbo.Clientes.Saldo = 0
              '  End If
              'End If
              ''------------------------------------------------------------------------------------------------------------------------------------

              'agrego 1 registro al datatable q voy a mandar al gridview.
              Dim fila As DataRow = DS_liqfinal.Tables("Creditos").NewRow
              fila("Cliente") = CInt(Cliente_Codigo)
              Dim FechaPrestamo_Ori As Date = DS_Creditos.Tables(0).Rows(i).Item("Fecha")
              fila("Fecha_Ori") = FechaPrestamo_Ori.ToString("dd-MM-yyyy")
              fila("Importe_Cob") = (Math.Round(importe, 2).ToString("N2"))
              fila("Cuota") = (Math.Round(nro_cta_cobrada, 2).ToString("N2"))
              fila("Saldo") = (Math.Round(PrestamosCreditos_Saldo, 2).ToString("N2"))

              Dim porcentaje As Decimal = DS_Creditos.Tables(0).Rows(i).Item("Porcentaje")
              Dim Interes As Decimal = porcentaje / 100
              Dim MontoInteres As Decimal = CDec(DS_Creditos.Tables(0).Rows(i).Item("Importe")) * Interes
              Dim Credito As Decimal = CDec(DS_Creditos.Tables(0).Rows(i).Item("Importe")) + MontoInteres
              '              Dim Credito As Decimal = DS_Creditos.Tables(0).Rows(i).Item("Importe")


              fila("Credito") = (Math.Round(Credito, 2).ToString("N2"))
              DS_liqfinal.Tables("Creditos").Rows.Add(fila)

            End If

          End If


          i = i + 1
        End While


      End If

      'ahora muestro DS_liqfinal.tables("Creditos") en un gridview.

      GridView1.DataSource = DS_liqfinal.Tables("Creditos")
      GridView1.DataBind()

      '------------------AQUIREPORTE ------------------------------------------------
      Dim fila_1 As DataRow = DS_liqfinal.Tables("Creditos_info").NewRow
      fila_1("Fecha") = CDate(HF_fecha.Value)
      DS_liqfinal.Tables("Creditos_info").Rows.Add(fila_1)
      Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
      CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
      CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/LiquidacionFinal_informe04.rpt"))
      CrReport.Database.Tables("Creditos").SetDataSource(DS_liqfinal.Tables("Creditos"))
      CrReport.Database.Tables("Creditos_info").SetDataSource(DS_liqfinal.Tables("Creditos_info"))
      CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/LiqFinal_CobCreditos.pdf"))

      '------------------------------------------------------------------------------

      If GridView1.Rows.Count = 0 Then
        Label_noprestamos.Visible = True
      End If


      '-------------------------------------------------------------------------------
      'FECHA: 2022-10-22
      'NOTA: se copian los registros de la tabla XCargas y XCargas_recorridos en la tabla XCrgHistoria, esto me sirve para llevar un registro para futuras consultas.

      DALiquidacion.LiquidacionFinal_XCrgHistoria_Guardar2()

      '-------------------------------------------------------------------------------

      DAParametro.Parametro_finalizar_dia(HF_fecha.Value)

      '----SECCION PARA RELIQUIDACION----------------------------------------------------
      '1) verifico si el ultimo registro de la tabla Parametro tiene el campo Terminales en False (0), si es asi tengo que pasar a la bd Web Central los registros de la tabla Copy.
      Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
      If ds_parametro.Tables(0).Rows.Count <> 0 Then
        Dim Reliquidacion As String = ""
        Try
          Reliquidacion = CStr(ds_parametro.Tables(0).Rows(0).Item("Reliquidacion"))
        Catch ex As Exception
          Reliquidacion = ""
        End Try

        If Reliquidacion.ToUpper = "SI" Then

          'entonces voy a pasar los registros nuevos de la BD copy a Web Central.
          'b) voy a consultar en la bd copia cual es el regist

          DAReliquidacion.Reliquidacion_ObtenerDeBkp()

        End If
      End If

      '-------------------------------------------------------------------------------
      'NOTA: ELIMINO LOS REGISTRO EN XCARGAS 1 A N.
      DALiquidacion.XCargas_delete()
      'fecha:11-08-2022
      '-------------------------------------------------------------------------------



      '------------------------------2023-03-17--------------------------------------
      'Liquidacion de grupos con clientedeuda, CONSISTE EN PASAR LO DE CTACTE DEJOGANO DE CADA CLIENTE A UN CLIENTE ESPECIFICO: "CLIENTE DEUDA" QUE SE CONFIGURO EN EL ABM DE GRUPO.
      Liquidacion_ClienteDeuda()

      '-----------------------------------------------------------------------------


      DAParametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal_Creditos.aspx, Actualizar ImprimeRegalo") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.
      '---------------------2023-04-26---------------------------------------------
      'se actualiza el campo ctacte.ImprimeRegalo  = Clientes.SaldoRegalo para todos los registros en ctacte.fecha = fecha de liquidacion.
      Actualizar_ImprimeRegalo()
      '----------------------------------------------------------------------------



      DAParametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal_Creditos.aspx, Liqfinal Completa") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.


      Try
        '---------------------------------------------------------------------------------------------------------------------
        'Modif 22-10-18 se intenta eliminar un bkp con la misma fecha y estado... ejemplo: "C:\BKPWC\WC_20220314A.bak"
        Dim fecha_año As String = CDate(HF_fecha.Value).Year.ToString
        Dim fecha_mes As String = CDate(HF_fecha.Value).Month.ToString
        Dim fecha_dia As String = CDate(HF_fecha.Value).Day.ToString
        If fecha_dia.ToString.Length = 1 Then
          fecha_dia = "0" + fecha_dia
        End If
        If fecha_mes.ToString.Length = 1 Then
          fecha_mes = "0" + fecha_mes
        End If
        Dim archivo As String = "C:\BKPWC\" + "WC_" + fecha_año + fecha_mes + fecha_dia + "T.bak"
        DAReliquidacion.Reliquidacion_DeleteBkp(archivo)
        '---------------------------------------------------------------------------------------------------------------------

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

        '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        'nota: se creará un backup de la bd posterior a la liquidacion solo se almacenara la fecha y una letra al final...en este caso "T" para indicar que es una copia POSTERIOR a la liquidacion final
        DALiquidacion.BACKUP("T", CDate(HF_fecha.Value))
        '------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


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



      Catch ex As Exception

      End Try

      'TERMINALES ------ a) al ultimo registro voy a poner en 1(true) el campo terminales
      DAReliquidacion.Reliquidacion_TerminalesEstado(1, CInt(HF_parametro_id.Value))



      btn_continuar.Focus()

      Try

        Session("OP") = "5" 'la opcion 5 es para generar un .zip con las 2 bd, WebCentral posterior a la Liq. y la BD Copy.
        Session("BKP_fecha") = CDate(HF_fecha.Value)

        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx','_blank','height=600px,width=600px,scrollbars=1');", True)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx');", True)


      Catch ex As Exception

      End Try


    End If
  End Sub

  Private Sub Actualizar_ImprimeRegalo()

    'recupero todos los registros de ctacte de la fecha de liquidacion.

    Dim ds_ctacte As DataSet = DALiquidacion.CtaCte_obtener_fecha(CDate(HF_fecha.Value))
    Dim i As Integer = 0
    While i < ds_ctacte.Tables(0).Rows.Count
      Dim IdCtaCte As String = CStr(ds_ctacte.Tables(0).Rows(i).Item("IdCtacte"))
      Dim Codigo As String = CStr(ds_ctacte.Tables(0).Rows(i).Item("Codigo"))
      DALiquidacion.CtaCte_ActualizarImprimeRegalo(IdCtaCte, Codigo)

      i = i + 1
    End While





  End Sub


  Private Sub Liquidacion_ClienteDeuda()
    'recupero primero todos los grupos
    Dim ds_grupos As DataSet = DAgrupos.Grupos_obtenertodos()
    Dim i As Integer = 0
    While i < ds_grupos.Tables(0).Rows.Count
      Dim Clientedeuda As Integer = 0
      Try
        Clientedeuda = ds_grupos.Tables(0).Rows(i).Item("Clientedeuda")
      Catch ex As Exception
      End Try
      If Clientedeuda <> 0 Then
        'obtener todos los registros de ctacte con fecha del dia y que correspondan a ese grupo.
        Dim Grupo_Id As Integer = ds_grupos.Tables(0).Rows(i).Item("Grupo_id")
        Dim DS_CTACTE As DataSet = DALiquidacion.CTACTE_obtenergrupo_fecha(HF_fecha.Value, CStr(Grupo_Id))
        Dim SUMDejo As Decimal = 0 'acumula los positivos
        Dim SumGano As Decimal = 0 'acumula los negativos
        Dim j As Integer = 0
        While j < DS_CTACTE.Tables(0).Rows.Count
          If CInt(DS_CTACTE.Tables(0).Rows(j).Item("Codigo")) <> Clientedeuda Then
            Dim Cliente_codigo As Integer = CInt(DS_CTACTE.Tables(0).Rows(j).Item("Codigo"))

            Dim CtaCte_DejoGano As Decimal = 0
            Try
              CtaCte_DejoGano = DS_CTACTE.Tables(0).Rows(j).Item("DejoGano")
            Catch ex As Exception
            End Try
            Dim CtaCte_DejoGanoSC As Decimal = 0
            Try
              CtaCte_DejoGanoSC = DS_CTACTE.Tables(0).Rows(j).Item("DejoGanoSC")
            Catch ex As Exception
            End Try
            Dim CtaCte_DejoGanoB As Decimal = 0
            Try
              CtaCte_DejoGanoB = DS_CTACTE.Tables(0).Rows(j).Item("DejoGanoB")
            Catch ex As Exception
            End Try

            If CtaCte_DejoGano > 0 Then
              SUMDejo = SUMDejo + CtaCte_DejoGano
            End If
            If CtaCte_DejoGano < 0 Then
              SumGano = SumGano + CtaCte_DejoGano
            End If

            If CtaCte_DejoGanoSC > 0 Then
              SUMDejo = SUMDejo + CtaCte_DejoGanoSC
            End If
            If CtaCte_DejoGanoSC < 0 Then
              SumGano = SumGano + CtaCte_DejoGanoSC
            End If

            If CtaCte_DejoGanoB > 0 Then
              SUMDejo = SUMDejo + CtaCte_DejoGanoB
            End If
            If CtaCte_DejoGanoB < 0 Then
              SumGano = SumGano + CtaCte_DejoGanoB
            End If

            'pongo en 0 saldo y traslado el saldo a saldo anterior...
            Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(CStr(Cliente_codigo))
            Dim SaldoAnterior As Decimal = ds_clie.Tables(0).Rows(0).Item("Saldo")

            DACliente.Clientes_ActualizarSaldo(CStr(Cliente_codigo), SaldoAnterior, CDec(0))
          End If
          j = j + 1
        End While

        'actualizo registro en tabla cliente , para el "cliente deuda"
        Dim ds_cliedeuda As DataSet = DACliente.Clientes_buscar_codigo(CStr(Clientedeuda))
        Dim SaldoAnt As Decimal = 0
        Try
          SaldoAnt = ds_cliedeuda.Tables(0).Rows(0).Item("Saldo")
        Catch ex As Exception
        End Try
        SumGano = (Math.Round(SumGano, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
        SUMDejo = (Math.Round(SUMDejo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
        Dim saldo = SaldoAnt + (SUMDejo + SumGano) 'estoy sumando SumGano ya que es un valor negativo (-x-=+)
        saldo = (Math.Round(saldo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
        DACliente.Clientes_ActualizarSaldo(CStr(Clientedeuda), SaldoAnt, saldo)

        'creo un registro en ctacte para el clientedeuda
        Dim ds_ctacte1 As DataSet = DALiquidacion.LiquidacionRegalos_obtenerctacte(Clientedeuda, HF_fecha.Value)

        If ds_ctacte1.Tables(0).Rows.Count <> 0 Then
          Dim IdCtaCte As Integer = ds_ctacte1.Tables(0).Rows(0).Item("IdCtaCte")
          DALiquidacion.CtaCte_ActualizarClienteDeuda(IdCtaCte, SUMDejo, SumGano)
        Else
          Dim ClienteDeuda_grupo_id As Integer = ds_cliedeuda.Tables(0).Rows(0).Item("Grupo_id")
          DALiquidacion.CtaCte_altaClienteDeuda(HF_fecha.Value, ClienteDeuda_grupo_id, Clientedeuda, SUMDejo, SumGano)
        End If
      End If

        i = i + 1
    End While



  End Sub

  Private Sub btn_continuar_ServerClick(sender As Object, e As EventArgs) Handles btn_continuar.ServerClick
    Session("fecha_parametro") = HF_fecha.Value
    Response.Redirect("~/Inicio.aspx")
  End Sub
#End Region

End Class
