using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public abstract class Enemy_BaseClass : MonoBehaviour, IDamageable
    {
        protected Rigidbody2D rb;
        [SerializeField] protected BoxCollider2D boxCol;
        [SerializeField] protected Animator anim;
        protected Vector2 prevMoveDir = Vector2.zero;

        protected Enemy_State_Behaviour_Base currentState;
        protected string currentExitStateID;

        [SerializeField] protected List<Enemy_State_Behaviour_Base> states = new();

        private void OnDestroy()
        {
            foreach (Enemy_State_Behaviour_Base item in states)
            {
                item.stateExitEvent -= ChangeState;
            }
        }

        protected abstract void SetMoveDir(Vector2 _moveDir);
        
        protected abstract void ChangeState();

        public abstract void OnHit(bool popOut);

    }
}
