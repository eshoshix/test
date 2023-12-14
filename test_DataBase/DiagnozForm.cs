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
    public partial class DiagnozForm : MetroFramework.Forms.MetroForm
    {

        DataBase DataBase = new DataBase();
        public DiagnozForm()
        {
            InitializeComponent();
        }

        private void reg_button_Click(object sender, EventArgs e)
        {
            var name = name_TextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите ваше имя!", "Ошибка!");
                return;


            }

            

           
            string querystring = $"insert into Диагноз (Наименование) values('{name}')";


            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());

            DataBase.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех!");

                this.Hide();


            }
            else
            {

                MessageBox.Show("Аккаунт не создан!");




            }
            DataBase.closeConnection();
        }

        private void DiagnozForm_Load(object sender, EventArgs e)
        {

        }
    }
}
    

