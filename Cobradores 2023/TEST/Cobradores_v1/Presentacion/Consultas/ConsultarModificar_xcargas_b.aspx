<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="ConsultarModificar_xcargas_b.aspx.vb" Inherits="Presentacion.ConsultarModificar_xcargas_b" %>
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
                          <div class="col-md-12">
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
                          <div class="col-md-12">
                            <asp:Label ID="Label_recorridos" runat="server" Text="RECORRIDOS:"></asp:Label>  
                          </div>

                    </div>
                        
                    </div>


                  <asp:HiddenField ID="HF_cliente_id" runat="server" />
                    <div class="form-group">
          
                        <div class="row justify-content-center">
                          <div class="col-md-6">                            
                            <asp:Label ID="Label_cliente" runat="server" Text="CLIENTE:"></asp:Label>
                                <asp:TextBox ID="Txt_ClienteCod" runat="server" 
                                              placeholder="Ingrese codigo..." class="form-control" 
                                              onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);" 
                                              MaxLength="4"></asp:TextBox>
                          </div>
                          <div class="col-md-6">
                            <asp:Label ID="Label4" runat="server" Text="NOMBRE:"></asp:Label>
                            <asp:TextBox ID="Txt_cliente_nombre" runat="server" placeholder="" class="form-control" MaxLength="50" onkeydown="tecla_op(event);" ></asp:TextBox>
                          </div>
                          
                            
                        </div>
                  </div>

                  
                  

                  <div id="Div1" class="row justify-content-center" visible="True" runat="server">
                    <div class="col-md-12">
                    <div class="card">
                    
                    <!-- /.card-header -->
                    <div class="card-body table-responsive p-0" style="height: 400px" onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridView1" runat="server" class="table table-head-fixed text-nowrap" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                        <asp:BoundField DataField="IDcarga" HeaderText="IDcarga" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Recorrido">
                                            <EditItemTemplate>
                                              <asp:Label ID="Label1" runat="server" Text='<%# Bind("Recorrido") %>'></asp:Label>  
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              <asp:TextBox ID="Txt_recorrido" runat="server" Text='<%# Bind("Recorrido") %>' onkeydown="tecla_op(event);"></asp:TextBox>  
                                                                                           
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Pid">
                                            <EditItemTemplate>
                                              <asp:Label ID="Label1" runat="server" Text='<%# Bind("Pid") %>'></asp:Label>  
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              <asp:TextBox ID="Txt_Pid" runat="server" Text='<%# Bind("Pid") %>' Width="40" MaxLength="4" onkeypress="return justNumbers(event);" onkeydown="tecla_op(event);"></asp:TextBox>  
                                              
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe">
                                            <EditItemTemplate>
                                              <asp:Label ID="Label2" runat="server" Text='<%# Bind("Importe") %>'></asp:Label>  
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              <asp:TextBox ID="Txt_Importe" runat="server" Text='<%# Bind("Importe") %>' MaxLength="8" onkeypress="return validateDecimalKeyPress(this, event);" onkeydown="tecla_op(event);"></asp:TextBox>  
                                              
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S">
                                            <EditItemTemplate>
                                              <asp:Label ID="Label3" runat="server" Text='<%# Bind("Suc") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_suc" runat="server" Text='<%# Bind("Suc") %>' Width="30" MaxLength="2" onkeypress="return justNumbers(event);" onkeydown="tecla_op(event);"></asp:TextBox>
                                              
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pid2">
                                            <EditItemTemplate>
                                              <asp:Label ID="Label4" runat="server" Text='<%# Bind("Pid2") %>'></asp:Label>  
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              <asp:TextBox ID="Txt_Pid2" runat="server" Text='<%# Bind("Pid2") %>' Width="40" MaxLength="3" onkeypress="return justNumbers(event);" onkeydown="tecla_op(event);"></asp:TextBox>  
                                              
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S2">
                                            <EditItemTemplate>
                                              <asp:Label ID="Label5" runat="server" Text='<%# Bind("Suc2") %>'></asp:Label>  
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              <asp:TextBox ID="Txt_suc2" runat="server" Text='<%# Bind("Suc2") %>' Width="30" MaxLength="2" onkeypress="return justNumbers(event);" onkeydown="tecla_op(event);"></asp:TextBox>  
                                              
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="R" HeaderText="R" >
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="SC">
                                            <EditItemTemplate>
                                              <asp:Label ID="Label6" runat="server" Text='<%# Bind("SinComputo") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_sincomputo" runat="server" Text='<%# Bind("SinComputo") %>' Wrap="True" Width="50" MaxLength="2" onkeydown="tecla_op(event);"></asp:TextBox>
                                              
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TotalImporte" HeaderText="TotalImporte" >
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Verificado" HeaderText="V" >
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField> 
                                      <asp:BoundField DataField="Terminal" HeaderText="T" >
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField> 
                                      <asp:BoundField DataField="Item" HeaderText="Item" >
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField> 
                                      <asp:BoundField DataField="Fecha" HeaderText="Fecha" >
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField> 
                                      <asp:BoundField DataField="Hora" HeaderText="Hora" >
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField> 
                                                                            
                                      <asp:BoundField DataField="Tabla" HeaderText="Tabla" >
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField> 


                                    </Columns>
                                </asp:GridView>
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
<button type="submit" UseSubmitBehavior="false" class="btn btn-primary" runat="server" id="btn_retroceder" onkeydown="tecla_op_botones(event);">ESC = RETROCEDE</button>
    &nbsp;</div>
<div class="form-group">
                    <button type="button" id="btn_modificar" runat="server" class="btn btn-primary" data-toggle="modal" data-target="#Mdl_modificar" onkeydown="tecla_op_botones(event);">
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


  <!-- Modal MODIFICAR CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_modificar" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H1">Graba</h5>
        <button type="button" id="btn_modificar_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        ¿Confirma la operación?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_modificar_mdl_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_modificar_mdll" class="btn btn-primary" runat="server" data-dismiss="modal" onclick="this.disabled=true;">Confirmar</button>
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



</ContentTemplate>
</asp:UpdatePanel>
  <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
            AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
       
         <div style="background-color: Gray; filter:alpha(opacity=60); opacity:0.60; width: 100%; top: 0px; left: 0px; position: fixed; height: 100%;"> </div>
          <div style="margin:auto;
              font-family:Trebuchet MS;
              filter: alpha(opacity=100);
              opacity: 1;
              font-size:small;
              vertical-align: middle;
              top: 40%;
              position: fixed;
              right: 40%;
              color: #275721;
              text-align: center;
              background-color: White;
              height: 100px;
              ">


              <div class="card card-danger">
              <div class="card-header">
                <h3 class="card-title">Procesando Solicitud</h3>
              </div>
              <div class="card-body">
                Aguarde un Momento Por Favor...
              </div>
              <!-- /.card-body -->
              <!-- Loading (remove the following to stop the loading)-->
              <div class="overlay">
                <i class="fa fa-refresh fa-spin"></i>
              </div>
              <!-- end loading -->
            </div>
                   

        </div>

        
        </ProgressTemplate>
        
        </asp:UpdateProgress>


</asp:Content>
