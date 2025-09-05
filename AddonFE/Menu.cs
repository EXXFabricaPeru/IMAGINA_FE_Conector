using AddonFE.Configuraciones;
using AddonFE.Configuraciones;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AddonFE
{
    class Menu
    {
        SAPbobsCOM.Company oCompany2;
        public Menu(SAPbobsCOM.Company oCompany)
        {
            if (oCompany2 == null)
            {
                oCompany2 = oCompany;
            }
        }


        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus;
            SAPbouiCOM.MenuItem oMenuItem;




            oMenus = Program.SboAplicacion.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Program.SboAplicacion.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Program.SboAplicacion.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "AddOnSmartCode";
            oCreationPackage.String = "Exxis";
            //oCreationPackage.String = "Slin";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;
            oCreationPackage.Image = Environment.CurrentDirectory + "\\logoSmart.png";

            oMenus = oMenuItem.SubMenus;

            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception ex)
            {
                Program.SboAplicacion.SetStatusBarMessage("Verifique " + ex, SAPbouiCOM.BoMessageTime.bmt_Short, false);

            }




            oMenuItem = Program.SboAplicacion.Menus.Item("AddOnSmartCode");
            oMenus = oMenuItem.SubMenus;
            try
            {
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "SMC1";
                oCreationPackage.String = "Crear Campo Usuario";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception ex)
            { //  Menu already exists
                Program.SboAplicacion.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            //try
            //{
            //    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            //    oCreationPackage.UniqueID = "SMC2SP";
            //    oCreationPackage.String = "Crear Procedimientos Almacenados";
            //    oMenus.AddEx(oCreationPackage);
            //}
            //catch (Exception)
            //{
            //    Program.SboAplicacion.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            //}
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {



                if (pVal.BeforeAction && pVal.MenuUID == "SMC1")
                {
                    Thread crearCamposTablas = new Thread(new ThreadStart(this.crearCamposTablas));
                    crearCamposTablas.Start();
                    //this.crearCamposTablas();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "SMC2SP")
                {
                    Thread crearprocedimientos = new Thread(new ThreadStart(this.CrearProcedimientosAlmacenados));
                    crearprocedimientos.Start();
                    //this.crearCamposTablas();
                }

            }
            catch (Exception ex)
            {
                Program.SboAplicacion.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

        #region crearCamposTablas
        public void crearCamposTablas()
        {
            try
            {


                sapObjUser sapObj = new sapObjUser(Program.SboAplicacion, oCompany2);
                string[] validValues = null;
                string[] validDes = null;



                #region CamposCreado con metodo nuevo
                sapObj.CreaCampoMD("OINV", "SMC_ORDENCOMPRA_FE", "FE ORDEN COMPRA", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
               20, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                #region GUIA DE REMISION ELECTRONICA



                sapObj.CreaTablaMD("SMC_IND_SERV", "SMC: Indicador Servicio GRE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                
                sapObj.CreaTablaMD("SMC_UNID_MED_CPE", "SMC: Unidad Medida CPE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                sapObj.CreaCampoMD("SMC_UNID_MED_CPE", "SMC_UNIDAD_MEDIDA_SAP", "Unidad Medida", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                  50, SAPbobsCOM.BoYesNoEnum.tNO, validValues, validDes, null, null, null);
                sapObj.CreaCampoMD("SMC_UNID_MED_CPE", "SMC_UND_MED_NAC_GRE", "Unidad Medida Nac GRE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                  50, SAPbobsCOM.BoYesNoEnum.tNO, validValues, validDes, null, null, null);
                sapObj.CreaCampoMD("SMC_UNID_MED_CPE", "SMC_UND_MED_EXP_GRE", "Unidad Medida Exp GRE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                  50, SAPbobsCOM.BoYesNoEnum.tNO, validValues, validDes, null, null, null);
                sapObj.CreaCampoMD("SMC_UNID_MED_CPE", "SMC_UND_MED_NAC_CPE", "Unidad Medida Nac CPE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                  50, SAPbobsCOM.BoYesNoEnum.tNO, validValues, validDes, null, null, null);
                sapObj.CreaCampoMD("SMC_UNID_MED_CPE", "SMC_UND_MED_EXP_CPE", "Unidad Medida Exp CPE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                  50, SAPbobsCOM.BoYesNoEnum.tNO, validValues, validDes, null, null, null);

                #region Conductor GREE
                sapObj.CreaTablaMD("SMC_CONDUCTOR_GRE", "SMC: Datos del Conductor GRE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                sapObj.CreaCampoMD("SMC_CONDUCTOR_GRE", "SMC_NOMBRES", "Nombres", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("SMC_CONDUCTOR_GRE", "SMC_APELLIDOS", "Apellidos", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);


                validValues = new string[5];
                validDes = new string[5];
                validValues[0] = "0";
                validValues[1] = "1";
                validValues[2] = "4";
                validValues[3] = "6";
                validValues[4] = "7";
                validDes[0] = "OTROS TIPOS DOCUMENTO";
                validDes[1] = "DNI";
                validDes[2] = "CARNET EXTRANJERIA";
                validDes[3] = "RUC";
                validDes[4] = "PASAPORTE";
                sapObj.CreaCampoMD("SMC_CONDUCTOR_GRE", "SMC_TIPODOCUI", "Tipo Documento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                   40, SAPbobsCOM.BoYesNoEnum.tNO, validValues, validDes, null, null, null);


                validValues = new string[2];
                validDes = new string[2];
                validValues[0] = "Y";
                validValues[1] = "N";
                validDes[0] = "SI";
                validDes[1] = "NO";
                sapObj.CreaCampoMD("OUSR", "SMC_ANU_FE", "ANULAR FE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                   40, SAPbobsCOM.BoYesNoEnum.tNO, validValues, validDes,null, null, null);

                sapObj.CreaCampoMD("OINV", "SMC_ENVIO_AUTO", "ENVIAR AUT. FE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                 40, SAPbobsCOM.BoYesNoEnum.tNO, validValues, validDes, null, null, null);


                sapObj.CreaCampoMD("SMC_CONDUCTOR_GRE", "SMC_NUMERODOCI", "NRO DOCUMENTO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                15, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("SMC_CONDUCTOR_GRE", "SMC_LICENCIA", "LICENCIA", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                20, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("ODLN", "SMC_CONDUCTOR_GRE", "GRE Conductor", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_CONDUCTOR_GRE", null);

                #endregion

                #region Estado GRE
                
                sapObj.CreaTablaMD("SMC_ESTADO_GRE", "SMC: Estado GRE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                
                sapObj.CreaCampoMD("ODLN", "SMC_ESTADO_GRE", "GRE ESTADO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                 50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_ESTADO_GRE", null);

                #endregion

                
                sapObj.CreaCampoMD("ODLN", "SMC_SERIE_GRE", "GRE SERIE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                4, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("ODLN", "SMC_NUM_GRE", "GRE Correlativo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                8, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("ODLN", "SMC_BULTOS_GRE", "GRE Bultos", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None,
                10, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("ODLN", "SMC_MOTVOTROS_GRE", "GRE Motivo Otros", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                100, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("ODLN", "SMC_MSJ_ESTADO_GRE", "GRE ESTADO MSJ", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                150, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                #region Modalidad Traslado GRE
                sapObj.CreaTablaMD("SMC_MOD_TRAS_GRE", "SMC : Modalidad Traslado GRE", SAPbobsCOM.BoUTBTableType.bott_NoObject);

                sapObj.CreaCampoMD("ODLN", "SMC_MOD_TRAS", "GRE MODALIDAD TRASLADO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_MOD_TRAS_GRE", null);
                #endregion

                #region Unidad Medida GRE
                sapObj.CreaTablaMD("SMC_UNID_MED_GRE", "SMC: Unidad Medida GRE", SAPbobsCOM.BoUTBTableType.bott_NoObject);

                sapObj.CreaCampoMD("ODLN", "SMC_UND_MED_GRE", "GRE UND MEDIDA", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                10, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_UNID_MED_GRE", null);
                #endregion


                #region Lugar Empresa GRE
                sapObj.CreaTablaMD("SMC_GRE_LUGAREMP", "SMC: Lugar Empresa GRE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                sapObj.CreaCampoMD("SMC_GRE_LUGAREMP", "SMC_DEPARTAMENTO", "DEPARTAMENTO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                100, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                
                sapObj.CreaCampoMD("SMC_GRE_LUGAREMP", "SMC_PROVINCIA", "Provincia", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                100, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                
                sapObj.CreaCampoMD("SMC_GRE_LUGAREMP", "SMC_DISTRITO", "Distrito", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                100, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                
                sapObj.CreaCampoMD("SMC_GRE_LUGAREMP", "SMC_DIRECCION", "Direccion", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                100, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                
                sapObj.CreaCampoMD("SMC_GRE_LUGAREMP", "SMC_UBIGEO", "Ubigeo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                10, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                
                sapObj.CreaCampoMD("SMC_GRE_LUGAREMP", "SMC_CODESTABLE", "CodEstablecimiento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                8, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("ODLN", "SMC_LUGARPAR_GRE", "GRE Lugar Partida", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_GRE_LUGAREMP", null);

                #endregion


                #endregion


                #region COMPROBANTES DE PAGO ELECTRONICO (CPE)

                #region Tipo Operacion
                sapObj.CreaTablaMD("SMC_TIPOPERACION_FE", "SMC: Tipo Operacion FE", SAPbobsCOM.BoUTBTableType.bott_NoObject);

                
                sapObj.CreaCampoMD("OINV", "SMC_TIPOOPERACION_FE", "FE TIPO OPERACION:", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_TIPOPERACION_FE", null);
                #endregion

                #region TipoAfectacion Detalle
                sapObj.CreaTablaMD("SMC_TIPOAFEC_FE", "SMC: Tipo AFECTACION FE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                sapObj.CreaCampoMD("INV1", "SMC_TIPOAFECT_FE", "FE TIPO OPERACION:", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_TIPOAFEC_FE", null);
                #endregion

                sapObj.CreaCampoMD("OINV", "SMC_SERIE_FE", "FE SERIE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
               4, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

            

                sapObj.CreaCampoMD("OINV", "SMC_NUM_FE", "FE Correlativo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                8, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                #region Estado FE
                sapObj.CreaTablaMD("SMC_ESTADO_FE", "SMC: Estado FE", SAPbobsCOM.BoUTBTableType.bott_NoObject);

                sapObj.CreaCampoMD("OINV", "SMC_ESTADO_FE", "FE ESTADO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                 50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_ESTADO_FE", null);
                #endregion

                sapObj.CreaCampoMD("OINV", "SMC_MSJ_ESTADO_FE", "FE ESTADO MSJ", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                150, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("ORIN", "SMC_MOTNC_FE", "FE MOTIVO NC", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                100, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                
                sapObj.CreaTablaMD("SMC_TIPONC_FE", "SMC: Tipo de Nota de Credito", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                sapObj.CreaCampoMD("ORIN", "SMC_TIPONC_FE", "FE TIPO NC", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                100, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_TIPONC_FE", null);

                sapObj.CreaTablaMD("SMC_TIPOND_FE", "SMC: Tipo de Nota de Debito", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                sapObj.CreaCampoMD("ORIN", "SMC_TIPOND_FE", "FE TIPO ND", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_TIPOND_FE", null);

                sapObj.CreaTablaMD("SMC_CUOTASNC", "SMC CUOTAS NC", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                sapObj.CreaTablaMD("SMC_CUOTASNCD", "SMC DETALLE CUOTAS NC", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
                sapObj.CreaCampoMD("SMC_CUOTASNCD", "SMC_CUOTA", "Num Cuota", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None,
                2, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_CUOTASNCD", "SMC_MONTO", "Monto", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_CUOTASNCD", "SMC_FECHAV", "Fecha V.", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);


                sapObj.CreaCampoMD("OVPM", "SMC_SERIE_FE", "FE SERIE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                4, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("OVPM", "SMC_NUM_FE", "FE Correlativo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                8, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);


                sapObj.CreaCampoMD("OVPM", "SMC_ESTADO_FE", "FE ESTADO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                 50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, "SMC_ESTADO_FE", null);

                sapObj.CreaCampoMD("OVPM", "SMC_MSJ_ESTADO_FE", "FE ESTADO MSJ", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                150, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                #region Resumen de BAJA
                sapObj.CreaTablaMD("SMC_RESUBAJA_FE", "SMC: RESUMEN BAJA FE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_RESUMENID", "RESUMEN ID", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_FECHAEMI", "FECHA EMISION", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_FECHAGENE", "FECHA GENERACION", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_RAZONSOCIAL", "RAZON SOCIAL", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                150, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_TIPODOCUMENTO", "TIPO DOCUMENTO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                10, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_SERIEBAJA", "SERIE CPE BAJA", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                8, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_NUMERO", "NUMERO CPE BAJA", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                10, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_MOTIVOBAJA", "MOTIVO BAJA", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                150, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_ESTADOBAJA", "ESTADO BAJA", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
               10, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("SMC_RESUBAJA_FE", "SMC_RUCEMISOR", "RUC EMISOR", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                20, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                sapObj.CreaCampoMD("OINV", "SMC_MOTIVOBAJA", "FE MOTIVO BAJA", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None,
                80, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);

                sapObj.CreaCampoMD("OINV", "SMC_FECHABAJA", "FE FECHA BAJA", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None,
                50, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null, null);
                #endregion

                #endregion


                #endregion


                #region Crear UDOS

                // Inicializa el objeto UserObjectsMD para crear un nuevo UDO
                UserObjectsMD UDO = (UserObjectsMD)oCompany2.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
                UDO.Code = "SMC_CUOTASNC"; // Código único para el UDO
                UDO.Name = "CUOTAS NC"; // Nombre del UDO
                UDO.ObjectType = BoUDOObjType.boud_MasterData; // Tipo de objeto UDO (en este caso, datos maestros)
                UDO.TableName = "SMC_CUOTASNC"; // Nombre de la tabla asociada al UDO
               
                UDO.CanDelete = BoYesNoEnum.tYES; // Define si se puede eliminar el UDO
                UDO.CanFind = BoYesNoEnum.tYES; // Define si se puede buscar el UDO
                UDO.CanLog = BoYesNoEnum.tYES; // Define si se puede transferir el UDO a un nuevo año fiscal
                
                UDO.CanCreateDefaultForm= BoYesNoEnum.tYES;
                UDO.OverwriteDllfile= BoYesNoEnum.tYES;
                // Agrega el UDO al sistema
                if (UDO.Add() != 0)
                {
                    int errorCode;
                    string errorMessage= oCompany2.GetLastErrorDescription();
                    Program.SboAplicacion.MessageBox(errorMessage);
  
                }
                else
                {
                    Program.SboAplicacion.MessageBox("UDO creado exitosamente.");
                }
                
                #endregion


                Program.SboAplicacion.MessageBox("Campo usuario creado", 1, "Ok", "", "");
            }
            catch (Exception ex)
            {

                Program.SboAplicacion.MessageBox(ex.ToString(), 1, "Ok", "", "");

            }
        }
        #endregion

        #region CrearProcedimientosAlmacenados
        public void CrearProcedimientosAlmacenados()
        {
            bool isHana = this.IsHana();
            Procedures storeproceduremain = new Procedures(oCompany2);
            storeproceduremain.DropCreateProcedure("SMC_Actualizar_FELFolioPrefNumFactAnt", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_Actualizar_FELFolioPrefNumGUIA", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_Actualizar_FELFolioPrefNumOINV", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_Actualizar_FELFolioPrefNumORIN", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_ActualizarEstadoFE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_ANULACION_OINV_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_ANULACION_OVPM_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTE_DETALLE_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTE_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTECUOTAS_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTECUOTASODPI_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTENC_DETALLE_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTENC_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTEODPI_DETALLE_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTEODPI_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTES_ELECTRONICOS", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_COMPROBANTES_ELECTRONICOS_SERVICIO", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_LISTARANTICIPOS_OINV", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_OBTENER_SUCURSALES", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_ODLN_GRE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_OIGE_GRE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_OINV_GRE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_ORPD_GRE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_OWTR_GRE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_RETENCION_DETALLE_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_RETENCION_FE", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_Validar_FELFolioFactAnt", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_Validar_FELFolioOINV", isHana, "AddonFE.Properties.Resources");
            storeproceduremain.DropCreateProcedure("SMC_Validar_FELFolioORIN", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Comprobante_GuiaRemision", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Actualizar_FELFolioPrefNumOINV", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_GetConfig", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Validar_FELFolioOINV", isHana, "AddonFE.Properties.Resources");
            ////storeproceduremain.DropCreateFunction("RPT_SMC_ConvertirNumeroLetra", isHana, "AddonFE.Properties.Resources");
            ////storeproceduremain.DropCreateFunction("RPT_SMC_ConvertirNumeroLetraINGLES", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Comprobante_21", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_MontosAnticipos_21", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_FE_Retencion_FND", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_FE_FormaPago_FND", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Detalle_Exportacion_21", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Empresa_21", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Empresa_Config", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Anulacion", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Validar_FELFolioORIN", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Actualizar_FELFolioPrefNumORIN", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Validar_FELFolioFactAnt", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Actualizar_FELFolioPrefNumFactAnt", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Comprobante_21_Anticipo", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Detalle_Exportacion_21_Anticipo", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_documentos_actualizarEstadoFE", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Comprobante_NotaCredito_21", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Comprobante_NotaCredito_Adicional_21", isHana, "AddonFE.Properties.Resources");
            //storeproceduremain.DropCreateProcedure("SMC_Detalle_NotaCredito_21", isHana, "AddonFE.Properties.Resources");

            //storeproceduremain.DropCreateProcedure("SMC_FE_FormaPago_NC", isHana, "AddonFE.Properties.Resources");

            //#region Clientes con Retencion
            //if (ConfigurationManager.AppSettings["Retencion"] == "Y")
            //{
            //    storeproceduremain.DropCreateProcedure("SMC_FE_Retencion_NC", isHana, "AddonFE.Properties.Resources");
            //}

            //#endregion

        }
        #endregion

        public bool IsHana()
        {
            try
            {
                if (oCompany2.DbServerType == (SAPbobsCOM.BoDataServerTypes)9)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Program.SboAplicacion.MessageBox(ex.Message);
                return false;
            }
        }
    }
}
