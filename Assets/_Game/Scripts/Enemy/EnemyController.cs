using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace TemplateFx.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float speed = 5;
        [SerializeField] private Transform _enemyTransform;
        [SerializeField] private Collider _enemyCollider;
        [SerializeField] private Rigidbody _enemyRigidbody;
        [SerializeField] private Animator _enemyAnimator;
        [SerializeField] private float distance;
        [SerializeField] private float damage = 1;
        bool isChop;
        bool isDied;
        bool isFirstInput;

        void Start()
        {
            _enemyAnimator.speed = 0;
        }

        private void OnEnable()
        {
            GameState.Instance.OnFinishGameEvent += Instance_OnFinishGameEvent;
            LevelManager.Instance.eventManager.OnFirstInputEvent += EventManager_OnFirstInputEvent;
        }

        private void EventManager_OnFirstInputEvent()
        {
            isFirstInput = true;
            _enemyAnimator.speed = 1;
        }

        private void Instance_OnFinishGameEvent(LevelFinishStatus levelFinish)
        {
            _enemyAnimator.speed = 0;
        }

       
        private void OnDisable()
        {
            GameState.Instance.OnFinishGameEvent -= Instance_OnFinishGameEvent;
            LevelManager.Instance.eventManager.OnFirstInputEvent -= EventManager_OnFirstInputEvent;
        }

        public void OnEnemyDie()
        {
            if(isDied)
            {
                return;
            }
            _enemyAnimator.speed = 0;
            isDied = true;
            _enemyCollider.enabled = false;
            _enemyRigidbody.isKinematic = true;
            LevelManager.Instance.eventManager.OnEnemyDied();
        }

        public void OnEnemyHitTree()
        {
            if (isDied || !GameState.Instance.IsPlaying())
            {
                return;
            }
            LevelManager.Instance.eventManager.OnEnemyHitTree(damage);
        }

        // Update is called once per frame
        void Update()
        {


            if (isDied || !GameState.Instance.IsPlaying() || !isFirstInput)
            {
                return;
            }

            if (!isChop)
            {
                distance = Vector3.Distance(transform.position, Vector3.zero);
                if(distance < 1)
                {
                    _enemyAnimator.SetBool("Chop", true);
                    isChop = true;
                }
            }

            _enemyTransform.LookAt(Vector3.zero);
            _enemyTransform.localEulerAngles = new Vector3(0, _enemyTransform.localEulerAngles.y, 0);

            _enemyTransform.position += _enemyTransform.forward.normalized * Time.deltaTime * speed;
        }


    }

}
