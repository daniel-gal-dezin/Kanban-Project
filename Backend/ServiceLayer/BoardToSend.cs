namespace IntroSE.Kanban.Backend.ServiceLayer;

public class BoardToSend
{
    private string BoardName;
    private string email;


    /*
     * This class is for returning a board in a minimized json.
     */
    public BoardToSend(string boardName, string email)
    {
        BoardName = boardName;
        this.email = email;
    }

    public string BoardName1
    {
        get => BoardName;
        set => BoardName = value;
    }

    public string Email
    {
        get => email;
        set => email = value;
    }
}