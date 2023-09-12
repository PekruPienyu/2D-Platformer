using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public class TortoiseScript : Enemy_BaseClass
    {
        private StateData_Helper data;
        private bool immuneToDamage;
        private float immuneTimer;

        private void Awake()
        {
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
            rb.velocity = currentState.StateFixedUpdate();
        }

        private void Update()
        {
            if (immuneToDamage)
            {
                immuneTimer += Time.deltaTime;
                if(immuneTimer >= 0.2f)
                {
                    immuneTimer = 0;
                    immuneToDamage = false;
                }
            }
            else
            {
                PlayerScan();
            }
            currentState.StateUpdate();
        }

        [SerializeField] private LayerMask playerLayer;

        private void PlayerScan()
        {
            if(currentState.ID() == "Rest")
            {
                RaycastHit2D hit = Physics2D.BoxCast(boxCol.bounds.center, new(boxCol.size.x + 0.2f, boxCol.size.y * 0.8f), 0, Vector2.zero, 0, playerLayer);
                if(hit.collider != null)
                {
                    OnHit(false);
                }
            }
        }

        public override void OnHit(bool popOut)
        {
            if (immuneToDamage) return;
            currentExitStateID = currentState.ID();
            if (currentState.GetMoveDirection() != Vector2.zero) SetMoveDir(currentState.GetMoveDirection());
            if (popOut)
            {
                ChangeStateToDeathPopOut();
                Invoke("DeactivateSelf", 0.5f);
                Player.instance.AddToScore(100);
                FloatingScorePool.instance.GetFromPool(transform.position, 100);
            }
            else
            {
                switch (currentExitStateID)
                {
                    case "Walk":
                        ChangeStateToRest();
                        immuneToDamage = true;
                        break;
                    case "Rest":
                        if (Player.instance.transform.position.x > transform.root.position.x)
                        {
                            ChangeStateToDash(Vector2.left);
                        }
                        else ChangeStateToDash(Vector2.right);
                        Player.instance.AddToScore(100);
                        FloatingScorePool.instance.GetFromPool(transform.position, 100);
                        immuneToDamage = true;
                        break;
                    case "Dash":
                        ChangeStateToRest();
                        break;
                    case "DeathPopOut":
                        ChangeStateToRest();
                        break;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("DeathLine"))
            {
                gameObject.SetActive(false);
            }
        }

        protected void ChangeStateToWalk()
        {
            currentState = states[0];
            anim.Play("Walk");
            states[0].EnterState(data);
        }

        protected void ChangeStateToRest()
        {
            currentState = states[1];
            anim.Play("Rest");
            states[1].EnterState(data);
        }

        protected void ChangeStateToDash(Vector2 _moveDir)
        {
            data.moveDir = _moveDir;
            currentState = states[2];
            states[2].EnterState(data);
            anim.Play("Rest");
        }

        private void ChangeStateToDeathPopOut()
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            currentState = states[3];
            states[2].EnterState(data);
            boxCol.gameObject.SetActive(false);
            anim.enabled = false;
            Invoke("DeactivateSelf", 0.5f);
        }

        protected override void ChangeState()
        {
            currentExitStateID = currentState.ID();
            if (currentState.GetMoveDirection() != Vector2.zero) SetMoveDir(currentState.GetMoveDirection());

            switch (currentExitStateID)
            {
                case "Rest":
                    ChangeStateToWalk();
                    break;
                case "DeathPopOut":
                    ChangeStateToRest();
                    break;
            }
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
