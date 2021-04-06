using System;
using System.Collections;
using System.Collections.Generic;
using Spinwheel.Models;
using Spinwheel.Presenters;
using UnityEngine;

namespace Spinwheel.Presenters
{
    public class SpinWheelPresenter : ISpinWheelPresenter
    {
        private ISpinWheelModel _model;

        private PlayerModel _player;

        private PlayerSpinModel _playerSpin;

        #region Events

        public event EventHandler PlayerModelSet;

        public event EventHandler PlayerModelUpdated;

        #endregion

        #region Helpers

        public long GetPlayerBalance() => _player.Balance;

        public int GetPlayerMultiplier() => _playerSpin.Multiplier;

        public long GetPlayerIniialWin() => _playerSpin.InitialWin;

        #endregion

        public SpinWheelPresenter(ISpinWheelModel spinWheelModel)
        {
            _model = spinWheelModel;
            _playerSpin = new PlayerSpinModel();
            SetPlayer();
        }

        #region Model Functions

        private void SetPlayer()
        {
            var playerPromise = _model.GetPlayerModel();
            playerPromise.Then((result) =>
            {
                _player = new PlayerModel()
                {
                    Balance = result.Balance,
                };
                RaisePlayerModelSet();
            });
        }

        #endregion

        #region Event Invocation

        private void RaisePlayerModelSet()
        {
            PlayerModelSet.Trigger(this);
        }

        private void RaisePlayerModelUpdated()
        {
            PlayerModelUpdated.Trigger(this);
        }

        #endregion
    }
}