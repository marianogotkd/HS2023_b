<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="LiquidacionParcial_PremiosxClientes.aspx.vb" Inherits="Presentacion.LiquidacionParcial_PremiosxClientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
      <div class="card card-primary">
          <div class="card-header">
                <h3 class="card-title">LIQUIDACION PARCIAL - PREMIOS POR CLIENTES</h3>
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
                                <div class="col-lg"> <%--antes col-4--%>
                                        <div class="form-group">
                                                <asp:HiddenField ID="HF_parametro_id" runat="server" />
                                            <asp:Label ID="LABEL_FECHA" runat="server" Text="PREMIOS POR CLIENTES PARA LA FECHA:"></asp:Label>
                                          <asp:Label ID="LABEL_fecha_parametro" runat="server" Text=""></asp:Label>
                                            
                                          <asp:HiddenField ID="HF_fecha" runat="server" />
                                        </div>
                                        
                                    
                                </div>
                                <%--<div class="col-4">
                                  
                                                                        
                                </div>
                                <div class="col-4">
                                    
                                                                        
                                </div>--%>

                        </div>
                </div>


                
          <div class="form-group">
              <div class="row justify-content-center">
              
                <div class="col-lg"> <%--antes col-8--%>
                  <asp:GridView ID="GridView2" runat="server">
                    </asp:GridView>
                <div class="card">
                    
                    <div class="card-body table-responsive p-0" style="height: 400px" onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridView1" runat="server" class="table table-head-fixed text-nowrap" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                        <asp:BoundField DataField="Cliente" HeaderText="CLIENTE" >                                                               
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Recorrido" HeaderText="RECORRIDO" >                                                               
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Importe" HeaderText="IMPORTE" >                                                               
                                        </asp:BoundField>
                                        
                                        
                                        <asp:BoundField DataField="PID" HeaderText="PID" />
                                        <asp:BoundField DataField="SUC" HeaderText="SUC" />
                                        <asp:BoundField DataField="P2" HeaderText="P2" />
                                        <asp:BoundField DataField="S2" HeaderText="S2" />
                                        <asp:BoundField DataField="SC" HeaderText="SC" />
                                        <asp:BoundField DataField="Premio" HeaderText="PREMIO" />
                                        <asp:BoundField DataField="T" HeaderText="T" />
                                        <asp:BoundField DataField="OBS" HeaderText="OBS" />
                                        
                                        
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
            <button type="submit" UseSubmitBehavior="false" class="btn btn-primary" runat="server" id="btn_avanzar" onkeydown="tecla_op_botones(event);" onclick="this.disabled=true;">
                CONTINUAR</button>
            &nbsp;
            </div>
          

            

            <div class="form-group">
                       
              <asp:Button ID="BTN_IMPRIMIR" runat="server" Text="IMPRIMIR" class="btn btn-primary" OnClientClick="window.open('/WC_Reportes/Rpt/LiqParcial_premiosxclientes.pdf','_blank')" />
            
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
