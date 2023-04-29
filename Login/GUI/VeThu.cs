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
using Microsoft.Office.Interop.Excel;
using System.IO;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Globalization;

namespace Login
{
    public partial class VeThu : Form
    {
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";
        public VeThu()
        {
            InitializeComponent();
        }
        void hienthi()
        {
            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "SELECT * FROM VETHu";
                OracleCommand cmd = new OracleCommand(sql, con);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                System.Data.DataTable table = new System.Data.DataTable();
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
        private void VeThu_Load(object sender, EventArgs e)
        {
            hienthi();
            loadcbb();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            // Khởi tạo đối tượng Excel
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                // Tạo workbook mới
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);
                // Tạo worksheet mới
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

                // Lấy số hàng và số cột của DataGridView
                int rowsCount = dataGridView1.Rows.Count;
                int columnsCount = dataGridView1.Columns.Count;

                // Thêm tiêu đề cho các cột trong worksheet
                for (int i = 0; i < columnsCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }

                // Thêm dữ liệu từ DataGridView vào worksheet
                for (int i = 0; i < rowsCount; i++)
                {
                    for (int j = 0; j < columnsCount; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[j].Value != null)
                        {
                            worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        }
                        else
                        {
                            worksheet.Cells[i + 2, j + 1] = "";
                        }
                    }
                }

                // Lưu workbook và đóng Excel
                workbook.SaveAs("C:\\Users\\kingg\\Downloads\\DoAn\\data1.xlsx");
                workbook.Close();
                excel.Quit();
                MessageBox.Show(" thành công ");
               
            }
            catch
            {
                MessageBox.Show(" file đã tồn tại ");

            }

        }

        private void btn_nhap_Click(object sender, EventArgs e)
        {
            try
            {
                /// Khởi tạo đối tượng OpenFileDialog
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

                // Nếu người dùng chọn file và nhấn nút OK
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Chuỗi kết nối tới CSDL Oracle
                    string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";

                    // Tạo đối tượng OracleConnection
                    OracleConnection connection = new OracleConnection(connString);
                    connection.Open();

                    // Tạo đối tượng OracleCommand để thực hiện truy vấn SQL
                    OracleCommand cmd = new OracleCommand("SELECT * FROM vethu", connection);

                    // Tạo đối tượng OracleDataAdapter để đọc dữ liệu từ CSDL Oracle
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                    // Tạo DataTable để lưu trữ dữ liệu
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);

                    // Tạo đối tượng ExcelPackage
                    using (var excel = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                    {
                        // Set LicenseContext
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        var worksheet = excel.Workbook.Worksheets.First();
                        var rows = worksheet.Dimension.Rows;
                        var cols = worksheet.Dimension.Columns;

                        // Tạo đối tượng OracleBulkCopy để insert dữ liệu từ file Excel vào CSDL Oracle
                        OracleBulkCopy bulkCopy = new OracleBulkCopy(connection);
                        bulkCopy.DestinationTableName = "vethu";

                        // Khai báo cột trong file Excel tương ứng với các cột trong bảng vethu của CSDL Oracle
                        bulkCopy.ColumnMappings.Add("MAVETHU", "MAVETHU");
                        bulkCopy.ColumnMappings.Add("MATRAM", "MATRAM");
                        bulkCopy.ColumnMappings.Add("MAVACHTHE", "MAVACHTHE");
                        bulkCopy.ColumnMappings.Add("NGAYGIOTHU", "NGAYGIOTHU");

                        for (int row = 2; row <= rows; row++)
                        {
                            // Tạo một đối tượng DataRow mới
                            DataRow newRow = dt.NewRow();

                            // Đọc giá trị từ các ô trong file Excel và gán vào các cột của đối tượng DataRow
                            newRow["MAVETHU"] = worksheet.Cells[row, 1].Value.ToString();
                            newRow["MATRAM"] = worksheet.Cells[row, 2].Value.ToString();
                            newRow["MAVACHTHE"] = worksheet.Cells[row, 3].Value.ToString();
                            newRow["NGAYGIOTHU"] = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString());

                            // Thêm đối tượng DataRow vào DataTable
                            dt.Rows.Add(newRow);
                        }

                        // Sử dụng OracleDataAdapter để update dữ liệu từ DataTable vào CSDL Oracle
                        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                        adapter.Update(dt);

                        // Đóng kết nối và giải phóng bộ nhớ
                        connection.Close();
                        connection.Dispose();
                    }

                    MessageBox.Show("Imported successfully!", "Information");
                    hienthi();
                }
            }
            catch
            {
                MessageBox.Show(" thất bại ");
            }

        }
        void loadcbb()
        {
            con.ConnectionString = connString;
            using (var conn = new OracleConnection(connString)) // replace connectionString with your database connection string
            {
                conn.Open();
                using (var cmd = new OracleCommand("SELECT MaTram FROM TRAMTHUPHIBOT", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetString(0)); // replace comboBox1 with the name of your ComboBox
                        }
                    }
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            con.ConnectionString = connString;
            string maTram = comboBox1.Text;
            

            using (OracleConnection con = new OracleConnection(connString))
            {
                con.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(*) AS TongSoVe FROM VETHU WHERE MaTram = :maTram", con);
                cmd.Parameters.Add(new OracleParameter(":maTram", maTram));

                int i = Convert.ToInt32(cmd.ExecuteScalar());
                MessageBox.Show($"Tong so ve cua tram {maTram} la: {i}");
            }


        }

    }
}
