using AddonFE.IntegradorBizLinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AddonFE.DTO
{
    [XmlRoot("SignOnLineSummaryCmd")]
    public  class ResumenAnulacionDTO
    {
        [XmlAttribute("declare-sunat")]
        public string DeclareSunat { get; set; }

        [XmlAttribute("replicate")]
        public string Replicate { get; set; }

        [XmlAttribute("output")]
        public string output { get; set; }

        [XmlElement("parameter")]
        public string parameter { get; set; }

        [XmlElement("documento")]
        public DocumentoAnulacion documento { get; set; }
    }
    public class DocumentoAnulacion
    {
        [XmlElement("numeroDocumentoEmisor")]
        public string numeroDocumentoEmisor { get; set; }

        [XmlElement("version")]
        public string version { get; set; }

        [XmlElement("versionUBL")]
        public string versionUBL { get; set; }

        [XmlElement("tipoDocumentoEmisor")]
        public string tipoDocumentoEmisor { get; set; }

        [XmlElement("resumenId")]
        public string resumenId { get; set; }

        [XmlElement("fechaEmisionComprobante")]
        public string fechaEmisionComprobante { get; set; }

        [XmlElement("fechaGeneracionResumen")]
        public string fechaGeneracionResumen { get; set; }

        [XmlElement("razonSocialEmisor")]
        public string razonSocialEmisor { get; set; }

        [XmlElement("correoEmisor")]
        public string correoEmisor { get; set; }

        [XmlElement("inHabilitado")]
        public int inHabilitado { get; set; }

        [XmlElement("resumenTipo")]
        public string resumenTipo { get; set; }

        [XmlElement("ResumenItem")]
        public ResumenItem resumenItem { get; set; }
    }

    
    public class ResumenItem
    {
        [XmlElement("numeroFila")]
        public int numeroFila { get; set; }

        [XmlElement("tipoDocumento")]
        public string tipoDocumento { get; set; }

        [XmlElement("serieDocumentoBaja")]
        public string serieDocumentoBaja { get; set; }

        [XmlElement("numeroDocumentoBaja")]
        public string numeroDocumentoBaja { get; set; }

        [XmlElement("motivoBaja")]
        public string motivoBaja { get; set; }
    }
}
