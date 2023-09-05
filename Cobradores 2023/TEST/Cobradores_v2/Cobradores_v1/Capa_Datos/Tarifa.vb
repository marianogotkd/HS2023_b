Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Tarifa
    Inherits Capa_Datos.Conexion

    Public Function Tarifa_alta(ByVal LOCAL_ID As Integer,
                                ByVal TARIFA_precio As Decimal,
                                ByVal TARIFA_tipo As String,
                                ByVal TARIFA_desc As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Tarifa_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARIFA_precio", TARIFA_precio))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARIFA_tipo", TARIFA_tipo))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@TARIFA_dias", TARIFA_dias))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARIFA_desc", TARIFA_desc))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Tarifa")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Tarifa_modificar(ByVal TARIFA_ID As String, ByVal TARIFA_precio As String) As DataTable
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        TARIFA_ID = "'" + TARIFA_ID + "'"
        TARIFA_precio = "'" + TARIFA_precio + "'"
        Dim Consulta As String = "update Tarifa set TARIFA_precio = " + TARIFA_precio + " where Tarifa.TARIFA_ID = " + TARIFA_ID
        Consulta = Consulta + "  SELECT * FROM TARIFA WHERE TARIFA_ID = " + TARIFA_ID

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Tarifa")
        dbconn.Close()
        Return ds.Tables(0)
        ''''### son las 20:16
    End Function


    Public Function Tarifa_EstadoModificar(ByVal TARIFA_ID As Integer, ByVal TARIFA_estado As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Tarifa_EstadoModificar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARIFA_ID", TARIFA_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARIFA_estado", TARIFA_estado))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Tarifa")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Tarifa_recuperartodolocal(ByVal LOCAL_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Tarifa_recuperartodolocal", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Tarifa")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function TarifaCliente_alta(ByVal TARIFA_ID As Integer,
                                       ByVal CLIE_ID As Integer,
                                       ByVal TARCLIE_precio As Decimal,
                                       ByVal TARCLIE_deuda As Decimal,
                                       ByVal TARCLIE_tipo As String,
                                       ByVal TARCLIE_desc As String,
                                       ByVal TARCLIE_fechainicio As Date) As DataSet 'ByVal TARCLIE_dias As Integer,
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("TarifaCliente_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARIFA_ID", TARIFA_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_precio", TARCLIE_precio))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_deuda", TARCLIE_deuda))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_tipo", TARCLIE_tipo))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_dias", TARCLIE_dias))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_desc", TARCLIE_desc))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_fechainicio", TARCLIE_fechainicio))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "TarifaCliente")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function TarifaCliente_modificar(ByVal TARIFA_ID As String, ByVal TARCLIE_precio As String, ByVal TARCLIE_deuda As String) As DataTable
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        TARIFA_ID = "'" + TARIFA_ID + "'"
        TARCLIE_precio = "'" + TARCLIE_precio + "'"
        TARCLIE_deuda = "'" + TARCLIE_deuda + "'"
        Dim Consulta As String = "update TarifaCliente set TARCLIE_precio = " + TARCLIE_precio + "," + " TARCLIE_deuda = " + TARCLIE_deuda + " where TARIFA_ID = " + TARIFA_ID
        Consulta = Consulta + "  SELECT * FROM TarifaCliente WHERE TARIFA_ID = " + TARIFA_ID

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Tarifa")
        dbconn.Close()
        Return ds.Tables(0)
        ''''### son las 20:16
    End Function



    Public Function TarifaClienteDia_alta(ByVal TARCLIE_ID As Integer,
                                       ByVal TARCLIEDIA_desc As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("TarifaClienteDia_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARIFA_ID", TARCLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIEDIA_desc", TARCLIEDIA_desc))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "TarifaClienteDia")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function TarifaCliente_EstadoModificar(ByVal TARCLIE_ID As Integer,
                                       ByVal TARCLIE_estado As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("TarifaCliente_EstadoModificar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_ID", TARCLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_estado", TARCLIE_estado))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "TarifaCliente")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function TarifaCliente_obtener(ByVal CLIE_ID As Integer,
                                       ByVal LOCAL_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("TarifaCliente_obtener", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Tarifas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


#Region "ADMIN PROC. DIARIO"
    Public Function TarifaCliente_estadomodif(ByVal TARCLIE_ID As Integer, ByVal TARCLIE_estado As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("TarifaCliente_estadomodif", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_ID", TARCLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_estado", TARCLIE_estado))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "TarifaCliente")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function



    Public Function TarifaCliente_procdiario001(ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("TarifaCliente_procdiario001", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "TarifaCliente")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function TarifaCliente_procdiario002(ByVal TARCLIE_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("TarifaCliente_procdiario002", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_ID", TARCLIE_ID))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "TarifaClienteDia")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function




#End Region






End Class
