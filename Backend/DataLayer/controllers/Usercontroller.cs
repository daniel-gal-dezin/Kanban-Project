using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace IntroSE.Kanban.Backend.DataLayer;

using System.Data.SQLite;

public class UserController:DalController
{
    
    private const String tablename = "userstable";
    private const string EmailName = "email";
    private const string passwordName = "password";
   
    


    internal UserController() : base(tablename)
    {
    }
    
    internal bool Insert(UserDTO u)
    {
        
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"INSERT INTO  {tablename}  ({EmailName},{passwordName}) " +
                                      $"VALUES (@mail,@paswword);";
                
                
                
                SQLiteParameter mailpar = new SQLiteParameter(@"mail", u.Email);
                SQLiteParameter paswwordpar = new SQLiteParameter(@"paswword", u.Password);

                command.Parameters.Add(mailpar);
                command.Parameters.Add(paswwordpar);
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("add new user to the table");
            }
            catch (Exception e)
            {
                log.Error("failed in inserting user to db");
                throw new Exception(e.Message);
            }
            finally
            {
                command.Dispose();
                connection.Close(); 
            }
            


            return res > 0;
        }
    }

    internal List<UserDTO> LoadDatausers()
    {
        List<UserDTO> users = LoadData().Cast<UserDTO>().ToList();
        return users;
    }

  


    internal override DalDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        UserDTO result = new UserDTO(reader.GetString(0), reader.GetString(1));
        return result;
    }
}