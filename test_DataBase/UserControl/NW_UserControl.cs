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
    public partial class NW_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        int CurrentId;
        public NW_UserControl(int id)
        {
            CurrentId = id;
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            label123();


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





        private void button1_Click(object sender, EventArgs e)
        {
            var doctor = comboBox3.SelectedIndex;
            if (doctor == -1)
            {

                MessageBox.Show("Выберите врача", "Ошибка");
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
                return;

            }


            if (checkTime() == false)
            {
                MessageBox.Show("Врач не работает в заданное время!", "Ошибка!");
                return;

            }




            var date = dateTimePicker1.Value.ToString("yyyyMMdd") + " " + comboBox2.Text;


            var doctorId = savedDoctorIDs2[doctor];
            string querystring3 = $"select [Стоимость посещения] from Врач where [ID_Врача] = '{doctorId}'";
            SqlCommand command3 = new SqlCommand(querystring3, DataBase.getConnection());
            DataBase.openConnection();




            var aim = richTextBox1.Text;
            string querystring = $"Insert into Запись_Прием ([ID_Врача],[ID_Пациента], [Дата_Посещения], [Цель_Посещения]) values('{doctorId}','{CurrentId}','{date}', '{aim}')";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();

            command.ExecuteNonQuery();

            RefreshDataGrid(dataGridView1);

            richTextBox1.Clear();
            MessageBox.Show("Вы успешно записаны на прием", "Успех!");



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
            dataGridView1.Columns.Add("ФИО", "Фио пациента");

            dataGridView1.Columns.Add("Дата_Посещения", "Дата посещения");

            dataGridView1.Columns.Add("Цель_Посещения", "Цель посещения");

            dataGridView1.Columns.Add("ФИО", "Фио врача");

            dataGridView1.Columns.Add("Специальность", "Врач");

        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetString(1) + " " + record.GetString(2) + " " + record.GetString(3), record.GetDateTime(4).ToString("dd.MM.yyyy HH:mm"), record.GetString(5), record.GetString(6), record.GetString(7));

        }



        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();



            string queryString = $"select [ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО,Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where Пациент.[ID_Пациента] = '{CurrentId}'";
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
            var date = dateTimePicker1.Value.ToString("yyyyMMdd") + " " + comboBox2.Text;

            var doctorId = savedDoctorIDs2[doctor];
            string querystring = $"select Дата_Посещения,ID_Пациента, ID_Врача from Запись_Прием where DATEDIFF(hour, Дата_Посещения, '{date}') < 12 and ID_Пациента = '{CurrentId}' and ID_Врача = '{doctorId}' ";
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
            string querystring = $"select ID_Врача,День_Недели, Время_Начала_Работы, Время_Конца_Работы from Расписание where Время_Начала_Работы < CAST('{date}' AS DATETIME2(3)) and Время_Конца_Работы > CAST('{date}' AS DATETIME2(3)) and День_Недели = '{day()}' and ID_Врача = {doctorId}";
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




                    var date = reader.GetDateTime(0);
                    timelist.Add((DateTime)date);



                }

            }

            savedDates = timelist;


            foreach (var item in timelist)
            {

                comboBox2.Items.Add(item);

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

        private void Cost()
        {
            var doctor = comboBox3.SelectedIndex;

            var doctorID = savedDoctorIDs2[doctor];

            string querystring2 = $"select [Стоимость посещения] from Врач where ID_Врача = '{doctorID}'";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();
            using (SqlDataReader reader = command2.ExecuteReader())
            {
                reader.Read();
                object column1Data = reader["Стоимость посещения"];
                decimal cost = Convert.ToInt32(column1Data);
                Math.Round(cost, 2).ToString();
                CosttextBox1.Text = $"{cost}" + " Руб.";


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
    }


}

