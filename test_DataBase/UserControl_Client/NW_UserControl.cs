﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace test_DataBase
{
    public partial class NW_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        int CurrentId;
        public NW_UserControl(int id)
        {
            CurrentId = id;
            InitializeComponent();
        }

        

        private void label123()
        {
            string querystring = $"select Фамилия,Имя,Отчество from Пациент where [ID_Пациента] = '{CurrentId}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                string column1Data = reader["Фамилия"].ToString();

                string column2Data = reader["Имя"].ToString();    

                string column3Data = reader["Отчество"].ToString();

            }


        }

        private void ComboBoxSpec()
        {


            string querystring2 = $"select distinct Специальность from Врач";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();


            List<string> listSpec = new List<string>();



            using (SqlDataReader reader = command2.ExecuteReader())
            {

                while (reader.Read())
                {

                    var doctorSpec1 = reader.GetString(0);
                    listSpec.Add((string)doctorSpec1);

                }


            }

            foreach (var item in listSpec)
            {

                comboBox1.Items.Add(item);

            }


        }




        public int GenerateRandom()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
            var doctor = comboBox3.SelectedIndex;
            if (doctor == -1)
            {

                MessageBox.Show("Выберите врача", "Ошибка");
                label1.ForeColor = Color.Red;
                label6.ForeColor = Color.Red;

                return;
            }
            
            if (checkDayAppointment() == false)
            {
                MessageBox.Show("Можно сделать только одну запись раз в 12 часов!", "Ошибка!");
                return;

            }



            if (checkOtherAppointment() == false)
            {
                MessageBox.Show("Эта дата посещения уже занята", "Ошибка!");
                label2.ForeColor = Color.Red;
                return;

            }


            if (checkTime() == false)
            {
                MessageBox.Show("Врач не работает в заданное время!", "Ошибка!");
                label2.ForeColor = Color.Red;
                return;

            }



            
            var date = dateTimePicker1.Value.ToString("yyyyMMdd") + " " + comboBox2.Text;
            if (date == "" & comboBox2.Text == "")
            {
                MessageBox.Show("Выберите дату посещения!", "Ошибка!");
                label2.ForeColor = Color.Red;
                return;

            }


            var doctorId = savedDoctorIDs2[doctor];
            string querystring3 = $"select [Стоимость посещения] from Врач where [ID_Врача] = '{doctorId}'";
            SqlCommand command3 = new SqlCommand(querystring3, DataBase.getConnection());
            DataBase.openConnection();




            var aim = richTextBox1.Text;
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                aim = "Цель не указана";
            }
            int talon = GenerateRandom();

            string querystring = $"Insert into Запись_Прием ([ID_Врача],[ID_Пациента], [Дата_Посещения], [Цель_Посещения],Талон,ID_Кабинета) values('{doctorId}','{CurrentId}','{date}', '{aim}',{talon},'{GetCabinet()}')";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();

            command.ExecuteNonQuery();

            RefreshDataGrid(dataGridView1);

            richTextBox1.Clear();
            MessageBox.Show("Вы успешно записаны на прием", "Успех!");
            comboBoxTime();



        }
        private int GetCabinet()
        {
            var doctor = comboBox3.SelectedIndex;
            var doctorId = savedDoctorIDs2[doctor];
            string querystring = $"Select Кабинет.ID_Кабинета from Врач inner join Кабинет on Кабинет.ID_Кабинета = Врач.ID_Кабинета where ID_Врача = '{doctorId}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();
            int ID;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

               ID = reader.GetInt32(0);


                





            }
            return ID;
        }

        private int CellIdClient()
        {
            int idclient = 0;

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                idclient = Convert.ToInt32(selectedRow.Cells["ID_Приёма"].Value);
            }
            return idclient;




        }
        
        private void createColumns()
        {
            dataGridView1.Columns.Add("ID_Приёма", "ID");
            this.dataGridView1.Columns["ID_Приёма"].Visible = false;
         

            dataGridView1.Columns.Add("Дата_Посещения", "Дата посещения");

            dataGridView1.Columns.Add("Цель_Посещения", "Цель посещения");

            dataGridView1.Columns.Add("ФИО", "Фио врача");

            dataGridView1.Columns.Add("Специальность", "Врач");
            dataGridView1.Columns.Add("Талон", "Талон");
            dataGridView1.Columns.Add("Номер кабинета", "Номер кабинета");

        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetDateTime(1).ToString("dd.MM.yyyy HH:mm"), record.GetString(2), record.GetString(3), record.GetString(4),record.GetString(5), record.GetString(6));

        }



        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();



            string queryString = $"select [ID_Приёма],[Дата_Посещения], [Цель_Посещения], ФИО,Специальность,Талон,[номер кабинета] from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] inner join Кабинет on Кабинет.ID_Кабинета = Врач.ID_Кабинета where Пациент.[ID_Пациента] = '{CurrentId}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }

        private void deleteRow()
        {

            var id = CellIdClient();

            if (id < 0)
            {

                return;
            }

            var deleteQuery = $"delete from Запись_Прием where [ID_Приёма] = '{id}'";

            var command = new SqlCommand(deleteQuery, DataBase.getConnection());
            command.ExecuteNonQuery();

        }



        private bool? checkDayAppointment()
        {
            var doctor = comboBox3.SelectedIndex;
            if (doctor == -1)
            {


                return null;
            }
            var date = dateTimePicker1.Value.ToString("yyyy-MM-dd") + " " + comboBox2.Text;

            var doctorId = savedDoctorIDs2[doctor];
            string querystring = $"select Дата_Посещения,ID_Пациента, ID_Врача from Запись_Прием where ABS(DATEDIFF(hour, Дата_Посещения, '{date}')) < 12 and ID_Пациента = '{CurrentId}' and ID_Врача = '{doctorId}' ";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();

            int count = 0;

            using (SqlDataReader reader = command.ExecuteReader())
            {



                while (reader.Read())
                {
                    count++;



                    return false;

                }





            }
            return count == 0;
        }
        private bool? checkTime()
        {
            var doctor = comboBox3.SelectedIndex;
            if (doctor == -1)
            {


                return null;
            }
            var date = dateTimePicker1.Value.ToString("1900-01-01") + " " + comboBox2.Text;
            var doctorId = savedDoctorIDs2[doctor];
            string querystring = $"select ID_Врача,День_Недели, Время_Начала_Работы, Время_Конца_Работы from Расписание where Время_Начала_Работы <= CAST('{date}' AS DATETIME2(3)) and Время_Конца_Работы >= CAST('{date}' AS DATETIME2(3)) and День_Недели = '{day()}' and ID_Врача = {doctorId}";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();

            int count = 0;

            using (SqlDataReader reader = command.ExecuteReader())
            {



                while (reader.Read())
                {

                    count++;


                }


            }
            return count > 0;
        }
        private bool? checkOtherAppointment()
        {
            var doctor = comboBox3.SelectedIndex;
            if (doctor == -1)
            {
                return null;
            }
            var date = dateTimePicker1.Value.ToString("yyyy-MM-dd") + " " + comboBox2.Text;

            var doctorId = savedDoctorIDs2[doctor];
            string querystring = $"select ID_Пациента, ID_Врача, Дата_Посещения from Запись_Прием where ID_Врача = {doctorId} and Дата_Посещения = CAST('{date}' AS DATETIME2(3)) and ID_Пациента != '{CurrentId}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();

            int count = 0;

            using (SqlDataReader reader = command.ExecuteReader())
            {

                while (reader.Read())
                {

                    count++;

                    return false;

                }
            }
            return count == 0;
        }

        private string day()
        {
            var date = dateTimePicker1.Value;
            var weekday = date.DayOfWeek;

            switch (weekday)
            {
                case DayOfWeek.Monday:
                    return "Понедельник";
                case DayOfWeek.Tuesday:
                    return "Вторник";
                case DayOfWeek.Wednesday:
                    return "Среда";
                case DayOfWeek.Thursday:
                    return "Четверг";
                case DayOfWeek.Friday:
                    return "Пятница";
                case DayOfWeek.Saturday:
                    return "Суббота";
                default:
                    return "Воскресенье";
            }

        }

       

        List<DateTime> savedDates2 = new List<DateTime>();
        private void comboBoxTime()
        {
            comboBox2.Items.Clear();
            var doctor = comboBox3.SelectedIndex;
            if (doctor == -1)
            {
                return;
            }
            var doctorId = savedDoctorIDs2[doctor];
            var day1 = day();
          
            string querystring = $"select Время_Приема from Расписание where ID_Врача = '{doctorId}' and День_Недели = '{day1}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();
            List<DateTime> timelist = new List<DateTime>();
            
            using (SqlDataReader reader = command.ExecuteReader())
            {

                

                while (reader.Read())
                {
                    

                    
                    var dateList = reader.GetDateTime(0);
                    timelist.Add((DateTime)dateList);

                    

                }
              
               
            }


            savedDates = timelist;

          
            var date = dateTimePicker1.Value;
            string querystring2 = $"select Дата_Посещения from Запись_Прием where ID_Врача = '{doctorId}' and CAST(Дата_Посещения AS date) ='{date}'";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();
            command2.ExecuteNonQuery();
            List<DateTime> timelist2 = new List<DateTime>();
            using (SqlDataReader reader = command2.ExecuteReader())
            {



                while (reader.Read())
                {



                    var dateList = reader.GetDateTime(0);
                    timelist2.Add((DateTime)dateList);



                }


            }
            savedDates2 = timelist2;
            foreach (var item in timelist)
            {
                bool xz = false;
                foreach (var item1 in timelist2)
                {   
                    if(item.Hour == item1.Hour && item.Minute == item1.Minute)
                    {

                        xz = true;
                        break;

                    }


                }
                if(xz == false)
                {

                    comboBox2.Items.Add(item);
                }
               
               
            }
           

            if (timelist.Count > 0)
            {

                comboBox2.DropDownHeight = 300;
            }
            else
                comboBox2.DropDownHeight = 1;



        }
        List<DateTime> savedDates = new List<DateTime>();
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void NW_Load(object sender, EventArgs e)
        {
            ComboBoxSpec();
            createColumns();
            RefreshDataGrid(dataGridView1);
            dataGridView1.ReadOnly = true;
            richTextBox1.MaxLength = 312;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            label123();
           
            dateTimePicker1.MinDate = DateTime.Now;


        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

            RefreshDataGrid(dataGridView1);

        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверенны, что хотите удалить запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                deleteRow();
                MessageBox.Show("Запись успешно удалена", "Успешно!");
                RefreshDataGrid(dataGridView1);
            }
            return;

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void врачBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Doctors ds = new Doctors();
            ds.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {

           
            log_in login = new log_in();
            login.Close();

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            this.Hide();
        }



        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            comboBox2.DropDownHeight = 1;
            comboBoxTime();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var doctor = comboBox1.SelectedIndex;
            if (doctor == -1)
            {
                MessageBox.Show("Выберите врача", "Ошибка!");
                return;
            }
        }
        private void ComboBox3Fill()
        {

            var doctorSpec = comboBox1.Text;

            string querystring2 = $"select ID_Врача, ФИО from Врач where Специальность = '{doctorSpec}'";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();


            List<string> listName = new List<string>();
            List<int> listID = new List<int>();


            using (SqlDataReader reader = command2.ExecuteReader())
            {

                while (reader.Read())
                {
                    var IDDoctor = reader.GetInt32(0);
                    listID.Add(IDDoctor);
                    var doctorname = reader.GetString(1);
                    listName.Add((string)doctorname);

                }


            }


            savedDoctorIDs2 = listID;


            foreach (var item in listName)
            {

                comboBox3.Items.Add(item);

            }

        }


        List<int> savedDoctorIDs2 = new List<int>();


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CosttextBox1.Text = "";
            comboBox3.Items.Clear();
            comboBox3.Text = "";
            ComboBox3Fill();
            comboBoxTime();

        }

        private int YO()
        {
            int age = 0;
            string querystring2 = $"Select Case when DATEADD(YEAR , DATEDIFF(YY, Дата_Рождения,GETDATE()), Дата_Рождения) > GETDATE() Then DateDiff(YY, (Дата_Рождения), GetDate()) - 1 else DateDiff(YY, (Дата_Рождения), GetDate()) end as Age from Пациент where ID_Пациента = '{CurrentId}'";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();
            using (SqlDataReader reader = command2.ExecuteReader())
            {
                reader.Read();

                int column1Data = Convert.ToInt32(reader["Age"]);

                age = column1Data;
                


            }
            return age;

        }
        private void Cost()
        {
            var doctor = comboBox3.SelectedIndex;
      
            var doctorID = savedDoctorIDs2[doctor];
            int age = YO();
            string querystring2 = $"select [Стоимость посещения] from Врач where ID_Врача = '{doctorID}'";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();
            using (SqlDataReader reader = command2.ExecuteReader())
            {

                reader.Read();
                object column1Data = reader["Стоимость посещения"];
                double cost = Convert.ToInt32(column1Data);
                Math.Round(cost, 2).ToString();
                if (age > 60)
                { 
                    cost = cost - (cost * 0.25);
                    CosttextBox1.Text = $"{cost}" + " Руб." + "(Скидка 25%)";
                    costlable.Text = "(с учетом скидки для людей старше 60 лет)";
                }
                else if (age < 18)
                {
                    cost = cost - (cost * 0.15);
                    CosttextBox1.Text = $"{cost}" + " Руб." + "(Скидка 15%)";
                    costlable.Text = "(с учетом скидки для детей младше 18 лет)";
                }
                else
                {
                    CosttextBox1.Text = $"{cost}" + " Руб.";
                }

               


            }

        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cost();
            comboBoxTime();

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void CosttextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void costlable_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(costlable, "Нажмите чтобы узнать больше");
        }

        private void costlable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Discount ad = new Discount();
            ad.ShowDialog();    
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {

          


        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            int ID = CellIdClient();
            Talon_Print Tp = new Talon_Print(ID);
            Tp.Show();
            Tp.Visible = false;

        }
    }


}

