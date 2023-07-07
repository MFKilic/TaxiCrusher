using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TemplateFx.Enemy
{
    public class EnemyRagdoll : MonoBehaviour
    {
        [SerializeField] private Collider[] _ragdollColliders;
        [SerializeField] private Rigidbody[] _ragdollRigidbodies;
        [SerializeField] Animator _ragdollAnimator;
        [SerializeField] EnemyController _enemyController;

        // Start is called before the first frame update

       

        void Start()
        {
         
        }

        public void OpenRagdoll()
        {
            if(_enemyController != null)
            {
                _enemyController.OnEnemyDie();
            }

            _ragdollAnimator.enabled = false;

            foreach (Collider collider in _ragdollColliders)
            {
                collider.enabled = true;
            }

            foreach (Rigidbody rb in _ragdollRigidbodies)
            {
                rb.isKinematic = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}

