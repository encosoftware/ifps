using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Interfaces
{
    public interface ISchedulingOptimizationService
    {
        string RunLayoutPlanning(string inputFilePath, string layoutDir, string strategyType);
        string RunJobScheduling(string inputFilePath, string scheduleDir, string cuttingsJsonFilePath, int ShiftNumber, int ShiftLength, string scheduleMode);
    }
}
