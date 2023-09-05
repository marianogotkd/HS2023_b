<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Cob_LocalAlta.aspx.vb" Inherits="Presentacion.Cob_LocalAlta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <div class="card card-primary">
        <div class="card-header">
                <h3 class="card-title">LOCAL A/B/M</h3>
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
                <h3 class="card-title">DATOS DEL LOCAL</h3>
              </div>
            <form role="form">
              <div class="card-body">
                
          <label for="Lb_Sector">Sector:</label>
                <asp:DropDownList ID="DropDLSector" runat="server" class="form-control" AutoPostBack="True" onkeydown="tecla_op(event);">
                </asp:DropDownList>
                
                <asp:Label ID="lb_error_Sector" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Sector" runat="server" visible=false class="form-text text-muted">Error, seleccione Sector.</small>
          <br />
          <label for="Lb_Pasillo">Pasillo:</label>
                <asp:DropDownList ID="DropDLPasillo" runat="server" class="form-control" AutoPostBack="True" onkeydown="tecla_op(event);">
                </asp:DropDownList>
                                       
                <asp:Label ID="lb_error_pasillo" runat="server" ForeColor="Red" Text="*error, seleccione Jerarquia" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Pasillo" runat="server" visible=false class="form-text text-muted">Error, seleccione Pasillo.</small>
                <br />
          <asp:HiddenField ID="HF_LOCAL_ID" runat="server" />
                <label for="Lb_codigolocal">Codigo:</label>
                <asp:TextBox ID="TxtCodigo" runat="server" placeholder="Ingrese codigo..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox>
                                       
                <asp:Label ID="lb_error_codigolocal" runat="server" ForeColor="Red" Text="*error, ingrese codigo" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Codigolocal" runat="server" visible=false class="form-text text-muted">Error, ingrese codigo.</small>
          <br />
<label for="Lb_local">Local:</label>
                <asp:TextBox ID="TxtLocal" runat="server" placeholder="Ingrese local..." class="form-control" MaxLength="100" onkeydown="tecla_op(event);"></asp:TextBox>
                                       
                <asp:Label ID="Lb_error_local" runat="server" ForeColor="Red" Text="*error, ingrese local" 
                            Visible="False"></asp:Label>
                <small id="SmallError_Local" runat="server" visible=false class="form-text text-muted">Error, ingrese local.</small>
              <br />
          <%--<asp:Button ID="Button2" runat="server" Text="Guardar" />--%>
              </div>
            </form>

          </div>

          
          
        
          </div>
        
        <div class="col-md-6">
          <div class="card card-secondary">
            <div class="card-header">
                <h3 class="card-title">ASIGNAR TARIFAS AL LOCAL</h3>
              </div>
          <div class="card-body">
            <label for="Lb_TarifaDesc">Descripcion:</label>
                <asp:TextBox ID="TxtTarifa" runat="server" placeholder="Ingrese Descripcion..." class="form-control" MaxLength="50" onkeydown="tecla_op(event);"></asp:TextBox>
          <asp:Label ID="Lb_error_tarifadesc" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_tarifadesc" runat="server" ForeColor="Red" visible=false class="form-text text-muted">Error, ingrese descripcion.</small>
          
            
          <br />
          <label for="Lb_TarifaTipo">Tipo:</label>
                <asp:DropDownList ID="DropDLTarifaTipo" runat="server" class="form-control" AutoPostBack="True" onkeydown="tecla_op(event);">
                    <asp:ListItem Selected="True">Unica</asp:ListItem>
                    <asp:ListItem>Periodica</asp:ListItem>
                </asp:DropDownList>
          <asp:Label ID="Lb_error_tarifatipo" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_tarifatipo" runat="server" visible=false class="form-text text-muted">Error, seleccione opcion.</small>

          
            <div id="dias_oculto" runat="server" visible="false">
              <br />
              <label for="Lb_TarifaDia">Dias:</label>
              <asp:TextBox ID="TxtTarifaDias" runat="server" placeholder="0" class="form-control" MaxLength="3" onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);"></asp:TextBox>
              <asp:Label ID="Lb_error_tarifadias" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
              <small id="SmallError_tarifadias" runat="server" ForeColor="Red" visible=false class="form-text text-muted">Error, ingrese dias.</small>
            </div>
          
        <br />
          <label for="Lb_TarifaPrecio">Precio:</label>
                <asp:TextBox ID="TxtTarifaPrecio" runat="server" placeholder="0,00" class="form-control" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);"></asp:TextBox>
          <asp:Label ID="Lb_error_tarifaprecio" runat="server" ForeColor="Red" Text="*" 
                            Visible="False"></asp:Label>
                <small id="SmallError_tarifaprecio" runat="server" ForeColor="Red" visible=false class="form-text text-muted">Error, ingrese precio.</small>
        <br />
          <asp:Button ID="BtnAsignar" runat="server" Text="Asignar" />
            
            </div>


          </div>
            
        </div>
        
        </div>
        </div>
                       

          </div>

        </div>

      <div id="Div1" class="row justify-content-center" visible="True" runat="server">
                    <div class="col-md-12">
                    <div class="card card-secondary">
                    <div class="card-header">
                      <h3 class="card-title">LISTADO DE TARIFAS ASIGNADAS</h3>      
                      <%--<h3 class="card-title">RESUMEN A LA FECHA:</h3>--%>

                              <div class="card-tools">
                                    <div class="input-group input-group-sm" style="width: 200px;">
                                    <%--<input type="text" id="txt_buscar" runat="server" onkeydown="tecla_op(event);" name="table_search" class="form-control float-right" placeholder="Buscar...">--%>
                                    <asp:TextBox ID="txt_buscar" runat="server" onkeydown="tecla_op_BUSQUEDA(event);" name="table_search" class="form-control" TextMode="SingleLine"  ></asp:TextBox>
                                    <div class="input-group-append">
                                    <button type="submit" id="btn_buscar" runat="server" class="btn btn-default" onkeydown="tecla_op_botones(event);"><i class="fas fa-search"></i></button>
                                    </div>
                                    </div>
                            </div>
                    </div>
                      <%--<form role="form">--%>
                        <div class="card-body">
                                              <!-- /.card-header -->
                    <div class="card-body table-responsive p-0" style="height: 300px" onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridView1" runat="server" class="table table-head-fixed text-nowrap" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                        <asp:BoundField DataField="TARIFA_ID" HeaderText="ID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TARIFA" HeaderText="TARIFAS" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LOCAL_ID" HeaderText="LOCAL_ID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="TIPO" HeaderText="TIPO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      
                                      <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                                                             
                                        
                                        <asp:TemplateField HeaderText="EDITAR">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              
                                              <asp:Button ID="Button_edit" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="E" Width="70px" CommandName="ID1" CommandArgument='<%# Eval("TARIFA") %>' />
                                            </ItemTemplate>
                                            
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>

                                      <asp:TemplateField HeaderText="QUITAR">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="Button_quitar" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="X" Width="70px" CommandName="ID2" CommandArgument='<%# Eval("TARIFA") %>' />
                                            </ItemTemplate>
                                            
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
                        </div>


                        </div>
                      <%--</form>--%>




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
              <p>Confirma la operacion?&hellip;</p>
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


  <div class="modal fade" id="Mdl_modificarprecio" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H3">MODIFICAR PRECIO</h5>
        <button type="button" id="Btn_modifprecio_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:HiddenField ID="HF_TARIFA_itemdesc" runat="server" />  
        <asp:Label ID="Lb_precio" runat="server" Text="PRECIO:"></asp:Label>
        <asp:TextBox ID="txt_modif_precio" runat="server" placeholder="0,00" class="form-control" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);"></asp:TextBox>
      
      </div>
      <div class="modal-footer">
        <button type="button" id="Btn_modifprecio_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="Btn_modifprecio_confirmar" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>




</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
