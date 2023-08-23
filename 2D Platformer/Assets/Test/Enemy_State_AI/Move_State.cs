using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public class Move_State : Enemy_State_Behaviour_Base
    {
        private Enemy_State_PhysicsHandle physicsHandle;
        private Vector2 moveDir;
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private Vector2 initialMoveDir;
        [SerializeField] private float duration;

        public override void Initialize()
        {
            moveDir = initialMoveDir;
            physicsHandle = GetComponent<Enemy_State_PhysicsHandle>();
            physicsHandle.Initialize(moveDir, true, true, true, false);
        }

        public override void EnterState(Vector2 _moveDir)
        {
            if (_moveDir != Vector2.zero)
                moveDir = _moveDir;
            Invoke("ExitState", duration);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override Vector2 StateFixedUpdate()
        {
             moveDir = physicsHandle.ApplyPhysicsAndGetMoveDirection(moveDir);
            return moveSpeed * Time.fixedDeltaTime * 10 * moveDir;
        }

        public override void StateUpdate()
        {

        }
        public override Vector2 GetMoveDirection()
        {
            return moveDir;
        }
    }
}
