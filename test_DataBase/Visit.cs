using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace test_DataBase
{
    public partial class Visit : MetroFramework.Forms.MetroForm
    {
        DataBase DataBase = new DataBase();
        int CurrentDoctor;
        public Visit(int CurrentId)
        {
            CurrentDoctor = CurrentId ;
            InitializeComponent();
        }

        private void Visit_Load(object sender, EventArgs e)
        {
            createColumns();
            RefreshDataGrid(dataGridView1);
            DiagnozComboBox();
            createColumns2();
            button2DizClick();
          
           
            


        }

        private void button2DizClick()
        {
            if (dataGridView2.SelectedCells.Count <= 0)
            {
                

                button2.Enabled = false;
              

            }
            else
             button2.Enabled = true;


        }

        private int CellIdClient()
        {
            int idclient = 0;

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                idclient = Convert.ToInt32(selectedRow.Cells["ID_Пациента"].Value); 
            }
            return idclient;

           
            

        }
        private int CellIdSickleave()
        {
            RefreshDataGrid2(dataGridView2);
            int idSickleave = 0;

            if (dataGridView2.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView2.Rows[selectedrowindex];
                idSickleave = Convert.ToInt32(selectedRow.Cells["ID_Больничного"].Value);
            }
            return idSickleave;




        }
      

        private int IDSickleave()
        {

           
            var connection = DataBase.getConnection();
            var idClient = CellIdClient();
            connection.Open();
            String querystring2 = $"SELECT ID_Больничного,ID_диагноза, ID_Врача from Больничные where ID_Пациента = '{idClient}' and CAST(Дата_Выписки AS DATETIME2(3)) is null ";

            SqlCommand command2 = new SqlCommand(querystring2, connection);

            var reader = command2.ExecuteReader();
            reader.Read();

            var id = (int)(reader.GetInt32(0));
            reader.Close();
            connection.Close();
            return id;

        }

     



        private int? IDClient()
        {
          
            var idClient = CellIdClient();
            if(idClient <= 0)
            {
                return null;
            }
            

           var connection = DataBase.getConnection();

            connection.Open();
            String querystring2 = $"SELECT ID_Пациента from Пациент where ID_Пациента = '{idClient}'";

            SqlCommand command2 = new SqlCommand(querystring2, connection);

            var reader = command2.ExecuteReader();
            reader.Read();

            var id = (int)(reader.GetInt32(0));
            reader.Close();
            connection.Close();
            return id;


        }
        private bool? checkSickLeave()
        {
            var doctor = comboBox1.SelectedIndex;
            if (doctor == -1)
            {


                return null;
            }
            DataBase.getConnection().Close();
            var idClient = IDClient();
            DataBase.getConnection().Close();
            string querystring = $"select ID_Врача,Дата_Выписки from Больничные where ID_Врача = '{CurrentDoctor}' and ID_Пациента = '{idClient}' and CAST(Дата_Выписки AS DATETIME2(3)) is null ";
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
            DataBase.getConnection().Close();
            var IdClient = IDClient();
            DataBase.getConnection().Close();
            if (IdClient == null)
            {
                MessageBox.Show("Выберите пациента из таблицы!", "Ошибка!");
                return;
            }
            if (checkSickLeave()== false)
            {
                MessageBox.Show("У этого пациента уже есть прием!", "Ошибка!");
                return;

            }
            var diagnoz = comboBox1.SelectedIndex;
            
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Укажите диагноз!", "Ошибка!");
                return;
            }
            var IDdiagnoz = savedDiagnozID[diagnoz];
            var date1 = dateTimePicker1.Value.ToString("yyyyMMdd");
           
            var note = textBox2.Text;
            var drugs = textBox3.Text;
            string querystring = $"Insert into Больничные ([ID_Пациента], [ID_Диагноза],[ID_Врача], Дата_Начала_Заболевания, Предписание, Лекарства) values('{IdClient}','{IDdiagnoz}','{CurrentDoctor}', '{date1}', '{note}','{drugs}')";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();
           
            if (command.ExecuteNonQuery() == 1)
            {
                DataBase.getConnection().Close();
                var IDsickLeave = IDSickleave();
                DataBase.getConnection().Close();
                
                var IDclient = IDClient();
                DataBase.getConnection().Close();
                DataBase.getConnection().Open();
                
                var date = DateTime.Now;
                string querystring2 = $"Insert into Состоявщийся_Прием ([ID_Врача], [ID_Пациента],[ID_Больничного], Дата_Посещения) values('{CurrentDoctor}','{IDclient}','{IDsickLeave}', '{date}')";
                SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
                DataBase.openConnection();

                command2.ExecuteNonQuery();

                RefreshDataGrid2(dataGridView2);
                dataGridView2.ClearSelection();
            }
            
           

        }


        private void createColumns()
        {
            dataGridView1.Columns.Add("ID_Пациента", "ID");
            this.dataGridView1.Columns["ID_Пациента"].Visible = false;
            dataGridView1.Columns.Add("ID_Медкарты", "ID_Медкарты");
            this.dataGridView1.Columns["ID_Медкарты"].Visible = false;
            dataGridView1.Columns.Add("ФИО", "Пациент");
            dataGridView1.Columns.Add("Дата_Рождения", "Дата рождения");
            dataGridView1.Columns.Add("Адрес", "Адрес");
           

        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetString(2) + " " + record.GetString(3) + " " + record.GetString(4), record.GetDateTime(5).ToString("dd.MM.yyyy"), record.GetString(6));

        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchQuerry = $"select [ID_Пациента], ID_Медкарты, Фамилия, Имя,  Отчество, [Дата_Рождения], [Адрес] from Пациент where concat (Фамилия , ' ' , Имя ,' ' , Отчество , ' ' ,CONVERT(VARCHAR(10),Дата_Рождения,104),' ', Адрес) like '%" + textBox4.Text + "%'";
            SqlCommand command = new SqlCommand(searchQuerry, DataBase.getConnection());
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

            dgw.Rows.Clear();



            string queryString = $"select [ID_Пациента], ID_Медкарты, Фамилия, Имя, Отчество, [Дата_Рождения], [Адрес] from Пациент";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow(dgw, reader);


            }
            reader.Close();

        }
        private void createColumns2()
        {
            
            dataGridView2.Columns.Add("ID_Больничного", "ID_Больничного");
            this.dataGridView2.Columns["ID_Больничного"].Visible = false;
            dataGridView2.Columns.Add("Наименование", "Диагноз");
            dataGridView2.Columns.Add("Дата_начала_заболевания", "Дата заболевания");
            dataGridView2.Columns.Add("Предписание", "Предписание");
            dataGridView2.Columns.Add("Лекарства", "Лекарства");

        }
        private void ReadSingleRow2(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0),record.GetString(1), record.GetDateTime(2).ToString("dd.MM.yyyy"), record.GetString(3), record.GetString(4));

        }
        private void RefreshDataGrid2(DataGridView dgw)
        {

            dgw.Rows.Clear();

            DataBase.getConnection().Close();
            var idClient = IDClient();
            DataBase.getConnection().Close();

            if(idClient == null)
            {

                return;
            }
            string queryString = $"select ID_Больничного, Наименование, Дата_Начала_Заболевания, Предписание, Лекарства from Больничные inner join Пациент on Пациент.ID_Пациента = Больничные.ID_Пациента inner join Диагноз on Диагноз.ID_Диагноза = Больничные.ID_Диагноза where Больничные.ID_Пациента = {idClient} and CAST(Дата_Выписки AS DATETIME2(3)) is null";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                ReadSingleRow2(dgw, reader);


            }
            reader.Close();

        }
        
        private void DiagnozComboBox()
        {
            string querystring2 = $"select [ID_Диагноза], Наименование from Диагноз";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();
            List<string> list = new List<string>();
            List<int> list2 = new List<int>();

            using (SqlDataReader reader = command2.ExecuteReader())
            {

                while (reader.Read())
                {

                    var Diagnoz = reader.GetString(1);
                    list.Add(Diagnoz);
                    var DiagnozId = reader.GetInt32(0);
                    list2.Add((int)DiagnozId);


                }

            }

            savedDiagnozID = list2;
            foreach (var item in list)
            {

                comboBox1.Items.Add(item);

            }


        }
        List<int> savedDiagnozID = new List<int>();
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button2DizClick();
        }
        private void Visit_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            button2DizClick();
            textBox1.Clear();
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
            if (CellIdSickleave() <= 0)
            {
                
                button2DizClick();
                MessageBox.Show("Выберите больничный!","Ошибка!");
                return;
                
            }
            

            RefreshDataGrid2(dataGridView2);
            var idCellSick = CellIdSickleave(); 
            DataBase.closeConnection();
            var connection = DataBase.getConnection();
            connection.Open();
            String querystring2 = $"SELECT ID_Больничного,ID_диагноза, ID_Врача from Больничные where ID_Больничного = '{idCellSick}'";

                SqlCommand command2 = new SqlCommand(querystring2, connection);

            using (SqlDataReader reader = command2.ExecuteReader())
            {
                var userExist = reader.Read();

                if (userExist == false)
                {

                   
                    return;
                }

                var Id = reader.GetInt32(0);

                Visit2 vs2 = new Visit2(Id);

                vs2.ShowDialog();
                RefreshDataGrid2(dataGridView2);
                
            }

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
            RefreshDataGrid(dataGridView1);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {
            Search(dataGridView1);
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
    
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
               
             
                
                    string name = "";
                    if (row.Cells["ФИО"].Value != null)
                    {
                        name = row.Cells["ФИО"].Value.ToString();
                       
                    }
                    textBox1.Text = name;


            }
           
           
            

            RefreshDataGrid2(dataGridView2);
            dataGridView2.ClearSelection();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            textBox1.Clear();
            dataGridView2.ClearSelection();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            textBox1.Clear();
            dataGridView2.ClearSelection();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
