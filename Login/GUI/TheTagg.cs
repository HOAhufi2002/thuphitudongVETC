using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
namespace Login
{
    public partial class TheTagg : Form
    {
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";
        public TheTagg()
        {
            InitializeComponent();
        }

        private void TheTagg_Load(object sender, EventArgs e)
        {
            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "SELECT * FROM THETAG";
                OracleCommand cmd = new OracleCommand(sql, con);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable table = new DataTable();
                da.Fill(table);
                // Hiển thị dữ liệu lên DataGridView
                dataGridView1.DataSource = table;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối hoặc truy vấn: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GUI.addTheTag frm = new GUI.addTheTag();
            frm.Show();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                // Lấy giá trị mới của ô đã chỉnh sửa
                string maVachThe = dataGridView1.Rows[e.RowIndex].Cells["MaVachThe"].Value.ToString();
                string maXe = dataGridView1.Rows[e.RowIndex].Cells["MaXe"].Value.ToString();
                string maKH = dataGridView1.Rows[e.RowIndex].Cells["MaKH"].Value.ToString();
                decimal soDuTK = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells["SoDuTK"].Value.ToString());

                // Thực hiện cập nhật vào CSDL
                con.Open();
                string sql = $"UPDATE THETAG SET MaXe = '{maXe}', MaKH = '{maKH}', SoDuTK = {soDuTK} WHERE MaVachThe = '{maVachThe}'";
                OracleCommand cmd = new OracleCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close();

                // Hiển thị lại dữ liệu trên DataGridView
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dữ liệu: " + ex.Message);
            }
            
        }
    
       
    }
}
