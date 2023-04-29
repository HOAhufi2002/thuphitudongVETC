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
using Oracle.ManagedDataAccess.Types;

namespace Login
{
    public partial class KhachHang : Form
    {
        public KhachHang(string quyen)
        {
            InitializeComponent();
            if (quyen == "admin")
            {
                // Hiển thị tất cả các control trên giao diện của MainForm
                button1.Visible = true;
                button3.Visible = true;
              
            }
            else if (quyen == "nhanvien")
            {
                // Chỉ hiển thị một số control trên giao diện của MainForm
                button1.Visible = false;
                button3.Visible = false;
           
            }
            else
            {
                // Hiển thị thông báo lỗi nếu quyền hạn không hợp lệ
                MessageBox.Show("Quyền hạn không hợp lệ");
                this.Close();
            }
        }
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";
        private void KhachHang_Load(object sender, EventArgs e)
        {
            loadkh();
        }
        public void loadkh()
        {
            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "SELECT * FROM khachhang";
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
        public void tongkhachhang()
        {
            using (OracleConnection con = new OracleConnection(connString))
            {
                using (OracleCommand cmd = new OracleCommand("TONG_KHACH_HANG", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("TONG_KHACH", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    OracleDecimal oracleDecimalValue = (OracleDecimal)cmd.Parameters["TONG_KHACH"].Value;
                    int tongKhachHang = oracleDecimalValue.ToInt32();

                    MessageBox.Show("Tổng số khách hàng: " + tongKhachHang);
                }
            }


        }
        private void button2_Click(object sender, EventArgs e)
        {
            tongkhachhang();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GUI.addKhachHang f = new GUI.addKhachHang();
            f.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            GUI.xoaKH f = new GUI.xoaKH();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            string maKH = row.Cells["MaKH"].Value.ToString();
            string hoKH = row.Cells["HoKH"].Value.ToString();
            string tenKH = row.Cells["TenKH"].Value.ToString();
            DateTime ngaySinhKH = DateTime.Parse(row.Cells["NgaySinhKH"].Value.ToString());
            string gioiTinhKH = row.Cells["GioiTinhKH"].Value.ToString();
            string diaChiKH = row.Cells["DiaChiKH"].Value.ToString();
            string emailKH = row.Cells["EmailKH"].Value.ToString();
            string sdtKH = row.Cells["SDTKH"].Value.ToString();
            string cccdKH = row.Cells["CCCDKH"].Value.ToString();

            using (OracleConnection con = new OracleConnection(connString))
            {
                using (OracleCommand cmd = new OracleCommand("SUA_KHACH_HANG", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_MaKH", OracleDbType.Varchar2, 6).Value = maKH;
                    cmd.Parameters.Add("p_HoKH", OracleDbType.Varchar2, 20).Value = hoKH;
                    cmd.Parameters.Add("p_TenKH", OracleDbType.Varchar2, 25).Value = tenKH;
                    cmd.Parameters.Add("p_NgaySinhKH", OracleDbType.Date).Value = ngaySinhKH;
                    cmd.Parameters.Add("p_GioiTinhKH", OracleDbType.Varchar2, 25).Value = gioiTinhKH;
                    cmd.Parameters.Add("p_DiaChiKH", OracleDbType.Varchar2, 100).Value = diaChiKH;
                    cmd.Parameters.Add("p_EmailKH", OracleDbType.Varchar2, 25).Value = emailKH;
                    cmd.Parameters.Add("p_SDTKH", OracleDbType.Varchar2, 20).Value = sdtKH;
                    cmd.Parameters.Add("p_CCCDKH", OracleDbType.Varchar2, 20).Value = cccdKH;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

            try
            {
                con.Open();

                // Lấy giá trị từ TextBox và tạo truy vấn SQL
                string tenKH = textBox1.Text;
                string sql = "SELECT * FROM KHACHHANG WHERE TenKH LIKE '%" + tenKH + "%'";

                // Thực hiện truy vấn và hiển thị kết quả lên DataGridView
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối
                con.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy hàng đã chọn
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                // Lấy giá trị của cột MaKH để xác định khách hàng cần xóa
                string maKH = row.Cells["MaKH"].Value.ToString();

                // Xóa khách hàng khỏi cơ sở dữ liệu
                using (OracleConnection connection = new OracleConnection(connString))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("DELETE FROM KhachHang WHERE MaKH = :maKH", connection);
                    command.Parameters.Add(new OracleParameter("maKH", maKH));
                    command.ExecuteNonQuery();
                }

                // Cập nhật lại DataGridView để hiển thị dữ liệu mới
                this.dataGridView1.Rows.Remove(row);
            }

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            try
            {
                con.Open();

                OracleCommand cmd = new OracleCommand("SELECT MINHHOA1.count_khachhang FROM dual", con);
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int count = dr.GetInt32(0);
                    MessageBox.Show("Tổng số khách " + count);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối
                con.Close();
            }
        }
    }
}
    





