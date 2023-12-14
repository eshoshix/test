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

        private void button1_Click(object sender, EventArgs e)
        {
            
           
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddDoctor AddDoctor = new AddDoctor();
            
            AddDoctor.ShowDialog();



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



        private void FormAdmin_Load(object sender, EventArgs e)
        {
            label();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DiagnozForm Diagnoz = new DiagnozForm();
         
            Diagnoz.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PlannedVisits PlannedVisits = new PlannedVisits(CurrentId);

            PlannedVisits.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            admin_Login login = new admin_Login();
            login.Close();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
           timetable tt = new timetable(CurrentId);
            tt.ShowDialog();

        }

        
          
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int id = CurrentId;
            Visit vv = new Visit(id);
            vv.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click_1(object sender, EventArgs e)
        {

            EditDoctorsAcc efd = new EditDoctorsAcc(CurrentId);  
            efd.ShowDialog();
        }
    }
}
