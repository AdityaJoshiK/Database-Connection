using Database_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Database_Connection.Controllers
{
    public class LOC_CityController : Controller
    {

        private IConfiguration Configuration;
        public LOC_CityController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Back()
        {
            return Index();
        }

        #region SelectAll
        public IActionResult Index()
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_City_SelectAll";
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
            }

            return View("LOC_CityList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CityID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_DeleteByPK";
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Index");
        }
        #endregion
        
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            #region Insert & Update
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_City.CityID == null)
            {
                cmd.CommandText = "PR_LOC_City_Insert";
            }
            else
            {
                cmd.CommandText = "PR_LOC_City_UpdateByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelLOC_City.CityID;
            }

            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_City.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLOC_City.StateID;
            cmd.Parameters.Add("@CityName", SqlDbType.VarChar).Value = modelLOC_City.CityName;
            //cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = modelLOC_Country.Created;
            //cmd.Parameters.Add("@ModificationDate", SqlDbType.DateTime).Value = modelLOC_Country.Modified;
            cmd.Parameters.Add("@CityCode", SqlDbType.Int).Value = modelLOC_City.CityCode;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_City.CityID == null)
                {
                    TempData["CityInsertMsg"] = "SuccessFully Inserted";
                }

                else
                {
                    TempData["CityInsertMsg"] = "SuccessFully Updated";
                }
            }
            conn.Close();

            return RedirectToAction("Index");
            #endregion
        }

        public IActionResult Add(int? CityID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");

            #region CountryDropdown
            SqlConnection conn1 = new SqlConnection(str);
            conn1.Open();
            SqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_LOC_Country_SelectForDropDown";
            DataTable dt1 = new DataTable();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt1.Load(sdr1);

            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }
            ViewBag.CountryList = list;
            #endregion

            #region StateDropdown
            //SqlConnection conn2 = new SqlConnection(str);
            //conn2.Open();
            //SqlCommand cmd2 = conn2.CreateCommand();
            //cmd2.CommandType = CommandType.StoredProcedure;
            //cmd2.CommandText = "PR_LOC_State_SelectForDropDown";
            //DataTable dt2 = new DataTable();
            //SqlDataReader sdr2 = cmd2.ExecuteReader();
            //dt2.Load(sdr2);

            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            //foreach (DataRow dr in dt2.Rows)
            //{
            //    LOC_StateModel vlst1 = new LOC_StateModel();
            //    vlst1.StateID = Convert.ToInt32(dr["StateID"]);
            //    vlst1.StateName = dr["StateName"].ToString();
            //    list1.Add(vlst1);
            //}
            ViewBag.StateList = list1;
            #endregion

            #region SelectByPK
            if (CityID != null)
            {
                //string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_City_SelectByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
                LOC_CityModel modelLOC_City = new LOC_CityModel();
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);


                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.CityName = dr["CityName"].ToString();
                    modelLOC_City.CityCode = Convert.ToInt32(dr["CityCode"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                }

                conn.Close();

                return View("LOC_CityAddEdit", modelLOC_City);
            }
            #endregion
            return View("LOC_CityAddEdit");
        }

        public IActionResult DropDownByCountry(int CountryID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            DataTable dt = new DataTable();
            conn.Open();
            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_LOC_State_SelectDropDownByCountryID";
            cmd1.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt.Load(sdr1);
            conn.Close();

            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_StateDropDownModel vlst1 = new LOC_StateDropDownModel();
                vlst1.StateID = Convert.ToInt32(dr["StateID"]);
                vlst1.StateName = dr["StateName"].ToString();
                list1.Add(vlst1);
            }

            var vModel = list1;
            return Json(vModel);
        }
    }
}
