Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WC_Web
    Inherits Capa_Datos.Conexion

    Public Function WebDeshabilitar() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("WebDeshabilitar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Idusuario", Idusuario))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Permisos".
        DA.Fill(ds, "Web")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function WebHabilitar() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("WebHabilitar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Idusuario", Idusuario))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Permisos".
        DA.Fill(ds, "Web")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function





End Class
