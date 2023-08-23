Public Class LiquidacionRegalosMensual_det
  Inherits System.Web.UI.Page

#Region "DECLARACIONES"
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAliquidacion As New Capa_Datos.WC_Liquidacion
  Dim DAconfiguracion As New Capa_Datos.Configuracion
  Dim DACliente As New Capa_Datos.WB_clientes
  Dim DACtaCte As New Capa_Datos.WC_CtaCte
  Dim DAReliquidacion As New Capa_Datos.WC_Reliquidacion
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DAweb As New Capa_Datos.WC_Web
  Dim DAgrupos As New Capa_Datos.WC_grupos

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
        HF_parametro_id.Value = ds_parametro.Tables(0).Rows(0).Item("Parametro_id")
        HF_fecha.Value = ds_parametro.Tables(0).Rows(0).Item("Fecha")
        Dim FECHA As Date = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha"))
        LABEL_fecha_parametro.Text = FECHA.ToString("dd-MM-yyyy")
        Metodo1()

        '---------------------2023-04-26---------------------------------------------
        'se actualiza el campo ctacte.ImprimeRegalo  = Clientes.SaldoRegalo para todos los registros en ctacte.fecha = fecha de liquidacion.
        Actualizar_ImprimeRegalo()
        '----------------------------------------------------------------------------



        btn_continuar.Focus()




        Crear_BKP()

      End If
    End If
  End Sub

  Private Sub Actualizar_ImprimeRegalo()

    'recupero todos los registros de ctacte de la fecha de liquidacion.

    Dim ds_ctacte As DataSet = DAliquidacion.CtaCte_obtener_fecha(CDate(HF_fecha.Value))
    Dim i As Integer = 0
    While i < ds_ctacte.Tables(0).Rows.Count
      Dim IdCtaCte As String = CStr(ds_ctacte.Tables(0).Rows(i).Item("IdCtacte"))
      Dim Codigo As String = CStr(ds_ctacte.Tables(0).Rows(i).Item("Codigo"))
      DAliquidacion.CtaCte_ActualizarImprimeRegalo(IdCtaCte, Codigo)

      i = i + 1
    End While

  End Sub

  Private Sub btn_continuar_ServerClick(sender As Object, e As EventArgs) Handles btn_continuar.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub
#End Region

#Region "METODOS"
  Private Sub Metodo1()
    'Buscar todos los clientes que tengan en el campo dbo.clientes.Proceso = "M".
    Dim ds_clientes As DataSet = DAliquidacion.LiquidacionRegalos_obtener_ClieMensual

    Dim DS_liqregalos As New DS_liqregalos
    DS_liqregalos.Tables("Mensual").Rows.Clear()

    If ds_clientes.Tables(0).Rows.Count <> 0 Then
      '//////////////////////////////////////////////////////////////////////////////////////////////////////////
      'colocamos el campo LiqRegalos = "S" en la tabla Parametro.
      Dim DS_Parametro As DataSet = DAliquidacion.LiqRegalos_BuscarEnParametro(HF_fecha.Value)
      Dim LiqRegalos_tipo As String = ""
      Try
        LiqRegalos_tipo = CStr(DS_Parametro.Tables(0).Rows(0).Item("LiqRegalos"))
      Catch ex As Exception
      End Try
      Dim FirstCharacter As Integer = LiqRegalos_tipo.IndexOf("S")
      If LiqRegalos_tipo.IndexOf("M") = -1 Then 'IndexOf devuelve -1 cuando no encuentra M en cadena.
        'solo agrego M si no existe en la cadena.
        LiqRegalos_tipo = LiqRegalos_tipo + "M"
        DAliquidacion.LiqRegalos_ActualizarTablaParametro(LiqRegalos_tipo, HF_fecha.Value)
      End If
      '//////////////////////////////////////////////////////////////////////////////////////////////////////////
      Dim i As Integer = 0
      While i < ds_clientes.Tables(0).Rows.Count
        Dim Cliente_ID As Integer = CInt(ds_clientes.Tables(0).Rows(i).Item("Cliente"))
        Dim Grupo_Id As String = CStr(ds_clientes.Tables(0).Rows(i).Item("Grupo_id"))
        Dim Cliente_Codigo As Integer = CInt(ds_clientes.Tables(0).Rows(i).Item("Codigo"))
        Dim SaldoRegalo As Decimal = CDec(ds_clientes.Tables(0).Rows(i).Item("SaldoRegalo"))

        'Buscar todos los clientes que en el campo dbo.Clientes.SaldoRegalo tengan un valor en negativo. 
        If SaldoRegalo <> 0 Then 'modificacion: 07-12-2022 , ahora se van a pasar saldos negativos y positivos.
          'obtener cta cta para la fecha de la ultima liquidacion completada.
          Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Cliente_Codigo, HF_fecha.Value)
          Dim IdCtaCte As Integer = 0
          If ds_ctacte.Tables(0).Rows.Count <> 0 Then
            IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")
            'dbo.CtaCte.Regalos = dbo.Clientes.SaldoRegalo
            Dim CtaCte_Regalos As Decimal = SaldoRegalo
            DAliquidacion.LiquidacionRegalosMensual_actualizarCtaCte(IdCtaCte, CtaCte_Regalos)

            '            dbo.Clientes.SaldoRegalo = 0
            '           dbo.Clientes.Saldo = dbo.Clientes.Saldo - dbo.CtaCte.Regalos.
            'SaldoRegalo = 0
            'Dim Clientes_Saldo As Decimal = ds_clientes.Tables(0).Rows(i).Item("Saldo")
            'Clientes_Saldo = Clientes_Saldo + CtaCte_Regalos

            'DAliquidacion.LiquidacionRegalosMensual_actualizarClie(Cliente_ID, SaldoRegalo, Clientes_Saldo)

          Else
            'si no existe, creo un registro en ctacte?
            'vamos a crear un registro con todo en cero salvo, regalo.
            Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Cliente_Codigo), HF_fecha.Value)
            Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
            Dim CtaCte_Regalos As Decimal = SaldoRegalo
            DAliquidacion.LiquidacionRegalosMensual_actualizarCtaCte(IdCtaCte_a, CtaCte_Regalos)

          End If

        End If

        i = i + 1
      End While


      Dim ds_clie As DataSet = DAliquidacion.LiquidacionRegalos_obtener_ClieMensual
      If ds_clie.Tables(0).Rows.Count <> 0 Then


        Dim filaa As DataRow = DS_liqregalos.Tables("Mensual").NewRow
        filaa("Cliente") = "Cliente"
        'filaa("Monto_Favor") = ""
        filaa("Favor") = ""
        filaa("Contra") = ""
        'filaa("Monto_Contra") = ""
        DS_liqregalos.Tables("Mensual").Rows.Add(filaa)

        Dim ii As Integer = 0
        While ii < ds_clie.Tables(0).Rows.Count
          Dim Cliente_Codigo As Integer = CInt(ds_clie.Tables(0).Rows(ii).Item("Codigo"))
          Dim CLiente_SaldoRegalo As Decimal = CDec(ds_clie.Tables(0).Rows(ii).Item("SaldoRegalo"))
          If CLiente_SaldoRegalo <> CDec(0) Then '- NO HACE FALTA MOSTRAR TODOS LOS CLIENTES QUE TENGAN LA CONDICION QUE SE LIQUIDO SI EL dbo.clientes.SaldoRegalo = 0
            Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Cliente_Codigo, HF_fecha.Value)
            Dim IdCtaCte As Integer = 0
            Dim Monto_contra As Decimal = 0
            Dim Monto_favor As Decimal = 0
            Dim Contra As String = ""
            Dim Favor As String = ""
            If ds_ctacte.Tables(0).Rows.Count <> 0 Then
              If ds_ctacte.Tables(0).Rows(0).Item("Regalos") < CDec(0) Then 'es a favor si es negativo
                Monto_favor = ds_ctacte.Tables(0).Rows(0).Item("Regalos")
                Favor = "A FAVOR"

                '------AQUI SE ACTUALIZA EL LA TABLA CLIENTE EL "SALDO" Y "SALDOREGALO"------------------
                Dim Cliente_ID As Integer = CInt(ds_clie.Tables(0).Rows(ii).Item("Cliente"))
                Dim SaldoRegalo As Decimal = 0
                SaldoRegalo = 0
                Dim Clientes_Saldo As Decimal = ds_clientes.Tables(0).Rows(ii).Item("Saldo")
                Dim CtaCte_Regalos As Decimal = ds_ctacte.Tables(0).Rows(0).Item("Regalos")
                Clientes_Saldo = Clientes_Saldo + CtaCte_Regalos
                DAliquidacion.LiquidacionRegalosMensual_actualizarClie(Cliente_ID, SaldoRegalo, Clientes_Saldo)
                '-----------------------------------------------------------------------------------------
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                'FECHA: 2022-12-29.
                'NOTA: GUARDAR EN CTACTE.SALIDA EL TOTAL DEL SALDO FINAL CUANDO EL CLIENTE GANA, ES DECIR CUANDO EL MONTO ES NEGATIVO.
                'recupero saldo del cliente.  
                Dim ds_cliee As DataSet = DACliente.Clientes_buscar_codigo(CStr(Cliente_Codigo))
                Dim Clie_saldo As Decimal = 0
                Try
                  Clie_saldo = ds_cliee.Tables(0).Rows(0).Item("Saldo")
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

                '--------------------------------2023-03-17: CLIENTE DEUDA----------------------------------------------------------------------------
                Dim Grupo_id As Integer = ds_clie.Tables(0).Rows(ii).Item("Grupo_id")
                Dim ds_grupo As DataSet = DAgrupos.Grupo_buscarID(CStr(Grupo_id))
                Dim Clientedeuda As Integer = 0
                Try
                  Clientedeuda = ds_grupo.Tables(0).Rows(0).Item("Clientedeuda")
                Catch ex As Exception
                End Try
                If (Clientedeuda <> 0) And (Cliente_Codigo <> Clientedeuda) Then
                  '/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º
                  'pongo en 0 saldo y traslado el saldo a saldo anterior...
                  Dim ds_clie1 As DataSet = DACliente.Clientes_buscar_codigo(CStr(Cliente_Codigo))
                  Dim SaldoAnterior As Decimal = ds_clie1.Tables(0).Rows(0).Item("Saldoanterior") '---mantengo el mismo valor
                  DACliente.Clientes_ActualizarSaldo(CStr(Cliente_Codigo), SaldoAnterior, CDec(0)) '---pongo en 0 saldo

                  '/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º

                  'actualizo registro en tabla cliente , para el "cliente deuda"
                  Dim ds_cliedeuda As DataSet = DACliente.Clientes_buscar_codigo(CStr(Clientedeuda))
                  Dim SaldoAnt As Decimal = 0
                  Try
                    SaldoAnt = ds_cliedeuda.Tables(0).Rows(0).Item("Saldoanterior") '-----mantengo el mismo valor
                  Catch ex As Exception
                  End Try
                  Dim Saldo As Decimal = 0
                  Try
                    Saldo = ds_cliedeuda.Tables(0).Rows(0).Item("Saldo")
                  Catch ex As Exception
                  End Try
                  Dim SUMsaldo = Saldo + (CtaCte_Regalos) 'estoy sumando solo regalos
                  SUMsaldo = (Math.Round(SUMsaldo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                  DACliente.Clientes_ActualizarSaldo(CStr(Clientedeuda), SaldoAnt, SUMsaldo)
                  '/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º
                  Dim SUMDejo As Decimal = 0 'acumula los positivos
                  Dim SumGano As Decimal = 0 'acumula los negativos

                  Dim DS_CTACTE1 As DataSet = DACtaCte.CtaCte_obtener(CInt(Clientedeuda), HF_fecha.Value)
                  If DS_CTACTE1.Tables(0).Rows.Count <> 0 Then
                    Dim CtaCte_DejoGano As Decimal = 0
                    Try
                      CtaCte_DejoGano = DS_CTACTE1.Tables(0).Rows(0).Item("DejoGano")
                    Catch ex As Exception
                    End Try
                    Dim CtaCte_DejoGanoSC As Decimal = 0
                    Try
                      CtaCte_DejoGanoSC = DS_CTACTE1.Tables(0).Rows(0).Item("DejoGanoSC")
                    Catch ex As Exception
                    End Try
                    SUMDejo = CtaCte_DejoGano
                    SumGano = CtaCte_DejoGanoSC
                    If CtaCte_Regalos > CDec(0) Then
                      SUMDejo = CtaCte_Regalos + SUMDejo
                    End If
                    If CtaCte_Regalos < CDec(0) Then
                      SumGano = CtaCte_Regalos + SumGano
                    End If
                    SumGano = (Math.Round(SumGano, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                    SUMDejo = (Math.Round(SUMDejo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

                    DAliquidacion.CtaCte_ActualizarClienteDeuda(DS_CTACTE1.Tables(0).Rows(0).Item("IdCtacte"), SUMDejo, SumGano)
                  Else
                    'no existe, creo el registro en ctacte
                    If CtaCte_Regalos > CDec(0) Then
                      SUMDejo = CtaCte_Regalos
                    End If
                    If CtaCte_Regalos < CDec(0) Then
                      SumGano = CtaCte_Regalos
                    End If
                    SumGano = (Math.Round(SumGano, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                    SUMDejo = (Math.Round(SUMDejo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                    Dim ClienteDeuda_grupo_id As Integer = ds_cliedeuda.Tables(0).Rows(0).Item("Grupo_id")
                    DAliquidacion.CtaCte_altaClienteDeuda(HF_fecha.Value, ClienteDeuda_grupo_id, Clientedeuda, SUMDejo, SumGano)
                  End If
                  '/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º/º
                End If
                '--------------------------------------------------------------------------------------------






                ''---------------FECHA: 2022-11-11--------------SE AGREGA TABLA CONFIGURACION----------------------------------------------------------
                'Dim SaldosACero As Integer = 0 'de momento hay que considerar que el dbo.Configuracion.SaldosACero = 0. 
                'Try
                '  Dim ds_config As DataSet = DAconfiguracion.Configuracion_obtenertodo
                '  SaldosACero = CInt(ds_config.Tables(0).Rows(0).Item("SaldosACero"))
                'Catch ex As Exception
                'End Try
                'If SaldosACero = 1 Then
                '  'recupero info del cliente.
                '  Dim ds_cliente As DataSet = DACliente.Clientes_buscar_codigo(CStr(Cliente_Codigo))
                '  Dim Saldo As Decimal = CDec(ds_cliente.Tables(0).Rows(0).Item("Saldo"))
                '  If Saldo < 0 Then 'si el saldo es negativo el Cliente Gana.
                '    DACtaCte.CtaCte_ActualizarSalida(Cliente_Codigo, CDate(HF_fecha.Value), Saldo) 'Guardar en el campo dbo.CtaCte.Salida el saldo final del cliente cuando el cliente gana, 
                '    DACliente.Clientes_ActualizarSaldo(CStr(Cliente_Codigo), Saldo, CDec(0)) 'y poner en 0 el saldo del cliente dbo.Clientes.Saldo = 0
                '  End If
                'End If
                ''------------------------------------------------------------------------------------------------------------------------------------

              Else
                Monto_contra = CLiente_SaldoRegalo
                Contra = "EN CONTRA"
              End If

            Else
              Monto_contra = CLiente_SaldoRegalo
              Contra = "EN CONTRA"
            End If
            Dim fila As DataRow = DS_liqregalos.Tables("Mensual").NewRow
            fila("Cliente") = CStr(Cliente_Codigo)
            If Monto_favor <> 0 Then
              fila("Monto_Favor") = Monto_favor
            End If
            fila("Favor") = Favor
            fila("Contra") = Contra
            If Monto_contra <> 0 Then
              fila("Monto_Contra") = Monto_contra
            End If
            DS_liqregalos.Tables("Mensual").Rows.Add(fila)
          End If


          ii = ii + 1
        End While

        If DS_liqregalos.Tables("Mensual").Rows.Count > 1 Then
          'si es mayor a 1 entonces hay varios registros...voy a calcular el total
          Dim Total As Decimal = CDec(0)
          Dim j As Integer = 1
          While j < DS_liqregalos.Tables("Mensual").Rows.Count
            Dim Monto_Favor As Decimal = 0
            Try
              Monto_Favor = CDec(DS_liqregalos.Tables("Mensual").Rows(j).Item("Monto_Favor"))
            Catch ex As Exception
              Monto_Favor = 0
            End Try
            Total = Total + Monto_Favor
            j = j + 1
          End While

          Dim fila1 As DataRow = DS_liqregalos.Tables("Mensual").NewRow
          fila1("Cliente") = "TOTAL:"
          fila1("Monto_Favor") = Total
          fila1("Favor") = ""
          fila1("Contra") = ""
          'fila1("Monto_Contra") = ""
          DS_liqregalos.Tables("Mensual").Rows.Add(fila1)

        Else
          DS_liqregalos.Tables("Mensual").Rows.Clear()
          Label_noregalos.Visible = True
        End If

        GridView1.DataSource = DS_liqregalos.Tables("Mensual")
        GridView1.DataBind()

      End If


    Else
      'aqui msj?
      Label_noregalos.Visible = True
    End If

    '----AQUI GENERO REPORTE-------

    'NOTA: PARA EL REPORTE VOY A QUITAR DE DS_liqregalos.Tables("Diario") el ultimo registro que vendria a ser el "TOTAL", ya que en el reporte se lo va agregar como un campo de "corte de control".
    If DS_liqregalos.Tables("Mensual").Rows.Count > 1 Then
      Dim ultimo_registro As Integer = DS_liqregalos.Tables("Mensual").Rows.Count - 1
      DS_liqregalos.Tables("Mensual").Rows.RemoveAt(ultimo_registro)

    End If

    Dim fila_1 As DataRow = DS_liqregalos.Tables("Mensual_info").NewRow
    fila_1("Fecha") = CDate(HF_fecha.Value)
    DS_liqregalos.Tables("Mensual_info").Rows.Add(fila_1)
    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
    CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/LiquidacionRegalos_informe03.rpt"))
    CrReport.Database.Tables("Mensual").SetDataSource(DS_liqregalos.Tables("Mensual"))
    CrReport.Database.Tables("Mensual_info").SetDataSource(DS_liqregalos.Tables("Mensual_info"))
    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/LiqRegalos_Mensual.pdf"))

    '------------------------------



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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 3 or null

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
            If (Menu = "8" And Opcion = "") Or (Menu = "8" And Opcion = "3") Then
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
  Private Sub Crear_BKP()
    '//////////borrar back T y crear uno nuevo.//////////////////////////////////
    'Modif 22-10-18 se intenta eliminar un bkp con la misma fecha y estado... ejemplo: "C:\BKPWC\WC_20220314A.bak"
    Dim ds_parametro As DataSet = DAparametro.Parametro_consultar_ultliq
    If ds_parametro.Tables(0).Rows.Count <> 0 Then
      Dim fecha_año As String = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha")).Year.ToString
      Dim fecha_mes As String = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha")).Month.ToString
      Dim fecha_dia As String = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha")).Day.ToString
      If fecha_dia.ToString.Length = 1 Then
        fecha_dia = "0" + fecha_dia
      End If
      If fecha_mes.ToString.Length = 1 Then
        fecha_mes = "0" + fecha_mes
      End If
      Dim archivo As String = "C:\BKPWC\" + "WC_" + fecha_año + fecha_mes + fecha_dia + "T.bak"
      DAReliquidacion.Reliquidacion_DeleteBkp(archivo)

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
      DAliquidacion.BACKUP("T", CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha")))
      '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

      '///////////////////////SE EJECUTA EL SP WebHabilitar solo si Configuracion.Web=true/////////////////////////////////

      If ds_config.Tables(0).Rows.Count <> 0 Then
        If ds_config.Tables(0).Rows(0).Item("Web") = True Then
          Try
            DAweb.WebHabilitar()
          Catch ex As Exception
          End Try
        End If
      End If
      '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      Try

        Session("OP") = "5" 'la opcion 5 es para generar un .zip con las 2 bd, WebCentral posterior a la Liq. y la BD Copy.
        Session("BKP_fecha") = CDate(ds_parametro.Tables(0).Rows(0).Item("Fecha"))

        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx','_blank','height=600px,width=600px,scrollbars=1');", True)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx');", True)


      Catch ex As Exception

      End Try



    End If
  End Sub


#End Region



End Class
