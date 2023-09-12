using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CustomEnemyScript
{
    public class Enemy_State_PhysicsHandle : MonoBehaviour
    {
        private Vector2 moveDir;
        private bool groundCheckEnable;
        private bool gravityEnable;
        private bool chechAheadEnable;

        public void Initialize(Vector2 _moveDir,bool groundCheck, bool gravityCheck, bool checkAhead, bool _friendlyFire)
        {
            moveDir = _moveDir;
            groundCheckEnable = groundCheck;
            gravityEnable = gravityCheck;
            chechAheadEnable = checkAhead;
            friendlyFire = _friendlyFire;
            localScaleReference = transform.root.localScale;
        }

        public Vector2 ApplyPhysicsAndGetMoveDirection(Vector2 _moveDir)
        {
            moveDir = _moveDir;
            GroundCheck();
            ApplyGravity();
            CheckAhead();
            CheckForDamageableEntities();
            return moveDir;
        }

        public bool isGrounded;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private BoxCollider2D boxCol;
        private Vector2 boxCastOrigin;
        private void GroundCheck()
        {
            if(!groundCheckEnable)
            {
                isGrounded = false;
                return;
            }
            boxCastOrigin = new Vector2(boxCol.bounds.center.x, boxCol.bounds.center.y - boxCol.bounds.extents.y);
            RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, new Vector2(boxCol.size.x, 0.07f), 0, Vector2.zero, 0, groundLayer);
            if (hit.collider != null)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        [SerializeField] private float gravityForce = 0.5f;
        [SerializeField] private float maxFallSpeed = 5f;
        private void ApplyGravity()
        {
            if(!gravityEnable)
            {
                return;
            }
            if (!isGrounded)
            {
                moveDir.y -= gravityForce;

                if (moveDir.y <= -maxFallSpeed)
                {
                    moveDir.y = -maxFallSpeed;
                }
            }
            else
            {
                moveDir.y = 0;
            }
        }

        private Vector2 rayDir;
        private Vector2 localScaleReference;
        [SerializeField] private LayerMask rayAheadLayer;
        private bool friendlyFire = false;
        private void CheckAhead()
        {
            if(!chechAheadEnable)
            {
                return;
            }
            if (moveDir.x != 0) rayDir.x = Mathf.Abs(moveDir.x) / moveDir.x;
            RaycastHit2D[] rays = Physics2D.RaycastAll(boxCol.bounds.center, rayDir, boxCol.bounds.extents.x + 0.02f, rayAheadLayer);
            Debug.DrawRay(boxCol.bounds.center, new Vector3(rayDir.x * (boxCol.bounds.extents.x + 0.02f), 0, 0));

            for (int i = 0; i < rays.Length; i++)
            {
                if (friendlyFire)
                {
                    if (i == 0 || rays[i].collider.CompareTag("Enemy")) continue;
                    else moveDir.x *= -1;
                    FlipSprite(moveDir);
                }
                else
                {
                    if (i == 0) continue;
                    else moveDir.x *= -1;
                    FlipSprite(moveDir);
                }
            }
        }

        public void FlipSprite(Vector2 _moveDir)
        {
            if (_moveDir.x > 0)
            {
                transform.root.localScale = new Vector2(-localScaleReference.x, localScaleReference.y);
            }
            else if (_moveDir.x < 0)
            {
                transform.root.localScale = localScaleReference;
            }
        }

        [SerializeField] private LayerMask damageableLayer;
        private Vector2 attackBox;

        public void CheckForDamageableEntities()
        {
            attackBox = new Vector2(boxCol.size.x + 0.1f, boxCol.size.y * 0.5f);
            RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCol.bounds.center, attackBox, 0, Vector2.zero, 0, damageableLayer);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Player"))
                {
                    hits[i].collider.GetComponentInParent<IDamageable>().OnHit(true);
                }
                if ( hits[i].collider.CompareTag("Enemy"))
                { 
                    if (hits[i].collider.gameObject.transform.root == transform.root) return;
                    else
                    {
                        hits[i].collider.GetComponentInParent<IDamageable>().OnHit(true);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            attackBox = new Vector2(boxCol.size.x + 0.05f, boxCol.size.y * 0.5f);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(boxCol.bounds.center, attackBox);
        }
    }
}
