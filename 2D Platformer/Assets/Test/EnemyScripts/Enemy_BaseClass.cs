using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public abstract class Enemy_BaseClass : MonoBehaviour, IDamageable
    {
        protected Rigidbody2D rb;
        [SerializeField] protected BoxCollider2D boxCol;
        protected Animator anim;
        protected Vector2 prevMoveDir;

        protected Enemy_State_Behaviour_Base currentState;
        protected string currentExitStateID;

        [SerializeField] protected List<Enemy_State_Behaviour_Base> states = new();

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            currentState = states[0];
            foreach (Enemy_State_Behaviour_Base item in states)
            {
                item.Initialize();
                item.stateExitEvent += ChangeState;
            }
            ChangeStateToWalk();
        }

        private void OnDestroy()
        {
            foreach (Enemy_State_Behaviour_Base item in states)
            {
                item.stateExitEvent -= ChangeState;
            }
        }

        protected void ChangeStateToWalk()
        {
            currentState = states[0];
            states[0].EnterState(prevMoveDir);
        }
        protected void ChangeStateToRest()
        {
            currentState = states[1];
            states[1].EnterState(prevMoveDir);
        }
        protected void ChangeStateToDash()
        {
            currentState = states[2];
            states[2].EnterState(prevMoveDir);
        }


        protected void ChangeState()
        {
            if (currentState.GetMoveDirection().x != 0)
            {
                prevMoveDir = currentState.GetMoveDirection();
                Debug.Log(prevMoveDir.x);
            }
            switch (currentState.ID())
            {
                case "Walk":
                    ChangeStateToDash();
                    break;
                case "Rest":
                    ChangeStateToWalk();
                    break;
                case "Dash":
                    ChangeStateToRest();
                    break;
            }
        }

        public abstract void OnHit(bool popOut);


    }
}
