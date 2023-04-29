using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login.GUI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";
        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            con.ConnectionString = connString;
            con.Open();
            // Truy vấn cơ sở dữ liệu để lấy thông tin tài khoản của người dùng
            string sql = "SELECT * FROM taikhoan WHERE username=:username AND password=:password";
            OracleCommand cmd = new OracleCommand(sql, con);
            cmd.Parameters.Add(new OracleParameter("username", textBox1.Text));
            cmd.Parameters.Add(new OracleParameter("password", textBox2.Text));
            OracleDataReader dr = cmd.ExecuteReader();

            // Kiểm tra thông tin đăng nhập có trùng khớp với thông tin trong cơ sở dữ liệu không
            if (dr.Read())
            {
                string quyen = dr["quyen"].ToString();

                // Chuyển đến MainForm và hiển thị các chức năng tương ứng với quyền hạn của người dùng
                TrangChinh mainForm = new TrangChinh(quyen);
               
                this.Hide();
                mainForm.Show();
            }
            else
            {
                // Hiển thị thông báo lỗi và yêu cầu nhập lại thông tin đăng nhập
                MessageBox.Show("Thông tin đăng nhập không hợp lệ. Vui lòng nhập lại.");
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }

            // Đóng kết nối và giải phóng các tài nguyên
            dr.Close();
            cmd.Dispose();
            con.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
    }
    }
