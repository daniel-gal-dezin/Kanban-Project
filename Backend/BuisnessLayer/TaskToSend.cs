using System;

namespace IntroSE.Kanban.Backend.BuisnessLayer;

public class TaskToSend
{
     /*
      * This class if to return a clean json for the grading service.
      * we copy existing tasks to this class and than return them as jsons.
      */
     
     private int id;
     private DateTime creationTime;
     private string title;
     private string description;
     private DateTime dueDate;

     public TaskToSend(int id, DateTime creationTime, string title, string description, DateTime dueDate)
     {
          this.id = id;
          this.creationTime = creationTime;
          this.title = title;
          this.description = description;
          this.dueDate = dueDate;
     }

     public TaskToSend(Task t)
     {
          id = t.Id;
          creationTime = t.CreationDate;
          title = t.Title;
          description = t.Description;
          dueDate = t.DueDate;
     }

     public int Id
     {
          get => id;
          set => id = value;
     }

     public DateTime CreationTime
     {
          get => creationTime;
          set => creationTime = value;
     }

     public string Title
     {
          get => title;
          set => title = value;
     }

     public string Description
     {
          get => description;
          set => description = value;
     }

     public DateTime DueDate
     {
          get => dueDate;
          set => dueDate = value;
     }
}