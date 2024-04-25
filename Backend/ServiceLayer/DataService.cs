using System;
using System.Data.Entity.Migrations.Design;
using System.Text.Json;
using IntroSE.Kanban.Backend.Backend;

namespace IntroSE.Kanban.Backend.ServiceLayer;

public class DataService
{
    private BoardFacade bf;
    private UserFacade uf;

    public DataService(BoardFacade bFac, UserFacade uFac)
    {
        this.bf = bFac;
        this.uf = uFac;
    }

    public string LoadAllData()
    {
        try
        {
            uf.LoadData();
            bf.LoadData();
            return JsonSerializer.Serialize(new Response(null,null));
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - LoadData collapsed"));
        }
        
    }

    public string DeleteAllData()
    {
        
        try
        {
            uf.DeleteData();
            bf.DeleteData();
            return JsonSerializer.Serialize(new Response(null,null));
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - DeleteData collapsed"));
        }
    }
}