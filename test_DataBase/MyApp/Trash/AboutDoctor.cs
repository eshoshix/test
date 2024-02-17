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
    public partial class AboutDoctor : MetroFramework.Forms.MetroForm
    {
        DataBase DataBase = new DataBase(); 
        int CurrentDoctor;
        public AboutDoctor(int ID)
        {
            StartPosition = FormStartPosition.CenterScreen;
            CurrentDoctor = ID; 
            InitializeComponent();
        }

        private void AboutDoctor_Load(object sender, EventArgs e)
        {
            Info();
            loadout();
        }
    
        private void Info()
        {
           

            string queryString = $"SELECT ФИО, Специальность,[Стоимость посещения],Описание_Врача, DATEDIFF(YEAR, Дата_Найма, GETDATE()) as 'Лет' , DATEDIFF(MONTH, Дата_Найма, GETDATE()) % 12 as 'Месяцев' FROM Врач where ID_Врача = '{CurrentDoctor}'";
            string queryString2 = $"select count(ID_Больничного) as 'Количество приемов' from Больничные where ID_Врача = '{CurrentDoctor}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            SqlCommand command2 = new SqlCommand(queryString2, DataBase.getConnection());
            DataBase.openConnection();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                object column1Data = reader["ФИО"];
                object column2Data = reader["Специальность"];
                double column3Data = Convert.ToDouble(reader["Стоимость посещения"]);
                
                object column4Data = reader["Лет"];
                object column5Data = reader["Месяцев"];
                string column6Data = reader["Описание_Врача"].ToString();

                label1.Text = column1Data.ToString();
                textBox1.Text = column1Data.ToString(); 
                textBox2.Text = column2Data.ToString();
                textBox3.Text = Math.Round(column3Data).ToString() + " руб.";
                textBox4.Text = column4Data.ToString() + " Лет" +  " " + column5Data.ToString() + " Месяцев";
                if (string.IsNullOrEmpty(column6Data))
                {
                    richTextBox1.Text = "Подробная информация отсутствует";
                }
                else
                richTextBox1.Text = column6Data.ToString(); 

            }

            using (SqlDataReader reader = command2.ExecuteReader())
            {
                reader.Read();

                
               object column6Data = reader["Количество приемов"].ToString();

                textBox5.Text = column6Data.ToString();

            }



        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }


        private void loadout()
        {
            DataBase.openConnection();
            var dobavkaQuery = $"select Фото from Врач where ID_Врача = {CurrentDoctor}";
            var command = new SqlCommand(dobavkaQuery, DataBase.getConnection());
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            var image = reader["Фото"];
            if (image == null || image == System.DBNull.Value)
                pictureBox1.Image = null;
            else
            {
                var bytes = (byte[])image;
                using (var ms = new MemoryStream(bytes))
                {
                    var kartinka = Image.FromStream(ms);
                    pictureBox1.Image = kartinka;
                }
            }

            DataBase.closeConnection();
        }
    }
}
