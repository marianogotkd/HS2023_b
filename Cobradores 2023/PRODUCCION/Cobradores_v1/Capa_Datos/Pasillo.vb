Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Pasillo
    Inherits Capa_Datos.Conexion

    Public Function Pasillo_alta(ByVal SECTOR_ID As Integer, ByVal PASILLO_desc As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Pasillo_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_ID", SECTOR_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_desc", PASILLO_desc))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "PASILLO")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Pasillo_modificar(ByVal PASILLO_ID As Integer, ByVal PASILLO_desc As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Pasillo_modificar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_ID", PASILLO_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_desc", PASILLO_desc))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "PASILLO")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Pasillo_EstadoModificar(ByVal PASILLO_ID As Integer, ByVal PASILLO_estado As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Pasillo_EstadoModificar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_ID", PASILLO_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_estado", PASILLO_estado))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "PASILLO")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Pasillo_cobradores_obteneractivos(ByVal SECTOR_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Pasillo_cobradores_obteneractivos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_ID", SECTOR_ID))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_estado", PASILLO_estado))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "PASILLO")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

End Class
