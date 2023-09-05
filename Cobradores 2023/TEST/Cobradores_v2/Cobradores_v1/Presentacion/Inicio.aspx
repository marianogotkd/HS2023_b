<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Inicio.aspx.vb" Inherits="Presentacion.Inicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>

    //funcion que reconoce teclas para ir a los botones retroceso, baja y graba
    function tecla_op_botones(e) {
        var keycode = e.keyCode;
        ///ENTER
        if (keycode == '13') {
            e.preventDefault();
            document.getElementsByTagName('button')[0].focus();
            document.getElementsByTagName('button')[0].click();

        }

//        ///F2 
//        if (keycode == '113') {
//            e.preventDefault();
//            document.getElementsByTagName('button')[0].focus();
//            document.getElementsByTagName('button')[0].click();
//        }

    }

    //funcion para seleccionar todo le contenido de un textbox cuando se pone el foco sobre el control. se agrega como atributo en el codebehind
    function seleccionarTexto(obj) {
        if (obj != null) {
            obj.select();
        }
    }


</script>
 

 



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<%--<asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/descarga (1).jpg" />--%>

<asp:ScriptManager ID="ScriptManager1" runat="server" 
                            EnableScriptGlobalization="True">
</asp:ScriptManager>             

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>



<div class="card card-primary">


<div class="card-header">
<h3 class="card-title">MENU PRINCIPAL</h3>
</div>


<form role="form">
<div class="card-body">
<div class="container-fluid">   <%--align="center"--%>
<div class="row justify-content-center" >   <%--class="row "--%>
<div class="col-lg-12">
<div class="card">
<div class="card-body">
    <div class="form-group">
                <div class="row justify-content-start "> <%--row justify-content-center--%>
            <div class="col-md-6" id="Menu_column1" runat="server" visible="true">  <%--class="col-4"--%>
              
              
              <div class="form-group" id ="Div_Op1" runat="server" visible="false">
              1 - USUARIOS.
              </div>
              <div class="form-group" id="Div_Op2" runat="server" visible="false">
              2 - SECTOR A/B/M. <%--TARIFAS A/B/M/.--%>
              </div>
              <div class="form-group" id="Div_Op3" runat="server" visible="false" >
              3 - LOCAL A/B/M.
              </div>
              <div class="form-group" id="Div_Op4" runat="server" visible="false">
              4 - CLIENTES A/B/M.
              </div>
              <div class="form-group" id="Div_Op5" runat="server" visible="false">
              5 - COBRADORES - LISTADO DE LOCALES A COBRAR.
              </div>
              <div class="form-group" id="Div_Op6" runat="server" visible="false">
              6 - ADMIN - PROC.DIARIO.
              </div>
              <div id="OCULTAR" runat="server" visible="false" >
                <div class="form-group" id="Div_Op7" runat="server" visible="false">
              7 - .
              </div>
              <div class="form-group" id="Div_Op8" runat="server" visible="false">
              8 - .
              </div>
              <div class="form-group" id="Div_Op9" runat="server" visible="false" >
              9 - .
              </div>
              <div class="form-group" id="Div_Op10" runat="server" visible="false">
              10 - .
              </div>
              <div class="form-group" id="Div_Op11" runat="server" visible="false">
              11 - .
              </div>
              <div class="form-group" id="Div_Op99" runat="server" visible="false">
              99 - .
              </div>

              </div>
              
              
            </div>
            <div class="col-md-6"> <%--class="col-4"--%>
              <div id="OCULTO2" runat="server" visible="false">
                <div class="form-group" id="Div_OpA" runat="server" visible="false" >
              A - .
              </div>
              <div class="form-group" id="Div_OpB" runat="server" visible="false">
              B - .
              </div>
              <div class="form-group" id="Div_OpC" runat="server" visible="false">
              C - .
              </div>
              <div class="form-group" id="Div_OpD" runat="server" visible="false">
              D - .
              </div>
              <div class="form-group" id="Div_OpE" runat="server" visible="false">
              E - .
              </div>
              <div class="form-group" id="Div_OpF" runat="server" visible="false">
              F - .
              </div>
              <div class="form-group" id="Div_OpG" runat="server" visible="false">
              G - .
              </div>
              <div class="form-group" id="Div_OpH" runat="server" visible="false">
              H - .
              </div>
              <div class="form-group" id="Div_OpI" runat="server" visible="false">
              I - .
              </div>
              <div class="form-group" id="Div_OpJ" runat="server" visible="false">
              J - .
              </div>
              <div class="form-group" id="Div_OpZ" runat="server" visible="false">
              Z - .
              </div>
              </div>
              
            </div>
        </div>
    </div>


    <div class="form-group">

            <div class="row justify-content-start">
            
            <div class="col-md-6">
                        <asp:TextBox ID="txt_opcion" runat="server" placeholder="ingrese opcion..." 
            onkeydown="tecla_op_botones(event);" MaxLength="2" Width="150px"></asp:TextBox>
        
                        &nbsp;
        
        <button type="button" id="btn_opcion" runat="server" class="btn btn-primary"  onkeydown="tecla_op_botones(event);">
                          IR
        </button>    
            </div>
            
            </div>
            <%--class="btn btn-primary"--%>
    </div>
    <div class="form-group">
            <div class="row justify-content-center">

            </div>
    </div>

        

    

</div>





</div>
</div>

</div>
</div>
</div>
</form>
</div>


<%--MODAL MSJ CENTRADO - ERROR OPCION--%>
<div class="modal fade" id="modal-sm_error" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error!</h4>
              <button type="button" id="btn_close_error" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>OPCION INCORRECTA!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_error" runat="server" tabindex="1" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


</ContentTemplate>
</asp:UpdatePanel>


</asp:Content>
