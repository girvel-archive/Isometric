using Newtonsoft.Json.Linq;

namespace Isometric.Server.Modules.RequestManaging
{
    public interface IRequestManager
    {
        bool Execute(JObject request, Connection connection);
    }
}