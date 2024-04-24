using Microsoft.AspNetCore.Mvc;
using IoFile = System.IO.File;

namespace QuotesView2.Controllers;

[ApiController]
[Route("")]
public class DefaultController : ControllerBase
{
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return Content(HtmlContent, "text/html");
    }
    
    private static string HtmlContent => GetContent();

    private static string GetContent()
    {
        var html = IoFile.ReadAllText("./public/index.html");
        var quotes = IoFile.ReadAllText("./public/quotes.js");
        var startup = IoFile.ReadAllText("./public/startup.js");

        return html.Replace("<script src=\"quotes.js\"></script>", $"<script>\n{quotes}\n</script>")
            .Replace("<script src=\"startup.js\"></script>", $"<script>\n{startup}\n</script");
    }
}

