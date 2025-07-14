
using AddonFE.Configuraciones;
using AddonFE.DAO;
using AddonFE.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AddonFE.Main
{
    class FacturaSAP
    {
        SAPbobsCOM.Company oCompany;
        string formulario = "";


        SAPbouiCOM.EditText DocEntryFormDA = null;
        SAPbouiCOM.EditText DocNumFormDA = null;
        SAPbouiCOM.EditText ObjTypeDA = null;
        SAPbouiCOM.DataTable dtFormDA = null;
        SAPbouiCOM.Matrix grdListaFormDA;

        SAPbouiCOM.Matrix grdListaFormDA1;
        SAPbouiCOM.DataTable dtFormDA1 = null;



        public void CrearObjetoenFactura(SAPbobsCOM.Company oCompany2, SAPbouiCOM.Application SBO_Application)
        {
            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
            if (oCompany == null)
                oCompany = oCompany2;
        }

        void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    System.Windows.Forms.Application.Exit();
                    break;
                default:
                    break;
            }
        }


        void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (oCompany == null)
                {
                    oCompany = (SAPbobsCOM.Company)Program.SboAplicacion.Company.GetDICompany();
                }


                #region validar Campos
                /*
                if (pVal.FormTypeEx.Equals("133") && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD))
                {
                    //et_ALL_EVENTS

                    Program.SboAplicacion.SetStatusBarMessage("aqui", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }
                */
                /*
                if ((pVal.FormTypeEx.Equals("133")

                   //|| pVal.FormTypeEx.Equals("65213") || pVal.FormTypeEx.Equals("65214")  || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("940")
                   //|| pVal.FormTypeEx.Equals("182") || pVal.FormTypeEx.Equals("180")
                   ) && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK) && pVal.ActionSuccess && pVal.ItemUID == "1" &&  pVal.BeforeAction==false)
                {
                    try
                    {
                        Program.SboAplicacion.SetStatusBarMessage("Procesando registro FE. espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                        SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.Item(FormUID);
                        int DocEntry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));

                        if (DocEntry > 0)
                        {
                            // Aquí puedes utilizar el valor de docEntry
                            // Realiza las operaciones necesarias con el DocEntry obtenido
                        }
                        else
                        {
                            // No se pudo convertir el DocEntry a un valor entero
                            // Maneja el error adecuadamente
                        }
                    }
                    catch (Exception)
                    {

                     
                    }
                    BubbleEvent = true;
                    return;

                }
                */
                #endregion

                //reversion retencion
                if ((pVal.FormTypeEx.Equals("426"))
                   && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK) && pVal.Before_Action && pVal.ItemUID == "btnRever")
                {
                    var respDialog = Program.SboAplicacion.MessageBox("Desea anular la retención ?", 2, "Ok", "Cancel");
                    if (respDialog > 1)
                        return;

                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.ActiveForm;

                    string mensajeerror = "";
                    string messageSunatContent = "";
                    string nombreArchivo = "";
                    string pdf_url = "";
                    string xml_url = "";
                    string cdr_url = "";
                    bool respuestaanexo = false;

                    Program.SboAplicacion.SetStatusBarMessage("Consultando registro de comprobante. Espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                    int docentry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));
                    int ObjType = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("ObjType", 0));



                    #region RESUMEN ANULACION COMPROBANTE BIZLINKS
                    IntegradorBizLinks.EBizGenericInvokerClient oServicio21 = new IntegradorBizLinks.EBizGenericInvokerClient();
                    oServicio21.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["usuarioBizlinks"].ToString();
                    oServicio21.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["passwordBizlinks"].ToString(); ;
                    string documento = new RetencionDAO().ResumenAnulacion(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                    var respuesta = oServicio21.invoke(documento);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(respuesta.ToString());
                    string Status = "";
                    string mensajeError = "";
                    XmlNodeList documentNodes = xmlDoc.GetElementsByTagName("document");
                    foreach (XmlNode documentNode in documentNodes)
                    {
                        Status = documentNode.SelectSingleNode("status").InnerText;
                    }

                    #region si es Error
                    if (Status.ToUpper() == "ERROR")
                    {
                        XmlNodeList messagesNode = xmlDoc.GetElementsByTagName("messages");
                        foreach (XmlNode messagesNodes in messagesNode)
                        {
                            mensajeError += messagesNodes.SelectSingleNode("codeDetail").InnerText + " - " + messagesNodes.SelectSingleNode("descriptionDetail").InnerText + "\n";
                            // Add more fields as needed
                        }

                        if (mensajeError.ToUpper().Equals("EL DOCUMENTO YA FUE FIRMADO"))
                        {
                            //new FacturaDAO().ResumenAnulacionActualizar(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                        }
                        Program.SboAplicacion.MessageBox("Error : " + mensajeError);
                        GC.Collect();
                        return;
                    }
                    #endregion



                    #endregion

                }


                #region Envio Comprobante Electronico CPE
                if ((pVal.FormTypeEx.Equals("133")
                || pVal.FormTypeEx.Equals("65304") // BOELTA DE VENTA
                || pVal.FormTypeEx.Equals("65300") // FACTURA ANTICIPO
                || pVal.FormTypeEx.Equals("179") // NOTA DE CREDITO
                || pVal.FormTypeEx.Equals("65303") // NOTA DE DEBITO
                || pVal.FormTypeEx.Equals("426") // Retencion
                                                 //|| pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("65304") || pVal.FormTypeEx.Equals("65303") ||
                                                 //pVal.FormTypeEx.Equals("179") || pVal.FormTypeEx.Equals("65300") || pVal.FormTypeEx.Equals("65307")
                    )
                && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK) && pVal.Before_Action && pVal.ItemUID == "btnFE")
                {

                    var respDialog = Program.SboAplicacion.MessageBox("Desea enviar el comprobante a PSE ?", 2, "Ok", "Cancel");
                    if (respDialog > 1)
                        return;

                    Program.SboAplicacion.SetStatusBarMessage("Procesando registro FE. espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.Item(FormUID);
                    string FolioPrefijo = "";
                    string FolioNumero = "";
                    string numerosap = "";
                    string tipodocumento = "";
                    if (!pVal.FormTypeEx.Equals("426"))
                    {
                        FolioPrefijo = ((SAPbouiCOM.EditText)oform.Items.Item("208").Specific).Value.ToString();
                        FolioNumero = ((SAPbouiCOM.EditText)oform.Items.Item("211").Specific).Value.ToString();
                        numerosap = ((SAPbouiCOM.EditText)oform.Items.Item("8").Specific).Value.ToString();
                        tipodocumento = ((SAPbouiCOM.ComboBox)oform.Items.Item("120").Specific).Value.ToString();
                    }
                    else
                    {
                        FolioPrefijo = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_SERIE_FE", 0);
                        FolioNumero = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_NUM_FE", 0);
                        numerosap = oform.DataSources.DBDataSources.Item(0).GetValue("DocNum", 0);
                        tipodocumento = "99";
                    }

                    bool mregistrado = false;
                    int DocEntry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));
                    int ObjType = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("ObjType", 0));
                    string formulario = pVal.FormTypeEx;
                    string correlativo = FolioNumero.PadLeft(8, '0');
                    //string nombre_archivo = ConfigurationManager.AppSettings["RucEmisor"] + "-" + tipodocumento + "-" + FolioPrefijo + "-" + correlativo;


                    if (tipodocumento != "01" && tipodocumento != "03" && tipodocumento != "08" && tipodocumento != "07" && tipodocumento != "99")
                    {
                        Program.SboAplicacion.MessageBox("Pestaña Finanzas - Su Tipo Documento es " + tipodocumento);
                        return;
                    }

                    if (FolioPrefijo.Length == 0 || FolioNumero.Length == 0)
                    {
                        Program.SboAplicacion.MessageBox("Asigne primero Folio al documento");
                        return;
                    }



                    // FACTURACION_21.SRVSoapClient oServicio21 = new FACTURACION_21.SRVSoapClient();

                    string tramaZipCDR = "";
                    bool rptaRegistrar = RegistrarDocumento(Convert.ToInt32(numerosap), ObjType, tipodocumento, ref mregistrado, true, DocEntry, ref tramaZipCDR);
                    Program.SboAplicacion.SetStatusBarMessage("Se termino el proceso de registro", SAPbouiCOM.BoMessageTime.bmt_Short, false);


                }


                #endregion

                #region Botones Guia [ODNL]
                if ((pVal.FormTypeEx.Equals("140")
                    || pVal.FormTypeEx.Equals("940")
                     || pVal.FormTypeEx.Equals("720")
                      || pVal.FormTypeEx.Equals("182")
                    //|| pVal.FormTypeEx.Equals("65213") || pVal.FormTypeEx.Equals("65214")  || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("940")
                    //|| pVal.FormTypeEx.Equals("182") || pVal.FormTypeEx.Equals("180")
                    ) && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) && pVal.Before_Action)
                {
                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.Item(FormUID);

                    #region Boton Registrar SUNAT
                    SAPbouiCOM.Item oitem = oform.Items.Add("btnFEG", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(1).Top;
                    oitem.Left = oform.Items.Item(2).Left + oform.Items.Item(2).Left + 5;

                    if (pVal.FormTypeEx.Equals("940"))
                    {
                        oitem.Left =oform.Items.Item(2).Left + 5;
                    }
                    if (pVal.FormTypeEx.Equals("720"))
                    {
                        oitem.Left = oform.Items.Item(2).Left + 40;
                    }

                    oitem.Width = "ENVIAR PSE".Length * 8;
                    oitem.Height = oform.Items.Item(1).Height;
                    (oitem.Specific as SAPbouiCOM.Button).Caption = "ENVIAR PSE";
                    #endregion

                    #region Boton Consultar
                    oitem = oform.Items.Add("btnFE6", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(1).Top;
                    oitem.Left = oform.Items.Item(2).Left + oform.Items.Item(2).Left + ("ENVIAR PSE".Length * 8) + 10;
                    if (pVal.FormTypeEx.Equals("940") )
                    {
                        oitem.Left =oform.Items.Item(2).Left + ("ENVIAR PSE".Length * 8) + 10;
                    }
                    if (pVal.FormTypeEx.Equals("720"))
                    {
                        oitem.Left = oform.Items.Item(2).Left + ("ENVIAR PSE".Length * 8) +45;
                    }
                    oitem.Width = "CONSULTAR".Length * 8;
                    oitem.Height = oform.Items.Item(1).Height;
                    (oitem.Specific as SAPbouiCOM.Button).Caption = "CONSULTAR";
                    #endregion

                    #region AsingnarFolio
                    oitem = oform.Items.Add("btnFEG5", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(10).Top + +((oform.Items.Item(1).Height) * 5) + 10;
                    if (pVal.FormTypeEx.Equals("940") )
                    {
                        oitem.Top = oform.Items.Item(16).Top + +((oform.Items.Item(1).Height) * 2) + 10;
                    }

                    if (pVal.FormTypeEx.Equals("720"))
                    {
                        oitem.Top = oform.Items.Item(7).Top + +((oform.Items.Item(1).Height) * 4) + 10;
                    }
                    oitem.Left = oform.Items.Item(10).Left + 20;
                    oitem.Width = "Asignar Folio".Length * 6;
                    oitem.Height = oform.Items.Item(1).Height;
                    (oitem.Specific as SAPbouiCOM.Button).Caption = "Asignar Folio";
                    #endregion

                }
                #endregion

                #region Botones Comprobantes de Pago CPE - RETENCION
                if ((pVal.FormTypeEx.Equals("426")

                    //|| pVal.FormTypeEx.Equals("65213") || pVal.FormTypeEx.Equals("65214")  || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("940")
                    //|| pVal.FormTypeEx.Equals("182") || pVal.FormTypeEx.Equals("180")
                    ) && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) && pVal.Before_Action)
                {
                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.Item(FormUID);

                    #region Boton Registrar SUNAT
                    SAPbouiCOM.Item oitem = oform.Items.Add("btnFE", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(1).Top;
                    oitem.Left = oform.Items.Item(1).Left + oform.Items.Item(1).Left + 5;
                    oitem.Height = oform.Items.Item(1).Height;
                    //oitem.Width = oform.Items.Item(1).Width - 1 ;
                    oitem.Width = 65;
                    (oitem.Specific as SAPbouiCOM.Button).Caption = "Envio PSE";
                    #endregion


                    SAPbouiCOM.Item itemRever = oform.Items.Add("btnRever", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    itemRever.Top = oform.Items.Item(95).Top;
                    itemRever.Left = oform.Items.Item(7).Left + 350;
                    itemRever.Width = 100;
                    (itemRever.Specific as SAPbouiCOM.Button).Caption = "Anular/Reversion";



                    #region Boton GenerarPDF [BIZLINK NO CUENTA CON PREVIA EN GUIA]
                    //oitem = oform.Items.Add("btnFE4", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    //oitem.Top = oform.Items.Item(1).Top;
                    //oitem.Left = oform.Items.Item(84).Left - 25;
                    //oitem.Width = 65;
                    //(oitem.Specific as SAPbouiCOM.Button).Caption = "Generar PDF";
                    #endregion


                    #region AsingnarFolio
                    if (!pVal.FormTypeEx.Equals("426"))
                    {
                        oitem = oform.Items.Add("btnFE5", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                        oitem.Top = oform.Items.Item(90).Top + 60;
                        oitem.Left = oform.Items.Item(84).Left;
                        oitem.Width = 67;
                        (oitem.Specific as SAPbouiCOM.Button).Caption = "Asignar Folio";
                    }
                    #endregion

                    #region Quitar/Asignar Folio
                    if (!pVal.FormTypeEx.Equals("426"))
                    {
                        oitem = oform.Items.Add("btnFE55", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                        oitem.Top = oform.Items.Item(90).Top + 60;
                        oitem.Left = oform.Items.Item(84).Left + 75;
                        oitem.Width = 62;
                        (oitem.Specific as SAPbouiCOM.Button).Caption = "Quitar/Asignar Folio";
                    }
                    #endregion

                    #region Boton Consultar
                    oitem = oform.Items.Add("btnFE6", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(1).Top;
                    oitem.Left = oform.Items.Item(1).Left + oform.Items.Item(1).Left + 75;
                    oitem.Width = 50;
                    oitem.Height = oform.Items.Item(1).Height;
                    (oitem.Specific as SAPbouiCOM.Button).Caption = "Consultar";
                    #endregion
                }
                #endregion

                #region Botones Comprobantes de Pago CPE
                if ((pVal.FormTypeEx.Equals("133") // FACTURA DEUDORES
                    || pVal.FormTypeEx.Equals("65300") // FACTURA ANTICIPO
                    || pVal.FormTypeEx.Equals("179") // NOTA DE CREDITO
                    || pVal.FormTypeEx.Equals("65303") // NOTA DE DEBITO
                    || pVal.FormTypeEx.Equals("65304") // NOTA DE DEBITO
                                                       //|| pVal.FormTypeEx.Equals("65213") || pVal.FormTypeEx.Equals("65214")  || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("940")
                                                       //|| pVal.FormTypeEx.Equals("182") || pVal.FormTypeEx.Equals("180")
                    ) && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) && pVal.Before_Action)
                {
                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.Item(FormUID);

                   

                    #region Boton Registrar SUNAT
                    SAPbouiCOM.Item oitem = oform.Items.Add("btnFE", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(1).Top;
                    oitem.Left = oform.Items.Item(2).Left + oform.Items.Item(2).Left + 5;
                    oitem.Width = "ENVIAR PSE".Length * 8;
                    oitem.Height = oform.Items.Item(1).Height;//12:22 14:27


                    (oitem.Specific as SAPbouiCOM.Button).Caption = "ENVIAR PSE";
                    #endregion

                    #region Boton Consultar
                    oitem = oform.Items.Add("btnFE6", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(1).Top;
                    oitem.Left = oform.Items.Item(2).Left + oform.Items.Item(2).Left + ("ENVIAR PSE".Length * 8) + 8;
                    oitem.Width = "Consultar".Length * 8;
                    oitem.Height = oform.Items.Item(1).Height;
                    (oitem.Specific as SAPbouiCOM.Button).Caption = "Consultar";
                    #endregion

                    #region AsingnarFolio
                    oitem = oform.Items.Add("btnFE5", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(10).Top + ((oform.Items.Item(1).Height) * 5) + 10;
                    oitem.Left = oform.Items.Item(10).Left + 5;
                    oitem.Width = "Asignar Folio".Length * 6;
                    oitem.Height = oform.Items.Item(1).Height;
                    (oitem.Specific as SAPbouiCOM.Button).Caption = "Asignar Folio";
                    #endregion





                    #region Quitar/Asignar Folio
                    oitem = oform.Items.Add("btnFE55", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    oitem.Top = oform.Items.Item(10).Top + ((oform.Items.Item(1).Height) * 5) + 10;
                    oitem.Left = oform.Items.Item(10).Left + 8 + ("Asignar Folio".Length * 6);
                    oitem.Width = "Quitar/Asignar Folio".Length * 5;
                    oitem.Height = oform.Items.Item(1).Height;
                    (oitem.Specific as SAPbouiCOM.Button).Caption = "Quitar/Asignar Folio";
                    #endregion



                    #region Anulacion de Comprobantes btnFE8
                    SAPbobsCOM.Recordset oRecordSet;
                    oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    oRecordSet.DoQuery("SELECT U_SMC_ANU_FE FROM OUSR WHERE USER_CODE = '" + oCompany.UserName + "'");
                    if (oRecordSet.Fields.Item(0).Value.ToString().Equals("Y"))
                    {
                        oitem = oform.Items.Add("btnFE8", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                        oitem.Top = oform.Items.Item(10).Top + ((oform.Items.Item(1).Height) * 5) + 10;
                        oitem.Left = oform.Items.Item(10).Left + ("Asignar Folio".Length * 6) + ("Quitar/Asignar Folio".Length * 6);
                        oitem.Height = oform.Items.Item(1).Height;
                        (oitem.Specific as SAPbouiCOM.Button).Caption = "Anular F.E";
                    }
                    #endregion

                    #region Boton GenerarPDF [BIZLINK NO CUENTA CON PREVIA EN GUIA]
                    //oitem = oform.Items.Add("btnFE4", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    //oitem.Top = oform.Items.Item(1).Top;
                    //oitem.Left = oform.Items.Item(84).Left - 25;
                    //oitem.Width = 65;
                    //(oitem.Specific as SAPbouiCOM.Button).Caption = "Generar PDF";
                    #endregion

                }
                #endregion

                #region Asignar Folio CPE
                if ((pVal.FormTypeEx.Equals("133")
                   || pVal.FormTypeEx.Equals("65300") // FACTURA ANTICIPO
                     || pVal.FormTypeEx.Equals("65304") // BOLETA DE VENTA
                    || pVal.FormTypeEx.Equals("179") // NOTA DE CREDITO
                    || pVal.FormTypeEx.Equals("65303") // NOTA DE DEBITO
                                                       //|| pVal.FormTypeEx.Equals("940") || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("182")
                                                       //|| pVal.FormTypeEx.Equals("180") || pVal.FormTypeEx.Equals("720")
                    )
                    && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK) && pVal.Before_Action && pVal.ItemUID == "btnFE5")
                {
                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.ActiveForm;
                    string numerosap = "";
                    string tipodocumento = "";
                    String numero_fe = "";
                    String serie_fe = "";
                    bool verfolio = true;

                    Program.SboAplicacion.SetStatusBarMessage("Asignando Folio FE. espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                    int DocEntry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));
                    int ObjType = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("ObjType", 0));
                    numerosap = ((SAPbouiCOM.EditText)oform.Items.Item("8").Specific).Value.ToString();
                    tipodocumento = ((SAPbouiCOM.ComboBox)oform.Items.Item("120").Specific).Value.ToString();
                    string Formulario = pVal.FormTypeEx;

                    #region Validar Existencia de Folio 
                    ////Factura Anticipo
                    if (Formulario == "65300")
                    {
                        if (tipodocumento == "01" || tipodocumento == "03" || tipodocumento == "07" || tipodocumento == "08")
                        {

                            verfolio = ValidarExistenciaFolioFactAnt(DocEntry, tipodocumento, Convert.ToInt32(numerosap), ObjType);
                        }
                    }
                    else
                    {
                        if (tipodocumento == "01" || tipodocumento == "03" || tipodocumento == "08")
                        {

                            verfolio = ValidarExistenciaFolioOINV(DocEntry, tipodocumento, Convert.ToInt32(numerosap), ObjType);
                        }
                        if (tipodocumento == "07")
                        {

                            verfolio = ValidarExistenciaFolioORIN(DocEntry, tipodocumento, Convert.ToInt32(numerosap), ObjType);
                        }
                    }
                    #endregion

                    #region Si no existe Folio
                    if (verfolio == false)
                    {
                        bool actfolio = ActualizarFolioSAP(DocEntry, Convert.ToInt32(numerosap), ObjType, tipodocumento, oCompany);

                        SAPbouiCOM.BoFormObjectEnum odocumentosap = new SAPbouiCOM.BoFormObjectEnum();

                        if (ObjType == 203)
                        {
                            odocumentosap = (SAPbouiCOM.BoFormObjectEnum)203;
                        }

                        if (ObjType == 13)
                        {
                            odocumentosap = SAPbouiCOM.BoFormObjectEnum.fo_Invoice;
                        }

                        if (ObjType == 14)
                        {
                            odocumentosap = SAPbouiCOM.BoFormObjectEnum.fo_InvoiceCreditMemo;
                        }

                        oform.Close();
                        Program.SboAplicacion.OpenForm(odocumentosap, "", DocEntry.ToString());
                    }
                    #endregion



                }
                #endregion

                #region Anulacion de comprobante
                if ((pVal.FormTypeEx.Equals("133")
                   || pVal.FormTypeEx.Equals("65300") // FACTURA ANTICIPO
                    || pVal.FormTypeEx.Equals("179") // NOTA DE CREDITO
                    || pVal.FormTypeEx.Equals("65303") // NOTA DE DEBITO
                                                       //|| pVal.FormTypeEx.Equals("940") || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("182")
                                                       //|| pVal.FormTypeEx.Equals("180") || pVal.FormTypeEx.Equals("720")
                    )
                    && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK) && pVal.Before_Action && pVal.ItemUID == "btnFE8")
                {

                    Program.SboAplicacion.SetStatusBarMessage("Proceso de anulacion de comprobante. espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                    var respDialog = Program.SboAplicacion.MessageBox("Desea anular el comprobante ?", 2, "Ok", "Cancel");
                    if (respDialog > 1)
                        return;
                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.ActiveForm;


                    string mensajeerror = "";
                    string messageSunatContent = "";
                    string nombreArchivo = "";
                    string pdf_url = "";
                    string xml_url = "";
                    string cdr_url = "";
                    bool respuestaanexo = false;

                    Program.SboAplicacion.SetStatusBarMessage("Consultando registro de comprobante. Espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);



                    int docentry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));
                    int ObjType = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("ObjType", 0)); ;





                    #region RESUMEN ANULACION COMPROBANTE BIZLINKS
                    IntegradorBizLinks.EBizGenericInvokerClient oServicio21 = new IntegradorBizLinks.EBizGenericInvokerClient();
                    oServicio21.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["usuarioBizlinks"].ToString();
                    oServicio21.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["passwordBizlinks"].ToString(); ;
                    string documento = new FacturaDAO().ResumenAnulacion(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                    var respuesta = oServicio21.invoke(documento);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(respuesta.ToString());
                    string Status = "";
                    string mensajeError = "";
                    XmlNodeList documentNodes = xmlDoc.GetElementsByTagName("document");
                    foreach (XmlNode documentNode in documentNodes)
                    {
                        Status = documentNode.SelectSingleNode("status").InnerText;
                    }

                    #region si es Error
                    if (Status.ToUpper() == "ERROR")
                    {
                        XmlNodeList messagesNode = xmlDoc.GetElementsByTagName("messages");
                        foreach (XmlNode messagesNodes in messagesNode)
                        {
                            mensajeError += messagesNodes.SelectSingleNode("codeDetail").InnerText + " - " + messagesNodes.SelectSingleNode("descriptionDetail").InnerText + "\n";
                            // Add more fields as needed
                        }

                        if (mensajeError.ToUpper().Equals("EL DOCUMENTO YA FUE FIRMADO"))
                        {
                            new FacturaDAO().ResumenAnulacionActualizar(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                        }
                        Program.SboAplicacion.MessageBox("Error : " + mensajeError);
                        GC.Collect();
                        return;
                    }
                    #endregion



                    #endregion

                }
                #endregion

                #region Asignar Folio GRE
                if ((pVal.FormTypeEx.Equals("140") || pVal.FormTypeEx.Equals("940") || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("182")
                    || pVal.FormTypeEx.Equals("180") || pVal.FormTypeEx.Equals("720"))
                    && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK) && pVal.Before_Action && pVal.ItemUID == "btnFEG5")
                {
                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.ActiveForm;
                    string numerosap = "";
                    string tipodocumento = "";
                    string formulario = pVal.FormTypeEx;
                    string FolioPref="";
                    string FolioNum = "";
                    string existeFolio = "";
                    String numero_fe = "";
                    String serie_fe = "";

                    existeFolio = oform.DataSources.DBDataSources.Item(0).GetValue("FolioPref", 0);
                    int DocEntry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));
                    int ObjType = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("ObjType", 0));
                    serie_fe = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_SERIE_GRE", 0);
                    numero_fe = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_NUM_GRE", 0);
                    if (!(serie_fe=="0" && numero_fe == "0"))
                    {
                        if (existeFolio.Length > 0 )
                        {
                            if (serie_fe == "T001")
                            {
                                if (!(int.TryParse(numero_fe, out int numero)))
                                {
                                    GC.Collect();
                                    Program.SboAplicacion.MessageBox("No se puede asignar Folio, el documento se encuentra foliado");
                                    return;
                                }
                            }

                          
                        }
                    }
                    

                    if (pVal.FormTypeEx.Equals("140"))
                    {
                        numerosap = ((SAPbouiCOM.EditText)oform.Items.Item("8").Specific).Value.ToString();
                        tipodocumento = ((SAPbouiCOM.ComboBox)oform.Items.Item("120").Specific).Value.ToString();
                        if (tipodocumento != "09")
                        {
                            GC.Collect();
                            Program.SboAplicacion.MessageBox("Para asignar Folio,debe seleccionar el tipo de documento 09");
                            return;
                        }

                    }else if (pVal.FormTypeEx.Equals("182"))
                    {
                        numerosap = ((SAPbouiCOM.EditText)oform.Items.Item("8").Specific).Value.ToString();
                        tipodocumento = ((SAPbouiCOM.ComboBox)oform.Items.Item("120").Specific).Value.ToString();
                        if (tipodocumento != "09")
                        {
                            GC.Collect();
                            Program.SboAplicacion.MessageBox("Para asignar Folio,debe seleccionar el tipo de documento 09");
                            return;
                        }
                    }
                    else
                    {
                        numerosap = ((SAPbouiCOM.EditText)oform.Items.Item("11").Specific).Value.ToString();
                        tipodocumento = "09";
                    }


                    if (tipodocumento == "09")
                    {
                        try
                        {
                           
                            MetodoGuiaDAO oMetodoGuiaDAO = new MetodoGuiaDAO();
                            oMetodoGuiaDAO.Update_Folio_Guia(DocEntry, ObjType, tipodocumento, ref FolioPref, ref FolioNum, oCompany);

                            dynamic odoc=null;
                            switch (ObjType)
                            {
                                case 15:
                                    odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
                                    break;
                                case 67:
                                    odoc = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);
                                    break;
                                case 60:
                                    odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
                                    break;
                                case 21:
                                    odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns);
                                    break;
                            }
                            odoc.GetByKey(Convert.ToInt32(DocEntry));
                            odoc.FolioPrefixString = FolioPref;
                            odoc.FolioNumber = Convert.ToInt32(FolioNum);
                            if (serie_fe == "0" && numero_fe == "0")
                            {
                                odoc.FolioPrefixString = "";
                                odoc.FolioNumber = 0;
                            }

                            if (serie_fe == "T001")
                            {
                                if (int.TryParse(numero_fe, out int numero))
                                {
                                    odoc.FolioPrefixString = serie_fe;
                                    odoc.FolioNumber = Convert.ToInt32(numero_fe); ;
                                }
                            }



                            int res = odoc.Update();

                            int temp_int = res;
                            string temp_string = "";
                            oCompany.GetLastError(out temp_int, out temp_string);

                            if (res > -1)
                            {
                                
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(odoc);
                                odoc = null;
                                GC.Collect();
                                //Update_CorrelativoSAP(tipodocumento, Convert.ToInt32(docnum), Convert.ToInt32(objectype));
                                //MessageBox.Show("actualizado");
                            }
                            else
                            {
                                Program.SboAplicacion.SetStatusBarMessage(temp_string, SAPbouiCOM.BoMessageTime.bmt_Short, true);
                                //MessageBox.Show(temp_string); roche
                            }
                            oform.Close();
                            Program.SboAplicacion.
                            OpenForm((SAPbouiCOM.BoFormObjectEnum)ObjType, "", DocEntry.ToString());

                        }
                        catch (Exception ex)
                        {
                            Program.SboAplicacion.SetStatusBarMessage("Verifique " + ex, SAPbouiCOM.BoMessageTime.bmt_Short, false);
                            return;
                        }
                    }

                }
                #endregion

                #region Quitar/Asignar Folio CPE
                if ((pVal.FormTypeEx.Equals("133")  // FACTURA DEUDORES
                    || pVal.FormTypeEx.Equals("65300") // FACTURA ANTICIPO
                    || pVal.FormTypeEx.Equals("65304") // BOLETA DE VENTA
                    || pVal.FormTypeEx.Equals("179") // NOTA DE CREDITO
                    || pVal.FormTypeEx.Equals("65303") // NOTA DE DEBITO
                                                       //|| pVal.FormTypeEx.Equals("60091") || pVal.FormTypeEx.Equals("65304") || pVal.FormTypeEx.Equals("65303")
                                                       //|| pVal.FormTypeEx.Equals("179") || pVal.FormTypeEx.Equals("65300") || pVal.FormTypeEx.Equals("140")
                    ) && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK)
                    && pVal.Before_Action && pVal.ItemUID == "btnFE55")
                {
                    Program.SboAplicacion.SetStatusBarMessage("Asignando Folio FE. espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.Item(FormUID);
                    SAPbouiCOM.Form oformActual = Program.SboAplicacion.Forms.ActiveForm;
                    SAPbouiCOM.Form oformCamposdeUsuario = Program.SboAplicacion.Forms.Item(oformActual.UDFFormUID);

                    int DocEntry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));
                    int ObjType = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("ObjType", 0));
                    String numerosap = ((SAPbouiCOM.EditText)oform.Items.Item("8").Specific).Value.ToString();
                    String tipodocumento = ((SAPbouiCOM.ComboBox)oform.Items.Item("120").Specific).Value.ToString();
                    String formulario = pVal.FormTypeEx;
                    string usuario = oCompany.UserSignature.ToString();
                    String serie = "";
                    Int32 numero = 0;
                    Program.SboAplicacion.SetStatusBarMessage("Asignando Folio FE. espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                    string estadocpe = oformCamposdeUsuario.DataSources.DBDataSources.Item(0).GetValue("U_SMC_ESTADO_FE", 0).ToString();


                    if (estadocpe.Trim() == "02")
                    {
                        Program.SboAplicacion.MessageBox("No se puede quitar Folio a un Documento Aceptado en SUNAT");
                        return;
                    }


                    String referencia = ((SAPbouiCOM.EditText)oform.Items.Item("14").Specific).Value.ToString();
                    if (referencia != "")
                    {
                        String[] array = referencia.Split((new Char[] { '-' }), StringSplitOptions.RemoveEmptyEntries);
                        serie = array[0];
                        numero = Convert.ToInt32(array[1]);
                        bool verfolio = true;
                    }
                    else
                    {
                        serie = "";
                        numero = 0;
                    }



                    string temp = "";
                    bool actfolio = ActualizarFolioSAP2019(DocEntry, ObjType, tipodocumento, serie, numero, oCompany, ref temp);
                    if (temp != "")
                    {
                        Program.SboAplicacion.SetStatusBarMessage(temp, SAPbouiCOM.BoMessageTime.bmt_Medium, false);
                        return;
                    }

                    SAPbouiCOM.BoFormObjectEnum odocumentosap = new SAPbouiCOM.BoFormObjectEnum();

                    if (ObjType == 203) { odocumentosap = (SAPbouiCOM.BoFormObjectEnum)203; }
                    if (ObjType == 13) { odocumentosap = SAPbouiCOM.BoFormObjectEnum.fo_Invoice; }
                    if (ObjType == 14) { odocumentosap = SAPbouiCOM.BoFormObjectEnum.fo_InvoiceCreditMemo; }

                    oform.Close();
                    Program.SboAplicacion.OpenForm(odocumentosap, "", DocEntry.ToString());

                }
                #endregion

                #region Consultar Guia Electronica - CPE
                if ((pVal.FormTypeEx.Equals("140")
                     || pVal.FormTypeEx.Equals("940") //TRANSFERENCIA STOCK
                     || pVal.FormTypeEx.Equals("179")
                     || pVal.FormTypeEx.Equals("182")
                     || pVal.FormTypeEx.Equals("133")
                     || pVal.FormTypeEx.Equals("720")
                     || pVal.FormTypeEx.Equals("65300")
                     || pVal.FormTypeEx.Equals("65303")
                     || pVal.FormTypeEx.Equals("65304")
                     || pVal.FormTypeEx.Equals("426") // Retencion// || pVal.FormTypeEx.Equals("65214") || pVal.FormTypeEx.Equals("940") || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("142")
                     //|| pVal.FormTypeEx.Equals("182") || pVal.FormTypeEx.Equals("180")
                    )
                    && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK) && pVal.Before_Action && pVal.ItemUID == "btnFE6")
                {
                    string mensajeerror = "";
                    string messageSunatContent = "";
                    string nombreArchivo = "";
                    string pdf_url = "";
                    string xml_url = "";
                    string cdr_url = "";
                    bool respuestaanexo = false;

                    Program.SboAplicacion.SetStatusBarMessage("Consultando registro de comprobante. Espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.Item(FormUID);

                    int docentry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));
                    int ObjType = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("ObjType", 0)); ;





                    #region CONSULTAR COMPROBANTE BIZLINKS
                    IntegradorBizLinks.EBizGenericInvokerClient oServicio21 = new IntegradorBizLinks.EBizGenericInvokerClient();
                    oServicio21.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["usuarioBizlinks"].ToString();
                    oServicio21.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["passwordBizlinks"].ToString(); ;
                    string documento = "";
                    switch (ObjType)
                    {
                        case 67:
                            documento = new MetodosGuia().ObtenerStringConsultarGuia(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                            break;
                        case 21:
                            documento = new MetodosGuia().ObtenerStringConsultarGuia(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                            break;
                        case 60:
                            documento = new MetodosGuia().ObtenerStringConsultarGuia(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                            break;
                        case 15:
                            documento = new MetodosGuia().ObtenerStringConsultarGuia(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                            break;
                        case 203:
                            documento = new FacturaDAO().ObtenerStringCPE(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                            break;
                        case 14:
                            documento = new NotaCreditoDAO().ObtenerStringCPE(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                            break;
                        case 13:
                            documento = new FacturaDAO().ObtenerStringCPE(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                            break;

                        case 46:
                            documento = new RetencionDAO().ObtenerStringCPE(docentry, Convert.ToInt32(pVal.FormTypeEx), ObjType, oCompany, ref mensajeerror);
                            break;
                        // ... additional cases
                        default:
                            // Code to execute if expression doesn't match any case
                            break;
                    }
                   
                  
                    var respuesta = oServicio21.invoke(documento);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(respuesta.ToString());
                    // Obtener el valor de statusSunat
                    XmlNode statusSunatNode = xmlDoc.SelectSingleNode("//statusSunat");
                    XmlNodeList documentNodes = xmlDoc.GetElementsByTagName("document");
                    string statusSunatValue = "";
                    try
                    {
                        statusSunatValue = statusSunatNode.InnerText;
                    }
                    catch (Exception)
                    {
                        statusSunatValue = "PE_02";
                    }

                    // Obtener el elemento messageSunat
                    XmlNode messageSunatNode = xmlDoc.SelectSingleNode("//messageSunat");
                    // Verificar si el elemento messageSunat existe
                    if (messageSunatNode != null)
                    {
                        // Obtener el contenido del elemento messageSunat
                        messageSunatContent = messageSunatNode.InnerText;
                        if (statusSunatValue == "AC_03")
                        {
                            if (ObjType==46)
                            {
                                Program.SboAplicacion.MessageBox("SU RETENCION SE ENCUENTRA ACEPTADA");
                            }
                            else
                            {
                                Program.SboAplicacion.MessageBox("SU FACTURA SE ENCUENTRA ACEPTADA");
                            }
                            
                        }
                        else
                        {
                            Program.SboAplicacion.MessageBox(messageSunatContent);
                        }
                    }

                    #region Anexar Datos Comprobantes
                    if (statusSunatValue == "AC_03" || ConfigurationManager.AppSettings["descargardocumentoserror"].ToString() == "Y")
                    {
                        foreach (XmlNode documentNode in documentNodes)
                        {
                            nombreArchivo = documentNode.SelectSingleNode("numeroDocumentoEmisor").InnerText + "-" + documentNode.SelectSingleNode("typeDocument").InnerText + "-" + documentNode.SelectSingleNode("idDocument").InnerText;
                            pdf_url = documentNode.SelectSingleNode("pdfFileUrl").InnerText;
                            xml_url = documentNode.SelectSingleNode("xmlFileSignUrl").InnerText;
                            cdr_url = documentNode.SelectSingleNode("xmlFileSunatUrl").InnerText;
                            // Add more fields as needed
                        }

                        switch (ObjType)
                        {
                            case 15:
                                respuestaanexo = new MetodosGuia().AnexarDocumentos(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref mensajeerror);
                                break;
                            case 67:
                                respuestaanexo = new MetodosGuia().AnexarDocumentos(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref mensajeerror);
                                break;
                            case 60:
                                respuestaanexo = new MetodosGuia().AnexarDocumentos(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref mensajeerror);
                                break;
                            case 13:
                                respuestaanexo = AnexarDocumentosCpe(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref mensajeerror);
                                break;
                            case 203:
                                respuestaanexo = AnexarDocumentosCpe(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref mensajeerror);
                                break;
                            case 14:
                                respuestaanexo = AnexarDocumentosCpe(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref mensajeerror);
                                break;
                            case 46:
                                respuestaanexo = AnexarDocumentosCpe(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref mensajeerror);
                                break;
                            default:
                                break;
                        }

                    }
                    #endregion

                    if (messageSunatContent.Length > 150)
                    {
                        messageSunatContent = messageSunatContent.Substring(0, 150);
                    }

                    new FacturaDAO().SMC_ActualizarEstadoFE(ObjType, docentry, messageSunatContent, statusSunatValue, oCompany);



                    #endregion

                    #region Cerrar y Abrir Formulario
                    SAPbouiCOM.BoFormObjectEnum odocumentosap = new SAPbouiCOM.BoFormObjectEnum();

                    if (ObjType == 203)
                    {
                        odocumentosap = (SAPbouiCOM.BoFormObjectEnum)203;
                    }

                    if (ObjType == 13)
                    {
                        odocumentosap = SAPbouiCOM.BoFormObjectEnum.fo_Invoice;
                    }

                    if (ObjType == 14)
                    {
                        odocumentosap = SAPbouiCOM.BoFormObjectEnum.fo_InvoiceCreditMemo;
                    }


                    if (ObjType == 15)
                    {
                        odocumentosap = (SAPbouiCOM.BoFormObjectEnum)ObjType;
                    }

                    if (ObjType == 46)
                    {
                        odocumentosap = (SAPbouiCOM.BoFormObjectEnum)ObjType;
                    }


                    if (ObjType == 67)
                    {
                        odocumentosap = SAPbouiCOM.BoFormObjectEnum.fo_StockTransfers;
                    }
                    if (ObjType == 60)
                    {
                        odocumentosap = (SAPbouiCOM.BoFormObjectEnum)ObjType;
                    }
                    if (ObjType == 21)
                    {
                        odocumentosap = (SAPbouiCOM.BoFormObjectEnum)ObjType;
                    }

                    oform.Close();
                    Program.SboAplicacion.OpenForm(odocumentosap, "", docentry.ToString());
                    #endregion




                    //bool respuestaanexo = new MetodosGuia().AnexarDocumentos(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref MENSAJE);
                    //oform.Close();
                    //Program.SboAplicacion.
                    //OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_DeliveryNotes, "", docentry.ToString());
                    //Program.SboAplicacion.MessageBox("Se registro la GUIA REMISION correctamente");
                    return;




                }
                #endregion

                #region Registrar Guia Electronica
                //REGISTRAR GUIA
                if ((pVal.FormTypeEx.Equals("140") // || pVal.FormTypeEx.Equals("65214") || pVal.FormTypeEx.Equals("940") || pVal.FormTypeEx.Equals("143") || pVal.FormTypeEx.Equals("142")
                      || pVal.FormTypeEx.Equals("940")   //|| pVal.FormTypeEx.Equals("182") || pVal.FormTypeEx.Equals("180")
                      || pVal.FormTypeEx.Equals("720")
                        || pVal.FormTypeEx.Equals("182")

                    )
                    && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK) && pVal.Before_Action && pVal.ItemUID == "btnFEG")
                {

                    Program.SboAplicacion.SetStatusBarMessage("Procesando registro GRE. espere por favor...", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                    SAPbouiCOM.Form oform = Program.SboAplicacion.Forms.Item(FormUID);
                    string cardCode = "";
                    int enviado = 0;
                    int docentry = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0));
                    string bultos = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_BULTOS_GRE", 0);
                    string motivo = oform.DataSources.DBDataSources.Item(0).GetValue("U_EXX_MOTIVTRA", 0);
                    string modalidad = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_MOD_TRAS", 0);
                    string serie = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_SERIE_GRE", 0);
                    string correlativo = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_NUM_GRE", 0);

                    string serie_fe = oform.DataSources.DBDataSources.Item(0).GetValue("FolioPref", 0);
                    string correlativo_fe = oform.DataSources.DBDataSources.Item(0).GetValue("FolioNum", 0);
                    int ObjType = Convert.ToInt32(oform.DataSources.DBDataSources.Item(0).GetValue("ObjType", 0)); 


                    if (serie_fe == "" || correlativo_fe == "")
                    {
                        Program.SboAplicacion.MessageBox("NECESITA COLOCAR LA SERIE Y EL CORRELATIVO");
                        return;
                    }


                    if (bultos == "")
                    {
                        Program.SboAplicacion.MessageBox("FALTA LLENAR EL CAMPO DE USUARIO 'GRE N. BULTOS'");
                        return;
                    }
                    if (motivo == "")
                    {
                        Program.SboAplicacion.MessageBox("FALTA LLENAR EL CAMPO DE USUARIO 'GRE MOTIVO TRASLADO'");
                        return;
                    }
                    if (modalidad == "")
                    {
                        Program.SboAplicacion.MessageBox("FALTA LLENAR EL CAMPO DE USUARIO 'GRE MODALIDAD TRASLADO'");
                        return;
                    }



                    string EstadoFE = oform.DataSources.DBDataSources.Item(0).GetValue("U_SMC_ESTADO_GRE", 0);
                    if (EstadoFE == "02" || EstadoFE == "03")
                    {
                        Program.SboAplicacion.MessageBox("EL COMPROBANTE ELECTRONICO, SE ENCUENTRA ACEPTADO.");
                        return;
                    }

                    #region ENVIAR COMPROBANTE BIZLINKS
                    string MENSAJE = "";
                    string Status = ""; //Estados de la guia
                    string mensajeError = "";
                    string nombreArchivo = "";
                    string pdf_url = "";
                    string xml_url = "";
                    string cdr_url = "";


                    IntegradorBizLinks.EBizGenericInvokerClient oServicio21 = new IntegradorBizLinks.EBizGenericInvokerClient();
                    oServicio21.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["usuarioBizlinks"].ToString();
                    oServicio21.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["passwordBizlinks"].ToString(); ;
                    string documentoguia = new MetodosGuia().obtenerGuiaRemision(docentry, ref MENSAJE, Convert.ToInt32(pVal.FormTypeEx), oCompany);
                    var respuesta = oServicio21.invoke(documentoguia);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(respuesta);
                    XmlNodeList documentNodes = xmlDoc.GetElementsByTagName("document");
                    foreach (XmlNode documentNode in documentNodes)
                    {
                        Status = documentNode.SelectSingleNode("status").InnerText;
                    }

                    #region si es Error
                    if (Status.ToUpper() == "ERROR")
                    {
                        XmlNodeList messagesNode = xmlDoc.GetElementsByTagName("messages");
                        foreach (XmlNode messagesNodes in messagesNode)
                        {
                            mensajeError += messagesNodes.SelectSingleNode("codeDetail").InnerText + " - " + messagesNodes.SelectSingleNode("descriptionDetail").InnerText + "\n";
                            // Add more fields as needed
                        }
                        Program.SboAplicacion.MessageBox("Error : " + mensajeError);
                        GC.Collect();
                        return;
                    }

                    #endregion


                    foreach (XmlNode documentNode in documentNodes)
                    {
                        nombreArchivo = documentNode.SelectSingleNode("numeroDocumentoEmisor").InnerText + "-" + documentNode.SelectSingleNode("typeDocument").InnerText + "-" + documentNode.SelectSingleNode("idDocument").InnerText;
                        pdf_url = documentNode.SelectSingleNode("pdfFileUrl").InnerText;
                        xml_url = documentNode.SelectSingleNode("xmlFileSignUrl").InnerText;
                        cdr_url = documentNode.SelectSingleNode("xmlFileSunatUrl").InnerText;
                        // Add more fields as needed
                    }

                    bool respuestaanexo = new MetodosGuia().AnexarDocumentos(docentry, nombreArchivo, pdf_url, xml_url, cdr_url, ObjType, oCompany, ref MENSAJE);
                    oform.Close();
                    Program.SboAplicacion.
                    //OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_DeliveryNotes, "", docentry.ToString());
                    OpenForm((SAPbouiCOM.BoFormObjectEnum)ObjType, "", docentry.ToString());

                   

                    Program.SboAplicacion.MessageBox("Se registro la GUIA REMISION correctamente");
                    return;
                    #endregion



                }
                #endregion

                if ((pVal.FormTypeEx.Equals("45")) && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) && pVal.Before_Action)
                {
                    SAPbouiCOM.Form oformPago = Program.SboAplicacion.Forms.Item(FormUID);
                    SAPbouiCOM.Item itemCarla2 = oformPago.Items.Add("btnOT", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                    itemCarla2.Top = oformPago.Items.Item(1).Top + 76;
                    itemCarla2.Left = oformPago.Items.Item(7).Left + 200;
                    itemCarla2.Width = 90;
                    (itemCarla2.Specific as SAPbouiCOM.Button).Caption = "Crear Excel con OT";
                }

            }
            catch (Exception ex)
            {
                Program.SboAplicacion.MessageBox("Facturación Electrónica: " + ex.Message);
            }
        }





        #region Validar y Actualizar Existencia de Folio Anticipo
        public bool ValidarExistenciaFolioFactAnt(int DocEntry, string TipoDoc, int DocNum, int ObjecType)
        {
            return new FacturaDAO().ValidarExistenciaFolioFactAnt(DocEntry, TipoDoc, DocNum, ObjecType, oCompany);
        }
        public void ActualizarFolioFactAnt(int docmun, int objectype, string tipodocumento, int docentry, ref string foliopref, ref string folionum)
        {
            new FacturaDAO().Update_Folio_FactAnt(tipodocumento, docmun, objectype, docentry, ref foliopref, ref folionum, oCompany);
        }
        #endregion

        #region Validar y Actualizar Existencia de Folio OINV
        public bool ValidarExistenciaFolioOINV(int DocEntry, string TipoDoc, int DocNum, int ObjectType)
        {
            return new FacturaDAO().ValidarExistenciaFolioOINV(DocEntry, TipoDoc, DocNum, ObjectType, oCompany);
        }

        public void ActualizarFolioFact(int docmun, int objectype, string tipodocumento, int docentry, ref string foliopref, ref string folionum)
        {
            new FacturaDAO().Update_Folio_Fact(tipodocumento, docmun, objectype, docentry, ref foliopref, ref folionum, oCompany);
        }
        #endregion


        #region Validad Existencia de Folio ORIN
        public bool ValidarExistenciaFolioORIN(int DocEntry, string TipoDoc, int DocNum, int ObjectType)
        {
            return new NotaCreditoDAO().ValidarExistenciaFolioORIN(DocEntry, TipoDoc, DocNum, ObjectType, oCompany);
        }

        public void ActualizarFolioNotaCredito(int docmun, int objectype, string tipodocumento, int docentry, ref string foliopref, ref string folionum)
        {
            new NotaCreditoDAO().Update_Folio_NotaCredito(tipodocumento, docmun, objectype, docentry, ref foliopref, ref folionum, oCompany);
        }
        #endregion

        #region Actualizar Folio SAP
        public bool ActualizarFolioSAP(int DocEntry, int docnum, int objectype, string tipodocumento, SAPbobsCOM.Company oCompany)
        {
            bool folioact = true;
            string FolioPref = "";
            string FolioNum = "";
            bool verfolio = true;

            try
            {
                SAPbobsCOM.Documents odoc = null; //nota de credito

                if (objectype == 203)
                {
                    if (tipodocumento == "01" || tipodocumento == "03" || tipodocumento == "08" || tipodocumento == "08")
                    {
                        ActualizarFolioFactAnt(docnum, objectype, tipodocumento, DocEntry, ref FolioPref, ref FolioNum);
                        odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDownPayments); //nota de credito
                    }
                }
                else
                {
                    if (tipodocumento == "01" || tipodocumento == "03" || tipodocumento == "08")
                    {
                        ActualizarFolioFact(docnum, objectype, tipodocumento, DocEntry, ref FolioPref, ref FolioNum);
                        odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices); //nota de credito
                    }

                    if (tipodocumento == "07")
                    {
                        ActualizarFolioNotaCredito(docnum, objectype, tipodocumento, DocEntry, ref FolioPref, ref FolioNum);
                        odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject((SAPbobsCOM.BoObjectTypes)Convert.ToInt32(objectype));
                        //odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes); //nota de credito
                    }

                }

                odoc.GetByKey(Convert.ToInt32(DocEntry));

                try
                {
                    //odoc.NumAtCard = FolioPref + "-" + FolioNum;
                    odoc.FolioPrefixString = FolioPref;
                    odoc.FolioNumber = Convert.ToInt32(FolioNum);
                    //odoc.Printed = SAPbobsCOM.PrintStatusEnum.psYes;
                    int res = odoc.Update();

                    int temp_int = res;
                    string temp_string = "";
                    oCompany.GetLastError(out temp_int, out temp_string);

                    if (res > -1)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(odoc);
                        odoc = null;
                        GC.Collect();
                        //Update_CorrelativoSAP(tipodocumento, Convert.ToInt32(docnum), Convert.ToInt32(objectype));
                        //MessageBox.Show("actualizado");
                    }
                    else
                    {
                        Program.SboAplicacion.SetStatusBarMessage("Verifique " + temp_string, SAPbouiCOM.BoMessageTime.bmt_Short, false);
                        return false;

                    }
                }
                catch (Exception ex)
                {
                    Program.SboAplicacion.SetStatusBarMessage("No contiene número de folio  ", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                    return false;
                    //MessageBox.Show(ex.Message);
                }

            }
            catch (Exception ex)
            {
                Program.SboAplicacion.SetStatusBarMessage("Verifique " + ex, SAPbouiCOM.BoMessageTime.bmt_Short, false);
                return false;
            }

            return folioact;
        }


        public bool ActualizarFolioSAP2019(Int32 docentry, Int32 objectype, String tipodocumento, String serie, Int32 numero, SAPbobsCOM.Company oCompany, ref string cadena)
        {
            bool folioact = true;
            try
            {
                SAPbobsCOM.Documents odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices); //nota de credito

                if (objectype == 203)
                {
                    if (tipodocumento == "01" || tipodocumento == "03" || tipodocumento == "08" || tipodocumento == "08")
                    {
                        odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDownPayments); //nota de credito
                    }
                }
                else
                {
                    if (tipodocumento == "01" || tipodocumento == "03" || tipodocumento == "08")
                    {
                        odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices); //nota de credito
                    }

                    if (tipodocumento == "07")
                    {
                        odoc = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes); //nota de credito
                    }
                }
                odoc.GetByKey(Convert.ToInt32(docentry));

                try
                {
                    //odoc.NumAtCard = FolioPref + "-" + FolioNum;
                    odoc.FolioPrefixString = serie;
                    odoc.FolioNumber = Convert.ToInt32(numero);
                    Int32 res = odoc.Update();
                    Int32 temp_int = res;
                    String temp_string = "";
                    oCompany.GetLastError(out temp_int, out temp_string);

                    if (res > -1)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(odoc);
                        odoc = null;
                        GC.Collect();
                        //Update_CorrelativoSAP(tipodocumento, Convert.ToInt32(docnum), Convert.ToInt32(objectype));
                        //MessageBox.Show("actualizado");
                    }
                    else
                    {
                        cadena = temp_string;
                        /*MessageBox.Show(temp_string); roche */
                    }
                }
                catch (Exception ex)
                {
                    Program.SboAplicacion.SetStatusBarMessage("No contiene número de folio  ", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                    return false;//MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Program.SboAplicacion.SetStatusBarMessage("Verifique " + ex, SAPbouiCOM.BoMessageTime.bmt_Short, false);
                return false;
            }
            return folioact;
        }
        #endregion


        #region Registrar Documento CPE
        public bool RegistrarDocumento(int docmun, int objectype, string tipodocumento, ref bool mregistrado, bool tiempo, int docentry, ref string TramaCdr)
        {
            bool result = false;
            string mensaje_error = "";
            IntegradorBizLinks.EBizGenericInvokerClient oServicio21 = new IntegradorBizLinks.EBizGenericInvokerClient();
            oServicio21.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["usuarioBizlinks"].ToString();
            oServicio21.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["passwordBizlinks"].ToString(); ;
            string DatosComprobante = "";
            string Status = "";

            try
            {
                DateTime date1 = DateTime.Now;
                switch (objectype)
                {
                    case 13:
                        DatosComprobante = new FacturaDAO().getGeneral21(docentry);
                        break;
                    case 14:
                        DatosComprobante = new NotaCreditoDAO().getGeneral21(docentry);
                        break;
                    case 46:
                        DatosComprobante = new RetencionDAO().getGeneral21(docentry);
                        break;
                    case 203:
                        DatosComprobante = new FacturaDAO().getGeneral21Ant(docentry);
                        break;
                    default:
                        break;
                }

                /*
                if (objectype == 13)
                {
                    DatosComprobante = new FacturaDAO().getGeneral21(docentry);
                }
                
                if (objectype == 14)
                {
                    DatosComprobante = new NotaCreditoDAO().getGeneral21(docentry);
                }
                */
                string mcadena = "";
                mregistrado = false;




                try
                {
                    string enviado = DatosComprobante;
                    //return false;
                    var responseDocumento = oServicio21.invoke(DatosComprobante);
                    string er = "";

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseDocumento.ToString());

                    XmlNodeList documentNodes = xmlDoc.GetElementsByTagName("document");
                    foreach (XmlNode documentNode in documentNodes)
                    {
                        Status = documentNode.SelectSingleNode("status").InnerText;
                    }

                    #region si es Error
                    if (Status.ToUpper() == "ERROR")
                    {
                        XmlNodeList messagesNode = xmlDoc.GetElementsByTagName("messages");
                        foreach (XmlNode messagesNodes in messagesNode)
                        {
                            mensaje_error += messagesNodes.SelectSingleNode("codeDetail").InnerText + " - " + messagesNodes.SelectSingleNode("descriptionDetail").InnerText + "\n";
                            // Add more fields as needed
                        }
                        Program.SboAplicacion.MessageBox("Error : " + "\n" + mensaje_error);
                        GC.Collect();
                        return false;
                    }

                    #endregion





                    // Obtener el elemento messageSunat
                    XmlNode messageSunatNode = xmlDoc.SelectSingleNode("//messages");

                    // Verificar si el elemento messageSunat existe
                    if (messageSunatNode != null)
                    {
                        // Obtener el contenido del elemento messageSunat
                        string messageSunatContent = messageSunatNode.InnerText;

                        Program.SboAplicacion.MessageBox(messageSunatContent);
                        return false;
                    }


                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("No había ningún extremo escuchando en"))
                    {
                        Program.SboAplicacion.MessageBox("Error - Revise Su Conexion a Internet");
                    }
                    Program.SboAplicacion.SetStatusBarMessage(ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Short, false);
                }

            }
            catch (Exception ex)
            {
                if (11 == null)
                    Program.SboAplicacion.MessageBox("Error - Su comprobante no tiene folio - Facturación Electrónica");
                else
                    Program.SboAplicacion.SetStatusBarMessage("Verifique " + ex, SAPbouiCOM.BoMessageTime.bmt_Short, false);
            }

            return result;
        }

        #endregion

        #region Anexar documentos CEP
        public bool AnexarDocumentosCpe(int DocEntry, string nombreArchivo, string pdf_url, string xml_url, string cdr_url, int ObjType, SAPbobsCOM.Company oCompany, ref string mensaje)
        {
            FuncionesRequeridas oFuncionesRequeridas = new FuncionesRequeridas();
            bool respuestapeso = false;


            #region PDF
            string mensajeerrorpdf = "";
            string rutaArchivoPdf = ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString() + "\\" + nombreArchivo + ".pdf";

            if (!ValidarPesoArchivo(nombreArchivo + ".pdf"))
            {
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
                SetAnexoSAPCPE(nombreArchivo + ".pdf", ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString(), DocEntry, oCompany, ObjType, ref mensajeerrorpdf);

            }

            #endregion

            #region XML
            if (!ValidarPesoArchivo(nombreArchivo + ".xml"))
            {
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
                SetAnexoSAPCPE(nombreArchivo + ".xml", ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString(), DocEntry, oCompany, ObjType, ref mensajeerrorpdf);

            }
            #endregion


            #region CDR
            respuestapeso = ValidarPesoArchivo("R-" + nombreArchivo + ".xml");
            if (!ValidarPesoArchivo("R-" + nombreArchivo + ".xml"))
            {
                string rutaArchivoCdr = ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString() + "\\R-" + nombreArchivo + ".xml";
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
                SetAnexoSAPCPE("R-" + nombreArchivo + ".xml", ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString(), DocEntry, oCompany, ObjType, ref mensajeerrorpdf);
            }
            #endregion

            Program.SboAplicacion.SetStatusBarMessage("Se termino el proceso de Consulta ", SAPbouiCOM.BoMessageTime.bmt_Short, false);
            return true;
        }


        public bool SetAnexoSAPCPE(string nombrearchivo, string rutaarchivo, int docentry, SAPbobsCOM.Company oCompany, int ObjectType, ref string mensajeErrorInterno)
        {
            int existearchivoanexado = 0;
            int numeroexistencia = 0;
            try
            {
                string exportPathForSAP = rutaarchivo;
                exportPathForSAP = exportPathForSAP.Replace("\\\\\\\\", "\\\\");
                exportPathForSAP = exportPathForSAP.Replace("XML\\", "XML");

                SAPbobsCOM.Documents oinvoice = null;
                oinvoice = (SAPbobsCOM.Documents)oCompany.GetBusinessObject((SAPbobsCOM.BoObjectTypes)Convert.ToInt32(ObjectType));
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
                        EliminiarArchivoSAPAnexo(exportPathForSAP + "\\" + nombrearchivo, nombrearchivo);
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


        #region Validar Peso Archivo
        public bool ValidarPesoArchivo(string NombreArchivo)
        {
            bool respuesta = false;
            SAPbobsCOM.CompanyService comServ = oCompany.GetCompanyService();
            SAPbobsCOM.PathAdmin pthAdm = comServ.GetPathAdmin();
            string path = pthAdm.AttachmentsFolderPath;
            if (path.Substring(path.Length - 1, 1).Equals(@"\"))
                path = path.Substring(0, path.Length - 1);

            string rutaArchivo = path + "\\" + NombreArchivo;
            // Verificar si el archivo existe
            if (File.Exists(rutaArchivo))
            {
                // Obtener información del archivo
                FileInfo fileInfo = new FileInfo(rutaArchivo);

                // Obtener el tamaño del archivo en bytes
                long pesoEnBytes = fileInfo.Length;

                // Convertir el tamaño a kilobytes (1 KB = 1024 bytes)
                double pesoEnKilobytes = (double)pesoEnBytes / 1024;
                if (pesoEnKilobytes > 0)
                {
                    respuesta = true;
                }

            }
            return respuesta;
        }
        #endregion

        #region Eliminar un archivo de SAP para que pueda crear otro
        public bool EliminiarArchivoSAPAnexo(string RutaArchivo,string NombreArchivo)
        {
            bool respuesta = false;

            #region Eliminar Archivo
            try
            {
                // Verificar si el archivo existe antes de intentar borrarlo
                if (File.Exists(RutaArchivo))
                {
                    // Borrar el archivo
                    File.Delete(RutaArchivo);
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
            }
            catch (Exception ex)
            {
                // Capturar y manejar cualquier excepción que pueda ocurrir durante el proceso de borrado
                respuesta = false;
            }
            #endregion

            #region Copiar Archivo
            try
            {
                string rutaoriginal = ConfigurationManager.AppSettings["rutaarchivoslocal"].ToString()+ NombreArchivo;
                // Verificar si el archivo existe antes de intentar borrarlo
                if (File.Exists(rutaoriginal))
                {
                    // Borrar el archivo
                    File.Copy(rutaoriginal, RutaArchivo);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion

            return respuesta;

        }
        #endregion

    }
}
