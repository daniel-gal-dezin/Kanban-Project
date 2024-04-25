using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Transactions;

namespace IntroSE.Kanban.Backend.DataLayer;


public class Boardcontroller:DalController
{
    
    private const string tableName = "boardstable";
    private const string idCol = "boardid";
    private const string nameCol = "name";
    private const string ownCol = "owner";
    private const string backCol = "limitbacklog";
    private const string inprogCol = "limitinprog";
    private const string doneCol = "limitdone";
    private const string dateCol = "createdate";


    
    internal Boardcontroller() : base(tableName)
    {
        
    }
    
    
    internal bool Insert(BoardDTO b)
    {
        
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    command.CommandText = $"INSERT INTO  {tableName}  ({idCol},{nameCol},{ownCol},{backCol},{inprogCol},{doneCol},{dateCol})" +
                                      $"VALUES (@boardid,@name,@owner,@limitbacklog,@limitinprog,@limitdone,@date);";


                    command.Parameters.Add(new SQLiteParameter(@"boardid", b.BoardId));
                    command.Parameters.Add(new SQLiteParameter(@"name", b.BoardName));
                    command.Parameters.Add(new SQLiteParameter(@"owner", b.Owner));
                    command.Parameters.Add(new SQLiteParameter(@"limitbacklog", b.LimitBack));
                    command.Parameters.Add(new SQLiteParameter(@"limitinprog", b.LimitInprog));
                    command.Parameters.Add(new SQLiteParameter(@"limitdone", b.LimitDone));
                    command.Parameters.Add(new SQLiteParameter(@"date", b.CreationDate));
                    command.Prepare();

                    res = command.ExecuteNonQuery();

                    log.Info("add new board to the table");
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                log.Error("failed to insert board to db");
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

    internal bool updateLimit(int boardid, int column, int limit)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                if(column == 0)
                    command.CommandText = $"update {tableName} set [{backCol}] = @lim where {idCol} = @bId";
                else
                {
                    if(column == 1)
                        command.CommandText = $"update {tableName} set [{inprogCol}]= @lim where {idCol} = @bId";
                    else
                        command.CommandText = $"update {tableName} set [{doneCol}]= @lim where {idCol} = @bId";    
                }


                SQLiteParameter limPar = new SQLiteParameter(@"lim", limit);
                SQLiteParameter idPar = new SQLiteParameter(@"bId", boardid);
                command.Parameters.Add(limPar);
                command.Parameters.Add(idPar);
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("update user ownership");
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

    internal bool DeleteBoard(int boardId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"DELETE FROM {tableName} WHERE {idCol} = @bId";
                SQLiteParameter bidPar = new SQLiteParameter(@"bId", boardId);
               
                command.Parameters.Add(bidPar);
                
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("deleted user 2 board");
            }
            catch (Exception e)
            {
                log.Error("not finish the delete");
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
    
    internal List<BoardDTO> LoadBoards()
    {
        List<BoardDTO> boards = LoadData().Cast<BoardDTO>().ToList();
        return boards;
    }
    
    internal override DalDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        BoardDTO result = new BoardDTO(reader.GetInt32(0), reader.GetString(1),
            reader.GetString(2), reader.GetInt32(3),reader.GetInt32(4),reader.GetInt32(5), reader.GetString(6));
        return result;
    }
}