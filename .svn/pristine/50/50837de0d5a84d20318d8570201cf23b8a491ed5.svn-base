<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pago.aspx.cs" Inherits="ManifestacionEnLinea.Pago" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        .hidden{
            display:none;
        }
    </style>
    <script>
        function llamarServicio() {
            var referenciaPago = document.getElementById("<%= referencia.ClientID %>").value;
            var NombrePagoAvaluoJS = document.getElementById("<%= NombrePersona.ClientID %>").value;
            var CuentaCatastral = document.getElementById("<%= CuentaCatastralPago.ClientID %>").value;
            var Precio = document.getElementById("<%= HF_Precio.ClientID %>").value;
            console.log(Precio);
            var hoy = new Date();
            var mes = hoy.getMonth();
            var dia = hoy.getDate();
            var anio = hoy.getFullYear();

            if (mes === 0) {
                var messtr = '01';
            }
            if (mes === 1) {
                var messtr = '02';
            }
            if (mes === 2) {
                var messtr = '03';
            }
            if (mes === 3) {
                var messtr = '04';
            }
            if (mes === 4) {
                var messtr = '05';
            }
            if (mes === 5) {
                var messtr = '06';
            }
            if (mes === 6) {
                var messtr = '07';
            }
            if (mes === 7) {
                var messtr = '08';
            }
            if (mes === 8) {
                var messtr = '09';
            }
            if (mes === 9) {
                var messtr = '10';
            }
            if (mes === 10) {
                var messtr = '11';
            }
            if (mes === 11) {
                var messtr = '12';
            }

            if (dia === 1) {
                var diastr = '01';
            }
            if (dia === 2) {
                var diastr = '02';
            }
            if (dia === 3) {
                var diastr = '03';
            }
            if (dia === 4) {
                var diastr = '04';
            }
            if (dia === 5) {
                var diastr = '05';
            }
            if (dia === 6) {
                var diastr = '06';
            }
            if (dia === 7) {
                var diastr = '07';
            }
            if (dia === 8) {
                var diastr = '08';
            }
            if (dia === 9) {
                var diastr = '09';
            }
            if (dia === 10) {
                var diastr = '10'
            }
            if (dia === 11) {
                var diastr = '11';
            }
            if (dia === 12) {
                var diastr = '12';
            }
            if (dia === 13) {
                var diastr = '13';
            }
            if (dia === 14) {
                var diastr = '14';
            }
            if (dia === 15) {
                var diastr = '15';
            }
            if (dia === 16) {
                var diastr = '16';
            }
            if (dia === 17) {
                var diastr = '17';
            }
            if (dia === 18) {
                var diastr = '18';
            }
            if (dia === 19) {
                var diastr = '19';
            }
            if (dia === 20) {
                var diastr = '20';
            }
            if (dia === 21) {
                var diastr = '21';
            }
            if (dia === 22) {
                var diastr = '22';
            }
            if (dia === 23) {
                var diastr = '23';
            }
            if (dia === 24) {
                var diastr = '24';
            }
            if (dia === 25) {
                var diastr = '25';
            }
            if (dia === 26) {
                var diastr = '26';
            }
            if (dia === 27) {
                var diastr = '27';
            }
            if (dia === 28) {
                var diastr = '28';
            }
            if (dia === 29) {
                var diastr = '29';
            }
            if (dia === 30) {
                var diastr = '30';
            }
            if (dia === 31) {
                var diastr = '31';
            }

            var objdatoscobro = {
                "rfc": "SINRFC", "curp": "", "nombre": NombrePagoAvaluoJS,
                "calle": "", "no_ext": "", "no_int": "0", "colonia": "",
                "codpos": 0, "telefono": "", "municipio": 1, "ent_fede": 1, "poblacion": 1, "observacion":
                    referenciaPago + " " + CuentaCatastral, "servicio": 2, "referencia": referenciaPago, "total": 198,
                "vigencia": '2025-12-31', "urlRetorno": "https://eservicios2.aguascalientes.gob.mx/IRC/ManifestacionEnLinea/index.aspx",
                "conceptos": [{ "concepto": "4020306010703", "cantidad": 1, "precio": 198 }],
            };

            var parametros = JSON.stringify(objdatoscobro, null, 2);
            var txValue = document.getElementById('datosCobro');
            txValue.value = parametros;
            var formulario = document.getElementById('formularioPago');
            formulario.submit();
        }
    </script>
</head>
<body>
    <%--<form id="formularioPago" runat="server" method="post" action="https://eservicios2.aguascalientes.gob.mx/Contribuciones/ServicioCobro.aspx">--%>
        <%--<form id="formularioPago" runat="server" method="post" action="https://epagos.aguascalientes.gob.mx/contribuciones/ServicioCobro.aspx">--%>
    <form id="formularioPago" runat="server" method="post" action="https://epagos.aguascalientes.gob.mx/contribuciones/ServicioCobro.aspx">
    <%--<form id="formularioPago" runat="server" method="post" action="https://eservicios2.aguascalientes.gob.mx/sefi/bancos/ServicioCobro.aspx"> Pruebas--%> 
        <div>
            <h1>ESPERE UN MOMENTO, NO CIERRE LA VENTANA...</h1>
            <input class="hidden" id="nombreCompletoPago" type="text"  />
            <input class="hidden" id="datosCobro" type="text" name="datosCobro" />
            <asp:HiddenField runat="server" ID="referencia" />
            <asp:HiddenField runat="server" ID="NombrePersona" />
            <asp:HiddenField runat="server" ID="CuentaCatastralPago" />
            <asp:HiddenField runat="server" ID="HF_Precio" />
        </div>
    </form>
</body>
</html>
