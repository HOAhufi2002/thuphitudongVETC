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
    public partial class addTheTag : Form
    {
        public addTheTag()
        {
            InitializeComponent();
        }
        OracleConnection con = new OracleConnection();
        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));Password=123;User ID=HR";
        private void button1_Click(object sender, EventArgs e)
        {
            con.ConnectionString = connString;
            con.Open();

            try
            {
                string maVachThe = textBox1.Text;
                string maXe = textBox2.Text;
                string maKH = textBox3.Text;
                decimal soDuTK = decimal.Parse(textBox4.Text);

                string sql = $"INSERT INTO THETAG (MaVachThe, MaXe, MaKH, SoDuTK) VALUES ('{maVachThe}', '{maXe}', '{maKH}', {soDuTK})";
                OracleCommand cmd = new OracleCommand(sql, con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("thành công");
            }
            catch
            {
                MessageBox.Show(" thất bại");
            }
        }
    }
}
