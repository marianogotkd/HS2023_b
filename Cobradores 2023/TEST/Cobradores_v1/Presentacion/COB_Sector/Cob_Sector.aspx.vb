Public Class Cob_Sector
  Inherits System.Web.UI.Page
  Dim daSector As New Capa_Datos.Sector
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos

      'Obtener_usuarios()
      CARGAR_EJEMPLO()

    End If
  End Sub

  Private Sub CARGAR_EJEMPLO()
    Dim DS_Cob_sector As New DS_Cob_sector

    DS_Cob_sector.Tables("Sector").Rows.Clear()

    GridView1.Columns(0).Visible = True 'columna ID

    Dim ds_sector As DataSet = daSector.consultarsoloactivos

    DS_Cob_sector.Tables("Sector").Merge(ds_sector.Tables(0))

    Dim i As Integer
    While i < DS_Cob_sector.Tables("Sector").Rows.Count
      Dim SECTOR_ID As Integer = DS_Cob_sector.Tables("Sector").Rows(i).Item("SECTOR_ID")
      Dim j As Integer = 0
      Dim cantpasillos As Integer = 0
      While j < ds_sector.Tables(1).Rows.Count
        If SECTOR_ID = ds_sector.Tables(1).Rows(j).Item("SECTOR_ID") Then
          DS_Cob_sector.Tables("Sector").Rows(i).Item("CantPasillos") = ds_sector.Tables(1).Rows(j).Item("CantPasillos")
          Exit While
        End If
        j = j + 1
      End While
      i = i + 1
    End While
    'Dim fila1 As DataRow = DS_Cob_sector.Tables("Sector").NewRow
    'fila1("SECTOR_ID") = 1
    'fila1("SECTOR_desc") = "SECTOR 01"
    'fila1("CantPasillos") = "2"
    'DS_Cob_sector.Tables("Sector").Rows.Add(fila1)

    'Dim fila2 As DataRow = DS_Cob_sector.Tables("Sector").NewRow
    'fila2("SECTOR_ID") = 2
    'fila2("SECTOR_desc") = "SECTOR 02"
    'fila2("CantPasillos") = "1"
    'DS_Cob_sector.Tables("Sector").Rows.Add(fila2)

    'Dim fila3 As DataRow = DS_Cob_sector.Tables("Sector").NewRow
    'fila3("SECTOR_ID") = 3
    'fila3("SECTOR_desc") = "SECTOR 03"
    'fila3("CantPasillos") = "1"
    'DS_Cob_sector.Tables("Sector").Rows.Add(fila3)

    GridView1.DataSource = DS_Cob_sector.Tables("Sector")
    GridView1.DataBind()
    GridView1.Columns(0).Visible = False '0 es columna ID

  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_nuevo_ServerClick(sender As Object, e As EventArgs) Handles btn_nuevo.ServerClick
    Session("Sector_OP") = "ALTA"
    Response.Redirect("~/COB_Sector/Cob_SectorAlta.aspx")

  End Sub

  Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand

    If (e.CommandName = "ID") Then
        ' Retrieve the row index stored in the CommandArgument property.
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
      'Session("usuario_id") = id
      'Response.Redirect("Mensaje_Datos_Personales.aspx")


      Session("Sector_OP") = "MODIFICAR"
      Session("SECTOR_ID") = id
      Response.Redirect("~/COB_Sector/Cob_SectorAlta.aspx")


      'If id = "0" Then
      '  'lo elimino.
      '  GridView1.DeleteRow(index)

      'End If

      'Session("USU_ID") = id 'CON ESTA VARIABLE VOY A PODER RECUPERAR LA INFO EN EL FORM Cob_usuariosAlta.aspx
      'Session("Usuarios_OP") = "MODIFICAR"
      'Response.Redirect("~/COB_Usuarios/Cob_usuariosAlta.aspx")


    End If

  End Sub
End Class
