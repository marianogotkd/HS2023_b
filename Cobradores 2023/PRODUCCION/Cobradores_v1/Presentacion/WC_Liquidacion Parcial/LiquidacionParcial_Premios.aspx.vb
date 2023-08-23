Public Class LiquidacionParcial_Premios
  Inherits System.Web.UI.Page
#Region "Declaraciones"
  Dim DALiquidacion As New Capa_Datos.WC_Liquidacion
  Dim DACliente As New Capa_Datos.WB_clientes
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DArecorrido As New Capa_Datos.WC_recorridos_zonas
  Dim DAparametro As New Capa_Datos.WC_parametro
#End Region

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

      '---------------------------------------------------------
      Dim DS_liqparcial As New DS_liqparcial
      DS_liqparcial.Tables("Recorridos_seleccionados").Merge(Session("tabla_recorridos_seleccionados"))
      'GridView2.DataSource = DS_liqparcial.Tables("Recorridos_seleccionados")
      'GridView2.DataBind()
      GridView2.Visible = False

      'obtener_totales_parciales(DS_liqparcial)
      obtener_premios_x_clientes(DS_liqparcial)



      'Btn_next.Focus()

    End If
  End Sub

#Region "Metodos"

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
            If (Menu = "4" And Opcion = "") Or (Menu = "4" And Opcion = "1") Then
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


  Private Sub obtener_premios_x_clientes(ByVal DS_liqparcial As DataSet)
    '1) recupero todos los registros de Xcargas y los ordenos en un datatable-----------------------------------------
    Dim DS_XCARGAS1 As DataSet = DALiquidacion.Liquidacion_parcial_recuperarXcargas(DS_liqparcial.Tables("Recorridos_seleccionados").Rows(0).Item("Codigo"), HF_fecha.Value)
    Dim i As Integer = 1
    While i < DS_liqparcial.Tables("Recorridos_seleccionados").Rows.Count
      Dim codigo As String = DS_liqparcial.Tables("Recorridos_seleccionados").Rows(i).Item("Codigo")
      Dim DS_XCARGAS2 As DataSet = DALiquidacion.Liquidacion_parcial_recuperarXcargas(codigo, HF_fecha.Value)
      If DS_XCARGAS2.Tables(0).Rows.Count <> 0 Then
        DS_XCARGAS1.Tables(0).Merge(DS_XCARGAS2.Tables(0))
      End If
      i = i + 1
    End While
    'ahora ordeno
    Dim rows() As DataRow = DS_XCARGAS1.Tables(0).Select("IDcarga > 0", "Cliente, Recorrido_codigo ASC")
    Dim dtTemp As DataTable = DS_XCARGAS1.Tables(0).Clone() 'copio la estructura de la tabla.
    For Each row As DataRow In rows
      ' Indicamos que el registro ha sido añadido
      row.SetAdded()
      dtTemp.ImportRow(row)
    Next
    'dtTemp tiene todos los registros de XCargas ya ordenas para poder continuar.
    '-------------fin paso 1------------------------------------------------------------------------------------------

    carga_liquidacion_parcial(DS_liqparcial.Tables("Recorridos_seleccionados"), dtTemp) '---esta se usa actualmente: 01-03-2023
    'carga_liquidacion_parcial_optimizada(dtTemp)

    'GridView2.DataSource = dtTemp
    'GridView2.DataBind()

    'AQUI VIENE LA GENERACION DEL REPORTE.

    dtTemp.Rows.Clear()
    dtTemp.Dispose()

  End Sub

  Private Sub carga_liquidacion_parcial(ByVal Datatable_RecorridosSeleccionados As DataTable, ByVal dtTemp As DataTable)
    Dim ds_info As DataSet = DAparametro.Parametro_obtener_dia
    Dim dia As Integer = CInt(ds_info.Tables(0).Rows(0).Item("Dia"))
    '--------------------------------------------------------------------------------
    Dim DS_liqparcial1 As New DS_liqparcial
    DS_liqparcial1.Tables("PremiosxClientesParcial").Rows.Clear()

    Dim valor11 = 1234
    Dim undigito As String = valor11.ToString.Substring(3, 1)
    Dim dosdigitos As String = valor11.ToString.Substring(2, 2)
    Dim tresdigitos As String = valor11.ToString.Substring(1, 3)
    '--------------------------------------------------------------------------------

    'aqui voy a agregar correciones del: 13/03/2023
    Dim kk As Integer = 0
    While kk < Datatable_RecorridosSeleccionados.Rows.Count
      Dim CodigoRec As String = Datatable_RecorridosSeleccionados.Rows(kk).Item("Codigo")
      Dim ds_zonasHab As DataSet = DArecorrido.recorridos_zonas_ObtenerUnRecorrido_x_dia(CStr(dia), HF_fecha.Value, CodigoRec)
      Dim controlar = ""
      If CodigoRec = "1E" Then
        controlar = "si"
      End If

      '----------PREMIOS ETAPA 1-------------------------------------
      Dim Codigo As String = ds_zonasHab.Tables(0).Rows(0).Item("Codigo")
      Dim referencia_recorrido As String = ds_zonasHab.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
      '----------OP 1----------
      Dim ds_filtroOP1 As DataSet = DArecorrido.Liquidacion_recuperarXcargas_FiltroOP1(Codigo, HF_fecha.Value)
      Dim jj As Integer = 0
      While jj < ds_filtroOP1.Tables(0).Rows.Count
        If CDec(ds_filtroOP1.Tables(0).Rows(jj).Item("Importe")) <> CDec(0) Then
          Dim XCargas_Pid = ds_filtroOP1.Tables(0).Rows(jj).Item("Pid")

          If controlar = "si" And XCargas_Pid = "26" Then
            Dim controlar2 = "si"
          End If

          Dim Puntos_P1 = ds_zonasHab.Tables(0).Rows(0).Item("P1")
          Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
            Case 1
              'comparar con 1 digito en puntos_p1
              Select Case Len(Puntos_P1)
                Case 1
                'Puntos_P1 = Puntos_P1
                Case 2
                  Puntos_P1 = Puntos_P1.ToString.Substring(1, 1)
                Case 3
                  Puntos_P1 = Puntos_P1.ToString.Substring(2, 1)
                Case 4
                  Puntos_P1 = Puntos_P1.ToString.Substring(3, 1)
              End Select
              If XCargas_Pid = Puntos_P1 Then

                Dim respuesta = "son iguales"
                grabar_premios_op1(DS_liqparcial1, ds_filtroOP1.Tables(0).Rows(jj), referencia_recorrido)
              Else
                Dim respuesta = "no hay coincidencia"

              End If
            Case 2
              'comparar con 2 digito en puntos_p1
              Select Case Len(Puntos_P1)
                Case 1
                'Puntos_P1 = Puntos_P1
                Case 2
               'Puntos_P1
                Case 3
                  Puntos_P1 = Puntos_P1.ToString.Substring(1, 2)
                Case 4
                  Puntos_P1 = Puntos_P1.ToString.Substring(2, 2)
              End Select
              If XCargas_Pid = Puntos_P1 Then

                Dim respuesta = "son iguales"
                grabar_premios_op1(DS_liqparcial1, ds_filtroOP1.Tables(0).Rows(jj), referencia_recorrido)
              Else
                Dim respuesta = "no hay coincidencia"

              End If
            Case 3
              'comparar con 3 digito en puntos_p1
              Select Case Len(Puntos_P1)
                Case 1
                'Puntos_P1 = Puntos_P1
                Case 2
               'Puntos_P1
                Case 3
                'Puntos_P1
                Case 4
                  Puntos_P1 = Puntos_P1.ToString.Substring(1, 3)
              End Select
              If XCargas_Pid = Puntos_P1 Then

                Dim respuesta = "son iguales"
                grabar_premios_op1(DS_liqparcial1, ds_filtroOP1.Tables(0).Rows(jj), referencia_recorrido)
              Else
                Dim respuesta = "no hay coincidencia"

              End If
            Case 4
              'comparar con 4 digito en puntos_p1
              If XCargas_Pid = Puntos_P1 Then

                Dim respuesta = "son iguales"
                grabar_premios_op1(DS_liqparcial1, ds_filtroOP1.Tables(0).Rows(jj), referencia_recorrido)
              Else
                Dim respuesta = "no hay coincidencia"

              End If
          End Select
        End If
        jj = jj + 1
      End While
      '----------OP 2----------
      Dim ds_filtroOP2 As DataSet = DArecorrido.Liquidacion_recuperarXcargas_FiltroOP2(Codigo, HF_fecha.Value)
      jj = 0
      While jj < ds_filtroOP2.Tables(0).Rows.Count
        If CDec(ds_filtroOP2.Tables(0).Rows(jj).Item("Importe")) <> CDec(0) Then
          Dim XCargas_Pid = ds_filtroOP2.Tables(0).Rows(jj).Item("Pid")
          Dim XCargas_Suc = ds_filtroOP2.Tables(0).Rows(jj).Item("Suc")
          'Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
          'Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
          'Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
          Dim IndicePuntos As Integer = 7 '7 ES LA POSICION DE P1 EN LA CONSULTA.

          '16 ES P10

          Dim PtoLimite = CInt(XCargas_Suc) + 6 + 1 'mas 6 x que p1 empieza en la celda 7 Y mas uno porque para el limite uso un while con la condicion "menor"

          Dim ContCoincidencia As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid = PtoSelec"

          Dim i1 As Integer = 0
          While IndicePuntos < PtoLimite
            Dim PtoSelec = ds_zonasHab.Tables(0).Rows(0).Item(IndicePuntos)
            Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
              Case 1
                'comparar con 1 digito en puntos_p1

                Select Case Len(PtoSelec)
                  Case 1
                'Puntos_P1 = Puntos_P1
                  Case 2
                    PtoSelec = PtoSelec.ToString.Substring(1, 1)
                  Case 3
                    PtoSelec = PtoSelec.ToString.Substring(2, 1)
                  Case 4
                    PtoSelec = PtoSelec.ToString.Substring(3, 1)
                End Select
                If XCargas_Pid = PtoSelec Then

                  Dim respuesta = "son iguales"
                  ContCoincidencia = ContCoincidencia + 1
                Else
                  Dim respuesta = "no hay coincidencia"
                End If
              Case 2
                'comparar con 2 digito en puntos_p1
                Select Case Len(PtoSelec)
                  Case 1
                'Puntos_P1 = Puntos_P1
                  Case 2
               'Puntos_P1
                  Case 3
                    PtoSelec = PtoSelec.ToString.Substring(1, 2)
                  Case 4
                    PtoSelec = PtoSelec.ToString.Substring(2, 2)
                End Select
                If XCargas_Pid = PtoSelec Then
                  Dim respuesta = "son iguales"
                  ContCoincidencia = ContCoincidencia + 1
                Else
                  Dim respuesta = "no hay coincidencia"
                End If
              Case 3
                'comparar con 3 digito en puntos_p1
                Select Case Len(PtoSelec)
                  Case 1
                'Puntos_P1 = Puntos_P1
                  Case 2
               'Puntos_P1
                  Case 3
                'Puntos_P1
                  Case 4
                    PtoSelec = PtoSelec.ToString.Substring(1, 3)
                End Select
                If XCargas_Pid = PtoSelec Then
                  Dim respuesta = "son iguales"
                  ContCoincidencia = ContCoincidencia + 1
                Else
                  Dim respuesta = "no hay coincidencia"
                End If
              Case 4
                'comparar con 4 digito en puntos_p1
                If XCargas_Pid = PtoSelec Then
                  Dim respuesta = "son iguales"
                  ContCoincidencia = ContCoincidencia + 1
                Else
                  Dim respuesta = "no hay coincidencia"
                End If
            End Select
            IndicePuntos = IndicePuntos + 1
            i1 = i1 + 1
          End While

          If ContCoincidencia <> 0 Then
            grabar_premios_op2(DS_liqparcial1, ds_filtroOP2.Tables(0).Rows(jj), referencia_recorrido, ContCoincidencia)
          End If
        End If
        jj = jj + 1
      End While

      '---------op3------------
      Dim ds_filtroOP3 As DataSet = DArecorrido.Liquidacion_recuperarXcargas_FiltroOP3(Codigo, HF_fecha.Value)
      jj = 0
      While jj < ds_filtroOP3.Tables(0).Rows.Count
        If CDec(ds_filtroOP3.Tables(0).Rows(jj).Item("Importe")) <> CDec(0) Then
          Dim XCargas_Suc = ds_filtroOP3.Tables(0).Rows(jj).Item("Suc")
          Dim XCargas_Suc2 = 0
          Try
            XCargas_Suc2 = ds_filtroOP3.Tables(0).Rows(jj).Item("Suc2") 'lo uso en la 3ra alternativa
          Catch ex As Exception
          End Try
          Dim XCargas_Pid = ds_filtroOP3.Tables(0).Rows(jj).Item("Pid")
          Dim XCargas_Pid2 As String = ""
          Try
            XCargas_Pid2 = ds_filtroOP3.Tables(0).Rows(jj).Item("Pid2")
          Catch ex As Exception
          End Try
          'Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
          'Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
          'Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
          Dim Puntos_P1 = ds_zonasHab.Tables(0).Rows(0).Item("P1")
          Dim Coincidencia1 As String = "" 'pid = p1
          Dim Coincidencia2 As String = "" 'pid2 = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc2 + 1, excluyendo la coincidencia con el campo dbo.Puntos.P1)
          '1) primera validacion: pid=p1
          Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
            Case 2 'tener en cuenta que Pid va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.Puntos.P1
              'comparar con 2 digito en puntos_p1
              Select Case Len(Puntos_P1)
                Case 1
                'Puntos_P1 = Puntos_P1
                Case 2
               'Puntos_P1
                Case 3
                  Puntos_P1 = Puntos_P1.ToString.Substring(1, 2)
                Case 4
                  Puntos_P1 = Puntos_P1.ToString.Substring(2, 2)
              End Select
              If XCargas_Pid = Puntos_P1 Then
                Dim respuesta = "son iguales"
                Coincidencia1 = "si"
              Else
                Dim respuesta = "no hay coincidencia"
              End If
          End Select
          '2) segunda validacion: pid2 = 
          Dim IndicePuntos As Integer = 8 '8 ES LA POSICION DE P2 EN LA CONSULTA.
          Dim PtoLimite As Integer = 0
          If XCargas_Suc2 < 20 Then
            PtoLimite = XCargas_Suc2 + 1 'dbo.XCargasL.Suc2 + 1. NOTA: lo hago solo si es menor a 20...sino hay desbordamiento.."CONSULTAR ESTO"
          Else
            PtoLimite = XCargas_Suc2
          End If
          PtoLimite = PtoLimite + 6 + 1 'mas 6 x que p2 empieza en la celda 8 Y mas uno porque para el limite uso un while con la condicion "menor"
          Dim ContCoincidencia As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid2 = PtoSelec"

          Dim cont_items_recorridos As Integer = 1
          While IndicePuntos < PtoLimite

            If (cont_items_recorridos < CInt(XCargas_Suc2)) Then

              Dim PtoSelec = ds_zonasHab.Tables(0).Rows(0).Item(IndicePuntos)
              Select Case Len(XCargas_Pid2) 'devuelve cantidad de digitos en pid2
                Case 2 'tener en cuenta que Pid2 va ha tener 2 digitos.
                  'comparar con 2 digito en puntos_p
                  Select Case Len(PtoSelec)
                    Case 1
                'Puntos_P1 = Puntos_P1
                    Case 2
               'Puntos_P1
                    Case 3
                      PtoSelec = PtoSelec.ToString.Substring(1, 2)
                    Case 4
                      PtoSelec = PtoSelec.ToString.Substring(2, 2)
                  End Select
                  If XCargas_Pid2 = PtoSelec Then
                    Dim respuesta = "son iguales"
                    Coincidencia2 = "si"
                    ContCoincidencia = ContCoincidencia + 1
                  Else
                    Dim respuesta = "no hay coincidencia"
                  End If

              End Select
              cont_items_recorridos = cont_items_recorridos + 1 'para controlar los elementos leidos en P1 a Pn
            End If

            IndicePuntos = IndicePuntos + 1
          End While
          If Coincidencia1 = "si" And Coincidencia2 = "si" Then
            grabar_premios_op3(DS_liqparcial1, ds_filtroOP3.Tables(0).Rows(jj), referencia_recorrido, ContCoincidencia)

          End If

        End If
        jj = jj + 1
      End While
      'FIN COMENTO OP 3

      '---------op4------------
      'AQUI COMENTO OP 4
      Dim ds_filtroOP4 As DataSet = DArecorrido.Liquidacion_recuperarXcargas_FiltroOP4(Codigo, HF_fecha.Value)
      jj = 0
      While jj < ds_filtroOP4.Tables(0).Rows.Count
        If CDec(ds_filtroOP4.Tables(0).Rows(jj).Item("Importe")) <> CDec(0) Then
          Dim XCargas_Suc = ds_filtroOP4.Tables(0).Rows(jj).Item("Suc")
          Dim XCargas_Suc2 = 0
          Try
            XCargas_Suc2 = ds_filtroOP4.Tables(0).Rows(jj).Item("Suc2") 'lo uso en la 3ra alternativa
          Catch ex As Exception
          End Try
          Dim XCargas_Pid = ds_filtroOP4.Tables(0).Rows(jj).Item("Pid")
          Dim XCargas_Pid2 As String = ""
          Try
            XCargas_Pid2 = ds_filtroOP4.Tables(0).Rows(jj).Item("Pid2")
          Catch ex As Exception
          End Try
          'Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
          'Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
          'Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
          Dim IndicePuntos As Integer = 7 '7 ES LA POSICION DE P1 EN LA CONSULTA.
          Dim PtoLimite As Integer = 0
          PtoLimite = XCargas_Suc + 6 + 1 'mas 6 x que p1 empieza en la celda 7 Y mas uno porque para el limite uso un while con la condicion "menor"
          '1)primera validacion: buscar hasta dbo.Puntos.P(el valor de dbo.XCargasL.Suc) que dbo.XCargasL.Pid = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc) 
          '(tener en cuenta que Pid va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc))
          Dim ContCoincidencia1 As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid = PtoSelec"
          While IndicePuntos < PtoLimite
            Dim PtoSelec = ds_zonasHab.Tables(0).Rows(0).Item(IndicePuntos)
            Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
              Case 2 'tener en cuenta que Pid va ha tener 2 digitos.
                'comparar con 2 digito en puntos_p
                Select Case Len(PtoSelec)
                  Case 1
                      'Puntos_P1 = Puntos_P1
                  Case 2
                      'Puntos_P1
                  Case 3
                    PtoSelec = PtoSelec.ToString.Substring(1, 2)
                  Case 4
                    PtoSelec = PtoSelec.ToString.Substring(2, 2)
                End Select
                If XCargas_Pid = PtoSelec Then
                  Dim respuesta = "son iguales"
                  ContCoincidencia1 = ContCoincidencia1 + 1
                Else
                  Dim respuesta = "no hay coincidencia"
                End If

            End Select
            IndicePuntos = IndicePuntos + 1
          End While
          '2)buscar hasta dbo.Puntos.P(el valor de dbo.XCargasL.Suc2) que dbo.XCargasL.Pid2 = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc2)
          '(tener en cuenta que Pid2 va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.PuntosP(con algunos de los campos hasta el valor de dbo.XcargasL.Suc2))
          IndicePuntos = 7 'reinicio a la posicion de p1
          PtoLimite = XCargas_Suc2 + 6 + 1 'cambio el PtoLimite hasta el "P" que me indice el campo Suc2
          Dim ContCoincidencia2 As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid2 = PtoSelec"
          While IndicePuntos < PtoLimite
            Dim PtoSelec = ds_zonasHab.Tables(0).Rows(0).Item(IndicePuntos)
            Select Case Len(XCargas_Pid2) 'devuelve cantidad de digitos en pid2
              Case 2 'tener en cuenta que Pid2 va ha tener 2 digitos.
                'comparar con 2 digito en puntos_p
                Select Case Len(PtoSelec)
                  Case 1
                '     Puntos_P1 = Puntos_P1
                  Case 2
                      'Puntos_P1
                  Case 3
                    PtoSelec = PtoSelec.ToString.Substring(1, 2)
                  Case 4
                    PtoSelec = PtoSelec.ToString.Substring(2, 2)
                End Select
                If XCargas_Pid2 = PtoSelec Then
                  Dim respuesta = "son iguales"
                  ContCoincidencia2 = ContCoincidencia2 + 1
                Else
                  Dim respuesta = "no hay coincidencia"
                End If
            End Select
            IndicePuntos = IndicePuntos + 1
          End While

          '3) verifico si se dan ambas coincidencias
          If ContCoincidencia1 <> 0 And ContCoincidencia2 <> 0 Then
            grabar_premios_op4(DS_liqparcial1, ds_filtroOP4.Tables(0).Rows(jj), referencia_recorrido, ContCoincidencia1, ContCoincidencia2)
          End If

        End If
        jj = jj + 1
      End While

      'FIN COMENTO OP 4

      kk = kk + 1
    End While


    'Dim DS_liqparcial1 As New DS_liqparcial
    'DS_liqparcial1.Tables("PremiosxClientes").Rows.Clear()

    'Dim valor11 = 1234
    'Dim undigito As String = valor11.ToString.Substring(3, 1)
    'Dim dosdigitos As String = valor11.ToString.Substring(2, 2)
    'Dim tresdigitos As String = valor11.ToString.Substring(1, 3)

    Dim saltar As String = ""
    Dim i As Integer = 0
    If saltar = "no entrar aqui" Then

      While i < dtTemp.Rows.Count
        Dim XCargas_Suc = dtTemp.Rows(i).Item("Suc")
        Dim XCargas_Suc2 As Integer = 0
        Try
          XCargas_Suc2 = dtTemp.Rows(i).Item("Suc2") 'lo uso en la 3ra alternativa
        Catch ex As Exception

        End Try
        Dim XCargas_R As Boolean = False
        Try
          XCargas_R = dtTemp.Rows(i).Item("R") 'true or false
        Catch ex As Exception
        End Try

        '--------------------------------------1) PRIMERA ALTERNATIVA-------------------------------------------------------------------------------
        If (XCargas_Suc = 0 Or XCargas_Suc = 1) And XCargas_R = False Then
          Dim XCargas_Pid = dtTemp.Rows(i).Item("Pid")
          Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
          Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
          Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
          Dim Puntos_P1 = ds_recorridos.Tables(0).Rows(0).Item("P1")

          Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
            Case 1
              'comparar con 1 digito en puntos_p1

              Select Case Len(Puntos_P1)
                Case 1
                'Puntos_P1 = Puntos_P1
                Case 2
                  Puntos_P1 = Puntos_P1.ToString.Substring(1, 1)
                Case 3
                  Puntos_P1 = Puntos_P1.ToString.Substring(2, 1)
                Case 4
                  Puntos_P1 = Puntos_P1.ToString.Substring(3, 1)
              End Select
              If XCargas_Pid = Puntos_P1 Then

                Dim respuesta = "son iguales"
                grabar_premios_op1(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido)
              Else
                Dim respuesta = "no hay coincidencia"

              End If

            Case 2
              'comparar con 2 digito en puntos_p1
              Select Case Len(Puntos_P1)
                Case 1
                'Puntos_P1 = Puntos_P1
                Case 2
               'Puntos_P1
                Case 3
                  Puntos_P1 = Puntos_P1.ToString.Substring(1, 2)
                Case 4
                  Puntos_P1 = Puntos_P1.ToString.Substring(2, 2)
              End Select
              If XCargas_Pid = Puntos_P1 Then

                Dim respuesta = "son iguales"
                grabar_premios_op1(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido)
              Else
                Dim respuesta = "no hay coincidencia"

              End If
            Case 3
              'comparar con 3 digito en puntos_p1
              Select Case Len(Puntos_P1)
                Case 1
                'Puntos_P1 = Puntos_P1
                Case 2
               'Puntos_P1
                Case 3
                'Puntos_P1
                Case 4
                  Puntos_P1 = Puntos_P1.ToString.Substring(1, 3)
              End Select
              If XCargas_Pid = Puntos_P1 Then

                Dim respuesta = "son iguales"
                grabar_premios_op1(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido)
              Else
                Dim respuesta = "no hay coincidencia"

              End If
            Case 4
              'comparar con 4 digito en puntos_p1
              If XCargas_Pid = Puntos_P1 Then

                Dim respuesta = "son iguales"
                grabar_premios_op1(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido)
              Else
                Dim respuesta = "no hay coincidencia"

              End If
          End Select
          '--------------------------------------1) FIN-------------------------------------------------------------------------------------------------

        Else
          If XCargas_Suc > 1 And XCargas_R = False Then
            '--------------------------------------2) SEGUNDA ALTERNATIVA-------------------------------------------------------------------------------
            Dim XCargas_Pid = dtTemp.Rows(i).Item("Pid")
            Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
            Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
            Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper

            Dim IndicePuntos As Integer = 7 '7 ES LA POSICION DE P1 EN LA CONSULTA.

            '16 ES P10

            Dim PtoLimite = CInt(XCargas_Suc) + 6 + 1 'mas 6 x que p1 empieza en la celda 7 Y mas uno porque para el limite uso un while con la condicion "menor"

            Dim ContCoincidencia As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid = PtoSelec"

            Dim i1 As Integer = 0
            While IndicePuntos < PtoLimite
              Dim PtoSelec = ds_recorridos.Tables(0).Rows(0).Item(IndicePuntos)
              Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
                Case 1
                  'comparar con 1 digito en puntos_p1

                  Select Case Len(PtoSelec)
                    Case 1
                'Puntos_P1 = Puntos_P1
                    Case 2
                      PtoSelec = PtoSelec.ToString.Substring(1, 1)
                    Case 3
                      PtoSelec = PtoSelec.ToString.Substring(2, 1)
                    Case 4
                      PtoSelec = PtoSelec.ToString.Substring(3, 1)
                  End Select
                  If XCargas_Pid = PtoSelec Then

                    Dim respuesta = "son iguales"
                    ContCoincidencia = ContCoincidencia + 1
                  Else
                    Dim respuesta = "no hay coincidencia"
                  End If
                Case 2
                  'comparar con 2 digito en puntos_p1
                  Select Case Len(PtoSelec)
                    Case 1
                'Puntos_P1 = Puntos_P1
                    Case 2
               'Puntos_P1
                    Case 3
                      PtoSelec = PtoSelec.ToString.Substring(1, 2)
                    Case 4
                      PtoSelec = PtoSelec.ToString.Substring(2, 2)
                  End Select
                  If XCargas_Pid = PtoSelec Then
                    Dim respuesta = "son iguales"
                    ContCoincidencia = ContCoincidencia + 1
                  Else
                    Dim respuesta = "no hay coincidencia"
                  End If
                Case 3
                  'comparar con 3 digito en puntos_p1
                  Select Case Len(PtoSelec)
                    Case 1
                'Puntos_P1 = Puntos_P1
                    Case 2
               'Puntos_P1
                    Case 3
                'Puntos_P1
                    Case 4
                      PtoSelec = PtoSelec.ToString.Substring(1, 3)
                  End Select
                  If XCargas_Pid = PtoSelec Then
                    Dim respuesta = "son iguales"
                    ContCoincidencia = ContCoincidencia + 1
                  Else
                    Dim respuesta = "no hay coincidencia"
                  End If
                Case 4
                  'comparar con 4 digito en puntos_p1
                  If XCargas_Pid = PtoSelec Then
                    Dim respuesta = "son iguales"
                    ContCoincidencia = ContCoincidencia + 1
                  Else
                    Dim respuesta = "no hay coincidencia"
                  End If
              End Select
              IndicePuntos = IndicePuntos + 1
              i1 = i1 + 1
            End While

            If ContCoincidencia <> 0 Then
              grabar_premios_op2(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido, ContCoincidencia)
            End If

          Else
            If (XCargas_Suc = 0 Or XCargas_Suc = 1) And XCargas_R = True Then
              '--------------------------------------3) TERCERA ALTERNATIVA-------------------------------------------------------------------------------
              Dim XCargas_Pid = dtTemp.Rows(i).Item("Pid")
              Dim XCargas_Pid2 As String = ""
              Try
                XCargas_Pid2 = dtTemp.Rows(i).Item("Pid2")
              Catch ex As Exception

              End Try

              Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
              Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
              Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
              Dim Puntos_P1 = ds_recorridos.Tables(0).Rows(0).Item("P1")
              Dim Coincidencia1 As String = "" 'pid = p1
              Dim Coincidencia2 As String = "" 'pid2 = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc2 + 1, excluyendo la coincidencia con el campo dbo.Puntos.P1)
              '1) primera validacion: pid=p1
              Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
                Case 2 'tener en cuenta que Pid va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.Puntos.P1
                  'comparar con 2 digito en puntos_p1
                  Select Case Len(Puntos_P1)
                    Case 1
                'Puntos_P1 = Puntos_P1
                    Case 2
               'Puntos_P1
                    Case 3
                      Puntos_P1 = Puntos_P1.ToString.Substring(1, 2)
                    Case 4
                      Puntos_P1 = Puntos_P1.ToString.Substring(2, 2)
                  End Select
                  If XCargas_Pid = Puntos_P1 Then
                    Dim respuesta = "son iguales"
                    Coincidencia1 = "si"
                  Else
                    Dim respuesta = "no hay coincidencia"
                  End If
              End Select
              '2) segunda validacion: pid2 = 
              Dim IndicePuntos As Integer = 8 '8 ES LA POSICION DE P2 EN LA CONSULTA.
              Dim PtoLimite As Integer = 0
              If XCargas_Suc2 < 20 Then
                PtoLimite = XCargas_Suc2 + 1 'dbo.XCargasL.Suc2 + 1. NOTA: lo hago solo si es menor a 20...sino hay desbordamiento.."CONSULTAR ESTO"
              End If
              PtoLimite = PtoLimite + 6 + 1 'mas 6 x que p2 empieza en la celda 8 Y mas uno porque para el limite uso un while con la condicion "menor"
              Dim ContCoincidencia As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid2 = PtoSelec"
              While IndicePuntos < PtoLimite
                Dim PtoSelec = ds_recorridos.Tables(0).Rows(0).Item(IndicePuntos)
                Select Case Len(XCargas_Pid2) 'devuelve cantidad de digitos en pid2
                  Case 2 'tener en cuenta que Pid2 va ha tener 2 digitos.
                    'comparar con 2 digito en puntos_p
                    Select Case Len(PtoSelec)
                      Case 1
                'Puntos_P1 = Puntos_P1
                      Case 2
               'Puntos_P1
                      Case 3
                        PtoSelec = PtoSelec.ToString.Substring(1, 2)
                      Case 4
                        PtoSelec = PtoSelec.ToString.Substring(2, 2)
                    End Select
                    If XCargas_Pid2 = PtoSelec Then
                      Dim respuesta = "son iguales"
                      Coincidencia2 = "si"
                      ContCoincidencia = ContCoincidencia + 1
                    Else
                      Dim respuesta = "no hay coincidencia"
                    End If

                End Select
                IndicePuntos = IndicePuntos + 1
              End While
              If Coincidencia1 = "si" And Coincidencia2 = "si" Then
                grabar_premios_op3(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido, ContCoincidencia)

              End If
              '--------------------------------------3) TERCERA ALTERNATIVA (FIN)-------------------------------------------------------------------------------------------------
            Else
              If XCargas_Suc > 1 And XCargas_R = True Then
                '--------------------------------------4) CUARTA ALTERNATIVA-------------------------------------------------------------------------------
                Dim XCargas_Pid = dtTemp.Rows(i).Item("Pid")
                Dim XCargas_Pid2 As String = ""
                Try
                  XCargas_Pid2 = dtTemp.Rows(i).Item("Pid2")
                Catch ex As Exception

                End Try
                Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
                Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
                Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
                Dim IndicePuntos As Integer = 7 '7 ES LA POSICION DE P1 EN LA CONSULTA.
                Dim PtoLimite As Integer = 0
                PtoLimite = XCargas_Suc + 6 + 1 'mas 6 x que p1 empieza en la celda 7 Y mas uno porque para el limite uso un while con la condicion "menor"
                '1)primera validacion: buscar hasta dbo.Puntos.P(el valor de dbo.XCargasL.Suc) que dbo.XCargasL.Pid = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc) 
                '(tener en cuenta que Pid va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc))
                Dim ContCoincidencia1 As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid = PtoSelec"
                While IndicePuntos < PtoLimite
                  Dim PtoSelec = ds_recorridos.Tables(0).Rows(0).Item(IndicePuntos)
                  Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
                    Case 2 'tener en cuenta que Pid va ha tener 2 digitos.
                      'comparar con 2 digito en puntos_p
                      Select Case Len(PtoSelec)
                        Case 1
                      'Puntos_P1 = Puntos_P1
                        Case 2
                      'Puntos_P1
                        Case 3
                          PtoSelec = PtoSelec.ToString.Substring(1, 2)
                        Case 4
                          PtoSelec = PtoSelec.ToString.Substring(2, 2)
                      End Select
                      If XCargas_Pid = PtoSelec Then
                        Dim respuesta = "son iguales"
                        ContCoincidencia1 = ContCoincidencia1 + 1
                      Else
                        Dim respuesta = "no hay coincidencia"
                      End If

                  End Select
                  IndicePuntos = IndicePuntos + 1
                End While
                '2)buscar hasta dbo.Puntos.P(el valor de dbo.XCargasL.Suc2) que dbo.XCargasL.Pid2 = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc2)
                '(tener en cuenta que Pid2 va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.PuntosP(con algunos de los campos hasta el valor de dbo.XcargasL.Suc2))
                IndicePuntos = 7 'reinicio a la posicion de p1
                PtoLimite = XCargas_Suc2 + 6 + 1 'cambio el PtoLimite hasta el "P" que me indice el campo Suc2
                Dim ContCoincidencia2 As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid2 = PtoSelec"
                While IndicePuntos < PtoLimite
                  Dim PtoSelec = ds_recorridos.Tables(0).Rows(0).Item(IndicePuntos)
                  Select Case Len(XCargas_Pid2) 'devuelve cantidad de digitos en pid2
                    Case 2 'tener en cuenta que Pid2 va ha tener 2 digitos.
                      'comparar con 2 digito en puntos_p
                      Select Case Len(PtoSelec)
                        Case 1
                '     Puntos_P1 = Puntos_P1
                        Case 2
                      'Puntos_P1
                        Case 3
                          PtoSelec = PtoSelec.ToString.Substring(1, 2)
                        Case 4
                          PtoSelec = PtoSelec.ToString.Substring(2, 2)
                      End Select
                      If XCargas_Pid2 = PtoSelec Then
                        Dim respuesta = "son iguales"
                        ContCoincidencia2 = ContCoincidencia2 + 1
                      Else
                        Dim respuesta = "no hay coincidencia"
                      End If
                  End Select
                  IndicePuntos = IndicePuntos + 1
                End While

                '3) verifico si se dan ambas coincidencias
                If ContCoincidencia1 <> 0 And ContCoincidencia2 <> 0 Then
                  grabar_premios_op4(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido, ContCoincidencia1, ContCoincidencia2)
                End If
                '--------------------------------------4) CUARTA ALTERNATIVA (FIN)-------------------------------------------------------------------------------------------------

              End If
            End If
          End If
        End If
        i = i + 1
      End While

    End If
    Dim PreOrd As DataTable = DS_liqparcial1.Tables("PremiosxClientesParcial").Copy 'copio la estructura de la tabla.
    Dim rows() As DataRow = PreOrd.Select("Cliente <> 0", "Cliente, T, ID ASC")
    'Dim rows() As DataRow = PreOrd.Select("Cliente <> 0")
    Dim DTPremiosOrdenados As DataTable = DS_liqparcial1.Tables("PremiosxClientesParcial").Clone() 'copio la estructura de la tabla.

    For Each row As DataRow In rows
      ' Indicamos que el registro ha sido añadido
      'row.SetAdded()
      DTPremiosOrdenados.ImportRow(row)
    Next

    '-----AQUI CARGO LA INFO EN UN REPORTE---------------------------------------------------------------------------

    Dim fila_1 As DataRow = DS_liqparcial1.Tables("PremiosxClientes_info").NewRow
    fila_1("Fecha") = CDate(HF_fecha.Value)
    DS_liqparcial1.Tables("PremiosxClientes_info").Rows.Add(fila_1)

    Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
    CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/LiquidacionParcial_informe02_premiosxclientes.rpt"))
    CrReport.Database.Tables("PremiosxClientes").SetDataSource(DTPremiosOrdenados)
    'CrReport.Database.Tables("PremiosxClientes").SetDataSource(DS_liqparcial1.Tables("PremiosxClientes"))
    CrReport.Database.Tables("PremiosxClientes_info").SetDataSource(DS_liqparcial1.Tables("PremiosxClientes_info"))
    CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/LiqParcial_premiosxclientes.pdf"))

    '---------------------------------------------------------------------------------------------------------------







    'AQUI AGREGO LOS CORTES DE CONTROL ---------------------------------------------------------------------
    '----------COPIO LA ESTRUCTURA EN OTRO DATATABLE
    Dim dtTemp_con_cortes_control As DataTable = DS_liqparcial1.Tables("PremiosxClientes").Clone()

    i = 0
    Dim total_general As Decimal = 0
    While i < DTPremiosOrdenados.Rows.Count
      total_general = total_general + CDec(DTPremiosOrdenados.Rows(i).Item("Premio"))
      Dim Cliente = DTPremiosOrdenados.Rows(i).Item("Cliente")

      If dtTemp_con_cortes_control.Rows.Count = 0 Then
        'agrego registro
        dtTemp_con_cortes_control.ImportRow(DTPremiosOrdenados.Rows(i))

      Else
        Dim validar = ""
        Dim ultimo_registro As Integer = dtTemp_con_cortes_control.Rows.Count - 1
        If dtTemp_con_cortes_control.Rows(ultimo_registro).Item("Cliente") = Cliente Then
          'agrego
          dtTemp_con_cortes_control.ImportRow(DTPremiosOrdenados.Rows(i))
        Else
          'si son diferentes primero hago el recuento de los premios para ese cliente y luego agrego un registro a modo de resumen.
          Dim j As Integer = 0
          Dim suma_premio As Decimal = 0
          Cliente = DTPremiosOrdenados.Rows(i - 1).Item("Cliente")
          While j < dtTemp_con_cortes_control.Rows.Count
            If CStr(Cliente) = dtTemp_con_cortes_control.Rows(j).Item("Cliente") Then
              suma_premio = suma_premio + CDec(dtTemp_con_cortes_control.Rows(j).Item("Premio"))
            End If
            j = j + 1
          End While
          Dim fila As DataRow = dtTemp_con_cortes_control.NewRow
          fila("Cliente") = "TOTAL"
          fila("Premio") = (Math.Round(suma_premio, 2).ToString("N2"))
          dtTemp_con_cortes_control.Rows.Add(fila)
          'ahora agrego el registro diferente, o sea nuevo cliente
          dtTemp_con_cortes_control.ImportRow(DTPremiosOrdenados.Rows(i))
        End If
      End If


      i = i + 1
    End While

    If dtTemp_con_cortes_control.Rows.Count <> 0 Then
      'calculo total para ultimo cliente ingreso
      Dim j As Integer = 0
      Dim suma_premio As Decimal = 0
      Dim ultimo_registro As Integer = dtTemp_con_cortes_control.Rows.Count - 1
      Dim Cliente = dtTemp_con_cortes_control.Rows(ultimo_registro).Item("Cliente")
      If Cliente <> "TOTAL" Then
        While j < dtTemp_con_cortes_control.Rows.Count
          If Cliente = dtTemp_con_cortes_control.Rows(j).Item("Cliente") Then
            suma_premio = suma_premio + CDec(dtTemp_con_cortes_control.Rows(j).Item("Premio"))
          End If
          j = j + 1
        End While
        Dim filaa As DataRow = dtTemp_con_cortes_control.NewRow
        filaa("Cliente") = "TOTAL"
        filaa("Premio") = (Math.Round(suma_premio, 2).ToString("N2"))
        dtTemp_con_cortes_control.Rows.Add(filaa)
      End If
      'agrego fila con TOTAL GENERAL
      Dim fila_a As DataRow = dtTemp_con_cortes_control.NewRow
      fila_a("Cliente") = "TOTAL GENERAL"
      fila_a("Premio") = (Math.Round(total_general, 2).ToString("N2"))
      dtTemp_con_cortes_control.Rows.Add(fila_a)

    End If

    'MUESTRO EN EL GRIDVIEW---------------------------------------------------------------------------------
    GridView1.DataSource = dtTemp_con_cortes_control
    GridView1.DataBind()

    'GridView1.DataSource = DS_liqparcial1.Tables("PremiosxClientes")
    'GridView1.DataBind()
    '-------------------------------------------------------------------------------------------------------

    DS_liqparcial1.Tables("PremiosxClientes").Rows.Clear()
    DS_liqparcial1.Tables("PremiosxClientes").Dispose()
    DS_liqparcial1.Tables("PremiosxClientesParcial").Rows.Clear()
    DS_liqparcial1.Tables("PremiosxClientesParcial").Dispose()
    dtTemp_con_cortes_control.Rows.Clear()
    dtTemp_con_cortes_control.Dispose()


  End Sub


  Private Sub grabar_premios_op1(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String)

    Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientesParcial").NewRow
    fila("Cliente") = registro.Item("Cliente")
    fila("Recorrido") = referencia_recorrido
    fila("Importe") = registro.Item("Importe")
    fila("PID") = registro.Item("Pid")
    fila("SUC") = registro.Item("Suc")
    fila("P2") = ""

    fila("ID") = registro.Item("ID")

    Dim SinComputo As Boolean = False
    Try
      SinComputo = registro.Item("SinComputo")
    Catch ex As Exception
    End Try
    If SinComputo = True Then
      fila("SC") = "X"
    Else
      fila("SC") = ""
    End If
    Dim premio As Decimal = 0
    Select Case Len(registro.Item("Pid").ToString)
      Case 1
        premio = CDec(CDec(registro.Item("Importe")) * 7)
      Case 2
        premio = CDec(CDec(registro.Item("Importe")) * 70)
      Case 3
        premio = CDec(CDec(registro.Item("Importe")) * 600)
      Case 4
        premio = CDec(CDec(registro.Item("Importe")) * 3500)
    End Select
    fila("Premio") = (Math.Round(premio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

    fila("T") = registro.Item("Terminal").ToString.ToUpper

    fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.
    Dim ds_cliente As DataSet = DACliente.Clientes_buscar_codigo(registro.Item("Cliente"))
    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        fila("OBS") = "CUB."
        fila("Premio") = fila("Premio") * (-1)
      End If
    End If


    DS_liqparcial1.Tables("PremiosxClientesParcial").Rows.Add(fila)

  End Sub

  Private Sub grabar_premios_op2(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia As Integer)
    Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientesParcial").NewRow
    fila("Cliente") = registro.Item("Cliente")
    fila("Recorrido") = referencia_recorrido
    fila("Importe") = registro.Item("Importe")
    fila("PID") = registro.Item("Pid")
    fila("SUC") = registro.Item("Suc")
    fila("P2") = ""

    fila("ID") = registro.Item("ID")

    Dim SinComputo As Boolean = False
    Try
      SinComputo = registro.Item("SinComputo")
    Catch ex As Exception

    End Try
    If SinComputo = True Then
      fila("SC") = "X"
    Else
      fila("SC") = ""
    End If
    Dim premio As Decimal = 0
    Select Case Len(registro.Item("Pid").ToString)
      Case 1
        premio = ((CDec(CDec(registro.Item("Importe")) * 7)) / (CDec(registro.Item("Suc")))) * ContCoincidencia
      Case 2
        premio = ((CDec(CDec(registro.Item("Importe")) * 70)) / (CDec(registro.Item("Suc")))) * ContCoincidencia
      Case 3
        premio = ((CDec(CDec(registro.Item("Importe")) * 600)) / (CDec(registro.Item("Suc")))) * ContCoincidencia
      Case 4
        premio = ((CDec(CDec(registro.Item("Importe")) * 3500)) / (CDec(registro.Item("Suc")))) * ContCoincidencia
    End Select

    fila("Premio") = (Math.Round(premio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento


    fila("T") = registro.Item("Terminal").ToString.ToUpper
    fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.
    Dim ds_cliente As DataSet = DACliente.Clientes_buscar_codigo(registro.Item("Cliente"))
    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        fila("OBS") = "CUB."
        fila("Premio") = fila("Premio") * (-1)
      End If
    End If
    DS_liqparcial1.Tables("PremiosxClientesParcial").Rows.Add(fila)
  End Sub

  Private Sub grabar_premios_op3(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia As Integer)
    Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientesParcial").NewRow
    fila("Cliente") = registro.Item("Cliente")
    fila("Recorrido") = referencia_recorrido
    fila("Importe") = registro.Item("Importe")
    fila("PID") = registro.Item("Pid")
    fila("SUC") = registro.Item("Suc")
    fila("S2") = registro.Item("Suc2")
    fila("P2") = registro.Item("Pid2")
    Dim premio As Decimal = 0

    fila("ID") = registro.Item("ID")

    Dim SinComputo As Boolean = False
    Try
      SinComputo = registro.Item("SinComputo")
    Catch ex As Exception

    End Try
    If SinComputo = True Then
      fila("SC") = "X"
    Else
      fila("SC") = ""
    End If

    '----CORRECCIONES 2022-12-22----------------------------------------------------------------------------
    'FACTOR: ahora se recupera Factor que puede tomar el valor 80 si es true y 70 si es false. Este valor cambia la formula para obtener el "premio"
    Dim ds_cliente As DataSet = DACliente.Clientes_buscar_codigo(registro.Item("Cliente"))
    Dim Factor As Integer = 80
    Try
      If ds_cliente.Tables(0).Rows(0).Item("Factor") = True Then
        Factor = 80
      End If
      If ds_cliente.Tables(0).Rows(0).Item("Factor") = False Then
        Factor = 70
      End If
    Catch ex As Exception
    End Try

    'nota: 12-01-2023 , no aplica para este caso.
    'Se divide en 2 el valor de ContCoincidencia solo si el Pid y Pid2 son iguales.
    'Try
    '  If registro.Item("Pid") = registro.Item("Pid2") Then
    '    ContCoincidencia = ContCoincidencia / 2
    '  End If
    'Catch ex As Exception

    'End Try
    '-----------------------------------------------------------------------------------------------------

    Try
      '-------------------------------------------------------------------------------------------------------------------------------------------
      '----------------•	si XCargasL.Suc2 < 20, dbo.Premios.Premio = el valor del importe encontrado en -----------------------------------------
      '----------------el registro de la coincidencia dbo.XCargasL.Importe * 80 * ((80 / dbo.XCargasL.Suc2) * la cantidad de veces ---------------
      '----------------que coincidio dbo.XCargasL.Pid2 dentro de la dbo.Puntos.P(hasta el valor de dbo.XCargasL.Suc2 + 1)) -----------------------
      '-------------------------------------------------------------------------------------------------------------------------------------------
      If CInt(registro.Item("Suc2")) < 20 Then
        'Dim Suc2 As Integer = CInt(registro.Item("Suc2"))
        'Dim Importe As Decimal = CDec(registro.Item("Importe"))
        'Dim premio As Decimal = Importe * 80 * ((80 / Suc2) * ContCoincidencia)
        'fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
        Dim Suc2 As Integer = CInt(registro.Item("Suc2"))
        Dim Importe As Decimal = CDec(registro.Item("Importe"))
        'premio = Importe * 80 * ((80 / Suc2) * ContCoincidencia)
        premio = Importe * Factor * ((Factor / Suc2) * ContCoincidencia)
        fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
      Else
        '-------------------------------------------------------------------------------------------------------------------------------------------
        '-------------•	si XCargasL.Suc2 = 20, dbo.Premios.Premio = el valor del importe encontrado en el registro
        '-------------de la coincidencia dbo.XCargasL.Importe * 80 * ((80 / 19) * la cantidad de veces que------------------------------------------
        '-------------coincidio dbo.XCargasL.Pid2 dentro de la dbo.Puntos.P(hasta el valor de dbo.XCargasL.Suc2))-----------------------------------
        '-------------------------------------------------------------------------------------------------------------------------------------------
        If CInt(registro.Item("Suc2")) = 20 Then
          'Dim Importe As Decimal = CDec(registro.Item("Importe"))
          'Dim premio As Decimal = Importe * 80 * ((80 / 19) * ContCoincidencia)
          'fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
          Dim Importe As Decimal = CDec(registro.Item("Importe"))
          'premio = Importe * 80 * ((80 / 19) * ContCoincidencia)
          premio = Importe * Factor * ((Factor / 19) * ContCoincidencia)
          fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
        End If
      End If
    Catch ex As Exception

    End Try
    fila("T") = registro.Item("Terminal").ToString.ToUpper
    fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.
    'Dim ds_cliente As DataSet = DACliente.Clientes_buscar_codigo(registro.Item("Cliente"))
    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        fila("OBS") = "CUB."
        fila("Premio") = fila("Premio") * (-1)
      End If
    End If
    DS_liqparcial1.Tables("PremiosxClientesParcial").Rows.Add(fila)

  End Sub

  Private Sub grabar_premios_op4(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia1 As Integer, ByVal ContCoincidencia2 As Integer)
    Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientesParcial").NewRow
    fila("Cliente") = registro.Item("Cliente")
    fila("Recorrido") = referencia_recorrido
    fila("Importe") = registro.Item("Importe")
    fila("PID") = registro.Item("Pid")
    fila("SUC") = registro.Item("Suc")
    fila("S2") = registro.Item("Suc2")
    fila("P2") = registro.Item("Pid2")
    Dim premio As Decimal = 0
    Dim SinComputo As Boolean = False

    fila("ID") = registro.Item("ID")

    Try
      SinComputo = registro.Item("SinComputo")
    Catch ex As Exception

    End Try
    If SinComputo = True Then
      fila("SC") = "X"
    Else
      fila("SC") = ""
    End If

    '----CORRECCIONES 2022-12-22----------------------------------------------------------------------------
    'FACTOR: ahora se recupera Factor que puede tomar el valor 80 si es true y 70 si es false. Este valor cambia la formula para obtener el "premio"
    Dim ds_cliente As DataSet = DACliente.Clientes_buscar_codigo(registro.Item("Cliente"))
    Dim Factor As Integer = 80
    Try
      If ds_cliente.Tables(0).Rows(0).Item("Factor") = True Then
        Factor = 80
      End If
      If ds_cliente.Tables(0).Rows(0).Item("Factor") = False Then
        Factor = 70
      End If
    Catch ex As Exception
    End Try
    'Se divide en 2 el valor de ContCoincidencia solo si el Pid y Pid2 son iguales.
    Try
      If registro.Item("Pid") = registro.Item("Pid2") Then
        ContCoincidencia1 = ContCoincidencia1 / 2
        ContCoincidencia2 = ContCoincidencia2 / 2
      End If
    Catch ex As Exception
    End Try
    '-----------------------------------------------------------------------------------------------------




    Try
      Dim importe As Decimal = CDec(registro.Item("Importe"))
      Dim suc As Integer = CInt(registro.Item("Suc"))
      Dim suc2 As Integer = CInt(registro.Item("Suc2"))
      premio = importe * ((Factor / suc) * ContCoincidencia1) * ((Factor / suc2) * ContCoincidencia2)
      fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
    Catch ex As Exception

    End Try
    fila("T") = registro.Item("Terminal").ToString.ToUpper
    fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.

    'Dim ds_cliente As DataSet = DACliente.Clientes_buscar_codigo(registro.Item("Cliente"))
    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        fila("OBS") = "CUB."
        fila("Premio") = fila("Premio") * (-1)
      End If
    End If
    DS_liqparcial1.Tables("PremiosxClientesParcial").Rows.Add(fila)

  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub





#End Region

End Class
