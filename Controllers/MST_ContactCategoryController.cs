using Database_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Database_Connection.Controllers
{
    public class MST_ContactCategory : Controller
    {

        private IConfiguration Configuration;
        public MST_ContactCategory(IConfiguration _configuration)
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
                cmd.CommandText = "PR_MST_ContactCategory_SelectAll";   
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
            }

            return View("MST_ContactCategoryList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int ContactCategoryID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_CON_ContactCategory_DeleteByPK";
            cmd.Parameters.AddWithValue("@ContactCategoryID", ContactCategoryID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Index");
        }
        #endregion

        public IActionResult Save(MST_ContactCategoryModel modelMST_ContactCategory)
        {
            #region Insert & Update
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if(modelMST_ContactCategory.ContactCategoryID == null)
            {
                cmd.CommandText = "PR_CON_ContactCategory_Insert";
            }
            else
            {
                cmd.CommandText = "PR_CON_ContactCategory_UpdateByPK";
                cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelMST_ContactCategory.ContactCategoryID;
            }

            cmd.Parameters.Add("@ContactCategoryName", SqlDbType.VarChar).Value = modelMST_ContactCategory.ContactCategoryName;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if(modelMST_ContactCategory.ContactCategoryID == null)
                {
                     TempData["ContactCategoryInsertMsg"] = "SuccessFully Inserted";
                }

                else
                {
                    TempData["CountryInsertMsg"] = "SuccessFully Updated";
                }
             }
            conn.Close();

            //return View("MST_ContactCategoryAddEdit");
            return RedirectToAction("Index");
            #endregion
        }

        public IActionResult Add(int? ContactCategoryID)
        {
            #region SelectByPK
            if (ContactCategoryID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_CON_ContactCategory_SelectByPK";
                cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = ContactCategoryID;
                MST_ContactCategoryModel modelMST_ContactCategory = new MST_ContactCategoryModel();
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);


                foreach (DataRow dr in dt.Rows)
                {
                    modelMST_ContactCategory.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelMST_ContactCategory.ContactCategoryName = dr["ContactCategoryName"].ToString();
                }

                conn.Close();

                return View("MST_ContactCategoryAddEdit", modelMST_ContactCategory);
            }
            #endregion

            return View("MST_ContactCategoryAddEdit");
        }
    }
}
