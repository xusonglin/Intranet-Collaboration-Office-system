using System;
using System.Web;
using System.Text;


using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;

using System.Web.Security;

using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using JiaoShiXinXiTongJi;
using Maticsoft.DBUtility;
using Aspose.Cells;


namespace JiaoShiXinXiTongJi.Systems
{
    public partial class LoadInfo : BasePage
    {
        protected Int32 FileLength = 0;           //记录文件长度变量
        protected string userid = "";
        protected string XY = "";
        protected string XX = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            userid = Session["UserID"].ToString();
            XY = Session["XY"].ToString();
            XX = Session["XX"].ToString();
            if (!Page.IsPostBack)
            {
                Bindxuexiao();
                BindRole();
            }

        }

        private void BindRole()
        {


            Common common = new Common();
            SqlConnection Conn = common.coon();
            string strSql = "";

            strSql = "select * from Bap_Role where UserID='" + userid + "'";
           
            SqlDataAdapter adp = new SqlDataAdapter(strSql, Conn);
            Conn.Open();
            DataSet dt = new DataSet();
            adp.Fill(dt, "Bap_Role");
            DropDownListxyrole.AppendDataBoundItems = true;
            DropDownListxyrole.Items.Add(new ListItem("-- 请选择角色 --", "0"));
            DropDownListxyrole.DataSource = dt.Tables["Bap_Role"].DefaultView;
            DropDownListxyrole.DataTextField = "Role";
            DropDownListxyrole.DataValueField = "ID";
            DropDownListxyrole.DataBind();
            Conn.Close();


        }

        private void Bindxuexiao()
        {
          

            Common common = new Common();
            SqlConnection Conn = common.coon();
            string strSql = "";
            if (XX == "00000000-0000-0000-0000-000000000000")
            {
                strSql = "select * from Bap_school where ParentID='" + XX + "'";
            }
            else
            {
                strSql = "select * from Bap_school where ID='" + XX + "'";
            }

            SqlDataAdapter adp = new SqlDataAdapter(strSql, Conn);
            Conn.Open();
            DataSet dt = new DataSet();
            adp.Fill(dt, "Bap_school");
            DropDownLisxx.AppendDataBoundItems = true;
            DropDownLisxx.Items.Add(new ListItem("-- 请选择学校 --", "0"));
            DropDownLisxx.DataSource = dt.Tables["Bap_school"].DefaultView;
            DropDownLisxx.DataTextField = "MC";
            DropDownLisxx.DataValueField = "ID";
            DropDownLisxx.DataBind();
            Conn.Close();

        }



        private void Bindxueyuan()
        {
            DropDownListxy.Items.Clear();
            Common common = new Common();
            SqlConnection Conn = common.coon();
            //SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            string strSql = "";
            string selectid = DropDownLisxx.SelectedValue.ToString();
            if (XY == "00000000-0000-0000-0000-000000000000")
            {
                strSql = "select * from Bap_school where ParentID='" + selectid + "'";
            }
            else
            {
                strSql = "select * from Bap_school where ID='" + XY + "'";
            }

            SqlDataAdapter adp = new SqlDataAdapter(strSql, Conn);
            Conn.Open();
            DataSet dt = new DataSet();
            adp.Fill(dt, "Bap_school");
            DropDownListxy.AppendDataBoundItems = true;
            DropDownListxy.Items.Add(new ListItem("-- 请选择学院 --", "0"));
            DropDownListxy.DataSource = dt.Tables["Bap_school"].DefaultView;
            DropDownListxy.DataTextField = "MC";
            DropDownListxy.DataValueField = "ID";
            DropDownListxy.DataBind();

            Conn.Close();
        }





        protected void DropDownListxy_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bindxueyuan();
        }


        protected void Button_Submit(System.Object sender, System.EventArgs e)
        {
            HttpPostedFile fUpload = UP_FILE.PostedFile;   //HttpPostedFile对象，用于读取图象
            string strFileName = Path.GetFileName(fUpload.FileName);
            string XY = DropDownLisxx.SelectedValue.ToString();
            string YX = DropDownListxy.SelectedValue.ToString();

            string role = DropDownListxyrole.SelectedValue.ToString();
            tishi.Visible = true;
            if (XY == "0")
            {

                tishi.Text = "学校不能为空！"; 
               }
            else if (YX == "0")
            {
               
                tishi.Text = "学院不能为空！";
            }
            //else if (role == "0")
            //{
              
            //    tishi.Text = "角色不能为空！";

            //}


            else if (strFileName.ToString() == "")
            {
               
                tishi.Text = "Excel数据表不能为空！";

            }
            else
            {
              
                tishi.Text = "数据导入中，请稍后.......";


                //定义Excel表

              
                Common common = new Common();
                SqlConnection conn = common.coon();
                conn.Open();

                //以下代码实现将Excel文件上传到服务器
                string filePath = Server.MapPath("/Excel/");
                UP_FILE.PostedFile.SaveAs(filePath + strFileName);
                //以下代码使用Aspose技术实现从磁盘中读入Excel表格
                //创建一个工作簿对象，并使用Excel文件路径打开一个Excel文件
                Workbook workbook = new Workbook();
                workbook.Open(filePath + strFileName);
                //取得指向sheet0的Worksheet对象
                Worksheet worksheet = workbook.Worksheets[0];
                Cells cells = worksheet.Cells;
                //循环读取所有行，并读取Excel文件中的每一列。在这里可以实现不同数据格式的转换


                if (role == "" || role == "0")
                { role = "C45339F4-36F1-43DB-ACD9-92FC343D4CE7"; }
                StringBuilder insertSql = new StringBuilder();
          

                    for (int i = 1; i < cells.Rows.Count; i++)
                    {
                        string ZGBH = cells[i, 0].Value.ToString();
                        string ZGXM = cells[i, 1].Value.ToString();
                        //将从Excel文件中读取的用户姓名，试卷名称，考试分数，考试时间添加到SQL Server事先建立好的数据表ExcelData中
                        string sqlstr = "insert into Bap_USER(UserID,ZGBH,ZGXM,Pass,XX,XY,Role) select newid(), '" + ZGBH + "','" + ZGXM + "','On8U4+dy1Rs=','" + XY + "','" + YX + "','" + role + "' ";
                        insertSql.Append(sqlstr);

                    }

                    if (DbHelperSQL.ExecuteSql(insertSql.ToString()) > 0)
                    {
                        tishi.Text = "数据导入成功!";
                    }
                    else
                    {

                        tishi.Text = "数据导入失败,请重试!";
                    }

                    File.Delete(filePath + strFileName);

                    // Response.Write("<script >alert('数据导入成功.！')</script>");
                    //Response.Write("<script>document.location=document.location;</script>");
                    conn.Close();//关闭SQL Server数据库的连接

           
            }
        }

        public DataSet GetDataSet(string sqlstr)
        {

            Common common = new Common();
            SqlConnection conn = common.coon();
            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, conn);
            DataSet ds = new DataSet();
            myda.Fill(ds);
            return ds;
        }

    }


}