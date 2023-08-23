Public Class Cob_SectorAlta
  Inherits System.Web.UI.Page
  Dim daSector As New Capa_Datos.Sector
  Dim daPasillo As New Capa_Datos.Pasillo
  Dim DS_Cob_sector As New DS_Cob_sector

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos

      If Session("Sector_OP") = "ALTA" Then
        Session("Sector_OP") = ""
        HF_Sector_ID.Value = 0
        gridvacio()
      End If
      If Session("Sector_OP") = "MODIFICAR" Then
        'MODIFICAR 
        Session("Sector_OP") = ""
        HF_Sector_ID.Value = Session("SECTOR_ID")
        recuperar_info_sector()

        TxtSector.Focus()

      End If

      'Obtener_usuarios()


    End If
  End Sub

  Private Sub recuperar_info_sector()
    Dim ds_SectorInfo As DataSet = daSector.SectorPasillos_recuperarID(HF_Sector_ID.Value)
    TxtSector.Text = ds_SectorInfo.Tables(0).Rows(0).Item("SECTOR_desc")

    DS_Cob_sector.Tables("Pasillo").Rows.Clear()
    GridView1.Columns(0).Visible = True 'columna ID
    GridView1.Columns(2).Visible = True 'columna SECTOR_ID

    Dim i As Integer = 0
    While i < ds_SectorInfo.Tables(1).Rows.Count
      Dim fila As DataRow = DS_Cob_sector.Tables("Pasillo").NewRow
      fila("PASILLO_ID") = ds_SectorInfo.Tables(1).Rows(i).Item("PASILLO_ID")
      fila("PASILLO_desc") = ds_SectorInfo.Tables(1).Rows(i).Item("PASILLO_desc")
      fila("SECTOR_ID") = HF_Sector_ID.Value
      DS_Cob_sector.Tables("Pasillo").Rows.Add(fila)
      i = i + 1
    End While


    GridView1.DataSource = DS_Cob_sector.Tables("Pasillo")
    GridView1.DataBind()
    GridView1.Columns(0).Visible = False '0 es columna ID
    GridView1.Columns(2).Visible = False 'columna SECTOR_ID

  End Sub
  Private Sub gridvacio()


    DS_Cob_sector.Tables("Pasillo").Rows.Clear()

    GridView1.Columns(0).Visible = True 'columna ID
    GridView1.Columns(2).Visible = True 'columna SECTOR_ID
    'Dim fila1 As DataRow = DS_Cob_sector.Tables("Pasillo").NewRow
    'fila1("PASILLO_ID") = 1
    'fila1("PASILLO_desc") = "PASILLO 01"
    'fila1("SECTOR_ID") = "1"
    'DS_Cob_sector.Tables("Pasillo").Rows.Add(fila1)

    'Dim fila2 As DataRow = DS_Cob_sector.Tables("Pasillo").NewRow
    'fila2("PASILLO_ID") = 2
    'fila2("PASILLO_desc") = "PASILLO 02"
    'fila2("SECTOR_ID") = "1"
    'DS_Cob_sector.Tables("Pasillo").Rows.Add(fila2)

    'Dim fila3 As DataRow = DS_Cob_sector.Tables("Pasillo").NewRow
    'fila3("PASILLO_ID") = 3
    'fila3("PASILLO_desc") = "PASILLO 03"
    'fila3("SECTOR_ID") = "1"
    'DS_Cob_sector.Tables("Pasillo").Rows.Add(fila3)

    GridView1.DataSource = DS_Cob_sector.Tables("Pasillo")
    GridView1.DataBind()
    GridView1.Columns(0).Visible = False '0 es columna ID
    GridView1.Columns(2).Visible = False 'columna SECTOR_ID
  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Sector/Cob_Sector.aspx")
  End Sub

  Private Sub BtnAsignar_Click(sender As Object, e As EventArgs) Handles BtnAsignar.Click
    GridView1.Columns(0).Visible = True 'columna ID
    GridView1.Columns(2).Visible = True 'columna SECTOR_ID
    'If HF_Sector_ID.Value = "0" Then
    '----------------ALTA Y MODIFICACION---------------------------------------------------------
    If TxtPasillo.Text <> "" Then '1 VALIDACION.

        Dim i As Integer = 0
          '2DA VALIDACION, CONTROLAR Q NO EXISSTA EL PASILLO EN EL GRIDVIEW.
          Dim ExistePasillo As String = "no"
          While i < GridView1.Rows.Count
            If GridView1.Rows(i).Cells(1).Text.ToString.ToUpper = TxtPasillo.Text.ToUpper Then
              ExistePasillo = "si"
              Exit While
            End If
            i = i + 1
          End While
          If ExistePasillo = "no" Then
            Dim j As Integer = 0
            While j < GridView1.Rows.Count
              Dim fila As DataRow = DS_Cob_sector.Tables("Pasillo").NewRow
              fila("PASILLO_ID") = GridView1.Rows(j).Cells(0).Text
              fila("PASILLO_desc") = GridView1.Rows(j).Cells(1).Text
              fila("SECTOR_ID") = GridView1.Rows(j).Cells(2).Text
              DS_Cob_sector.Tables("Pasillo").Rows.Add(fila)
              j = j + 1
            End While

            'lo agregamos al gridview.
            Dim fila1 As DataRow = DS_Cob_sector.Tables("Pasillo").NewRow
            fila1("PASILLO_ID") = "0"
            fila1("PASILLO_desc") = TxtPasillo.Text.ToUpper
            fila1("SECTOR_ID") = "0"
            DS_Cob_sector.Tables("Pasillo").Rows.Add(fila1)
            GridView1.DataSource = DS_Cob_sector.Tables("Pasillo")
            GridView1.DataBind()


          Else
            'MENSAJE Error, existe el pasillo.
            'modal_sn_okerrorvarios
            Label_errorvarios.Text = "El pasillo ya existe."
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
          End If




      Else
        'MENSAJE ERROR, COMPLETAR INFO SOLICITADA.
        'modal_sn_okerrorvarios
        Label_errorvarios.Text = "Complete la info. Solicitada."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
      End If

      'End If
      GridView1.Columns(0).Visible = False '0 es columna ID
    GridView1.Columns(2).Visible = False 'columna SECTOR_ID
    TxtPasillo.Text = ""
    TxtPasillo.Focus()


  End Sub

  Private Sub btn_errorvarios_close_ServerClick(sender As Object, e As EventArgs) Handles btn_errorvarios_close.ServerClick


    If Label_errorvarios.Text = "El pasillo ya existe." Then
      TxtPasillo.Focus()
    End If

    If Label_errorvarios.Text = "Complete la info. Solicitada." Then
      If TxtSector.Text = "" Then
        TxtSector.Focus()
      Else
        TxtPasillo.Focus()
      End If
    End If


    If Label_errorvarios.Text = "El sector ya existe. Modifique." Then
      TxtSector.Focus()
    End If

  End Sub

  Private Sub btn_ok_errorvarios_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_errorvarios.ServerClick
    If Label_errorvarios.Text = "El pasillo ya existe." Then
      TxtPasillo.Focus()
    End If
    If Label_errorvarios.Text = "Complete la info. Solicitada." Then
      If TxtSector.Text = "" Then
        TxtSector.Focus()
      Else
        TxtPasillo.Focus()
      End If
    End If
    If Label_errorvarios.Text = "El sector ya existe. Modifique." Then
      TxtSector.Focus()
    End If
  End Sub

  Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand

    If (e.CommandName = "ID1") Then
      ' Retrieve the row index stored in the CommandArgument property.
      'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      'Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
      'Session("usuario_id") = id
      'Response.Redirect("Mensaje_Datos_Personales.aspx")

      'If id = "0" Then
      '  'lo elimino.
      '  GridView1.DeleteRow(index)

      'End If

      'Session("USU_ID") = id 'CON ESTA VARIABLE VOY A PODER RECUPERAR LA INFO EN EL FORM Cob_usuariosAlta.aspx
      'Session("Usuarios_OP") = "MODIFICAR"
      'Response.Redirect("~/COB_Usuarios/Cob_usuariosAlta.aspx")
      Dim rowselect_descripcion As String = e.CommandArgument.ToString
      Dim i As Integer = 0
      While i < GridView1.Rows.Count
        If GridView1.Rows(i).Cells(1).Text = rowselect_descripcion Then
          HF_grid1index.Value = i
          TxteditPasillo.Text = GridView1.Rows(i).Cells(1).Text
          Exit While
        End If
        i = i + 1
      End While

      TxteditPasillo.Focus()
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_GRIDEDITAR", "$(document).ready(function () {$('#Mdl_GRIDEDITAR').modal();});", True)


    End If


    If (e.CommandName = "ID2") Then
      ' Retrieve the row index stored in the CommandArgument property.
      'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
      'Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())



      'If id = "0" Then
      'entonces lo quito
      GridView1.Columns(0).Visible = True 'columna ID
      GridView1.Columns(2).Visible = True 'columna SECTOR_ID
      Dim j As Integer = 0
        While j < GridView1.Rows.Count
        Dim rowselect_descripcion As String = e.CommandArgument.ToString
        If GridView1.Rows(j).Cells(1).Text <> rowselect_descripcion Then

          'solo lo quito, sin afectar a la bd
          Dim fila As DataRow = DS_Cob_sector.Tables("Pasillo").NewRow
          fila("PASILLO_ID") = GridView1.Rows(j).Cells(0).Text
          fila("PASILLO_desc") = GridView1.Rows(j).Cells(1).Text
          fila("SECTOR_ID") = GridView1.Rows(j).Cells(2).Text
          DS_Cob_sector.Tables("Pasillo").Rows.Add(fila)
        Else
          Dim rowselect_pasilloid As Integer = GridView1.Rows(j).Cells(0).Text
          If rowselect_pasilloid <> 0 Then
            'si es distinto de cero al quitar tengo q actualizar en bd. (eliminacion logica/cambio de estado)
            'poner un mensaje o algo.
          End If
        End If
        j = j + 1
        End While
        GridView1.DataSource = DS_Cob_sector.Tables("Pasillo")
        GridView1.DataBind()
        GridView1.Columns(0).Visible = False '0 es columna ID
      GridView1.Columns(2).Visible = False 'columna SECTOR_ID
      'End If



    End If


  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick

    If HF_Sector_ID.Value = 0 Then
      'es un alta, empezamos con la validacion principal

      '1) que se ingrese un sector y ademas que ese sector no exista.
      Dim ds_sector As DataSet = daSector.BuscarActivoDesc(TxtSector.Text)

      If TxtSector.Text <> "" Then
        If ds_sector.Tables(0).Rows.Count = 0 Then

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_GRABAR", "$(document).ready(function () {$('#Mdl_GRABAR').modal();});", True)

        Else
          'aqui mensaje el sector ya existe.
          'MENSAJE Error, existe el pasillo.
          'modal_sn_okerrorvarios
          Label_errorvarios.Text = "El sector ya existe. Modifique."
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
        End If
      Else
        'aqui mensaje que complete la info solicitada.
        'modal_sn_okerrorvarios
        Label_errorvarios.Text = "Complete la info. Solicitada."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
      End If
    End If
    If HF_Sector_ID.Value <> 0 Then

      If TxtSector.Text <> "" Then

        'VALIDO QUE NO SE HAYA CAMBIADO EL NOMBRE DEL SECTOR.
        Dim DS_sectordesc As DataSet = daSector.BuscarActivoDesc(TxtSector.Text.ToUpper)
        Dim valido_desc As String = "si"
        Dim i As Integer = 0
        While i < DS_sectordesc.Tables(0).Rows.Count
          If DS_sectordesc.Tables(0).Rows(i).Item("SECTOR_ID") <> HF_Sector_ID.Value Then
            valido_desc = "no"
            Exit While
          End If
          i = 1 + 1
        End While
        If valido_desc = "si" Then

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "Mdl_GRABAR", "$(document).ready(function () {$('#Mdl_GRABAR').modal();});", True)






        Else
          'aqui mensaje el sector ya existe.
          'MENSAJE Error, existe el pasillo.
          Label_errorvarios.Text = "El sector ya existe. Modifique."
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
    If HF_Sector_ID.Value = 0 Then
      Dim ds_sector As DataSet = daSector.Sector_alta(TxtSector.Text.ToUpper)
      Dim SECTOR_ID As Integer = ds_sector.Tables(0).Rows(0).Item("SECTOR_ID")
      'ahora ingreso pasillos si los hay.
      Dim i As Integer = 0
      While i < GridView1.Rows.Count
        Dim PASILLO_desc As String = GridView1.Rows(i).Cells(1).Text.ToUpper
        daPasillo.Pasillo_alta(SECTOR_ID, PASILLO_desc)
        i = i + 1
      End While
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)

    End If
    '-------------MODIFICAR
    If HF_Sector_ID.Value <> 0 Then
      daSector.Sector_modificar(HF_Sector_ID.Value, TxtSector.Text.ToUpper)
      'aqui inserto o modifico los pasillos existentes.
      Dim ds_pasillosexistentes As DataSet = daSector.SectorPasillos_recuperarID(HF_Sector_ID.Value)
      'primero veo cuales se modifican y cuales se agregan.
      GridView1.Columns(0).Visible = True 'columna ID

      Dim i As Integer = 0
      While i < GridView1.Rows.Count
        Dim PASILLO_ID As Integer = GridView1.Rows(i).Cells(0).Text
        If PASILLO_ID = 0 Then
          'es alta
          Dim PASILLO_desc As String = GridView1.Rows(i).Cells(1).Text.ToUpper
          daPasillo.Pasillo_alta(HF_Sector_ID.Value, PASILLO_desc)
        Else
          'se modifica...no voy a validar si hay otro igual ya q para esta instancia esto ya deberia estar validado.
          daPasillo.Pasillo_modificar(PASILLO_ID, GridView1.Rows(i).Cells(1).Text.ToUpper)
        End If
        i = i + 1
      End While

      'ahora quito aquellos q en la bd estan pero no asi en el grid.
      'la eliminacion es logica, estado= inactivo.
      i = 0
      While i < ds_pasillosexistentes.Tables(1).Rows.Count
        Dim PASILLO_ID As Integer = ds_pasillosexistentes.Tables(1).Rows(i).Item("PASILLO_ID")
        Dim existe = "no"
        Dim j As Integer = 0
        While j < GridView1.Rows.Count
          If PASILLO_ID = GridView1.Rows(j).Cells(0).Text Then
            existe = "si"
            Exit While
          End If
          j = j + 1
        End While
        If existe = "no" Then
          'lo elimino logicamente
          daPasillo.Pasillo_EstadoModificar(PASILLO_ID, "inactivo")
        End If
        i = i + 1
      End While

      GridView1.Columns(0).Visible = False 'columna ID
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_OKGRABADO", "$(document).ready(function () {$('#modal-sm_OKGRABADO').modal();});", True)
    End If


  End Sub

  Private Sub btn_ok_ServerClick(sender As Object, e As EventArgs) Handles btn_ok.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Sector/Cob_Sector.aspx")
  End Sub

  Private Sub btn_graba_close_ServerClick(sender As Object, e As EventArgs) Handles btn_graba_close.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Sector/Cob_Sector.aspx")
  End Sub

  Private Sub btn_editar_confirmar_ServerClick(sender As Object, e As EventArgs) Handles btn_editar_confirmar.ServerClick

    Dim valido = "si"
    If (GridView1.Rows(HF_grid1index.Value).Cells(1).Text = TxteditPasillo.Text.ToUpper) Or (TxteditPasillo.Text = "") Then
      'no cambio nada
    Else
      Dim i As Integer = 0
      While i < GridView1.Rows.Count
        If GridView1.Rows(i).Cells(1).Text = TxteditPasillo.Text.ToUpper Then
          If i <> HF_grid1index.Value Then
            valido = "no"
          End If
        End If
        i = i + 1
      End While
      If valido = "si" Then
        GridView1.Rows(HF_grid1index.Value).Cells(1).Text = TxteditPasillo.Text.ToUpper
      Else
        'MENSAJE Error, existe el pasillo.
        'modal_sn_okerrorvarios
        Label_errorvarios.Text = "El pasillo ya existe."
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal_sn_okerrorvarios", "$(document).ready(function () {$('#modal_sn_okerrorvarios').modal();});", True)
      End If
    End If

  End Sub

  Private Sub btn_baja_mdll_ServerClick(sender As Object, e As EventArgs) Handles btn_baja_mdll.ServerClick

    If HF_Sector_ID.Value <> 0 Then
      daSector.SectorPasillos_baja(HF_Sector_ID.Value)
    End If
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Sector/Cob_Sector.aspx")

  End Sub

  'PARA EL ALTA, GUARDAR EN MAYUSCULA LOS SECTORES Y PASILLOS.

End Class
