Public Class LiquidacionFinal_PrestamosComision
  Inherits System.Web.UI.Page

#Region "DECLARACIONES"
  Dim DaPrestamosCreditos As New Capa_Datos.WC_prestamoscreditos
  Dim DACtaCte As New Capa_Datos.WC_CtaCte
  Dim DACliente As New Capa_Datos.WB_clientes
  Dim DAconfiguracion As New Capa_Datos.Configuracion
  Dim DAliquidacion As New Capa_Datos.WC_Liquidacion
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DAparametro As New Capa_Datos.WC_parametro
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
      DAparametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal_PrestamosComision.aspx") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.



      'Luego de continuar deberia consultar si se desea descontar "prestamos por comision"
      'en el caso de contestar que "SI" el sistema deberia hacer el cobro de los prestamos activos por comision,
      'en el caso de contestar que "NO" deberia saltar este proceso y continuar.

      Dim DS_liqfinal As New DS_liqfinal
      DS_liqfinal.Tables("PrestamosComision").Rows.Clear()

      '1) El proceso de cobro de prestamos a descontar de la comision consiste en buscar los prestamos
      'activos dbo.PrestamosCreditos.Estado = "A" y en donde dbo.PrestamosCreditos.Tipo = P y
      'dbo.PrestamosCreditos.Tipocobro = 1 para cada cliente 
      Dim DS_PrestamosComision As DataSet = DaPrestamosCreditos.Prestamos_obtener_prestamosxcomision()
      If DS_PrestamosComision.Tables(0).Rows.Count <> 0 Then
        'evaluar si el cliente tuvo movimientos. De haber tenido movimiento hay que ver,
        'si cobro comision en el dia (dbo.CtaCte.Comision > 0) De haber cobrado comision el proceso deberia
        'grabar un movimiento en la tabla dbo.CobroPrestamosCreditos.
        Dim i As Integer = 0
        While i < DS_PrestamosComision.Tables(0).Rows.Count
          Dim Cliente_ID As Integer = DS_PrestamosComision.Tables(0).Rows(i).Item("Cliente_ID")
          Dim Cliente_Codigo As String = DS_PrestamosComision.Tables(0).Rows(i).Item("Cliente_Codigo")
          Dim fecha_prestamo = DS_PrestamosComision.Tables(0).Rows(i).Item("Fecha")

          'Dim fecha1 As String = fecha_prestamo.Day + "/" + fecha_prestamo.Month + "/" + fecha_prestamo.Year
          'Dim fecha_actual As String = CDate(HF_fecha.Value).Day + "/" + CDate(HF_fecha.Value).Month + "/" + CDate(HF_fecha.Value).Year

          'si el prestamo fue dado de alta en la fecha del dia, tengo que agregar el monto del prestamo a Ctacte.


          Dim ds_ctacte As DataSet = DACtaCte.CtaCte_obtener(CInt(Cliente_Codigo), HF_fecha.Value)
          '-------------------------------------------------------------------------------------------------------------------------
          '-------------------------------------------------------------------------------------------------------------------------
          '------------------------FECHA: 09-12-2022  SE REGISTRA EL PRESTAMO EN CTACTE---------------------------------------------
          'If fecha_prestamo = HF_fecha.Value Then
          '  If ds_ctacte.Tables(0).Rows.Count <> 0 Then
          '    'NO modifico. Si existe ctacte quiere decir que se lo agrego en la etapa anterior de la liq fina. en el form "LiquidacionFinal.aspx"
          '    'Dim PrestamoImporte As Decimal = DS_PrestamosComision.Tables(0).Rows(i).Item("Importe")
          '    'Dim IDCtaCte As Integer = ds_ctacte.Tables(0).Rows(0).Item("IDCtacte")
          '    'DAliquidacion.Prestamos_InsertCtaCte(IDCtaCte, CStr(PrestamoImporte).Replace(",", "."))

          '    ''Actualizar el saldo dbo.Clientes.Saldo = dbo.Clientes.Saldo + prestamo
          '    'DACliente.Cliente_ActualizarSaldo_ctacte(Cliente_ID, PrestamoImporte)
          '  Else
          '    'inserto
          '    Dim PrestamoImporte As Decimal = DS_PrestamosComision.Tables(0).Rows(i).Item("Importe")
          '    Dim Grupo_id As String = CStr(DS_PrestamosComision.Tables(0).Rows(i).Item("Grupo_id"))
          '    'vamos a crear un registro con todo en cero salvo SaldoAnterior.
          '    Dim ds_ctacte_Recu As DataSet = DACtaCte.CtaCte_Alta_vacia(Grupo_id, CStr(Cliente_Codigo), HF_fecha.Value)
          '    Dim IdCtaCte_a As Integer = ds_ctacte_Recu.Tables(0).Rows(0).Item("IdCtaCte")
          '    'Dim CtaCte_Regalos As Decimal = SaldoRegalo
          '    DAliquidacion.Prestamos_InsertCtaCte(IdCtaCte_a, CStr(PrestamoImporte).Replace(",", "."))

          '    'Actualizar el saldo dbo.Clientes.Saldo = dbo.Clientes.Saldo + prestamo
          '    DACliente.Cliente_ActualizarSaldo_ctacte(Cliente_ID, PrestamoImporte)
          '  End If
          'End If
          '-------------------------------------------------------------------------------------------------------------------------
          '-------------------------------------------------------------------------------------------------------------------------

          If fecha_prestamo <> HF_fecha.Value Then 'Si el prestamo fue dado de alta en la fecha del dia a liquidar no deberia ejecutarse ningun cobro.

            If ds_ctacte.Tables(0).Rows.Count <> 0 Then
              Dim IdCtaCte As Integer = ds_ctacte.Tables(0).Rows(0).Item("IdCtaCte")
              Dim CtaCte_Comision As Decimal = CDec(0)
              Try
                CtaCte_Comision = CDec(ds_ctacte.Tables(0).Rows(0).Item("Comision"))
              Catch ex As Exception
                CtaCte_Comision = CDec(0)
              End Try
              If CtaCte_Comision > CDec(0) Then
                Dim PrestamosCreditos_IdPrestamoCredito As Integer = DS_PrestamosComision.Tables(0).Rows(i).Item("Idprestamocredito")
                Dim PrestamosCreditos_Porcentaje As Decimal = DS_PrestamosComision.Tables(0).Rows(i).Item("Porcentaje")
                Dim PrestamosCreditos_Saldo As Decimal = DS_PrestamosComision.Tables(0).Rows(i).Item("Saldo")

                'dbo.CobroPrestamosCreditos.Importe = dbo.CtaCte.Comision * dbo.PrestamosCreditos.Porcentaje
                '(hay que revisar el saldo que le queda al prestamo dbo.PrestamosCreditos.Saldo ya que el cobro
                'no puede ser mayor al saldo que le queda al prestamo)
                Dim importe As Decimal = (CtaCte_Comision * PrestamosCreditos_Porcentaje) / 100
                Dim Estado_id As Integer = 1 '1 activo, 2 cancelado, 3 baja
                If importe > PrestamosCreditos_Saldo Then
                  importe = PrestamosCreditos_Saldo
                End If

                '/////////////////////////////////////////////////////////
                'AQUI GUARDO EN BD
                DaPrestamosCreditos.CobroPrestamosCreditos_alta(PrestamosCreditos_IdPrestamoCredito, HF_fecha.Value, importe)
                '/////////////////////////////////////////////////////////

                'Actualizar el saldo del prestamo dbo.PrestamosCreditos.Saldo = dbo.PrestamosCreditos.Saldo - dbo.CobroPrestamoCredito.Importe
                '(si el cobro del prestamo cancela el saldo es decir el total del prestamo se deberia marcar este prestamo como "Cancelado" dbo.PrestamosCreditos.Estado = C)
                PrestamosCreditos_Saldo = PrestamosCreditos_Saldo - importe
                If (PrestamosCreditos_Saldo = 0) Or (PrestamosCreditos_Saldo < 0) Then
                  PrestamosCreditos_Saldo = 0
                  'esta cancelado el prestamo
                  Estado_id = 2
                End If
                DaPrestamosCreditos.PrestamosCreditos_ActualizarSaldo(PrestamosCreditos_IdPrestamoCredito, PrestamosCreditos_Saldo, Estado_id)

                '--------------------------------------------------------------------------------------------------------------------------
                'Actualizar el campo dbo.CtaCte.CobPrestamo = dbo.CtaCte.CobPrestamo + dbo.CobroPrestamosCreditos.Importe
                Dim CobPrestamo As Decimal = 0
                Try 'el try lo uso x que el campo recuperado de ctacte es null
                  CobPrestamo = ds_ctacte.Tables(0).Rows(0).Item("CobPrestamo") + importe
                Catch ex As Exception
                  CobPrestamo = importe
                End Try

                'AQUI GUARDO EN BD
                DACtaCte.CtaCte_actualizarCobPrestamo(IdCtaCte, CobPrestamo)
                '--------------------------------------------------------------------------------------------------------------------------

                '--------------------------------------------------------------------------------------------------------------------------
                'Actualizar el saldo dbo.Clientes.Saldo = dbo.Clientes.Saldo + dbo.CtaCte.CobPrestamo
                DACliente.Cliente_ActualizarSaldo_ctacte(Cliente_ID, importe)
                '--------------------------------------------------------------------------------------------------------------------------
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
                Dim fila As DataRow = DS_liqfinal.Tables("PrestamosComision").NewRow
                fila("Cliente") = CInt(Cliente_Codigo)
                Dim FechaPrestamo_Ori As Date = DS_PrestamosComision.Tables(0).Rows(i).Item("Fecha")
                fila("Fecha_Ori") = FechaPrestamo_Ori.ToString("dd-MM-yyyy")
                fila("Importe_Cob") = (Math.Round(importe, 2).ToString("N2"))
                fila("Saldo") = (Math.Round(PrestamosCreditos_Saldo, 2).ToString("N2"))
                Dim Prestamo As Decimal = DS_PrestamosComision.Tables(0).Rows(i).Item("Importe")
                fila("Prestamo") = (Math.Round(Prestamo, 2).ToString("N2"))
                DS_liqfinal.Tables("PrestamosComision").Rows.Add(fila)


              End If
            End If
          End If

          i = i + 1
        End While

      Else
        'no hay prestamosxcomision
      End If

      '------------------AQUIREPORTE ------------------------------------------------
      Dim fila_1 As DataRow = DS_liqfinal.Tables("PrestamosComision_info").NewRow
      fila_1("Fecha") = CDate(HF_fecha.Value)
      DS_liqfinal.Tables("PrestamosComision_info").Rows.Add(fila_1)
      Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
      CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
      CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/LiquidacionFinal_informe03.rpt"))
      CrReport.Database.Tables("PrestamosComision").SetDataSource(DS_liqfinal.Tables("PrestamosComision"))
      CrReport.Database.Tables("PrestamosComision_info").SetDataSource(DS_liqfinal.Tables("PrestamosComision_info"))
      CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/LiqFinal_CobPrestamosComision.pdf"))

      '------------------------------------------------------------------------------



      'ahora muestro DS_liqfinal.tables("PrestamosComision") en un gridview.

      GridView1.DataSource = DS_liqfinal.Tables("PrestamosComision")
      GridView1.DataBind()



      If GridView1.Rows.Count = 0 Then
        Label_noprestamos.Visible = True
      End If

      btn_continuar.Focus()
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

  Private Sub btn_continuar_ServerClick(sender As Object, e As EventArgs) Handles btn_continuar.ServerClick
    'continuamos al form donde se consultara si hay prestamos x comision
    Session("fecha_parametro") = HF_fecha.Value
    Session("op_ingreso") = "si"
    Session("parametro_id") = HF_parametro_id.Value
    Response.Redirect("~/WC_LiquidacionFinal/LiquidacionFinal_Creditos.aspx")

  End Sub
#End Region

#Region "METODOS"

#End Region



End Class
