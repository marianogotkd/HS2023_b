Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Parametro
    Inherits Capa_Datos.Conexion

    Public Function Parametro_obtenerultimoproc() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_obtenerultimoproc", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_desc", descripcion))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_ID", PASILLO_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Parametro_alta(ByVal PAR_fecha As Date, ByVal PAR_obs As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@PAR_fecha", PAR_fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PAR_obs", PAR_obs))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

End Class
