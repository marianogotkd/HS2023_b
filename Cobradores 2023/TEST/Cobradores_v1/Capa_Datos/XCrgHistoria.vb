Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class XCrgHistoria
    Inherits Capa_Datos.Conexion



    Public Function XCrgHistoria_consultar(ByVal Fecha As Date, ByVal Cliente As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("XCrgHistoria_consultar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "XCrg")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function




End Class
