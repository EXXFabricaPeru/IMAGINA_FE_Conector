using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace AddonFE.Configuraciones
{
    class FuncionesRequeridas
    {

        public string SerializeToXml<T>(T obj)
        {
            // Crear un XmlSerializer para el tipo de objeto DTO
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Crear un StringWriter para almacenar el XML serializado
            StringWriter stringWriter = new StringWriter();

            // Configurar el XmlSerializer para omitir los espacios de nombres XML predeterminados
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty); // Agregar un espacio de nombres vacío

            // Configurar el XmlWriterSettings para excluir la declaración XML
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false // Puedes activar la indentación si deseas
            };

            // Serializar el objeto en XML y escribirlo en el StringWriter
            #region ANTES
            
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, obj, namespaces);
            }
            /*
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                foreach (var prop in typeof(T).GetProperties())
                {
                    // Obtener el valor de la propiedad
                    var value = prop.GetValue(obj);

                    // Verificar si el valor es 0 (para números enteros) o null
                    if (value != null && !(value is int intValue && intValue == 0))
                    {
                        // Agregar el tag al XML solo si el valor no es 0 o null
                        serializer.Serialize(xmlWriter, new XmlElementWrapper(prop.Name, value), namespaces);
                    }
                }
            }*/
            #endregion

            // Obtener la cadena XML resultante
            string xmlString = stringWriter.ToString();

            return xmlString;

            /*

            // Serializar el objeto en XML y escribirlo en el StringWriter
            serializer.Serialize(stringWriter, obj);

            // Obtener la cadena XML resultante
            string xmlString = stringWriter.ToString();

            return xmlString;*/
        }
        // Clase auxiliar para envolver los valores de las propiedades en un contenedor XmlElement
        public class XmlElementWrapper
        {
            public XmlElementWrapper(string name, object value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; set; }
            public object Value { get; set; }
        }

        #region DescargarArchivoURL
        public  void downloadFileToSpecificPath(string strURLFile, string strPathToSave)
        {
            // Se encierra el código dentro de un bloque try-catch.
            try
            {
                // Se valida que la URL no esté en blanco.
                if (String.IsNullOrEmpty(strURLFile))
                {
                    // Se retorna un mensaje de error al usuario.
                    throw new ArgumentNullException("La dirección URL del documento es nula o se encuentra en blanco.");
                }// Fin del if que valida que la URL no esté en blanco.

                // Se valida que la ruta física no esté en blanco.
                if (String.IsNullOrEmpty(strPathToSave))
                {
                    // Se retorna un mensaje de error al usuario.
                    throw new ArgumentNullException("La ruta para almacenar el documento es nula o se encuentra en blanco.");
                }// Fin del if que valida que la ruta física no esté en blanco.

                // Se descargar el archivo indicado en la ruta específicada.
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.DownloadFile(strURLFile, strPathToSave);
                }// Fin del using para descargar archivos.
            }// Fin del try.
            catch (Exception ex)
            {

                Program.SboAplicacion.MessageBox("AQUI ERROR:"+ex.ToString());
                // Se retorna la excepción al cliente.
                throw ex;
            }// Fin del catch.
        }// Fin del método downloadFileToSpecificPath.
        #endregion
    }
}
