using AddonFE.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddonFE
{
    static class Program
    {
        public static SAPbouiCOM.Application SboAplicacion;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                string strConexion = "";
                string[] strArgumentos = new string[4];
                SAPbouiCOM.SboGuiApi oSboGuiApi = null;



                #region Ejecutar desde visual
                oSboGuiApi = new SAPbouiCOM.SboGuiApi();
                strArgumentos[0] = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";
                if (strArgumentos.Length > 0)
                {
                    if (strArgumentos.Length > 1)
                    {
                        if (strArgumentos[0].LastIndexOf("\\") > 0) strConexion = strArgumentos[1];
                        else strConexion = strArgumentos[0];
                    }
                    else
                    {
                        if (strArgumentos[0].LastIndexOf("\\") > -1) strConexion = strArgumentos[0];
                        else
                        {
                            MessageBox.Show(" Error en: Conexion_SBO.cs > ObtenerAplicacion(): SAP Business One no esta en ejecucion", "Aceptar",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(" Error en: Conexion_SBO.cs > ObtenerAplicacion(): SAP Business One no esta en ejecucion", "Aceptar",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                oSboGuiApi.Connect(strConexion);
                SboAplicacion = oSboGuiApi.GetApplication(-1);
                SAPbobsCOM.Company oCompany;
                oCompany = (SAPbobsCOM.Company)SboAplicacion.Company.GetDICompany();


                #endregion





                //#region para Instalar Addon
                //oSboGuiApi = new SAPbouiCOM.SboGuiApi();
                //oSboGuiApi.Connect(System.Environment.GetCommandLineArgs().GetValue(1).ToString());
                //SboAplicacion = oSboGuiApi.GetApplication(-1);
                //SboAplicacion.StatusBar.SetText("SMC_AddonFe conectado", SAPbouiCOM.BoMessageTime.bmt_Medium, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                //SAPbobsCOM.Company oCompany;
                //oCompany = (SAPbobsCOM.Company)SboAplicacion.Company.GetDICompany();
                //#endregion



                FacturaSAP ofacturasap = new FacturaSAP();
                //SAPbobsCOM.Company oCompany;
                //oCompany = (SAPbobsCOM.Company)SAPbouiCOM..Application.SBO_Application.Company.GetDICompany();

                Menu MyMenu = new Menu(oCompany);
                MyMenu.AddMenuItems();


                ofacturasap.CrearObjetoenFactura(oCompany, SboAplicacion);
                SboAplicacion.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(MyMenu.SBO_Application_MenuEvent);

                //SboAplicacion.AppEvent
                //oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                //oApp.Run();
                //System.Windows.Forms.Application.EnableVisualStyles();
                //System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                throw;
            }
        }
    }
}
