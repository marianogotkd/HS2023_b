Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Comprobante
    Inherits Capa_Datos.Conexion

    Public Function Comprobante_alta(ByVal nrocomprobante As String,
                                     ByVal CTACTEDET_ID As Integer,
                                     ByVal Concepto_Tarifa As String,
                                     ByVal Concepto_fecha As Date,
                                     ByVal Comprobante_Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Comprobante_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@nrocomprobante", nrocomprobante))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CTACTEDET_ID", CTACTEDET_ID))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Concepto_Tarifa", Concepto_Tarifa))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Concepto_fecha", Concepto_fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Comprobante_Fecha", Comprobante_Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Comprobante_obtener(ByVal nrocomprobante As String, ByVal CLIE_ID As String, ByVal LOCAL_ID As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        'IDcarga = "'" + IDcarga + "'"

        nrocomprobante = "'" + nrocomprobante + "'"
        CLIE_ID = "'" + CLIE_ID + "'"
        LOCAL_ID = "'" + LOCAL_ID + "'"

        Dim Consulta As String = ""

        Consulta += "SELECT * FROM Clientes INNER JOIN CtaCte ON Clientes.CLIE_ID = CtaCte.CLIE_ID WHERE Clientes.CLIE_ID = " + CLIE_ID
        Consulta += " Select * FROM Local inner join Pasillo on Local.PASILLO_ID=Pasillo.PASILLO_ID 
inner join Sector on Pasillo.SECTOR_ID = Sector.SECTOR_ID  WHERE Local.LOCAL_ID = " + LOCAL_ID

        Consulta += " select Comprobante.Comprobante_id, Comprobante.nrocomprobante, Comprobante.CTACTEDET_ID, Comprobante.Concepto_Tarifa, Comprobante.Concepto_fecha, Comprobante.Comprobante_Fecha, CtaCteDetalle.CTACTEDET_importe from Comprobante inner join CtaCteDetalle on Comprobante.CTACTEDET_ID= CtaCteDetalle.CTACTEDET_ID where Comprobante.nrocomprobante = " + nrocomprobante
        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Comprobante")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function





End Class
