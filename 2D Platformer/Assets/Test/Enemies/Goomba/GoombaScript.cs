using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public class GoombaScript : Enemy_BaseClass
    {
        private StateData_Helper data;
        private bool isActive;
        private void Awake()
        {
            isActive = true;
            rb = GetComponent<Rigidbody2D>();
            foreach (Enemy_State_Behaviour_Base item in states)
            {
                item.Initialize();
                item.stateExitEvent += ChangeState;
            }
            ChangeStateToWalk();
        }

        private void FixedUpdate()
        {
            if (currentState.ID() == "Rest")
            { 
                if(isActive)
                {
                    rb.velocity = currentState.StateFixedUpdate();
                    isActive = false;
                }
            }
            else
            {
                rb.velocity = currentState.StateFixedUpdate();
            }
        }

        private void Update()
        {
            currentState.StateUpdate();
        }

        public override void OnHit(bool popOut)
        {
            if(popOut)
            {
                ChangeStateToDeathPopOut();
            }
            else ChangeStateToDeath();

            int score = 100 * Player.instance.GetKillComboPointMultiplier();
            Player.instance.ActivateKillComboTimer();
            Player.instance.AddToScore(score);
            FloatingScorePool.instance.GetFromPool(transform.position, score);
        }

        protected override void ChangeState()
        {
            currentExitStateID = currentState.ID();
            if(currentState.GetMoveDirection() != Vector2.zero) SetMoveDir(currentState.GetMoveDirection());

            switch (currentExitStateID)
            {
                case "Walk":
                    ChangeStateToRest();
                    break;
                case "Dash":
                    ChangeStateToWalk();
                    break;
                case "DeathPopOut":
                    ChangeStateToRest();
                    break;
                case "Death":
                    DeactivateSelf();
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DeathLine"))
            {
                gameObject.SetActive(false);
            }
        }

        private void ChangeStateToDeath()
        {
            anim.Play("Death");
            currentState = states[3];
            states[3].EnterState(data);
            boxCol.gameObject.SetActive(false);
        }
        private void ChangeStateToDeathPopOut()
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            currentState = states[2];
            states[2].EnterState(data);
            boxCol.gameObject.SetActive(false);
            anim.enabled = false;
            Invoke("DeactivateSelf", 0.5f);
        }

        private void ChangeStateToWalk()
        {
            currentState = states[0];
            states[0].EnterState(data);
        }

        public void ChangeStateToRest()
        {
            currentState = states[1];
            states[1].EnterState(data);
        }

        protected override void SetMoveDir(Vector2 _moveDir)
        {
            data.moveDir = _moveDir;
        }

        private void DeactivateSelf()
        {
            gameObject.SetActive(false);
        }
    }
}
