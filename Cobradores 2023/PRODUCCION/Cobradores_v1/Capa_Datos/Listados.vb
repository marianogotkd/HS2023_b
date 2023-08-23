Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Listados
    Inherits Capa_Datos.Conexion

    Public Function SaldosyRegalos_obtener(ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("SaldosyRegalos_obtener", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)

        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Listado_ClientesGanan(ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Listado_ClientesGanan", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)

        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function



    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function EntradaSalida_BuscarRangoFechasUno(ByVal Grupo_codigo As String, ByVal FechaDesde As Date, ByVal FechaHasta As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        Dim CFechaDesde As String = "'" + FechaDesde.Year.ToString + "-" + FechaDesde.Month.ToString + "-" + FechaDesde.Day.ToString + "'" 'le agrego comillas a la fecha
        'FechaHasta = "'" + FechaHasta.ToString + "'" 'le agrego comillas a la fecha
        Dim CFechaHasta As String = "'" + FechaHasta.Year.ToString + "-" + FechaHasta.Month.ToString + "-" + FechaHasta.Day.ToString + "'" 'le agrego comillas a la fecha

        Grupo_codigo = "'" + Grupo_codigo + "'"

        Consulta += "declare @Grupo_id int "
        Consulta += "select @Grupo_id = Grupo_id from Grupos where Grupos.Codigo = " + Grupo_codigo
        Consulta += "SELECT CtaCte.Grupo_Id AS 'Grupo_id',Grupos.Codigo as 'Grupo_codigo',Grupos.Nombre  as 'Grupo_nombre',Clientes.Codigo as 'Cliente_codigo', "
        Consulta += "Clientes.Cliente as 'Cliente_id',CtaCte.Cobros,CtaCte.Salida, CtaCte.Fecha as 'CtaCte_Fecha' "
        Consulta += "FROM CtaCte INNER JOIN Grupos ON CtaCte.Grupo_Id = Grupos.Grupo_id INNER JOIN Clientes ON CtaCte.Codigo = Clientes.Codigo WHERE (CtaCte.Grupo_Id = @Grupo_id) AND "
        Consulta += "(CtaCte.Fecha BETWEEN " + CFechaDesde + "and " + CFechaHasta + ")"
        Consulta += " order by convert(int, Grupos.Codigo), convert(int,Clientes.Codigo) asc "
        Consulta += "select Gastos.Grupo_id, Grupos.Codigo, Grupos.Nombre as 'Grupo_nombre', GastosTipo.Motivo , Gastos.Importe, Gastos.Fecha as 'Gastos_Fecha' from Gastos inner join GastosTipo on Gastos.Gastotipo_id = GastosTipo.Gastotipo_id "
        Consulta += "inner join Grupos on Gastos.Grupo_id = Grupos.Grupo_id where (Gastos.Grupo_id = @Grupo_id) AND "
        Consulta += "(Gastos.Fecha BETWEEN " + CFechaDesde + "and " + CFechaHasta + ") AND Gastos.Eliminado = 0 "
        Consulta += " order by convert(int,Grupos.Codigo), GastosTipo.Motivo asc"


        'Consulta += "PrestamosCreditos.Importe,PrestamosCreditos.Saldo,Clientes.Codigo As 'Cliente_Codigo', Clientes.Cliente as 'Clie_ID', Clientes.Grupo_id "
        'Consulta += "from PrestamosCreditos inner join Clientes on PrestamosCreditos.Cliente = Clientes.Cliente "
        'Consulta += "where PrestamosCreditos.Tipo = 'P' and PrestamosCreditos.Tipocobro = '2' and PrestamosCreditos.Estado_id = '1' and PrestamosCreditos.Fecha = " + Fecha


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "EntradasSalidas")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function EntradaSalida_BuscarRangoFechasTodo(ByVal FechaDesde As Date, ByVal FechaHasta As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        Dim CFechaDesde As String = "'" + FechaDesde.Year.ToString + "-" + FechaDesde.Month.ToString + "-" + FechaDesde.Day.ToString + "'" 'le agrego comillas a la fecha
        'FechaHasta = "'" + FechaHasta.ToString + "'" 'le agrego comillas a la fecha
        Dim CFechaHasta As String = "'" + FechaHasta.Year.ToString + "-" + FechaHasta.Month.ToString + "-" + FechaHasta.Day.ToString + "'" 'le agrego comillas a la fecha

        'Grupo_codigo = "'" + Grupo_codigo + "'"


        Consulta += "SELECT CtaCte.Grupo_Id AS 'Grupo_id',Grupos.Codigo as 'Grupo_codigo',Grupos.Nombre  as 'Grupo_nombre',Clientes.Codigo as 'Cliente_codigo', "
        Consulta += "Clientes.Cliente as 'Cliente_id',CtaCte.Cobros,CtaCte.Salida, CtaCte.Fecha as 'CtaCte_Fecha' "
        Consulta += "FROM CtaCte INNER JOIN Grupos ON CtaCte.Grupo_Id = Grupos.Grupo_id INNER JOIN Clientes ON CtaCte.Codigo = Clientes.Codigo WHERE "
        Consulta += "(CtaCte.Fecha BETWEEN " + CFechaDesde + "and " + CFechaHasta + ")"
        Consulta += " order by convert(int, Grupos.Codigo), convert(int,Clientes.Codigo) asc "
        Consulta += "select Gastos.Grupo_id, Grupos.Codigo, Grupos.Nombre as 'Grupo_nombre', GastosTipo.Motivo , Gastos.Importe, Gastos.Fecha as 'Gastos_Fecha' from Gastos inner join GastosTipo on Gastos.Gastotipo_id = GastosTipo.Gastotipo_id "
        Consulta += "inner join Grupos on Gastos.Grupo_id = Grupos.Grupo_id where "
        Consulta += "(Gastos.Fecha BETWEEN " + CFechaDesde + "and " + CFechaHasta + ") AND Gastos.Eliminado = 0 "
        Consulta += " order by convert(int,Grupos.Codigo), GastosTipo.Motivo asc"


        'Consulta += "PrestamosCreditos.Importe,PrestamosCreditos.Saldo,Clientes.Codigo As 'Cliente_Codigo', Clientes.Cliente as 'Clie_ID', Clientes.Grupo_id "
        'Consulta += "from PrestamosCreditos inner join Clientes on PrestamosCreditos.Cliente = Clientes.Cliente "
        'Consulta += "where PrestamosCreditos.Tipo = 'P' and PrestamosCreditos.Tipocobro = '2' and PrestamosCreditos.Estado_id = '1' and PrestamosCreditos.Fecha = " + Fecha


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "EntradasSalidas")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function




End Class
