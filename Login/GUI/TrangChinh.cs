using System;
using System.Windows.Forms;
namespace Login
{
    public partial class TrangChinh : Form
    {
        private string quyen;
        public TrangChinh(string quyen)
        {
            this.quyen = quyen;
            InitializeComponent();
            if (quyen == "admin")
            {
                // Hiển thị tất cả các control trên giao diện của MainForm
                btn_KhachHang.Visible = true;
                
            }
            else if (quyen == "nhanvien")
            {
                // Chỉ hiển thị một số control trên giao diện của MainForm
                
                btn_VeThu.Visible = false;
                button7.Visible = false;
                button6.Visible = false;
                button5.Visible = false;
                button4.Visible = false;
                button3.Visible = false;
                button2.Visible = false;
                button1.Visible = false;
                btn_Xe.Visible = false;
                btn_Thetag.Visible = false;



            }
            else
            {
                // Hiển thị thông báo lỗi nếu quyền hạn không hợp lệ
                MessageBox.Show("Quyền hạn không hợp lệ");
                this.Close();
            }
        }
        private Form currentFormChild;
        

        private void OpenChildForm(Form childForm)
        {
            if(currentFormChild!=null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel3.Controls.Add(childForm);
            panel3.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TrangChinh_Load(object sender, EventArgs e)
        {
                
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_TramThuPhi_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TramThuPhi(quyen));
        }

        private void btn_TinhThanh_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TinhThanh());
        }

        private void btn_KhachHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new KhachHang(quyen));
        }

        private void btn_Thetag_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TheTagg());
        }

        private void btn_VeThu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new VeThu());
        }

        private void btn_Xe_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Xe());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Policy());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new GUI.TableSpace());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new GUI.Profile());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new GUI.User());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        { 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenChildForm(new GUI.PGA());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenChildForm(new GUI.SGA());
        }

        private void button8_Click(object sender, EventArgs e)
        {
           
                // đóng MainForm
                this.Close();

                // mở lại LoginForm
                GUI.Login loginForm = new GUI.Login();
                loginForm.Show();
            

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
