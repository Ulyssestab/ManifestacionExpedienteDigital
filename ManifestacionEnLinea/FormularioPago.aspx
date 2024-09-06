<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormularioPago.aspx.cs" Inherits="ManifestacionEnLinea.FormularioPago" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Instituto Registral y Catastral</title>
    <style>
        body{
            background-image: url(Imagenes/Iconos/fondo.jpg);
        }

        .Ocultar{
            display:none;
        }

        .infoCat{
         width: 20px;
         height: 20px;

        }
        .btn-cancelar {
            background-color: #dc3545;
            color: #fff;
            border-color: #dc3545;

        }
        .btn-cancelar:hover {
            background-color: #c82333;
            color: #fff;
            border-color: #bd2130;

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

        function eliminarPagoActivo(msg) {
            swal({
                title: 'Existe un folio de pago Activo',
                icon: 'warning',
                text: msg,
                buttons: {
                    catch: {
                        text: "Continuar",
                        value: "catch",
                    },
                    default: {
                        text: "Generar un folio nuevo",
                        value: "default",
                        className: 'btn-cancelar',
                        closeModal:true
                    },
                },
                buttonStyling: false,
                customClass: {
                    confirmButton: 'btn btn-primary',
                    cancelButton: 'btn btn-danger'
                }
            }).then((value) => {
                if (value === "default") {
                    swal("Ya puede ingresar nuevamente al sistema de pago.", {
                        icon: "success",
                    });
                    document.getElementById("<%= BotonResetPago.ClientID %>").click();

                }
                if (value === "catch") {
                    swal("Por Favor cerrar esta ventana.", {
                        icon: "success",
                    });
                }
            });
        }
    </script>
    <script>
        function openModal() {
            document.getElementById("btnmodalPagoActivo").click();
            
        }
    </script>
    <script>
        function Notapopupin() {
            var imggoin = document.getElementById("<%= popupMetodo.ClientID %>");
            imggoin.style.display = "block";
        }

        function Notapopupout() {
            
            var imggoin = document.getElementById("<%= popupMetodo.ClientID %>");
                imggoin.style.display = "none";
               
        }

        function Notapopupmpin() {
            var imggoin = document.getElementById("<%= popupMetodo3.ClientID %>");
            imggoin.style.display = "block";
        }

        function Notapopupmpout() {
            
            var imggoin = document.getElementById("<%= popupMetodo3.ClientID %>");
            imggoin.style.display = "none";

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
    <form id="FormularioPagoAvaluo" runat="server">
        <div class="container">
            <h1 class="titulo"> Pago de Avalúo Catastral</h1>
        <b>1. Ingresar nombre </b><br />
          <div class="row">
            <div class="col-md-6">
                <b>Nombre Completo</b>               
                <asp:TextBox runat="server" ID="nombreContribuyente" CssClass="form-control" placeholder="Nombre"></asp:TextBox>
                
            </div>
            
        </div>
        <div class="row">
            <div class="col-md-4">
                <b>Clave Catastral</b>
                <asp:TextBox runat="server" ID="ClaveOriginal" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
            <div class="row">
                <div class="col-md-4">
                    <b>Total a pagar</b>
                    <div style="display:flex; align-items:center">
                        <span style="margin-right:5px;">$</span>
                        <asp:TextBox runat="server" ID="Txt_Total" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                       <asp:Button runat="server" ID="botonEnviar" Text="Realizar Pago" CssClass="btn bg-2" onclick="botonEnviar_Click" />
                <asp:Button runat="server" ID="BotonResetPago" Text="Reset Pago" CssClass="Ocultar" OnClick="BotonResetPago_Click" />
                <asp:Button runat="server" ID="BotonRegresar" Text=" Regresar a la Pagina de Manifestación" CssClass="btn bg-3" OnClick="BotonRegresar_Click" />
                </div>
            </div>
              
        <br />
        <div class="row" style="margin-left:10px;" >
            <div  style="border:1px solid black; width: 55%; background-color:white">

            
            <label>INSTRUCCIONES</label> <br />
            <span ><b>1. Escribir nombre de la persona que realiza el pago.</b></span> <br />
            <span><b>2. Dar clic en el boton "Realizar Pago"</b></span> <br />
            <span><b>3. Seleccionar método de pago y si nececita factura </b></span> <asp:Image runat="server" ID="imgCatInfo" onmouseover="Notapopupin(this)" onmouseout="Notapopupout(this)" CssClass="infoCat" ImageUrl="~/Imagenes/Iconos/info.png" />
            <asp:Image runat="server" ID="popupMetodo" ImageUrl="~/Imagenes/Pago/CapturaMetodopago.PNG" style="display:none"  /> <br />
            <span><b>4. Dar clic en el botón continuar.</b></span><br />
            <span><b>5. Seleccionar forma de pago</b></span> <asp:Image runat="server" ID="ImageCatOpcs" onmouseover="Notapopupmpin(this)" onmouseout="Notapopupmpout(this)" CssClass="infoCat" ImageUrl="~/Imagenes/Iconos/info.png" />
            <asp:Image runat="server" ID="popupMetodo3" ImageUrl="~/Imagenes/Pago/CapturaMetodopago4.PNG" style="display:none"  /> <br />
            <span style="font-style:italic"><b> *Tranferencia</b></span> <br />
            <span>  -Nota: esta opcion nececita que previamente tenga contratado banca en linea con su banco. </span> <br />
            <span style="font-style:italic"><b>*Ventanilla Bancaria</b></span> 
            <span> - Dar clic en el botón Pagar en Ventanilla</span><br />
            <span> - Descargue el Formato para imprimirlo</span> <br />
            <span> - Presentar el documento y pagarlo en ventanilla bancaria de los siguientes bancos</span><br />
            <span> Banorte, Bancomer, Santander, Banamex, Scotiabank, HSBC, Telecomm, Banco Azteca, Banregio </span> <br />
            <span style="font-style:italic"><b> *Tarjeta de Crédito o Débito </b></span><br />
            <span> Seleccionar tipo de tarjeta VISA, Mastercard o American Express.</span>
            <span> Ingresar datos de la tarjeta y dar clic en el botón Pagar, al finalizar descargar comprobante de pago.</span><br />
            <span> <b> 6. Regresar a la página de Manifestación en Línea para subir sus documentos.</b></span> 
            </div>
        </div>
            </div>
    </form>
    
    <div id="fgea" ></div>


</body>
</html>
