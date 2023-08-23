Public Class Cobro_PrestamosxRegalos_det
  Inherits System.Web.UI.Page

#Region "DECLARACIONES"
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAprestamoscreditos As New Capa_Datos.WC_prestamoscreditos
  Dim DActacte As New Capa_Datos.WC_CtaCte
  Dim DACliente As New Capa_Datos.WB_clientes
  Dim DAconfiguracion As New Capa_Datos.Configuracion
  Dim DAReliquidacion As New Capa_Datos.WC_Reliquidacion
  Dim DAliquidacion As New Capa_Datos.WC_Liquidacion
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DAweb As New Capa_Datos.WC_Web
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
        Crear_BKP()
      Else
        'error, no hay liquidacion completada
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
      End If
    End If
  End Sub

  Private Sub btn_continuar_ServerClick(sender As Object, e As EventArgs) Handles btn_continuar.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub


#End Region

#Region "METODOS"
  Private Sub Metodo1()
    Dim DS_cobro_prestamos_regalos As New DS_cobro_prestamos_regalos
    DS_cobro_prestamos_regalos.Tables("CobPrestRegalo").Rows.Clear()

    Dim CLiente_Codigo_anterior As Integer = 0 'va a ir cambiando a medida q avanzo en el ciclo.
    Dim ctacte_regalos_aux As Decimal = 0 'esto lo uso para validar cuando hay varios prestamos para un mismo cliente, no quiero alterar en dbo.ctacte.regalos su valor, entonces descuento en una variable temporal.

    Dim ds_prestamos As DataSet = DAprestamoscreditos.Prestamos_obtener_prestamosxregalo
    If ds_prestamos.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_prestamos.Tables(0).Rows.Count
        'POR CADA PRESTAMOS: evaluar si el cliente tuvo movimiento de cobro de regalos dbo.CtaCte.Regalos.
        Dim Cliente_ID As Integer = ds_prestamos.Tables(0).Rows(i).Item("Cliente_ID")
        Dim Cliente_Codigo As Integer = CInt(ds_prestamos.Tables(0).Rows(i).Item("Cliente_Codigo"))
        Dim Fecha_prestamo As Date = CDate(ds_prestamos.Tables(0).Rows(i).Item("Fecha"))
        Dim Idprestamocredito As Integer = ds_prestamos.Tables(0).Rows(i).Item("Idprestamocredito")
        Dim ds_ctacte As DataSet = DActacte.CtaCte_obtener(Cliente_Codigo, HF_fecha.Value) 'la fecha es la de la ultima liquidacion 
        If ds_ctacte.Tables(0).Rows.Count <> 0 Then
          'verifico si CtaCte.Regalos es <> 0, NOTA:Si el prestamo fue dado de alta en la fecha del dia a liquidar no deberia ejecutarse ningun cobro.
          If Fecha_prestamo <> CDate(HF_fecha.Value) Then
            Dim CtaCte_Regalos As Decimal = 0
            Dim IdCtaCte As Integer = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")
            Try
              CtaCte_Regalos = CDec(ds_ctacte.Tables(0).Rows(0).Item("Regalos"))
            Catch ex As Exception
              CtaCte_Regalos = CDec(0)
            End Try

            '----CORRECCION 2023-04-01-------------------------------
            If (CLiente_Codigo_anterior = 0) Then
              ctacte_regalos_aux = CtaCte_Regalos

            Else
              If (CLiente_Codigo_anterior = Cliente_Codigo) Then
                'queda igual ya que ctacte_regalos_aux tiene el dbo.ctate.regalos descontado del ciclo anterior
              Else
                ctacte_regalos_aux = CtaCte_Regalos
              End If
            End If
            '--------------------------------------------------------

            '---CORRECCION 2023-04-01, CAMBIAMOS CtaCte_Regalos por CtaCte_Regalos_aux 
            If ctacte_regalos_aux < CDec(0) Then 'VALORES NEGATIVOS SOLAMENTE

              'tuvo movimiento en regalos
              '------------------------------------------------------------------------------------------------------------
              'dbo.CobroPrestamosCreditos.Importe = dbo.CtaCte.Regalos * dbo.PrestamosCreditos.Porcentaje
              '(hay que revisar el saldo que le queda al prestamo dbo.PrestamosCreditos.Saldo ya que el
              'cobro no puede ser mayor al saldo que le queda al prestamo)
              Dim PrestamosCreditos_Porcentaje As Decimal = ds_prestamos.Tables(0).Rows(i).Item("Porcentaje")
              Dim PrestamosCreditos_Saldo As Decimal = ds_prestamos.Tables(0).Rows(i).Item("Saldo")
              Dim importe As Decimal = Math.Abs((ctacte_regalos_aux * PrestamosCreditos_Porcentaje) / 100) 'ponemos valor absoluto x que el resultado es negativo////antes se usaba CtaCte_Regalos en lugar de ctacte_regalos_aux
              Dim importe_absoluto As Decimal = Math.Abs(importe)

              Dim Estado_id As Integer = 1 '1 activo, 2 cancelado, 3 baja
              If importe > PrestamosCreditos_Saldo Then
                importe = PrestamosCreditos_Saldo
              End If
              '/////////////////////////////////////////////////////////
              'AQUI GUARDO EN BD
              DAprestamoscreditos.CobroPrestamosCreditos_alta(Idprestamocredito, HF_fecha.Value, importe)
              '/////////////////////////////////////////////////////////

              'CORRECCION 2023-04-01 ---------------------------------- 
              ctacte_regalos_aux = ctacte_regalos_aux + importe
              CLiente_Codigo_anterior = Cliente_Codigo
              '--------------------------------------------------------

              '------------------------------------------------------------------------------------------------------------
              'Actualizar el saldo del prestamo dbo.PrestamosCreditos.Saldo = dbo.PrestamosCreditos.Saldo - dbo.CobroPrestamoCredito.Importe
              '(si el cobro del prestamo cancela el saldo es decir el total del prestamo se deberia
              'marcar este prestamo como "Cancelado" dbo.PrestamosCreditos.Estado = C)
              PrestamosCreditos_Saldo = PrestamosCreditos_Saldo - importe
              If (PrestamosCreditos_Saldo = 0) Or (PrestamosCreditos_Saldo < 0) Then
                PrestamosCreditos_Saldo = 0
                'esta cancelado el prestamo
                Estado_id = 2
              End If

              '/////////////////////////////////////////////////////////
              'AQUI GUARDO EN BD
              DAprestamoscreditos.PrestamosCreditos_ActualizarSaldo(Idprestamocredito, PrestamosCreditos_Saldo, Estado_id)
              '/////////////////////////////////////////////////////////

              '------------------------------------------------------------------------------------------------------------
              'Actualizar el campo dbo.CtaCte.CobPrestamo = dbo.CtaCte.CobPrestamo + dbo.CobroPrestamosCreditos.Importe

              Dim CobPrestamo As Decimal = 0
              Try 'el try lo uso x que el campo recuperado de ctacte es null
                CobPrestamo = ds_ctacte.Tables(0).Rows(0).Item("CobPrestamo") + importe
              Catch ex As Exception
                CobPrestamo = importe
              End Try

              '/////////////////////////////////////////////////////////
              'AQUI GUARDO EN BD
              DActacte.CtaCte_actualizarCobPrestamo(IdCtaCte, CobPrestamo)
              '/////////////////////////////////////////////////////////

              '------------------------------------------------------------------------------------------------------------
              'Actualizar el saldo dbo.Clientes.Saldo = dbo.Clientes.Saldo + dbo.CtaCte.CobPrestamo
              DACliente.Cliente_ActualizarSaldo_ctacte(Cliente_ID, importe)
              '-----------------------------------------------------------------------------------------------------------
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
                Dim DS_CTACTE1 As DataSet = DActacte.CtaCte_obtener(CInt(Cliente_Codigo), HF_fecha.Value)
                If DS_CTACTE1.Tables(0).Rows.Count <> 0 Then
                  'Dim IdCtaCte As Integer = CInt(DS_CTACTE1.Tables(0).Rows(0).Item("IdCtaCte"))
                  DActacte.CtaCte_ActualizarSalida(CInt(Cliente_Codigo), HF_fecha.Value, CDec(Clie_saldo))
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
              '  Dim ds_cliente As DataSet = DACliente.Clientes_buscar_codigo(CStr(Cliente_Codigo))
              '  Dim Saldo As Decimal = CDec(ds_cliente.Tables(0).Rows(0).Item("Saldo"))
              '  If Saldo < 0 Then 'si el saldo es negativo el Cliente Gana.
              '    DActacte.CtaCte_ActualizarSalida(Cliente_Codigo, CDate(HF_fecha.Value), Saldo) 'Guardar en el campo dbo.CtaCte.Salida el saldo final del cliente cuando el cliente gana, 
              '    DACliente.Clientes_ActualizarSaldo(CStr(Cliente_Codigo), Saldo, CDec(0)) 'y poner en 0 el saldo del cliente dbo.Clientes.Saldo = 0
              '  End If
              'End If
              ''------------------------------------------------------------------------------------------------------------------------------------


              Dim fila As DataRow = DS_cobro_prestamos_regalos.Tables("CobPrestRegalo").NewRow
              fila("Cliente") = CInt(Cliente_Codigo)
              fila("Fecha_Ori") = HF_fecha.Value
              fila("Importe") = (Math.Round(importe, 2).ToString("N2"))
              fila("Saldo") = (Math.Round(PrestamosCreditos_Saldo, 2).ToString("N2"))

              DS_cobro_prestamos_regalos.Tables("CobPrestRegalo").Rows.Add(fila)

            Else
              'no tuvo ningun movimiento en CtaCte para la fecha indicada.
            End If
          Else
            'no cobro, es prestamo se genero la misma fecha
          End If
        Else
          'no tuvo ningun movimiento en CtaCte para la fecha indicada.
        End If

        i = i + 1
      End While



    Else
      'error, no hay prestamos x regalo
    End If

    GridView1.DataSource = DS_cobro_prestamos_regalos.Tables("CobPrestRegalo")
    GridView1.DataBind()

    If DS_cobro_prestamos_regalos.Tables("CobPrestRegalo").Rows.Count = 0 Then
      Label_noregalos.Visible = True
    Else
      'si tiene registros, genero un pdf con el listado.

      Dim fila As DataRow = DS_cobro_prestamos_regalos.Tables("Reporte").NewRow
      fila("Fecha") = HF_fecha.Value
      DS_cobro_prestamos_regalos.Tables("Reporte").Rows.Add(fila)



      '------------------AQUIREPORTE ------------------------------------------------

      Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
      CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
      CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/Cobro_PrestamosxRegalos_informe.rpt"))
      CrReport.Database.Tables("Reporte").SetDataSource(DS_cobro_prestamos_regalos.Tables("Reporte"))
      CrReport.Database.Tables("CobPrestRegalo").SetDataSource(DS_cobro_prestamos_regalos.Tables("CobPrestRegalo"))
      CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/Cobro_PrestamosxRegalos_informe.pdf"))
      CrReport.Dispose()
      '------------------------------------------------------------------------------




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
            If (Menu = "9" And Opcion = "") Or (Menu = "9" And Opcion = "1") Then
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
