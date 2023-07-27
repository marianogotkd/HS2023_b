<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CargarImagen.aspx.vb" Inherits="fitfa.WebForm1" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   
    <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>Registro Federativo</title>

        <!-- CSS -->
        <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Raleway:400,700">
        <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
        <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">
        <link rel="stylesheet" href="assets/css/animate.css">
        <link rel="stylesheet" href="assets/css/style.css">
        <link rel="stylesheet" href="assets/css/media-queries.css">

        <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
        <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
        <!--[if lt IE 9]>
          
        <![endif]-->

        <!-- Favicon and touch icons -->
        <link rel="shortcut icon" href="assets/ico/favicon.png">
        <link rel="apple-touch-icon-precomposed" sizes="144x144" href="assets/ico/apple-touch-icon-144-precomposed.png">
        <link rel="apple-touch-icon-precomposed" sizes="114x114" href="assets/ico/apple-touch-icon-114-precomposed.png">
        <link rel="apple-touch-icon-precomposed" sizes="72x72" href="assets/ico/apple-touch-icon-72-precomposed.png">
        <link rel="apple-touch-icon-precomposed" href="assets/ico/apple-touch-icon-57-precomposed.png">

        <style>
        .modalBackground
        {
            background-color:black;
            filter:alpha(opacity=99) !important;
            opacity:0.6 ! important;
            z-index:20;
            }
        .modalpopup
        {
                      padding:0px 0px 24px 10px;
            position:fixed;
            width:500px;
            height:100px;
            background-color: Black;
            border:2px solid black;
            border-color: Gray; 
                 }
        
        input {
  font-family: monospace;
}
label {
  display: block;
}
div {
  margin: 0 0 1rem 0;
}

        
        
        
        </style>

</head>
<body>
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
                                    EnableScriptGlobalization="True">
                             </asp:ScriptManager>
                               


    <div>
     	
				
        <!-- Top content -->
        <div class="top-content">
            <div class="container">
        	
			    <!--div class="row">
					<div class="col-sm-8 col-sm-offset-2 text">
						<h1 class="wow fadeInLeftBig">Register Landing Page</h1>
						<div class="description wow fadeInLeftBig">
							<p>
								We have been working very hard to create the new version of our app. It comes with a lot of new features. Check it out now!
							</p>
					    </div>
				    </div>
				</div-->

                
                
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             
             <ContentTemplate> 
				
                <div class="row">
				    
                
                	<div class="col-sm-6 col-sm-offset-3 r-form-1-box wow fadeInUp">
				
                
                		<div class="r-form-1-top">
				
                
                
                

                            <div class="r-form-1-top-left">
                                <h3>
                                    Foto personal</h3>
                                    
                                    
                                    
                                    <div>
                                    <asp:Image ID="Image1" runat="server" Height="221px" Width="256px" 
                                            ImageUrl="~/Registro/imagen/usuario-registrado.jpg" />
                                    
                                    </div>
                                    <dix>
                                        <asp:Button ID="Button_adjuntar" runat="server" Text="Seleccionar" 
                                            Visible="False" />
                                    </dix>
                                  

                                <asp:Button ID="Button1" runat="server" Text="Cargar Imagen" BackColor="#00CC99" 
                                    Font-Bold="True" ForeColor="White" />
                             
                                  
                            </div>
                            

                         
                         
                                                  
                 <div class="form-group">
                
                     <asp:HiddenField ID="HiddenMostrar" runat="server" />
                     
                 </div>
                      


                 <div id="div_registroexitoso" runat="server" visible="false" style="color: #99CCFF">
                 Registro exitoso!
                 </div>
                            

                 

        				    </div>
                            </div>
                            </div>


                            <div>
                            <asp:Panel ID="Panel1" runat="server" CssClass="modalpopup">
                                            <div>
                                            
                                            <div style="color: #FF00FF; "> 
                                            Foto de perfil
                                            </div>
                                                    
                                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                                                                                
                                                <asp:Button ID="Btn_aceptar" runat="server" Text="Aceptar" 
                                                    BackColor="#00CC99" Font-Bold="True" ForeColor="White" />&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="Btn_cancelar"
                                                    runat="server" Text="Cancelar" BackColor="#FF6666" Font-Bold="True" 
                                                    ForeColor="White" />       
        
                                            
        
        
                                            </div>
                                 </asp:Panel>
                                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                                    CancelControlID="Btn_cancelar" PopupControlID="Panel1" 
                                    TargetControlID="Button1" BackgroundCssClass="modalBackground">
                                </cc1:ModalPopupExtender>  
                            </div>


                            <asp:Panel ID="Panel2" runat="server" CssClass="modalpopup">
                                <asp:Label ID="Label12" runat="server" Text="Registro exitoso!"></asp:Label>
                                <br />
                                <asp:Button ID="Btn_ok" runat="server" Text="OK" />
                            </asp:Panel>
                            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="Panel2" TargetControlID="HiddenMostrar" BackgroundCssClass="modalBackground">
                            </cc1:ModalPopupExtender>


                            <asp:HiddenField ID="Hidden_Error1" runat="server" />
                            <asp:Panel ID="Panel_error1" runat="server" CssClass="modalpopup">
                                <asp:Label ID="Label13" runat="server" Text="Error, complete la info solicitada!"></asp:Label>
                                <br />
                                <asp:Button ID="Btn_error1_ok" runat="server" Text="OK" />
                            </asp:Panel>
                 <cc1:ModalPopupExtender ID="ModalPopupExtender3" runat="server" PopupControlID="Panel_error1" TargetControlID="Hidden_Error1" BackgroundCssClass="modalBackground">
                 </cc1:ModalPopupExtender>

                            <div>
                            <div class="card-footer">
                  
                </div>


                            </div>
                                


      </ContentTemplate> 
                 <Triggers>
                  
                     <asp:PostBackTrigger ControlID="Btn_aceptar" />
                 </Triggers>
      </asp:UpdatePanel>
                            
                            




                            
                            </div>
        </div>
        				

                        
					
				
                
            
        
						
        

        <!-- Features -->
        <!--div class="features-container section-container">
	        <div class="container">
	            <div class="row">
	                <div class="col-sm-12 features section-description wow fadeIn">
	                    <h2>Why You Should Join? . . . . . . .</div>
	                    <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et.
	                    	Ut wisi enim ad minim veniam, quis nostrud tempor incididunt ut labore et.</p>
	                </div>
	            </div>
	            <div class="row">
                	<div class="col-sm-4 features-box wow fadeInUp">
	                	<div class="features-box-icon"><i class="fa fa-thumbs-up"></i></div>
	                    <h3>Easy To Use</h3>
	                    <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt.</p>
                    </div>
                    <div class="col-sm-4 features-box wow fadeInDown">
	                	<div class="features-box-icon"><i class="fa fa-cog"></i></div>
	                    <h3>Responsive Design</h3>
	                    <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt.</p>
                    </div>
                    <div class="col-sm-4 features-box wow fadeInUp">
	                	<div class="features-box-icon"><i class="fa fa-commenting"></i></div>
	                    <h3>24/7 Support</h3>
	                    <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt.</p>
                    </div>
	            </div>
	            <div class="row">
	            	<div class="col-sm-12 section-bottom-button wow fadeInUp">
                        <a class="btn btn-link-1 scroll-link" href="#top-content">Sign Up Now <i class="fa fa-angle-right"></i></a>
	            	</div>
	            </div>
	        </div-->
        
        
        
        
        </div>
    
    




        <!-- Javascript -->
        <script src="assets/js/jquery-1.11.1.min.js"></script>
        <script src="assets/bootstrap/js/bootstrap.min.js"></script>
        <script src="assets/js/jquery.backstretch.min.js"></script>
        <script src="assets/js/wow.min.js"></script>
        <script src="assets/js/retina-1.1.0.min.js"></script>
        <script src="assets/js/scripts.js"></script>
       <%-- <script src="//static.codepen.io/assets/common/stopExecutionOnTimeout-41c52890748cd7143004e05d3c5f786c66b19939c4500ce446314d1748483e13.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
        <script src="https://s3-us-west-2.amazonaws.com/s.cdpn.io/3/jquery.inputmask.bundle.js"></script>--%>

        <!--[if lt IE 10]>
            <script src="assets/js/placeholder.js"></script>
        <![endif]-->

        <script>
            $(":input").inputmask();

            $("#phone").inputmask({ "mask": "(999) 999-9999" });

        </script>


    </div>
    </form>
</body>
</html>

