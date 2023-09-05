<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Cob_adminprocdiario.aspx.vb" Inherits="Presentacion.Cob_adminprocdiario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" 
                            EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200">
</asp:ScriptManager>

  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<div class="card card-primary">
<div class="card-header">
                <h3 class="card-title">ADMINISTRADOR - PROC.DIARIO</h3>
</div>
<form role="form">
<div class="card-body">
    <div align="center">
    <div class="row justify-content-center" >   <%--class="row"--%>
        <div class="col-lg-12">
            <div class="card">
                <div class="card-body">
                    <div class="form-group">
                      <asp:Label ID="Label1" runat="server" Text="EJECUTAR EL PROCESO PARA EL SIGUIENTE INTERVALO:"></asp:Label>  
                        <br />
                      
                        
                        
                    </div>
                    
                    <div class="form-group">
                      <asp:HiddenField ID="HF_parametro_id" runat="server" />
                      <asp:Label ID="Label2" runat="server" Text="DESDE EL DIA:"></asp:Label>
                      <asp:TextBox ID="Txt_asig_fechaDesde" onkeydown="tecla_op(event);" runat="server" TextMode="Date"></asp:TextBox>
                      <%--<asp:Label ID="Label_fecha" runat="server" Text=""></asp:Label>--%>
                      <asp:HiddenField ID="HF_fechaDesde" runat="server" />
                                           
                      
                    </div>
                  <div class="form-group">
                    <asp:Label ID="Label6" runat="server" Text="HASTA EL DIA:"></asp:Label>
                      <asp:TextBox ID="Txt_asig_fechaHasta" onkeydown="tecla_op(event);" runat="server" TextMode="Date"></asp:TextBox>
                      <%--<asp:Label ID="Label_fecha" runat="server" Text=""></asp:Label>--%>
                      <asp:HiddenField ID="HF_fechaHasta" runat="server" />
                  </div>


                  <div class="form-group">
                      <%--<asp:Label ID="Label3" runat="server" Text="DIA:"></asp:Label>
                      <asp:Label ID="Label_dia" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="HF_dia_id" runat="server" />--%>
                      
                    </div>
                   <%--<div class="form-group">
                      <asp:Label ID="Label4" runat="server" Text="TIPEAR OK: "></asp:Label>
                      <asp:TextBox ID="Txt_OP" placeholder="" runat="server" 
                            CausesValidation="True" onkeydown="tecla_op(event);"  MaxLength="2" Width="46px"></asp:TextBox>
                      
                    </div>--%>                     
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
                    <button type="button" id="btn_modificar" runat="server" class="btn btn-primary" onkeydown="tecla_op_botones(event);" onclick="this.disabled=true;">
                          GRABA
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
              <%--<p>Error, primero debe iniciar dia!&hellip;</p>--%>
              <asp:Label ID="LabelMsj_Modal_ok_error" runat="server" Text=""></asp:Label>
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

       <%--MODAL MSJ CENTRADO - MSJ_CONTINUE--%>
<div class="modal fade" id="modal-msj_continue" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title"></h4>
              <button type="button" id="Button1" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Se descargara una copia de la base de datos.
                OK para continuar con el proceso...&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="Btn_Ok_continue" runat="server" tabindex="1" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


  <%--Modal MENSAJE ERROR OK 1--%>
<div class="modal fade" id="modal-ok-complete" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Mensaje</h4>
              <button type="button" id="Btn_ok_complete_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <%--<p>Error, primero debe iniciar dia!&hellip;</p>--%>
              <asp:Label ID="Label5" runat="server" Text="El proc.Finalizo correctamente!"></asp:Label>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="Btn_ok_complete" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
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
