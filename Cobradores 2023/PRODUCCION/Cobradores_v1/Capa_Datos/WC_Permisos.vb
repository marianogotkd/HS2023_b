Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WC_Permisos
    Inherits Capa_Datos.Conexion

    Public Function Permisos_buscar(ByVal Idusuario As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Permisos_buscar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Idusuario", Idusuario))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Permisos".
        DA.Fill(ds, "Permisos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

End Class
