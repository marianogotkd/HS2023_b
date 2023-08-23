<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Cob_tarifasAlta.aspx.vb" Inherits="Presentacion.Cob_tarifasAlta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <div class="card card-primary">
        <div class="card-header">
                <h3 class="card-title">TARIFAS A/B/M</h3>
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
        <div class="col-md-4">
                <label for="Lb_Descripcion">Descripcion:</label>
                <asp:TextBox ID="TxtDescripcion" runat="server" placeholder="Ingrese descripcion..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox>
                <asp:Label ID="lb_error_Descripcion" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Descripcion" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
        </div>
        <div class="col-md-4">
<%--                <label for="Lb_Contrasena">Password:</label>
                <asp:TextBox ID="TxtContrasena" runat="server" placeholder="Ingrese password..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);" TextMode="Password"></asp:TextBox>
                <asp:Label ID="lb_error_Contrasena" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Contrasena" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>--%>
        </div>
        </div>
        </div>  
      
      <div class="form-group">
                <div class="row justify-content-center">
                    <div class="col-md-4">
                <label for="Lb_Tipo">Tipo:</label>
                <asp:DropDownList ID="DropDLTipo" runat="server" class="form-control" onkeydown="tecla_op(event);">
                    <asp:ListItem Selected="True" Value="Unica">Unica</asp:ListItem>
                    <asp:ListItem Value="Periodica">Periodica</asp:ListItem>
                        </asp:DropDownList>
                <asp:Label ID="lb_error_tipo" runat="server" ForeColor="Red" Text="*error, seleccione Jerarquia" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Tipo" runat="server" visible=false class="form-text text-muted">Error, seleccione Tipo.</small>
                    </div>
                    <div class="col-md-4">
                            <%--<label for="Label_cliente_id">DNI:</label>
                            <asp:TextBox ID="Txt_dni" runat="server" MaxLength="9" placeholder="Ingrese DNI..." class="form-control" CausesValidation="True" onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);"></asp:TextBox>
                            <asp:Label ID="lb_error_dni" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                            <small id="Small2" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>--%>
                    </div>
                </div>

        </div>

      <div class="form-group">
                <div class="row justify-content-center">
                    <div class="col-md-4">
                <label for="Lb_Dias">Dias:</label>
                <asp:TextBox ID="TxtDias" runat="server" placeholder="Ingrese Dias..." class="form-control" MaxLength="2" onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);"></asp:TextBox>
                <asp:Label ID="lb_error_Dias" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Dias" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                
                    </div>
                    <div class="col-md-4">
                            <%--<label for="Label_cliente_id">DNI:</label>
                            <asp:TextBox ID="Txt_dni" runat="server" MaxLength="9" placeholder="Ingrese DNI..." class="form-control" CausesValidation="True" onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);"></asp:TextBox>
                            <asp:Label ID="lb_error_dni" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                            <small id="Small2" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>--%>
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
                  <button type="submit" UseSubmitBehavior="false" class="btn btn-primary" runat="server" id="btn_retroceder" onkeydown="tecla_op_botones(event);">RETROCEDE</button>
                    &nbsp;</div>
                      
                        <div class="form-group">
                            <button type="button" Class="btn btn-primary" data-toggle="modal" data-target="#Mdl_baja" onkeydown="tecla_op_botones(event);">DAR DE BAJA</button>
                            &nbsp;</div>  
            
            
                      <div class="form-group">
                                <button type="button" Class="btn btn-primary" id = "BOTON_GRABA" runat="server" onkeydown="tecla_op_botones(event);">GRABAR</button>
        
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
              <p>No se puede eliminar, el cliente posee saldo!&hellip;</p>
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
