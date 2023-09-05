<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home.Master" CodeBehind="Cob_CobroCliente.aspx.vb" Inherits="Presentacion.Cob_CobroCliente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeOut="7200"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div class="card card-primary">
<div class="card-header">
                <h3 class="card-title">COBRAR CLIENTE A/B/M</h3>
</div>
<form role="form">
<%--<div class="card-body"> --%>
        <div class="container-fluid">
            <div class="row justify-content-center">
            <div class="col-lg-12">
                   <%-- <div class="card"> tercer card--%>
                            <%--<div class="card-body">--%>
                            <div class="form-group">
                            <div class="row justify-content-center">
                                    <div class="col-md-12"> <%--col-md-4--%>
                                      <asp:Label ID="Lb_ctacte" runat="server" Text="CTACTE:"></asp:Label>
                                      <asp:HiddenField ID="HF_CLIE_ID" runat="server" />
                                      <asp:HiddenField ID="HF_LOCAL_ID" runat="server" />
                                        <br />
                                      <asp:Label ID="Lb_cliente" runat="server" Text="CLIENTE:"></asp:Label>
                                      
                                        <br />
                                      <asp:Label ID="Lb_codlocal" runat="server" Text="COD.LOCAL:"></asp:Label>
                                      
                                        <br />
                                      <asp:Label ID="Lb_local" runat="server" Text="LOCAL:"></asp:Label>
                                      
                                        <br />
                                      <asp:Label ID="Lb_ctactesaldo" runat="server" Text="SALDO DEUDOR:$"></asp:Label>
                                      
                                    </div>
                             
                            </div>
                            </div>
                            
                            <div id="Div1" class="row justify-content-center" visible="True" runat="server">
                    <div class="col-md-12">
                    <div class="card card-secondary">
                    <div class="card-header">
                      <h3 class="card-title">TARIFAS A COBRAR:</h3>      
                              <%--<h3 class="card-title">RESUMEN A LA FECHA:</h3>--%>

                              
                    </div>
                    <!-- /.card-header -->
                    <div class="card-body"> 
                      <div id="Seccion01" runat="server" visible="false">
                        <asp:HiddenField ID="HF01_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF01_TARCLIE_ID" runat="server" />
                        
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular01"  runat="server" Visible="false" />
                                      <asp:Label ID="Lb_anular01" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>

                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha01">Fecha:</label>
                                      <asp:Label ID="Lb_fecha01info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa01">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa01info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo01">Saldo:</label>
                                      <asp:Label ID="Lb_saldo01info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga01">Paga:</label>
                                      <asp:TextBox ID="Txt_paga01" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
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
                        <asp:HiddenField ID="HF02_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF02_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular02" runat="server" Visible="false" />
                                      <asp:Label ID="Lb_anular02" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha02" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha02info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa02">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa02info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo02">Saldo:</label>
                                      <asp:Label ID="Lb_saldo02info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga02">Paga: </label>
                                      <asp:TextBox ID="Txt_paga02" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
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
                        <asp:HiddenField ID="HF03_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF03_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular03" runat="server" Visible="false"/>
                                      <asp:Label ID="Lb_anular03" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha03" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha03info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa03">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa03info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo03">Saldo:</label>
                                      <asp:Label ID="Lb_saldo03info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga03">Paga: </label>
                                      <asp:TextBox ID="Txt_paga03" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
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
                        <asp:HiddenField ID="HF04_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF04_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular04" runat="server" Visible="false"/>
                                      <asp:Label ID="Lb_anular04" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha04" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha04info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa04">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa04info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo04">Saldo:</label>
                                      <asp:Label ID="Lb_saldo04info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga04">Paga: </label>
                                      <asp:TextBox ID="Txt_paga04" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
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
                        <asp:HiddenField ID="HF05_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF05_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular05" runat="server" Visible="false"/>
                                      <asp:Label ID="Lb_anular05" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha05" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha05info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa05">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa05info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo05">Saldo:</label>
                                      <asp:Label ID="Lb_saldo05info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga05">Paga: </label>
                                      <asp:TextBox ID="Txt_paga05" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      </div>
                      <div id="Seccion06" runat="server" visible="false">
                        <asp:HiddenField ID="HF06_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF06_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular06" runat="server" Visible="false"/>
                                      <asp:Label ID="Lb_anular06" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha06" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha06info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa06">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa06info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo06">Saldo:</label>
                                      <asp:Label ID="Lb_saldo06info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga06">Paga: </label>
                                      <asp:TextBox ID="Txt_paga06" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      </div>
                      <div id="Seccion07" runat="server" visible="false">
                        <asp:HiddenField ID="HF07_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF07_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular07" runat="server" Visible="false"/>
                                      <asp:Label ID="Lb_anular07" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha07" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha07info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa07">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa07info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo07">Saldo:</label>
                                      <asp:Label ID="Lb_saldo07info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga07">Paga: </label>
                                      <asp:TextBox ID="Txt_paga07" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      </div>
                      <div id="Seccion08" runat="server" visible="false">
                        <asp:HiddenField ID="HF08_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF08_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular08" runat="server" Visible="false"/>
                                      <asp:Label ID="Lb_anular08" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha08" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha08info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa08">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa08info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo08">Saldo:</label>
                                      <asp:Label ID="Lb_saldo08info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga08">Paga: </label>
                                      <asp:TextBox ID="Txt_paga08" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      </div>
                      <div id="Seccion09" runat="server" visible="false">
                        <asp:HiddenField ID="HF09_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF09_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular09" runat="server" Visible="false"/>
                                      <asp:Label ID="Lb_anular09" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha09" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha09info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa09">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa09info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo09">Saldo:</label>
                                      <asp:Label ID="Lb_saldo09info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga09">Paga: </label>
                                      <asp:TextBox ID="Txt_paga09" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      </div>
                      <div id="Seccion10" runat="server" visible="false">
                        <asp:HiddenField ID="HF10_CTACTEDET_ID" runat="server" />
                        <asp:HiddenField ID="HF10_TARCLIE_ID" runat="server" />
                        <div class="container-fluid">
                        <div class="row justify-content-center">
                          <div class="col-lg-12">
                            <div class="card">
                              <div class="card-body">
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    <div class="col-lg-auto">
                                      <asp:CheckBox ID="Chkbox_anular10" runat="server" Visible="false"/>
                                      <asp:Label ID="Lb_anular10" runat="server" Text="Anular" ForeColor="#0099FF" Visible="false"></asp:Label>
                                    </div>
                                  </div>
                                </div>
                                <div class="form-group">
                                  <div class="row justify-content-lg-start">
                                    
                                    <div class="col-md-3">
                                      <label for="Lb_fecha10" runat="server" >Fecha:</label>
                                  <asp:Label ID="Lb_fecha10info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-5">
                                      <label for="Lb_tarifa10">Tarifa:</label>
                                      <asp:Label ID="Lb_tarifa10info" runat="server" Text=""></asp:Label>
                                      
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_saldo10">Saldo:</label>
                                      <asp:Label ID="Lb_saldo10info" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                      <label for="Lb_Paga10">Paga: </label>
                                      <asp:TextBox ID="Txt_paga10" runat="server" placeholder="0,00" CausesValidation="True" validationgroup="check_2" xmlns:asp="#unknown2" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="100px"></asp:TextBox>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      </div>                 
                      <div if ="secciongrid" runat="server" visible="false">
                        <div class="card-body table-responsive p-0"  onkeydown="tecla_op_botones(event);"> <%--div class="form-group"--%>
                            <asp:GridView ID="GridView1" runat="server" class="table table-bordered table-hover table-responsive-sm" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False" 
                                   BorderColor="Black" GridLines="None" 
                                  EnableSortingAndPagingCallbacks="True"> 
                                    <Columns>
                                      <asp:TemplateField HeaderText="X">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              <asp:CheckBox ID="CheckBox_Anular" runat="server" />
                                                <%--<asp:Button ID="Button1" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="IR" Width="70px" CommandName="ID" CommandArgument='<%# Eval("CLIE_ID") %>' />--%>
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>  
                                      <asp:BoundField DataField="TARCLIE_ID" HeaderText="ID" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        
                                      
                                      <asp:BoundField DataField="TARCLIE_precio" HeaderText="SALDO" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>     
                                                                            

                                        <asp:TemplateField HeaderText="PAGO">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                              <asp:TextBox ID="Txt_pago" runat="server" placeholder="0,00" MaxLength="17" onkeydown="tecla_op(event);" onkeypress="return validateDecimalKeyPress(this, event);" Width="70"></asp:TextBox>
                                                <%--<asp:Button ID="Button1" runat="server" Font-Bold="True" ForeColor="Black" 
                                                    Text="IR" Width="70px" CommandName="ID" CommandArgument='<%# Eval("CLIE_ID") %>' />--%>
                                            </ItemTemplate>
                                            <HeaderStyle ForeColor="#0099FF" />
                                        </asp:TemplateField>
                                      <asp:BoundField DataField="TARCLIE_desc" HeaderText="TARIFA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                      <asp:BoundField DataField="Fecha" HeaderText="FECHA" >                                                               
                                        <HeaderStyle ForeColor="#0099FF" />
                                        </asp:BoundField>
                                        
                                    </Columns>
                                </asp:GridView>
                        </div>
                      </div>
                      
<%--                      <div class="form-check">
                        <asp:CheckBoxList ID="Chkbox_anulartodo" runat="server"></asp:CheckBoxList>
                    <label class="form-check-label" for="exampleCheck1">Anular todo.</label>
                  </div>--%>
                    </div>
                      


                    </div>
                    </div>
                    </div>
                            
                            
                            
                            
                            <%--</div> segundo card--%>
                   <%-- </div> 3er card--%>
            </div>
            </div>
        </div>
<%--</div> fin card body--%>
</form>
</div>

<div class="card-footer">
        <div class="row justify-content-center" >
        

         <div class="row align-items-center">
            
                <div class="form-group">
                  <button type="submit" UseSubmitBehavior="false" class="btn btn-primary" runat="server" id="btn_retroceder" onkeydown="tecla_op_botones(event);">RETROCEDE</button>
                    &nbsp;
                    

        
        
                    </div>

           <div class="form-group">
                                <button type="button" Class="btn btn-primary" id = "BOTON_GRABA" runat="server" onkeydown="tecla_op_botones(event);">GRABAR</button>
        
                            </div>

           

                    
                 
                
         </div>

        </div>
        

</div>


<!-- Modal BAJA CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_eliminar" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H1">Eliminar registro</h5>
        <button type="button" id="btn_eliminar1_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        ¿Confirma la operación?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_eliminar_mdl_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_eliminar_mdll" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>


<%--Modal MENSAJE OK ELIMINADO CORRECTAMENTE--%>
<div class="modal fade" id="modal-sm_OKELIMINADO" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Eliminar registro</h4>
              <button type="button" id="btn_ELIMINAR_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Se eliminó correctamente!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_elimnar" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
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
        <h5 class="modal-title" id="H2">Dar de Baja</h5>
        <button type="button" id="Button3" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        ¿Confirma la operación?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_baja_mdl_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_baja_mdll" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>

<%--Modal MENSAJE OK ELIMINADO CORRECTAMENTE--%>
<div class="modal fade" id="modal-sm_OKBAJA" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Dar de baja</h4>
              <button type="button" id="btn_BAJA_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Baja correcta!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_baja" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


<%--Modal MENSAJE OK ERROR_BUSQUEDA--%>
<div class="modal fade" id="modal_error_busqueda" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_close_error_busqueda" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>La busqueda no arrojó resultados!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_ok_error_busqueda" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->

  <%--Modal MENSAJE OK ERROR_BUSQUEDA--%>
<div class="modal fade" id="modal_error_carga" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_close_error_carga" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Consulte al administrador!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="Btn_ok_error_carga" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->

  <!-- Modal GRABA PREGUNTA CENTRADO EN PANTALLA -->
<div class="modal fade" id="Mdl_cobro" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="H3">GRABAR</h5>
        <button type="button" id="btn_cobro_close" class="close" runat="server" tabindex="-1" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        Confirma la operacion de cobro?...
      </div>
      <div class="modal-footer">
        <button type="button" id="btn_cobro_cancelar" class="btn btn-secondary" runat="server" data-dismiss="modal">Cancelar</button>
        <button type="button" id="btn_cobro_confirmar" class="btn btn-primary" runat="server" data-dismiss="modal">Confirmar</button>
      </div>
    </div>
  </div>
</div>

  <%--Modal MENSAJE OK GRABADO--%>
<div class="modal fade" id="modal-sm_OKGRABADO" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">GRABADO</h4>
              <button type="button" id="btn_grabado_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Se guardo correctamente!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_grabado_ok" runat="server" class="btn btn-primary" data-dismiss="modal" OnClientClick="window.open('/COB_Cobradores/Comprobante.pdf','_blank')" >OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->

  <%--Modal MENSAJE OK ERROR_BUSQUEDA--%>
<div class="modal fade" id="modal_error_validacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Error</h4>
              <button type="button" id="btn_errorvalidacion_close" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Ingreso invalido, complete la info solicitada!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <button type="button" id="btn_errorvalidacion_ok" runat="server" class="btn btn-primary" data-dismiss="modal">OK</button>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->


  <div class="modal fade" id="modal_reporte" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm modal-dialog-centered " role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h4 class="modal-title">Reporte</h4>
              <button type="button" id="Button1" runat="server" class="close" tabindex="-1" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <p>Operacion finalizada, ver comprobante!&hellip;</p>
            </div>
            <div class="modal-footer justify-content-center ">
            <%--<div class="modal-footer justify-content-between">--%>
              <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
              <%--<button type="button" id="Button2" runat="server" class="btn btn-primary" data-dismiss="modal" OnClientClick="window.open('/COB_Cobradores/Comprobante.pdf','_blank')">OK</button>--%>
            
            <asp:Button ID="BTN_IMPRIMIR" runat="server" Text="OK..." class="btn btn-primary" data-dismiss="modal" onkeydown="tecla_op_botones(event);" OnClientClick="window.open('/COB_Cobradores/Comprobante.pdf','_blank')" />
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->





</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
