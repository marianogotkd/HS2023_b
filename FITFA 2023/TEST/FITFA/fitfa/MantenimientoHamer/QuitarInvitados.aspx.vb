Public Class QuitarInvitados
    Inherits System.Web.UI.Page
    Dim DAinstructor As New Capa_de_datos.Instructor
    Dim DAusuario As New Capa_de_datos.usuario
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then


        End If
    End Sub

    Private Sub Btn_QuitarInvitados_Click(sender As Object, e As EventArgs) Handles Btn_QuitarInvitados.Click
        Dim ds_instructor As DataSet = DAinstructor.Instructor_obtener_invitado
        If ds_instructor.Tables(0).Rows.Count <> 0 Then

            Dim i As Integer = 0
            While i < ds_instructor.Tables(0).Rows.Count
                Dim instructor_id As String = CStr(ds_instructor.Tables(0).Rows(i).Item("instructor_id"))

                Dim dt_alumnos As DataTable = DAusuario.Alumnos_x_instructor_obtener(instructor_id)

                Dim j As Integer = 0
                While j < dt_alumnos.Rows.Count
                    Dim usuario_id As String = CStr(dt_alumnos.Rows(j).Item("usuario_id"))
                    DAusuario.usuario_pasar_inactivo(usuario_id)
                    j = j + 1
                End While
                i = i + 1
            End While
        End If

        Lb_guardado_ok.Visible = True

    End Sub
End Class