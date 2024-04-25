using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using log4net;

namespace IntroSE.Kanban.Backend.DataLayer;

public abstract class DalController
{
    protected String path;
    protected String connectionString;
    protected String tableName;
    protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
    
    
    
    internal DalController(String tableName)
    {   
        this.path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
        connectionString = $"Data source={path}; version=3;";
        this.tableName = tableName;
    }

    internal List<DalDTO> LoadData()
    {
        List<DalDTO> toReturn = new List<DalDTO>();

        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM {tableName}";
                SQLiteDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    toReturn.Add(this.ConvertReaderToObject(dataReader));
                }

            }
            catch (Exception e)
            {
                log.Error("didnt load the data");
                throw new Exception(e.Message);
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }


            log.Info("return list with boardsDTO");
            return toReturn;
        }

    }


    internal Boolean DeleteData()
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {

                connection.Open();
                command.CommandText = $"DELETE FROM {tableName}";


                command.Prepare();
                res = command.ExecuteNonQuery();
                log.Info("delete all data");

            }
            catch (Exception e)
            {
                log.Error("didnt delete the data");
            }
            finally
            {
                command.Dispose();
                connection.Close();

            }

            log.Info("delete all data");

            return res > 0;
        }

    }






   internal abstract DalDTO ConvertReaderToObject(SQLiteDataReader reader);
   
    }

    









