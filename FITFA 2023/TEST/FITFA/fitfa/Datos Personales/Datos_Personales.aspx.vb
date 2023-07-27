﻿Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Drawing
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports itextsharp.text.pdf
Imports System.Drawing.Imaging


Public Class Datos_Personales
    Inherits System.Web.UI.Page
    Dim DAusuario As New Capa_de_datos.usuario
    Dim DAllave As New Capa_de_datos.Llave
    Dim guardado As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lbl_errApe.Visible = False
        lbl_errCP.Visible = False
        lbl_errDir.Visible = False
        lbl_errFecNac.Visible = False
        'lbl_errNac.Visible = False
        lbl_errNom.Visible = False
        lbl_errTel.Visible = False
        lbl_errMail.Visible = False
        lbl_err_libreta.Visible = False
        lbl_errpass.Visible = False
        lbl_errus.Visible = False
        If Not IsPostBack Then

            Cargar_Datos()
            div_modalmsjOK.Visible = False
            guardado = False

        End If

    End Sub

    Private Sub Cargar_Datos()
        Dim ds_Usuarios As DataSet = DAusuario.Datos_Personales_Obtener_Datos_Usuarios(Session("Us_id"))
        ''Dim ds_Usuarios As DataSet = DAusuario.Datos_Personales_Obtener_Datos_Usuarios(199)
        If ds_Usuarios.Tables(0).Rows.Count <> 0 Then
            tb_nombre.Value = ds_Usuarios.Tables(0).Rows(0).Item(0)
            tb_apellido.Value = ds_Usuarios.Tables(0).Rows(0).Item(1)
            tb_fechnacc.Value = ds_Usuarios.Tables(0).Rows(0).Item(2)
            tb_us.Value = ds_Usuarios.Tables(0).Rows(0).Item("usuario_usuario")
            tb_pass.Attributes("placeholder") = ds_Usuarios.Tables(0).Rows(0).Item("usuario_password")

            Dim vari = combo_Sexo.SelectedIndex
            Dim varia = combo_Sexo.SelectedValue
            Dim variasl = combo_Sexo.Items
            Dim variassl = combo_Sexo.SelectedItem

            If ds_Usuarios.Tables(0).Rows(0).Item("usuario_sexo") = "Hombre" Then
                combo_Sexo.SelectedValue = "Hombre"
            Else
                combo_Sexo.SelectedValue = "Mujer"
            End If

            combo_EstCivil.SelectedValue = ds_Usuarios.Tables(0).Rows(0).Item(5)
            'tb_profesion.Value = ds_Usuarios.Tables(0).Rows(0).Item(9)
            tb_dir.Value = ds_Usuarios.Tables(0).Rows(0).Item(8)
            'tb_CP.Value = ds_Usuarios.Tables(0).Rows(0).Item(10)
            textbox_CP.Text = ds_Usuarios.Tables(0).Rows(0).Item(10)
            Combo_provincia.SelectedValue = ds_Usuarios.Tables(0).Rows(0).Item(15)
            combo_ciudad.SelectedValue = ds_Usuarios.Tables(0).Rows(0).Item(16)
            tb_tel.Value = ds_Usuarios.Tables(0).Rows(0).Item(11)
            tb_Email.Value = ds_Usuarios.Tables(0).Rows(0).Item(12)
            tb_nrolibreta.Value = ds_Usuarios.Tables(0).Rows(0).Item("usuario_nrolibreta").ToString
            tb_dni.Text = ds_Usuarios.Tables(0).Rows(0).Item("usuario_doc").ToString
            cmb_instructor.SelectedValue = ds_Usuarios.Tables(3).Rows(0).Item("instructor_id")


            Dim graduacion_id As Integer = ds_Usuarios.Tables(0).Rows(0).Item("graduacion_id")
            'como en el evento init recupero la graduacion, solo tengo que seleccionarla
            Combo_graduacion.SelectedValue = graduacion_id


            'If Session("imagen64") = "" Then
            '    Dim ImagenBD As Byte() = ds_Usuarios.Tables(0).Rows(0).Item("usuario_foto")
            '    Dim ImagenDataURL64 As String = "data:image/jpg;base64," + Convert.ToBase64String(ImagenBD)
            '    imgPrev.ImageUrl = ImagenDataURL64
            '    Session("imagen_registro") = ImagenBD
            '    ' Dim imagenPrev As HtmlImage = CType(panelImagen.FindControl("imagenPrev"), HtmlImage)
            'Else
            '    imgPrev.ImageUrl = Session("imagen64")
            'End If






            tb_grad.Value = Combo_graduacion.SelectedItem.ToString
                tb_inst.Value = cmb_instructor.SelectedItem.ToString

                'If ds_Usuarios.Tables(0).Rows(0).Item("usuario_tipo").ToString() = "instructor" Then
                '    Combo_graduacion.Visible = True
                '    tb_grad.Visible = False
                '    cmb_instructor.Visible = True
                '    tb_inst.Visible = False
                'End If

            End If
    End Sub
    Private Sub obtener_graduaciones()
        Dim ds_graduaciones As DataSet = DAusuario.Usuario_ObtenerGraduaciones()
        If ds_graduaciones.Tables(0).Rows.Count <> 0 Then
            Combo_graduacion.DataSource = ds_graduaciones.Tables(0)
            Combo_graduacion.DataTextField = "graduacion_desc"
            Combo_graduacion.DataValueField = "graduacion_id"
            Combo_graduacion.DataBind()
        End If
    End Sub

    '' FOTO

#Region "Combos"



    Private Sub obtener_estadocivil()
        Dim ds_estadocivil As DataSet = DAusuario.Estado_civil_obtener
        If ds_estadocivil.Tables(0).Rows.Count <> 0 Then
            combo_EstCivil.DataSource = ds_estadocivil.Tables(0)
            combo_EstCivil.DataTextField = "estadocivil_desc"
            combo_EstCivil.DataValueField = "estadocivil_id"
            combo_EstCivil.DataBind()
        End If

    End Sub
    Public Sub Obtener_provincias()
        Dim ds_provincias As DataSet = DAusuario.Usuario_ObtenerProvincias()

        If ds_provincias.Tables(0).Rows.Count <> 0 Then
            Combo_provincia.DataSource = ds_provincias.Tables(0)
            Combo_provincia.DataTextField = "provincia_desc"
            Combo_provincia.DataValueField = "provincia_id"
            Combo_provincia.DataBind()
        End If
    End Sub
    Private Sub Obtener_ciudad()
        'filtrar
        combo_ciudad.DataSource = ""
        combo_ciudad.DataBind()

        Dim ds_ciudades As DataSet = DAusuario.Usuario_filtrarciudades_x_Provincias(CInt(Combo_provincia.SelectedValue))
        If ds_ciudades.Tables(0).Rows.Count <> 0 Then
            combo_ciudad.DataSource = ds_ciudades.Tables(0)

            combo_ciudad.DataTextField = "ciudad_desc"
            combo_ciudad.DataValueField = "ciudad_id"
            combo_ciudad.DataBind()
        End If
    End Sub

    Private Sub Obtener_instructores()
        'filtrar
        cmb_instructor.DataSource = ""
        cmb_instructor.DataBind()
        Dim ds_instructor As DataSet = DAusuario.Usuario_ObtenerInstructor(23) '23 es la Institucion ANT
        If ds_instructor.Tables(0).Rows.Count <> 0 Then
            cmb_instructor.DataSource = ds_instructor.Tables(0)
            cmb_instructor.DataTextField = "Nombre"
            cmb_instructor.DataValueField = "instructor_id"
            cmb_instructor.DataBind()
        End If
    End Sub

    Private Sub cmb_instructor_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmb_instructor.Init
        Obtener_instructores()
    End Sub

    Private Sub Combo_provincia_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Combo_provincia.Init
        Obtener_provincias()
        Obtener_ciudad()
        'recuperar las graduaciones
        obtener_graduaciones()
    End Sub
    Private Sub combo_EstCivil_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles combo_EstCivil.Init
        obtener_estadocivil()
    End Sub

    Private Sub Combo_provincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Combo_provincia.SelectedIndexChanged
        Obtener_ciudad()
    End Sub


#End Region





    Private Sub btn_cargarImagen_Click(sender As Object, e As EventArgs) Handles btn_cargarImagen.Click
        Response.Redirect("CargarImagen.aspx")
    End Sub

    Dim nom1 As Integer = 1111
    Dim nom2 As Integer = 2222
    Dim nom3 As Integer = 3333
    Dim nom4 As Integer = 4444

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
    '    'Response.Clear()
    '    'Response.ContentType = "application/pdf"
    '    'Response.Cache.SetCacheability(HttpCacheability.NoCache)

    '    'Dim doc As New Document(PageSize.LETTER.Rotate(), 10, 10, 10, 10)

    '    'Dim path As String = Me.Server.MapPath(".") + "\Miarchivo.pdf"

    '    'Dim file As New FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)

    '    'PdfWriter.GetInstance(doc, file)
    '    'doc.Open()


    '    'doc.Add(New Paragraph("Reporte:"))
    '    'doc.Add(New Paragraph("DATOS PERSONALES:"))
    '    'doc.Add(New Paragraph(" "))
    '    'doc.Add(New Paragraph("Apellido y nombre:"))

    '    'doc.NewPage()
    '    'doc.Add(New Paragraph("Hello world again"))



    '    'doc.Close()
    '    'Process.Start(path)

    '    ''Dim pdfDoc As New Document()
    '    ''Dim pdfWrite As PdfWriter = PdfWriter.GetInstance(pdfDoc, New FileStream("Simple.pdf", FileMode.Create))
    '    ''pdfDoc.Open()
    '    ''pdfDoc.Add(New Paragraph("Hello world"))
    '    ''pdfDoc.NewPage()
    '    ''pdfDoc.Add(New Paragraph("Hello world again"))
    '    ''pdfDoc.Close()

    'End Sub
#Region "Gestion Foto"

    Dim tamanio As Integer
    Dim ImagenOriginal As Byte()
    Dim ImagenOriginalBinaria As Bitmap
    Dim ImagenDataURL64 As String




    'Private Sub Button_adjuntar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_adjuntar.Click

    '    If FileUpload1.HasFile Then
    '        Dim fileExt As String = System.IO.Path.GetExtension(FileUpload1.FileName)
    '        If fileExt = ".jpeg" Or fileExt = ".bmp" Or fileExt = ".png" Or fileExt = ".jpg" Then



    '            tamanio = FileUpload1.PostedFile.ContentLength
    '            'int Tamanio = fuploadImagen.PostedFile.ContentLength;
    '            'choco
    '            ImagenOriginal = New Byte(tamanio - 1) {}
    '            'byte[] ImagenOriginal = new byte[Tamanio];
    '            'choco
    '            FileUpload1.PostedFile.InputStream.Read(ImagenOriginal, 0, tamanio)
    '            'fuploadImagen.PostedFile.InputStream.Read(ImagenOriginal, 0, Tamanio);
    '            'choco
    '            ImagenOriginalBinaria = New Bitmap(FileUpload1.PostedFile.InputStream)
    '            'Bitmap ImagenOriginalBinaria = new Bitmap(fuploadImagen.PostedFile.InputStream);
    '            'choco
    '            ImagenDataURL64 = "data:image/jpg;base64," + Convert.ToBase64String(ImagenOriginal)
    '            'string ImagenDataURL64 = "data:image/jpg;base64." + Convert.ToBase64String(ImagenOriginal);

    '            Session("imagen") = ImagenOriginal
    '            Image1.ImageUrl = ImagenDataURL64

    '            Image1.Visible = True
    '            'lbl_errImg.Visible = False
    '            'btn_Examinar.Visible = False
    '            'btn_quitar.Visible = True
    '        Else
    '            'lbl_errImg.Visible = True
    '            'lbl_errImg.InnerText = "Solo Archivos de Tipo Imagen"
    '        End If

    '    End If

    'End Sub



    'Public Function ImageControlToByteArray(ByVal foto)
    '    Return File.ReadAllBytes(Server.MapPath(foto.ImageUrl))
    'End Function

    Dim foto_cargada As String

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click


        Dim Vacio As Boolean
        If tb_apellido.Value <> "" Then

        Else
            lbl_errApe.Visible = True
            Vacio = True
        End If

        'If textbox_CP.Text <> "" Then

        'Else
        '    lbl_errCP.Visible = True
        '    Vacio = True
        'End If
        'If tb_dir.Value <> "" Then

        'Else
        '    lbl_errDir.Visible = True
        '    Vacio = True
        'End If

        'If tb_Email.Value <> "" Then

        'Else
        '    lbl_errMail.Visible = True
        '    Vacio = True
        'End If

        If tb_fechnacc.Value <> "" Then

        Else
            lbl_errFecNac.Visible = True
            Vacio = True
        End If

        'If tb_nacionalidad.Value <> "" Then

        'Else
        '    lbl_errNac.Visible = True
        '    Vacio = True
        'End If

        If tb_nombre.Value <> "" Then

        Else
            lbl_errNom.Visible = True
            Vacio = True
        End If

        'If tb_tel.Value <> "" Then

        'Else
        '    lbl_errTel.Visible = True
        '    Vacio = True
        'End If

        If tb_us.Value <> "" Then

        Else
            lbl_errus.Visible = True
            Vacio = True
        End If

        If tb_pass.Value <> "" Then

        Else
            lbl_errpass.Visible = True
            Vacio = True
        End If
        'If tb_nrolibreta.Value <> "" Then
        'Else
        '    lbl_err_libreta.Visible = True
        '    Vacio = True
        'End If
        Try
            If textbox_CP.Text = "" Then
                textbox_CP.Text = CInt(0)
            End If
        Catch ex As Exception

        End Try


        If Vacio <> True Then
            'DAusuario.Datos_Personales_Actualizar_Datos(CInt(Session("Us_id")), tb_nombre.Value, tb_apellido.Value, tb_fechnacc.Value, "Argentino", combo_Sexo.SelectedValue, combo_EstCivil.SelectedValue, "Estudiante", tb_dir.Value, textbox_CP.Text, Combo_provincia.SelectedValue, combo_ciudad.SelectedValue, tb_tel.Value, tb_Email.Value, tb_nrolibreta.Value, Combo_graduacion.SelectedValue, tb_us.Value, tb_pass.Value, Session("imagen_registro"))
            DAusuario.Datos_Personales_Actualizar_Datos_SinFoto(CInt(Session("Us_id")), tb_nombre.Value, tb_apellido.Value, tb_fechnacc.Value, "Argentino", combo_Sexo.SelectedValue, combo_EstCivil.SelectedValue, "", tb_dir.Value, textbox_CP.Text, Combo_provincia.SelectedValue, combo_ciudad.SelectedValue, tb_tel.Value, tb_Email.Value, tb_nrolibreta.Value, Combo_graduacion.SelectedValue, tb_us.Value, tb_pass.Value)
            '
            'div_registro_guardado.Visible = True
            'Limpio las Variables de sesion
            Session("imagen64") = ""
            Session("imagen_registro") = ""
            'Actualizo Instructor 19-10-21 -MGO
            DAusuario.alumuno_x_instructor_Actulizar(cmb_instructor.SelectedValue, CInt(Session("Us_id")))


            '++++++++++++++Esto hago para que se haga visible el cartel de "datos actualizados"++++++++++++++
            div_modalmsjOK.Visible = True
            Modal_msjOK.Show()
            '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        Else
            div_registro_guardado.Visible = False

        End If


    End Sub

    Private Sub boton_ok_Click(sender As Object, e As EventArgs) Handles boton_ok.Click


        Response.Redirect("../Inicio_Blanco.aspx")
    End Sub

    'Protected Sub btnCargarImagen_Click(sender As Object, e As EventArgs) Handles btnCargarImagen.Click
    '    Response.Redirect("CargarImagen.aspx")
    'End Sub




    'Private Sub Btn_aceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_aceptar.Click
    '    foto_cargada = "no"
    '    If FileUpload1.HasFile Then
    '        Dim fileExt As String = System.IO.Path.GetExtension(FileUpload1.FileName)
    '        If fileExt = ".jpeg" Or fileExt = ".bmp" Or fileExt = ".png" Or fileExt = ".jpg" Then

    '            tamanio = FileUpload1.PostedFile.ContentLength
    '            'int Tamanio = fuploadImagen.PostedFile.ContentLength;
    '            'choco
    '            ImagenOriginal = New Byte(tamanio - 1) {}
    '            'byte[] ImagenOriginal = new byte[Tamanio];
    '            'choco
    '            FileUpload1.PostedFile.InputStream.Read(ImagenOriginal, 0, tamanio)
    '            'fuploadImagen.PostedFile.InputStream.Read(ImagenOriginal, 0, Tamanio);
    '            'choco
    '            ImagenOriginalBinaria = New Bitmap(FileUpload1.PostedFile.InputStream)
    '            'Bitmap ImagenOriginalBinaria = new Bitmap(fuploadImagen.PostedFile.InputStream);
    '            'choco
    '            ImagenDataURL64 = "data:image/jpg;base64," + Convert.ToBase64String(ImagenOriginal)
    '            'string ImagenDataURL64 = "data:image/jpg;base64." + Convert.ToBase64String(ImagenOriginal);
    '            Session("imagen") = ImagenOriginal
    '            Image1.ImageUrl = ImagenDataURL64
    '            'Image1.Visible = True
    '            'lbl_errImg.Visible = False
    '            'btn_Examinar.Visible = False
    '            'btn_quitar.Visible = True
    '            foto_cargada = "si"

    '            Session("foto_subido_registro") = "si" 'choco: 23-07-2019
    '            Session("imagen_registro") = ImagenOriginal 'choco: 23-07-2019
    '        Else


    '        End If

    '    End If

    'End Sub

    'Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
    '    'boton quitar foto
    '    Image1.ImageUrl = "~/Registro/imagen/usuario-registrado.jpg"
    '    Session("imagen") = ""
    '    Session("foto_subido_registro") = "no"
    '    Session("foto_subido") = "no"
    '    FileUpload1.Attributes.Clear()
    'End Sub
#End Region


End Class