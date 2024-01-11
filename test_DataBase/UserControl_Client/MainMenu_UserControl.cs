using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_DataBase
{
    public partial class MainMenu_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();

        int CurrentClient;
        public MainMenu_UserControl(int CurrentClientID)
        {
            CurrentClient = CurrentClientID;
            InitializeComponent();
        }

        private void MainMenu_UserControl_Load(object sender, EventArgs e)
        {
            label3.Text = "Статистика о заболеваниях";
            int ID = CurrentClient;
            Statistic_UserControl st = new Statistic_UserControl(ID);
            addUserControl(st);
        }
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
       
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "Информация о нашей поликлинике";
            AboutUs_UserControl mm = new AboutUs_UserControl();
            addUserControl(mm);


        }

        private void button3_Click(object sender, EventArgs e)
        {

            Add_Description_About_Clinic_UserControl mm = new Add_Description_About_Clinic_UserControl();
            addUserControl(mm);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = "Оценить приемы";
            int ID = CurrentClient;
            LastAppointment_UserControl la = new LastAppointment_UserControl(ID);
            addUserControl(la);
        }

        

        private void button3_Click_1(object sender, EventArgs e)
        {
            label3.Text = "Статистика о заболеваниях";
            int ID = CurrentClient;
            Statistic_UserControl st = new Statistic_UserControl(ID);
            addUserControl(st);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Add_Description_About_Clinic_UserControl asd = new Add_Description_About_Clinic_UserControl();
            addUserControl(asd);

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Add_Description_About_Clinic_UserControl ds = new Add_Description_About_Clinic_UserControl();
            addUserControl(ds);
        }
    }
}
