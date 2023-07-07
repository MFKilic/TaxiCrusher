using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TemplateFx.Enemy;

namespace TemplateFx.Player
{
    public class CarCollisionController : MonoBehaviour
    {
        private const string strFence = "Fence";
        private const string strHitParticle = "HitParticle";
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out EnemyRagdoll enemyRagdoll))
            {
                enemyRagdoll.OpenRagdoll();
                GameObject hitParticle = PoolManager.Instance.GetPooledObject(strHitParticle);
                hitParticle.transform.position = collision.transform.position;

            }

            if (collision.gameObject.CompareTag(strFence))
            {
                Rigidbody colRb = collision.gameObject.GetComponent<Rigidbody>();
                if(colRb != null )
                {
                    colRb.isKinematic = false;
                }
            }
        }
    }
}

