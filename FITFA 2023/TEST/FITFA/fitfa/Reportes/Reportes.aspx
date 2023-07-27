<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="Reportes.aspx.vb" Inherits="fitfa.Reportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1 {
            font-weight: bold;
            font-size: medium;
        }
        .label-heading {
            font-size: 18px;
            margin-bottom: 5px;
        }
        .label-subheading {
            font-size: 14px;
            color: #666;
            margin-bottom: 10px;
        }
        .form-group {
            margin-bottom: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="card card-primary">
                <div class="card-header">
                    <h3 class="card-title">Informes</h3>
                </div>
                <div>&nbsp;</div>
                <form role="form">
                    <div class="card-body">
                        <div class="form-group row">
                            <div class="col-md-6">
                                <asp:Label ID="Lb_instructor" runat="server" Text="Seleccione instructor:" CssClass="label-heading"></asp:Label>
                                <asp:DropDownList ID="DropDown_instructor" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="Lb_evento" runat="server" Text="Seleccione evento:" CssClass="label-heading"></asp:Label>
                                <asp:DropDownList ID="DropDown_Evento" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btn_buscar" runat="server" Text="Buscar" CssClass="btn btn-primary" />
                        </div>
                        <div class="card-body table-responsive p-0" id="div_grid" visible="false" runat="server">
                            <div align="center" style="background-color: #C0C0C0">
                                <asp:Label ID="Label2" runat="server" Text="Alumnos Inscriptos" Font-Bold="True"></asp:Label>
                            </div>
                            <div class="card-body table-responsive p-0">
                                <asp:Label ID="Label17" runat="server" Font-Underline="True" Text="Listado de inscriptos en:" CssClass="style1" Visible="False"></asp:Label>
                                &nbsp;<asp:Label ID="label_torneo" runat="server" Text="Categoría seleccionada" CssClass="style1" Visible="False"></asp:Label>
                            </div>
                        </div>

                        <div class="card-body table-responsive p-0" id="Inscriptos_curso_examen" runat="server" visible="false">
                            <asp:Label ID="label1" runat="server" Font-Underline="True" Text="Cantidad de inscriptos:" CssClass="style1" Visible="False"></asp:Label>
                            &nbsp;<asp:Label ID="Label_cant" runat="server" Text="Categoría seleccionada" CssClass="style1" Visible="False"></asp:Label>
                            <br />
                            <div class="table-responsive">
                                <asp:GridView ID="GridView1" class="table table-hover table-bordered" runat="server" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black">
                                    <Columns>
                                        <asp:BoundField DataField="usuario_id" HeaderText="ID"></asp:BoundField>
                                        <asp:BoundField DataField="usuario_doc" HeaderText="Documento" />
                                        <asp:BoundField DataField="usuario_apellido" HeaderText="Apellido" />
                                        <asp:BoundField DataField="usuario_nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="graduacion" HeaderText="Graduacion" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="card-body table-responsive p-0" id="Inscriptos_Forma" runat="server" visible="false">
                            <asp:Label ID="label_inscriptosenforma" runat="server" Font-Underline="True" Text="Cantidad de inscriptos en Forma:" CssClass="style1" Visible="true"></asp:Label>
                            &nbsp;<asp:Label ID="Label_cantforma" runat="server" Text="" CssClass="style1" Visible="true"></asp:Label>
                            <br />
                            <div class="table-responsive">
                                <asp:GridView ID="GridView_forma" class="table table-hover table-bordered" runat="server" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black">
                                    <Columns>
                                        <asp:BoundField DataField="usuario_id" HeaderText="ID"></asp:BoundField>
                                        <asp:BoundField DataField="usuario_doc" HeaderText="Documento" />
                                        <asp:BoundField DataField="usuario_apellido" HeaderText="Apellido" />
                                        <asp:BoundField DataField="usuario_nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="graduacion" HeaderText="Graduacion" />
                                        <asp:BoundField DataField="Edad" HeaderText="Edad" />
                                        <asp:BoundField DataField="Sexo" HeaderText="Sexo" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="card-body table-responsive p-0" id="Inscriptos_Lucha" runat="server" visible="false">
                            <asp:Label ID="label_inscriptosenlucha" runat="server" Font-Underline="True" Text="Cantidad de inscriptos en Lucha:" CssClass="style1" Visible="true"></asp:Label>
                            &nbsp;<asp:Label ID="Label_cantlucha" runat="server" Text="" CssClass="style1" Visible="true"></asp:Label>
                            <br />
                            <div class="table-responsive">
                                <asp:GridView ID="GridView_lucha" class="table table-hover table-bordered" runat="server" AutoGenerateColumns="False" AllowSorting="True" BorderColor="Black">
                                    <Columns>
                                        <asp:BoundField DataField="usuario_id" HeaderText="ID"></asp:BoundField>
                                        <asp:BoundField DataField="usuario_doc" HeaderText="Documento" />
                                        <asp:BoundField DataField="usuario_apellido" HeaderText="Apellido" />
                                        <asp:BoundField DataField="usuario_nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="graduacion" HeaderText="Graduacion" />
                                        <asp:BoundField DataField="Peso" HeaderText="Peso" />
                                        <asp:BoundField DataField="Edad" HeaderText="Edad" />
                                        <asp:BoundField DataField="Sexo" HeaderText="Sexo" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div>
                            <asp:Label ID="lbl_err" runat="server" align="center" Visible="false" Text="No Tiene Inscriptos al evento seleccionado" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
                        </div>
                        <br />
                    </div>
                </form>
            </div>

            <!-- Agregar el UpdateProgress -->
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                <ProgressTemplate>
                    <div class="progress">
                        <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
