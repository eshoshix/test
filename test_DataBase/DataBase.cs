using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;




namespace test_DataBase
{
     class DataBase
     {
        //@"Data Source=5.44.47.210; Initial Catalog=ebaTest; Integrated Security=false; User ID=sa;Password=OJX0jtd4srKcjvcx-aa3;"
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=exPC; Initial Catalog=EbaTest2; Integrated Security=True");


        public void openConnection()
        {

            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {

                sqlConnection.Open();

            }



        }


        public void closeConnection()
        {

            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {

                sqlConnection.Close();

            }



        }



        public SqlConnection getConnection()
        {

            return sqlConnection;
        }




     }








}
