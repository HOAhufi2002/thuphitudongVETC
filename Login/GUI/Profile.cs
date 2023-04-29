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
    public partial class Profile : Form
    {
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User id=sys;Password=1;DBA Privilege=SYSDBA;";
        public Profile()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            con.ConnectionString = connString;
            con.Open();
                string x = textBox1.Text;
                string SESSIONS_PER_USER = textBox2.Text;
                string FAILED_LOGIN_ATTEMPTS = textBox3.Text;
                string PASSWORD_LIFE_TIME = textBox4.Text;
                string IDLE_TIME = textBox5.Text;
                string create = $"CREATE  PROFILE {x} LIMIT SESSIONS_PER_USER {SESSIONS_PER_USER} FAILED_LOGIN_ATTEMPTS {FAILED_LOGIN_ATTEMPTS}PASSWORD_LIFE_TIME {PASSWORD_LIFE_TIME}IDLE_TIME {IDLE_TIME}";

                using (OracleConnection connection = new OracleConnection(connString))
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand(create, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Tạo  thành công");
                        loadprofile();
                    }
                }

          

        }

        private void Profile_Load(object sender, EventArgs e)
        {
            loadprofile();
        }
        public void loadprofile()
        {

            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "SELECT * FROM DBA_profiles";
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
                MessageBox.Show("Chúc mừng bạn đã tạo profile thành công: " );
            }
        }
    }
}
