using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Promises;

namespace Spinwheel.Models
{
    public interface ISpinWheelModel
    {
        IPromise<PlayerModel> GetPlayerModel();

        IPromise<PlayerSpinModel> GetPlayerSpinValues();
    }
}