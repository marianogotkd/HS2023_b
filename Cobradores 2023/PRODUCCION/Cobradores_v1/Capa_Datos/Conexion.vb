Imports System.Configuration
Public Class Conexion
    'BD MARTIN CHOCO-RYZEN - hamachi
    'Public dbconn As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Password=123choco;Persist Security Info=True;User ID=choco;Initial Catalog=Martin;Data Source=25.112.197.145\SQLEXPRESS_CHOK")

    ''LOCAL
    ' Public dbconn As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Password=123choco;Persist Security Info=True;User ID=choco;Initial Catalog=WebCentral;Data Source=(local)")

    ''SERVIDOR
    'Public dbconn As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WebCentral;Data Source=SRVSQL-CENTRAL;Password=webcentral123; user ID=webcentral")

    'BD WEB-CENTRAL CHOCO-RYZEN -. hamachi
    'Public dbconn As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Password=123choco;Persist Security Info=True;User ID=choco;Initial Catalog=WebCentral_clie;Data Source=25.112.197.145\SQLEXPRESS_CHOK")

    '-----------------------PARA SERVIDOR DEL CLIENTE------------------------------------------------------------------------------------
    ''SERVIDOR con WinNT
    Public dbconn As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Cobradores;Data Source=localhost")
    Public dbconnMaster As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WebCentral_copy;Data Source=localhost")

    '---------------------------------------HAMERSOFT DESARROLLO--------------------------------------------------------------------------
    'bd WEB-Cobradores CHOCO-RYZEN -.local 18-05-2023
    'Public dbconn As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Password=123choco;Persist Security Info=True;User ID=CHOCO;Initial Catalog=Cobradores;Data Source=DESKTOP-IPJ62B9\SQLEXPRESS_CHOK")

    'cadena MASTER
    'Public dbconnMaster As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Password=123choco;Persist Security Info=True;User ID=choco;Initial Catalog=WebCentral_copy;Data Source=DESKTOP-IPJ62B9\SQLEXPRESS_CHOK")

    'NOTE Mariano Dell

    'Public dbconn As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WebCentral;Data Source=STFARGAZ203-L\SQLEXPRESS")
    'Public dbconnMaster As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WebCentral_copy;Data Source=STFARGAZ203-L\SQLEXPRESS")

    '-------------------------------------------------------------------------------------------------------------------------------------



End Class
