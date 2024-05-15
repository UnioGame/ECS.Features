﻿namespace Game.Ecs.Ai.Targeting.Converters
{
    using System;
    using Components;
    using Leopotam.EcsLite;
    using UniGame.LeoEcs.Shared.Extensions;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class SelectByCategoryConverter : ITargetSelectorConverter
    {
        [SerializeField]
        private SelectByCategoryComponent _value;
        
        public void Apply(EcsWorld world, int entity)
        {
            ref var selectComponent = ref world.AddComponent<SelectByCategoryComponent>(entity);
            _value.Apply(ref selectComponent);
        }
    }
}