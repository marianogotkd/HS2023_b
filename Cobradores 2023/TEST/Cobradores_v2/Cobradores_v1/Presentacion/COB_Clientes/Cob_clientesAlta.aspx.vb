Public Class Cob_clientesAlta
  Inherits System.Web.UI.Page
  Dim daLocal As New Capa_Datos.Local
  Dim daClientes As New Capa_Datos.Clientes
  Dim daCtaCte As New Capa_Datos.CtaCte
  Dim daTarifa As New Capa_Datos.Tarifa
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then


      If Session("Clientes_OP") = "ALTA" Then

        HF_CLIE_ID.Value = 0


        Recuperar_Locales()


        Session("Clientes_OP") = ""

        TxtDni.Focus()

        'gridvacio()
      End If
      If Session("Clientes_OP") = "MODIFICAR" Then
        'MODIFICAR 

        HF_CLIE_ID.Value = Session("CLIE_ID")


        recuperar_info_cliente()


        Recuperar_Locales()


        Session("Clientes_OP") = ""

        TxtDni.Focus()

      End If


    End If
  End Sub

  Private Sub recuperar_info_cliente()
    GridView1.Columns(0).Visible = True    '0 es columna TARCLIE_ID
    GridView1.Columns(1).Visible = True  '1 es columna TARIFA_ID
    GridView1.Columns(3).Visible = True  '3 es columna LOCAL_ID

    Dim ds_clieinfo As DataSet = daClientes.Cliente_recuperarID(HF_CLIE_ID.Value)

    TxtCtaCte.Text = ds_clieinfo.Tables(0).Rows(0).Item("CTACTE_ID")
    TxtDni.Text = ds_clieinfo.Tables(0).Rows(0).Item("CLIE_dni")
    TxtApellido.Text = ds_clieinfo.Tables(0).Rows(0).Item("CLIE_ape")
    TxtNombre.Text = ds_clieinfo.Tables(0).Rows(0).Item("CLIE_nom")
    TxtDireccion.Text = ds_clieinfo.Tables(0).Rows(0).Item("CLIE_direccion")
    TxtTelefono.Text = ds_clieinfo.Tables(0).Rows(0).Item("CLIE_telefono")
    TxtMail.Text = ds_clieinfo.Tables(0).Rows(0).Item("CLIE_mail")
    TxtObservacion.Text = ds_clieinfo.Tables(0).Rows(0).Item("CLIE_obs")

    'Dim DS_Cob_clientes As New DS_Cob_clientes

    'DS_Cob_clientes.Tables("Tarifa").Rows.Add(ds_clieinfo.Tables(1))


    'RECORRER EL datatable y cargar los dias para las tarifas que sean periodicas.
    Dim i As Integer = 0
    While i < ds_clieinfo.Tables(1).Rows.Count
      If ds_clieinfo.Tables(1).Rows(i).Item("TIPO").ToString = "PERIODICA" Then
        Dim TARCLIE_ID As Integer = ds_clieinfo.Tables(1).Rows(i).Item("TARCLIE_ID")
        Dim j As Integer = 0
        Dim item_add As Integer = 0
        Dim dias_semana As String = ""
        While j < ds_clieinfo.Tables(2).Rows.Count
          If TARCLIE_ID = ds_clieinfo.Tables(2).Rows(j).Item("TARCLIE_ID") Then
            If item_add = 0 Then
              dias_semana = ds_clieinfo.Tables(2).Rows(j).Item("TARCLIEDIA_desc").ToString
            Else
              dias_semana = dias_semana + "," + ds_clieinfo.Tables(2).Rows(j).Item("TARCLIEDIA_desc").ToString
            End If
            item_add = item_add + 1
          End If
          j = j + 1
        End While
        ds_clieinfo.Tables(1).Rows(i).Item("DIAS") = dias_semana
      End If
      i = i + 1
    End While



    GridView1.DataSource = ds_clieinfo.Tables(1)
    GridView1.DataBind()


    GridView1.Columns(0).Visible = False   '0 es columna TARCLIE_ID
    GridView1.Columns(1).Visible = False  '1 es columna TARIFA_ID
    GridView1.Columns(3).Visible = False  '3 es columna LOCAL_ID

    GridView1.Columns(5).Visible = False   '0 es columna SECTOR
    GridView1.Columns(6).Visible = False  '1 es columna PASILLO
    GridView1.Columns(7).Visible = False  '3 es columna LOCAL




  End Sub



  Private Sub Recuperar_Locales()
    Dim DS_Cob_local As New DS_Cob_local

    DS_Cob_local.Tables("Local").Rows.Clear()

    GridView2.Columns(0).Visible = True 'columna ID

    Dim ds_local As DataSet = daLocal.consultarsoloactivos

    DS_Cob_local.Tables("Local").Merge(ds_local.Tables(0))


    If Session("Clientes_OP") = "ALTA" Then
      'voy a quitar aquellos locales que esten asignados a otros clientes
      Dim ds_LocalesAsignados As DataTable = daLocal.ClienteLocal
      Dim i As Integer = 0
      While i < ds_LocalesAsignados.Rows.Count
        Dim LOCAL_ID As Integer = ds_LocalesAsignados.Rows(i).Item("LOCAL_ID")
        Dim j As Integer = 0
        While j < DS_Cob_local.Tables("Local").Rows.Count
          If LOCAL_ID = DS_Cob_local.Tables("Local").Rows(j).Item("LOCAL_ID") Then
            DS_Cob_local.Tables("Local").Rows.RemoveAt(j)
            Exit While
          End If
          j = j + 1
        End While
        i = i + 1
      End While


    End If

    If Session("Clientes_OP") = "MODIFICAR" Then
      'voy a quitar aquellos locales que esten asignados a otros clientes
      Dim ds_LocalesAsignados As DataTable = daLocal.ClienteLocal
      Dim i As Integer = 0
      While i < ds_LocalesAsignados.Rows.Count
        Dim LOCAL_ID As Integer = ds_LocalesAsignados.Rows(i).Item("LOCAL_ID")
        Dim CLIE_ID As Integer = ds_LocalesAsignados.Rows(i).Item("CLIE_ID")


        If CLIE_ID <> HF_CLIE_ID.Value Then
          Dim j As Integer = 0
          While j < DS_Cob_local.Tables("Local").Rows.Count
            If LOCAL_ID = DS_Cob_local.Tables("Local").Rows(j).Item("LOCAL_ID") Then
              DS_Cob_local.Tables("Local").Rows.RemoveAt(j)
              Exit While
            End If
            j = j + 1
          End While
        End If


        i = i + 1
      End While


    End If


    GridView2.DataSource = DS_Cob_local.Tables("Local")
    GridView2.DataBind()
    GridView2.Columns(0).Visible = False '0 es columna ID

  End Sub


  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/COB_Clientes/Cob_clientes.aspx")
  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick

    lb_error_Dni.Visible = False
    lb_error_Apellido.Visible = False
    lb_error_Nombre.Visible = False
    'los datos obligatorios son: dni, apellido y nombre.
    If HF_CLIE_ID.Value = 0 Then
      '------ALTA---------
      Dim valido_cliente As String = "si"
      Try
        Dim dni As Integer = CInt(TxtDni.Text)
      Catch ex As Exception
        valido_cliente = "no"
        lb_error_Dni.Visible = True
      End Try
      Try
        If TxtApellido.Text = "" Then
          lb_error_Apellido.Visible = True
          valido_cliente = "no"
        End If
      Catch ex As Exception
        lb_error_Apellido.Visible = True
        valido_cliente = "no"
      End Try
      Try
        If TxtNombre.Text = "" Then
          lb_error_Nombre.Visible = True
          valido_cliente = "no"
        End If
      Catch ex As Exception
        lb_error_Nombre.Visible = True
        valido_cliente = "no"
      End Try
      If valido_cliente = "si" Then
        'ahora valido que no exista el dni.
        Dim ds_cliedni As DataSet = daClientes.Clientes_buscaractivodni(CInt(TxtDni.Text))
        If ds_cliedni.Tables(0).Rows.Count = 0 Then

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_GRABAR", "$(document).ready(function () {$('#Mdl_GRABAR').modal();});", True)

        Else
          'aqui mensaje el cliente ya existe.

          'modal_sn_okerrorvarios
          lb_error_Dni.Visible = True
          Label_errorvarios.Text = "El cliente ya existe. Modifique."
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)

        End If


      Else
        'aqui mensaje que complete la info solicitada.
        'modal_sn_okerrorvarios
        Label_errorvarios.Text = "Complete la info. Solicitada."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
      End If


    End If

    If HF_CLIE_ID.Value <> 0 Then
      Dim valido_cliente As String = "si"

      Try
        Dim dni As Integer = CInt(TxtDni.Text)
      Catch ex As Exception
        valido_cliente = "no"
        lb_error_Dni.Visible = True
      End Try
      Try
        If TxtApellido.Text = "" Then
          lb_error_Apellido.Visible = True
          valido_cliente = "no"
        End If
      Catch ex As Exception
        lb_error_Apellido.Visible = True
        valido_cliente = "no"
      End Try
      Try
        If TxtNombre.Text = "" Then
          lb_error_Nombre.Visible = True
          valido_cliente = "no"
        End If
      Catch ex As Exception
        lb_error_Nombre.Visible = True
        valido_cliente = "no"
      End Try
      If valido_cliente = "si" Then
        'ahora valido que no exista el dni.
        Dim ds_cliedni As DataSet = daClientes.Clientes_buscaractivodni(CInt(TxtDni.Text))
        Dim valido_dni As String = "si"

        Dim i As Integer = 0
        While i < ds_cliedni.Tables(0).Rows.Count
          If HF_CLIE_ID.Value <> ds_cliedni.Tables(0).Rows(i).Item("CLIE_ID") Then
            valido_dni = "no"
            Exit While
          End If
          i = i + 1
        End While
        If valido_dni = "si" Then

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_GRABAR", "$(document).ready(function () {$('#Mdl_GRABAR').modal();});", True)

        Else
          'aqui mensaje el cliente ya existe.

          'modal_sn_okerrorvarios
          lb_error_Dni.Visible = True
          Label_errorvarios.Text = "El cliente ya existe. Modifique."
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)

        End If



      Else
          'aqui mensaje que complete la info solicitada.
          'modal_sn_okerrorvarios
          Label_errorvarios.Text = "Complete la info. Solicitada."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)

      End If

    End If






  End Sub

  Private Sub btn_ok_errorvarios_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_errorvarios.ServerClick
    If Label_errorvarios.Text = "Complete la info. Solicitada." Then

      If lb_error_Dni.Visible = True Then
        TxtDni.Focus()
      Else
        If lb_error_Apellido.Visible = True Then
          TxtApellido.Focus()
        Else
          If lb_error_Nombre.Visible = True Then
            TxtNombre.Focus()
          End If
        End If
      End If


    End If
    'El cliente ya existe. Modifique.
    If Label_errorvarios.Text = "El cliente ya existe. Modifique" Then
      TxtDni.Focus()

    End If




  End Sub

  Private Sub btn_errorvarios_close_ServerClick(sender As Object, e As EventArgs) Handles btn_errorvarios_close.ServerClick
    'mismo codigo que btn_ok_errorvarios_ServerClick
    If Label_errorvarios.Text = "Complete la info. Solicitada." Then

      If lb_error_Dni.Visible = True Then
        TxtDni.Focus()
      Else
        If lb_error_Apellido.Visible = True Then
          TxtApellido.Focus()
        Else
          If lb_error_Nombre.Visible = True Then
            TxtNombre.Focus()
          End If
        End If
      End If


    End If
    'El cliente ya existe. Modifique.
    If Label_errorvarios.Text = "El cliente ya existe. Modifique" Then
      TxtDni.Focus()

    End If


  End Sub

  Private Sub Btn_grabar_confirmar_ServerClick(sender As Object, e As EventArgs) Handles Btn_grabar_confirmar.ServerClick
    'aqui todo el codigo para grabar
    '------PROCESO DE ALTA-----------------------


    If HF_CLIE_ID.Value = 0 Then

      Dim ds_clie As DataSet = daClientes.Clientes_alta(TxtDni.Text, TxtApellido.Text, TxtNombre.Text, TxtDireccion.Text, TxtTelefono.Text, TxtMail.Text, TxtObservacion.Text)
      Dim CLIE_ID As Integer = ds_clie.Tables(0).Rows(0).Item("CLIE_ID")

      'doy de alta una ctacte para el cliente nuevo.
      daCtaCte.CtaCte_alta(CLIE_ID, Today)

      'ahora ingreso las tarifas.
      GridView1.Columns(0).Visible = True    '0 es columna TARCLIE_ID
      GridView1.Columns(1).Visible = True  '1 es columna TARIFA_ID
      GridView1.Columns(3).Visible = True  '3 es columna LOCAL_ID

      Dim i As Integer = 0
      While i < GridView1.Rows.Count
        Dim TARIFA_ID As Integer = CInt(GridView1.Rows(i).Cells(1).Text)
        Dim TARCLIE_precio As Decimal = CDec(GridView1.Rows(i).Cells(10).Text)
        Dim TARCLIE_tipo As String = GridView1.Rows(i).Cells(8).Text
        'Dim TARCLIE_dias As Integer = CInt(GridView1.Rows(i).Cells(9).Text)
        Dim TARCLIE_desc As String = GridView1.Rows(i).Cells(2).Text
        'Dim TARCLIE_fechainicio As String = GridView1.Rows(i).Cells(11).Text
        Dim TARCLIE_fechainicio As Date = CDate(GridView1.Rows(i).Cells(11).Text)
        Dim ds_tarclie As DataSet = daTarifa.TarifaCliente_alta(TARIFA_ID, CLIE_ID, TARCLIE_precio, TARCLIE_precio, TARCLIE_tipo, TARCLIE_desc, CDate(TARCLIE_fechainicio))

        Dim TARCLIE_ID As Integer = ds_tarclie.Tables(0).Rows(0).Item("TARCLIE_ID")

        'SI LA TARIFA ES PERIODICA VAMOS A GUARDAR LOS DIAS QUE SE INDICARON.

        If TARCLIE_tipo = "PERIODICA" Then
          'Dim TARCLIEDIA_desc As String = ""
          Dim Dias As String() = GridView1.Rows(i).Cells(9).Text.Split(",")
          If Dias.Count <> 0 Then
            Dim i2 As Integer = 0
            While i2 < Dias.Count
              Dim dia_de_semana As String = Dias(i2)
              If dia_de_semana <> "" Then
                daTarifa.TarifaClienteDia_alta(TARCLIE_ID, dia_de_semana)
              End If
              i2 = i2 + 1
            End While
          End If
        End If

        '================================================================================================
        'aqui agregamos un registro en la tabla ClienteLocal para vincular el local x unica vez.
        Dim LOCAL_ID As Integer = CInt(GridView1.Rows(i).Cells(3).Text)
        'primero verifico si no está ya vinculada
        Dim ds_local As DataSet = daLocal.ClienteLocal_buscar(CLIE_ID, LOCAL_ID)
        If ds_local.Tables(0).Rows.Count = 0 Then
          'LO AGREGO
          daLocal.ClienteLocal_alta(CLIE_ID, LOCAL_ID)
        End If
        '================================================================================================

        i = i + 1
      End While

      GridView1.Columns(0).Visible = False     '0 es columna TARCLIE_ID
      GridView1.Columns(1).Visible = False  '1 es columna TARIFA_ID
      GridView1.Columns(3).Visible = False  '3 es columna LOCAL_ID

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)

    End If

    '-----PROCEDO MODIFICAR
    If HF_CLIE_ID.Value <> 0 Then

      'ya se valido el dni. entonces solo guardo
      daClientes.Clientes_modificar(HF_CLIE_ID.Value, TxtDni.Text, TxtApellido.Text, TxtNombre.Text, TxtDireccion.Text, TxtTelefono.Text, TxtMail.Text, TxtObservacion.Text)

      Dim ds_tarifasexistentes As DataSet = daClientes.Cliente_recuperarID(HF_CLIE_ID.Value)


      'primero veo cuales se modifican y cuales se agregan.
      GridView1.Columns(0).Visible = True   '0 es columna TARCLIE_ID
      GridView1.Columns(1).Visible = True  '1 es columna TARIFA_ID
      GridView1.Columns(3).Visible = True  '3 es columna LOCAL_ID

      Dim i As Integer = 0
      While i < GridView1.Rows.Count
        Dim TARCLIE_ID As Integer = CInt(GridView1.Rows(i).Cells(0).Text)
        If TARCLIE_ID = 0 Then
          'es alta
          Dim TARIFA_ID As Integer = CInt(GridView1.Rows(i).Cells(1).Text)
          Dim TARCLIE_precio As Decimal = CDec(GridView1.Rows(i).Cells(10).Text)
          Dim TARCLIE_tipo As String = GridView1.Rows(i).Cells(8).Text
          'Dim TARCLIE_dias As Integer = CInt(GridView1.Rows(i).Cells(9).Text)
          Dim TARCLIE_desc As String = GridView1.Rows(i).Cells(2).Text
          Dim TARCLIE_fechainicio As Date = CDate(GridView1.Rows(i).Cells(11).Text)

          Dim ds_tarclie As DataSet = daTarifa.TarifaCliente_alta(TARIFA_ID, HF_CLIE_ID.Value, TARCLIE_precio, TARCLIE_precio, TARCLIE_tipo, TARCLIE_desc, TARCLIE_fechainicio)

          Dim TARCLIE_ID1 As Integer = ds_tarclie.Tables(0).Rows(0).Item("TARCLIE_ID")

          'SI LA TARIFA ES PERIODICA VAMOS A GUARDAR LOS DIAS QUE SE INDICARON.

          If TARCLIE_tipo = "PERIODICA" Then
            'Dim TARCLIEDIA_desc As String = ""
            Dim Dias As String() = GridView1.Rows(i).Cells(9).Text.Split(",")
            If Dias.Count <> 0 Then
              Dim i2 As Integer = 0
              While i2 < Dias.Count
                Dim dia_de_semana As String = Dias(i2)
                If dia_de_semana <> "" Then
                  daTarifa.TarifaClienteDia_alta(TARCLIE_ID1, dia_de_semana)
                End If
                i2 = i2 + 1
              End While
            End If
          End If




        Else
          'se modifica...no voy a validar si hay otro igual ya q para esta instancia esto ya deberia estar validado.
          'X AHORA NO CONSIDERO EDITAR ALGO DEL GRID. NO SE COMO AFECTA A LAS TARIFA YA ASIGNADAS A UN CLIENTE.
          'daPasillo.Pasillo_modificar(PASILLO_ID, GridView1.Rows(i).Cells(1).Text.ToUpper)
        End If

        '================================================================================================
        'aqui agregamos un registro en la tabla ClienteLocal para vincular el local x unica vez.
        Dim LOCAL_ID As Integer = CInt(GridView1.Rows(i).Cells(3).Text)
        'primero verifico si no está ya vinculada
        Dim ds_local As DataSet = daLocal.ClienteLocal_buscar(HF_CLIE_ID.Value, LOCAL_ID)
        If ds_local.Tables(0).Rows.Count = 0 Then
          'LO AGREGO
          daLocal.ClienteLocal_alta(HF_CLIE_ID.Value, LOCAL_ID)
        End If
        '================================================================================================

        i = i + 1



      End While

      'ahora quito aquellos q en la bd estan pero no asi en el grid.
      'la eliminacion es logica, estado= inactivo.

      i = 0
      While i < ds_tarifasexistentes.Tables(1).Rows.Count
        Dim TARCLIE_ID As Integer = ds_tarifasexistentes.Tables(1).Rows(i).Item("TARCLIE_ID")
        Dim existe = "no"
        Dim j As Integer = 0
        While j < GridView1.Rows.Count
          If TARCLIE_ID = GridView1.Rows(j).Cells(0).Text Then
            existe = "si"
            Exit While
          End If
          j = j + 1
        End While
        If existe = "no" Then
          'lo elimino logicamente
          daTarifa.TarifaCliente_EstadoModificar(TARCLIE_ID, "inactivo eliminado")

        End If
        i = i + 1
      End While

      Dim ds_localesvinculados As DataSet = daLocal.ClienteLocal_buscar2(HF_CLIE_ID.Value)
      i = 0
      While i < ds_localesvinculados.Tables(0).Rows.Count
        Dim AUXLOCAL_ID As Integer = ds_localesvinculados.Tables(0).Rows(i).Item("LOCAL_ID")
        Dim J As Integer = 0
        Dim EXISTE = "NO"
        While J < GridView1.Rows.Count
          If AUXLOCAL_ID = CInt(GridView1.Rows(J).Cells(3).Text) Then
            EXISTE = "SI"
            Exit While
          End If
          J = J + 1
        End While
        If EXISTE = "NO" Then
          'DESVINCULO EL LOCAL EN LA TABLA CLIENTELOCAL
          daLocal.ClienteLocal_eliminarlocal(HF_CLIE_ID.Value, CStr(AUXLOCAL_ID))
        End If
        i = i + 1
      End While

      GridView1.Columns(0).Visible = False     '0 es columna TARCLIE_ID
      GridView1.Columns(1).Visible = False  '1 es columna TARIFA_ID
      GridView1.Columns(3).Visible = False  '3 es columna LOCAL_ID

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)


    End If




  End Sub

  Private Sub btn_ok_ServerClick(sender As Object, e As EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Clientes/Cob_clientes.aspx")
  End Sub

  Private Sub btn_graba_close_ServerClick(sender As Object, e As EventArgs) Handles btn_graba_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Clientes/Cob_clientes.aspx")
  End Sub

  Private Sub GridView2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand


    Dim DS_Cob_clientes As New DS_Cob_clientes
    DS_Cob_clientes.Tables("Tarifa").Rows.Clear()
    'LOCAL_ID
    If (e.CommandName = "ID1") Then
      'GridView1.Columns(0).Visible = True  '0 es columna TARCLIE_ID
      'GridView1.Columns(1).Visible = True  '1 es columna TARIFA_ID
      'GridView1.Columns(3).Visible = True  '3 es columna LOCAL_ID


      ' Retrieve the row index stored in the CommandArgument property.
      'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      Dim LOCAL_ID As Integer = Integer.Parse(e.CommandArgument.ToString())

      HF_ASIG_LOCAL_ID.Value = LOCAL_ID

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_ASIGNAR", "$(document).ready(function () {$('#Mdl_ASIGNAR').modal();});", True)


    End If

    If (e.CommandName = "ID2") Then

      GridView_det.Columns(0).Visible = True  '0 es columna TARCLIE_ID
      GridView_det.Columns(1).Visible = True '0 es columna TARIFA_ID
      GridView_det.Columns(3).Visible = True '0 es columna LOCAL_ID
      GridView_det.Columns(4).Visible = True '0 es columna Codigo
      GridView_det.Columns(5).Visible = True '0 es columna Sector
      GridView_det.Columns(6).Visible = True '0 es columna Pasillo
      GridView_det.Columns(7).Visible = True '0 es columna Local

      ' Retrieve the row index stored in the CommandArgument property.
      'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      Dim LOCAL_ID As Integer = Integer.Parse(e.CommandArgument.ToString())

      Dim ds_tarifas As DataSet = daTarifa.Tarifa_recuperartodolocal(LOCAL_ID)
      'cargo en el gridview_det todas las tarifas activas para el local seleccionado.

      Dim i As Integer = 0
      While i < ds_tarifas.Tables(0).Rows.Count
        Dim fila As DataRow = DS_Cob_clientes.Tables("Tarifa").NewRow
        fila("TARCLIE_ID") = "0"
        fila("TARIFA_ID") = ds_tarifas.Tables(0).Rows(i).Item("TARIFA_ID")
        fila("TARIFA") = ds_tarifas.Tables(0).Rows(i).Item("TARIFA")
        fila("LOCAL_ID") = ds_tarifas.Tables(0).Rows(i).Item("LOCAL_ID")
        fila("Codigo") = ds_tarifas.Tables(0).Rows(i).Item("Codigo")
        fila("Sector") = ds_tarifas.Tables(0).Rows(i).Item("Sector")
        fila("Pasillo") = ds_tarifas.Tables(0).Rows(i).Item("Pasillo")
        fila("Local") = ds_tarifas.Tables(0).Rows(i).Item("Local")
        fila("TIPO") = ds_tarifas.Tables(0).Rows(i).Item("TIPO")
        'fila("DIAS") = ds_tarifas.Tables(0).Rows(i).Item("DIAS")
        fila("PRECIO") = ds_tarifas.Tables(0).Rows(i).Item("PRECIO")
        DS_Cob_clientes.Tables("Tarifa").Rows.Add(fila)
        i = i + 1
      End While

      GridView_det.DataSource = DS_Cob_clientes.Tables("Tarifa")
      GridView_det.DataBind()

      'oculto ciertas columnas.
      GridView_det.Columns(0).Visible = False '0 es columna TARCLIE_ID
      GridView_det.Columns(1).Visible = False '0 es columna TARIFA_ID
      GridView_det.Columns(3).Visible = False '0 es columna LOCAL_ID
      GridView_det.Columns(4).Visible = False '0 es columna Codigo
      GridView_det.Columns(5).Visible = False '0 es columna Sector
      GridView_det.Columns(6).Visible = False '0 es columna Pasillo
      GridView_det.Columns(7).Visible = False '0 es columna Local

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_DETALLE", "$(document).ready(function () {$('#Mdl_DETALLE').modal();});", True)

    End If



  End Sub

  Private Sub GridView_det_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView_det.RowCommand
    'Dim DS_Cob_clientes As New DS_Cob_clientes

    'If (e.CommandName = "ID") Then
    '  ' Retrieve the row index stored in the CommandArgument property.
    '  'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '  Dim TARIFA_ID As Integer = Integer.Parse(e.CommandArgument.ToString())


    '  Dim i As Integer = 0
    '  While i < GridView_det.Rows.Count
    '    If GridView_det.Rows(i).Cells(1).Text <> TARIFA_ID Then
    '      Dim fila As DataRow = DS_Cob_clientes.Tables("Tarifa").NewRow
    '      fila("TARCLIE_ID") = GridView_det.Rows(i).Cells(0).Text
    '      fila("TARIFA_ID") = GridView_det.Rows(i).Cells(1).Text
    '      fila("TARIFA") = GridView_det.Rows(i).Cells(2).Text
    '      fila("LOCAL_ID") = GridView_det.Rows(i).Cells(3).Text
    '      fila("Codigo") = GridView_det.Rows(i).Cells(4).Text
    '      fila("Sector") = GridView_det.Rows(i).Cells(5).Text
    '      fila("Pasillo") = GridView_det.Rows(i).Cells(6).Text
    '      fila("Local") = GridView_det.Rows(i).Cells(7).Text
    '      fila("TIPO") = GridView_det.Rows(i).Cells(8).Text
    '      fila("DIAS") = GridView_det.Rows(i).Cells(9).Text
    '      fila("PRECIO") = GridView_det.Rows(i).Cells(10).Text
    '      DS_Cob_clientes.Tables("Tarifa").Rows.Add(fila)
    '    End If
    '    i = i + 1
    '  End While

    '  GridView_det.DataSource = DS_Cob_clientes.Tables("Tarifa")
    '  GridView_det.DataBind()

    '  'oculto ciertas columnas.
    '  GridView_det.Columns(0).Visible = False '0 es columna TARCLIE_ID
    '  GridView_det.Columns(1).Visible = False '0 es columna TARIFA_ID
    '  GridView_det.Columns(3).Visible = False '0 es columna LOCAL_ID
    '  GridView_det.Columns(4).Visible = False '0 es columna Codigo
    '  GridView_det.Columns(5).Visible = False '0 es columna Sector
    '  GridView_det.Columns(6).Visible = False '0 es columna Pasillo
    '  GridView_det.Columns(7).Visible = False '0 es columna Local

    '  ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_DETALLE", "$(document).ready(function () {$('#Mdl_DETALLE').modal();});", True)



    'End If


  End Sub

  Private Sub Btn_detalle_asignar_ServerClick(sender As Object, e As EventArgs) Handles Btn_detalle_asignar.ServerClick



    GridView_det.Columns(0).Visible = True '0 es columna TARCLIE_ID
    GridView_det.Columns(1).Visible = True '1 es columna TARIFA_ID
    GridView_det.Columns(3).Visible = True '3 es columna LOCAL_ID
    GridView_det.Columns(4).Visible = True '4 es columna Codigo
    GridView_det.Columns(5).Visible = True '5 es columna Sector
    GridView_det.Columns(6).Visible = True '6 es columna Pasillo
    GridView_det.Columns(7).Visible = True '7 es columna Local

    GridView1.Columns(0).Visible = True  '0 es columna TARCLIE_ID
    GridView1.Columns(1).Visible = True  '1 es columna TARIFA_ID
    GridView1.Columns(3).Visible = True  '3 es columna LOCAL_ID


    Dim DS_Cob_clientes As New DS_Cob_clientes


    'DS_Cob_clientes.Tables("Tarifa").Rows.Add(GridView_det.DataSource)

    'recorro el gridview_det y veo que filas estan chequeadas
    Dim valido = "no"
    Dim i As Integer = 0

    Dim contvalidos As Integer = 0
      While i < GridView_det.Rows.Count
      Dim check1 As CheckBox = GridView_det.Rows(i).Cells(10).FindControl("CheckBox_det")

      If check1.Checked = True Then
          valido = "si"
          'veo si se puede agregar
          Dim j As Integer = 0
          While j < GridView1.Rows.Count
          If CInt(GridView1.Rows(j).Cells(1).Text) = CInt(GridView_det.Rows(i).Cells(1).Text) Then
            valido = "no"
            Exit While
          End If
          j = j + 1
          End While
          If valido = "si" Then
            contvalidos = contvalidos + 1
          End If
        End If
        i = i + 1
      End While

      If contvalidos <> 0 Then 'aqui viene la carga real
        i = 0
        While i < GridView1.Rows.Count
          Dim fila As DataRow = DS_Cob_clientes.Tables("Tarifa").NewRow
        fila("TARCLIE_ID") = CInt(GridView1.Rows(i).Cells(0).Text)
        fila("TARIFA_ID") = CInt(GridView1.Rows(i).Cells(1).Text)
        fila("TARIFA") = GridView1.Rows(i).Cells(2).Text
        fila("LOCAL_ID") = CInt(GridView1.Rows(i).Cells(3).Text)
        fila("Codigo") = GridView1.Rows(i).Cells(4).Text
          fila("Sector") = GridView1.Rows(i).Cells(5).Text
          fila("Pasillo") = GridView1.Rows(i).Cells(6).Text
          fila("Local") = GridView1.Rows(i).Cells(7).Text
          fila("TIPO") = GridView1.Rows(i).Cells(8).Text
        If GridView1.Rows(i).Cells(8).Text = "UNICA" Then
          fila("DIAS") = ""
        Else
          fila("DIAS") = GridView1.Rows(i).Cells(9).Text
        End If

        fila("PRECIO") = GridView1.Rows(i).Cells(10).Text
        fila("Fechainicio") = GridView1.Rows(i).Cells(11).Text
        DS_Cob_clientes.Tables("Tarifa").Rows.Add(fila)
          i = i + 1
        End While
        i = 0
        While i < GridView_det.Rows.Count
        Dim check1 As CheckBox = GridView_det.Rows(i).Cells(10).FindControl("CheckBox_det")

        If check1.Checked = True Then
            valido = "si"
            'veo si se puede agregar
            Dim j As Integer = 0

          'SI EXISTE, NO LO AGREGO
          While j < GridView1.Rows.Count
            If CInt(GridView1.Rows(j).Cells(1).Text) = CInt(GridView_det.Rows(i).Cells(1).Text) Then
              valido = "no"
              Exit While
            End If
            j = j + 1
          End While

          'SI LA TARIFA CORRESPONDE A UN LOCAL ASINAGNADO A OTRO CLIENTE, TAMPOCO AGREGO.
          Dim ds_localasig As DataSet = daLocal.ClienteLocal_buscar3(CStr(GridView_det.Rows(i).Cells(3).Text))
          j = 0
          If (HF_CLIE_ID.Value = 0) And (ds_localasig.Tables(0).Rows.Count <> 0) Then
            valido = "no"
          Else
            If HF_CLIE_ID.Value <> 0 Then
              While j < ds_localasig.Tables(0).Rows.Count
                If CInt(GridView_det.Rows(i).Cells(3).Text) = ds_localasig.Tables(0).Rows(j).Item("LOCAL_ID") Then
                  If HF_CLIE_ID.Value <> ds_localasig.Tables(0).Rows(j).Item("CLIE_ID") Then
                    valido = "no"
                    Exit While
                  End If
                End If
                j = j + 1
              End While
            End If
          End If

          If valido = "si" Then
              Dim fila As DataRow = DS_Cob_clientes.Tables("Tarifa").NewRow
              fila("TARCLIE_ID") = "0"
            fila("TARIFA_ID") = CInt(GridView_det.Rows(i).Cells(1).Text)
            fila("TARIFA") = GridView_det.Rows(i).Cells(2).Text
            fila("LOCAL_ID") = CInt(GridView_det.Rows(i).Cells(3).Text)
            fila("Codigo") = GridView_det.Rows(i).Cells(4).Text
              fila("Sector") = GridView_det.Rows(i).Cells(5).Text
              fila("Pasillo") = GridView_det.Rows(i).Cells(6).Text
              fila("Local") = GridView_det.Rows(i).Cells(7).Text
              fila("TIPO") = GridView_det.Rows(i).Cells(8).Text
            If GridView_det.Rows(i).Cells(8).Text = "PERIODICA" Then
              Dim DIAS_string As String = ""
              Dim dias_ad As Integer = 0
              If ChkLunes.Checked = True Then
                DIAS_string = "Lu"
                dias_ad = dias_ad + 1
              End If
              If ChkMartes.Checked = True Then
                If dias_ad > 0 Then
                  DIAS_string = DIAS_string + "," + "Ma"
                Else
                  DIAS_string = "Ma"
                End If
                dias_ad = dias_ad + 1
              End If
              If ChkMiercoles.Checked = True Then
                If dias_ad > 0 Then
                  DIAS_string = DIAS_string + "," + "Mi"
                Else
                  DIAS_string = "Mi"
                End If
                dias_ad = dias_ad + 1
              End If
              If ChkJueves.Checked = True Then
                If dias_ad > 0 Then
                  DIAS_string = DIAS_string + "," + "Ju"
                Else
                  DIAS_string = "Ju"
                End If
                dias_ad = dias_ad + 1
              End If
              If ChkViernes.Checked = True Then
                If dias_ad > 0 Then
                  DIAS_string = DIAS_string + "," + "Vi"
                Else
                  DIAS_string = "Vi"
                End If
                dias_ad = dias_ad + 1
              End If
              If ChkSabado.Checked = True Then
                If dias_ad > 0 Then
                  DIAS_string = DIAS_string + "," + "Sa"
                Else
                  DIAS_string = "Sa"
                End If
                dias_ad = dias_ad + 1
              End If
              If ChkDomingo.Checked = True Then
                If dias_ad > 0 Then
                  DIAS_string = DIAS_string + "," + "Do"
                Else
                  DIAS_string = "Do"
                End If
                dias_ad = dias_ad + 1
              End If
              fila("DIAS") = DIAS_string ' aqui tengo que ver que se checkeo y concatenar, separado por comas. GridView_det.Rows(i).Cells(9).Text
            Else
              fila("DIAS") = ""
            End If


            fila("PRECIO") = GridView_det.Rows(i).Cells(9).Text

            Try
              fila("Fechainicio") = CDate(Txt_det_fecha.Text).ToShortDateString.ToString
            Catch ex As Exception
              fila("Fechainicio") = Today.ToShortDateString.ToString
            End Try


            DS_Cob_clientes.Tables("Tarifa").Rows.Add(fila)

          End If
          End If

          i = i + 1
        End While
        GridView1.DataSource = DS_Cob_clientes.Tables("Tarifa")
        GridView1.DataBind()

      End If


    GridView_det.Columns(0).Visible = False '0 es columna TARCLIE_ID
    GridView_det.Columns(1).Visible = False '1 es columna TARIFA_ID
    GridView_det.Columns(3).Visible = False '3 es columna LOCAL_ID
    GridView_det.Columns(4).Visible = False '4 es columna Codigo
    GridView_det.Columns(5).Visible = False '5 es columna Sector
    GridView_det.Columns(6).Visible = False '6 es columna Pasillo
    GridView_det.Columns(7).Visible = False '7 es columna Local

    GridView1.Columns(0).Visible = False   '0 es columna TARCLIE_ID
    GridView1.Columns(1).Visible = False  '1 es columna TARIFA_ID
    GridView1.Columns(3).Visible = False  '3 es columna LOCAL_ID

    GridView1.Columns(5).Visible = False   '0 es columna SECTOR
    GridView1.Columns(6).Visible = False  '1 es columna PASILLO
    GridView1.Columns(7).Visible = False  '3 es columna LOCAL


  End Sub

  Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
    If (e.CommandName = "ID") Then
      ' Retrieve the row index stored in the CommandArgument property.
      'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      Dim TARIFA_ID As Integer = Integer.Parse(e.CommandArgument.ToString())

      GridView1.Columns(0).Visible = True    '0 es columna TARCLIE_ID
      GridView1.Columns(1).Visible = True  '1 es columna TARIFA_ID
      GridView1.Columns(3).Visible = True  '3 es columna LOCAL_ID

      Dim i As Integer = 0
      Dim DS_Cob_clientes As New DS_Cob_clientes
      DS_Cob_clientes.Tables("Tarifa").Rows.Clear()

      While i < GridView1.Rows.Count
        If CInt(GridView1.Rows(i).Cells(1).Text) <> TARIFA_ID Then
          Dim fila As DataRow = DS_Cob_clientes.Tables("Tarifa").NewRow
          fila("TARCLIE_ID") = CInt(GridView1.Rows(i).Cells(0).Text)
          fila("TARIFA_ID") = CInt(GridView1.Rows(i).Cells(1).Text)
          fila("TARIFA") = GridView1.Rows(i).Cells(2).Text
          fila("LOCAL_ID") = CInt(GridView1.Rows(i).Cells(3).Text)
          fila("Codigo") = GridView1.Rows(i).Cells(4).Text
          fila("Sector") = GridView1.Rows(i).Cells(5).Text
          fila("Pasillo") = GridView1.Rows(i).Cells(6).Text
          fila("Local") = GridView1.Rows(i).Cells(7).Text
          fila("TIPO") = GridView1.Rows(i).Cells(8).Text
          If GridView1.Rows(i).Cells(8).Text = "UNICA" Then
            fila("DIAS") = ""
          Else
            fila("DIAS") = GridView1.Rows(i).Cells(9).Text
          End If
          fila("PRECIO") = GridView1.Rows(i).Cells(10).Text
          fila("Fechainicio") = GridView1.Rows(i).Cells(11).Text
          DS_Cob_clientes.Tables("Tarifa").Rows.Add(fila)
        End If
        i = i + 1
      End While
      GridView1.DataSource = DS_Cob_clientes.Tables("Tarifa")
      GridView1.DataBind()

      GridView1.Columns(0).Visible = False   '0 es columna TARCLIE_ID
      GridView1.Columns(1).Visible = False  '1 es columna TARIFA_ID
      GridView1.Columns(3).Visible = False  '3 es columna LOCAL_ID
    End If


  End Sub

  Private Sub Btn_asig_confirmar_ServerClick(sender As Object, e As EventArgs) Handles Btn_asig_confirmar.ServerClick
    GridView1.Columns(0).Visible = True    '0 es columna TARCLIE_ID
    GridView1.Columns(1).Visible = True  '1 es columna TARIFA_ID
    GridView1.Columns(3).Visible = True  '3 es columna LOCAL_ID

    GridView1.Columns(5).Visible = True   '0 es columna SECTOR
    GridView1.Columns(6).Visible = True  '1 es columna PASILLO
    GridView1.Columns(7).Visible = True  '3 es columna LOCAL

    Dim DS_Cob_clientes As New DS_Cob_clientes


    'cargo en el gridview1 todas las tarifas activas para el local seleccionado.

    Dim LOCAL_ID As Integer = HF_ASIG_LOCAL_ID.Value


    Dim ds_tarifas As DataSet = daTarifa.Tarifa_recuperartodolocal(LOCAL_ID)

    'valido que no existan tarifas cargadas en el gridview1 para ese local.

    Dim valido As String = "si"
    Dim i As Integer = 0
    While i < GridView1.Rows.Count
      If GridView1.Rows(i).Cells(3).Text = LOCAL_ID Then
        valido = "no"
        Exit While
      End If
      i = i + 1
    End While

    'SI LA TARIFA CORRESPONDE A UN LOCAL ASINAGNADO A OTRO CLIENTE, TAMPOCO AGREGO.
    Dim ds_localasig As DataSet = daLocal.ClienteLocal_buscar3(CStr(LOCAL_ID))
    Dim j As Integer = 0
    If (HF_CLIE_ID.Value = 0) And (ds_localasig.Tables(0).Rows.Count <> 0) Then
      valido = "no"
    Else
      If HF_CLIE_ID.Value <> 0 Then
        While j < ds_localasig.Tables(0).Rows.Count
          If LOCAL_ID = ds_localasig.Tables(0).Rows(j).Item("LOCAL_ID") Then
            If HF_CLIE_ID.Value <> ds_localasig.Tables(0).Rows(j).Item("CLIE_ID") Then
              valido = "no"
              Exit While
            End If
          End If
          j = j + 1
        End While
      End If
    End If



    If valido = "si" Then
      i = 0
      While i < GridView1.Rows.Count
        Dim fila As DataRow = DS_Cob_clientes.Tables("Tarifa").NewRow
        fila("TARCLIE_ID") = GridView1.Rows(i).Cells(0).Text
        fila("TARIFA_ID") = GridView1.Rows(i).Cells(1).Text
        fila("TARIFA") = GridView1.Rows(i).Cells(2).Text
        fila("LOCAL_ID") = GridView1.Rows(i).Cells(3).Text
        fila("Codigo") = GridView1.Rows(i).Cells(4).Text
        fila("Sector") = GridView1.Rows(i).Cells(5).Text
        fila("Pasillo") = GridView1.Rows(i).Cells(6).Text
        fila("Local") = GridView1.Rows(i).Cells(7).Text
        fila("TIPO") = GridView1.Rows(i).Cells(8).Text
        If GridView1.Rows(i).Cells(8).Text = "UNICA" Then
          fila("DIAS") = ""
        Else
          fila("DIAS") = GridView1.Rows(i).Cells(9).Text
        End If

        fila("PRECIO") = GridView1.Rows(i).Cells(10).Text
        fila("Fechainicio") = GridView1.Rows(i).Cells(11).Text
        DS_Cob_clientes.Tables("Tarifa").Rows.Add(fila)
        i = i + 1
      End While

      'AHORA AGREGAMOS AL GRIDVIEW LAS N TARIFAS
      i = 0
      While i < ds_tarifas.Tables(0).Rows.Count
        Dim fila As DataRow = DS_Cob_clientes.Tables("Tarifa").NewRow
        fila("TARCLIE_ID") = "0"
        fila("TARIFA_ID") = ds_tarifas.Tables(0).Rows(i).Item("TARIFA_ID")
        fila("TARIFA") = ds_tarifas.Tables(0).Rows(i).Item("TARIFA")
        fila("LOCAL_ID") = ds_tarifas.Tables(0).Rows(i).Item("LOCAL_ID")
        fila("Codigo") = ds_tarifas.Tables(0).Rows(i).Item("Codigo")
        fila("Sector") = ds_tarifas.Tables(0).Rows(i).Item("Sector")
        fila("Pasillo") = ds_tarifas.Tables(0).Rows(i).Item("Pasillo")
        fila("Local") = ds_tarifas.Tables(0).Rows(i).Item("Local")
        fila("TIPO") = ds_tarifas.Tables(0).Rows(i).Item("TIPO")

        If ds_tarifas.Tables(0).Rows(i).Item("TIPO") = "PERIODICA" Then
          Dim DIAS_string As String = ""
          Dim dias_ad As Integer = 0
          If Chk1Lunes.Checked = True Then
            DIAS_string = "Lu"
            dias_ad = dias_ad + 1
          End If
          If Chk1Martes.Checked = True Then
            If dias_ad > 0 Then
              DIAS_string = DIAS_string + "," + "Ma"
            Else
              DIAS_string = "Ma"
            End If
            dias_ad = dias_ad + 1
          End If
          If Chk1Miercoles.Checked = True Then
            If dias_ad > 0 Then
              DIAS_string = DIAS_string + "," + "Mi"
            Else
              DIAS_string = "Mi"
            End If
            dias_ad = dias_ad + 1
          End If
          If Chk1Jueves.Checked = True Then
            If dias_ad > 0 Then
              DIAS_string = DIAS_string + "," + "Ju"
            Else
              DIAS_string = "Ju"
            End If
            dias_ad = dias_ad + 1
          End If
          If Chk1Viernes.Checked = True Then
            If dias_ad > 0 Then
              DIAS_string = DIAS_string + "," + "Vi"
            Else
              DIAS_string = "Vi"
            End If
            dias_ad = dias_ad + 1
          End If
          If Chk1Sabado.Checked = True Then
            If dias_ad > 0 Then
              DIAS_string = DIAS_string + "," + "Sa"
            Else
              DIAS_string = "Sa"
            End If
            dias_ad = dias_ad + 1
          End If
          If Chk1Domingo.Checked = True Then
            If dias_ad > 0 Then
              DIAS_string = DIAS_string + "," + "Do"
            Else
              DIAS_string = "Do"
            End If
            dias_ad = dias_ad + 1
          End If
          fila("DIAS") = DIAS_string ' aqui tengo que ver que se checkeo y concatenar, separado por comas. GridView_det.Rows(i).Cells(9).Text


        Else
          fila("DIAS") = ""
        End If


        fila("PRECIO") = ds_tarifas.Tables(0).Rows(i).Item("PRECIO")
        Try
          fila("Fechainicio") = CDate(Txt_asig_fecha.Text).ToShortDateString.ToString
        Catch ex As Exception
          fila("Fechainicio") = Today.ToShortDateString.ToString
        End Try


        DS_Cob_clientes.Tables("Tarifa").Rows.Add(fila)
        i = i + 1
      End While

      GridView1.DataSource = DS_Cob_clientes.Tables("Tarifa")
      GridView1.DataBind()

    Else
      'aqui mensaje que complete la info solicitada.
      'modal_sn_okerrorvarios
      Label_errorvarios.Text = "Local ya asignado."
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
    End If

    GridView1.Columns(0).Visible = False   '0 es columna TARCLIE_ID
    GridView1.Columns(1).Visible = False  '1 es columna TARIFA_ID
    GridView1.Columns(3).Visible = False  '3 es columna LOCAL_ID

    GridView1.Columns(5).Visible = False   '0 es columna SECTOR
    GridView1.Columns(6).Visible = False  '1 es columna PASILLO
    GridView1.Columns(7).Visible = False  '3 es columna LOCAL



  End Sub

  Private Sub btn_baja_mdll_ServerClick(sender As Object, e As EventArgs) Handles btn_baja_mdll.ServerClick

    If HF_CLIE_ID.Value <> 0 Then
      daClientes.ClienteLocalTarifas_baja(HF_CLIE_ID.Value)
    End If
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Clientes/Cob_clientes.aspx")


  End Sub

  Private Sub BOTON_CTACTE_ServerClick(sender As Object, e As EventArgs) Handles BOTON_CTACTE.ServerClick
    If (HF_CLIE_ID.Value <> 0) And (TxtCtaCte.Text <> "") Then

      Session("CLIE_ID") = HF_CLIE_ID.Value
      Session("CTACTE_ID") = TxtCtaCte.Text

      Response.Redirect("~/COB_CtaCte/Cob_ctacte.aspx")

    End If

  End Sub
End Class
