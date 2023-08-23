Public Class Cob_LocalAlta
  Inherits System.Web.UI.Page
  Dim daSector As New Capa_Datos.Sector
  Dim daPasillo As New Capa_Datos.Pasillo
  Dim daLocal As New Capa_Datos.Local
  Dim daTarifa As New Capa_Datos.Tarifa
  Dim DS_Cob_local As New DS_Cob_local

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos
      CargarSectorPasillos()

      TxtTarifaDias.Text = "1"
      TxtTarifaDias.ReadOnly = True

      If Session("Local_OP") = "ALTA" Then
        Session("Local_OP") = ""
        HF_LOCAL_ID.Value = 0

        DropDLSector.Focus()

        'gridvacio()
      End If
      If Session("LOCAL_OP") = "MODIFICAR" Then
        'MODIFICAR 
        Session("Local_OP") = ""
        HF_LOCAL_ID.Value = Session("LOCAL_ID")
        recuperar_info_local()


        DropDLSector.Enabled = False 'deshabilito, no se cambia esto.
        DropDLPasillo.Enabled = False 'deshabilito, no se cambia esto.
        TxtCodigo.Focus()

      End If

      'Obtener_usuarios()


    End If


  End Sub

  Private Sub recuperar_info_local()
    Dim ds_LocalInfo As DataSet = daLocal.LocalTarifas_recuperarID(HF_LOCAL_ID.Value)
    Try
      DropDLSector.SelectedValue = ds_LocalInfo.Tables(0).Rows(0).Item("SECTOR_ID")
    Catch ex As Exception
    End Try

    Try
      DropDLPasillo.SelectedValue = ds_LocalInfo.Tables(0).Rows(0).Item("PASILLO_ID")
    Catch ex As Exception
    End Try
    TxtCodigo.Text = ds_LocalInfo.Tables(0).Rows(0).Item("LOCAL_codigo")
    TxtLocal.Text = ds_LocalInfo.Tables(0).Rows(0).Item("LOCAL_desc")

    DS_Cob_local.Tables("Tarifa").Rows.Clear()
    GridView1.Columns(0).Visible = True 'columna TARIFA_ID
    GridView1.Columns(2).Visible = True 'columna LOCAL_ID

    Dim i As Integer = 0
    While i < ds_LocalInfo.Tables(1).Rows.Count
      Dim fila As DataRow = DS_Cob_local.Tables("Tarifa").NewRow
      fila("TARIFA_ID") = ds_LocalInfo.Tables(1).Rows(i).Item("TARIFA_ID")
      fila("LOCAL_ID") = ds_LocalInfo.Tables(1).Rows(i).Item("LOCAL_ID")
      fila("TIPO") = ds_LocalInfo.Tables(1).Rows(i).Item("TARIFA_tipo")
      'fila("DIAS") = ds_LocalInfo.Tables(1).Rows(i).Item("TARIFA_dias")
      fila("PRECIO") = ds_LocalInfo.Tables(1).Rows(i).Item("TARIFA_precio")
      fila("TARIFA") = ds_LocalInfo.Tables(1).Rows(i).Item("TARIFA_desc")
      DS_Cob_local.Tables("Tarifa").Rows.Add(fila)
      i = i + 1
    End While

    GridView1.DataSource = DS_Cob_local.Tables("Tarifa")
    GridView1.DataBind()
    GridView1.Columns(0).Visible = False '0 es columna ID
    GridView1.Columns(2).Visible = False 'columna SECTOR_ID




  End Sub



  Private Sub CargarSectorPasillos()
    Dim ds_sector As DataSet = daSector.ObtenerTodosActivos
    DropDLSector.DataSource = ds_sector.Tables(0)
    DropDLSector.DataTextField = "SECTOR_desc"
    DropDLSector.DataValueField = "SECTOR_ID"
    DropDLSector.DataBind()

    Dim ds_pasillo As DataSet = daSector.SectorPasillos_recuperarID(DropDLSector.SelectedValue)
    DropDLPasillo.DataSource = ds_pasillo.Tables(1)
    DropDLPasillo.DataTextField = "PASILLO_desc"
    DropDLPasillo.DataValueField = "PASILLO_ID"
    DropDLPasillo.DataBind()








  End Sub


  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Local/Cob_Local.aspx")
  End Sub

  Private Sub BtnAsignar_Click(sender As Object, e As EventArgs) Handles BtnAsignar.Click
    Lb_error_tarifadesc.Visible = False
    'Lb_error_tarifadias.Visible = False
    Lb_error_tarifaprecio.Visible = False

    GridView1.Columns(0).Visible = True 'columna TARIFA_ID
    GridView1.Columns(2).Visible = True 'columna LOCAL_ID

    Dim VALIDO As String = "si"

    Try
      If TxtTarifa.Text = "" Then
        VALIDO = "no"
        Lb_error_tarifadesc.Visible = True
      End If
    Catch ex As Exception

    End Try

    'Try
    '  If CInt(TxtTarifaDias.Text) <> CInt(0) Then
    '  Else
    '    VALIDO = "no"
    '    Lb_error_tarifadias.Visible = True
    '  End If
    'Catch ex As Exception
    '  VALIDO = "no"
    '  Lb_error_tarifadias.Visible = True
    'End Try

    Try
      If CDec(TxtTarifaPrecio.Text) <> CDec(0) Then
      Else
        VALIDO = "no"
        Lb_error_tarifaprecio.Visible = True
      End If
    Catch ex As Exception
      VALIDO = "no"
      Lb_error_tarifaprecio.Visible = True
    End Try

    If VALIDO = "si" Then
      Dim i As Integer = 0
      '2DA VALIDACION, CONTROLAR Q NO EXISSTA LA TARIFA EN EL GRIDVIEW.
      Dim ExisteTarifa As String = "no"
      While i < GridView1.Rows.Count
        If GridView1.Rows(i).Cells(1).Text.ToString.ToUpper = TxtTarifa.Text.ToUpper Then
          ExisteTarifa = "si"
          Exit While
        End If
        i = i + 1
      End While
      If ExisteTarifa = "no" Then
        Dim j As Integer = 0
        While j < GridView1.Rows.Count
          Dim fila As DataRow = DS_Cob_local.Tables("Tarifa").NewRow
          fila("TARIFA_ID") = GridView1.Rows(j).Cells(0).Text
          fila("TARIFA") = GridView1.Rows(j).Cells(1).Text
          fila("LOCAL_ID") = GridView1.Rows(j).Cells(2).Text
          fila("TIPO") = GridView1.Rows(j).Cells(3).Text
          'fila("DIAS") = GridView1.Rows(j).Cells(4).Text
          fila("PRECIO") = GridView1.Rows(j).Cells(4).Text

          DS_Cob_local.Tables("Tarifa").Rows.Add(fila)
          j = j + 1
        End While

        'lo agregamos al gridview.
        Dim fila1 As DataRow = DS_Cob_local.Tables("Tarifa").NewRow
        fila1("TARIFA_ID") = "0"
        fila1("TARIFA") = TxtTarifa.Text.ToUpper
        fila1("LOCAL_ID") = "0"
        fila1("TIPO") = DropDLTarifaTipo.Text.ToUpper
        'fila1("DIAS") = TxtTarifaDias.Text.ToUpper
        fila1("PRECIO") = CDec(TxtTarifaPrecio.Text.ToUpper.Replace(".", ","))

        DS_Cob_local.Tables("Tarifa").Rows.Add(fila1)
        GridView1.DataSource = DS_Cob_local.Tables("Tarifa")
        GridView1.DataBind()

        TxtTarifa.Text = ""
        TxtTarifaPrecio.Text = ""
        'TxtTarifaDias.Text = "1"

      Else
        'MENSAJE Error, existe el pasillo.
        'modal_sn_okerrorvarios
        Label_errorvarios.Text = "La tarifa ya existe."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
      End If

    Else
      'MENSAJE ERROR, COMPLETAR INFO SOLICITADA.
      'modal_sn_okerrorvarios
      Label_errorvarios.Text = "Complete la info. Solicitada."
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
    End If

    GridView1.Columns(0).Visible = False '0 es columna TARIFA_ID
    GridView1.Columns(2).Visible = False 'columna LOCAL_ID


    TxtTarifa.Focus()



  End Sub

  Private Sub DropDLSector_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDLSector.SelectedIndexChanged
    Dim ds_pasillo As DataSet = daSector.SectorPasillos_recuperarID(DropDLSector.SelectedValue)
    DropDLPasillo.DataSource = ds_pasillo.Tables(1)
    DropDLPasillo.DataTextField = "PASILLO_desc"
    DropDLPasillo.DataValueField = "PASILLO_ID"
    DropDLPasillo.DataBind()

  End Sub

  Private Sub DropDLTarifaTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDLTarifaTipo.SelectedIndexChanged
    If DropDLTarifaTipo.Text = "Unica" Then
      TxtTarifaDias.Text = "1"
      TxtTarifaDias.ReadOnly = True
      TxtTarifaPrecio.Focus()
    End If

    If DropDLTarifaTipo.Text = "Periodica" Then
      TxtTarifaDias.Text = "1"
      TxtTarifaDias.ReadOnly = False
      TxtTarifaDias.Focus()
    End If
  End Sub

  Private Sub btn_errorvarios_close_ServerClick(sender As Object, e As EventArgs) Handles btn_errorvarios_close.ServerClick
    If Label_errorvarios.Text = "Complete la info. Solicitada." Then
      If (lb_error_Sector.Visible = True) Or (lb_error_pasillo.Visible = True) Or (lb_error_codigolocal.Visible = True) Or (Lb_error_local.Visible = True) Then
        TxtCodigo.Focus()
      Else
        TxtTarifa.Focus()
      End If
    End If

      If Label_errorvarios.Text = "La tarifa ya existe." Then
      Lb_error_tarifadesc.Visible = True
      TxtTarifa.Focus()
    End If

  End Sub

  Private Sub btn_ok_errorvarios_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_errorvarios.ServerClick
    If Label_errorvarios.Text = "Complete la info. Solicitada." Then
      If (lb_error_Sector.Visible = True) Or (lb_error_pasillo.Visible = True) Or (lb_error_codigolocal.Visible = True) Or (Lb_error_local.Visible = True) Then
        TxtCodigo.Focus()
      Else
        TxtTarifa.Focus()
      End If
    End If

    If Label_errorvarios.Text = "La tarifa ya existe." Then
      Lb_error_tarifadesc.Visible = True
      TxtTarifa.Focus()
    End If

    If Label_errorvarios.Text = "El local ya existe. Modifique." Then
      TxtCodigo.Focus()
    End If


  End Sub

  Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand

    If (e.CommandName = "ID2") Then
      ' Retrieve the row index stored in the CommandArgument property.
      'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      'Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())



      'If id = "0" Then
      'entonces lo quito
      GridView1.Columns(0).Visible = True 'columna TARIFA_ID
      GridView1.Columns(2).Visible = True 'columna LOCAL_ID
      Dim j As Integer = 0
      While j < GridView1.Rows.Count
        Dim rowselect_descripcion As String = e.CommandArgument.ToString
        If GridView1.Rows(j).Cells(1).Text <> rowselect_descripcion Then

          'solo lo quito, sin afectar a la bd
          Dim fila As DataRow = DS_Cob_local.Tables("Tarifa").NewRow
          fila("TARIFA_ID") = GridView1.Rows(j).Cells(0).Text
          fila("TARIFA") = GridView1.Rows(j).Cells(1).Text
          fila("LOCAL_ID") = GridView1.Rows(j).Cells(2).Text
          fila("TIPO") = GridView1.Rows(j).Cells(3).Text
          'fila("DIAS") = GridView1.Rows(j).Cells(4).Text
          fila("PRECIO") = GridView1.Rows(j).Cells(4).Text
          DS_Cob_local.Tables("Tarifa").Rows.Add(fila)
        Else
          Dim rowselect_pasilloid As Integer = GridView1.Rows(j).Cells(0).Text
          If rowselect_pasilloid <> 0 Then
            'si es distinto de cero al quitar tengo q actualizar en bd. (eliminacion logica/cambio de estado)
            'poner un mensaje o algo.
          End If
        End If
        j = j + 1
      End While
      GridView1.DataSource = DS_Cob_local.Tables("Tarifa")
      GridView1.DataBind()
      GridView1.Columns(0).Visible = False '0 es columna ID
      GridView1.Columns(2).Visible = False 'columna SECTOR_ID
      'End If



    End If

  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick
    lb_error_Sector.Visible = False
    lb_error_pasillo.Visible = False
    lb_error_codigolocal.Visible = False
    Lb_error_local.Visible = False

    If HF_LOCAL_ID.Value = 0 Then
      '------ALTA-------------
      Dim valido_local As String = "si"

      Try
        Dim SECTOR_ID As Integer = CInt(DropDLSector.SelectedValue)
        If SECTOR_ID = 0 Then
          valido_local = "no"
          lb_error_Sector.Visible = True
        End If
      Catch ex As Exception
        valido_local = "no"
        lb_error_Sector.Visible = True
      End Try

      Try
        Dim PASILLO_ID As Integer = CInt(DropDLPasillo.SelectedValue)
        If PASILLO_ID = 0 Then
          valido_local = "no"
          lb_error_pasillo.Visible = True
        End If
      Catch ex As Exception
        valido_local = "no"
        lb_error_pasillo.Visible = True
      End Try

      Try
        If TxtCodigo.Text = "" Then
          valido_local = "no"
          lb_error_codigolocal.Visible = True
        End If
      Catch ex As Exception
        valido_local = "no"
        lb_error_codigolocal.Visible = True
      End Try

      Try
        If TxtLocal.Text = "" Then
          Lb_error_local.Visible = True
          valido_local = "no"
        End If
      Catch ex As Exception
        Lb_error_local.Visible = True
        valido_local = "no"
      End Try

      If valido_local = "si" Then
        'ahora valido que no exista ese local en el sector/pasillo
        Dim ds_localdesc As DataSet = daLocal.BuscarActivoDesc(TxtLocal.Text.ToUpper, CInt(DropDLPasillo.SelectedValue))
        Dim ds_localcod As DataSet = daLocal.BuscarActivoCodigo(TxtCodigo.Text.ToUpper, CInt(DropDLPasillo.SelectedValue))

        If (ds_localdesc.Tables(0).Rows.Count = 0) And (ds_localcod.Tables(0).Rows.Count = 0) Then

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_GRABAR", "$(document).ready(function () {$('#Mdl_GRABAR').modal();});", True)


        Else
          If (ds_localcod.Tables(0).Rows.Count <> 0) Then
            lb_error_codigolocal.Visible = True
          End If
          If (ds_localdesc.Tables(0).Rows.Count <> 0) Then
            Lb_error_local.Visible = True
          End If
          'aqui mensaje el local ya existe.

          'modal_sn_okerrorvarios
          Label_errorvarios.Text = "El local ya existe. Modifique."
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)


        End If

      Else
        'aqui mensaje que complete la info solicitada.
        'modal_sn_okerrorvarios
        Label_errorvarios.Text = "Complete la info. Solicitada."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
      End If

    End If
    If HF_LOCAL_ID.Value <> 0 Then
      Dim valido_local As String = "si"
      Try
        If TxtCodigo.Text = "" Then
          valido_local = "no"
          lb_error_codigolocal.Visible = True
        End If
      Catch ex As Exception
        valido_local = "no"
        lb_error_codigolocal.Visible = True
      End Try

      Try
        If TxtLocal.Text = "" Then
          Lb_error_local.Visible = True
          valido_local = "no"
        End If
      Catch ex As Exception
        Lb_error_local.Visible = True
        valido_local = "no"
      End Try

      If valido_local = "si" Then
        'VALIDO QUE NO SE HAYA CAMBIADO EL CODIGO DEL LOCAL.
        Dim DS_localcod As DataSet = daLocal.BuscarActivoCodigo(TxtCodigo.Text.ToUpper, DropDLPasillo.SelectedValue)
        Dim valido_cod As String = "si"
        Dim i As Integer = 0
        While i < DS_localcod.Tables(0).Rows.Count
          If DS_localcod.Tables(0).Rows(i).Item("LOCAL_ID") <> HF_LOCAL_ID.Value Then
            valido_cod = "no"
            Exit While
          End If
          i = 1 + 1
        End While
        If valido_cod = "no" Then
          lb_error_codigolocal.Visible = True
        End If
        '------------------------------------------------------------------------
        'VALIDO QUE NO SE HAYA CAMBIADO LA DEC DEL LOCAL.
        Dim DS_localdesc As DataSet = daLocal.BuscarActivoDesc(TxtLocal.Text.ToUpper, DropDLPasillo.SelectedValue)
        Dim valido_desc As String = "si"
        i = 0
        While i < DS_localdesc.Tables(0).Rows.Count
          If DS_localdesc.Tables(0).Rows(i).Item("LOCAL_ID") <> HF_LOCAL_ID.Value Then
            valido_desc = "no"
            Exit While
          End If
          i = 1 + 1
        End While
        If valido_desc = "no" Then
          Lb_error_local.Visible = True
        End If

        If (valido_cod = "si") And (valido_desc = "si") Then

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_GRABAR", "$(document).ready(function () {$('#Mdl_GRABAR').modal();});", True)

        Else
          'aqui mensaje el local ya existe.
          'modal_sn_okerrorvarios
          Label_errorvarios.Text = "El local ya existe. Modifique."
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

  Private Sub Btn_grabar_confirmar_ServerClick(sender As Object, e As EventArgs) Handles Btn_grabar_confirmar.ServerClick
    'aqui todo el codigo para grabar

    '------ALTA-----------------------
    If HF_LOCAL_ID.Value = 0 Then
      Dim ds_local As DataSet = daLocal.Local_alta(CInt(DropDLPasillo.SelectedValue), TxtLocal.Text.ToUpper, TxtCodigo.Text.ToUpper)
      Dim LOCAL_ID As Integer = ds_local.Tables(0).Rows(0).Item("LOCAL_ID")
      'ahora ingreso tarifas si los hay.
      Dim i As Integer = 0
      While i < GridView1.Rows.Count
        Dim TARIFA_precio As Decimal = CDec(GridView1.Rows(i).Cells(4).Text)
        Dim TARIFA_tipo As String = GridView1.Rows(i).Cells(3).Text
        'Dim TARIFA_dias As Integer = CInt(GridView1.Rows(i).Cells(4).Text)
        Dim TARIFA_desc As String = GridView1.Rows(i).Cells(1).Text.ToUpper

        daTarifa.Tarifa_alta(LOCAL_ID, TARIFA_precio, TARIFA_tipo, TARIFA_desc)
        i = i + 1
      End While
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
    End If
    '-------------MODIFICAR
    If HF_LOCAL_ID.Value <> 0 Then
      daLocal.Local_modificar(HF_LOCAL_ID.Value, TxtLocal.Text.ToUpper, TxtCodigo.Text.ToUpper)
      'aqui inserto o modifico las tarifas existentes.
      Dim ds_tarifasexistentes As DataSet = daLocal.LocalTarifas_recuperarID(HF_LOCAL_ID.Value)


      'primero veo cuales se modifican y cuales se agregan.
      GridView1.Columns(0).Visible = True 'columna TARIFA_ID
      GridView1.Columns(2).Visible = True 'columna LOCAL_ID

      Dim i As Integer = 0
      While i < GridView1.Rows.Count
        Dim TARIFA_ID As Integer = GridView1.Rows(i).Cells(0).Text
        If TARIFA_ID = 0 Then
          'es alta
          Dim TARIFA_desc As String = GridView1.Rows(i).Cells(1).Text.ToUpper
          Dim TIPO As String = GridView1.Rows(i).Cells(3).Text.ToUpper
          'Dim DIAS As Integer = GridView1.Rows(i).Cells(4).Text.ToUpper
          Dim PRECIO As Decimal = GridView1.Rows(i).Cells(4).Text.ToUpper
          daTarifa.Tarifa_alta(HF_LOCAL_ID.Value, PRECIO, TIPO, TARIFA_desc)

        Else
          'se modifica...no voy a validar si hay otro igual ya q para esta instancia esto ya deberia estar validado.
          'X AHORA NO CONSIDERO EDITAR ALGO DEL GRID. NO SE COMO AFECTA A LAS TARIFA YA ASIGNADAS A UN CLIENTE.
          'daPasillo.Pasillo_modificar(PASILLO_ID, GridView1.Rows(i).Cells(1).Text.ToUpper)
        End If
        i = i + 1
      End While
      'ahora quito aquellos q en la bd estan pero no asi en el grid.
      'la eliminacion es logica, estado= inactivo.

      i = 0
      While i < ds_tarifasexistentes.Tables(1).Rows.Count
        Dim TARIFA_ID As Integer = ds_tarifasexistentes.Tables(1).Rows(i).Item("TARIFA_ID")
        Dim existe = "no"
        Dim j As Integer = 0
        While j < GridView1.Rows.Count
          If TARIFA_ID = GridView1.Rows(j).Cells(0).Text Then
            existe = "si"
            Exit While
          End If
          j = j + 1
        End While
        If existe = "no" Then
          'lo elimino logicamente
          daTarifa.Tarifa_EstadoModificar(TARIFA_ID, "inactivo")

        End If
        i = i + 1
      End While

      GridView1.Columns(0).Visible = False 'columna ID
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)


    End If

  End Sub

  Private Sub btn_ok_ServerClick(sender As Object, e As EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Local/Cob_Local.aspx")
  End Sub

  Private Sub btn_graba_close_ServerClick(sender As Object, e As EventArgs) Handles btn_graba_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Local/Cob_Local.aspx")
  End Sub

  Private Sub btn_baja_mdll_ServerClick(sender As Object, e As EventArgs) Handles btn_baja_mdll.ServerClick
    If HF_LOCAL_ID.Value <> 0 Then
      daLocal.LocalTarifa_baja(HF_LOCAL_ID.Value)
    End If
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Local/Cob_Local.aspx")



  End Sub
End Class
