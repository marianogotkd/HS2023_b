Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WB_Consultas
    Inherits Capa_Datos.Conexion

    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function CargasClientesDesdeHasta(ByVal Desde As String, ByVal Hasta As String, ByVal Codigos As String, ByVal Fecha_valor As Date) As DataTable
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""


        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "SELECT Pid,Importe,Recorrido_codigo FROM Xcargas"
        Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "
        Consulta += " WHERE Cliente BETWEEN " + Desde + " AND " + Hasta
        Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        Consulta += " AND Fecha = " + Fecha + " ORDER BY XCargas.Importe DESC"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Clientes")
        dbconn.Close()
        Return ds.Tables(0)
        ''''### son las 20:16
    End Function

    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function Cargas_Zona_PID(ByVal Codigos As String, ByVal Pid As String, ByVal Fecha_valor As Date) As DataTable
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "SELECT Pid,Importe,Recorrido_codigo FROM Xcargas"
        Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "
        'Consulta += " WHERE Cliente BETWEEN " + Desde + " AND " + Hasta
        Consulta += " WHERE (Recorrido_Codigo IN (" + Codigos + "))"
        Consulta += "AND Pid = " + "'" + Pid + "'"
        Consulta += " AND Fecha = " + Fecha + " ORDER BY XCargas.Importe DESC"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Clientes")
        dbconn.Close()
        Return ds.Tables(0)
        ''''### son las 20:16
    End Function


    Public Function IngresoTerminales() As DataTable
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        Consulta += "SELECT Terminal, COUNT(*) Registros , CONVERT(TIME, MAX(Hora)) Ultima_Carga FROM XCargas "
        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "
        Consulta += " GROUP BY Terminal"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)

        Dim ds As New DataSet()
        DA.Fill(ds, "Clientes")

        dbconn.Close()
        Return ds.Tables(0)

    End Function


#Region "consulta/modificar xcargas"

    Public Function XCargas_ObtenerNombraTablas() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim comando As New OleDbCommand("XCargas_ObtenerNombraTablas", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))



        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "ConsultaXcargas".
        DA.Fill(ds, "ConsultaXcargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function XCargas_obtenerTabla_todoslosregistros(ByVal Tabla As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "SELECT IDcarga, Recorrido, Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, TotalImporte, CONVERT(Varchar,Fecha, 103) as 'FECHA', CONVERT(VARCHAR,Hora,108)as 'HORA', Verificado, Terminal, Item FROM " + Tabla
        Consulta += " ORDER BY IDcarga ASC"




        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "ConsultaXcargas")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function



    Public Function XCargas_obtenerTabla(ByVal Tabla As String, ByVal Cliente As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "SELECT IDcarga, Recorrido, Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, TotalImporte, CONVERT(Varchar,Fecha, 103) as 'FECHA', CONVERT(VARCHAR,Hora,108)as 'HORA', Verificado, Terminal, Item FROM " + Tabla
        Consulta += " WHERE Cliente = " + Cliente + " ORDER BY IDcarga ASC"




        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "ConsultaXcargas")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function



    Public Function XCargas_modificarTabla(ByVal Tabla As String, ByVal IDcarga As Integer, ByVal Recorrido As String, ByVal Pid As String, ByVal Importe As String, ByVal Suc As String, ByVal SinComputo As String, ByVal TotalImporte As String, ByVal Pid2 As String, ByVal Suc2 As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        Recorrido = "'" + Recorrido + "'"

        Pid = "'" + Pid + "'"
        Importe = "'" + CStr(Importe.Replace(",", ".")) + "'"
        TotalImporte = "'" + CStr(TotalImporte.Replace(",", ".")) + "'"
        Suc = "'" + Suc + "'"
        SinComputo = "'" + SinComputo + "'"


        Pid2 = "'" + Pid2 + "'"
        Dim Suc2_valor
        If Suc2 = "" Then
            Suc2_valor = "null"

        Else
            Suc2_valor = Suc2
        End If

        Consulta += "update " + Tabla + " set Recorrido = " + Recorrido + ", Pid = " + Pid + ", Importe = " + Importe + ", TotalImporte = " + TotalImporte + ", Suc = " + Suc + ", SinComputo = " + SinComputo + ", Pid2 = " + CStr(Pid2) + ", Suc2 = " + Suc2_valor + " where IDcarga = " + CStr(IDcarga)

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "ConsultaXcargas")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function
    'Public Function XCargas_modificarTabla_pid2(ByVal Tabla As String, ByVal IDcarga As Integer, ByVal Pid2 As String) As DataSet
    '    Try
    '        dbconn.Open()
    '    Catch ex As Exception
    '    End Try

    '    'Dim Pid2_valor
    '    'If Pid2 = "" Then
    '    '    Pid2_valor = DBNull.Value
    '    'Else
    '    '    Pid2_valor = Pid2
    '    'End If

    '    Dim Consulta As String = ""
    '    Pid2 = "'" + Pid2 + "'"

    '    Consulta += "update " + Tabla + " set Pid2 = " + CStr(Pid2) + " where IDcarga = " + CStr(IDcarga)

    '    Dim DA As New OleDbDataAdapter(Consulta, dbconn)
    '    Dim ds As New DataSet()
    '    DA.Fill(ds, "ConsultaXcargas")
    '    dbconn.Close()
    '    Return ds
    '    ''''### son las 20:16
    'End Function

    'Public Function XCargas_modificarTabla_suc2(ByVal Tabla As String, ByVal IDcarga As Integer, ByVal Suc2 As String) As DataSet
    '    Try
    '        dbconn.Open()
    '    Catch ex As Exception
    '    End Try

    '    Dim Suc2_valor
    '    If Suc2 = "" Then
    '        Suc2_valor = "null"

    '    Else
    '        Suc2_valor = Suc2
    '    End If

    '    Dim Consulta As String = ""

    '    Consulta += "update " + Tabla + " set Suc2 = " + Suc2_valor + " where IDcarga = " + CStr(IDcarga)

    '    Dim DA As New OleDbDataAdapter(Consulta, dbconn)
    '    Dim ds As New DataSet()
    '    DA.Fill(ds, "ConsultaXcargas")
    '    dbconn.Close()
    '    Return ds
    '    ''''### son las 20:16

    'End Function




#End Region

End Class
