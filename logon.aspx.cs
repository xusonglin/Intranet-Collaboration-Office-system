
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
using System.Security.Cryptography;// 密码加密用到
using System.IO;// 密码加密用到
using System.Text;
using JiaoShiXinXiTongJi;
using Maticsoft.DBUtility;

namespace JiaoShiXinXiTongJi
{
    public partial class logon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Page.IsPostBack)
            {
                string username = Request.Form["username"].ToString();
                string pass = Request.Form["password"].ToString();
           

                Common common = new Common();
                string select = " select * from  bap_user where ZGBH ='" + username + "'";
                SqlDataReader dr = DbHelperSQL.ExecuteReader(select);
                while (dr.Read())
                {
                    if ((dr["Pass"].ToString().Trim() == common.Encrypting(pass.Trim())))
                    {
                        Session["ZGBH"] = username;
                        Session["ZGXM"] = dr["ZGXM"].ToString();
                        Session["UserID"] = dr["UserID"].ToString();
                        Session["XX"] = dr["XX"].ToString();
                        Session["XY"] = dr["XY"].ToString();
                        Session["Role"] = dr["Role"].ToString();
                       
                            Response.Redirect("~/Main.aspx");
                     
                    }

                    else
                    {
                        Response.Write("<script> alert('用户名或密码错误！')</script>");
                        Response.Write("<script>document.location=document.location;</script>");
                    }
                }
            }

        }



    }



}
