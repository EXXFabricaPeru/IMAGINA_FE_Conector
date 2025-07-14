using AddonFE.Configuraciones;
using AddonFE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddonFE.DAO
{
    public class NotaCreditoDAO
    {
        #region Validar Existencia Folio ORIN
        public bool ValidarExistenciaFolioORIN(int DocEntry,string TipoDoc, int DocNum, int ObjectType, SAPbobsCOM.Company oCompanyDAO)
        {
            bool ExistenciaFolio = false;
            string nombre_Store = "";
            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            //SAPbobsCOM.Company oCompanyDAO;
            //oCompanyDAO = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            nombre_Store = "SMC_Validar_FELFolioORIN";
            string Query = "";
            String FolioNum = "";
            try
            {
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" ('" + DocEntry + "','" + ObjectType+ "','" + TipoDoc + "')"; }
                else { Query = "EXEC \"" + nombre_Store + "\" '" + DocEntry + "','" + ObjectType + "','" + TipoDoc + "'"; }
                oRecFE = oProcedures.RunQuery(Query);
                if (oRecFE != null)
                {
                    if (oRecFE.RecordCount > 0)
                    {
                        oRecFE.MoveFirst();
                        while (!oRecFE.EoF)
                        {
                            FolioNum = oRecFE.Fields.Item("FolioNum").Value.ToString();
                            oRecFE.MoveNext();
                        }

                    }
                }
                if (FolioNum != "0")
                {
                    ExistenciaFolio = true;
                }
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

            return ExistenciaFolio;
        }

        public void Update_Folio_NotaCredito(string TipoDoc, int DocNum, int ObjectType,int DocEntry, ref string FolioPref, ref string FolioNum, SAPbobsCOM.Company oCompanyDAO)
        {

            string nombre_Store = "SMC_Actualizar_FELFolioPrefNumORIN";
            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            //SAPbobsCOM.Company oCompanyDAO;
            //oCompanyDAO = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            string Query = "";
            try
            {
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" ('" + DocEntry+ "','" + ObjectType + "','" + TipoDoc + "')"; }
                else { Query = "EXEC \"" + nombre_Store + "\" '" + DocEntry + "','" + ObjectType + "','" + TipoDoc + "'"; }
                oRecFE = oProcedures.RunQuery(Query);
                if (oRecFE != null)
                {
                    if (oRecFE.RecordCount > 0)
                    {
                        oRecFE.MoveFirst();
                        while (!oRecFE.EoF)
                        {
                            //DocEntry = oRecFE.Fields.Item("DocEntry").Value.ToString();
                            FolioPref = oRecFE.Fields.Item("FolioPref").Value.ToString();
                            FolioNum = oRecFE.Fields.Item("FolioNum").Value.ToString();
                            oRecFE.MoveNext();
                        }
                    }
                }

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




        }

        #endregion


        #region Obener Datos del CPE
        public string getGeneral21(int DocEntry)
        {
            string xmlString = "";
            FuncionesRequeridas oFuncionesRequeridas = new FuncionesRequeridas();
            string nombre_Store = "SMC_COMPROBANTENC_FE";
            string nombre_Store1 = "SMC_COMPROBANTENC_DETALLE_FE";
            /*
            string nombre_Store2 = "SMC_Comprobante_21";
            string nombre_Store3 = "SMC_MontosAnticipos_21";
            string nombre_Store4 = "SMC_Comprobante_NotaDebito_21";
            string nombre_Store5 = "SMC_FE_Retencion_FND";
            string nombre_Store6 = "SMC_FE_FormaPago_FND";
            string nombre_Store7 = "SMC_Detalle_Exportacion_21";
            */

            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            SAPbobsCOM.Recordset oRecFE1 = default(SAPbobsCOM.Recordset);
            SAPbobsCOM.Company oCompanyDAO;

            oCompanyDAO = (SAPbobsCOM.Company)Program.SboAplicacion.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            string Query = "";


            DocumentoFE oDocumentoFE = new DocumentoFE();
            #region  DATOS CABECERA
            SignOnLineCmd oSignOnLineCmd = new SignOnLineCmd();
            oSignOnLineCmd.DeclareSunat = "1";
            oSignOnLineCmd.DeclareDirectSunat = "0";
            oSignOnLineCmd.Publish = "1";
            oSignOnLineCmd.Output = "PDF";
            oSignOnLineCmd.Contingencia = "0";
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
                            oDocumentoFE.serieNumero = oRecFE.Fields.Item("serieNumero").Value.ToString();
                            oDocumentoFE.fechaEmision = oRecFE.Fields.Item("fechaEmision").Value.ToString();
                            oDocumentoFE.tipoDocumento = oRecFE.Fields.Item("tipoDocumento").Value.ToString();
                            oDocumentoFE.tipoMoneda = oRecFE.Fields.Item("tipoMoneda").Value.ToString();
                            oDocumentoFE.numeroDocumentoEmisor = oRecFE.Fields.Item("numeroDocumentoEmisor").Value.ToString();
                            oDocumentoFE.tipoDocumentoEmisor = Convert.ToInt32(oRecFE.Fields.Item("tipoDocumentoEmisor").Value.ToString());
                            oDocumentoFE.nombreComercialEmisor = oRecFE.Fields.Item("nombreComercialEmisor").Value.ToString();
                            oDocumentoFE.razonSocialEmisor = oRecFE.Fields.Item("razonSocialEmisor").Value.ToString();
                            oDocumentoFE.direccionEmisor = oRecFE.Fields.Item("direccionEmisor").Value.ToString();
                            oDocumentoFE.provinciaEmisor = oRecFE.Fields.Item("provinciaEmisor").Value.ToString();
                            oDocumentoFE.ubigeoEmisor = oRecFE.Fields.Item("ubigeoEmisor").Value.ToString();
                            oDocumentoFE.departamentoEmisor = oRecFE.Fields.Item("departamentoEmisor").Value.ToString();
                            oDocumentoFE.distritoEmisor = oRecFE.Fields.Item("distritoEmisor").Value.ToString();
                            oDocumentoFE.paisEmisor = oRecFE.Fields.Item("paisEmisor").Value.ToString();
                            oDocumentoFE.codigoLocalAnexoEmisor = oRecFE.Fields.Item("codigoLocalAnexoEmisor").Value.ToString();
                            oDocumentoFE.numeroDocumentoAdquiriente = oRecFE.Fields.Item("numeroDocumentoAdquiriente").Value.ToString();
                            oDocumentoFE.tipoDocumentoAdquiriente = Convert.ToInt32(oRecFE.Fields.Item("tipoDocumentoAdquiriente").Value.ToString());
                            oDocumentoFE.razonSocialAdquiriente = oRecFE.Fields.Item("razonSocialAdquiriente").Value.ToString();
                            oDocumentoFE.direccionAdquiriente = oRecFE.Fields.Item("direccionAdquiriente").Value.ToString();
                            oDocumentoFE.urbanizacionAdquiriente = oRecFE.Fields.Item("urbanizacionAdquiriente").Value.ToString();
                            oDocumentoFE.provinciaAdquiriente = oRecFE.Fields.Item("provinciaAdquiriente").Value.ToString();
                            oDocumentoFE.ubigeoAdquiriente = oRecFE.Fields.Item("ubigeoAdquiriente").Value.ToString();
                            oDocumentoFE.departamentoAdquiriente = oRecFE.Fields.Item("departamentoAdquiriente").Value.ToString();
                            oDocumentoFE.distritoAdquiriente = oRecFE.Fields.Item("distritoAdquiriente").Value.ToString();
                            oDocumentoFE.paisAdquiriente = oRecFE.Fields.Item("paisAdquiriente").Value.ToString();

                            oDocumentoFE.totalValorVentaNetoOpGravadas = Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpGravadas").Value.ToString());
                            oDocumentoFE.totalValorVentaNetoOpNoGravada = Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpNoGravada").Value.ToString());
                            oDocumentoFE.totalIgv = Convert.ToDecimal(oRecFE.Fields.Item("totalIgv").Value.ToString());
                            oDocumentoFE.totalVenta = Convert.ToDecimal(oRecFE.Fields.Item("totalVenta").Value.ToString());
                            oDocumentoFE.totalImpuestos = Convert.ToDecimal(oRecFE.Fields.Item("totalImpuestos").Value.ToString());

                            oDocumentoFE.codigoSerieNumeroAfectado = oRecFE.Fields.Item("codigoSerieNumeroAfectado").Value.ToString();
                            oDocumentoFE.motivoDocumento = oRecFE.Fields.Item("motivoDocumento").Value.ToString();
                            oDocumentoFE.tipoDocumentoReferenciaPrincipal = oRecFE.Fields.Item("tipoDocumentoReferenciaPrincipal").Value.ToString();
                            oDocumentoFE.numeroDocumentoReferenciaPrincipal = oRecFE.Fields.Item("numeroDocumentoReferenciaPrincipal").Value.ToString();

                            oDocumentoFE.codigoLeyenda_1 = oRecFE.Fields.Item("codigoLeyenda_1").Value.ToString();
                            oDocumentoFE.textoLeyenda_1 = oRecFE.Fields.Item("textoLeyenda_1").Value.ToString();



                            if (oRecFE.Fields.Item("ordenCompra").Value.ToString().Length > 0)
                            {
                                oDocumentoFE.ordenCompra = oRecFE.Fields.Item("ordenCompra").Value.ToString();
                            }

                            if (oRecFE.Fields.Item("numeroDocumentoReferencia_1").Value.ToString().Length > 0)
                            {
                                oDocumentoFE.tipoReferencia_1 = oRecFE.Fields.Item("tipoReferencia_1").Value.ToString();
                                oDocumentoFE.numeroDocumentoReferencia_1 = oRecFE.Fields.Item("numeroDocumentoReferencia_1").Value.ToString();
                            }
                            oRecFE.MoveNext();
                        }
                    }
                }
                #endregion

                #region SMC_COMPROBANTE_DETALLE_FE
                oDocumentoFE.Items = new List<Item>();
                Item oItem = new Item();
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
                            oItem = new Item();
                            oItem.numeroOrdenItem = contadorItem;
                            oItem.unidadMedida = oRecFE.Fields.Item("unidadMedida").Value.ToString();
                            oItem.cantidad = Convert.ToDecimal(oRecFE.Fields.Item("cantidad").Value.ToString());
                            oItem.codigoProducto = oRecFE.Fields.Item("codigoProducto").Value.ToString();
                            oItem.codigoProductoSUNAT = oRecFE.Fields.Item("codigoProductoSUNAT").Value.ToString();
                            oItem.descripcion = oRecFE.Fields.Item("descripcion").Value.ToString();
                            oItem.importeUnitarioSinImpuesto = Convert.ToDecimal(oRecFE.Fields.Item("importeUnitarioSinImpuesto").Value.ToString());
                            oItem.importeUnitarioConImpuesto = Convert.ToDecimal(oRecFE.Fields.Item("importeUnitarioConImpuesto").Value.ToString());
                            oItem.codigoImporteUnitarioConImpuesto = oRecFE.Fields.Item("codigoImporteUnitarioConImpuesto").Value.ToString();
                            oItem.importeReferencial = Convert.ToDecimal(oRecFE.Fields.Item("importeReferencial").Value.ToString());
                            oItem.codigoRazonExoneracion = (oRecFE.Fields.Item("codigoRazonExoneracion").Value.ToString());

                            //02: Valor referencialunitario en operacionesno onerosas
                            if (oRecFE.Fields.Item("codigoImporteReferencial").Value.ToString() != "")
                            {
                                oItem.codigoImporteReferencial = (oRecFE.Fields.Item("codigoImporteReferencial").Value.ToString());
                            }



                            oItem.importeTotalImpuestos = Convert.ToDecimal(oRecFE.Fields.Item("importeTotalImpuestos").Value.ToString());
                            oItem.montoBaseIgv = Convert.ToDecimal(oRecFE.Fields.Item("montoBaseIgv").Value.ToString());
                            oItem.tasaIgv = Convert.ToDecimal(oRecFE.Fields.Item("tasaIgv").Value.ToString());
                            oItem.importeIgv = Convert.ToDecimal(oRecFE.Fields.Item("importeIgv").Value.ToString());
                            oItem.importeTotalSinImpuesto = Convert.ToDecimal(oRecFE.Fields.Item("importeTotalSinImpuesto").Value.ToString());
                            contadorItem++;
                            oDocumentoFE.Items.Add(oItem);
                            oRecFE.MoveNext();
                        }
                    }
                }
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



            try
            {
                switch (ObjType)
                {
                    case 13:
                        nombre_Store = "SMC_COMPROBANTE_FE"; //OINV
                        break;
                    case 14:
                        nombre_Store = "SMC_COMPROBANTENC_FE"; //OPDN
                        break;

                    default:
                        nombre_Store = "SMC_COMPROBANTE_FE"; //OINV
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
            xmlString = xmlString.Replace("<parametros>", "<parametros/>");
            return xmlString;
        }
        #endregion

    }
}
