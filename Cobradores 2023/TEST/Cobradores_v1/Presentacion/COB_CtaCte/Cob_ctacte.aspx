<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Cob_ctacte.aspx.vb" Inherits="Presentacion.Cob_ctacte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div class="card card-primary">
<div class="card-header">
                <h3 class="card-title">CUENTA CORRIENTE</h3>
</div>
<form role="form">
<%--<div class="card-body"> --%>
        <div class="container-fluid">
            <div class="row justify-content-center">
            <div class="col-lg-12">
                   <%-- <div class="card"> tercer card--%>
                            <%--<div class="card-body">--%>
                            <div class="form-group">
                            <div class="row justify-content-center">
                                    <div class="col-md-12"> <%--col-md-4--%>
                                      <asp:Label ID="Lb_ctacte" runat="server" Text="CTACTE:"></asp:Label>
                                      <asp:HiddenField ID="HF_CLIE_ID" runat="server" />
                                      <asp:HiddenField ID="HF_CTACTE_ID" runat="server" />
                                      <asp:HiddenField ID="HF_LOCAL_ID" runat="server" />
                                        <br />
                                      <asp:Label ID="Lb_cliente" runat="server" Text="CLIENTE:"></asp:Label>
                                      
                                        <br />
                                      <asp:Label ID="Lb_ctactesaldo" runat="server" Text="SALDO DEUDOR:$"></asp:Label>
                                      
                                    
                                    <div id ="seccion_locales" runat="server" visible="false">
                        <div class="card-body table-responsive p-0"  onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridViewLocales" runat="server" class="table table-bordered table-hover table-responsive-sm" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                      
                                      <asp:BoundField DataField="codlocal" HeaderText="CODIGO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        
                                      
                                      <asp:BoundField DataField="local" HeaderText="LOCAL" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>     
                                     
                                        
                                    </Columns>
                                </asp:GridView>
                        </div>
                      </div>
                                    
                                    </div>
                             
                            </div>
                            </div>
                            
                            <div id="Div1" class="row justify-content-center" visible="True" runat="server">
                    <div class="col-md-12">
                    <div class="card card-secondary">
                    <div class="card-header">
                      <h3 class="card-title">MOVIMIENTOS:</h3>      
                              <%--<h3 class="card-title">RESUMEN A LA FECHA:</h3>--%>

                              
                    </div>
                    <!-- /.card-header -->
                    <div class="card-body"> 
                      
                      <div id ="secciongrid" runat="server" visible="false">
                        <div class="card-body table-responsive p-0" style="height: 400px"  onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridView1" runat="server" class="table table-bordered table-hover table-responsive-sm" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                        
                                      <asp:BoundField DataField="TARCLIE_ID" HeaderText="TARCLIE_ID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        
                                   <asp:BoundField DataField="CTACTEDET_ID" HeaderText="CTACTEDET_ID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>

                                  <asp:BoundField DataField="LOCAL_Codigo" HeaderText="LOCAL" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      
                                      <asp:BoundField DataField="Fecha" HeaderText="FECHA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>

                                      <asp:BoundField DataField="TARCLIE_desc" HeaderText="TARIFA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      
                                      <asp:BoundField DataField="Debe" HeaderText="DEBE" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>

                                      <asp:BoundField DataField="Haber" HeaderText="HABER" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        
                                     <asp:BoundField DataField="comprobante" HeaderText="COMPROBANTE" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                                                            

                                        
                                        
                                    </Columns>
                                </asp:GridView>
                        </div>
                      </div>
                      
<%--                      <div class="form-check">
                        <asp:CheckBoxList ID="Chkbox_anulartodo" runat="server"></asp:CheckBoxList>
                    <label class="form-check-label" for="exampleCheck1">Anular todo.</label>
                  </div>--%>
                    </div>
                      


                    </div>
                    </div>
                    </div>
                            
                            
                            
                            
                            <%--</div> segundo card--%>
                   <%-- </div> 3er card--%>
            </div>
            </div>
        </div>
<%--</div> fin card body--%>
</form>
</div>

<div class="card-footer">
        <div class="row justify-content-center" >
        

         <div class="row align-items-center">
            
                <div class="form-group">
                  <button type="submit" UseSubmitBehavior="false" class="btn btn-primary" runat="server" id="btn_retroceder" onkeydown="tecla_op_botones(event);">RETROCEDE</button>
                    &nbsp;
                    

        
        
                    </div>

           <div class="form-group">
                                <button type="button" Class="btn btn-primary" id = "BOTON_GRABA" runat="server" onkeydown="tecla_op_botones(event);">GRABAR</button>
        
                            </div>

           

                    
                 
                
         </div>

        </div>
        

</div>


<!-- Modal BAJA CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_eliminar" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H1">Eliminar registro</h5>
        <button type="button" id="btn_eliminar1_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        ¿Confirma la operación?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_eliminar_mdl_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_eliminar_mdll" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>


<%--Modal MENSAJE OK ELIMINADO CORRECTAMENTE--%>
<div class="modal fade" id="modal-sm_OKELIMINADO" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Eliminar registro</h4>
              <button type="button" id="btn_ELIMINAR_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Se eliminó correctamente!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_elimnar" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
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
        <h5 class="modal-title" id="H2">Dar de Baja</h5>
        <button type="button" id="Button3" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
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

<%--Modal MENSAJE OK ELIMINADO CORRECTAMENTE--%>
<div class="modal fade" id="modal-sm_OKBAJA" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Dar de baja</h4>
              <button type="button" id="btn_BAJA_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Baja correcta!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_baja" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


<%--Modal MENSAJE OK ERROR_BUSQUEDA--%>
<div class="modal fade" id="modal_error_busqueda" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_close_error_busqueda" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>La busqueda no arrojó resultados!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_error_busqueda" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->

  <%--Modal MENSAJE OK ERROR_BUSQUEDA--%>
<div class="modal fade" id="modal_error_carga" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_close_error_carga" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Consulte al administrador!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="Btn_ok_error_carga" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->

  <!-- Modal GRABA PREGUNTA CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_cobro" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H3">GRABAR</h5>
        <button type="button" id="btn_cobro_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        Confirma la operacion de cobro?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_cobro_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_cobro_confirmar" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>

  <%--Modal MENSAJE OK GRABADO--%>
<div class="modal fade" id="modal-sm_OKGRABADO" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">GRABADO</h4>
              <button type="button" id="btn_grabado_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Se guardo correctamente!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_grabado_ok" runat="server" class="btn btn-primary" data-dismiss="modal" OnClientClick="window.open('/COB_Cobradores/Comprobante.pdf','_blank')" >OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->

  <%--Modal MENSAJE OK ERROR_BUSQUEDA--%>
<div class="modal fade" id="modal_error_validacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_errorvalidacion_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Ingreso invalido, complete la info solicitada!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_errorvalidacion_ok" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
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
