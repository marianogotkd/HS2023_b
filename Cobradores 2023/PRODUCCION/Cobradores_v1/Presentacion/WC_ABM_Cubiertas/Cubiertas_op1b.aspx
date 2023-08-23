<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Cubiertas_op1b.aspx.vb" Inherits="Presentacion.Cubiertas_op1b" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>

    //funcion que reconoce teclas para ir a los botones retroceso, baja y graba
    function tecla_op(e) {
        var keycode = e.keyCode;
        ///ESC RETROCEDE
        if (keycode == '27') {
            e.preventDefault();
            document.getElementsByTagName('button')[0].focus();
            document.getElementsByTagName('button')[0].click();

        }
        ///se anula el enter
        if (keycode == '13') {
            e.preventDefault();

        }

        ///F4 ELIMINA
        if (keycode == '115') {
            e.preventDefault();
            document.getElementsByTagName('button')[1].focus();
            document.getElementsByTagName('button')[1].click();
        }

        //F8 GRABA
        if (keycode == '119') {
            e.preventDefault();
            document.getElementsByTagName('button')[2].focus();
            document.getElementsByTagName('button')[2].click();
        }
    }



    //funcion que reconoce teclas para ir a los botones retroceso, baja y graba
    function tecla_op_botones(e) {
        var keycode = e.keyCode;
        ///ESC RETROCEDE
        if (keycode == '27') {
            e.preventDefault();
            document.getElementsByTagName('button')[0].focus();
            document.getElementsByTagName('button')[0].click();

        }
        //        ///no voy a anular el ENTER
        //        if (keycode == '13') {
        //            e.preventDefault();
        //        }

        ///F4 ELIMINA
        if (keycode == '115') {
            e.preventDefault();
            document.getElementsByTagName('button')[1].focus();
            document.getElementsByTagName('button')[1].click();
        }

        //F8 GRABA
        if (keycode == '119') {
            e.preventDefault();
            document.getElementsByTagName('button')[2].focus();
            document.getElementsByTagName('button')[2].click();
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
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <div class="card card-primary">
        <div class="card-header">
                <h3 class="card-title">A.B.M. CUBIERTAS - INDIVIDUAL DIVIDE EN EL MISMO CLIENTE.</h3>
        </div>
    <form role="form">
    <div class="card-body">
    <div class="container-fluid">
    <div class="row justify-content-center">
    <div class="col-lg-12">
    <div class="card">

    <div class="card-body">
      <div class="form-group">
        <div class="row justify-content-center">
          <div class="col-md-6">
            <label for="Label_info_a">TOME EN CUENTA QUE DEJAR EN CERO "0" ALGUN VALOR IMPLICA PASAR TODO A CUBIERTA</label>
            <label for="Label_info_b">SI NO DESEA TOMAR ALGUNA CIFRA COLOCAR TODOS NUEVES "99999.99" O DAR DE BAJA EL REGISTRO</label>
          </div>
        </div>
      </div>

        <div class="form-group">
                <div class="row justify-content-center">
                    <div class="col-md-2">
                      <asp:HiddenField ID="HF_IdCubCliente" runat="server" />      
                      <asp:HiddenField ID="HF_cliente_id" runat="server" />
                            <label for="Label_cliente_id">Cliente:</label>
                            <asp:TextBox ID="Txt_cliente_codigo" runat="server" 
                                placeholder="Ingrese codigo..." class="form-control" 
                                onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);"   
                                MaxLength="4"></asp:TextBox>
                            
                            
                    </div>
                    <div class="col-md-4">
                            <label for="Label_cliente_nomb">Nombre:</label>
                            <asp:TextBox ID="Txt_cliente_nombre" runat="server" placeholder="" class="form-control" MaxLength="50" onkeydown="tecla_op(event);" ></asp:TextBox>
                    </div>
                </div>

        </div>


                
                 
      <div class="form-group">
                <div class="row justify-content-center">
                  
                  <div class="col-md-4">
                <label for="Lb_1">Excedente 1 cifra:</label>
                            <asp:TextBox ID="Txt_exc1" runat="server" placeholder="0,00" class="form-control"  MaxLength="8" onkeydown="tecla_op(event);" onkeypress="return validateDecimalNegativoKeyPress(this, event);" ></asp:TextBox>            
                            
                            
                    </div>
                    
                </div>
        <div class="row justify-content-center">
                    <div class="col-md-4">
                <label for="Lb_1">Excedente 2 cifras:</label>
                            <asp:TextBox ID="Txt_exc2" runat="server" placeholder="0,00" class="form-control"  MaxLength="8" onkeydown="tecla_op(event);" onkeypress="return validateDecimalNegativoKeyPress(this, event);" ></asp:TextBox>            
                            
                            
                    </div>
                    
                </div>
        <div class="row justify-content-center">
                    <div class="col-md-4">
                <label for="Lb_1">Excedente 3 cifras:</label>
                            <asp:TextBox ID="Txt_exc3" runat="server" placeholder="0,00" class="form-control"  MaxLength="8" onkeydown="tecla_op(event);" onkeypress="return validateDecimalNegativoKeyPress(this, event);" ></asp:TextBox>            
                            
                            
                    </div>
                    
                </div>
        <div class="row justify-content-center">
                    <div class="col-md-4">
                    <label for="Lb_1">Excedente 4 cifras:</label>
                    <asp:TextBox ID="Txt_exc4" runat="server" placeholder="0,00" class="form-control"  MaxLength="8" onkeydown="tecla_op(event);" onkeypress="return validateDecimalNegativoKeyPress(this, event);"></asp:TextBox>                  
                            
                            
                    </div>
                    
                </div>
        <div class="row justify-content-center">
                    <div class="col-md-2">
                <label for="Lb_1">Considerar:</label>
                
                <asp:DropDownList ID="DropDownList_considerar" runat="server" class="form-control" onkeydown="tecla_op(event);">
                            <asp:ListItem Selected="True" Value="0">0</asp:ListItem>
                            <asp:ListItem Value="1">1</asp:ListItem>
                            </asp:DropDownList>
                      
                      
                            
                            
                    </div>
          <div class="col-md-4">
                  
            </div>
                    
                </div>
        <div class="row justify-content-center">
          <div class="col-md-6">
                <small id="Small1" class="form-text text-muted">0 = NO CONSIDERA SUCURSALES > 2 NI PID/2.</small>
                <small id="Small2" class="form-text text-muted">1 = CONSIDERA TODAS LAS SUCURSALES Y PID/2</small>
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
                            <button type="button" Class="btn btn-primary" data-toggle="modal" data-target="#Mdl_baja" onkeydown="tecla_op_botones(event);">F4 = DAR DE BAJA</button>
                            &nbsp;</div>  
            
            
                      <div class="form-group">
                                <button type="button" Class="btn btn-primary" id = "BOTON_GRABA" runat="server" onkeydown="tecla_op_botones(event);">F8 = GRABA</button>
        
                            </div>
            
            
            
                  
            
         </div>

        </div>
        

</div>

<div class="modal fade" id="modal-graba">
        <div class="modal-dialog">
          <div class="modal-content bg-primary">
            <div class="modal-header">
              <h4 class="modal-title">Graba</h4>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span></button>
            </div>
            <div class="modal-body">
              <p>¿Confirma la operación?&hellip;</p>
            </div>
            <div class="modal-footer justify-content-between">
              <button type="button"    class="btn btn-outline-light" data-dismiss="modal">Cancelar</button>
              <button type="button" id="btn_graba_modal" runat="server"  class="btn btn-outline-light" data-dismiss="modal">Confirmar</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->

<div class="modal fade" id="modal-baja" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
          <div class="modal-content bg-primary">
            <div class="modal-header">
              <h4 class="modal-title">Dar de Baja</h4>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span></button>
            </div>
            <div class="modal-body">
              <p>¿Confirma la operación?&hellip;</p>
            </div>
            <div class="modal-footer justify-content-between">
              <button type="button"    class="btn btn-outline-light" data-dismiss="modal">Cancelar</button>
              <button type="button" id="btn_baja_modal" runat="server"  class="btn btn-outline-light" data-dismiss="modal">Confirmar</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


<!-- Modal BAJA CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_baja" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H1">Dar de Baja</h5>
        <button type="button" id="btn_baja_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        ¿Confirma la operación?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_baja_mdl_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_baja_mdll" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>

<%--Modal MENSAJE OK GRABADO--%>
<div class="modal fade" id="modal-sm_OKGRABADO" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Graba</h4>
              <button type="button" id="btn_graba_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Se guardo correctamente!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


      <%--Modal MENSAJE OK ERROR ELIMINAR--%>
<div class="modal fade" id="modal_sn_okerror_eliminar" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_error_eliminar_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>No se puede eliminar, no existe registro!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_erroreliminar" runat="server" class="btn btn-primary" data-dismiss="modal" onfocus="true">OK</button>
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
