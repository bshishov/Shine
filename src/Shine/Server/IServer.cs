namespace Shine.Server
{
    public interface IServer
    {
        void Run(IAsyncRequestHandler requestHandler);
        void Stop();
    }
}
