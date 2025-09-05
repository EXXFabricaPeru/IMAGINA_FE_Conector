using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AddonFE.DTO
{
    internal class ComprobanteFEDTO
    {
    }

    public class SignOnLineCmd
    {
        [XmlAttribute("declare-sunat")]
        public string DeclareSunat { get; set; }

        [XmlAttribute("declare-direct-sunat")]
        public string DeclareDirectSunat { get; set; }

        [XmlAttribute("publish")]
        public string Publish { get; set; }

        [XmlAttribute("output")]
        public string Output { get; set; }

        [XmlAttribute("contingencia")]
        public string Contingencia { get; set; }

        public string parameter { get; set; }

        [XmlElement("documento")]
        public DocumentoFE documento { get; set; }
    }
    public class DocumentoFE
    {
        [XmlElement("correoEmisor")]
        public string correoEmisor { get; set; }

        [XmlElement("correoAdquiriente")]
        public string correoAdquiriente { get; set; }

        [XmlElement("serieNumero")]
        public string serieNumero { get; set; }

        [XmlElement("fechaEmision")]
        public string fechaEmision { get; set; }

        [XmlElement("tipoDocumento")]
        public string tipoDocumento { get; set; }

        [XmlElement("tipoMoneda")]
        public string tipoMoneda { get; set; }

        [XmlElement("numeroDocumentoEmisor")]
        public string numeroDocumentoEmisor { get; set; }

        [XmlElement("tipoDocumentoEmisor")]
        public int tipoDocumentoEmisor { get; set; }

        [XmlElement("nombreComercialEmisor")]
        public string nombreComercialEmisor { get; set; }

        [XmlElement("razonSocialEmisor")]
        public string razonSocialEmisor { get; set; }

        [XmlElement("direccionEmisor")]
        public string direccionEmisor { get; set; }

        [XmlElement("provinciaEmisor")]
        public string provinciaEmisor { get; set; }

        [XmlElement("ubigeoEmisor")]
        public string ubigeoEmisor { get; set; }

        [XmlElement("departamentoEmisor")]
        public string departamentoEmisor { get; set; }

        [XmlElement("distritoEmisor")]
        public string distritoEmisor { get; set; }

        [XmlElement("paisEmisor")]
        public string paisEmisor { get; set; }

        [XmlElement("codigoLocalAnexoEmisor")]
        public string codigoLocalAnexoEmisor { get; set; }

        [XmlElement("numeroDocumentoAdquiriente")]
        public string numeroDocumentoAdquiriente { get; set; }

        [XmlElement("tipoDocumentoAdquiriente")]
        public int tipoDocumentoAdquiriente { get; set; }

        [XmlElement("razonSocialAdquiriente")]
        public string razonSocialAdquiriente { get; set; }

        [XmlElement("direccionAdquiriente")]
        public string direccionAdquiriente { get; set; }

        [XmlElement("urbanizacionAdquiriente")]
        public string urbanizacionAdquiriente { get; set; }

        [XmlElement("provinciaAdquiriente")]
        public string provinciaAdquiriente { get; set; }

        [XmlElement("ubigeoAdquiriente")]
        public string ubigeoAdquiriente { get; set; }

        [XmlElement("departamentoAdquiriente")]
        public string departamentoAdquiriente { get; set; }

        [XmlElement("distritoAdquiriente")]
        public string distritoAdquiriente { get; set; }

        [XmlElement("paisAdquiriente")]
        public string paisAdquiriente { get; set; }

        [XmlElement("totalImpuestos")]
        public decimal totalImpuestos { get; set; }

        [XmlElement("totalValorVentaNetoOpGratuitas")]
        public decimal totalValorVentaNetoOpGratuitas { get; set; }

        [XmlElement("totalValorVentaNetoOpGravadas")]
        public decimal totalValorVentaNetoOpGravadas { get; set; }

        [XmlElement("totalValorVentaNetoOpNoGravada")]
        public decimal totalValorVentaNetoOpNoGravada { get; set; }


        [XmlElement("totalIgv")]
        public decimal totalIgv { get; set; }

        [XmlElement("totalVenta")]
        public decimal totalVenta { get; set; }

        [XmlElement("totalValorVenta")]
        public decimal totalValorVenta { get; set; }

        [XmlElement("totalPrecioVenta")]
        public decimal totalPrecioVenta { get; set; }

        [XmlElement("montoRedondeoTotalVenta")]
        public decimal montoRedondeoTotalVenta { get; set; }

        [XmlElement("tipoOperacion")]
        public string tipoOperacion { get; set; }

        [XmlElement("codigoLeyenda_1")]
        public string codigoLeyenda_1 { get; set; }

        [XmlElement("textoLeyenda_1")]
        public string textoLeyenda_1 { get; set; }

        [XmlElement("codigoLeyenda_4")]
        public string codigoLeyenda_4 { get; set; }

        [XmlElement("textoLeyenda_4")]
        public string textoLeyenda_4 { get; set; }

        [XmlElement("formaPagoNegociable")]
        public int formaPagoNegociable { get; set; }

        [XmlElement("totalTributosOpeGratuitas")]
        public decimal totalTributosOpeGratuitas { get; set; }

        [XmlElement("item")]
        public List<Item> Items { get; set; }

        [XmlElement("anticipo")]
        public List<Anticipo> Anticipos { get; set; }

        [XmlElement("codigoSerieNumeroAfectado")]
        public string codigoSerieNumeroAfectado { get; set; } //Nota de Credito

        [XmlElement("motivoDocumento")]
        public string motivoDocumento { get; set; } //Nota de Credito

        [XmlElement("tipoDocumentoReferenciaPrincipal")]
        public string tipoDocumentoReferenciaPrincipal { get; set; } //Nota de Credito


        [XmlElement("numeroDocumentoReferenciaPrincipal")]
        public string numeroDocumentoReferenciaPrincipal { get; set; } //Nota de Credito

        #region Cuotas


        [XmlElement("montoNetoPendiente")]
        public decimal? montoNetoPendiente { get; set; }

        [XmlElement("montoPagoCuota1")]
        public decimal? montoPagoCuota1 { get; set; }

        [XmlElement("fechaPagoCuota1")]
        public string fechaPagoCuota1 { get; set; }

        [XmlElement("montoPagoCuota2")]
        public decimal? montoPagoCuota2 { get; set; }

        [XmlElement("fechaPagoCuota2")]
        public string fechaPagoCuota2 { get; set; }

        [XmlElement("montoPagoCuota3")]
        public decimal? montoPagoCuota3 { get; set; }

        [XmlElement("fechaPagoCuota3")]
        public string fechaPagoCuota3 { get; set; }
        #endregion

        #region Retencion
        [XmlElement("importeOpeRetencion")]
        public decimal? importeOpeRetencion { get; set; }

        [XmlElement("porcentajeRetencion")]
        public decimal? porcentajeRetencion { get; set; }

        [XmlElement("importeRetencion")]
        public decimal? importeRetencion { get; set; }

        [XmlElement("inHabilitado")]
        public string inHabilitado { get; set; } //Nota de Credito
        #endregion


        #region Detraccion
        [XmlElement("codigoAuxiliar100_5")]
        public string codigoAuxiliar100_5 { get; set; }

        [XmlElement("textoAuxiliar100_5")]
        public string textoAuxiliar100_5 { get; set; }

        [XmlElement("codigoAuxiliar500_1")]
        public string codigoAuxiliar500_1 { get; set; }

        [XmlElement("textoAuxiliar500_1")]
        public string textoAuxiliar500_1 { get; set; }

        [XmlElement("codigoDetraccion")]
        public string codigoDetraccion { get; set; }

        [XmlElement("numeroCtaBancoNacion")]
        public string numeroCtaBancoNacion { get; set; }

        [XmlElement("formaPago")]
        public string formaPago { get; set; }

        [XmlElement("totalDetraccion")]
        public string totalDetraccion { get; set; }

        [XmlElement("porcentajeDetraccion")]
        public string porcentajeDetraccion { get; set; }
        #endregion


        [XmlElement("tipoReferencia_1")]
        public string tipoReferencia_1 { get; set; }
        [XmlElement("numeroDocumentoReferencia_1")]
        public string numeroDocumentoReferencia_1 { get; set; }

        [XmlElement("ordenCompra")]
        public string ordenCompra { get; set; }

        #region anticipos
        [XmlElement("montoBaseDsctoGlobalAnticipo")]
        public decimal? montoBaseDsctoGlobalAnticipo { get; set; }

        [XmlElement("porcentajeDsctoGlobalAnticipo")]
        public decimal? porcentajeDsctoGlobalAnticipo { get; set; }

        [XmlElement("totalDsctoGlobalesAnticipo")]
        public decimal? totalDsctoGlobalesAnticipo { get; set; }

        [XmlElement("totalDocumentoAnticipo")]
        public decimal? totalDocumentoAnticipo { get; set; }

        #endregion


    }

    public class Item
    {
        [XmlElement("numeroOrdenItem")]
        public int numeroOrdenItem { get; set; }

        [XmlElement("unidadMedida")]
        public string unidadMedida { get; set; }

        [XmlElement("cantidad")]
        public decimal cantidad { get; set; }

        [XmlElement("codigoProducto")]
        public string codigoProducto { get; set; }

        [XmlElement("codigoProductoSUNAT")]
        public string codigoProductoSUNAT { get; set; }

        [XmlElement("descripcion")]
        public string descripcion { get; set; }

        [XmlElement("importeUnitarioSinImpuesto")]
        public decimal importeUnitarioSinImpuesto { get; set; }

        [XmlElement("importeUnitarioConImpuesto")]
        public decimal importeUnitarioConImpuesto { get; set; }

        [XmlElement("codigoImporteUnitarioConImpuesto")]
        public string codigoImporteUnitarioConImpuesto { get; set; }

        [XmlElement("importeReferencial")]
        public decimal importeReferencial { get; set; }

        [XmlElement("codigoImporteReferencial")]
        public string codigoImporteReferencial { get; set; }

        [XmlElement("importeTotalImpuestos")]
        public decimal importeTotalImpuestos { get; set; }

        [XmlElement("montoBaseIgv")]
        public decimal montoBaseIgv { get; set; }

        [XmlElement("tasaIgv")]
        public decimal tasaIgv { get; set; }

        [XmlElement("codigoRazonExoneracion")]
        public string codigoRazonExoneracion { get; set; }

        [XmlElement("importeIgv")]
        public decimal importeIgv { get; set; }

        [XmlElement("importeTotalSinImpuesto")]
        public decimal importeTotalSinImpuesto { get; set; }
    }

    public class Anticipo
    {
        [XmlElement("indicador")]
        public string indicador { get; set; }

        [XmlElement("numeroOrdenAnticipo")]
        public int numeroOrdenAnticipo { get; set; }


        [XmlElement("totalPrepagadoAnticipo")]
        public decimal totalPrepagadoAnticipo { get; set; }

        [XmlElement("fechaPago")]
        public string fechaPago { get; set; }

        [XmlElement("serieNumeroDocumentoAnticipo")]
        public string serieNumeroDocumentoAnticipo { get; set; }


        [XmlElement("tipoDocumentoAnticipo")]
        public string tipoDocumentoAnticipo { get; set; }


        [XmlElement("numeroDocumentoEmisorAnticipo")]
        public string numeroDocumentoEmisorAnticipo { get; set; }

        [XmlElement("tipoDocumentoEmisorAnticipo")]
        public string tipoDocumentoEmisorAnticipo { get; set; }
    }
}
