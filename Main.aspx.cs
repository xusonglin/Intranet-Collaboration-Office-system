using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using JiaoShiXinXiTongJi;
using Maticsoft.DBUtility;


namespace JiaoShiXinXiTongJi
{
    public partial class _Default : BasePage
    {
        #region 定义变量
        protected string role;
        protected string functionids;
        protected string Funs;
        protected string sysFuns;
        protected string sysFunsname;
        protected string userid;
        protected string username = "";
        protected string taskcount = "";
        #endregion
     protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
      
            role = Session["Role"].ToString();

            userid = Session["UserID"].ToString();
            labRealName.Text = Session["ZGXM"].ToString();
            username = Session["ZGXM"].ToString();

            string selcount = "SELECT COUNT(*) FROM dbo.Bap_Task_User WHERE UserID='" + userid + "' AND State='未完成'";
             taskcount = DbHelperSQL.GetSingle(selcount) + "";
            if (taskcount == "")
            {
                taskcount = "0";
            }
            #region 取得角色权限
            string sel = "select FunctionIDs from bap_role where id='" + role + "'";
            functionids = DbHelperSQL.GetSingle(sel).ToString();
            string[] arrids = functionids.Split(',');
            for (int i = 0; i < arrids.Length; i++)
            {
                string Fun = "";
                string functionid = arrids[i];
                string selfunction = "select * from bap_function where functionid='" + functionid + "' order by px";
                SqlDataReader drr = DbHelperSQL.ExecuteReader(selfunction);
                while (drr.Read())
                {
                    string FunctionName = drr["FunctionName"].ToString();
                    string Url = drr["Url"].ToString();
                    string ParentID = drr["ParentID"].ToString().ToUpper();
                    Fun = FunctionName + "%#" + Url + "%#" + ParentID;
                }
                Funs += Fun + "&#";
            }
            #endregion
            #region 取得功能
            Common common = new Common();
            SqlConnection con = common.coon();
            con.Open();
            string sysfuncion = "select * from bap_function where ParentID='00000000-0000-0000-0000-000000000000'";
             SqlDataAdapter da_text = new SqlDataAdapter(sysfuncion, con);
            DataSet ds_text = new DataSet();
            da_text.Fill(ds_text);
            string[] arr_text = new string[100];
            for (int j = 0; j < ds_text.Tables[0].Rows.Count; j++)
            {
                sysFuns += ds_text.Tables[0].Rows[j][0].ToString().ToUpper() + ","; //拼接 FunctionID名称
                sysFunsname += ds_text.Tables[0].Rows[j][1].ToString() + ","; //拼接 FunctionID名称
            }
            sysFuns = sysFuns.Substring(0, sysFuns.Length - 1);
            sysFunsname = sysFunsname.Substring(0, sysFunsname.Length - 1);
            con.Close();
            #endregion
           
         
        }

         if (Page.IsPostBack)
         {
         Session.Clear();
         Session.Abandon();
         Response.Redirect("~/logon.aspx");
         
         }
    }
    }
}
