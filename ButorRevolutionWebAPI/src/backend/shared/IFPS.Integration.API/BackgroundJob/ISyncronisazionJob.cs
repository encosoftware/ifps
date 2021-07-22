using System.Threading.Tasks;

namespace IFPS.Integration.API.BackgroundJob
{
    public interface ISynchronizationJob
    {
        Task Syncronise();
    }
}