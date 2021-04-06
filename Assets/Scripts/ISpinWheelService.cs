using Promises;

namespace Spinwheel.Services
{
    public interface ISpinWheelService
    {
        Promise<long> GetPlayerBalance();

        Promise<int> GetPlayerMultiplier();

        Promise<int> GetInitialWin();

        IPromise UpdatePlayerBalance(long playerBalance);
    }
}