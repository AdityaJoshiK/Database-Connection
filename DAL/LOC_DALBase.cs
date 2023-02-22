using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;

namespace Database_Connection.DAL
{
    public class LOC_DALBase
    {
        #region PR_LOC_Country_SelectAll

        public DataTable PR_LOC_Country_SelectAll(string conn)
        {
            try
            {
                SqlDatabase sqldb = new SqlDatabase(conn);
                DbCommand dbCMD = sqldb.GetStoredProcCommand("PR_LOC_Country_SelectAll");

                DataTable dt = new DataTable();

                using (IDataReader dr = sqldb.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return dt;
            }

            catch (Exception ex) { return null; }
        }

        #endregion
    }
}
