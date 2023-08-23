Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Configuracion
    Inherits Capa_Datos.Conexion


    Public Function Configuracion_obtenertodo_b() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Configuracion_obtenertodo", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        'comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)

        DA.Fill(ds, "Configuracion")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Configuracion_obtenertodo() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "SELECT * FROM Configuracion"



        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Configuracion")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


End Class
