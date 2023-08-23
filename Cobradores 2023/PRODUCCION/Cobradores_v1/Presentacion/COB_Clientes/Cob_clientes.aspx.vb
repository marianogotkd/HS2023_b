Public Class Cob_clientes
  Inherits System.Web.UI.Page

  Dim daClientes As New Capa_Datos.Clientes

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If


    If Not IsPostBack Then
      'Permisos() comentado x ahora, no tengo armada la tabla de permisos

      Obtener_clientes()


    End If
  End Sub

  Private Sub Obtener_clientes()
    Dim DS_Cob_clientes As New DS_Cob_clientes
    DS_Cob_clientes.Clientes.Rows.Clear()

    GridView1.Columns(0).Visible = True 'columna ID


    Dim ds_clie As DataSet = daClientes.Cliente_consultarsoloactivos

    ''aqui una carga de ejemplo
    'Dim fila As DataRow = DS_Cob_clientes.Clientes.NewRow
    'fila("CLIE_ID") = 1
    'fila("Nombre") = "Juarez, Martin"
    'fila("CtaCte") = "001"
    'fila("Dni") = "32547904"
    'DS_Cob_clientes.Clientes.Rows.Add(fila)
    'Dim fila2 As DataRow = DS_Cob_clientes.Clientes.NewRow
    'fila2("CLIE_ID") = 1
    'fila2("Nombre") = "Fernandez, Luis Carlos"
    'fila2("CtaCte") = "002"
    'fila2("Dni") = "32547905"
    'DS_Cob_clientes.Clientes.Rows.Add(fila2)


    DS_Cob_clientes.Clientes.Merge(ds_clie.Tables(0))

    GridView1.DataSource = DS_Cob_clientes.Clientes
    GridView1.DataBind()

    GridView1.Columns(0).Visible = False '0 es columna ID


  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_nuevo_ServerClick(sender As Object, e As EventArgs) Handles btn_nuevo.ServerClick
    'aqui redirecciono a nuevo
    Session("Clientes_OP") = "ALTA"
    Response.Redirect("~/COB_clientes/Cob_clientesAlta.aspx")
  End Sub

  Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
    Try
      If (e.CommandName = "ID") Then
        ' Retrieve the row index stored in the CommandArgument property.
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim id As Integer = Integer.Parse(e.CommandArgument.ToString())
        'Session("usuario_id") = id
        'Response.Redirect("Mensaje_Datos_Personales.aspx")

        Session("Clientes_OP") = "MODIFICAR"
        Session("CLIE_ID") = id 'CON ESTA VARIABLE VOY A PODER RECUPERAR LA INFO EN EL FORM Cob_clientesAlta.aspx
        Response.Redirect("~/COB_clientes/Cob_clientesAlta.aspx")


      End If
    Catch ex As Exception

    End Try
  End Sub
End Class
