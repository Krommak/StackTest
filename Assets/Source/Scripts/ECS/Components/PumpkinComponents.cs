using System;
using UnityEngine;

namespace Game.Ecs.Components
{
    struct PumpkinPoint
    {
    }

    struct NewPumpkin
    {
        internal float SpawnTimer;
    }

    struct MoveToStack
    {
        internal Transform Value;
        internal Action OnApplyAction;
    }

    struct CollectStack
    {
        internal Transform TargetPosition;
    }
}