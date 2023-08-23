<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="ConsultarModificar_xcargas_a.aspx.vb" Inherits="Presentacion.ConsultarModificar_xcargas_a" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        //funcion que reconoce teclas para ir a los botones retroceso, baja y graba
        function tecla_op(e) {
            var keycode = e.keyCode;
            
            ///se anula el enter y va al boton de modificar
            if (keycode == '13') {
                e.preventDefault();
                document.getElementsByTagName('button')[1].focus();
                document.getElementsByTagName('button')[1].click();
            }

            ///F8 continuar 
            if (keycode == '119') {
                e.preventDefault();
                document.getElementsByTagName('button')[1].focus();
                document.getElementsByTagName('button')[1].click();
            }

            ///ESC RETROCEDE
            if (keycode == '27') {
                e.preventDefault();
                document.getElementsByTagName('button')[0].focus();
                document.getElementsByTagName('button')[0].click();

            }
        
        
        }



        //funcion que reconoce teclas para ir a los botones retroceso, baja y graba
        function tecla_op_botones(e) {
            var keycode = e.keyCode;
            
            ///no anulo el ENTER
            //            if (keycode == '13') {
            //                e.preventDefault();
            //            }

            ///F8
            if (keycode == '119') {
                e.preventDefault();
                document.getElementsByTagName('button')[1].focus();
                document.getElementsByTagName('button')[1].click();
            }

            ///ESC RETROCEDE
            if (keycode == '27') {
                e.preventDefault();
                document.getElementsByTagName('button')[0].focus();
                document.getElementsByTagName('button')[0].click();

            }

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
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
                            EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200">
</asp:ScriptManager>

  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<div class="card card-primary">
<div class="card-header">
                <h3 class="card-title">VISUALIZACION/MODIFICACION DE REGISTROS</h3>
</div>
<form role="form">
<div class="card-body">
    <div class="container-fluid"> <%--<div align="center">--%>
    <div class="row justify-content-center" >   <%--class="row"--%>
        <div class="col-lg-12">
            <div class="card">
                <div class="card-body">
                    
                  <div class="form-group">
                    <div class="row justify-content-center">
                          <div class="col-md-4">
                            <asp:HiddenField ID="HF_parametro_id" runat="server" />
                            <asp:Label ID="Label2" runat="server" Text="FECHA:"></asp:Label>
                            <asp:Label ID="Label_fecha" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="HF_fecha" runat="server" />
                          
                              <br />
                          
                            <asp:Label ID="Label3" runat="server" Text="DIA:"></asp:Label>
                            <asp:Label ID="Label_dia" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="HF_dia_id" runat="server" />
                          
                          </div>
                          
                    </div>
                        
                    </div>

                    
                  
                  <div class="form-group">
                    <div class="row justify-content-center">
                          <div class="col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="SELECCIONE EL CLIENTE QUE DESEA VISUALIZAR O MODIFICAR:"></asp:Label>  
                          </div>

                    </div>
                        
                    </div>

                  <div class="form-group">
          
                        <div class="row justify-content-center">
                          <div class="col-md-4">
                            <asp:Label ID="Label_cliente" runat="server" Text="CLIENTE:"></asp:Label>
                                <asp:TextBox ID="Txt_ClienteCod" runat="server" 
                                              placeholder="Ingrese codigo..." class="form-control" 
                                              onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);" 
                                              MaxLength="4"></asp:TextBox>
                          </div>
                          
                            
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

<div class="card-footer">
<div class="row justify-content-center" >
<div class="row align-items-center">
<div class="form-group">
<button type="submit" UseSubmitBehavior="false" class="btn btn-primary" runat="server" id="btn_retroceder" onkeydown="tecla_op_botones(event);">ESC = RETROCEDE</button>
    &nbsp;</div>
<div class="form-group">
                    <button type="button" id="btn_modificar" runat="server" class="btn btn-primary" onkeydown="tecla_op_botones(event);">
                          F8 = GRABA
                        </button>
                    
</div>


</div>
</div>
</div>

<%--Modal MENSAJE ERROR OK 1--%>
<div class="modal fade" id="modal-ok_error" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_error_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Error, primero debe iniciar dia!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_error" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


  <%--Modal MENSAJE ERRORES VALIDACION 1--%>
<div class="modal fade" id="modal-ErrorValidacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="Btn_ErrorValidacionClose" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <asp:Label ID="Label_ErrorValidacion" runat="server" Text=""></asp:Label>
              
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="Btn_ErrorValidacionOk" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
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
