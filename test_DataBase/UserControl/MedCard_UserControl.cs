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
    public partial class MedCard_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        int CurrentClient;
        public MedCard_UserControl(int ID)
        {
            CurrentClient = ID;

            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void MedCard_Load(object sender, EventArgs e)
        {
            createColumns();
            RefreshDataGrid(dataGridView1);
        }
        private void createColumns()
        {
            dataGridView1.Columns.Add("Спецальность", "Спецальность");

            dataGridView1.Columns.Add("ФИО врача", "ФИО врача");

            dataGridView1.Columns.Add("Диагноз", "Диагноз");

            dataGridView1.Columns.Add("Дата_начала_заболевания", "Начало заболевания");

            dataGridView1.Columns[3].DefaultCellStyle.Format = "dd.MM.yyy";
            dataGridView1.Columns.Add("Дата_конца_заболевания", "Конец заболевания");

            dataGridView1.Columns[4].DefaultCellStyle.Format = "dd.MM.yyy";
            dataGridView1.Columns.Add("Дата_Выписки", "Дата Выписки");

            dataGridView1.Columns[5].DefaultCellStyle.Format = "dd.MM.yyy";
            dataGridView1.Columns.Add("Предписание", "Предписание");

            dataGridView1.Columns.Add("Лекарства", "Лекарства");
            dataGridView1.Columns.Add("ID_Больничного", "ID");
            dataGridView1.Columns[8].Visible = false;
        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetString(0), record.GetString(1), record.GetString(2), record.GetDateTime(3), record.GetDateTime(4), record.GetDateTime(5), record.GetString(6), record.GetString(7), record.GetInt32(8));
        }


        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();


            string queryString = $"select Специальность, ФИО, Наименование,Дата_начала_заболевания,Дата_конца_заболевания,Дата_Выписки,Предписание,Лекарства,ID_Больничного from Больничные inner join Пациент on Пациент.ID_Пациента = Больничные.ID_Пациента inner join Врач on Врач.ID_Врача = Больничные.ID_Врача inner join Диагноз on Диагноз.ID_Диагноза = Больничные.ID_Диагноза where Больничные.ID_Пациента = '{CurrentClient}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }
        private int CellIdClient()
        {
            int idclient = 0;

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                idclient = Convert.ToInt32(selectedRow.Cells["ID_Больничного"].Value);
            }
            return idclient;




        }


        private void Text()
        {
            int IDLeave = CellIdClient();
            string queryString = $"select Фамилия, Имя, Отчество, Специальность, ФИО, Наименование,Дата_начала_заболевания,Дата_конца_заболевания,Дата_Выписки,Предписание,Лекарства,ID_Больничного from Больничные inner join Пациент on Пациент.ID_Пациента = Больничные.ID_Пациента inner join Врач on Врач.ID_Врача = Больничные.ID_Врача inner join Диагноз on Диагноз.ID_Диагноза = Больничные.ID_Диагноза where Больничные.ID_Больничного = '{IDLeave}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                object column1Data = reader["Фамилия"];
                object column2Data = reader["Имя"];
                object column3Data = reader["Отчество"];
                textBox1.Text = column1Data.ToString() + " " + column2Data + " " + column3Data;

                object column4Data = reader["ФИО"];
                textBox2.Text = column4Data.ToString();
                object column5Data = reader["Специальность"];
                textBox3.Text = column5Data.ToString();
                object column6Data = reader["Наименование"];
                textBox4.Text = column6Data.ToString();
                object column7Data = reader["Дата_начала_Заболевания"];
                textBox5.Text = Convert.ToDateTime(column7Data).ToString("dd.MM.yyy");
                object column8Data = reader["Дата_конца_Заболевания"];
                textBox6.Text = Convert.ToDateTime(column8Data).ToString("dd.MM.yyy");
                object column9Data = reader["Дата_Выписки"];
                textBox7.Text = Convert.ToDateTime(column9Data).ToString("dd.MM.yyy");
                object column10Data = reader["Предписание"];
                richTextBox1.Text = column10Data.ToString();
                object column11Data = reader["Лекарства"];
                richTextBox2.Text = column11Data.ToString();





            }
            DataBase.closeConnection();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Text();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
