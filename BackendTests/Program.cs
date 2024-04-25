// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;
using BackendTests;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Reflection;
using IntroSE.Kanban.Backend.DataLayer;
using log4net;
using log4net.Config;
using log4net.Filter;

class Program
{
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);//This goes at each class

    static void Main(string[] args)
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        Log.Info("Starting log!");
        Log.Info("Program was started");




        Kanban_Init kanban = new Kanban_Init();
        
        RegistartionTests regTests = new RegistartionTests(kanban.Us);
        LoginTests loginTests = new LoginTests(kanban.Us);
        CreateBoardsTests createboardtest = new CreateBoardsTests(kanban.Bs);
        AddTaskTests addtask = new AddTaskTests(kanban.Ts);
        ForwardTaskTests ftasks = new ForwardTaskTests(kanban.Bs);
        GetAllInProgTaskTests getAllInProg = new GetAllInProgTaskTests(kanban.Us);
        DeleteBoardsTests deleteBoardsTests = new DeleteBoardsTests(kanban.Bs);
        GradingService gs = new GradingService(kanban);
        Add_Remove_member addRemoveMember = new Add_Remove_member(kanban.Bs);
        OwnershipTests ownerTests = new OwnershipTests(kanban.Bs);
        AssignTasksTests assignTasksTests = new AssignTasksTests(kanban.Ts);

       // gs.Register("mail1@mail.com", "Password1");
        gs.LoadData();
        gs.Login("mail@mail.com", "Password1");
        
       // gs.LoadData();





    }
}