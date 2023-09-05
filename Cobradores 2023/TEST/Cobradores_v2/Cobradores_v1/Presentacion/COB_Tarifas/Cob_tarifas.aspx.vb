Public Class Cob_tarifas
  Inherits System.Web.UI.Page

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



    Dim DS_Cob_tarifas As New DS_Cob_tarifas


    DS_Cob_tarifas.Tables("Tarifas").Rows.Clear()

    GridView1.Columns(0).Visible = True 'columna ID

    Dim fila1 As DataRow = DS_Cob_tarifas.Tables("Tarifas").NewRow
    fila1("TARIFATIPO_ID") = 1
    fila1("TARIFATIPO_descripcion") = "DIARIA"
    fila1("TARIFATIPO_tipo") = "Periodica"
    fila1("TARIFATIPO_dias") = "1"
    DS_Cob_tarifas.Tables("Tarifas").Rows.Add(fila1)

    Dim fila2 As DataRow = DS_Cob_tarifas.Tables("Tarifas").NewRow
    fila2("TARIFATIPO_ID") = 2
    fila2("TARIFATIPO_descripcion") = "SEMANAL"
    fila2("TARIFATIPO_tipo") = "Periodica"
    fila2("TARIFATIPO_dias") = "7"
    DS_Cob_tarifas.Tables("Tarifas").Rows.Add(fila2)

    Dim fila3 As DataRow = DS_Cob_tarifas.Tables("Tarifas").NewRow
    fila3("TARIFATIPO_ID") = 3
    fila3("TARIFATIPO_descripcion") = "SEMESTRAL"
    fila3("TARIFATIPO_tipo") = "Unica"
    fila3("TARIFATIPO_dias") = "1"
    DS_Cob_tarifas.Tables("Tarifas").Rows.Add(fila3)

    GridView1.DataSource = DS_Cob_tarifas.Tables("Tarifas")
    GridView1.DataBind()

    GridView1.Columns(0).Visible = False '0 es columna ID




  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub btn_nuevo_ServerClick(sender As Object, e As EventArgs) Handles btn_nuevo.ServerClick
    Response.Redirect("~/COB_Tarifas/Cob_tarifasAlta.aspx")
  End Sub
End Class
