using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace Map_download
{
    public partial class frmMapDetail : Form
    {
        DataTable c_dataTable;
        int? c_rowIndex = null;
        public frmMapDetail(DataTable dt)
        {
            InitializeComponent();
            c_dataTable = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (c_rowIndex == null )
            {
                MessageBox.Show("กรุณาเลือก Map Data ที่ต้องการโหลดที่ตาราง");
                return;
            }
            if (MessageBox.Show("คุณต้องการโหลด Map นี้ใช่หรือไม่ ??", "",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            
            string folderMode = @"d:\MapData";
            if (!Directory.Exists(folderMode))
            {
                Directory.CreateDirectory(folderMode);
            }

            int rowIndex = (int)c_rowIndex;

            var ProcMode = c_dataTable.Rows[rowIndex]["ProcessMode"];
            byte[] binDownload = (byte[])c_dataTable.Rows[rowIndex]["MAPData"];
            string UnZipDir = folderMode + @"\" + c_dataTable.Rows[rowIndex]["LotNo"] + "_" + ProcMode + ".zip";
            string lotNo = c_dataTable.Rows[rowIndex]["LotNo"].ToString();
            File.WriteAllBytes(UnZipDir, binDownload);

            string mapPath = folderMode + @"\" + lotNo;
            if (Directory.Exists(mapPath))
            {
                Directory.Delete(mapPath, true);
            }
            ZipFile.ExtractToDirectory(UnZipDir, mapPath);
            File.Delete(UnZipDir);

            MessageBox.Show("Download เรียบร้อยดี :D...");
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = c_dataTable;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            c_rowIndex = e.RowIndex;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
