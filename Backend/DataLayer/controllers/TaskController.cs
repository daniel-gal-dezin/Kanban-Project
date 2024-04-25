using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
namespace IntroSE.Kanban.Backend.DataLayer;

using IntroSE.Kanban.Backend.BuisnessLayer;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

public class TaskController:DalController
{
    private const string tablename = "taskstable";
    private const string idCol = "id";
    private const string useremailCol = "useremail";
    private const string titleCol = "title";
    private const string descCol = "description";
    private const string dateCol = "creationtime";
    private const string duedateCol = "duedate";
    private const string columCol = "colum";
    private const string boardidCol = "boardid";

    internal TaskController():base("taskstable")
    {
    }
internal bool Insert(TaskDTO t)
{

    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        SQLiteCommand command = new SQLiteCommand(null, connection);
        int res = -1;
        try
        {
            connection.Open();
            command.CommandText = $"INSERT INTO  {tablename}  ({idCol},{useremailCol},{titleCol},{descCol},{dateCol},{duedateCol},{columCol},{boardidCol}) " +
                                  $"VALUES (@id,@useremail,@title,@description,@creationtime,@duedate,@colum,@boardid);";



            SQLiteParameter taskidpar = new SQLiteParameter(@"id", t.Id);
            SQLiteParameter useremailpar = new SQLiteParameter(@"useremail", t.AssignEmail);
            SQLiteParameter titlepar = new SQLiteParameter(@"title", t.Title);
            SQLiteParameter descriptionpar = new SQLiteParameter(@"description", t.Description);
            SQLiteParameter creationtimepar = new SQLiteParameter(@"creationtime", t.CreationTime);
            SQLiteParameter duedatepar = new SQLiteParameter(@"duedate", t.DueDate);
            SQLiteParameter columpar = new SQLiteParameter(@"colum", t.Colum);
            SQLiteParameter boardidpar = new SQLiteParameter(@"boardid", t.Boardid);


            command.Parameters.Add(taskidpar);
            command.Parameters.Add(useremailpar);
            command.Parameters.Add(titlepar);
            command.Parameters.Add(descriptionpar);
            command.Parameters.Add(creationtimepar);
            command.Parameters.Add(duedatepar);
            command.Parameters.Add(columpar);
            command.Parameters.Add(boardidpar);
            command.Prepare();

            res = command.ExecuteNonQuery();
            log.Info("add new task to the table");
        }
        catch (Exception e)
        {
            log.Error("failed in inserting task to db");
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
internal bool updateDescription(string newDesc, int id)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"update {tableName} set [{descCol}] = @description where {idCol} = @id";
                SQLiteParameter decPar = new SQLiteParameter(@"description", newDesc);
                SQLiteParameter idPar = new SQLiteParameter(@"id", id);
                command.Parameters.Add(decPar);
                command.Parameters.Add(idPar);
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("update task description");
            }
            catch (Exception e)
            {
                log.Error("not finish the update");
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
    internal bool updateTitle(string newTitle, int id)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"update {tableName} set [{titleCol}] = @title where {idCol} = @id";
                SQLiteParameter titlePar = new SQLiteParameter(@"title", newTitle);
                SQLiteParameter idPar = new SQLiteParameter(@"id", id);
                command.Parameters.Add(titlePar);
                command.Parameters.Add(idPar);
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("update task title");
            }
            catch (Exception e)
            {
                log.Error("not finish the update");
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
    internal bool updateDuedate(string newDuedate, int id)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"update {tableName} set [{duedateCol}] = @duedate where {idCol} = @id";
                SQLiteParameter DuedatePar = new SQLiteParameter(@"duedate", newDuedate);
                SQLiteParameter idPar = new SQLiteParameter(@"id", id);
                command.Parameters.Add(DuedatePar);
                command.Parameters.Add(idPar);
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("update task duedate");
            }
            catch (Exception e)
            {
                log.Error("not finish the update");
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
    internal bool updatecolum(int newcolum, int id)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"update {tableName} set [{columCol}] = @colum where {idCol} = @id";
                SQLiteParameter columPar = new SQLiteParameter(@"colum", newcolum);
                SQLiteParameter idPar = new SQLiteParameter(@"id", id);
                command.Parameters.Add(columPar);
                command.Parameters.Add(idPar);
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("update task colum");
            }
            catch (Exception e)
            {
                log.Error("not finish the update");
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
    internal bool updateAssign(string newuseremail, int id)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"update {tableName} set [{useremailCol}] = @usermail where {idCol} = @id";
                SQLiteParameter useremailPar = new SQLiteParameter(@"usermail", newuseremail);
                SQLiteParameter idPar = new SQLiteParameter(@"id", id);
                command.Parameters.Add(useremailPar);
                command.Parameters.Add(idPar);
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("update task assign");
            }
            catch (Exception e)
            {
                log.Error("not finish the update");
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
    internal List<TaskDTO> LoadTasks()
    {
        List<TaskDTO> tasks = LoadData().Cast<TaskDTO>().ToList();
        return tasks;
    }
    internal override DalDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        TaskDTO result;
        if(reader.IsDBNull(1))
           result = new TaskDTO(reader.GetInt32(0), null,
                reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6),reader.GetInt32(7));
        else
        {
            result = new TaskDTO(reader.GetInt32(0), reader.GetString(1),
                reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6),reader.GetInt32(7));
        }

        return result;
    }
    
}

