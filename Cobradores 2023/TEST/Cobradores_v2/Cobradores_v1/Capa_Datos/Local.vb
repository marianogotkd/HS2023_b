Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Local
    Inherits Capa_Datos.Conexion

    Public Function ClienteLocal() As DataTable
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim Consulta As String = "Select * from ClienteLocal order by ClienteLocal.CLIE_ID asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "ClienteLocal")
        dbconn.Close()
        Return ds.Tables(0)
        ''''### son las 20:16
    End Function



    Public Function BuscarActivoDesc(ByVal descripcion As String, ByVal PASILLO_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Local_buscaractivodesc", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_desc", descripcion))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_ID", PASILLO_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function BuscarActivoCodigo(ByVal codigolocal As String, ByVal PASILLO_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Local_buscaractivocodigo", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_codigo", codigolocal))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_ID", PASILLO_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Local_alta(ByVal PASILLO_ID As Integer, ByVal LOCAL_desc As String, ByVal LOCAL_codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Local_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_ID", PASILLO_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_desc", LOCAL_desc))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_codigo", LOCAL_codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function consultarsoloactivos() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Local_consultarsoloactivos", dbconn)
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
    'RECUPERA TODA INFO DEL LOCAL Y TAMBIEN LAS TARIFAS ACTIVAS VINCULADAS AL LOCAL.
    Public Function LocalTarifas_recuperarID(ByVal LOCAL_ID As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LocalTarifas_recuperarID", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Local_modificar(ByVal LOCAL_ID As Integer, ByVal LOCAL_desc As String, ByVal LOCAL_codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Local_modificar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_desc", LOCAL_desc))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_codigo", LOCAL_codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function LocalTarifa_baja(ByVal LOCAL_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LocalTarifa_baja", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function LocalClientes_obteneractivos(ByVal PASILLO_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LocalClientes_obteneractivos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@PASILLO_ID", PASILLO_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function ClienteLocal_buscar(ByVal CLIE_ID As Integer, ByVal LOCAL_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("ClienteLocal_buscar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function ClienteLocal_alta(ByVal CLIE_ID As Integer, ByVal LOCAL_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("ClienteLocal_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Local")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function ClienteLocal_buscar2(ByVal CLIE_ID As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = "SELECT * FROM ClienteLocal WHERE CLIE_ID = " + CLIE_ID

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "ClienteLocal")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function ClienteLocal_buscar3(ByVal LOCAL_ID As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = "SELECT * FROM ClienteLocal WHERE LOCAL_ID = " + LOCAL_ID

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "ClienteLocal")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


    Public Function ClienteLocal_eliminarlocal(ByVal CLIE_ID As String, ByVal LOCAL_ID As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = "DELETE FROM ClienteLocal WHERE CLIE_ID = " + CLIE_ID + " AND LOCAL_ID = " + LOCAL_ID

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "ClienteLocal")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

End Class
