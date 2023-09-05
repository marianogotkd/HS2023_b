Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Cubiertas
    Inherits Capa_Datos.Conexion

    Public Function ClientesCub_BuscarCliente(ByVal Codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("ClientesCub_BuscarCliente", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "ClientesCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function ClientesCub_alta_op1(ByVal Codigo As String, ByVal UnaCifra As Decimal, ByVal DosCifras As Decimal, ByVal TresCifras As Decimal, ByVal CuatroCifras As Decimal, ByVal Consideracion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("ClientesCub_alta_op1", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@UnaCifra", UnaCifra))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DosCifras", DosCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TresCifras", TresCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CuatroCifras", CuatroCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Consideracion", Consideracion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "ClientesCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function ClientesCub_modificar_op1(ByVal IdCubCliente As Integer, ByVal UnaCifra As Decimal, ByVal DosCifras As Decimal, ByVal TresCifras As Decimal, ByVal CuatroCifras As Decimal, ByVal Consideracion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("ClientesCub_modificar_op1", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCubCliente", IdCubCliente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@UnaCifra", UnaCifra))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DosCifras", DosCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TresCifras", TresCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CuatroCifras", CuatroCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Consideracion", Consideracion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "ClientesCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function ClientesCub_eliminar_op1(ByVal IdCubCliente As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("ClientesCub_eliminar_op1", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCubCliente", IdCubCliente))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "ClientesCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function GruposCub_BuscarOp2(ByVal Codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_BuscarOp2", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_alta_op2(ByVal Grupo_Codigo As String, ByVal UnaCifra As Decimal, ByVal DosCifras As Decimal,
                                       ByVal TresCifras As Decimal, ByVal CuatroCifras As Decimal, ByVal Cliente_id As Integer, ByVal Consideracion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_alta_op2", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Codigo", Grupo_Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@UnaCifra", UnaCifra))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DosCifras", DosCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TresCifras", TresCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CuatroCifras", CuatroCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente_id", Cliente_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Consideracion", Consideracion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_modificar_op2(ByVal IdCubGrupo As Integer, ByVal UnaCifra As Decimal, ByVal DosCifras As Decimal,
                                       ByVal TresCifras As Decimal, ByVal CuatroCifras As Decimal, ByVal Consideracion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_modificar_op2", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCubGrupo", IdCubGrupo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@UnaCifra", UnaCifra))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DosCifras", DosCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TresCifras", TresCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CuatroCifras", CuatroCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Consideracion", Consideracion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_eliminar_op2(ByVal IdCubGrupo As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_eliminar_op2", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCubGrupo", IdCubGrupo))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    'ClientesCub
    Public Function ClientesCub_Obtener() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim Consulta As String = ""

        Consulta += "select IdCubCliente,Cliente_id,	Codigo,	UnaCifra,DosCifras,	TresCifras,	CuatroCifras, Consideracion from ClientesCub"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "ClientesCub")
        dbconn.Close()
        Return ds

    End Function




    'SELECT IdCubGrupo, Tipo, Grupo_id, UnaCifra, DosCifras, TresCifras, CuatroCifras, Cliente_id, Consideracion FROM GruposCub where GruposCub.Tipo = 0
    Public Function GruposCub_ObtenerOp2() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim Consulta As String = ""

        Consulta += "SELECT IdCubGrupo, Tipo, Grupo_id, UnaCifra, DosCifras, TresCifras, CuatroCifras, Cliente_id, Consideracion FROM GruposCub where GruposCub.Tipo = 0"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "GruposCub")
        dbconn.Close()
        Return ds

    End Function

    'SELECT IdCubGrupo, Tipo, Grupo_id, UnaCifra, DosCifras, TresCifras, CuatroCifras, Cliente_id, Consideracion FROM GruposCub where GruposCub.Tipo = 1
    Public Function GruposCub_ObtenerOp3() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim Consulta As String = ""

        Consulta += "SELECT IdCubGrupo, Tipo, Grupo_id, UnaCifra, DosCifras, TresCifras, CuatroCifras, Cliente_id, Consideracion FROM GruposCub where GruposCub.Tipo = 1"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "GruposCub")
        dbconn.Close()
        Return ds

    End Function



    Public Function GruposCub_BuscarOp3(ByVal Codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_BuscarOp3", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_alta_op3(ByVal Grupo_Codigo As String, ByVal UnaCifra As Decimal, ByVal DosCifras As Decimal,
                                       ByVal TresCifras As Decimal, ByVal CuatroCifras As Decimal, ByVal Cliente_id As Integer, ByVal Consideracion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_alta_op3", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Codigo", Grupo_Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@UnaCifra", UnaCifra))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DosCifras", DosCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TresCifras", TresCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CuatroCifras", CuatroCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente_id", Cliente_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Consideracion", Consideracion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_modificar_op3(ByVal IdCubGrupo As Integer, ByVal UnaCifra As Decimal, ByVal DosCifras As Decimal,
                                       ByVal TresCifras As Decimal, ByVal CuatroCifras As Decimal, ByVal Consideracion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_modificar_op3", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCubGrupo", IdCubGrupo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@UnaCifra", UnaCifra))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DosCifras", DosCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TresCifras", TresCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CuatroCifras", CuatroCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Consideracion", Consideracion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_eliminar_op3(ByVal IdCubGrupo As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_eliminar_op3", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCubGrupo", IdCubGrupo))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_BuscarOp4() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_BuscarOp4", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        'comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_alta_op4(ByVal UnaCifra As Decimal, ByVal DosCifras As Decimal, ByVal TresCifras As Decimal, ByVal CuatroCifras As Decimal, ByVal Cliente_id As Integer, ByVal Consideracion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_alta_op4", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        'comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Codigo", Grupo_Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@UnaCifra", UnaCifra))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DosCifras", DosCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TresCifras", TresCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CuatroCifras", CuatroCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente_id", Cliente_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Consideracion", Consideracion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_modificar_op4(ByVal IdCubGrupo As Integer, ByVal UnaCifra As Decimal, ByVal DosCifras As Decimal,
                                       ByVal TresCifras As Decimal, ByVal CuatroCifras As Decimal, ByVal Consideracion As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_modificar_op4", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCubGrupo", IdCubGrupo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@UnaCifra", UnaCifra))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DosCifras", DosCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TresCifras", TresCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@CuatroCifras", CuatroCifras))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Consideracion", Consideracion))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function GruposCub_eliminar_op4(ByVal IdCubGrupo As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("GruposCub_eliminar_op4", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCubGrupo", IdCubGrupo))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "GruposCub")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


#Region "SETEADO DE TABLA XCARGAS PARA CUBIERTAS - INDIVIDUAL DIVIDE EN EL MISMO CLIENTE"

    'se agregan los registros que tenga Z en recorrido, si se agregaron en XCargas, no asi en xcargas_recorrido, ver proc. alm: XCargas_load 
    Public Function XCargas_Recorridos_agregarRegZ() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "INSERT INTO XCargas_Recorridos (IDcarga, Recorrido_codigo) "
        Consulta += "SELECT IDcarga, Recorrido FROM XCargasJunto WHERE XCargasJunto.Recorrido = 'Z' or XCargasJunto.Recorrido = 'z'"

        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Xcargas_recorridos")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function




    Public Function XCargas_Consultar_N1() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "SELECT * FROM XCargas ORDER BY IDcarga ASC "
        Consulta += " SELECT * FROM XCargas_Recorridos ORDER BY IDcarga ASC"

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

    Public Function XCargasJunto_eliminarReg(ByVal IDcarga As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "DELETE FROM XCargasJunto WHERE IDcarga = " + IDcarga


        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargasJunto")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function XCargasJunto_COPIARegFromXCargas(ByVal IDcarga As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        'IDcarga = "'" + IDcarga + "'"

        Consulta += "INSERT INTO XCargasJunto (Recorrido, Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, TotalImporte, Fecha, Hora, Verificado, Terminal, Item) "
        Consulta += "SELECT XCargas_Recorridos.Recorrido_codigo, Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, Importe, Fecha, Hora, Verificado, Terminal, Item  FROM XCargas INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga WHERE XCargas_Recorridos.IDcarga = " + IDcarga

        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargasJunto")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function XCargas_eliminarRegYRecargar() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "DELETE FROM XCargas "
        Consulta += " DELETE FROM XCargas_Recorridos "
        Consulta += " INSERT INTO XCargas (Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, TotalImporte, Fecha, Hora, Verificado, Terminal, Item) "
        Consulta += " SELECT Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, TotalImporte, Fecha, Hora, Verificado, Terminal, Item FROM XCargasJunto "
        Consulta += " INSERT INTO XCargas_Recorridos (IDcarga, Recorrido_codigo) "
        Consulta += " SELECT IDcarga, Recorrido FROM XCargasJunto "


        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargasJunto")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function
    Public Function XCargas_Y_XCargasJunto_ActualizarSinComputo(ByVal IDcarga As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "UPDATE XCargas SET XCARGAS.SinComputo = 1 WHERE XCargas.IDcarga = " + IDcarga
        Consulta += " UPDATE XCargasJunto SET SinComputo = 1 WHERE IDcarga = " + IDcarga

        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargasJunto")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


#End Region


End Class
