﻿Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WC_parametro
    Inherits Capa_Datos.Conexion

    Public Function Parametro_Iniciar_dia(ByVal Fecha As Date, ByVal Dia As Integer, ByVal Recorrido As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_Iniciar_dia", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Dia", Dia))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Recorrido", Recorrido))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Parametro_finalizar_dia(ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_finalizar_dia", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Parametro_modificar_dia(ByVal Parametro_id As Integer, ByVal Fecha As Date, ByVal Dia As Integer, ByVal Recorrido As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_modificar_dia", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Parametro_id", Parametro_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Dia", Dia))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Recorrido", Recorrido))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function




    Public Function Parametro_consultar_fecha(ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_consultar_fecha", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Parametro_obtener_dia() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_obtener_dia", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Parametro_consultar_ultliq() As DataSet
        'Resumen: recupera el ultimo registro de la tabla Parametro donde el estado='inactivo', es decir el ultimo dia donde se ejecuto la liquidacion.
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_consultar_ultliq", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Parametro".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


#Region "reliquidacion"
    Public Function Parametro_obtener_UltimoDiaLiq() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_obtener_UltimoDiaLiq", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function
#End Region



    Public Function Parametro_LiqFinalModifEstado(ByVal Parametro_id As Integer, ByVal LiqFinal As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_LiqFinalModifEstado", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Parametro_id", Parametro_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@LiqFinal", LiqFinal))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function



    Public Function Parametro_ReliquidacionModifEstado(ByVal Parametro_id As Integer, ByVal Reliquidacion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Parametro_ReliquidacionModifEstado", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Parametro_id", Parametro_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Reliquidacion", Reliquidacion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupos".
        DA.Fill(ds, "Parametro")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


End Class
