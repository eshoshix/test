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

namespace test_DataBase
{
    public partial class admin_Login : MetroFramework.Forms.MetroForm
    {
        DataBase DataBase = new DataBase();

        public admin_Login()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void admin_Load(object sender, EventArgs e)
        {
            Password_TextBox.PasswordChar = '*';
            Password_TextBox.MaxLength = 20;
            login_TextBox.MaxLength = 20;
        }

        private void log_button_Click(object sender, EventArgs e)
        {
            var loginUser = login_TextBox.Text;
            var passUser = Password_TextBox.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select [ID_Врача], Login_user, Password_user from Врач where Login_user = '{loginUser}' and Password_user = '{passUser}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());



            DataBase.openConnection();
            using (SqlDataReader reader = command.ExecuteReader())
            {


                var userExist = reader.Read();

                if (userExist == false)
                {

                    MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                var Id = reader.GetInt32(0);


                MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);


                FormAdmin FormAdmin = new FormAdmin(Id);
                this.Hide();
                FormAdmin.ShowDialog();
                this.Show();
                PlannedVisits PlannedVisits = new PlannedVisits(Id);
                



            }
        
            
        
        
        
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            log_in log = new log_in();

            log.Show();
          
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
