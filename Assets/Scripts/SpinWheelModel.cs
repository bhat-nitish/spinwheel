using System.Collections.Generic;
using System.Threading.Tasks;
using Promises;
using Spinwheel.Services;
using UnityEngine;
using Promise = Promises.Promise;

namespace Spinwheel.Models
{
    public class SpinWheelModel : ISpinWheelModel
    {
        private readonly List<int> _validMultipliers = new List<int>() {2, 3, 4, 5, 6, 8, 10};
        private readonly int _defaultMultiplier = 2;

        private ISpinWheelService _service;

        public PlayerModel _player { get; }

        public SpinWheelModel(ISpinWheelService spinWheelService)
        {
            _service = spinWheelService;
            _player = new PlayerModel();
        }

        public IPromise<PlayerModel> GetPlayerModel()
        {
            Promise<PlayerModel> playerModelPromise = new Promise<PlayerModel>();
            PlayerModel model = new PlayerModel();
            _service.GetPlayerBalance()
                .Then((balance) => { model.Balance = balance; })
                .Catch((error) => { Debug.Log(error.Message); })
                .Done(() => { playerModelPromise.Resolve(model); });
            return playerModelPromise;
        }

        public IPromise<PlayerSpinModel> GetPlayerSpinValues()
        {
            // This should ideally be in Promise.All. There is an issue with Promise.ALl currently and this this is being used
            Promise<PlayerSpinModel> playerSpinPromise = new Promise<PlayerSpinModel>();
            PlayerSpinModel model = new PlayerSpinModel();
            _service.GetInitialWin()
                .Then((initialWin) => { model.InitialWin = initialWin; })
                .Catch((error) => { Debug.Log(error.Message); })
                .Done(() =>
                {
                    _service.GetPlayerMultiplier()
                        .Then((multiplier) =>
                        {
                            model.Multiplier = CheckForValidMultiplierAndHandleInvalid(multiplier);
                        })
                        .Done(() => { playerSpinPromise.Resolve(model); });
                });
            return playerSpinPromise;
        }


        /// <summary>
        /// Checks if the multiplier returned from the service is acceptable, else sets multiplier to a default value
        /// Acceptable Multipliers are 2,3,4,5,6,8,10
        /// </summary>
        /// <param name="multiplier">Multiplier received from the server</param>
        /// <returns>Multiplier returned from the server if valid, Default Multiplier otherwise</returns>
        private int CheckForValidMultiplierAndHandleInvalid(int multiplier)
        {
            bool isValidMultiplier = _validMultipliers.Contains(multiplier);
            if (isValidMultiplier)
            {
                return multiplier;
            }
            else
            {
                Debug.Log(
                    $"Multiplier received from server = {multiplier} and multipler being returned = {_defaultMultiplier}");
                return _defaultMultiplier;
            }
        }

        public IPromise SetPlayerBalance(long balance)
        {
            Promises.Promise playerBalancePromise = new Promises.Promise();
            _service.UpdatePlayerBalance(balance)
                .Then(() => { })
                .Catch((error) => { Debug.Log(error.Message); })
                .Done(() => { playerBalancePromise.Resolve(); });
            return playerBalancePromise;
        }
    }
}