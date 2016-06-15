namespace Shine.Errors
{
    public class Http404Exception : HttpErrorException
    {
        public Http404Exception(string message = "") : base(404, message)
        {
        }
    }
}