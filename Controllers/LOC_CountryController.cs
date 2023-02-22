using Database_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Database_Connection.Controllers
{
    public class LOC_CountryController : Controller
    {

        private IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index(LOC_CountryModel model)
        {

            string str = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                if (model.CountryName == null) {

                    cmd.CommandText = "PR_LOC_Country_SelectAll";
                }
                else
                {
                    cmd.CommandText = "PR_LOC_Country_SelectByCountryNameCode";
                    cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = model.CountryName;

                    if (model.CountryCode != null)
                        cmd.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = model.CountryCode;

                    else
                    {
                        cmd.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = "";
                    }
                }
                SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                conn.Close();
            }

            return View("LOC_CountryList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_DeleteByPK";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Index");
        }
        #endregion

        [HttpPost]
         public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            if (ModelState.IsValid)
            {
                #region Insert & Update
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                if (modelLOC_Country.CountryID == null)
                {
                    cmd.CommandText = "PR_LOC_Country_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_LOC_Country_UpdateByPK";
                    cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_Country.CountryID;
                }

                cmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelLOC_Country.CountryName;
                //cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = modelLOC_Country.Created;
                //cmd.Parameters.Add("@ModificationDate", SqlDbType.DateTime).Value = modelLOC_Country.Modified;
                cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelLOC_Country.CountryCode;

                if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
                {
                    if (modelLOC_Country.CountryID == null)
                    {
                        TempData["CountryInsertMsg"] = "SuccessFully Inserted";
                    }

                    else
                    {
                        TempData["CountryInsertMsg"] = "SuccessFully Updated";
                    }
                }
                conn.Close();

            #endregion
            }
            return RedirectToAction("Index");

        }

        public IActionResult Back()
        {
            return View("Index");
        }

        public IActionResult Filter(LOC_CountryModel model)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            DataTable dt = new DataTable();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_SelectByCountryNameCode";
            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value =model.CountryName ;
            cmd.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = model.CountryCode;
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();

            return View("LOC_CountryList",dt);
        }

        public IActionResult Add(int? CountryID)
        {
            #region SelectByPK
            if (CountryID!=null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_Country_SelectByPK";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                LOC_CountryModel modelLOC_Country=new LOC_CountryModel();
                DataTable dt=new DataTable();
                SqlDataReader sdr=cmd.ExecuteReader();
                dt.Load(sdr);


                foreach(DataRow dr in dt.Rows)
                {
                    modelLOC_Country.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_Country.CountryName = dr["CountryName"].ToString();
                    modelLOC_Country.CountryCode = dr["CountryCode"].ToString();
                }

                conn.Close();

                return View("LOC_CountryAddEdit", modelLOC_Country);
            }
            #endregion

            return View("LOC_CountryAddEdit");
        }
    }
}
