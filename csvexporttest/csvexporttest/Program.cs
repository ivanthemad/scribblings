using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Data.SqlClient;

namespace csvexporttest
{
    class Program
    {
        static void Main(string[] args) {

            List<double> times = new List<double>();

            Program p = new Program();

            for (int i = 0; i < 0; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                p.export();

                stopwatch.Stop();

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
                times.Add(ts.Seconds);
            }

            Console.WriteLine();
            double totalTime = 0;
            foreach (double s in times)
            {
                totalTime += s;
            }

            double avgTime = 0.0;
            if (totalTime != 0) avgTime = (totalTime / times.Count);

            Console.WriteLine("average of " + times.Count + " runs: " + avgTime + "(s)");
            Console.ReadKey();
            
        }
        void export()
        {
            SqlConnection myConnection = new SqlConnection("user id=USER;" +
               "password=PASSWORD;server=SERVER;" +
               "Trusted_Connection=yes;" +
               "database=DATABASE; " +
               "connection timeout=99999");

            try
            {
                myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select * from table",
                                                         myConnection);

                StringBuilder sb = new StringBuilder();
                StringBuilder sbColNames = new StringBuilder();
                StreamWriter sw = new StreamWriter("temp.csv");

                myReader = myCommand.ExecuteReader();

                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    sbColNames.Append(myReader.GetName(i));
                    if (i != (myReader.FieldCount - 1))
                    {
                        sbColNames.Append(",");
                    }
                }
                sw.Write(sbColNames.ToString());
                sw.Write("\n");
                sbColNames.Clear();

                while (myReader.Read())
                {
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {

                        sb.Append(myReader[i].ToString());
                        if (i != (myReader.FieldCount - 1))
                        {
                            sb.Append(",");
                        }
                    }

                    sw.Write(sb.ToString());
                    sw.Write("\n");
                    sb.Clear();
                } 

                try
                {
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            try
            {
                myConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            

        }
    }
}
