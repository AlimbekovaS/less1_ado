using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace ADO.NET
{
    class Program
    {
        SqlConnection conn = null;
        public Program() //(localdb)\v11.0
        {
            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=(localdb)\v11.0; Initial Catalog = Library; Integrated Security = SSPI;";
            //conn= new 
        }
        static void Main(string[] args)
        {
            Program pr = new Program();
            //pr.InsertQuery();
            //pr.ReadData();
            //pr.ReadData1();
           // pr.ReadData2();
            pr.ExecStoreProcedure();
        }
        public void InsertQuery()
        {
            try
            {
                //откр соединение
                conn.Open();
                //подготовить запрос insert
                // в переменной типа string
                string insertString = @"insert into Books(AuthorsID, Title) values ('Kuanush', 'Shonbai')";
                //создать объект command
                //инициализация 
                SqlCommand cmd = new SqlCommand(insertString, conn);
                //выолнить запрос
                //в объект command
                cmd.ExecuteNonQuery();
            }
            finally
            {
                //присоединенный режим работы
                //закрыть соединение
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void ReadData()
        {
            SqlDataReader rdr = null;
            try
            {
                //откр соединение
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Authors", conn);

                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine(rdr[1] + " " + rdr[2]);
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void ReadData1()
        {
            SqlDataReader rdr = null;
            try
            {
                //откр соединение
                conn.Open();
                //cоздать новый объект command с запросом select
                SqlCommand cmd = new SqlCommand("select * from Authors", conn);
                //выполнить запрос select сохранение
                rdr = cmd.ExecuteReader();
                int line = 0;//счетчик строк
                //извлеч полученные строки

                while (rdr.Read())
                {
                    if (line == 0)
                    {

                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            Console.Write(rdr.GetName(i).ToString() + " ");
                        }
                    }
                    Console.WriteLine();
                    line++;
                    Console.WriteLine(rdr[0] + " " + rdr[1]+" "+rdr[2]);
                    
                }
                Console.WriteLine("Всего обработано записей: " + line.ToString());
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void ReadData2()
        {
            SqlDataReader rdr = null;
            try
            {
                //откр соединение
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Authors; select * from Books", conn);

                rdr = cmd.ExecuteReader();
                int line = 0;
                //извлечь полученные строки
                do
                {
                    while (rdr.Read())
                    {
                        if (line == 0)//формируем шапку
                                      //таблицы перед выводом первой строки
                        {
                            //цикл по числу прочитанных полей
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                //вывести в консольное окно
                                //имена полей
                                Console.Write(rdr.GetName(i).ToString() + "\t");
                            }
                            Console.WriteLine();
                        }
                        line++;
                        Console.WriteLine(rdr[0] + "\t" + rdr[1] + "\t" + rdr[2]);
                    }
                    Console.WriteLine("Всего обработано записей: " + line.ToString());
                } while (rdr.NextResult());
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public void ExecStoreProcedure()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetBooksNumber", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AuthorsID", SqlDbType.Int).Value = 2;
            SqlParameter outputParam = new SqlParameter("@BookCount", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            //outputParam.Value=0; //заполнять value не надо!
            cmd.Parameters.Add(outputParam);
            cmd.ExecuteNonQuery();
            Console.WriteLine(cmd.Parameters["@BookCount"].Value.ToString());


        }

    }
}
