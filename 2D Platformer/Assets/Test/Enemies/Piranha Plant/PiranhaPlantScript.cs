using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public class PiranhaPlantScript : Enemy_BaseClass
    {
        private StateData_Helper data;
        private bool isPopedOut = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            foreach (Enemy_State_Behaviour_Base item in states)
            {
                item.Initialize();
                item.stateExitEvent += ChangeState;
            }
            ChangeStateToRestLong();
        }

        private void Update()
        {
            currentState.StateUpdate();
        }

        public override void OnHit(bool popOut)
        {
            gameObject.SetActive(false);
        }
        protected override void ChangeState()
        {
            currentExitStateID = currentState.ID();

            switch (currentExitStateID)
            {
                case "PopUp":
                    ChangeStateToRestShort();
                    break;
                case "Rest":
                    if(isPopedOut)
                    {
                        ChangeStateToPopDown();
                    }
                    else ChangeStateToPopUp();
                    break;
                case "PopDown":
                    ChangeStateToRestLong();
                    break;
            }
        }

        private void ChangeStateToPopUp()
        {
            isPopedOut = true;
            currentState = states[0];
            states[0].EnterState(data);
        }

        private void ChangeStateToPopDown()
        {
            isPopedOut = false;
            currentState = states[3];
            states[3].EnterState(data);
        }

        public void ChangeStateToRestShort()
        {
            currentState = states[1];
            states[1].EnterState(data);
        }
        public void ChangeStateToRestLong()
        {
            currentState = states[2];
            states[2].EnterState(data);
        }

        protected override void SetMoveDir(Vector2 _moveDir)
        {
            data.moveDir = _moveDir;
        }
    }
}
