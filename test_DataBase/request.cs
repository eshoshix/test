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
    public partial class request : MetroFramework.Forms.MetroForm
    {
        DataBase DataBase = new DataBase();
        int Currentid;
        public request(int idClient)
        {
            InitializeComponent();
           Currentid = idClient;
            StartPosition = FormStartPosition.CenterScreen;


            InitializeComponent();
           


            CosttextBox1.ReadOnly = true;

        }

        private void request_Load(object sender, EventArgs e)
        {
            label123();
            createColumns();
            RefreshDataGrid(dataGridView1);
            dataGridView1.ReadOnly = true;
            aimtextBox1.MaxLength = 50;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }
        private void label123()
        {
            

            string querystring2 = $"select ФИО, Специальность, [ID_Врача], [Стоимость Посещения] from Врач";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();
            List<decimal> listCost = new List<decimal>();
            List<string> listSpec = new List<string>();
            List<string> listName = new List<string>();
            List<int> listID = new List<int>();
            using (SqlDataReader reader = command2.ExecuteReader())
            {

                while (reader.Read())
                {
                    var doctorname = reader.GetString(0);
                    listName.Add((string)doctorname);
                    var doctorspec = reader.GetString(1);
                    listSpec.Add(doctorspec);
                    var IDDoctor = reader.GetInt32(2);
                    listID.Add(IDDoctor);
                    var cost = reader.GetDecimal(3);
                    listCost.Add(cost);
                }


            }

            savedDoctorIDs = listID;
            savedCosts = listCost;


            foreach (var item in listSpec)
            {

                comboBox1.Items.Add(item);

            }


        }

        List<int> savedDoctorIDs = new List<int>();
        List<decimal> savedCosts = new List<decimal>();

      

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBox1.SelectedIndex;
            if (index == -1)
            {

                CosttextBox1.Text = "";
                return;
            }

            CosttextBox1.Text = Convert.ToInt32(Math.Round(savedCosts[index])) + " руб.".ToString();
            comboBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var doctor = comboBox1.SelectedIndex;
            if (doctor == -1)
            {

                MessageBox.Show("Выберите врача", "Ошибка");
                return;
            }
            if (checkAppointment() == false)
            {
                MessageBox.Show("Вы уже записанны к данному врачу на эту дату !", "Ошибка!");
                return;

            }
            if (checkRecordedAppointment() == false)
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


            var doctorId = savedDoctorIDs[doctor];
            string querystring3 = $"select [Стоимость посещения] from Врач where [ID_Врача] = '{doctorId}'";
            SqlCommand command3 = new SqlCommand(querystring3, DataBase.getConnection());
            DataBase.openConnection();

            using (SqlDataReader reader = command3.ExecuteReader())
            {
                reader.Read();

                object column1Data = reader["Стоимость посещения"];

            }


            var aim = aimtextBox1.Text;
            string querystring = $"Insert into Запись_Прием ([ID_Врача],[ID_Пациента], [Дата_Посещения], [Цель_Посещения]) values('{doctorId}','{Currentid}','{date}', '{aim}')";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();

            command.ExecuteNonQuery();

            RefreshDataGrid(dataGridView1);

            aimtextBox1.Clear();
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



            string queryString = $"select [ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО,Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where Пациент.[ID_Пациента] = '{Currentid}'";
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



        private bool? checkAppointment()
        {
            var doctor = comboBox1.SelectedIndex;
            if (doctor == -1)
            {


                return null;
            }
            var date = dateTimePicker1.Value.ToString("yyyyMMdd") + " " + comboBox2.Text;

            var doctorId = savedDoctorIDs[doctor];
            string querystring = $"select Дата_Посещения,ID_Пациента, ID_Врача from Запись_Прием where DATEDIFF(hour, Дата_Посещения, '{date}') < 12 and ID_Пациента = '{Currentid}' and ID_Врача = '{doctorId}' ";
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
            var doctor = comboBox1.SelectedIndex;
            if (doctor == -1)
            {


                return null;
            }
            var date = dateTimePicker1.Value.ToString("1900-01-01") + " " + comboBox2.Text;
            var doctorId = savedDoctorIDs[doctor];
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
        private bool? checkRecordedAppointment()
        {
            var doctor = comboBox1.SelectedIndex;
            if (doctor == -1)
            {
                return null;
            }
            var date = dateTimePicker1.Value.ToString("yyyy-MM-dd") + " " + comboBox2.Text;

            var doctorId = savedDoctorIDs[doctor];
            string querystring = $"select ID_Пациента, ID_Врача, Дата_Посещения from Запись_Прием where ID_Врача = {doctorId} and Дата_Посещения = CAST('{date}' AS DATETIME2(3))  ";
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



        private void comboBox()
        {
            comboBox2.Items.Clear();
            var doctor = comboBox1.SelectedIndex;
            if (doctor == -1)
            {
                return;
            }
            var doctorId = savedDoctorIDs[doctor];
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           
            comboBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            deleteRow();
        }
    }
}
