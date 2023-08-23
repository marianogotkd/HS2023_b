Public Class RecaudacionxCliente
  Inherits System.Web.UI.Page
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DApuntos As New Capa_Datos.WC_puntos
  Dim DALiquidacion As New Capa_Datos.WC_Liquidacion
  Dim DArecorrido As New Capa_Datos.WC_recorridos_zonas
  Dim DAclientes As New Capa_Datos.WB_clientes

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      Permisos()
      'AQUI VALIDO, SI NO HAY NINGUNA FECHA EN LA TABLA PARAMETRO, PONGO UN MENSAJE MODAL QUE DIGA:
      'ERROR, PRIMERO DEBE INICIAR DIA.
      Dim ds_info As DataSet = DAparametro.Parametro_obtener_dia
      If ds_info.Tables(0).Rows.Count <> 0 Then
        'cargo la fecha y el dia en los textbox
        HF_parametro_id.Value = ds_info.Tables(0).Rows(0).Item("Parametro_id")
        Dim FECHA As Date = CDate(ds_info.Tables(0).Rows(0).Item("Fecha"))
        HF_fecha.Value = ds_info.Tables(0).Rows(0).Item("Fecha")
        'Label_fecha.Text = FECHA.ToString("yyyy-MM-dd")
        Label_fecha.Text = FECHA.ToString("dd-MM-yyyy")
        Dim dia As Integer = CInt(ds_info.Tables(0).Rows(0).Item("Dia"))
        HF_dia_id.Value = dia
        Select Case dia
          Case 1
            Label_dia.Text = "DOMINGO."
          Case 2
            Label_dia.Text = "LUNES."
          Case 3
            Label_dia.Text = "MARTES."
          Case 4
            Label_dia.Text = "MIERCOLES."
          Case 5
            Label_dia.Text = "JUEVES."
          Case 6
            Label_dia.Text = "VIERNES."
          Case 7
            Label_dia.Text = "SABADO."
        End Select
        METODO1()


      Else
        'AQUI MENSAJE Y QUE CON EL BOTON "OK" U "CLOSE" VUELVA AL MENU PRINCIPAL.
        LabelMsj_Modal_ok_error.Text = "Primero debe iniciar el dia!"
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)

      End If



    End If
  End Sub

  Private Sub METODO1()
    'IMPORTANTE SE CARGA DESDE CERO LA TABLA XCARGAS Y XCARGAS RECORRIDOS. FECHA: 22-08-04
    DALiquidacion.XCargas_load()

    Dim DS_liqparcial As New DS_liqparcial
    '1ra VALIDACION.------------------------------------
    Dim check As String = "no"
    Dim valido As String = "si"
    Dim codigo_error As String = "" 'aqui se va a almacenar el codigo donde la validación falló, para poder mostrarlo posteriormente en un mensaje al usuario.
    Dim valido_xcargas As String = "si"
    'validamos todos los elementos de Recorrido1

    Validacion(DS_liqparcial, valido, valido_xcargas, codigo_error, check)

    If valido = "si" Then
      'en la rutina VALIDACION se cargaron los codigos de las zonas habilitadas aqui: DS_liqparcial.Tables("Recorridos_seleccionados")

      Dim DS_liqfinal As New DS_liqfinal


      Dim I As Integer = 0
      While I < DS_liqparcial.Tables("Recorridos_seleccionados").Rows.Count
        Label_recorridos.Text += DS_liqparcial.Tables("Recorridos_seleccionados").Rows(I).Item("Codigo")
        I = I + 1
      End While




      Liquidacion(DS_liqparcial, DS_liqfinal)



    Else
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ErrorValidacion", "$(document).ready(function () {$('#modal-ErrorValidacion').modal();});", True)
    End If

  End Sub

  Private Sub Liquidacion(ByRef DS_liqparcial As DataSet, ByRef DS_liqfinal As DataSet)
    Dim CADENA_RECORRIDOS As String = ""
    'Dim CADENA_RECORTADA As String = CADENA_RECORRIDOS.Replace("1E", "")
    'CADENA_RECORTADA = CADENA_RECORTADA.Replace("2C", "") 'LA CADENA RESULTANTE DEBERIA SER: 1A1B1C1D2A2B2D
    Dim dc As Integer = 0
    While dc < DS_liqparcial.Tables("Recorridos_seleccionados").Rows.Count
      CADENA_RECORRIDOS += DS_liqparcial.Tables("Recorridos_seleccionados").Rows(dc).Item("Codigo")

      dc = dc + 1
    End While


    Dim DS_liqparcial1 As New DS_liqparcial


    DS_liqparcial1.Tables("RecaudacionxCliente").Rows.Clear()
    '//////////////voy a llenar el datatable con todos los clientes.
    Dim ds_clie As DataSet = DAclientes.Obtener_ordenados_codigo()
    DS_liqparcial1.Tables("RecaudacionxCliente").Merge(ds_clie.Tables(0))

    dc = 0
    While dc < DS_liqparcial1.Tables("RecaudacionxCliente").Rows.Count
      Dim Codigo As String = CStr(DS_liqparcial1.Tables("RecaudacionxCliente").Rows(dc).Item("Cliente"))
      Dim ds_recaudacion As DataSet = DALiquidacion.XCargas_recuperarclierecaudacion(HF_fecha.Value, Codigo)
      If ds_recaudacion.Tables(0).Rows.Count <> 0 Then
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(dc).Item("Recaudacion") = ds_recaudacion.Tables(0).Rows(0).Item("Importe")
      Else
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(dc).Item("Recaudacion") = CDec(0)
      End If
      DS_liqparcial1.Tables("RecaudacionxCliente").Rows(dc).Item("RecNoTrabajados") = CADENA_RECORRIDOS

      Dim j As Integer = 0
      While j < ds_recaudacion.Tables(1).Rows.Count
        '//////////////quitar recorrido trabajado///////////////
        Dim rectrabajado As String = ds_recaudacion.Tables(1).Rows(j).Item("Recorrido_codigo")
        'DS_liqparcial1.Tables("RecaudacionxCliente").Rows(dc).Item("RecNoTrabajados")
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(dc).Item("RecNoTrabajados") = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(dc).Item("RecNoTrabajados").ToString.Replace(rectrabajado, "")
        '//////////////////////////////////////////////////////
        j = j + 1
      End While
      dc = dc + 1
    End While


    DS_liqparcial1.Tables("PremiosxClientes").Rows.Clear()

    Dim valor11 = 1234
    Dim undigito As String = valor11.ToString.Substring(3, 1)
    Dim dosdigitos As String = valor11.ToString.Substring(2, 2)
    Dim tresdigitos As String = valor11.ToString.Substring(1, 3)

    Dim ds_zonasHab As DataSet = DArecorrido.recorridos_zonas_ObtenerTodoHabilitados_x_dia(CStr(HF_dia_id.Value), HF_fecha.Value)

    Dim ee As Integer = 0
    While ee < ds_zonasHab.Tables(0).Rows.Count
      '----------PREMIOS ETAPA 1-------------------------------------
      Dim Codigo As String = ds_zonasHab.Tables(0).Rows(ee).Item("Codigo")
      Dim referencia_recorrido As String = ds_zonasHab.Tables(0).Rows(ee).Item("Referencia").ToString.ToUpper
      '----------OP 1----------
      Dim ds_filtroOP1 As DataSet = DArecorrido.Liquidacion_recuperarXcargas_FiltroOP1(Codigo, HF_fecha.Value)
      Dim jj As Integer = 0
      While jj < ds_filtroOP1.Tables(0).Rows.Count
        If CDec(ds_filtroOP1.Tables(0).Rows(jj).Item("Importe")) <> CDec(0) Then

          Dim XCargas_Pid = ds_filtroOP1.Tables(0).Rows(jj).Item("Pid")
          'Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
          'Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
          Dim Puntos_P1 = ds_zonasHab.Tables(0).Rows(ee).Item("P1")
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
      'AQUI COMENTO OP 2
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
            Dim PtoSelec = ds_zonasHab.Tables(0).Rows(ee).Item(IndicePuntos)
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
      'FIN COMENTO OP 2

      '---------op3------------
      'AQUI COMENTO OP 3
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
          Dim Puntos_P1 = ds_zonasHab.Tables(0).Rows(ee).Item("P1")
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

              Dim PtoSelec = ds_zonasHab.Tables(0).Rows(ee).Item(IndicePuntos)
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
            Dim PtoSelec = ds_zonasHab.Tables(0).Rows(ee).Item(IndicePuntos)
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
            Dim PtoSelec = ds_zonasHab.Tables(0).Rows(ee).Item(IndicePuntos)
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

      ee = ee + 1
    End While

    Dim RecaudacionTotal As Decimal = 0
    Dim PremiosTotal As Decimal = 0
    Dim Totales As Decimal = 0
    Dim i As Integer = 0
    While i < DS_liqparcial1.Tables("RecaudacionxCliente").Rows.Count
      RecaudacionTotal = RecaudacionTotal + CDec(DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Recaudacion"))
      PremiosTotal = PremiosTotal + CDec(DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios"))
      Totales = Totales + CDec(DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Total"))

      Dim filaa As DataRow = DS_liqparcial1.Tables("RecaudacionxCliente1").NewRow
      filaa("Cliente") = CStr(DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Cliente"))
      filaa("Recaudacion") = CDec(DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Recaudacion"))
      filaa("Premios") = CDec(DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios"))
      filaa("Total") = CDec(DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Total"))
      filaa("RecNoTrabajados") = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("RecNoTrabajados")
      DS_liqparcial1.Tables("RecaudacionxCliente1").Rows.Add(filaa)

      i = i + 1
    End While

    'Aqui genero el reporte. antes de agregar la ultima fila que tiene los totales
    Dim fila_info As DataRow = DS_liqparcial1.Tables("RecaudacionxCliente_info").NewRow
    fila_info("Fecha") = HF_fecha.Value
    fila_info("Dia") = Label_dia.Text
    fila_info("Recorrido") = Label_recorridos.Text.ToUpper
    DS_liqparcial1.Tables("RecaudacionxCliente_info").Rows.Add(fila_info)

    Try
      Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
      CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
      CrReport.Load(Server.MapPath("~/WC_Reportes/RecaudacionxClientes_informe01.rpt"))
      CrReport.Database.Tables("RecaudacionxCliente_info").SetDataSource(DS_liqparcial1.Tables("RecaudacionxCliente_info"))
      CrReport.Database.Tables("RecaudacionxCliente1").SetDataSource(DS_liqparcial1.Tables("RecaudacionxCliente1"))
      CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/RecaudacionxCliente.pdf"))
    Catch ex As Exception

    End Try

    Dim fila As DataRow = DS_liqparcial1.Tables("RecaudacionxCliente1").NewRow
    fila("Cliente") = "TOTALES"
    fila("Recaudacion") = (Math.Round(RecaudacionTotal, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
    fila("Premios") = (Math.Round(PremiosTotal, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
    fila("Total") = (Math.Round(Totales, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

    DS_liqparcial1.Tables("RecaudacionxCliente1").Rows.Add(fila)

    GridView1.DataSource = DS_liqparcial1.Tables("RecaudacionxCliente1")



    GridView1.DataBind()


  End Sub



  Private Sub grabar_premios_op1(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String)

    'Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientes").NewRow
    'fila("Cliente") = registro.Item("Cliente")
    'fila("Recorrido") = referencia_recorrido
    'fila("Importe") = registro.Item("Importe")
    'fila("PID") = registro.Item("Pid")
    'fila("SUC") = registro.Item("Suc")
    'fila("P2") = ""

    'Dim SinComputo As Boolean = False
    'Try
    '  SinComputo = registro.Item("SinComputo")
    'Catch ex As Exception
    'End Try
    'If SinComputo = True Then
    '  fila("SC") = "X"
    'Else
    '  fila("SC") = ""
    'End If

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
    'fila("Premio") = (Math.Round(premio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

    'fila("T") = registro.Item("Terminal").ToString.ToUpper

    'fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.
    Dim ds_cliente As DataSet = DAclientes.Clientes_buscar_codigo(registro.Item("Cliente"))
    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        'fila("OBS") = "CUB."
        'fila("Premio") = fila("Premio") * (-1)
        premio = premio * (-1)
      End If
    End If


    'DS_liqparcial1.Tables("PremiosxClientes").Rows.Add(fila)

    '-----------------------AQUI GUARDO EN LA BASE DATOS ----NUEVO REGISTRO EN TABLA PREMIOS----------------------
    Dim NroTicket As String = ""
    If registro.Item("Terminal").ToString.ToUpper = "W" Then
      NroTicket = registro.Item("Item").ToString
    End If
    'DAPremios.Premios_altaOP1y2(HF_fecha.Value, registro.Item("Recorrido_codigo"), CStr(registro.Item("Pid")), CDec(registro.Item("Importe")), CInt(registro.Item("Suc")), CInt(0), CInt(SinComputo), premio, NroTicket, CStr(registro.Item("Terminal")), CStr(registro.Item("Cliente")))


    '///////////LO BUSCO Y SUMO EL PREMIO, calculo total
    Dim i As Integer = 0
    While i < DS_liqparcial1.Tables("RecaudacionxCliente").Rows.Count
      If DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Cliente") = CStr(registro.Item("Cliente")) Then
        Dim Recaudacion As Decimal = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Recaudacion")
        Dim SUMpremio As Decimal = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios") + premio
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios") = (Math.Round(SUMpremio, 2).ToString("N2"))
        Dim Total As Decimal = Recaudacion - DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios")
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Total") = (Math.Round(Total, 2).ToString("N2"))


        Exit While
      End If
      i = i + 1
    End While
  End Sub

  Private Sub grabar_premios_op2(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia As Integer)
    'Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientes").NewRow
    'fila("Cliente") = registro.Item("Cliente")
    'fila("Recorrido") = referencia_recorrido
    'fila("Importe") = registro.Item("Importe")
    'fila("PID") = registro.Item("Pid")
    'fila("SUC") = registro.Item("Suc")
    'fila("P2") = ""
    'Dim SinComputo As Boolean = False
    'Try
    '  SinComputo = registro.Item("SinComputo")
    'Catch ex As Exception

    'End Try
    'If SinComputo = True Then
    '  fila("SC") = "X"
    'Else
    '  fila("SC") = ""
    'End If
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

    'fila("Premio") = (Math.Round(premio, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento


    'fila("T") = registro.Item("Terminal").ToString.ToUpper
    'fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.
    Dim ds_cliente As DataSet = DAclientes.Clientes_buscar_codigo(registro.Item("Cliente"))
    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        'fila("OBS") = "CUB."
        'fila("Premio") = fila("Premio") * (-1)
        premio = premio * (-1)
      End If
    End If
    'DS_liqparcial1.Tables("PremiosxClientes").Rows.Add(fila)

    '-----------------------AQUI GUARDO EN LA BASE DATOS ----NUEVO REGISTRO EN TABLA PREMIOS----------------------
    Dim NroTicket As String = ""
    If registro.Item("Terminal").ToString.ToUpper = "W" Then
      NroTicket = registro.Item("Item").ToString
    End If
    'DAPremios.Premios_altaOP1y2(HF_fecha.Value, registro.Item("Recorrido_codigo"), CStr(registro.Item("Pid")), CDec(registro.Item("Importe")), CInt(registro.Item("Suc")), 0, CInt(SinComputo),premio, NroTicket, CStr(registro.Item("Terminal")), CStr(registro.Item("Cliente")))

    '///////////LO BUSCO Y SUMO EL PREMIO, calculo total
    Dim i As Integer = 0
    While i < DS_liqparcial1.Tables("RecaudacionxCliente").Rows.Count
      If DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Cliente") = CStr(registro.Item("Cliente")) Then
        Dim Recaudacion As Decimal = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Recaudacion")
        Dim SUMpremio As Decimal = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios") + premio
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios") = (Math.Round(SUMpremio, 2).ToString("N2"))
        Dim Total As Decimal = Recaudacion - DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios")
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Total") = (Math.Round(Total, 2).ToString("N2"))


        Exit While
      End If
      i = i + 1
    End While


  End Sub

  Private Sub grabar_premios_op3(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia As Integer)
    'Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientes").NewRow
    'fila("Cliente") = registro.Item("Cliente")
    'fila("Recorrido") = referencia_recorrido
    'fila("Importe") = registro.Item("Importe")
    'fila("PID") = registro.Item("Pid")
    'fila("SUC") = registro.Item("Suc")
    'fila("S2") = registro.Item("Suc2")
    'fila("P2") = registro.Item("Pid2")
    Dim premio As Decimal = 0
    'Dim SinComputo As Boolean = False
    'Try
    '  SinComputo = registro.Item("SinComputo")
    'Catch ex As Exception

    'End Try
    'If SinComputo = True Then
    '  fila("SC") = "X"
    'Else
    '  fila("SC") = ""
    'End If

    '----CORRECCIONES 2022-12-22----------------------------------------------------------------------------
    'FACTOR: ahora se recupera Factor que puede tomar el valor 80 si es true y 70 si es false. Este valor cambia la formula para obtener el "premio"
    Dim ds_cliente As DataSet = DAclientes.Clientes_buscar_codigo(registro.Item("Cliente"))
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
        Dim Suc2 As Integer = CInt(registro.Item("Suc2"))
        Dim Importe As Decimal = CDec(registro.Item("Importe"))
        'premio = Importe * 80 * ((80 / Suc2) * ContCoincidencia)
        premio = Importe * Factor * ((Factor / Suc2) * ContCoincidencia)
        'fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
      Else
        '-------------------------------------------------------------------------------------------------------------------------------------------
        '-------------•	si XCargasL.Suc2 = 20, dbo.Premios.Premio = el valor del importe encontrado en el registro
        '-------------de la coincidencia dbo.XCargasL.Importe * 80 * ((80 / 19) * la cantidad de veces que------------------------------------------
        '-------------coincidio dbo.XCargasL.Pid2 dentro de la dbo.Puntos.P(hasta el valor de dbo.XCargasL.Suc2))-----------------------------------
        '-------------------------------------------------------------------------------------------------------------------------------------------
        If CInt(registro.Item("Suc2")) = 20 Then
          Dim Importe As Decimal = CDec(registro.Item("Importe"))
          'premio = Importe * 80 * ((80 / 19) * ContCoincidencia)
          premio = Importe * Factor * ((Factor / 19) * ContCoincidencia)
          'fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
        End If
      End If
    Catch ex As Exception

    End Try
    'fila("T") = registro.Item("Terminal").ToString.ToUpper
    'fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.

    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        'fila("OBS") = "CUB."
        'fila("Premio") = fila("Premio") * (-1)
        premio = premio * (-1)
      End If
    End If
    'DS_liqparcial1.Tables("PremiosxClientes").Rows.Add(fila)

    '-----------------------AQUI GUARDO EN LA BASE DATOS ----NUEVO REGISTRO EN TABLA PREMIOS----------------------
    Dim NroTicket As String = ""
    If registro.Item("Terminal").ToString.ToUpper = "W" Then
      NroTicket = registro.Item("Item").ToString
    End If
    'DAPremios.Premios_altaOP3y4(HF_fecha.Value, registro.Item("Recorrido_codigo"), CStr(registro.Item("Pid")), CDec(registro.Item("Importe")), CInt(registro.Item("Suc")), CStr(registro.Item("Pid2")), CInt(registro.Item("Suc2")), 1, CInt(SinComputo), premio, NroTicket, CStr(registro.Item("Terminal")), CStr(registro.Item("Cliente")))


    '///////////LO BUSCO Y SUMO EL PREMIO, calculo total
    Dim i As Integer = 0
    While i < DS_liqparcial1.Tables("RecaudacionxCliente").Rows.Count
      If DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Cliente") = CStr(registro.Item("Cliente")) Then
        Dim Recaudacion As Decimal = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Recaudacion")
        Dim SUMpremio As Decimal = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios") + premio
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios") = (Math.Round(SUMpremio, 2).ToString("N2"))
        Dim Total As Decimal = Recaudacion - DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios")
        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Total") = (Math.Round(Total, 2).ToString("N2"))


        Exit While
      End If
      i = i + 1
    End While
  End Sub

  Private Sub grabar_premios_op4(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia1 As Integer, ByVal ContCoincidencia2 As Integer)
    'Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientes").NewRow
    'fila("Cliente") = registro.Item("Cliente")
    'fila("Recorrido") = referencia_recorrido
    'fila("Importe") = registro.Item("Importe")
    'fila("PID") = registro.Item("Pid")
    'fila("SUC") = registro.Item("Suc")
    'fila("S2") = registro.Item("Suc2")
    'fila("P2") = registro.Item("Pid2")
    Dim premio As Decimal = 0
    'Dim SinComputo As Boolean = False
    'Try
    '  SinComputo = registro.Item("SinComputo")
    'Catch ex As Exception

    'End Try
    'If SinComputo = True Then
    '  fila("SC") = "X"
    'Else
    '  fila("SC") = ""
    'End If
    '----CORRECCIONES 2022-12-22----------------------------------------------------------------------------
    'FACTOR: ahora se recupera Factor que puede tomar el valor 80 si es true y 70 si es false. Este valor cambia la formula para obtener el "premio"
    Dim ds_cliente As DataSet = DAclientes.Clientes_buscar_codigo(registro.Item("Cliente"))
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
      'premio = importe * ((80 / suc) * ContCoincidencia1) * ((80 / suc2) * ContCoincidencia2)
      premio = importe * ((Factor / suc) * ContCoincidencia1) * ((Factor / suc2) * ContCoincidencia2)
      'fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
    Catch ex As Exception

    End Try
    'fila("T") = registro.Item("Terminal").ToString.ToUpper
    'fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.

    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        'fila("OBS") = "CUB."
        'fila("Premio") = fila("Premio") * (-1)
        premio = premio * (-1)
      End If
    End If
    'DS_liqparcial1.Tables("PremiosxClientes").Rows.Add(fila)

    '-----------------------AQUI GUARDO EN LA BASE DATOS ----NUEVO REGISTRO EN TABLA PREMIOS----------------------
    Dim NroTicket As String = ""
    If registro.Item("Terminal").ToString.ToUpper = "W" Then
      NroTicket = registro.Item("Item").ToString
    End If
    'DAPremios.Premios_altaOP3y4(HF_fecha.Value, registro.Item("Recorrido_codigo"),CStr(registro.Item("Pid")), CDec(registro.Item("Importe")),  CInt(registro.Item("Suc")), CStr(registro.Item("Pid2")), CInt(registro.Item("Suc2")), 1, CInt(SinComputo), premio, NroTicket, CStr(registro.Item("Terminal")), CStr(registro.Item("Cliente")))

    '///////////LO BUSCO Y SUMO EL PREMIO, calculo total
    Dim i As Integer = 0
    While i < DS_liqparcial1.Tables("RecaudacionxCliente").Rows.Count
      If DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Cliente") = CStr(registro.Item("Cliente")) Then
        Dim Recaudacion As Decimal = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Recaudacion")
        Dim SUMpremio As Decimal = DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios") + premio

        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios") = (Math.Round(SUMpremio, 2).ToString("N2"))
        Dim Total As Decimal = Recaudacion - DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Premios")

        DS_liqparcial1.Tables("RecaudacionxCliente").Rows(i).Item("Total") = (Math.Round(Total, 2).ToString("N2"))

        Exit While
      End If
      i = i + 1
    End While


  End Sub





  Private Sub Validacion(ByRef DS_liqparcial As DataSet, ByRef valido As String, ByRef valido_xcargas As String, ByRef codigo_error As String, ByRef check As String)
    'recupero los codigos de las zonas habilitadas para el dia vigente.
    Dim DS_Recorridos As DataSet = DArecorrido.recorridos_zonas_obtener_habilitados_x_dia(HF_dia_id.Value)
    'voy a recorrer todas las zonas habilitadas para el dia
    Dim ContZonaHab As Integer = 0 'contar zonas habilitadas
    Dim i As Integer = 0
    While i < DS_Recorridos.Tables(1).Rows.Count
      Dim Habilitada As Integer = conv_bit(CInt(DS_Recorridos.Tables(1).Rows(i).Item("Habilitada")))
      Dim codigo As String = DS_Recorridos.Tables(1).Rows(i).Item("Codigo")
      If Habilitada = 1 Then
        '1 VALIDACACION, contar zonas habilitadas 
        ContZonaHab = ContZonaHab + 1
        If valido = "si" And valido_xcargas = "si" Then

          'CORRECCION 2023-04-03 NO VALIDO PUNTOS CARGADOS---  
          '2 VALIDACION: que al menos tenga 1 punto para la zona habilitada
          'Validar_recorridos_a(valido, codigo, codigo_error, check)

          valido = "si"


        End If
      End If

      i = i + 1
    End While
    If ContZonaHab <> 0 Then
      If valido = "no" Then
        'mensaje error: alguna de las zonas no tiene puntos asignados.
        Label_ErrorValidacion.Text = "Alguna de las zonas no tiene puntos cargados."
      End If
    Else
      'mensaje...no hay zonas habilitadas
      Label_ErrorValidacion.Text = "No hay zonas habilitadas."
      valido = "no"
    End If

    If valido = "si" Then
      Dim ds_xcargas As DataSet = DALiquidacion.Liquidacion_todoXcargas
      '3 VALIDACION: QUE EXISTA AL MENOS 1 REGISTRO EN XCARGAS
      If ds_xcargas.Tables(0).Rows.Count <> 0 Then
        '4 VALIDACION: CONTROLAR QUE NO EXISTA UN REGISTRO CON FECHA DIFERENTE A LA DEL PARAMETRO
        Dim error_tipo As String = ""
        Dim j As Integer = 0
        While j < ds_xcargas.Tables(0).Rows.Count
          Dim fecha As Date = CDate(ds_xcargas.Tables(0).Rows(j).Item("Fecha"))

          If fecha = HF_fecha.Value Then
            '5 VALIDACION: Controlar que no exista ninguna Zona dentro de las tablas XCargas.. que no este habilitada en la tabla Parametro.
            '----------------------------------------------------------------------------------------------------------
            Dim Recorrido_codigo As String = ds_xcargas.Tables(0).Rows(j).Item("Recorrido_codigo")
            i = 0
            While i < DS_Recorridos.Tables(1).Rows.Count
              Dim Habilitada As Integer = conv_bit(CInt(DS_Recorridos.Tables(1).Rows(i).Item("Habilitada")))
              Dim codigo As String = DS_Recorridos.Tables(1).Rows(i).Item("Codigo")
              If Recorrido_codigo = codigo And Habilitada = 0 Then
                valido = "no"
                error_tipo = "Zona"
                Exit While
              End If

              i = i + 1
            End While
            If valido = "no" Then
              Exit While
            End If
            '----------------------------------------------------------------------------------------------------------
          Else
            valido = "no"
            error_tipo = "fecha"
            Exit While
          End If
          j = j + 1
        End While
        If valido = "si" Then
          'continuo con las validaciones
          Dim ds_banderas As DataSet = DALiquidacion.Liquidacion_obtenerBanderas
          '6 VALIDACION Y ULTIMA: Controlar que el campo "Web" de la tabla Banderas este en False.(Si se encuentra en True, mostrar mensaje y salir del proceso de liquidacion)
          Dim WEB = ds_banderas.Tables(0).Rows(0).Item("Web")
          If WEB = False Then
            'validacion CORRECTA
            valido = "si"
            Cargar_recorrido_habilitados(DS_Recorridos, DS_liqparcial)
          Else
            valido = "no"
            'mensaje error: bandera.web = true
            Label_ErrorValidacion.Text = "No se puede realizar la liquidacion. Consulte bandera.web."
          End If
        Else
          Select Case error_tipo
            Case "fecha"
              'mensaje error: uno de los registros en xcargas tiene fecha diferente.
              Label_ErrorValidacion.Text = "Registro en Xcargas con fecha diferente."
            Case "Zona"
              'mensaje error: se encontro un registro en xcargas con info de una zona no habilitada.
              Label_ErrorValidacion.Text = "Registro con zona no habilitada en Xcargas."
          End Select
        End If

      Else
        'no hay registros en xcargas
        Label_ErrorValidacion.Text = "No hay registros en Xcargas."
        valido = "no"
      End If


    End If

  End Sub

  Private Sub Cargar_recorrido_habilitados(ByRef DS_Recorridos As DataSet, ByRef DS_liqparcial As DataSet)
    Dim i As Integer = 0
    While i < DS_Recorridos.Tables(1).Rows.Count
      Dim Habilitada As Integer = conv_bit(CInt(DS_Recorridos.Tables(1).Rows(i).Item("Habilitada")))
      Dim codigo As String = DS_Recorridos.Tables(1).Rows(i).Item("Codigo")
      If Habilitada = 1 Then
        Dim fila As DataRow = DS_liqparcial.Tables("Recorridos_seleccionados").NewRow
        fila("Codigo") = codigo
        DS_liqparcial.Tables("Recorridos_seleccionados").Rows.Add(fila)
      End If
      i = i + 1
    End While
  End Sub

  Public Function conv_bit(ByRef estado As Integer)
    If estado = -1 Then
      estado = 1
    Else
      If estado = 0 Then

      End If
    End If
    Return estado
  End Function

  Private Sub Validar_recorridos_a(ByRef valido As String, ByVal Codigo As String, ByRef codigo_error As String, ByRef check As String)
    check = "si"

    'valido que exista al menos 1 punto cargado para el item seleccionado.
    Dim dataset_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, Codigo)
    Dim validacion As String = "no"
    If dataset_recorridos.Tables(0).Rows.Count <> 0 Then
      Dim punto_encontrado As String = ""
      Dim i As Integer = 7 'desde p1 a p20...va 7 porque es la posicion de la columna en el dataset
      While i < 27
        If dataset_recorridos.Tables(0).Rows(0).Item(i) <> "" Then
          validacion = "si"
          Exit While
        End If
        i = i + 1
      End While
      If validacion = "si" Then
        valido = "si"
        codigo_error = ""
      Else
        valido = "no"
        codigo_error = "No se encontraron puntos cargados."
      End If
    Else
      valido = "no"
      codigo_error = "No se encontraron puntos cargados."
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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 4 or null

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
            If (Menu = "5" And Opcion = "") Or (Menu = "5" And Opcion = "5") Then
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

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Btn_ErrorValidacionOk_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorValidacionOk.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Btn_ErrorValidacionClose_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorValidacionClose.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_ok_error_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_error_close_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub
End Class
