﻿Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WC_CtaCte
    Inherits Capa_Datos.Conexion

    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function CtaCte_Alta_vacia(ByVal Grupo_Id As String, ByVal Codigo As String, ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""
        Codigo = "'" + Codigo + "'"
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha

        'IDcarga = "'" + IDcarga + "'"
        Consulta += "declare @SaldoAnterior as decimal(38,2) "
        Consulta += "select @SaldoAnterior = Saldo from Clientes where Clientes.Codigo = " + Codigo
        'Consulta += "select @SaldoAnterior = Saldoanterior from Clientes where Clientes.Codigo = " + Codigo
        Consulta += " insert CtaCte (Grupo_Id, Codigo,Fecha, SaldoAnterior,Recaudacion, Comision,Premios, Reclamos,DejoGano,RecaudacionSC, "
        Consulta += "ComisionSC,PremiosSC,ReclamosSC,DejoGanoSC,RecaudacionB,ComisionB,PremiosB,ReclamosB,DejoGanoB,Cobros, Pagos, "
        Consulta += "Prestamo, CobPrestamo, Credito, CobCredito, Regalos) "
        Consulta += "values (" + Grupo_Id + "," + Codigo + "," + Fecha + ", @SaldoAnterior, '0', '0', '0', '0', '0', '0', '0', '0', '0', '0','0', '0','0', '0','0', '0','0','0', '0', '0', '0', '0') "
        Consulta += "select @@IDENTITY as 'IdCtaCte'"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function





    Public Function CtaCte_alta(ByVal Grupo_Id As Integer, ByVal Cliente_Codigo As Integer, ByVal Fecha As Date, ByVal SaldoAnterior As Decimal,
                                ByVal Recaudacion As Decimal, ByVal Comision As Decimal, ByVal Premios As Decimal, ByVal Reclamos As Decimal, ByVal DejoGano As Decimal,
                                ByVal RecaudacionSC As Decimal, ByVal ComisionSC As Decimal, ByVal PremiosSC As Decimal, ByVal ReclamosSC As Decimal, ByVal DejoGanoSC As Decimal,
                                ByVal RecaudacionB As Decimal, ByVal ComisionB As Decimal, ByVal PremiosB As Decimal, ByVal ReclamosB As Decimal, ByVal DejoGanoB As Decimal,
                                ByVal Cobros As Decimal, ByVal Pagos As Decimal, ByVal Prestamo As Decimal, ByVal Credito As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Id", Grupo_Id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Cliente_Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@SaldoAnterior", SaldoAnterior))

        comando.Parameters.Add(New OleDb.OleDbParameter("@Recaudacion", Recaudacion))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Comision", Comision))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Premios", Premios))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Reclamos", Reclamos))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGano", DejoGano))

        comando.Parameters.Add(New OleDb.OleDbParameter("@RecaudacionSC", RecaudacionSC))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ComisionSC", ComisionSC))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PremiosSC", PremiosSC))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ReclamosSC", ReclamosSC))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGanoSC", DejoGanoSC))

        comando.Parameters.Add(New OleDb.OleDbParameter("@RecaudacionB", RecaudacionB))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ComisionB", ComisionB))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PremiosB", PremiosB))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ReclamosB", ReclamosB))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGanoB", DejoGanoB))

        comando.Parameters.Add(New OleDb.OleDbParameter("@Cobros", Cobros))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Pagos", Pagos))

        comando.Parameters.Add(New OleDb.OleDbParameter("@Prestamo", Prestamo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CobPrestamo", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Credito", Credito))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CobCredito", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Regalos", CDec(0)))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CtaCte_alta_2(ByVal Grupo_Id As Integer, ByVal Cliente_Codigo As Integer, ByVal Fecha As Date, ByVal CobPrestamo As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Id", Grupo_Id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Cliente_Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@SaldoAnterior", CDec(0)))

        comando.Parameters.Add(New OleDb.OleDbParameter("@Recaudacion", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Comision", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Premios", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Reclamos", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGano", CDec(0)))

        comando.Parameters.Add(New OleDb.OleDbParameter("@RecaudacionSC", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ComisionSC", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PremiosSC", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ReclamosSC", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGanoSC", CDec(0)))

        comando.Parameters.Add(New OleDb.OleDbParameter("@RecaudacionB", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ComisionB", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PremiosB", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ReclamosB", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGanoB", CDec(0)))

        comando.Parameters.Add(New OleDb.OleDbParameter("@Cobros", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Pagos", CDec(0)))

        comando.Parameters.Add(New OleDb.OleDbParameter("@Prestamo", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CobPrestamo", CobPrestamo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Credito", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CobCredito", CDec(0)))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Regalos", CDec(0)))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function




    Public Function CtaCte_obtener(ByVal Codigo As Integer, ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim comando As New OleDbCommand("CtaCte_obtener", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function CtaCte_actualizarCobPrestamo(ByVal IdCtaCte As Integer, ByVal CobPrestamo As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim comando As New OleDbCommand("CtaCte_actualizarCobPrestamo", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCtaCte", IdCtaCte))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CobPrestamo", CobPrestamo))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CtaCte_actualizarCobCredito(ByVal IdCtaCte As Integer, ByVal CobCredito As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim comando As New OleDbCommand("CtaCte_actualizarCobCredito", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCtaCte", IdCtaCte))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CobCredito", CobCredito))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CtaCte_ActualizarSalida(ByVal Codigo As Integer, ByVal Fecha As Date, ByVal Salida As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim comando As New OleDbCommand("CtaCte_ActualizarSalida", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Salida", Salida))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function





End Class
