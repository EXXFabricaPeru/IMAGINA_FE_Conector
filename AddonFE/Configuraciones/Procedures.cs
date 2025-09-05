using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddonFE.Configuraciones
{
    class Procedures
    {
        SAPbobsCOM.Company oCompany2;
        SAPbobsCOM.Recordset oRec = default(SAPbobsCOM.Recordset);
        string Query = "";




        public bool IsHanaProcedure()
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
                MessageBox.Show(ex.Message);
                //SAPbobsCOM.Messagebb
                //Application.SBO_Application.MessageBox(ex.Message);
                return false;
            }
        }


        public Procedures(SAPbobsCOM.Company oCompany)
        {
            if (oCompany2 == null)
                oCompany2 = oCompany;

        }

        public SAPbobsCOM.Recordset RunQuery(string Query)
        {
            try
            {
                oRec = (SAPbobsCOM.Recordset)oCompany2.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRec.DoQuery(Query);
                return oRec;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //Application.SBO_Application.MessageBox(ex.Message);
                return null;
            }
        }

        public object Release(object objeto)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objeto);
            Query = null;
            GC.Collect();
            return null;
        }

        public string GetResourceValue(string name, string ResourceName)
        {
            //ResourceManager rm = new ResourceManager("BestPracticeB1H.Properties.Resources", Assembly.GetExecutingAssembly());
            ResourceManager rm = new ResourceManager(ResourceName, Assembly.GetExecutingAssembly());
            string value = rm.GetString(name);
            return value;
        }

        public void DropCreateProcedure(string ProcedureName, bool isHana, string ResourceName)
        {
            try
            {
                string action = "Creado";

                if (isHana == true)
                {
                    Query = "SELECT COUNT(1) FROM SYS.PROCEDURES WHERE PROCEDURE_NAME = '" + ProcedureName + "' AND SCHEMA_NAME = '" + oCompany2.CompanyDB + "'";
                    RunQuery(Query);
                    oRec.MoveFirst();
                    if ((int)oRec.Fields.Item(0).Value > 0) //Si ya existe borra el procedimiento actual y crea el que se encuentra registrado
                    {
                        Release(oRec);
                        Query = "DROP PROCEDURE \"" + ProcedureName + "\"";
                        RunQuery(Query);
                        Release(oRec);
                        action = "Actualizado";
                    }


                    Query = "CREATE " + GetResourceValue("HANA_" + ProcedureName, ResourceName);
                    RunQuery(Query);
                    Release(oRec);
                    //Application.SBO_Application.StatusBar.SetText("Procedure " + ProcedureName + ": " + action + " con éxito", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
                else //SQL
                {
                    Query = "SELECT COUNT(1) FROM SYS.PROCEDURES WHERE name = '" + ProcedureName + "'";
                    RunQuery(Query);
                    oRec.MoveFirst();
                    if ((int)oRec.Fields.Item(0).Value > 0)//Si ya existe borra el procedimiento actual y crea el que se encuentra registrado
                    {
                        Release(oRec);
                        Query = "DROP PROCEDURE " + ProcedureName;
                        RunQuery(Query);
                        Release(oRec);
                        action = "Actualizado";
                    }
                    Query = "CREATE " + GetResourceValue("SQL_" + ProcedureName, ResourceName);
                    RunQuery(Query);
                    Release(oRec);

                    //SAPbouiCOM.StatusBar.SetText("Procedure " + ProcedureName + ": " + action + " con éxito", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    Program.SboAplicacion.StatusBar.SetText("Procedure " + ProcedureName + ": " + action + " con éxito", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
            }
            catch (Exception ex)
            {
                Program.SboAplicacion.SetStatusBarMessage(ex.Message);
            }
        }

        public void DropCreateFunction(string FunctionName, bool isHana, string ResourceName)
        {
            try
            {
                string action = "Creada";
                if (isHana == true)
                {
                    Query = "SELECT COUNT(1) FROM sys.objects WHERE OBJECT_NAME = '" + FunctionName + "' AND SCHEMA_NAME = '" + oCompany2.CompanyDB + "'";
                    RunQuery(Query);
                    oRec.MoveFirst();
                    if ((int)oRec.Fields.Item(0).Value > 0) //Si ya existe borra el procedimiento actual y crea el que se encuentra registrado
                    {
                        Release(oRec);
                        Query = "DROP FUNCTION \"" + FunctionName + "\"";
                        RunQuery(Query);
                        Release(oRec);
                        action = "Actualizado";
                    }
                    Query = "CREATE " + GetResourceValue("HANA_" + FunctionName, ResourceName);
                    RunQuery(Query);
                    Release(oRec);
                    Program.SboAplicacion.StatusBar.SetText("Función " + FunctionName + ": " + action + " con éxito", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
                else //SQL
                {
                    Query = "SELECT COUNT(1) FROM SYS.OBJECTS WHERE name = '" + FunctionName + "'";
                    RunQuery(Query);
                    oRec.MoveFirst();
                    if ((int)oRec.Fields.Item(0).Value > 0)//Si ya existe borra el procedimiento actual y crea el que se encuentra registrado
                    {
                        Release(oRec);
                        Query = "DROP FUNCTION " + FunctionName;
                        RunQuery(Query);
                        Release(oRec);
                        action = "Actualizada";
                    }
                    Query = "CREATE " + GetResourceValue("SQL_" + FunctionName, ResourceName);
                    RunQuery(Query);
                    Release(oRec);
                    Program.SboAplicacion.StatusBar.SetText("Función " + FunctionName + ": " + action + " con éxito", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                }
            }
            catch (Exception ex)
            {
                Program.SboAplicacion.SetStatusBarMessage(ex.Message);
            }
        }



        public void DropProcedure(string ProcedureName, bool isHana)
        {
            try
            {
                if (isHana == true)
                {
                    Query = "SELECT COUNT(1) FROM SYS.PROCEDURES WHERE PROCEDURE_NAME = '" + ProcedureName + "' AND SCHEMA_NAME = '" + oCompany2.CompanyDB + "'";
                    RunQuery(Query);
                    oRec.MoveFirst();
                    if ((int)oRec.Fields.Item(0).Value > 0) //Si existe, borra el procedimiento
                    {
                        Release(oRec);
                        Query = "DROP PROCEDURE \"" + ProcedureName + "\"";
                        RunQuery(Query);
                        Release(oRec);
                    }
                }
                else //SQL
                {
                    Query = "SELECT COUNT(1) FROM SYS.PROCEDURES WHERE name = '" + ProcedureName + "'";
                    RunQuery(Query);
                    oRec.MoveFirst();
                    if ((int)oRec.Fields.Item(0).Value > 0)//Si existe, borra el procedimiento
                    {
                        Release(oRec);
                        Query = "DROP PROCEDURE " + ProcedureName;
                        RunQuery(Query);
                        Release(oRec);
                        Program.SboAplicacion.StatusBar.SetText("Procedure " + ProcedureName + ": Eliminado con éxito", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.SboAplicacion.SetStatusBarMessage(ex.Message);
            }
        }

        public void DropCreateTypeForHANA(string TypeName, bool isHana, string ResourceName)
        {
            string action = "Creado";
            if (isHana == true)
            {
                Query = "SELECT COUNT(1) FROM SYS.TABLES WHERE TABLE_NAME = '" + TypeName + "' AND SCHEMA_NAME = '" + oCompany2.CompanyDB + "'";
                RunQuery(Query);
                oRec.MoveFirst();
                if ((int)oRec.Fields.Item(0).Value > 0) //Si ya existe borra el Type actual y crea el que se encuentra registrado
                {
                    Release(oRec);
                    Query = "DROP TYPE \"" + TypeName + "\"";
                    RunQuery(Query);
                    Release(oRec);
                    action = "Actualizado";
                }
                Query = "CREATE " + GetResourceValue("HANA_" + TypeName, ResourceName);
                RunQuery(Query);
                Release(oRec);
                Program.SboAplicacion.StatusBar.SetText("Type " + TypeName + ": " + action + " con éxito", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
        }
    }
}
