using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace test_DataBase
{
    public partial class sign_up : MetroFramework.Forms.MetroForm
    {


        DataBase DataBase = new DataBase();
        public sign_up()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void sign_up_Load(object sender, EventArgs e)
        {
       
            Password_TextBox2.MaxLength = 20;
            login_TextBox2.MaxLength = 20;
            name_TextBox.MaxLength = 20;
            Ot_textBox.MaxLength = 20;
            lastName_TextBox.MaxLength = 20;
            Adress_textBox.MaxLength = 20;

        }


       private int newMedCard()
        {
           var date = DateTime.Now;
            String querystring1 = $"insert into Медкарта Values ('{date}')";
            String querystring2 = $"SELECT IDENT_CURRENT('Медкарта')";

            var connection = DataBase.getConnection();
            SqlCommand command1 = new SqlCommand(querystring1, connection);
            connection.Open();

            command1.ExecuteNonQuery();

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

            var name = name_TextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите ваше имя!", "Ошибка!");
                return;


            }

            var lastname = lastName_TextBox.Text;
            if (string.IsNullOrEmpty(lastname))
            {
                MessageBox.Show("Введите вашу фамилию!", "Ошибка!");
                return;


            }

            var ot = Ot_textBox.Text;
            if (string.IsNullOrEmpty(ot))
            {
                MessageBox.Show("Введите ваше отчество!", "Ошибка!");
                return;


            }

            var login = login_TextBox2.Text;

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин!", "Ошибка!");
                return;


            }


            var password = Password_TextBox2.Text;
            
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка!");
                return;


            }

            var DOB = dateTimePicker.Value.ToString("yyyy-MM-dd");
            if (string.IsNullOrEmpty(DOB))
            {
                MessageBox.Show("Введите вашу дату рождения!", "Ошибка!");
                return;


            }

            var adress = Adress_textBox.Text;

            if (string.IsNullOrEmpty(adress))
            {
                MessageBox.Show("Введите ваш адрес!", "Ошибка!");
                return;


            }


            var Sex = Sex_comboBox.SelectedIndex;

            if (Sex == -1)
            {
                MessageBox.Show("Введите ваш пол!", "Ошибка!");
                return;


            }
            

            if (checkuser())
            {
                return;
            }

            var medcard = newMedCard();

            string querystring = $"insert into Пациент ([ID_Медкарты], Login_user, Password_user,Фамилия,Имя,Отчество,[Дата_Рождения],Адрес,Пол) values('{medcard}','{login}','{password}','{lastname}','{name}','{ot}','{DOB}','{adress}', '{Sex}') ";


            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());

            DataBase.openConnection();

            if(command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех!");
                log_in slog = new log_in();
                this.Hide();
                slog.ShowDialog(); 

            }
            else
            {

                MessageBox.Show("Аккаунт не создан!");


               

            }
            DataBase.closeConnection();

        }

       
        private Boolean checkuser()
        {

            var loginUser = login_TextBox2.Text;
            var passUser = Password_TextBox2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();  
            DataTable table = new DataTable();
            string querystring = $"select Login_user, Password_user from Пациент where Login_user = '{loginUser}' and Password_user = '{passUser}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);
            
            if(table.Rows.Count > 0 ) 
            {
                MessageBox.Show("Пользователь уже существует!");
                return true;
            }
            else
            {
                return false;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            Password_TextBox2.PasswordChar = '*';

            if (checkBox2.Checked)
            {
                Password_TextBox2.UseSystemPasswordChar = true;
            }
            else
            {
                Password_TextBox2.UseSystemPasswordChar = false;
            }

        }


        private void Password_TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void name_TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            log_in log = new log_in();
            log.Show();
        }

        private void Password_TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Sex_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
