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
namespace JiaoShiXinXiTongJi.USER
{
    public partial class myinfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 显示信息
            if (!Page.IsPostBack)
            {

                string userid = Session["UserID"].ToString();
                string selectuser = "select bu.*,br.ROLE AS role_,bs.MC AS XY_,bss.MC AS YX_ ,xl.BM as xl,zc.BM as zc from Bap_User bu LEFT JOIN Bap_Role br ON bu.ROLE=br.id LEFT JOIN dbo.Bap_School bs ON bu.XX=bs.ID LEFT JOIN dbo.Bap_School bss ON bu.XY=bss.ID left join Tab_XLBM xl on bu.xl=xl.BM left join Tab_ZCBM zc on bu.zcjb=zc.BM  where bu.UserID='" + userid + "'";
                SqlDataReader dr = DbHelperSQL.ExecuteReader(selectuser);
                while (dr.Read())
                {
                    zgbh.Text = dr["ZGBH"].ToString();

                    zgxm.Text = dr["ZGXM"].ToString();
                    csrq.Text = dr["CSRQ"].ToString();
                    nl.Text = dr["NL"].ToString();
                    mz.Text = dr["MZ"].ToString();
                    jg.Text = dr["JG"].ToString();
                    jg.Text = dr["JG"].ToString();
                    zw.Text = dr["ZW"].ToString();
                    dzjz.Text = dr["DZJZ"].ToString();
                    cid.Text = dr["CID"].ToString();
                    rzsj.Text = dr["RZSJ"].ToString();
                    lxdh.Text = dr["Tel"].ToString();
                    email.Text = dr["Email"].ToString();
                    Session["xb"] = dr["XB"].ToString();
                    if (dr["xl"].ToString() == "" || dr["xl"].ToString() == null)
                    {
                        Session["xl"] = "8";

                    }
                    else
                    {
                        Session["xl"] = dr["xl"].ToString();
                    }
                    if (dr["zc"].ToString() == "" || dr["zc"].ToString() == null)
                    {
                        Session["zc"] = "8";
                    }
                    else
                    {
                        Session["zc"] = dr["zc"].ToString();
                    }
                    role.Text = dr["Role_"].ToString();
                    Session["role"] = dr["Role"].ToString();

                }

                dr.Close();
                bindxb();
                Bindxueli();
                Bindzhicheng();
                Bindxuexiao();
                Bindxueyuan();

            }
        }
            #endregion
        #region 数据绑定
        private void Bindxueyuan()
        {

            Common common = new Common();
            SqlConnection Conn = common.coon();
            string strSql = "select * from Bap_School where ID='" + Session["XY"].ToString() + "' ";
            SqlDataAdapter adp = new SqlDataAdapter(strSql, Conn);
            Conn.Open();
            DataSet dt = new DataSet();
            adp.Fill(dt, "MyTable");
            DropDownList_xy.AppendDataBoundItems = true;
            DropDownList_xy.DataSource = dt.Tables["MyTable"].DefaultView;
            DropDownList_xy.DataTextField = "MC";
            DropDownList_xy.DataValueField = "ID";
            DropDownList_xy.DataBind();
            Conn.Close();
        }

        private void Bindxuexiao()
        {

            Common common = new Common();
            SqlConnection Conn = common.coon();
            string strSql = "select * from Bap_School where ID='" + Session["XX"].ToString() + "' ";
            SqlDataAdapter adp = new SqlDataAdapter(strSql, Conn);
            Conn.Open();
            DataSet dt = new DataSet();
            adp.Fill(dt, "MyTable");
            DropDownList_xx.AppendDataBoundItems = true;
            DropDownList_xx.DataSource = dt.Tables["MyTable"].DefaultView;
            DropDownList_xx.DataTextField = "MC";
            DropDownList_xx.DataValueField = "ID";

            DropDownList_xx.DataBind();

            Conn.Close();
        }

        private void Bindzhicheng()
        {

            Common common = new Common();
            SqlConnection Conn = common.coon();
            string strSql = "select * from Tab_ZCBM ";
            SqlDataAdapter adp = new SqlDataAdapter(strSql, Conn);
            Conn.Open();
            DataSet dt = new DataSet();
            adp.Fill(dt, "MyTable");
            DropDownList_zc.SelectedValue = Session["zc"].ToString();
            DropDownList_zc.AppendDataBoundItems = true;
            DropDownList_zc.DataSource = dt.Tables["MyTable"].DefaultView;
            DropDownList_zc.DataTextField = "MC";
            DropDownList_zc.DataValueField = "BM";
            DropDownList_zc.DataBind();
            Conn.Close();
        }

        private void Bindxueli()
        {
            Common common = new Common();
            SqlConnection Conn = common.coon();
            string strSql = "select * from Tab_XLBM ";
            SqlDataAdapter adp = new SqlDataAdapter(strSql, Conn);
            Conn.Open();
            DataSet dt = new DataSet();
            adp.Fill(dt, "MyTable");
            DropDownList_xl.SelectedValue = Session["xl"].ToString();
            DropDownList_xl.AppendDataBoundItems = true;
            DropDownList_xl.DataSource = dt.Tables["MyTable"].DefaultView;
            DropDownList_xl.DataTextField = "MC";
            DropDownList_xl.DataValueField = "BM";
            DropDownList_xl.DataBind();
            Conn.Close();
        }

        private void bindxb()
        {
            xb.Text = Session["xb"].ToString();
        }
        #endregion

        #region 提交数据
        protected void submit_Click(object sender, EventArgs e)
        {
            string userid = Session["UserID"].ToString();
            string zgbh_ = zgbh.Text.ToString();
            string zgxm_ = zgxm.Text.ToString();
            string xb_ = xb.SelectedValue.ToString();
            string csrq_ = csrq.Text.ToString();
            string nl_ = nl.Text.ToString();
            string mz_ = mz.Text.ToString();
            string jg_ = jg.Text.ToString();
            string xx = DropDownList_xx.SelectedValue.ToString();
            if (xx == "" || xx == null)
            {
                xx = "00000000-0000-0000-0000-000000000000";
            }
            string xy = DropDownList_xy.SelectedValue.ToString();
            if (xy == "" || xy == null)
            {
                xy = "00000000-0000-0000-0000-000000000000";
            }
            string xl_ = DropDownList_xl.SelectedValue.ToString();
            string zw_ = zw.Text.ToString();
            string zcjb_ = DropDownList_zc.SelectedValue.ToString();
            string dzjz_ = dzjz.Text.ToString();
            string cid_ = cid.Text.ToString();
            string rzsj_ = rzsj.Text.ToString();
            string lxdh_ = lxdh.Text.ToString();
            string email_ = email.Text.ToString();
            string role_ = Session["role"].ToString();
            string insert = "update bap_user set ZGXM='" + zgxm_ + "',XB='" + xb_ + "',CSRQ='" + csrq_ + "',NL='" + nl_ + "',MZ='" + mz_ + "',JG='" + jg_ + "',XX='" + xx + "',XY='" + xy + "',XL='" + xl_ + "',ZW='" + zw_ + "',ZCJB='" + zcjb_ + "',DZJZ='" + dzjz_ + "',CID='" + cid_ + "',RZSJ='" + rzsj_ + "',Tel='" + lxdh_ + "',Email='" + email_ + "' where UserID='" + userid + "'";
            DbHelperSQL.ExecuteSql(insert);
            Response.Write("<script> alert('修改信息成功！')</script>");
            Response.Write("<script>document.location=document.location;</script>");
        }
        #endregion
    }
}
