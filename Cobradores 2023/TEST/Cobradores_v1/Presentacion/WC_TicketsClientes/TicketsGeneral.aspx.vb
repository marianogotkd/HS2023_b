Public Class TicketsGeneral
  Inherits System.Web.UI.Page

  Dim DAtickets As New Capa_Datos.WC_tickets
  Dim DAusuario As New Capa_Datos.WB_usuarios
  Dim DApermisos As New Capa_Datos.WC_Permisos
  Dim DAclientes As New Capa_Datos.WB_clientes
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Request.Cookies("Token_InicioSesion") Is Nothing Then
      'si esta vacia, redirecciona a login
      Response.Redirect("~/Index.aspx")
    End If
    If Not IsPostBack Then
      permisos
      Txt_FechaDesde.Focus()

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
          Session("Jerarquia") = "1"
        Case "2"
          'se verifica que permisos estan habilitados.
          'para este formulario deberia existir debe indicar en Permisos.Opcion = 1 or null
          Dim DS_ticketsclientes As New DS_ticketsclientes

          Dim ds_permisos As DataSet = DApermisos.Permisos_buscar(Idusuario)
          Dim i As Integer = 0
          Dim valido As String = "no"
          While i < ds_permisos.Tables(0).Rows.Count
            Dim Menu As String = ""
            Try
              Menu = ds_permisos.Tables(0).Rows(i).Item("Menu").ToString.ToUpper
            Catch ex As Exception
            End Try
            Dim Opcion As String = ""
            Try
              Opcion = ds_permisos.Tables(0).Rows(i).Item("Opcion")
            Catch ex As Exception
            End Try
            If (Menu = "H" And Opcion = "") Or (Menu = "H" And Opcion = "1") Then
              valido = "si"

              '/////GRUPOS////////
              'VOY A AGREGAR LOS GRUPOS....'SI LLEGASE A ENCONTRAR 1 GRUPO CUYO CAMPO ESTA EN VACIO SE TOMARA COMO QUE TIENE PERMISO A "TODOS"
              Dim grupo As String = ""
              Try
                grupo = CStr(ds_permisos.Tables(0).Rows(i).Item("Grupos"))
              Catch ex As Exception
              End Try
              If grupo = "" Then
                Session("Grupos_permisos") = "TODOS LOS GRUPOS"
                Exit While
              Else
                Dim FILA As DataRow = DS_ticketsclientes.Tables("Permisos_Usuario").NewRow
                FILA("Grupo") = CInt(grupo)
                DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Add(grupo)
                Session("Grupos_permisos") = ""
              End If
            End If
            i = i + 1
          End While
          If valido = "si" Then
            'se accede sin problemas
            Session("Jerarquia") = "2"
            If DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Count <> 0 Then
              Session("Tabla_Permisos_Usuario") = DS_ticketsclientes.Tables("Permisos_Usuario")
            End If

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
    '28-02-2023---19HRS.
    'RESUMEN: SE VALIDARA LAS CONSULTAS EN BASE A LOS PERMISOS.
    'Session("Jerarquia") = "1" para consultar todo.
    'si es Session("Jerarquia") = "2" se valida lo siguiente:
    '------Session("Grupos_permisos") = "TODOS LOS GRUPOS" para consultar todo.
    '------si Session("Grupos_permisos") = "" entonces se consulta lo siguiente:
    '-----------Session("Tabla_Permisos_Usuario") 'es un datatable con los codigos de Grupos habilitados.

  End Sub

  Private Sub btn_retroceder_ServerClick(sender As Object, e As EventArgs) Handles btn_retroceder.ServerClick
    Response.Redirect("~/Inicio.aspx")
  End Sub

  Private Sub Txt_FechaDesde_Init(sender As Object, e As EventArgs) Handles Txt_FechaDesde.Init
    Txt_FechaDesde.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_FechaHasta_Init(sender As Object, e As EventArgs) Handles Txt_FechaHasta.Init
    Txt_FechaHasta.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_Boleta_Init(sender As Object, e As EventArgs) Handles Txt_Boleta.Init
    Txt_Boleta.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_GrupoCodigo_Init(sender As Object, e As EventArgs) Handles Txt_GrupoCodigo.Init
    Txt_GrupoCodigo.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub Txt_ClienteCod_Init(sender As Object, e As EventArgs) Handles Txt_ClienteCod.Init
    Txt_ClienteCod.Attributes.Add("onfocus", "seleccionarTexto(this);")
  End Sub

  Private Sub BOTON_GRABA_ServerClick(sender As Object, e As EventArgs) Handles BOTON_GRABA.ServerClick

    Dim valido = "si"
    Try
      Txt_FechaDesde.Text = CDate(Txt_FechaDesde.Text)
      Dim fecha_base As Date = CDate("01/01/1900")
      If Txt_FechaDesde.Text < fecha_base Then
        valido = "no"
      End If
    Catch ex As Exception
      valido = "no"
    End Try

    Try
      Txt_FechaHasta.Text = CDate(Txt_FechaHasta.Text)

    Catch ex As Exception
      valido = "no"
    End Try


    If valido = "si" Then 'si hasta etapa estan bien las fechas, ahora controlo que el intervalo de fechas sea correcto
      If CDate(Txt_FechaHasta.Text) < CDate(Txt_FechaDesde.Text) Then
        valido = "no"
      End If
    End If


    If (Txt_Boleta.Text.ToUpper = "SI") Or (Txt_Boleta.Text.ToUpper = "NO") Then

    Else
      valido = "no"
    End If

    'puedo ingresar solo el grupo o bien solo el cliente....si ingreso cod de cliente, anulo la busqueda por grupo.


    If valido = "si" Then

      If Session("Jerarquia") = "1" Then

        If (Txt_GrupoCodigo.Text = "999") And (Txt_ClienteCod.Text = "") Then

          If Txt_Boleta.Text.ToUpper = "SI" Then
            GENERAR_REPORTE()
          Else
            GENERAR_REPORTE2() 'NO TENGO EN CUENTA BOLETA
          End If
        Else
          If Txt_ClienteCod.Text <> "" Then
            If Txt_Boleta.Text.ToUpper = "SI" Then
              GENERAR_REPORTE5()
            Else
              generar_reporte6() 'NO TENGO EN CUENTA BOLETA
            End If
          Else
            'entonces es 1 solo codigo
            If Txt_Boleta.Text.ToUpper = "SI" Then
              GENERAR_REPORTE3()
            Else
              GENERAR_REPORTE4() 'NO TENGO EN CUENTA BOLETA
            End If
          End If
        End If

      End If
      '////////////////////////////////////////////////////////////////////////////////////////////
      If (Session("Jerarquia") = "2") And (Session("Grupos_permisos") = "TODOS LOS GRUPOS") Then
        'igual que jerarquia 1
        If (Txt_GrupoCodigo.Text = "999") And (Txt_ClienteCod.Text = "") Then

          If Txt_Boleta.Text.ToUpper = "SI" Then
            GENERAR_REPORTE()
          Else
            GENERAR_REPORTE2() 'NO TENGO EN CUENTA BOLETA
          End If
        Else
          If Txt_ClienteCod.Text <> "" Then
            If Txt_Boleta.Text.ToUpper = "SI" Then
              GENERAR_REPORTE5()
            Else
              generar_reporte6() 'NO TENGO EN CUENTA BOLETA
            End If
          Else
            'entonces es 1 solo codigo
            If Txt_Boleta.Text.ToUpper = "SI" Then
              GENERAR_REPORTE3()
            Else
              GENERAR_REPORTE4() 'NO TENGO EN CUENTA BOLETA
            End If
          End If
        End If

      End If

      If (Session("Jerarquia") = "2") And (Session("Grupos_permisos") = "") Then
        Dim DS_ticketsclientes As New DS_ticketsclientes
        DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Clear()
        DS_ticketsclientes.Tables("Permisos_Usuario").Merge(Session("Tabla_Permisos_Usuario"))
        If DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Count <> 0 Then

          If (Txt_GrupoCodigo.Text = "999") And (Txt_ClienteCod.Text = "") Then

            If Txt_Boleta.Text.ToUpper = "SI" Then
              GENERAR_REPORTE_J2(DS_ticketsclientes) '----LO TENGO QUE HACER PERO SOLO PARA CONSULTAR CODIGOS PUNTUALES.
            Else
              GENERAR_REPORTE2_J2(DS_ticketsclientes) 'NO TENGO EN CUENTA BOLETA
            End If
          Else
            If Txt_ClienteCod.Text <> "" Then
              If Txt_Boleta.Text.ToUpper = "SI" Then
                GENERAR_REPORTE5_J2(DS_ticketsclientes)
              Else
                generar_reporte6_J2(DS_ticketsclientes) 'NO TENGO EN CUENTA BOLETA
              End If
            Else
              'entonces es 1 solo codigo
              If Txt_Boleta.Text.ToUpper = "SI" Then
                GENERAR_REPORTE3_J2(DS_ticketsclientes)
              Else
                GENERAR_REPORTE4_J2(DS_ticketsclientes) 'NO TENGO EN CUENTA BOLETA
              End If
            End If
          End If


        End If
      End If


    Else

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error2", "$(document).ready(function () {$('#modal-ok_error2').modal();});", True)

      'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)

    End If




  End Sub


  Private Sub GENERAR_REPORTE()
    Dim DS_ticketsclientes As New DS_ticketsclientes
    Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener1(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text))

    If DS_CTA.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables(0).Rows.Count

        Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))

        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda

          Dim filtro_foud_aux As String = "Cliente = " + CStr(Cliente)
        Dim rows_found_aux() As DataRow = DS_ticketsclientes.Tables("TicketGeneral").Select(filtro_foud_aux)

        If rows_found_aux.Count = 0 Then
          Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral").NewRow
          fila("Grupo") = CInt(DS_CTA.Tables(0).Rows(i).Item("Grupo_codigo"))
          fila("Cliente") = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
          fila("Recaudacion") = DS_CTA.Tables(0).Rows(i).Item("Recaudacion")
          fila("Comision") = DS_CTA.Tables(0).Rows(i).Item("Comision")
          fila("Premios") = DS_CTA.Tables(0).Rows(i).Item("Premios")
          fila("Reclamos") = DS_CTA.Tables(0).Rows(i).Item("Reclamos")
          'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
          Dim RegRegalos As Decimal = DS_CTA.Tables(0).Rows(i).Item("Regalos")

          If RegRegalos > CDec(0) Then
            RegRegalos = CDec(0)
          End If
          fila("DejoGano") = DS_CTA.Tables(0).Rows(i).Item("DejoGano")
          fila("RecaudacionSC") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionSC")
          fila("ComisionSC") = DS_CTA.Tables(0).Rows(i).Item("ComisionSC")
          fila("PremiosSC") = DS_CTA.Tables(0).Rows(i).Item("PremiosSC")
          fila("ReclamosSC") = DS_CTA.Tables(0).Rows(i).Item("ReclamosSC")
          fila("DejoGanoSC") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")

          fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
          fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
          fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
          fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
          fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")


          Dim valido = "no"
          If CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo")) = 636 Then
            valido = "si"
          End If


          Dim totaldejogano As Decimal = 0
          Try
            totaldejogano = DS_CTA.Tables(0).Rows(i).Item("DejoGano") + DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC") + DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
          Catch ex As Exception

          End Try
          fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

          DS_ticketsclientes.Tables("TicketGeneral").Rows.Add(fila)

          'ahora si hay mas registros para el mismo cliente, se suma sus valores en cada campo.
          'Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
          Dim j = i + 1
          While j < DS_CTA.Tables(0).Rows.Count

            If Cliente = CInt(DS_CTA.Tables(0).Rows(j).Item("Cliente_codigo")) Then
              Dim indice As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
              Try
                Dim Recaudacion As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") + DS_CTA.Tables(0).Rows(j).Item("Recaudacion")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") = (Math.Round(Recaudacion, 2).ToString("N2"))
              Catch ex As Exception

              End Try
              Try
                Dim Comision As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") + DS_CTA.Tables(0).Rows(j).Item("Comision")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") = (Math.Round(Comision, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim Premios As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") + DS_CTA.Tables(0).Rows(j).Item("Premios")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") = (Math.Round(Premios, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim Reclamos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") + DS_CTA.Tables(0).Rows(j).Item("Reclamos")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") = (Math.Round(Reclamos, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              '///////////////////////////REGALOS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
              Try
                'Dim Regalos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                'DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") = (Math.Round(Regalos, 2).ToString("N2"))
                If CDec(DS_CTA.Tables(0).Rows(j).Item("Regalos")) < CDec(0) Then
                  Dim Regalos As Decimal = RegRegalos + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                  RegRegalos = (Math.Round(Regalos, 2).ToString("N2"))
                End If
              Catch ex As Exception
              End Try
              '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

              '////////////////////////DEJOGANO/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
              'Dim Regalos_1 As Decimal = 0
              'Try
              '  Regalos_1 = CDec(DS_CTA.Tables(0).Rows(j).Item("Regalos"))
              'Catch ex As Exception
              'End Try

              'Try
              '  If Regalos_1 < CDec(0) Then
              '    Dim calculo As Decimal = CDec(DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano")) + Regalos_1
              '    calculo = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + calculo

              '    DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(calculo, 2).ToString("N2"))
              '  Else
              '    Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
              '    DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              '  End If


              'Catch ex As Exception
              'End Try
              Try
                Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              Catch ex As Exception

              End Try



              '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

              '/////////////////////////////////SIN CALCULO/////////////////////////////////////////////////////////////////
              Try
                Dim RecaudacionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") + DS_CTA.Tables(0).Rows(j).Item("RecaudacionSC")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") = (Math.Round(RecaudacionSC, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim ComisionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") + DS_CTA.Tables(0).Rows(j).Item("ComisionSC")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") = (Math.Round(ComisionSC, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim PremiosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") + DS_CTA.Tables(0).Rows(j).Item("PremiosSC")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") = (Math.Round(PremiosSC, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim ReclamosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") + DS_CTA.Tables(0).Rows(j).Item("ReclamosSC")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") = (Math.Round(ReclamosSC, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim DejoGanoSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoSC")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") = (Math.Round(DejoGanoSC, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              '/////////////////////////////////////BOLETA/////////////////////////////////////////////////////////////////
              Try
                Dim RecaudacionB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionB") + DS_CTA.Tables(0).Rows(j).Item("RecaudacionB")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionB") = (Math.Round(RecaudacionB, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim ComisionB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionB") + DS_CTA.Tables(0).Rows(j).Item("ComisionB")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionB") = (Math.Round(ComisionB, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim PremiosB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosB") + DS_CTA.Tables(0).Rows(j).Item("PremiosB")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosB") = (Math.Round(PremiosB, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim ReclamosB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosB") + DS_CTA.Tables(0).Rows(j).Item("ReclamosB")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosB") = (Math.Round(ReclamosB, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim DejoGanoB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoB") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoB")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoB") = (Math.Round(DejoGanoB, 2).ToString("N2"))
              Catch ex As Exception
              End Try
              Try
                Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
              Catch ex As Exception

              End Try
              Try
                Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoSC")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
              Catch ex As Exception

              End Try
              Try
                Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoB")
                DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
              Catch ex As Exception

              End Try
            Else
              i = j - 1 'RETROCEDO 1 PARA QUE EL INDICE APUNTE AL PROXIMO NO REPETIDO.(UNA VER EJECUTADO I=I+1)
              Exit While
            End If

            j = j + 1
          End While
          If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
            Dim ind As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
            DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("Regalos") = RegRegalos
            '//////////////////
            'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
            'Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") + RegRegalos
            Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano")
            DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
            '//////////////////
            'Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") + RegRegalos
            Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano")
            DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))



          End If
          Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
          Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral"), indce)



        End If

        End If

    i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)


        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe01a.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral"))

        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))
        CrReport.Dispose() 'esto para no sobrecargar crystal y generar desbordamientos
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)

      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If


    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub

  Private Sub GENERAR_REPORTE2() 'NO TENGO EN CUENTA BOLETA
    Dim DS_ticketsclientes As New DS_ticketsclientes
    Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener1(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text))

    DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Clear()
    DS_ticketsclientes.Tables("TicketGeneral").Rows.Clear()

    If DS_CTA.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables(0).Rows.Count
        Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))

        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda



          Dim filtro_foud_aux As String = "Cliente = " + CStr(Cliente)
          Dim rows_found_aux() As DataRow = DS_ticketsclientes.Tables("TicketGeneral").Select(filtro_foud_aux)

          If rows_found_aux.Count = 0 Then
            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables(0).Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables(0).Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables(0).Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables(0).Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables(0).Rows(i).Item("Reclamos")
            'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            Dim RegRegalos As Decimal = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            If RegRegalos > CDec(0) Then
              RegRegalos = CDec(0)
            End If
            fila("DejoGano") = DS_CTA.Tables(0).Rows(i).Item("DejoGano")
            fila("RecaudacionSC") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables(0).Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables(0).Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables(0).Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")

            'fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
            'fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
            'fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
            'fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
            'fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DS_CTA.Tables(0).Rows(i).Item("DejoGano") + DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
            Catch ex As Exception

            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

            DS_ticketsclientes.Tables("TicketGeneral").Rows.Add(fila)

            'ahora si hay mas registros para el mismo cliente, se suma sus valores en cada campo.
            'Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            Dim j = i + 1
            While j < DS_CTA.Tables(0).Rows.Count

              If Cliente = CInt(DS_CTA.Tables(0).Rows(j).Item("Cliente_codigo")) Then
                Dim indice As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
                Try
                  Dim Recaudacion As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") + DS_CTA.Tables(0).Rows(j).Item("Recaudacion")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") = (Math.Round(Recaudacion, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Comision As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") + DS_CTA.Tables(0).Rows(j).Item("Comision")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") = (Math.Round(Comision, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Premios As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") + DS_CTA.Tables(0).Rows(j).Item("Premios")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") = (Math.Round(Premios, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Reclamos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") + DS_CTA.Tables(0).Rows(j).Item("Reclamos")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") = (Math.Round(Reclamos, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                '///////////////////////////REGALOS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  'Dim Regalos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                  'DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") = (Math.Round(Regalos, 2).ToString("N2"))
                  If CDec(DS_CTA.Tables(0).Rows(j).Item("Regalos")) < CDec(0) Then
                    Dim Regalos As Decimal = RegRegalos + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                    RegRegalos = (Math.Round(Regalos, 2).ToString("N2"))
                  End If
                Catch ex As Exception
                End Try
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '////////////////////////DEJOGANO/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
                Catch ex As Exception

                End Try


                '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                '/////////////////////////////////SIN CALCULO/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") + DS_CTA.Tables(0).Rows(j).Item("RecaudacionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") = (Math.Round(RecaudacionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") + DS_CTA.Tables(0).Rows(j).Item("ComisionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") = (Math.Round(ComisionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") + DS_CTA.Tables(0).Rows(j).Item("PremiosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") = (Math.Round(PremiosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") + DS_CTA.Tables(0).Rows(j).Item("ReclamosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") = (Math.Round(ReclamosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") = (Math.Round(DejoGanoSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try

              Else
                i = j - 1 'RETROCEDO 1 PARA QUE EL INDICE APUNTE AL PROXIMO NO REPETIDO.(UNA VER EJECUTADO I=I+1)
                Exit While
              End If

              j = j + 1
            End While
            If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
              Dim ind As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("Regalos") = RegRegalos

              '//////////////////
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              'Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") + RegRegalos
              Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              '//////////////////
              'Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") + RegRegalos
              Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))


            End If
            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral"), indce)
          End If

        End If
        i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)

        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe02.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral"))
        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))
        CrReport.Dispose()
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If

    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub

  Private Sub GENERAR_REPORTE3() 'trae solo 1 grupo y considera boletas
    Dim DS_ticketsclientes As New DS_ticketsclientes

    DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Clear()
    DS_ticketsclientes.Tables("TicketGeneral").Rows.Clear()


    Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener1_grupo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), Txt_GrupoCodigo.Text)

    If DS_CTA.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables(0).Rows.Count
        Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda



          Dim filtro_foud_aux As String = "Cliente = " + CStr(Cliente)
          Dim rows_found_aux() As DataRow = DS_ticketsclientes.Tables("TicketGeneral").Select(filtro_foud_aux)

          If rows_found_aux.Count = 0 Then
            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables(0).Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables(0).Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables(0).Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables(0).Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables(0).Rows(i).Item("Reclamos")
            'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            Dim RegRegalos As Decimal = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            If RegRegalos > CDec(0) Then
              RegRegalos = CDec(0)
            End If

            fila("DejoGano") = DS_CTA.Tables(0).Rows(i).Item("DejoGano")
            fila("RecaudacionSC") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables(0).Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables(0).Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables(0).Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
            fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
            fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
            fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
            fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
            fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DS_CTA.Tables(0).Rows(i).Item("DejoGano") + DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC") + DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            Catch ex As Exception
            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))
            DS_ticketsclientes.Tables("TicketGeneral").Rows.Add(fila)

            'ahora si hay mas registros para el mismo cliente, se suma sus valores en cada campo.
            'Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            Dim j = i + 1

            Dim valido = "no"
            If CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo")) = 676 Then
              valido = "si"
            End If

            While j < DS_CTA.Tables(0).Rows.Count

              If Cliente = CInt(DS_CTA.Tables(0).Rows(j).Item("Cliente_codigo")) Then
                Dim indice As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
                Try
                  Dim Recaudacion As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") + DS_CTA.Tables(0).Rows(j).Item("Recaudacion")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") = (Math.Round(Recaudacion, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Comision As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") + DS_CTA.Tables(0).Rows(j).Item("Comision")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") = (Math.Round(Comision, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Premios As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") + DS_CTA.Tables(0).Rows(j).Item("Premios")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") = (Math.Round(Premios, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Reclamos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") + DS_CTA.Tables(0).Rows(j).Item("Reclamos")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") = (Math.Round(Reclamos, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                '///////////////////////////REGALOS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  'Dim Regalos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                  'DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") = (Math.Round(Regalos, 2).ToString("N2"))
                  If CDec(DS_CTA.Tables(0).Rows(j).Item("Regalos")) < CDec(0) Then
                    Dim Regalos As Decimal = RegRegalos + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                    RegRegalos = (Math.Round(Regalos, 2).ToString("N2"))
                  End If
                Catch ex As Exception
                End Try
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '////////////////////////DEJOGANO/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                Try
                  Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))

                Catch ex As Exception
                End Try
                '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '/////////////////////////////////SIN CALCULO/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") + DS_CTA.Tables(0).Rows(j).Item("RecaudacionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") = (Math.Round(RecaudacionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") + DS_CTA.Tables(0).Rows(j).Item("ComisionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") = (Math.Round(ComisionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") + DS_CTA.Tables(0).Rows(j).Item("PremiosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") = (Math.Round(PremiosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") + DS_CTA.Tables(0).Rows(j).Item("ReclamosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") = (Math.Round(ReclamosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") = (Math.Round(DejoGanoSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                '/////////////////////////////////////BOLETA/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionB") + DS_CTA.Tables(0).Rows(j).Item("RecaudacionB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionB") = (Math.Round(RecaudacionB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionB") + DS_CTA.Tables(0).Rows(j).Item("ComisionB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionB") = (Math.Round(ComisionB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosB") + DS_CTA.Tables(0).Rows(j).Item("PremiosB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosB") = (Math.Round(PremiosB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosB") + DS_CTA.Tables(0).Rows(j).Item("ReclamosB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosB") = (Math.Round(ReclamosB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoB") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoB") = (Math.Round(DejoGanoB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
              Else
                i = j - 1 'RETROCEDO 1 PARA QUE EL INDICE APUNTE AL PROXIMO NO REPETIDO.(UNA VER EJECUTADO I=I+1)
                Exit While
              End If

              j = j + 1
            End While
            If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
              Dim ind As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("Regalos") = RegRegalos

              '//////////////////
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              'Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") + RegRegalos
              Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              '//////////////////
              'Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") + RegRegalos
              Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))

            End If
            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral"), indce)
          End If
        End If
        i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)

        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe01a.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral"))
        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))
        CrReport.Dispose()
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If

    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub

  Private Sub GENERAR_REPORTE4() '1 SOLO GRUPO, PERO SIN BOLETAS
    Dim DS_ticketsclientes As New DS_ticketsclientes
    Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener1_grupo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), Txt_GrupoCodigo.Text)

    DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Clear()
    DS_ticketsclientes.Tables("TicketGeneral").Rows.Clear()

    If DS_CTA.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables(0).Rows.Count
        Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda

          Dim filtro_foud_aux As String = "Cliente = " + CStr(Cliente)
          Dim rows_found_aux() As DataRow = DS_ticketsclientes.Tables("TicketGeneral").Select(filtro_foud_aux)

          If rows_found_aux.Count = 0 Then
            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables(0).Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables(0).Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables(0).Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables(0).Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables(0).Rows(i).Item("Reclamos")
            'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            Dim RegRegalos As Decimal = DS_CTA.Tables(0).Rows(i).Item("Regalos")

            If RegRegalos > CDec(0) Then
              RegRegalos = CDec(0)
            End If
            fila("DejoGano") = DS_CTA.Tables(0).Rows(i).Item("DejoGano")
            fila("RecaudacionSC") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables(0).Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables(0).Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables(0).Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")

            'fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
            'fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
            'fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
            'fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
            'fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DS_CTA.Tables(0).Rows(i).Item("DejoGano") + DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
            Catch ex As Exception

            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

            DS_ticketsclientes.Tables("TicketGeneral").Rows.Add(fila)

            'ahora si hay mas registros para el mismo cliente, se suma sus valores en cada campo.
            'Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            Dim j = i + 1
            While j < DS_CTA.Tables(0).Rows.Count

              If Cliente = CInt(DS_CTA.Tables(0).Rows(j).Item("Cliente_codigo")) Then
                Dim indice As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
                Try
                  Dim Recaudacion As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") + DS_CTA.Tables(0).Rows(j).Item("Recaudacion")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") = (Math.Round(Recaudacion, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Comision As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") + DS_CTA.Tables(0).Rows(j).Item("Comision")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") = (Math.Round(Comision, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Premios As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") + DS_CTA.Tables(0).Rows(j).Item("Premios")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") = (Math.Round(Premios, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Reclamos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") + DS_CTA.Tables(0).Rows(j).Item("Reclamos")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") = (Math.Round(Reclamos, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                '///////////////////////////REGALOS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  'Dim Regalos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                  'DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") = (Math.Round(Regalos, 2).ToString("N2"))
                  If CDec(DS_CTA.Tables(0).Rows(j).Item("Regalos")) < CDec(0) Then
                    Dim Regalos As Decimal = RegRegalos + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                    RegRegalos = (Math.Round(Regalos, 2).ToString("N2"))
                  End If
                Catch ex As Exception
                End Try
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '////////////////////////DEJOGANO/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                Try
                  Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '/////////////////////////////////SIN CALCULO/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") + DS_CTA.Tables(0).Rows(j).Item("RecaudacionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") = (Math.Round(RecaudacionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") + DS_CTA.Tables(0).Rows(j).Item("ComisionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") = (Math.Round(ComisionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") + DS_CTA.Tables(0).Rows(j).Item("PremiosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") = (Math.Round(PremiosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") + DS_CTA.Tables(0).Rows(j).Item("ReclamosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") = (Math.Round(ReclamosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") = (Math.Round(DejoGanoSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try

              Else
                i = j - 1 'RETROCEDO 1 PARA QUE EL INDICE APUNTE AL PROXIMO NO REPETIDO.(UNA VER EJECUTADO I=I+1)
                Exit While
              End If

              j = j + 1
            End While
            If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
              Dim ind As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("Regalos") = RegRegalos
              '//////////////////
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano")
              'Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") + RegRegalos
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              '//////////////////
              Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano")
              'Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") + RegRegalos
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))

            End If
            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral"), indce)
          End If

        End If
        i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)


        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe02.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral"))

        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))

        CrReport.Dispose()

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)

      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If


    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub

  Private Sub GENERAR_REPORTE5() 'BUSCA 1 CLIENTE, CONSIDERANDO BOLETAS
    Dim DS_ticketsclientes As New DS_ticketsclientes
    Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener2(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), CInt(Txt_ClienteCod.Text))

    If DS_CTA.Tables(0).Rows.Count <> 0 Then

      '//////VERSION ANTERIOR - DESPLIEGA TODOS LOS REGISTRO DE CTACTE/////////////////////////////

      Dim i As Integer = 0
      While i < DS_CTA.Tables(0).Rows.Count


        Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda



          Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral1").NewRow
          fila("Grupo") = CInt(DS_CTA.Tables(0).Rows(i).Item("Grupo_codigo"))
          fila("Cliente") = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
          fila("Recaudacion") = DS_CTA.Tables(0).Rows(i).Item("Recaudacion")
          fila("Comision") = DS_CTA.Tables(0).Rows(i).Item("Comision")
          fila("Premios") = DS_CTA.Tables(0).Rows(i).Item("Premios")
          fila("Reclamos") = DS_CTA.Tables(0).Rows(i).Item("Reclamos")
          'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
          Dim RegRegalos As Decimal = DS_CTA.Tables(0).Rows(i).Item("Regalos")
          If RegRegalos < CDec(0) Then 'solo muestro regalos con negativo...es decir a Favor.
            fila("Regalos") = RegRegalos
          End If

          Dim DejoGano_1 As Decimal = CDec(DS_CTA.Tables(0).Rows(i).Item("DejoGano"))
          If RegRegalos < CDec(0) Then
            'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
            'DejoGano_1 = DejoGano_1 + RegRegalos
            DejoGano_1 = DejoGano_1
          End If

          fila("DejoGano") = (Math.Round(DejoGano_1, 2).ToString("N2"))
          fila("RecaudacionSC") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionSC")
          fila("ComisionSC") = DS_CTA.Tables(0).Rows(i).Item("ComisionSC")
          fila("PremiosSC") = DS_CTA.Tables(0).Rows(i).Item("PremiosSC")
          fila("ReclamosSC") = DS_CTA.Tables(0).Rows(i).Item("ReclamosSC")
          fila("DejoGanoSC") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
          fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
          fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
          fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
          fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
          fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
          fila("Fecha") = DS_CTA.Tables(0).Rows(i).Item("Fecha")

          Dim totaldejogano As Decimal = 0
          Try
            totaldejogano = DejoGano_1 + DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC") + DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
          Catch ex As Exception

          End Try
          fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

          DS_ticketsclientes.Tables("TicketGeneral1").Rows.Add(fila)

          Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral1").Rows.Count - 1
          Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral1"), indce)

        End If
        i = i + 1
      End While
      '///////////////////////////////////////////////////////////////////////////////////////



      If DS_ticketsclientes.Tables("TicketGeneral1").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)


        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe03.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral1").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral1"))

        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))

        CrReport.Dispose()

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If




    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub

  Private Sub generar_reporte6() '1 solo cliente, sin boletas
    Dim DS_ticketsclientes As New DS_ticketsclientes
    Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener2(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), CInt(Txt_ClienteCod.Text))

    If DS_CTA.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables(0).Rows.Count
        Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral1").NewRow
        Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda



          fila("Grupo") = CInt(DS_CTA.Tables(0).Rows(i).Item("Grupo_codigo"))
          fila("Cliente") = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
          fila("Recaudacion") = DS_CTA.Tables(0).Rows(i).Item("Recaudacion")
          fila("Comision") = DS_CTA.Tables(0).Rows(i).Item("Comision")
          fila("Premios") = DS_CTA.Tables(0).Rows(i).Item("Premios")
          fila("Reclamos") = DS_CTA.Tables(0).Rows(i).Item("Reclamos")

          Dim RegRegalos As Decimal = DS_CTA.Tables(0).Rows(i).Item("Regalos")
          If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
            fila("Regalos") = RegRegalos
          End If
          Dim DejoGano_1 As Decimal = CDec(DS_CTA.Tables(0).Rows(i).Item("DejoGano"))
          If RegRegalos < CDec(0) Then
            'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
            'DejoGano_1 = DejoGano_1 + RegRegalos
            DejoGano_1 = DejoGano_1
          End If
          fila("DejoGano") = (Math.Round(DejoGano_1, 2).ToString("N2"))
          fila("RecaudacionSC") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionSC")
          fila("ComisionSC") = DS_CTA.Tables(0).Rows(i).Item("ComisionSC")
          fila("PremiosSC") = DS_CTA.Tables(0).Rows(i).Item("PremiosSC")
          fila("ReclamosSC") = DS_CTA.Tables(0).Rows(i).Item("ReclamosSC")
          fila("DejoGanoSC") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
          fila("Fecha") = DS_CTA.Tables(0).Rows(i).Item("Fecha")
          'fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
          'fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
          'fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
          'fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
          'fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
          Dim totaldejogano As Decimal = 0
          Try
            totaldejogano = DejoGano_1 + DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
          Catch ex As Exception

          End Try
          fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

          DS_ticketsclientes.Tables("TicketGeneral1").Rows.Add(fila)

          Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral1").Rows.Count - 1
          Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral1"), indce)
        End If
        i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral1").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)
        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe04.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral1").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral1"))
        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))
        CrReport.Dispose()
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If
    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub


#Region "reportes para jerarquia 2"
  Private Sub GENERAR_REPORTE_J2(ByVal DS_ticketsclientes As DataSet)
    Dim k As Integer = 0
    DS_ticketsclientes.Tables("Consulta_J2").Rows.Clear()

    If DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Count <> 0 Then
      'ordeno los codigos orden ASC...
      Dim filtro As String = "Grupo > 0 "
      Dim rows_grupos() As DataRow = DS_ticketsclientes.Tables("Permisos_Usuario").Select(filtro)

      While k < rows_grupos.Count

        Dim grupo_codigo As String = rows_grupos(k).Item("Grupo")
        Dim DS_grupo As DataSet = DAtickets.TicketGeneral_obtener1_grupo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), grupo_codigo)
        DS_ticketsclientes.Tables("Consulta_J2").Merge(DS_grupo.Tables(0))
        k = k + 1
      End While
    End If



    Dim DS_CTA As New DS_ticketsclientes

    DS_CTA.Tables("Consulta_J2").Merge(DS_ticketsclientes.Tables("Consulta_J2"))

    'Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener1(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text))

    If DS_CTA.Tables("Consulta_J2").Rows.Count <> 0 Then 'If DS_CTA.Tables(0).Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables("Consulta_J2").Rows.Count
        Dim Cliente As Integer = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo"))
        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda

          Dim filtro_foud_aux As String = "Cliente = " + CStr(Cliente)
          Dim rows_found_aux() As DataRow = DS_ticketsclientes.Tables("TicketGeneral").Select(filtro_foud_aux)

          If rows_found_aux.Count = 0 Then
            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Reclamos")
            'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            Dim RegRegalos As Decimal = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Regalos")

            If RegRegalos > CDec(0) Then
              RegRegalos = CDec(0)
            End If
            fila("DejoGano") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGano")
            fila("RecaudacionSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoSC")

            fila("RecaudacionB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("RecaudacionB")
            fila("ComisionB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ComisionB")
            fila("PremiosB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("PremiosB")
            fila("ReclamosB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ReclamosB")
            fila("DejoGanoB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoB")


            Dim valido = "no"
            If CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo")) = 636 Then
              valido = "si"
            End If


            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGano") + DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoSC") + DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoB")
            Catch ex As Exception

            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

            DS_ticketsclientes.Tables("TicketGeneral").Rows.Add(fila)

            'ahora si hay mas registros para el mismo cliente, se suma sus valores en cada campo.
            'Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            Dim j = i + 1
            While j < DS_CTA.Tables("Consulta_J2").Rows.Count

              If Cliente = CInt(DS_CTA.Tables("Consulta_J2").Rows(j).Item("Cliente_codigo")) Then
                Dim indice As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
                Try
                  Dim Recaudacion As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Recaudacion")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") = (Math.Round(Recaudacion, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim Comision As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Comision")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") = (Math.Round(Comision, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Premios As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Premios")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") = (Math.Round(Premios, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Reclamos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Reclamos")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") = (Math.Round(Reclamos, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                '///////////////////////////REGALOS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  'Dim Regalos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                  'DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") = (Math.Round(Regalos, 2).ToString("N2"))
                  If CDec(DS_CTA.Tables("Consulta_J2").Rows(j).Item("Regalos")) < CDec(0) Then
                    Dim Regalos As Decimal = RegRegalos + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Regalos")
                    RegRegalos = (Math.Round(Regalos, 2).ToString("N2"))
                  End If
                Catch ex As Exception
                End Try
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '////////////////////////DEJOGANO/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                'Dim Regalos_1 As Decimal = 0
                'Try
                '  Regalos_1 = CDec(DS_CTA.Tables(0).Rows(j).Item("Regalos"))
                'Catch ex As Exception
                'End Try

                'Try
                '  If Regalos_1 < CDec(0) Then
                '    Dim calculo As Decimal = CDec(DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano")) + Regalos_1
                '    calculo = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + calculo

                '    DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(calculo, 2).ToString("N2"))
                '  Else
                '    Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables(0).Rows(j).Item("DejoGano")
                '    DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
                '  End If


                'Catch ex As Exception
                'End Try
                Try
                  Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
                Catch ex As Exception

                End Try



                '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '/////////////////////////////////SIN CALCULO/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("RecaudacionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") = (Math.Round(RecaudacionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ComisionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") = (Math.Round(ComisionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("PremiosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") = (Math.Round(PremiosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ReclamosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") = (Math.Round(ReclamosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") = (Math.Round(DejoGanoSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                '/////////////////////////////////////BOLETA/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("RecaudacionB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionB") = (Math.Round(RecaudacionB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ComisionB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionB") = (Math.Round(ComisionB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("PremiosB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosB") = (Math.Round(PremiosB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ReclamosB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosB") = (Math.Round(ReclamosB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoB") = (Math.Round(DejoGanoB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
              Else
                i = j - 1 'RETROCEDO 1 PARA QUE EL INDICE APUNTE AL PROXIMO NO REPETIDO.(UNA VER EJECUTADO I=I+1)
                Exit While
              End If

              j = j + 1
            End While
            If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
              Dim ind As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("Regalos") = RegRegalos
              '//////////////////
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              'Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") + RegRegalos
              Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              '//////////////////
              'Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") + RegRegalos
              Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))



            End If
            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral"), indce)



          End If

        End If
        i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)


        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe01a.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral"))

        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))
        CrReport.Dispose() 'esto para no sobrecargar crystal y generar desbordamientos
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)

      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If


    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub

  Private Sub GENERAR_REPORTE2_J2(ByVal DS_ticketsclientes As DataSet) 'NO TENGO EN CUENTA BOLETA
    Dim k As Integer = 0
    DS_ticketsclientes.Tables("Consulta_J2").Rows.Clear()

    If DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Count <> 0 Then
      'ordeno los codigos orden ASC...
      Dim filtro As String = "Grupo > 0 "
      Dim rows_grupos() As DataRow = DS_ticketsclientes.Tables("Permisos_Usuario").Select(filtro)

      While k < rows_grupos.Count

        Dim grupo_codigo As String = rows_grupos(k).Item("Grupo")
        Dim DS_grupo As DataSet = DAtickets.TicketGeneral_obtener1_grupo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), grupo_codigo)
        DS_ticketsclientes.Tables("Consulta_J2").Merge(DS_grupo.Tables(0))
        k = k + 1
      End While
    End If

    Dim DS_CTA As New DS_ticketsclientes

    DS_CTA.Tables("Consulta_J2").Merge(DS_ticketsclientes.Tables("Consulta_J2"))

    'Dim DS_ticketsclientes As New DS_ticketsclientes
    'Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener1(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text))

    DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Clear()
    DS_ticketsclientes.Tables("TicketGeneral").Rows.Clear()

    If DS_CTA.Tables("Consulta_J2").Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables("Consulta_J2").Rows.Count
        Dim Cliente As Integer = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo"))

        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda



          Dim filtro_foud_aux As String = "Cliente = " + CStr(Cliente)
          Dim rows_found_aux() As DataRow = DS_ticketsclientes.Tables("TicketGeneral").Select(filtro_foud_aux)

          If rows_found_aux.Count = 0 Then
            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Reclamos")
            'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            Dim RegRegalos As Decimal = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Regalos")
            If RegRegalos > CDec(0) Then
              RegRegalos = CDec(0)
            End If
            fila("DejoGano") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGano")
            fila("RecaudacionSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoSC")

            'fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
            'fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
            'fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
            'fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
            'fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGano") + DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoSC")
            Catch ex As Exception

            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

            DS_ticketsclientes.Tables("TicketGeneral").Rows.Add(fila)

            'ahora si hay mas registros para el mismo cliente, se suma sus valores en cada campo.
            'Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            Dim j = i + 1
            While j < DS_CTA.Tables("Consulta_J2").Rows.Count

              If Cliente = CInt(DS_CTA.Tables("Consulta_J2").Rows(j).Item("Cliente_codigo")) Then
                Dim indice As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
                Try
                  Dim Recaudacion As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Recaudacion")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") = (Math.Round(Recaudacion, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Comision As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Comision")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") = (Math.Round(Comision, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Premios As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Premios")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") = (Math.Round(Premios, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Reclamos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Reclamos")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") = (Math.Round(Reclamos, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                '///////////////////////////REGALOS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  'Dim Regalos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                  'DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") = (Math.Round(Regalos, 2).ToString("N2"))
                  If CDec(DS_CTA.Tables("Consulta_J2").Rows(j).Item("Regalos")) < CDec(0) Then
                    Dim Regalos As Decimal = RegRegalos + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Regalos")
                    RegRegalos = (Math.Round(Regalos, 2).ToString("N2"))
                  End If
                Catch ex As Exception
                End Try
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '////////////////////////DEJOGANO/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
                Catch ex As Exception

                End Try


                '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                '/////////////////////////////////SIN CALCULO/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("RecaudacionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") = (Math.Round(RecaudacionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ComisionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") = (Math.Round(ComisionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("PremiosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") = (Math.Round(PremiosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ReclamosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") = (Math.Round(ReclamosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") = (Math.Round(DejoGanoSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try

              Else
                i = j - 1 'RETROCEDO 1 PARA QUE EL INDICE APUNTE AL PROXIMO NO REPETIDO.(UNA VER EJECUTADO I=I+1)
                Exit While
              End If

              j = j + 1
            End While
            If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
              Dim ind As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("Regalos") = RegRegalos

              '//////////////////
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              'Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") + RegRegalos
              Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              '//////////////////
              'Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") + RegRegalos
              Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))


            End If
            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral"), indce)
          End If

        End If
        i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)

        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe02.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral"))
        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))
        CrReport.Dispose()
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If

    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub

  Private Sub GENERAR_REPORTE3_J2(ByVal DS_ticketsclientes As DataSet) 'trae solo 1 grupo y considera boletas
    Dim k As Integer = 0
    DS_ticketsclientes.Tables("Consulta_J2").Rows.Clear()

    If DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Count <> 0 Then
      'ordeno los codigos orden ASC...
      Dim filtro As String = "Grupo > 0 "
      Dim rows_grupos() As DataRow = DS_ticketsclientes.Tables("Permisos_Usuario").Select(filtro)

      While k < rows_grupos.Count

        Dim grupo_codigo As String = rows_grupos(k).Item("Grupo")
        Dim Grupo_ingresado As Integer = 0
        If grupo_codigo = Txt_GrupoCodigo.Text.ToUpper Then
          Dim DS_grupo As DataSet = DAtickets.TicketGeneral_obtener1_grupo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), grupo_codigo)
          DS_ticketsclientes.Tables("Consulta_J2").Merge(DS_grupo.Tables(0))
          Exit While
        End If
        k = k + 1
      End While
    End If

    Dim DS_CTA As New DS_ticketsclientes

    DS_CTA.Tables("Consulta_J2").Merge(DS_ticketsclientes.Tables("Consulta_J2"))

    'Dim DS_ticketsclientes As New DS_ticketsclientes

    DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Clear()
    DS_ticketsclientes.Tables("TicketGeneral").Rows.Clear()


    'Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener1_grupo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), Txt_GrupoCodigo.Text)

    If DS_CTA.Tables("Consulta_J2").Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables("Consulta_J2").Rows.Count
        Dim Cliente As Integer = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo"))

        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda

          Dim filtro_foud_aux As String = "Cliente = " + CStr(Cliente)
          Dim rows_found_aux() As DataRow = DS_ticketsclientes.Tables("TicketGeneral").Select(filtro_foud_aux)

          If rows_found_aux.Count = 0 Then
            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Reclamos")
            'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            Dim RegRegalos As Decimal = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Regalos")
            If RegRegalos > CDec(0) Then
              RegRegalos = CDec(0)
            End If

            fila("DejoGano") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGano")
            fila("RecaudacionSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoSC")
            fila("RecaudacionB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("RecaudacionB")
            fila("ComisionB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ComisionB")
            fila("PremiosB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("PremiosB")
            fila("ReclamosB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ReclamosB")
            fila("DejoGanoB") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoB")
            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGano") + DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoSC") + DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoB")
            Catch ex As Exception
            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))
            DS_ticketsclientes.Tables("TicketGeneral").Rows.Add(fila)

            'ahora si hay mas registros para el mismo cliente, se suma sus valores en cada campo.
            'Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            Dim j = i + 1

            Dim valido = "no"
            If CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo")) = 676 Then
              valido = "si"
            End If

            While j < DS_CTA.Tables("Consulta_J2").Rows.Count

              If Cliente = CInt(DS_CTA.Tables("Consulta_J2").Rows(j).Item("Cliente_codigo")) Then
                Dim indice As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
                Try
                  Dim Recaudacion As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Recaudacion")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") = (Math.Round(Recaudacion, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Comision As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Comision")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") = (Math.Round(Comision, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Premios As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Premios")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") = (Math.Round(Premios, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Reclamos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Reclamos")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") = (Math.Round(Reclamos, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                '///////////////////////////REGALOS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  'Dim Regalos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                  'DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") = (Math.Round(Regalos, 2).ToString("N2"))
                  If CDec(DS_CTA.Tables("Consulta_J2").Rows(j).Item("Regalos")) < CDec(0) Then
                    Dim Regalos As Decimal = RegRegalos + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Regalos")
                    RegRegalos = (Math.Round(Regalos, 2).ToString("N2"))
                  End If
                Catch ex As Exception
                End Try
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '////////////////////////DEJOGANO/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                Try
                  Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))

                Catch ex As Exception
                End Try
                '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '/////////////////////////////////SIN CALCULO/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("RecaudacionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") = (Math.Round(RecaudacionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ComisionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") = (Math.Round(ComisionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("PremiosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") = (Math.Round(PremiosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ReclamosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") = (Math.Round(ReclamosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") = (Math.Round(DejoGanoSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                '/////////////////////////////////////BOLETA/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("RecaudacionB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionB") = (Math.Round(RecaudacionB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ComisionB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionB") = (Math.Round(ComisionB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("PremiosB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosB") = (Math.Round(PremiosB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ReclamosB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosB") = (Math.Round(ReclamosB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoB As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoB") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoB") = (Math.Round(DejoGanoB, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoB")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
              Else
                i = j - 1 'RETROCEDO 1 PARA QUE EL INDICE APUNTE AL PROXIMO NO REPETIDO.(UNA VER EJECUTADO I=I+1)
                Exit While
              End If

              j = j + 1
            End While
            If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
              Dim ind As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("Regalos") = RegRegalos

              '//////////////////
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              'Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") + RegRegalos
              Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              '//////////////////
              'Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") + RegRegalos
              Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano")
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))

            End If
            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral"), indce)
          End If

        End If


        i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)

        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe01a.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral"))
        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))
        CrReport.Dispose()
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If

    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub


  Private Sub GENERAR_REPORTE4_J2(ByVal DS_ticketsclientes As DataSet) '1 SOLO GRUPO, PERO SIN BOLETAS
    Dim k As Integer = 0
    DS_ticketsclientes.Tables("Consulta_J2").Rows.Clear()

    If DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Count <> 0 Then
      'ordeno los codigos orden ASC...
      Dim filtro As String = "Grupo > 0 "
      Dim rows_grupos() As DataRow = DS_ticketsclientes.Tables("Permisos_Usuario").Select(filtro)

      While k < rows_grupos.Count

        Dim grupo_codigo As String = rows_grupos(k).Item("Grupo")
        Dim Grupo_ingresado As Integer = 0
        If grupo_codigo = Txt_GrupoCodigo.Text.ToUpper Then
          Dim DS_grupo As DataSet = DAtickets.TicketGeneral_obtener1_grupo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), grupo_codigo)
          DS_ticketsclientes.Tables("Consulta_J2").Merge(DS_grupo.Tables(0))
          Exit While
        End If
        k = k + 1
      End While
    End If

    Dim DS_CTA As New DS_ticketsclientes

    DS_CTA.Tables("Consulta_J2").Merge(DS_ticketsclientes.Tables("Consulta_J2"))

    'Dim DS_ticketsclientes As New DS_ticketsclientes
    'Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener1_grupo(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), Txt_GrupoCodigo.Text)

    DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Clear()
    DS_ticketsclientes.Tables("TicketGeneral").Rows.Clear()

    If DS_CTA.Tables("Consulta_J2").Rows.Count <> 0 Then
      Dim i As Integer = 0
      While i < DS_CTA.Tables("Consulta_J2").Rows.Count
        Dim Cliente As Integer = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo"))
        Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
        If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda

          Dim filtro_foud_aux As String = "Cliente = " + CStr(Cliente)
          Dim rows_found_aux() As DataRow = DS_ticketsclientes.Tables("TicketGeneral").Select(filtro_foud_aux)

          If rows_found_aux.Count = 0 Then
            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables("Consulta_J2").Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Reclamos")
            'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            Dim RegRegalos As Decimal = DS_CTA.Tables("Consulta_J2").Rows(i).Item("Regalos")

            If RegRegalos > CDec(0) Then
              RegRegalos = CDec(0)
            End If
            fila("DejoGano") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGano")
            fila("RecaudacionSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoSC")

            'fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
            'fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
            'fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
            'fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
            'fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGano") + DS_CTA.Tables("Consulta_J2").Rows(i).Item("DejoGanoSC")
            Catch ex As Exception

            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

            DS_ticketsclientes.Tables("TicketGeneral").Rows.Add(fila)

            'ahora si hay mas registros para el mismo cliente, se suma sus valores en cada campo.
            'Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            Dim j = i + 1
            While j < DS_CTA.Tables("Consulta_J2").Rows.Count

              If Cliente = CInt(DS_CTA.Tables("Consulta_J2").Rows(j).Item("Cliente_codigo")) Then
                Dim indice As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
                Try
                  Dim Recaudacion As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Recaudacion")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Recaudacion") = (Math.Round(Recaudacion, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Comision As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Comision")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Comision") = (Math.Round(Comision, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Premios As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Premios")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Premios") = (Math.Round(Premios, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim Reclamos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Reclamos")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Reclamos") = (Math.Round(Reclamos, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                '///////////////////////////REGALOS/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Try
                  'Dim Regalos As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") + DS_CTA.Tables(0).Rows(j).Item("Regalos")
                  'DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("Regalos") = (Math.Round(Regalos, 2).ToString("N2"))
                  If CDec(DS_CTA.Tables("Consulta_J2").Rows(j).Item("Regalos")) < CDec(0) Then
                    Dim Regalos As Decimal = RegRegalos + DS_CTA.Tables("Consulta_J2").Rows(j).Item("Regalos")
                    RegRegalos = (Math.Round(Regalos, 2).ToString("N2"))
                  End If
                Catch ex As Exception
                End Try
                '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '////////////////////////DEJOGANO/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                Try
                  Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                '/////////////////////////////////SIN CALCULO/////////////////////////////////////////////////////////////////
                Try
                  Dim RecaudacionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("RecaudacionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("RecaudacionSC") = (Math.Round(RecaudacionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ComisionSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ComisionSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ComisionSC") = (Math.Round(ComisionSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim PremiosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("PremiosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("PremiosSC") = (Math.Round(PremiosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim ReclamosSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("ReclamosSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("ReclamosSC") = (Math.Round(ReclamosSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try
                Try
                  Dim DejoGanoSC As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("DejoGanoSC") = (Math.Round(DejoGanoSC, 2).ToString("N2"))
                Catch ex As Exception
                End Try

                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGano")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try
                Try
                  Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") + DS_CTA.Tables("Consulta_J2").Rows(j).Item("DejoGanoSC")
                  DS_ticketsclientes.Tables("TicketGeneral").Rows(indice).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))
                Catch ex As Exception

                End Try

              Else
                i = j - 1 'RETROCEDO 1 PARA QUE EL INDICE APUNTE AL PROXIMO NO REPETIDO.(UNA VER EJECUTADO I=I+1)
                Exit While
              End If

              j = j + 1
            End While
            If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
              Dim ind As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("Regalos") = RegRegalos
              '//////////////////
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano")
              'Dim DejoGano As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") + RegRegalos
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("DejoGano") = (Math.Round(DejoGano, 2).ToString("N2"))
              '//////////////////
              Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano")
              'Dim totalDG As Decimal = DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") + RegRegalos
              DS_ticketsclientes.Tables("TicketGeneral").Rows(ind).Item("TotalDejoGano") = (Math.Round(totalDG, 2).ToString("N2"))

            End If
            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral"), indce)
          End If

        End If

        i = i + 1
      End While

      If DS_ticketsclientes.Tables("TicketGeneral").Rows.Count <> 0 Then
        'GENERO EL REPORTE.
        Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
        fila2("Fecha") = CDate(Now)
        fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
        fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
        DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)


        Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
        CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe02.rpt"))
        CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
        CrReport.Database.Tables("TicketGeneral").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral"))

        CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))

        CrReport.Dispose()

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)

      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If


    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub


  Private Sub GENERAR_REPORTE5_J2(ByVal DS_ticketsclientes As DataSet) 'BUSCA 1 CLIENTE, CONSIDERANDO BOLETAS
    'VALIDACION: SOLO CONSULTO SI EL CLIENTE PERTENECE A ALGUN GRUPO CONFIGURADO PARA EL USUARIO DE JERARQUIA 2.
    Dim valido = "no"
    Dim Cliente_codigo As String = Txt_ClienteCod.Text
    Dim dt_clie_search As DataTable = DAclientes.Clientes_buscar_grupo(Cliente_codigo)
    If dt_clie_search.Rows.Count <> 0 Then
      Dim Grupo_codigo As Integer = CInt(dt_clie_search.Rows(0).Item("Grupos_codigo"))
      Dim i As Integer = 0
      While i < DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Count
        If DS_ticketsclientes.Tables("Permisos_Usuario").Rows(i).Item("Grupo") = Grupo_codigo Then
          valido = "si"
          Exit While
        End If
        i = i + 1
      End While
    Else
      valido = "no"
    End If

    If valido = "si" Then

      'Dim DS_ticketsclientes As New DS_ticketsclientes
      Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener2(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), CInt(Txt_ClienteCod.Text))

      If DS_CTA.Tables(0).Rows.Count <> 0 Then

        '//////VERSION ANTERIOR - DESPLIEGA TODOS LOS REGISTRO DE CTACTE/////////////////////////////

        Dim i As Integer = 0
        While i < DS_CTA.Tables(0).Rows.Count
          Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
          Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
          If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda



            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral1").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables(0).Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables(0).Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables(0).Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables(0).Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables(0).Rows(i).Item("Reclamos")
            'fila("Regalos") = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            Dim RegRegalos As Decimal = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            If RegRegalos < CDec(0) Then 'solo muestro regalos con negativo...es decir a Favor.
              fila("Regalos") = RegRegalos
            End If

            Dim DejoGano_1 As Decimal = CDec(DS_CTA.Tables(0).Rows(i).Item("DejoGano"))
            If RegRegalos < CDec(0) Then
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              'DejoGano_1 = DejoGano_1 + RegRegalos
              DejoGano_1 = DejoGano_1
            End If

            fila("DejoGano") = (Math.Round(DejoGano_1, 2).ToString("N2"))
            fila("RecaudacionSC") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables(0).Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables(0).Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables(0).Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
            fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
            fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
            fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
            fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
            fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            fila("Fecha") = DS_CTA.Tables(0).Rows(i).Item("Fecha")

            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DejoGano_1 + DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC") + DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            Catch ex As Exception

            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

            DS_ticketsclientes.Tables("TicketGeneral1").Rows.Add(fila)

            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral1").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral1"), indce)

          End If
          i = i + 1
        End While
        '///////////////////////////////////////////////////////////////////////////////////////

        If DS_ticketsclientes.Tables("TicketGeneral1").Rows.Count <> 0 Then
          'GENERO EL REPORTE.
          Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
          fila2("Fecha") = CDate(Now)
          fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
          fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
          DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)


          Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
          CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
          CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe03.rpt"))
          CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
          CrReport.Database.Tables("TicketGeneral1").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral1"))

          CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))

          CrReport.Dispose()

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
        Else
          'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
        End If
      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If
    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If


  End Sub

  Private Sub generar_reporte6_J2(ByVal DS_ticketsclientes As DataSet) '1 solo cliente, sin boletas
    'VALIDACION: SOLO CONSULTO SI EL CLIENTE PERTENECE A ALGUN GRUPO CONFIGURADO PARA EL USUARIO DE JERARQUIA 2.
    Dim valido = "no"
    Dim Cliente_codigo As String = Txt_ClienteCod.Text
    Dim dt_clie_search As DataTable = DAclientes.Clientes_buscar_grupo(Cliente_codigo)
    If dt_clie_search.Rows.Count <> 0 Then
      Dim Grupo_codigo As Integer = CInt(dt_clie_search.Rows(0).Item("Grupos_codigo"))
      Dim i As Integer = 0
      While i < DS_ticketsclientes.Tables("Permisos_Usuario").Rows.Count
        If DS_ticketsclientes.Tables("Permisos_Usuario").Rows(i).Item("Grupo") = Grupo_codigo Then
          valido = "si"
          Exit While
        End If
        i = i + 1
      End While
    Else
      valido = "no"
    End If

    If valido = "si" Then
      'Dim DS_ticketsclientes As New DS_ticketsclientes
      Dim DS_CTA As DataSet = DAtickets.TicketGeneral_obtener2(CDate(Txt_FechaDesde.Text), CDate(Txt_FechaHasta.Text), CInt(Txt_ClienteCod.Text))

      If DS_CTA.Tables(0).Rows.Count <> 0 Then
        Dim i As Integer = 0
        While i < DS_CTA.Tables(0).Rows.Count
          Dim Cliente As Integer = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
          Dim SearchClienteDeuda As DataSet = DAtickets.ClienteDeudaBuscar(CStr(Cliente))
          If SearchClienteDeuda.Tables(0).Rows.Count = 0 Then 'solamente agrego registro si no es un ClienteDeuda



            Dim fila As DataRow = DS_ticketsclientes.Tables("TicketGeneral1").NewRow
            fila("Grupo") = CInt(DS_CTA.Tables(0).Rows(i).Item("Grupo_codigo"))
            fila("Cliente") = CInt(DS_CTA.Tables(0).Rows(i).Item("Cliente_codigo"))
            fila("Recaudacion") = DS_CTA.Tables(0).Rows(i).Item("Recaudacion")
            fila("Comision") = DS_CTA.Tables(0).Rows(i).Item("Comision")
            fila("Premios") = DS_CTA.Tables(0).Rows(i).Item("Premios")
            fila("Reclamos") = DS_CTA.Tables(0).Rows(i).Item("Reclamos")

            Dim RegRegalos As Decimal = DS_CTA.Tables(0).Rows(i).Item("Regalos")
            If RegRegalos < 0 Then 'solo muestro regalos con negativo...es decir a Favor.
              fila("Regalos") = RegRegalos
            End If
            Dim DejoGano_1 As Decimal = CDec(DS_CTA.Tables(0).Rows(i).Item("DejoGano"))
            If RegRegalos < CDec(0) Then
              'NOTA: 2023-01-27 LA COLUMNA REGALOS NO AFECTA AL DEJOGANO
              'DejoGano_1 = DejoGano_1 + RegRegalos
              DejoGano_1 = DejoGano_1
            End If
            fila("DejoGano") = (Math.Round(DejoGano_1, 2).ToString("N2"))
            fila("RecaudacionSC") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionSC")
            fila("ComisionSC") = DS_CTA.Tables(0).Rows(i).Item("ComisionSC")
            fila("PremiosSC") = DS_CTA.Tables(0).Rows(i).Item("PremiosSC")
            fila("ReclamosSC") = DS_CTA.Tables(0).Rows(i).Item("ReclamosSC")
            fila("DejoGanoSC") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
            fila("Fecha") = DS_CTA.Tables(0).Rows(i).Item("Fecha")
            'fila("RecaudacionB") = DS_CTA.Tables(0).Rows(i).Item("RecaudacionB")
            'fila("ComisionB") = DS_CTA.Tables(0).Rows(i).Item("ComisionB")
            'fila("PremiosB") = DS_CTA.Tables(0).Rows(i).Item("PremiosB")
            'fila("ReclamosB") = DS_CTA.Tables(0).Rows(i).Item("ReclamosB")
            'fila("DejoGanoB") = DS_CTA.Tables(0).Rows(i).Item("DejoGanoB")
            Dim totaldejogano As Decimal = 0
            Try
              totaldejogano = DejoGano_1 + DS_CTA.Tables(0).Rows(i).Item("DejoGanoSC")
            Catch ex As Exception

            End Try
            fila("TotalDejoGano") = (Math.Round(totaldejogano, 2).ToString("N2"))

            DS_ticketsclientes.Tables("TicketGeneral1").Rows.Add(fila)

            Dim indce As Integer = DS_ticketsclientes.Tables("TicketGeneral1").Rows.Count - 1
            Validar_RegConInfo(DS_ticketsclientes.Tables("TicketGeneral1"), indce)
          End If
          i = i + 1
        End While

        If DS_ticketsclientes.Tables("TicketGeneral1").Rows.Count <> 0 Then
          'GENERO EL REPORTE.
          Dim fila2 As DataRow = DS_ticketsclientes.Tables("TicketGeneral_info").NewRow
          fila2("Fecha") = CDate(Now)
          fila2("Fecha_desde") = CDate(Txt_FechaDesde.Text)
          fila2("Fecha_hasta") = CDate(Txt_FechaHasta.Text)
          DS_ticketsclientes.Tables("TicketGeneral_info").Rows.Add(fila2)
          Dim CrReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
          CrReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
          CrReport.Load(Server.MapPath("~/WC_Reportes/Rpt/TicketGeneral_informe04.rpt"))
          CrReport.Database.Tables("TicketGeneral_info").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral_info"))
          CrReport.Database.Tables("TicketGeneral1").SetDataSource(DS_ticketsclientes.Tables("TicketGeneral1"))
          CrReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, String.Concat(Server.MapPath("~"), "/WC_Reportes/Rpt/TicketGeneral.pdf"))
          CrReport.Dispose()
          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok", "$(document).ready(function () {$('#modal-ok').modal();});", True)
        Else
          'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

          ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
        End If
      Else
        'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
      End If
    Else
      'AQUI MSJ ERROR...LA BUSQUEDA NO ARROJO RESULTADOS

      ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "modal-ok_error3", "$(document).ready(function () {$('#modal-ok_error3').modal();});", True)
    End If

  End Sub


#End Region


  Private Sub Validar_RegConInfo(ByRef TicketGeneral As DataTable, ByVal indice As Integer)
    'se muestra el registro siempre que tenga un valor en TotalDejoGano
    Dim valido As String = "no"
    Try
      Dim TotalDejoGano As Decimal = TicketGeneral.Rows(indice).Item("TotalDejoGano")
      If TotalDejoGano <> CDec(0) Then
        valido = "si"
      End If
    Catch ex As Exception
    End Try

    If valido = "no" Then
      TicketGeneral.Rows.RemoveAt(indice)
    End If

  End Sub
  Private Sub btn_error_close2_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close2.ServerClick
    Txt_FechaDesde.Focus()
  End Sub

  Private Sub btn_ok_error2_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error2.ServerClick
    Txt_FechaDesde.Focus()
  End Sub

  Private Sub btn_ok_error3_ServerClick(sender As Object, e As EventArgs) Handles btn_ok_error3.ServerClick
    Txt_FechaDesde.Focus()
  End Sub

  Private Sub btn_error_close3_ServerClick(sender As Object, e As EventArgs) Handles btn_error_close3.ServerClick
    Txt_FechaDesde.Focus()
  End Sub
End Class
