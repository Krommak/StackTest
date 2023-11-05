using System.Collections.Generic;
using UnityEngine;

namespace Game.Ecs.Components
{
    struct PlayerComponent
    {
        internal Rigidbody PlayerRb;
        internal Animator Animator;
    }

    struct Stack
    {
        internal Transform StackTransform;
        internal Stack<Transform> Value;
    }
}