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
    public partial class Add_Description_About_Clinic_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        public Add_Description_About_Clinic_UserControl()
        {
            InitializeComponent();
        }

        public bool? UploadPhoto()
        {
            DataBase.openConnection();
            string insertCommand = $"update Описание SET Фото = @Image where ID = 1";
            using (var cmd = new SqlCommand(insertCommand, DataBase.getConnection()))
            {

                using (FileStream fs = new FileStream(img1, FileMode.Open, FileAccess.Read))
                {
                    byte[] imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, (int)fs.Length);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Image", imageBytes);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            DataBase.closeConnection();


        }
        private void loadout()
        {
            DataBase.openConnection();
            var dobavkaQuery = $"select Фото from Описание where ID = 1";
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
        private void loadout2()
        {
            DataBase.openConnection();
            var dobavkaQuery = $"select Фото from Описание where ID = 2";
            var command = new SqlCommand(dobavkaQuery, DataBase.getConnection());
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            var image = reader["Фото"];
            if (image == null || image == System.DBNull.Value)
                pictureBox2.Image = null;
            else
            {
                var bytes = (byte[])image;
                using (var ms = new MemoryStream(bytes))
                {
                    var kartinka = Image.FromStream(ms);
                    pictureBox2.Image = kartinka;
                }
            }

            DataBase.closeConnection();
        }


        private void textLoadOut()
        {
            string queryString = $"select Описание from Описание where ID = 3";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();


                object column1Data = reader["Описание"].ToString();

                richTextBox1.Text = column1Data.ToString();


            }

        }
        private void textLoadOut2()
        {
            string queryString = $"select Описание from Описание where ID = 4";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();


                object column1Data = reader["Описание"].ToString();

                richTextBox2.Text = column1Data.ToString();


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *jpeg, *.png) | *.jpg; *.jpeg; *.png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                img1 = ofd.FileName.ToString();
                pictureBox1.ImageLocation = img1;
              

            }
            return;
        }
             string img1 = "";

        private void Add_Description_About_Clinic_UserControl_Load(object sender, EventArgs e)
        {
            loadout();
            textLoadOut();
            loadout2();
            textLoadOut2();

        }

        private void text()
        {
            var description = richTextBox1.Text;
            

            string queryString = $"update Описание set Описание = '{description}' where ID = 3";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();

        }
        private void text2()
        {
            var description = richTextBox2.Text;


            string queryString = $"update Описание set Описание = '{description}' where ID = 4";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            text();
            text2();
            MessageBox.Show("Обновления сохранены", "Успех");
            if (img1 !="")
            {
                UploadPhoto();
               
            }
            if (img2 !="")
            {

                UploadPhoto2();
            }
            

        }
        string img2 = "";
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *jpeg, *.png) | *.jpg; *.jpeg; *.png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                img2 = ofd.FileName.ToString();
                pictureBox2.ImageLocation = img2;


            }
            return;
        }

        public bool? UploadPhoto2()
        {
            DataBase.openConnection();
            string insertCommand = $"update Описание SET Фото = @Image where ID = 2";
            using (var cmd = new SqlCommand(insertCommand, DataBase.getConnection()))
            {

                using (FileStream fs = new FileStream(img2, FileMode.Open, FileAccess.Read))
                {
                    byte[] imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, (int)fs.Length);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Image", imageBytes);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            DataBase.closeConnection();


        }

    }
}
