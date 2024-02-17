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
    public partial class Schedule_Edit_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        int CurrentId;
        public Schedule_Edit_UserControl(int ID)
        {
            CurrentId = ID;
            InitializeComponent();
        }

        private void Schedule_Edit_UserControl_Load(object sender, EventArgs e)
        {
            DaysComboBox();
            createColumns();
            RefreshDataGrid(dataGridView1);
            
        }
        
       
        private int CellTime()
        {
            int idclient = 0;

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                idclient = Convert.ToInt32(selectedRow.Cells["ID_Расписания"].Value);
            }
            return idclient;




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
                

                string querystring2 = $"Insert into Расписание ([ID_Врача], День_Недели, Время_Начала_Работы, Время_Конца_Работы,Время_Приема) values('{CurrentId}','{day}','{dateStart}', '{dateFinish}','{currentTime}')";
                SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
                DataBase.openConnection();

                command2.ExecuteNonQuery();
                currentTime = currentTime.Add(interval);

            }


            RefreshDataGrid(dataGridView1);










        }

        private void createColumns()
        {
            dataGridView1.Columns.Add("ID_Расписания", "ID_Расписания");
            dataGridView1.Columns["ID_Расписания"].Visible = false;
            dataGridView1.Columns.Add("ID_Врача", "ID_Врача");
            this.dataGridView1.Columns["ID_Врача"].Visible = false;
            dataGridView1.Columns.Add("День_Недели", "День недели");
            dataGridView1.Columns.Add("Время_Начала_Работы", "Время начала работы");
            dataGridView1.Columns[3].DefaultCellStyle.Format = "HH:mm";
            dataGridView1.Columns.Add("Время_Конца_Работы", "Время конца работы");
            dataGridView1.Columns[4].DefaultCellStyle.Format = "HH:mm";
            dataGridView1.Columns.Add("Время_Приема", "Время приема");
            dataGridView1.Columns[5].DefaultCellStyle.Format = "HH:mm";
          
        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetString(2), record.GetDateTime(3), record.GetDateTime(4), record.GetDateTime(5));

        }


        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();


            var day = comboBox2.Text;

            string queryString = $"select [ID_Расписания], Расписание.ID_Врача, День_Недели,Время_Начала_Работы, Время_Конца_Работы,Время_Приема,Кабинет.[Номер кабинета] from Расписание inner join Врач on Врач.[ID_Врача] = Расписание.[ID_Врача] inner join Кабинет on Кабинет.ID_Кабинета = Врач.ID_Кабинета  where Расписание.ID_Врача = '{CurrentId}' and День_Недели = '{day}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

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
            if (MessageBox.Show("Вы уверенны, что хотите удалить все расписание?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                MessageBox.Show("Расписание успешно удалено!", "Успешно!");
                return;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверенны, что хотите удалить время из расписания?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var time = CellTime();
                var day = comboBox2.Text;
                if (chekTimeToDelete() == false)
                {

                    string querystring = $"delete from Расписание where ID_Врача = '{CurrentId}' and День_Недели = '{day}' and ID_Расписания = '{time}'";
                    SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
                    DataBase.openConnection();
                    command.ExecuteNonQuery();



                }
                RefreshDataGrid(dataGridView1);
                MessageBox.Show("Расписание успешно удалено!", "Успешно!");
                return;
            }
        }
    }
}
