using Database_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Database_Connection.Controllers
{
    public class LOC_StateController : Controller
    {

        private IConfiguration Configuration;
        public LOC_StateController(IConfiguration _configuration)
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
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_State_SelectAll";
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
            }

            return View("LOC_StateList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int StateID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_DeleteByPK";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Index");
        }
        #endregion

        [HttpPost] 
        public IActionResult Save(LOC_StateModel modelLOC_State)
        {
            #region Insert & Update
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_State.StateID == null)
            {   
                cmd.CommandText = "PR_LOC_State_Insert";
            }
            else
            {
                    cmd.CommandText = "PR_LOC_State_UpdateByPK";
                    cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLOC_State.StateID;
            }

            cmd.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelLOC_State.StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.Int).Value = modelLOC_State.StateCode;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_State.CountryID;   
            //cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = modelLOC_State.Created;
            //cmd.Parameters.Add("@ModificationDate", SqlDbType.DateTime).Value = modelLOC_State.Modified;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_State.StateID == null)
                {
                    TempData["StateInsertMsg"] = "SuccessFully Inserted";
                }

                 else
                {
                    TempData["CityInsertMsg"] = "SuccessFully Updated";
                }
            }
            conn.Close();
            return Index();

            //return RedirectToAction("Index");
            #endregion
        }

        public IActionResult Add(int? StateID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            #region CountryDropdown
            SqlConnection conn1 = new SqlConnection(str);
            conn1.Open();
            SqlCommand cmd = conn1.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_SelectForDropDown";
            DataTable dt1 = new DataTable();
            SqlDataReader sdr1 = cmd.ExecuteReader();
            dt1.Load(sdr1);
                
            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach(DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }
            ViewBag.CountryList = list;
            #endregion

            #region Record Select By PK
            if (StateID != null)
            {
                //string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand objcmd = conn.CreateCommand();
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.CommandText = "PR_LOC_State_SelectByPK";
                objcmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
                LOC_StateModel modelLOC_State = new LOC_StateModel();
                DataTable dt = new DataTable();
                SqlDataReader sdr = objcmd.ExecuteReader();
                dt.Load(sdr);

                if(dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        modelLOC_State.StateID = Convert.ToInt32(dr["StateID"]);
                        modelLOC_State.StateName = dr["StateName"].ToString();
                        modelLOC_State.StateCode = Convert.ToInt32(dr["StateCode"]);
                        modelLOC_State.CountryID = Convert.ToInt32(dr["CountryID"]);
                    }

                    conn.Close();

                    return View("LOC_StateAddEdit", modelLOC_State);
                }
                    
            }
            #endregion

            return View("LOC_StateAddEdit");
        }
    }
}
