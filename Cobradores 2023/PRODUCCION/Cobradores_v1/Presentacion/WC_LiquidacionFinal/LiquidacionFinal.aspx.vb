Public Class LiquidacionFinal
  Inherits System.Web.UI.Page

#Region "DECLARACIONES"
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DArecorrido As New Capa_Datos.WC_recorridos_zonas
  Dim DApuntos As New Capa_Datos.WC_puntos
  Dim DALiquidacion As New Capa_Datos.WC_Liquidacion
  Dim DACliente As New Capa_Datos.WB_clientes
  Dim DAPremios As New Capa_Datos.WC_premios
  Dim DAAnticipados As New Capa_Datos.WC_anticipados
  Dim DACtaCte As New Capa_Datos.WC_CtaCte
  Dim DAreliquidacion As New Capa_Datos.WC_Reliquidacion
  Dim DAconsultas As New Capa_Datos.WB_Consultas
  Dim DAconfiguracion As New Capa_Datos.Configuracion
  Dim DAcubiertas As New Capa_Datos.Cubiertas
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
      Permisos()
      'AQUI VALIDO, SI NO HAY NINGUNA FECHA EN LA TABLA PARAMETRO, PONGO UN MENSAJE MODAL QUE DIGA:
      'ERROR, PRIMERO DEBE INICIAR DIA.
      Dim ds_info As DataSet = DAparametro.Parametro_obtener_dia
      If ds_info.Tables(0).Rows.Count <> 0 Then
        Dim Estado_liquidacion As String = ""
        Try
          Estado_liquidacion = CStr(ds_info.Tables(0).Rows(0).Item("LiqFinal"))
        Catch ex As Exception
          Estado_liquidacion = ""
        End Try

        If Estado_liquidacion = "" Then
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
          'mostrar_zonas_habilitadas(dia)
        Else
          'AQUI MENSAJE: "ERROR, DEBE EJECUTAR PREVIAMIENTE EL PROCESO DE RELIQUIDACION PARA LA FECHA DE TRABAJO."
          LabelMsj_Modal_ok_error.Text = "Debe ejecutar previamente el proc. De Reliquidacion!"
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
        End If

      Else
        'AQUI MENSAJE Y QUE CON EL BOTON "OK" U "CLOSE" VUELVA AL MENU PRINCIPAL.
        LabelMsj_Modal_ok_error.Text = "Primero debe iniciar el dia!"
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
      End If

      Txt_OP.Focus()
      Session("VALIDACION") = "CONTINUAR"

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

  Private Sub Txt_OP_Init(sender As Object, e As EventArgs) Handles Txt_OP.Init
    Txt_OP.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_error_close_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_ok_error_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_modificar_ServerClick(sender As Object, e As EventArgs) Handles btn_modificar.ServerClick
    If Txt_OP.Text.ToUpper = "OK" Then
      Session("OP") = "4" 'la opcion 4 es para generar un .zip con las 2 bd, WebCentral previa a la Liq. y la BD Copy.

      'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx','_blank','height=600px,width=600px,scrollbars=1');", True)
      ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript:window.open( '../WC_Backup/Descargando.aspx');", True)

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-msj_continue", "$(document).ready(function () {$('#modal-msj_continue').modal();});", True)

      'metodo1() ahora se ejecuta metodo1, al hacer click en el boton Btn_Ok_continue



    Else
      'error
      Txt_OP.Focus()
    End If
  End Sub

  Private Sub Btn_ErrorValidacionOk_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorValidacionOk.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Btn_ErrorValidacionClose_ServerClick(sender As Object, e As EventArgs) Handles Btn_ErrorValidacionClose.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub


#End Region

#Region "METODOS"
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

  Private Sub CubiertasGeneralAOtroCliente_recorrerYsumar(ByVal Importe_max As Decimal, ByVal PID As Integer, ByVal Recorrido As String, ByVal ds_Xcargas As DataSet, ByVal Consideracion As Integer, ByVal Cliente_codigo As Integer, ByVal registro As DataRow, ByRef DS_liqfinal As DataSet, ByRef DT_xcargas_copy As DataTable)
    'primero verifico que el recorrido y pid no se encuentren en la tabla auxiliar XCargas_duplicados.
    'nota: su existencia en XCargas_duplicados saltea todo el paso ya que se supero previamente el importe excedente. 
    Dim valido As String = "si"
    Dim t As Integer = 0
    While t < DS_liqfinal.Tables("XCargas_duplicados").Rows.Count
      If (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Recorrido") = registro.Item("Recorrido_codigo") And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Pid") = PID)) Then
        valido = "no"
        Exit While
      End If
      t = t + 1
    End While

    If valido = "si" Then
      Dim i As Integer = 0
      Dim Suma As Decimal = 0
      'While i < ds_Xcargas.Tables(0).Rows.Count
      '  If (PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
      '    Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
      '  End If
      '  i = i + 1
      'End While
      'ahora vemos si la suma > importe_max
      If Suma = 0 Then
        'vuelvo a sumar pero solo considerando los registros que sus campos sean iguales en S, Pid2, S2, R, SC, V, T
        i = 0
        Suma = 0
        While i < ds_Xcargas.Tables(0).Rows.Count
          If (CStr(PID) = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
#Region "Asignacion en variables"
            Dim S As Integer = 0
            Dim PID2 As String = ""
            Dim S2 As Integer = 0
            Dim R As Boolean = False
            Try
              S = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
            Catch ex As Exception
            End Try
            Try
              PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
            Catch ex As Exception
            End Try
            Try
              S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
            Catch ex As Exception
            End Try
            Try
              R = ds_Xcargas.Tables(0).Rows(i).Item("R")
            Catch ex As Exception
            End Try

            Dim reg_S As Integer = 0
            Dim reg_Pid2 As String = ""
            Dim reg_S2 As Integer = 0
            Dim reg_R As Boolean = False
            Try
              reg_S = registro.Item("Suc")
            Catch ex As Exception
            End Try
            Try
              reg_Pid2 = registro.Item("Pid2")
            Catch ex As Exception
            End Try
            Try
              reg_S2 = registro.Item("Suc2")
            Catch ex As Exception
            End Try
            Try
              reg_R = registro.Item("R")
            Catch ex As Exception
            End Try
#End Region
            'If (reg_Pid2 = PID2) And (reg_S2 = S2) And (reg_R = R) Then
            '  Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
            'End If
            Select Case Consideracion
              Case 0
                If (S = 0 Or S = 1) And (PID2 = "") And (S2 = 0) And (R = False) Then
                  Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                End If
              Case 1
                Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                'If (reg_Pid2 = PID2) And (reg_S2 = S2) And (reg_R = R) Then
                '  Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                'End If
            End Select

          End If
          i = i + 1
        End While
        'DUPLICO REGISTRO EN BD
        Dim excedente As Decimal = Suma - Importe_max
        If Suma > Importe_max Then

          DALiquidacion.XCargas_duplicar(registro.Item("IDcarga"), excedente, registro.Item("Recorrido_codigo"), Cliente_codigo)



          '----PRUEBA: SE VA AGREGAR REGISTRO EN TABLA AUXILIAR PARA GUARDAR TODO DE UNA AL FINAL DEL PROC CUBIERTAS GENERAL

          'Dim ee As Integer = 0
          'Dim existe As String = "no"
          'While ee < DT_xcargas_copy.Rows.Count
          '  If (registro.Item("IDcarga") = DT_xcargas_copy.Rows(ee).Item("IDcarga")) And (excedente = DT_xcargas_copy.Rows(ee).Item("Importe")) Then
          '    existe = "si"
          '    Exit While
          '  End If
          '  ee = ee + 1
          'End While
          'If existe = "no" Then
          '  DT_xcargas_copy.ImportRow(registro)
          '  Dim ultimo As Integer = DT_xcargas_copy.Rows.Count - 1
          '  DT_xcargas_copy.Rows(ultimo).Item("Cliente") = Cliente_codigo
          '  DT_xcargas_copy.Rows(ultimo).Item("Importe") = excedente
          '  DT_xcargas_copy.Rows(ultimo).Item("ID_unir") = ultimo
          'End If
          'Dim Fila_dos As DataRow = DS_liqfinal.Tables("CubGeneral_xcargas_recorridos").NewRow
          'Fila_dos("IDcarga") = CInt(registro.Item("IDcarga"))
          'Fila_dos("Recorrido_Codigo") = registro.Item("Recorrido_codigo")
          'Fila_dos("ID_unir") = DT_xcargas_copy.Rows(DT_xcargas_copy.Rows.Count - 1).Item("ID_unir")
          'DS_liqfinal.Tables("CubGeneral_xcargas_recorridos").Rows.Add(Fila_dos)
          ''actualizo Totalimporte
          'ee = 0
          'Dim Totalimporte As Decimal = 0
          'Dim ID_unir = DT_xcargas_copy.Rows(DT_xcargas_copy.Rows.Count - 1).Item("ID_unir")
          'While ee < DS_liqfinal.Tables("CubGeneral_xcargas_recorridos").Rows.Count

          '  If DS_liqfinal.Tables("CubGeneral_xcargas_recorridos").Rows(ee).Item("ID_unir") = ID_unir Then
          '    Totalimporte = Totalimporte + excedente
          '  End If
          '  ee = ee + 1
          'End While
          'DT_xcargas_copy.Rows(DT_xcargas_copy.Rows.Count - 1).Item("Totalimporte") = Totalimporte
          '-------------------------------------------------------------------

          'agrego un registro en una tabla auxiliar para saber cuales son los registros que se duplicaron y que ya no se consideraran si se los vuelve a encontrar.
          Dim fila As DataRow = DS_liqfinal.Tables("XCargas_duplicados").NewRow
          fila("Cliente") = Cliente_codigo
          fila("Recorrido") = registro.Item("Recorrido_codigo")
          fila("Pid") = PID
          DS_liqfinal.Tables("XCargas_duplicados").Rows.Add(fila)
        End If
      End If

    End If



  End Sub

  Private Sub Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(ByVal Importe_max As Decimal, ByVal PID As Integer, ByVal Recorrido As String, ByVal ds_Xcargas As DataSet, ByVal Consideracion As Integer, ByVal Cliente_codigo As Integer, ByVal registro As DataRow, ByRef DS_liqfinal As DataSet, ByVal Grupo_id As Integer)
    'primero verifico que el recorrido y pid no se encuentren en la tabla auxiliar XCargas_duplicados.
    'nota: su existencia en XCargas_duplicados saltea todo el paso ya que se supero previamente el importe excedente. 
    Dim valido As String = "si"
    Dim t As Integer = 0
    While t < DS_liqfinal.Tables("XCargas_duplicados").Rows.Count
      If (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Recorrido") = registro.Item("Recorrido_codigo")) And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Pid") = CStr(PID)) And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Cliente") = Cliente_codigo) Then
        valido = "no"
        Exit While
      End If
      t = t + 1
    End While

    If valido = "si" Then
      Dim i As Integer = 0
      Dim Suma As Decimal = 0
      'While i < ds_Xcargas.Tables(0).Rows.Count
      '  'recupero info del cliente, necesito saber a que grupo pertenece.
      '  Dim DT_Clie As DataTable = DACliente.Clientes_buscar_grupo(CStr(ds_Xcargas.Tables(0).Rows(i).Item("Cliente")))
      '  If DT_Clie.Rows.Count <> 0 Then
      '    Dim Clie_Grupo_id As Integer = DT_Clie.Rows(0).Item("Grupo_id")
      '    'si registro en xcargas pertenece al mismo grupo, se lo suma
      '    If (Grupo_id = Clie_Grupo_id) And (PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
      '      Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
      '    End If
      '  End If
      '  i = i + 1
      'End While
      'ahora vemos si la suma > importe_max
      If Suma = 0 Then
        'vuelvo a sumar pero solo considerando los registros que sus campos sean iguales en S, Pid2, S2, R, SC, V, T
        i = 0
        Suma = 0
        While i < ds_Xcargas.Tables(0).Rows.Count
          'recupero info del cliente, necesito saber a que grupo pertenece.
          Dim DT_Clie As DataTable = DACliente.Clientes_buscar_grupo(CStr(ds_Xcargas.Tables(0).Rows(i).Item("Cliente")))
          If DT_Clie.Rows.Count <> 0 Then
            Dim Clie_Grupo_id As Integer = DT_Clie.Rows(0).Item("Grupo_id")
            'si registro en xcargas pertenece al mismo grupo, se lo suma
            If (Grupo_id = Clie_Grupo_id) And (CStr(PID) = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
#Region "Asignacion en variables"
              Dim S As Integer = 0
              Dim PID2 As String = ""
              Dim S2 As Integer = 0
              Dim R As Boolean = False
              Try
                S = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
              Catch ex As Exception
              End Try
              Try
                PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                R = ds_Xcargas.Tables(0).Rows(i).Item("R")
              Catch ex As Exception
              End Try

              Dim reg_S As Integer = 0
              Dim reg_Pid2 As String = ""
              Dim reg_S2 As Integer = 0
              Dim reg_R As Boolean = False
              Try
                reg_S = registro.Item("Suc")
              Catch ex As Exception
              End Try
              Try
                reg_Pid2 = registro.Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                reg_S2 = registro.Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                reg_R = registro.Item("R")
              Catch ex As Exception
              End Try
#End Region
              'If (reg_Pid2 = PID2) And (reg_S2 = S2) And (reg_R = R) Then
              '  Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
              'End If
              Select Case Consideracion
                Case 0
                  If (S = 0 Or S = 1) And (PID2 = "") And (S2 = 0) And (R = False) Then
                    Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                  End If
                Case 1
                  Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                  'If (reg_Pid2 = PID2) And (reg_S2 = S2) And (reg_R = R) Then
                  '  Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                  'End If
              End Select
            End If
          End If
          i = i + 1
        End While
        'DUPLICO REGISTRO EN BD
        Dim excedente As Decimal = Suma - Importe_max
        If Suma > Importe_max Then
          DALiquidacion.XCargas_duplicar(registro.Item("IDcarga"), excedente, registro.Item("Recorrido_codigo"), Cliente_codigo)
          'agrego un registro en una tabla auxiliar para saber cuales son los registros que se duplicaron y que ya no se consideraran si se los vuelve a encontrar.
          Dim fila As DataRow = DS_liqfinal.Tables("XCargas_duplicados").NewRow
          fila("Cliente") = Cliente_codigo
          fila("Recorrido") = registro.Item("Recorrido_codigo")
          fila("Pid") = PID
          DS_liqfinal.Tables("XCargas_duplicados").Rows.Add(fila)
        End If
      End If

    End If
  End Sub


  Private Sub Cubiertas_PorGrupoAOtroCliente_recorrerYsumar(ByVal Importe_max As Decimal, ByVal PID As Integer, ByVal Recorrido As String, ByVal ds_Xcargas As DataSet, ByVal Consideracion As Integer, ByVal Cliente_codigo As Integer, ByVal registro As DataRow, ByRef DS_liqfinal As DataSet, ByVal Grupo_id As Integer)
    'primero verifico que el recorrido y pid no se encuentren en la tabla auxiliar XCargas_duplicados.
    'nota: su existencia en XCargas_duplicados saltea todo el paso ya que se supero previamente el importe excedente. 
    Dim valido As String = "si"
    Dim t As Integer = 0
    While t < DS_liqfinal.Tables("XCargas_duplicados").Rows.Count
      If (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Recorrido") = registro.Item("Recorrido_codigo")) And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Pid") = PID) And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Cliente") = Cliente_codigo) Then
        valido = "no"
        Exit While
      End If
      t = t + 1
    End While

    If valido = "si" Then
      Dim i As Integer = 0
      Dim Suma As Decimal = 0
      While i < ds_Xcargas.Tables(0).Rows.Count
        'recupero info del cliente, necesito saber a que grupo pertenece.
        Dim DT_Clie As DataTable = DACliente.Clientes_buscar_grupo(CStr(ds_Xcargas.Tables(0).Rows(i).Item("Cliente")))
        If DT_Clie.Rows.Count <> 0 Then
          Dim Clie_Grupo_id As Integer = DT_Clie.Rows(0).Item("Grupo_id")
          'si registro en xcargas pertenece al mismo grupo, se lo suma
          If (Grupo_id = Clie_Grupo_id) And (PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
            Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
          End If
        End If
        i = i + 1
      End While
      'ahora vemos si la suma > importe_max
      If Suma > Importe_max Then
        'vuelvo a sumar pero solo considerando los registros que sus campos sean iguales en S, Pid2, S2, R, SC, V, T
        i = 0
        Suma = 0
        While i < ds_Xcargas.Tables(0).Rows.Count
          'recupero info del cliente, necesito saber a que grupo pertenece.
          Dim DT_Clie As DataTable = DACliente.Clientes_buscar_grupo(CStr(ds_Xcargas.Tables(0).Rows(i).Item("Cliente")))
          If DT_Clie.Rows.Count <> 0 Then
            Dim Clie_Grupo_id As Integer = DT_Clie.Rows(0).Item("Grupo_id")
            'si registro en xcargas pertenece al mismo grupo, se lo suma
            If (Grupo_id = Clie_Grupo_id) And (PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
#Region "Asignacion en variables"
              Dim S As Integer = 0
              Dim PID2 As String = ""
              Dim S2 As Integer = 0
              Dim R As Boolean = False
              Try
                S = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
              Catch ex As Exception
              End Try
              Try
                PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                R = ds_Xcargas.Tables(0).Rows(i).Item("R")
              Catch ex As Exception
              End Try

              Dim reg_S As Integer = 0
              Dim reg_Pid2 As String = ""
              Dim reg_S2 As Integer = 0
              Dim reg_R As Boolean = False
              Try
                reg_S = registro.Item("Suc")
              Catch ex As Exception
              End Try
              Try
                reg_Pid2 = registro.Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                reg_S2 = registro.Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                reg_R = registro.Item("R")
              Catch ex As Exception
              End Try
#End Region
              If (reg_Pid2 = PID2) And (reg_S2 = S2) And (reg_R = R) Then
                Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
              End If
            End If
          End If
          i = i + 1
        End While
        'DUPLICO REGISTRO EN BD
        Dim excedente As Decimal = Suma - Importe_max
        DALiquidacion.XCargas_duplicar(registro.Item("IDcarga"), excedente, registro.Item("Recorrido_codigo"), Cliente_codigo)
        'agrego un registro en una tabla auxiliar para saber cuales son los registros que se duplicaron y que ya no se consideraran si se los vuelve a encontrar.
        Dim fila As DataRow = DS_liqfinal.Tables("XCargas_duplicados").NewRow
        fila("Cliente") = Cliente_codigo
        fila("Recorrido") = registro.Item("Recorrido_codigo")
        fila("Pid") = PID
        DS_liqfinal.Tables("XCargas_duplicados").Rows.Add(fila)
      End If

    End If

  End Sub


  Private Sub Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(ByVal Importe_max As Decimal, ByVal PID As Integer, ByVal Recorrido As String, ByVal ds_Xcargas As DataSet, ByVal Consideracion As Integer, ByVal Cliente_codigo As Integer, ByVal registro As DataRow, ByRef DS_liqfinal As DataSet, ByRef DT_xcargas_modif As DataTable)
    'primero verifico que el recorrido y pid no se encuentren en la tabla auxiliar XCargas_duplicados.
    'nota: su existencia en XCargas_duplicados saltea todo el paso ya que se supero previamente el importe excedente. 
    Dim valido As String = "si"
    Dim t As Integer = 0
    While t < DS_liqfinal.Tables("XCargas_duplicados").Rows.Count
      If (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Recorrido") = registro.Item("Recorrido_codigo")) And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Pid") = CStr(PID)) And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Cliente") = Cliente_codigo) Then
        'SI HAY COINCIDENCIA QUIERE DECIR QUE EL REGISTRO ES PURO EXCEDENTE...LO VAMOS A ACTUALIZAR PONIENDO SINCOMPUTO=TRUE
        DAcubiertas.XCargas_Y_XCargasJunto_ActualizarSinComputo(CStr(registro.Item("IDcarga")))

#Region "modifico en copia y marco registro alterado ademas SC en true"
        Dim k As Integer = 0
        While k < DT_xcargas_modif.Rows.Count
          If (DT_xcargas_modif.Rows(k).Item("IDcarga") = registro.Item("IDcarga")) Then
            DT_xcargas_modif.Rows(k).Item("Modif") = "modificado"
            If (DT_xcargas_modif.Rows(k).Item("Recorrido_codigo") = registro.Item("Recorrido_codigo")) Then
              DT_xcargas_modif.Rows(k).Item("SinComputo") = True
            End If
          End If
          k = k + 1
        End While
#End Region



        valido = "no"
        Exit While
      End If
      t = t + 1
    End While

    If valido = "si" Then
      Dim i As Integer = 0
      Dim Suma As Decimal = 0
      'While i < ds_Xcargas.Tables(0).Rows.Count
      '  If Cliente_codigo = ds_Xcargas.Tables(0).Rows(i).Item("Cliente") Then
      '    'se Suma el importe, cuando el cliente, PID y recorrido sea el mismo.
      '    If (PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
      '      Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
      '    End If
      '  End If
      '  i = i + 1
      'End While

      'While i < ds_Xcargas.Tables(0).Rows.Count

      '  i = i + 1
      'End While

      'ahora vemos si la suma > importe_max
      If Suma = 0 Then
        'vuelvo a sumar pero solo considerando los registros que sus campos sean iguales en S, Pid2, S2, R, SC, V, T
        i = 0
        Suma = 0
        While i < ds_Xcargas.Tables(0).Rows.Count
          If Cliente_codigo = ds_Xcargas.Tables(0).Rows(i).Item("Cliente") Then
            'se suma el importe, cuando el cliente, pid y recorrido sea el mismo.
            If (CStr(PID) = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
#Region "Asignacion en variables"
              Dim S As Integer = 0
              Dim PID2 As String = ""
              Dim S2 As Integer = 0
              Dim R As Boolean = False
              Try
                S = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
              Catch ex As Exception
              End Try
              Try
                PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                R = ds_Xcargas.Tables(0).Rows(i).Item("R")
              Catch ex As Exception
              End Try

              Dim reg_S As Integer = 0
              Dim reg_Pid2 As String = ""
              Dim reg_S2 As Integer = 0
              Dim reg_R As Boolean = False
              Try
                reg_S = registro.Item("Suc")
              Catch ex As Exception
              End Try
              Try
                reg_Pid2 = registro.Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                reg_S2 = registro.Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                reg_R = registro.Item("R")
              Catch ex As Exception
              End Try
#End Region
              Select Case Consideracion
                Case 0

                  If (S = 0 Or S = 1) And (PID2 = "") And (S2 = 0) And (R = False) Then
                    Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                  End If
                Case 1
                  Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                  'If (reg_Pid2 = PID2) And (reg_S2 = S2) And (reg_R = R) Then
                  '  Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
                  'End If
              End Select
              'aqui defino corte del ciclo, quiero que cuente hasta el registro actual.
              If registro.Item("IDcarga1") = ds_Xcargas.Tables(0).Rows(i).Item("IDcarga1") Then
                Exit While
              End If
            End If
          End If
          i = i + 1
        End While
        'DUPLICO REGISTRO EN BD
        If Suma > Importe_max Then
          Dim excedente As Decimal = Suma - Importe_max
          If excedente > 0 Then
            DALiquidacion.XCargas_duplicarYmodificar(registro.Item("IDcarga"), excedente, Importe_max, registro.Item("Recorrido_codigo"), Cliente_codigo)
            'agrego un registro en una tabla auxiliar para saber cuales son los registros que se duplicaron y que ya no se consideraran si se los vuelve a encontrar.
            Dim fila As DataRow = DS_liqfinal.Tables("XCargas_duplicados").NewRow
            fila("Cliente") = Cliente_codigo
            fila("Recorrido") = registro.Item("Recorrido_codigo")
            fila("Pid") = PID
            DS_liqfinal.Tables("XCargas_duplicados").Rows.Add(fila)

#Region "modifico en copia y marco registro alterado"
            Dim k As Integer = 0
            While k < DT_xcargas_modif.Rows.Count
              If (DT_xcargas_modif.Rows(k).Item("IDcarga") = registro.Item("IDcarga")) Then
                DT_xcargas_modif.Rows(k).Item("Modif") = "modificado"
                If (DT_xcargas_modif.Rows(k).Item("Recorrido_codigo") = registro.Item("Recorrido_codigo")) Then
                  DT_xcargas_modif.Rows(k).Item("Importe") = CDec(DT_xcargas_modif.Rows(k).Item("Importe")) - excedente
                End If
              End If
              k = k + 1
            End While
#End Region
          End If
        End If

      End If

    End If

  End Sub

  Private Sub Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar(ByVal Importe_max As Decimal, ByVal PID As Integer, ByVal Recorrido As String, ByVal ds_Xcargas As DataSet, ByVal Consideracion As Integer, ByVal Cliente_codigo As Integer, ByVal registro As DataRow, ByRef DS_liqfinal As DataSet)
    'primero verifico que el recorrido y pid no se encuentren en la tabla auxiliar XCargas_duplicados.
    'nota: su existencia en XCargas_duplicados saltea todo el paso ya que se supero previamente el importe excedente. 
    Dim valido As String = "si"
    Dim t As Integer = 0
    While t < DS_liqfinal.Tables("XCargas_duplicados").Rows.Count
      If (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Recorrido") = registro.Item("Recorrido_codigo")) And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Pid") = PID) And (DS_liqfinal.Tables("XCargas_duplicados").Rows(t).Item("Cliente") = Cliente_codigo) Then
        'SI HAY COINCIDENCIA QUIERE DECIR QUE EL REGISTRO ES PURO EXCEDENTE...LO VAMOS A ACTUALIZAR PONIENDO SINCOMPUTO=TRUE
        DAcubiertas.XCargas_Y_XCargasJunto_ActualizarSinComputo(CStr(registro.Item("IDcarga")))
        valido = "no"
        Exit While
      End If
      t = t + 1
    End While

    If valido = "si" Then
      Dim i As Integer = 0
      Dim Suma As Decimal = 0
      While i < ds_Xcargas.Tables(0).Rows.Count
        If Cliente_codigo = ds_Xcargas.Tables(0).Rows(i).Item("Cliente") Then
          'se suma el importe, cuando el cliente, pid y recorrido sea el mismo.
          If (PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
            Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
          End If
        End If
        i = i + 1
      End While
      'ahora vemos si la suma > importe_max
      If Suma > Importe_max Then
        'vuelvo a sumar pero solo considerando los registros que sus campos sean iguales en S, Pid2, S2, R, SC, V, T
        i = 0
        Suma = 0
        While i < ds_Xcargas.Tables(0).Rows.Count
          If Cliente_codigo = ds_Xcargas.Tables(0).Rows(i).Item("Cliente") Then
            'se suma el importe, cuando el cliente, pid y recorrido sea el mismo.
            If (PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")) And (Recorrido = ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo")) Then
#Region "Asignacion en variables"
              Dim S As Integer = 0
              Dim PID2 As String = ""
              Dim S2 As Integer = 0
              Dim R As Boolean = False
              Try
                S = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
              Catch ex As Exception
              End Try
              Try
                PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                R = ds_Xcargas.Tables(0).Rows(i).Item("R")
              Catch ex As Exception
              End Try

              Dim reg_S As Integer = 0
              Dim reg_Pid2 As String = ""
              Dim reg_S2 As Integer = 0
              Dim reg_R As Boolean = False
              Try
                reg_S = registro.Item("Suc")
              Catch ex As Exception
              End Try
              Try
                reg_Pid2 = registro.Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                reg_S2 = registro.Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                reg_R = registro.Item("R")
              Catch ex As Exception
              End Try
#End Region
              If (reg_Pid2 = PID2) And (reg_S2 = S2) And (reg_R = R) Then
                Suma = Suma + CDec(ds_Xcargas.Tables(0).Rows(i).Item("Importe"))
              End If
            End If
          End If
          i = i + 1
        End While
        'DUPLICO REGISTRO EN BD
        Dim excedente As Decimal = Suma - Importe_max
        DALiquidacion.XCargas_duplicarYmodificar(registro.Item("IDcarga"), excedente, Importe_max, registro.Item("Recorrido_codigo"), Cliente_codigo)
        'agrego un registro en una tabla auxiliar para saber cuales son los registros que se duplicaron y que ya no se consideraran si se los vuelve a encontrar.
        Dim fila As DataRow = DS_liqfinal.Tables("XCargas_duplicados").NewRow
        fila("Cliente") = Cliente_codigo
        fila("Recorrido") = registro.Item("Recorrido_codigo")
        fila("Pid") = PID
        DS_liqfinal.Tables("XCargas_duplicados").Rows.Add(fila)
      End If

    End If

  End Sub

  Private Sub SETEADO_XCARGAS()
    DAcubiertas.XCargas_Recorridos_agregarRegZ()

    'RECUPERO TODO XCARGAS Y XCARGAS RECORRIDO
    Dim ds_xcargas As DataSet = DAcubiertas.XCargas_Consultar_N1()
    If ds_xcargas.Tables(0).Rows.Count <> 0 Then
      'voy a buscar aquellos registros en xcargas_recorrido que sean mayor a 1
      Dim i As Integer = 0
      While i < ds_xcargas.Tables(0).Rows.Count
        Dim IDcarga As Integer = ds_xcargas.Tables(0).Rows(i).Item("IDcarga")
        DAcubiertas.XCargasJunto_eliminarReg(CStr(IDcarga))
        '2) copio en XCargasJunto aquellos registros de XCargas donde IDcarga sea igual al parametro proporcionado
        DAcubiertas.XCargasJunto_COPIARegFromXCargas(CStr(IDcarga))

        'Dim j As Integer = 0
        'Dim cont As Integer = 0
        'While j < ds_xcargas.Tables(1).Rows.Count
        '  If IDcarga = ds_xcargas.Tables(1).Rows(j).Item("IDcarga") Then
        '    cont = cont + 1
        '  End If
        '  j = j + 1
        'End While
        'If cont > 1 Then
        '  'si hay mas de uno:
        '  '1) elimino el registro en XCargas_Junto
        '  DAcubiertas.XCargasJunto_eliminarReg(CStr(IDcarga))
        '  '2) copio en XCargasJunto aquellos registros de XCargas donde IDcarga sea igual al parametro proporcionado
        '  DAcubiertas.XCargasJunto_COPIARegFromXCargas(CStr(IDcarga))
        'End If

        i = i + 1
      End While
      'AHORA ELIMINO XCARGAS Y XCARGAS_RECORRIDO Y LA VUELVO A LLENAR EN BASE A XCARGASJUNTO
      DAcubiertas.XCargas_eliminarRegYRecargar()


    End If
  End Sub

  Private Sub Cubiertas_IndividualDivideEnElMismoCliente_prueba1() 'ejecutar 4to
    'Validamos si hay registros tipo 1 en gruposcub
    Dim valido = "no"
    Dim ds_validar As DataSet = DAcubiertas.ClientesCub_Obtener
    If ds_validar.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_validar.Tables(0).Rows.Count
        Dim UnaCifra As Decimal = 0
        Dim DosCifras As Decimal = 0
        Dim TresCifras As Decimal = 0
        Dim CuatroCifras As Decimal = 0
        Try
          UnaCifra = CDec(ds_validar.Tables(0).Rows(i).Item("UnaCifra"))
        Catch ex As Exception
        End Try
        Try
          DosCifras = CDec(ds_validar.Tables(0).Rows(i).Item("DosCifras"))
        Catch ex As Exception
        End Try
        Try
          TresCifras = CDec(ds_validar.Tables(0).Rows(i).Item("TresCifras"))
        Catch ex As Exception
        End Try
        Try
          CuatroCifras = CDec(ds_validar.Tables(0).Rows(i).Item("CuatroCifras"))
        Catch ex As Exception
        End Try
        Dim cifra_anular As Decimal = 99999.99
        If UnaCifra <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If DosCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If TresCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If CuatroCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        i = i + 1
      End While
    Else
      valido = "no"
    End If


    If valido = "si" Then
      Dim DS_liqfinal As New DS_liqfinal

      'obtener todos los registros de 
      Dim ds_Xcargas As DataSet = DALiquidacion.Liquidacion_todoXcargas
      If ds_Xcargas.Tables(0).Rows.Count <> 0 Then
        Dim DT_xcargas_modif As DataTable = ds_Xcargas.Tables(0).Copy()
        DT_xcargas_modif.Columns.Add("Modif") 'aqui voy a ir marcando los registros q se alteraron

        Dim i As Integer = 0
        While i < ds_Xcargas.Tables(0).Rows.Count
          Dim Cliente As Integer = ds_Xcargas.Tables(0).Rows(i).Item("Cliente")
          Dim PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")
          Dim Recorrido As String = CStr(ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo").ToString).ToUpper
          'recupero info del cliente, necesito recuperar la configuracion de cubierta si es que posee.
          Dim Ds_Clie As DataSet = DAcubiertas.ClientesCub_BuscarCliente(CStr(Cliente))
          If Ds_Clie.Tables(0).Rows.Count <> 0 Then
            'Dim Grupo_id As Integer = DT_Clie.Rows(0).Item("Grupo_id")
            'Dim Grupo_codigo As String = DT_Clie.Rows(0).Item("Grupos_Codigo")
            'consulto si tiene cubierta configurada.
            Dim Cliente_codigo As Integer = Cliente 'este codigo se va a insertar en cada registro que se requiera duplicar.
            Dim UnaCifra As Decimal = CDec(Ds_Clie.Tables(0).Rows(0).Item("UnaCifra"))
            Dim DosCifras As Decimal = CDec(Ds_Clie.Tables(0).Rows(0).Item("DosCifras"))
            Dim TresCifras As Decimal = CDec(Ds_Clie.Tables(0).Rows(0).Item("TresCifras"))
            Dim CuatroCifras As Decimal = CDec(Ds_Clie.Tables(0).Rows(0).Item("CuatroCifras"))
            Dim Consideracion As Integer = CInt(Ds_Clie.Tables(0).Rows(0).Item("Consideracion"))

            Dim RecValido = ""
            Recorridos_Validar(RecValido, Recorrido)
            If RecValido = "no" Then 'si hay una Z o algun caracter q no sea un recorrido, se lo pasa x alto.
            Else
              Dim pid_valido = "si"
              Try

                Dim valid_PID As Integer = CInt(ds_Xcargas.Tables(0).Rows(i).Item("Pid"))

              Catch ex As Exception
                'si es un simbolo o letra se lo pasa x alto.
                pid_valido = "no"
              End Try
              If pid_valido = "si" Then
                Dim Suc As Integer = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
                Dim PID2 As String = ""
                Dim S2 As Integer = 0
                Dim R As Boolean = False
                Try
                  PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
                Catch ex As Exception
                End Try
                Dim S2_validacion = ""
                Try
                  S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
                Catch ex As Exception
                  S2_validacion = "vacio"
                End Try
                Try
                  R = ds_Xcargas.Tables(0).Rows(i).Item("R")
                Catch ex As Exception
                End Try

                Select Case Consideracion
                  Case 0
                    If (Suc = 0 Or Suc = 1) And ((PID2 = "") Or (S2_validacion = "vacio") Or (R = False)) Then
                      Dim v_anular As Decimal = "99999,99"
                      Select Case Len(PID) 'devuelve cantidad de digitos en pid
                        Case 1
                          ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                          If UnaCifra <> v_anular Then
                            Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                          End If
                        Case 2
                          If DosCifras <> v_anular Then
                            Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                          End If
                        Case 3
                          If TresCifras <> v_anular Then
                            Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                          End If
                        Case 4
                          If CuatroCifras <> v_anular Then
                            Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                          End If
                      End Select
                    End If
                  Case 1 'se consideran todos los registros, sin importar el valor que tenga en SUC, y los demas datos en Pid2, Suc2 y R.
                    Dim v_anular As Decimal = "99999,99"
                    Select Case Len(PID) 'devuelve cantidad de digitos en pid
                      Case 1
                        ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                        If UnaCifra <> v_anular Then
                          Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                        End If
                      Case 2
                        If DosCifras <> v_anular Then
                          Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                        End If
                      Case 3
                        If TresCifras <> v_anular Then
                          Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                        End If
                      Case 4
                        If CuatroCifras <> v_anular Then
                          Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                        End If
                    End Select
                End Select


                'If (Consideracion = 0) And (PID <> "0" Or PID <> "1") And ((PID2 <> "") Or (S2 <> 0) Or (R = True)) Then 'Consideracion=0, solo considera Suc=0 o Suc=1, con S2,Pid2,R=false....si no se cumple esto, se ignora.
                '  'ignorar, pasar por alto.
                'Else
                '  Dim v_anular As Decimal = "99999,99"
                '  Select Case Len(PID) 'devuelve cantidad de digitos en pid
                '    Case 1
                '      ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                '      If UnaCifra <> v_anular Then
                '        Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                '      End If
                '    Case 2
                '      If DosCifras <> v_anular Then
                '        Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                '      End If
                '    Case 3
                '      If TresCifras <> v_anular Then
                '        Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                '      End If
                '    Case 4
                '      If CuatroCifras <> v_anular Then
                '        Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar_prueba2(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_modif)
                '      End If
                '  End Select
                'End If
              End If
            End If

          Else
            'no existe el cliente
          End If
          i = i + 1
        End While

#Region "MODIFICO REGISTRO ORIGINALES EN BD, CREO NUEVOS AGRUPOS POR RECORRIDO Y CLIENTES CON EL MISMO IMPORTE EN UN DETERMINADO IDCARGA"
        If DT_xcargas_modif.Rows.Count <> 0 Then
          Dim k As Integer = 0
          While k < DT_xcargas_modif.Rows.Count
            Dim Modif As String = ""
            Try

            Catch ex As Exception

            End Try
            If (DT_xcargas_modif.Rows(k).Item("Terminal") <> "C") And (DT_xcargas_modif.Rows(k).Item("Modif").ToString = "modificado") Then
              Dim IDcarga As Integer = DT_xcargas_modif.Rows(k).Item("IDcarga")

              If IDcarga = 6 Then
                Dim choquito = "estoy en 6"
              End If

              Dim cont As Integer = 0 'cuento la cant de registro para IDcarga, es decir cant de recorridos
              Dim kk As Integer = 0
              While kk < DT_xcargas_modif.Rows.Count
                If IDcarga = DT_xcargas_modif.Rows(kk).Item("IDcarga") Then
                  cont = cont + 1
                End If
                kk = kk + 1
              End While
              Dim filtro_foud_aux As String = "IDcarga = " + CStr(IDcarga)
              Dim rows_found_aux() As DataRow = DS_liqfinal.Tables("AUXILIAR1").Select(filtro_foud_aux, "Importe ASC")


              If (cont > 1) And (rows_found_aux.Count = 0) Then
                'quiere decir que tengo que separar los registros.
                'para ello voy a usar unas tablas auxiliar para ir colocando cierta informacion.
                Dim dt_filtado As DataTable = DT_xcargas_modif.Clone
                Dim filtro As String = "IDcarga = " + CStr(IDcarga)
                Dim rows() As DataRow = DT_xcargas_modif.Select(filtro, "Importe ASC")
                For Each row As DataRow In rows
                  ' Indicamos que el registro ha sido añadido
                  'row.SetAdded()
                  dt_filtado.ImportRow(row)
                Next
                Dim idc As Integer = 0
                While idc < dt_filtado.Rows.Count
                  Dim Importe As Decimal = dt_filtado.Rows(idc).Item("Importe")
                  Dim SinComputo As Boolean = False
                  Try
                    SinComputo = dt_filtado.Rows(idc).Item("SinComputo")
                  Catch ex As Exception
                  End Try

                  'Dim dt_validar As DataTable = DS_liqfinal.Tables("AUXILIAR1").Clone
                  Dim filtro2 As String = "Importe = " + CStr(Importe).Replace(",", ".") + " AND IDcarga = " + CStr(IDcarga)
                  'Dim filtro2 As String = "Importe = 10.00 "
                  Dim rows_validar() As DataRow = DS_liqfinal.Tables("AUXILIAR1").Select(filtro2)

                  If rows_validar.Count = 0 Then
                    Dim idc2 As Integer = 0
                    Dim contador_1 As Integer = 0
                    While idc2 < dt_filtado.Rows.Count
                      Dim SC As Boolean = False
                      Try
                        SC = dt_filtado.Rows(idc2).Item("SinComputo")
                      Catch ex As Exception
                      End Try
                      If (Importe = dt_filtado.Rows(idc2).Item("Importe")) And (SinComputo = False) Then
                        contador_1 = contador_1 + 1
                        Dim fila As DataRow = DS_liqfinal.Tables("AUXILIAR2").NewRow
                        fila("id_aux") = CInt(DS_liqfinal.Tables("AUXILIAR1").Rows.Count)
                        fila("ID") = CInt(dt_filtado.Rows(idc2).Item("ID"))
                        fila("Recorrido_codigo") = CStr(dt_filtado.Rows(idc2).Item("Recorrido_codigo"))
                        DS_liqfinal.Tables("AUXILIAR2").Rows.Add(fila)
                      End If
                      idc2 = idc2 + 1
                    End While
                    If contador_1 <> 0 Then
                      Dim fila2 As DataRow = DS_liqfinal.Tables("AUXILIAR1").NewRow
                      fila2("id_aux") = DS_liqfinal.Tables("AUXILIAR1").Rows.Count
                      fila2("IDcarga") = IDcarga
                      fila2("Importe") = Importe
                      fila2("cont") = contador_1
                      fila2("SC") = 0
                      DS_liqfinal.Tables("AUXILIAR1").Rows.Add(fila2)
                    End If

                    idc2 = 0
                    contador_1 = 0
                    While idc2 < dt_filtado.Rows.Count
                      Dim SC As Boolean = False
                      Try
                        SC = dt_filtado.Rows(idc2).Item("SinComputo")
                      Catch ex As Exception
                      End Try
                      If (Importe = dt_filtado.Rows(idc2).Item("Importe")) And (SinComputo = True) Then
                        contador_1 = contador_1 + 1
                        Dim fila As DataRow = DS_liqfinal.Tables("AUXILIAR2").NewRow
                        fila("id_aux") = CInt(DS_liqfinal.Tables("AUXILIAR1").Rows.Count)
                        fila("ID") = CInt(dt_filtado.Rows(idc2).Item("ID"))
                        fila("Recorrido_codigo") = CStr(dt_filtado.Rows(idc2).Item("Recorrido_codigo"))
                        DS_liqfinal.Tables("AUXILIAR2").Rows.Add(fila)
                      End If
                      idc2 = idc2 + 1
                    End While
                    If contador_1 <> 0 Then
                      Dim fila2 As DataRow = DS_liqfinal.Tables("AUXILIAR1").NewRow
                      fila2("id_aux") = DS_liqfinal.Tables("AUXILIAR1").Rows.Count
                      fila2("IDcarga") = IDcarga
                      fila2("Importe") = Importe
                      fila2("cont") = contador_1
                      fila2("SC") = 1
                      DS_liqfinal.Tables("AUXILIAR1").Rows.Add(fila2)
                    End If
                  End If
                  idc = idc + 1
                End While
              Else

                '30-11-2022 ---esto si es 1 registro
                'directamente actualizo en xcargas, xcargas recorrido y xcargasjunto.
                Dim Importe As Decimal = CDec(DT_xcargas_modif.Rows(k).Item("Importe"))
                DALiquidacion.XCargasYXCargasJunto_actualizar2(CStr(IDcarga), CStr(Importe).Replace(",", "."))
              End If
            End If

            k = k + 1
          End While

          If DS_liqfinal.Tables("AUXILIAR1").Rows.Count <> 0 Then
            k = 0
            While k < DS_liqfinal.Tables("AUXILIAR1").Rows.Count
              Dim id_aux As String = DS_liqfinal.Tables("AUXILIAR1").Rows(k).Item("id_aux")
              Dim IDcarga As Integer = CInt(DS_liqfinal.Tables("AUXILIAR1").Rows(k).Item("idcarga"))
              Dim importe As Decimal = CDec(DS_liqfinal.Tables("AUXILIAR1").Rows(k).Item("Importe"))
              Dim cont As Integer = CInt(DS_liqfinal.Tables("AUXILIAR1").Rows(k).Item("cont"))
              Dim TotalImporte As Decimal = importe * cont
              Dim SinComputo As Integer = CInt(DS_liqfinal.Tables("AUXILIAR1").Rows(k).Item("SC"))
              Dim ds_xcg As DataSet = DALiquidacion.XCargas_Cubiertainsert(IDcarga, importe, TotalImporte, SinComputo)

              If ds_xcg.Tables(0).Rows.Count <> 0 Then
                Dim new_IDcarga As Integer = ds_xcg.Tables(0).Rows(0).Item("IDcarga")
                Dim Cadena_Recorridos As String = ""
                Dim j As Integer = 0
                Dim filtro As String = "id_aux = " + id_aux
                Dim rows() As DataRow = DS_liqfinal.Tables("AUXILIAR2").Select(filtro, "Recorrido_codigo")
                Dim dt_Auxiliar2_filtrado As DataTable = DS_liqfinal.Tables("AUXILIAR2").Clone
                For Each row As DataRow In rows
                  ' Indicamos que el registro ha sido añadido
                  'row.SetAdded()
                  dt_Auxiliar2_filtrado.ImportRow(row)
                Next
                While j < dt_Auxiliar2_filtrado.Rows.Count
                  Dim Recorrido_codigo As String = dt_Auxiliar2_filtrado.Rows(j).Item("Recorrido_codigo")
                  Cadena_Recorridos = Cadena_Recorridos + Recorrido_codigo
                  DALiquidacion.Eliminar_XCargas_Recorridos(IDcarga, Recorrido_codigo)
                  DALiquidacion.XCargas_Recorridos_alta(new_IDcarga, Recorrido_codigo)
                  j = j + 1
                End While

                'ahora inserto en XCargasJunto
                DALiquidacion.XCargasJunto_insert(new_IDcarga, Cadena_Recorridos)
              End If
              k = k + 1
            End While
            'ahora elimino registros viejos.
            k = 0
            While k < DS_liqfinal.Tables("AUXILIAR1").Rows.Count
              Dim IDcarga As Integer = CInt(DS_liqfinal.Tables("AUXILIAR1").Rows(k).Item("Idcarga"))
              Dim ds_xcargas_recorridos As DataSet = DALiquidacion.XCargas_Recorridos_obtener(IDcarga)
              Dim id_aux As Integer = CInt(DS_liqfinal.Tables("AUXILIAR1").Rows(k).Item("id_aux"))
              'para xcargas_recorridos solo quito en base a Recorrido_codigo e idcarga.
              Dim filtro As String = "id_aux = " + CStr(id_aux)
              Dim rows() As DataRow = DS_liqfinal.Tables("AUXILIAR2").Select(filtro, "Recorrido_codigo")
              Dim dt_Auxiliar2_filtrado As DataTable = DS_liqfinal.Tables("AUXILIAR2").Clone
              For Each row As DataRow In rows
                dt_Auxiliar2_filtrado.ImportRow(row)
              Next
              'verifico si todos los recorridos corresponden a los modificados
              If ds_xcargas_recorridos.Tables(0).Rows.Count = CInt(DS_liqfinal.Tables("AUXILIAR1").Rows(k).Item("cont")) Then
                'entonces puedo eliminar y cargar todo
                DALiquidacion.Eliminar_XCargasYXCargasJunto(IDcarga)
              Else
                If ds_xcargas_recorridos.Tables(0).Rows.Count = 0 Then
                  DALiquidacion.Eliminar_XCargasYXCargasJunto(IDcarga)
                Else
                  'entonces no elimino, solo actualizo xcargas el TotalImporte x la diferencia de registros
                  Dim Importe As Decimal = ds_xcargas_recorridos.Tables(0).Rows(0).Item("Importe")
                  Dim TotalImporte As Decimal = 0
                  Dim Recorridos_reducidos As String = ""
                  Dim indc As Integer = 0
                  While indc < ds_xcargas_recorridos.Tables(0).Rows.Count
                    Dim ii As Integer = 0
                    Dim encontrado As String = "no"
                    While ii < dt_Auxiliar2_filtrado.Rows.Count
                      Dim Recorrido_codigo As String = dt_Auxiliar2_filtrado.Rows(ii).Item("Recorrido_codigo")
                      If ds_xcargas_recorridos.Tables(0).Rows(indc).Item("Recorrido_codigo") = Recorrido_codigo Then
                        'DALiquidacion.Eliminar_XCargas_Recorridos(IDcarga, Recorrido_codigo)
                        encontrado = "si"
                      Else
                        TotalImporte = TotalImporte + ds_xcargas_recorridos.Tables(0).Rows(indc).Item("TotalImporte")
                      End If
                      ii = ii + 1
                    End While
                    If encontrado = "si" Then
                      Recorridos_reducidos = Recorridos_reducidos + CStr(ds_xcargas_recorridos.Tables(0).Rows(indc).Item("Recorrido_codigo"))
                    End If
                    indc = indc + 1
                  End While
                  DALiquidacion.XCargasYXCargasJunto_actualizar(CStr(IDcarga), Recorridos_reducidos, CStr(TotalImporte).Replace(",", "."))
                End If
              End If
              Dim indice As Integer = 0
              While indice < dt_Auxiliar2_filtrado.Rows.Count
                Dim Recorrido_codigo As String = dt_Auxiliar2_filtrado.Rows(indice).Item("Recorrido_codigo")
                DALiquidacion.Eliminar_XCargas_Recorridos(IDcarga, Recorrido_codigo)
                indice = indice + 1
              End While
              k = k + 1
            End While
          End If
        End If
#End Region

      End If

    End If


  End Sub

  Private Sub Cubiertas_GeneralAOtroCliente()
    Dim DS_liqfinal As New DS_liqfinal
    'recuperar configuracion

    'Validamos si hay registros tipo 1 en gruposcub
    Dim valido = "no"
    Dim ds_cubinfo As DataSet = DAcubiertas.GruposCub_BuscarOp4()
    If ds_cubinfo.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_cubinfo.Tables(0).Rows.Count
        Dim UnaCifra As Decimal = 0
        Dim DosCifras As Decimal = 0
        Dim TresCifras As Decimal = 0
        Dim CuatroCifras As Decimal = 0
        Try
          UnaCifra = CDec(ds_cubinfo.Tables(0).Rows(i).Item("UnaCifra"))
        Catch ex As Exception
        End Try
        Try
          DosCifras = CDec(ds_cubinfo.Tables(0).Rows(i).Item("DosCifras"))
        Catch ex As Exception
        End Try
        Try
          TresCifras = CDec(ds_cubinfo.Tables(0).Rows(i).Item("TresCifras"))
        Catch ex As Exception
        End Try
        Try
          CuatroCifras = CDec(ds_cubinfo.Tables(0).Rows(i).Item("CuatroCifras"))
        Catch ex As Exception
        End Try
        Dim cifra_anular As Decimal = 99999.99
        If UnaCifra <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If DosCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If TresCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If CuatroCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        i = i + 1
      End While
    Else
      valido = "no"
    End If






    If valido = "si" Then

      If ds_cubinfo.Tables(0).Rows.Count <> 0 Then
        Dim Cliente_codigo As Integer = CInt(ds_cubinfo.Tables(1).Rows(0).Item("Clie_Codigo")) 'este codigo se va a insertar en cada registro que se requiera modificar.
        Dim UnaCifra As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("UnaCifra"))
        Dim DosCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("DosCifras"))
        Dim TresCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("TresCifras"))
        Dim CuatroCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("CuatroCifras"))
        Dim Consideracion As Integer = CInt(ds_cubinfo.Tables(0).Rows(0).Item("Consideracion"))
        'obtener todos los registros de 
        Dim ds_Xcargas As DataSet = DALiquidacion.Liquidacion_todoXcargas
        If ds_Xcargas.Tables(0).Rows.Count <> 0 Then
          Dim i As Integer = 0

          Dim DT_xcargas_copy As DataTable = ds_Xcargas.Tables(0).Clone 'copio solo la estructura
          DT_xcargas_copy.Columns.Add("ID_unir")

          While i < ds_Xcargas.Tables(0).Rows.Count
            Dim Recorrido As String = CStr(ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo").ToString).ToUpper
            Dim PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")
            Dim RecValido = ""
            Recorridos_Validar(RecValido, Recorrido)
            If RecValido = "no" Then 'si hay una Z o algun caracter q no sea un recorrido, se lo pasa x alto.
            Else
              Dim pid_valido = "si"
              Try

                Dim valid_PID As Integer = CInt(ds_Xcargas.Tables(0).Rows(i).Item("Pid"))

              Catch ex As Exception
                'si es un simbolo o letra se lo pasa x alto.
                pid_valido = "no"
              End Try
              If pid_valido = "si" Then
                Dim Suc As Integer = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
                Dim PID2 As String = ""
                Dim S2 As Integer = 0
                Dim R As Boolean = False
                Try
                  PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
                Catch ex As Exception
                End Try
                Try
                  S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
                Catch ex As Exception
                End Try
                Dim S2_validacion = ""
                Try
                  S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
                Catch ex As Exception
                  S2_validacion = "vacio"
                End Try


                Try
                  R = ds_Xcargas.Tables(0).Rows(i).Item("R")
                Catch ex As Exception
                End Try

                Select Case Consideracion
                  Case 0
                    If (Suc = 0 Or Suc = 1) And ((PID2 = "") Or (S2_validacion = "vacio") Or (R = False)) Then
                      Dim v_anular As Decimal = "99999,99"
                      Select Case Len(PID) 'devuelve cantidad de digitos en pid
                        Case 1
                          ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                          If UnaCifra <> v_anular Then
                            CubiertasGeneralAOtroCliente_recorrerYsumar(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                          End If
                        Case 2
                          If DosCifras <> v_anular Then
                            CubiertasGeneralAOtroCliente_recorrerYsumar(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                          End If
                        Case 3
                          If TresCifras <> v_anular Then
                            CubiertasGeneralAOtroCliente_recorrerYsumar(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                          End If
                        Case 4
                          If CuatroCifras <> v_anular Then
                            CubiertasGeneralAOtroCliente_recorrerYsumar(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                          End If
                      End Select
                    End If
                  Case 1 'se consideran todos los registros, sin importar el valor que tenga en SUC, y los demas datos en Pid2, Suc2 y R.
                    Dim v_anular As Decimal = "99999,99"
                    Select Case Len(PID) 'devuelve cantidad de digitos en pid
                      Case 1
                        ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                        If UnaCifra <> v_anular Then
                          CubiertasGeneralAOtroCliente_recorrerYsumar(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                        End If
                      Case 2
                        If DosCifras <> v_anular Then
                          CubiertasGeneralAOtroCliente_recorrerYsumar(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                        End If
                      Case 3
                        If TresCifras <> v_anular Then
                          CubiertasGeneralAOtroCliente_recorrerYsumar(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                        End If
                      Case 4
                        If CuatroCifras <> v_anular Then
                          CubiertasGeneralAOtroCliente_recorrerYsumar(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                        End If
                    End Select
                End Select

                'If (Consideracion = 0) And (PID <> "0" Or PID <> "1") And ((PID2 <> "") Or (S2 <> 0) Or (R = True)) Then
                '  'ignorar, pasar por alto.
                'Else
                '  Dim v_anular As Decimal = "99999,99"
                '  Select Case Len(PID) 'devuelve cantidad de digitos en pid
                '    Case 1
                '      ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                '      If UnaCifra <> v_anular Then
                '        CubiertasGeneralAOtroCliente_recorrerYsumar(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                '      End If
                '    Case 2
                '      If DosCifras <> v_anular Then
                '        CubiertasGeneralAOtroCliente_recorrerYsumar(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                '      End If
                '    Case 3
                '      If TresCifras <> v_anular Then
                '        CubiertasGeneralAOtroCliente_recorrerYsumar(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                '      End If
                '    Case 4
                '      If CuatroCifras <> v_anular Then
                '        CubiertasGeneralAOtroCliente_recorrerYsumar(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, DT_xcargas_copy)
                '      End If
                '  End Select
                'End If
              End If
            End If
            i = i + 1
          End While

          '------PRUEBA: AQUI AGREGO TODOS LOS REGISTROS DE UNA...Y AGRUPADOS.
          'If DT_xcargas_copy.Rows.Count <> 0 Then
          '  Dim jj As Integer = 0
          '  While jj < DT_xcargas_copy.Rows.Count
          '    'insertar 1 reg en xcargas
          '    Dim IDcarga As Integer = DT_xcargas_copy.Rows(jj).Item("IDcarga")
          '    Dim importe As Decimal = DT_xcargas_copy.Rows(jj).Item("Importe")
          '    Dim TotalImporte As Decimal = DT_xcargas_copy.Rows(jj).Item("TotalImporte")
          '    Dim Cliente As Integer = DT_xcargas_copy.Rows(jj).Item("Cliente")
          '    Dim ds_consulta As DataSet = DALiquidacion.CubGeneral_duplicar1(IDcarga, importe, TotalImporte, Cliente)
          '    Dim IDcarga_nueva As Integer = ds_consulta.Tables(0).Rows(0).Item("IDcarga")
          '    Dim ID_unir = DT_xcargas_copy.Rows(jj).Item("ID_unir")
          '    Dim ee As Integer = 0
          '    Dim Cadena_Cod_Recorridos As String = ""
          '    While ee < DS_liqfinal.Tables("CubGeneral_xcargas_recorridos").Rows.Count

          '      If ID_unir = DS_liqfinal.Tables("CubGeneral_xcargas_recorridos").Rows(ee).Item("ID_unir") Then
          '        Dim Reco As String = DS_liqfinal.Tables("CubGeneral_xcargas_recorridos").Rows(ee).Item("Recorrido_Codigo")
          '        'inserto 1 reg en xcargas_recorrido
          '        DALiquidacion.CubGeneral_duplicar1b(IDcarga_nueva, Reco)
          '        Cadena_Cod_Recorridos = Cadena_Cod_Recorridos + Reco
          '      End If
          '      ee = ee + 1
          '    End While
          '    'aqui lo agrego al registro en XcargasJunto
          '    DALiquidacion.CubGeneral_duplicarXcargasJunto(IDcarga_nueva, Cadena_Cod_Recorridos)
          '    jj = jj + 1
          '  End While


          'End If


          '-------------------------------------------------------------------



        End If
      End If

    End If

  End Sub

  Private Sub Cubiertas_PorGrupoAOtroClientexIMPTotal()

    'Validamos si hay registros tipo 1 en gruposcub
    Dim valido = "no"
    Dim ds_validar As DataSet = DAcubiertas.GruposCub_ObtenerOp3
    If ds_validar.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_validar.Tables(0).Rows.Count
        Dim UnaCifra As Decimal = 0
        Dim DosCifras As Decimal = 0
        Dim TresCifras As Decimal = 0
        Dim CuatroCifras As Decimal = 0
        Try
          UnaCifra = CDec(ds_validar.Tables(0).Rows(i).Item("UnaCifra"))
        Catch ex As Exception
        End Try
        Try
          DosCifras = CDec(ds_validar.Tables(0).Rows(i).Item("DosCifras"))
        Catch ex As Exception
        End Try
        Try
          TresCifras = CDec(ds_validar.Tables(0).Rows(i).Item("TresCifras"))
        Catch ex As Exception
        End Try
        Try
          CuatroCifras = CDec(ds_validar.Tables(0).Rows(i).Item("CuatroCifras"))
        Catch ex As Exception
        End Try
        Dim cifra_anular As Decimal = 99999.99
        If UnaCifra <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If DosCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If TresCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If CuatroCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        i = i + 1
      End While
    Else
      valido = "no"
    End If


    If valido = "si" Then

      'obtener todos los registros de Xcargas
      Dim ds_Xcargas As DataSet = DALiquidacion.Liquidacion_todoXcargas
      If ds_Xcargas.Tables(0).Rows.Count <> 0 Then
        Dim i As Integer = 0
        While i < ds_Xcargas.Tables(0).Rows.Count
          Dim Cliente As Integer = ds_Xcargas.Tables(0).Rows(i).Item("Cliente")
          Dim PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")
          Dim Recorrido As String = CStr(ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo").ToString).ToUpper
          'recupero info del cliente, necesito saber a que grupo pertenece.
          Dim DT_Clie As DataTable = DACliente.Clientes_buscar_grupo(CStr(Cliente))
          If DT_Clie.Rows.Count <> 0 Then
            Dim Grupo_id As Integer = DT_Clie.Rows(0).Item("Grupo_id")
            Dim Grupo_codigo As String = DT_Clie.Rows(0).Item("Grupos_Codigo")
            'consulto si tiene cubierta configurada.
            Dim ds_cubinfo As DataSet = DAcubiertas.GruposCub_BuscarOp3(Grupo_codigo)
            If ds_cubinfo.Tables(0).Rows.Count <> 0 Then
              Dim Cliente_codigo As Integer = CInt(ds_cubinfo.Tables(1).Rows(0).Item("Clie_Codigo")) 'este codigo se va a insertar en cada registro que se requiera duplicar.
              Dim UnaCifra As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("UnaCifra"))
              Dim DosCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("DosCifras"))
              Dim TresCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("TresCifras"))
              Dim CuatroCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("CuatroCifras"))
              Dim Consideracion As Integer = CInt(ds_cubinfo.Tables(0).Rows(0).Item("Consideracion"))
              Dim RecValido = ""
              Recorridos_Validar(RecValido, Recorrido)
              If RecValido = "no" Then 'si hay una Z o algun caracter q no sea un recorrido, se lo pasa x alto.
              Else
                Dim pid_valido = "si"
                Try

                  Dim valid_PID As Integer = CInt(ds_Xcargas.Tables(0).Rows(i).Item("Pid"))

                Catch ex As Exception
                  'si es un simbolo o letra se lo pasa x alto.
                  pid_valido = "no"
                End Try
                If pid_valido = "si" Then
                  Dim Suc As Integer = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
                  Dim PID2 As String = ""
                  Dim S2 As Integer = 0
                  Dim R As Boolean = False
                  Try
                    PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
                  Catch ex As Exception
                  End Try
                  Dim S2_validacion = ""
                  Try
                    S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
                  Catch ex As Exception
                    S2_validacion = "vacio"
                  End Try
                  Try
                    R = ds_Xcargas.Tables(0).Rows(i).Item("R")
                  Catch ex As Exception
                  End Try

                  Select Case Consideracion
                    Case 0
                      If (Suc = 0 Or Suc = 1) And ((PID2 = "") Or (S2_validacion = "vacio") Or (R = False)) Then
                        Dim v_anular As Decimal = "99999,99"
                        Select Case Len(PID) 'devuelve cantidad de digitos en pid
                          Case 1
                            ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                            If UnaCifra <> v_anular Then
                              Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                              If Importe > UnaCifra Then
                                'DUPLICO EL REGISTRO
                                DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                              End If
                            End If
                          Case 2
                            If DosCifras <> v_anular Then
                              Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                              If Importe > DosCifras Then
                                'DUPLICO EL REGISTRO
                                DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                              End If
                            End If
                          Case 3
                            If TresCifras <> v_anular Then
                              Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                              If Importe > TresCifras Then
                                'DUPLICO EL REGISTRO
                                DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                              End If
                            End If
                          Case 4
                            If CuatroCifras <> v_anular Then
                              Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                              If Importe > CuatroCifras Then
                                'DUPLICO EL REGISTRO
                                DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                              End If
                            End If
                        End Select
                      End If
                    Case 1 'se consideran todos los registros, sin importar el valor que tenga en SUC, y los demas datos en Pid2, Suc2 y R.
                      Dim v_anular As Decimal = "99999,99"
                      Select Case Len(PID) 'devuelve cantidad de digitos en pid
                        Case 1
                          ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                          If UnaCifra <> v_anular Then
                            Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                            If Importe > UnaCifra Then
                              'DUPLICO EL REGISTRO
                              DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                            End If
                          End If
                        Case 2
                          If DosCifras <> v_anular Then
                            Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                            If Importe > DosCifras Then
                              'DUPLICO EL REGISTRO
                              DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                            End If
                          End If
                        Case 3
                          If TresCifras <> v_anular Then
                            Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                            If Importe > TresCifras Then
                              'DUPLICO EL REGISTRO
                              DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                            End If
                          End If
                        Case 4
                          If CuatroCifras <> v_anular Then
                            Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                            If Importe > CuatroCifras Then
                              'DUPLICO EL REGISTRO
                              DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                            End If
                          End If
                      End Select
                  End Select

                  'If (Consideracion = 0) And (PID <> "0" Or PID <> "1") And ((PID2 <> "") Or (S2 <> 0) Or (R = True)) Then
                  '  'ignorar, pasar por alto.
                  'Else
                  '  Dim v_anular As Decimal = "99999,99"
                  '  Select Case Len(PID) 'devuelve cantidad de digitos en pid
                  '    Case 1
                  '      ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                  '      If UnaCifra <> v_anular Then
                  '        Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                  '        If Importe > UnaCifra Then
                  '          'DUPLICO EL REGISTRO
                  '          DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                  '        End If
                  '      End If
                  '    Case 2
                  '      If DosCifras <> v_anular Then
                  '        Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                  '        If Importe > DosCifras Then
                  '          'DUPLICO EL REGISTRO
                  '          DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                  '        End If
                  '      End If
                  '    Case 3
                  '      If TresCifras <> v_anular Then
                  '        Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                  '        If Importe > TresCifras Then
                  '          'DUPLICO EL REGISTRO
                  '          DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                  '        End If
                  '      End If
                  '    Case 4
                  '      If CuatroCifras <> v_anular Then
                  '        Dim Importe As Decimal = ds_Xcargas.Tables(0).Rows(i).Item("Importe")
                  '        If Importe > CuatroCifras Then
                  '          'DUPLICO EL REGISTRO
                  '          DALiquidacion.XCargas_duplicar(ds_Xcargas.Tables(0).Rows(i).Item("IDcarga"), Importe, ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo"), Cliente_codigo)
                  '        End If
                  '      End If
                  '  End Select
                  'End If
                End If
              End If
            End If
          Else
            'no existe el cliente
          End If
          i = i + 1
        End While
      End If

    End If


  End Sub

  Private Sub Cubiertas_PorGrupoAOtroCliente() 'EJECUTAR 3RO

    'Validamos si hay registros tipo 0 en gruposcub
    Dim valido = "no"
    Dim ds_validar As DataSet = DAcubiertas.GruposCub_ObtenerOp2
    If ds_validar.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_validar.Tables(0).Rows.Count
        Dim UnaCifra As Decimal = 0
        Dim DosCifras As Decimal = 0
        Dim TresCifras As Decimal = 0
        Dim CuatroCifras As Decimal = 0
        Try
          UnaCifra = CDec(ds_validar.Tables(0).Rows(i).Item("UnaCifra"))
        Catch ex As Exception
        End Try
        Try
          DosCifras = CDec(ds_validar.Tables(0).Rows(i).Item("DosCifras"))
        Catch ex As Exception
        End Try
        Try
          TresCifras = CDec(ds_validar.Tables(0).Rows(i).Item("TresCifras"))
        Catch ex As Exception
        End Try
        Try
          CuatroCifras = CDec(ds_validar.Tables(0).Rows(i).Item("CuatroCifras"))
        Catch ex As Exception
        End Try
        Dim cifra_anular As Decimal = 99999.99
        If UnaCifra <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If DosCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If TresCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        If CuatroCifras <> cifra_anular Then
          valido = "si"
          Exit While
        End If
        i = i + 1
      End While
    Else
      valido = "no"
    End If


    If valido = "si" Then
      Dim DS_liqfinal As New DS_liqfinal

      'obtener todos los registros de 
      Dim ds_Xcargas As DataSet = DALiquidacion.Liquidacion_todoXcargas
      If ds_Xcargas.Tables(0).Rows.Count <> 0 Then
        Dim i As Integer = 0
        While i < ds_Xcargas.Tables(0).Rows.Count
          Dim Cliente As Integer = ds_Xcargas.Tables(0).Rows(i).Item("Cliente")
          Dim PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")
          Dim Recorrido As String = CStr(ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo").ToString).ToUpper
          'recupero info del cliente, necesito saber a que grupo pertenece.
          Dim DT_Clie As DataTable = DACliente.Clientes_buscar_grupo(CStr(Cliente))
          If DT_Clie.Rows.Count <> 0 Then
            Dim Grupo_id As Integer = DT_Clie.Rows(0).Item("Grupo_id")
            Dim Grupo_codigo As String = DT_Clie.Rows(0).Item("Grupos_Codigo")
            'consulto si tiene cubierta configurada.
            Dim ds_cubinfo As DataSet = DAcubiertas.GruposCub_BuscarOp2(Grupo_codigo)
            If ds_cubinfo.Tables(0).Rows.Count <> 0 Then
              Dim Cliente_codigo As Integer = CInt(ds_cubinfo.Tables(1).Rows(0).Item("Clie_Codigo")) 'este codigo se va a insertar en cada registro que se requiera duplicar.
              Dim UnaCifra As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("UnaCifra"))
              Dim DosCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("DosCifras"))
              Dim TresCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("TresCifras"))
              Dim CuatroCifras As Decimal = CDec(ds_cubinfo.Tables(0).Rows(0).Item("CuatroCifras"))
              Dim Consideracion As Integer = CInt(ds_cubinfo.Tables(0).Rows(0).Item("Consideracion"))

              Dim RecValido = ""
              Recorridos_Validar(RecValido, Recorrido)
              If RecValido = "no" Then 'si hay una Z o algun caracter q no sea un recorrido, se lo pasa x alto.
              Else
                Dim pid_valido = "si"
                Try

                  Dim valid_PID As Integer = CInt(ds_Xcargas.Tables(0).Rows(i).Item("Pid"))

                Catch ex As Exception
                  'si es un simbolo o letra se lo pasa x alto.
                  pid_valido = "no"
                End Try
                If pid_valido = "si" Then
                  Dim Suc As Integer = ds_Xcargas.Tables(0).Rows(i).Item("Suc")
                  Dim PID2 As String = ""
                  Dim S2 As Integer = 0
                  Dim R As Boolean = False
                  Try
                    PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
                  Catch ex As Exception
                  End Try
                  Dim S2_validacion = ""
                  Try
                    S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
                  Catch ex As Exception
                    S2_validacion = "vacio"
                  End Try
                  Try
                    R = ds_Xcargas.Tables(0).Rows(i).Item("R")
                  Catch ex As Exception
                  End Try

                  Select Case Consideracion
                    Case 0
                      If (Suc = 0 Or Suc = 1) And ((PID2 = "") Or (S2_validacion = "vacio") Or (R = False)) Then
                        Dim v_anular As Decimal = "99999,99"
                        Select Case Len(PID) 'devuelve cantidad de digitos en pid
                          Case 1
                            ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                            If UnaCifra <> v_anular Then
                              Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                            End If
                          Case 2
                            If DosCifras <> v_anular Then
                              Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                            End If
                          Case 3
                            If TresCifras <> v_anular Then
                              Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                            End If
                          Case 4
                            If CuatroCifras <> v_anular Then
                              Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                            End If
                        End Select
                      End If
                    Case 1 'se consideran todos los registros, sin importar el valor que tenga en SUC, y los demas datos en Pid2, Suc2 y R.
                      Dim v_anular As Decimal = "99999,99"
                      Select Case Len(PID) 'devuelve cantidad de digitos en pid
                        Case 1
                          ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                          If UnaCifra <> v_anular Then
                            Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                          End If
                        Case 2
                          If DosCifras <> v_anular Then
                            Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                          End If
                        Case 3
                          If TresCifras <> v_anular Then
                            Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                          End If
                        Case 4
                          If CuatroCifras <> v_anular Then
                            Cubiertas_PorGrupoAOtroCliente_recorrerYsumar_B(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                          End If
                      End Select
                  End Select

                  'If (Consideracion = 0) And (PID <> "0" Or PID <> "1") And ((PID2 <> "") Or (S2 <> 0) Or (R = True)) Then
                  '  'ignorar, pasar por alto.
                  'Else
                  '  Dim v_anular As Decimal = "99999,99"
                  '  Select Case Len(PID) 'devuelve cantidad de digitos en pid
                  '    Case 1
                  '      ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                  '      If UnaCifra <> v_anular Then
                  '        Cubiertas_PorGrupoAOtroCliente_recorrerYsumar(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                  '      End If
                  '    Case 2
                  '      If DosCifras <> v_anular Then
                  '        Cubiertas_PorGrupoAOtroCliente_recorrerYsumar(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                  '      End If
                  '    Case 3
                  '      If TresCifras <> v_anular Then
                  '        Cubiertas_PorGrupoAOtroCliente_recorrerYsumar(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                  '      End If
                  '    Case 4
                  '      If CuatroCifras <> v_anular Then
                  '        Cubiertas_PorGrupoAOtroCliente_recorrerYsumar(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal, Grupo_id)
                  '      End If
                  '  End Select
                  'End If
                End If
              End If
            End If
          Else
            'no existe el cliente
          End If
          i = i + 1
        End While
      End If

    End If


  End Sub

  Private Sub Cubiertas_IndividualDivideEnElMismoCliente() 'EJECUTAR 4TO

    Dim DS_liqfinal As New DS_liqfinal

    'obtener todos los registros de 
    Dim ds_Xcargas As DataSet = DALiquidacion.Liquidacion_todoXcargas
    If ds_Xcargas.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_Xcargas.Tables(0).Rows.Count
        Dim Cliente As Integer = ds_Xcargas.Tables(0).Rows(i).Item("Cliente")
        Dim PID = ds_Xcargas.Tables(0).Rows(i).Item("Pid")
        Dim Recorrido As String = CStr(ds_Xcargas.Tables(0).Rows(i).Item("Recorrido_codigo").ToString).ToUpper
        'recupero info del cliente, necesito recuperar la configuracion de cubierta si es que posee.
        Dim Ds_Clie As DataSet = DAcubiertas.ClientesCub_BuscarCliente(CStr(Cliente))
        If Ds_Clie.Tables(0).Rows.Count <> 0 Then
          'Dim Grupo_id As Integer = DT_Clie.Rows(0).Item("Grupo_id")
          'Dim Grupo_codigo As String = DT_Clie.Rows(0).Item("Grupos_Codigo")
          'consulto si tiene cubierta configurada.
          Dim Cliente_codigo As Integer = Cliente 'este codigo se va a insertar en cada registro que se requiera duplicar.
          Dim UnaCifra As Decimal = CDec(Ds_Clie.Tables(0).Rows(0).Item("UnaCifra"))
          Dim DosCifras As Decimal = CDec(Ds_Clie.Tables(0).Rows(0).Item("DosCifras"))
          Dim TresCifras As Decimal = CDec(Ds_Clie.Tables(0).Rows(0).Item("TresCifras"))
          Dim CuatroCifras As Decimal = CDec(Ds_Clie.Tables(0).Rows(0).Item("CuatroCifras"))
          Dim Consideracion As Integer = CInt(Ds_Clie.Tables(0).Rows(0).Item("Consideracion"))

          Dim RecValido = ""
          Recorridos_Validar(RecValido, Recorrido)
          If RecValido = "no" Then 'si hay una Z o algun caracter q no sea un recorrido, se lo pasa x alto.
          Else
            Dim pid_valido = "si"
            Try

              Dim valid_PID As Integer = CInt(ds_Xcargas.Tables(0).Rows(i).Item("Pid"))

            Catch ex As Exception
              'si es un simbolo o letra se lo pasa x alto.
              pid_valido = "no"
            End Try
            If pid_valido = "si" Then

              Dim PID2 As String = ""
              Dim S2 As Integer = 0
              Dim R As Boolean = False
              Try
                PID2 = ds_Xcargas.Tables(0).Rows(i).Item("Pid2")
              Catch ex As Exception
              End Try
              Try
                S2 = ds_Xcargas.Tables(0).Rows(i).Item("Suc2")
              Catch ex As Exception
              End Try
              Try
                R = ds_Xcargas.Tables(0).Rows(i).Item("R")
              Catch ex As Exception
              End Try
              If (Consideracion = 0) And (PID <> "0" Or PID <> "1") And ((PID2 <> "") Or (S2 <> 0) Or (R = True)) Then 'Consideracion=0, solo considera Suc=0 o Suc=1, con S2,Pid2,R=false....si no se cumple esto, se ignora.
                'ignorar, pasar por alto.
              Else
                Dim v_anular As Decimal = "99999,99"
                Select Case Len(PID) 'devuelve cantidad de digitos en pid
                  Case 1
                    ' UnaCifra es = 99999.99, no se evalua esa cifra para el proceso.
                    If UnaCifra <> v_anular Then
                      Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar(UnaCifra, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal)
                    End If
                  Case 2
                    If DosCifras <> v_anular Then
                      Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar(DosCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal)
                    End If
                  Case 3
                    If TresCifras <> v_anular Then
                      Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar(TresCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal)
                    End If
                  Case 4
                    If CuatroCifras <> v_anular Then
                      Cubiertas_IndividualDivideEnElMismoCliente_recorrerYsumar(CuatroCifras, PID, Recorrido, ds_Xcargas, Consideracion, Cliente_codigo, ds_Xcargas.Tables(0).Rows(i), DS_liqfinal)
                    End If
                End Select
              End If
            End If
          End If

        Else
          'no existe el cliente
        End If
        i = i + 1
      End While
    End If
  End Sub

  Private Sub Recorridos_Validar(ByRef RecValido As String, ByVal Recorrido As String)
    Dim nro As Integer = 1
    If (Recorrido = "1A") Or (Recorrido = "1B") Or (Recorrido = "1C") Or (Recorrido = "1D") Or (Recorrido = "1E") Or (Recorrido = "1F") Or (Recorrido = "1G") Or (Recorrido = "1H") Or (Recorrido = "1I") Or (Recorrido = "1J") Then
      RecValido = "si"
    Else
      If (Recorrido = "2A") Or (Recorrido = "2B") Or (Recorrido = "2C") Or (Recorrido = "2D") Or (Recorrido = "2E") Or (Recorrido = "2F") Or (Recorrido = "2G") Or (Recorrido = "2H") Or (Recorrido = "2I") Or (Recorrido = "2J") Then
        RecValido = "si"
      Else
        If (Recorrido = "3A") Or (Recorrido = "3B") Or (Recorrido = "3C") Or (Recorrido = "3D") Or (Recorrido = "3E") Or (Recorrido = "3F") Or (Recorrido = "3G") Or (Recorrido = "3H") Or (Recorrido = "3I") Or (Recorrido = "3J") Then
          RecValido = "si"
        Else
          If (Recorrido = "4A") Or (Recorrido = "4B") Or (Recorrido = "4C") Or (Recorrido = "4D") Or (Recorrido = "4E") Or (Recorrido = "4F") Or (Recorrido = "4G") Or (Recorrido = "4H") Or (Recorrido = "4I") Or (Recorrido = "4J") Then
            RecValido = "si"
          Else
            If (Recorrido = "5A") Or (Recorrido = "5B") Or (Recorrido = "5C") Or (Recorrido = "5D") Or (Recorrido = "5E") Or (Recorrido = "5F") Or (Recorrido = "5G") Or (Recorrido = "5H") Or (Recorrido = "5I") Or (Recorrido = "5J") Then
              RecValido = "si"
            Else
              RecValido = "no"
            End If
          End If
        End If
      End If
    End If
  End Sub

  Private Sub metodo1()
    'TERMINALES ------ a) al ultimo registro voy a poner en 1(true) el campo terminales
    DAreliquidacion.Reliquidacion_TerminalesEstado(0, CInt(HF_parametro_id.Value))


    'IMPORTANTE SE CARGA DESDE CERO LA TABLA XCARGAS Y XCARGAS RECORRIDOS. FECHA: 22-08-04
    DALiquidacion.XCargas_load()

    'IMPORTANTE: SE GENERA UN BACKUP DE LA BASE DE DATOS ANTES DE LA LIQUIDACION.
    'NOTA: SE COMENTA DALiquidacion.BKP ya que este proc alm solo guarda un bkp con fecha y hora
    'DALiquidacion.BKP()


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
    Dim archivo As String = "C:\BKPWC\" + "WC_" + fecha_año + fecha_mes + fecha_dia + "A.bak"

    DAreliquidacion.Reliquidacion_DeleteBkp(archivo)
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


    '---------SE CREA EL BACKUP A DE WEBCENTRAL----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    'nota: se usara un backup de la bd previa a la liquidacion solo se almacenara la fecha y una letra al final...en este caso "A" para indicar que es una copia ANTERIOR a la liquidacion final
    DALiquidacion.BACKUP("A", CDate(HF_fecha.Value))
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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

    DAparametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal.aspx, Proc.Cubiertas") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.


    '///////////////////////////////CUBIERTAS - FECHA: 15-11-2022////////////////////////////////////
    'SETEADO_XCARGAS() 'aqui se separan los registros...1 registro x recorrido ya que estos datos se alteran y son sensibles a calculos en Cubiertas_individual...

    'SE DUPLICARAN REGISTROS EN XCARGAS Y XCARGAS RECORRIDOS SI EL CASO LO REQUIERE. 
    Cubiertas_GeneralAOtroCliente() '---OK
    Cubiertas_PorGrupoAOtroClientexIMPTotal() '---OK
    Cubiertas_PorGrupoAOtroCliente() '---OK
    'Cubiertas_IndividualDivideEnElMismoCliente()---YA NO LO USO....30-11-2022
    Cubiertas_IndividualDivideEnElMismoCliente_prueba1() '---OK

    '///////////////////////////////////////////////////////////////////////////////////////////////

    DAparametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal.aspx, ValidarXCargas") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.

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


      DAparametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal.aspx, Proc.Liquidacion") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.
      Liquidacion(DS_liqparcial, DS_liqfinal)

      'envio los parametros y tablas para generar el informe con los Totales Finales.
      Session("fecha_parametro") = HF_fecha.Value
      Session("tabla_recorridos_seleccionados") = DS_liqparcial.Tables("Recorridos_seleccionados")

      If DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Count <> 0 Then
        Dim SumPagos As Decimal = 0
        Dim SumCobros As Decimal = 0
        Dim SumReclamos As Decimal = 0
        Dim i As Integer = 0
        While i < DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Count
          Dim movimiento As String = DS_liqfinal.Tables("PagosCobrosReclamos").Rows(i).Item("Movimiento").ToString
          If movimiento = "PAGO" Then
            SumPagos = SumPagos + CDec(DS_liqfinal.Tables("PagosCobrosReclamos").Rows(i).Item("Importe"))
          End If
          If movimiento = "COBRO" Then
            SumCobros = SumCobros + CDec(DS_liqfinal.Tables("PagosCobrosReclamos").Rows(i).Item("Importe"))
          End If
          If movimiento = "RECLAMO" Then
            SumReclamos = SumReclamos + CDec(DS_liqfinal.Tables("PagosCobrosReclamos").Rows(i).Item("Importe"))
          End If
          i = i + 1
        End While
        DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add() 'fila en blanco


        SumPagos = (Math.Round(SumPagos, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
        SumCobros = (Math.Round(SumCobros, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
        SumReclamos = (Math.Round(SumReclamos, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
        Dim fila_p As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
        fila_p("Cliente") = ""
        fila_p("Movimiento") = "TOTAL PAGOS:"
        fila_p("Importe") = SumPagos
        DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_p)

        Dim fila_c As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
        fila_c("Cliente") = ""
        fila_c("Movimiento") = "TOTAL COBROS:"
        fila_c("Importe") = SumCobros
        DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_c)

        Dim fila_r As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
        fila_r("Cliente") = ""
        fila_r("Movimiento") = "TOTAL RECLAMOS:"
        fila_r("Importe") = SumReclamos
        DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_r)
      End If

      Session("tabla_PagosCobrosReclamos") = DS_liqfinal.Tables("PagosCobrosReclamos")

      Session("op_ingreso") = "si"
      Session("parametro_id") = HF_parametro_id.Value

      Response.Redirect("~/WC_LiquidacionFinal/LiquidacionFinal_TotalesFinales.aspx")

    Else
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ErrorValidacion", "$(document).ready(function () {$('#modal-ErrorValidacion').modal();});", True)
    End If
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
          '2 VALIDACION: que al menos tenga 1 punto para la zona habilitada
          Validar_recorridos_a(valido, codigo, codigo_error, check)

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

  Private Sub Liquidacion(ByRef DS_liqparcial As DataSet, ByRef DS_liqfinal As DataSet)
    'obtener_premios_x_clientes(DS_liqparcial)
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
    'ahora ordeno por Cliente ASC
    Dim rows() As DataRow = DS_XCARGAS1.Tables(0).Select("IDcarga > 0", "Cliente, Recorrido_codigo ASC")
    Dim dtTemp As DataTable = DS_XCARGAS1.Tables(0).Clone() 'copio la estructura de la tabla.
    For Each row As DataRow In rows
      ' Indicamos que el registro ha sido añadido
      row.SetAdded()
      dtTemp.ImportRow(row)
    Next
    'dtTemp tiene todos los registros de XCargas ya ordenas para poder continuar.
    '-------------fin paso 1------------------------------------------------------------------------------------------

    proceso_liquidacion(dtTemp, DS_liqfinal)


    'GridView2.DataSource = dtTemp
    'GridView2.DataBind()
  End Sub

  Private Sub proceso_liquidacion(ByVal dtTemp As DataTable, ByRef DS_liqfinal As DataSet)
    Dim DS_liqparcial1 As New DS_liqparcial
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






    '---------------PRIMERA ETAPA: PREMIOS--------------------------------
    'Dim DS_liqparcial1 As New DS_liqparcial
    'DS_liqparcial1.Tables("PremiosxClientes").Rows.Clear()

    'Dim valor11 = 1234
    'Dim undigito As String = valor11.ToString.Substring(3, 1)
    'Dim dosdigitos As String = valor11.ToString.Substring(2, 2)
    'Dim tresdigitos As String = valor11.ToString.Substring(1, 3)

    Dim i As Integer = 0
    'COMENTO SECCION VIEJA DE PREMIOS---------------------------------
    'While i <dtTemp.Rows.Count
    '  Dim XCargas_Suc = dtTemp.Rows(i).Item("Suc")
    '  Dim XCargas_Suc2 = 0
    '  Try
    '    XCargas_Suc2 = dtTemp.Rows(i).Item("Suc2") 'lo uso en la 3ra alternativa
    '  Catch ex As Exception
    '  End Try
    '  Dim XCargas_R As Boolean = False
    '  Try
    '    XCargas_R = dtTemp.Rows(i).Item("R") 'true or false
    '  Catch ex As Exception
    '  End Try

    '  If CDec(dtTemp.Rows(i).Item("Importe")) <> CDec(0) Then
    '    '--------------------------------------1) PRIMERA ALTERNATIVA-------------------------------------------------------------------------------
    '    If (XCargas_Suc = 0 Or XCargas_Suc = 1) And XCargas_R = False Then
    '      Dim XCargas_Pid = dtTemp.Rows(i).Item("Pid")
    '      Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
    '      Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
    '      Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
    '      Dim Puntos_P1 = ds_recorridos.Tables(0).Rows(0).Item("P1")

    '      Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
    '        Case 1
    '          'comparar con 1 digito en puntos_p1

    '          Select Case Len(Puntos_P1)
    '            Case 1
    '            'Puntos_P1 = Puntos_P1
    '            Case 2
    '              Puntos_P1 = Puntos_P1.ToString.Substring(1, 1)
    '            Case 3
    '              Puntos_P1 = Puntos_P1.ToString.Substring(2, 1)
    '            Case 4
    '              Puntos_P1 = Puntos_P1.ToString.Substring(3, 1)
    '          End Select
    '          If XCargas_Pid = Puntos_P1 Then

    '            Dim respuesta = "son iguales"
    '            grabar_premios_op1(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido)
    '          Else
    '            Dim respuesta = "no hay coincidencia"

    '          End If

    '        Case 2
    '          'comparar con 2 digito en puntos_p1
    '          Select Case Len(Puntos_P1)
    '            Case 1
    '            'Puntos_P1 = Puntos_P1
    '            Case 2
    '           'Puntos_P1
    '            Case 3
    '              Puntos_P1 = Puntos_P1.ToString.Substring(1, 2)
    '            Case 4
    '              Puntos_P1 = Puntos_P1.ToString.Substring(2, 2)
    '          End Select
    '          If XCargas_Pid = Puntos_P1 Then

    '            Dim respuesta = "son iguales"
    '            grabar_premios_op1(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido)
    '          Else
    '            Dim respuesta = "no hay coincidencia"

    '          End If
    '        Case 3
    '          'comparar con 3 digito en puntos_p1
    '          Select Case Len(Puntos_P1)
    '            Case 1
    '            'Puntos_P1 = Puntos_P1
    '            Case 2
    '           'Puntos_P1
    '            Case 3
    '            'Puntos_P1
    '            Case 4
    '              Puntos_P1 = Puntos_P1.ToString.Substring(1, 3)
    '          End Select
    '          If XCargas_Pid = Puntos_P1 Then

    '            Dim respuesta = "son iguales"
    '            grabar_premios_op1(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido)
    '          Else
    '            Dim respuesta = "no hay coincidencia"

    '          End If
    '        Case 4
    '          'comparar con 4 digito en puntos_p1
    '          If XCargas_Pid = Puntos_P1 Then

    '            Dim respuesta = "son iguales"
    '            grabar_premios_op1(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido)
    '          Else
    '            Dim respuesta = "no hay coincidencia"

    '          End If
    '      End Select
    '      '--------------------------------------1) FIN-------------------------------------------------------------------------------------------------

    '    Else
    '      If XCargas_Suc > 1 And XCargas_R = False Then
    '        '--------------------------------------2) SEGUNDA ALTERNATIVA-------------------------------------------------------------------------------
    '        Dim XCargas_Pid = dtTemp.Rows(i).Item("Pid")
    '        Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
    '        Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
    '        Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper

    '        Dim IndicePuntos As Integer = 7 '7 ES LA POSICION DE P1 EN LA CONSULTA.

    '        '16 ES P10

    '        Dim PtoLimite = CInt(XCargas_Suc) + 6 + 1 'mas 6 x que p1 empieza en la celda 7 Y mas uno porque para el limite uso un while con la condicion "menor"

    '        Dim ContCoincidencia As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid = PtoSelec"

    '        Dim i1 As Integer = 0
    '        While IndicePuntos < PtoLimite
    '          Dim PtoSelec = ds_recorridos.Tables(0).Rows(0).Item(IndicePuntos)
    '          Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
    '            Case 1
    '              'comparar con 1 digito en puntos_p1

    '              Select Case Len(PtoSelec)
    '                Case 1
    '            'Puntos_P1 = Puntos_P1
    '                Case 2
    '                  PtoSelec = PtoSelec.ToString.Substring(1, 1)
    '                Case 3
    '                  PtoSelec = PtoSelec.ToString.Substring(2, 1)
    '                Case 4
    '                  PtoSelec = PtoSelec.ToString.Substring(3, 1)
    '              End Select
    '              If XCargas_Pid = PtoSelec Then

    '                Dim respuesta = "son iguales"
    '                ContCoincidencia = ContCoincidencia + 1
    '              Else
    '                Dim respuesta = "no hay coincidencia"
    '              End If
    '            Case 2
    '              'comparar con 2 digito en puntos_p1
    '              Select Case Len(PtoSelec)
    '                Case 1
    '            'Puntos_P1 = Puntos_P1
    '                Case 2
    '           'Puntos_P1
    '                Case 3
    '                  PtoSelec = PtoSelec.ToString.Substring(1, 2)
    '                Case 4
    '                  PtoSelec = PtoSelec.ToString.Substring(2, 2)
    '              End Select
    '              If XCargas_Pid = PtoSelec Then
    '                Dim respuesta = "son iguales"
    '                ContCoincidencia = ContCoincidencia + 1
    '              Else
    '                Dim respuesta = "no hay coincidencia"
    '              End If
    '            Case 3
    '              'comparar con 3 digito en puntos_p1
    '              Select Case Len(PtoSelec)
    '                Case 1
    '            'Puntos_P1 = Puntos_P1
    '                Case 2
    '           'Puntos_P1
    '                Case 3
    '            'Puntos_P1
    '                Case 4
    '                  PtoSelec = PtoSelec.ToString.Substring(1, 3)
    '              End Select
    '              If XCargas_Pid = PtoSelec Then
    '                Dim respuesta = "son iguales"
    '                ContCoincidencia = ContCoincidencia + 1
    '              Else
    '                Dim respuesta = "no hay coincidencia"
    '              End If
    '            Case 4
    '              'comparar con 4 digito en puntos_p1
    '              If XCargas_Pid = PtoSelec Then
    '                Dim respuesta = "son iguales"
    '                ContCoincidencia = ContCoincidencia + 1
    '              Else
    '                Dim respuesta = "no hay coincidencia"
    '              End If
    '          End Select
    '          IndicePuntos = IndicePuntos + 1
    '          i1 = i1 + 1
    '        End While

    '        If ContCoincidencia <> 0 Then
    '          grabar_premios_op2(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido, ContCoincidencia)
    '        End If

    '      Else
    '        If (XCargas_Suc = 0 Or XCargas_Suc = 1) And XCargas_R = True Then
    '          '--------------------------------------3) TERCERA ALTERNATIVA-------------------------------------------------------------------------------


    '          Dim Cliente As String = CStr(dtTemp.Rows(i).Item("Cliente"))
    '          If Cliente = "627" Then
    '            Dim encontre = "si"
    '          End If


    '          Dim XCargas_Pid = dtTemp.Rows(i).Item("Pid")
    '          Dim XCargas_Pid2 As String = ""
    '          Try
    '            XCargas_Pid2 = dtTemp.Rows(i).Item("Pid2")
    '          Catch ex As Exception
    '          End Try
    '          Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
    '          Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
    '          Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
    '          Dim Puntos_P1 = ds_recorridos.Tables(0).Rows(0).Item("P1")
    '          Dim Coincidencia1 As String = "" 'pid = p1
    '          Dim Coincidencia2 As String = "" 'pid2 = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc2 + 1, excluyendo la coincidencia con el campo dbo.Puntos.P1)
    '          '1) primera validacion: pid=p1
    '          Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
    '            Case 2 'tener en cuenta que Pid va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.Puntos.P1
    '              'comparar con 2 digito en puntos_p1
    '              Select Case Len(Puntos_P1)
    '                Case 1
    '            'Puntos_P1 = Puntos_P1
    '                Case 2
    '           'Puntos_P1
    '                Case 3
    '                  Puntos_P1 = Puntos_P1.ToString.Substring(1, 2)
    '                Case 4
    '                  Puntos_P1 = Puntos_P1.ToString.Substring(2, 2)
    '              End Select
    '              If XCargas_Pid = Puntos_P1 Then
    '                Dim respuesta = "son iguales"
    '                Coincidencia1 = "si"
    '              Else
    '                Dim respuesta = "no hay coincidencia"
    '              End If
    '          End Select
    '          '2) segunda validacion: pid2 = 
    '          Dim IndicePuntos As Integer = 8 '8 ES LA POSICION DE P2 EN LA CONSULTA.
    '          Dim PtoLimite As Integer = 0
    '          If XCargas_Suc2 < 20 Then
    '            PtoLimite = XCargas_Suc2 + 1 'dbo.XCargasL.Suc2 + 1. NOTA: lo hago solo si es menor a 20...sino hay desbordamiento.."CONSULTAR ESTO"
    '          Else
    '            PtoLimite = XCargas_Suc2
    '          End If
    '          PtoLimite = PtoLimite + 6 + 1 'mas 6 x que p2 empieza en la celda 8 Y mas uno porque para el limite uso un while con la condicion "menor"
    '          Dim ContCoincidencia As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid2 = PtoSelec"

    '          Dim cont_items_recorridos As Integer = 1
    '          While IndicePuntos < PtoLimite

    '            If (cont_items_recorridos < CInt(XCargas_Suc2)) Then

    '              Dim PtoSelec = ds_recorridos.Tables(0).Rows(0).Item(IndicePuntos)
    '              Select Case Len(XCargas_Pid2) 'devuelve cantidad de digitos en pid2
    '                Case 2 'tener en cuenta que Pid2 va ha tener 2 digitos.
    '                  'comparar con 2 digito en puntos_p
    '                  Select Case Len(PtoSelec)
    '                    Case 1
    '            'Puntos_P1 = Puntos_P1
    '                    Case 2
    '           'Puntos_P1
    '                    Case 3
    '                      PtoSelec = PtoSelec.ToString.Substring(1, 2)
    '                    Case 4
    '                      PtoSelec = PtoSelec.ToString.Substring(2, 2)
    '                  End Select
    '                  If XCargas_Pid2 = PtoSelec Then
    '                    Dim respuesta = "son iguales"
    '                    Coincidencia2 = "si"
    '                    ContCoincidencia = ContCoincidencia + 1
    '                  Else
    '                    Dim respuesta = "no hay coincidencia"
    '                  End If

    '              End Select
    '              cont_items_recorridos = cont_items_recorridos + 1 'para controlar los elementos leidos en P1 a Pn
    '            End If

    '            IndicePuntos = IndicePuntos + 1
    '          End While
    '          If Coincidencia1 = "si" And Coincidencia2 = "si" Then
    '            grabar_premios_op3(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido, ContCoincidencia)

    '          End If
    '          '--------------------------------------3) TERCERA ALTERNATIVA (FIN)-------------------------------------------------------------------------------------------------
    '        Else
    '          If XCargas_Suc > 1 And XCargas_R = True Then
    '            '--------------------------------------4) CUARTA ALTERNATIVA-------------------------------------------------------------------------------
    '            Dim XCargas_Pid = dtTemp.Rows(i).Item("Pid")
    '            Dim XCargas_Pid2 As String = ""
    '            Try
    '              XCargas_Pid2 = dtTemp.Rows(i).Item("Pid2")
    '            Catch ex As Exception
    '            End Try
    '            Dim XCargas_recorridocodigo = dtTemp.Rows(i).Item("Recorrido_codigo")
    '            Dim ds_recorridos As DataSet = DALiquidacion.Liquidacion_validar_recorridos(HF_fecha.Value, XCargas_recorridocodigo)
    '            Dim referencia_recorrido As String = ds_recorridos.Tables(0).Rows(0).Item("Referencia").ToString.ToUpper
    '            Dim IndicePuntos As Integer = 7 '7 ES LA POSICION DE P1 EN LA CONSULTA.
    '            Dim PtoLimite As Integer = 0
    '            PtoLimite = XCargas_Suc + 6 + 1 'mas 6 x que p1 empieza en la celda 7 Y mas uno porque para el limite uso un while con la condicion "menor"
    '            '1)primera validacion: buscar hasta dbo.Puntos.P(el valor de dbo.XCargasL.Suc) que dbo.XCargasL.Pid = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc) 
    '            '(tener en cuenta que Pid va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc))
    '            Dim ContCoincidencia1 As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid = PtoSelec"
    '            While IndicePuntos < PtoLimite
    '              Dim PtoSelec = ds_recorridos.Tables(0).Rows(0).Item(IndicePuntos)
    '              Select Case Len(XCargas_Pid) 'devuelve cantidad de digitos en pid
    '                Case 2 'tener en cuenta que Pid va ha tener 2 digitos.
    '                  'comparar con 2 digito en puntos_p
    '                  Select Case Len(PtoSelec)
    '                    Case 1
    '                  'Puntos_P1 = Puntos_P1
    '                    Case 2
    '                  'Puntos_P1
    '                    Case 3
    '                      PtoSelec = PtoSelec.ToString.Substring(1, 2)
    '                    Case 4
    '                      PtoSelec = PtoSelec.ToString.Substring(2, 2)
    '                  End Select
    '                  If XCargas_Pid = PtoSelec Then
    '                    Dim respuesta = "son iguales"
    '                    ContCoincidencia1 = ContCoincidencia1 + 1
    '                  Else
    '                    Dim respuesta = "no hay coincidencia"
    '                  End If

    '              End Select
    '              IndicePuntos = IndicePuntos + 1
    '            End While
    '            '2)buscar hasta dbo.Puntos.P(el valor de dbo.XCargasL.Suc2) que dbo.XCargasL.Pid2 = dbo.Puntos.P(con algunos de los campos hasta el valor de dbo.XCargasL.Suc2)
    '            '(tener en cuenta que Pid2 va ha tener 2 digitos. Hay que comparar con la unidad y decena de dbo.PuntosP(con algunos de los campos hasta el valor de dbo.XcargasL.Suc2))
    '            IndicePuntos = 7 'reinicio a la posicion de p1
    '            PtoLimite = XCargas_Suc2 + 6 + 1 'cambio el PtoLimite hasta el "P" que me indice el campo Suc2
    '            Dim ContCoincidencia2 As Integer = 0 'cuento la cantidad de veces donde "XCargas_Pid2 = PtoSelec"
    '            While IndicePuntos < PtoLimite
    '              Dim PtoSelec = ds_recorridos.Tables(0).Rows(0).Item(IndicePuntos)
    '              Select Case Len(XCargas_Pid2) 'devuelve cantidad de digitos en pid2
    '                Case 2 'tener en cuenta que Pid2 va ha tener 2 digitos.
    '                  'comparar con 2 digito en puntos_p
    '                  Select Case Len(PtoSelec)
    '                    Case 1
    '            '     Puntos_P1 = Puntos_P1
    '                    Case 2
    '                  'Puntos_P1
    '                    Case 3
    '                      PtoSelec = PtoSelec.ToString.Substring(1, 2)
    '                    Case 4
    '                      PtoSelec = PtoSelec.ToString.Substring(2, 2)
    '                  End Select
    '                  If XCargas_Pid2 = PtoSelec Then
    '                    Dim respuesta = "son iguales"
    '                    ContCoincidencia2 = ContCoincidencia2 + 1
    '                  Else
    '                    Dim respuesta = "no hay coincidencia"
    '                  End If
    '              End Select
    '              IndicePuntos = IndicePuntos + 1
    '            End While

    '            '3) verifico si se dan ambas coincidencias
    '            If ContCoincidencia1 <> 0 And ContCoincidencia2 <> 0 Then
    '              grabar_premios_op4(DS_liqparcial1, dtTemp.Rows(i), referencia_recorrido, ContCoincidencia1, ContCoincidencia2)
    '            End If
    '            '--------------------------------------4) CUARTA ALTERNATIVA (FIN)-------------------------------------------------------------------------------------------------

    '          End If
    '        End If
    '      End If
    '    End If

    '  End If
    '  i = i + 1
    'End While

    'FINALIZO SECCION COMENTADA VIEJA DE PREMIOS--------------------------------

    'AQUI AGREGO LOS CORTES DE CONTROL ---------------------------------------------------------------------
    '----------COPIO LA ESTRUCTURA EN OTRO DATATABLE
    Dim dtTemp_con_cortes_control As DataTable = DS_liqparcial1.Tables("PremiosxClientes").Clone()

    i = 0
    Dim total_general As Decimal = 0
    While i < DS_liqparcial1.Tables("PremiosxClientes").Rows.Count
      total_general = total_general + CDec(DS_liqparcial1.Tables("PremiosxClientes").Rows(i).Item("Premio"))
      Dim Cliente = DS_liqparcial1.Tables("PremiosxClientes").Rows(i).Item("Cliente")

      If dtTemp_con_cortes_control.Rows.Count = 0 Then
        'agrego registro
        dtTemp_con_cortes_control.ImportRow(DS_liqparcial1.Tables("PremiosxClientes").Rows(i))

      Else
        Dim validar = ""
        Dim ultimo_registro As Integer = dtTemp_con_cortes_control.Rows.Count - 1
        If dtTemp_con_cortes_control.Rows(ultimo_registro).Item("Cliente") = Cliente Then
          'agrego
          dtTemp_con_cortes_control.ImportRow(DS_liqparcial1.Tables("PremiosxClientes").Rows(i))
        Else
          'si son diferentes primero hago el recuento de los premios para ese cliente y luego agrego un registro a modo de resumen.
          Dim j As Integer = 0
          Dim suma_premio As Decimal = 0
          Cliente = DS_liqparcial1.Tables("PremiosxClientes").Rows(i - 1).Item("Cliente")
          While j < dtTemp_con_cortes_control.Rows.Count
            If Cliente = dtTemp_con_cortes_control.Rows(j).Item("Cliente") Then
              suma_premio = suma_premio + CDec(dtTemp_con_cortes_control.Rows(j).Item("Premio"))
            End If
            j = j + 1
          End While
          Dim fila As DataRow = dtTemp_con_cortes_control.NewRow
          fila("Cliente") = "TOTAL"
          fila("Premio") = (Math.Round(suma_premio, 2).ToString("N2"))
          dtTemp_con_cortes_control.Rows.Add(fila)
          'ahora agrego el registro diferente, o sea nuevo cliente
          dtTemp_con_cortes_control.ImportRow(DS_liqparcial1.Tables("PremiosxClientes").Rows(i))
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
    'GridView1.DataSource = dtTemp_con_cortes_control
    'GridView1.DataBind()


    '-------------------------------------------------------------------------------------------------------

    'SEGUNDA ETAPA OPTIMIZADA---REDUCIMOS LOS CICLOS EN LA BUSQUEDA.
    Dim DS_ClientesEnXcargas As DataSet = DArecorrido.XCargasJunto_obtenerclientes(HF_fecha.Value)
    If DS_ClientesEnXcargas.Tables(0).Rows.Count Then
      i = 0
      While i < DS_ClientesEnXcargas.Tables(0).Rows.Count
        'recupero registros totalizados para 1 cliente en xcargas y otras tablas extras.
        Dim ds_info As DataSet = DArecorrido.XCargasJunto_infoclienteyrecaudacion(HF_fecha.Value, CStr(DS_ClientesEnXcargas.Tables(0).Rows(i).Item("Cliente")))
        If (ds_info.Tables(1).Rows.Count <> 0) Or (ds_info.Tables(2).Rows.Count <> 0) Then
          Dim Grupo_id As Integer = ds_info.Tables(0).Rows(0).Item("Grupo_id")
          Dim Codigo_cliente As String = ds_info.Tables(0).Rows(0).Item("Codigo")
          Dim SaldoAnterior As Decimal = ds_info.Tables(0).Rows(0).Item("Cliente_Saldo")
          Dim recaudacion As Decimal = 0
          Dim recaudacionSC As Decimal = 0
          'operacion: Recaudacion = a la suma de todos los importes de la tabla dbo.XCargasL.TotalImporte donde dbo.XCargasL.Sincomputo = False 
          Try
            recaudacion = ds_info.Tables(1).Rows(0).Item("TotalImporte")
          Catch ex As Exception
          End Try
          'operacion: RecaudacionSC = a la suma de todos los importes de la tabla dbo.XCargasL.TotalImporte donde dbo.XCargasL.Sincomputo = True 
          Try
            recaudacionSC = ds_info.Tables(2).Rows(0).Item("TotalImporte")
          Catch ex As Exception
          End Try
          recaudacion = (Math.Round(recaudacion, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          recaudacionSC = (Math.Round(recaudacionSC, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento

          'calculo comision y comisionSC---------------------------------------------------------------------------------------------------------------------------------------
          'operacion: Comision = al calculo del porcentaje del total de Recaudacion (el porcentaje se obtiene de la tabla dbo.Clientes.Comision) del cliente.
          Dim cliente_comision As Decimal = ds_info.Tables(0).Rows(0).Item("Cliente_Comision")
          Dim comision As Decimal = (recaudacion * cliente_comision) / 100
          comision = (Math.Round(comision, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          'operacion: ComisionSC = al calculo del porcentaje del total de Recaudacion (el porcentaje se obtiene de la tabla dbo.Clientes.Comision) del cliente.
          Dim comisionSC As Decimal = (recaudacionSC * cliente_comision) / 100
          '--------------------------------------------------------------------------------------------------------------------------------------------------------------------

          'calculo premios-----------------------------------------------------------------------------------------------------------------------------------------------------
          'operacion: Premios = a la suma de todos los importes de la tabla dbo.Premios.Premio del cliente, donde dbo.Premios.Sincomputo = False
          'operacion 2: PremiosSC = a la suma de todos los importes de la tabla dbo.Premios.Premio del cliente, donde dbo.Premios.Sincomputo = True
          Dim ds_premios As DataSet = DAPremios.Premios_ClienteobtenerXfecha(HF_fecha.Value, Codigo_cliente)
          Dim Premios As Decimal = 0
          Dim PremiosSC As Decimal = 0
          Dim jj As Integer = 0
          While jj < ds_premios.Tables(0).Rows.Count
            Dim SinComputo As Boolean = False
            Try
              SinComputo = ds_premios.Tables(0).Rows(jj).Item("Sincomputo")
            Catch ex As Exception

            End Try
            If SinComputo = False Then
              Premios = Premios + ds_premios.Tables(0).Rows(jj).Item("Premio")
            End If
            If SinComputo = True Then
              PremiosSC = PremiosSC + ds_premios.Tables(0).Rows(jj).Item("Premio")
            End If
            jj = jj + 1
          End While
          Premios = (Math.Round(Premios, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          '---------------------------------------------------------------------------------------------------------------------------------------------------------------------

          'calculo Reclamos y ReclamosSC-----------------------------------------------------------------------------------------------------------------------------------------------------
          'operacion: Reclamos = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente,
          'donde dbo.Anticipados.Sincalculo = False y dbo.Anticipados.Origen = False y dbo.Anticipados.Tipo = 1.
          'operacion2: ReclamosSC = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente, donde dbo.Anticipados.Sincalculo = True y dbo.Anticipados.Origen = False y dbo.Anticipados.Tipo = 1
          'operacion3: dbo.CtaCte.ReclamosB = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente, donde dbo.Anticipados.Origen = True y dbo.Anticipados.Tipo = 1 
          Dim Reclamos As Decimal = 0
          Dim ReclamosSC As Decimal = 0
          Dim ReclamosB As Decimal = 0
          Dim ds_anticipados As DataSet = DAAnticipados.Anticipados_ClienteobtenerXfecha(HF_fecha.Value, Codigo_cliente)
          'operacon4: Cobros = a la suma de todos los importes de la tabla dbo.Anticpados.Importe del cliente, donde dbo.Anticipados.Tipo = 2
          'operacion5: Pagos = a la suma de todos los importes de la tabla dbo.Anticpados.Importe del cliente, donde dbo.Anticipados.Tipo = 3
          Dim Cobros As Decimal = 0
          Dim Pagos As Decimal = 0
          Dim ii As Integer = 0
          While ii < ds_anticipados.Tables(0).Rows.Count
            If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 1 Then
              If (ds_anticipados.Tables(0).Rows(ii).Item("Sincalculo") = False) And (ds_anticipados.Tables(0).Rows(ii).Item("Origen") = False) Then
                Reclamos = Reclamos + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
              End If
              If (ds_anticipados.Tables(0).Rows(ii).Item("Sincalculo") = True) And (ds_anticipados.Tables(0).Rows(ii).Item("Origen") = False) Then
                ReclamosSC = ReclamosSC + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
              End If
              'If ds_anticipados.Tables(0).Rows(ii).Item("Origen") = True Then
              '  ReclamosB = ReclamosB + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
              'End If
            End If
            If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 2 Then
              Cobros = Cobros + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
            End If
            If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 3 Then
              Pagos = Pagos + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
            End If
            ii = ii + 1
          End While
          Reclamos = (Math.Round(Reclamos, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          ReclamosSC = (Math.Round(ReclamosSC, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          ReclamosB = (Math.Round(ReclamosB, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          Cobros = (Math.Round(Cobros, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          Pagos = (Math.Round(Pagos, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          '--------------------------------------------------------------------------------------------------------------------------------------------------------------------

          'calculo DejoGano y DejoGanoSC----------------------------------------------------------------------------------------------------------------------------------------------------
          'operacion: DejoGano = dbo.CtaCte.Recaudacion - dbo.CtaCte.Comision - dbo.CtaCte.Premios - dbo.CtaCte.Reclamos
          Dim DejoGano As Decimal = recaudacion - comision - Premios - Reclamos
          DejoGano = (Math.Round(DejoGano, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          'operacion2: DejoGanoSC = dbo.CtaCte.RecaudacionSC - dbo.CtaCte.ComisionSC - dbo.CtaCte.PremiosSC - dbo.CtaCte.ReclamosSC
          Dim DejoGanoSC As Decimal = recaudacionSC - comisionSC - PremiosSC - ReclamosSC
          DejoGanoSC = (Math.Round(DejoGanoSC, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
          'operacion3: DejoGanoB = dbo.CtaCte.RecaudacionB - dbo.CtaCte.ComisionB - dbo.CtaCte.PremiosB - dbo.CtaCte.ReclamosB
          Dim RecaudacionB = 0
          Dim ComisionB = 0
          Dim PremiosB = 0
          Dim DejoGanoB As Decimal = RecaudacionB - ComisionB - PremiosB - ReclamosB
          '--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


          '--------------------------------------------------------------------------------------------------------------------------------------------------
          'dbo.CtaCte.Prestamo = a la suma de todos los prestamos dados de alta para la fecha (dbo.PrestamosCreditos.Saldo)
          'dbo.CtaCte.Credito = a la suma de todos los creditos dados de alta para la fecha (dbo.PrestamosCreditos.Saldo)
          Dim Prestamo As Decimal = 0
          Dim Credito As Decimal = 0

          Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(Codigo_cliente)
          Dim Cliente_id As Integer = ds_clie.Tables(0).Rows(0).Item("Cliente")

          'recupero primero todos los prestamos donde fecha= fecha_liquidacion
          Dim ds_prescred As DataSet = DALiquidacion.LiquidacionFinal_obtener_prestamoscreditos(CDate(HF_fecha.Value), Cliente_id)
          Dim k As Integer = 0
          While k < ds_prescred.Tables(0).Rows.Count
            Select Case ds_prescred.Tables(0).Rows(k).Item("Tipo")
              Case "P"
                Prestamo = Prestamo + CDec(ds_prescred.Tables(0).Rows(k).Item("Saldo"))
              Case "C"
                Credito = Credito + CDec(ds_prescred.Tables(0).Rows(k).Item("Saldo"))
            End Select
            k = k + 1
          End While

          '--------------------------------------------------------------------------------------------------------------------------------------------------

          '-------aqui guardo en bd-----
          DACtaCte.CtaCte_alta(Grupo_id, CInt(Codigo_cliente), HF_fecha.Value, SaldoAnterior, recaudacion, comision, Premios, Reclamos, DejoGano,
                        recaudacionSC, comisionSC, PremiosSC, ReclamosSC, DejoGanoSC,
                        RecaudacionB, ComisionB, PremiosB, ReclamosB, DejoGanoB, Cobros, Pagos, Prestamo, Credito)
          '---------fin--------------


          '//////////////////////////////EN ESTA SECCION AGREGO UN REGISTRO POR CADA MOVIMIENTO DEL CLIENTE: PAGOS, COBROS, RECLAMOS/////////////////
          'valido si alguno de esos parametros es distinto de 0 lo agrego.
          If Pagos <> CDec(0) Then
            Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
            fila_mov("Cliente") = Codigo_cliente
            fila_mov("Movimiento") = "PAGO"
            fila_mov("Importe") = CDec(Pagos)
            DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
          End If
          If Cobros <> CDec(0) Then
            Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
            fila_mov("Cliente") = Codigo_cliente
            fila_mov("Movimiento") = "COBRO"
            fila_mov("Importe") = CDec(Cobros)
            DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
          End If
          Dim SumReclamos As Decimal = Reclamos + ReclamosSC + ReclamosB
          If SumReclamos <> CDec(0) Then
            Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
            fila_mov("Cliente") = Codigo_cliente
            fila_mov("Movimiento") = "RECLAMO"
            fila_mov("Importe") = CDec(SumReclamos)
            DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
          End If
          '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

          '-------------TERCERA ETAPA: Actualizacion de Saldo y SaldoRegaldo por cada cliente que tuvo movimento en la fecha del parametro a liquidar.------------------------------------------------------------

          'ACTUALIZACION DE SALDO------------
          'dbo.Clientes.SaldoAnterior = dbo.Clientes.Saldo
          Dim Clie_Saldo As Decimal = ds_info.Tables(0).Rows(0).Item("Cliente_Saldo")
          Dim Clie_SaldoAnterior As Decimal = Clie_Saldo
          'dbo.Clientes.Saldo = dbo.Clientes.Saldo + dbo.CtaCteRecaudacion + dbo.CtaCteRecaudacionSC + dbo.CtaCteRecaudacionB - dbo.CtaCte.Comision - dbo.CtaCte.ComisionSC - dbo.CtaCte.ComisionB - dbo.CtaCte.Premios - dbo.CtaCte.PremiosSC - dbo.CtaCte.PremiosB - dbo.CtaCte.Reclamos - dbo.CtaCte.ReclamosSC - dbo.CtaCte.ReclamosB - dbo.CtaCte.Cobros + dbo.CtaCte.Pagos + dbo.CtaCte.CobPrestamo + dbo.CtaCte.CobCredito + dbo.Ctacte.Prestamo + dbo.Ctacte.Credito
          Clie_Saldo = Clie_Saldo + recaudacion + recaudacionSC + RecaudacionB - comision - comisionSC - ComisionB - Premios - PremiosSC - PremiosB - Reclamos - ReclamosSC - ReclamosB - Cobros + Pagos + 0 + 0 - Prestamo + Credito 'NOTA 2022-01-06 SE RESTA EL PARAMETRO PRESTAMO

          '---aqui guardo en bd -----
          DACliente.Clientes_ActualizarSaldo(Codigo_cliente, Clie_SaldoAnterior, Clie_Saldo)
          '--------------------------
          '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
          'FECHA: 2022-12-29.
          'NOTA: GUARDAR EN CTACTE.SALIDA EL TOTAL DEL SALDO FINAL CUANDO EL CLIENTE GANA, ES DECIR CUANDO EL MONTO ES NEGATIVO.
          If Clie_Saldo < CDec(0) Then
            'recupero el id de la ctacta para la fecha a liquidar.
            Dim DS_CTACTE As DataSet = DACtaCte.CtaCte_obtener(CInt(Codigo_cliente), HF_fecha.Value)
            If DS_CTACTE.Tables(0).Rows.Count <> 0 Then
              Dim IdCtaCte As Integer = CInt(DS_CTACTE.Tables(0).Rows(0).Item("IdCtaCte"))
              DACtaCte.CtaCte_ActualizarSalida(CInt(Codigo_cliente), HF_fecha.Value, CDec(Clie_Saldo))
            End If
          End If
          '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


          'ACTUALIZACION DE SALDO REGALO--------------
          'Operacion: dbo.Clientes.SaldoRegalo = dbo.Clientes.SaldoRegalo + ((dbo.CtaCte.Recaudacion - dbo.CtaCte.Comision - dbo.CtaCte.Premios - dbo.CtaCte.Reclamos) * dbo.Clientes.Regalo)
          Dim Clie_Regalo As Decimal = ds_info.Tables(0).Rows(0).Item("Cliente_Regalo")
          Dim SaldoRegalo As Decimal = ds_info.Tables(0).Rows(0).Item("Cliente_SaldoRegalo")
          SaldoRegalo = SaldoRegalo + (((recaudacion - comision - Premios - Reclamos) / 100) * Clie_Regalo * -1)

          '---aqui guardo en bd----
          DACliente.Clientes_ActualizarSaldoRegalo(Codigo_cliente, SaldoRegalo)
          '------------------------

          ''---------------FECHA: 2022-11-11--------------SE AGREGA TABLA CONFIGURACION----------------------------------------------------------
          'Dim SaldosACero As Integer = 0 'de momento hay que considerar que el dbo.Configuracion.SaldosACero = 0. 
          'Try
          '  Dim ds_config As DataSet = DAconfiguracion.Configuracion_obtenertodo
          '  SaldosACero = CInt(ds_config.Tables(0).Rows(0).Item("SaldosACero"))
          'Catch ex As Exception
          'End Try
          'If SaldosACero = 1 Then
          '  'recupero info del cliente.
          '  Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(Codigo_cliente)
          '  Dim Saldo As Decimal = CDec(ds_clie.Tables(0).Rows(0).Item("Saldo"))
          '  If Saldo < 0 Then 'si el saldo es negativo el Cliente Gana.
          '    DACtaCte.CtaCte_ActualizarSalida(Codigo_cliente, CDate(HF_fecha.Value), Saldo) 'Guardar en el campo dbo.CtaCte.Salida el saldo final del cliente cuando el cliente gana, 
          '    DACliente.Clientes_ActualizarSaldo(Codigo_cliente, Saldo, CDec(0)) 'y poner en 0 el saldo del cliente dbo.Clientes.Saldo = 0
          '  End If
          'End If
          ''------------------------------------------------------------------------------------------------------------------------------------



        End If


        i = i + 1
      End While

    End If


    Dim saltar As String = ""

    If saltar = "no entrar aqui" Then

      '------------SEGUNDA ETAPA: CALCULO Y GRABACION EN LA TABLA CTACTE------------------------------------------------
      'NOTA: ds_xcargas recupera todos los registros de la fecha del parametro, ordenados por cliente ASC
      Dim ds_xcargas As DataSet = DALiquidacion.Liquidacion_final_recuperarXcargas(HF_fecha.Value)
      If ds_xcargas.Tables(0).Rows.Count <> 0 Then
        Dim Fecha As Date = HF_fecha.Value
        i = 0
        Dim cliente_agregado = ""
        While i < ds_xcargas.Tables(0).Rows.Count
          Dim Grupo_id As Integer = ds_xcargas.Tables(0).Rows(i).Item("Grupo_id")
          Dim Codigo_cliente As String = ds_xcargas.Tables(0).Rows(i).Item("Cliente")
          Dim SaldoAnterior As Decimal = ds_xcargas.Tables(0).Rows(i).Item("Cliente_Saldo")

          '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
          'AQUI VALIDO QUE EL REGISTRO NO TENGA EN RECORRIDO Z..SI ES ASI SE LO IGNORA EN ESTA ETAPA
          Dim IDcarga As Integer = ds_xcargas.Tables(0).Rows(i).Item("IDcarga")
          Dim ds_regZ As DataSet = DALiquidacion.LiquidacionFinal_validarZ(CStr(IDcarga))
          Dim recorridoZ As String = ""
          Try
            recorridoZ = CStr(ds_regZ.Tables(0).Rows(0).Item("Recorrido")).ToUpper
          Catch ex As Exception
          End Try
          '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
          If recorridoZ <> "Z" Then
            Dim recaudacion As Decimal = 0
            Dim recaudacionSC As Decimal = 0
            If cliente_agregado <> Codigo_cliente Then
              Dim j As Integer = 0
              While j < ds_xcargas.Tables(0).Rows.Count
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                'AQUI VALIDO QUE EL REGISTRO NO TENGA EN RECORRIDO Z..SI ES ASI SE LO IGNORA EN ESTA ETAPA
                IDcarga = ds_xcargas.Tables(0).Rows(j).Item("IDcarga")
                Dim dsa_regZ As DataSet = DALiquidacion.LiquidacionFinal_validarZ(CStr(IDcarga))
                recorridoZ = ""
                Try
                  recorridoZ = CStr(dsa_regZ.Tables(0).Rows(0).Item("Recorrido")).ToUpper
                Catch ex As Exception
                End Try
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                If recorridoZ <> "Z" Then
                  Dim SinComputo As Boolean = False
                  Try
                    SinComputo = ds_xcargas.Tables(0).Rows(j).Item("SinComputo")
                  Catch ex As Exception
                  End Try
                  'operacion: Recaudacion = a la suma de todos los importes de la tabla dbo.XCargasL.TotalImporte donde dbo.XCargasL.Sincomputo = False 
                  If (Codigo_cliente = ds_xcargas.Tables(0).Rows(j).Item("Cliente")) And (SinComputo = False) Then
                    recaudacion = recaudacion + ds_xcargas.Tables(0).Rows(j).Item("TotalImporte")
                  End If
                  'operacion: RecaudacionSC = a la suma de todos los importes de la tabla dbo.XCargasL.TotalImporte donde dbo.XCargasL.Sincomputo = True 
                  If (Codigo_cliente = ds_xcargas.Tables(0).Rows(j).Item("Cliente")) And (SinComputo = True) Then
                    recaudacionSC = recaudacionSC + ds_xcargas.Tables(0).Rows(j).Item("TotalImporte")
                  End If
                End If
                j = j + 1
              End While
              recaudacion = (Math.Round(recaudacion, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              recaudacionSC = (Math.Round(recaudacionSC, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento


              'calculo comision y comisionSC---------------------------------------------------------------------------------------------------------------------------------------
              'operacion: Comision = al calculo del porcentaje del total de Recaudacion (el porcentaje se obtiene de la tabla dbo.Clientes.Comision) del cliente.
              Dim cliente_comision As Decimal = ds_xcargas.Tables(0).Rows(i).Item("Cliente_Comision")
              Dim comision As Decimal = (recaudacion * cliente_comision) / 100
              comision = (Math.Round(comision, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              'operacion: ComisionSC = al calculo del porcentaje del total de Recaudacion (el porcentaje se obtiene de la tabla dbo.Clientes.Comision) del cliente.
              Dim comisionSC As Decimal = (recaudacionSC * cliente_comision) / 100
              '--------------------------------------------------------------------------------------------------------------------------------------------------------------------

              'calculo premios-----------------------------------------------------------------------------------------------------------------------------------------------------
              'operacion: Premios = a la suma de todos los importes de la tabla dbo.Premios.Premio del cliente, donde dbo.Premios.Sincomputo = False
              'operacion 2: PremiosSC = a la suma de todos los importes de la tabla dbo.Premios.Premio del cliente, donde dbo.Premios.Sincomputo = True
              Dim ds_premios As DataSet = DAPremios.Premios_ClienteobtenerXfecha(HF_fecha.Value, Codigo_cliente)
              Dim Premios As Decimal = 0
              Dim PremiosSC As Decimal = 0
              Dim jj As Integer = 0
              While jj < ds_premios.Tables(0).Rows.Count
                Dim SinComputo As Boolean = False
                Try
                  SinComputo = ds_premios.Tables(0).Rows(jj).Item("Sincomputo")
                Catch ex As Exception

                End Try
                If SinComputo = False Then
                  Premios = Premios + ds_premios.Tables(0).Rows(jj).Item("Premio")
                End If
                If SinComputo = True Then
                  PremiosSC = PremiosSC + ds_premios.Tables(0).Rows(jj).Item("Premio")
                End If
                jj = jj + 1
              End While
              Premios = (Math.Round(Premios, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              '---------------------------------------------------------------------------------------------------------------------------------------------------------------------

              'calculo Reclamos y ReclamosSC-----------------------------------------------------------------------------------------------------------------------------------------------------
              'operacion: Reclamos = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente,
              'donde dbo.Anticipados.Sincalculo = False y dbo.Anticipados.Origen = False y dbo.Anticipados.Tipo = 1.
              'operacion2: ReclamosSC = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente, donde dbo.Anticipados.Sincalculo = True y dbo.Anticipados.Origen = False y dbo.Anticipados.Tipo = 1
              'operacion3: dbo.CtaCte.ReclamosB = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente, donde dbo.Anticipados.Origen = True y dbo.Anticipados.Tipo = 1 
              Dim Reclamos As Decimal = 0
              Dim ReclamosSC As Decimal = 0
              Dim ReclamosB As Decimal = 0
              Dim ds_anticipados As DataSet = DAAnticipados.Anticipados_ClienteobtenerXfecha(HF_fecha.Value, Codigo_cliente)
              'operacon4: Cobros = a la suma de todos los importes de la tabla dbo.Anticpados.Importe del cliente, donde dbo.Anticipados.Tipo = 2
              'operacion5: Pagos = a la suma de todos los importes de la tabla dbo.Anticpados.Importe del cliente, donde dbo.Anticipados.Tipo = 3
              Dim Cobros As Decimal = 0
              Dim Pagos As Decimal = 0
              Dim ii As Integer = 0
              While ii < ds_anticipados.Tables(0).Rows.Count
                If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 1 Then
                  If (ds_anticipados.Tables(0).Rows(ii).Item("Sincalculo") = False) And (ds_anticipados.Tables(0).Rows(ii).Item("Origen") = False) Then
                    Reclamos = Reclamos + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
                  End If
                  If (ds_anticipados.Tables(0).Rows(ii).Item("Sincalculo") = True) And (ds_anticipados.Tables(0).Rows(ii).Item("Origen") = False) Then
                    ReclamosSC = ReclamosSC + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
                  End If
                  'If ds_anticipados.Tables(0).Rows(ii).Item("Origen") = True Then
                  '  ReclamosB = ReclamosB + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
                  'End If
                End If
                If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 2 Then
                  Cobros = Cobros + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
                End If
                If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 3 Then
                  Pagos = Pagos + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
                End If
                ii = ii + 1
              End While
              Reclamos = (Math.Round(Reclamos, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              ReclamosSC = (Math.Round(ReclamosSC, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              ReclamosB = (Math.Round(ReclamosB, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              Cobros = (Math.Round(Cobros, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              Pagos = (Math.Round(Pagos, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              '--------------------------------------------------------------------------------------------------------------------------------------------------------------------

              'calculo DejoGano y DejoGanoSC----------------------------------------------------------------------------------------------------------------------------------------------------
              'operacion: DejoGano = dbo.CtaCte.Recaudacion - dbo.CtaCte.Comision - dbo.CtaCte.Premios - dbo.CtaCte.Reclamos
              Dim DejoGano As Decimal = recaudacion - comision - Premios - Reclamos
              DejoGano = (Math.Round(DejoGano, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              'operacion2: DejoGanoSC = dbo.CtaCte.RecaudacionSC - dbo.CtaCte.ComisionSC - dbo.CtaCte.PremiosSC - dbo.CtaCte.ReclamosSC
              Dim DejoGanoSC As Decimal = recaudacionSC - comisionSC - PremiosSC - ReclamosSC
              DejoGanoSC = (Math.Round(DejoGanoSC, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
              'operacion3: DejoGanoB = dbo.CtaCte.RecaudacionB - dbo.CtaCte.ComisionB - dbo.CtaCte.PremiosB - dbo.CtaCte.ReclamosB
              Dim RecaudacionB = 0
              Dim ComisionB = 0
              Dim PremiosB = 0
              Dim DejoGanoB As Decimal = RecaudacionB - ComisionB - PremiosB - ReclamosB
              '--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


              '--------------------------------------------------------------------------------------------------------------------------------------------------
              'dbo.CtaCte.Prestamo = a la suma de todos los prestamos dados de alta para la fecha (dbo.PrestamosCreditos.Saldo)
              'dbo.CtaCte.Credito = a la suma de todos los creditos dados de alta para la fecha (dbo.PrestamosCreditos.Saldo)
              Dim Prestamo As Decimal = 0
              Dim Credito As Decimal = 0

              Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(Codigo_cliente)
              Dim Cliente_id As Integer = ds_clie.Tables(0).Rows(0).Item("Cliente")

              'recupero primero todos los prestamos donde fecha= fecha_liquidacion
              Dim ds_prescred As DataSet = DALiquidacion.LiquidacionFinal_obtener_prestamoscreditos(CDate(HF_fecha.Value), Cliente_id)
              Dim k As Integer = 0
              While k < ds_prescred.Tables(0).Rows.Count
                Select Case ds_prescred.Tables(0).Rows(k).Item("Tipo")
                  Case "P"
                    Prestamo = Prestamo + CDec(ds_prescred.Tables(0).Rows(k).Item("Saldo"))
                  Case "C"
                    Credito = Credito + CDec(ds_prescred.Tables(0).Rows(k).Item("Saldo"))
                End Select
                k = k + 1
              End While


              '--------------------------------------------------------------------------------------------------------------------------------------------------

              '-------aqui guardo en bd-----
              DACtaCte.CtaCte_alta(Grupo_id, CInt(Codigo_cliente), HF_fecha.Value, SaldoAnterior, recaudacion, comision, Premios, Reclamos, DejoGano,
                        recaudacionSC, comisionSC, PremiosSC, ReclamosSC, DejoGanoSC,
                        RecaudacionB, ComisionB, PremiosB, ReclamosB, DejoGanoB, Cobros, Pagos, Prestamo, Credito)
              '---------fin--------------


              '//////////////////////////////EN ESTA SECCION AGREGO UN REGISTRO POR CADA MOVIMIENTO DEL CLIENTE: PAGOS, COBROS, RECLAMOS/////////////////
              'valido si alguno de esos parametros es distinto de 0 lo agrego.
              If Pagos <> CDec(0) Then
                Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
                fila_mov("Cliente") = Codigo_cliente
                fila_mov("Movimiento") = "PAGO"
                fila_mov("Importe") = CDec(Pagos)
                DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
              End If
              If Cobros <> CDec(0) Then
                Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
                fila_mov("Cliente") = Codigo_cliente
                fila_mov("Movimiento") = "COBRO"
                fila_mov("Importe") = CDec(Cobros)
                DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
              End If
              Dim SumReclamos As Decimal = Reclamos + ReclamosSC + ReclamosB
              If SumReclamos <> CDec(0) Then
                Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
                fila_mov("Cliente") = Codigo_cliente
                fila_mov("Movimiento") = "RECLAMO"
                fila_mov("Importe") = CDec(SumReclamos)
                DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
              End If
              '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

              '-------------TERCERA ETAPA: Actualizacion de Saldo y SaldoRegaldo por cada cliente que tuvo movimento en la fecha del parametro a liquidar.------------------------------------------------------------

              'ACTUALIZACION DE SALDO------------
              'dbo.Clientes.SaldoAnterior = dbo.Clientes.Saldo
              Dim Clie_Saldo As Decimal = ds_xcargas.Tables(0).Rows(i).Item("Cliente_Saldo")
              Dim Clie_SaldoAnterior As Decimal = Clie_Saldo
              'dbo.Clientes.Saldo = dbo.Clientes.Saldo + dbo.CtaCteRecaudacion + dbo.CtaCteRecaudacionSC + dbo.CtaCteRecaudacionB - dbo.CtaCte.Comision - dbo.CtaCte.ComisionSC - dbo.CtaCte.ComisionB - dbo.CtaCte.Premios - dbo.CtaCte.PremiosSC - dbo.CtaCte.PremiosB - dbo.CtaCte.Reclamos - dbo.CtaCte.ReclamosSC - dbo.CtaCte.ReclamosB - dbo.CtaCte.Cobros + dbo.CtaCte.Pagos + dbo.CtaCte.CobPrestamo + dbo.CtaCte.CobCredito + dbo.Ctacte.Prestamo + dbo.Ctacte.Credito
              Clie_Saldo = Clie_Saldo + recaudacion + recaudacionSC + RecaudacionB - comision - comisionSC - ComisionB - Premios - PremiosSC - PremiosB - Reclamos - ReclamosSC - ReclamosB - Cobros + Pagos + 0 + 0 - Prestamo + Credito 'NOTA 2022-01-06 SE RESTA EL PARAMETRO PRESTAMO

              '---aqui guardo en bd -----
              DACliente.Clientes_ActualizarSaldo(Codigo_cliente, Clie_SaldoAnterior, Clie_Saldo)
              '--------------------------
              '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
              'FECHA: 2022-12-29.
              'NOTA: GUARDAR EN CTACTE.SALIDA EL TOTAL DEL SALDO FINAL CUANDO EL CLIENTE GANA, ES DECIR CUANDO EL MONTO ES NEGATIVO.
              If Clie_Saldo < CDec(0) Then
                'recupero el id de la ctacta para la fecha a liquidar.
                Dim DS_CTACTE As DataSet = DACtaCte.CtaCte_obtener(CInt(Codigo_cliente), HF_fecha.Value)
                If DS_CTACTE.Tables(0).Rows.Count <> 0 Then
                  Dim IdCtaCte As Integer = CInt(DS_CTACTE.Tables(0).Rows(0).Item("IdCtaCte"))
                  DACtaCte.CtaCte_ActualizarSalida(CInt(Codigo_cliente), HF_fecha.Value, CDec(Clie_Saldo))
                End If
              End If
              '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


              'ACTUALIZACION DE SALDO REGALO--------------
              'Operacion: dbo.Clientes.SaldoRegalo = dbo.Clientes.SaldoRegalo + ((dbo.CtaCte.Recaudacion - dbo.CtaCte.Comision - dbo.CtaCte.Premios - dbo.CtaCte.Reclamos) * dbo.Clientes.Regalo)
              Dim Clie_Regalo As Decimal = ds_xcargas.Tables(0).Rows(i).Item("Cliente_Regalo")
              Dim SaldoRegalo As Decimal = ds_xcargas.Tables(0).Rows(i).Item("Cliente_SaldoRegalo")
              SaldoRegalo = SaldoRegalo + (((recaudacion - comision - Premios - Reclamos) / 100) * Clie_Regalo * -1)

              '---aqui guardo en bd----
              DACliente.Clientes_ActualizarSaldoRegalo(Codigo_cliente, SaldoRegalo)
              '------------------------

              ''---------------FECHA: 2022-11-11--------------SE AGREGA TABLA CONFIGURACION----------------------------------------------------------
              'Dim SaldosACero As Integer = 0 'de momento hay que considerar que el dbo.Configuracion.SaldosACero = 0. 
              'Try
              '  Dim ds_config As DataSet = DAconfiguracion.Configuracion_obtenertodo
              '  SaldosACero = CInt(ds_config.Tables(0).Rows(0).Item("SaldosACero"))
              'Catch ex As Exception
              'End Try
              'If SaldosACero = 1 Then
              '  'recupero info del cliente.
              '  Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(Codigo_cliente)
              '  Dim Saldo As Decimal = CDec(ds_clie.Tables(0).Rows(0).Item("Saldo"))
              '  If Saldo < 0 Then 'si el saldo es negativo el Cliente Gana.
              '    DACtaCte.CtaCte_ActualizarSalida(Codigo_cliente, CDate(HF_fecha.Value), Saldo) 'Guardar en el campo dbo.CtaCte.Salida el saldo final del cliente cuando el cliente gana, 
              '    DACliente.Clientes_ActualizarSaldo(Codigo_cliente, Saldo, CDec(0)) 'y poner en 0 el saldo del cliente dbo.Clientes.Saldo = 0
              '  End If
              'End If
              ''------------------------------------------------------------------------------------------------------------------------------------

            End If
            cliente_agregado = Codigo_cliente 'esto lo hago para no contar 2 veces la recaudacion, y pasar continuar hasta el nuevo codigo de cliente
          Else
            Dim paso_x_aqui = "si"

          End If
          i = i + 1
        End While



      End If

    End If

    'ACTUALIZACON DE ULTIMA FECHA DE LIQUIDACION---------------
    'Operacion: dbo.Clientes.UltFechaLiq = a la fecha del parametro del dia de liquidacion, se actuliza la fecha de liquidacion aunque el cliente no haya tenido ningun movimiento.
    DACliente.Clientes_ActualizarFechaLiq(HF_fecha.Value)


    DAparametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal.aspx, Proc.TotalesXBoletas") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.
    '------FECHA: 2022-11-10---------------------------------------------------------------------------------
    LiqFinal_TotalesXBoletas()
    '-------------------------------------------------------------------------------------------------------

    DAparametro.Parametro_LiqFinalModifEstado(CInt(HF_parametro_id.Value), "LiquidacionFinal.aspx, Proc.Solo_PagosCobrosReclamos") 'Guardo en la bd info sobre la etapa actual del proc de liquidacion, me sirve para detectar errores y reliquidar oportunamente.
    'RUTINA PARA AGREGAR UN REGISTRO EN CTACTE SI HAY SOLAMENTE PAGOS,COBROS Y RECLAMOS PARA UN CLIENTE Q NO ESTE EN XCARGAS.
    Solo_PagosCobrosReclamos(DS_liqfinal)
    '-----------------------------------------------------------------------------------------------------------------


  End Sub

  Private Sub Solo_PagosCobrosReclamos(ByRef DS_liqfinal As DataSet)
    Dim ds_Ant As DataTable = DAAnticipados.Anticipados_obtenerTodoxFecha(HF_fecha.Value)

    'voy a ir agregando en una tabla auxiliar aquellos clientes que no tengan un registro para la fecha en ctacte.
    If ds_Ant.Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_Ant.Rows.Count
        Dim Cliente_Codigo As String = ds_Ant.Rows(i).Item("Codigo").ToString
        Dim Grupo_id As String = CStr(ds_Ant.Rows(i).Item("Grupo_id"))
        Dim SaldoAnterior As Decimal = ds_Ant.Rows(i).Item("Saldo")
        Dim Regalo As Decimal = ds_Ant.Rows(i).Item("Regalo")
        Dim SaldoRegalo As Decimal = ds_Ant.Rows(i).Item("SaldoRegalo")



        Dim filtro As String = "Cliente = " + "'" + Cliente_Codigo + "'"
        Dim rows_clientes() As DataRow = DS_liqfinal.Tables("Clientes_No_CtaCte").Select(filtro)

        If rows_clientes.Count = 0 Then
          Dim ds_CtaCte As DataSet = DACtaCte.CtaCte_obtener(CInt(Cliente_Codigo), HF_fecha.Value)
          If ds_CtaCte.Tables(0).Rows.Count = 0 Then
            'no existe registro, entonces lo agrego si ya no está en tabla auxiliar.
            Dim fila As DataRow = DS_liqfinal.Tables("Clientes_No_CtaCte").NewRow
            fila("Cliente") = Cliente_Codigo
            fila("Grupo_id") = Grupo_id
            fila("SaldoAnterior") = SaldoAnterior
            fila("Regalo") = Regalo
            fila("SaldoRegalo") = SaldoRegalo
            DS_liqfinal.Tables("Clientes_No_CtaCte").Rows.Add(fila)
          End If
        End If
        i = i + 1
      End While
    End If
    'Con la tabla auxiliar "DS_liqfinal.Tables("Clientes_No_CtaCte")" voy a ir ciclando para agregar los registros en ctacte.

    Dim j As Integer = 0
    While j < DS_liqfinal.Tables("Clientes_No_CtaCte").Rows.Count
      Dim Codigo_cliente As Integer = CInt(DS_liqfinal.Tables("Clientes_No_CtaCte").Rows(j).Item("Cliente"))
      Dim Grupo_id As Integer = CInt(DS_liqfinal.Tables("Clientes_No_CtaCte").Rows(j).Item("Grupo_id"))
      Dim SaldoAnterior As Decimal = DS_liqfinal.Tables("Clientes_No_CtaCte").Rows(j).Item("SaldoAnterior")
      Dim Regalo As Decimal = DS_liqfinal.Tables("Clientes_No_CtaCte").Rows(j).Item("Regalo")
      Dim SaldoRegalo As Decimal = DS_liqfinal.Tables("Clientes_No_CtaCte").Rows(j).Item("SaldoRegalo")

      'calculo Reclamos y ReclamosSC-----------------------------------------------------------------------------------------------------------------------------------------------------
      'operacion: Reclamos = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente,
      'donde dbo.Anticipados.Sincalculo = False y dbo.Anticipados.Origen = False y dbo.Anticipados.Tipo = 1.
      'operacion2: ReclamosSC = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente, donde dbo.Anticipados.Sincalculo = True y dbo.Anticipados.Origen = False y dbo.Anticipados.Tipo = 1
      'operacion3: dbo.CtaCte.ReclamosB = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente, donde dbo.Anticipados.Origen = True y dbo.Anticipados.Tipo = 1 
      Dim Reclamos As Decimal = 0
      Dim ReclamosSC As Decimal = 0
      Dim ReclamosB As Decimal = 0
      Dim ds_anticipados As DataSet = DAAnticipados.Anticipados_ClienteobtenerXfecha(HF_fecha.Value, Codigo_cliente)
      'operacon4: Cobros = a la suma de todos los importes de la tabla dbo.Anticpados.Importe del cliente, donde dbo.Anticipados.Tipo = 2
      'operacion5: Pagos = a la suma de todos los importes de la tabla dbo.Anticpados.Importe del cliente, donde dbo.Anticipados.Tipo = 3
      Dim Cobros As Decimal = 0
      Dim Pagos As Decimal = 0
      Dim ii As Integer = 0

      While ii < ds_anticipados.Tables(0).Rows.Count
        If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 1 Then
          If (ds_anticipados.Tables(0).Rows(ii).Item("Sincalculo") = False) And (ds_anticipados.Tables(0).Rows(ii).Item("Origen") = False) Then
            Reclamos = Reclamos + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
          End If
          If (ds_anticipados.Tables(0).Rows(ii).Item("Sincalculo") = True) And (ds_anticipados.Tables(0).Rows(ii).Item("Origen") = False) Then
            ReclamosSC = ReclamosSC + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
          End If
          If ds_anticipados.Tables(0).Rows(ii).Item("Origen") = True Then
            ReclamosB = ReclamosB + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
          End If
        End If
        If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 2 Then
          Cobros = Cobros + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
        End If
        If ds_anticipados.Tables(0).Rows(ii).Item("AnticipadosTipo_id") = 3 Then
          Pagos = Pagos + ds_anticipados.Tables(0).Rows(ii).Item("Importe")
        End If
        ii = ii + 1
      End While
      Reclamos = (Math.Round(Reclamos, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
      ReclamosSC = (Math.Round(ReclamosSC, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
      ReclamosB = (Math.Round(ReclamosB, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
      Cobros = (Math.Round(Cobros, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
      Pagos = (Math.Round(Pagos, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
      '--------------------------------------------------------------------------------------------------------------------------------------------------------------------

      'calculo DejoGano y DejoGanoSC----------------------------------------------------------------------------------------------------------------------------------------------------
      'operacion: DejoGano = dbo.CtaCte.Recaudacion - dbo.CtaCte.Comision - dbo.CtaCte.Premios - dbo.CtaCte.Reclamos

      Dim DejoGano As Decimal = 0 - 0 - 0 - Reclamos
      DejoGano = (Math.Round(DejoGano, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
      'operacion2: DejoGanoSC = dbo.CtaCte.RecaudacionSC - dbo.CtaCte.ComisionSC - dbo.CtaCte.PremiosSC - dbo.CtaCte.ReclamosSC
      Dim DejoGanoSC As Decimal = 0 - 0 - 0 - ReclamosSC
      DejoGanoSC = (Math.Round(DejoGanoSC, 2).ToString("N2")) 'redondeo a 2dig en el decimal para evitar desbordamiento
      'operacion3: DejoGanoB = dbo.CtaCte.RecaudacionB - dbo.CtaCte.ComisionB - dbo.CtaCte.PremiosB - dbo.CtaCte.ReclamosB
      Dim RecaudacionB = 0
      Dim ComisionB = 0
      Dim PremiosB = 0
      Dim DejoGanoB As Decimal = RecaudacionB - ComisionB - PremiosB - ReclamosB
      '--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

      '-------aqui guardo en bd-----
      DACtaCte.CtaCte_alta(Grupo_id, CInt(Codigo_cliente), HF_fecha.Value, SaldoAnterior, 0, 0, 0, Reclamos, DejoGano,
                      0, 0, 0, ReclamosSC, DejoGanoSC,
                      RecaudacionB, ComisionB, PremiosB, ReclamosB, DejoGanoB, Cobros, Pagos, 0, 0)
      '---------fin--------------


      '//////////////////////////////EN ESTA SECCION AGREGO UN REGISTRO POR CADA MOVIMIENTO DEL CLIENTE: PAGOS, COBROS, RECLAMOS/////////////////
      'valido si alguno de esos parametros es distinto de 0 lo agrego.
      If Pagos <> CDec(0) Then
        Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
        fila_mov("Cliente") = Codigo_cliente
        fila_mov("Movimiento") = "PAGO"
        fila_mov("Importe") = CDec(Pagos)
        DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
      End If
      If Cobros <> CDec(0) Then
        Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
        fila_mov("Cliente") = Codigo_cliente
        fila_mov("Movimiento") = "COBRO"
        fila_mov("Importe") = CDec(Cobros)
        DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
      End If
      Dim SumReclamos As Decimal = Reclamos + ReclamosSC + ReclamosB
      If SumReclamos <> CDec(0) Then
        Dim fila_mov As DataRow = DS_liqfinal.Tables("PagosCobrosReclamos").NewRow
        fila_mov("Cliente") = Codigo_cliente
        fila_mov("Movimiento") = "RECLAMO"
        fila_mov("Importe") = CDec(SumReclamos)
        DS_liqfinal.Tables("PagosCobrosReclamos").Rows.Add(fila_mov)
      End If
      '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      '-------------TERCERA ETAPA: Actualizacion de Saldo y SaldoRegaldo por cada cliente que tuvo movimento en la fecha del parametro a liquidar.------------------------------------------------------------


      'ACTUALIZACION DE SALDO------------
      'dbo.Clientes.SaldoAnterior = dbo.Clientes.Saldo
      Dim Clie_Saldo As Decimal = DS_liqfinal.Tables("Clientes_No_CtaCte").Rows(j).Item("SaldoAnterior") 'que es el saldo del cliente previo a la liq.final
      Dim Clie_SaldoAnterior As Decimal = Clie_Saldo

      'Clie_Saldo = Clie_Saldo + recaudacion + recaudacionSC + RecaudacionB - comision - comisionSC - ComisionB - Premios - PremiosSC - PremiosB - Reclamos - ReclamosSC - ReclamosB - Cobros + Pagos + 0 + 0 + Prestamo + Credito

      Clie_Saldo = Clie_Saldo + 0 + 0 + RecaudacionB - 0 - 0 - ComisionB - 0 - 0 - PremiosB - Reclamos - ReclamosSC - ReclamosB - Cobros + Pagos + 0 + 0 + 0 + 0

      '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      'FECHA: 2022-12-29.
      'NOTA: GUARDAR EN CTACTE.SALIDA EL TOTAL DEL SALDO FINAL CUANDO EL CLIENTE GANA, ES DECIR CUANDO EL MONTO ES NEGATIVO.
      If Clie_Saldo < CDec(0) Then
        'recupero el id de la ctacta para la fecha a liquidar.
        Dim DS_CTACTE As DataSet = DACtaCte.CtaCte_obtener(CInt(Codigo_cliente), HF_fecha.Value)
        If DS_CTACTE.Tables(0).Rows.Count <> 0 Then
          Dim IdCtaCte As Integer = CInt(DS_CTACTE.Tables(0).Rows(0).Item("IdCtaCte"))
          DACtaCte.CtaCte_ActualizarSalida(CInt(Codigo_cliente), HF_fecha.Value, CDec(Clie_Saldo))
        End If
      End If
      '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      '---aqui guardo en bd -----
      DACliente.Clientes_ActualizarSaldo(Codigo_cliente, Clie_SaldoAnterior, Clie_Saldo)
      '--------------------------

      'ACTUALIZACION DE SALDO REGALO--------------
      'Operacion: dbo.Clientes.SaldoRegalo = dbo.Clientes.SaldoRegalo + ((dbo.CtaCte.Recaudacion - dbo.CtaCte.Comision - dbo.CtaCte.Premios - dbo.CtaCte.Reclamos) * dbo.Clientes.Regalo)
      Dim Clie_Regalo As Decimal = Regalo
      'Dim SaldoRegalo As Decimal = SaldoRegalo

      'SaldoRegalo = SaldoRegalo + (((recaudacion - comision - Premios - Reclamos) / 100) * Clie_Regalo * -1)
      SaldoRegalo = SaldoRegalo + (((0 - 0 - 0 - Reclamos) / 100) * Clie_Regalo * -1)

      '---aqui guardo en bd----
      DACliente.Clientes_ActualizarSaldoRegalo(Codigo_cliente, SaldoRegalo)
      '------------------------


      j = j + 1
    End While
  End Sub


  Private Sub LiqFinal_TotalesXBoletas()
    Dim DS_liqfinal As New DS_liqfinal
    DS_liqfinal.XCargas_Z.Rows.Clear()
    Dim ds_tablas As DataSet = DAconsultas.XCargas_ObtenerNombraTablas
    If ds_tablas.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < ds_tablas.Tables(0).Rows.Count
        Dim NombreTabla As String = ds_tablas.Tables(0).Rows(i).Item("nombre")
        If NombreTabla <> "XCargasW" Then 'si no es XCargasW, recupero todos los registros donde Recorrido = 'Z'
          Dim ds_tabla_regz As DataSet = DALiquidacion.XCargasN_recuperar_Z(NombreTabla, HF_fecha.Value)
          If ds_tabla_regz.Tables(0).Rows.Count <> 0 Then
            DS_liqfinal.XCargas_Z.Merge(ds_tabla_regz.Tables(0))
          End If
        End If
        i = i + 1
      End While
    End If


    If DS_liqfinal.XCargas_Z.Rows.Count <> 0 Then
      'ahora ordeno por Cliente ASC
      Dim rows() As DataRow = DS_liqfinal.XCargas_Z.Select("IDcarga > 0", "Cliente ASC")
      Dim dtTemp As DataTable = DS_liqfinal.XCargas_Z.Clone() 'copio la estructura de la tabla.
      For Each row As DataRow In rows
        ' Indicamos que el registro ha sido añadido
        row.SetAdded()
        dtTemp.ImportRow(row)
      Next
      'dtTemp tiene todos los registros de xcargas ya ordenas para poder continuar.

      Dim i As Integer = 0
      Dim Cliente As Integer = 0
      Dim ClieProcesado As Integer = 0
      While i < dtTemp.Rows.Count
        Cliente = CInt(dtTemp.Rows(i).Item("Cliente"))

        If Cliente <> ClieProcesado Then
          '//////vuelvo a recorrer para contar
          Dim j As Integer = 0
          Dim RecaudacionB As Decimal = 0
          While j < dtTemp.Rows.Count
            If Cliente = dtTemp.Rows(j).Item("Cliente") Then
              'dbo.CtaCte.RecaudacionB = a la suma de todos los importes de la tabla dbo.XCargas.TotalImporte donde dbo.XCargas.Recorrido = "Z"
              RecaudacionB = RecaudacionB + dtTemp.Rows(j).Item("TotalImporte")
            End If
            j = j + 1
          End While
          RecaudacionB = (Math.Round(RecaudacionB, 2).ToString("N2"))
          '**********************************************************************************
          'dbo.CtaCte.ComisionB = al calculo del porcentaje del total de RecaudacionB (el porcentaje se obtiene de la tabla dbo.Clientes.Comision1) del cliente.
          Dim ds_clie As DataSet = DACliente.Clientes_buscar_codigo(CStr(Cliente))
          Dim Comision1 As Decimal = 0
          If ds_clie.Tables(0).Rows.Count <> 0 Then
            Try
              Comision1 = CDec(ds_clie.Tables(0).Rows(0).Item("Comision1"))
            Catch ex As Exception
            End Try
          End If
          Dim ComisionB As Decimal = (RecaudacionB * Comision1) / 100
          ComisionB = (Math.Round(ComisionB, 2).ToString("N2"))
          '**********************************************************************************
          'INCOMPLETO  ......VER CUAL ES LA TABLA PREMIOSB
          'dbo.CtaCte.PremiosB = a la suma de todos los importes de la tabla dbo.PremiosB.Premio del cliente
          Dim PremiosB As Decimal = 0
          'dbo.CtaCte.ReclamosB = a la suma de todos los importes de la tabla dbo.Anticipados.Importe del cliente, donde dbo.Anticipados.Origen = True y dbo.Anticipados.Tipo = 1 
          '**********************************************************************************
          Dim ds_anticipo As DataSet = DAAnticipados.Anticipados_ClienteobtenerXfecha_reclamos(CDate(HF_fecha.Value), Cliente)
          Dim ReclamosB As Decimal = 0
          If ds_anticipo.Tables(0).Rows.Count <> 0 Then
            j = 0
            While j < ds_anticipo.Tables(0).Rows.Count
              Try
                ReclamosB = ReclamosB + CDec(ds_anticipo.Tables(0).Rows(j).Item("Importe"))
              Catch ex As Exception
              End Try
              j = j + 1
            End While
            ReclamosB = (Math.Round(ReclamosB, 2).ToString("N2"))
          End If
          ReclamosB = (Math.Round(ReclamosB, 2).ToString("N2"))
          '**********************************************************************************
          'dbo.CtaCte.DejoGanoB = dbo.CtaCte.RecaudacionB - dbo.CtaCte.ComisionB - dbo.CtaCte.PremiosB - dbo.CtaCte.ReclamosB
          Dim DejoGanoB As Decimal = RecaudacionB - ComisionB - PremiosB - ReclamosB
          DejoGanoB = (Math.Round(DejoGanoB, 2).ToString("N2"))
          '**********************************************************************************


          'ACTUALIZAR EN CTACTE.
          DALiquidacion.CtaCte_actualizarCamposB(Cliente, CDate(HF_fecha.Value), RecaudacionB, ComisionB, PremiosB, ReclamosB, DejoGanoB)

          '*****************************************************************************************************************
          'AQUI CREO QUE VA UNA ACTUALIZACION DE SALDO, X EL PARAMETRO DEJOGANO - 27-01-2023
          Dim Saldoanterior As Decimal = ds_clie.Tables(0).Rows(0).Item("Saldoanterior")
          Dim Saldo As Decimal = ds_clie.Tables(0).Rows(0).Item("Saldo")
          Saldo = (Math.Round(Saldo, 2).ToString("N2"))
          Saldo = Saldo + DejoGanoB
          '---aqui guardo en bd -----
          DACliente.Clientes_ActualizarSaldo(CStr(Cliente), Saldoanterior, Saldo)
          '*****************************************************************************************************************

          ClieProcesado = Cliente 'esto es para no volver a sumarlo en el prox ciclo.

        End If


        i = i + 1
      End While


    End If


  End Sub

  Private Sub grabar_premios_op1(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String)

    Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientes").NewRow
    fila("Cliente") = registro.Item("Cliente")
    fila("Recorrido") = referencia_recorrido
    fila("Importe") = registro.Item("Importe")
    fila("PID") = registro.Item("Pid")
    fila("SUC") = registro.Item("Suc")
    fila("P2") = ""

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
        premio = premio * (-1)
      End If
    End If


    DS_liqparcial1.Tables("PremiosxClientes").Rows.Add(fila)

    '-----------------------AQUI GUARDO EN LA BASE DATOS ----NUEVO REGISTRO EN TABLA PREMIOS----------------------
    Dim NroTicket As String = ""
    If registro.Item("Terminal").ToString.ToUpper = "W" Then
      NroTicket = registro.Item("Item").ToString
    End If
    DAPremios.Premios_altaOP1y2(HF_fecha.Value, registro.Item("Recorrido_codigo"),
                           CStr(registro.Item("Pid")), CDec(registro.Item("Importe")),
                           CInt(registro.Item("Suc")), CInt(0), CInt(SinComputo),
premio, NroTicket, CStr(registro.Item("Terminal")), CStr(registro.Item("Cliente")))



  End Sub

  Private Sub grabar_premios_op2(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia As Integer)
    Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientes").NewRow
    fila("Cliente") = registro.Item("Cliente")
    fila("Recorrido") = referencia_recorrido
    fila("Importe") = registro.Item("Importe")
    fila("PID") = registro.Item("Pid")
    fila("SUC") = registro.Item("Suc")
    fila("P2") = ""
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
        premio = premio * (-1)
      End If
    End If
    DS_liqparcial1.Tables("PremiosxClientes").Rows.Add(fila)

    '-----------------------AQUI GUARDO EN LA BASE DATOS ----NUEVO REGISTRO EN TABLA PREMIOS----------------------
    Dim NroTicket As String = ""
    If registro.Item("Terminal").ToString.ToUpper = "W" Then
      NroTicket = registro.Item("Item").ToString
    End If
    DAPremios.Premios_altaOP1y2(HF_fecha.Value, registro.Item("Recorrido_codigo"),
                           CStr(registro.Item("Pid")), CDec(registro.Item("Importe")),
                           CInt(registro.Item("Suc")), 0, CInt(SinComputo),
premio, NroTicket, CStr(registro.Item("Terminal")), CStr(registro.Item("Cliente")))

  End Sub

  Private Sub grabar_premios_op3(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia As Integer)
    Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientes").NewRow
    fila("Cliente") = registro.Item("Cliente")
    fila("Recorrido") = referencia_recorrido
    fila("Importe") = registro.Item("Importe")
    fila("PID") = registro.Item("Pid")
    fila("SUC") = registro.Item("Suc")
    fila("S2") = registro.Item("Suc2")
    fila("P2") = registro.Item("Pid2")
    Dim premio As Decimal = 0
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

    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        fila("OBS") = "CUB."
        fila("Premio") = fila("Premio") * (-1)
        premio = premio * (-1)
      End If
    End If
    DS_liqparcial1.Tables("PremiosxClientes").Rows.Add(fila)

    '-----------------------AQUI GUARDO EN LA BASE DATOS ----NUEVO REGISTRO EN TABLA PREMIOS----------------------
    Dim NroTicket As String = ""
    If registro.Item("Terminal").ToString.ToUpper = "W" Then
      NroTicket = registro.Item("Item").ToString
    End If
    DAPremios.Premios_altaOP3y4(HF_fecha.Value, registro.Item("Recorrido_codigo"),
                           CStr(registro.Item("Pid")), CDec(registro.Item("Importe")),
                           CInt(registro.Item("Suc")), CStr(registro.Item("Pid2")), CInt(registro.Item("Suc2")), 1, CInt(SinComputo),
                           premio, NroTicket, CStr(registro.Item("Terminal")), CStr(registro.Item("Cliente")))


  End Sub

  Private Sub grabar_premios_op4(ByRef DS_liqparcial1 As DataSet, ByVal registro As DataRow, ByVal referencia_recorrido As String, ByVal ContCoincidencia1 As Integer, ByVal ContCoincidencia2 As Integer)
    Dim fila As DataRow = DS_liqparcial1.Tables("PremiosxClientes").NewRow
    fila("Cliente") = registro.Item("Cliente")
    fila("Recorrido") = referencia_recorrido
    fila("Importe") = registro.Item("Importe")
    fila("PID") = registro.Item("Pid")
    fila("SUC") = registro.Item("Suc")
    fila("S2") = registro.Item("Suc2")
    fila("P2") = registro.Item("Pid2")
    Dim premio As Decimal = 0
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
      fila("Premio") = (Math.Round(premio, 2).ToString("N2"))
    Catch ex As Exception

    End Try
    fila("T") = registro.Item("Terminal").ToString.ToUpper
    fila("OBS") = "" 'corresponde graba "CUB.", en el caso que el cliente tenga la variable1 = true y ademas el valor del premio va en negativo.

    If ds_cliente.Tables(0).Rows.Count <> 0 Then
      If ds_cliente.Tables(0).Rows(0).Item("Variable1") = True Then
        fila("OBS") = "CUB."
        fila("Premio") = fila("Premio") * (-1)
        premio = premio * (-1)
      End If
    End If
    DS_liqparcial1.Tables("PremiosxClientes").Rows.Add(fila)

    '-----------------------AQUI GUARDO EN LA BASE DATOS ----NUEVO REGISTRO EN TABLA PREMIOS----------------------
    Dim NroTicket As String = ""
    If registro.Item("Terminal").ToString.ToUpper = "W" Then
      NroTicket = registro.Item("Item").ToString
    End If
    DAPremios.Premios_altaOP3y4(HF_fecha.Value, registro.Item("Recorrido_codigo"),
                           CStr(registro.Item("Pid")), CDec(registro.Item("Importe")),
                           CInt(registro.Item("Suc")), CStr(registro.Item("Pid2")), CInt(registro.Item("Suc2")), 1, CInt(SinComputo),
premio, NroTicket, CStr(registro.Item("Terminal")), CStr(registro.Item("Cliente")))



  End Sub

  Private Sub Btn_Ok_continue_ServerClick(sender As Object, e As EventArgs) Handles Btn_Ok_continue.ServerClick
    metodo1()
  End Sub

#End Region

End Class
