Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Sector
    Inherits Capa_Datos.Conexion

    Public Function ObtenerTodosActivos() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Sector_obtenertodosactivos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Usuario", Usuario))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Contraseña", Contraseña))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Sector")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function consultarsoloactivos() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Sector_consultarsoloactivos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Usuario", Usuario))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Contraseña", Contraseña))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Sector")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function BuscarActivoDesc(ByVal descripcion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Sector_buscaractivodesc", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_desc", descripcion))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Contraseña", Contraseña))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Sector")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Sector_alta(ByVal descripcion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Sector_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_desc", descripcion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Sector")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Sector_modificar(ByVal SECTOR_ID As Integer, ByVal SECTOR_desc As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Sector_modificar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_ID", SECTOR_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_desc", SECTOR_desc))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Sector")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    'recupera info del sector y todos los pasillos vinculados. activos.
    Public Function SectorPasillos_recuperarID(ByVal SECTOR_ID As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("SectorPasillos_recuperarID", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_ID", SECTOR_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Sector")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function SectorPasillos_baja(ByVal SECTOR_ID As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("SectorPasillos_baja", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_ID", SECTOR_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Sector")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Sector_cobradores_obteneractivos() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Sector_cobradores_obteneractivos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@SECTOR_ID", SECTOR_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Sector")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

End Class
