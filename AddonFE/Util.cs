using System;
using System.Runtime.InteropServices;

namespace AddonFE
{
    class Util
    {
        public static string DbaseName = "SBO_INDUSTRIAS_ENVASE_FINAL.dbo";
        public static string NombAddon = "AddonFE";
        #region Metodos
        public static void liberarObjeto(Object objeto)
        {
            try
            {
                if (objeto != null)
                {
                    Marshal.ReleaseComObject(objeto);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(NombAddon + " Error Liberando Objeto: " + ex.Message);
            }
        }
        #endregion
    }
}
