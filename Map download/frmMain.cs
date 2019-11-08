using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
namespace Map_download
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            string Password = Interaction.InputBox("กรุณากรอก Password");
            if (Password != "000652")
            {
                MessageBox.Show("ใส่รหัสไม่ถูกต้องกรุณากรอกใหม่");
                return;
            }

            if (tbLotNo.Text.Length != 10)
            {
                MessageBox.Show("LotNo: " + tbLotNo.Text +" ไม่ถูกต้องกรุณาตรวจสอบ");
                return;
            }

            DataTable dt =  GetDataTableFromMap_MapData(tbLotNo.Text);
            if (dt != null) // OK มีข้อมูล
            {
                using (frmMapDetail frm = new frmMapDetail(dt))
                {
                    frm.ShowDialog();
                }
            }
            else // NG ไม่มีข้อมูล 
            {
                MessageBox.Show("ไม่มีข้อมูล LotNo: " + tbLotNo.Text + " ในฐานข้อมูล Map_MapData Table");
            }
        }
        DataTable GetDataTableFromMap_MapData(string lotNo)
        {
            DataTable ret = null;
            using (SqlConnection con = new SqlConnection (Properties.Settings.Default.DbConnectionString))
            {
                con.Open();
                string sqlText = "SELECT MCNo, Process, ProcessMode, LotNo, MAPData, LotStartTime, LotEndTime, Remark FROM MAP_MAPData WHERE (LotNo = @LotNo) AND (NOT (Remark LIKE 'LotCancel') OR Remark IS NULL) ORDER BY LotEndTime ASC";
                SqlCommand cmd = new SqlCommand(sqlText, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@LotNo", SqlDbType.VarChar).Value = lotNo;
                SqlDataReader rd = cmd.ExecuteReader();
                DataTable datatable = new DataTable();
                datatable.Load(rd);
                if (datatable.Rows.Count != 0)
                {
                    ret = datatable;
                }
            }
            return ret;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string path = @"D:\MapData";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            System.Diagnostics.Process.Start(path);
        }
    }
}
