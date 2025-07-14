using AddonFE.Configuraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddonFE.DAO
{
    public class MetodoGuiaDAO
    {
        public void Update_Folio_Guia(int DocEntry, int ObjectType, string TipoDoc, ref string FolioPref, ref string FolioNum, SAPbobsCOM.Company oCompanyDAO)
        {
            FolioNum = "0";
            FolioPref = "0";
            string nombre_Store = "SMC_Actualizar_FELFolioPrefNumGUIA";
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
    }
}
