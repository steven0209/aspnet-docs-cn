using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MyRazorPage : RazorPage<dynamic>
  {
      public override Task ExecuteAsync()
      {
          var output = "Hello World";
  
          WriteLiteral("<div>Output: ");
          Write(output);
          WriteLiteral("</div>");

        var z = new List<string>();

          return Task.CompletedTask;
      }
  }
