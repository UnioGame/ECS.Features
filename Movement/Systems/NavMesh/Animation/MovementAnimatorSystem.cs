﻿namespace Movement.Systems.NavMesh.Animation
{
    using System;
    using Game.Ecs.Animation.Aspects;
    using Game.Ecs.Animation.Components;
    using Game.Ecs.Animation.Data;
    using Game.Ecs.Characteristics.Speed.Components;
    using Game.Ecs.Core.Components;
    using Game.Ecs.Movement.Components;
    using Leopotam.EcsLite;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Components;
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class MovementAnimatorSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        
        private EcsPool<SpeedComponent> _speedPool;
        private EcsPool<AnimatorComponent> _animatorPool;
        private EcsPool<TransformComponent> _transformPool;
        private EcsPool<NavMeshAgentComponent> _navMeshPool;
        private EcsPool<MovementAnimationInfoComponent> _animationInfoPool;
        private EcsPool<AnimatorMapComponent> _animatorMapPool;
        private readonly AnimationClipId _idleClipId;
        private readonly AnimationClipId _walkClipId;
        private AnimationsAnimatorAspect _animatorAspect;

        public MovementAnimatorSystem(AnimationClipId idleClipId, AnimationClipId walkClipId)
        {
            _idleClipId = idleClipId;
            _walkClipId = walkClipId;
        }
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world
                .Filter<AnimatorComponent>()
                .Inc<AnimatorMapComponent>()
                .Inc<TransformComponent>()
                .Inc<NavMeshAgentComponent>()
                .Inc<MovementAnimationInfoComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var speedComponent = ref _speedPool.Get(entity);
                var speed = speedComponent.Value;
                
                ref var transform = ref _transformPool.Get(entity);
                ref var navMeshAgent = ref _navMeshPool.Get(entity);
                ref var animationInfo = ref _animationInfoPool.Get(entity);
                ref var animatorComponent = ref _animatorPool.Get(entity);
                
                var animator = animatorComponent.Value;
                var controller = animator.runtimeAnimatorController;
                var agent = navMeshAgent.Value;
                
                if(agent.isOnNavMesh == false) continue;
                
                if (animator == null || !animator.isActiveAndEnabled) continue;
                var velocity = navMeshAgent.Value.velocity.normalized;
                velocity = transform.Value.InverseTransformDirection(velocity);
                
                var agentVelocityMagnitude = agent.velocity.magnitude;
                var speedValue = (velocity.z * speed) / animationInfo.RunSpeed;
                var forwardAmount = agentVelocityMagnitude / speed;
            
                animator.speed = speedValue > animationInfo.MaxRunSpeed
                    ? speedValue / animationInfo.MaxRunSpeed
                    : 1.0f;
                if(controller == null) continue;
                
                ref var animatorMapComponent = ref _animatorAspect.AnimatorMap.Get(entity);
                
                //todo что будет если такие стейтов не существует?
                if (!animatorMapComponent.value.TryGetValue(_idleClipId, out var idleStateName)
                    || !animatorMapComponent.value.TryGetValue(_walkClipId, out var walkStateName))
                {
                    GameLog.LogError("AnimatorMapComponent has no such states");
                    continue;
                }
                
                if (speedValue != 0)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(walkStateName))
                    {
                        animator.Play(walkStateName);
                    }
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(idleStateName))
                    {
                        animator.Play(idleStateName);
                    }
                }
            }
        }
    }
}