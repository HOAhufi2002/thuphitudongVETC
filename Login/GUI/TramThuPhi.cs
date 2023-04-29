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
    public partial class TramThuPhi : Form
    {
        public TramThuPhi(string quyen)
        {
           
            InitializeComponent();
            if (quyen == "admin")
            {
                // Hiển thị tất cả các control trên giao diện của MainForm
                textBox1.Visible = true;
                textBox2.Visible = true;
                
            }
            else if (quyen == "nhanvien")
            {
                // Chỉ hiển thị một số control trên giao diện của MainForm
                textBox1.Visible = false;
                textBox2.Visible = false;
                button2.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
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
        private DataTable ExecuteQuery(string query)
        {
            DataTable result = new DataTable();
            try
            {
                con.Open();
                OracleCommand cmd = new OracleCommand(query, con);
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public void tinhvaluuv()
        {
            string maThe = textBox1.Text;
            string query = "SELECT X.*, T.SoDuTK, K.HoKH, K.TenKH FROM XE X, THETAG T, KHACHHANG K WHERE X.MaXe = T.MaXe AND K.MaKH = X.MaKH and T.MaVachThe = '" + maThe + "'";
            DataTable result = ExecuteQuery(query);
            if (result.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy xe nào đi qua trạm thu phí với mã thẻ này.");
            }
            else
            {
                DataRow row = result.Rows[0];
                string message = "Tên Chủ Xe " + row["HOKH"].ToString() + row["TENKH"].ToString() + " Biển số " + row["BienSoXe"].ToString() + " (" + row["LoaiXe"].ToString() + ", " + row["Mau"].ToString() + ") ";
                int soDu = Convert.ToInt32(row["SoDuTK"]);
                if (soDu < 100000)
                {
                    message += " Số dư trong thẻ của bạn không đủ để thanh toán. Vui lòng nạp thêm tiền.";
                }
                else
                {
                    try
                    {
                        string maTram = textBox2.Text; // Lấy mã trạm từ textbox 2
                        string maVeThu = "VT" + (new Random().Next(100, 999)).ToString(); // Mã vé thu ngẫu nhiên
                        string maVachThe = maThe;
                        DateTime ngayGioThu = DateTime.Now;
                        // Thực thi truy vấn INSERT vào bảng VETHU
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "INSERT INTO VETHU(MaVeThu, MaTram, MaVachThe, NgayGioThu) VALUES(:maVeThu, :maTram, :maVachThe, :ngayGioThu)";
                        cmd.Parameters.Add(":maVeThu", maVeThu);
                        cmd.Parameters.Add(":maTram", maTram);
                        cmd.Parameters.Add(":maVachThe", maVachThe);
                        cmd.Parameters.Add(":ngayGioThu", ngayGioThu);
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu xuống cơ sở dữ liệu: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                MessageBox.Show(message);
            }
        }
        public void TONG()
        {
            label5.Text = (dataGridView1.Rows.Count).ToString();
        }
        public void tinhvaluuve()
        {
            string maThe = textBox1.Text;
            string query = "SELECT X.*, T.SoDuTK, K.HoKH, K.TenKH FROM XE X, THETAG T, KHACHHANG K WHERE X.MaXe = T.MaXe AND K.MaKH = X.MaKH and T.MaVachThe = '" + maThe + "'";
            DataTable result = ExecuteQuery(query);
            if (result.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy xe nào đi qua trạm thu phí với mã thẻ này.");
            }
            else
            {
                DataRow row = result.Rows[0];
                string message = "Tên Chủ Xe " + row["HOKH"].ToString() + row["TENKH"].ToString() + " Biển số " + row["BienSoXe"].ToString() + " (" + row["LoaiXe"].ToString() + ", " + row["Mau"].ToString() + ") đã đi qua trạm thu phí.";
                int soDu = Convert.ToInt32(row["SoDuTK"]);
                if (soDu < 100000)
                {
                    message += " Số dư trong thẻ của bạn không đủ để thanh toán. Vui lòng nạp thêm tiền.";
                }
                else
                {
                    try
                    {
                        string maVeThu = "VT" + (new Random().Next(100, 999)).ToString();
                        string maTram = textBox2.Text;
                        string maVachThe = maThe;
                        DateTime ngayGioThu = DateTime.Now;

                        using (OracleConnection con = new OracleConnection(connString))
                        {
                            using (OracleCommand cmd = new OracleCommand("INSERT_VETHU", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("MaVeThu", OracleDbType.Varchar2).Value = maVeThu;
                                cmd.Parameters.Add("MaTram", OracleDbType.Varchar2).Value = maTram;
                                cmd.Parameters.Add("MaVachThe", OracleDbType.Varchar2).Value = maVachThe;
                                cmd.Parameters.Add("NgayGioThu", OracleDbType.Date).Value = ngayGioThu;

                                con.Open();
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Lưu thông tin vé thu thành công!");
                                }
                                else
                                {
                                    MessageBox.Show("Lưu thông tin vé thu thất bại!");
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu xuống cơ sở dữ liệu: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
        private void TramThuPhi_Load(object sender, EventArgs e)
        {
            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "SELECT * FROM tramthuphibot";
                OracleCommand cmd = new OracleCommand(sql, con);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable table = new DataTable();
                da.Fill(table);
                // Hiển thị dữ liệu lên DataGridView
                dataGridView1.DataSource = table;
                con.Close();
                TONG();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối hoặc truy vấn: " + ex.Message);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {

            tinhvaluuv();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                // Lấy giá trị từ TextBox và tạo truy vấn SQL
                string TINH = textBox3.Text;
                string sql = "SELECT * FROM TRAMTHUPHIBOT WHERE MATINH LIKE '%" + TINH + "%'";

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
    }
}
