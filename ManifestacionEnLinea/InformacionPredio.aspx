<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformacionPredio.aspx.cs" Inherits="ManifestacionEnLinea.InformacionPredio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <title>Instituto Catastral De Aguascalientes</title>
    <style>
     body,html{
                height: 100%;
                width: 100%;
                margin-top: 5px;
                margin-bottom: 2%;
                background-image: url(Imagenes/Iconos/fondo.jpg);
            }

     #map { height: 25%;
            width: 25%;
            margin: 0 auto;
     }

     .infoCat{
         width: 20px;
         height: 20px;
     }
     
     table.grid td,tr,th{
         border: 1px solid black;
         text-align:center;
         background-color:white;
     }
     .gridTitle {
        font-weight: bold;
        font-size: 16px;
        background-color: #f2f2f2;
        border-bottom: 1px solid #ddd;
    }
     

     /*input[type="checkbox"] {*/
         /* Double-sized Checkboxes */
         /*-ms-transform: scale(4);*/ /* IE */
         /*-moz-transform: scale(4);*/ /* FF */
         /*-webkit-transform: scale(4);*/ /* Safari and Chrome */
         /*-o-transform: scale(4);*/ /* Opera */
         
       
     /*}*/
     .tamLoad{
         width:25px;
         height:25px;
     }

     .idSimbol{
         position:absolute;
         width:200px;
         height:200px;
         right:20%;
         top:300px;
         left:400px;
     }

     .TamIconPdf{
         height: 50px;
         width:50px;
     }

     .ocultarboton{
         visibility:hidden;
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
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.3/dist/leaflet.css" integrity="sha256-kLaT2GOSpHechhsozzB+flnD+zUyjE2LlfWPgU04xyI=" crossorigin=""/>
    <script src="https://unpkg.com/leaflet@1.9.3/dist/leaflet.js" integrity="sha256-WBkoXOwTeyKclOHuWtc+i2uENFpDZ9YPdf5Hf+D7ewM="crossorigin=""></script>

    <script>
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

        function Confirmacion(msg) {
            swal({
                title: 'Confirmación',
                icon: 'warning',
                text: msg,
                buttons: {
                    cancel: {
                        text: "Cancelar",
                        classname:"btn btn-danger",
                        value: null,
                    },
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
                    document.getElementById("<%= btnUploadArchivos.ClientID %>").click();
        }
    });
        }
    </script>

    <script>
        function cargaCroquis() {

            document.getElementById("AvisoCroquis").style.display = "none";
        }
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
        $(document).ready(function () {
            $("#BotonUploadArchivos").click(function () {
                botonverde = 1;
                var opcM = document.getElementById("CheckboxManifestacion"); // variable para el checkbox de mani
                var opcA = document.getElementById("CheckboxAvaluo"); // variable para el checkbox de avaluo
                var opcMA = document.getElementById("CheckboxManiAvaluo");//variable para el checkbox de Mani y Avaluo
                if (opcM.checked === false && opcA.checked === false && opcMA.checked === false) {
                    warningsalert("No se seleccionó tramite(s) a realizar.");
                }
                if (opcMA.checked === true) {
                    Confirmacion("Confirmar Manifestación y Avalúo");
                }
                if (opcA.checked === true) {
                    Confirmacion("Confirmar Avalúo");
                }
                if (opcM.checked === true) {
                    Confirmacion("Confirmar Manifestación");
                }
               
            });

            /*Oculta el modal, modal cuando no se seleccionaron tramites*/
            $("#modal-btn-Ok").click(function () {
                $("#modalTramite").modal('hide');
            });
            /*Envia a la pagina para enviar archivos, Solo Manifestación*/
            $("#modal-btn-siMani").click(function () {
                if (botonverde == 1) {
                    document.getElementById("CheckboxManiAvaluo").checked = true;
                    document.getElementById("CheckboxManifestacion").checked = false;
                    document.getElementById("<%= btnUploadArchivos.ClientID %>").click();
                }
                if (botonverde == 0) {
                    document.getElementById("CheckboxAvaluo").checked = true;
                   <%-- document.getElementById("<%= btnConfirm.ClientID %>").click();--%>
                }

            });
            /*Oculta el Modal de confirmacion de Mani*/
            $("#modal-btn-noMani").click(function () {
                if (botonverde == 1) {
                    document.getElementById("<%= btnUploadArchivos.ClientID %>").click();
                }
                if (botonverde == 0) {
                    <%--document.getElementById("<%= btnConfirm.ClientID %>").click();--%>
                }
                
            });
            /*Envia a la pagina para enviar archivos, solo Avalúo*/
            $("#modal-btn-siAvaluo").click(function () {
                if (botonverde == 1) {
                    document.getElementById("<%= btnUploadArchivos.ClientID %>").click();
                }
                if (botonverde == 0) {
                    <%--document.getElementById("<%= btnConfirm.ClientID %>").click();--%>
                }
                
            });


            /*oculta el Modal de confirmacion de Avaluo*/
            $("#modal-btn-noAvaluo").click(function () {
                $("#mymodalAvaluo").modal('hide');
            });
            /*Envia a la pagina para enviar archivos, Avalúo y Manifestación*/
            $("#modal-btn-siManiAvaluo").click(function () {
                if (botonverde == 1) {
                    document.getElementById("<%= btnUploadArchivos.ClientID %>").click();
                }
                if (botonverde == 0) {
                    <%--document.getElementById("<%= btnConfirm.ClientID %>").click();--%>
                }

            });
            /*oculta el Modal de confirmacion de Mani y Avaluo*/
            $("#modal-btn-noManiAvaluo").click(function () {
                $("#mymodalManiAvaluo").modal('hide');
            });

        });
    </script>
    <script>
        function validarCheckbox() {
            var opcM = document.getElementById("CheckboxManifestacion"); // variable para el checkbox de mani
            var opcA = document.getElementById("CheckboxAvaluo"); // variable para el checkbox de avaluo
            var opcMA = document.getElementById("CheckboxManiAvaluo");//variable para el checkbox de Mani y Avaluo

            if (opcM.checked === true && opcA.checked === true && opcMA.checked === false) { /*Si mani y avaluo se seleccionan*/
                opcM.checked = false;
                opcA.checked = false;
                opcMA.checked = true;
               
            }
            if (opcMA.checked === true && opcA.checked === true && opcM.checked === false) {/* Si maniavaluo y avaluo se seleccionan*/
                opcM.checked = false;
                opcA.checked = false;
                opcMA.checked = true;
                
            }
            if (opcMA.checked === true && opcM.checked === true && opcA.checked === false) { /* si maniavaluo y mani se seleccioan*/
                opcM.checked = false;
                opcA.checked = false;
                opcMA.checked = true;
                
            }
            if (opcM.checked === true && opcA.checked === false & opcMA.checked === false) {
                opcM.checked = true;
                opcA.checked = false;
                opcMA.checked = false;

            }
            if (opcM.checked === false && opcA.checked === true & opcMA.checked === false) {
                opcM.checked = false;
                opcA.checked = true;
                opcMA.checked = false;

            }
            if (opcM.checked === false && opcA.checked === false & opcMA.checked === true) {
                opcM.checked = false;
                opcA.checked = false;
                opcMA.checked = true;
 
            }

            
        }
    </script>
    


    <script>
        function PagarAvaluoEnLinea(msg) {
            $(document).ready(function () {
                window.open('FormularioPago.aspx?value='+msg, '_newtab');
            });
        }
    </script>
</head>
<body>
     <div id="hgea"></div> <br /><br /><br /><br /><br />
    
    <form id="EnvioMani" runat="server">
        <br /> <br /> <br />
        <div align="center">
            <input type="hidden" id="hiddenRegimen" runat="server"/>
                      
            <table style="border:1px solid #000000">
                <tr>
                    <td colspan="4" align="center" style="background-color:white"><b>INFORMACIÓN DEL TERRENO</b></td>

                </tr>
                <tr align="center" style="background-color:white; border:1px solid black">
                    <th style="border: 1px solid black"><b>CLAVE CATASTRAL</b></th>
                    <th style="border: 1px solid black"><b>TIPO PREDIO</b></th>
                    <th style="border: 1px solid black"><b>RÉGIMEN</b></th>
                    <th style="border: 1px solid black"><b>SUPERFICIE</b></th>
                </tr>
                
                <tr style="background-color:white;">
                    <td>
                        <asp:label ID="CVE_TABLE" runat="server"/>
                    </td>
                    <td>
                        <asp:label ID="TIPO_TABLE" runat="server"/>
                    </td>
                    <td>
                        <asp:label ID="REGIMEN_TABLE" runat="server"/>
                    </td>
                    <td>
                        <asp:label ID="SUPPRIV_TABLE" runat="server"/>
                    </td>
                </tr>
            </table>
            <br />

            <asp:GridView runat="server" ID="GridConst" AutoGenerateColumns="false" style="border:1px black solid" CssClass="grid">
               

                <HeaderStyle/>
                <Columns>
                    
                    <asp:BoundField DataField="DESCRIPCION_TIPO" HeaderText="TIPO" />
                    <asp:BoundField DataField="DESCRIPCION_ESTADO" HeaderText="ESTADO" />
                    <asp:BoundField DataField="ANIO" HeaderText="AÑO" />
                    <asp:BoundField DataField="DESCRIPCION_AVANCE" HeaderText="AVANCE" />
                    <asp:BoundField DataField="AREA" HeaderText="SUPERFICIE M2" DataFormatString="{0:0.00}" HtmlEncode="false" />
                   
                 </Columns>
            </asp:GridView>
            <%--<div style="display:none;" runat="server" id="tableSC" >
                <table style="border:1px solid black">
                    <tr>
                        <td colspan="5" align="center"> <b> INFORMACIÓN DE LAS CONSTRUCCIONES</b></td>
                    </tr>
                     <tr align="center" style="background-color:white; border:1px solid black">
                    <th style="border: 1px solid black"><b>DESCRIPCION_TIPO</b></th>
                    <th style="border: 1px solid black"><b>DESCRIPCION_ESTADO</b></th>
                    <th style="border: 1px solid black"><b>AÑO</b></th>
                    <th style="border: 1px solid black"><b>DESCRIPCION_AVANCE</b></th>
                    <th style="border: 1px solid black"><b>AREA</b></th>
                     </tr>
                    <tr style="border:1px solid black">
                        <td colspan="5" style="border:1px solid black"><label><b> SIN CONTRUCCIONES</b></label></td>
                    </tr>
                </table>
            </div>--%>
        
        </div>
        
        <%--<div style="position: absolute; left:13%;">
            <label>Catalogo de Construcciones</label>
             <a target="_blank" href="https://drive.google.com/file/d/1Lix8VFMq6DcMAJI-v8x6c1LX3AW_WOuW/view?usp=sharing"><asp:Image runat="server" id="imgInf" style="width:2%;" ImageUrl="~/Imagenes/Iconos/info.png"/></a>
         </div>--%>
        <br />
        <div align="center">
            <label><b>UBICACIÓN</b></label>
        </div><br />
        <div id="mapP" align="center" style="width: 600px; height: 400px; position:relative; left: 35%" ></div> 
        <script>
            //var UTMScaleFactor = 0.9996;
            //var pi = 3.14159265358979;
            //var sm_a = 6378137.0;
            //var sm_b = 6356752.314;
            //var sm_EccSquared = 6.69437999013e-03;
            //var Var_Coordinadas = [];
            //var VAR_Coordinadasconst = [];
            //var var_coordinada;
            //var var_coordinadasconst;
            //var VAR_definitive = [];
            // Función para obtener los parámetros de la URL
            const params = new URLSearchParams(window.location.search);
            const latitud = parseFloat(params.get('latitud'));
            const longitud = parseFloat(params.get('longitud'));
            console.log("Latitud:" + latitud);
            console.log("Longitud:" + longitud);

            const zonaUTM = 13; // O 14 dependiendo de la ubicación
            const hemi = "N";
            const latlon = new Array(2);
            UTMXYToLatLon(longitud, latitud, zonaUTM, hemi, latlon);
            const lon = RadToDeg(latlon[1]);
            const lat = RadToDeg(latlon[0]);
            latLng = [lat, lon];
            let currentMarker = null;
                
            function UTMXYToLatLon(x, y, zone, southhemi, latlon) {
                var cmeridian;
                var UTMScaleFactor = 0.9996;
                x -= 500000.0;
                x /= UTMScaleFactor;

                y /= UTMScaleFactor;

                cmeridian = UTMCentralMeridian(zone);
                MapXYToLatLon(x, y, cmeridian, latlon);

                return;
            }

            function UTMCentralMeridian(zone) {
                var cmeridian;

                cmeridian = DegToRad(-183.0 + (zone * 6.0));

                return cmeridian;
            }

            function DegToRad(deg) {
                var pi = 3.14159265358979;
                return (deg / 180.0 * pi);
            }
            function RadToDeg(rad) {
                var pi = 3.14159265358979;
                return (rad / pi * 180.0);
            }

            function MapXYToLatLon(x, y, lambda0, philambda) {
                var phif, Nf, Nfpow, nuf2, ep2, tf, tf2, tf4, cf;
                var x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac;
                var x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;
                var sm_a = 6378137.0;
                var sm_b = 6356752.314;


                /* Get the value of phif, the footpoint latitude. */
                phif = FootpointLatitude(y);

                /* Precalculate ep2 */
                ep2 = (Math.pow(sm_a, 2.0) - Math.pow(sm_b, 2.0)) / Math.pow(sm_b, 2.0);

                /* Precalculate cos (phif) */
                cf = Math.cos(phif);

                /* Precalculate nuf2 */
                nuf2 = ep2 * Math.pow(cf, 2.0);

                /* Precalculate Nf and initialize Nfpow */
                Nf = Math.pow(sm_a, 2.0) / (sm_b * Math.sqrt(1 + nuf2));
                Nfpow = Nf;

                /* Precalculate tf */
                tf = Math.tan(phif);
                tf2 = tf * tf;
                tf4 = tf2 * tf2;

                /* Precalculate fractional coefficients for x**n in the equations
                   below to simplify the expressions for latitude and longitude. */
                x1frac = 1.0 / (Nfpow * cf);

                Nfpow *= Nf;   /* now equals Nf**2) */
                x2frac = tf / (2.0 * Nfpow);

                Nfpow *= Nf;   /* now equals Nf**3) */
                x3frac = 1.0 / (6.0 * Nfpow * cf);

                Nfpow *= Nf;   /* now equals Nf**4) */
                x4frac = tf / (24.0 * Nfpow);

                Nfpow *= Nf;   /* now equals Nf**5) */
                x5frac = 1.0 / (120.0 * Nfpow * cf);

                Nfpow *= Nf;   /* now equals Nf**6) */
                x6frac = tf / (720.0 * Nfpow);

                Nfpow *= Nf;   /* now equals Nf**7) */
                x7frac = 1.0 / (5040.0 * Nfpow * cf);

                Nfpow *= Nf;   /* now equals Nf**8) */
                x8frac = tf / (40320.0 * Nfpow);

                /* Precalculate polynomial coefficients for x**n.
                   -- x**1 does not have a polynomial coefficient. */
                x2poly = -1.0 - nuf2;

                x3poly = -1.0 - 2 * tf2 - nuf2;

                x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2
                    - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

                x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

                x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2
                    + 162.0 * tf2 * nuf2;

                x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

                x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

                /* Calculate latitude */
                philambda[0] = phif + x2frac * x2poly * (x * x)
                    + x4frac * x4poly * Math.pow(x, 4.0)
                    + x6frac * x6poly * Math.pow(x, 6.0)
                    + x8frac * x8poly * Math.pow(x, 8.0);

                /* Calculate longitude */
                philambda[1] = lambda0 + x1frac * x
                    + x3frac * x3poly * Math.pow(x, 3.0)
                    + x5frac * x5poly * Math.pow(x, 5.0)
                    + x7frac * x7poly * Math.pow(x, 7.0);

                return;
            }

            function FootpointLatitude(y) {
                var y_, alpha_, beta_, gamma_, delta_, epsilon_, n;
                var result;
                var sm_a = 6378137.0;
                var sm_b = 6356752.314;
                /* Precalculate n (Eq. 10.18) */
                n = (sm_a - sm_b) / (sm_a + sm_b);

                /* Precalculate alpha_ (Eq. 10.22) */
                /* (Same as alpha in Eq. 10.17) */
                alpha_ = ((sm_a + sm_b) / 2.0)
                    * (1 + (Math.pow(n, 2.0) / 4) + (Math.pow(n, 4.0) / 64));

                /* Precalculate y_ (Eq. 10.23) */
                y_ = y / alpha_;

                /* Precalculate beta_ (Eq. 10.22) */
                beta_ = (3.0 * n / 2.0) + (-27.0 * Math.pow(n, 3.0) / 32.0)
                    + (269.0 * Math.pow(n, 5.0) / 512.0);

                /* Precalculate gamma_ (Eq. 10.22) */
                gamma_ = (21.0 * Math.pow(n, 2.0) / 16.0)
                    + (-55.0 * Math.pow(n, 4.0) / 32.0);

                /* Precalculate delta_ (Eq. 10.22) */
                delta_ = (151.0 * Math.pow(n, 3.0) / 96.0)
                    + (-417.0 * Math.pow(n, 5.0) / 128.0);

                /* Precalculate epsilon_ (Eq. 10.22) */
                epsilon_ = (1097.0 * Math.pow(n, 4.0) / 512.0);

                /* Now calculate the sum of the series (Eq. 10.21) */
                result = y_ + (beta_ * Math.sin(2.0 * y_))
                    + (gamma_ * Math.sin(4.0 * y_))
                    + (delta_ * Math.sin(6.0 * y_))
                    + (epsilon_ * Math.sin(8.0 * y_));

                return result;
            }

            // Inicializar el mapa en una ubicación por defecto
            const map = L.map('mapP').setView([21.8823400, -102.2825900], 13);

            var Esri_WorldImagery = L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
                maxZoom: 20,
                attribution: 'Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community'
            }).addTo(map);

            if (latLng && latLng.every(coord => isFinite(coord))) {
                if (currentMarker) {
                    currentMarker.setLatLng(latLng);
                } else {
                    currentMarker = L.marker(latLng).addTo(map);
                }
                map.setView(latLng, 19);
            } else {
                alert("Por favor, ingresa coordenadas válidas.");
            }
            

        </script>
        <br />
        <div id="Simb" style="display:none">
            <asp:Image runat="server" CssClass="idSimbol" ImageUrl="~/Imagenes/Iconos/simbologia.png" />
        </div>
<%--        <div id="InfoCroquis" style="display:none" align="center">
            <span style="color:red;font-size:20px;"> <b>Croquis No Disponible. <br /> Por favor acudir a las oficinas del Instituto Catastral para ubicar el predio. </b></span><br />
            <span><b> Av. de la Convención Ote. No. 102 Planta Alta, Colonia del Trabajo, C.P. 20180, Aguascalientes, Ags. </b></span>
        </div>--%>
        <div>
            <asp:HiddenField runat="server" ID="ClaveCatastralOculta" />
            <asp:HiddenField runat="server" ID="hiddenLongitud" />
            <asp:HiddenField runat="server" ID="hiddenLatitud" />
        </div>
        <div align="center">
            <span style="font-size:16px;"><b>Coordenada X:</b></span><asp:label ID="CoordenadaX" runat="server"></asp:label> <span style="font-size:16px;"><b>Coordenada Y:</b></span><asp:label runat="server" id="CoordenadaY"></asp:label>
        </div>
            <div id="AvisoCroquis" style="display:none" align="center">
            <span>Esperar a que cargue el croquis, en caso de que no se muestre, vuelva a pulsar el botón.</span>
            <asp:Image ID="gifLoad" CssClass="tamLoad" runat="server" ImageUrl="~/Imagenes/Iconos/Loading.gif" />
        </div>
        <br />
        <br />
        <%--<div align="center">
            <span style="font-size:16px;">En caso de estar de acuerdo con la construcción que aparece en el cuadro de información, señalarlo en la solicitud. </span> <br /><br />
            <label style="font-size:16px;">Estoy de acuerdo</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" id="igualCartografia" runat="server" /><br /><br />
            <span style="font-size:16px;">En caso contrario seguir las instrucciones de llenado de la solicitud.</span><br />
        </div>--%>
    
        <div id="Instrucciones" align="center" style="margin-bottom:15px;">
            <div  style="border:1px solid black; width: 55%; background-color:white" >
                <span style="font-size:16px;"><b>LEER INSTRUCCIONES MANIFESTACIÓN CATASTRAL Y/O AVALÚO CATASTRAL EN LINEA</b></span><br/><br />
                
                <div class="row">
                    <span style="font-size:16px;"><b>1. Seleccionar el/los trámite(s) a realizar.</b></span>
                </div>  <br />              
               <div class="row"> 
                   <div class="col-md-4 col-sm-4 col-xs-4">
                       <label style="font-size:16px">Manifestación de Construcción<br /></label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" id="CheckboxManifestacion" runat="server" onclick="validarCheckbox()" />
                   </div>
                   <div class="col-md-4 col-sm-4 col-xs-4">
                       <label style="font-size:16px">Avalúo Catastral <br/>  </label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                       
                       <input type="checkbox" id="CheckboxAvaluo" runat="server" onclick="validarCheckbox()" />
                   </div>
                   <div class="col-md-4 col-sm-4 col-xs-4">
                       <label style="font-size:16px">Manifestación de Construcción <br /> con Avalúo Catastral</label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                       
                       <input type="checkbox" id="CheckboxManiAvaluo" runat="server" onclick="validarCheckbox()" /><br /> <br />
                       
                   </div>                   
                   <br /> <br />
               </div>
                <br />
               <%-- <span style="font-size:16px;" id="SpanPago"><b>2. Pago de avalúo en caso de requerirlo, dar clic en el botón "Pagar Avalúo", al finalizar el proceso escanear el pago. </b></span><br /> <br />
                <asp:Button runat="server" ID="BotonEnviar" class="btn btn-primary" onclick="BotonEnviar_Click" Text="Pagar Avalúo" /><br />
                <span style="font-size:16px; display:none;" id="spanEscaneoMani" ><b>2. Escanear los siguientes documentos e integrarlos en un mismo archivo PDF</b></span><br />
                <span style="font-size:16px;" id="spanEscaneoManiyAvaluo"><b>3. Escanear los siguientes documentos e integrarlos en un mismo archivo PDF.  </b></span><br />
               
                <!--<a target="_blank" href="https://www.aguascalientes.gob.mx/seguot/pdf/IC/SolInfCat2020.pdf" >Formato de solicitud.</a> <br /> -->
                <span style="font-size:16px;"><b>- Formato de solicitud llenado y firmado.</b></span><asp:ImageButton ID="ImagenPdf" CssClass="TamIconPdf" runat="server" ImageUrl="~/Imagenes/Iconos/icon-pdf.png" target="_blank" onClientClick="window.open('https://drive.google.com/file/d/1qvQObJ_ZpauzqpSu3DbpWeoG3HMPDern/view?usp=sharing')"/> <br />
                <a style="font-size:16px;" target="_blank" href="https://drive.google.com/file/d/1gmw6EHZW9uPOJL-ascVrznrND2HQ-TL3/view?usp=sharing">Ver Catalogo de Construcciones</a> &nbsp; <asp:Image runat="server" ID="imgCatInfo" onmouseover="Leyendain(this)" onmouseout="Leyendaout(this)" CssClass="infoCat" ImageUrl="~/Imagenes/Iconos/info.png" /><span id="labcons" style="display:none;">Te servirá de ayuda para identificar el tipo de construcción.</span><br />
                <span style="font-size:16px;"><b>- Copia de identificación oficial.</b></span><br/><br />
                <span style="font-size:16px;"><b> En caso de no ser el propietario:</b> </span><br />
                <span style="font-size:16px;">Integrar en un PDF: Carta poder simple en original firmada por el propietario, gestor y 2 testigos, así como copia de la identificación oficial de cada uno.</span><br />
                <span style="font-size:16px;" id="spancontrato" ><b> - Contrato de compraventa si viene por parte del IVSOP</b></span><br /> 
                <span style="font-size:16px" id="spanpagoavaluo"><b>- Comprobante del pago de Avalúo Catastral</b></span><br /><br />
                <span style="font-size:16px; display:none;" id="spanSubirMani"><b>3. Dar clic en el botón "Subir Archivos" para cargar sus documentos.</b></span><br />--%>
                <span style="font-size:16px;" id="spanSubirManiAvaluo"><b>2. Dar clic en el botón "Subir Archivos" para cargar sus documentos.</b></span>   <br />    
                
                
                <button type="button" id="BotonUploadArchivos" class="btn btn-success" data-toggle="modal" data-target="mymodalMani">Subir Archivos</button>
                <asp:Button runat="server" ID="btnUploadArchivos" CssClass="ocultarboton" Text="" OnClick="btnUploadArchivos_Click" />
                <div class="col-md-2">
                    <asp:Button runat="server" ID="botonreturnP" CssClass="btn btn-danger" Text="Regresar" OnClick="botonreturnP_Click" />
                    
                </div>
                
                <%--<asp:button runat="server" ID="btnConfirm" CssClass="OcultarBoton" Text="" OnClick="EnviarPago"/>--%>
                
                 <%--<asp:Button runat="server" ID="BotonCancelar" CssClass="btn btn-danger" OnClick="BotonCancelar_Click" Text="Cancelar"/>--%>
            </div>
        </div>
    </form>
    <div id="fgea"></div>

</body>
</html>
