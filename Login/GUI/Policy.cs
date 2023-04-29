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
    public partial class Policy : Form
    {
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User id=sys;Password=1;DBA Privilege=SYSDBA;";
        public Policy()
        {
            InitializeComponent();
        }

        private void Policy_Load(object sender, EventArgs e)
        {
            policyuser();
            policyhethong();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPolicy m = new AddPolicy();
            m.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeletePolicy m = new DeletePolicy();
            m.Show();
        }
        public void policyuser()
        {
            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "select OBJECT_SCHEMA,OBJECT_NAME,POLICY_OWNER,POLICY_NAME,ENABLED,SEL ,INS,UPD,DEL,AUDIT_TRAIL,POLICY_COLUMN_OPTIONS from ALL_AUDIT_POLICIES";
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
        public void policyhethong()
        {
            try
            {
                con.ConnectionString = connString;
                con.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng NHANVIEN
                string sql = "select session_id,db_user,timestamp,os_user,userhost,object_schema,policy_name,sql_text,statement_type,extended_timestamp,current_user from DBA_FGA_AUDIT_TRAIL";
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}
