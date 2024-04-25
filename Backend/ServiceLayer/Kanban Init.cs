using IntroSE.Kanban.Backend.Backend;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer;

public class Kanban_Init
{
    /* This class is for having 1 initialization for all of the kanban functionalities.
     * All of the services are conected to the same events of facades,
     * therefore changes can be done by each one of them and the others will update as well.
     */
    
    private UserFacade totalUF;
    private BoardFacade totalBF;
    private DataService ds;
    private UserService us;
    private BoardService bs;
    private TaskService ts;
    
    public Kanban_Init()
    {
        totalUF = new UserFacade();
        totalBF = new BoardFacade(totalUF);
        us = new UserService(totalUF);
        bs = new BoardService(totalBF);
        ts = new TaskService(totalBF);
        ds = new DataService(totalBF, totalUF);
    }

    public UserService Us
    {
        get => us;
        set => us = value;
    }

    public BoardService Bs
    {
        get => bs;
        set => bs = value;
    }

    public TaskService Ts
    {
        get => ts;
        set => ts = value;
    }

    public DataService Ds
    {
        get => ds;
        set => ds = value;
    }
}