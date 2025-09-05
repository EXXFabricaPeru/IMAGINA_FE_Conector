using AddonFE.Configuraciones;
using AddonFE.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddonFE.Main
{
    class MetodosGuia
    {
        string dbName = Util.DbaseName;
        public string obtenerGuiaRemision(int DocEntry, ref string mensaje, int Formulario, SAPbobsCOM.Company oCompany)
        {
            Documento oDocumento = new Documento();

            GuiaItem oGuiaItem = new GuiaItem();
            string nombre_Store = "";
            SAPbobsCOM.Recordset oRecGuia = default(SAPbobsCOM.Recordset);
            Procedures oProcedures = new Procedures(oCompany);
            FuncionesRequeridas oFuncionesRequeridas = new FuncionesRequeridas();

            #region Cabecera
            SignOnLineDespatchCmd oSignOnLineCmdDTO = new SignOnLineDespatchCmd();
            oSignOnLineCmdDTO.DeclareSunat = "0";
            oSignOnLineCmdDTO.DeclareDirectSunat = "1";
            oSignOnLineCmdDTO.Publish = "1";
            oSignOnLineCmdDTO.Output = "PDF";
            oSignOnLineCmdDTO.Contingencia = "0";
            oSignOnLineCmdDTO.parameter = "AQUI";
            #endregion


            try
            {


                switch (Formulario)
                {
                    case 140:
                        nombre_Store = "SMC_ODLN_GRE"; //ODLN
                        break;
                    case 940:
                        nombre_Store = "SMC_OWTR_GRE"; // OWTR
                        break;
                    case 720:
                        nombre_Store = "SMC_OIGE_GRE"; // OWTR
                        break;
                    case 182:
                        nombre_Store = "SMC_ORPD_GRE"; // OWTR
                        break;
                    case 143:
                        nombre_Store = "SMC_Entrada_Mercancia_GuiaRemision_Compra\""; //OPDN
                        break;
                    case 142:
                        nombre_Store = "SMC_Comprobante_GuiaRemision_OC"; //OPOR -- ODLN???
                        break;
                    case 180:
                        nombre_Store = "SMC_Comprobante_GuiaRemision_Venta_Devolucion"; //ORDN
                        break;
                 
                    case 65214:
                        nombre_Store = "SMC_Comprobante_GuiaReciboProduccion"; // OWTR
                        break;
                    case 65213:
                        nombre_Store = "SMC_Comprobante_GuiaEmisionProduccion"; // OWTR
                        break;
                    default:
                        nombre_Store = "SMC_Comprobante_GuiaRemision"; //ODLN
                        break;
                }


                string Query = "";
                int contadorresultado = 0;
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" (" + DocEntry + ")"; }
                else { Query = "EXEC \"" + nombre_Store + "\" " + DocEntry; }
                oRecGuia = oProcedures.RunQuery(Query);
                if (oRecGuia != null)
                {
                    if (oRecGuia.RecordCount > 0)
                    {


                        #region Cabecera Datos
                        oRecGuia.MoveFirst();
                        while (!oRecGuia.EoF)
                        {
                            oDocumento.correoAdquiriente = oRecGuia.Fields.Item("correoAdquiriente").Value.ToString();
                            oDocumento.correoEmisor = oRecGuia.Fields.Item("correoEmisor").Value.ToString();
                            oDocumento.serieNumeroGuia = oRecGuia.Fields.Item("serieNumeroGuia").Value.ToString();
                            oDocumento.fechaEmisionGuia = oRecGuia.Fields.Item("fechaEmisionGuia").Value.ToString();
                            oDocumento.horaEmisionGuia = oRecGuia.Fields.Item("horaEmisionGuia").Value.ToString();
                            oDocumento.tipoDocumentoGuia = oRecGuia.Fields.Item("tipoDocumentoGuia").Value.ToString();
                            oDocumento.observaciones = oRecGuia.Fields.Item("observaciones").Value.ToString();
                            oDocumento.numeroDocumentoRemitente = oRecGuia.Fields.Item("numeroDocumentoRemitente").Value.ToString();
                            oDocumento.tipoDocumentoRemitente = oRecGuia.Fields.Item("tipoDocumentoRemitente").Value.ToString();
                            oDocumento.razonSocialRemitente = oRecGuia.Fields.Item("razonSocialRemitente").Value.ToString();
                            oDocumento.numeroDocumentoDestinatario = oRecGuia.Fields.Item("numeroDocumentoDestinatario").Value.ToString();
                            oDocumento.tipoDocumentoDestinatario = oRecGuia.Fields.Item("tipoDocumentoDestinatario").Value.ToString();
                            oDocumento.razonSocialDestinatario = oRecGuia.Fields.Item("razonSocialDestinatario").Value.ToString();
                            oDocumento.motivoTraslado = oRecGuia.Fields.Item("motivoTraslado").Value.ToString();
                            oDocumento.pesoBrutoTotalBienes = Convert.ToInt32(oRecGuia.Fields.Item("pesoBrutoTotalBienes").Value.ToString());
                            oDocumento.unidadMedidaPesoBruto = (oRecGuia.Fields.Item("unidadMedidaPesoBruto").Value.ToString());
                            oDocumento.modalidadTraslado = oRecGuia.Fields.Item("modalidadTraslado").Value.ToString();
                            if (oDocumento.modalidadTraslado == "01")
                            {
                                oDocumento.fechaEntregaBienes = oRecGuia.Fields.Item("fechaEntregaBienes").Value.ToString();
                            }
                            else
                            {
                                oDocumento.fechaInicioTraslado = oRecGuia.Fields.Item("fechaInicioTraslado").Value.ToString();
                            }


                            oDocumento.ubigeoPtoPartida = oRecGuia.Fields.Item("ubigeoPtoPartida").Value.ToString();
                            oDocumento.direccionPtoPartida = oRecGuia.Fields.Item("direccionPtoPartida").Value.ToString();
                            oDocumento.ubigeoPtoLLegada = oRecGuia.Fields.Item("ubigeoPtoLLegada").Value.ToString();
                            oDocumento.direccionPtoLLegada = oRecGuia.Fields.Item("direccionPtoLLegada").Value.ToString();
                            oDocumento.tipoDocumentoConductor = oRecGuia.Fields.Item("tipoDocumentoConductor").Value.ToString();
                            oDocumento.numeroDocumentoConductor = oRecGuia.Fields.Item("numeroDocumentoConductor").Value.ToString();
                            oDocumento.nombreConductor = oRecGuia.Fields.Item("nombreConductor").Value.ToString();
                            oDocumento.apellidoConductor = oRecGuia.Fields.Item("apellidoConductor").Value.ToString();
                            oDocumento.numeroLicencia = oRecGuia.Fields.Item("numeroLicencia").Value.ToString();
                            oDocumento.numeroPlacaVehiculoPrin = oRecGuia.Fields.Item("numeroPlacaVehiculoPrin").Value.ToString();
                            oDocumento.modalidadTraslado = oRecGuia.Fields.Item("modalidadTraslado").Value.ToString();
                            oRecGuia.MoveNext();
                        }
                        #endregion

                        #region DETALLE Datos
                        oDocumento.GuiaItem = new GuiaItem[oRecGuia.RecordCount];

                        oRecGuia.MoveFirst();
                        while (!oRecGuia.EoF)
                        {

                            contadorresultado++;

                            oGuiaItem = new GuiaItem();
                            oGuiaItem.indicador = "D";
                            oGuiaItem.numeroOrdenItem = contadorresultado;
                            oGuiaItem.cantidad = Convert.ToInt32(oRecGuia.Fields.Item("cantidad").Value.ToString());
                            oGuiaItem.unidadMedida = oRecGuia.Fields.Item("unidadMedida").Value.ToString(); ;
                            oGuiaItem.descripcion = oRecGuia.Fields.Item("descripcion").Value.ToString(); ;
                            oGuiaItem.codigo = oRecGuia.Fields.Item("codigo").Value.ToString(); ;
                            oGuiaItem.codigoProductoSUNAT = oRecGuia.Fields.Item("codigoProductoSUNAT").Value.ToString(); ;
                            oDocumento.GuiaItem[(contadorresultado - 1)] = oGuiaItem;
                            oRecGuia.MoveNext();
                        }
                        #endregion


                    }
                }
            }
            catch (Exception exx)
            {

                mensaje = exx.Message;
            }
            finally
            {
                oProcedures.Release(oRecGuia);
                oRecGuia = null;
                GC.Collect();
            }
            oSignOnLineCmdDTO.documento = oDocumento;
            string parametros = $@"<parameter value=""{oDocumento.numeroDocumentoRemitente}"" name=""idEmisor""/>
                                    <parameter value=""09"" name=""tipoDocumento""/>";

            string xmlString = oFuncionesRequeridas.SerializeToXml(oSignOnLineCmdDTO);
            xmlString = xmlString.Replace("<parameter>AQUI</parameter>", parametros);
            xmlString = xmlString.Replace("<GuiaItem><GuiaItem>", "<GuiaItem>").Replace("</GuiaItem></GuiaItem>", "</GuiaItem>");
            return xmlString;
        }

        public bool AnexarDocumentos(int DocEntry, string nombreArchivo, string pdf_url, string xml_url, string cdr_url, int ObjType, SAPbobsCOM.Company oCompany, ref string mensaje)
        {
            FuncionesRequeridas oFuncionesRequeridas = new FuncionesRequeridas();

            #region PDF
            string mensajeerrorpdf = "";
            string rutaArchivoPdf = ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString() + "\\" + nombreArchivo + ".pdf";
            if (!string.IsNullOrEmpty(pdf_url))
            {
                if (!File.Exists(rutaArchivoPdf))
                    oFuncionesRequeridas.downloadFileToSpecificPath(pdf_url, rutaArchivoPdf);
                else
                {
                    File.Delete(rutaArchivoPdf);
                    oFuncionesRequeridas.downloadFileToSpecificPath(pdf_url, rutaArchivoPdf);
                }
            }
            SetAnexoSAP(nombreArchivo + ".pdf", ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString(), DocEntry, oCompany, ObjType, ref mensajeerrorpdf);
            #endregion

            #region XML
            string rutaArchivoXml = ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString() + "\\" + nombreArchivo + ".xml";
            if (!string.IsNullOrEmpty(xml_url))
            {
                if (!File.Exists(rutaArchivoXml))
                    oFuncionesRequeridas.downloadFileToSpecificPath(xml_url, rutaArchivoXml);
                else
                {
                    File.Delete(rutaArchivoXml);
                    oFuncionesRequeridas.downloadFileToSpecificPath(xml_url, rutaArchivoXml);
                }
            }
            SetAnexoSAP(nombreArchivo + ".xml", ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString(), DocEntry, oCompany, ObjType, ref mensajeerrorpdf);
            #endregion


            #region CDR
            string rutaArchivoCdr = ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString() + "\\" + nombreArchivo + ".xml";
            if (!string.IsNullOrEmpty(cdr_url))
            {
                if (!File.Exists(rutaArchivoCdr))
                    oFuncionesRequeridas.downloadFileToSpecificPath(cdr_url, rutaArchivoCdr);
                else
                {
                    File.Delete(rutaArchivoCdr);
                    oFuncionesRequeridas.downloadFileToSpecificPath(cdr_url, rutaArchivoCdr);
                }
            }
            #endregion


            return true;
        }


        #region Anexo SAP
        public bool SetAnexoSAP(string nombrearchivo, string rutaarchivo, int docentry, SAPbobsCOM.Company oCompany, int ObjectType, ref string mensajeErrorInterno)
        {
            int existearchivoanexado = 0;
            int numeroexistencia = 0;
            try
            {
                string exportPathForSAP = rutaarchivo;
                exportPathForSAP = exportPathForSAP.Replace("\\\\\\\\", "\\\\");
                exportPathForSAP = exportPathForSAP.Replace("XML\\", "XML");

                dynamic oinvoice = null;
                if (ObjectType==67)
                {
            
                    oinvoice = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject((SAPbobsCOM.BoObjectTypes)Convert.ToInt32(ObjectType));
                }
                else
                {
    
                    oinvoice = (SAPbobsCOM.Documents)oCompany.GetBusinessObject((SAPbobsCOM.BoObjectTypes)Convert.ToInt32(ObjectType));
                }
               
                //SAPbobsCOM.Documents oinvoice = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                oinvoice.GetByKey(Convert.ToInt32(docentry));

                SAPbobsCOM.Attachments2 oAttachment = (SAPbobsCOM.Attachments2)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                //SAPbobsCOM.Attachments2_Lines oAttachmentLines = (SAPbobsCOM.Attachments2_Lines)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);

                SAPbobsCOM.CompanyService comServ = oCompany.GetCompanyService();
                SAPbobsCOM.PathAdmin pthAdm = comServ.GetPathAdmin();
                string path = pthAdm.AttachmentsFolderPath;
                if (path.Substring(path.Length - 1, 1).Equals(@"\"))
                    path = path.Substring(0, path.Length - 1);

                //string errorMensaje = "";

                if (oAttachment.GetByKey(oinvoice.AttachmentEntry))
                // if (oAttachment.GetByKey(Convert.ToInt32(docentry)))
                {
                    int s223232adasdasdasdas = oAttachment.Lines.Count;

                    //
                    for (int i = 0; i < oAttachment.Lines.Count; i++)
                    {

                        oAttachment.Lines.SetCurrentLine(i);
                        if (oAttachment.Lines.FileName == nombrearchivo)
                        {
                            existearchivoanexado = 1;
                            numeroexistencia = i;
                        }

                        oAttachment.Lines.SourcePath = path;
                    }
                    oAttachment.Lines.Add();
                    int sadasdasdasdas = oAttachment.Lines.Count;
                    oAttachment.Lines.SetCurrentLine(oAttachment.Lines.Count - 1);
                    if (existearchivoanexado == 1)
                    {
                        oAttachment.Lines.SetCurrentLine(numeroexistencia);
                    }

                    oAttachment.Lines.FileName = nombrearchivo;
                    oAttachment.Lines.SourcePath = exportPathForSAP;
                    oAttachment.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

                    oAttachment.Lines.FileName = oAttachment.Lines.FileName;
                    if (oAttachment.Update() != 0)
                    {
                        mensajeErrorInterno = oCompany.GetLastErrorDescription();

                        if (mensajeErrorInterno.Length > 0)
                        {
                            oAttachment.Lines.SetCurrentLine(oAttachment.Lines.Count - 1);
                            //oAttachment.Lines.SourcePath = exportPathForSAP;
                            oAttachment.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                            oAttachment.Lines.FileName = oAttachment.Lines.FileName;
                            oAttachment.Lines.SourcePath = exportPathForSAP.Replace("WIN-SOPORTESAP", "192.168.1.9");

                            if (oAttachment.Update() != 0)
                            {
                                mensajeErrorInterno = oCompany.GetLastErrorDescription();
                            }
                        }
                    }
                }
                else
                {
                    oAttachment.Lines.FileName = nombrearchivo;
                    oAttachment.Lines.SourcePath = exportPathForSAP;
                    oAttachment.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                    if (oAttachment.Add() != 0)
                    {
                        mensajeErrorInterno = oCompany.GetLastErrorDescription();
                    }

                    string objKey = oCompany.GetNewObjectKey();
                    //  objKey = docentry;
                    oAttachment.GetByKey(Convert.ToInt32(objKey));
                    int absEntry = oAttachment.AbsoluteEntry;
                    oinvoice.AttachmentEntry = oAttachment.AbsoluteEntry;

                    if (oinvoice.Update() != 0)
                    {
                        mensajeErrorInterno = oCompany.GetLastErrorDescription();
                    }
                }


            }
            catch (Exception ex)
            {
                mensajeErrorInterno = ex.Message;
                //Program.SboAplicacion.SetStatusBarMessage(ex.Message + " " + nombrearchivo, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            if (mensajeErrorInterno.Length > 1)
            {
                return false;
            }

            return true;
        }
        #endregion



        public string ObtenerStringConsultarGuia(int DocEntry,int Formulario,int ObjType, SAPbobsCOM.Company oCompany,ref string mensaje)
        {
            Documento oDocumento = new Documento();

            GuiaItem oGuiaItem = new GuiaItem();
            string nombre_Store = "";
            SAPbobsCOM.Recordset oRecGuia = default(SAPbobsCOM.Recordset);
            Procedures oProcedures = new Procedures(oCompany);
            FuncionesRequeridas oFuncionesRequeridas = new FuncionesRequeridas();



            string idEmisor = "";
            string tipoDocumento = "";
            string serieGrupoDocumento = "";
            string numeroCorrelativoInicio = "";
            string numeroCorrelativoFin = "";
            ConsultCmdDto oConsultCmdDto = new ConsultCmdDto();
            oConsultCmdDto.Output = "PDF";



            try
            {
                switch (Formulario)
                {
                    case 140:
                        nombre_Store = "SMC_ODLN_GRE"; //ODLN
                        break;
                    case 940:
                        nombre_Store = "SMC_OWTR_GRE"; // OWTR
                        break;
                    case 720:
                        nombre_Store = "SMC_OIGE_GRE"; // OWTR
                        break;
                    case 182:
                        nombre_Store = "SMC_ORPD_GRE"; //ORPD
                        break;
                    case 143:
                        nombre_Store = "SMC_Entrada_Mercancia_GuiaRemision_Compra\""; //OPDN
                        break;
                    case 142:
                        nombre_Store = "SMC_Comprobante_GuiaRemision_OC"; //OPOR -- ODLN???
                        break;
                    case 180:
                        nombre_Store = "SMC_Comprobante_GuiaRemision_Venta_Devolucion"; //ORDN
                        break;
                    
                   
                    case 65214:
                        nombre_Store = "SMC_Comprobante_GuiaReciboProduccion"; // OWTR
                        break;
                    case 65213:
                        nombre_Store = "SMC_Comprobante_GuiaEmisionProduccion"; // OWTR
                        break;
                    default:
                        nombre_Store = "SMC_Comprobante_GuiaRemision"; //ODLN
                        break;
                }


                string Query = "";
                int contadorresultado = 0;
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" (" + DocEntry + ")"; }
                else { Query = "EXEC \"" + nombre_Store + "\" " + DocEntry; }
                oRecGuia = oProcedures.RunQuery(Query);
                if (oRecGuia != null)
                {
                    if (oRecGuia.RecordCount > 0)
                    {


                        #region Cabecera Datos
                        oRecGuia.MoveFirst();
                        while (!oRecGuia.EoF)
                        {
                            idEmisor = oRecGuia.Fields.Item("idEmisor").Value.ToString();
                            tipoDocumento = oRecGuia.Fields.Item("tipoDocumento").Value.ToString();
                            serieGrupoDocumento = oRecGuia.Fields.Item("Serie").Value.ToString();
                            numeroCorrelativoInicio = oRecGuia.Fields.Item("Correlativo").Value.ToString();
                            numeroCorrelativoFin = oRecGuia.Fields.Item("Correlativo").Value.ToString();
                           
                            oRecGuia.MoveNext();
                        }
                        #endregion

                    


                    }
                }
            }
            catch (Exception exx)
            {

                mensaje = exx.Message;
            }
            finally
            {
                oProcedures.Release(oRecGuia);
                oRecGuia = null;
                GC.Collect();
            }

            // Crear un objeto ConsultCmdDto
            var consultCmd = new ConsultCmdDto
            {
                Output = "PDF",
                Parametros = new Parametros
                {
                    Parameters = new[]
                    {
                    new Parameter { Name = "idEmisor", Value = idEmisor },
                    new Parameter { Name = "tipoDocumento", Value = tipoDocumento  },
                    new Parameter { Name = "serieGrupoDocumento", Value = serieGrupoDocumento},
                    new Parameter { Name = "numeroCorrelativoInicio", Value = numeroCorrelativoInicio.PadLeft(8,'0') },
                    new Parameter { Name = "numeroCorrelativoFin", Value =numeroCorrelativoFin.PadLeft(8,'0')  }
                }
                }
            };
            string xmlString = oFuncionesRequeridas.SerializeToXml(consultCmd);
            xmlString = xmlString.Replace("</parametros>", "");
            xmlString = xmlString.Replace( "<parametros>", "<parametros/>");
            return xmlString;
        }

    }
}
