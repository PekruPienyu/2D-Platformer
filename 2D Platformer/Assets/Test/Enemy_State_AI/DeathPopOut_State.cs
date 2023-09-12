using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public class DeathPopOut_State : Enemy_State_Behaviour_Base
    {
        private Enemy_State_PhysicsHandle physicsHandle;
        private Vector2 moveDir;
        [SerializeField] private Vector2 initialMoveDir;
        [SerializeField] private float duration;

        private float timer = 0;

        public override void Initialize()
        {
            moveDir = initialMoveDir;
            physicsHandle = GetComponent<Enemy_State_PhysicsHandle>();
            physicsHandle.Initialize(moveDir, false, true, false, false);
        }

        public override void EnterState(StateData_Helper data)
        {
            timer = 0;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override Vector2 StateFixedUpdate()
        {
            return new Vector2(2, 1);
        }

        public override void StateUpdate()
        {
            if (duration != 0)
            {
                timer += Time.deltaTime;
                if (timer >= duration)
                {
                    ExitState();
                }
            }
        }

        public override Vector2 GetMoveDirection()
        {
            return moveDir;
        }

        public override bool IsGrounded()
        {
            return physicsHandle.isGrounded;
        }
    }
}
