Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WC_recorridos_zonas
    Inherits Capa_Datos.Conexion

    Public Function recorridos_zonas_consultar_dia(ByVal Dia_id As Integer) As DataSet
        'trae todos los recorridos/zonas de 1 dia puntual.
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("recorridos_zonas_consultar_dia", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Dia_id", Dia_id))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)

        DA.Fill(ds, "Recorridos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function recorridos_zonas_activacion(ByVal Dia_id As Integer, ByVal Codigo As String, ByVal Habilitada As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim habi As Integer

        Try
            habi = CInt(Habilitada)
        Catch ex As Exception
            habi = 0
        End Try
        Dim comando As New OleDbCommand("recorridos_zonas_activacion", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Dia_id", Dia_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Habilitada", habi))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "recorridos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

#Region "CARGA DE RECORRIDOS Y ZONAS"
    Public Function recorridos_zonas_obtener_habilitados_x_dia(ByVal Dia_id As Integer) As DataSet
        'trae todos los recorridos/zonas de 1 dia puntual.
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("recorridos_zonas_obtener_habilitados_x_dia", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Dia_id", Dia_id))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)

        DA.Fill(ds, "Recorridos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function recorridos_zonas_buscar_codigo(ByVal Codigo As String, ByVal Dia_id As Integer) As DataSet
        'trae todos los recorridos/zonas de 1 dia puntual.
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("recorridos_zonas_buscar_codigo", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Dia_id", Dia_id))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)

        DA.Fill(ds, "Recorridos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function
    Public Function recorridos_zonas_obtener_info_zona(ByVal Idrecorrido As Integer, ByVal Fecha As Date) As DataSet
        'trae todos los recorridos/zonas de 1 dia puntual.
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("recorridos_zonas_obtener_info_zona", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Idrecorrido", Idrecorrido))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)

        DA.Fill(ds, "Recorridos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


#End Region


#Region "PRUEBAS NUEVAS EN LIQUIDACION PARA MEJORAR TIEMPOS"
    Public Function recorridos_zonas_ObtenerUnRecorrido_x_dia(ByVal Dia_id As String, ByVal Fecha_valor As Date, ByVal CodigoRec As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dia_id = "'" + Dia_id + "'"
        CodigoRec = "'" + CodigoRec + "'"

        Dim Consulta As String = ""

        Consulta += "Select * From Recorridos inner Join Puntos On Recorridos.Idrecorrido = Puntos.Idrecorrido Where (Recorridos.Dia_id = " + Dia_id + " ) "
        Consulta += "AND (Recorridos.Habilitada = 1) and (Puntos.Fecha = " + Fecha + " ) and (Recorridos.Codigo = " + CodigoRec + " ) order by Recorridos.Codigo asc"
        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Recorrido")
        dbconn.Close()
        Return ds
        ''''### son las 20:16

    End Function



    'Select Case* From Recorridos inner Join Puntos On Recorridos.Idrecorrido = Puntos.Idrecorrido Where Recorridos.Dia_id = '7' and Recorridos.Habilitada = 1 order by Recorridos.Codigo asc
    Public Function recorridos_zonas_ObtenerTodoHabilitados_x_dia(ByVal Dia_id As String, ByVal Fecha As Date) As DataSet

        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("recorridos_zonas_ObtenerTodoHabilitados_x_dia", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Dia_id", Dia_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "XCargas".
        DA.Fill(ds, "Recorridos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds

    End Function

    Public Function Liquidacion_recuperarXcargas_FiltroOP1(ByVal Codigo As String, ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_recuperarXcargas_FiltroOP1", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "XCargas".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Liquidacion_recuperarXcargas_FiltroOP2(ByVal Codigo As String, ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_recuperarXcargas_FiltroOP2", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "XCargas".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Liquidacion_recuperarXcargas_FiltroOP3(ByVal Codigo As String, ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_recuperarXcargas_FiltroOP3", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "XCargas".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Liquidacion_recuperarXcargas_FiltroOP4(ByVal Codigo As String, ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_recuperarXcargas_FiltroOP4", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "XCargas".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function XCargasJunto_obtenerclientes(ByVal Fecha_valor As Date) As DataSet 'recupera un listado todos los clientes en xcargas.
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dim Consulta As String = ""

        Consulta += "SELECT Cliente FROM XCargasJunto where Fecha = " + Fecha + " GROUP BY Cliente ORDER BY Cliente ASC"
        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargasJunto")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function XCargasJunto_infoclienteyrecaudacion(ByVal Fecha_valor As Date, ByVal Codigo As String) As DataSet 'recupera un listado todos los clientes en xcargas.
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        Codigo = "'" + Codigo + "'"
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dim Consulta As String = ""

        Consulta += "SELECT Clientes.Codigo, Clientes.Grupo_id,Clientes.Saldo as 'Cliente_Saldo',Clientes.Comision as 'Cliente_Comision',Clientes.Regalo as 'Cliente_Regalo',Clientes.SaldoRegalo as 'Cliente_SaldoRegalo' "
        Consulta += "FROM Clientes inner join Grupos on Clientes.Grupo_id=Grupos.Grupo_id  WHERE Clientes.Codigo = " + Codigo
        Consulta += "select Cliente, SUM(TotalImporte) as 'TotalImporte'  from XCargasJunto where Fecha = " + Fecha + " AND SinComputo = '0' AND Cliente = " + Codigo + " AND (Recorrido <> 'Z') AND ((Recorrido <> 'z')) GROUP BY Cliente "
        Consulta += "select Cliente, SUM(TotalImporte) as 'TotalImporte'  from XCargasJunto where Fecha = " + Fecha + " AND SinComputo = '1' AND Cliente = " + Codigo + " AND (Recorrido <> 'Z') AND ((Recorrido <> 'z')) GROUP BY Cliente "

        'Consulta += "SELECT Cliente FROM XCargasJunto where Fecha = " + Fecha + " GROUP BY Cliente ORDER BY Cliente ASC"
        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargasJunto")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function




#End Region
End Class
