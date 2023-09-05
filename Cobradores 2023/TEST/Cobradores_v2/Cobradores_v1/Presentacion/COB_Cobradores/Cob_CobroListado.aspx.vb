Public Class Cob_CobroListado
  Inherits System.Web.UI.Page
  Dim daLocal As New Capa_Datos.Local
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then

      HF_PASILLO_ID.Value = Session("PASILLO_ID")
      HF_SECTOR_ID.Value = Session("SECTOR_ID")
      'Obtener_usuarios()
      Recuperar_Locales(HF_PASILLO_ID.Value)



    End If
  End Sub

  Private Sub Recuperar_Locales(ByVal PASILLO_ID As Integer)

    Dim DS_Cobradores As New DS_Cobradores

    Dim ds_locales As DataSet = daLocal.LocalClientes_obteneractivos(PASILLO_ID)

    Dim i As Integer = 0
    While i < ds_locales.Tables(0).Rows.Count
      ds_locales.Tables(0).Rows(i).Item("ITEM") = i
      i = i + 1
    End While
    GridView2.DataSource = ds_locales.Tables(0)
    GridView2.DataBind()

    GridView2.Columns(1).Visible = False   '0 es columna ITEM
    GridView2.Columns(2).Visible = False  '1 es columna LOCAL_ID
    GridView2.Columns(3).Visible = False  '2 es columna CLIE_ID


  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Session("SECTOR_ID") = HF_SECTOR_ID.Value
    Response.Redirect("~/COB_Cobradores/CobroListadoPasillo.aspx")
  End Sub

  Private Sub GridView2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand
    Try
      If (e.CommandName = "ID") Then
        ' Retrieve the row index stored in the CommandArgument property.
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim ITEM As Integer = Integer.Parse(e.CommandArgument.ToString())
        'Session("usuario_id") = id
        'Response.Redirect("Mensaje_Datos_Personales.aspx")

        Dim LOCAL_ID As Integer = 0
        Dim CLIE_ID As Integer = 0
        Dim i As Integer = 0
        While i < GridView2.Rows.Count
          If ITEM = GridView2.Rows(i).Cells(1).Text Then
            LOCAL_ID = CInt(GridView2.Rows(i).Cells(2).Text)
            CLIE_ID = CInt(GridView2.Rows(i).Cells(3).Text)
            Exit While
          End If
          i = i + 1
        End While
        'Session("COBROLISTADO_OP") = "MODIFICAR"
        Session("LOCAL_ID") = LOCAL_ID
        Session("CLIE_ID") = CLIE_ID

        Response.Redirect("~/COB_Cobradores/Cob_CobroCliente.aspx")

      End If
    Catch ex As Exception

    End Try



  End Sub
End Class
