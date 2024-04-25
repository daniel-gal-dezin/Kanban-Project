using Frontend.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        public int id { get; set; }
        public string description { get; set; }
        public DateTime dueDate { get; set; }
        public DateTime creationDate { get; set; }
        public string assignee { get; set; }
        public string title { get; set; }

        public TaskModel(BackendController controller, int id, string title, string description, DateTime date,DateTime creationDate) : base(controller)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.dueDate = date;
            this.creationDate = creationDate;


        }



    }
}

    