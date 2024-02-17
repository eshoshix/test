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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;


namespace test_DataBase
{
    public partial class Statistic_UserControl : UserControl
    {
        int CurrentClient;
        DataBase DataBase = new DataBase();
        public Statistic_UserControl(int ID)
        {
            CurrentClient = ID;
            InitializeComponent();
        }

        private void Statistic_UserControl_Load(object sender, EventArgs e)
        {
            statisticByMonth();
            statisticByYear();
            statisticByYearForClient();
        }
    
        private void statisticByMonth()
        {

            string queryString = $"select count (Наименование) as 'Количество',CAST (count(*) * 100.0 / (select count(*) from Больничные where Дата_начала_заболевания >= DATEADD(mm,DATEDIFF(mm,0,DATEADD(MM,-1,GETDATE())),0)) as decimal(9,2))  as 'Процент', Наименование from Больничные inner join Диагноз on Диагноз.ID_Диагноза = Больничные.ID_Диагноза  where Дата_начала_заболевания >= DATEADD(DD,DATEDIFF(DD,0,DATEADD(MM,-1,GETDATE())),0)  group by Наименование";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();
            int count = 1;
            while (reader.Read())
            {
                
                object column1 = reader["Процент"];
                object column2 = reader["Наименование"];
                object column3 = reader["Количество"];

                chart1.Series.Add(column2 + " " + column1 + "%" + " " + "(" + column3 + " чел." + ")".ToString());
                chart1.Series[column2 + " " + column1 + "%" + " " +"("+ column3 + " чел." + ")".ToString()].Points.AddXY(count, column1);

               
                count++;

            }
            reader.Close();

        }
        private void statisticByYear()
        {

            string queryString = $"select count (Наименование) as 'Количество', CAST (count(*) * 100.0 / (select count(*) from Больничные where Дата_начала_заболевания >= DATEADD(mm,DATEDIFF(mm,0,DATEADD(YY,-1,GETDATE())),0)) as decimal(9,2))  as 'Процент', Наименование from Больничные inner join Диагноз on Диагноз.ID_Диагноза = Больничные.ID_Диагноза  where Дата_начала_заболевания >= DATEADD(DD,DATEDIFF(DD,0,DATEADD(YY,-1,GETDATE())),0)  group by Наименование";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();
            int count = 1;
            while (reader.Read())
            {

                object column1 = reader["Процент"];
                object column2 = reader["Наименование"];
                object column3 = reader["Количество"];

                chart2.Series.Add(column2 + " " + column1 + "%" + " " + "(" + column3 + " чел." + ")".ToString());
                chart2.Series[column2 + " " + column1 + "%" + " " + "(" + column3 + " чел." + ")".ToString()].Points.AddXY(count, column1);


                count++;

            }
            reader.Close();

        }

        private void statisticByYearForClient()
        {

            string queryString = $"select count (Наименование) as 'Количество', Наименование from Больничные inner join Диагноз on Диагноз.ID_Диагноза = Больничные.ID_Диагноза where Дата_начала_заболевания >= DATEADD(yy,DATEDIFF(yy,0,DATEADD(yy,-1,GETDATE())),0) and ID_Пациента = 4 group by Наименование";

            SqlCommand command = new SqlCommand(queryString, DataBase.getConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();
            int count = 1;
            while (reader.Read())
            {
                object column1 = reader["Наименование"];
                object column2 = reader["Количество"];


              
                chart3.Series.Add("Вы болели " + "\"" + column1.ToString() + "\"" + " " + column2.ToString() + " раз");
                chart3.Series["Вы болели " + "\"" + column1.ToString() + "\"" + " " + column2.ToString() + " раз"].Points.AddXY(count, column2);


                count++;
               
               

            }
            if (count < 2)
            {
                label6.Text = "Статистика отсутствует";
                label5.Text = "Статистика отсутствует";
                label4.Text = "Статистика отсутствует";

            }
            reader.Close();

        }


        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void chart3_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}
