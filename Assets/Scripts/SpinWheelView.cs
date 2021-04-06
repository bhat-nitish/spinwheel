using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Spinwheel.Presenters;
using TMPro;
using UnityEngine;
using Zenject;
using Extensions;

namespace Spinwheel.Views
{
    public class SpinWheelView : MonoBehaviour
    {
        private ISpinWheelPresenter _presenter;

        #region Model

        private long _playerBalance;

        private int _playerMultiplier;

        private long _playerInitialWin;

        #endregion

        #region Properties

        public TextMeshProUGUI _playerBalanceField;

        public TextMeshProUGUI _playerMultiplierField;

        public TextMeshProUGUI _playerInitialWinField;

        #endregion

        #region Event Handler Subscription

        private void OnPlayerModelSet(object sender, EventArgs e) => SetPlayer();

        #endregion

        [Inject]
        private void Init(ISpinWheelPresenter spinWheelPresenter)
        {
            _presenter = spinWheelPresenter;
        }

        private void SubscribeToPresenterEvents()
        {
            _presenter.PlayerModelSet += OnPlayerModelSet;
        }

        private void UnSubscribeToPresenterEvents()
        {
            _presenter.PlayerModelSet -= OnPlayerModelSet;
        }

        private void Awake()
        {
            SubscribeToPresenterEvents();
        }

        private void OnDestroy()
        {
            UnSubscribeToPresenterEvents();
        }

        private void SetPlayerBalance()
        {
            _playerBalanceField.SetText(_playerBalance.ToCurrency());
        }

        private void SetPlayerMultiplier()
        {
            _playerMultiplierField.SetText(_playerMultiplier.ToString());
        }

        private void SetPlayerInitialWin()
        {
            _playerInitialWinField.SetText(_playerInitialWin.ToCurrency());
        }

        #region GamePlay

        private void SetPlayer()
        {
            _playerBalance = _presenter.GetPlayerBalance();
            SetPlayerBalance();
        }

        #endregion
    }
}