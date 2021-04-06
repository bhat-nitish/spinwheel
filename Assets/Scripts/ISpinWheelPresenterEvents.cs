using System;

namespace Spinwheel.Presenters
{
    public interface ISpinWheelPresenterEvents
    {
        event EventHandler PlayerModelSet;

        event EventHandler PlayerModelUpdated;
    }
}