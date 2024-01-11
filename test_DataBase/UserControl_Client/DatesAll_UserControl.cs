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
    public partial class DatesAll_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        public DatesAll_UserControl()
        {
            InitializeComponent();
        }

        private void DatesAllUserControl_Load(object sender, EventArgs e)
        {
            createColumns();
            comboBoxSpec();
            DaysComboBox();
            RefreshDataGrid1(dataGridView1);


        }


        private void comboBoxSpec()
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
        private void comboBox2Fill()
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

                comboBox2.Items.Add(item);

            }


        }
        List<int> savedDoctorIDs2 = new List<int>();






        private void name()
        {

            var doctor = comboBox1.SelectedIndex;
            if (doctor == -1)
            {
                return;
            }
            var doctorId = savedDoctorIDs2[doctor];
            string queryString = $"select ФИО from Врач where ID_Врача = '{doctorId}'";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();



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


        private void RefreshDataGrid1(DataGridView dgw)
        {

            dgw.Rows.Clear();


            var doctorSpec = comboBox1.Text;

            string queryString = $"select [ID_Расписания], Расписание.ID_Врача, ФИО, Специальность, [Стоимость посещения], День_Недели,Время_Начала_Работы,Время_Конца_Работы,Время_Приема from Расписание inner join Врач on Врач.[ID_Врача] = Расписание.[ID_Врача] ";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }
        private void RefreshDataGrid11(DataGridView dgw)
        {

            dgw.Rows.Clear();


            var doctorSpec = comboBox1.Text;

            string queryString = $"select [ID_Расписания], Расписание.ID_Врача, ФИО, Специальность, [Стоимость посещения], День_Недели,Время_Начала_Работы,Время_Конца_Работы,Время_Приема from Расписание inner join Врач on Врач.[ID_Врача] = Расписание.[ID_Врача] where Специальность = '{doctorSpec}' ";

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
                comboBox3.Items.Add(item);

            }

        }




        private void RefreshDataGrid2(DataGridView dgw)
        {

            dgw.Rows.Clear();
            var doctor = comboBox2.SelectedIndex;
            if (doctor == -1)
            {
                return;
            }
            var doctorId = savedDoctorIDs2[doctor];

            var day = comboBox1.Text.ToString();
            string queryString = $"select [ID_Расписания], Расписание.ID_Врача, ФИО, Специальность, [Стоимость посещения], День_Недели,Время_Начала_Работы,Время_Конца_Работы,Время_Приема from Расписание inner join Врач on Врач.[ID_Врача] = Расписание.[ID_Врача] where Расписание.ID_Врача = '{doctorId}' ";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }
        private void RefreshDataGrid3(DataGridView dgw)
        {

            dgw.Rows.Clear();
            var doctor = comboBox2.SelectedIndex;
            if (doctor == -1)
            {
                return;
            }
            var doctorId = savedDoctorIDs2[doctor];


            var day = comboBox3.Text.ToString();
            string queryString = $"select [ID_Расписания], Расписание.ID_Врача, ФИО, Специальность, [Стоимость посещения], День_Недели,Время_Начала_Работы,Время_Конца_Работы,Время_Приема from Расписание inner join Врач on Врач.[ID_Врача] = Расписание.[ID_Врача] where Расписание.ID_Врача = '{doctorId}' and День_Недели = '{day}' ";

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
                RefreshDataGrid11(dataGridView1);

            }
            RefreshDataGrid2(dataGridView1);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Text = null;
            RefreshDataGrid11(dataGridView1);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Text = null;
            comboBox3.Text = null;
            comboBox2Fill();
            RefreshDataGrid11(dataGridView1);

        }



        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
           
            
            
            RefreshDataGrid2(dataGridView1);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            RefreshDataGrid3(dataGridView1);


        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {

            if (comboBox3.SelectedText == "")
            {
                
                



                RefreshDataGrid2(dataGridView1);
            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedText == "")
            {





                RefreshDataGrid11(dataGridView1);
            }
        }

        
        

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
          

            if (comboBox1.SelectedText == null)
            {
                comboBox2.Items.Clear ();   




                RefreshDataGrid1(dataGridView1);
            }
        }
    }
}
