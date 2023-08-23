Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Clientes
    Inherits Capa_Datos.Conexion

    Public Function Clientes_alta(ByVal CLIE_dni As String, ByVal CLIE_ape As String, ByVal CLIE_nom As String,
                                  ByVal CLIE_direccion As String, ByVal CLIE_telefono As String,
                                  ByVal CLIE_mail As String, ByVal CLIE_obs As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Clientes_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_dni", CLIE_dni))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ape", CLIE_ape))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_nom", CLIE_nom))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_direccion", CLIE_direccion))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_telefono", CLIE_telefono))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_mail", CLIE_mail))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_obs", CLIE_obs))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Clientes_modificar(ByVal CLIE_ID As Integer, ByVal CLIE_dni As String, ByVal CLIE_ape As String, ByVal CLIE_nom As String,
                                  ByVal CLIE_direccion As String, ByVal CLIE_telefono As String,
                                  ByVal CLIE_mail As String, ByVal CLIE_obs As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Clientes_modificar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ape", CLIE_ape))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_nom", CLIE_nom))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_dni", CLIE_dni))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_direccion", CLIE_direccion))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_telefono", CLIE_telefono))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_mail", CLIE_mail))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_obs", CLIE_obs))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Clientes_buscaractivodni(ByVal CLIE_dni As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Clientes_buscaractivodni", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_dni", CLIE_dni))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Cliente_consultarsoloactivos() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Cliente_consultarsoloactivos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_dni", CLIE_dni))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Cliente_recuperarID(ByVal CLIE_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Cliente_recuperarID", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function ClienteLocalTarifas_baja(ByVal CLIE_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("ClienteLocalTarifas_baja", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function



End Class
