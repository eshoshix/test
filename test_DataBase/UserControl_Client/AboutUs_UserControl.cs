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
    
    public partial class AboutUs_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        public AboutUs_UserControl()
        {
            InitializeComponent();
        }

        private void AboutUs_UserControl_Load(object sender, EventArgs e)
        {

            info();
            info2();
            loadout();
            loadout2();

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
        private void info()
        {
            string queryString = $"select Описание from Описание where ID = 3";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                object column1Data = reader["Описание"];
                richTextBox1.Text = column1Data.ToString();


            }
            reader.Close();


        }
        private void info2()
        {
            string queryString = $"select Описание from Описание where ID = 4";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                object column1Data = reader["Описание"];
                richTextBox2.Text = column1Data.ToString();


            }
            reader.Close();


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
