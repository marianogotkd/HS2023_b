Public Class Inicio
  Inherits System.Web.UI.Page

  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If

    If Not IsPostBack Then
      Permisos()

      'Div_Op1.Visible = True
      'Div_Op2.Visible = True
      'Div_Op3.Visible = True
      'Div_Op4.Visible = True
      'Div_Op5.Visible = True
      'Div_Op6.Visible = True
      'Div_Op7.Visible = True
      'Div_Op8.Visible = True
      'Div_Op9.Visible = True
      'Div_Op10.Visible = True
      'Div_Op11.Visible = True
      'Div_Op99.Visible = True

      'Div_OpA.Visible = True
      'Div_OpB.Visible = True
      'Div_OpC.Visible = True
      'Div_OpD.Visible = True
      'Div_OpE.Visible = True
      'Div_OpF.Visible = True
      'Div_OpG.Visible = True
      'Div_OpH.Visible = True
      'Div_OpI.Visible = True
      'Div_OpJ.Visible = True
      'Div_OpZ.Visible = True
      OcultarMenuColumn1()
      txt_opcion.Focus()
      'Session("pagina_open") = Nothing
    End If


  End Sub

  Private Sub OcultarMenuColumn1()
    If Div_Op1.Visible = False Then
      If Div_Op2.Visible = False Then
        If Div_Op3.Visible = False Then
          If Div_Op4.Visible = False Then
            If Div_Op5.Visible = False Then
              If Div_Op6.Visible = False Then
                If Div_Op7.Visible = False Then
                  If Div_Op8.Visible = False Then
                    If Div_Op9.Visible = False Then
                      If Div_Op10.Visible = False Then
                        If Div_Op11.Visible = False Then
                          If Div_Op99.Visible = False Then
                            Menu_column1.Visible = False
                          End If
                        End If
                      End If
                    End If
                  End If
                End If
              End If
            End If
          End If
        End If
      End If
    End If
  End Sub

  Private Sub Permisos()
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
          'se muestran todos los items del menu
          Div_Op1.Visible = True
          Div_Op2.Visible = True
          Div_Op3.Visible = True
          Div_Op4.Visible = True
          Div_Op5.Visible = True
          Div_Op6.Visible = True
          Div_Op7.Visible = True
          Div_Op8.Visible = True
          Div_Op9.Visible = True
          Div_Op10.Visible = True
          Div_Op11.Visible = True
          Div_Op99.Visible = True

          Div_OpA.Visible = True
          Div_OpB.Visible = True
          Div_OpC.Visible = True
          Div_OpD.Visible = True
          Div_OpE.Visible = True
          Div_OpF.Visible = True
          Div_OpG.Visible = True
          Div_OpH.Visible = True
          Div_OpI.Visible = True
          Div_OpJ.Visible = True
          Div_OpZ.Visible = True
        Case "2"
          'se muestran todos los items del menu
          Div_Op1.Visible = True
          Div_Op2.Visible = True
          Div_Op3.Visible = True
          Div_Op4.Visible = True
          Div_Op5.Visible = True
          Div_Op6.Visible = True
          Div_Op7.Visible = True
          Div_Op8.Visible = True
          Div_Op9.Visible = True
          Div_Op10.Visible = True
          Div_Op11.Visible = True
          Div_Op99.Visible = True

          Div_OpA.Visible = True
          Div_OpB.Visible = True
          Div_OpC.Visible = True
          Div_OpD.Visible = True
          Div_OpE.Visible = True
          Div_OpF.Visible = True
          Div_OpG.Visible = True
          Div_OpH.Visible = True
          Div_OpI.Visible = True
          Div_OpJ.Visible = True
          Div_OpZ.Visible = True
        Case "3" 'no usar por ahora...ver q le habilito al cobrador
          'se verifica que permisos estan habilitados.
          'se muestran todos los items del menu

          Dim ds_permisos As DataSet = DApermisos.Permisos_buscar(Idusuario)
          Dim i As Integer = 0
          While i < ds_permisos.Tables(0).Rows.Count
            Dim Menu As String = ""
            Try
              Menu = ds_permisos.Tables(0).Rows(i).Item("Menu").ToString.ToUpper
            Catch ex As Exception
            End Try
            Select Case Menu
              Case "1"
                Div_Op1.Visible = True
              Case "2"
                Div_Op2.Visible = True
              Case "3"
                Div_Op3.Visible = True
              Case "4"
                Div_Op4.Visible = True
              Case "5"
                Div_Op5.Visible = True
              Case "6"
                Div_Op6.Visible = True
              Case "7"
                Div_Op7.Visible = True
              Case "8"
                Div_Op8.Visible = True
              Case "9"
                Div_Op9.Visible = True
              Case "10"
                Div_Op10.Visible = True
              Case "11"
                Div_Op11.Visible = True
              Case "99"
                Div_Op99.Visible = True
              Case "A"
                Div_OpA.Visible = True
              Case "B"
                Div_OpB.Visible = True
              Case "C"
                Div_OpC.Visible = True
              Case "D"
                Div_OpD.Visible = True
              Case "E"
                Div_OpE.Visible = True
              Case "F"
                Div_OpF.Visible = True
              Case "G"
                Div_OpG.Visible = True
              Case "H"
                Div_OpH.Visible = True
              Case "I"
                Div_OpI.Visible = True
              Case "J"
                Div_OpJ.Visible = True
              Case "Z"
                Div_OpZ.Visible = True
            End Select
            i = i + 1
          End While
      End Select
    End If
  End Sub


  Private Sub btn_opcion_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_opcion.ServerClick

    Select Case txt_opcion.Text.ToUpper
      Case "1"
        If Div_Op1.Visible = True Then
          Session("op_ingreso") = "si"
          'aqui tengo que poner la ruta a usuarios administracion.

          Response.Redirect("~/COB_Usuarios/Cob_usuarios.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "2"
        If Div_Op2.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/COB_Sector/Cob_sector.aspx")
          'Response.Redirect("~/COB_Tarifas/Cob_tarifas.aspx")

          'Response.Redirect("~/WC_Carga de Recorridos_Zonas/carga_recorridos_zonas_a.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "3"
        If Div_Op3.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/COB_Local/Cob_Local.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "4"
        If Div_Op4.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/COB_Clientes/Cob_clientes.aspx")

          '  Session("op_ingreso") = "si"
          '  Response.Redirect("~/WC_Liquidacion Parcial/LiquidacionParcial_recorridos.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "5"
        If Div_Op5.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/COB_Cobradores/CobroListadoSector.aspx")
          'Session("op_ingreso") = "si"

        Else
          txt_opcion.Focus()
        End If

      Case "6"
        If Div_Op6.Visible = True Then
          Session("op_ingreso") = "si"
          Response.Redirect("~/COB_Administrador/Cob_adminprocdiario.aspx")
        Else
          txt_opcion.Focus()
        End If

      Case "7"
        'If Div_Op7.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_LiquidacionFinal/LiquidacionFinal.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "8"
        'If Div_Op8.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_LiquidacionRegalos/LiquidacionRegalos_op.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "9"
        'If Div_Op9.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_CobroPrestamosXRegalos/Cobro_PrestamosxRegalos.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "10"
        'If Div_Op10.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_LiquidacionGrupos/LiquidacionGrupos.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "11"
        'If Div_Op11.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_Visualizar_RegAnteriores/VisRegAnt_a.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "99"
        'If Div_Op99.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_ReliquidacionXError/ReliquidacionXError.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "A"
        'If Div_OpA.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_Grupos/Grupos_abm.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "B"
        'If Div_OpB.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_Cliente/Cliente_abm.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "C"
        'If Div_OpC.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_ABML Prestamos_Creditos/abml_prestamoscreditos.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "D"
        'If Div_OpD.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_ACL Gastos/acl_gastos.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "E"
        'If Div_OpE.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_AB Recorridos_Zonas/ab_recorridos_zonas.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "F"
        'If Div_OpF.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_Modificar Saldos/Modificar_saldos.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "G"
        'If Div_OpG.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_TicketsClientes/TicketsClientes.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "H"
        'If Div_OpH.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_TicketsClientes/TicketsGeneral.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "I"
        'If Div_OpI.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_Listados/Listados_op.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "J"
        'If Div_OpJ.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_ABM_Cubiertas/Cubiertas_op.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case "Z"
        'If Div_OpZ.Visible = True Then
        '  Session("op_ingreso") = "si"
        '  Response.Redirect("~/WC_Backup/Respaldo.aspx")
        'Else
        '  txt_opcion.Focus()
        'End If

      Case Else
        ''aqui va mensaje de error.
        'no existe
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-sm_error", "$(document).ready(function () {$('#modal-sm_error').modal();});", True)

    End Select

  End Sub

  Private Sub btn_ok_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ok_error.ServerClick
    txt_opcion.Focus()
  End Sub

  Private Sub btn_close_error_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close_error.ServerClick
    txt_opcion.Focus()
  End Sub


  'AQUI agrego el atributo onfocus y asocio a la rutina js seleccionartexto para que cuando se ponga el foco en un textbox se seleccione todo el contenido
  Private Sub txt_opcion_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_opcion.Init
    txt_opcion.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub
End Class
