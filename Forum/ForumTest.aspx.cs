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

namespace JiaoShiXinXiTongJi.Forum
{
    public partial class ForumTest : BasePage
    {
        #region 定义变量
        protected string strs="";
        protected int  Count=0;
        protected string userid = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        userid = Session["UserID"].ToString();
         string type=  Request.Form["type"]+"";
         if(type=="add")
         {
            string liuyanconten = Request.Form["Content"].ToString();
           
            DateTime LyDate = DateTime.Now;
            string ins = "insert into bap_forum select newid(),'" + liuyanconten + "','" + LyDate + "','" + userid + "'";
            DbHelperSQL.ExecuteSql(ins);
                
         
         }

         if (type == "deleteliuyan")
         { 
         
         string CountID = Request.Form["CountID"].ToString();

         string dele = "delete from bap_forum where CountID='" + CountID + "'";
         string delehf = "delete from bap_forum_hf where CountID='" + CountID + "'";
         DbHelperSQL.ExecuteSql(dele);
         DbHelperSQL.ExecuteSql(delehf);
         
         }

                string se = "select bf.*,bu.ZGXM from bap_forum bf left join bap_user bu on bf.userid=bu.userid order by bf.Lydate desc";
                DataSet dt = DbHelperSQL.Query(se);
                string[] arrlist = new string[1000];
                if (dt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {

                        arrlist[i] = dt.Tables[0].Rows[i]["CountID"].ToString() + "_" + dt.Tables[0].Rows[i]["Content"].ToString() + "_" + dt.Tables[0].Rows[i]["LyDate"].ToString() + "_" + dt.Tables[0].Rows[i]["UserID"].ToString() + "_" + dt.Tables[0].Rows[i]["ZGXM"].ToString();
                        strs += arrlist[i] + "&";
                    }

                    strs = strs.Substring(0, strs.Length - 1);
                    Count = dt.Tables[0].Rows.Count;

                }
 
        }

   

    }
}

