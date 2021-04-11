using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DefaultNamespace;
using EventArgs;
using Spinwheel.Presenters;
using TMPro;
using UnityEngine;
using Zenject;
using Extensions;
using ModestTree;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace Spinwheel.Views
{
    public class SpinWheelView : MonoBehaviour
    {
        private ISpinWheelPresenter _presenter;

        #region Model

        private long _playerBalance;

        private int _playerMultiplier;

        private long _playerInitialWin;

        private bool _shouldSpin;

        private float _normalSpeedMultiplier = 1;

        private float _slowSpeedMultiplier = 0.7f;

        private float _stoppingSpeedMultiplier = 0.2f;

        private float _targetMultiplierAngle = 0;

        private SpinMode _spinMode;

        #endregion

        #region Properties

        public TextMeshProUGUI _playerBalanceField;

        public TextMeshProUGUI _playerMultiplierField;

        public TextMeshProUGUI _playerInitialWinField;

        public Button _spinButton;

        public Button _spinButtonDown;

        public GameObject _spinWheel;

        public GameObject _spinWheelPointer;

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

        private void EnableSpin()
        {
            _shouldSpin = true;
            _spinMode = SpinMode.Normal;
            _targetMultiplierAngle = 0;
            StartCoroutine(Spin());
        }

        private void DisableSpin()
        {
            _shouldSpin = false;
        }

        IEnumerator Spin()
        {
            while (_shouldSpin)
            {
                if (_spinMode == SpinMode.Normal)
                {
                    _spinWheel.transform.Rotate(0, 0, _normalSpeedMultiplier);
                    yield return new WaitForEndOfFrame();
                }
                else if (_spinMode == SpinMode.Slow)
                {
                    float randomTimeInSeconds = Random.Range(1f, 5f);
                    float elapsedTime = 0;
                    while (elapsedTime < randomTimeInSeconds)
                    {
                        _spinWheel.transform.Rotate(0, 0, _slowSpeedMultiplier);
                        elapsedTime += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }

                    elapsedTime = 0;
                    randomTimeInSeconds = Random.Range(1f, 5f);
                    while (elapsedTime < randomTimeInSeconds)
                    {
                        _spinWheel.transform.Rotate(0, 0, _slowSpeedMultiplier / 2);
                        elapsedTime += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }

                    while (Math.Abs(_spinWheel.transform.eulerAngles.z - _targetMultiplierAngle) > 1)
                    {
                        _spinWheel.transform.Rotate(0, 0, _stoppingSpeedMultiplier);
                        yield return new WaitForEndOfFrame();
                    }

                    DisableSpin();
                    SetPlayerMultiplier();
                    _spinMode = SpinMode.None;
                    for (float timer = 0; timer < 3f; timer += Time.deltaTime)
                    {
                        float progress = timer / 3f;
                        _playerInitialWinField.SetText(((long) Mathf.Lerp(0, _playerInitialWin, progress))
                            .ToCurrency());
                        _playerBalanceField.SetText(
                            ((long) Mathf.Lerp(0, _playerBalance, progress)).ToCurrency());
                        yield return null;
                    }

                    EnableSpinButton();
                }
            }
        }

        private void SetPlayer()
        {
            _playerBalance = _presenter.GetPlayerBalance();
            SetPlayerBalance();
            EnableSpinButton();
        }

        private void UpdatePlayerBalance(PlayerBalanceUpdatedEventArgs playerValues)
        {
            UpdateSpinValues();
            _spinMode = SpinMode.Slow;
        }

        private void UpdateSpinValues()
        {
            _playerBalance = _presenter.GetPlayerBalance();
            _playerMultiplier = _presenter.GetPlayerMultiplier();
            _playerInitialWin = _presenter.GetPlayerIniialWin();
            _targetMultiplierAngle = WheelHelper.GetAngleForMultiplier(0, _playerMultiplier);
        }

        private void SetPlayerBalance()
        {
            _playerBalanceField.SetText(_playerBalance.ToCurrency());
        }

        private void SetPlayerMultiplier()
        {
            _playerMultiplierField.SetText(_playerMultiplier.ToString());
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

        private void ResetMultiplier()
        {
            _playerMultiplierField.SetText("??");
        }

        private void ResetInitialWin()
        {
            _playerInitialWinField.SetText("XXXXXXXXXX");
        }

        private void SpinWheel()
        {
            ResetInitialWin();
            ResetMultiplier();
            EnableSpin();
            DisableSpinButton();
            _presenter.SpinWheel();
        }

        #endregion
    }
}