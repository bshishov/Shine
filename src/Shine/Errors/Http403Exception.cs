namespace Shine.Errors
{
    public class Http403Exception : HttpErrorException
    {
        public Http403Exception(string message = "") : base(403, message)
        {
        }
    }
}