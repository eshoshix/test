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
    public partial class EditDoctorsAcc : Form
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
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            UploadPhoto();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
