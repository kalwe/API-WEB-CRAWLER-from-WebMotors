using System;

public class ExceptionHelp
{
    public void InfoException(Exception e){
        Console.WriteLine("SessionInsertAsync SaveChanges() error - message:\n {0}", e.Message);
        Console.WriteLine("SessionInsertAsync SaveChanges() error - stacktracer:\n {0}", e.StackTrace);
    }
}