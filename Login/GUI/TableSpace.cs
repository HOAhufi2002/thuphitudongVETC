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

namespace Login.GUI
{
    public partial class TableSpace : Form
    {
        public TableSpace()
        {
            InitializeComponent();
        }
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User id=sys;Password=1;DBA Privilege=SYSDBA;";
        public void hienthiTB()
        {
            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "SELECT tablespace_name FROM dba_tablespaces";
                OracleCommand cmd = new OracleCommand(sql, con);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable table = new DataTable();
                da.Fill(table);
                // Hiển thị dữ liệu lên DataGridView
                dataGridView2.DataSource = table;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối hoặc truy vấn: " + ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.ConnectionString = connString;
            try
            {
                string tablespaceName = textBox1.Text;
                string dataFile = textBox2.Text;
                string size = textBox3.Text;
                string createTablespaceSQL = $"CREATE TABLESPACE {tablespaceName} DATAFILE '{dataFile}' SIZE {size}";

                using (OracleConnection connection = new OracleConnection(connString))
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand(createTablespaceSQL, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Tạo tablespace thành công");
                    }
                }
                hienthiTB(); 
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void TableSpace_Load(object sender, EventArgs e)
        {
            hienthiTB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.ConnectionString = connString;
            try
            {
                string tablespaceName = textBox1.Text;
                string Droptablespace= $"DROP TABLESPACE {tablespaceName}  INCLUDING CONTENTS";

                using (OracleConnection connection = new OracleConnection(connString))
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand(Droptablespace, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Tạo tablespace thành công");
                    }
                }
                hienthiTB();
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void Fill(object sender, DataGridViewAutoSizeColumnModeEventArgs e)
        {

        }
    }
}
