using MetroFramework.Controls;
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
    public partial class Doctors_UserControl : UserControl
    {
        DataBase DataBase  = new DataBase();    
        public Doctors_UserControl()
        {
            InitializeComponent();
        }

        private void Doctors_UserControl_Load(object sender, EventArgs e)
        {
            createColumns();
            RefreshDataGrid(dataGridView1);

        }
        private void addUserControl1(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(userControl);
            userControl.BringToFront();
        }
        private void addUserControl2(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(userControl);
            userControl.BringToFront();
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
            DataGridViewColumn column1 = dataGridView1.Columns[2];
            column1.Width = 181;
            DataGridViewColumn column2 = dataGridView1.Columns[1];
            column2.Width = 220;




        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2));

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
        private void Search(DataGridView dgw)
        {
           
            dgw.Rows.Clear();
            string searchQuery = $"select ID_Врача, ФИО, Специальность, [Стоимость посещения] from Врач where concat (ФИО, ' ' , Специальность) like '%" + textBox1.Text + "%'";
            SqlCommand command = new SqlCommand(searchQuery, DataBase.getConnection());
            DataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();


        }


   

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (CellIDdoctor() < 1)
            {
                MessageBox.Show("Выберите врача!", "Ошибка!");
                return;
            }
            var ID = CellIDdoctor();
            AboutDoctor_UserControl AD = new AboutDoctor_UserControl(ID);
            addUserControl1(AD);

            if (CellIDdoctor() < 1)
            {
                MessageBox.Show("Выберите врача!", "Ошибка!");
                return;
            }
            var ID2 = CellIDdoctor();
            Dates_UserControl ds = new Dates_UserControl(ID2);
            addUserControl2(ds);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

        
}
