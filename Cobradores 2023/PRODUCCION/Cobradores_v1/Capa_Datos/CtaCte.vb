Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class CtaCte
    Inherits Capa_Datos.Conexion

    Public Function CtaCte_alta(ByVal CLIE_ID As Integer, ByVal CTACTE_fechaapertura As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTE_fechaapertura", CTACTE_fechaapertura))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CtaCteDetalle_alta(ByVal CTACTEDET_fecha As Date,
                                       ByVal CTACTE_ID As Integer,
                                       ByVal CTACTEDET_importe As Decimal,
                                       ByVal CTACTEDET_tipo As String,
                                       ByVal CTACTEDET_comprobante As String,
                                       ByVal TARCLIE_ID As Integer,
                                       ByVal CTACTEDET_saldodeudor As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCteDetalle_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTEDET_fecha", CTACTEDET_fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTE_ID", CTACTE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTEDET_importe", CTACTEDET_importe))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTEDET_tipo", CTACTEDET_tipo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTEDET_comprobante", CTACTEDET_comprobante))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_ID", TARCLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTEDET_saldodeudor", CTACTEDET_saldodeudor))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCteDetalle")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CtaCte_obtenerID(ByVal CTACTE_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_obtenerID", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTE_ID", CTACTE_ID))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function CtaCte_SaldodeudorModif(ByVal CTACTE_ID As Integer, ByVal saldodeudor As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_SaldodeudorModif", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTE_ID", CTACTE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@saldodeudor", saldodeudor))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function CtaCteDetalle_obtenerxTARCLIE_ID(ByVal CTACTE_ID As Integer, ByVal TARCLIE_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCteDetalle_obtenerxTARCLIE_ID", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTE_ID", CTACTE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_ID", TARCLIE_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCteDetalle")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CtaCte_historial(ByVal CLIE_ID As Integer,
                                       ByVal CTACTE_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_historial", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTE_ID", CTACTE_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Tarifas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function



#Region "Cobros"
    'SELECT * FROM CtaCteDetalle WHERE CTACTEDET_ID = '135'

    Public Function CtaCteDetalle_consulta(ByVal TARCLIE_ID As Integer, ByVal CLIE_ID As Integer, ByVal LOCAL_ID As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim comando As New OleDbCommand("CtaCteDetalle_consulta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CLIE_ID", CLIE_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LOCAL_ID", LOCAL_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TARCLIE_ID", TARCLIE_ID))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCteDetalle")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CtaCteDetalle_modifsaldodeudor(ByVal CTACTEDET_ID As Integer, ByVal CTACTEDET_saldodeudor As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCteDetalle_modifsaldodeudor", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTEDET_ID", CTACTEDET_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTEDET_saldodeudor", CTACTEDET_saldodeudor))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCteDetalle")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function



    Public Function CtaCteDetalle_ObtenerUltimoComprobante() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        'IDcarga = "'" + IDcarga + "'"

        Dim Consulta As String = ""


        Consulta += "select top 1 CTACTEDET_comprobante, CtaCteDetalle.CTACTEDET_ID from CtaCteDetalle where CTACTEDET_tipo = 'haber' order by CTACTEDET_ID desc"
        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCteDetalle")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function




#End Region





End Class
