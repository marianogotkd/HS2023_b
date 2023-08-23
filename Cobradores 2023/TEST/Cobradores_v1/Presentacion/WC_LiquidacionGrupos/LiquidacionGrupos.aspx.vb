Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO

Public Class LiquidacionGrupos
  Inherits System.Web.UI.Page
  Dim DAgrupos As New Capa_Datos.WC_grupos
  Dim DAliquidacion As New Capa_Datos.WC_Liquidacion
  Dim DAParametro As New Capa_Datos.WC_parametro
  Dim DACtaCte As New Capa_Datos.WC_CtaCte
  Dim DAReliquidacion As New Capa_Datos.WC_Reliquidacion
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos
      Txt_fecha_desde.Focus()
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
            If (Menu = "10" And Opcion = "") Or (Menu = "10" And Opcion = "1") Then
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

  Private Sub Txt_fecha_desde_Init(sender As Object, e As EventArgs) Handles Txt_fecha_desde.Init
    Txt_fecha_desde.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_fecha_hasta_Init(sender As Object, e As EventArgs) Handles Txt_fecha_hasta.Init
    Txt_fecha_hasta.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_grupo_codigo_Init(sender As Object, e As EventArgs) Handles Txt_grupo_codigo.Init
    Txt_grupo_codigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Actualizar_Saldo_ClientePorcentaje(ByVal Codigo As String, ByVal Regalo As Decimal)
    'NOTA: 2023-01-17 FALTABA ACTUALIZAR EL SALDO Y SALDO ANTERIOR EN LA TABLA CLIENTES 
    Dim DACliente As New Capa_Datos.WB_clientes
    Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(Codigo)
    Try
      Dim SaldoAnterior As Decimal = ds_clie.Tables(0).Rows(0).Item("Saldo")
      Dim Saldo As Decimal = ds_clie.Tables(0).Rows(0).Item("Saldo")
      Saldo = Saldo + Regalo
      DACliente.Clientes_ActualizarSaldo(Codigo, SaldoAnterior, Saldo)
    Catch ex As Exception

    End Try

  End Sub

  Dim DABackup As New Capa_Datos.WC_Backup
  Private Sub btn_modificar_ServerClick(sender As Object, e As EventArgs) Handles btn_modificar.ServerClick

    'Session("OP") = "3" 'la opcion 3 es para generar un .zip con las 2 bd, WebCentral y Copy.

    'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx','_blank','height=600px,width=600px,scrollbars=1');", True)
    'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx');", True)


    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-msj_continue", "$(document).ready(function () {$('#modal-msj_continue').modal();});", True)


    Dim DS_liqgrupos As New DS_liqgrupos

    'valido el ingreso del rango de fechas
    Dim valido = "si"

    Dim fecha_desde As Date = Today
    Try
      fecha_desde = CDate(Txt_fecha_desde.Text)
    Catch ex As Exception
      valido = "no"
    End Try
    Dim fecha_hasta As Date = Today
    Try
      fecha_hasta = CDate(Txt_fecha_hasta.Text)
    Catch ex As Exception
      valido = "no"
    End Try

    If Txt_grupo_codigo.Text = "" Then
      valido = "no"
    End If

    If valido = "si" Then
      If (fecha_hasta > fecha_desde) Or (fecha_desde = fecha_hasta) Then

        If Txt_grupo_codigo.Text = "999" Then
          'se realiza una consulta para todos los grupos
          Dim ds_grupo As DataSet = DAgrupos.Grupos_obtenertodos
          If ds_grupo.Tables(0).Rows.Count <> 0 Then
            Dim ii As Integer = 0
            While ii < ds_grupo.Tables(0).Rows.Count
              Dim Grupo_Id As Integer = ds_grupo.Tables(0).Rows(ii).Item("Grupo_id")
              Dim Grupo_Codigo As String = ds_grupo.Tables(0).Rows(ii).Item("Codigo")

              If Grupo_Codigo = "19" Then
                Dim observacion = "este es el codigo a controlar"
              End If


              Dim Grupo_Nombre As String = ds_grupo.Tables(0).Rows(ii).Item("Nombre")
              Dim Grupo_Tipo As String = ds_grupo.Tables(0).Rows(ii).Item("Tipo")
              Dim Clienteporcentaje As Integer = 0
              Try
                Clienteporcentaje = ds_grupo.Tables(0).Rows(ii).Item("Clienteporcentaje")
              Catch ex As Exception
              End Try



              If Grupo_Tipo = "3" Or Grupo_Tipo = "4" Or Grupo_Tipo = "2" Then 'correccion: 20-03-2023 'se agregó tipo 2
                Dim fila As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                fila("Columna1") = ""
                fila("Columna2") = "RESULTADO DEL PERIODO " + CDate(Txt_fecha_desde.Text).ToString("dd-MM-yyyy") + " AL " + CDate(Txt_fecha_hasta.Text).ToString("dd-MM-yyyy")
                'fila("Columna3") = "
                fila("Columna4") = ""
                DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila)

                Dim fila2 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                fila2("Columna1") = ""
                fila2("Columna2") = "GRUPO:" + Grupo_Codigo + " " + Grupo_Nombre.ToString.ToUpper
                'fila2("Columna3") = ""
                fila2("Columna4") = ""
                DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila2)


                Dim Grupo_fecha As Date = CDate(ds_grupo.Tables(0).Rows(ii).Item("Fecha"))
                'paso el valor de Importe a SaldoAnterior en la tabla Grupo
                DAliquidacion.LiquidacionGrupo_modifSaldoAnterior(CStr(Grupo_Id))
                Dim ds_grup As DataSet = DAgrupos.Grupos_buscar_codigo(Grupo_Codigo)

                Dim Grupo_SaldoAnterior As Decimal = CDec(ds_grup.Tables(0).Rows(0).Item("Saldoanterior")) 'se toma del importe de procesamiento, Elias: 2022-12-06
                Dim Grupo_CodigoCobro As String = ds_grupo.Tables(0).Rows(ii).Item("Codigocobro")
                Dim Grupo_Porcentaje As Decimal = ds_grupo.Tables(0).Rows(ii).Item("Porcentaje")
                Dim ds_ctacteGrupo As DataSet = DAliquidacion.LiquidacionGrupos_ObtenerCtaCtexrangofecha(fecha_desde, fecha_hasta, Grupo_Id)

                Dim fila3 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                fila3("Columna1") = ""
                fila3("Columna2") = "SALDO ANTERIOR AL " + Grupo_fecha
                fila3("Columna3") = (Math.Round(Grupo_SaldoAnterior, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                fila3("Columna4") = ""
                DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila3)

                Dim CtaCte_DejoGano As Decimal = 0
                Dim CtaCte_DejoGanoSC As Decimal = 0
                Dim CtaCte_DejoGanoB As Decimal = 0

                Dim TrabajoPeriodo As Decimal = 0

                If ds_ctacteGrupo.Tables(0).Rows.Count <> 0 Then
                  '-------CALCULO DE "TRABAJO EN PERIODO"
                  Dim i As Integer = 0

                  While i < ds_ctacteGrupo.Tables(0).Rows.Count
                    Dim Regalos As Decimal = 0
                    Try
                      Regalos = CDec(ds_ctacteGrupo.Tables(0).Rows(i).Item("Regalos"))
                    Catch ex As Exception
                    End Try
                    Dim DejoGano As Decimal = CDec(ds_ctacteGrupo.Tables(0).Rows(i).Item("DejoGano"))
                    If Regalos < CDec(0) Then
                      DejoGano = DejoGano + Regalos
                    End If
                    CtaCte_DejoGano = CtaCte_DejoGano + DejoGano
                    CtaCte_DejoGanoSC = CtaCte_DejoGanoSC + CDec(ds_ctacteGrupo.Tables(0).Rows(i).Item("DejoGanoSC"))
                    CtaCte_DejoGanoB = CtaCte_DejoGanoB + CDec(ds_ctacteGrupo.Tables(0).Rows(i).Item("DejoGanoB"))
                    i = i + 1
                  End While

                  Select Case Grupo_CodigoCobro
                    Case "1"
                      'Si dbo.Grupos.CodigoCobro = 1,
                      'EL TRABAJO EN EL PERIODO = dbo.CtaCte.DejoGano + dbo.CtaCte.DejoGanoSC + dbo.CtaCte.DejoGanoB
                      'entre las fechas seleccionadas para la liquidacion.
                      '(Si el resultado del calculo es positivo se colocara la siguiente referencia ++DEJO,
                      'si el resultado del calculo es negativo se colocara la siguiente referencia --GANO)
                      TrabajoPeriodo = CtaCte_DejoGano + CtaCte_DejoGanoSC + CtaCte_DejoGanoB
                    Case "2"
                      TrabajoPeriodo = CtaCte_DejoGano + CtaCte_DejoGanoSC
                    Case "3"
                      TrabajoPeriodo = CtaCte_DejoGanoB
                    Case "4"
                      TrabajoPeriodo = CtaCte_DejoGano
                  End Select
                Else
                  'msj, no hay resultados para el rango de fecha
                End If

                Dim fila4 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                If TrabajoPeriodo > 0 Then
                  fila4("Columna1") = "++DEJO"

                End If
                If TrabajoPeriodo < 0 Then
                  fila4("Columna1") = "--GANO"
                End If
                fila4("Columna2") = "EL TRABAJO EN EL PERIODO"
                fila4("Columna3") = (Math.Round(TrabajoPeriodo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                fila4("Columna4") = ""
                DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila4)


                '///////////////////////////////////////////////////////////////////////////////////////////////////////
                'El total de gastos es igual a la suma de los gastos dbo.Gastos.Importe cargados al grupo entre las fechas seleccionadas para la liquidacion.
                Dim DS_Gastos As DataSet = DAliquidacion.LiquidacionGrupos_ObtenerGastosxrangofecha(fecha_desde, fecha_hasta, Grupo_Id)
                Dim Gastos_importe As Decimal = 0
                If DS_Gastos.Tables(0).Rows.Count <> 0 Then
                  Dim j As Integer = 0
                  While j < DS_Gastos.Tables(0).Rows.Count
                    Gastos_importe = Gastos_importe + CDec(DS_Gastos.Tables(0).Rows(j).Item("Importe"))
                    j = j + 1
                  End While
                Else
                  'No se registran gastos para el rango de fecha

                End If

                Gastos_importe = Gastos_importe * -1



                Dim fila5 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                fila5("Columna1") = ""
                fila5("Columna2") = "GASTOS"
                fila5("Columna3") = (Math.Round(Gastos_importe, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                fila5("Columna4") = ""
                DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila5)

                '///////////////////////////////////////////////////////////////////////////////////////////////////////

                Dim Saldo_periodo As Decimal = Grupo_SaldoAnterior + TrabajoPeriodo + Gastos_importe
                Dim fila6 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                fila6("Columna1") = ""
                fila6("Columna2") = "SALDO PARA EL PERIODO"


                fila6("Columna3") = (Math.Round(Saldo_periodo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                If Saldo_periodo > 0 Then
                  fila6("Columna4") = "++DEJO"
                End If
                If Saldo_periodo < 0 Then
                  fila6("Columna4") = "--GANO"
                End If
                DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila6)

                If Grupo_Tipo = "3" Then
                  'Si el saldo para el periodo es positivo y el grupo es de tipo=3 (% del grupo) se debe
                  'calcular que % le corresponde al socio. Esto se obtine de multiplicar el % dbo.Grupos.Porcentaje * el total del "SALDO PARA EL PERIODO"
                  Dim porcentaje_socio As Decimal = 0
                  If Saldo_periodo > 0 And Grupo_Tipo = "3" Then  '---++DEJO
                    porcentaje_socio = (Saldo_periodo * Grupo_Porcentaje) / 100
                    Dim fila7 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                    fila7("Columna1") = ""
                    fila7("Columna2") = "% CORRESPONDIENTE AL SOCIO"
                    If porcentaje_socio <> 0 Then
                      fila7("Columna3") = (Math.Round(porcentaje_socio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------
                      '-----------------CORRECCION 2022-12-22---------------------------------------------------
                      'Ver si el grupo tiene un codigo de cliente en el campo Grupos.Clienteporcentaje.
                      'Si es distinto de 0. se va a grabar en un registro de ctacte. CtaCte.Regalos = porcentaje_socio
                      'si porcentaje_socio es negativo afecta al saldo final en el ticket del cliente.
                      'si es positivo se va a mostrar en el ticket como ATRASO GRUPO: EN NEGATIVO (-CtaCte.Regalos) POR EJEMPLO.
                      If Clienteporcentaje <> 0 Then 'esto es el id del cliente
                        Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
                        If ds_parametro.Tables(0).Rows.Count <> 0 Then
                          'si hay 1 liquidacion, la ultima. se va a grabar un registro en ctacte para el cliente configurado.

                          Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Clienteporcentaje, ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                          Dim IdCtaCte As Integer = 0
                          If ds_ctacte.Tables(0).Rows.Count <> 0 Then
                            'existe
                            IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")

                            Dim regalos As Decimal = (Math.Round(porcentaje_socio, 2).ToString("N2")) * -1 'va en negativo para q en el ticket de cliente salga como "PAGO REGALO"
                            DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte, regalos) 'actualiza el valor en el campo regalos '

                            Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                          Else
                            'no existe, lo creo
                            'si no existe, creo un registro en ctacte?
                            'vamos a crear un registro con todo en cero salvo, regalo y SaldoAnterior.
                            Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Clienteporcentaje), ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                            Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
                            Dim regalos As Decimal = (Math.Round(porcentaje_socio, 2).ToString("N2")) * -1 'va en negativo para q en el ticket de cliente salga como "PAGO REGALO"
                            DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte_a, regalos)

                            Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                          End If
                        End If
                      End If

                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------


                    Else
                      'fila7("Columna3") = ""
                    End If
                    fila7("Columna4") = ""
                    DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila7)
                  Else
                    If Saldo_periodo < 0 And Grupo_Tipo = "3" Then '--GANO
                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------
                      '-----------------CORRECCION 2022-12-22---------------------------------------------------
                      'Ver si el grupo tiene un codigo de cliente en el campo Grupos.Clienteporcentaje.
                      'Si es distinto de 0. se va a grabar en un registro de ctacte. CtaCte.Regalos = porcentaje_socio
                      'si porcentaje_socio es negativo afecta al saldo final en el ticket del cliente.
                      'si es positivo se va a mostrar en el ticket como ATRASO GRUPO: EN NEGATIVO (-CtaCte.Regalos) POR EJEMPLO.
                      If Clienteporcentaje <> 0 Then 'esto es el id del cliente
                        Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
                        If ds_parametro.Tables(0).Rows.Count <> 0 Then
                          'si hay 1 liquidacion, la ultima. se va a grabar un registro en ctacte para el cliente configurado.

                          Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Clienteporcentaje, ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                          Dim IdCtaCte As Integer = 0
                          If ds_ctacte.Tables(0).Rows.Count <> 0 Then
                            'existe
                            IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")

                            Dim regalos As Decimal = (Math.Round(Saldo_periodo, 2).ToString("N2")) * -1 'va positivo para q en el ticket de cliente salga como "ATRASO GRUPO"
                            DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte, regalos) 'actualiza el valor en el campo regalos '

                            'NOTA: 2023-01-27 , COMENTO LA LINEA DE ABAJO YA QUE NO SE ACTUALIZA EL SALDO DEL CLIENTE CUANDO EL REGALO ES UN ATRASO
                            'Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                          Else
                            'no existe, lo creo
                            'si no existe, creo un registro en ctacte?
                            'vamos a crear un registro con todo en cero salvo, regalo y SaldoAnterior.
                            Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Clienteporcentaje), ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                            Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
                            Dim regalos As Decimal = (Math.Round(Saldo_periodo, 2).ToString("N2")) * -1 'va positivo para q en el ticket de cliente salga como "ATRASO GRUPO"
                            DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte_a, regalos)

                            'NOTA: 2023-01-27 , COMENTO LA LINEA DE ABAJO YA QUE NO SE ACTUALIZA EL SALDO DEL CLIENTE CUANDO EL REGALO ES UN ATRASO
                            '  Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                          End If
                        End If
                      End If

                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------


                    End If
                    'va la fila en blanco
                    DS_liqgrupos.Tables("LiqGrupos").Rows.Add()
                  End If
                End If

                If Grupo_Tipo = "2" Then
                  If Clienteporcentaje <> 0 Then
                    'PARA TODOS LOS CLIENTES DEL GRUPO 2, SE PONDRA dbo.Clientes.SaldoRegalo=0
                    DAliquidacion.Clientes_SaldoRegaloaCero(Grupo_Id, Clienteporcentaje)

                    'Se deberia dejar en "0" todos los dbo.Ctacte.Regalos de los clientes que esten dentro del grupo., el rango de la consulta solamente.
                    Dim ia As Integer = 0
                    While ia < ds_ctacteGrupo.Tables(0).Rows.Count
                      Dim IdCtacte As Integer = ds_ctacteGrupo.Tables(0).Rows(ia).Item("IdCtacte")
                      DAliquidacion.CtaCte_RegalosaCero(CStr(IdCtacte))
                      ia = ia + 1
                    End While
                  End If

                  'Si el saldo para el periodo es positivo y el grupo es de tipo=2 (% de los que ganan no cobran.) se debe
                  'calcular que % le corresponde al socio. Esto se obtine de multiplicar el % dbo.Grupos.Porcentaje * el total del "SALDO PARA EL PERIODO"
                  Dim porcentaje_socio As Decimal = 0
                  If Saldo_periodo > 0 And Grupo_Tipo = "2" Then  '---++DEJO
                    porcentaje_socio = (Saldo_periodo * Grupo_Porcentaje) / 100
                    Dim fila7 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                    fila7("Columna1") = ""
                    fila7("Columna2") = "% CORRESPONDIENTE AL SOCIO"
                    If porcentaje_socio <> 0 Then
                      fila7("Columna3") = (Math.Round(porcentaje_socio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------
                      '-----------------CORRECCION 2022-12-22---------------------------------------------------
                      'Ver si el grupo tiene un codigo de cliente en el campo Grupos.Clienteporcentaje.
                      'Si es distinto de 0. se va a grabar en un registro de ctacte. CtaCte.Regalos = porcentaje_socio
                      'si porcentaje_socio es negativo afecta al saldo final en el ticket del cliente.
                      'si es positivo se va a mostrar en el ticket como ATRASO GRUPO: EN NEGATIVO (-CtaCte.Regalos) POR EJEMPLO.
                      If Clienteporcentaje <> 0 Then 'esto es el id del cliente
                        Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
                        If ds_parametro.Tables(0).Rows.Count <> 0 Then
                          'si hay 1 liquidacion, la ultima. se va a grabar un registro en ctacte para el cliente configurado.

                          Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Clienteporcentaje, ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                          Dim IdCtaCte As Integer = 0
                          If ds_ctacte.Tables(0).Rows.Count <> 0 Then
                            'existe
                            IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")

                            Dim regalos As Decimal = (Math.Round(porcentaje_socio, 2).ToString("N2")) * -1 'va en negativo para q en el ticket de cliente salga como "PAGO REGALO"
                            DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte, regalos) 'actualiza el valor en el campo regalos '

                            Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                          Else
                            'no existe, lo creo
                            'si no existe, creo un registro en ctacte?
                            'vamos a crear un registro con todo en cero salvo, regalo y SaldoAnterior.
                            Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Clienteporcentaje), ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                            Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
                            Dim regalos As Decimal = (Math.Round(porcentaje_socio, 2).ToString("N2")) * -1 'va en negativo para q en el ticket de cliente salga como "PAGO REGALO"
                            DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte_a, regalos)

                            Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                          End If
                        End If
                      End If

                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------


                    Else
                      'fila7("Columna3") = ""
                    End If
                    fila7("Columna4") = ""
                    DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila7)
                  Else
                    If Saldo_periodo < 0 And Grupo_Tipo = "2" Then '--GANO
                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------
                      '-----------------CORRECCION 2022-12-22---------------------------------------------------
                      'Ver si el grupo tiene un codigo de cliente en el campo Grupos.Clienteporcentaje.
                      'Si es distinto de 0. se va a grabar en un registro de ctacte. CtaCte.Regalos = porcentaje_socio
                      'si porcentaje_socio es negativo afecta al saldo final en el ticket del cliente.
                      'si es positivo se va a mostrar en el ticket como ATRASO GRUPO: EN NEGATIVO (-CtaCte.Regalos) POR EJEMPLO.
                      If Clienteporcentaje <> 0 Then 'esto es el id del cliente
                        Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
                        If ds_parametro.Tables(0).Rows.Count <> 0 Then
                          'si hay 1 liquidacion, la ultima. se va a grabar un registro en ctacte para el cliente configurado.

                          Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Clienteporcentaje, ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                          Dim IdCtaCte As Integer = 0
                          If ds_ctacte.Tables(0).Rows.Count <> 0 Then
                            'existe
                            IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")

                            Dim regalos As Decimal = (Math.Round(Saldo_periodo, 2).ToString("N2")) * -1 'va positivo para q en el ticket de cliente salga como "ATRASO GRUPO"
                            DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte, regalos) 'actualiza el valor en el campo regalos '

                            'NOTA: 2023-01-27 , COMENTO LA LINEA DE ABAJO YA QUE NO SE ACTUALIZA EL SALDO DEL CLIENTE CUANDO EL REGALO ES UN ATRASO
                            'Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                          Else
                            'no existe, lo creo
                            'si no existe, creo un registro en ctacte?
                            'vamos a crear un registro con todo en cero salvo, regalo y SaldoAnterior.
                            Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Clienteporcentaje), ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                            Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
                            Dim regalos As Decimal = (Math.Round(Saldo_periodo, 2).ToString("N2")) * -1 'va positivo para q en el ticket de cliente salga como "ATRASO GRUPO"
                            DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte_a, regalos)

                            'NOTA: 2023-01-27 , COMENTO LA LINEA DE ABAJO YA QUE NO SE ACTUALIZA EL SALDO DEL CLIENTE CUANDO EL REGALO ES UN ATRASO
                            '  Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                          End If
                        End If
                      End If

                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------
                      '-----------------------------------------------------------------------------------------


                    End If
                    'va la fila en blanco
                    DS_liqgrupos.Tables("LiqGrupos").Rows.Add()
                  End If

                End If




                DS_liqgrupos.Tables("LiqGrupos").Rows.Add()

                '///////////////////GUARDO EN BD///////////////////////////
                DAliquidacion.LiquidacionGrupos_GruposModiffecha(Grupo_Id, fecha_hasta)
                If Grupo_Tipo = "2" Or Grupo_Tipo = "3" Or Grupo_Tipo = "4" Then
                  If Saldo_periodo > 0 Then 'si DEJO VA CERO
                    DAliquidacion.LiquidacionGrupos_GruposModifimporte(Grupo_Id, CDec(0))
                  Else
                    If Saldo_periodo < 0 Then 'SI GANO Y ES DE TIPO 4...se guarda 0
                      If Grupo_Tipo = "2" Then
                        DAliquidacion.LiquidacionGrupos_GruposModifimporte(Grupo_Id, Saldo_periodo)
                      End If
                      If Grupo_Tipo = "3" Then
                        DAliquidacion.LiquidacionGrupos_GruposModifimporte(Grupo_Id, Saldo_periodo)
                      End If
                      If Grupo_Tipo = "4" Then '
                        DAliquidacion.LiquidacionGrupos_GruposModifimporte(Grupo_Id, CDec(0))
                      End If
                    End If
                  End If
                End If
                '//////////////////////////////////////////////////////////

              Else
                'no se considera grupos del tipo 1
              End If

              ii = ii + 1
            End While

            '-----AQUI ARMO EL DATATABLE Q ENVIARE AL FORM "LiquidacionGrupos_det" para armar el reporte.-----------------------------
            If DS_liqgrupos.Tables("LiqGrupos").Rows.Count <> 0 Then
              Dim i As Integer = 0
              Dim ID As Integer = 1
              While i < DS_liqgrupos.Tables("LiqGrupos").Rows.Count
                If DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna2").ToString <> "" Then
                  Dim fila_a As DataRow = DS_liqgrupos.Tables("LiqGrupos_rpt").NewRow
                  fila_a("ID") = ID
                  fila_a("Columna1") = DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna1")
                  fila_a("Columna2") = DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna2")
                  fila_a("Columna3") = DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna3")
                  fila_a("Columna4") = DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna4")
                  DS_liqgrupos.Tables("LiqGrupos_rpt").Rows.Add(fila_a)
                Else
                  ID = ID + 1
                End If
                i = i + 1
              End While
            End If
            '------------------------------------------------------------------------------------------------------------------------

            Session("Tabla_LiqGrupos_rpt") = DS_liqgrupos.Tables("LiqGrupos_rpt")
            Session("Tabla_LiqGrupos") = DS_liqgrupos.Tables("LiqGrupos")
            Session("op_ingreso") = "si"
            Response.Redirect("~/WC_LiquidacionGrupos/LiquidacionGrupos_det.aspx")
          Else
            'error, no hay grupos


          End If



        Else
          'se realiza una consulta con un codigo de grupo especifico.

          Dim ds_grup As DataSet = DAgrupos.Grupos_buscar_codigo(Txt_grupo_codigo.Text)
          If ds_grup.Tables(0).Rows.Count <> 0 Then
            Dim Grupo_Id As Integer = ds_grup.Tables(0).Rows(0).Item("Grupo_id")

            'paso el valor de Importe a SaldoAnterior en la tabla Grupo
            DAliquidacion.LiquidacionGrupo_modifSaldoAnterior(CStr(Grupo_Id))

            Dim ds_grupo As DataSet = DAgrupos.Grupos_buscar_codigo(Txt_grupo_codigo.Text)


            Dim Grupo_Codigo As String = ds_grupo.Tables(0).Rows(0).Item("Codigo")
            Dim Grupo_Nombre As String = ds_grupo.Tables(0).Rows(0).Item("Nombre")
            Dim Grupo_Tipo As String = ds_grupo.Tables(0).Rows(0).Item("Tipo")

            Dim Clienteporcentaje As Integer = 0
            Try
              Clienteporcentaje = ds_grupo.Tables(0).Rows(0).Item("Clienteporcentaje")
            Catch ex As Exception
            End Try

            If Grupo_Tipo = "3" Or Grupo_Tipo = "4" Or Grupo_Tipo = "2" Then
              Dim fila As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
              fila("Columna1") = ""
              fila("Columna2") = "RESULTADO DEL PERIODO " + CDate(Txt_fecha_desde.Text).ToString("dd-MM-yyyy") + " AL " + CDate(Txt_fecha_hasta.Text).ToString("dd-MM-yyyy")
              'fila("Columna3") = "
              fila("Columna4") = ""
              DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila)

              Dim fila2 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
              fila2("Columna1") = ""
              fila2("Columna2") = "GRUPO:" + Grupo_Codigo + " " + Grupo_Nombre.ToString.ToUpper
              'fila2("Columna3") = ""
              fila2("Columna4") = ""
              DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila2)


              Dim Grupo_fecha As Date = CDate(ds_grupo.Tables(0).Rows(0).Item("Fecha"))
              Dim Grupo_SaldoAnterior As Decimal = CDec(ds_grupo.Tables(0).Rows(0).Item("Saldoanterior")) 'se toma del importe de procesamiento, Elias: 2022-12-06
              Dim Grupo_CodigoCobro As String = ds_grupo.Tables(0).Rows(0).Item("Codigocobro")
              Dim Grupo_Porcentaje As Decimal = ds_grupo.Tables(0).Rows(0).Item("Porcentaje")
              Dim ds_ctacteGrupo As DataSet = DAliquidacion.LiquidacionGrupos_ObtenerCtaCtexrangofecha(fecha_desde, fecha_hasta, Grupo_Id)

              Dim fila3 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
              fila3("Columna1") = ""
              fila3("Columna2") = "SALDO ANTERIOR AL " + Grupo_fecha
              fila3("Columna3") = (Math.Round(Grupo_SaldoAnterior, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              fila3("Columna4") = ""
              DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila3)

              Dim CtaCte_DejoGano As Decimal = 0
              Dim CtaCte_DejoGanoSC As Decimal = 0
              Dim CtaCte_DejoGanoB As Decimal = 0

              Dim TrabajoPeriodo As Decimal = 0

              If ds_ctacteGrupo.Tables(0).Rows.Count <> 0 Then
                '-------CALCULO DE "TRABAJO EN PERIODO"
                Dim i As Integer = 0

                While i < ds_ctacteGrupo.Tables(0).Rows.Count
                  Dim Regalos As Decimal = 0
                  Try
                    Regalos = CDec(ds_ctacteGrupo.Tables(0).Rows(i).Item("Regalos"))
                  Catch ex As Exception
                  End Try
                  Dim DejoGano As Decimal = CDec(ds_ctacteGrupo.Tables(0).Rows(i).Item("DejoGano"))
                  If Regalos < CDec(0) Then
                    DejoGano = DejoGano + Regalos
                  End If

                  CtaCte_DejoGano = CtaCte_DejoGano + DejoGano
                  CtaCte_DejoGanoSC = CtaCte_DejoGanoSC + CDec(ds_ctacteGrupo.Tables(0).Rows(i).Item("DejoGanoSC"))
                  CtaCte_DejoGanoB = CtaCte_DejoGanoB + CDec(ds_ctacteGrupo.Tables(0).Rows(i).Item("DejoGanoB"))
                  i = i + 1
                End While

                Select Case Grupo_CodigoCobro
                  Case "1"
                    'Si dbo.Grupos.CodigoCobro = 1,
                    'EL TRABAJO EN EL PERIODO = dbo.CtaCte.DejoGano + dbo.CtaCte.DejoGanoSC + dbo.CtaCte.DejoGanoB
                    'entre las fechas seleccionadas para la liquidacion.
                    '(Si el resultado del calculo es positivo se colocara la siguiente referencia ++DEJO,
                    'si el resultado del calculo es negativo se colocara la siguiente referencia --GANO)
                    TrabajoPeriodo = CtaCte_DejoGano + CtaCte_DejoGanoSC + CtaCte_DejoGanoB
                  Case "2"
                    TrabajoPeriodo = CtaCte_DejoGano + CtaCte_DejoGanoSC
                  Case "3"
                    TrabajoPeriodo = CtaCte_DejoGanoB
                  Case "4"
                    TrabajoPeriodo = CtaCte_DejoGano
                End Select
              Else
                'msj, no hay resultados para el rango de fecha
              End If

              Dim fila4 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
              If TrabajoPeriodo > 0 Then
                fila4("Columna1") = "++DEJO"

              End If
              If TrabajoPeriodo < 0 Then
                fila4("Columna1") = "--GANO"
              End If
              fila4("Columna2") = "EL TRABAJO EN EL PERIODO"
              fila4("Columna3") = (Math.Round(TrabajoPeriodo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              fila4("Columna4") = ""
              DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila4)


              '///////////////////////////////////////////////////////////////////////////////////////////////////////
              'El total de gastos es igual a la suma de los gastos dbo.Gastos.Importe cargados al grupo entre las fechas seleccionadas para la liquidacion.
              Dim DS_Gastos As DataSet = DAliquidacion.LiquidacionGrupos_ObtenerGastosxrangofecha(fecha_desde, fecha_hasta, Grupo_Id)
              Dim Gastos_importe As Decimal = 0
              If DS_Gastos.Tables(0).Rows.Count <> 0 Then
                Dim j As Integer = 0
                While j < DS_Gastos.Tables(0).Rows.Count
                  Gastos_importe = Gastos_importe + CDec(DS_Gastos.Tables(0).Rows(j).Item("Importe"))
                  j = j + 1
                End While
              Else
                'No se registran gastos para el rango de fecha

              End If

              Gastos_importe = Gastos_importe * -1

              Dim fila5 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
              fila5("Columna1") = ""
              fila5("Columna2") = "GASTOS"
              fila5("Columna3") = (Math.Round(Gastos_importe, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              fila5("Columna4") = ""
              DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila5)

              '///////////////////////////////////////////////////////////////////////////////////////////////////////

              Dim Saldo_periodo As Decimal = Grupo_SaldoAnterior + TrabajoPeriodo + Gastos_importe
              Dim fila6 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
              fila6("Columna1") = ""
              fila6("Columna2") = "SALDO PARA EL PERIODO"
              fila6("Columna3") = (Math.Round(Saldo_periodo, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              If Saldo_periodo > 0 Then
                fila6("Columna4") = "++DEJO"
              End If
              If Saldo_periodo < 0 Then
                fila6("Columna4") = "--GANO"
              End If
              DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila6)


              If Grupo_Tipo = "3" Then
                'Si el saldo para el periodo es positivo y el grupo es de tipo=3 (% del grupo) se debe
                'calcular que % le corresponde al socio. Esto se obtine de multiplicar el % dbo.Grupos.Porcentaje * el total del "SALDO PARA EL PERIODO"
                Dim porcentaje_socio As Decimal = 0
                If Saldo_periodo > 0 And Grupo_Tipo = "3" Then
                  porcentaje_socio = (Saldo_periodo * Grupo_Porcentaje) / 100


                  Dim fila7 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                  fila7("Columna1") = ""
                  fila7("Columna2") = "% CORRESPONDIENTE AL SOCIO"
                  If porcentaje_socio <> 0 Then
                    fila7("Columna3") = (Math.Round(porcentaje_socio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------
                    '-----------------CORRECCION 2022-12-22---------------------------------------------------
                    'Ver si el grupo tiene un codigo de cliente en el campo Grupos.Clienteporcentaje.
                    'Si es distinto de 0. se va a grabar en un registro de ctacte. CtaCte.Regalos = porcentaje_socio
                    'si porcentaje_socio es negativo afecta al saldo final en el ticket del cliente.
                    'si es positivo se va a mostrar en el ticket como ATRASO GRUPO: EN NEGATIVO (-CtaCte.Regalos) POR EJEMPLO.
                    If Clienteporcentaje <> 0 Then
                      Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
                      If ds_parametro.Tables(0).Rows.Count <> 0 Then
                        'si hay 1 liquidacion, la ultima. se va a grabar un registro en ctacte para el cliente configurado.

                        Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Clienteporcentaje, ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                        Dim IdCtaCte As Integer = 0
                        If ds_ctacte.Tables(0).Rows.Count <> 0 Then
                          'existe
                          IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")

                          Dim regalos As Decimal = (Math.Round(porcentaje_socio, 2).ToString("N2")) * -1 'va en negativo para q en el ticket de cliente salga como "PAGO REGALO"
                          DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte, regalos) 'actualiza el valor en el campo regalos '

                          Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                        Else
                          'no existe, lo creo
                          'si no existe, creo un registro en ctacte?
                          'vamos a crear un registro con todo en cero salvo, regalo y SaldoAnterior.
                          Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Clienteporcentaje), ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                          Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
                          Dim regalos As Decimal = (Math.Round(porcentaje_socio, 2).ToString("N2")) * -1 'va en negativo para q en el ticket de cliente salga como "PAGO REGALO"
                          DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte_a, regalos)

                          Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                        End If
                      End If
                    End If

                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------

                  Else
                    'fila7("Columna3") = ""
                  End If

                  If porcentaje_socio <> 0 Then
                    fila7("Columna3") = (Math.Round(porcentaje_socio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                  Else
                    'fila7("Columna3") = ""
                  End If
                  fila7("Columna4") = ""
                  DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila7)
                Else
                  If Saldo_periodo < 0 And Grupo_Tipo = "3" Then '--GANO
                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------
                    '-----------------CORRECCION 2022-12-22---------------------------------------------------
                    'Ver si el grupo tiene un codigo de cliente en el campo Grupos.Clienteporcentaje.
                    'Si es distinto de 0. se va a grabar en un registro de ctacte. CtaCte.Regalos = porcentaje_socio
                    'si porcentaje_socio es negativo afecta al saldo final en el ticket del cliente.
                    'si es positivo se va a mostrar en el ticket como ATRASO GRUPO: EN NEGATIVO (-CtaCte.Regalos) POR EJEMPLO.
                    If Clienteporcentaje <> 0 Then 'esto es el id del cliente
                      Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
                      If ds_parametro.Tables(0).Rows.Count <> 0 Then
                        'si hay 1 liquidacion, la ultima. se va a grabar un registro en ctacte para el cliente configurado.

                        Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Clienteporcentaje, ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                        Dim IdCtaCte As Integer = 0
                        If ds_ctacte.Tables(0).Rows.Count <> 0 Then
                          'existe
                          IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")

                          Dim regalos As Decimal = (Math.Round(Saldo_periodo, 2).ToString("N2")) * -1 'va positivo para q en el ticket de cliente salga como "ATRASO GRUPO"
                          DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte, regalos) 'actualiza el valor en el campo regalos '

                          'NOTA: 2023-01-27 , COMENTO LA LINEA DE ABAJO YA QUE NO SE ACTUALIZA EL SALDO DEL CLIENTE CUANDO EL REGALO ES UN ATRASO
                          'Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                        Else
                          'no existe, lo creo
                          'si no existe, creo un registro en ctacte?
                          'vamos a crear un registro con todo en cero salvo, regalo y SaldoAnterior.
                          Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Clienteporcentaje), ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                          Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
                          Dim regalos As Decimal = (Math.Round(Saldo_periodo, 2).ToString("N2")) * -1 'va positivo para q en el ticket de cliente salga como "ATRASO GRUPO"
                          DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte_a, regalos)

                          'NOTA: 2023-01-27 , COMENTO LA LINEA DE ABAJO YA QUE NO SE ACTUALIZA EL SALDO DEL CLIENTE CUANDO EL REGALO ES UN ATRASO
                          'Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                        End If
                      End If
                    End If

                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------


                  End If

                End If


              End If

              If Grupo_Tipo = "2" Then
                If Clienteporcentaje <> 0 Then
                  'PARA TODOS LOS CLIENTES DEL GRUPO 2, SE PONDRA dbo.Clientes.SaldoRegalo=0
                  DAliquidacion.Clientes_SaldoRegaloaCero(Grupo_Id, Clienteporcentaje)

                  'Se deberia dejar en "0" todos los dbo.Ctacte.Regalos de los clientes que esten dentro del grupo., el rango de la consulta solamente.
                  Dim ia As Integer = 0
                  While ia < ds_ctacteGrupo.Tables(0).Rows.Count
                    Dim IdCtacte As Integer = ds_ctacteGrupo.Tables(0).Rows(ia).Item("IdCtacte")
                    DAliquidacion.CtaCte_RegalosaCero(CStr(IdCtacte))
                    ia = ia + 1
                  End While
                End If


                'Si el saldo para el periodo es positivo y el grupo es de tipo=3 (% del grupo) se debe
                'calcular que % le corresponde al socio. Esto se obtine de multiplicar el % dbo.Grupos.Porcentaje * el total del "SALDO PARA EL PERIODO"
                Dim porcentaje_socio As Decimal = 0
                If Saldo_periodo > 0 And Grupo_Tipo = "2" Then
                  porcentaje_socio = (Saldo_periodo * Grupo_Porcentaje) / 100


                  Dim fila7 As DataRow = DS_liqgrupos.Tables("LiqGrupos").NewRow
                  fila7("Columna1") = ""
                  fila7("Columna2") = "% CORRESPONDIENTE AL SOCIO"
                  If porcentaje_socio <> 0 Then
                    fila7("Columna3") = (Math.Round(porcentaje_socio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------
                    '-----------------CORRECCION 2022-12-22---------------------------------------------------
                    'Ver si el grupo tiene un codigo de cliente en el campo Grupos.Clienteporcentaje.
                    'Si es distinto de 0. se va a grabar en un registro de ctacte. CtaCte.Regalos = porcentaje_socio
                    'si porcentaje_socio es negativo afecta al saldo final en el ticket del cliente.
                    'si es positivo se va a mostrar en el ticket como ATRASO GRUPO: EN NEGATIVO (-CtaCte.Regalos) POR EJEMPLO.
                    If Clienteporcentaje <> 0 Then
                      Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
                      If ds_parametro.Tables(0).Rows.Count <> 0 Then
                        'si hay 1 liquidacion, la ultima. se va a grabar un registro en ctacte para el cliente configurado.

                        Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Clienteporcentaje, ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                        Dim IdCtaCte As Integer = 0
                        If ds_ctacte.Tables(0).Rows.Count <> 0 Then
                          'existe
                          IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")

                          Dim regalos As Decimal = (Math.Round(porcentaje_socio, 2).ToString("N2")) * -1 'va en negativo para q en el ticket de cliente salga como "PAGO REGALO"
                          DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte, regalos) 'actualiza el valor en el campo regalos '

                          Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                        Else
                          'no existe, lo creo
                          'si no existe, creo un registro en ctacte?
                          'vamos a crear un registro con todo en cero salvo, regalo y SaldoAnterior.
                          Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Clienteporcentaje), ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                          Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
                          Dim regalos As Decimal = (Math.Round(porcentaje_socio, 2).ToString("N2")) * -1 'va en negativo para q en el ticket de cliente salga como "PAGO REGALO"
                          DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte_a, regalos)

                          Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                        End If
                      End If
                    End If

                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------

                  Else
                    'fila7("Columna3") = ""
                  End If

                  If porcentaje_socio <> 0 Then
                    fila7("Columna3") = (Math.Round(porcentaje_socio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
                  Else
                    'fila7("Columna3") = ""
                  End If
                  fila7("Columna4") = ""
                  DS_liqgrupos.Tables("LiqGrupos").Rows.Add(fila7)
                Else
                  If Saldo_periodo < 0 And Grupo_Tipo = "2" Then '--GANO
                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------
                    '-----------------CORRECCION 2022-12-22---------------------------------------------------
                    'Ver si el grupo tiene un codigo de cliente en el campo Grupos.Clienteporcentaje.
                    'Si es distinto de 0. se va a grabar en un registro de ctacte. CtaCte.Regalos = porcentaje_socio
                    'si porcentaje_socio es negativo afecta al saldo final en el ticket del cliente.
                    'si es positivo se va a mostrar en el ticket como ATRASO GRUPO: EN NEGATIVO (-CtaCte.Regalos) POR EJEMPLO.
                    If Clienteporcentaje <> 0 Then 'esto es el id del cliente
                      Dim ds_parametro As DataSet = DAParametro.Parametro_obtener_UltimoDiaLiq()
                      If ds_parametro.Tables(0).Rows.Count <> 0 Then
                        'si hay 1 liquidacion, la ultima. se va a grabar un registro en ctacte para el cliente configurado.

                        Dim ds_ctacte As DataSet = DAliquidacion.LiquidacionRegalos_obtenerctacte(Clienteporcentaje, ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                        Dim IdCtaCte As Integer = 0
                        If ds_ctacte.Tables(0).Rows.Count <> 0 Then
                          'existe
                          IdCtaCte = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")

                          Dim regalos As Decimal = (Math.Round(Saldo_periodo, 2).ToString("N2")) * -1 'va positivo para q en el ticket de cliente salga como "ATRASO GRUPO"
                          DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte, regalos) 'actualiza el valor en el campo regalos '

                          'NOTA: 2023-01-27 , COMENTO LA LINEA DE ABAJO YA QUE NO SE ACTUALIZA EL SALDO DEL CLIENTE CUANDO EL REGALO ES UN ATRASO
                          'Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                        Else
                          'no existe, lo creo
                          'si no existe, creo un registro en ctacte?
                          'vamos a crear un registro con todo en cero salvo, regalo y SaldoAnterior.
                          Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_Id, CStr(Clienteporcentaje), ds_parametro.Tables(0).Rows(0).Item("Fecha"))
                          Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
                          Dim regalos As Decimal = (Math.Round(Saldo_periodo, 2).ToString("N2")) * -1 'va positivo para q en el ticket de cliente salga como "ATRASO GRUPO"
                          DAliquidacion.LiquidacionRegalosSemanal_actualizarCtaCte(IdCtaCte_a, regalos)

                          'NOTA: 2023-01-27 , COMENTO LA LINEA DE ABAJO YA QUE NO SE ACTUALIZA EL SALDO DEL CLIENTE CUANDO EL REGALO ES UN ATRASO
                          'Actualizar_Saldo_ClientePorcentaje(Clienteporcentaje, regalos) 'nota: agregado el 2023-01-17

                        End If
                      End If
                    End If

                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------
                    '-----------------------------------------------------------------------------------------


                  End If

                End If


              End If


              '///////////////////GUARDO EN BD///////////////////////////
              DAliquidacion.LiquidacionGrupos_GruposModiffecha(Grupo_Id, fecha_hasta)
              If Grupo_Tipo = "2" Or Grupo_Tipo = "3" Or Grupo_Tipo = "4" Then
                If Saldo_periodo > 0 Then 'si DEJo va cero
                  DAliquidacion.LiquidacionGrupos_GruposModifimporte(Grupo_Id, CDec(0))
                Else
                  If Saldo_periodo < 0 Then 'SI GANO Y ES DE TIPO 4...SE GUARDA 0
                    If Grupo_Tipo = "2" Then
                      DAliquidacion.LiquidacionGrupos_GruposModifimporte(Grupo_Id, Saldo_periodo)
                    End If
                    If Grupo_Tipo = "3" Then
                      DAliquidacion.LiquidacionGrupos_GruposModifimporte(Grupo_Id, Saldo_periodo)
                    End If
                    If Grupo_Tipo = "4" Then
                      DAliquidacion.LiquidacionGrupos_GruposModifimporte(Grupo_Id, CDec(0))
                    End If
                  End If
                End If
              End If
              '//////////////////////////////////////////////////////////


              '-----AQUI ARMO EL DATATABLE Q ENVIARE AL FORM "LiquidacionGrupos_det" para armar el reporte.-----------------------------
              If DS_liqgrupos.Tables("LiqGrupos").Rows.Count <> 0 Then
                Dim i As Integer = 0
                Dim ID As Integer = 1
                While i < DS_liqgrupos.Tables("LiqGrupos").Rows.Count
                  If DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna2").ToString <> "" Then
                    Dim fila_a As DataRow = DS_liqgrupos.Tables("LiqGrupos_rpt").NewRow
                    fila_a("ID") = ID
                    fila_a("Columna1") = DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna1")
                    fila_a("Columna2") = DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna2")
                    fila_a("Columna3") = DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna3")
                    fila_a("Columna4") = DS_liqgrupos.Tables("LiqGrupos").Rows(i).Item("Columna4")
                    DS_liqgrupos.Tables("LiqGrupos_rpt").Rows.Add(fila_a)
                  Else
                    ID = ID + 1
                  End If
                  i = i + 1
                End While
              End If
              '------------------------------------------------------------------------------------------------------------------------

              Session("Tabla_LiqGrupos_rpt") = DS_liqgrupos.Tables("LiqGrupos_rpt")
              Session("Tabla_LiqGrupos") = DS_liqgrupos.Tables("LiqGrupos")
              Session("op_ingreso") = "si"
              Response.Redirect("~/WC_LiquidacionGrupos/LiquidacionGrupos_det.aspx")


            Else
              'no se considera grupos del tipo 1 y 2
            End If

          Else
            'error, el código no existe
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error3", "$(document).ready(function () {$('#modal-sm_error3').modal();});", True)
          End If


        End If

      Else
        'error, aqui msj que ingrese rango de fechas validas. 
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error2", "$(document).ready(function () {$('#modal-sm_error2').modal();});", True)
      End If
    Else
      'error, ingrese la informacion solicitada
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error1", "$(document).ready(function () {$('#modal-sm_error1').modal();});", True)
    End If
  End Sub

  Private Sub btn_ok_error1_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error1.ServerClick
    Txt_fecha_desde.Focus()
  End Sub

  Private Sub btn_close_error1_ServerClick(sender As Object, e As EventArgs) Handles btn_close_error1.ServerClick
    Txt_fecha_desde.Focus()
  End Sub

  Private Sub Btn_close_error2_ServerClick(sender As Object, e As EventArgs) Handles Btn_close_error2.ServerClick
    Txt_fecha_desde.Focus()
  End Sub

  Private Sub Btn_ok_error2_ServerClick(sender As Object, e As EventArgs) Handles Btn_ok_error2.ServerClick
    Txt_fecha_desde.Focus()
  End Sub

  Private Sub Btn_close_error3_ServerClick(sender As Object, e As EventArgs) Handles Btn_close_error3.ServerClick
    Txt_grupo_codigo.Focus()
  End Sub

  Private Sub Btn_ok_error3_ServerClick(sender As Object, e As EventArgs) Handles Btn_ok_error3.ServerClick
    Txt_grupo_codigo.Focus()
  End Sub

  Private Sub Btn_Ok_continue_ServerClick(sender As Object, e As EventArgs) Handles Btn_Ok_continue.ServerClick


  End Sub
End Class
