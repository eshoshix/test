using MetroFramework.Components;
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


namespace test_DataBase
{
    public partial class log_in : MetroFramework.Forms.MetroForm
    {

        DataBase DataBase = new DataBase();
        public log_in()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

        }

        private void log_in_Load(object sender, EventArgs e)
        {
            Password_TextBox.PasswordChar = '*';
            Password_TextBox.MaxLength = 20;
            login_TextBox.MaxLength = 20;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }

        private void reg_button_Click(object sender, EventArgs e)
        {

            var loginUser = login_TextBox.Text;
            var passUser = Password_TextBox.Text;

           
            
            string querystring = $"select [ID_Пациента], Login_user, Password_user from Пациент where Login_user = '{loginUser}' and Password_user = '{passUser}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());



            DataBase.openConnection();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                

                var userExist = reader.Read();

                if(userExist == false)
                {

                    MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                var Id = reader.GetInt32(0);
              

                MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Client_acc CA = new Client_acc(Id);
                this.Hide();
                CA.ShowDialog();
                this.Show();
                




            }


        }
    


        private void Password_TextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

       



        private void login_TextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sign_up sig = new sign_up();
            
                sig.Show();
                this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {


            
           
            if (checkBox1.Checked)

            {

                Password_TextBox.UseSystemPasswordChar = true;

            }
            else
            {

                
                Password_TextBox.UseSystemPasswordChar = false;
            }



        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            admin_Login adlog = new admin_Login();
            adlog.Show();
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Password_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {

                e.Handled = true;
            }
        }
    }


}
