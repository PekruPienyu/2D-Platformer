using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public class Death_State : Enemy_State_Behaviour_Base
    {
        [SerializeField] private float duration;

        private float timer = 0;

        public override void Initialize()
        {
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
            return Vector2.zero;
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
            return Vector2.zero;
        }

        public override bool IsGrounded()
        {
            return false;
        }
    }
}
