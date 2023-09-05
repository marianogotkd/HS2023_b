Public Class Index
  Inherits System.Web.UI.Page
  'Dim DAUsuario As New Capa_Datos.Usuario
  Dim DAusuarios_wc As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim cookie As HttpCookie = New HttpCookie("Token_InicioSesion", "") '----------------1 IMPORTANTE. SI CADUCA SE REDIRIGE A INDEX.ASPX
    'cookie.Expires = DateTime.Now.AddMinutes(-10)
    cookie.Expires = DateTime.Now.AddDays(-2)
    Response.Cookies.Add(cookie)

    Dim cookie2 As HttpCookie = New HttpCookie("Token_Idusuario", "") '----------------2 IMPORTANTE PARA VER QUE SE HABILITA EN EL MENU.
    'cookie2.Expires = DateTime.Now.AddMinutes(-10)
    cookie2.Expires = DateTime.Now.AddDays(-2)
    Response.Cookies.Add(cookie2)

    If Not IsPostBack Then



      lbl_Err2.Visible = False
      lbl_Err3.Visible = False
      tb_us.Focus()
    End If


  End Sub


  Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
    lbl_Err2.Visible = False
    lbl_Err3.Visible = False
    'Dim ds As DataSet = DAUsuario.Usuario_ObtenerUsuario(tb_us.Text, tb_pass.Text)
    'If ds.Tables(0).Rows.Count <> 0 Then
    '    Session("UsuId") = ds.Tables(0).Rows(0).Item("UsuId")
    '    Session("Us") = tb_us.Text
    '    Session("Pass") = tb_pass.Text
    '    Response.Redirect("Inicio.aspx")
    'Else
    '    lbl_Err2.Visible = True
    'End If

    Dim ds As DataSet = DAusuarios_wc.Usuarios_ObtenerUsuario(tb_us.Text, tb_pass.Text)
    If ds.Tables(0).Rows.Count <> 0 Then
      'Dim Idusuario As Integer = ds.Tables(0).Rows(0).Item("Idusuario")
      'Session("UsuId") = ds.Tables(0).Rows(0).Item("Idusuario")
      'Session("Us") = tb_us.Text
      'Session("Pass") = tb_pass.Text

      'Response.Redirect("Inicio.aspx")

      Dim Jerarquia As String = ""
      Try
        Jerarquia = CStr(ds.Tables(0).Rows(0).Item("Jerarquia"))
      Catch ex As Exception
      End Try
      Select Case Jerarquia
        Case "1" 'ingresa como admin geeral, el menu esta con todos los items.
          Dim Idusuario As Integer = ds.Tables(0).Rows(0).Item("Idusuario")
          Session("UsuId") = ds.Tables(0).Rows(0).Item("Idusuario")
          Session("Us") = tb_us.Text
          Session("Pass") = tb_pass.Text

          Dim cookie As HttpCookie = New HttpCookie("Token_InicioSesion", "conectado")
          'cookie.Expires = DateTime.Now.AddMinutes(60)
          cookie.Expires = DateTime.Now.AddDays(2)
          Response.Cookies.Add(cookie)

          Dim cookie2 As HttpCookie = New HttpCookie("Token_Idusuario", Idusuario)
          'cookie2.Expires = DateTime.Now.AddMinutes(60)
          cookie2.Expires = DateTime.Now.AddDays(2)
          Response.Cookies.Add(cookie2)

          Response.Redirect("Inicio.aspx")
        Case "2" 'administrador
          Dim Idusuario As Integer = ds.Tables(0).Rows(0).Item("Idusuario")
          Session("UsuId") = ds.Tables(0).Rows(0).Item("Idusuario")
          Session("Us") = tb_us.Text
          Session("Pass") = tb_pass.Text

          Dim cookie As HttpCookie = New HttpCookie("Token_InicioSesion", "conectado")
          'cookie.Expires = DateTime.Now.AddMinutes(60)
          cookie.Expires = DateTime.Now.AddDays(2)
          Response.Cookies.Add(cookie)

          Dim cookie2 As HttpCookie = New HttpCookie("Token_Idusuario", Idusuario)
          'cookie2.Expires = DateTime.Now.AddMinutes(60)
          cookie2.Expires = DateTime.Now.AddDays(2)
          Response.Cookies.Add(cookie2)

          Response.Redirect("Inicio.aspx")
        Case "3" 'no usar, aun no se q permisos tiene
          'verificar que permisos tiene asignado
          Dim Idusuario As Integer = ds.Tables(0).Rows(0).Item("Idusuario")
          Dim ds_p As DataSet = DApermisos.Permisos_buscar(Idusuario)
          If ds_p.Tables(0).Rows.Count <> 0 Then
            Session("UsuId") = ds.Tables(0).Rows(0).Item("Idusuario")
            Session("Us") = tb_us.Text
            Session("Pass") = tb_pass.Text


            Dim cookie As HttpCookie = New HttpCookie("Token_InicioSesion", "conectado") '----------------1 IMPORTANTE. SI CADUCA SE REDIRIGE A INDEX.ASPX
            'cookie.Expires = DateTime.Now.AddMinutes (10)
            cookie.Expires = DateTime.Now.AddDays(2)
            Response.Cookies.Add(cookie)

            Dim cookie2 As HttpCookie = New HttpCookie("Token_Idusuario", Idusuario) '----------------2 IMPORTANTE PARA VER QUE SE HABILITA EN EL MENU.
            'cookie2.Expires = DateTime.Now.AddMinutes(10)
            cookie2.Expires = DateTime.Now.AddDays(2)
            Response.Cookies.Add(cookie2)

            Response.Redirect("Inicio.aspx")
          Else
            'si no tiene permisos no ingresa
            lbl_Err3.Visible = True
          End If
        Case Else
          'si no tiene Jerarquia no ingresa
          lbl_Err2.Visible = True
      End Select


    Else
      lbl_Err2.Visible = True
    End If



  End Sub


End Class
