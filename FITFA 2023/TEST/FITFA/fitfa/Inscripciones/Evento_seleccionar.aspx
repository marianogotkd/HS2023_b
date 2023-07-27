﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="Evento_seleccionar.aspx.vb" Inherits="fitfa.Evento_seleccionar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type = "text/css">
        #Background
        {
                          
           position: fixed;
            top: 0px;
            bottom: 0px;
            left: 0px;
            right: 0px;
            overflow: hidden;
            padding: 0;
            margin: 0;
            background-color: Black ;
            filter: alpha(opacity=99);
            opacity: 0.6;
            z-index: 100000;
                     
            
        }
        #Progress
        {
            position: fixed;
            top: 40%;
            left: 40%;
            height: 10%;
            width: 10%;
            z-index: 100001;
            background-color: #FFFFFF;
            border: 1px solid Gray;
            background-repeat: no-repeat;
            background-position: center;
            
        }          
        
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

    
    <div class="card card-primary">
                           
              <div class="card-header">
                <h3 class="card-title">Eventos disponibles</h3>
                  
              </div>

              <form role="form">
              <div class="card-body"> 
              <div id="Seccion01" runat="server" visible="false">
                  <div class="container-fluid">
                      
                      <div class="row justify-content-center">
                          <div class="col-lg-12">
                              <div class="card">
                                  <div class="card-body">
                                      <div class="form-group">
                                          <div class="row justify-content-lg-start">
                                              <div class="col-md-3">
                                                  <asp:HiddenField ID="HF_SEC01_id" runat="server" />
                                                  <asp:LinkButton ID="LinkButton_SEC01_evento" runat="server" ForeColor="#6600FF" Font-Bold="True"></asp:LinkButton>

                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC01_tipo" runat="server" Text="">Tipo:</asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC01_fecha" runat="server" Text="Fecha:"></asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC01_FechaCierre" runat="server" Text="Cierre de inscripcion:"></asp:Label>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
              <div id="Seccion02" runat="server" visible="false">
                  <div class="container-fluid">
                      <div class="row justify-content-center">
                          <div class="col-lg-12">
                              <div class="card">
                                  <div class="card-body">
                                      <div class="form-group">
                                          <div class="row justify-content-lg-start">
                                              <div class="col-md-3">
                                                  <asp:HiddenField ID="HF_SEC02_id" runat="server" />
                                                  <asp:LinkButton ID="LinkButton_SEC02_evento" runat="server" ForeColor="#6600FF" Font-Bold="True"></asp:LinkButton>

                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC02_tipo" runat="server" Text="">Tipo:</asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC02_fecha" runat="server" Text="Fecha:"></asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC02_FechaCierre" runat="server" Text="Cierre de inscripcion:"></asp:Label>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
                  <div id="Seccion03" runat="server" visible="false">
                  <div class="container-fluid">
                      <div class="row justify-content-center">
                          <div class="col-lg-12">
                              <div class="card">
                                  <div class="card-body">
                                      <div class="form-group">
                                          <div class="row justify-content-lg-start">
                                              <div class="col-md-3">
                                                  <asp:HiddenField ID="HF_SEC03_id" runat="server" />
                                                  <asp:LinkButton ID="LinkButton_SEC03_evento" runat="server" ForeColor="#6600FF" Font-Bold="True"></asp:LinkButton>

                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC03_tipo" runat="server" Text="">Tipo:</asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC03_fecha" runat="server" Text="Fecha:"></asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC03_FechaCierre" runat="server" Text="Cierre de inscripcion:"></asp:Label>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
                  <div id="Seccion04" runat="server" visible="false">
                  <div class="container-fluid">
                      <div class="row justify-content-center">
                          <div class="col-lg-12">
                              <div class="card">
                                  <div class="card-body">
                                      <div class="form-group">
                                          <div class="row justify-content-lg-start">
                                              <div class="col-md-3">
                                                  <asp:HiddenField ID="HF_SEC04_id" runat="server" />
                                                  <asp:LinkButton ID="LinkButton_SEC04_evento" runat="server" ForeColor="#6600FF" Font-Bold="True"></asp:LinkButton>

                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC04_tipo" runat="server" Text="">Tipo:</asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC04_fecha" runat="server" Text="Fecha:"></asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC04_FechaCierre" runat="server" Text="Cierre de inscripcion:"></asp:Label>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
                  <div id="Seccion05" runat="server" visible="false">
                  <div class="container-fluid">
                      <div class="row justify-content-center">
                          <div class="col-lg-12">
                              <div class="card">
                                  <div class="card-body">
                                      <div class="form-group">
                                          <div class="row justify-content-lg-start">
                                              <div class="col-md-3">
                                                  <asp:HiddenField ID="HF_SEC05_id" runat="server" />
                                                  <asp:LinkButton ID="LinkButton_SEC05_evento" runat="server" ForeColor="#6600FF" Font-Bold="True"></asp:LinkButton>

                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC05_tipo" runat="server" Text="">Tipo:</asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC05_fecha" runat="server" Text="Fecha:"></asp:Label>
                                              </div>
                                              <div class="col-md-3">
                                                  <asp:Label ID="Label_SEC05_FechaCierre" runat="server" Text="Cierre de inscripcion:"></asp:Label>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
                  

                  
                  
                  
                  <div align="center">
                    
                    <div id="no_evento" runat="server">
                          <div class="card card-primary">
                                <div class="card-header">
                                    <h3 class="card-title">Advertencia</h3>
                                </div>
                                <form role="form">
                                  <div class="card-body"> 
                                  <div class="row">
                                  <div align="center">
                                        <asp:Label ID="Label2" runat="server" Text="No hay eventos disponibles!"></asp:Label>
                                        &nbsp;
                                  </div>
               
                                  </div>
                                  </div>
                                </form>              
                        </div> 
                    
                    </div>

                    
                  
                  
                  
                  <div id="SeccionGrid" runat="server" visible="false">
                      <div class="col-md-8">
                    <div class="card">
                    <div class="card-body table-responsive p-0" style="height: 500px"> <%--class="form-group"--%>
                    <asp:GridView ID="GridView1" runat="server" class="table table-hover" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                           BorderColor="Black" GridLines="None" 
                          EnableSortingAndPagingCallbacks="True">
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="ID" >                                                               
                                <HeaderStyle ForeColor="#0099FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripción" HeaderText="Descripción" >
                                <HeaderStyle ForeColor="#0099FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" >
                                <HeaderStyle ForeColor="#0099FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" >
                                <HeaderStyle ForeColor="#0099FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cierre de inscripción" 
                                    HeaderText="Cierre de inscripción" >
                                <HeaderStyle ForeColor="#0099FF" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                  <ItemTemplate>
                                      <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" 
                                          CommandName='ID' ImageUrl="~/img/lupa.png" CommandArgument='<%# Eval("ID") %>' Text="" ToolTip="Inscribir" />
                                  </ItemTemplate>
                                  <ControlStyle Height="30px" Width="30px" />
                                  <HeaderStyle ForeColor="#0099FF" />
                                  <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                              </asp:TemplateField>
                            </Columns>
                        </asp:GridView>



                    </div>
                    </div>
                    </div>
                  </div>
                  
                  
                  
                    
                    
    
                </div>
              
              </div>
              </form>

              <div class="card-footer">
                  <asp:Label ID="Label1" runat="server" Text="Seleccione el evento en el que desea participar"></asp:Label>
              </div>

    </div>
            
    <div id="choco" runat="server">
    <asp:HiddenField ID="HiddenField_msj" runat="server" />
        <asp:Panel ID="Panel1" runat="server" >
                          <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">Advertencia</h3>
            </div>
            <form role="form">
              <div class="card-body"> 
              <div class="row">
              <div align="center">
                    <asp:Label ID="Label12" runat="server" Text="No hay eventos disponibles!"></asp:Label>
                    &nbsp;
              </div>
              <div align="center">
                    <asp:Button ID="Btb_msj_no_eventos" runat="server" Text="OK" />
              </div>  
              </div>
              </div>
            </form>              
            </div> 
        </asp:Panel>
        
<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField_msj" PopupControlID="Panel1" CancelControlID="Btb_msj_no_eventos" BackgroundCssClass="modalBackground">
</asp:ModalPopupExtender>
    
    </div>
           

<div id="div_Modal_error_inscripto" runat="server">
                <asp:HiddenField ID="HiddenField_Err" runat="server" />
                <asp:Panel ID="Panel_Modal_Err" runat="server" >
     
                    <div class="card card-danger">
            <div class="card-header">
                <h3 class="card-title">Inscripciones</h3>
            </div>
            <form role="form">
              <div class="card-body"> 
                <div class="row">
                    <div align="right">
                        <asp:Label ID="lbl_Modal_err" runat="server" Text="Ya se encuentra inscripto a este evento." Font-Size="Small"></asp:Label>
                        <br />
                    <asp:Label ID="Label3" runat="server" Text="¿Desea cancelar su inscripción anterior?" Font-Size="Small"></asp:Label>
                    </div>
                </div>
              </div>
            </form>  
            <div align="center">
                    <asp:Button ID="Btn_Modal_si" runat="server" Text="Si" CssClass="btn btn-danger"  />
                    <asp:Button ID="Btn_Modal_no" runat="server" Text="No" CssClass="btn btn-danger"  />
              </div>
                          
              <div>
                 &nbsp;
              </div>             
            </div>
                    <asp:ModalPopupExtender ID="Modal_error_inscripto" runat="server" TargetControlID="HiddenField_Err" PopupControlID="Panel_Modal_Err" CancelControlID="Btn_Modal_no" BackgroundCssClass="modalBackground">
                    </asp:ModalPopupExtender>
             
                 </asp:Panel>

</div>





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
