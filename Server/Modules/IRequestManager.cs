using Newtonsoft.Json.Linq;

namespace Isometric.Server.Modules
{
    public interface IRequestManager
    {
        bool Execute(JObject request, Connection connection);
    }
}