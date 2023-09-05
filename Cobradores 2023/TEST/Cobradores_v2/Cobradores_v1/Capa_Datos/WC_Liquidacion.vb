Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class WC_Liquidacion
    Inherits Capa_Datos.Conexion


#Region "LIQUIDACION CREDITOS - IMPRIME REGALO"

    Public Function CtaCte_obtener_fecha(ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dim Consulta As String = ""

        Consulta += "select * from CtaCte where Fecha = " + Fecha + " order by CtaCte.IdCtacte asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function CtaCte_obtener_registro(ByVal Codigo As String, ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Codigo = "'" + Codigo + "'"

        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dim Consulta As String = ""

        Consulta += "select * from CtaCte where Codigo = " + Codigo + " and Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function CtaCte_ActualizarImprimeRegalo(ByVal IdCtaCte As String, ByVal Codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        IdCtaCte = "'" + IdCtaCte + "'"
        Codigo = "'" + Codigo + "'"


        'Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dim Consulta As String = ""
        Consulta += "declare @ImprimeReg as decimal(38,2) "
        Consulta += " select @ImprimeReg = SaldoRegalo from Clientes where Clientes.Codigo = " + Codigo
        Consulta += " update CtaCte set ImprimeRegalo = @ImprimeReg where CtaCte.IdCtacte = " + IdCtaCte

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


#End Region


#Region "LIQUIDACION GRUPO _ TIPO 2"
    Public Function CtaCte_RegalosaCero(ByVal IdCtacte As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        IdCtacte = "'" + IdCtacte + "'"

        Dim Consulta As String = ""

        Consulta += "update CtaCte set Regalos=0 where CtaCte.IdCtacte = " + IdCtacte

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function Clientes_SaldoRegaloaCero(ByVal Grupo_id As String, ByVal ClientePorcentaje As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Grupo_id = "'" + Grupo_id + "'"
        ClientePorcentaje = "'" + ClientePorcentaje + "'"
        Dim Consulta As String = ""

        Consulta += "update Clientes set SaldoRegalo = 0 where Clientes.Grupo_id = " + Grupo_id + " and (Clientes.Codigo <> " + ClientePorcentaje + ")"

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


#End Region


#Region "LIQUIDACION CLIENTEDEUDA"
    Public Function CTACTE_obtenergrupo_fecha(ByVal Fecha_valor As Date, ByVal Grupo_id As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Grupo_id = "'" + Grupo_id + "'"
        Dim Consulta As String = ""

        Consulta += "SELECT * FROM CtaCte WHERE (Fecha = " + Fecha + " ) AND (Grupo_Id = " + Grupo_id + " ) ORDER BY CtaCte.Codigo ASC "

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function CtaCte_ActualizarClienteDeuda(ByVal IdCtacte As Integer, ByVal DejoGano As Decimal, ByVal DejoGanoSC As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_ActualizarClienteDeuda", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCtacte", IdCtacte))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGano", DejoGano))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGanoSC", DejoGanoSC))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "Recorridos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CtaCte_altaClienteDeuda(ByVal Fecha As Date, ByVal Grupo_Id As Integer, ByVal Codigo As Integer, ByVal DejoGano As Decimal, ByVal DejoGanoSC As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_altaClienteDeuda", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Id", Grupo_Id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGano", DejoGano))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGanoSC", DejoGanoSC))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function




#End Region


    Public Function XCargas_recuperarclierecaudacion(ByVal Fecha_valor As Date, ByVal codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        codigo = "'" + codigo + "'"
        Dim Consulta As String = ""

        Consulta += "select XCargas.Cliente, SUM(XCargas.Importe) as 'Importe' from XCargas inner join XCargas_Recorridos on XCargas.IDcarga = XCargas_Recorridos.IDcarga"
        Consulta += " where XCargas.Cliente = " + codigo + " and (XCargas_Recorridos.Recorrido_codigo <> 'z') and (XCargas_Recorridos.Recorrido_codigo <> 'Z') and "
        Consulta += "(XCargas.Fecha = " + Fecha + " ) group by XCargas.Cliente "
        Consulta += "select XCargas.Cliente, XCargas_Recorridos.Recorrido_codigo from XCargas inner join XCargas_Recorridos on XCargas.IDcarga = XCargas_Recorridos.IDcarga"
        Consulta += " where XCargas.Cliente = " + codigo + " and (XCargas_Recorridos.Recorrido_codigo <> 'z') and (XCargas_Recorridos.Recorrido_codigo <> 'Z') and "
        Consulta += "(XCargas.Fecha = " + Fecha + " ) group by Recorrido_codigo, XCargas.Cliente order by Recorrido_codigo asc "

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function






    Public Function LiquidacionFinal_validarZ(ByVal IDcarga As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        IDcarga = "'" + IDcarga + "'"

        Dim Consulta As String = ""

        Consulta += "select IDcarga, Recorrido, Cliente from XCargasJunto where IDcarga = " + IDcarga
        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargasJunto")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


    'ESTE PROC ES PARA RECUPERAR TODOS LOS RECORRIDOS HABILITADOS PARA UNA FECHA
    Public Function Liquidacion_validar_recorridosB(ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dim Consulta As String = ""

        Consulta += "select * from Recorridos inner join Puntos on Recorridos.Idrecorrido = Puntos.Idrecorrido"
        Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Recorridos")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


    Public Function Liquidacion_validar_recorridos(ByVal Fecha As Date, ByVal Codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_validar_recorridos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "recorridos".
        DA.Fill(ds, "Recorridos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Liquidacion_parcial_recuperar(ByVal Codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_parcial_recuperar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "XCargas".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Liquidacion_parcial_recuperarXcargas(ByVal Codigo As String, ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_parcial_recuperarXcargas", dbconn)
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

    Public Function Liquidacion_recuperarXcargas_totales() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_recuperarXcargas_totales", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "XCargas".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    Public Function Liquidacion_todoXcargas() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_todoXcargas", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "XCargas".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Liquidacion_obtenerBanderas() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_obtenerBanderas", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Banderas".
        DA.Fill(ds, "Banderas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function Liquidacion_final_recuperarXcargas(ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Liquidacion_final_recuperarXcargas", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Banderas".
        DA.Fill(ds, "Xcargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

#Region "LIQUIDACION FINAL"



    Public Function LiquidacionFinal_obtener_prestamoscreditos(ByVal Fecha As Date, ByVal Cliente As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionFinal_obtener_prestamoscreditos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "PrestamosCreditos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function XCargasN_recuperar_Z(ByVal Tabla As String, ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dim Consulta As String = ""

        Consulta += "SELECT IDcarga, Recorrido, convert( int,Cliente) as 'Cliente', Importe, TotalImporte FROM " + Tabla
        Consulta += " WHERE Recorrido = 'Z' AND Fecha = " + Fecha + " ORDER BY Cliente ASC"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function CtaCte_actualizarCamposB(ByVal Codigo As Integer, ByVal Fecha As Date, ByVal RecaudacionB As Decimal, ByVal ComisionB As Decimal, ByVal PremiosB As Decimal,
                                             ByVal ReclamosB As Decimal, ByVal DejoGanoB As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CtaCte_actualizarCamposB", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Codigo", Codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))
        comando.Parameters.Add(New OleDb.OleDbParameter("@RecaudacionB", RecaudacionB))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ComisionB", ComisionB))
        comando.Parameters.Add(New OleDb.OleDbParameter("@PremiosB", PremiosB))
        comando.Parameters.Add(New OleDb.OleDbParameter("@ReclamosB", ReclamosB))
        comando.Parameters.Add(New OleDb.OleDbParameter("@DejoGanoB", DejoGanoB))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function XCargas_duplicar(ByVal IDcarga As Integer, ByVal Importe As Decimal, ByVal Recorrido_codigo As String, ByVal Cliente As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("XCargas_duplicar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IDcarga", IDcarga))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe", Importe))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Recorrido_codigo", Recorrido_codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function XCargas_duplicarYmodificar(ByVal IDcarga As Integer, ByVal excedente As Decimal, ByVal importe_max As Decimal,
                                               ByVal Recorrido_codigo As String, ByVal Cliente As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("XCargas_duplicarYmodificar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IDcarga", IDcarga))
        comando.Parameters.Add(New OleDb.OleDbParameter("@excedente", excedente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe_max", importe_max))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Recorrido_codigo", Recorrido_codigo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    'XCargas_Cubiertainsert
    Public Function XCargas_Cubiertainsert(ByVal IDcarga As Integer, ByVal Importe_max As Decimal, ByVal TotalImporte As Decimal,
                                               ByVal SinComputo As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("XCargas_Cubiertainsert", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IDcarga", IDcarga))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe_max", Importe_max))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TotalImporte", TotalImporte))
        comando.Parameters.Add(New OleDb.OleDbParameter("@SinComputo", SinComputo))
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Recorrido_codigo", Recorrido_codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function XCargasJunto_insert(ByVal IDcarga As String, ByVal Recorrido_codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Recorrido_codigo = "'" + Recorrido_codigo + "'"
        IDcarga = "'" + IDcarga + "'"
        Dim Consulta As String = ""

        Consulta += "INSERT INTO XCargasJunto (Recorrido, Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, TotalImporte, Fecha, Hora, Verificado, Terminal, Item) "
        Consulta += "select " + Recorrido_codigo + ", Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, TotalImporte, Fecha, Hora, Verificado, Terminal, Item from XCargas where IDcarga = " + IDcarga

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function
    'XCargas_Recorridos_alta
    Public Function XCargas_Recorridos_alta(ByVal IDcarga As String, ByVal Recorrido_codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Recorrido_codigo = "'" + Recorrido_codigo + "'"
        IDcarga = "'" + IDcarga + "'"

        Dim Consulta As String = "insert XCargas_Recorridos (IDcarga, Recorrido_codigo) values (" + IDcarga + ", " + Recorrido_codigo + ")"


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function Eliminar_XCargasYXCargasJunto(ByVal IDcarga As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        IDcarga = "'" + IDcarga + "'"

        Dim Consulta As String = "delete from XCargas where IDcarga = " + IDcarga

        Consulta += " delete from XCargasJunto where IDcarga = " + IDcarga


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function Eliminar_XCargas_Recorridos(ByVal IDcarga As String, ByVal Recorrido_codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        IDcarga = "'" + IDcarga + "'"
        Recorrido_codigo = "'" + Recorrido_codigo + "'"
        Dim Consulta As String = "delete from XCargas_Recorridos where IDcarga = " + IDcarga
        Consulta += " and Recorrido_codigo = " + Recorrido_codigo


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    'obtener registros xcargas y xcargas recorridos en un inner join
    'select * from XCargas inner join XCargas_Recorridos on XCargas.IDcarga= XCargas_Recorridos.IDcarga where IDcarga = ''
    Public Function XCargas_Recorridos_obtener(ByVal IDcarga As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        IDcarga = "'" + IDcarga + "'"

        Dim Consulta As String = "select * from XCargas inner join XCargas_Recorridos on XCargas.IDcarga= XCargas_Recorridos.IDcarga where XCargas.IDcarga = " + IDcarga


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    'update XCargas set TotalImporte = '' where IDcarga = ''
    'update XCargasJunto Set Recorrido = '', TotalImporte = '' where IDcarga = '''

    Public Function XCargasYXCargasJunto_actualizar(ByVal IDcarga As String, ByVal Recorrido_codigo As String, ByVal TotalImporte As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Recorrido_codigo = "'" + Recorrido_codigo + "'"
        IDcarga = "'" + IDcarga + "'"
        TotalImporte = "'" + TotalImporte + "'"

        Dim Consulta As String = "update XCargas set TotalImporte = " + TotalImporte + " where IDcarga = " + IDcarga
        Consulta += " update XCargasJunto Set Recorrido = " + Recorrido_codigo + ", TotalImporte = " + TotalImporte + " where IDcarga = " + IDcarga

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function XCargasYXCargasJunto_actualizar2(ByVal IDcarga As String, ByVal Importe As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        'Recorrido_codigo = "'" + Recorrido_codigo + "'"
        IDcarga = "'" + IDcarga + "'"
        Importe = "'" + Importe + "'"

        Dim Consulta As String = "update XCargas set TotalImporte = " + Importe + ", Importe =" + Importe + " where IDcarga = " + IDcarga
        Consulta += " update XCargasJunto Set Importe = " + Importe + ", TotalImporte = " + Importe + " where IDcarga = " + IDcarga

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    'CubGeneral_duplicar1
    Public Function CubGeneral_duplicar1(ByVal IDcarga As Integer, ByVal Importe As Decimal, ByVal TotalImporte As Decimal, ByVal Cliente As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CubGeneral_duplicar1", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IDcarga", IDcarga))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe", Importe))
        comando.Parameters.Add(New OleDb.OleDbParameter("@TotalImporte", TotalImporte))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function CubGeneral_duplicar1b(ByVal IDcarga As String, ByVal Recorrido As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Recorrido = "'" + Recorrido + "'"
        IDcarga = "'" + IDcarga + "'"
        Dim Consulta As String = ""

        Consulta += "INSERT XCargas_Recorridos (IDcarga , Recorrido_codigo) VALUES (" + IDcarga + ", " + Recorrido + ")"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "XCargas_Z")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    Public Function CubGeneral_duplicarXcargasJunto(ByVal IDcarga As Integer, ByVal Recorrido_codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("CubGeneral_duplicarXcargasJunto", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IDcarga", IDcarga))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Recorrido_codigo", Recorrido_codigo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "PrestamosCreditos".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

#Region "COBRO PRESTAMOS MANUALES"

    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function PrestamosRegalos_BuscarFecha(ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "select Idprestamocredito,PrestamosCreditos.Fecha, "
        Consulta += "PrestamosCreditos.Importe,PrestamosCreditos.Saldo,Clientes.Codigo AS 'Cliente_Codigo', Clientes.Cliente as 'Clie_ID', Clientes.Grupo_id "
        Consulta += "from PrestamosCreditos inner join Clientes on PrestamosCreditos.Cliente = Clientes.Cliente "
        Consulta += "where PrestamosCreditos.Tipo = 'P' and PrestamosCreditos.Tipocobro = '2' and PrestamosCreditos.Estado_id = '1' and PrestamosCreditos.Fecha = " + Fecha


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "PrestamosManuales")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function



    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function PrestamosManuales_BuscarFecha(ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "select Idprestamocredito,PrestamosCreditos.Fecha, "
        Consulta += "PrestamosCreditos.Importe,PrestamosCreditos.Saldo,Clientes.Codigo AS 'Cliente_Codigo', Clientes.Cliente as 'Clie_ID', Clientes.Grupo_id "
        Consulta += "from PrestamosCreditos inner join Clientes on PrestamosCreditos.Cliente = Clientes.Cliente "
        Consulta += "where PrestamosCreditos.Tipo = 'P' and PrestamosCreditos.Tipocobro = '3' and PrestamosCreditos.Estado_id = '1' and PrestamosCreditos.Fecha = " + Fecha


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "PrestamosManuales")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function
#End Region
#Region "COBRO PRESTAMOS POR COMISION"

    Public Function Prestamos_InsertCtaCte(ByVal IdCtacte As String, ByVal Prestamo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        IdCtacte = "'" + IdCtacte + "'"
        Prestamo = "'" + Prestamo + "'"
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "update CtaCte set Prestamo = " + Prestamo + " where CtaCte.IdCtacte = " + IdCtacte


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

#End Region
#Region "Cobro Creditos"
    Public Function Credito_InsertCtaCte(ByVal IdCtacte As String, ByVal Credito As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        IdCtacte = "'" + IdCtacte + "'"
        Credito = "'" + Credito + "'"
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "update CtaCte set Credito = " + Credito + " where CtaCte.IdCtacte = " + IdCtacte


        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "CtaCte")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

#End Region




#End Region

#Region "LIQUIDACION DE REGALOS"

    Public Function LiqRegalos_BuscarProcesoEnClientes(ByVal Codigo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim Consulta As String = ""

        Consulta += "SELECT Cliente, Codigo, Proceso FROM Clientes WHERE Clientes.Codigo = " + Codigo

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Clientes")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function LiqRegalos_BuscarEnParametro(ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Dim Consulta As String = ""

        Consulta += "SELECT * FROM Parametro WHERE Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Parametro")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function

    '----------------FORMATO DE FECHA CORRECTA: 20-12-2022---------------------------------------------
    Public Function LiqRegalos_ActualizarTablaParametro(ByVal Liquidacion As String, ByVal Fecha_valor As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Fecha As String = "'" + Fecha_valor.Year.ToString + "-" + Fecha_valor.Month.ToString + "-" + Fecha_valor.Day.ToString + "'" 'le agrego comillas a la fecha
        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Liquidacion = "'" + Liquidacion + "'"
        Dim Consulta As String = ""

        Consulta += "Update Parametro set LiqRegalos = " + Liquidacion + " where Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Parametro")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function







    'recuperar clientes, procedo="D"
    Public Function LiquidacionRegalos_obtener_ClieDiario() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalos_obtener_ClieDiario", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Clientes".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    'obtiene la info de la ctacta del cliente para la fecha de la ultima liquidacion completada
    Public Function LiquidacionRegalos_obtenerctacte(ByVal Codigo As Integer, ByVal Fecha As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalos_obtenerctacte", dbconn)
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

    'actualizo ctacte el campo regalos
    Public Function LiquidacionRegalosDiario_actualizarCtaCte(ByVal IdCtaCte As Integer, ByVal Regalos As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalosDiario_actualizarCtaCte", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCtaCte", IdCtaCte))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Regalos", Regalos))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    'actualizar en tabla cliente, campos saldoregalo y saldo
    Public Function LiquidacionRegalosDiario_actualizarClie(ByVal Cliente As Integer, ByVal SaldoRegalo As Decimal, ByVal Saldo As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalosDiario_actualizarClie", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@SaldoRegalo", SaldoRegalo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Saldo", Saldo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "Cliente")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    'recuperar clientes, procedo="S"
    Public Function LiquidacionRegalos_obtener_ClieSemanal() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalos_obtener_ClieSemanal", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Clientes".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    'actualizo ctacte el campo regalos
    Public Function LiquidacionRegalosSemanal_actualizarCtaCte(ByVal IdCtaCte As Integer, ByVal Regalos As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalosSemanal_actualizarCtaCte", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCtaCte", IdCtaCte))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Regalos", Regalos))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    'actualizar en tabla cliente, campos saldoregalo y saldo
    Public Function LiquidacionRegalosSemanal_actualizarClie(ByVal Cliente As Integer, ByVal SaldoRegalo As Decimal, ByVal Saldo As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalosSemanal_actualizarClie", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@SaldoRegalo", SaldoRegalo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Saldo", Saldo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "Cliente")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    'recuperar clientes, procedo="M"
    Public Function LiquidacionRegalos_obtener_ClieMensual() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalos_obtener_ClieMensual", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Clientes".
        DA.Fill(ds, "Clientes")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    'actualizo ctacte el campo regalos
    Public Function LiquidacionRegalosMensual_actualizarCtaCte(ByVal IdCtaCte As Integer, ByVal Regalos As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalosMensual_actualizarCtaCte", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@IdCtaCte", IdCtaCte))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Regalos", Regalos))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function



    'actualizar en tabla cliente, campos saldoregalo y saldo
    Public Function LiquidacionRegalosMensual_actualizarClie(ByVal Cliente As Integer, ByVal SaldoRegalo As Decimal, ByVal Saldo As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionRegalosMensual_actualizarClie", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Cliente", Cliente))
        comando.Parameters.Add(New OleDb.OleDbParameter("@SaldoRegalo", SaldoRegalo))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Saldo", Saldo))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "Cliente")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


#End Region

#Region "LIQUIDACION DE GRUPOS"

    Function LiquidacionGrupo_modifSaldoAnterior(ByVal Grupo_id As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha
        Grupo_id = "'" + Grupo_id + "'"

        Consulta += "UPDATE Grupos SET Saldoanterior = Importe where Grupos.Grupo_id = " + Grupo_id


        'Consulta += " INNER JOIN XCargas_Recorridos ON XCargas.IDcarga = XCargas_Recorridos.IDcarga "

        'Consulta += " WHERE Fecha = " + Fecha + " AND (Cliente = " + Cliente + ")"
        'Consulta += " AND (Recorrido_Codigo IN (" + Codigos + "))"
        'Consulta += " AND Fecha = " + Fecha

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Grupos")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function



    Public Function LiquidacionGrupos_ObtenerCtaCtexrangofecha(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Grupo_Id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionGrupos_ObtenerCtaCtexrangofecha", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@FechaDesde", FechaDesde))
        comando.Parameters.Add(New OleDb.OleDbParameter("@FechaHasta", FechaHasta))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Id", Grupo_Id))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "CtaCte")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function LiquidacionGrupos_ObtenerGastosxrangofecha(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Grupo_Id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionGrupos_ObtenerGastosxrangofecha", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@FechaDesde", FechaDesde))
        comando.Parameters.Add(New OleDb.OleDbParameter("@FechaHasta", FechaHasta))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Id", Grupo_Id))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "CtaCte".
        DA.Fill(ds, "Gastos")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function LiquidacionGrupos_GruposModiffecha(ByVal Grupo_Id As Integer, ByVal FechaHasta As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionGrupos_GruposModiffecha", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Id", Grupo_Id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", FechaHasta))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupo".
        DA.Fill(ds, "Grupo")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function LiquidacionGrupos_GruposModifimporte(ByVal Grupo_Id As Integer, ByVal Importe As Decimal) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionGrupos_GruposModifimporte", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Grupo_Id", Grupo_Id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Importe", Importe))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupo".
        DA.Fill(ds, "Grupo")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

#End Region

#Region "LOAD XCARGAS"

    Public Function XCargas_load() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("XCargas_load", dbconn)
        comando.CommandType = CommandType.StoredProcedure



        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupo".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

#End Region

#Region "BACKUP y RESTORE"
    Public Function BKP() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("BKP", dbconn)
        comando.CommandType = CommandType.StoredProcedure



        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupo".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function BACKUP(ByVal CONDICION As String, ByVal FECHA As Date) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("BACKUP", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CONDICION", CONDICION))
        comando.Parameters.Add(New OleDb.OleDbParameter("@FECHA", FECHA))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupo".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function BACKUP_aux(ByVal CONDICION As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("BACKUP_aux", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@CONDICION", CONDICION))


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupo".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


    'Se elimina la base de datos en el modulo de reliquidacion
    Public Function DROP_DATABASE_WEBCENTRAL_COPY() As DataTable
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""
        Consulta += "DROP DATABASE WebCentral_copy"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Clientes")
        dbconn.Close()
        Return ds.Tables(0)
        ''''### son las 20:16
    End Function


    'restore_backup
    Public Function restore_backup(ByVal bdName As String, ByVal PathTarget As String, ByVal devicePhyname As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("restore_backup", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@bdName", bdName)) 'NOMBRE DE NUEVA BD
        comando.Parameters.Add(New OleDb.OleDbParameter("@PathTarget", PathTarget)) 'RUTA DESTINO DONDE SE GUARDARAN LOS ARCHIVOS .MDF Y .LDF
        comando.Parameters.Add(New OleDb.OleDbParameter("@devicePhyname", devicePhyname)) 'RUTA ORIGEN DONDE SE ENCUENTRA EL ARCHIVO .BAK


        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupo".
        DA.Fill(ds, "BD_NUEVA")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function


#End Region

#Region "DELETE XCARGAS"
    Public Function XCargas_delete() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("XCargas_delete", dbconn)
        comando.CommandType = CommandType.StoredProcedure



        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Grupo".
        DA.Fill(ds, "XCargas")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function
#End Region



#Region "FUNCIONES EN XCrgHistoria"

    Public Function LiquidacionFinal_XCrgHistoria_Guardar() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LiquidacionFinal_XCrgHistoria_Guardar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        'comando.Parameters.Add(New OleDb.OleDbParameter("@Fecha", Fecha))

        Dim ds As New DataSet()
        Dim DA As New OleDbDataAdapter(comando)
        ''Fill= Método que Agrega filas al objeto DataSet y crea un objeto DataTable denominado "Tabla", en nuestro caso "Banderas".
        DA.Fill(ds, "Historia")
        ''Cierro la conexión
        dbconn.Close()
        Return ds
    End Function

    Public Function LiquidacionFinal_XCrgHistoria_Guardar2() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim Consulta As String = ""

        'Fecha = "'" + Fecha.ToString + "'" 'le agrego comillas a la fecha

        Consulta += "INSERT INTO dbo.XCrgHistoria (Recorrido, Cliente, Pid, Importe, Suc, Pid2, Suc2, R, SinComputo, TotalImporte, Fecha, Hora, Verificado, Terminal, Item) "
        Consulta += "SELECT  t.Recorrido , t.Cliente, t.Pid, t.Importe, t.Suc, t.Pid2, t.Suc2, t.R, t.SinComputo, t.TotalImporte, t.Fecha, t.Hora, t.Verificado, t.Terminal, t.Item FROM XCargasJunto  AS t"




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





#End Region

End Class