using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}
