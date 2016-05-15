using System.Text;

namespace Shine.Responses
{
    public class TemplatedResponse : HttpResponse
    {
        public TemplatedResponse(string templatePath, object context, int status = 200)
            : base(status: status, contenttype: "text/html; charset=utf-8")
        {
            Content = Encoding.UTF8.GetBytes(App.TemplateProcessor.Render(templatePath, context));
        }
    }
}