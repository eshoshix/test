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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace test_DataBase
{
    
    public partial class timetable : MetroFramework.Forms.MetroForm
    {
        DataBase DataBase = new DataBase();
        int CurrentId; 
        public timetable(int ID)
        {
            CurrentId = ID;
            InitializeComponent();
        }

        private void timetable_Load(object sender, EventArgs e)
        {
            DaysComboBox();
            
            createColumns();
          

        }

        private void DaysComboBox()
        {

            var days = new List<string>() { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

            foreach (var item in days)
            {
                comboBox2.Items.Add(item);
            }

        }


       
        private bool? chekTimeToDelete()
        {
            var day = comboBox2.Text;
            string querystring = $"select * from Расписание where ID_Врача = '{CurrentId}' and День_Недели = '{day}' ";
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
        


        private void button1_Click(object sender, EventArgs e)
        {
            var day = comboBox2.Text;
            var dateStart = dateTimePicker1.Value.ToString("HH:mm");
            var dateFinish = dateTimePicker2.Value.ToString("HH:mm");
            if (chekTimeToDelete() == false)
            {

                string querystring = $"delete from Расписание where ID_Врача = '{CurrentId}' and День_Недели = '{day}'";
                SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
                DataBase.openConnection();
                command.ExecuteNonQuery();



            }

                DateTime startTime = dateTimePicker1.Value; // начальное время
                DateTime endTime = dateTimePicker2.Value; // конечное время
                TimeSpan interval = TimeSpan.FromMinutes(15); // интервал времени (15 минут)

                DateTime currentTime = startTime;
                while (currentTime < endTime)
                {
                    currentTime = currentTime.Add(interval);

                   
                    string querystring2 = $"Insert into Расписание ([ID_Врача], День_Недели, Время_Начала_Работы, Время_Конца_Работы,Время_Приема) values('{CurrentId}','{day}','{dateStart}', '{dateFinish}','{currentTime}')";
                    SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
                    DataBase.openConnection();

                    command2.ExecuteNonQuery();


                }


                RefreshDataGrid(dataGridView1);


            







        }

        private void createColumns()
        {
            dataGridView1.Columns.Add("ID_Расписания", "ID_Расписания");
            dataGridView1.Columns["ID_Расписания"].Visible = false;
            dataGridView1.Columns.Add("ID_Врача", "ID_Врача");
            this.dataGridView1.Columns["ID_Врача"].Visible = false;
            dataGridView1.Columns.Add("ФИО", "Фио врача");
            dataGridView1.Columns.Add("Специальность", "Специальность");
            dataGridView1.Columns.Add("День_Недели", "День недели");
            dataGridView1.Columns.Add("Время_Начала_Работы", "Время начала работы");
            dataGridView1.Columns[5].DefaultCellStyle.Format = "HH:mm:ss";
            dataGridView1.Columns.Add("Время_Конца_Работы", "Время конца работы");
            dataGridView1.Columns[6].DefaultCellStyle.Format = "HH:mm:ss";
            dataGridView1.Columns.Add("Время_Приема", "Время приема");


        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetDateTime(5), record.GetDateTime(6),record.GetDateTime(7));

        }


        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();

            
            var day = comboBox2.Text;

            string queryString = $"select [ID_Расписания], Расписание.ID_Врача, ФИО, Специальность, День_Недели,Время_Начала_Работы, Время_Конца_Работы,Время_Приема from Расписание inner join Врач on Врач.[ID_Врача] = Расписание.[ID_Врача] where Расписание.ID_Врача = '{CurrentId}' and День_Недели = '{day}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var index = comboBox2.SelectedIndex;
            if (index == -1)
            {


                return;
            }
            RefreshDataGrid(dataGridView1);
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBox2.SelectedIndex;
            if (index == -1)
            {


                return;
            }
            RefreshDataGrid(dataGridView1);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверенны, что хотите удалить расписание?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var day = comboBox2.Text;
                if (chekTimeToDelete() == false)
                {

                    string querystring = $"delete from Расписание where ID_Врача = '{CurrentId}' and День_Недели = '{day}'";
                    SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
                    DataBase.openConnection();
                    command.ExecuteNonQuery();



                }
                RefreshDataGrid(dataGridView1);
                MessageBox.Show("Расписание успешено удалено!", "Успешно!");
                return;
            }
            
        }
    }
}
