using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Helpers
{
  public static class ExceptionHelper
  {
    public static string CreateExceptionMessage(Exception ex)
    {
      StringBuilder errorMessageBuilder = new StringBuilder();
      Stack<Exception> exceptionStack = new Stack<Exception>();
      Exception ex2 = ex;

      do
      {
        exceptionStack.Push(ex2);
        ex2 = ex2.InnerException;
      }
      while (ex2 != null);

      foreach (Exception e in exceptionStack)
      {
        errorMessageBuilder.AppendLine(e.Message);
        errorMessageBuilder.AppendLine(e.StackTrace);
        errorMessageBuilder.AppendLine("******************************");
        errorMessageBuilder.AppendLine().AppendLine();
      }

      return errorMessageBuilder.ToString();
    }
  }
}
