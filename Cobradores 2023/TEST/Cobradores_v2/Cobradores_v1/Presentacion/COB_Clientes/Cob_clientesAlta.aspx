<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Cob_clientesAlta.aspx.vb" Inherits="Presentacion.Cob_clientesAlta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <div class="card card-primary">
        <div class="card-header">
                <h3 class="card-title">CLIENTES A/B/M</h3>
        </div>
    <form role="form">
    <div class="card-body">
    <div class="container-fluid">
    <div class="row justify-content-center">
    <div class="col-lg-12">
    <%--<div class="card"> 2DO CARD COMENTADO--%>
      
    <%--<div class="card-body"> 1ER CARD COMENTADO--%>
      <div id="Div2" class="row justify-content-center" visible="True" runat="server">
        <div class="col-md-12">
          
            <div class="form-group">
        <div class="row justify-content-center">
        
          <div class="col-md-6">
          <div class="card card-secondary">
            <div class="card-header">
                <h3 class="card-title">DATOS DEL CLIENTE.</h3>
              </div>
            
            
      <form role="form">      


              <div class="card-body">
                <asp:HiddenField ID="HF_CLIE_ID" runat="server" />
                <label for="Lb_CtaCte">CtaCte:</label>
                <asp:TextBox ID="TxtCtaCte" runat="server" ReadOnly="true"  placeholder="Ingrese CtaCte..." class="form-control" MaxLength="100" onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);"></asp:TextBox>
                <asp:Label ID="Lb_error_CtaCte" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_CtaCte" runat="server" visible=false class="form-text text-muted">Error, ingrese el dato solicitado.</small>           
                <br />
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
                <br />
                <label for="Lb_Observacion">Observacion:</label>
                <asp:TextBox ID="TxtObservacion" runat="server" placeholder="Ingrese Observacion..." class="form-control" onkeydown="tecla_op(event);" ></asp:TextBox>
                <%--<br />
                <asp:Button ID="Button2" runat="server" Text="Guardar" />--%>
                </div>
            
        </form>
          </div>

          
          
        
          </div>

          <asp:HiddenField ID="HF_ASIG_LOCAL_ID" runat="server" />
        <div class="col-md-6">
          <div class="card card-secondary">
            <div class="card-header">
                <h3 class="card-title">ASIGNACION DE LOCALES.</h3>
              
            </div>
            <%--<form role="form">--%>
              <div class="card-body">
                <div class="row justify-content-center ">
                  <div class="card-tools">
                                    <div class="input-group input-group-sm" style="width: 200px;">
                                    <%--<input type="text" id="txt_buscar" runat="server" onkeydown="tecla_op(event);" name="table_search" class="form-control float-right" placeholder="Buscar...">--%>
                                    <asp:TextBox ID="TextBox2" runat="server" onkeydown="tecla_op_BUSQUEDA(event);" name="table_search" class="form-control" TextMode="SingleLine"  ></asp:TextBox>
                                    <div class="input-group-append">
                                    <button type="submit" id="Button3" runat="server" class="btn btn-default" onkeydown="tecla_op_botones(event);"><i class="fas fa-search"></i></button>
                                    </div>
                                    </div>
                            </div>
                </div>
                
                <div class="card-body table-responsive p-0" style="height: 200px" onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridView2" runat="server" class="table table-head-fixed text-nowrap" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                        <asp:BoundField DataField="LOCAL_ID" HeaderText="ID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Codigo" HeaderText="CODIGO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Sector" HeaderText="SECTOR" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Pasillo" HeaderText="PASILLO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="Local" HeaderText="LOCAL" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>  


                                        <asp:TemplateField HeaderText="ASIGNAR">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="BtnAsig" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="ASIG" Width="70px" CommandName="ID1" CommandArgument='<%# Eval("LOCAL_ID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>

                                      <asp:TemplateField HeaderText="DETALLE">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="BtnDet" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="DET" Width="70px" CommandName="ID2" CommandArgument='<%# Eval("LOCAL_ID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>

                                        
                                    </Columns>
                                </asp:GridView>
                        </div>
                
                
                <%--<label for="Lb_Local">Local:</label>
                <asp:DropDownList ID="DropDLLocal" runat="server" class="form-control" onkeydown="tecla_op(event);">
                </asp:DropDownList>
                
                <asp:Label ID="lb_error_Local" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Local" runat="server" visible=false class="form-text text-muted">Error, seleccione Local.</small>
                <br />
                <label for="Lb_Tarifa">Tarifa:</label>
                <asp:DropDownList ID="DropDLTarifa" runat="server" class="form-control" onkeydown="tecla_op(event);">
                </asp:DropDownList>
                
                <asp:Label ID="Lb_error_tarifa" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Tarifa" runat="server" visible=false class="form-text text-muted">Error, seleccione Tarifa.</small>
                <br />            
          
                <label for="Lb_TarifaPrecio">Precio:</label>
                <asp:TextBox ID="TxtTarifaPrecio" runat="server" placeholder="0,00" class="form-control" MaxLength="50" onkeydown="tecla_op(event);" ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Lb_error_tarifaprecio" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_tarifaprecio" runat="server" visible=false class="form-text text-muted">Error, ingrese precio.</small>
                <br />--%>


              </div>

            <%--</form>--%>

          </div>
            
          <div class="card card-secondary">
            <div class="card card-secondary">

              
                <div class="card-header">
                      <h3 class="card-title">LISTADO DE LOCALES/TARIFAS ASIGNADAS</h3>      
                      <%--<h3 class="card-title">RESUMEN A LA FECHA:</h3>--%>

                              
                    </div>
                      <%--<form role="form">--%>
                        <div class="card-body">
                                              <!-- /.card-header -->
                    <div class="card-body table-responsive p-0" style="height: 300px" onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridView1" runat="server" class="table table-head-fixed text-nowrap" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                      <asp:BoundField DataField="TARCLIE_ID" HeaderText="ID TARCLIE" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TARIFA_ID" HeaderText="ID TARIFA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TARIFA" HeaderText="TARIFA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LOCAL_ID" HeaderText="LOCAL_ID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Codigo" HeaderText="COD.LOCAL" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Sector" HeaderText="SECTOR" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Pasillo" HeaderText="PASILLO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Local" HeaderText="LOCAL" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="TIPO" HeaderText="TIPO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="DIAS" HeaderText="DIAS" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fechainicio" HeaderText="FECHA INICIO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>                    
                                        
                                        <%--<asp:TemplateField HeaderText="EDITAR">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="Button1" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="E" Width="70px" CommandName="ID" CommandArgument='<%# Eval("TARIFA_ID") %>' />
                                            </ItemTemplate>
                                            
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>--%>

                                      <asp:TemplateField HeaderText="CANCELAR">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="Btn_quitar" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="X" Width="70px" CommandName="ID" CommandArgument='<%# Eval("TARIFA_ID") %>' />
                                            </ItemTemplate>
                                            
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
                        </div>


                        </div>
                   <%--   </form>--%>




            </div>

          </div>

        </div>
        
        </div>
        </div>
                       

          </div>

        </div>

      <%--<div id="Div1" class="row justify-content-center" visible="True" runat="server">
                    <div class="col-md-12">
                    <div class="card card-secondary">
                    





                    </div>
                    </div>
                    </div>--%>

   <%--   </div> <%--FIN DEL 1ER CARD-BODY--%>
        



      

      
    
    
<%--    </div> FIN 2DO CARD COMENTADD--%>

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
                        &nbsp;
                            </div>
           <div class="form-group">
                                <button type="button" Class="btn btn-primary" id = "BOTON_CTACTE" runat="server" onkeydown="tecla_op_botones(event);">CTACTE</button>

        
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

  <%--Modal MENSAJE OK ERROR VARIOS--%>
<div class="modal fade" id="modal_sn_okerrorvarios" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_errorvarios_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <asp:Label ID="Label_errorvarios" runat="server" Text=""></asp:Label>
              
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_errorvarios" runat="server" class="btn btn-primary" data-dismiss="modal" onfocus="true">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->

   <!-- Modal GRABAR CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_GRABAR" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H2">GRABAR</h5>
        <button type="button" id="Btn_grabar_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        Confirma la operacion?...
      </div>
      <div class="modal-footer">
        <button type="button" id="Btn_grabar_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="Btn_grabar_confirmar" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>


  <!-- Modal DETALLE CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_DETALLE" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H3">DETALLE TARIFAS</h5>
        <button type="button" id="Btn_detalle_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:Label ID="Label8" runat="server" Text="Fecha inicio:"></asp:Label>
        <asp:TextBox ID="Txt_det_fecha" onkeydown="tecla_op(event);" runat="server" TextMode="Date"></asp:TextBox>
        <div>
          <asp:Label ID="Label2" runat="server" Text="Dias:"></asp:Label>
        
          <div class="form-group">
            <div class="row justify-content-center">
                  <div class="col-md">
                    <div class="form-check">
                  <asp:CheckBox ID="ChkLunes" runat="server" />
                  <label class="form-check-label">Lunes</label>          
                </div>
                <div class="form-check">
                  <asp:CheckBox ID="ChkMartes" runat="server" />
                  <label class="form-check-label">Martes</label>          
                </div>
                  <div class="form-check">
                  <asp:CheckBox ID="ChkMiercoles" runat="server" />
                  <label class="form-check-label">Miercoles</label>          
                </div>
                  <div class="form-check">
                  <asp:CheckBox ID="ChkJueves" runat="server" />
                  <label class="form-check-label">Jueves</label>          
                </div>
                  </div>
              
                      <div class="col-md">
                  <div class="form-check">
                  <asp:CheckBox ID="ChkViernes" runat="server" />
                  <label class="form-check-label">Viernes</label>          
                </div>
                  <div class="form-check">
                  <asp:CheckBox ID="ChkSabado" runat="server" />
                  <label class="form-check-label">Sabado</label>          
                </div>
                  <div class="form-check">
                  <asp:CheckBox ID="ChkDomingo" runat="server" />
                  <label class="form-check-label">Domingo</label>          
                </div>
                  </div>
            
            </div>
          </div>

        </div>
         
        
          <div class="card-body table-responsive p-0" style="height: 200px" onkeydown="tecla_op_botones(event);">


        <asp:GridView ID="GridView_det" runat="server" class="table table-head-fixed text-nowrap" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                      <asp:BoundField DataField="TARCLIE_ID" HeaderText="ID TARCLIE" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TARIFA_ID" HeaderText="ID TARIFA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TARIFA" HeaderText="TARIFA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LOCAL_ID" HeaderText="LOCAL_ID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Codigo" HeaderText="COD.LOCAL" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Sector" HeaderText="SECTOR" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Pasillo" HeaderText="PASILLO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Local" HeaderText="LOCAL" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="TIPO" HeaderText="TIPO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <%--<asp:BoundField DataField="DIAS" HeaderText="DIAS" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>--%>
                                      <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                                                             
                                        
                                        <%--<asp:TemplateField HeaderText="EDITAR">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="Button1" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="E" Width="70px" CommandName="ID" CommandArgument='<%# Eval("TARIFA_ID") %>' />
                                            </ItemTemplate>
                                            
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>--%>

                                      <asp:TemplateField HeaderText="SELEC">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              <asp:CheckBox ID="CheckBox_det" runat="server" />
                                              
                                            </ItemTemplate>
                                            
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
          </div>
        
        
      </div>
      <div class="modal-footer">
        <button type="button" id="Btn_detalle_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="Btn_detalle_asignar" class="btn btn-primary" runat="server" data-dismiss="modal">Asignar</button>
      </div>
    </div>
  </div>
</div>


  <!-- Modal GRABAR CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_ASIGNAR" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H4">ASIGNAR LOCAL</h5>
        <button type="button" id="Btn_asig_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:Label ID="Label1" runat="server" Text="Fecha inicio:"></asp:Label>
        <asp:TextBox ID="Txt_asig_fecha" onkeydown="tecla_op(event);" runat="server" TextMode="Date"></asp:TextBox>
      <div>
          <asp:Label ID="Label3" runat="server" Text="Dias:"></asp:Label>
        
          <div class="form-group">
            <div class="row justify-content-center">
                  <div class="col-md">
                    <div class="form-check">
                  <asp:CheckBox ID="Chk1Lunes" runat="server" />
                  <label class="form-check-label">Lunes</label>          
                </div>
                <div class="form-check">
                  <asp:CheckBox ID="Chk1Martes" runat="server" />
                  <label class="form-check-label">Martes</label>          
                </div>
                  <div class="form-check">
                  <asp:CheckBox ID="Chk1Miercoles" runat="server" />
                  <label class="form-check-label">Miercoles</label>          
                </div>
                  <div class="form-check">
                  <asp:CheckBox ID="Chk1Jueves" runat="server" />
                  <label class="form-check-label">Jueves</label>          
                </div>
                  </div>
              
                      <div class="col-md">
                  <div class="form-check">
                  <asp:CheckBox ID="Chk1Viernes" runat="server" />
                  <label class="form-check-label">Viernes</label>          
                </div>
                  <div class="form-check">
                  <asp:CheckBox ID="Chk1Sabado" runat="server" />
                  <label class="form-check-label">Sabado</label>          
                </div>
                  <div class="form-check">
                  <asp:CheckBox ID="Chk1Domingo" runat="server" />
                  <label class="form-check-label">Domingo</label>          
                </div>
                  </div>
            
            </div>
          </div>

        </div>
      
      
      </div>
      <div class="modal-footer">
        <button type="button" id="Btn_asig_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="Btn_asig_confirmar" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>



</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
