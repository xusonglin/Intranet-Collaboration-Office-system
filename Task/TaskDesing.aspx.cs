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

namespace JiaoShiXinXiTongJi.Task
{
    public partial class TaskDesing : BasePage
    {
        #region 定义变量
        protected string strtable;
        protected string usernam;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            usernam = Session["ZGXM"].ToString();
            string selecttable = "select name from dbo.sysobjects";
            SqlDataReader dr = DbHelperSQL.ExecuteReader(selecttable);
            while (dr.Read())
            {
                strtable += dr["name"].ToString() + ",";   //避免重复表名的出现
            }

            Common common = new Common();
            SqlConnection con = common.coon();

            if (Page.IsPostBack)
            {
                string userid = Session["UserID"].ToString();
                string username = Session["ZGXM"].ToString();
                DateTime strTime = System.DateTime.Now;
                con.Open();
                if (Request.Form["list"] != null && Request.Form["list_text"] != null)
                {
                    string list_create = "";
                    string list = Request.Form["list"];
                    string list_textt = Request.Form["list_text"];
                    if (list.Length == 0 || list_textt.Length == 0)
                    {
                        Response.Write("<script>alert('任务创建失败！请重试！')</script>");
                        Response.Write("<script>document.location=document.location;</script>");

                    }

                    else
                    {
                        #region 将标题和字段信息插入到固定表中
                        string newid = "";
                        string listt = list.Substring(1, list.Length - 1);
                        string[] arr = list.Split(',');
                        string sel = "select newid()";
                        SqlCommand cooom = new SqlCommand(sel, con);//获取newid
                        newid = cooom.ExecuteScalar().ToString();
                        cooom.Clone();
                        con.Close();

                        string ins = " insert into Bap_Title(TaskID,TitleName,TitleFontFamily,TitleFontSize,TitleFontColor,ISFold) select '" + newid + "', '" + arr[0] + "','" + arr[1] + "','" + arr[2] + "','" + arr[3] + "','" + arr[4] + "'";

                        string TableName = arr[0];
                        SqlCommand com = new SqlCommand(ins, con);
                        DbHelperSQL.ExecuteSql(ins);
                        string list_text = list_textt.Substring(0, list_textt.Length - 1);
                        string[] arr_text = list_text.Split('_');
                        for (int i = 0; i < arr_text.Length; i++)
                        {
                            string[] arra_text = arr_text[i].Split(',');
                            string ins_text = " insert into Bap_Text(TaskID,TextName,TextFontFamily,TextFontSize,TextFontColor,Type) select '" + newid + "','" + arra_text[0] + "','" + arra_text[1] + "','" + arra_text[2] + "','" + arra_text[3] + "','" + arra_text[4] + "'";
                            DbHelperSQL.ExecuteSql(ins_text);
                            list_create += arra_text[0] + ",";

                        }

                        #endregion
                        #region 创建临时数据库表
                        //创建表
                        string inserttask = "insert into bap_task (TaskID,UserID,TaskName,TaskMaker,DateTime,State) values('" + newid + "','" + userid + "','" + TableName + "','" + username + "','" + strTime + "','未发布')";

                        DbHelperSQL.ExecuteSql(inserttask);
                        string sqlStr = "if   objectproperty(object_id('[" + TableName + "【" + username + "】 ]'),'IsUserTable')=1   select 1 else   select   0";//和下面的区别 []
                        con.Open();
                        SqlCommand cmd = new SqlCommand(sqlStr, con);
                        object c = cmd.ExecuteScalar();
                        con.Close();
                        if (c.ToString() == "1")
                        {

                            string cmddel = @" drop table [" + TableName + "]   ";//注意显示问题‘’
                            con.Open();
                            SqlCommand coomn = new SqlCommand(cmddel, con);
                            coomn.ExecuteScalar();
                            con.Close();

                        }

                        using (SqlConnection coon = common.coon())
                        {
                            string li = list_create;
                            string lis = list_create.Substring(0, list_create.Length - 1);
                            string[] list1 = lis.Split(',');
                            string strlist = "";
                            for (int i = 0; i < list1.Length; i++)
                            {
                                strlist += (list1[i] + " " + "varchar(50)") + ",";
                                string strlist1 = list1[0];
                            }
                            string cmdText = @" create table [" + TableName + "【" + username + "】](UserID UNIQUEIDENTIFIER, " + strlist + " px int NOT NULL  IDENTITY(1,1) , )   ";//2012-1-7增加UserID处理信息填写时修改
                            coon.Open();
                            SqlCommand coom = new SqlCommand(cmdText, coon);
                            coom.ExecuteScalar();
                            coon.Close();

                            Response.Write("<script>alert('创建数据表成功！')</script>");
                            Response.Write("<script>document.location=document.location;</script>");

                        }
                        #endregion

                    }
                }

            }
        }

    }
}
