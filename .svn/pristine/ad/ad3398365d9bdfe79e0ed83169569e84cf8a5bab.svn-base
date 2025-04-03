<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ManifestacionEnLinea.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Instituto Registral y Catastral</title>
    <style>
         body{
            background-image: url(Imagenes/Iconos/fondo.jpg);
        }
        
         .PosicionTutorial{
            position:absolute;
            width: 20%;
            bottom:15%;
        }

         .OcultarBoton{
             display:none;
         }

        .LogoNav{
            width:30px;
            height:30px;
        }

        .TamIconPdf {
            height: 75px;
            width: 75px;
        }

        .infoCat{
         width: 20px;
         height: 20px;

        }

    </style>
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
    <script src='https://www.google.com/recaptcha/api.js'></script>

        <script>
            function soloNumeros(e) {
                var key = window.Event ? e.which : e.keyCode;
                if (key === 110) {
                    return null;
                }
                else {
                    return (key <= 13 || (key >= 48 && key <= 57));
                }             
            }

    </script>

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

            function warningUbicacion(msg) {
                swal({
                    title: 'Presentarse con su documentación en Catastro',
                    icon: 'warning',
                    text: msg,
                    buttons: {
                        catch: {
                            text: "Aceptar",
                            value: "catch",
                        }
                    },
                }).then((value) => {
                    if (value === "catch") {
                        swal("", {
                        icon: "success",
                        });
                        document.getElementById("<%= DescargaBolUbi.ClientID %>").click();
                }
            });
            }

        </script>
<%--    <script>
        function Notapopupin() {
            
            var imggoin = document.getElementById("<%= popupchrome.ClientID %>");
            var imgpopin = document.getElementById("<%= poupnav.ClientID %>");
            imggoin.style.display = "block";
            imgpopin.style.display = "block";
        }

        function Notapopupout() {
            
            var imggoin = document.getElementById("<%= popupchrome.ClientID %>");
            var imgpopin = document.getElementById("<%= poupnav.ClientID %>");
            imggoin.style.display = "none";
            imgpopin.style.display = "none";
        }
    </script>--%>
    <script>
        $(document).ready(function () {
            $("#<%= MunicipiosList.ClientID %>").change(function () {
                var DropList = document.getElementById("<%=MunicipiosList.ClientID%>");
                var seleccionado = DropList.options[DropList.selectedIndex].value;
                var TxtBoxMunicipio = document.getElementById("<%= TxtMunicipio.ClientID %>");

                if (seleccionado === "01") {
                    
                    document.getElementById("AgsClave").style.display = 'block';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    
                }
                if (seleccionado === "02") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'block';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                if (seleccionado === "03") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'block';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                if (seleccionado === "04") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'block';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID%>').value = seleccionado;
                }
                if (seleccionado === "05") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'block';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                if (seleccionado === "06") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'block';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                if (seleccionado === "07") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'block';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                if (seleccionado === "08") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'block';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                if (seleccionado === "09") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'block';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                if (seleccionado === "10") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'block';
                    document.getElementById("SanFranciscoClave").style.display = 'none';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                if (seleccionado === "11") {
                    
                    document.getElementById("AgsClave").style.display = 'none';
                    document.getElementById("AsientosClave").style.display = 'none';
                    document.getElementById("CalvilloClave").style.display = 'none';
                    document.getElementById("CosioClave").style.display = 'none';
                    document.getElementById("JesusMariaClave").style.display = 'none';
                    document.getElementById("PabellonClave").style.display = 'none';
                    document.getElementById("RinconClave").style.display = 'none';
                    document.getElementById("SanJoseClave").style.display = 'none';
                    document.getElementById("TepezalaClave").style.display = 'none';
                    document.getElementById("LlanoClave").style.display = 'none';
                    document.getElementById("SanFranciscoClave").style.display = 'block';
                    document.getElementById('<%= TxtMunicipio.ClientID %>').value = seleccionado;
                }
                TxtBoxMunicipio.value = seleccionado;
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $("#mensajepantalla").click(function () {
                $("#mensajepantalla").hide();
            });
        });
    </script>
    <script>

        function Leyendain() {
            var textin = document.getElementById("labcons");
            textin.style.display = "block";
        }
        function Leyendaout() {
            var textin = document.getElementById("labcons");
            textin.style.display = "none";
        }
    </script>
    <script>
        function PagarAvaluoEnLinea() {
            <%--var municipio = document.getElementById("<%= TxtMunicipio.ClientID %>").value;
            console.log("municipio" + municipio);--%>

            $(document).ready(function () {
                
               window.open('FormularioPago.aspx', '_newtab');
            });
        }
    </script>
    <script>

            function descargarPDF() {
        // URL del archivo PDF que deseas descargar
                var url = 'https://eservicios2.aguascalientes.gob.mx/irc/formatos/Sol_Inf_Catastral.pdf';

            // Abre una nueva pestaña y descarga el archivo
            window.open(url, '_blank');
    }

    </script>
</head>
<body>
    <div id="hgea"></div><br /><br /><br /><br /><br /><br />
    <form id="form1" runat="server">
        <div class="container">
            <div align="center">
                <h1 class="titulo"> Solicitud de Manifestación de Predio y/o Avalúo Catastral</h1>
                <label style="font-size:16px;"><b> Seleccionar municipio:</b></label>
                <asp:DropDownList style="width:auto;" CssClass="form-control" ID="MunicipiosList" runat="server">
                    <asp:ListItem Selected="True" Value="00">SELECCIONE UN MUNICIPIO</asp:ListItem>
                    <asp:ListItem Value="01">AGUASCALIENTES</asp:ListItem>
                    <asp:ListItem Value="02">ASIENTOS</asp:ListItem>
                    <asp:ListItem Value="03">CALVILLO</asp:ListItem>
                    <asp:ListItem Value="04">COSÍO</asp:ListItem>
                    <asp:ListItem Value="05">JESÚS MARÍA</asp:ListItem>
                    <asp:ListItem Value="06">PABELLÓN DE ARTEAGA</asp:ListItem>
                    <asp:ListItem Value="07">RINCÓN DE ROMOS</asp:ListItem>
                    <asp:ListItem Value="08">SAN JOSÉ DE GRACIA</asp:ListItem>
                    <asp:ListItem Value="09">TEPEZALÁ</asp:ListItem>
                    <asp:ListItem Value="10">EL LLANO</asp:ListItem>
                    <asp:ListItem Value="11">SAN FRANCISCO DE LOS ROMO</asp:ListItem>
                </asp:DropDownList> 
        <br />
        <label style="font-size:16px;"> <b>Ingrese su clave catastral: </b></label>
            <div class="row justify-content-center">
                <div class="col-md-2" >
                    <asp:TextBox CssClass="form-control" ID="TxtMunicipio" runat="server" placeholder="Municipio" Value="00"></asp:TextBox>
                </div>
                <div class="col-md-2" >
                    <asp:TextBox CssClass="form-control" ID="TxtLocalidad" runat="server" onkeypress="return soloNumeros(event);" placeholder="Localidad" MaxLength="3" required="required"></asp:TextBox>
                </div>
                <div class="col-md-2" >
                    <asp:TextBox CssClass="form-control" ID="TxtSector" runat="server" onkeypress="return soloNumeros(event);" onblur="numeroRustico();" placeholder="Sector" MaxLength="2" required="required"></asp:TextBox>
                </div>
                <div class="col-md-2" >
                    <asp:TextBox CssClass="form-control" ID="TxtManzana" runat="server" onkeypress="return soloNumeros(event);" placeholder="Manzana" MaxLength="4" required="required"></asp:TextBox>
                </div>
                <div class="col-md-2" >
                    <asp:TextBox CssClass="form-control" ID="TxtPredio" runat="server" onkeypress="return soloNumeros(event);" placeholder="Predio" MaxLength="3" required="required"></asp:TextBox>
                </div>
                <div class="col-md-2" >
                    <asp:TextBox CssClass="form-control" ID="TxtCondominio" runat="server" onkeypress="return soloNumeros(event);" placeholder="Condominio" MaxLength="3" required=" required"></asp:TextBox>
                </div>
            </div>
            <div class="row justify-content-center">
                <label>Si el sector en su clave catastral es 00 y no da resultados cambiarlo por 99 </label>
            </div>

        <br />
                <div class="col-md-4">
                     <asp:Button ID="BuscarInfo" runat="server" OnClick="BuscarInfo_Click" Text="Ingresar" CssClass="btn btn-primary"/> <br />
                    <asp:Button ID="DescargaBolUbi" runat="server" CssClass="OcultarBoton" OnClick="DescargaBolUbi_Click" />
                </div>
       

    </div>
            <br />
            <!-- Cartas -->
            <div class="row">
                <div class="col-sm-6">
                    <div class="card">
                        <div class="card-body">
                            <h5 class="titulo-cita" style="text-align:center">Pago de Avalúo Catastral</h5>
                            <p class="card-text">Costo: $198.00 Clave:43060703 </p>
                            <p class="card-text">Con fundamento el Artículo 7 fracción VII, Inciso 2) de la Ley de Ingresos del Estado de Aguascalientes para el Ejercicio Fiscal del Año 2025. </p>
                            <button class="btn-form btn-form-descarga" onclick="PagarAvaluoEnLinea()">
                                <i class="fas fa-money-bill-alt me-2"></i>Pagar Avalúo
                            </button>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="card">
                        <div class="card-body">
                            <h5 class="card-title" style="text-align:center">Solicitud de Manifestación y/o Avalúo Catastral</h5>
                            <p><b> Requisitos: Digitalize los siguientes documentos. </b></p>
                            <p class="card-text">1. Descargue, llene y firme la solicitud.  </p>
                            <p class="card-text">2. Realize el pago de Avalúo en caso de requerirlo</p>
                            <p class="card-text">3. Copia de identificación oficial. Nota: En caso de no ser el propietario: Carta poder simple en original firmada por el propietario, gestor y 2 testigos, así como copia de la identificación oficial de cada uno.</p>
                            <div class="text-center">
                                <button class="btn-form btn-form-descarga" onclick="descargarPDF()">
                                    <span class="iconify me-2" data-inline="true" data-icon="material-symbols:download"></span>Descargar Formato
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />

            
            <!-- Acoordeones-->
<div class="accordion accordion-flush" id="accordionFlushExample">
  <div class="accordion-item">
    <h2 class="accordion-header" id="flush-headingOne">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#flush-collapseOne" aria-expanded="false" aria-controls="flush-collapseOne">
        ¿Como se conforma la Clave Catastral?
      </button>
    </h2>
    <div id="flush-collapseOne" class="accordion-collapse collapse" aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushExample">
      <div class="accordion-body">
          <div class="rows">
                <div class="col-md-6 col-sm-6 col-xs-6" id="AgsClave">
                    <asp:Image ID="imgClaveAgs" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/AGS_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredAgs" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/AGUASCALIENTESPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="AsientosClave" style="display:none;">
                    <asp:Image ID="imgClaveAsi" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/ASI_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredAsi" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/ASIENTOSPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="CalvilloClave" style="display:none;">
                    <asp:Image ID="imgClaveCal" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/CAL_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredCal" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/CALVILLOPREDIAL2.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="CosioClave" style="display:none;">
                    <asp:Image ID="imgClaveCos" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/COS_CLAVE.JPG" />
                   <%-- <asp:Image ID="imgPredCos" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/COSIOPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="JesusMariaClave" style="display:none;">
                    <asp:Image ID="imgClaveJm" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/JES_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredJm" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/JESUSMARIAPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="PabellonClave" style="display:none;">
                    <asp:Image ID="imgClavePab" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/PAB_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredPab" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/PABELLONPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="RinconClave" style="display:none;">
                    <asp:Image ID="imgClaveRin" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/RIN_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredRin" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/RINCONPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="SanJoseClave" style="display:none;">
                    <asp:Image ID="imgClaveSj" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/JOS_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredSj" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/SANJOSEPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="TepezalaClave" style="display:none;">
                    <asp:Image ID="imgClaveTep" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/TEP_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredTep" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/TEPEZALAPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="LlanoClave" style="display:none;">
                    <asp:Image ID="imgClaveLla" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/LLA_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredLLa" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/ELLLANOPREDIAL.jpg" />--%>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6" id="SanFranciscoClave" style="display:none;">
                    <asp:Image ID="imgClaveSf" runat="server" Cssclass="img-responsive" ImageUrl="~/Imagenes/FormatoClaves/SFC_CLAVE.JPG" />
                    <%--<asp:Image ID="imgPredSf" runat="server" CssClass="img-responsive" ImageUrl="~/Imagenes/Predial/SANFCOPREDIAL.jpg" />--%>
                </div>
              </div>
      </div>
    </div>
  </div>
  <%--<div class="accordion-item">
    <h2 class="accordion-header" id="flush-headingTwo">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#flush-collapseTwo" aria-expanded="false" aria-controls="flush-collapseTwo">
        Requisitos
      </button>
    </h2>
    <div id="flush-collapseTwo" class="accordion-collapse collapse" aria-labelledby="flush-headingTwo" data-bs-parent="#accordionFlushExample">
      <div class="accordion-body">
          *Formato de Solicitud.
          *Copia de identificación oficial del propietario.
          *Copia de Carta poder en caso necesario.
          *Identificaciones de persona que recibe el poder y testigos. en caso de ser necesarios.</div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header" id="flush-headingThree">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#flush-collapseThree" aria-expanded="false" aria-controls="flush-collapseThree">
        Instrucciones
      </button>
    </h2>
    <div id="flush-collapseThree" class="accordion-collapse collapse" aria-labelledby="flush-headingThree" data-bs-parent="#accordionFlushExample">
      <div class="accordion-body">
          <asp:panel runat="server" Style="background-color:white; padding:10px;">
                    <h2> Manifestación de construcción</h2>
                    <h4>Sin Costo</h4>                 
                    <h4><b>* Requisitos </b></h4> 
                <label style="font-family:Arial; font-size:14px;">1. Llenar y firmar formato de solicitud.(clic en la imagen para descargar)</label><br />
                <asp:ImageButton ID="ImagenPdfDwn" CssClass="TamIconPdf" runat="server" ImageUrl="~/Imagenes/Iconos/icon-pdf.png" target="_blank" onClientClick="window.open('https://drive.google.com/file/d/1qvQObJ_ZpauzqpSu3DbpWeoG3HMPDern/view?usp=sharing')"/> <br />                
                <label style="font-family:Arial; font-size:14px;">2. Copia de identificación oficial. </label><br />
                <label  style="font-family:Arial; font-size:14px;">Nota: En caso de no ser el propietario: Carta poder simple en original firmada por el propietario, gestor y 2 testigos, así como copia de la identificación oficial de cada uno. </label>
                
                <h2> Instrucciones</h2>
                
                 <label style="font-family:Arial; font-size:14px;">1. Digitalizar los documentos</label><br />
                    <span>*Formato de Solicitud.</span><br />
                    <span>*Copia de identificación oficial del propietario.</span><br />
                    <span>*Copia de Carta poder en caso necesario.</span><br />
                    <span>*Identificaciones de persona que recibe el poder y testigos. en caso de ser necesarios.</span><br />
                    <span>*</span>
                 <label style="font-family:Arial; font-size:14px;">2. Ingresar su clave catastral y dar clic en el botón "Ingresar"</label> <br /> 
                 <label style="font-family:Arial; font-size:14px;">3. Ingresar sus datos de contacto.</label><br />
                 <label style="font-family:Arial; font-size:14px;">4. Seleccionar el documento digitalizado</label><br />
                 <label style="font-family:Arial; font-size:14px;">5. Dar clic en el botón, "Subir Archivos" </label>
                    
                </asp:panel>
          <asp:panel runat="server" Style="background-color:white; padding:10px;">
                    <h2> Avalúo Catastral</h2>
                    <h4>Costo: $188.00 Clave:43060703</h4>  
                    <label style="font-family:Arial; font-size:14px;">Con fundamento el Artículo 7 fracción VII, Inciso 2) de la Ley de Ingresos del Estado de Aguascalientes para el Ejercicio Fiscal del Año 2024.</label><br />
                    <h4><b>* Requisitos </b></h4> 
                <label style="font-family:Arial; font-size:14px;">1. Realizar el pago de derechos.(Ingresar su clave catastral y clic en el botón para pagar.)</label><br />
                 <div class="col-md-4">
                     <asp:Button runat="server" ID="ServerCveValidar" CssClass="btn btn-success" Text="Pagar Avalúo" OnClick="ServerCveValidar_Click" />
                 </div>
              <br />
                    <label> Nota: Habilitar ventanas emergentes</label> <asp:Image runat="server" ID="imgCatInfo" onmouseover="Notapopupin(this)" onmouseout="Notapopupout(this)" CssClass="infoCat" ImageUrl="~/Imagenes/Iconos/info.png" /><span id="labcons" style="display:none;"></span>
                    <asp:Image runat="server" ID="popupchrome" ImageUrl="~/Imagenes/Iconos/popupbloqueados.png" style="display:none"  /> <asp:Image runat="server" ID="poupnav" ImageUrl="~/Imagenes/Iconos/FirefoxVentanas.png" style="display:none;" /><br />
                <label style="font-family:Arial; font-size:14px;">2. Llenar y firmar formato de solicitud.(clic en la imagen para descargar)</label><br />
                <asp:ImageButton ID="ImageButton1" CssClass="TamIconPdf" runat="server" ImageUrl="~/Imagenes/Iconos/icon-pdf.png" target="_blank" onClientClick="window.open('https://drive.google.com/file/d/1qvQObJ_ZpauzqpSu3DbpWeoG3HMPDern/view?usp=sharing')"/> <br />                
                <label style="font-family:Arial; font-size:14px;">3. Copia de identificación oficial. </label><br />
                <label  style="font-family:Arial; font-size:14px;">Nota: En caso de no ser el propietario: Carta poder simple en original firmada por el propietario, gestor y 2 testigos, así como copia de la identificación oficial de cada uno. </label>
                <label style="font-family:Arial; font-size:14px;">4. Contrato de compraventa (Si viene por parte del IVSOP) </label>
                <label style="font-family:Arial; font-size:14px; color:red">Nota sobre el Avalúo: Si su predio tiene construcciones, es necesario solicitar la manifestación</label>
               <h2> Instrucciones</h2>
                <label style="font-family:Arial; font-size:14px;">1. Digitalizar los documentos</label><br />
                    <span>*Formato de Solicitud</span><br />
                    <span>*Copia de indentificación oficial.</span><br />
                 <label style="font-family:Arial; font-size:14px;">2. Ingresar su clave catastral y dar clic en el botón "Ingresar"</label> <br /> 
                 <label style="font-family:Arial; font-size:14px;">3. Ingresar sus datos de contacto.</label><br />
                 <label style="font-family:Arial; font-size:14px;">4. Seleccionar el documento digitalizado</label><br />
                 <label style="font-family:Arial; font-size:14px;">5. Dar clic en el botón, "Subir Archivos" </label>
                 
                    
                </asp:panel>

      </div>
    </div>
  </div>--%>
</div>
            <div style="margin-left:10px;">
        <label>Utilizar un navegador actualizado, de preferencia Google Chrome. </label><asp:Image runat="server" CssClass="LogoNav" ImageUrl="~/Imagenes/Iconos/Chrome_Logo.png" />  <%--<label>Reportar cualquier error al correo catastro@aguascalientes.gob.mx</label>--%>
    </div>
    </div>

    


    </form>
   <div id="fgea"></div>


</body>
</html>
