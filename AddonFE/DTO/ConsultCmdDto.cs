using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AddonFE.DTO
{
    [XmlRoot("ConsultCmd")]
    public class ConsultCmdDto
    {
        [XmlAttribute("output")]
        public string Output { get; set; }

        [XmlElement("parametros")]
        public Parametros Parametros { get; set; }
        public ConsultCmdDto()
        {
            Parametros = new Parametros();
        }
    }

    public class Parametros
    {
        [XmlElement("parameter")]
        public Parameter[] Parameters { get; set; }
    }

    public class Parameter
    {
        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
