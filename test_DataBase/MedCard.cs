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
    public partial class MedCard : Form
    {
        DataBase DataBase = new DataBase();
        int CurrentClient;
        public MedCard(int ID)
        {
            CurrentClient = ID; 
            InitializeComponent();
        }

        private void MedCard_Load(object sender, EventArgs e)
        {
            createColumns();    
            RefreshDataGrid(dataGridView1);
        }
        private void createColumns()
        {
            dataGridView1.Columns.Add("ФИО пациента", "ФИО пациента");
            dataGridView1.Columns.Add("ФИО врача", "ФИО врача");
            dataGridView1.Columns.Add("Диагноз", "Диагноз");
            dataGridView1.Columns.Add("Дата_начала_заболевания", "Дата_конца_заболевания");
            dataGridView1.Columns.Add("Дата_конца_заболевания", "Дата_конца_заболевания");
            dataGridView1.Columns.Add("Дата_Выписки", "Дата_Выписки");
            dataGridView1.Columns.Add("Предписание", "Предписание");
            dataGridView1.Columns.Add("Лекарства", "Лекарства");
        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetString(0) + " " + record.GetString(1) + " " + record.GetString(2), record.GetString(3),record.GetString(4), record.GetDateTime(5), record.GetDateTime(6), record.GetDateTime(7), record.GetString(8), record.GetString(9));
        }


        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();
            

            string queryString = $"select Фамилия, Имя, Отчество, ФИО, Наименование,Дата_начала_заболевания,Дата_конца_заболевания,Дата_Выписки,Предписание,Лекарства from Больничные inner join Пациент on Пациент.ID_Пациента = Больничные.ID_Пациента inner join Врач on Врач.ID_Врача = Больничные.ID_Врача inner join Диагноз on Диагноз.ID_Диагноза = Больничные.ID_Диагноза where Больничные.ID_Пациента = '{CurrentClient}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
