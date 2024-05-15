namespace Game.Ecs.GameAi.Move.Converters
{
    using System;
    using System.Collections.Generic;
    using AI.Abstract;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UniGame.LeoEcs.Converter.Runtime.Abstract;
    using UnityEngine;
    using Components;

    [Serializable]
    public class MoveToTargetPlannerConverter : PlannerConverter<MovePlannerComponent>, 
        ILeoEcsGizmosDrawer
    {
        [SerializeReference]
        [InlineProperty]
        public List<IMoveByConverter> converters = new List<IMoveByConverter>();

        protected override void OnApplyComponents(GameObject target, EcsWorld world, int entity)
        {
            foreach (var converter in converters)
            {
                converter.Apply(world, entity, actionId);
            }
        }

        public void DrawGizmos(GameObject target)
        {
            foreach (var converter in converters)
            {
                if (converter is not ILeoEcsGizmosDrawer drawer)
                {
                    continue;
                }

                drawer.DrawGizmos(target);
            }
        }
    }
}