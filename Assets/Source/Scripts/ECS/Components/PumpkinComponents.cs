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
    }
}