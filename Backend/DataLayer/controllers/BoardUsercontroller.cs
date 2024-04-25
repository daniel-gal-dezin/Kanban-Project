using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IntroSE.Kanban.Backend.DataLayer;

public class BoardUsercontroller:DalController
{
    private const String TableName = "boardusertable";
    private const String Email = "email";
    private const string BoardId = "boardid";
    private const string Owner = "owner";
    
    
    
    
    
    internal BoardUsercontroller() : base(TableName)
    {
    }

    internal bool insertBoardUser(BoardUserDTO a)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"INSERT INTO {TableName}({Email},{BoardId},{Owner})" + $"VALUES (@mail,@boardid,@owner);";
                
                
                SQLiteParameter mailpar = new SQLiteParameter(@"mail", a.Email1);
               
                // ReSharper disable once HeapView.BoxingAllocation
                SQLiteParameter boardidpar = new SQLiteParameter(@"boardid",a.Boardid1);
                // ReSharper disable once HeapView.BoxingAllocation
                SQLiteParameter ownerpar = new SQLiteParameter(@"owner", a.Owner1);
               
                
                command.Parameters.Add(mailpar);
                command.Parameters.Add(boardidpar);
                command.Parameters.Add(ownerpar);
                
                
                
                
                command.Prepare();

                res = command.ExecuteNonQuery();
                log.Info("add new board and user to the table");
            }
            catch (Exception e)
            {
                log.Error("didnt finish the insert");
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
    
    
    
    
    
    
    internal bool updateOwner(String email,int owner)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"update {TableName} set [{Owner}]= @owner where {Email} = @email";


                SQLiteParameter ownerpar = new SQLiteParameter(@"owner", (Object)owner);
                SQLiteParameter emailpar = new SQLiteParameter(@"email", email);


                command.Parameters.Add(ownerpar);
                command.Parameters.Add(emailpar);
                
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
    
    
    
    
    
    
    
    internal bool deleteBoardUser(String email, int boardId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText = $"DELETE FROM {TableName} WHERE {Email}= @email and {BoardId} = @bId";
                SQLiteParameter emailpar = new SQLiteParameter(@"email", email);
                SQLiteParameter bidPar = new SQLiteParameter(@"bId", boardId);

                command.Parameters.Add(emailpar);
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

    internal List<BoardUserDTO> LoadBoard2User()
    {
        List<BoardUserDTO> B2user = LoadData().Cast<BoardUserDTO>().ToList();
        return B2user;
    }
    
    internal override DalDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        BoardUserDTO result = new BoardUserDTO(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
        return result;
    }
    
}