Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WC_Backup
    Inherits Capa_Datos.Conexion

    Public Function Backup(ByVal backupfile As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "backup database " & "WebCentral" & " to disk='" & backupfile



        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "PrestamosManuales")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function
    Public Function Backup_copy(ByVal backupfile As String) As DataSet
        Try
            dbconnMaster.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "backup database " & "WebCentral" & " to disk='" & backupfile



        Dim DA As New OleDbDataAdapter(Consulta, dbconnMaster)
        Dim ds As New DataSet()
        DA.Fill(ds, "PrestamosManuales")
        dbconnMaster.Close()
        Return ds
        ''''### son las 20:16
    End Function


End Class
