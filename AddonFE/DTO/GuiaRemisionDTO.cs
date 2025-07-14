using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AddonFE.DTO
{
    internal class GuiaRemisionDTO
    {
    }

    public class SignOnLineDespatchCmd
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
        public Documento documento { get; set; }
    }


    public class Documento
    {
  
        public string correoEmisor { get; set; }
        public string correoAdquiriente { get; set; }
        public string serieNumeroGuia { get; set; }
        public string fechaEmisionGuia { get; set; }
        public string horaEmisionGuia { get; set; }
        public string tipoDocumentoGuia { get; set; }
        public string observaciones { get; set; }
        public string numeroDocumentoRemitente { get; set; }
        public string tipoDocumentoRemitente { get; set; }
        public string razonSocialRemitente { get; set; }
        public string numeroDocumentoDestinatario { get; set; }
        public string tipoDocumentoDestinatario { get; set; }
        public string razonSocialDestinatario { get; set; }
        public string motivoTraslado { get; set; }
        public int pesoBrutoTotalBienes { get; set; }
        public string unidadMedidaPesoBruto { get; set; }
        public string modalidadTraslado { get; set; }
        public string fechaInicioTraslado { get; set; }
        public string fechaEntregaBienes { get; set; }
        public string ubigeoPtoPartida { get; set; }
        public string direccionPtoPartida { get; set; }
        public string ubigeoPtoLLegada { get; set; }
        public string direccionPtoLLegada { get; set; }
        public string tipoDocumentoConductor { get; set; }
        public string numeroDocumentoConductor { get; set; }
        public string nombreConductor { get; set; }
        public string apellidoConductor { get; set; }
        public string numeroLicencia { get; set; }
        public string numeroPlacaVehiculoPrin { get; set; }
        public DocumentoRelacionado documentoRelacionado { get; set; }
        public GuiaItem[] GuiaItem { get; set; }
    }

    public class GuiaItem
    {
        public string indicador { get; set; }
        public int numeroOrdenItem { get; set; }
        public int cantidad { get; set; }
        public string unidadMedida { get; set; }
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public string codigoProductoSUNAT { get; set; }
    }

    public class DocumentoRelacionado
    {
        public string indicador { get; set; }
        public int ordenDocRel { get; set; }
        public string tipoDocumentoDocRel { get; set; }
        public string codigoDocumentoDocRel { get; set; }
        public string numeroDocumentoDocRel { get; set; }
        public string numeroDocumentoEmisorDocRel { get; set; }
        public string tipoDocumentoEmisorDocRel { get; set; }
    }
}
