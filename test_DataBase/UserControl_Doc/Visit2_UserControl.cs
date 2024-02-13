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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace test_DataBase
{
    public partial class Visit2_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        int CurrentIDSick;
        int DoctorID;
        public Visit2_UserControl(int ID, int CurrentDoctor)
        {
            DoctorID = CurrentDoctor;  
            CurrentIDSick =ID;
            InitializeComponent();
        }

        private void Visit2_UserControl_Load(object sender, EventArgs e)
        {

            label();
            CurrentSickWork();

        }

        private void label()
        {


            string querystring = $"select ID_Больничного from Больничные where [ID_Больничного] = '{CurrentIDSick}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();

            using (SqlDataReader reader = command.ExecuteReader())
            {

                var SickExist = reader.Read();

                if (SickExist == false)
                {



                    return;
                }

                var Id = reader.GetInt32(0);

                label1.Text = "Прием номер: " + Id.ToString();


            }


        }



        private void CurrentSickWork()
        {

            string querystring = $"select ID_Больничного,Фамилия, Имя, Отчество, Наименование,ФИО,Дата_Начала_Заболевания,Дата_Конца_Заболевания from Больничные join Пациент on Пациент.ID_Пациента = Больничные.ID_Пациента inner join Диагноз on Диагноз.ID_Диагноза = Больничные.ID_Диагноза inner join Врач on Врач.ID_Врача = Больничные.ID_Врача where [ID_Больничного] = '{CurrentIDSick}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                string column1Data = reader["Фамилия"].ToString();

                string column2Data = reader["Имя"].ToString();

                string column3Data = reader["Отчество"].ToString();

                string column4Data = reader["Наименование"].ToString();

                string column5Data = reader["ФИО"].ToString();

                string column6Data = reader["Дата_Начала_Заболевания"].ToString();

                string column7Data = reader["Дата_Конца_Заболевания"].ToString();

                textBox1.Text = $"{column2Data} {column1Data} {column3Data}";
                textBox2.Text = $"{column4Data}";
                textBox3.Text = $"{column5Data}";
                textBox4.Text = $"{column6Data}";

            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            var date = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm");
            var dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string querystring = $"UPDATE Больничные SET Дата_конца_заболевания = CONVERT(DATETIME, '{date}', 120), Дата_Выписки = CONVERT(DATETIME, '{dateNow}', 120) where [ID_Больничного] = '{CurrentIDSick}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            command.ExecuteNonQuery();
            DataBase.openConnection();
            MessageBox.Show("Пациент успешно выписан с больничного", "Успех");


        }
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(userControl);
            userControl.BringToFront();
        }
        private void button2_Click(object sender, EventArgs e)
        {
           Visit_UserControl vs2 = new Visit_UserControl(DoctorID);
            addUserControl(vs2);    
            
              
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
