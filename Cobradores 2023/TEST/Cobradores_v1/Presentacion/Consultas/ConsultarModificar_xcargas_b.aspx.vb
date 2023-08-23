Public Class ConsultarModificar_xcargas_b
  Inherits System.Web.UI.Page
  Dim DAparametro As New Capa_Datos.WC_parametro
  Dim DAcliente As New Capa_Datos.WB_clientes
  Dim DAconsultas As New Capa_Datos.WB_Consultas
  Dim DArecorrido As New Capa_Datos.WC_recorridos_zonas
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos

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
        'mostrar_zonas_habilitadas(dia)

        'MOSTRAR CODIGO CLIENTE Y NOMBRE
        Dim ds_clie As DataSet = DAcliente.Clientes_buscar_codigo(CStr(Session("cliente_codigo")))
        Try
          HF_cliente_id.Value = ds_clie.Tables(0).Rows(0).Item("Cliente")
          Txt_ClienteCod.Text = ds_clie.Tables(0).Rows(0).Item("Codigo")
          Txt_cliente_nombre.Text = ds_clie.Tables(0).Rows(0).Item("Nombre")
          Txt_ClienteCod.ReadOnly = True
          Txt_cliente_nombre.ReadOnly = True
        Catch ex As Exception

        End Try

        obtener_xcargas()
        mostrar_zonas_habilitadas(dia)



      Else
        'AQUI MENSAJE Y QUE CON EL BOTON "OK" U "CLOSE" VUELVA AL MENU PRINCIPAL.
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error", "$(document).ready(function () {$('#modal-ok_error').modal();});", True)
      End If

      Txt_ClienteCod.Focus()
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
            If (Menu = "5" And Opcion = "") Or (Menu = "5" And Opcion = "4") Then
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

  Dim DS_Consultas As New DS_Consultas

  Private Sub obtener_xcargas()

    DS_Consultas.XCargas.Rows.Clear()

    Dim ds_tablas_disp As DataSet = DAConsultas.XCargas_ObtenerNombraTablas()

    If ds_tablas_disp.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      Dim cont_existe As Integer = 0

      While i < ds_tablas_disp.Tables(0).Rows.Count
        Dim Tabla As String = ds_tablas_disp.Tables(0).Rows(i).Item("nombre").ToString

        'hago una consulta x cada tabla buscando x cliente
        Dim DS_INFO As DataSet = DAConsultas.XCargas_obtenerTabla(Tabla, Txt_ClienteCod.Text)
        If DS_INFO.Tables(0).Rows.Count <> 0 Then

          Dim j As Integer = 0
          While j < DS_INFO.Tables(0).Rows.Count
            Dim fila As DataRow = DS_Consultas.XCargas.NewRow
            fila("IDcarga") = DS_INFO.Tables(0).Rows(j).Item("IDcarga")
            fila("Recorrido") = DS_INFO.Tables(0).Rows(j).Item("Recorrido")
            fila("Cliente") = DS_INFO.Tables(0).Rows(j).Item("Cliente")
            fila("Pid") = DS_INFO.Tables(0).Rows(j).Item("Pid")
            fila("Importe") = DS_INFO.Tables(0).Rows(j).Item("Importe")
            fila("Suc") = DS_INFO.Tables(0).Rows(j).Item("Suc")
            fila("Pid2") = DS_INFO.Tables(0).Rows(j).Item("Pid2")
            fila("Suc2") = DS_INFO.Tables(0).Rows(j).Item("Suc2")

            Try
              If DS_INFO.Tables(0).Rows(j).Item("R") = True Then
                fila("R") = "Si"
              Else
                fila("R") = "No"
              End If
            Catch ex As Exception

            End Try
            Try
              If DS_INFO.Tables(0).Rows(j).Item("SinComputo") = True Then
                fila("SinComputo") = "Si"
              Else
                fila("SinComputo") = "No"
              End If
            Catch ex As Exception

            End Try
            fila("TotalImporte") = DS_INFO.Tables(0).Rows(j).Item("TotalImporte")
            fila("Fecha") = DS_INFO.Tables(0).Rows(j).Item("Fecha")
            fila("Hora") = DS_INFO.Tables(0).Rows(j).Item("Hora")
            Try
              If DS_INFO.Tables(0).Rows(j).Item("Verificado") = True Then
                fila("Verificado") = "Si"
              Else
                fila("Verificado") = "No"
              End If
            Catch ex As Exception

            End Try
            fila("Terminal") = DS_INFO.Tables(0).Rows(j).Item("Terminal")
            fila("Item") = DS_INFO.Tables(0).Rows(j).Item("Item")
            fila("Tabla") = Tabla

            DS_Consultas.XCargas.Rows.Add(fila)

            j = j + 1
          End While
          cont_existe = cont_existe + 1
        End If
        i = i + 1
      End While
      If cont_existe <> 0 Then
        GridView1.DataSource = DS_Consultas.XCargas
        'GridView1.ViewStateMode = ViewStateMode.

        GridView1.DataBind()

        GridView1.Columns(0).Visible = False '0 es la columna IDcarga
        GridView1.Columns(2).Visible = False '2 es la columna Cliente
        GridView1.Columns(10).Visible = False '10 es la columna TotalImporte
        GridView1.Columns(16).Visible = False '10 es la columna tabla


      Else
        'MENSAJE: La busqueda no arrojo resultados

        Label_ErrorValidacion.Text = "La busqueda no arrojo resultados!"
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ErrorValidacion", "$(document).ready(function () {$('#modal-ErrorValidacion').modal();});", True)

      End If

    Else
      'no existe ninguna tabla "xcargas"

    End If





  End Sub

  Private Sub mostrar_zonas_habilitadas(ByVal dia As Integer)
    Label_recorridos.Text = "RECORRIDOS:"
    Dim DS_Recorridos As DataSet = DArecorrido.recorridos_zonas_obtener_habilitados_x_dia(dia)
    Dim i As Integer = 0
    While i < DS_Recorridos.Tables(1).Rows.Count
      Dim Habilitada As Integer = conv_bit(CInt(DS_Recorridos.Tables(1).Rows(i).Item("Habilitada")))
      Dim codigo As String = DS_Recorridos.Tables(1).Rows(i).Item("Codigo")
      If Habilitada = 1 Then
        Label_recorridos.Text += codigo
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


  Private Sub btn_error_close_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_ok_error_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub VALIDAR1_RECORRIDO(ByRef VALIDACION_RECORRIDO As String, ByVal recorrido As String, ByRef contador_codigos As Integer)

    Dim CADENA As String = recorrido
    Dim CADENA_VALIDA As String = "si"
    If (CADENA.Length Mod 2) <> 0 Then
      'El número es impar.
      CADENA_VALIDA = "no"

    Else
      'El número es par.
      Dim i As Integer
      Dim codigo As String = ""
      Dim cad2 As String = ""
      For i = 1 To Len(CADENA)

        If (i Mod 2) <> 0 Then 'es impar, entonces coloco 2 digitos en una variable
          contador_codigos = contador_codigos + 1
          codigo = Mid(CADENA, i, 2)
          cadena_validar(CADENA_VALIDA, codigo)

          If CADENA_VALIDA = "no" Then
            Exit For
          End If
        End If

      Next i
    End If
    If CADENA_VALIDA = "si" Then
      VALIDACION_RECORRIDO = "si"
    Else
      VALIDACION_RECORRIDO = "no"
    End If


  End Sub

  Private Sub cadena_validar(ByRef CADENA_VALIDA As String, ByRef codigo As String)
    CADENA_VALIDA = "no"
    Dim DS_Recorridos As DataSet = DArecorrido.recorridos_zonas_obtener_habilitados_x_dia(HF_dia_id.Value)
    Dim i As Integer = 0
    While i < DS_Recorridos.Tables(1).Rows.Count
      Dim Habilitada As Integer = conv_bit(CInt(DS_Recorridos.Tables(1).Rows(i).Item("Habilitada")))
      Dim codigo_recuperado As String = DS_Recorridos.Tables(1).Rows(i).Item("Codigo")
      If Habilitada = 1 Then
        If codigo = codigo_recuperado Then
          CADENA_VALIDA = "si"
          Exit While
        End If
      End If

      i = i + 1
    End While
  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/ConsultarModificar_xcargas_a.aspx")
  End Sub

  Private Sub btn_modificar_mdll_ServerClick(sender As Object, e As EventArgs) Handles btn_modificar_mdll.ServerClick
    Label_ErrorValidacion.Text = ""
    Dim error_tipo As String = ""
    Dim valido = "si"
    Dim i As Integer = 0

    While i < GridView1.Rows.Count
      'Dim recorrido As String = GridView1.Rows(i).FindControl()
      Dim IDcarga As Integer = CInt(GridView1.Rows(i).Cells(0).Text)
      'VALIDAR RECORRIDO--------------------------------------------------------------------------------------
      Dim VALIDACION_RECORRIDO = "si"
      Dim recorrido = (CType(GridView1.Rows(i).FindControl("Txt_recorrido"), TextBox).Text.ToUpper)
      Dim contador_codigos As Integer = 0
      VALIDAR1_RECORRIDO(VALIDACION_RECORRIDO, recorrido, contador_codigos)
      If VALIDACION_RECORRIDO = "no" Then
        valido = "no"
        error_tipo = "Error recorrido"
        Exit While
      End If
      '-------------------------------------------------------------------------------------------------------
      'VALIDAR PID--------------------------------------------------------------------------------------------
      Dim Pid = (CType(GridView1.Rows(i).FindControl("Txt_Pid"), TextBox).Text)
      If Pid = "" Then
        valido = "no"
        error_tipo = "Error Pid"
        Exit While
      End If
      '-------------------------------------------------------------------------------------------------------
      'VALIDAR IMPORTE----------------------------------------------------------------------------------------
      Dim Importe As Decimal = 0
      Try
        Importe = (CType(GridView1.Rows(i).FindControl("Txt_Importe"), TextBox).Text).Replace(".", ",")
      Catch ex As Exception
        If (CType(GridView1.Rows(i).FindControl("Txt_Importe"), TextBox).Text) = "" Then
          Importe = 0
        End If
      End Try
      If Importe > 99999.99 Then
        valido = "no"
        error_tipo = "Error Importe"
        Exit While
      End If
      '-------------------------------------------------------------------------------------------------------
      'VALIDAR TOTALIMPORTE-----------------------------------------------------------------------------------
      Dim TOTALIMPORTE As Decimal = Importe * contador_codigos

      If TOTALIMPORTE > 9999999.99 Then
        valido = "no"
        error_tipo = "Error IMPORTE, la sumatoria genera desbordamiento. Total Importe > 99999.99"
        Exit While
      End If

      'VALIDAR SUC--------------------------------------------------------------------------------------------
      Try
        Dim Suc As Integer = (CType(GridView1.Rows(i).FindControl("Txt_suc"), TextBox).Text)
        If Suc > 20 Then
          valido = "no"
          error_tipo = "Error S > 20"
        End If
      Catch ex As Exception
        If (CType(GridView1.Rows(i).FindControl("Txt_suc"), TextBox).Text) = "" Then
          valido = "no"
          error_tipo = "Error S vacio"
          Exit While
        End If
      End Try
      '-------------------------------------------------------------------------------------------------------
      Dim Pid2 = (CType(GridView1.Rows(i).FindControl("Txt_Pid2"), TextBox).Text)
      '-------------------------------------------------------------------------------------------------------
      'VALIDAR SUC2-------------------------------------------------------------------------------------------
      Dim Suc2 = (CType(GridView1.Rows(i).FindControl("Txt_suc2"), TextBox).Text)
      If Suc2 <> "" Then
        Dim Suc As Integer = (CType(GridView1.Rows(i).FindControl("Txt_suc"), TextBox).Text)
        If CInt(Suc2) > 20 Then
          valido = "no"
          error_tipo = "Error S2 > 20"
          Exit While
        Else
          If CInt(Suc2) < Suc Then
            valido = "no"
            error_tipo = "Error S2 < S"
            Exit While
          End If
        End If
      End If
      '-------------------------------------------------------------------------------------------------------
      'VALIDAR SINCOMPUTO-------------------------------------------------------------------------------------
      Dim SinComputo As String = (CType(GridView1.Rows(i).FindControl("Txt_sincomputo"), TextBox).Text)
      If SinComputo = "" Then
        valido = "no"
        error_tipo = "Error SC, solo Si/No"
        Exit While
      Else
        If SinComputo.ToUpper = "SI" Or SinComputo.ToUpper = "NO" Then
        Else
          valido = "no"
          error_tipo = "Error SC, solo Si/No"
          Exit While
        End If
      End If
      '-------------------------------------------------------------------------------------------------------

      i = i + 1
    End While

    If valido = "si" Then
      'si esta todo correcto comienzo con el proceso de update en tabla.
      i = 0
      While i < GridView1.Rows.Count
        'Dim recorrido As String = GridView1.Rows(i).FindControl()
        Dim IDcarga As Integer = CInt(GridView1.Rows(i).Cells(0).Text)
        Dim Tabla As String = GridView1.Rows(i).Cells(16).Text
        Dim recorrido = (CType(GridView1.Rows(i).FindControl("Txt_recorrido"), TextBox).Text.ToUpper)
        Dim Pid = (CType(GridView1.Rows(i).FindControl("Txt_Pid"), TextBox).Text)
        Dim Importe As Decimal = (CType(GridView1.Rows(i).FindControl("Txt_Importe"), TextBox).Text).Replace(".", ",")

        Dim Suc As Integer = (CType(GridView1.Rows(i).FindControl("Txt_suc"), TextBox).Text)
        Dim SinComputo As String = (CType(GridView1.Rows(i).FindControl("Txt_sincomputo"), TextBox).Text)
        If SinComputo.ToUpper = "SI" Then
          SinComputo = "1"
        Else
          SinComputo = "0"
        End If
        Dim CONTADOR = 0
        OBTENER_CANT_CODIGOS(CONTADOR, recorrido)
        Dim TOTALIMPORTE As Decimal = Importe * CONTADOR
        Dim Pid2 = (CType(GridView1.Rows(i).FindControl("Txt_Pid2"), TextBox).Text)
        Dim Suc2 = (CType(GridView1.Rows(i).FindControl("Txt_suc2"), TextBox).Text)
        DAconsultas.XCargas_modificarTabla(Tabla, IDcarga, recorrido, Pid, CStr(Importe), CStr(Suc), SinComputo, CStr(TOTALIMPORTE), Pid2, Suc2)

        'DAconsultas.XCargas_modificarTabla_pid2(Tabla, IDcarga, Pid2)

        'DAconsultas.XCargas_modificarTabla_suc2(Tabla, IDcarga, Suc2)
        i = i + 1
      End While
      'aqui mensaje para terminar.
      'modal-sm_OKGRABADO
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)


    Else
      'msj error
      'modal-ErrorValidacion
      Label_ErrorValidacion.Text = error_tipo + ". Revisar parametros ingresados."
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ErrorValidacion", "$(document).ready(function () {$('#modal-ErrorValidacion').modal();});", True)
    End If

  End Sub

  Private Sub OBTENER_CANT_CODIGOS(ByRef CONTADOR As Integer, ByVal RECORRIDO As String)
    Dim CADENA As String = RECORRIDO
    Dim CADENA_VALIDA As String = "si"
    If (CADENA.Length Mod 2) <> 0 Then
      'El número es impar.
      CADENA_VALIDA = "no"

    Else
      'El número es par.
      Dim i As Integer
      Dim codigo As String = ""
      Dim cad2 As String = ""
      For i = 1 To Len(CADENA)

        If (i Mod 2) <> 0 Then 'es impar, entonces coloco 2 digitos en una variable
          CONTADOR = CONTADOR + 1
          codigo = Mid(CADENA, i, 2)

        End If

      Next i
    End If
  End Sub

  Private Sub btn_ok_ServerClick(sender As Object, e As EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/ConsultarModificar_xcargas_a.aspx")
  End Sub

  Private Sub btn_graba_close_ServerClick(sender As Object, e As EventArgs) Handles btn_graba_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Consultas/ConsultarModificar_xcargas_a.aspx")
  End Sub

  Private Sub btn_modificar_mdl_cancelar_ServerClick(sender As Object, e As EventArgs) Handles btn_modificar_mdl_cancelar.ServerClick
    GridView1.Focus()
  End Sub

  Private Sub btn_modificar_close_ServerClick(sender As Object, e As EventArgs) Handles btn_modificar_close.ServerClick
    GridView1.Focus()
  End Sub
End Class
