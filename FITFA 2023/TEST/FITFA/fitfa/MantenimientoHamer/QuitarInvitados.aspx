<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="QuitarInvitados.aspx.vb" Inherits="fitfa.QuitarInvitados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card card-primary">
                <div class="card-header">
                <h3 class="card-title">MANTENIMIENTO HAMER - QUITAR ALUMNOS INVITADOS<asp:Label ID="Lb_evento" runat="server" Text="CAMBIAR CATEGORIAS EN INSCRIPCION"></asp:Label>      </h3>
                  
              </div>
                <form role="form">
                    <div class="card-body"> 
                        <div class="container-fluid">
                    <div class="row justify-content-center">
                        <div class="col-lg-12">
                            <div class="card">
                                        <div class="card-body">
                                            <div class="row">
                                            <div class="col-sm-4" align="center"></div>
                                            <div class="col-sm-4" align="center">
                  

                                                <div class="form-group">
                                                <asp:Button ID="Btn_QuitarInvitados" runat="server" Text="Quitar Invitados" />
                                                </div>
    
                                            </div>
                                            <div class="col-sm-4" align="center">

                                                <asp:Label ID="Lb_guardado_ok" runat="server" Text="La op se ejecutó correctamente." ForeColor="#339966" Visible="false" ></asp:Label>

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
