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
using System.Xml.Linq;

namespace test_DataBase
{
    public partial class AddDoctor : MetroFramework.Forms.MetroForm
    {

        DataBase DataBase = new DataBase();
        public AddDoctor()
        {
            InitializeComponent();
        }


        private void AddDoctor_Load(object sender, EventArgs e)
        {
            logintextBox2.MaxLength = 20;
            PasswordtextBox1.MaxLength = 20;
            name_TextBox.MaxLength = 20;
            speciality_TextBox2.MaxLength = 20;
            Cost_TextBox2.MaxLength = 4;
            


        }
      
        private int newDoctor()
        {
            
            String querystring2 = $"SELECT IDENT_CURRENT('Врач')";

            var connection = DataBase.getConnection();
          
            connection.Open();

            SqlCommand command2 = new SqlCommand(querystring2, connection);

            var reader = command2.ExecuteReader();
            reader.Read();

            var id = (int)(reader.GetDecimal(0));
            reader.Close();

            connection.Close();
            return id;

        }
        private void reg_button_Click(object sender, EventArgs e)
        {
            var Login_user = logintextBox2.Text;
            if (string.IsNullOrEmpty(Login_user))
            {
                MessageBox.Show("Введите логин!", "Ошибка!");
                return;


            }

            var Password_user = PasswordtextBox1.Text;
            if (string.IsNullOrEmpty(Password_user))
            {
                MessageBox.Show("Введите пароль!", "Ошибка!");
                return;


            }
            var name = name_TextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите ФИО!", "Ошибка!");
                return;


            }
            

            var speciality = speciality_TextBox2.Text;
            if (string.IsNullOrEmpty(speciality))
            {
                MessageBox.Show("Введите специальность!", "Ошибка!");
                return;


            }

            var Cost = Cost_TextBox2.Text;
            if (string.IsNullOrEmpty(Cost))
            {
                MessageBox.Show("Введите стоимость посещения!", "Ошибка!");
                return;

            }

            var date = DateTime.Now;
            string querystring = $"insert into Врач (Login_user,Password_user, ФИО, Специальность, Дата_Найма, [Стоимость посещения]) values('{Login_user}','{Password_user}','{name}','{speciality}','{date}','{Cost}') ";


            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());

            DataBase.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Врач успешно Добавлен!", "Успех!");

                this.Hide();


            }
            else
            {

                MessageBox.Show("Врач не добавлен!");




            }
            DataBase.closeConnection();



        


            

            
        

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void logintextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PasswordtextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
              
            
        }
    }
}
