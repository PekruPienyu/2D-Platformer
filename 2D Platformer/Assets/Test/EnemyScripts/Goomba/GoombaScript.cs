using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEnemyScript
{
    public class GoombaScript : Enemy_BaseClass
    {
        private void FixedUpdate()
        {
            rb.velocity = currentState.StateFixedUpdate();
        }

        public override void OnHit(bool popOut)
        {
            Debug.Log("Dead");
        }
    }
}
