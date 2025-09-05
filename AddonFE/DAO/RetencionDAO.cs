using AddonFE.Configuraciones;
using AddonFE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddonFE.DAO
{
    public class RetencionDAO
    {

        #region Obener Datos del CPE
        public string getGeneral21(int DocEntry)
        {
            string xmlString = "";
            FuncionesRequeridas oFuncionesRequeridas = new FuncionesRequeridas();
            string nombre_Store = "SMC_RETENCION_FE";
            string nombre_Store1 = "SMC_RETENCION_DETALLE_FE";


            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            SAPbobsCOM.Recordset oRecFE1 = default(SAPbobsCOM.Recordset);
            SAPbobsCOM.Company oCompanyDAO;

            oCompanyDAO = (SAPbobsCOM.Company)Program.SboAplicacion.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            string Query = "";


            DocumentoRetencionFE oDocumentoFE = new DocumentoRetencionFE();
            #region  DATOS CABECERA
            SignOnLineRetentionCmd oSignOnLineCmd = new SignOnLineRetentionCmd();
            oSignOnLineCmd.DeclareSunat = "0";
            oSignOnLineCmd.DeclareDirectSunat = "1";
            oSignOnLineCmd.Publish = "1";
            oSignOnLineCmd.Output = "PDF";
            oSignOnLineCmd.parameter = "AQUI";
            #endregion



            try
            {

                #region SMC_Comprobante_21
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" (" + DocEntry + ")"; }
                else { Query = "EXEC \"" + nombre_Store + "\" " + DocEntry; }
                oRecFE = oProcedures.RunQuery(Query);
                if (oRecFE != null)
                {
                    if (oRecFE.RecordCount > 0)
                    {
                        oRecFE.MoveFirst();
                        while (!oRecFE.EoF)
                        {
                            oDocumentoFE.correoEmisor = oRecFE.Fields.Item("correoEmisor").Value.ToString();
                            oDocumentoFE.correoAdquiriente = oRecFE.Fields.Item("correoAdquiriente").Value.ToString();
                            oDocumentoFE.serieNumeroRetencion = oRecFE.Fields.Item("serieNumeroRetencion").Value.ToString();
                            oDocumentoFE.fechaEmision = oRecFE.Fields.Item("fechaEmision").Value.ToString();
                            oDocumentoFE.tipoDocumento = oRecFE.Fields.Item("tipoDocumento").Value.ToString();
                            oDocumentoFE.numeroDocumentoEmisor = oRecFE.Fields.Item("numeroDocumentoEmisor").Value.ToString();
                            oDocumentoFE.tipoDocumentoEmisor = oRecFE.Fields.Item("tipoDocumentoEmisor").Value.ToString();
                            oDocumentoFE.razonSocialEmisor = oRecFE.Fields.Item("razonSocialEmisor").Value.ToString();
                            oDocumentoFE.numeroDocumentoEmisor = oRecFE.Fields.Item("numeroDocumentoEmisor").Value.ToString();
                            oDocumentoFE.nombreComercialEmisor = (oRecFE.Fields.Item("nombreComercialEmisor").Value.ToString());
                            oDocumentoFE.ubigeoEmisor = oRecFE.Fields.Item("ubigeoEmisor").Value.ToString();
                            oDocumentoFE.direccionEmisor = oRecFE.Fields.Item("direccionEmisor").Value.ToString();
                            oDocumentoFE.urbanizacionEmisor = oRecFE.Fields.Item("urbanizacionEmisor").Value.ToString();
                            oDocumentoFE.departamentoEmisor = oRecFE.Fields.Item("departamentoEmisor").Value.ToString();
                            oDocumentoFE.provinciaEmisor = oRecFE.Fields.Item("provinciaEmisor").Value.ToString();
                            oDocumentoFE.distritoEmisor = oRecFE.Fields.Item("distritoEmisor").Value.ToString();
                            oDocumentoFE.codigoPaisEmisor = oRecFE.Fields.Item("codigoPaisEmisor").Value.ToString();
                            oDocumentoFE.razonSocialProveedor = oRecFE.Fields.Item("razonSocialProveedor").Value.ToString();
                            oDocumentoFE.numeroDocumentoProveedor = oRecFE.Fields.Item("numeroDocumentoProveedor").Value.ToString();
                            oDocumentoFE.tipoDocumentoProveedor = oRecFE.Fields.Item("tipoDocumentoProveedor").Value.ToString();
                            oDocumentoFE.nombreComercialProveedor = (oRecFE.Fields.Item("nombreComercialProveedor").Value.ToString());
                            oDocumentoFE.direccionProveedor = oRecFE.Fields.Item("direccionProveedor").Value.ToString();
                            oDocumentoFE.urbanizacionProveedor = oRecFE.Fields.Item("urbanizacionProveedor").Value.ToString();
                            oDocumentoFE.provinciaProveedor = oRecFE.Fields.Item("provinciaProveedor").Value.ToString();
                            oDocumentoFE.departamentoProveedor = oRecFE.Fields.Item("departamentoProveedor").Value.ToString();
                            oDocumentoFE.distritoProveedor = oRecFE.Fields.Item("distritoProveedor").Value.ToString();
                            oDocumentoFE.codigoPaisProveedor = oRecFE.Fields.Item("codigoPaisProveedor").Value.ToString();
                            oDocumentoFE.ubigeoProveedor = oRecFE.Fields.Item("ubigeoProveedor").Value.ToString();
                            oDocumentoFE.regimenRetencion = oRecFE.Fields.Item("regimenRetencion").Value.ToString();

                            oDocumentoFE.tasaRetencion = (oRecFE.Fields.Item("tasaRetencion").Value.ToString());
                            oDocumentoFE.observaciones = (oRecFE.Fields.Item("observaciones").Value.ToString());
                            oDocumentoFE.importeTotalRetenido = (oRecFE.Fields.Item("importeTotalRetenido").Value.ToString());
                            oDocumentoFE.tipoMonedaTotalRetenido = (oRecFE.Fields.Item("tipoMonedaTotalRetenido").Value.ToString());

                            oDocumentoFE.importeTotalPagado = oRecFE.Fields.Item("importeTotalPagado").Value.ToString();
                            oDocumentoFE.tipoMonedaTotalPagado = oRecFE.Fields.Item("tipoMonedaTotalPagado").Value.ToString();



                            oRecFE.MoveNext();
                        }
                    }
                }
                #endregion

                #region SMC_COMPROBANTE_DETALLE_FE
                oDocumentoFE.RetencionItems = new List<RetencionItem>();
                RetencionItem oItem = new RetencionItem();
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store1 + "\" (" + DocEntry + ")"; }
                else { Query = "EXEC \"" + nombre_Store1 + "\" " + DocEntry; }
                oRecFE = oProcedures.RunQuery(Query);
                if (oRecFE != null)
                {
                    if (oRecFE.RecordCount > 0)
                    {
                        int contadorItem = 1;
                        oRecFE.MoveFirst();
                        while (!oRecFE.EoF)
                        {
                            oItem = new RetencionItem();
                            oItem.numeroOrdenItem = contadorItem;
                            oItem.numeroDocumentoRelacionado = oRecFE.Fields.Item("numeroDocumentoRelacionado").Value.ToString();
                            oItem.fechaEmisionDocumentoRelacionado = (oRecFE.Fields.Item("fechaEmisionDocumentoRelacionado").Value.ToString());
                            oItem.tipoDocumentoRelacionado = oRecFE.Fields.Item("tipoDocumentoRelacionado").Value.ToString();
                            oItem.importeTotalDocumentoRelacionado = oRecFE.Fields.Item("importeTotalDocumentoRelacionado").Value.ToString();
                            oItem.tipoMonedaDocumentoRelacionado = oRecFE.Fields.Item("tipoMonedaDocumentoRelacionado").Value.ToString();
                            oItem.fechaPago = (oRecFE.Fields.Item("fechaPago").Value.ToString());
                            oItem.numeroPago = (oRecFE.Fields.Item("numeroPago").Value.ToString());
                            oItem.importePagoSinRetencion = oRecFE.Fields.Item("importePagoSinRetencion").Value.ToString();
                            oItem.importeRetenido = (oRecFE.Fields.Item("importeRetenido").Value.ToString());
                            oItem.monedaImporteRetenido = (oRecFE.Fields.Item("monedaImporteRetenido").Value.ToString());
                            oItem.monedaPago = (oRecFE.Fields.Item("monedaPago").Value.ToString());

                            oItem.fechaRetencion = (oRecFE.Fields.Item("fechaRetencion").Value.ToString());
                            oItem.importeTotalPagarNeto = (oRecFE.Fields.Item("importeTotalPagarNeto").Value.ToString());
                            oItem.monedaMontoNetoPagado = (oRecFE.Fields.Item("monedaMontoNetoPagado").Value.ToString());


                            if (oRecFE.Fields.Item("monedaPago").Value.ToString() != "PEN")
                            {
                                oItem.monedaReferenciaTipoCambio = (oRecFE.Fields.Item("monedaReferenciaTipoCambio").Value.ToString());
                                oItem.monedaObjetivoTasaCambio = (oRecFE.Fields.Item("monedaObjetivoTasaCambio").Value.ToString());
                                oItem.factorTipoCambioMoneda = (oRecFE.Fields.Item("factorTipoCambioMoneda").Value.ToString());
                                oItem.fechaCambio = (oRecFE.Fields.Item("fechaCambio").Value.ToString());
                            }


                            contadorItem++;
                            oDocumentoFE.RetencionItems.Add(oItem);
                            oRecFE.MoveNext();
                        }
                    }
                }
                #endregion


                #region Calculando Importes Totales
                decimal SumimporteTotalPagado = 0;
                decimal SumimporteTotalRetenido = 0;
                foreach (var item in oDocumentoFE.RetencionItems)
                {
                    SumimporteTotalPagado += Convert.ToDecimal(item.importeTotalPagarNeto);
                    SumimporteTotalRetenido += Convert.ToDecimal(item.importeRetenido);
                    //oDocumentoFE.importeTotalPagado += Convert.ToDecimal(item.importeTotalPagarNeto);
                    //oDocumentoFE.importeTotalRetenido += Convert.ToDecimal(item.importeRetenido);
                }
                oDocumentoFE.importeTotalPagado = SumimporteTotalPagado.ToString();
                oDocumentoFE.importeTotalRetenido = SumimporteTotalRetenido.ToString();
                #endregion

                oSignOnLineCmd.documento = oDocumentoFE;
                string parametros = $@"<parameter value=""{oDocumentoFE.numeroDocumentoEmisor}"" name=""idEmisor""/>
                                    <parameter value=""{oDocumentoFE.tipoDocumento}"" name=""tipoDocumento""/>";

                xmlString = oFuncionesRequeridas.SerializeToXml(oSignOnLineCmd);
                xmlString = xmlString.Replace("<parameter>AQUI</parameter>", parametros);
                xmlString = xmlString.Replace("<GuiaItem><GuiaItem>", "<GuiaItem>").Replace("</GuiaItem></GuiaItem>", "</GuiaItem>");


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                oProcedures.Release(oRecFE);
                oRecFE = null;
                GC.Collect();
            }




            return xmlString;
        }
        #endregion

        #region Consultar CPE
        public string ObtenerStringCPE(int DocEntry, int Formulario, int ObjType, SAPbobsCOM.Company oCompany, ref string mensaje)
        {

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
            nombre_Store = "SMC_RETENCION_FE"; //RETENCION


            try
            {



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
            xmlString = xmlString.Replace("<parametros>", "<parametros/>");
            return xmlString;
        }
        #endregion



        #region Resumen Reversion Retencion
        public string ResumenAnulacion(int DocEntry, int Formulario, int ObjType, SAPbobsCOM.Company oCompany, ref string mensaje)
        {
            string xmlString = "";
            string nombre_Store = "";
            SAPbobsCOM.Recordset oRec = default(SAPbobsCOM.Recordset);
            Procedures oProcedures = new Procedures(oCompany);
            FuncionesRequeridas oFuncionesRequeridas = new FuncionesRequeridas();



            string idEmisor = "";
            string tipoDocumento = "";
            string serieGrupoDocumento = "";
            string numeroCorrelativoInicio = "";
            string numeroCorrelativoFin = "";
            ResumenAnulacionDTO oResumenAnulacionDTO = new ResumenAnulacionDTO();
            oResumenAnulacionDTO.documento = new DocumentoAnulacion();
            DocumentoAnulacion odocumento = new DocumentoAnulacion();
            odocumento.resumenItem = new ResumenItem();

            try
            {
                switch (ObjType)
                {
                    case 46:
                        nombre_Store = "SMC_ANULACION_OVPM_FE"; //OVPM
                        break;
                    //case 143:
                    //    nombre_Store = "SMC_Entrada_Mercancia_GuiaRemision_Compra\""; //OPDN
                    //    break;

                    default:
                        nombre_Store = "SMC_ANULACION_OVPM_FE"; //OVPM
                        break;
                }


                string Query = "";
                int contadorresultado = 0;
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\"(" + DocEntry + ")"; }
                else { Query = "EXEC \"" + nombre_Store + "\" " + DocEntry; }
                oRec = oProcedures.RunQuery(Query);
                if (oRec != null)
                {
                    if (oRec.RecordCount > 0)
                    {


                        #region Cabecera Datos
                        oRec.MoveFirst();
                        while (!oRec.EoF)
                        {
                            oResumenAnulacionDTO.parameter = "AQUI";
                            odocumento.numeroDocumentoEmisor = oRec.Fields.Item("numeroDocumentoEmisor").Value.ToString();
                            odocumento.version = oRec.Fields.Item("version").Value.ToString();
                            odocumento.versionUBL = oRec.Fields.Item("versionUBL").Value.ToString();
                            odocumento.tipoDocumentoEmisor = oRec.Fields.Item("tipoDocumentoEmisor").Value.ToString();
                            odocumento.resumenId = oRec.Fields.Item("resumenId").Value.ToString();
                            odocumento.fechaEmisionComprobante = oRec.Fields.Item("fechaEmisionComprobante").Value.ToString();
                            odocumento.fechaGeneracionResumen = oRec.Fields.Item("fechaGeneracionResumen").Value.ToString();
                            odocumento.razonSocialEmisor = oRec.Fields.Item("razonSocialEmisor").Value.ToString();
                            odocumento.correoEmisor = oRec.Fields.Item("correoEmisor").Value.ToString();
                            odocumento.inHabilitado = Convert.ToInt32(oRec.Fields.Item("inHabilitado").Value.ToString());
                            odocumento.resumenTipo = oRec.Fields.Item("resumenTipo").Value.ToString();
                            odocumento.numeroDocumentoEmisor = oRec.Fields.Item("numeroDocumentoEmisor").Value.ToString();
                            odocumento.numeroDocumentoEmisor = oRec.Fields.Item("numeroDocumentoEmisor").Value.ToString();
                            odocumento.resumenItem.numeroFila = Convert.ToInt32(oRec.Fields.Item("numeroFila").Value.ToString());
                            odocumento.resumenItem.tipoDocumento = oRec.Fields.Item("tipoDocumento").Value.ToString();
                            odocumento.resumenItem.serieDocumentoBaja = oRec.Fields.Item("serieDocumentoBaja").Value.ToString();
                            odocumento.resumenItem.numeroDocumentoBaja = oRec.Fields.Item("resumenTipo").Value.ToString();
                            odocumento.resumenItem.motivoBaja = oRec.Fields.Item("motivoBaja").Value.ToString();

                            oRec.MoveNext();
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
                oProcedures.Release(oRec);
                oRec = null;
                GC.Collect();
            }

            oResumenAnulacionDTO.documento = odocumento;
            string parametros = $@"<parameter value=""{odocumento.numeroDocumentoEmisor}"" name=""idEmisor""/>
                                    <parameter value=""RA"" name=""tipoDocumento""/>";

            xmlString = oFuncionesRequeridas.SerializeToXml(oResumenAnulacionDTO);
            xmlString = xmlString.Replace("<parameter>AQUI</parameter>", parametros);
            xmlString = xmlString.Replace("<GuiaItem><GuiaItem>", "<GuiaItem>").Replace("</GuiaItem></GuiaItem>", "</GuiaItem>");
            return xmlString;
        }
        #endregion


    }
}
