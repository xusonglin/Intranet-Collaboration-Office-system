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
    public partial class TaskPass : BasePage
    {
        #region 定义变量
        protected string userid = "";
        protected string XX = "";
        protected string XY = "";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                userid = Session["UserID"].ToString();
                XX = Session["XX"].ToString();
                XY = Session["XY"].ToString();
            }
            #region 发布任务
            if (Page.IsPostBack)
            {
                string taskid = Request["taskid"].ToString();
                string userid = Request.Form["userid"].ToString();
                string taskdes = Request.Form["taskdes"].ToString();
                 string starttime = Request.Form["starttime"].ToString();
                string endtime = Request.Form["endtime"].ToString();

                taskdes = taskdes.Replace(" ", "&nbsp;  "); // 空格替换，以便在页面中显示的时候可以显示空格
                taskdes = taskdes.Replace("\n", "<br>");// 换行替换，以便在页面中显示的时候可以显示换行
                string lx = DropDownList_lx.SelectedValue.ToString();
                if (taskid != "" && userid != "")
                {
                    string[] arr_task = taskid.Split(',');
                    string[] arr_user = userid.Split(',');
                    for (int i = 0; i < arr_task.Length; i++)
                    {
                        for (int j = 0; j < arr_user.Length; j++)
                        {
                         string ins = "insert into bap_task_user(TaskID,UserID,TaskDes,StartTime,EndTime,State,LeiXin) select '" + arr_task[i] + "','" + arr_user[j] + "','" + taskdes + "','" + starttime + "','" + endtime + "','未完成','" + lx + "' ";
                          DbHelperSQL.ExecuteSql(ins);
                        }


                    }
                    for (int k = 0; k < arr_task.Length; k++)
                    {
                        string update = "update bap_task set State='已发布'where TaskID='" + arr_task[k] + "'";
                        DbHelperSQL.ExecuteSql(update);
                    }
                    Response.Write("<script>alert('任务发布成功！')</script>");
                    Response.Write("<script>document.location=document.location;</script>");

                }

                else
                {
                    Response.Write("<script>alert('任务发布失败！请重试！')</script>");
                    Response.Write("<script>document.location=document.location;</script>");



                }
            }
            #endregion

        }
    }
}