Public Class CodigoMasPremiado
  Inherits System.Web.UI.Page

#Region "Declaraciones"
  Dim Darecorridos_zonas As New Capa_Datos.WC_recorridos_zonas
  Dim Daparametro As New Capa_Datos.WC_parametro
  Dim DALConsultas As New Capa_Datos.WB_Consultas
  Dim DALiquidacion As New Capa_Datos.WC_Liquidacion
  Dim Lista1Cifras As New List(Of Capa_Datos.CodigoMasCargadoDTO)
  Dim Lista2Cifras As New List(Of Capa_Datos.CodigoMasCargadoDTO)
  Dim Lista3Cifras As New List(Of Capa_Datos.CodigoMasCargadoDTO)
  Dim Lista4Cifras As New List(Of Capa_Datos.CodigoMasCargadoDTO)
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos


#End Region
#Region "Eventos"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      Permisos()

      txtClienteDesde.Focus()
      'recuperar fecha de tabla parametro.

      HF_fecha.Value = Session("fecha_parametro")
      HF_dia_id.Value = Session("dia")


      Dim ds_fecha As DataSet = Daparametro.Parametro_obtener_dia
      If ds_fecha.Tables(0).Rows.Count <> 0 Then
        Dim FECHA As Date = CDate(ds_fecha.Tables(0).Rows(0).Item("Fecha"))
        'txt_fecha.Text = FECHA.ToString("yyyy-MM-dd")

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
            If (Menu = "5" And Opcion = "") Or (Menu = "5" And Opcion = "1") Then
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
    Response.Redirect("~/Consultas/CodigoMasPremiadoRecorridos.aspx")
  End Sub


  Private Sub BusquedaValidadInicial()
    Try
      txtClienteDesde.Text = CInt(txtClienteDesde.Text)
    Catch ex As Exception
      txtClienteDesde.Text = 0
    End Try
    Try
      txtClienteHasta.Text = CInt(txtClienteHasta.Text)
    Catch ex As Exception
      txtClienteHasta.Text = 0
    End Try


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


  Private Sub CargaTabla1Cifra(ByRef DS_Consultas As DataSet, ByRef CadenaCodigos As String)
    DS_Consultas.Tables("UNA_CIFRA").Rows.Clear()

    Dim dt_consulta As DataTable = DALConsultas.CargasClientesDesdeHasta(txtClienteDesde.Text, txtClienteHasta.Text, CadenaCodigos, HF_fecha.Value)



    Dim i As Integer = 0
    While i < dt_consulta.Rows.Count
      If dt_consulta.Rows(i).Item(0).ToString.Length = 1 Then
        Dim PID As String = dt_consulta.Rows(i).Item(0).ToString
        Dim Codigo_Zona As String = dt_consulta.Rows(i).Item(2).ToString
        ''//////////////////////////////////////////////////////////////////////////////////////////////////////////

        'Dim Referencia As String = ""
        'Try
        '  Dim ds_recorrido_zonas As DataSet = Darecorridos_zonas.recorridos_zonas_buscar_codigo(Codigo_Zona, CInt(HF_dia_id.Value))
        '  Referencia = ds_recorrido_zonas.Tables(0).Rows(0).Item("Referencia")
        'Catch ex As Exception

        'End Try
        ''//////////////////////////////////////////////////////////////////////////////////////////////////////////
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


    Dim datatable_unacifra As DataTable = DS_Consultas.Tables("UNA_CIFRA").Clone 'copia la estructura pero no los registros

    Dim importe_minimo As Decimal
    Try
      importe_minimo = CDec(txtImporte1.Text.Replace(".", ","))
    Catch ex As Exception
      importe_minimo = 0
    End Try

    'se debe mostrar en pantalla solo las sumatorias de los PID por Zonas que superen el importe
    'que se ingreso en la pantalla de seleccion de "IMPORTE MINIMO PARA......, desde el mayor importe al menor.

    i = 0

    While i < DS_Consultas.Tables("UNA_CIFRA").Rows.Count
      Dim resto = CDec(DS_Consultas.Tables("UNA_CIFRA").Rows(i).Item("IMPORTE")) - importe_minimo
      If resto > 0 Then
        Dim fila As DataRow = datatable_unacifra.NewRow
        fila("PID") = DS_Consultas.Tables("UNA_CIFRA").Rows(i).Item("PID")
        fila("ZONA") = DS_Consultas.Tables("UNA_CIFRA").Rows(i).Item("ZONA")
        fila("IMPORTE") = resto
        datatable_unacifra.Rows.Add(fila)
      End If
      i = i + 1
    End While

    If datatable_unacifra.Rows.Count <> 0 Then
      'ordeno por importe de mayor a menor.
      Dim dt_filtado As DataTable = datatable_unacifra.Clone
      Dim filtro As String = "IMPORTE > 0"
      Dim rows() As DataRow = datatable_unacifra.Select(filtro, "IMPORTE DESC")
      For Each row As DataRow In rows
        ' Indicamos que el registro ha sido añadido
        'row.SetAdded()
        dt_filtado.ImportRow(row)
      Next
      grvCifra1.DataSource = dt_filtado
      grvCifra1.DataBind()

      '////////////////////////////armado de la grilla con todos los importes//////////////////////////
      If dt_filtado.Rows.Count <> 0 Then
        i = 0
        While i < dt_filtado.Rows.Count
          Dim filaimportes As DataRow = DS_Consultas.Tables("CodigoMasPremiado_importes").NewRow
          filaimportes("pid1") = dt_filtado.Rows(i).Item("PID")
          filaimportes("zona1") = dt_filtado.Rows(i).Item("ZONA")
          filaimportes("importe1") = dt_filtado.Rows(i).Item("IMPORTE")
          DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Add(filaimportes)
          i = i + 1
        End While
      End If
      '////////////////////////////////////////////////////////////////////////////////////////////////



    End If


    'ELIMINO LOS REGISTROS QUE NO SEAN MAYOR O IGUAL AL IMPORTE MINIMO PARA 1 DIGITO.


    'Dim importe_minimo As Decimal
    'Try
    '  importe_minimo = CDec(txtImporte1.Text.Replace(".", ","))
    'Catch ex As Exception
    '  importe_minimo = 0
    'End Try
    'While i < DS_Consultas.Tables("UNA_CIFRA").Rows.Count
    '  If DS_Consultas.Tables("UNA_CIFRA").Rows(i).Item("IMPORTE") < importe_minimo Then
    '    'elimino
    '    DS_Consultas.Tables("UNA_CIFRA").Rows.RemoveAt(i)
    '    i = 0
    '  Else
    '    i = i + 1
    '  End If

    'End While



  End Sub

  Private Sub CargaTabla2Cifra(ByRef DS_Consultas As DataSet, ByRef CadenaCodigos As String)
    DS_Consultas.Tables("DOS_CIFRAS").Rows.Clear()
    Dim dt_consulta As DataTable = DALConsultas.CargasClientesDesdeHasta(txtClienteDesde.Text, txtClienteHasta.Text, CadenaCodigos, HF_fecha.Value)
    Dim i As Integer = 0
    While i < dt_consulta.Rows.Count
      If dt_consulta.Rows(i).Item(0).ToString.Length = 2 Then
        Dim PID As String = dt_consulta.Rows(i).Item(0).ToString
        Dim Codigo_Zona As String = dt_consulta.Rows(i).Item(2).ToString
        ''//////////////////////////////////////////////////////////////////////////////////////////////////////////

        'Dim Referencia As String = ""
        'Try
        '  Dim ds_recorrido_zonas As DataSet = Darecorridos_zonas.recorridos_zonas_buscar_codigo(Codigo_Zona, CInt(HF_dia_id.Value))
        '  Referencia = ds_recorrido_zonas.Tables(0).Rows(0).Item("Referencia")
        'Catch ex As Exception

        'End Try
        ''//////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    'i = 0
    'Dim importe_minimo As Decimal
    'Try
    '  importe_minimo = CDec(txtImporte2.Text.Replace(".", ","))
    'Catch ex As Exception
    '  importe_minimo = 0
    'End Try
    'While i < DS_Consultas.Tables("DOS_CIFRAS").Rows.Count
    '  If DS_Consultas.Tables("DOS_CIFRAS").Rows(i).Item("IMPORTE") < importe_minimo Then
    '    'elimino
    '    DS_Consultas.Tables("DOS_CIFRAS").Rows.RemoveAt(i)
    '    i = 0
    '  Else
    '    i = i + 1
    '  End If
    'End While

    Dim datatable_doscifras As DataTable = DS_Consultas.Tables("DOS_CIFRAS").Clone 'copia la estructura pero no los registros

    Dim importe_minimo As Decimal
    Try
      importe_minimo = CDec(txtImporte2.Text.Replace(".", ","))
    Catch ex As Exception
      importe_minimo = 0
    End Try

    i = 0

    While i < DS_Consultas.Tables("DOS_CIFRAS").Rows.Count
      Dim resto = CDec(DS_Consultas.Tables("DOS_CIFRAS").Rows(i).Item("IMPORTE")) - importe_minimo
      If resto > 0 Then
        Dim fila As DataRow = datatable_doscifras.NewRow
        fila("PID") = DS_Consultas.Tables("DOS_CIFRAS").Rows(i).Item("PID")
        fila("ZONA") = DS_Consultas.Tables("DOS_CIFRAS").Rows(i).Item("ZONA")
        fila("IMPORTE") = resto
        datatable_doscifras.Rows.Add(fila)
      End If
      i = i + 1
    End While

    If datatable_doscifras.Rows.Count <> 0 Then
      'ordeno por importe de mayor a menor.
      Dim dt_filtado As DataTable = datatable_doscifras.Clone
      Dim filtro As String = "IMPORTE > 0"
      Dim rows() As DataRow = datatable_doscifras.Select(filtro, "IMPORTE DESC")
      For Each row As DataRow In rows
        ' Indicamos que el registro ha sido añadido
        'row.SetAdded()
        dt_filtado.ImportRow(row)
      Next
      grvCifra2.DataSource = dt_filtado
      grvCifra2.DataBind()

      '////////////////////////////armado de la grilla con todos los importes//////////////////////////
      Dim nueva_fila = "si" 'se pone en no si carga en un registro existente.
      If dt_filtado.Rows.Count <> 0 Then
        i = 0
        While i < dt_filtado.Rows.Count
          Dim J = 0
          Dim modif_fila = "no"
          If nueva_fila = "si" Then
            While J < DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Count
              Dim zona2 As String = ""
              Try
                zona2 = CStr(DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("zona2"))
              Catch ex As Exception
              End Try
              If zona2 = "" Then
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("pid2") = dt_filtado.Rows(i).Item("PID")
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("zona2") = dt_filtado.Rows(i).Item("ZONA")
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("importe2") = dt_filtado.Rows(i).Item("IMPORTE")
                modif_fila = "si"
                Exit While
              End If
              J = J + 1
            End While
            If modif_fila = "no" Then
              Dim filaimportes As DataRow = DS_Consultas.Tables("CodigoMasPremiado_importes").NewRow
              filaimportes("pid2") = dt_filtado.Rows(i).Item("PID")
              filaimportes("zona2") = dt_filtado.Rows(i).Item("ZONA")
              filaimportes("importe2") = dt_filtado.Rows(i).Item("IMPORTE")
              DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Add(filaimportes)
              nueva_fila = "no"
            End If
          Else
            'solo ingreso en fila nueva
            Dim filaimportes As DataRow = DS_Consultas.Tables("CodigoMasPremiado_importes").NewRow
            filaimportes("pid2") = dt_filtado.Rows(i).Item("PID")
            filaimportes("zona2") = dt_filtado.Rows(i).Item("ZONA")
            filaimportes("importe2") = dt_filtado.Rows(i).Item("IMPORTE")
            DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Add(filaimportes)

          End If
          i = i + 1
        End While
      End If
      '////////////////////////////////////////////////////////////////////////////////////////////////


    End If

    'grvCifra2.DataSource = DS_Consultas.Tables("DOS_CIFRAS")
    'grvCifra2.DataBind()
  End Sub

  Private Sub CargaTabla3Cifra(ByRef DS_Consultas As DataSet, ByRef CadenaCodigos As String)
    DS_Consultas.Tables("TRES_CIFRAS").Rows.Clear()
    Dim dt_consulta As DataTable = DALConsultas.CargasClientesDesdeHasta(txtClienteDesde.Text, txtClienteHasta.Text, CadenaCodigos, HF_fecha.Value)
    Dim i As Integer = 0
    While i < dt_consulta.Rows.Count
      If dt_consulta.Rows(i).Item(0).ToString.Length = 3 Then
        Dim PID As String = dt_consulta.Rows(i).Item(0).ToString
        Dim Codigo_Zona As String = dt_consulta.Rows(i).Item(2).ToString
        ''//////////////////////////////////////////////////////////////////////////////////////////////////////////

        'Dim Referencia As String = ""
        'Try
        '  Dim ds_recorrido_zonas As DataSet = Darecorridos_zonas.recorridos_zonas_buscar_codigo(Codigo_Zona, CInt(HF_dia_id.Value))
        '  Referencia = ds_recorrido_zonas.Tables(0).Rows(0).Item("Referencia")
        'Catch ex As Exception

        'End Try
        ''//////////////////////////////////////////////////////////////////////////////////////////////////////////


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
    'Dim importe_minimo As Decimal
    'Try
    '  importe_minimo = CDec(txtImporte3.Text.Replace(".", ","))
    'Catch ex As Exception
    '  importe_minimo = 0
    'End Try
    'While i < DS_Consultas.Tables("TRES_CIFRAS").Rows.Count
    '  If DS_Consultas.Tables("TRES_CIFRAS").Rows(i).Item("IMPORTE") < importe_minimo Then
    '    'elimino
    '    DS_Consultas.Tables("TRES_CIFRAS").Rows.RemoveAt(i)
    '    i = 0
    '  Else
    '    i = i + 1
    '  End If
    'End While

    Dim datatable_trescifras As DataTable = DS_Consultas.Tables("TRES_CIFRAS").Clone 'copia la estructura pero no los registros

    Dim importe_minimo As Decimal
    Try
      importe_minimo = CDec(txtImporte3.Text.Replace(".", ","))
    Catch ex As Exception
      importe_minimo = 0
    End Try

    i = 0

    While i < DS_Consultas.Tables("TRES_CIFRAS").Rows.Count
      Dim resto = CDec(DS_Consultas.Tables("TRES_CIFRAS").Rows(i).Item("IMPORTE")) - importe_minimo
      If resto > 0 Then
        Dim fila As DataRow = datatable_trescifras.NewRow
        fila("PID") = DS_Consultas.Tables("TRES_CIFRAS").Rows(i).Item("PID")
        fila("ZONA") = DS_Consultas.Tables("TRES_CIFRAS").Rows(i).Item("ZONA")
        fila("IMPORTE") = resto
        datatable_trescifras.Rows.Add(fila)
      End If
      i = i + 1
    End While

    If datatable_trescifras.Rows.Count <> 0 Then
      'ordeno por importe de mayor a menor.
      Dim dt_filtado As DataTable = datatable_trescifras.Clone
      Dim filtro As String = "IMPORTE > 0"
      Dim rows() As DataRow = datatable_trescifras.Select(filtro, "IMPORTE DESC")
      For Each row As DataRow In rows
        ' Indicamos que el registro ha sido añadido
        'row.SetAdded()
        dt_filtado.ImportRow(row)
      Next

      grvCifra3.DataSource = dt_filtado
      grvCifra3.DataBind()

      '////////////////////////////armado de la grilla con todos los importes//////////////////////////
      Dim nueva_fila = "si" 'se pone en no si carga en un registro existente.
      If dt_filtado.Rows.Count <> 0 Then
        i = 0
        While i < dt_filtado.Rows.Count
          Dim J = 0
          Dim modif_fila = "no"
          If nueva_fila = "si" Then
            While J < DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Count
              Dim zona3 As String = ""
              Try
                zona3 = CStr(DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("zona3"))
              Catch ex As Exception
              End Try
              If zona3 = "" Then
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("pid3") = dt_filtado.Rows(i).Item("PID")
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("zona3") = dt_filtado.Rows(i).Item("ZONA")
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("importe3") = dt_filtado.Rows(i).Item("IMPORTE")
                modif_fila = "si"
                Exit While
              End If
              J = J + 1
            End While
            If modif_fila = "no" Then
              Dim filaimportes As DataRow = DS_Consultas.Tables("CodigoMasPremiado_importes").NewRow
              filaimportes("pid3") = dt_filtado.Rows(i).Item("PID")
              filaimportes("zona3") = dt_filtado.Rows(i).Item("ZONA")
              filaimportes("importe3") = dt_filtado.Rows(i).Item("IMPORTE")
              DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Add(filaimportes)
              nueva_fila = "no"
            End If
          Else
            'solo ingreso en fila nueva
            Dim filaimportes As DataRow = DS_Consultas.Tables("CodigoMasPremiado_importes").NewRow
            filaimportes("pid3") = dt_filtado.Rows(i).Item("PID")
            filaimportes("zona3") = dt_filtado.Rows(i).Item("ZONA")
            filaimportes("importe3") = dt_filtado.Rows(i).Item("IMPORTE")
            DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Add(filaimportes)

          End If
          i = i + 1
        End While
      End If
      '////////////////////////////////////////////////////////////////////////////////////////////////


    End If



  End Sub

  Private Sub CargaTabla4Cifra(ByRef DS_Consultas As DataSet, ByRef CadenaCodigos As String)
    DS_Consultas.Tables("CUATRO_CIFRAS").Rows.Clear()
    Dim dt_consulta As DataTable = DALConsultas.CargasClientesDesdeHasta(txtClienteDesde.Text, txtClienteHasta.Text, CadenaCodigos, HF_fecha.Value)
    Dim i As Integer = 0
    While i < dt_consulta.Rows.Count
      If dt_consulta.Rows(i).Item(0).ToString.Length = 4 Then
        Dim PID As String = dt_consulta.Rows(i).Item(0).ToString
        Dim Codigo_Zona As String = dt_consulta.Rows(i).Item(2).ToString
        ''//////////////////////////////////////////////////////////////////////////////////////////////////////////

        'Dim Referencia As String = ""
        'Try
        '  Dim ds_recorrido_zonas As DataSet = Darecorridos_zonas.recorridos_zonas_buscar_codigo(Codigo_Zona, CInt(HF_dia_id.Value))
        '  Referencia = ds_recorrido_zonas.Tables(0).Rows(0).Item("Referencia")
        'Catch ex As Exception

        'End Try
        ''//////////////////////////////////////////////////////////////////////////////////////////////////////////

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
    'Dim importe_minimo As Decimal
    'Try
    '  importe_minimo = CDec(txtImporte4.Text.Replace(".", ","))
    'Catch ex As Exception
    '  importe_minimo = 0
    'End Try
    'While i < DS_Consultas.Tables("CUATRO_CIFRAS").Rows.Count
    '  If DS_Consultas.Tables("CUATRO_CIFRAS").Rows(i).Item("IMPORTE") < importe_minimo Then
    '    'elimino
    '    DS_Consultas.Tables("CUATRO_CIFRAS").Rows.RemoveAt(i)
    '    i = 0
    '  Else
    '    i = i + 1
    '  End If

    'End While

    Dim datatable_cuatrocifras As DataTable = DS_Consultas.Tables("CUATRO_CIFRAS").Clone 'copia la estructura pero no los registros

    Dim importe_minimo As Decimal
    Try
      importe_minimo = CDec(txtImporte4.Text.Replace(".", ","))
    Catch ex As Exception
      importe_minimo = 0
    End Try

    i = 0

    While i < DS_Consultas.Tables("CUATRO_CIFRAS").Rows.Count
      Dim resto = CDec(DS_Consultas.Tables("CUATRO_CIFRAS").Rows(i).Item("IMPORTE")) - importe_minimo
      If resto > 0 Then
        Dim fila As DataRow = datatable_cuatrocifras.NewRow
        fila("PID") = DS_Consultas.Tables("CUATRO_CIFRAS").Rows(i).Item("PID")
        fila("ZONA") = DS_Consultas.Tables("CUATRO_CIFRAS").Rows(i).Item("ZONA")
        fila("IMPORTE") = resto
        datatable_cuatrocifras.Rows.Add(fila)
      End If
      i = i + 1
    End While

    If datatable_cuatrocifras.Rows.Count <> 0 Then
      'ordeno por importe de mayor a menor.
      Dim dt_filtado As DataTable = datatable_cuatrocifras.Clone
      Dim filtro As String = "IMPORTE > 0"
      Dim rows() As DataRow = datatable_cuatrocifras.Select(filtro, "IMPORTE DESC")
      For Each row As DataRow In rows
        ' Indicamos que el registro ha sido añadido
        'row.SetAdded()
        dt_filtado.ImportRow(row)
      Next
      grvCifra4.DataSource = dt_filtado
      grvCifra4.DataBind()

      '////////////////////////////armado de la grilla con todos los importes//////////////////////////
      Dim nueva_fila = "si" 'se pone en no si carga en un registro existente.
      If dt_filtado.Rows.Count <> 0 Then
        i = 0
        While i < dt_filtado.Rows.Count
          Dim J = 0
          Dim modif_fila = "no"
          If nueva_fila = "si" Then
            While J < DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Count
              Dim zona4 As String = ""
              Try
                zona4 = CStr(DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("zona4"))
              Catch ex As Exception
              End Try
              If zona4 = "" Then
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("pid4") = dt_filtado.Rows(i).Item("PID")
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("zona4") = dt_filtado.Rows(i).Item("ZONA")
                DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(J).Item("importe4") = dt_filtado.Rows(i).Item("IMPORTE")
                modif_fila = "si"
                Exit While
              End If
              J = J + 1
            End While
            If modif_fila = "no" Then
              Dim filaimportes As DataRow = DS_Consultas.Tables("CodigoMasPremiado_importes").NewRow
              filaimportes("pid4") = dt_filtado.Rows(i).Item("PID")
              filaimportes("zona4") = dt_filtado.Rows(i).Item("ZONA")
              filaimportes("importe4") = dt_filtado.Rows(i).Item("IMPORTE")
              DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Add(filaimportes)
              nueva_fila = "no"
            End If
          Else
            'solo ingreso en fila nueva
            Dim filaimportes As DataRow = DS_Consultas.Tables("CodigoMasPremiado_importes").NewRow
            filaimportes("pid4") = dt_filtado.Rows(i).Item("PID")
            filaimportes("zona4") = dt_filtado.Rows(i).Item("ZONA")
            filaimportes("importe4") = dt_filtado.Rows(i).Item("IMPORTE")
            DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Add(filaimportes)

          End If
          i = i + 1
        End While
      End If
      '////////////////////////////////////////////////////////////////////////////////////////////////


    End If



  End Sub

  Private Sub btnBuscar_ServerClick(sender As Object, e As EventArgs) Handles btnBuscar.ServerClick



    'IMPORTANTE SE CARGA DESDE CERO LA TABLA XCARGAS Y XCARGAS RECORRIDOS. FECHA: 22-08-04
    DALiquidacion.XCargas_load()



    '--------------VALIDACION INICIAL------------------------------------------------
    BusquedaValidadInicial()
    '--------------FIN--------------------------------------------------------------

    '--------------AGREGO TODOS LOS COGIDOS EN UNA VARIABLE STRING-------------------
    Dim DS_liqparcial As New DS_liqparcial
    DS_liqparcial.Tables("Recorridos_seleccionados").Merge(Session("tabla_recorridos_seleccionados"))
    Dim CadenaCodigos As String = ""
    GenerarCadenaCodigos(DS_liqparcial.Tables("Recorridos_seleccionados"), CadenaCodigos)
    '--------------FIN---------------------------------------------------------------


    Dim DS_Consultas As New DS_Consultas


    CargaTabla1Cifra(DS_Consultas, CadenaCodigos)
    CargaTabla2Cifra(DS_Consultas, CadenaCodigos)
    CargaTabla3Cifra(DS_Consultas, CadenaCodigos)
    CargaTabla4Cifra(DS_Consultas, CadenaCodigos)

    'LlenarTabla1Cifra(CadenaCodigos)
    'LlenarTabla2Cifra(CadenaCodigos)
    'LlenarTabla3Cifra(CadenaCodigos)
    'LlenarTabla4Cifra(CadenaCodigos)

    'If (grvCifra1.Rows.Count = 0) And (grvCifra2.Rows.Count = 0) And (grvCifra3.Rows.Count = 0) And (grvCifra4.Rows.Count = 0) Then
    '  seccion1.Visible = False

    'Else
    '  seccion1.Visible = True
    'End If
    'txtClienteDesde.Focus()







    If DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Count <> 0 Then
      'VOY A CAMBIAR LOS CODIGOS DE ZONAS POR REFERENCIAS.
      Dim ds_zonas As DataSet = Darecorridos_zonas.recorridos_zonas_consultar_dia(HF_dia_id.Value)
      Dim i As Integer = 0
      While i < DS_Consultas.Tables("CodigoMasPremiado_importes").Rows.Count
        Try
          Dim codigoZ1 As String = CStr(DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(i).Item("zona1"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ1, i, "zona1")
        Catch ex As Exception
        End Try
        Try
          Dim codigoZ2 As String = CStr(DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(i).Item("zona2"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ2, i, "zona2")
        Catch ex As Exception
        End Try
        Try
          Dim codigoZ3 As String = CStr(DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(i).Item("zona3"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ3, i, "zona3")
        Catch ex As Exception
        End Try
        Try
          Dim codigoZ4 As String = CStr(DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(i).Item("zona4"))
          buscar_zona(DS_Consultas, ds_zonas, codigoZ4, i, "zona4")
        Catch ex As Exception
        End Try
        i = i + 1
      End While


      Session("fecha_parametro") = HF_fecha.Value
      Session("dia") = HF_dia_id.Value
      Session("tabla_consulta") = DS_Consultas.Tables("CodigoMasPremiado_importes")
      Session("op_ingreso") = "si"
      Session("cliente_desde") = txtClienteDesde.Text
      Session("cliente_hasta") = txtClienteHasta.Text
      Session("importe1") = txtImporte1.Text
      Session("importe2") = txtImporte2.Text
      Session("importe3") = txtImporte3.Text
      Session("importe4") = txtImporte4.Text
      Response.Redirect("~/Consultas/CodigosMasCargados_consulta.aspx")
    Else
      'error, la busqueda no arrojó resultados.
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_msjerror_busqueda01", "$(document).ready(function () {$('#modal_msjerror_busqueda01').modal();});", True)
    End If



  End Sub

  Private Sub buscar_zona(ByRef DS_Consultas As DataSet, ByVal ds_zonas As DataSet, ByVal codigoZ As String, ByVal indice As Integer, ByVal item_zona As String)
    Dim i As Integer = 0
    While i < ds_zonas.Tables(0).Rows.Count
      If codigoZ = ds_zonas.Tables(0).Rows(i).Item("Codigo") Then
        DS_Consultas.Tables("CodigoMasPremiado_importes").Rows(indice).Item(item_zona) = ds_zonas.Tables(0).Rows(i).Item("Referencia")
        Exit While
      End If
      i = i + 1
    End While
  End Sub

  Private Sub btn_ok_error_busqueda01_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error_busqueda01.ServerClick
    txtClienteDesde.Focus()
  End Sub

  Private Sub btn_close_error_busqueda01_ServerClick(sender As Object, e As EventArgs) Handles btn_close_error_busqueda01.ServerClick
    txtClienteDesde.Focus()
  End Sub

  Private Sub txtClienteDesde_Init(sender As Object, e As EventArgs) Handles txtClienteDesde.Init
    txtClienteDesde.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub
  Private Sub txtClienteHasta_Init(sender As Object, e As EventArgs) Handles txtClienteHasta.Init
    txtClienteHasta.Attributes.Add("onfocus", "seleccionarTexto(this);")
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
#End Region


#Region "Metodos"

  Private Function Buscar(ByVal ClienteDesde As Integer, ByVal ClienteHasta As Integer, ByVal Cifras As Integer, ByVal Monto As String, ByVal CadenaCodigos As String) As List(Of Capa_Datos.CodigoMasCargadoDTO)
    Dim MyLista As DataTable
    Dim ListaAUX As New List(Of Capa_Datos.CodigoMasCargadoDTO)
    MyLista = DALConsultas.CargasClientesDesdeHasta(ClienteDesde, ClienteHasta, CadenaCodigos, HF_fecha.Value)

    For Each r As DataRow In MyLista.Rows
      If r(0).ToString.Length = Cifras Then
        If r(1) >= Monto Then
          Dim MyCMC As New Capa_Datos.CodigoMasCargadoDTO

          MyCMC.PID = r(0)
          MyCMC.Importe = r(1)
          MyCMC.Zona = r(2)

          ListaAUX.Add(MyCMC)
        End If


      End If

    Next
    Return ListaAUX

  End Function

  Private Sub LlenarTabla1Cifra(ByVal CadenaCodigos As String)
    Lista1Cifras = Buscar(txtClienteDesde.Text, txtClienteHasta.Text, 1, txtImporte1.Text, CadenaCodigos)
    grvCifra1.DataSource = Lista1Cifras
    grvCifra1.DataBind()

  End Sub

  Private Sub LlenarTabla2Cifra(ByVal CadenaCodigos As String)
    Lista2Cifras = Buscar(txtClienteDesde.Text, txtClienteHasta.Text, 2, txtImporte2.Text, CadenaCodigos)
    grvCifra2.DataSource = Lista2Cifras
    grvCifra2.DataBind()

  End Sub
  Private Sub LlenarTabla3Cifra(ByVal CadenaCodigos As String)
    Lista3Cifras = Buscar(txtClienteDesde.Text, txtClienteHasta.Text, 3, txtImporte3.Text, CadenaCodigos)
    grvCifra3.DataSource = Lista3Cifras
    grvCifra3.DataBind()

  End Sub
  Private Sub LlenarTabla4Cifra(ByVal CadenaCodigos As String)
    Lista4Cifras = Buscar(txtClienteDesde.Text, txtClienteHasta.Text, 4, txtImporte4.Text, CadenaCodigos)
    grvCifra4.DataSource = Lista4Cifras
    grvCifra4.DataBind()

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

#End Region


  'Private Sub btn_buscar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.ServerClick
  'lb_cliente_nomb.InnerText = "Nombre:"
  'Txt_importe.Text = ""
  'Txt_diasacobrar.Text = ""
  'Txt_porcentaje.Text = ""

  'Try
  '  Dim Fecha As Date = CDate(txt_fecha.Text)

  '  If Txt_cliente_codigo.Text <> "" Then
  '    Dim ds_info As DataSet = DAprestamoscreditos.Creditos_buscar_cliente_info(Txt_cliente_codigo.Text, txt_fecha.Text)
  '    If ds_info.Tables(2).Rows.Count <> 0 Then
  '      'cargo la info del credito que se recupero para esa fecha.
  '      lb_cliente_nomb.InnerText = "Nombre: " + ds_info.Tables(2).Rows(0).Item("Nombre")
  '      Txt_importe.Text = CDec(ds_info.Tables(2).Rows(0).Item("Importe"))
  '      Txt_porcentaje.Text = CDec(ds_info.Tables(2).Rows(0).Item("Porcentaje"))
  '      Txt_diasacobrar.Text = ds_info.Tables(2).Rows(0).Item("Dias")
  '      Txt_importe.Focus()
  '    Else
  '      If ds_info.Tables(0).Rows.Count <> 0 Then
  '        lb_cliente_nomb.InnerText = "Nombre: " + ds_info.Tables(0).Rows(0).Item("Nombre")
  '        Txt_importe.Focus()
  '      Else
  '        'no existe, emitir un mensaje.
  '        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_noexiste", "$(document).ready(function () {$('#modal-sm_error_noexiste').modal();});", True)
  '      End If


  '    End If
  '  Else
  '    'no existe, emitir un mensaje.
  '    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_noexiste", "$(document).ready(function () {$('#modal-sm_error_noexiste').modal();});", True)
  '  End If


  'Catch ex As Exception
  '  'mensaje ingrese fecha para buscar.
  '  'no existe, emitir un mensaje.
  '  ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error_fecha", "$(document).ready(function () {$('#modal-sm_error_fecha').modal();});", True)
  'End Try


  '  End Sub
  '
#Region "INIT"
  'AQUI agrego el atributo onfocus y asocio a la rutina js seleccionartexto para que cuando se ponga el foco en un textbox se seleccione todo el contenido
  'Private Sub Txt_cliente_codigo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_cliente_codigo.Init
  '  Txt_cliente_codigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  'End Sub

  'Private Sub txt_fecha_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_fecha.Init
  '  txt_fecha.Attributes.Add("onfocus", "seleccionarTexto(this);")
  'End Sub

  'Private Sub Txt_importe_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_importe.Init
  '  Txt_importe.Attributes.Add("onfocus", "seleccionarTexto(this);")
  'End Sub

  'Private Sub Txt_porcentaje_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_porcentaje.Init
  '  Txt_porcentaje.Attributes.Add("onfocus", "seleccionarTexto(this);")
  'End Sub

  'Private Sub Txt_diasacobrar_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_diasacobrar.Init
  '  Txt_diasacobrar.Attributes.Add("onfocus", "seleccionarTexto(this);")
  'End Sub

#End Region

#Region "modal-sm_error_noexiste"
  Private Sub btn_close_error_noexiste_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_noexiste.ServerClick
    'Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_ok_error_noexiste_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_noexiste.ServerClick
    ' Txt_cliente_codigo.Focus()
  End Sub
#End Region

#Region "modal-sm_error_fecha"
  Private Sub btn_close_error_fecha_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_fecha.ServerClick
    ' txt_fecha.Focus()
  End Sub

  Private Sub btn_ok_error_fecha_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_fecha.ServerClick
    'txt_fecha.Focus()
  End Sub
#End Region

  Private Sub limpiar_label_error()
    'lb_error_codigo.Visible = False
    'lb_error_fecha.Visible = False
    'lb_error_importe.Visible = False
    'lb_error_porcentaje.Visible = False
    'lb_error_dias.Visible = False
  End Sub




#Region "Mdl_graba_alta"
  Private Sub btn_graba_alta_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_alta_close.ServerClick
    'Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_graba_alta_cancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_alta_cancelar.ServerClick
    'Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_graba_alta_confirmar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_alta_confirmar.ServerClick
    ''aqui codigo de alta.
    'Dim importe As Decimal
    'Try
    '  importe = CDec(Txt_importe.Text.Replace(".", ","))

    'Catch ex As Exception
    '  importe = CDec(0)
    'End Try
    'Dim porcentaje As Decimal

    'Try
    '  porcentaje = CDec(Txt_porcentaje.Text.Replace(".", ","))

    'Catch ex As Exception
    '  porcentaje = CDec(0)
    'End Try

    'Dim Saldo As Decimal = CDec(importe) * CDec(porcentaje)
    'Dim Cuota_valor As Decimal = (importe * porcentaje) / CInt(Txt_diasacobrar.Text)
    'DAprestamoscreditos.Creditos_alta(CInt(Session("Cliente")), txt_fecha.Text, importe, porcentaje, Txt_diasacobrar.Text, Saldo, Cuota_valor)

    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
  End Sub
#End Region

#Region "modal-sm_OKGRABADO"
  Private Sub btn_graba_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABML Prestamos_Creditos/abml_prestamoscreditos.aspx")
  End Sub

  Private Sub btn_ok_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_ABML Prestamos_Creditos/abml_prestamoscreditos.aspx")
  End Sub
#End Region

#Region "modal-sm_error_ingreso"
  Private Sub btn_close_error_ingreso_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_ingreso.ServerClick
    ' Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_ok_error_ingreso_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_ingreso.ServerClick
    ' Txt_cliente_codigo.Focus()
  End Sub
#End Region


#Region "Mdl_graba_modif"
  Private Sub btn_graba_modif_cancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_modif_cancelar.ServerClick
    ' Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_graba_modif_close_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_modif_close.ServerClick
    ' Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_graba_modif_confirmar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_graba_modif_confirmar.ServerClick
    ''aqui codigo de alta.
    'Dim importe As Decimal
    'Try
    '  importe = CDec(Txt_importe.Text.Replace(".", ","))
    'Catch ex As Exception
    '  importe = CDec(0)
    'End Try

    'Dim porcentaje As Decimal
    'Try
    '  porcentaje = CDec(Txt_porcentaje.Text.Replace(".", ","))
    'Catch ex As Exception
    '  porcentaje = CDec(0)
    'End Try
    'Dim Saldo As Decimal = CDec(importe) * CDec(porcentaje)
    'Dim Cuota_valor As Decimal = (importe * porcentaje) / CInt(Txt_diasacobrar.Text)
    'DAprestamoscreditos.Creditos_modificar(CInt(Session("Cliente")), txt_fecha.Text, importe, porcentaje, Txt_diasacobrar.Text, Saldo, 1, Cuota_valor)
    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
  End Sub
#End Region

#Region "modal-sm_error_limite"
  Private Sub btn_close_error_limite_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error_limite.ServerClick
    'Txt_cliente_codigo.Focus()
  End Sub

  Private Sub btn_ok_error_limite_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error_limite.ServerClick
    'Txt_cliente_codigo.Focus()
  End Sub






#End Region



End Class
