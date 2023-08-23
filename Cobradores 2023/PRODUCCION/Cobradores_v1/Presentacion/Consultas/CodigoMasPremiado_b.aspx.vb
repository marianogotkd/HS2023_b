Public Class CodigoMasPremiado_b
  Inherits System.Web.UI.Page

#Region "Declaraciones"
  Dim Daparametro As New Capa_Datos.WC_parametro
  Dim DALConsultas As New Capa_Datos.WB_Consultas
  Dim DALiquidacion As New Capa_Datos.WC_Liquidacion
  Dim Lista1Cifras As New List(Of Capa_Datos.CodigoMasCargadoDTO)
  Dim Lista2Cifras As New List(Of Capa_Datos.CodigoMasCargadoDTO)
  Dim Lista3Cifras As New List(Of Capa_Datos.CodigoMasCargadoDTO)
  Dim Lista4Cifras As New List(Of Capa_Datos.CodigoMasCargadoDTO)
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DAConsultas As New Capa_Datos.WB_Consultas
  Dim DArecorrido As New Capa_Datos.WC_recorridos_zonas


#End Region

  'AQUI TRABAJO 2023-02-22 ---CODIGO PREMIADO

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos()
      TxtCodigo.Focus()
      'recuperar fecha de tabla parametro.

      HF_fecha.Value = Session("fecha_parametro")
      HF_dia_id.Value = Session("dia")
      Dim ds_fecha As DataSet = Daparametro.Parametro_obtener_dia
      If ds_fecha.Tables(0).Rows.Count <> 0 Then
        Dim FECHA As Date = CDate(ds_fecha.Tables(0).Rows(0).Item("Fecha"))
        'txt_fecha.Text = FECHA.ToString("yyyy-MM-dd")

        Txt_fecha.Text = FECHA.ToString("yyyy-MM-dd")
        Dim dia As Integer = Session("dia")
        Select Case dia
          Case 1
            Label_dia.Text = "DIA: DOMINGO."
          Case 2
            Label_dia.Text = "DIA: LUNES."
          Case 3
            Label_dia.Text = "DIA: MARTES."
          Case 4
            Label_dia.Text = "DIA: MIERCOLES."
          Case 5
            Label_dia.Text = "DIA: JUEVES."
          Case 6
            Label_dia.Text = "DIA: VIERNES."
          Case 7
            Label_dia.Text = "DIA: SABADO."
        End Select

        Dim DS_liqparcial As New DS_liqparcial
        DS_liqparcial.Tables("Recorridos_seleccionados").Merge(Session("tabla_recorridos_seleccionados"))
        TxtZona.Text = DS_liqparcial.Tables("Recorridos_seleccionados").Rows(0).Item("Codigo").ToString.ToUpper
        TxtCodigo.Focus()

        txtImporte1.ReadOnly = True
        txtImporte2.ReadOnly = True
        txtImporte3.ReadOnly = True
        txtImporte4.ReadOnly = True

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
              Menu = ds_permisos.Tables(0).Rows(i).Item("Menu")
            Catch ex As Exception
            End Try
            Dim Opcion As String = ""
            Try
              Opcion = ds_permisos.Tables(0).Rows(i).Item("Opcion")
            Catch ex As Exception
            End Try
            If (Menu = "5" And Opcion = "") Or (Menu = "5" And Opcion = "2") Then
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

  Private Sub btn_retroceder_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/CodigoMasPremiadoRecorridos_b.aspx")
  End Sub

  Private Sub BusquedaValidadInicial()


    Try
      txtImporte1.Text = CDec(txtImporte1.Text.Replace(".", ","))
    Catch ex As Exception
      txtImporte1.Text = CDec(0)
    End Try
    Try
      txtImporte2.Text = CDec(txtImporte2.Text.Replace(".", ","))
    Catch ex As Exception
      txtImporte2.Text = CDec(0)
    End Try
    Try
      txtImporte3.Text = CDec(txtImporte3.Text.Replace(".", ","))
    Catch ex As Exception
      txtImporte3.Text = CDec(0)
    End Try
    Try
      txtImporte4.Text = CDec(txtImporte4.Text.Replace(".", ","))
    Catch ex As Exception
      txtImporte4.Text = CDec(0)
    End Try

  End Sub

  Private Sub btn_ok_error_busqueda01_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error_busqueda01.ServerClick
    TxtCodigo.Focus()
  End Sub

  Private Sub btn_close_error_busqueda01_ServerClick(sender As Object, e As EventArgs) Handles btn_close_error_busqueda01.ServerClick
    TxtCodigo.Focus()
  End Sub

  Private Sub txtImporte1_Init(sender As Object, e As EventArgs) Handles txtImporte1.Init
    txtImporte1.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub
  Private Sub txtImporte2_Init(sender As Object, e As EventArgs) Handles txtImporte2.Init
    txtImporte2.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub
  Private Sub txtImporte3_Init(sender As Object, e As EventArgs) Handles txtImporte3.Init
    txtImporte3.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub
  Private Sub txtImporte4_Init(sender As Object, e As EventArgs) Handles txtImporte4.Init
    txtImporte4.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub TxtCodigo_Init(sender As Object, e As EventArgs) Handles TxtCodigo.Init
    TxtCodigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub


  Private Sub Buscar_nuevo()
    Dim DS_Consultas As New DS_Consultas
    DS_Consultas.Tables("ZonasHabilitadas_importes").Rows.Clear()
    Dim DS_Recorridos As DataSet = DArecorrido.recorridos_zonas_obtener_habilitados_x_dia(HF_dia_id.Value)
    Dim i As Integer = 0
    While i < DS_Recorridos.Tables(0).Rows.Count

      If DS_Recorridos.Tables(0).Rows(i).Item("Habilitada") = True Then
      'agrego.
      Dim Codigo As String = DS_Recorridos.Tables(0).Rows(i).Item("Codigo")
        Dim numero_codigo As String = Codigo.Substring(0, 1)

        Dim j As Integer = 0
        Dim zona_agregada As String = "no"
        While j < DS_Consultas.Tables("ZonasHabilitadas_importes").Rows.Count

          Select Case Codigo.Substring(0, 1)
            Case "1"
              Dim Zona1 As String = ""
              Try
                Zona1 = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona1"))
              Catch ex As Exception
              End Try
              If Zona1 = "" Then
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona1") = Codigo
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe1") = CDec(0)
                zona_agregada = "si"
                Exit While
              End If
            Case "2"
              Dim Zona2 As String = ""
              Try
                Zona2 = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona2"))
              Catch ex As Exception
              End Try
              If Zona2 = "" Then
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona2") = Codigo
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe2") = CDec(0)
                zona_agregada = "si"
                Exit While
              End If
            Case "3"
              Dim Zona3 As String = ""
              Try
                Zona3 = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona3"))
              Catch ex As Exception
              End Try
              If Zona3 = "" Then
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona3") = Codigo
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe3") = CDec(0)
                zona_agregada = "si"
                Exit While
              End If
            Case "4"
              Dim Zona4 As String = ""
              Try
                Zona4 = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona4"))
              Catch ex As Exception
              End Try
              If Zona4 = "" Then
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona4") = Codigo
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe4") = CDec(0)
                zona_agregada = "si"
                Exit While
              End If
            Case "5"
              Dim Zona5 As String = ""
              Try
                Zona5 = CStr(DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona5"))
              Catch ex As Exception
              End Try
              If Zona5 = "" Then
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona5") = Codigo
                DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe5") = CDec(0)
                zona_agregada = "si"
                Exit While
              End If
          End Select
          j = j + 1
        End While

        If zona_agregada = "no" Then
          Dim fila As DataRow = DS_Consultas.Tables("ZonasHabilitadas_importes").NewRow
          Select Case numero_codigo
            Case "1"
              fila("Zona1") = Codigo
              fila("Importe1") = CDec(0)
            Case "2"
              fila("Zona2") = Codigo
              fila("Importe2") = CDec(0)
            Case "3"
              fila("Zona3") = Codigo
              fila("Importe3") = CDec(0)
            Case "4"
              fila("Zona4") = Codigo
              fila("Importe4") = CDec(0)
            Case "5"
              fila("Zona5") = Codigo
              fila("Importe5") = CDec(0)
          End Select
          DS_Consultas.Tables("ZonasHabilitadas_importes").Rows.Add(fila)
        End If

      End If

    i = i + 1
    End While



    'IMPORTANTE SE CARGA DESDE CERO LA TABLA XCARGAS Y XCARGAS RECORRIDOS. FECHA: 22-08-04
    DALiquidacion.XCargas_load()

    Dim unacifra As Decimal = 0
    Dim doscifras As Decimal = 0
    Dim trescifras As Decimal = 0
    Dim cuatrocifras As Decimal = 0

    Dim TOTAL_RECAUDADO As Decimal = 0

    Dim codigo_ingresado As String = TxtCodigo.Text

    Dim DS_XCargas As DataSet = DALiquidacion.Liquidacion_todoXcargas
    i = 0
    While i < DS_XCargas.Tables(0).Rows.Count
      TOTAL_RECAUDADO = TOTAL_RECAUDADO + DS_XCargas.Tables(0).Rows(i).Item("Importe")

      Dim Recorrido_codigo As String = ""
      Try
        Recorrido_codigo = DS_XCargas.Tables(0).Rows(i).Item("Recorrido_codigo")
      Catch ex As Exception

      End Try

      If Recorrido_codigo = txtZona.Text Then
        Dim Pid = ""
        Try
          Pid = CStr(DS_XCargas.Tables(0).Rows(i).Item("Pid"))
        Catch ex As Exception

        End Try
        Dim Valor_p = codigo_ingresado
        Select Case Len(Pid) 'devuelve cantidad de digitos en pid
          Case 1
            'comprar con 1 digito en "Valor_p"
            Select Case Len(codigo_ingresado)
              Case 1
                'Puntos_P1 = Puntos_P1
              Case 2
                Valor_p = Valor_p.ToString.Substring(1, 1)
              Case 3
                Valor_p = Valor_p.ToString.Substring(2, 1)
              Case 4
                Valor_p = Valor_p.ToString.Substring(3, 1)
            End Select
            If Pid = Valor_p Then
              Try
                unacifra = unacifra + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
              Catch ex As Exception
              End Try
            End If
          Case 2
            'comprar con 1 digito en "Valor_p"
            Select Case Len(codigo_ingresado)
              Case 1
                'nada
              Case 2
                'nada
              Case 3
                Valor_p = Valor_p.ToString.Substring(1, 2)
              Case 4
                Valor_p = Valor_p.ToString.Substring(2, 2)
            End Select
            If Pid = Valor_p Then
              Try
                doscifras = doscifras + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
              Catch ex As Exception
              End Try
            End If
          Case 3
            'comprar con 1 digito en "Valor_p"
            Select Case Len(codigo_ingresado)
              Case 1
                'Puntos_P1 = Puntos_P1
              Case 2
                'nada
              Case 3
                'nada
              Case 4
                Valor_p = Valor_p.ToString.Substring(1, 3)
            End Select
            If Pid = Valor_p Then
              Try
                trescifras = trescifras + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
              Catch ex As Exception
              End Try
            End If
          Case 4
            'comparar con 4 digitos

            If Pid = Valor_p Then
              Try
                cuatrocifras = cuatrocifras + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
              Catch ex As Exception
              End Try
            End If
        End Select
      End If


      Dim j As Integer = 0
      While j < DS_Consultas.Tables("ZonasHabilitadas_importes").Rows.Count
        Try
          If Recorrido_codigo = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona1") Then
            DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe1") = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe1") + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
            Exit While
          End If
        Catch ex As Exception

        End Try

        Try
          If Recorrido_codigo = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona2") Then
            DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe2") = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe2") + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
            Exit While
          End If
        Catch ex As Exception

        End Try

        Try
          If Recorrido_codigo = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona3") Then
            DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe3") = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe3") + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
            Exit While
          End If
        Catch ex As Exception

        End Try

        Try
          If Recorrido_codigo = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona4") Then
            DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe4") = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe4") + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
            Exit While
          End If
        Catch ex As Exception

        End Try

        Try
          If Recorrido_codigo = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Zona5") Then
            DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe5") = DS_Consultas.Tables("ZonasHabilitadas_importes").Rows(j).Item("Importe5") + CDec(DS_XCargas.Tables(0).Rows(i).Item("Importe"))
            Exit While
          End If
        Catch ex As Exception

        End Try



        j = j + 1
      End While


      i = i + 1
    End While

    txtImporte1.Text = unacifra
    txtImporte2.Text = doscifras
    txtImporte3.Text = trescifras
    txtImporte4.Text = cuatrocifras



    Session("unacifra") = (Math.Round(unacifra, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
    Session("doscifras") = (Math.Round(doscifras, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
    Session("trescifras") = (Math.Round(trescifras, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
    Session("cuatrocifras") = (Math.Round(cuatrocifras, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
    Session("Zona") = TxtZona.Text
    Session("Codigo") = TxtCodigo.Text
    Session("TotalRecaudado") = (Math.Round(TOTAL_RECAUDADO, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
    Session("tabla_zonas") = DS_Consultas.Tables("ZonasHabilitadas_importes")
    Session("fecha_parametro") = HF_fecha.Value
    Session("dia") = HF_dia_id.Value
    Response.Redirect("~/Consultas/CodigoPremiado_consulta.aspx")


  End Sub



  Private Sub btnBuscar_ServerClick(sender As Object, e As EventArgs) Handles btnBuscar.ServerClick
    If TxtCodigo.Text <> "" Then
      Buscar_nuevo()
    Else
      'error, la busqueda no arrojó resultados.
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_msjerror_busqueda01", "$(document).ready(function () {$('#modal_msjerror_busqueda01').modal();});", True)
    End If



    ''IMPORTANTE SE CARGA DESDE CERO LA TABLA XCARGAS Y XCARGAS RECORRIDOS. FECHA: 22-08-04
    'DALiquidacion.XCargas_load()

    ''--------------VALIDACION INICIAL------------------------------------------------
    'BusquedaValidadInicial()
    ''--------------FIN--------------------------------------------------------------



    ''--------------AGREGO TODOS LOS COGIDOS EN UNA VARIABLE STRING-------------------
    'Dim DS_liqparcial As New DS_liqparcial
    'DS_liqparcial.Tables("Recorridos_seleccionados").Merge(Session("tabla_recorridos_seleccionados"))
    'Dim CadenaCodigos As String = ""
    'GenerarCadenaCodigos(DS_liqparcial.Tables("Recorridos_seleccionados"), CadenaCodigos)
    ''--------------FIN---------------------------------------------------------------

    'Dim DS_Consultas As New DS_Consultas


    'If TxtCodigo.Text <> "" Then
    '  CargaTabla1Cifra(DS_Consultas, CadenaCodigos)
    '  CargaTabla2Cifra(DS_Consultas, CadenaCodigos)
    '  CargaTabla3Cifra(DS_Consultas, CadenaCodigos)
    '  CargaTabla4Cifra(DS_Consultas, CadenaCodigos)

    '  'CALCULO LOS TOTALES
    '  CALCULAR_TOTALREGAUDADO(DS_Consultas)

    '  If (grvCifra1.Rows.Count = 0) And (grvCifra2.Rows.Count = 0) And (grvCifra3.Rows.Count = 0) And (grvCifra4.Rows.Count = 0) Then
    '    seccion1.Visible = False
    '    'error, la busqueda no arrojó resultados.
    '    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_msjerror_busqueda01", "$(document).ready(function () {$('#modal_msjerror_busqueda01').modal();});", True)
    '  Else
    '    seccion1.Visible = True
    '  End If

    'Else
    '  seccion1.Visible = False
    '  ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_msjerror_busqueda01", "$(document).ready(function () {$('#modal_msjerror_busqueda01').modal();});", True)
    'End If




    'TxtCodigo.Focus()


  End Sub

  Private Sub GenerarCadenaCodigos(ByRef TablaRecorridos As DataTable, ByRef CadenaCodigos As String)
    Dim i As Integer = 0
    While i < TablaRecorridos.Rows.Count
      If i = 0 Then
        CadenaCodigos = "'" + TablaRecorridos.Rows(i).Item("Codigo").ToString + "'"
      Else
        CadenaCodigos = CadenaCodigos + "," + "'" + TablaRecorridos.Rows(i).Item("Codigo").ToString + "'"
      End If
      i = i + 1
    End While
  End Sub

  Private Sub CargaTabla1Cifra(ByRef DS_Consultas As DataSet, ByRef CadenaCodigos As String)
    DS_Consultas.Tables("UNA_CIFRA").Rows.Clear()

    Dim dt_consulta As DataTable = DALConsultas.Cargas_Zona_PID(CadenaCodigos, TxtCodigo.Text, HF_fecha.Value)



    Dim i As Integer = 0
    While i < dt_consulta.Rows.Count
      If dt_consulta.Rows(i).Item(0).ToString.Length = 1 Then
        Dim PID As String = dt_consulta.Rows(i).Item(0).ToString
        Dim Codigo_Zona As String = dt_consulta.Rows(i).Item(2).ToString
        Dim Importe As Decimal = dt_consulta.Rows(i).Item(1)
        Dim existe As String = "no"
        Dim j As Integer = 0
        While j < DS_Consultas.Tables("UNA_CIFRA").Rows.Count

          If (PID = DS_Consultas.Tables("UNA_CIFRA").Rows(j).Item("PID")) And (Codigo_Zona = DS_Consultas.Tables("UNA_CIFRA").Rows(j).Item("ZONA")) Then
            DS_Consultas.Tables("UNA_CIFRA").Rows(j).Item("IMPORTE") = DS_Consultas.Tables("UNA_CIFRA").Rows(j).Item("IMPORTE") + Importe
            existe = "si"
            Exit While
          End If

          j = j + 1
        End While

        If existe = "no" Then
          Dim fila As DataRow = DS_Consultas.Tables("UNA_CIFRA").NewRow
          fila("PID") = PID
          fila("ZONA") = Codigo_Zona
          fila("IMPORTE") = Importe
          DS_Consultas.Tables("UNA_CIFRA").Rows.Add(fila)
        End If

      End If
      i = i + 1
    End While

    'ELIMINO LOS REGISTROS QUE NO SEAN MAYOR O IGUAL AL IMPORTE MINIMO PARA 1 DIGITO.
    i = 0
    Dim Importe_minimo As Decimal = CDec(txtImporte1.Text)
    While i < DS_Consultas.Tables("UNA_CIFRA").Rows.Count
      If DS_Consultas.Tables("UNA_CIFRA").Rows(i).Item("IMPORTE") < Importe_minimo Then
        'elimino
        DS_Consultas.Tables("UNA_CIFRA").Rows.RemoveAt(i)
        i = 0
      Else
        i = i + 1
      End If

    End While
    grvCifra1.DataSource = DS_Consultas.Tables("UNA_CIFRA")
    grvCifra1.DataBind()
  End Sub


  Private Sub CargaTabla2Cifra(ByRef DS_Consultas As DataSet, ByRef CadenaCodigos As String)
    DS_Consultas.Tables("DOS_CIFRAS").Rows.Clear()
    Dim dt_consulta As DataTable = DALConsultas.Cargas_Zona_PID(CadenaCodigos, TxtCodigo.Text, HF_fecha.Value)
    Dim i As Integer = 0
    While i < dt_consulta.Rows.Count
      If dt_consulta.Rows(i).Item(0).ToString.Length = 2 Then
        Dim PID As String = dt_consulta.Rows(i).Item(0).ToString
        Dim Codigo_Zona As String = dt_consulta.Rows(i).Item(2).ToString
        Dim Importe As Decimal = dt_consulta.Rows(i).Item(1)
        Dim existe As String = "no"
        Dim j As Integer = 0
        While j < DS_Consultas.Tables("DOS_CIFRAS").Rows.Count
          If (PID = DS_Consultas.Tables("DOS_CIFRAS").Rows(j).Item("PID")) And (Codigo_Zona = DS_Consultas.Tables("DOS_CIFRAS").Rows(j).Item("ZONA")) Then
            DS_Consultas.Tables("DOS_CIFRAS").Rows(j).Item("IMPORTE") = DS_Consultas.Tables("DOS_CIFRAS").Rows(j).Item("IMPORTE") + Importe
            existe = "si"
            Exit While
          End If
          j = j + 1
        End While
        If existe = "no" Then
          Dim fila As DataRow = DS_Consultas.Tables("DOS_CIFRAS").NewRow
          fila("PID") = PID
          fila("ZONA") = Codigo_Zona
          fila("IMPORTE") = Importe
          DS_Consultas.Tables("DOS_CIFRAS").Rows.Add(fila)
        End If
      End If
      i = i + 1
    End While
    'ELIMINO LOS REGISTROS QUE NO SEAN MAYOR O IGUAL AL IMPORTE MINIMO PARA 2 DIGITO.
    i = 0
    Dim Importe_minimo As Decimal = CDec(txtImporte2.Text)
    While i < DS_Consultas.Tables("DOS_CIFRAS").Rows.Count
      If DS_Consultas.Tables("DOS_CIFRAS").Rows(i).Item("IMPORTE") < Importe_minimo Then
        'elimino
        DS_Consultas.Tables("DOS_CIFRAS").Rows.RemoveAt(i)
        i = 0
      Else
        i = i + 1
      End If
    End While
    grvCifra2.DataSource = DS_Consultas.Tables("DOS_CIFRAS")
    grvCifra2.DataBind()
  End Sub

  Private Sub CargaTabla3Cifra(ByRef DS_Consultas As DataSet, ByRef CadenaCodigos As String)
    DS_Consultas.Tables("TRES_CIFRAS").Rows.Clear()
    Dim dt_consulta As DataTable = DALConsultas.Cargas_Zona_PID(CadenaCodigos, TxtCodigo.Text, HF_fecha.Value)
    Dim i As Integer = 0
    While i < dt_consulta.Rows.Count
      If dt_consulta.Rows(i).Item(0).ToString.Length = 3 Then
        Dim PID As String = dt_consulta.Rows(i).Item(0).ToString
        Dim Codigo_Zona As String = dt_consulta.Rows(i).Item(2).ToString
        Dim Importe As Decimal = dt_consulta.Rows(i).Item(1)
        Dim existe As String = "no"
        Dim j As Integer = 0
        While j < DS_Consultas.Tables("TRES_CIFRAS").Rows.Count

          If (PID = DS_Consultas.Tables("TRES_CIFRAS").Rows(j).Item("PID")) And (Codigo_Zona = DS_Consultas.Tables("TRES_CIFRAS").Rows(j).Item("ZONA")) Then
            DS_Consultas.Tables("TRES_CIFRAS").Rows(j).Item("IMPORTE") = DS_Consultas.Tables("TRES_CIFRAS").Rows(j).Item("IMPORTE") + Importe
            existe = "si"
            Exit While
          End If

          j = j + 1
        End While
        If existe = "no" Then
          Dim fila As DataRow = DS_Consultas.Tables("TRES_CIFRAS").NewRow
          fila("PID") = PID
          fila("ZONA") = Codigo_Zona
          fila("IMPORTE") = Importe
          DS_Consultas.Tables("TRES_CIFRAS").Rows.Add(fila)
        End If
      End If
      i = i + 1
    End While
    'ELIMINO LOS REGISTROS QUE NO SEAN MAYOR O IGUAL AL IMPORTE MINIMO PARA 3 DIGITO.
    i = 0
    Dim Importe_minimo As Decimal = CDec(txtImporte3.Text)
    While i < DS_Consultas.Tables("TRES_CIFRAS").Rows.Count
      If DS_Consultas.Tables("TRES_CIFRAS").Rows(i).Item("IMPORTE") < Importe_minimo Then
        'elimino
        DS_Consultas.Tables("TRES_CIFRAS").Rows.RemoveAt(i)
        i = 0
      Else
        i = i + 1
      End If
    End While
    grvCifra3.DataSource = DS_Consultas.Tables("TRES_CIFRAS")
    grvCifra3.DataBind()
  End Sub

  Private Sub CargaTabla4Cifra(ByRef DS_Consultas As DataSet, ByRef CadenaCodigos As String)
    DS_Consultas.Tables("CUATRO_CIFRAS").Rows.Clear()
    Dim dt_consulta As DataTable = DALConsultas.Cargas_Zona_PID(CadenaCodigos, TxtCodigo.Text, HF_fecha.Value)
    Dim i As Integer = 0
    While i < dt_consulta.Rows.Count
      If dt_consulta.Rows(i).Item(0).ToString.Length = 4 Then
        Dim PID As String = dt_consulta.Rows(i).Item(0).ToString
        Dim Codigo_Zona As String = dt_consulta.Rows(i).Item(2).ToString
        Dim Importe As Decimal = dt_consulta.Rows(i).Item(1)
        Dim existe As String = "no"
        Dim j As Integer = 0
        While j < DS_Consultas.Tables("CUATRO_CIFRAS").Rows.Count

          If (PID = DS_Consultas.Tables("CUATRO_CIFRAS").Rows(j).Item("PID")) And (Codigo_Zona = DS_Consultas.Tables("CUATRO_CIFRAS").Rows(j).Item("ZONA")) Then
            DS_Consultas.Tables("CUATRO_CIFRAS").Rows(j).Item("IMPORTE") = DS_Consultas.Tables("CUATRO_CIFRAS").Rows(j).Item("IMPORTE") + Importe
            existe = "si"
            Exit While
          End If

          j = j + 1
        End While

        If existe = "no" Then
          Dim fila As DataRow = DS_Consultas.Tables("CUATRO_CIFRAS").NewRow
          fila("PID") = PID
          fila("ZONA") = Codigo_Zona
          fila("IMPORTE") = Importe
          DS_Consultas.Tables("CUATRO_CIFRAS").Rows.Add(fila)
        End If

      End If
      i = i + 1
    End While

    'ELIMINO LOS REGISTROS QUE NO SEAN MAYOR O IGUAL AL IMPORTE MINIMO PARA 4 DIGITO.
    i = 0
    Dim Importe_minimo As Decimal = CDec(txtImporte4.Text)
    While i < DS_Consultas.Tables("CUATRO_CIFRAS").Rows.Count
      If DS_Consultas.Tables("CUATRO_CIFRAS").Rows(i).Item("IMPORTE") < Importe_minimo Then
        'elimino
        DS_Consultas.Tables("CUATRO_CIFRAS").Rows.RemoveAt(i)
        i = 0
      Else
        i = i + 1
      End If

    End While
    grvCifra4.DataSource = DS_Consultas.Tables("CUATRO_CIFRAS")
    grvCifra4.DataBind()
  End Sub

  Private Sub CALCULAR_TOTALREGAUDADO(ByRef DS_Consultas As DataSet)
    Dim TOTAL As Decimal = 0

    Dim i As Integer = 0
    While i < DS_Consultas.Tables("UNA_CIFRA").Rows.Count
      Try
        TOTAL = TOTAL + CDec(DS_Consultas.Tables("UNA_CIFRA").Rows(i).Item("IMPORTE"))
      Catch ex As Exception

      End Try
      i = i + 1
    End While
    i = 0
    While i < DS_Consultas.Tables("DOS_CIFRAS").Rows.Count
      Try
        TOTAL = TOTAL + CDec(DS_Consultas.Tables("DOS_CIFRAS").Rows(i).Item("IMPORTE"))
      Catch ex As Exception

      End Try
      i = i + 1
    End While
    i = 0
    While i < DS_Consultas.Tables("TRES_CIFRAS").Rows.Count
      Try
        TOTAL = TOTAL + CDec(DS_Consultas.Tables("TRES_CIFRAS").Rows(i).Item("IMPORTE"))
      Catch ex As Exception

      End Try
      i = i + 1
    End While
    i = 0
    While i < DS_Consultas.Tables("CUATRO_CIFRAS").Rows.Count
      Try
        TOTAL = TOTAL + CDec(DS_Consultas.Tables("CUATRO_CIFRAS").Rows(i).Item("IMPORTE"))
      Catch ex As Exception

      End Try
      i = i + 1
    End While
    Label_TotalRecaudado.Text = "TOTAL RECAUDADO: " + TOTAL.ToString


  End Sub

End Class
