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
                        .Then((multiplier) => { model.Multiplier = multiplier; })
                        .Done(() => { playerSpinPromise.Resolve(model); });
                });
            return playerSpinPromise;
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