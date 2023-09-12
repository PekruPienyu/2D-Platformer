using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public class PopDown_State : Enemy_State_Behaviour_Base
    {
        private Vector2 moveDir;
        private Vector3 startPos;
        [SerializeField] private float distanceToCover;
        [SerializeField] private float popDownSpeed = 1;
        [SerializeField] private float duration;
        [SerializeField] private Vector2 initialMoveDir;

        private float timer = 0;

        public override void Initialize()
        {
            startPos = transform.position;
            moveDir = initialMoveDir;
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
            if (transform.position.y > startPos.y - distanceToCover)
            {
                transform.root.Translate(popDownSpeed * Time.deltaTime * Vector3.down);
            }
            else
            {
                ExitState();
            }
        }

        public override Vector2 GetMoveDirection()
        {
            return moveDir;
        }

        public override bool IsGrounded()
        {
            return false;
        }
    }
}
