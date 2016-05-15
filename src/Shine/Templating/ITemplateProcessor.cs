namespace Shine.Templating
{
    public interface ITemplateProcessor
    {
        string Render(string templatePath, dynamic context);
    }
}