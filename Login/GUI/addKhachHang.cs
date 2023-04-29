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
    public partial class addKhachHang : Form
    {
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";
        public addKhachHang()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(connString))
            {
                using (OracleCommand cmd = new OracleCommand("THEM_KHACH_HANG", con))
                {
                    
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_MaKH", OracleDbType.Varchar2, 6).Value = textBox1.Text;
                        cmd.Parameters.Add("p_HoKH", OracleDbType.Varchar2, 20).Value = textBox2.Text;
                        cmd.Parameters.Add("p_TenKH", OracleDbType.Varchar2, 25).Value = textBox3.Text;
                        cmd.Parameters.Add("p_NgaySinhKH", OracleDbType.Date).Value = DateTime.Parse(textBox4.Text);
                        cmd.Parameters.Add("p_GioiTinhKH", OracleDbType.Varchar2, 25).Value = textBox5.Text;
                        cmd.Parameters.Add("p_DiaChiKH", OracleDbType.Varchar2, 100).Value = textBox6.Text;
                        cmd.Parameters.Add("p_EmailKH", OracleDbType.Varchar2, 25).Value = textBox7.Text;
                        cmd.Parameters.Add("p_SDTKH", OracleDbType.Varchar2, 20).Value = textBox8.Text;
                        cmd.Parameters.Add("p_CCCDKH", OracleDbType.Varchar2, 20).Value = textBox9.Text;

                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                      
                            MessageBox.Show("Thêm khách hàng thành công!");
                     
                        
                }
            }

        }

        private void addKhachHang_Load(object sender, EventArgs e)
        {

        }
    }
}
