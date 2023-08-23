Public Class VisRegAnt_b
  Inherits System.Web.UI.Page

  Dim DAXCrg As New Capa_Datos.XCrgHistoria
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      Permisos
      HF_fecha.Value = Session("Fecha")
      Dim FECHA As Date = Session("Fecha")
      Label_fecha.Text = FECHA.ToString("dd-MM-yyyy")
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
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 1 or null

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
            If (Menu = "11" And Opcion = "") Or (Menu = "11" And Opcion = "1") Then
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

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick
    Dim DS_VisRegAnt As New DS_VisRegAnt
    DS_VisRegAnt.Tables("XCrgHistoria").Rows.Clear()
    GridView1.DataSource = Nothing
    GridView1.DataBind()


    Dim valido As String = "si"
    Try
      Txt_ClienteCod.Text = CInt(Txt_ClienteCod.Text)
    Catch ex As Exception
      valido = "no"
    End Try

    If valido = "si" Then
      Dim ds_consulta As DataSet = DAXCrg.XCrgHistoria_consultar(HF_fecha.Value, CInt(Txt_ClienteCod.Text))
      If ds_consulta.Tables(0).Rows.Count <> 0 Then

        Dim i As Integer = 0
        While i < ds_consulta.Tables(0).Rows.Count
          Dim fila As DataRow = DS_VisRegAnt.Tables("XCrgHistoria").NewRow
          Try
            fila("RECORRIDO") = ds_consulta.Tables(0).Rows(i).Item("RECORRIDO")
          Catch ex As Exception
          End Try
          Try
            fila("PID") = ds_consulta.Tables(0).Rows(i).Item("PID")
          Catch ex As Exception
          End Try
          Try
            fila("IMPORTE") = ds_consulta.Tables(0).Rows(i).Item("IMPORTE")
          Catch ex As Exception
          End Try
          Try
            fila("S") = ds_consulta.Tables(0).Rows(i).Item("S")
          Catch ex As Exception
          End Try
          Try
            fila("PID2") = ds_consulta.Tables(0).Rows(i).Item("PID2")
          Catch ex As Exception
          End Try
          Try
            fila("S2") = ds_consulta.Tables(0).Rows(i).Item("S2")
          Catch ex As Exception
          End Try
          Try
            If ds_consulta.Tables(0).Rows(i).Item("R") = True Then
              fila("R") = "1"
            Else
              fila("R") = "0"
            End If
          Catch ex As Exception
            fila("R") = ""
          End Try
          Try
            If ds_consulta.Tables(0).Rows(i).Item("SC") = True Then
              fila("SC") = "1"
            Else
              fila("SC") = "0"
            End If
          Catch ex As Exception
            fila("SC") = ""
          End Try
          Try
            If ds_consulta.Tables(0).Rows(i).Item("V") = True Then
              fila("V") = "SI"
            Else
              fila("V") = "NO"
            End If
          Catch ex As Exception
            fila("V") = ""
          End Try
          Try
            fila("T") = ds_consulta.Tables(0).Rows(i).Item("T")
          Catch ex As Exception
          End Try
          Try
            fila("ITEM") = ds_consulta.Tables(0).Rows(i).Item("ITEM")
          Catch ex As Exception
          End Try
          Try
            fila("FECHA") = ds_consulta.Tables(0).Rows(i).Item("FECHA").ToString
          Catch ex As Exception
          End Try
          Try
            fila("HORA") = ds_consulta.Tables(0).Rows(i).Item("HORA").ToString
          Catch ex As Exception
          End Try
          DS_VisRegAnt.Tables("XCrgHistoria").Rows.Add(fila)

          i = i + 1
        End While
        GridView1.DataSource = DS_VisRegAnt.Tables("XCrgHistoria")
        GridView1.DataBind()

      Else
        'la busqueda no arrojo resultados
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error2", "$(document).ready(function () {$('#modal-ok_error2').modal();});", True)
      End If
    Else
      'la busqueda no arrojó resultados
      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error2", "$(document).ready(function () {$('#modal-ok_error2').modal();});", True)
    End If


  End Sub

  Private Sub btn_ok_error2_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error2.ServerClick
    Txt_ClienteCod.Focus()
  End Sub

  Private Sub btn_error_close2_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close2.ServerClick
    Txt_ClienteCod.Focus()
  End Sub

  Private Sub Txt_ClienteCod_Init(sender As Object, e As EventArgs) Handles Txt_ClienteCod.Init
    Txt_ClienteCod.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Session("op_ingreso") = "si"
    Response.Redirect("~/WC_Visualizar_RegAnteriores/VisRegAnt_a.aspx")
  End Sub
End Class
