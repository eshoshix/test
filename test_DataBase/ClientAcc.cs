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
    public partial class Client_acc : MetroFramework.Forms.MetroForm
    {
        DataBase DataBase = new DataBase();
        int CurrentClientID;
        public Client_acc(int ID)
        {
            CurrentClientID = ID;   
            InitializeComponent();
        
        
        
        }
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;  
            panel2.Controls.Clear();
            panel2.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void Client_acc_Load(object sender, EventArgs e)
        {
            label123();
            label1.Text = "Главное меню";

            MainMenu_UserControl mainMenu = new MainMenu_UserControl(CurrentClientID);
            addUserControl(mainMenu);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NW_UserControl userControl = new NW_UserControl(CurrentClientID);
             addUserControl(userControl);
            label1.Text = "Запись на прием";
        }

        private void label123()
        {
            string querystring = $"select Фамилия,Имя,Отчество from Пациент where [ID_Пациента] = '{CurrentClientID}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                string column1Data = reader["Фамилия"].ToString();
                string column2Data = reader["Имя"].ToString();
                string column3Data = reader["Отчество"].ToString();

                label2.Text =  $"{column1Data} {column2Data} {column3Data}";

            }

           


        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            Doctors_UserControl userControl = new Doctors_UserControl();
            addUserControl(userControl);
            label1.Text = "О врачах";


        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверенны, что хотите выйти из аккаунта?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                this.Close();

            }
            return;
          

          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "Расписание врачей";
            DatesAll_UserControl userControl = new DatesAll_UserControl();
            addUserControl(userControl);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = "Медицинская карта";
            int ID = CurrentClientID;

            MedCard_UserControl ms = new MedCard_UserControl(ID);
            addUserControl(ms);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label1.Text = "Главное меню";
            int ID = CurrentClientID;
            MainMenu_UserControl mm = new MainMenu_UserControl(ID);
            addUserControl(mm);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString("HH:mm.ss");
            label4.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }
    }
}
