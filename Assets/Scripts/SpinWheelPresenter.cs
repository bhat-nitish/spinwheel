using System;
using System.Collections;
using System.Collections.Generic;
using EventArgs;
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

        public event EventHandler<PlayerBalanceUpdatedEventArgs> PlayerBalanceUpdated;

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

        private void GetSpinWheelValues()
        {
            var spinWheelPromise = _model.GetPlayerSpinValues();
            spinWheelPromise.Then((result) =>
            {
                _playerSpin = new PlayerSpinModel()
                {
                    Multiplier = result.Multiplier,
                    InitialWin = result.InitialWin
                };
                UpdatePlayerBalance();
            });
        }

        private void UpdatePlayerBalance()
        {
            long balance = _player.Balance + (_playerSpin.Multiplier * _playerSpin.InitialWin);
            _model.SetPlayerBalance(balance).Then(() =>
            {
                _player.Balance = balance;
                RaisePlayerBalanceUpdated();
            });
        }

        #endregion

        #region Event Invocation

        private void RaisePlayerModelSet()
        {
            PlayerModelSet.Trigger(this);
        }

        private void RaisePlayerBalanceUpdated()
        {
            PlayerBalanceUpdated.TriggerWithData(this, new PlayerBalanceUpdatedEventArgs()
            {
                Multipler = _playerSpin.Multiplier,
                InitialWin = _playerSpin.InitialWin
            });
        }

        #endregion

        #region GamePlay

        public void SpinWheel()
        {
            GetSpinWheelValues();
        }

        #endregion
    }
}