using Promises;
using Server.API;

namespace Spinwheel.Services
{
    public class SpinWheelService : ISpinWheelService
    {
        private GameplayApi _api;

        public SpinWheelService()
        {
            _api = new GameplayApi();
            _api.Initialise();
        }

        public void InitializeApi()
        {
            _api.Initialise();
        }

        public Promise<long> GetPlayerBalance()
        {
            Promise<long> balancePromise = new Promise<long>();
            _api.GetPlayerBalance().Then((balance) => { balancePromise.Resolve(balance); });
            return balancePromise;
        }

        public Promise<int> GetPlayerMultiplier()
        {
            Promise<int> multiplierPromise = new Promise<int>();
            _api.GetMultiplier().Then((multiplier) => { multiplierPromise.Resolve(multiplier); });
            return multiplierPromise;
        }

        public Promise<int> GetInitialWin()
        {
            return _api.GetInitialWin();
        }
    }
}