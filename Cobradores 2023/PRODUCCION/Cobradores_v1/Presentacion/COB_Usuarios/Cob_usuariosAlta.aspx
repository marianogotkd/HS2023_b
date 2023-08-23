<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Cob_usuariosAlta.aspx.vb" Inherits="Presentacion.Cob_usuariosAlta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <div class="card card-primary">
        <div class="card-header">
                <h3 class="card-title">USUARIOS A/B/M</h3>
        </div>
    <form role="form">
    <div class="card-body">
    <div class="container-fluid">
    <div class="row justify-content-center">
    <div class="col-lg-12">
    <div class="card">
    <div class="card-body">
      <div id="Div2" class="row justify-content-center" visible="True" runat="server">
        <div class="col-md-12">
          <div class="form-group">
            <div class="row justify-content-center">
              <div class="col-md-6">
                <div class="card card-secondary">
                  <div class="card-header">
                    <h3 class="card-title">DATOS PERSONALES.</h3>
                  </div>
                  <form role="form">
                      <div class="card-body">
                        <asp:HiddenField ID="HF_USU_ID" runat="server" />
                        <label for="Lb_Dni">Dni:</label>
                        <asp:TextBox ID="TxtDni" runat="server" placeholder="Ingrese Dni..." class="form-control" MaxLength="10" onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);"></asp:TextBox>
                        <asp:Label ID="lb_error_Dni" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                        <small id="SmallError_Dni" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                        <br />
                        <label for="Lb_Apellido">Apellido:</label>
                        <asp:TextBox ID="TxtApellido" runat="server" placeholder="Ingrese Apellido..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox>
                        <asp:Label ID="lb_error_Apellido" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                        <small id="SmallError_Apellido" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                        <br />
                        <label for="Lb_Nombre">Nombre:</label>
                        <asp:TextBox ID="TxtNombre" runat="server" placeholder="Ingrese Nombre..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox>
                        <asp:Label ID="lb_error_Nombre" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                        <small id="SmallError_Nombre" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                        <br />
                        <label for="Lb_Direccion">Direccion:</label>
                        <asp:TextBox ID="TxtDireccion" runat="server" placeholder="Ingrese Direccion..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);" ></asp:TextBox>
                        <asp:Label ID="lb_error_Direccion" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                        <small id="SmallError_Direccion" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                        <br />
                        <label for="Lb_Telefono">Telefono:</label>
                        <asp:TextBox ID="TxtTelefono" runat="server" placeholder="Ingrese Telefono..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox>
                        <asp:Label ID="Lb_error_Telefono" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                        <small id="SmallError_Telefono" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                        <br />
                        <label for="Lb_Mail">Mail:</label>
                        <asp:TextBox ID="TxtMail" runat="server" placeholder="Ingrese Mail..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox>
                        <asp:Label ID="Lb_error_Mail" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                        <small id="SmallError_Mail" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                      </div>
                  </form>
                </div>
              </div>
              <div class="col-md-6">
                    <div class="card card-secondary">
                      <div class="card-header">
                          <h3 class="card-title">PERMISOS Y LOGIN.</h3>
                      </div>
                      <%--<form role="form">--%>
                          <div class="card-body">
                                <label for="Lb_Usuario">Usuario:</label>
                                <asp:TextBox ID="TxtUsuario" runat="server" placeholder="Ingrese usuario..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox>
                                <asp:Label ID="lb_error_Usuario" runat="server" ForeColor="Red" Text="*" 
                                      Visible="False"></asp:Label>
                                <small id="SmallError_Usuario" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                                <br />
                                <label for="Lb_Contrasena">Password:</label>
                                <asp:TextBox ID="TxtContrasena" runat="server" placeholder="Ingrese password..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox> <%--TextMode="Password"--%>
                                <asp:Label ID="lb_error_Contrasena" runat="server" ForeColor="Red" Text="*" 
                                      Visible="False"></asp:Label>
                                <small id="SmallError_Contrasena" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>
                                <br />
                                <label for="Lb_Jerarquia">Jerarquia:</label>
                                <asp:DropDownList ID="DropDLJerarquia" runat="server" class="form-control" onkeydown="tecla_op(event);">
                                  <asp:ListItem Selected="True" Value="2">Administrador</asp:ListItem>
                                  <asp:ListItem Value="3">Cobrador</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lb_error_Jerarquia" runat="server" ForeColor="Red" Text="*error, seleccione Jerarquia" 
                                    Visible="False"></asp:Label>
                                <small id="SmallError_Jerarquia" runat="server" visible=false class="form-text text-muted">Error, seleccione Jerarquia.</small>
                                <br />
                                <label for="Lb_Observacion">Observacion:</label>
                                <asp:TextBox ID="TxtObservacion" runat="server" placeholder="Ingrese Observacion..." class="form-control" onkeydown="tecla_op(event);" ></asp:TextBox>
                          </div>
                     <%-- </form>--%>
                    </div>
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
        Confirma la operacion?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_baja_mdl_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_baja_mdll" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>

<%--Modal MENSAJE OK GRABADO CENTRADO EN PANTALLA--%>
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


      <%--Modal MENSAJE OK ERROR ELIMINAR CENTRADO EN PANTALLA--%>
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


  <!-- Modal PREGUNTA ALTA MODIF CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_altamodif" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H2">Grabar</h5>
        <button type="button" id="btn_altamodif_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        Confirma la operacion?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_altamodif_mdl_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_altamodif_mdll" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>


  <%--Modal MENSAJE OK ERRORES VARIOS CENTRADO EN PANTALLA--%>
<div class="modal fade" id="Mdl_errores" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_errores_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">

              <asp:Label ID="Mdl_label_errores" runat="server" Text="AQUI MSJ"></asp:Label>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_errores_ok" runat="server" class="btn btn-primary" data-dismiss="modal" onfocus="true">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


</script>
  
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
