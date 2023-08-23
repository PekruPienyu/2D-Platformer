using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CustomEnemyScript
{
    [RequireComponent(typeof(Enemy_State_PhysicsHandle))]
    public abstract class Enemy_State_Behaviour_Base : MonoBehaviour
    {
        [SerializeField] protected string stateID;
        public event Action stateExitEvent;
        public abstract void Initialize();

        public virtual void ExitState()
        {
            stateExitEvent();
        }

        public string ID()
        {
            return stateID;
        }

        public abstract Vector2 StateFixedUpdate();
        public abstract void StateUpdate();

        public abstract void EnterState(Vector2 moveDir);

        public abstract Vector2 GetMoveDirection();
    }
}
