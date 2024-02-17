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
    public partial class EditDoctorsAcc : MetroFramework.Forms.MetroForm
    {
        int currentId;
        DataBase DataBase = new DataBase();
        public EditDoctorsAcc(int id)
        {
            currentId = id;
            InitializeComponent();
        }

        private void EditDoctorsAcc_Load(object sender, EventArgs e)
        {
            show();
            loadout();
        }
        private void loadout()
        {
            DataBase.openConnection();
            var dobavkaQuery = $"select Фото from Врач where ID_Врача = {currentId}";
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
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *jpeg, *.png) | *.jpg; *.jpeg; *.png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                img = ofd.FileName.ToString();
                pictureBox1.ImageLocation = img;
                UploadPhoto();
                MessageBox.Show("Фотография обновлена!","Успех");

            }
            return;
        }

        string img = "";

        public void UploadPhoto()
        {
            DataBase.openConnection();
            string insertCommand = $"update Врач SET Фото = @Image where ID_Врача = {currentId}";
            using (var cmd = new SqlCommand(insertCommand, DataBase.getConnection()))
            {

                using (FileStream fs = new FileStream(img, FileMode.Open, FileAccess.Read))
                {
                    byte[] imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, (int)fs.Length);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Image", imageBytes);
                    cmd.ExecuteNonQuery();
                }
            }
            DataBase.closeConnection();


        }
        private void show()
        {
            string queryString = $"SELECT ФИО, Специальность,[Стоимость посещения],Описание_Врача, DATEDIFF(YEAR, Дата_Найма, GETDATE()) as 'Лет' , DATEDIFF(MONTH, Дата_Найма, GETDATE()) % 12 as 'Месяцев' FROM Врач where ID_Врача = '{currentId}'";
            string queryString2 = $"select count (ID_Больничного) as 'Количество приемов' from Больничные where ID_Врача = '{currentId}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            SqlCommand command2 = new SqlCommand(queryString2, DataBase.getConnection());
            DataBase.openConnection();
            command2.ExecuteNonQuery();
            

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
                textBox3.Text = Math.Round(column3Data).ToString();
                textBox4.Text = column4Data.ToString() + " Лет" + " " + column5Data.ToString() + " Месяцев";
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


                object column7Data = reader["Количество приемов"].ToString();

                textBox5.Text = column7Data.ToString();


            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            var name = textBox1.Text;
            var spec = textBox2.Text;
           var cost = textBox3.Text;
            var description = richTextBox1.Text;
            string queryString = $"update Врач set ФИО = '{name}', Специальность = '{spec}' , [Стоимость посещения] = '{cost}', Описание_Врача = '{description}' where ID_Врача = '{currentId}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection()); 
            DataBase.openConnection();
            command.ExecuteNonQuery();
            MessageBox.Show("Данные обновлены!", "Успех");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

    }


}
