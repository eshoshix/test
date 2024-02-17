using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_DataBase
{
    public partial class Talon_Print : Form
    {
        DataBase DataBase = new DataBase();
        int Current;
        public Talon_Print(int ID)
        {
            Current = ID;
            InitializeComponent();
        }
       
        private void Talon_Print_Load(object sender, EventArgs e)
        {


            checkDayAppointment();
          
            print();








        }
        private void print() 
        {


            PrintDocument doc = new PrintDocument();
            doc.PrintPage += this.printDocument1_PrintPage;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = doc;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }


        }
        private void checkDayAppointment()
        {
           

       
            string querystring = $"select [ID_Приёма],Пациент.Имя,Пациент.Фамилия,Пациент.Отчество,[Дата_Посещения], ФИО,Специальность,[Номер кабинета],Талон from Запись_Прием inner join Пациент on Пациент.[ID_Пациента] = Запись_Прием.[ID_Пациента] inner join Врач on Врач.[ID_Врача] = Запись_Прием.[ID_Врача] inner join Кабинет on Кабинет.ID_Кабинета = Врач.ID_Кабинета where ID_Приёма = {Current}";
            SqlCommand command = new SqlCommand(querystring, DataBase.getConnection());
            DataBase.openConnection();
            command.ExecuteNonQuery();

        

            using (SqlDataReader reader = command.ExecuteReader())
            {



                while (reader.Read())
                {
                  
                    object column1 = reader.GetValue(1);
                    object column2 = reader.GetValue(2);
                    object column3 = reader.GetValue(3);
                    DateTime column4 = reader.GetDateTime(4);
                    string date = column4.ToString("dd.MM.yyyy HH:mm");
                    object column5 = reader.GetValue(5);
                    object column6 = reader.GetValue(6);
                    object column7 = reader.GetValue(7);
                    object column8 = reader.GetValue(8);
                    object column9 = reader.GetValue(0);
                    richTextBox1.Text = "           Талон для посещения врача" + "\n\n" + "Номер приема: " + column9 + "\n"+  "ФИО пациента: " + column1 + " " + column2 + " " + column3 + "\n" + "Дата посещения: " + date + "\n" + "ФИО врача: " + column5 + "\n" + "Специальность: " + column6 + "\n" +  "Номер кабинета: " + column7 + "\n" + "Номер талона: " + column8;    



                }





            }
          
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(bmp, new Rectangle(0, 0, panel1.Width, panel1.Height));
            e.Graphics.DrawImage((Image)bmp, x, y);


        }

       
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        
           

        }
    }
}
