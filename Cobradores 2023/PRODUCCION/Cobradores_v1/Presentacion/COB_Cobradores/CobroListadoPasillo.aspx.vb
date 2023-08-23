Public Class CobroListadoPasillo
  Inherits System.Web.UI.Page
  Dim daPasillo As New Capa_Datos.Pasillo


  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then
      HF_SECTOR_ID.Value = Session("SECTOR_ID")
      Recuperar_pasillos(HF_SECTOR_ID.Value)
    End If
  End Sub

  Private Sub Recuperar_pasillos(ByVal SECTOR_ID As Integer)

    Dim DS_Cobradores As New DS_Cobradores

    Dim ds_locales As DataSet = daPasillo.Pasillo_cobradores_obteneractivos(SECTOR_ID)

    Dim i As Integer = 0
    While i < ds_locales.Tables(0).Rows.Count
      ds_locales.Tables(0).Rows(i).Item("ITEM") = i
      i = i + 1
    End While
    GridView2.DataSource = ds_locales.Tables(0)
    GridView2.DataBind()

    GridView2.Columns(1).Visible = False   '0 es columna ITEM
    GridView2.Columns(2).Visible = False  '1 es columna PASILLO_ID



  End Sub


  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/COB_Cobradores/CobroListadoSector.aspx")
  End Sub

  Private Sub GridView2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand

    Try
      If (e.CommandName = "ID") Then
        ' Retrieve the row index stored in the CommandArgument property.
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
        'Session("usuario_id") = id
        'Response.Redirect("Mensaje_Datos_Personales.aspx")


        Session("PASILLO_ID") = id 'CON ESTA VARIABLE VOY A PODER RECUPERAR LA INFO EN EL FORM Cob_clientesAlta.aspx
        Response.Redirect("~/COB_Cobradores/Cob_CobroListado.aspx")
        Session("SECTOR_ID") = HF_SECTOR_ID.Value


      End If
    Catch ex As Exception

    End Try


  End Sub
End Class
