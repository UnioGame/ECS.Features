﻿using Game.Code.GameLayers.Category;
using Game.Code.GameLayers.Layer;
using Game.Code.GameLayers.Relationship;
using Leopotam.EcsLite;

namespace Game.Ecs.TargetSelection.Components
{
    using System;

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
    public struct SqrRangeTargetsSelectionRequestComponent
    {
        public int ResultHash;
        public float Radius;
        public CategoryId Category;
        public RelationshipId Relationship;
        public LayerId SourceLayer;
    }
}