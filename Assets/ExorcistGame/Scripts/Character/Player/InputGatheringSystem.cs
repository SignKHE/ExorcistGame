using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ExorcistGame.Character.Player
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class InputGatheringSystem : SystemBase
    {
        private PlayerActions _playerActions;

        protected override void OnCreate()
        {
            _playerActions = new PlayerActions();
            _playerActions.Enable();
        }

        protected override void OnDestroy()
        {
            _playerActions.Disable();
        }

        protected override void OnUpdate()
        {
            Vector2 moveDirection = _playerActions.Character.Move.ReadValue<Vector2>();
            float2 moveInput = new float2(moveDirection.x, moveDirection.y);

            foreach (var inputData in SystemAPI.Query<RefRW<MovementData>>().WithAll<PlayerTag>())
            {
                inputData.ValueRW.MoveDirection =  moveInput;
            }
        }
    }
}
