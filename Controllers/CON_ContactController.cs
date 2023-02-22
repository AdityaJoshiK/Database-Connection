using Database_Connection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Database_Connection.Controllers
{
    public class CON_ContactController : Controller
    {

        private IConfiguration Configuration;
        public CON_ContactController(IConfiguration _configuration)
        {
            Configuration = _configuration;
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
                cmd.CommandText = "PR_CON_Contact_SelectAll";
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
            }

            return View("CON_ContactList", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int ContactID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_CON_Contact_DeleteByPK";
                cmd.Parameters.AddWithValue("@ContactID", ContactID);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }
        #endregion

        public IActionResult Back()
        {
            return Index();
        }
        public IActionResult Save(CON_ContactModel modelCON_Contact)
        {
            if (modelCON_Contact.File != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileNameWithPath = Path.Combine(path, modelCON_Contact.File.FileName);
                //modelCON_Contact.PhotoPath = "~" + FilePath.Replace("wwwroot\\","/") + "/" + modelCON_Contact.File.FileName;
                modelCON_Contact.PhotoPath = FilePath.Replace("wwwroot\\", "/") + "/" + modelCON_Contact.File.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    modelCON_Contact.File.CopyTo(stream);
                }
            }

            #region Insert & Update
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelCON_Contact.CityID == null)
            {
            cmd.CommandText = "PR_CON_Contact_Insert";
            }
            else
            {
            cmd.CommandText = "PR_CON_Contact_UpdateByPK";
            cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = modelCON_Contact.ContactID;
            }
            cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelCON_Contact.ContactCategoryID;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelCON_Contact.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelCON_Contact.StateID;
            cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelCON_Contact.CityID;
            cmd.Parameters.Add("@ContactName", SqlDbType.VarChar).Value = modelCON_Contact.ContactName;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = modelCON_Contact.Address;
            cmd.Parameters.Add("@PhotoPath", SqlDbType.VarChar).Value = modelCON_Contact.PhotoPath;
            cmd.Parameters.Add("@PinCode", SqlDbType.VarChar).Value = modelCON_Contact.PinCode;
            cmd.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = modelCON_Contact.Mobile;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = modelCON_Contact.Email;
            cmd.Parameters.Add("@BirthDate", SqlDbType.Date).Value = modelCON_Contact.BirthDate;
            cmd.Parameters.Add("@Linkedln", SqlDbType.VarChar).Value = modelCON_Contact.LinkedIn;
            cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = modelCON_Contact.Gender;
            //cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = modelLOC_Country.Created;
            //cmd.Parameters.Add("@ModificationDate", SqlDbType.DateTime).Value = modelLOC_Country.Modified;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelCON_Contact.CityID == null)
                {
                    TempData["ContactInsertMsg"] = "SuccessFully Inserted";
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

        public IActionResult Add(int? ContactID)
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
                //LOC_StateDropDownModel vlst1 = new LOC_StateDropDownModel();
                //vlst1.StateID = Convert.ToInt32(dr["StateID"]);
                //vlst1.StateName = dr["StateName"].ToString();
                //list1.Add(vlst1);
            //}
            ViewBag.StateList = list1;
            #endregion

            //SqlConnection conn3 = new SqlConnection(str);
            //conn3.Open();
            //SqlCommand cmd3 = conn3.CreateCommand();
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.CommandText = "PR_LOC_City_SelectForDropDown";
            //DataTable dt3 = new DataTable();
            //SqlDataReader sdr3 = cmd3.ExecuteReader();
            //dt3.Load(sdr3);

            List<LOC_CityDropDownModel> list2 = new List<LOC_CityDropDownModel>();
            //foreach (DataRow dr in dt3.Rows)
            //{
            //    LOC_CityDropDownModel vlst2 = new LOC_CityDropDownModel();
            //    vlst2.CityID = Convert.ToInt32(dr["CityID"]);
            //    vlst2.CityName = dr["CityName"].ToString();
            //    list2.Add(vlst2);
            //}
            ViewBag.CityList = list2;

            SqlConnection conn4 = new SqlConnection(str);
            conn4.Open();
            SqlCommand cmd4 = conn4.CreateCommand();
            cmd4.CommandType = CommandType.StoredProcedure;
            cmd4.CommandText = "PR_CON_ContactCategory_SelectForDropDown";
            DataTable dt4 = new DataTable();
            SqlDataReader sdr4 = cmd4.ExecuteReader();
            dt4.Load(sdr4);

            List<MST_ContactCategoryDropDownModel> list3 = new List<MST_ContactCategoryDropDownModel>();
            foreach (DataRow dr in dt4.Rows)
            {
                MST_ContactCategoryDropDownModel vlst3 = new MST_ContactCategoryDropDownModel();
                vlst3.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                vlst3.ContactCategoryName = dr["ContactCategoryName"].ToString();
                list3.Add(vlst3);
            }
            ViewBag.ContactCategoryList = list3;

            #region SelectByPK
            if (ContactID != null)
            {
                //string str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_CON_Contact_SelectByPK";
                cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;
                CON_ContactModel modelCON_Contact = new CON_ContactModel();
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);


                foreach (DataRow dr in dt.Rows)
                {
                    modelCON_Contact.ContactID = Convert.ToInt32(dr["ContactID"]);
                    modelCON_Contact.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelCON_Contact.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelCON_Contact.StateID = Convert.ToInt32(dr["StateID"]);
                    modelCON_Contact.CityID = Convert.ToInt32(dr["CityID"]);
                    modelCON_Contact.ContactName = dr["ContactName"].ToString();
                    modelCON_Contact.Address = dr["Address"].ToString();
                    modelCON_Contact.PhotoPath = dr["PhotoPath"].ToString();
                    modelCON_Contact.PinCode = dr["PinCode"].ToString();
                    modelCON_Contact.Mobile = dr["Mobile"].ToString();
                    modelCON_Contact.Email = dr["Email"].ToString();
                    modelCON_Contact.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                    modelCON_Contact.LinkedIn = dr["LinkedIn"].ToString();
                    modelCON_Contact.Gender = dr["Gender"].ToString();
                }

                conn.Close();

                return View("CON_ContactAddEdit", modelCON_Contact);
            }
            #endregion

            return View("CON_ContactAddEdit");
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

        public IActionResult DropDownByState(int StateID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            DataTable dt = new DataTable();
            conn.Open();
            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_LOC_City_SelectDropDownByStateID";
            cmd1.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt.Load(sdr1);
            conn.Close();

            List<LOC_CityDropDownModel> list1 = new List<LOC_CityDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_CityDropDownModel vlst1 = new LOC_CityDropDownModel();
                vlst1.CityID = Convert.ToInt32(dr["CityID"]);
                vlst1.CityName = dr["CityName"].ToString();
                list1.Add(vlst1);
            }

            var vModel = list1;
            return Json(vModel);
        }
    }
}
