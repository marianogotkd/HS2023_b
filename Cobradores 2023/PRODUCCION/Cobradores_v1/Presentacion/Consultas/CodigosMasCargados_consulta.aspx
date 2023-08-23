<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="CodigosMasCargados_consulta.aspx.vb" Inherits="Presentacion.CodigosMasCargados_consulta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
      <div class="card card-primary">
          <div class="card-header">
                <h3 class="card-title">CODIGOS MAS CARGADOS</h3>
          </div>
          <form role="form">
<div class="card-body">
<div class="container-fluid">
<div class="row justify-content-center">
<div class="col-lg"> <%--aqui decia col-lg-6--%>
<div class="card">
        <div class="card-body">
            <div class="form-group">
                            <div class="row justify-content-center">
                                
                              
                              
                              <div class="col-4">
                                <div class="form-group">
                                                <asp:Label ID="Label_dia" runat="server" Text="DIA:"></asp:Label>
                                                <asp:HiddenField ID="HF_dia_id" runat="server" />                  
                                </div>
                                <div class="form-group">
                                            <asp:HiddenField ID="HF_parametro_id" runat="server" />
                                            <asp:Label ID="LABEL_FECHA" runat="server" Text="FECHA:"></asp:Label>
                                            <asp:TextBox ID="Txt_fecha" onkeydown="tecla_op(event);" runat="server" TextMode="Date"></asp:TextBox>   
                                            <asp:HiddenField ID="HF_fecha" runat="server" />          
                                </div>
                              </div>



                                    <%--<div class="col-md-4">
                                    </div>--%>
                            </div>
                    </div>    

          <div class="form-group">       
                <div class="row justify-content-center">
                  <div class="col-md-4">
                                                <label for="lblClienteDesde">DESDE CLIENTE</label>
                                                <asp:TextBox ID="txtClienteDesde" runat="server" 
                                                    placeholder="." class="form-control" 
                                                    onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);" 
                                                    MaxLength="4"></asp:TextBox>
                                                <asp:Label ID="lb_error_codigo" runat="server" ForeColor="Red" Text="*" Visible="false"></asp:Label>
                                                
                                    </div>
                  <div class="col-md-8">
                                          
                                                
                                    </div>
                  </div>
                </div>

          <div class="form-group">       
                                <div class="row justify-content-center">
                                        
                                    

                                    <div class="col-md-4">
                                                  <caption>
                                                      <label for="lblClienteHasta">
                                                      HASTA CLIENTE</label>
                                                      <asp:TextBox ID="txtClienteHasta" runat="server" class="form-control" MaxLength="4" onkeydown="tecla_op(event);" onkeypress="return justNumbers(event);" placeholder=""></asp:TextBox>
                                                      <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*" Visible="false"></asp:Label>
                                                  </caption>
                                                
                                     </div>
                                  <div class="col-md-8">
                                    
                                  
                                  </div>
                               

                    </div>
          </div>

                        <div class="form-group">       
                <div class="row justify-content-center">
                  <div class="col-md-4">
                                                  <label for="lblImporte1">IMPORTE MINIMO PARA 1 CIFRA:</label>
                                                  <asp:TextBox ID="txtImporte1" runat="server" class="form-control" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);"></asp:TextBox>
                                                  <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                            
                                     </div>

                  <div class="col-md-8">
                                      
                            
                                     </div>

                  </div>

                </div>
              <div class="form-group">       
                <div class="row justify-content-center">
                              <div class="col-md-4">
                                                  <label for="lblImporte2">IMPORTE MINIMO PARA 2 CIFRAS:</label>
                                                  <asp:TextBox ID="txtImporte2" runat="server" class="form-control" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);"></asp:TextBox>
                                                  <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                            
                                     </div>

                    <div class="col-md-8">

                      </div>
                  </div>
                </div>

              <div class="form-group">       
                <div class="row justify-content-center">
                                <div class="col-md-4">
                                                    <label for="lblImporte4">IMPORTE MINIMO PARA 3 CIFRAS:</label>
                                                    <asp:TextBox ID="txtImporte3" runat="server" class="form-control" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);"></asp:TextBox>
                                                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                            
                                       </div>

                    <div class="col-md-8">

                      </div>
                  </div>
                </div>


              <div class="form-group">       
                  <div class="row justify-content-center">
                                     
                 
                                      <div class="col-md-4">
                                                    <label for="lblImporte4">IMPORTE MINIMO PARA 4 CIFRAS:</label>
                                                    <asp:TextBox ID="txtImporte4" runat="server" class="form-control" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);"></asp:TextBox>
                                                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                            
                                       </div>
                    <div class="col-md-8">

                      </div>

                  </div>
              </div>

          

          

          

              




                
          <div class="form-group">
              <div class="row justify-content-center">
              
                <div class="col-12">
                  <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False">
                      <Columns>
                          <asp:BoundField DataField="Terminal" />
                          <asp:BoundField DataField="Registros" />
                          <asp:BoundField DataField="NoVerificados" />
                      </Columns>
                    </asp:GridView>
                <div class="card">
                    
                    <div class="card-body table-responsive p-0" style="height: 400px" onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridView1" runat="server" class="table table-head-fixed text-nowrap" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"  ShowHeader="true"> 
                                    <Columns>
                                      <asp:BoundField DataField="unacifra" HeaderText="UNA CIFRA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="pid1" HeaderText="PID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="zona1" HeaderText="ZONA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="importe1" HeaderText="IMPORTE" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="doscifras" HeaderText="DOS CIFRAS" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="pid2" HeaderText="PID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="zona2" HeaderText="ZONA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="importe2" HeaderText="IMPORTE" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="trescifras" HeaderText="TRES CIFRAS" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="pid3" HeaderText="PID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="zona3" HeaderText="ZONA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="importe3" HeaderText="IMPORTE" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="cuatrocifras" HeaderText="CUATRO CIFRAS" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="pid4" HeaderText="PID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="zona4" HeaderText="ZONA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="importe4" HeaderText="IMPORTE" >                                                               
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

        <div class="card-footer">
        <div class="row justify-content-center" >
        <div class="row align-items-center">
            
          <div class="form-group">
                  <button type="submit" UseSubmitBehavior="false" class="btn btn-primary" runat="server" id="btn_retroceder" onkeydown="tecla_op_botones(event);">ESC = RETROCEDE</button>
                    &nbsp;</div>
                 

            

            <div class="form-group">
            
              <asp:Button ID="BTN_IMPRIMIR" runat="server" Text="IMPRIMIR" class="btn btn-primary" onkeydown="tecla_op_botones(event);" OnClientClick="window.open('/WC_Reportes/Rpt/CodigosMasCargados.pdf','_blank')" />

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
