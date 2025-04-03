<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubirArchivos.aspx.cs" Inherits="ManifestacionEnLinea.SubirArchivos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        body{
            background-image: url(Imagenes/Iconos/fondo.jpg);
        }
        
        .hidden{
            display:none;
        }
        
        .infoCat{
         width: 20px;
         height: 20px;

        }

    </style>
   

    <script type="text/javascript">
        function warningsalert(msg) {
            swal({
                title: '¡Atención!',
                icon: 'warning',
                text: msg,
                type: 'warning'
            })
        }
        function sussessalert(msg) {
            swal({
                title: 'Exitoso',
                icon: 'success',
                text: msg,
                type: 'success'
            })
        }

    </script>

    <script>
        function ValidarFormulario() {
            var Tram = document.getElementById("<%=TxtTramite.ClientID %>").value;
            var NombreSolicitante = document.getElementById("<%= inputSolicitanteUpload.ClientID %>").value;
            var CorreoElectronico = document.getElementById("<%= CorreoElectronico.ClientID %>").value;
            var Btn_Enviar = document.getElementById("BotonEnviar");
            if (Tram === "MANIFESTACIÓN DE CONSTRUCCIÓN")
            {
                if (NombreSolicitante != "" && CorreoElectronico != "") {
                    document.getElementById("<%= UploadButton.ClientID  %>").click();
                    Btn_Enviar.disabled = true;
                }
                else
                {
                    if (NombreSolicitante == "")
                    {
                        warningsalert("Ingresar Nombre del Solicitante");
                    }
                    else
                    {
                        if (CorreoElectronico == "")
                        {
                            warningsalert("Ingresar correo electronico");
                        }
                    }
                }
            }
            if (Tram === "AVALÚO CATASTRAL") {
                if (NombreSolicitante != "" && CorreoElectronico != "") {
                    document.getElementById("<%= UploadButton.ClientID  %>").click();
                    Btn_Enviar.disabled = true;
                }
                else {
                    if (NombreSolicitante == "") {
                        warningsalert("Ingresar Nombre del Solicitante");
                    }
                    else {
                        if (CorreoElectronico == "") {
                            warningsalert("Ingresar correo electronico");
                        }
                    }
                }
            }
            if (Tram === "MANIFESTACIÓN CON AVALÚO") {
                var FolioPago = document.getElementById("<%= TxtFolioPago.ClientID %>").value;
                if (NombreSolicitante != "" && CorreoElectronico != "")
                {
                    document.getElementById("<%= UploadButton.ClientID  %>").click();
                    Btn_Enviar.disabled = true;
                }
                else
                {
                    //if (FolioPago == "")
                    //{
                    //    warningsalert("Ingresar Folio de Pago del Avaluo");
                    //}
                    //else
                    //{
                        if (NombreSolicitante == "")
                        {
                            warningsalert("Ingresar Nombre del Solicitante");
                        }
                        else
                        {
                            if (CorreoElectronico == "")
                            {
                                warningsalert("Ingresar correo electronico");
                            }
 
                        }
                   /* }*/
                    
                }
                /*$("#mymodalAceptarManiAvaluo").modal('show');*/
            }
            
        }
    </script>
  
    <script>
        function Leyendain() {
            var imggoin = document.getElementById("<%= SelectFiles.ClientID %>");
            imggoin.style.display = "block";
        }
        function Leyendaout() {
            var imggoin = document.getElementById("<%= SelectFiles.ClientID %>");
            imggoin.style.display = "none";
        }

    </script>
    <script>
        function Leyendacfdiin() {
            var imgcfdigoin = document.getElementById("<%= cfdiExample.ClientID %>");
            var imgpagogoin = document.getElementById("<%= PagoEnLinea.ClientID %>");
            var lblpagoin = document.getElementById("<%= LblReferencia.ClientID %>");
            imgcfdigoin.style.display = "block";
            imgpagogoin.style.display = "block";
            lblpagoin.style.display = "block";
        }
        function Leyendacfdiout() {
            var imgcfdigoin = document.getElementById("<%= cfdiExample.ClientID %>");
            var imgpagogoin = document.getElementById("<%= PagoEnLinea.ClientID %>");
            var lblpagoin = document.getElementById("<%= LblReferencia.ClientID %>");
            imgcfdigoin.style.display = "none";
            imgpagogoin.style.display = "none";
            lblpagoin.style.display = "none";

        }
    </script>
        <link rel="Shortcut Icon" type="image/ico" href="https://eservicios2.aguascalientes.gob.mx/portalgea/images/Favicon.ico" />
    <link href="../Content/style.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="https://eservicios2.aguascalientes.gob.mx/portalgea/css/tema-sae.css"/>

     <script src="https://eservicios2.aguascalientes.gob.mx/portalgea/js/geavue.js" type="text/javascript"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/mobile-detect@1.4.4/mobile-detect.min.js" type="text/javascript"></script>
    <script src="https://code.iconify.design/iconify-icon/1.0.0-beta.3/iconify-icon.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.5/dist/umd/popper.min.js" integrity="sha384-Xe+8cL9oJa6tN/veChSP7q+mnSPaj5Bcu9mPX5F5xIGE0DVittaqT5lorf0EI7Vk" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.min.js" integrity="sha384-ODmDIVzN+pFdexxHEHFBQH3/9/vQ9uori45z4JjnFsRydbmQbmL5t1tQ0culUzyK" crossorigin="anonymous"></script>

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
</head>
<body>
    <div id="hgea"></div>
    <br />
    <br />
    <br />
    <br /><br />
    <form id="formArchivos" method="post" runat="server">
        <div class="container">
          <div id="SubidaArchivos">
              <h4 class="subtitulo"><b>SUBIR DOCUMENTOS DIGITALIZADOS</b></h4>
              <div class="row">
                  <div class="col-md-4">
                      <b>CLAVE CATASTRAL</b>
                      <asp:TextBox runat="server" ID="cuentasubFiles"   CssClass="form-control" ReadOnly="true"  />
                  </div>
              </div>
              <div class="row">
                  <div class="col-md-4">
                      <b>TRÁMITE A REALIZAR</b>
                      <asp:TextBox runat="server" ID="TxtTramite"   CssClass="form-control" ReadOnly="true"  />
                  </div>
              </div>
              
              <div class="row" runat="server" id="divFolioPago">
                  <b>1. Ingresar folio de pago CFDI o Referencia</b>
                  <div class="col-md-4">
                      <b> FOLIO DE PAGO </b> <asp:Image runat="server" ID="ImageInfocfdi" onmouseover="Leyendacfdiin(this)" onmouseout="Leyendacfdiout(this)" CssClass="infoCat" ImageUrl="~/Imagenes/Iconos/info.png" /><br />
                      <asp:Image runat="server" ID="cfdiExample" ImageUrl="~/Imagenes/Pago/ReciboCajas.png" Style="display:none; padding:10px;" />
                      <asp:TextBox runat="server" ID="TxtFolioPago" CssClass="form-control" ></asp:TextBox>
                  </div>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                  <div class="col-md-4">
                      <label runat="server" id="LblReferencia" style="display:none;">Referencia</label>
                      <asp:Image runat="server" ID="PagoEnLinea" ImageUrl="~/Imagenes/Pago/AVALUO.png" Style="display:none;"  />
                  </div>
              </div>
              <b> Ingresar nombre completo.</b>
              <div class="row">
                  <div class="col-md-4">
                      <b>NOMBRE DEL SOLICITANTE </b>
                      <asp:TextBox runat="server" ID="inputSolicitanteUpload" Cssclass="form-control"  />
                  </div>
              </div>
              <b>Ingresar correo electronico</b>
              <div class="row">
                  <div class="col-md-4">
                      <b>CORREO ELECTRONICO</b>
                      <asp:TextBox runat="server" ID="CorreoElectronico" Cssclass="form-control"  />
                      <asp:Label runat="server">Respetar mayúsculas y minúsculas de su correo.</asp:Label>
                  </div>
              </div><br/>
              <div>
                  <b> Subir <b style="color:red">solo 1 documento PDF o bien seleccionar todos los documentos <asp:Image runat="server" ID="Image1" onmouseover="Leyendain(this)" onmouseout="Leyendaout(this)" CssClass="infoCat" ImageUrl="~/Imagenes/Iconos/info.png" /> </b></b><br />
                  <span>Documentos a subir: Solicitud de Manifestación y/o Avalúo, copia de identificación oficial, Poder e identificaciones de testigos en caso de no ser propietario, Contrato de compra-venta si viene de IVSOP, Comprobante de pago de avalúo en caso de solicitarlo.</span>               
                  <asp:Image runat="server" ID="SelectFiles" ImageUrl="~/Imagenes/CapturaSeleccionDocumentos.png" Style="display:none;" CssClass="TamSF" />
                  <asp:FileUpload ID="FileUploadControl" CssClass="form-control-file" runat="server"  AllowMultiple="true"  /><br/><br/>
                  <asp:Label runat="server" ID="ListofuploadedFiles"></asp:Label>
              </div>
              <div>
                <label style="font-size:16px; color:red">Nota: <b style="color:black;"> Los documentos no deben exceder los 10 MB. </b> </label>
              </div>
              <b>Dar clic en el botón "Enviar Archivos"</b><br />
              <label id="mcenviarcp" style="display:none; font-size:16px;">6. Dar clic en el botón "Enviar Archivos"</label><br />
              <div class="col-md-4">
                  <asp:Button runat="server" ID="UploadButton" onclick="UploadButton_Click" Text="Enviar Archivos" CssClass="btn btn-primary" /> 
                  <%--<button type="button" id="BotonEnviar" class="btn btn-primary" data-toggle="modal" onclick="ValidarFormulario()" >Enviar Archivos</button>--%>
              </div>
              <%--<div class="col-md-4">
                  <asp:Label runat="server" Text="Procesando..." Visible="false" ID="Lbl_MensajeEnviar"></asp:Label>
              </div>--%>
              
              <br />
              <br />
              <%--<asp:Button runat="server" ID="CancelarULArchivos" CssClass="btn btn-danger" OnClick="CancelarULArchivos_Click" Text="Cancelar" />--%>
              <div class="col-md-2">
                  <asp:Button runat="server" ID="RegresarButton" CssClass="btn btn-danger" OnClick="RegresarButton_Click" Text="Regresar" />
              </div>
              


          </div><!-- Final del formulario subir archivos -->
        </div>

    </form>
    <div id="fgea" ></div>

</body>
</html>
