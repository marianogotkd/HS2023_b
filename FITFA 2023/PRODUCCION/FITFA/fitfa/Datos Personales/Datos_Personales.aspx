<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="Datos_Personales.aspx.vb" Inherits="fitfa.Datos_Personales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Import Namespace="System.Web.UI.WebControls" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
  </asp:ScriptManager>

   <asp:UpdatePanel ID="upp" runat="server">
        <ContentTemplate>
       
       
       
<div class="card card-primary">
              <div class="card-header">
                <h3 class="card-title">Datos Personales</h3>
              </div>
              <!-- /.card-header -->
              <!-- form start -->
              <form role="form">
                <div class="card-body">
                  
                  <div class="row">
                  <div class="col-md-4 col-center">  
                  <div class="form-group" >
                    <%--<label>Nombre</label>--%>
                    <label>Nombre:</label>
                    <label id="lbl_errNom" class="label label-danger" runat="server">Debe Completar El Campo</label>
                      <input type="text" class="form-control" id="tb_nombre" runat="server" causesvalidation="False" required="" placeholder="Ingrese nombre..." maxlength="50"/>
                      
                  </div>
                    <div class="form-group">
                   <%-- <label for="exampleInputEmail1">Apellido</label>--%>
                    <label>Apellido:</label>
                    <label id="lbl_errApe" class="label label-danger" runat="server">Debe Completar El Campo</label>
                    <input type="text" class="form-control" id="tb_apellido" runat="server" 
                          required="" placeholder="Ingrese apellido..." maxlength="50">
                      
                  </div>
              
                 <div class="form-group">
                  <label>Fecha de Nacimiento:</label>
                  <label id="lbl_errFecNac" class="label label-danger" runat="server">Debe Completar El Campo</label>
                  <div class="input-group">
                    <div class="input-group-prepend">
                      <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                    </div>
                    <input type="text" class="form-control" runat="server" id="tb_fechnacc" data-inputmask="'alias': 'dd/mm/yyyy'" data-mask="">
                  </div>
                  <!-- /.input group -->
                </div>

                 <div class="form-group">
                    <label>DNI</label>
  <label id="lbl_costo" class="label label-danger" visible="false" runat="server">Debe Completar El Campo</label>
  <asp:TextBox ID="tb_dni" CssClass="form-control" runat="server"  onkeypress="return justNumbers(event);"></asp:TextBox>  
  <cc1:FilteredTextBoxExtender 
            ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tb_dni"
      FilterType="Numbers" ValidChars="0123456789" >
        </cc1:FilteredTextBoxExtender>
        </div>
        
                   <div class="form-group">
                  <label>Sexo:</label>
                  <asp:DropDownList ID="combo_Sexo" runat="server" class="form-control">
                      <asp:ListItem Value="Hombre" Selected="True">Hombre</asp:ListItem>
                      <asp:ListItem Value="Mujer">Mujer</asp:ListItem>
                   </asp:DropDownList>
                </div>
                 
                              <div class="form-group">
               <%--   <label >Estado Civil:</label>--%>
                   <asp:DropDownList ID="combo_EstCivil" runat="server" class="form-control" Visible=false>
                   </asp:DropDownList>
                  </div>


                     <div class="form-group">
                  <%--  <label for="exampleInputEmail1">Domicilio</label>--%>
                    <label>Domicilio:</label>
                    <label id="lbl_errDir" class="label label-danger" runat="server">Debe Completar El Campo</label>
                    <input type="text" class="form-control" id="tb_dir" runat="server" 
                             placeholder="Ingrese domicilio..." maxlength="50">
                     </div>

                     
                      <div class="form-group">
                   <%-- <label for="exampleInputEmail1">Codigo Postal</label>--%>
                    <label>Código Postal (CP):</label>
                    <label id="lbl_errCP" class="label label-danger" runat="server">Debe Completar El 
                          Campo</label><asp:TextBox ID="textbox_CP" class="form-control" runat="server" MaxLength="50" AutoPostBack="True"></asp:TextBox>
                          <cc1:FilteredTextBoxExtender ID="Filteredtextbox_CPExtender1" Enabled="True" TargetControlID="textbox_CP" ValidChars="0123456789" runat="server">
                          </cc1:FilteredTextBoxExtender>

                  </div>
                     
                     </div> <%--cierra el col-md-4 col-center--%>
                    
                    <div class="col-md-4 col-center">
                     

                  <div class="form-group">
                  <label>Provincia</label>
                  <asp:DropDownList ID="Combo_provincia" runat="server" class="form-control" AutoPostBack="true">
                   </asp:DropDownList>
                   <%--CssClass="form-control select2"--%>
                  </div>

                  <div class="form-group">
                  <label>Ciudad</label>
                   <asp:DropDownList ID="combo_ciudad" runat="server" class="form-control">
                   </asp:DropDownList>
                  </div>
             

                   <div class="form-group">
                  <%--  <label for="exampleInputEmail1">Telefono</label>--%>
                    <label>Teléfono:</label>
                    <label id="lbl_errTel" class="label label-danger" runat="server">Debe Completar El Campo</label>
                    <input type="text" class="form-control" id="tb_tel" runat="server" placeholder="Ingrese teléfono..." maxlength="50"/>
                  </div>

                  <div class="form-group">
                   <%-- <label for="exampleInputEmail1">Correo Electronico</label>--%>
                    <label>Email:</label>
                    <label id="lbl_errMail" class="label label-danger" runat="server">Debe Completar El Campo</label>
                    <input type="text" class="form-control" id="tb_Email" runat="server" placeholder="Ingrese email..." maxlength="50"/>
                  </div>

                  <div class="form-group">
                  <label>Nro. Libreta:</label>
                    <label id="lbl_err_libreta" class="label label-danger" runat="server">Debe Completar El Campo</label>
                    <input type="text" class="form-control" id="tb_nrolibreta" runat="server" placeholder="Ingrese número de libreta..." maxlength="50"/>
                  </div>

                  <div class="form-group">
                  <label>Graduación:</label>
                  <input type="text" class="form-control" id="tb_grad" runat="server" maxlength="50" readonly="readonly"/>
                  <asp:DropDownList ID="Combo_graduacion" runat="server" class="form-control" visible="false">
                   </asp:DropDownList>
                   </div>

                       <div class="form-group">
                  <label>Instructor:</label>
                   <input type="text" class="form-control" id="tb_inst" runat="server" maxlength="50" readonly="readonly"/>
                  <asp:DropDownList ID="cmb_instructor" runat="server" class="form-control" visible="false">
                   </asp:DropDownList>
                   </div>
                 

                    </div> <%--cierra el col-md-4 col-center--%>

                    <div class="col-md-4 col-center">

                  <div class="form-group">
                    <label>Usuario:</label>
                    <label id="lbl_errus" class="label label-danger" runat="server">Debe Completar El Campo</label>
                    <input type="text" class="form-control" id="tb_us" runat="server" required=""  maxlength="50"/>
                  </div>

                                 
                  <div class="form-group">             
                    <label>Contraseña:</label>
                    <label id="lbl_errpass" class="label label-danger" runat="server">Debe Completar El Campo</label>
                    <input type="password" class="form-control" id="tb_pass" placeholder="Ingrese Nueva Contraseña" runat="server" required=""  maxlength="50"/>
                  </div>

<%--////////////FOTO 2023/////////////////////--%>
                      
  
             <div id="seccion_foto" runat="server" visible="false" class="form-group" > 
                         <%-- para que funcione tiene que ser la imagen tipo asp:imagen no otra--%>
                     <asp:Image ID="imgPrev" runat="server" width="200" height="200"></asp:Image>

                 <br />
                   <br />

                 <asp:Button Id="btn_cargarImagen"  runat="server" Text="Cargar Nueva Imagen" />
                 
          

             </div>

                   



                               </div>  <%--cierra el col-md-4 col-center--%>


                  
                    </div>
                      

                      



                  
                  </div>
                  
                  
              

                    </form>
            </div>
        
       
                 
                <div class="card-footer">
                    <asp:Button id="btn_guardar" runat="server" Text="Guarda Cambios" class="btn btn-primary" />
                    <br />

                  
                </div>
                <div id="div_registro_guardado" runat="server" visible="false" 
                style="color: #00CC00; font-style: normal; font-variant: normal;">
                    Datos actualizados!
                </div>
            

          

           
            

          
        <div id="div_modalmsjOK" runat="server">
                                <asp:HiddenField ID="HiddenField_msj" runat="server" />
                                    <asp:Panel ID="Panel1" runat="server" >
              
                                        <div class="card card-success">
                                        <div class="card-header">
                                            <h3 class="card-title">Datos personales</h3>
                                        </div>
                                        <form role="form">
                                          <div class="card-body"> 
                                            <div class="row">
                                                <div align="center">
                                                    <asp:Label ID="Label1" runat="server" Text="Datos actualizados exitosamente."></asp:Label>
                                                    &nbsp;
                                                </div>
                                            </div>
                                          </div>
                                        </form>  
                                        <div align="center">
                                                <asp:Button ID="boton_ok" runat="server" Text="OK" CssClass="btn btn-success"  />
                                          </div> 
                                          <div>
                                             &nbsp;
                                          </div>             
                                        </div> 
                                    </asp:Panel>
        
            <cc1:ModalPopupExtender ID="Modal_msjOK" runat="server" TargetControlID="HiddenField_msj" PopupControlID="Panel1" CancelControlID="boton_ok" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
                            

    
        </div>





     </ContentTemplate>
        
        </asp:UpdatePanel>
       


       <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
            AssociatedUpdatePanelID="upp">
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

 
 