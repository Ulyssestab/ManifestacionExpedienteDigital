<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UbicarPredio.aspx.cs" Inherits="ManifestacionEnLinea.UbicarPredio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        #map {
      height: 400px;
    }
        .ocultarboton{
            visibility:hidden;
        }
    </style>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>

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
                        classname: "btn btn-danger",
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
                    document.getElementById("<%= btn_SeleccionarTramite.ClientID %>").click();
                }
            });
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
                     document.getElementById("<%= btn_SeleccionarTramite.ClientID %>").click();
                }
                if (botonverde == 0) {
                    document.getElementById("CheckboxAvaluo").checked = true;
                   <%-- document.getElementById("<%= btnConfirm.ClientID %>").click();--%>
                }

            });
            /*Oculta el Modal de confirmacion de Mani*/
            $("#modal-btn-noMani").click(function () {
                if (botonverde == 1) {
                    document.getElementById("<%= btn_SeleccionarTramite.ClientID %>").click();
                }
                if (botonverde == 0) {
                    <%--document.getElementById("<%= btnConfirm.ClientID %>").click();--%>
                }

            });
            /*Envia a la pagina para enviar archivos, solo Avalúo*/
            $("#modal-btn-siAvaluo").click(function () {
                if (botonverde == 1) {
                  document.getElementById("<%= btn_SeleccionarTramite.ClientID %>").click();
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
                    document.getElementById("<%= btn_SeleccionarTramite.ClientID %>").click();
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

</head>
<body>
    <div id="hgea"></div> <br /><br /><br /><br /><br />
    <form id="form1" runat="server">
       <div class="container">
           <div class="row">
               <div class="col-md-4">
                   <label>Clave catastral</label>
                   <asp:TextBox runat="server" ID="Txt_ClaveCatastralUbi" CssClass="form-control"></asp:TextBox>
               </div>
               <div class="col-md-4">
                   <label>Coordenada X</label>
                   <asp:TextBox runat="server" ID="Txt_CoordenadaX" CssClass="form-control" ></asp:TextBox>
               </div>
               <div class="col-md-4">
                   <label>Coordenada Y</label>
                   <asp:TextBox runat="server" ID="Txt_CoordenadaY" CssClass="form-control"></asp:TextBox>
               </div>
           </div>
            
            <div class="row">
                <b>Ubica en el mapa el predio</b>
                <div id="map"></div><br />
                <div class="row">
                    <div class="col-md-2">
                        <button type="button" onclick="downloadMapAsJPG()" class="btn btn-primary">Descargar Mapa</button>
                    </div>
                    <div class="col-md-2">
                        <button type="button" onclick="borrarDibujos()" class="btn btn-secondary">Borrar Dibujos</button>
                    </div>
                </div>
                <asp:HiddenField runat="server" ID="Hf_UTMX" Value="" /> 
                <asp:HiddenField runat="server" ID="Hf_UTMY" Value="" />
                
                <script async
                    src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCpjv33dzCsFd0llFZ9Zg11YQawUBFucLg&libraries=drawing&callback=initMap">
                </script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/proj4js/2.6.1/proj4.js"></script>
                <script>
                    var map;
                    var marker;
                    var markers = [];
                    var drawingManager;

                    function initMap() {
                        proj4.defs("EPSG:32613", "+proj=utm +zone=13 +north +ellps=WGS84 +datum=WGS84 +units=m +no_defs");

                        var ClaveCatastral = document.getElementById("<%= Txt_ClaveCatastralUbi.ClientID %>").value;
                        var municipio = ClaveCatastral.substring(0, 2);

                        var initialCenter = { lat: 21.88234, lng: -102.28259 };  // Default center (Aguascalientes)
                        var initialZoom = 13;

                        switch (municipio) {
                            case "01":
                                initialCenter = { lat: 21.88234, lng: -102.28259 }; // Aguascalientes
                                break;
                            case "02":
                                initialCenter = { lat: 22.23836, lng: -102.0894 }; // Asientos
                                break;
                            case "03":
                                initialCenter = { lat: 21.84583, lng: -102.71849 }; // Calvillo
                                break;
                            case "04":
                                initialCenter = { lat: 22.36625, lng: -102.30008 }; // Cosio
                                break;
                            case "05":
                                initialCenter = { lat: 21.96111, lng: -102.34333 }; // Jesus Maria
                                break;
                            case "06":
                                initialCenter = { lat: 22.14642, lng: -102.2769 }; // Pabellon de Areaga
                                break;
                            case "07":
                                initialCenter = { lat: 22.22819, lng: -102.32216 }; // Rincon de romos
                                break;
                            case "08":
                                initialCenter = { lat: 22.16918, lng: -102.59952 }; // San Jose de Gracia
                                break;
                            case "09":
                                initialCenter = { lat: 22.22393, lng: -102.1696 }; // Tepezala
                                break;
                            case "10":
                                initialCenter = { lat: 21.94389, lng: -102.32278 }; // El LLano
                                break;
                            case "11":
                                initialCenter = { lat: 22.07748, lng: -102.2714 }; // San Francisco de los romo
                                break;
                            // Add other cases as needed
                        }

                        map = new google.maps.Map(document.getElementById('map'), {
                            center: initialCenter,
                            zoom: initialZoom,
                        });

                        // Initialize Drawing Manager
                        drawingManager = new google.maps.drawing.DrawingManager({
                            drawingMode: google.maps.drawing.OverlayType.MARKER,
                            drawingControl: true,
                            drawingControlOptions: {
                                position: google.maps.ControlPosition.TOP_CENTER,
                                drawingModes: ['marker', 'circle', 'polygon', 'polyline', 'rectangle']
                            },
                            markerOptions: {
                                icon: {
                                    path: google.maps.SymbolPath.CIRCLE,
                                    fillColor: 'red',  // Color del marcador
                                    fillOpacity: 1,
                                    strokeWeight: 0,
                                    scale: 8
                                }
                            },
                            circleOptions: {
                                fillColor: '#ffff00',
                                fillOpacity: 0.5,
                                strokeWeight: 1,
                                clickable: false,
                                editable: true,
                                zIndex: 1
                            },
                            polygonOptions: {
                                strokeColor: 'red',  // Color de la polilínea
                                strokeOpacity: 1.0,
                                strokeWeight: 3
                            },
                            polylineOptions: {
                                strokeColor: 'red',  // Color de la polilínea
                                strokeOpacity: 1.0,
                                strokeWeight: 3
                            }
                        });
                        drawingManager.setMap(map);

                        //google.maps.event.addListener(map, "click", function (event) {
                        //    var clickedLatLng = event.latLng;
                        //    var lat = clickedLatLng.lat();
                        //    var lng = clickedLatLng.lng();

                        //    if (markers.length === 0) {
                        //        var clickedLatLng = event.latLng;
                        //        var markerUser = new google.maps.Marker({
                        //            position: clickedLatLng,
                        //            map: map,
                        //            title: "Marcador personalizado",
                        //        });
                        //        markers.push(markerUser);

                        //        map.setZoom(21);
                        //        map.setCenter(markerUser.getPosition());
                        //    }
                        //});
                        google.maps.event.addListener(drawingManager, 'overlaycomplete', function (event) {
                            if (event.type == google.maps.drawing.OverlayType.MARKER) {
                                var newMarker = event.overlay;
                                var lat = newMarker.getPosition().lat();
                                var lng = newMarker.getPosition().lng();

                                // Convertir a UTM
                                var utmCoords = proj4('EPSG:4326', 'EPSG:32613', [lng, lat]);

                                document.getElementById("<%= Hf_UTMX.ClientID %>").value = utmCoords[0];
                                document.getElementById("<%= Hf_UTMY.ClientID %>").value = utmCoords[1];

                                // Asignar valores a los TextBox
                                document.getElementById("<%= Txt_CoordenadaX.ClientID %>").value = lat;
                                document.getElementById("<%= Txt_CoordenadaY.ClientID %>").value = lng;

                                // Guardar el marcador
                                markers.push(newMarker);
                            }
                        });

                            
                        
                    }

                    function downloadMapAsJPG() {
                        var mapElement = document.getElementById('map');
                        html2canvas(mapElement, {
                            useCORS: true,
                            logging: true,
                            allowTaint: true,
                            backgroundColor: null
                        }).then(function (canvas) {
                            var link = document.createElement('a');
                            link.href = canvas.toDataURL('image/jpeg', 0.9);
                            link.download = 'map.jpg';
                            link.click();
                        }).catch(function (error) {
                            console.error("Error al capturar el mapa:", error);
                        });
                    }
                    function borrarDibujos() {
                        var ClaveCatastral = document.getElementById("<%= Txt_ClaveCatastralUbi.ClientID %>").value;
                        var municipio = ClaveCatastral.substring(0, 2);
                        document.getElementById("<%= Txt_CoordenadaX.ClientID %>").value = "";
                        document.getElementById("<%= Txt_CoordenadaY.ClientID %>").value = "";

                        switch (municipio) {
                            case "01":
                                initialCenter = { lat: 21.88234, lng: -102.28259 }; // Aguascalientes
                                break;
                            case "02":
                                initialCenter = { lat: 22.23836, lng: -102.0894 }; // Asientos
                                break;
                            case "03":
                                initialCenter = { lat: 21.84583, lng: -102.71849 }; // Calvillo
                                break;
                            case "04":
                                initialCenter = { lat: 22.36625, lng: -102.30008 }; // Cosio
                                break;
                            case "05":
                                initialCenter = { lat: 21.96111, lng: -102.34333 }; // Jesus Maria
                                break;
                            case "06":
                                initialCenter = { lat: 22.14642, lng: -102.2769 }; // Pabellon de Areaga
                                break;
                            case "07":
                                initialCenter = { lat: 22.22819, lng: -102.32216 }; // Rincon de romos
                                break;
                            case "08":
                                initialCenter = { lat: 22.16918, lng: -102.59952 }; // San Jose de Gracia
                                break;
                            case "09":
                                initialCenter = { lat: 22.22393, lng: -102.1696 }; // Tepezala
                                break;
                            case "10":
                                initialCenter = { lat: 21.94389, lng: -102.32278}; // El LLano
                                break;
                            case "11":
                                initialCenter = { lat: 22.07748, lng: -102.2714 }; // San Francisco de los romo
                                break;
                            
                        }

                        map = new google.maps.Map(document.getElementById('map'), {
                            center: initialCenter,
                            zoom: 13,
                        });

                        

                        drawingManager = new google.maps.drawing.DrawingManager({
                            drawingMode: google.maps.drawing.OverlayType.MARKER,
                            drawingControl: true,
                            drawingControlOptions: {
                                position: google.maps.ControlPosition.TOP_CENTER,
                                drawingModes: ['marker', 'circle', 'polygon', 'polyline', 'rectangle']
                            },
                            markerOptions: {
                                icon: {
                                    path: google.maps.SymbolPath.CIRCLE,
                                    fillColor: 'red',  // Color del marcador
                                    fillOpacity: 1,
                                    strokeWeight: 0,
                                    scale: 8
                                }
                            },
                            circleOptions: {
                                fillColor: '#ffff00',
                                fillOpacity: 0.5,
                                strokeWeight: 1,
                                clickable: false,
                                editable: true,
                                zIndex: 1
                            },
                            polygonOptions: {
                                strokeColor: 'red',  // Color de la polilínea
                                strokeOpacity: 1.0,
                                strokeWeight: 3
                            },
                            polylineOptions: {
                                strokeColor: 'red',  // Color de la polilínea
                                strokeOpacity: 1.0,
                                strokeWeight: 3
                            }

                        });

                        drawingManager.setMap(map);

                        google.maps.event.addListener(drawingManager, 'overlaycomplete', function (event) {
                            if (event.type == google.maps.drawing.OverlayType.MARKER) {
                                var newMarker = event.overlay;
                                var lat = newMarker.getPosition().lat();
                                var lng = newMarker.getPosition().lng();

                                // Asignar valores a los TextBox
                                document.getElementById("<%= Txt_CoordenadaX.ClientID %>").value = lat;
                                document.getElementById("<%= Txt_CoordenadaY.ClientID %>").value = lng;

                                // Guardar el marcador
                                markers.push(newMarker);
                            }
                        });


                       
                    }
                    
                </script>
            </div><br />
           <div class="row">
               <p>Instrucciones<br /> 
                   1. Navegue por el mapa y ubique el lugar exacto de su predio. <br />
                   2. Seleccione la opcion añadir un marcador y de click en el mapa.<br />
                   3. Utilize las herramientas y dibuje el perimetro de su predio.<br />
                   4. De clic en Descargar Mapa.<br />
                   5. Anexar el mapa junto con los otros documentos.<br />
               </p>
           </div>
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
                    <asp:BoundField DataField="AREA" HeaderText="SUPERFICIE M2" />
                   
                 </Columns>
            </asp:GridView>
                
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
                <asp:Button runat="server" ID="btn_SeleccionarTramite" CssClass="ocultarboton" Text="" OnClick="btn_SeleccionarTramite_Click" />
                <div class="col-md-2">
                    <asp:Button runat="server" ID="Btn_RegresarPagina" CssClass="btn btn-danger" Text="Regresar" OnClick="Btn_RegresarPagina_Click" />
                    
                </div>
                
                <%--<asp:button runat="server" ID="btnConfirm" CssClass="OcultarBoton" Text="" OnClick="EnviarPago"/>--%>
                
                 <%--<asp:Button runat="server" ID="BotonCancelar" CssClass="btn btn-danger" OnClick="BotonCancelar_Click" Text="Cancelar"/>--%>
            </div>
        </div>
        
        </div>
        </div>
    </form>
    <div id="fgea"></div>
</body>
</html>
