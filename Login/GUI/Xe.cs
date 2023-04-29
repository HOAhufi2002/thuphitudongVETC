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
    public partial class Xe : Form
    {
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";
        public Xe()
        {
            InitializeComponent();
        }

        private void Xe_Load(object sender, EventArgs e)
        {
            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "SELECT * FROM XE";
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
    }
}
