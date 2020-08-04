using HexUN.MonoB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelManager : AMonoSingletonPersistent<LevelManager>
    {
        public LevelControl Current { get; private set; }

        public void RegisterAsCurrent(LevelControl ctrl)
        {
            Current = ctrl;
        }
    }
}