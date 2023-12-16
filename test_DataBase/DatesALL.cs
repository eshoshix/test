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
    public partial class DatesALL : Form
    {

        DataBase DataBase = new DataBase();
        public DatesALL()
        {
            InitializeComponent();
        }

        private void DatesALL_Load(object sender, EventArgs e)
        {

        }
        private void name()
        {

            var CurrentDoctor = comboBox1.SelectedIndex;
            string queryString = $"select ФИО from Врач where ID_Врача = '{CurrentDoctor}'";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                string column1Data = reader["ФИО"].ToString();


                textBox1.Text = $"{column1Data}";

            }

        }
        private void createColumns()
        {
            dataGridView1.Columns.Add("ID_Расписания", "ID_Расписания");
            this.dataGridView1.Columns["ID_Расписания"].Visible = false;
            dataGridView1.Columns.Add("ID_Врача", "ID_Врача");
            this.dataGridView1.Columns["ID_Врача"].Visible = false;
            dataGridView1.Columns.Add("ФИО", "Фио врача");
            dataGridView1.Columns.Add("Специальность", "Специальность");
            dataGridView1.Columns.Add("[Стоимость посещения]", " Стоимость посещения");
            dataGridView1.Columns.Add("День_Недели", "День недели");
            dataGridView1.Columns.Add("Время_Начала_Работы", "Начало работы");
            dataGridView1.Columns[6].DefaultCellStyle.Format = "HH:mm";
            dataGridView1.Columns.Add("Время_Конца_Работы", "Конец работы");
            dataGridView1.Columns[7].DefaultCellStyle.Format = "HH:mm";
            dataGridView1.Columns.Add("Время_Приема", "Время приема");
            dataGridView1.Columns[8].DefaultCellStyle.Format = "HH:mm";


        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetString(2), record.GetString(3), (Math.Round(record.GetDecimal(4)) + " руб."), record.GetString(5), record.GetDateTime(6), record.GetDateTime(7), record.GetDateTime(8));

        }


        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();


            string queryString = $"select [ID_Расписания], Расписание.ID_Врача, ФИО, Специальность, [Стоимость посещения], День_Недели,Время_Начала_Работы,Время_Конца_Работы, Время_Приема from Расписание inner join Врач on Врач.[ID_Врача] = Расписание.[ID_Врача] where Расписание.ID_Врача = '{CurrentDoctor}'";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }






        private void DaysComboBox()
        {

            var days = new List<string>() { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

            foreach (var item in days)
            {
                comboBox1.Items.Add(item);

            }

        }


        

        private void RefreshDataGrid2(DataGridView dgw)
        {

            dgw.Rows.Clear();

            var day = comboBox1.Text.ToString();
            string queryString = $"select [ID_Расписания], Расписание.ID_Врача, ФИО, Специальность, [Стоимость посещения], День_Недели,Время_Приема,Время_Начала_Работы,Время_Конца_Работы from Расписание inner join Врач on Врач.[ID_Врача] = Расписание.[ID_Врача] where Расписание.ID_Врача = '{CurrentDoctor}' and День_Недели = '{day}' ";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.Text == "")
            {
                RefreshDataGrid(dataGridView1);

            }
            RefreshDataGrid2(dataGridView1);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Text = null;
            RefreshDataGrid(dataGridView1);
        }

    }
}
