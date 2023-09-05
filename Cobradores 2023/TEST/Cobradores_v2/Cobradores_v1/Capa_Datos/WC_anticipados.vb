Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WC_anticipados
    Inherits Capa_Datos.Conexion

    Public Function Anticipados_alta(ByVal Fecha As Date,
                                ByVal Cliente As Integer,
                                ByVal AnticipadosTipo_id As Integer,
                                ByVal Importe As Decimal,
                                ByVal Sincalculo As Integer,
                                ByVal Origen As Integer,
                                ByVal Descripcion As String,
                                ByVal FechaOrigen As Date,
                                ByVal Eliminado As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Anticipados_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@AnticipadosTipo_id", AnticipadosTipo_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe", Importe))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Sincalculo", Sincalculo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Origen", Origen))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Descripcion", Descripcion))
        comando.Parameters.Add(New OleDb.OleDbParameter("@FechaOrigen", FechaOrigen))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Eliminado", Eliminado))
        
        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Anticipados")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Anticipados_reclamo_alta(ByVal Fecha As Date,
                                ByVal Cliente As Integer,
                                ByVal AnticipadosTipo_id As Integer,
                                ByVal Importe As Decimal,
                                ByVal Sincalculo As Integer,
                                ByVal Origen As Integer,
                                ByVal Descripcion As String,
                                ByVal Eliminado As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Anticipados_reclamo_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@AnticipadosTipo_id", AnticipadosTipo_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe", Importe))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Sincalculo", Sincalculo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Origen", Origen))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Descripcion", Descripcion))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@FechaOrigen", FechaOrigen))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Eliminado", Eliminado))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Anticipados")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Anticipados_pago_alta(ByVal Fecha As Date,
                                ByVal Cliente As Integer,
                                ByVal AnticipadosTipo_id As Integer,
                                ByVal Importe As Decimal,
                                ByVal Eliminado As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Anticipados_pago_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@AnticipadosTipo_id", AnticipadosTipo_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe", Importe))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Eliminado", Eliminado))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Anticipados")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Anticipados_cobro_alta(ByVal Fecha As Date,
                                ByVal Cliente As Integer,
                                ByVal AnticipadosTipo_id As Integer,
                                ByVal Importe As Decimal,
                                ByVal Eliminado As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Anticipados_cobro_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@AnticipadosTipo_id", AnticipadosTipo_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe", Importe))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Eliminado", Eliminado))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Anticipados")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Anticipados_resumen(ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Anticipados_resumen", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Anticipados")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Anticipados_eliminar(ByVal Anticipados_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Anticipados_eliminar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Anticipados_id", Anticipados_id))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Anticipados")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Anticipados_ClienteobtenerXfecha(ByVal Fecha As Date, ByVal Cliente_Codigo As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Anticipados_ClienteobtenerXfecha", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente_Codigo", Cliente_Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Anticipados".
        DA.Fill(ds, "Anticipados")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function Anticipados_obtenerTodoxFecha(ByVal Fecha_valor As Date) As DataTable
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "SELECT Anticipados_id, Fecha, Anticipados.Cliente, AnticipadosTipo_id, Importe, Anticipados.Sincalculo, Origen, Descripcion, FechaOrigen, Eliminado, "
        Consulta += "Clientes.Codigo AS 'Codigo', Clientes.Grupo_id, Clientes.Saldo, Clientes.Regalo, Clientes.SaldoRegalo FROM Anticipados inner join Clientes on Anticipados.Cliente = Clientes.Cliente "
        Consulta += " WHERE Fecha = " + Fecha + " order by CONVERT(INT,Clientes.Codigo) ASC"

        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Clientes")
        dbconn.Close()
        Return ds.Tables(0)
        ''''### son las 20:16
    End Function




    Public Function Anticipados_ClienteobtenerXfecha_reclamos(ByVal Fecha As Date, ByVal Cliente_Codigo As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Anticipados_ClienteobtenerXfecha_reclamos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente_Codigo", Cliente_Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Anticipados".
        DA.Fill(ds, "Anticipados")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function




End Class
