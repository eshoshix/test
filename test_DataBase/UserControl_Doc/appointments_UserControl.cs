using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace test_DataBase
{
    public partial class appointments_UserControl : UserControl
    { 
        int CurrentID;
        DataBase DataBase = new DataBase();
        public appointments_UserControl(int ID)
        {
            CurrentID = ID;
            InitializeComponent();
        }

        private void appointments_UserControl_Load(object sender, EventArgs e)
        {
            createColumns();
            RefreshDataGrid(dataGridView1);

            comboBoxDate();
        }
       

      

       
        private void createColumns()
        {
            dataGridView1.Columns.Add("ID_Приёма", "ID");
            dataGridView1.Columns.Add("Фамилия", "Пациент");
            DataGridViewColumn column = dataGridView1.Columns[1];
            column.Width = 150;
            dataGridView1.Columns.Add("Дата_Посещения", "Дата посещения");
            DataGridViewColumn column2 = dataGridView1.Columns[2];
            column2.Width = 100;
            dataGridView1.Columns.Add("Цель_Посещения", "Цель посещения");
            DataGridViewColumn column3 = dataGridView1.Columns[3];
            column3.Width = 200;
            dataGridView1.Columns.Add("ФИО", "Фио врача");
            DataGridViewColumn column4 = dataGridView1.Columns[4];
            column4.Width = 100;
            dataGridView1.Columns.Add("Специальность", "Врач");
            DataGridViewColumn column5 = dataGridView1.Columns[5];
            column5.Width = 100;
            dataGridView1.Columns["ID_Приёма"].Visible = false;

        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            

            dgw.Rows.Add(record.GetInt32(0), record.GetString(1) + " " + record.GetString(2) + " " + record.GetString(3), record.GetDateTime(4), record.GetString(5), record.GetString(6), record.GetString(7).ToString(), RowState.ModifiedNew);
           

        }

        private void comboBoxDate()
        {
            var days = new List<string>() { "За сегодня", "За текущую неделю", "За текущий месяц", "За текущий год", "За прошлый месяц", "За прошлый год" };

            foreach (var item in days)
            {
                comboBox1.Items.Add(item);
            }


        }
        private void RefreshDataGrid2(DataGridView dgw)
        {
            var date = dateTimePicker1.Value.ToString("dd.MM.yyyy");    
            dgw.Rows.Clear();

            string queryString = $"select [ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО,Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where Cast(Дата_Посещения as date) = cast('{date}' as date) and Запись_Прием.ID_Врача = {CurrentID}";
            
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {


                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            string queryString ="";


            var date = dateTimePicker1.Text;
            if (comboBox1.Text == "")
            {
                queryString = $"select[ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО, Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where Запись_Прием.ID_Врача = {CurrentID}";


            }
            if (comboBox1.Text == "За сегодня")
            {
                queryString = $"select[ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО, Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where Cast(Дата_Посещения as date) = cast(GETDATE() as date) and Запись_Прием.ID_Врача = {CurrentID}";


            }
            else if(comboBox1.Text =="За текущую неделю")
            {
                queryString = $"select [ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО,Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where DatePart(\"ww\", Дата_Посещения ) = DatePart(ww, GETDATE()) and Запись_Прием.ID_Врача = {CurrentID}";


            }
            else if (comboBox1.Text == "За текущий месяц")
            {
                queryString = $"select [ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО,Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where MONTH (Дата_Посещения) = MONTH(GETDATE()) and Запись_Прием.ID_Врача = {CurrentID}";



            }
            else if (comboBox1.Text == "За текущий год")
            {
                queryString = $"select [ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО,Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where YEAR (Дата_Посещения) = Year(GETDATE()) and Запись_Прием.ID_Врача = {CurrentID}";




            }
            else if (comboBox1.Text == "За прошлый месяц")
            {
               queryString = $"select [ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО,Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where Дата_Посещения >=DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())-1,0) AND Дата_Посещения< DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE()) ,0) and Запись_Прием.ID_Врача = {CurrentID}";

            }
            else if (comboBox1.Text == "За прошлый год")
            {
                queryString =  $"select [ID_Приёма], Фамилия, Имя, Отчество, [Дата_Посещения], [Цель_Посещения], ФИО,Специальность from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] where YEAR (Дата_Посещения) =  Year(GETDATE())-1 and Запись_Прием.ID_Врача = {CurrentID}";

            }
            
           

            
            dgw.Rows.Clear();
                                                                                                                                   
            



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

            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;


            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {

                dataGridView1.Rows[index].Cells[7].Value = RowState.Deleted;

                return;
            }


        }

        private void Update()
        {

            DataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {

                if (dataGridView1.Rows[index].Visible)
                {
                    continue;

                }


                var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                var deleteQuery = $"delete from Запись_Прием where [ID_Приёма] = '{id}'";

                var command = new SqlCommand(deleteQuery, DataBase.getConnection());
                command.ExecuteNonQuery();





            }

            DataBase.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                deleteRow();
                Update();
                MessageBox.Show("Запись успешно удалена!", "Успешно!");
                return;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            RefreshDataGrid(dataGridView1);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            comboBox1.Text = "";
            RefreshDataGrid2(dataGridView1);


        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            RefreshDataGrid(dataGridView1);
        }
    }
}
