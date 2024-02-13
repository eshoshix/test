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
    public partial class Edit_cabinet_UserControl : UserControl
    {
        DataBase DataBase = new DataBase();
        int IDCurrent;
        public Edit_cabinet_UserControl(int ID)
        {
            IDCurrent = ID;
            
            InitializeComponent();
        }
        string img = "";
        private void Edit_cabinet_UserControl_Load(object sender, EventArgs e)
        {
            doctorComboBox();
            show();
            loadout();
            CabinetsComboBox();
            cabinet123();
        }
        private void doctorComboBox()
        {

            string querystring2 = $"select distinct Специальность from Врач";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();


            List<string> listSpec = new List<string>();



            using (SqlDataReader reader = command2.ExecuteReader())
            {

                while (reader.Read())
                {

                    var doctorSpec1 = reader.GetString(0);
                    listSpec.Add((string)doctorSpec1);

                }


            }

            foreach (var item in listSpec)
            {

                comboBox2.Items.Add(item);

            }

        }
        private void show()
        {
            string queryString = $"SELECT ФИО, Специальность,[Стоимость посещения],Описание_Врача, DATEDIFF(YEAR, Дата_Найма, GETDATE()) as 'Лет' , DATEDIFF(MONTH, Дата_Найма, GETDATE()) % 12 as 'Месяцев' FROM Врач where ID_Врача = '{IDCurrent}'";
            string queryString2 = $"select count (ID_Больничного) as 'Количество приемов' from Больничные where ID_Врача = '{IDCurrent}'";
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

                
                textBox6.Text = column1Data.ToString();
                comboBox2.Text = column2Data.ToString();
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
        private void cabinet123()
        {
            string querystring2 = $"select [Номер кабинета] from Кабинет inner join Врач on Кабинет.ID_Кабинета = Врач.ID_Кабинета where Врач.ID_Врача = '{IDCurrent}'";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();
            using (SqlDataReader reader = command2.ExecuteReader())
            {
                reader.Read();

              object column1 = reader.GetValue(0);  
              comboBox1.Text = column1.ToString(); 



            }

        }
        private void CabinetsComboBox()
        {

            string querystring2 = $"select * from Кабинет";
            SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
            DataBase.openConnection();


            List<string> listCab = new List<string>();
            List<int> listCabID = new List<int>();


            using (SqlDataReader reader = command2.ExecuteReader())
            {

                while (reader.Read())
                {

                    var cab = reader.GetString(1);
                    listCab.Add((string)cab);
                    var cabID = reader.GetInt32(0);
                    listCabID.Add((int)cabID);

                }



            }
            savedCabID = listCabID;
            foreach (var item in listCab)
            {

                comboBox1.Items.Add(item);

            }
           
        }
        List<int> savedCabID = new List<int>();

        private void button1_Click(object sender, EventArgs e)
        {
          

        }
        public void UploadPhoto()
        {
            DataBase.openConnection();
            string insertCommand = $"update Врач SET Фото = @Image where ID_Врача = {IDCurrent}";
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
        private void loadout()
        {
            DataBase.openConnection();
            var dobavkaQuery = $"select Фото from Врач where ID_Врача = {IDCurrent}";
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
        private void button2_Click(object sender, EventArgs e)
        {
            var name = textBox6.Text;
            var spec = comboBox2.Text;
            var cost = textBox3.Text;
            var description = richTextBox1.Text;
            string queryString = $"update Врач set ФИО = '{name}', Специальность = '{spec}' , [Стоимость посещения] = '{cost}', Описание_Врача = '{description}' where ID_Врача = '{IDCurrent}'";
            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();
            MessageBox.Show("Данные обновлены!", "Успех");
            var cabinet = comboBox1.SelectedIndex;
            var cabinetID = savedCabID[cabinet];
            if (cabinetID > 0)
            {
               
                string querystring2 = $"update Врач set ID_Кабинета = '{cabinetID}' where ID_Врача = ' {IDCurrent}'";
                SqlCommand command2 = new SqlCommand(querystring2, DataBase.getConnection());
                string querystring3 = $"Delete Кабинет_Врач where ID_Врача = '{IDCurrent}'";
                SqlCommand command3 = new SqlCommand(querystring3, DataBase.getConnection());
                string querystring4 = $"insert into Кабинет_Врач (ID_Кабинета,ID_Врача) values('{cabinetID}','{IDCurrent}')";
                SqlCommand command4 = new SqlCommand(querystring4, DataBase.getConnection());
                command2.ExecuteNonQuery();
                command3.ExecuteNonQuery();
                command4.ExecuteNonQuery();
                DataBase.openConnection();

                cabinet123();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *jpeg, *.png) | *.jpg; *.jpeg; *.png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                img = ofd.FileName.ToString();
                pictureBox1.ImageLocation = img;
                UploadPhoto();
                MessageBox.Show("Фотография обновлена!", "Успех");

            }
            return;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
         (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

           
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
          
        }
    }
}
