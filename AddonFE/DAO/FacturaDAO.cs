using AddonFE.Configuraciones;
using AddonFE.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AddonFE.DAO
{
    public class FacturaDAO
    {

        #region Validad y actualizar Existencia Factura Anticipo
        public bool ValidarExistenciaFolioFactAnt(int Docentry,string TipoDoc, int DocNum, int ObjectType, SAPbobsCOM.Company oCompanyDAO)
        {
            bool ExistenciaFolio = false;
            string nombre_Store = "";
            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            //SAPbobsCOM.Company oCompanyDAO;
            //oCompanyDAO = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            nombre_Store = "SMC_Validar_FELFolioFactAnt";
            string Query = "";
            String FolioNum = "";
            try
            {
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" ('" + Docentry + "','" + ObjectType + "','" + TipoDoc + "')"; }
                else { Query = "EXEC \"" + nombre_Store + "\" '" + Docentry + "','" + ObjectType + "','" + TipoDoc + "'"; }
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

        public void Update_Folio_FactAnt(string TipoDoc, int DocNum, int ObjectType, int DocEntry, ref string FolioPref, ref string FolioNum, SAPbobsCOM.Company oCompanyDAO)
        {
            FolioNum = "0";
            FolioPref = "0";
            string nombre_Store = "SMC_Actualizar_FELFolioPrefNumFactAnt";
            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            //SAPbobsCOM.Company oCompanyDAO;
            //oCompanyDAO = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            string Query = "";
            try
            {
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" ('" + DocEntry + "','" + ObjectType + "','" + TipoDoc + "')"; }
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


        #region Validar Existencia Folio OINV
        public bool ValidarExistenciaFolioOINV(int DocEntry, string TipoDoc, int DocNum, int ObjectType, SAPbobsCOM.Company oCompanyDAO)
        {
            bool ExistenciaFolio = false;
            string nombre_Store = "";
            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            //SAPbobsCOM.Company oCompanyDAO;
            //oCompanyDAO = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            nombre_Store = "SMC_Validar_FELFolioOINV";
            string Query = "";
            String FolioNum = "";
            try
            {
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" ('" + DocEntry + "','" + ObjectType + "','" + TipoDoc + "')"; }
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

        public void Update_Folio_Fact(string TipoDoc, int DocNum, int ObjectType, int DocEntry, ref string FolioPref, ref string FolioNum, SAPbobsCOM.Company oCompanyDAO)
        {
            string nombre_Store = "SMC_Actualizar_FELFolioPrefNumOINV";
            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            //SAPbobsCOM.Company oCompanyDAO;
            //oCompanyDAO = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            string Query = "";
            try
            {
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" ('" + DocEntry + "','" + ObjectType + "','" + TipoDoc + "')"; }
                else { Query = "EXEC \"" + nombre_Store + "\" '" + DocEntry + "','" + ObjectType + "','" + TipoDoc + "'"; }
                oRecFE = oProcedures.RunQuery(Query);
                if (oRecFE != null)
                {
                    if (oRecFE.RecordCount > 0)
                    {
                        oRecFE.MoveFirst();
                        while (!oRecFE.EoF)
                        {
                            // DocEntry = oRecFE.Fields.Item("DocEntry").Value.ToString();
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
            string nombre_Store = "SMC_COMPROBANTE_FE";
            string nombre_Store1 = "SMC_COMPROBANTE_DETALLE_FE";
            string nombre_Store2 = "SMC_COMPROBANTECUOTAS_FE";
            string nombre_Store3 = "SMC_LISTARANTICIPOS_OINV";
            
            /*
           
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

                            oDocumentoFE.tipoOperacion = oRecFE.Fields.Item("tipoOperacion").Value.ToString();
                            oDocumentoFE.totalImpuestos = Convert.ToDecimal(oRecFE.Fields.Item("totalImpuestos").Value.ToString());
                            oDocumentoFE.totalValorVentaNetoOpGratuitas = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpGratuitas").Value.ToString()),4);
                            oDocumentoFE.totalValorVentaNetoOpGravadas = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpGravadas").Value.ToString()), 4);
                            oDocumentoFE.totalValorVentaNetoOpNoGravada = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpNoGravada").Value.ToString()), 4);
                            oDocumentoFE.totalIgv = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalIgv").Value.ToString()),4);
                            oDocumentoFE.totalVenta = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalVenta").Value.ToString()), 4);
                            oDocumentoFE.totalValorVenta = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalValorVenta").Value.ToString()), 4);
                            oDocumentoFE.totalPrecioVenta = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalPrecioVenta").Value.ToString()), 4);
                            oDocumentoFE.formaPagoNegociable= Convert.ToInt32( oRecFE.Fields.Item("formaPagoNegociable").Value.ToString());

                            if (FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalTributosOpeGratuitas").Value.ToString()), 4) > 0)
                            {
                                oDocumentoFE.totalTributosOpeGratuitas = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalTributosOpeGratuitas").Value.ToString()), 4);
                            }
                            

                            var dd= FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpNoGravada").Value.ToString()), 4);

                            if (Convert.ToDecimal(oRecFE.Fields.Item("importeRetencion").Value.ToString())!=0)
                            {
                                oDocumentoFE.importeOpeRetencion = Convert.ToDecimal(oRecFE.Fields.Item("importeOpeRetencion").Value.ToString());
                                oDocumentoFE.porcentajeRetencion = Convert.ToDecimal(oRecFE.Fields.Item("porcentajeRetencion").Value.ToString());
                                oDocumentoFE.importeRetencion = Convert.ToDecimal(oRecFE.Fields.Item("importeRetencion").Value.ToString());
                             

                            }

                            #region Detraccion
                            if (oRecFE.Fields.Item("TieneDetraccion").Value.ToString()=="Y")
                            {
                                oDocumentoFE.codigoDetraccion = oRecFE.Fields.Item("codigoDetraccion").Value.ToString();
                                oDocumentoFE.numeroCtaBancoNacion = oRecFE.Fields.Item("numeroCtaBancoNacion").Value.ToString();
                                oDocumentoFE.formaPago = oRecFE.Fields.Item("formaPago").Value.ToString();
                                oDocumentoFE.totalDetraccion = oRecFE.Fields.Item("totalDetraccion").Value.ToString();
                                oDocumentoFE.porcentajeDetraccion = oRecFE.Fields.Item("porcentajeDetraccion").Value.ToString();

                                oDocumentoFE.codigoAuxiliar100_5 = oRecFE.Fields.Item("codigoAuxiliar100_5").Value.ToString();
                                oDocumentoFE.textoAuxiliar100_5 = oRecFE.Fields.Item("textoAuxiliar100_5").Value.ToString();
                                oDocumentoFE.codigoAuxiliar500_1 = oRecFE.Fields.Item("codigoAuxiliar500_1").Value.ToString();
                                oDocumentoFE.textoAuxiliar500_1 = oRecFE.Fields.Item("textoAuxiliar500_1").Value.ToString();
                                if (oRecFE.Fields.Item("codigoLeyenda_4").Value.ToString().Length > 0)
                                {
                                    oDocumentoFE.codigoLeyenda_4 = oRecFE.Fields.Item("codigoLeyenda_4").Value.ToString();
                                    oDocumentoFE.textoLeyenda_4 = oRecFE.Fields.Item("textoLeyenda_4").Value.ToString();
                                }

                            }

                            oDocumentoFE.codigoLeyenda_1 = oRecFE.Fields.Item("codigoLeyenda_1").Value.ToString();
                            oDocumentoFE.textoLeyenda_1 = oRecFE.Fields.Item("textoLeyenda_1").Value.ToString();

                            

                            if (oRecFE.Fields.Item("ordenCompra").Value.ToString().Length>0)
                            {
                                oDocumentoFE.ordenCompra = oRecFE.Fields.Item("ordenCompra").Value.ToString();
                            }

                            if (oRecFE.Fields.Item("numeroDocumentoReferencia_1").Value.ToString().Length > 0)
                            {
                                oDocumentoFE.tipoReferencia_1 = oRecFE.Fields.Item("tipoReferencia_1").Value.ToString();
                                oDocumentoFE.numeroDocumentoReferencia_1 = oRecFE.Fields.Item("numeroDocumentoReferencia_1").Value.ToString();
                            }
                            #endregion

                            #region Anticipos
                            if (Convert.ToDecimal(oRecFE.Fields.Item("totalDsctoGlobalesAnticipo").Value.ToString())> 0)
                            {
                                oDocumentoFE.montoBaseDsctoGlobalAnticipo = Convert.ToDecimal(oRecFE.Fields.Item("montoBaseDsctoGlobalAnticipo").Value.ToString());
                                oDocumentoFE.porcentajeDsctoGlobalAnticipo = Convert.ToDecimal(oRecFE.Fields.Item("porcentajeDsctoGlobalAnticipo").Value.ToString());
                                oDocumentoFE.totalDsctoGlobalesAnticipo = Convert.ToDecimal(oRecFE.Fields.Item("totalDsctoGlobalesAnticipo").Value.ToString());
                                oDocumentoFE.totalDocumentoAnticipo = Convert.ToDecimal(oRecFE.Fields.Item("totalDocumentoAnticipo").Value.ToString());
                            }
                            //else
                            //{
                            //    oDocumentoFE.totalDsctoGlobalesAnticipo = 0;
                            //}

                            #region Nota de Debito
                            if (oDocumentoFE.tipoDocumento=="08")
                            {
                                oDocumentoFE.codigoSerieNumeroAfectado = oRecFE.Fields.Item("codigoSerieNumeroAfectado").Value.ToString();
                                oDocumentoFE.motivoDocumento = oRecFE.Fields.Item("motivoDocumento").Value.ToString();
                                oDocumentoFE.tipoDocumentoReferenciaPrincipal = oRecFE.Fields.Item("tipoDocumentoReferenciaPrincipal").Value.ToString();
                                oDocumentoFE.numeroDocumentoReferenciaPrincipal = oRecFE.Fields.Item("numeroDocumentoReferenciaPrincipal").Value.ToString();

                            }

                            #endregion


                            #endregion


                            oDocumentoFE.inHabilitado = "1";
                            oRecFE.MoveNext();
                        }
                    }
                }


                #endregion

                #region Coutas Factura
                if (oDocumentoFE.formaPagoNegociable!=0)
                {
                    if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store2 + "\" (" + DocEntry + ")"; }
                    else { Query = "EXEC \"" + nombre_Store2 + "\" " + DocEntry; }
                    oRecFE = oProcedures.RunQuery(Query);
                    if (oRecFE != null)
                    {
                        if (oRecFE.RecordCount > 0)
                        {
                            int contadorItemCuota = 1;
                            oRecFE.MoveFirst();
                            while (!oRecFE.EoF)
                            {
                                oDocumentoFE.montoNetoPendiente= Convert.ToDecimal(oRecFE.Fields.Item("MontoPendientePago").Value.ToString());
                                if (contadorItemCuota == 1)
                                {
                                    oDocumentoFE.montoPagoCuota1= Convert.ToDecimal(oRecFE.Fields.Item("MontoCuota").Value.ToString());
                                    oDocumentoFE.fechaPagoCuota1 = (oRecFE.Fields.Item("FechaPago").Value.ToString());
                                }

                                if (contadorItemCuota == 2)
                                {
                                    oDocumentoFE.montoPagoCuota2 = Convert.ToDecimal(oRecFE.Fields.Item("MontoCuota").Value.ToString());
                                    oDocumentoFE.fechaPagoCuota2 = (oRecFE.Fields.Item("FechaPago").Value.ToString());
                                }


                                if (contadorItemCuota == 3)
                                {
                                    oDocumentoFE.montoPagoCuota3 = Convert.ToDecimal(oRecFE.Fields.Item("MontoCuota").Value.ToString());
                                    oDocumentoFE.fechaPagoCuota3 = (oRecFE.Fields.Item("FechaPago").Value.ToString());
                                }

                                contadorItemCuota++;
                                
                                oRecFE.MoveNext();
                            }
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
                            oItem.importeUnitarioSinImpuesto = FormatoDecimales( Convert.ToDecimal(oRecFE.Fields.Item("importeUnitarioSinImpuesto").Value.ToString()), 4);
                            oItem.importeUnitarioConImpuesto = FormatoDecimales( Convert.ToDecimal(oRecFE.Fields.Item("importeUnitarioConImpuesto").Value.ToString()),4);
                            oItem.codigoImporteUnitarioConImpuesto = oRecFE.Fields.Item("codigoImporteUnitarioConImpuesto").Value.ToString();
                            oItem.importeReferencial = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("importeReferencial").Value.ToString()),4);
                            oItem.codigoRazonExoneracion = (oRecFE.Fields.Item("codigoRazonExoneracion").Value.ToString());

                            //02: Valor referencialunitario en operacionesno onerosas
                            if (oRecFE.Fields.Item("codigoImporteReferencial").Value.ToString() != "")
                            {
                                oItem.codigoImporteReferencial = (oRecFE.Fields.Item("codigoImporteReferencial").Value.ToString());
                            }
                            
                            
                            
                            oItem.importeTotalImpuestos = Convert.ToDecimal(oRecFE.Fields.Item("importeTotalImpuestos").Value.ToString());
                            oItem.montoBaseIgv = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("montoBaseIgv").Value.ToString()),4);
                            oItem.tasaIgv = Convert.ToDecimal(oRecFE.Fields.Item("tasaIgv").Value.ToString());
                            oItem.importeIgv = Convert.ToDecimal(oRecFE.Fields.Item("importeIgv").Value.ToString());
                            oItem.importeTotalSinImpuesto = FormatoDecimales(Convert.ToDecimal(oRecFE.Fields.Item("importeTotalSinImpuesto").Value.ToString()), 4);
                            contadorItem++;
                            oDocumentoFE.Items.Add(oItem);
                            oRecFE.MoveNext();
                        }
                    }
                }
                #endregion

                #region Lista Anticipos
                //if (oDocumentoFE.totalDsctoGlobalesAnticipo>0)
                //{
                //    oDocumentoFE.Anticipos = new List<Anticipo>();
                //    Anticipo oAnticipo = new Anticipo();
                //    if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store3 + "\" (" + DocEntry + ")"; }
                //    else { Query = "EXEC \"" + nombre_Store3 + "\" " + DocEntry; }
                //    oRecFE = oProcedures.RunQuery(Query);
                //    if (oRecFE != null)
                //    {
                //        if (oRecFE.RecordCount > 0)
                //        {
                //            int contadorAnticipo = 1;
                //            oRecFE.MoveFirst();
                //            while (!oRecFE.EoF)
                //            {
                //                oAnticipo = new Anticipo();
                //                oAnticipo.indicador = oRecFE.Fields.Item("indicador").Value.ToString();
                //                oAnticipo.numeroOrdenAnticipo = contadorAnticipo;
                //                oAnticipo.totalPrepagadoAnticipo = Convert.ToDecimal(oRecFE.Fields.Item("totalPrepagadoAnticipo").Value.ToString());
                //                oAnticipo.fechaPago = oRecFE.Fields.Item("fechaPago").Value.ToString();
                //                oAnticipo.serieNumeroDocumentoAnticipo = oRecFE.Fields.Item("serieNumeroDocumentoAnticipo").Value.ToString();
                //                oAnticipo.tipoDocumentoAnticipo = oRecFE.Fields.Item("tipoDocumentoAnticipo").Value.ToString();
                //                oAnticipo.numeroDocumentoEmisorAnticipo = (oRecFE.Fields.Item("numeroDocumentoEmisorAnticipo").Value.ToString());
                //                oAnticipo.tipoDocumentoEmisorAnticipo = (oRecFE.Fields.Item("tipoDocumentoEmisorAnticipo").Value.ToString());
                //                contadorAnticipo++;
                //                oDocumentoFE.Anticipos.Add(oAnticipo);
                //                oRecFE.MoveNext();
                //            }
                //        }
                //    }
                //}
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

        public string getGeneral21Ant(int DocEntry)
        {
            string xmlString = "";
            FuncionesRequeridas oFuncionesRequeridas = new FuncionesRequeridas();
            string nombre_Store = "SMC_COMPROBANTEODPI_FE";
            string nombre_Store1 = "SMC_COMPROBANTEODPI_DETALLE_FE";
            string nombre_Store2 = "SMC_COMPROBANTECUOTASODPI_FE";
            /*
           
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

                            oDocumentoFE.tipoOperacion = oRecFE.Fields.Item("tipoOperacion").Value.ToString();
                            oDocumentoFE.totalImpuestos = Convert.ToDecimal(oRecFE.Fields.Item("totalImpuestos").Value.ToString());
                            oDocumentoFE.totalValorVentaNetoOpGratuitas = Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpGratuitas").Value.ToString());
                            oDocumentoFE.totalValorVentaNetoOpGravadas = Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpGravadas").Value.ToString());
                            oDocumentoFE.totalValorVentaNetoOpNoGravada = Convert.ToDecimal(oRecFE.Fields.Item("totalValorVentaNetoOpNoGravada").Value.ToString());
                            oDocumentoFE.totalIgv = Convert.ToDecimal(oRecFE.Fields.Item("totalIgv").Value.ToString());
                            oDocumentoFE.totalVenta = Convert.ToDecimal(oRecFE.Fields.Item("totalVenta").Value.ToString());
                            oDocumentoFE.totalValorVenta = Convert.ToDecimal(oRecFE.Fields.Item("totalValorVenta").Value.ToString());
                            oDocumentoFE.totalPrecioVenta = Convert.ToDecimal(oRecFE.Fields.Item("totalPrecioVenta").Value.ToString());
                            oDocumentoFE.formaPagoNegociable = Convert.ToInt32(oRecFE.Fields.Item("formaPagoNegociable").Value.ToString());


                            if (Convert.ToDecimal(oRecFE.Fields.Item("importeRetencion").Value.ToString()) != 0)
                            {
                                oDocumentoFE.importeOpeRetencion = Convert.ToDecimal(oRecFE.Fields.Item("importeOpeRetencion").Value.ToString());
                                oDocumentoFE.porcentajeRetencion = Convert.ToDecimal(oRecFE.Fields.Item("porcentajeRetencion").Value.ToString());
                                oDocumentoFE.importeRetencion = Convert.ToDecimal(oRecFE.Fields.Item("importeRetencion").Value.ToString());


                            }

                            #region Detraccion
                            if (oRecFE.Fields.Item("TieneDetraccion").Value.ToString() == "Y")
                            {
                                oDocumentoFE.codigoDetraccion = oRecFE.Fields.Item("codigoDetraccion").Value.ToString();
                                oDocumentoFE.numeroCtaBancoNacion = oRecFE.Fields.Item("numeroCtaBancoNacion").Value.ToString();
                                oDocumentoFE.formaPago = oRecFE.Fields.Item("formaPago").Value.ToString();
                                oDocumentoFE.totalDetraccion = oRecFE.Fields.Item("totalDetraccion").Value.ToString();
                                oDocumentoFE.porcentajeDetraccion = oRecFE.Fields.Item("porcentajeDetraccion").Value.ToString();

                                oDocumentoFE.codigoAuxiliar100_5 = oRecFE.Fields.Item("codigoAuxiliar100_5").Value.ToString();
                                oDocumentoFE.textoAuxiliar100_5 = oRecFE.Fields.Item("textoAuxiliar100_5").Value.ToString();
                                oDocumentoFE.codigoAuxiliar500_1 = oRecFE.Fields.Item("codigoAuxiliar500_1").Value.ToString();
                                oDocumentoFE.textoAuxiliar500_1 = oRecFE.Fields.Item("textoAuxiliar500_1").Value.ToString();

                                if (oRecFE.Fields.Item("codigoLeyenda_4").Value.ToString().Length > 0)
                                {
                                    oDocumentoFE.codigoLeyenda_4 = oRecFE.Fields.Item("codigoLeyenda_4").Value.ToString();
                                    oDocumentoFE.textoLeyenda_4 = oRecFE.Fields.Item("textoLeyenda_4").Value.ToString();
                                }

                            }
                            #endregion

                            if (oRecFE.Fields.Item("ordenCompra").Value.ToString().Length > 0)
                            {
                                oDocumentoFE.ordenCompra = oRecFE.Fields.Item("ordenCompra").Value.ToString();
                            }

                            if (oRecFE.Fields.Item("numeroDocumentoReferencia_1").Value.ToString().Length > 0)
                            {
                                oDocumentoFE.tipoReferencia_1 = oRecFE.Fields.Item("tipoReferencia_1").Value.ToString();
                                oDocumentoFE.numeroDocumentoReferencia_1 = oRecFE.Fields.Item("numeroDocumentoReferencia_1").Value.ToString();
                            }

                            oDocumentoFE.codigoLeyenda_1 = oRecFE.Fields.Item("codigoLeyenda_1").Value.ToString();
                            oDocumentoFE.textoLeyenda_1 = oRecFE.Fields.Item("textoLeyenda_1").Value.ToString();

                            oDocumentoFE.inHabilitado = "1";
                            oRecFE.MoveNext();
                        }
                    }
                }


                #endregion

                #region Coutas Factura
                if (oDocumentoFE.formaPagoNegociable != 0)
                {
                    if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store2 + "\" (" + DocEntry + ")"; }
                    else { Query = "EXEC \"" + nombre_Store2 + "\" " + DocEntry; }
                    oRecFE = oProcedures.RunQuery(Query);
                    if (oRecFE != null)
                    {
                        if (oRecFE.RecordCount > 0)
                        {
                            int contadorItemCuota = 1;
                            oRecFE.MoveFirst();
                            while (!oRecFE.EoF)
                            {
                                oDocumentoFE.montoNetoPendiente = Convert.ToDecimal(oRecFE.Fields.Item("MontoPendientePago").Value.ToString());
                                if (contadorItemCuota == 1)
                                {
                                    oDocumentoFE.montoPagoCuota1 = Convert.ToDecimal(oRecFE.Fields.Item("MontoCuota").Value.ToString());
                                    oDocumentoFE.fechaPagoCuota1 = (oRecFE.Fields.Item("FechaPago").Value.ToString());
                                }

                                if (contadorItemCuota == 2)
                                {
                                    oDocumentoFE.montoPagoCuota2 = Convert.ToDecimal(oRecFE.Fields.Item("MontoCuota").Value.ToString());
                                    oDocumentoFE.fechaPagoCuota2 = (oRecFE.Fields.Item("FechaPago").Value.ToString());
                                }


                                if (contadorItemCuota == 3)
                                {
                                    oDocumentoFE.montoPagoCuota3 = Convert.ToDecimal(oRecFE.Fields.Item("MontoCuota").Value.ToString());
                                    oDocumentoFE.fechaPagoCuota3 = (oRecFE.Fields.Item("FechaPago").Value.ToString());
                                }

                                contadorItemCuota++;

                                oRecFE.MoveNext();
                            }
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
                    case 203:
                        nombre_Store = "SMC_COMPROBANTEODPI_FE"; //
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

        #region Actualizar Estado FE
        public void SMC_ActualizarEstadoFE(int ObjectType, int DocEntry,string MensajeError, string Estado, SAPbobsCOM.Company oCompanyDAO)
        {
            
            string nombre_Store = "SMC_ActualizarEstadoFE";
            SAPbobsCOM.Recordset oRecFE = default(SAPbobsCOM.Recordset);
            //SAPbobsCOM.Company oCompanyDAO;
            //oCompanyDAO = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
            Procedures oProcedures = new Procedures(oCompanyDAO);
            string Query = "";
            try
            {
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\" ('" + DocEntry + "','" + ObjectType + "','" + MensajeError + "','" + Estado + "')"; }
                else { Query = "EXEC \"" + nombre_Store + "\" '" + DocEntry + "','" + ObjectType + "','" + MensajeError + "','" + Estado + "'"; }
                oRecFE = oProcedures.RunQuery(Query);

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

        #region Resumen Anulacion Factura
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
            odocumento.resumenItem= new ResumenItem();

            try
            {
                switch (ObjType)
                {
                    case 13:
                        nombre_Store = "SMC_ANULACION_OINV_FE"; //OINV
                        break;
                    //case 143:
                    //    nombre_Store = "SMC_Entrada_Mercancia_GuiaRemision_Compra\""; //OPDN
                    //    break;

                    default:
                        nombre_Store = "SMC_ANULACION_OINV_FE"; //OINV
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
                            odocumento.resumenItem.numeroDocumentoBaja = oRec.Fields.Item("numeroDocumentoBaja").Value.ToString();
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
        
        
        public bool ResumenAnulacionActualizar(int DocEntry, int Formulario, int ObjType, SAPbobsCOM.Company oCompany, ref string mensaje)
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
                    case 13:
                        nombre_Store = "SMC_ANULACION_OINV_FE_ACTUALIZAR"; //OINV
                        break;
                    //case 143:
                    //    nombre_Store = "SMC_Entrada_Mercancia_GuiaRemision_Compra\""; //OPDN
                    //    break;

                    default:
                        nombre_Store = "SMC_ANULACION_OINV_FE"; //OINV
                        break;
                }


                string Query = "";
                int contadorresultado = 0;
                if (oProcedures.IsHanaProcedure()) { Query = "CALL \"" + nombre_Store + "\"(" + DocEntry + ")"; }
                else { Query = "EXEC \"" + nombre_Store + "\" " + DocEntry; }
                oRec = oProcedures.RunQuery(Query);
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

           return true;
        }
        #endregion

        static decimal FormatoDecimales(decimal numero,int numDecimal)
        {
            // Si el número es 0 y se pide que se formatee con 2 decimales, devolver "0.00"
            if (numero == 0 && numDecimal == 2)
            {
                return 0.00m;
            }
            else
            {
                return Math.Round(numero, numDecimal);
            }
        }
    }
    
}
