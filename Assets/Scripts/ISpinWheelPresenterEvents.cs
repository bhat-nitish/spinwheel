using System;
using EventArgs;

namespace Spinwheel.Presenters
{
    public interface ISpinWheelPresenterEvents
    {
        event EventHandler PlayerModelSet;

        event EventHandler<PlayerBalanceUpdatedEventArgs> PlayerBalanceUpdated;
    }
}