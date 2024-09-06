<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="ManifestacionEnLinea.Documentos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <div id="hgea"></div>
    <form id="form1" runat="server" style="margin-top:120px;">
        <div class="container">
            <div style="text-align:center">
                <h2 class="titulo">Requisitos para realizar el trámite</h2>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <b>Clave Catastral</b> <br />
                    <asp:TextBox runat="server" ID="Txt_ClaveCatastral" placeholder="Clave Catastral" CssClass="form-control" ReadOnly="true" required="required"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <b>Nombre de solicitante.</b> <br />
                    <asp:TextBox runat="server" ID="Txt_NombreSolicitante" placeholder="Nombre completo" CssClass="form-control" ReadOnly="true" required="required"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <b>Correo Electrónico</b>
                    <asp:TextBox runat="server" ID="Txt_CorreoElectronico" CssClass="form-control" placeholder="Correo Electrónico" ReadOnly="true" required="required"></asp:TextBox>
                </div>
            </div>
            <div id="Div_TipoSolicitante" class="row" style="display:none" runat="server">
                <div class="col-md-6">
                    <b>Tipo de Solicitante</b>
                    <asp:TextBox runat="server" ID="Txt_RechazoTipo" CssClass="form-control" placeholder="Tipo de Solicitante" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div id="Div_NotaRechazo" class="row" style="display:none" runat="server">
                <div class="col-md-6">
                    <b>Nota del Tramitador</b>
                    <asp:TextBox ID="Txt_NotaRechazo" runat="server" CssClass="form-control" TextMode="MultiLine" ReadOnly="true"> </asp:TextBox>
                </div>
            </div>
            <div class="row" runat="server" id="Div_ReqSubdivision" visible="true" >
                <div class="col-md-6">
                    <b>¿Su predio derivo de alguna Subdivisión?</b>
                    <div class="form-check form-check-inline">
                        <asp:RadioButton runat="server" GroupName="radioSubdivision" ID="Rbt_Subdivision_Si" OnCheckedChanged="Rbt_Subdivision_Si_CheckedChanged" Text="Si" CssClass="form-control" AutoPostBack="true"/>
                    </div>
                    <div class="form-check form-check-inline">
                        <asp:RadioButton runat="server" GroupName="radioSubdivision" ID="Rbt_Subdivision_No" OnCheckedChanged="Rbt_Subdivision_No_CheckedChanged" Text="No" CssClass="form-control" Checked="true" AutoPostBack="true"/>

                    </div>
                </div>
            </div>
            <div class="row" runat="server" id="Div_ReqImpresion" visible="true">
                <div class="col-md-6">
                    <b>¿Requiere de la impresión del Plano y Oficio?</b>
                    <div class="form-check form-check-inline">
                        <asp:RadioButton runat="server" GroupName="radioImpresion" ID="Rbt_Impresion_Si" Text="Si" CssClass="form-control" />
                    </div>
                    <div class="form-check form-check-inline">
                        <asp:RadioButton runat="server" GroupName="radioImpresion" ID="Rbt_Impresion_No" Text="No" CssClass="form-control" Checked="true"/>

                    </div>
                </div>
            </div>
            <div class="row" id="Div_ReqTipoSolicitate" runat="server" visible="true">
                <div class="col-md-4">
                    <b>Seleccionar tipo de solicitante</b>
                    <asp:DropDownList ID="Cbo_ListaTipoSolicitante" runat="server" CssClass="form-control" OnSelectedIndexChanged="Cbo_ListaTipoSolicitante_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="SELECCIONE..." Value="0"></asp:ListItem>
                        <asp:ListItem Text="ALBACEA" Value="1"></asp:ListItem>
                        <asp:ListItem Text="APODERADO PERSONA FISICA" Value="2"></asp:ListItem>
                        <asp:ListItem Text="APODERADO PERSONA MORAL" Value="3"></asp:ListItem>
                        <asp:ListItem Text="APODERADO ALBACEA" Value="4"></asp:ListItem>
                        <asp:ListItem Text="PROPIETARIO PERSONA FISICA" Value="5"></asp:ListItem>
                        <asp:ListItem Text="PROPIETARIO PERSONA MORAL" Value="6"></asp:ListItem>
                        <asp:ListItem Text="NOTARIA" Value="7"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <asp:GridView ID="GridRevision" AutoGenerateColumns="false" AllowPaging="false" runat="server" CssClass="table table-bordered table-condensed table-responsive table-hover" OnRowDataBound="GridRevision_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="#" ReadOnly="true" ItemStyle-CssClass="TextoListadoDocumentos text-center" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Documento" HeaderText="Documento" ReadOnly="true" ItemStyle-CssClass="TextoListadoDocumentos2" HeaderStyle-CssClass="text-center"  />
                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" ReadOnly="true" ItemStyle-CssClass="TextoListadoDocumentos text-center" HeaderStyle-CssClass="text-center" />
                        <asp:TemplateField HeaderText="Ver Documento">
                            <ItemTemplate>
                                <asp:LinkButton ID="VerDocumentacion" runat="server" Text="Ver" CssClass="center-content" OnClick="VerDocumentacion_Click" CommandArgument='<%# Container.DataItemIndex %>'><span class="iconify" data-icon="bi:eye" style="font-size:40px;"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Agregar Documento">
                            <ItemTemplate>
                                <div class="center-text">
                                    <asp:LinkButton ID="SubirDocumentacion" runat="server" Text="+ Agregar" CssClass="btn btn-primary" OnClick="SubirDocumentacion_Click" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar Documento">
                                <ItemTemplate>
                                    <div class="center-text">
                                        <asp:LinkButton ID="EliminarDocumentacion" runat="server" Text="Eliminar" CssClass="btn btn-outline-danger" OnClick="EliminarDocumentacion_Click" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
            <div class="row">
                <div class="col-md-6">
                    <asp:Button ID="Btn_EnviarSolicitud" runat="server" CssClass="btn btn-form" Text="Enviar Solicitud" OnClick="Btn_EnviarSolicitud_Click" />
                </div>
            </div>
        </div>
                <!-- Modal para cargar documentos -->
        <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">Agregar Documento</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <!-- Agrega tu asp:FileUpload y otros controles aquí -->
                        <b>Clave Catastral:</b>
                        <asp:TextBox ID="Txt_ClaveModal" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        <b>Nombre del Documento a subir:</b>
                        <asp:TextBox ID="Txt_Archivo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="Hf_NombreArchivo" />
                        <b>Seleccionar Archivo:</b>
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" onchange="validarArchivoPDF(this)" />
                        
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                        <!-- Botón para guardar el archivo -->
                        <%--<asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClientClick="return obtenerFecha();" OnClick="btnGuardar_Click" CssClass="btn btn-primary" />--%>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal para Cargar CLG -->
        <div class="modal fade" id="myModalCLG" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabelCLG">Agregar Documento</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <!-- Agrega tu asp:FileUpload y otros controles aquí -->
                        <b>Clave Catastral:</b>
                        <asp:TextBox ID="Txt_ClaveModalCLG" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        <b>Nombre del Documento a subir:</b>
                        <asp:TextBox ID="Txt_ArchivoCLG" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="HiddenField1" />
                        <b>Seleccionar Archivo:</b>
                        <asp:FileUpload ID="fileUploadCLG" runat="server" CssClass="form-control" onchange="validarArchivoPDF(this)" />
                        <div runat="server" id="Div_FechaExpedicion" >
                            <asp:Label runat="server" ID="Lbl_FechaExpedicion" Text="Fecha Expedición:" Font-Bold="true"></asp:Label>
                            <input type="date" id="expedicion" name="expedicion"/>
                            <asp:HiddenField runat="server" ID="Hf_FechaExpedicion" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                        <!-- Botón para guardar el archivo -->
                        <asp:Button ID="Btn_GuardarCLG" runat="server" Text="Guardar" OnClientClick="return obtenerFecha(this);" OnClick="Btn_GuardarCLG_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </form>
        <div id="fgea" ></div>
    <script>
        function obtenerFecha() {
            var Documento = document.getElementById("<%= Txt_ArchivoCLG.ClientID %>").value;
            var fechaActual = new Date();

            // Obtener el elemento input por su id
            var inputFecha = document.getElementById("expedicion");

            // Obtener el valor de la fecha
            var valorFecha = inputFecha.value;
            console.log("Valor Fecha:" + valorFecha);
            if (valorFecha == "") {
                alert("Ingresar Fecha de Expedicion del Certificado de Libertad de Gravamen.");
                return false;
            }
            else
            {
                // Hacer algo con el valor de la fecha (por ejemplo, mostrarlo en la consola)
                 var FechaExpedicionCLG = document.getElementById("<%= Hf_FechaExpedicion.ClientID %>");
                 FechaExpedicionCLG.value = valorFecha

                 return true;
             }

         }
    </script>
    <script>
        function validarArchivoPDF(input) {
            var fileName = input.value;
            var extension = fileName.substring(fileName.lastIndexOf('.') + 1).toLowerCase();
            const file = input.files[0];
            const maxSize = 30 * 1024 * 1024; // 30MB
            if (extension !== "pdf") {
                alert("Por favor, seleccione un archivo PDF.");
                input.value = ''; // Limpiar el valor del input para deseleccionar el archivo no deseado
            }
            if (file && file.size > maxSize) {
                alert("El archivo supera los 30MB. Por favor, seleccione un archivo más pequeño.");
                input.value = ''; // Reset the input
            }
        }

    </script>
          <script src='<%=ResolveUrl("~/Scripts/sitc.js")%>' type="text/javascript"></script>
    <%--Para las validaceiones de login--%>
    <script src='<%=ResolveUrl("~/Scripts/aes.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/secure.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jsValida.js")%>' type="text/javascript"></script>
    <%--Para Validaciones de control--%>
    <script src='<%=ResolveUrl("~/Scripts/Validaciones.js")%>' type="text/javascript"></script>
    <%--libreria para las alertas--%>
<%--    <script src='<%=ResolveUrl("~/Scripts/sweetalert2.all.min.js")%>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/chosen.jquery.js")%>' type="text/javascript"></script>--%>
    <%--para convertir XML--%>
    <script src='<%=ResolveUrl("~/Scripts/XMLtoJSON.js")%>' type="text/javascript"></script>

</body>
</html>
