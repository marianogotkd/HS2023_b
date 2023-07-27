Imports System.Data.OleDb
Imports System.Data.DataRow
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



    End Sub


    Dim tamanio As Integer
    Dim ImagenOriginal As Byte()
    Dim ImagenOriginalBinaria As Bitmap
    Dim ImagenDataURL64 As String


    Private Sub Button_adjuntar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_adjuntar.Click

        If FileUpload1.HasFile Then
            Dim fileExt As String = System.IO.Path.GetExtension(FileUpload1.FileName)
            If fileExt = ".jpeg" Or fileExt = ".bmp" Or fileExt = ".png" Or fileExt = ".jpg" Then



                tamanio = FileUpload1.PostedFile.ContentLength
                'int Tamanio = fuploadImagen.PostedFile.ContentLength;
                'choco
                ImagenOriginal = New Byte(tamanio - 1) {}
                'byte[] ImagenOriginal = new byte[Tamanio];
                'choco
                FileUpload1.PostedFile.InputStream.Read(ImagenOriginal, 0, tamanio)
                'fuploadImagen.PostedFile.InputStream.Read(ImagenOriginal, 0, Tamanio);
                'choco
                ImagenOriginalBinaria = New Bitmap(FileUpload1.PostedFile.InputStream)
                'Bitmap ImagenOriginalBinaria = new Bitmap(fuploadImagen.PostedFile.InputStream);
                'choco
                ImagenDataURL64 = "data:image/jpg;base64," + Convert.ToBase64String(ImagenOriginal)
                'string ImagenDataURL64 = "data:image/jpg;base64." + Convert.ToBase64String(ImagenOriginal);

                Session("imagen") = ImagenOriginal
                Image1.ImageUrl = ImagenDataURL64

                Image1.Visible = True
                'lbl_errImg.Visible = False
                'btn_Examinar.Visible = False
                'btn_quitar.Visible = True
            Else
                'lbl_errImg.Visible = True
                'lbl_errImg.InnerText = "Solo Archivos de Tipo Imagen"
            End If

        End If

    End Sub

    Public Function ImageControlToByteArray(ByVal foto)
        Return File.ReadAllBytes(Server.MapPath(foto.ImageUrl))
    End Function

    Dim foto_cargada As String
    Private Sub Btn_aceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_aceptar.Click
        Try
            foto_cargada = "no"
            If FileUpload1.HasFile Then
                Dim fileExt As String = System.IO.Path.GetExtension(FileUpload1.FileName)
                If fileExt = ".jpeg" Or fileExt = ".bmp" Or fileExt = ".png" Or fileExt = ".jpg" Then

                    tamanio = FileUpload1.PostedFile.ContentLength
                    'int Tamanio = fuploadImagen.PostedFile.ContentLength;
                    'choco
                    ImagenOriginal = New Byte(tamanio - 1) {}
                    'byte[] ImagenOriginal = new byte[Tamanio];
                    'choco
                    FileUpload1.PostedFile.InputStream.Read(ImagenOriginal, 0, tamanio)
                    'fuploadImagen.PostedFile.InputStream.Read(ImagenOriginal, 0, Tamanio);
                    'choco
                    ImagenOriginalBinaria = New Bitmap(FileUpload1.PostedFile.InputStream)
                    'Bitmap ImagenOriginalBinaria = new Bitmap(fuploadImagen.PostedFile.InputStream);
                    'choco
                    ImagenDataURL64 = "data:image/jpg;base64," + Convert.ToBase64String(ImagenOriginal)
                    'string ImagenDataURL64 = "data:image/jpg;base64." + Convert.ToBase64String(ImagenOriginal);
                    Session("imagen") = ImagenOriginal
                    Session("imagen64") = ImagenDataURL64
                    Image1.ImageUrl = ImagenDataURL64
                    'Image1.Visible = True
                    'lbl_errImg.Visible = False
                    'btn_Examinar.Visible = False
                    'btn_quitar.Visible = True
                    foto_cargada = "si"

                    Session("foto_subido_registro") = "si" 'choco: 23-07-2019
                    Session("imagen_registro") = ImagenOriginal 'choco: 23-07-2019
                    Response.Redirect("Datos_Personales.aspx")
                Else


                End If

            End If
        Catch ex As Exception

        End Try


    End Sub




End Class