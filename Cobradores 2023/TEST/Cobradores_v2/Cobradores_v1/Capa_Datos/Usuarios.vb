Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Usuarios
    Inherits Capa_Datos.Conexion
    ''''################aquiiiiiiiiiiiiiii 18:34
    Public Function Usuarios_ObtenerUsuario(ByVal Usuario As String, ByVal Contraseña As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Usuarios_ObtenerUsuario", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Usuario", Usuario))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Contraseña", Contraseña))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Usuarios")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Usuarios_buscarID(ByVal IDusuario As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Usuarios_buscarID", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Idusuario", IDusuario))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Usuarios".
        DA.Fill(ds, "Usuarios")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Usuarios_obtenertodos() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Usuarios_obtenertodos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Idusuario", IDusuario))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Usuarios".
        DA.Fill(ds, "Usuarios")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Usuarios_alta(ByVal USU_usuario As String,
                                ByVal USU_contraseña As String,
                                ByVal USU_jerarquia As String,
                                ByVal USU_ape As String,
                                ByVal USU_nom As String,
                                ByVal USU_direccion As String,
                                ByVal USU_dni As String,
                                ByVal USU_telefono As String,
                                ByVal USU_mail As String,
                                ByVal USU_obs As String,
                                 ByVal USU_estado As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Usuarios_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_usuario", USU_usuario))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_contraseña", USU_contraseña))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_jerarquia", USU_jerarquia))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_ape", USU_ape))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_nom", USU_nom))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_direccion", USU_direccion))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_dni", USU_dni))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_telefono", USU_telefono))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_mail", USU_mail))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_obs", USU_obs))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_estado", USU_estado))
        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Usuario_Actualizar(ByVal USU_usuario As String,
                               ByVal USU_contraseña As String,
                               ByVal USU_jerarquia As String,
                               ByVal USU_ape As String,
                               ByVal USU_nom As String,
                               ByVal USU_direccion As String,
                               ByVal USU_dni As String,
                               ByVal USU_telefono As String,
                               ByVal USU_mail As String,
                               ByVal USU_obs As String,
                                ByVal USU_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Usuario_Actualizar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_usuario", USU_usuario))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_contraseña", USU_contraseña))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_jerarquia", USU_jerarquia))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_ape", USU_ape))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_nom", USU_nom))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_direccion", USU_direccion))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_dni", USU_dni))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_telefono", USU_telefono))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_mail", USU_mail))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_obs", USU_obs))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_ID", USU_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Usuario_Eliminar(ByVal USU_estado As String,
                                    ByVal USU_ID As Integer
                                     ) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Usuario_Eliminar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_estado", USU_estado))
        comando.Parameters.Add(New OleDb.OleDbParameter("@USU_ID", USU_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Usuarios".
        DA.Fill(ds, "Usuarios")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function



End Class
