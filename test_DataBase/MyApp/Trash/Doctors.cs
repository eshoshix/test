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
    
    public partial class Doctors : MetroFramework.Forms.MetroForm
    {
        DataBase DataBase = new DataBase(); 
        public Doctors()
        {
            InitializeComponent();
        }

        private void Doctors_Load(object sender, EventArgs e)
        {
            createColumns();
            RefreshDataGrid(dataGridView1);
        }

        private int CellIDdoctor()
        {
            int IDdoctor = 0;

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                IDdoctor = Convert.ToInt32(selectedRow.Cells["ID_Врача"].Value);
            }
            return IDdoctor;

        }

        private void createColumns()
        {
           
            dataGridView1.Columns.Add("ID_Врача", "ID_Врача");
            this.dataGridView1.Columns["ID_Врача"].Visible = false;
            dataGridView1.Columns.Add("ФИО", "Фио врача");
            dataGridView1.Columns.Add("Специальность", "Специальность");
            dataGridView1.Columns.Add("[Стоимость посещения]", "Стоимость посещения");





        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), (Math.Round(record.GetDecimal(3)) + " руб."));

        }


        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();

           

            string queryString = $"select ID_Врача, ФИО, Специальность, [Стоимость посещения] from Врач";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(CellIDdoctor() < 1)
            {
                MessageBox.Show("Выберите врача!", "Ошибка!");
                return;
            }
            var ID = CellIDdoctor();
            Dates ds = new Dates(ID);
            ds.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CellIDdoctor() <1)
            {
                MessageBox.Show("Выберите врача!", "Ошибка!");
                return;
            }
            var ID = CellIDdoctor();
            AboutDoctor AD = new AboutDoctor(ID);
            AD.ShowDialog();



        }
    }
}
