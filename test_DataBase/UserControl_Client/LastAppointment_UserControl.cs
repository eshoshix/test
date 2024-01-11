using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_DataBase
{
    public partial class LastAppointment_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        int CurrentClient;
        public LastAppointment_UserControl(int ID)
        {
            CurrentClient = ID;
            InitializeComponent();
        }

        private void LastAppointment_UserControl_Load(object sender, EventArgs e)
        {
            createColumns();
            RefreshDataGrid(dataGridView1);
            DaysComboBox();
            dataGridView1.ClearSelection();


        }

        private void createColumns()
        {
            dataGridView1.Columns.Add("ID_СостПриема", "ID");
            this.dataGridView1.Columns["ID_СостПриема"].Visible = false;
            dataGridView1.Columns.Add("ID_Врача", "ID_Врача");
            this.dataGridView1.Columns["ID_Врача"].Visible = false;
            dataGridView1.Columns.Add("ФИО", "ФИО врача");
            dataGridView1.Columns.Add("Специальность", "Специальность");
            dataGridView1.Columns.Add("Дата_Посещения", "Дата посещения");
        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetString(2), record.GetString(3), record.GetDateTime(4).ToString("dd.MM.yyyy"));



        }
        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();

            string queryString = $"select top 5 ID_СостПриема,Врач.ID_Врача, ФИО, Специальность,Дата_Посещения from Врач inner join Состоявщийся_Прием on Состоявщийся_Прием.ID_Врача = Врач.ID_Врача inner join Пациент on Пациент.ID_Пациента = Состоявщийся_Прием.ID_Пациента where Состоявщийся_Прием.ID_Пациента = '{CurrentClient}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {


                ReadSingleRow(dgw, reader);

            }
            reader.Close();

        }






        private int CellIdDoctor()
        {
            int id = 0;

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                id = Convert.ToInt32(selectedRow.Cells["ID_СостПриема"].Value);
            }
            return id;




        }
        private void button1_Click(object sender, EventArgs e)
        {

            var ID = CellIdDoctor();
            if (ID == 0)
            {
                MessageBox.Show("Выберите прием, который хотите оценить", "Ошибка");
                label5.ForeColor = Color.Red;
                return;


            }
            var mark = comboBox1.Text;
            if (mark == "")
            {
                MessageBox.Show("Выберите оценку от 0 до 10", "Ошибка");
                label4.ForeColor = Color.Red;
                return;
            }
            Convert.ToDecimal(mark);
            string queryString = $"update Состоявщийся_Прием set Оценка = {mark} where ID_СостПриема = '{ID}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();
            MessageBox.Show("Вы успешно оценили посещение на " + mark, "Успех");

        }
        private void DaysComboBox()
        {

            var days = new List<string>() { "10", "9", "8", "7", "6", "5", "4", "3", "2", "1", "0" };

            foreach (var item in days)
            {
                comboBox1.Items.Add(item);
            }

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Black;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            
        }

       

        private void comboBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label5.ForeColor = Color.Black;
        }

      

       
       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       

        private void panel1_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
