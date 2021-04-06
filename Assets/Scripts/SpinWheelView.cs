﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using EventArgs;
using Spinwheel.Presenters;
using TMPro;
using UnityEngine;
using Zenject;
using Extensions;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

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

        public Button _spinButton;

        public Button _spinButtonDown;

        #endregion

        #region Event Handler Subscription

        private void OnPlayerModelSet(object sender, System.EventArgs e) => SetPlayer();

        private void OnPlayerBalanceUpdated(object sender, PlayerBalanceUpdatedEventArgs e) => UpdatePlayerBalance(e);

        #endregion

        [Inject]
        private void Init(ISpinWheelPresenter spinWheelPresenter)
        {
            _presenter = spinWheelPresenter;
        }

        private void SubscribeToPresenterEvents()
        {
            _presenter.PlayerModelSet += OnPlayerModelSet;
            _presenter.PlayerBalanceUpdated += OnPlayerBalanceUpdated;
        }

        private void UnSubscribeToPresenterEvents()
        {
            _presenter.PlayerModelSet -= OnPlayerModelSet;
            _presenter.PlayerBalanceUpdated -= OnPlayerBalanceUpdated;
        }

        private void Awake()
        {
            SubscribeToPresenterEvents();
            RegisterButtonClicks();
            DisableSpinButton();
        }

        private void RegisterButtonClicks()
        {
            _spinButton.onClick.AddListener(SpinWheel);
        }

        private void OnDestroy()
        {
            UnSubscribeToPresenterEvents();
        }

        #region GamePlay

        private void SetPlayer()
        {
            _playerBalance = _presenter.GetPlayerBalance();
            SetPlayerBalance();
            EnableSpinButton();
        }

        private void UpdatePlayerBalance(PlayerBalanceUpdatedEventArgs playerValues)
        {
            UpdateSpinValues();
            SetPlayerBalance();
            SetPlayerInitialWin();
            SetPlayerMultiplier();
            EnableSpinButton();
        }

        private void UpdateSpinValues()
        {
            _playerBalance = _presenter.GetPlayerBalance();
            _playerMultiplier = _presenter.GetPlayerMultiplier();
            _playerInitialWin = _presenter.GetPlayerIniialWin();
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

        private void DisableSpinButton()
        {
            _spinButton.gameObject.SetActive(false);
            _spinButtonDown.gameObject.SetActive(true);
        }

        private void EnableSpinButton()
        {
            _spinButton.gameObject.SetActive(true);
            _spinButtonDown.gameObject.SetActive(false);
        }

        private void SpinWheel()
        {
            DisableSpinButton();
            _presenter.SpinWheel();
        }

        #endregion
    }
}