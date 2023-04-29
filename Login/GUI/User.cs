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
    public partial class User : Form
    {
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User id=sys;Password=1;DBA Privilege=SYSDBA;";
        public User()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = textBox1.Text;
            string b = textBox2.Text;
            string c = textBox3.Text;
            string d = "admin";

            try
            {
                using (OracleConnection connection = new OracleConnection(connString))
                {
                    connection.Open();

                    // Tạo user mới
                    string createUserQuery = $"CREATE USER {a} IDENTIFIED BY {b} PROFILE {c}";
                    using (OracleCommand command = new OracleCommand(createUserQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    // Thêm user vào bảng tài khoản
                    taousser();

                    MessageBox.Show("Tạo tài khoản thành công.");
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }



        }
        public void loadprofile()
        {

            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "SELECT * FROM dba_users";
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

        private void User_Load(object sender, EventArgs e)
        {
            loadprofile();
            loadcbb();
        }
        void taousser()
        {
            OracleConnection con = new OracleConnection();
            string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";

            using (OracleConnection connection = new OracleConnection(connString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("SP_INSERT_USER", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_USERNAME", OracleDbType.Varchar2).Value = textBox1.Text;
                    command.Parameters.Add("p_PASSWORD", OracleDbType.Varchar2).Value = textBox2.Text;
                    command.Parameters.Add("p_QUYEN", OracleDbType.Varchar2).Value = comboBox1.Text.ToString();

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            

        }
        void loadcbb()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("nhanvien");
            comboBox1.Items.Add("admin");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }
    }
}
