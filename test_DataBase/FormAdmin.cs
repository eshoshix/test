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

    public partial class FormAdmin : MetroFramework.Forms.MetroForm
    {
        DataBase DataBase = new DataBase();
        int CurrentId;
        public FormAdmin(int id)
        {
            CurrentId = id;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void FormAdmin_Load(object sender, EventArgs e)
        {
            label();
        }
        private void label()
        {


            string querystring = $"select ФИО from Врач where [ID_Врача] = '{CurrentId}'";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();



            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                string column1Data = reader["ФИО"].ToString();

              

                label1.Text = "Личный кабинет: " + $"{column1Data} ";



            }




        }
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(userControl);
            userControl.BringToFront();
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            appointments_UserControl appD = new appointments_UserControl(CurrentId);
            addUserControl(appD);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Visit_UserControl Vs1 = new Visit_UserControl(CurrentId);
            addUserControl(Vs1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Schedule_Edit_UserControl appD = new Schedule_Edit_UserControl(CurrentId);
            addUserControl(appD);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int ID = CurrentId;
           Edit_AccDoctor_UserControl appD = new Edit_AccDoctor_UserControl(ID);
            addUserControl(appD);
        }
    }
}
