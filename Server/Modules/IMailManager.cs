namespace Isometric.Server.Modules
{
    public interface IMailManager
    {
        void Send(string to, string subject, string message);

        void Connect(string host, int port, bool enableSsl, string email, string password);
    }
}