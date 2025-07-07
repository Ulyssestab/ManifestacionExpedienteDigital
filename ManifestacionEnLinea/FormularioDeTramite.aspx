<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormularioDeTramite.aspx.cs" Inherits="ManifestacionEnLinea.FormularioDeTramite" Async="true" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Solicitud de Información Catastral</title>
    <style>
        body {
            background-image: url(Imagenes/Iconos/fondo.jpg);
        }

        .hidden {
            display: none;
        }

        .infoCat {
            width: 20px;
            height: 20px;
        }

        .form-section h5 {
            font-size: 1.2em;
            margin-top: 0;
            margin-bottom: 10px;
        }

        .form-group {
            margin-bottom: 10px;
        }

            .form-group label {
                display: block;
                font-size: 0.9em;
                margin-bottom: 5px;
                font-weight: bold;
            }

        .form-control {
            width: calc(100% - 12px);
            padding: 6px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
            font-size: 0.9em;
        }

        .form-row {
            display: flex;
            gap: 10px;
            margin-bottom: 10px;
        }

            .form-row .form-group {
                flex-grow: 1;
                margin-bottom: 0;
            }

        .form-check {
            display: inline-flex;
            align-items: center;
            margin-right: 15px;
            font-size: 0.9em;
        }

        .form-check-input {
            margin-right: 5px;
        }

        .form-check-label {
            margin-right: 0;
        }

        .form-control-file {
            font-size: 0.9em;
        }

        .titulo {
            text-align: center;
        }

        .bt {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 10vh;
        }
    </style>
    <link rel="Shortcut Icon" type="image/ico" href="https://eservicios2.aguascalientes.gob.mx/portalgea/images/Favicon.ico" />
    <link href="../Content/style.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="https://eservicios2.aguascalientes.gob.mx/portalgea/css/tema-sae.css" />

    <script src="https://eservicios2.aguascalientes.gob.mx/portalgea/js/geavue.js" type="text/javascript"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/mobile-detect@1.4.4/mobile-detect.min.js" type="text/javascript"></script>
    <script src="https://code.iconify.design/iconify-icon/1.0.0-beta.3/iconify-icon.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.5/dist/umd/popper.min.js" integrity="sha384-Xe+8cL9oJa6tN/veChSP7q+mnSPaj5Bcu9mPX5F5xIGE0DVittaqT5lorf0EI7Vk" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.min.js" integrity="sha384-ODmDIVzN+pFdexxHEHFBQH3/9/vQ9uori45z4JjnFsRydbmQbmL5t1tQ0culUzyK" crossorigin="anonymous"></script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.css" />
    <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.3/dist/leaflet.css" integrity="sha256-kLaT2GOSpHechhsozzB+flnD+zUyjE2LlfWPgU04xyI=" crossorigin="" />
    <script src="https://unpkg.com/leaflet@1.9.3/dist/leaflet.js" integrity="sha256-WBkoXOwTeyKclOHuWtc+i2uENFpDZ9YPdf5Hf+D7ewM=" crossorigin=""></script>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const terminadoRadio = document.getElementById('Terminado');
            const conservacionRadios = document.querySelectorAll('.estado-conservacion');

            terminadoRadio.addEventListener('change', function () {
                if (this.checked) {
                    conservacionRadios.forEach(radio => radio.disabled = true);
                }
            });


        //    const tipoGeneralRadios = document.querySelectorAll('input[name="tipo_general"]');

        //    tipoGeneralRadios.forEach(radio => {
        //        radio.addEventListener('change', function () {
        //            if (!terminadoRadio.checked) {
        //                conservacionRadios.forEach(r => r.disabled = false);
        //            }
        //        });
        //    });
        //});

        const radioButtons = document.querySelectorAll('.tipo-selector-general');

        radioButtons.forEach(radioButton => {
            radioButton.addEventListener('change', function () {
                radioButtons.forEach(otherRadioButton => {
                    if (otherRadioButton !== this) {
                        otherRadioButton.disabled = this.checked;
                    }
                });
            });
        });

        type = "text/javascript" >
            function setAvanceObra(valor) {
                document.getElementById('<%= HiddenAvanceObra.ClientID %>').value = valor;
            }

        type = "text/javascript" >
            function setestado_conserv(valor) {
                document.getElementById('<%= Hiddenestado_conserv.ClientID %>').value = valor;
            }

            type = "text/javascript" >
                function setestado_conserv(valor) {
                    document.getElementById('<%= HiddenTipo.ClientID %>').value = valor;
            }
        type = "text/javascript" >
            function setTipoUso(valor) {
                document.getElementById('<%= HiddenTipoUso.ClientID %>').value = valor;
                    document.getElementById('<%= HiddenTipoIndustrial.ClientID %>').value = "";
                    document.getElementById('<%= HiddenComercialServicio.ClientID %>').value = "";
            }

        function setTipoIndustrial(valor) {
            document.getElementById('<%= HiddenTipoUso.ClientID %>').value = "";
            document.getElementById('<%= HiddenTipoIndustrial.ClientID %>').value = valor;
            document.getElementById('<%= HiddenComercialServicio.ClientID %>').value = "";
        }

        function setComercialServicio(valor) {
            document.getElementById('<%= HiddenTipoUso.ClientID %>').value = "";
        document.getElementById('<%= HiddenTipoIndustrial.ClientID %>').value = "";
        document.getElementById('<%= HiddenComercialServicio.ClientID %>').value = valor;
        }

    </script>
</head>
<body>
    <div id="hgea"></div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <form runat="server">
        <h1 class="titulo">Solicitud de información Catastral</h1>
        <div class="container">
            <!-- Datos del Propietario -->
            <div class="form-section mb-3">
                <h5>I. CLAVE CATRASTRAL ORIGINAL</h5>
                <div class="form-row">
                    <div class="form-group">
                        <label for="nombre">CLAVE CATRASTRAL</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtClave" />
                    </div>
                </div>
            </div>
            <div class="form-section mb-3">
                <h5>II. DATOS DEL PROPIETARIO</h5>
                <div class="form-row">
                    <div class="form-group">
                        <label for="nombre">NOMBRE</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtNombre" />
                    </div>
                    <div class="form-group">
                        <label for="correo">CORREO ELECTRONICO</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtCorreo" TextMode="Email" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label for="curp">CURP.</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtCURP" />
                    </div>
                    <div class="form-group">
                        <label for="rfc">RFC</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtRFC" />
                    </div>
                    <div class="form-group">
                        <label for="telefono">TELEFONO</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtTelefono" TextMode="Phone" />
                    </div>
                </div>
            </div>
            <!-- Sección II: Domicilio del Propietario -->
            <div class="form-row">
                <div class="form-group">
                    <label for="TxtDomicilioPropietario">DOMICILIO (calle)</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TxtDomicilioPropietario" />
                </div>
                <div class="form-group">
                    <label for="TxtNoExtPropietario">No. Ext.</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TxtNoExtPropietario" />
                </div>
                <div class="form-group">
                    <label for="TxtNoIntPropietario">No. Int.</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TxtNoIntPropietario" />
                </div>
            </div>
            <div class="form-row">
                <div class="form-group">
                    <label for="TxtCalle">Colonia</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TxtCalle" />
                </div>
                <div class="form-group">
                    <label for="TxtCP">C.P.</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TxtCP" />
                </div>
                <div class="form-group">
                    <label for="TxtLocalidad">Localidad</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TxtLocalidad" />
                </div>
                <div class="form-group">
                    <label for="TxtMunicipio">Municipio</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TxtMunicipio" />
                </div>
            </div>

            <asp:Label ID="lblMensajeError" runat="server" CssClass="text-danger" Text=""></asp:Label>

            <!-- II. UBICACIÓN DEL INMUEBLE -->
            <div class="form-section mb-3">
                <h5>III. UBICACIÓN DEL INMUEBLE</h5>
                <div class="form-group">
                    <label for="TxtDomInmueble">DOMICILIO (calle)</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="TxtDomInmueble" />
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label for="TxtColInmueble">COLONIA</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtColInmueble" />
                    </div>
                    <div class="form-group">
                        <label for="TxtNoExtInmueble">No. Ext.</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtNoExtInmueble" />
                    </div>
                    <div class="form-group">
                        <label for="TxtNoIntInmueble">No. Int.</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtNoIntInmueble" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label for="TxtCPInmueble">C.P.</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtCPInmueble" />
                    </div>
                    <div class="form-group">
                        <label for="TxtLocalidadInmueble">Localidad</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtLocalidadInmueble" />
                    </div>
                    <div class="form-group">
                        <label for="TxtMunicipioInmueble">Municipio</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtMunicipioInmueble" />
                    </div>
                    <div class="form-group">
                        <label for="TxtLote">Lote</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtLote" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label for="TxtManzana">Manzana</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtManzana" />
                    </div>
                    <div class="form-group">
                        <label for="TxtIndiviso">% Indiviso</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TxtIndiviso" />
                    </div>
                    <div class="form-group">
                        <label for="TextFolio">Folio Real</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TextFolio" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label for="TxtCordenasx">Cordenadas X</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TextCordenadasx" />
                    </div>
                    <div class="form-group">
                        <label for="TxtCordenasy">Cordenadas Y</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TextCordenadasy" />
                    </div>
                </div>
            </div>

            <!-- III. CARACTERÍSTICAS DEL PREDIO -->
            <h5>IV. CARACTERÍSTICAS DEL PREDIO</h5>
            <div class="form-group">
                <label>TIPO</label><br>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general1" id="Rústico" value="Rústico">
                    <label class="form-check-label" for="Terminado">Rústico</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general1" id="TipoUrbano" value="Urbano">
                    <label class="form-check-label" for="Urbano">Urbano</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general1" id="TipoTransición" value="Transición">
                    <label class="form-check-label" for="Transición">Transición</label>
                </div>
                <asp:HiddenField ID="HiddenTipo" runat="server" />
            </div>
            <div class="form-group">
                <label for="TxtMunicipioInmueble">Superficie (M²)</label>
                <asp:TextBox runat="server" CssClass="form-control" ID="Textsuperficie_predio" />
            </div>
            <!-- Caracteristicas -->
            <div class="form-section mb-3">
                <h5>V. CARACTERÍSTICAS DE LA CONSTRUCCIÓN</h5>
                <div class="form-group">
                    <label for="superficie_construccion">Superficie de la Construcción</label>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label for="TxtMunicipioInmueble">Concreto (M²)</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TextConcreto" />
                    </div>
                    <div class="form-group">
                        <label for="TxtMunicipioInmueble">Tejaban (M²)</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TextTejaban" />
                    </div>
                    <div class="form-group">
                        <label for="TxtMunicipioInmueble">Total (M²)</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TextTotal" />
                    </div>
                </div>
            </div>


            <div class="form-group">
                <label>Avance de Obra:</label><br>

                <!-- Terminado 100% -->
                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="avance_obra" id="Terminado" value="Terminado" onclick="setAvanceObra(this.value)">
                    <label class="form-check-label" for="Terminado">Terminado 100%</label>
                </div>

                <!-- Obra negra -->
                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="avance_obra" id="Bajo" value="Bajo" onclick="setAvanceObra(this.value)">
                    <label class="form-check-label" for="Bajo">51-61%</label>
                </div>

                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="avance_obra" id="SemiCompleto" value="SemiCompleto" onclick="setAvanceObra(this.value)">
                    <label class="form-check-label" for="SemiCompleto">64-72%</label>
                </div>

                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="avance_obra" id="Completo" value="Completo" onclick="setAvanceObra(this.value)">
                    <label class="form-check-label" for="Completo">80-85%</label>
                </div>
               
                 <!-- HiddenField para almacenar el valor seleccionado -->
                <asp:HiddenField ID="HiddenAvanceObra" runat="server" />
                </div>

                  <div class="form-group">
                <label>Estado de conservación</label><br>
                <!-- Estado De Conservacion -->
                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="estado_conserv" id="bueno" value="bueno" onclick="setestado_conserv(this.value)">
                    <label class="form-check-label" for="bueno">Bueno</label>
                </div>

                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="estado_conserv" id="regular" value="regular" onclick="setestado_conserv(this.value)">
                    <label class="form-check-label" for="regular">Regular</label>
                </div>

                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="estado_conserv" id="malo" value="malo" onclick="setestado_conserv(this.value)">
                    <label class="form-check-label" for="malo">Malo</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="estado_conserv" id="ruinas" value="ruinas" onclick="setestado_conserv(this.value)">
                    <label class="form-check-label" for="ruinas">Ruinas</label>
                </div>


                <!-- HiddenField para almacenar el valor seleccionado -->
   
                <asp:HiddenField ID="Hiddenestado_conserv" runat="server" />
            </div>


            <div class="form-group">
                <label for="Textantiguedad">Antigüedad (Años)</label>
                <asp:TextBox runat="server" CssClass="form-control" ID="Textantiguedad" />
            </div>

            <!-- TIPOS -->
            <div class="form-group">
                <label>Tipo:</label><br>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="Alta" value="Alta" onclick="setTipoUso(this.value)">
                    <label class="form-check-label" for="Alta">Alta</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="MediaAlta" value="Alta" onclick="setTipoUso(this.value)">
                    <label class="form-check-label" for="Malta">Media Alta</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="MediaBaja" value="Alta" onclick="setTipoUso(this.value)">
                    <label class="form-check-label" for="Mbaja">Media Baja</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="Social" value="Alta" onclick="setTipoUso(this.value)">
                    <label class="form-check-label" for="Social">Interes Social</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="Popular" value="Alta" onclick="setTipoUso(this.value)">
                    <label class="form-check-label" for="Popular">Popular</label>
                </div>
            </div>
            <!--TIPO INDUSTRIAL -->
            <div class="form-group">
                <label>Tipo industrial:</label><br>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="Pesado" value="Pesado" onclick="setTipoIndustrial(this.value)">
                    <label class="form-check-label" for="Pesado">Pesado</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="semiPesado" value="Pesado" onclick="setTipoIndustrial(this.value)">
                    <label class="form-check-label" for="Semipesado">Semi Pesado</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="Ligero" value="Pesado" onclick="setTipoIndustrial(this.value)">
                    <label class="form-check-label" for="Ligero">Ligero</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="Bodega" value="Pesado" onclick="setTipoIndustrial(this.value)">
                    <label class="form-check-label" for="Bodegas">Bodegas</label>
                </div>
            </div>
            <!--TIPO COMERCIAL Y DE SERVICIO -->
            <div class="form-group">
                <label>Comercial y de servicio</label><br>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="alto_comercial" value="Alto" onclick="setComercialServicio(this.value)">
                    <label class="form-check-label" for="alto_comercial">Alto</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="medio_comercial" value="Alto" onclick="setComercialServicio(this.value)">
                    <label class="form-check-label" for="medio_comercial">Medio</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input tipo-selector-general" type="radio" name="tipo_general" id="bajo_comercial" value="Alto" onclick="setComercialServicio(this.value)">
                    <label class="form-check-label" for="bajo_comercial">Bajo</label>
                </div>
            </div>
            <asp:HiddenField ID="HiddenTipoUso" runat="server" />
            <asp:HiddenField ID="HiddenTipoIndustrial" runat="server" />
            <asp:HiddenField ID="HiddenComercialServicio" runat="server" />


            <div class="row">
                <div class="col-12">
                    <h5>VI. REQUISITOS DEL TRAMITE</h5>
                    <asp:GridView ID="GridRevision" AutoGenerateColumns="false" AllowPaging="false" runat="server" CssClass="table table-bordered table-condensed table-responsive table-hover" OnRowDataBound="GridRevision_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="#" ReadOnly="true" ItemStyle-CssClass="TextoListadoDocumentos text-center" HeaderStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Documento" HeaderText="Documento" ReadOnly="true" HeaderStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" ReadOnly="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                            <asp:TemplateField HeaderText="Ver Documento" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="VerDocumentacion" runat="server" Text="Ver" CssClass="center-content" OnClick="VerDocumentacion_Click" CommandArgument='<%# Container.DataItemIndex %>'><span class="iconify" data-icon="bi:eye" style="font-size:40px;"></span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <br />
            <div class="form-group">
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" id="declaracion_verdad" required>
                    <label class="form-check-label" for="declaracion_verdad">
                        Declaro bajo protesta de decir Verdad, que la información declarada en la presente manifestación de catastral, es cierta, en caso contrario quedo enterado de las sanciones que pudiera incurrir establecidas en los Artículos 104 y 105 de la Ley de Catastro del Estado de Aguascalientes.
                    </label>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col">
                    <p class="note">NOTA: TODOS LOS ESPACIOS DEBERÁN DE SER DEBIDAMENTE LLENADOS</p>
                </div>
            </div>
            </div>
            <div class="row">
                <div class="col-md-4 mx-auto text-center bt">
                    <asp:Button ID="Redirigir" runat="server" OnClick="Redirigir_Click" Text="Enviar" CssClass="btn btn-primary" />
                </div>
            </div>
        <!-- </div> -->
    </form>
    <div id="fgea"></div>
</body>
</html>
