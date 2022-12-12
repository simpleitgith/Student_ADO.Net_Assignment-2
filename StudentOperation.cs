using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tally_Assignment2
{
    internal class StudentOperation
    {
        string connection = @"server = BLR1-LHP-N84399\SQLEXPRESS; database = TestDB; Integrated Security=true";
        public void AddStudent()
        {
            
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("usp_studentInsert", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Console.WriteLine("Enter Nmae, Address, Class");
            string name = Console.ReadLine();
            string address = Console.ReadLine();
            string standard = Console.ReadLine();
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@Class", standard);
            cmd.Parameters.Add("@sid", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            int getId = (int)cmd.Parameters["@sid"].Value;
            //Console.WriteLine(getId);
            Console.WriteLine("Enter the no of subjects:");
            int no = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < no; i++)
            {
                Console.WriteLine("Enter the SubId,name,maxMarks,MarksObtained");
                int subId = Convert.ToInt32(Console.ReadLine());
                SqlCommand checkId = new SqlCommand("SELECT COUNT(*) FROM Subject WHERE SubId = '" + subId + "' and StudId = '"+getId+"'", con);
                int StudExist = (int)checkId.ExecuteScalar();
                while (StudExist == 1)
                {
                   Console.WriteLine("Subject id already Exixt Please Enter another ID");
                   subId = Convert.ToInt32(Console.ReadLine());
                   checkId = new SqlCommand("SELECT COUNT(*) FROM Subject WHERE SubId = '" + subId + "' and StudId = '" + getId + "'", con);
                   StudExist = (int)checkId.ExecuteScalar();
                }
                string subName = Console.ReadLine();
                int maxMarks = Convert.ToInt32(Console.ReadLine());
                int marksObtained = Convert.ToInt32(Console.ReadLine());
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = con;
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                cmd2.CommandText = "usp_insertSubject";
                cmd2.Parameters.AddWithValue("@SubId", subId);
                cmd2.Parameters.AddWithValue("@StudId", getId);

                cmd2.Parameters.AddWithValue("@SubName", subName);
                cmd2.Parameters.AddWithValue("@MaxMarks", maxMarks);
                cmd2.Parameters.AddWithValue("@MarksObtained", marksObtained);
                try
                {
                    cmd2.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
        public void UpdateStudent(int studId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                SqlCommand checkId = new SqlCommand("SELECT COUNT(*) FROM Student WHERE StudId = '" + studId + "'", sqlConnection);
                int StudExist = (int)checkId.ExecuteScalar();
                if (StudExist == 1)
                {
                    Console.WriteLine("Student is present");
                    Console.WriteLine("Enter Name, Address, Class");
                    string name = Console.ReadLine();
                    string address = Console.ReadLine();
                    string standard = Console.ReadLine();
                    string updCmd = "UPDATE Student SET Name = '" + name + "',Address = '" + address + "',Class = '" + standard + "' WHERE StudId =" + studId;
                    SqlCommand cmd = new SqlCommand(updCmd, sqlConnection);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("The Student Data Updated Successfully");
                    bool flag = true;
                    while (flag)
                    {
                        Console.WriteLine("Enter the subId to update ");
                        int subId = Convert.ToInt32(Console.ReadLine());
                        SqlCommand checkSubId = new SqlCommand("SELECT COUNT(*) FROM Subject WHERE StudId = '" + studId + "' AND SubId ='" + subId + "'", sqlConnection);
                        int checkSubIdExist = (int)checkSubId.ExecuteScalar();
                        if (checkSubIdExist == 1)
                        {
                            Console.WriteLine("Subject is present");
                            Console.WriteLine("Enter SubName, MaxMarks, MarksObtained");
                            string subName = Console.ReadLine();
                            int maxMarks = Convert.ToInt32(Console.ReadLine());
                            int marksObtained = Convert.ToInt32(Console.ReadLine());
                            string updSubCmd = "UPDATE Subject SET SubName = '" + subName + "',MaxMarks = '" + maxMarks + "',MarksObtained = '" + marksObtained + "' WHERE StudId = '" + studId + "' AND SubId = '" + subId + "'";
                            SqlCommand cmd2 = new SqlCommand(updSubCmd, sqlConnection);
                            cmd2.ExecuteNonQuery();
                            Console.WriteLine("Subject updated successfully");
                            Console.WriteLine("To continue editing subject press 1 or 2 for exit");
                            int option = Convert.ToInt32(Console.ReadLine());
                            if (option == 1)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Subject with given SubId does not exist,Enter the valid one or press 0 to exit");
                            int option = Convert.ToInt32(Console.ReadLine());
                            if (option == 0)
                            {
                                flag = false;
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Student with given StudId cannot be found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DisplayAll()
        {
            SqlConnection sqlConnection = new SqlConnection(connection);
            sqlConnection.Open();
            string cmdDis = "SELECT * FROM DisplayStuDetails ORDER BY SubId";
            SqlCommand cmd = new SqlCommand(cmdDis, sqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader["Name"].ToString();
                string address = reader["Address"].ToString();
                string standard = reader["Class"].ToString();
                string subId = reader["SubId"].ToString();
                string subName = reader["SubName"].ToString();
                string maxMarks = reader["MaxMarks"].ToString();
                string marksObtained = reader["MarksObtained"].ToString();
                Console.WriteLine(name + "   " + address + "    " + standard + "     " + subId + "    " + subName
                    + "     " + maxMarks + "      " + marksObtained);
            }
            reader.Close();
            sqlConnection.Close();
        }

        public void DisplayById(int studId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                string cmdDisById = "SELECT * FROM GetStudentById('" + studId + "')";
                SqlCommand cmd = new SqlCommand(cmdDisById, sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader["Name"].ToString();
                    string address = reader["Address"].ToString();
                    string standard = reader["Class"].ToString();
                    string subId = reader["SubId"].ToString();
                    string subName = reader["SubName"].ToString();
                    string maxMarks = reader["MaxMarks"].ToString();
                    string marksObtained = reader["MarksObtained"].ToString();
                    Console.WriteLine(name + "   " + address + "    " + standard + "     " + subId + "    " + subName
                        + "     " + maxMarks + "      " + marksObtained);
                }
                if (reader.HasRows == false)
                {
                    Console.WriteLine("Enter the valid id");
                }
                reader.Close();
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteStudent(int studId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                string deleteCmd = "DELETE Student WHERE StudId = '" + studId + "'";
                SqlCommand cmd = new SqlCommand(deleteCmd, sqlConnection);
                int noOfRecordsEffect = cmd.ExecuteNonQuery();
                if (noOfRecordsEffect == 0)
                {
                    Console.WriteLine("Id is not present, please enter the valid id");
                }
                else
                {
                    Console.WriteLine("Student Data deleted successfully");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Pleakse enter the valid id");
            }
        }
    }
}
