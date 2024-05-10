﻿namespace Game.Ecs.AI.Components
{
    using System;
    using System.Collections.Generic;
    using Leopotam.EcsLite;

    /// <summary>
    /// ADD DESCRIPTION HERE
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct PrioritizedTargetComponent
    {
        public Dictionary<int, EcsPackedEntity> Value;
    }
}