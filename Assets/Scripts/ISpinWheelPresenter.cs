using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spinwheel.Presenters
{
    public interface ISpinWheelPresenter : ISpinWheelPresenterEvents
    {
        long GetPlayerBalance();

        int GetPlayerMultiplier();

        long GetPlayerIniialWin();

        void SpinWheel();
    }
}