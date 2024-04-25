using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace IntroSE.Kanban.Backend.ServiceLayer;

public class Response
{
    public string ErrorMessage { get; set; }
    public Object ReturnValue { get; set; }

    public Response(Object returnValue, string errorMessage)
    {
        ErrorMessage = errorMessage;
        ReturnValue = returnValue;
    }
}