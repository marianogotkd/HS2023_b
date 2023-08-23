Imports System.Data.OleDb
Imports System.Data.DataRow
Public Class Llave
    Inherits Capa_de_datos.Conexion


    Public Function obtener_graduacion() As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try


        Dim Consulta As String = "Select * from graduacion order by graduacion.graduacion_id asc"

        'Consulta += ""
        'Consulta += " Select * from graduacion order by graduacion.graduacion_id asc"

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "graduacion")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


    'recupero categoria segun un orden especifico, lo uso para emitir los listados ordenados
    Public Function obtener_categoria_x_orden(ByVal sexo As String, ByVal edadinicial As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        sexo = "'" + sexo.ToUpper + "'"
        edadinicial = "'" + edadinicial + "'"
        'evento_id = "'" + evento_id + "'"
        'categoria_tipo = "'" + categoria_tipo + "'"


        Dim Consulta As String = "select * from categoria where (categoria_tipo = 'Lucha' or categoria_tipo = 'Forma') and categoria_sexo = " + sexo + " and categoria_edadinicial = " + edadinicial
        Consulta += " order by convert(int,categoria.categoria_edadinicial), CONVERT(int,categoria.categoria_gradinicial) asc, categoria.categoria_tipo DESC"


        'Consulta += ""
        'Consulta += " Select * from graduacion order by graduacion.graduacion_id asc"

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "categorias")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function




    Public Function llave_obtenerinfo(ByVal llave_id As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        llave_id = "'" + llave_id + "'"

        Dim Consulta As String = "select * from Llave where Llave_id = " + llave_id

        'Consulta += ""
        'Consulta += " Select * from graduacion order by graduacion.graduacion_id asc"

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "Llave")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function










    Public Function Llave_item_alta(ByVal Llave_item_usuario_id As Integer, ByVal Llave_item_PIzq As Integer,
ByVal Llave_item_PDerecho As Integer,
ByVal Llave_item_nivel As Integer,
ByVal Llave_item_Numero As Integer, ByVal estado As String, ByVal Llave_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_usuario_id", Llave_item_usuario_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_PIzq", Llave_item_PIzq))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_PDerecho", Llave_item_PDerecho))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_nivel", Llave_item_nivel))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_Numero", Llave_item_Numero))
        comando.Parameters.Add(New OleDb.OleDbParameter("@estado", estado))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_id", Llave_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_item_actualizar(ByVal LLave_item_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_actualizar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LLave_item_id", LLave_item_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_item_actualizar_progreso(ByVal LLave_item_id As Integer, ByVal Llave_item_usuario_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_actualizar_progreso", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LLave_item_id", LLave_item_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_usuario_id", Llave_item_usuario_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_item_actualizar_raiz(ByVal LLave_item_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_actualizar_raiz", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LLave_item_id", LLave_item_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function


    Public Function Llave_item_actualizar_usuario(ByVal LLave_item_id As Integer, ByVal Llave_item_usuario_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_actualizar_usuario", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LLave_item_id", LLave_item_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_usuario_id", Llave_item_usuario_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_item_consulta(ByVal Llave_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_consulta", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_id", Llave_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "Usuario")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_item_consulta_nivel(ByVal Llave_item_nivel As Integer, ByVal Llave_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_consulta_nivel", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_nivel", Llave_item_nivel))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_id", Llave_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "Usuario")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_item_borrar_hoja(ByVal LLave_item_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_borrar_hoja", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LLave_item_id", LLave_item_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_item_quitar_enlace(ByVal LLave_item_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_quitar_enlace", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LLave_item_id", LLave_item_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "Usuario")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function LLave_obtener_inscriptos(ByVal evento_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LLave_obtener_inscriptos", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))

        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function


    Public Function Llave_obtener_llaves_generadas_info(ByVal evento_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_obtener_llaves_generadas_info", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))


        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function


    Public Function Llave_obtener_llaves_generadas_infoArea(ByVal evento_id As Integer, ByVal area_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_obtener_llaves_generadas_infoArea", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@area_id", area_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function LLave_obtener_inscriptos_filtrados(ByVal evento_id As Integer, ByVal categoria_tipo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LLave_obtener_inscriptos_filtrados", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@categoria_tipo", categoria_tipo))

        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function


    'trae todos los inscriptos, sin importar si estan en llave o no
    Public Function LLave_obtener_inscriptos_filtrados2(ByVal evento_id As String, ByVal categoria_tipo As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        evento_id = "'" + evento_id + "'"
        categoria_tipo = "'" + categoria_tipo + "'"


        Dim Consulta As String = ""

        Consulta += "select evento.evento_id, 
evento.evento_descripcion,
inscripcion.inscripcion_id,
inscripciones_x_torneo.torneo_id ,
inscripciones_x_torneo.categoria_id,
categoria.categoria_tipo,
categoria.categoria_gradinicial,
categoria.categoria_gradfinal,
categoria.categoria_edadinicial,
categoria.categoria_edadfinal,
categoria.categoria_peso_inical,
categoria.categoria_peso_Final,
categoria.categoria_sexo  
from inscripcion inner join inscripciones_x_torneo on inscripcion.inscripcion_id = inscripciones_x_torneo.inscripcion_id
inner join categoria on inscripciones_x_torneo.categoria_id=categoria.categoria_id
inner join evento on inscripcion.evento_id=evento.evento_id 
where inscripcion.evento_id= " + evento_id + " and categoria.categoria_tipo= " + categoria_tipo + " order by convert(int,categoria.categoria_edadinicial), CONVERT(int,categoria.categoria_gradinicial) asc "
        Consulta += " Select * from graduacion order by graduacion.graduacion_id asc"

        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "inscriptos")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function


    Public Function LLave_obtener_inscriptos_sin_llave(ByVal evento_id As Integer, ByVal categoria_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LLave_obtener_inscriptos_sin_llave", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@categoria_id", categoria_id))

        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    'trae todos los inscriptos esten o no en llave
    Public Function LLave_obtener_inscriptos_sin_llave2(ByVal evento_id As String, ByVal categoria_id As String) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        evento_id = "'" + evento_id + "'"
        categoria_id = "'" + categoria_id + "'"


        Dim Consulta As String = ""

        Consulta += "select inscripcion.usuario_id as 'ID',
usuario.usuario_doc as 'dni',
usuario.usuario_apellido+' '+usuario_nombre as 'ApellidoyNombre',
institucion.institucion_abreviacion as 'Institucion_abreviatura',
institucion.institucion_descripcion as 'Institucion',
provincia.provincia_desc as 'Provincia',
inscripcion.inscripcion_instructor_id as 'instructor_id',
inscripcion.inscripcion_peso as 'Peso', inscripcion.inscripcion_id, inscripciones_x_torneo.torneo_id 
from inscripcion inner join inscripciones_x_torneo on inscripcion.inscripcion_id = inscripciones_x_torneo.inscripcion_id
inner join categoria on inscripciones_x_torneo.categoria_id=categoria.categoria_id
inner join evento on inscripcion.evento_id=evento.evento_id
inner join usuario on inscripcion.usuario_id= usuario.usuario_id
inner join alumnos_x_instructor on usuario.usuario_id = alumnos_x_instructor.usuario_id
inner join institucion on alumnos_x_instructor.institucion_id= institucion.institucion_id
inner join provincia on institucion.provincia_id = provincia.provincia_id
where inscripcion.evento_id= " + evento_id + " and inscripciones_x_torneo.categoria_id= " + categoria_id + " order by ApellidoyNombre asc"


        'Consulta += " where Recorridos.Habilitada = '1' and Puntos.Fecha =" + Fecha + " order by Recorridos.Idrecorrido asc"

        Dim DA As New OleDbDataAdapter(Consulta, dbconn)
        Dim ds As New DataSet()
        DA.Fill(ds, "inscriptos")
        dbconn.Close()
        Return ds
        ''''### son las 20:16
    End Function






    'esta rutina me trae todos los inscritos en un determinada categoria y evento.
    Public Function LLave_obtener_inscriptos_categoria(ByVal evento_id As Integer, ByVal categoria_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LLave_obtener_inscriptos_categoria", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@categoria_id", categoria_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_alta(ByVal evento_id As Integer, ByVal categoria_id As Integer, ByVal Llave_cantidad As Integer, ByVal area_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try

        Dim comando As New OleDbCommand("Llave_alta", dbconn)
        comando.CommandType = CommandType.StoredProcedure

        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@categoria_id", categoria_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_cantidad", Llave_cantidad))
        comando.Parameters.Add(New OleDb.OleDbParameter("@area_id", area_id))

        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function



    Public Function LLave_obtener_llavegenerada_etc(ByVal evento_id As Integer, ByVal categoria_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LLave_obtener_llavegenerada_etc", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@categoria_id", categoria_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function LLave_obtener_llavegenerada_etc_2(ByVal Llave_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("LLave_obtener_llavegenerada_etc_2", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_id", Llave_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function



    Public Function llave_eliminar(ByVal llave_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("llave_eliminar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@llave_id", llave_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_deshacer_llave(ByVal usuario_id As Integer, ByVal evento_id As Integer, ByVal categoria_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_deshacer_llave", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@usuario_id", usuario_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@categoria_id", categoria_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function


    Public Function Llave_Verificar_existencia(ByVal cat_id As Integer, ByVal evento_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_Verificar_existencia", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@cat_id", cat_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@evento_id", evento_id))
        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    '------------------------------------------------------------------------------------------------
    'CHOCO fecha: 31-08-2022 16.49hrs Choco. para form Lllave_generar.aspx
    Public Function Llave_para_reordenar(ByVal Llave_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_para_reordenar", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_id", Llave_id))

        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    Public Function Llave_item_actualizar_orden(ByVal LLave_item_id As Integer, ByVal Llave_item_usuario_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llave_item_actualizar_orden", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@LLave_item_id", LLave_item_id))
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_item_usuario_id", Llave_item_usuario_id))

        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function

    '------------------------------------------------------------------------------------------------
    'CHOCO fecha: 31-08-2022 16.49hrs Choco. para form Lllave_generar.aspx
    '-------------------------------------------------------------------------------------------------
    Public Function Llaves_Finalizadas_filtro(ByVal Llave_id As Integer) As DataSet
        Try
            dbconn.Open()
        Catch ex As Exception
        End Try
        Dim comando As New OleDbCommand("Llaves_Finalizadas_filtro", dbconn)
        comando.CommandType = CommandType.StoredProcedure
        comando.Parameters.Add(New OleDb.OleDbParameter("@Llave_id", Llave_id))


        Dim ds_JE As New DataSet()
        Dim da_JE As New OleDbDataAdapter(comando)
        da_JE.Fill(ds_JE, "llave")
        dbconn.Close()
        Return ds_JE
    End Function
    '---------------------------------------------------------------------------------------------------


End Class
