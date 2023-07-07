using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TemplateFx
{
    public class EventManager : MonoBehaviour
    {
        public delegate void OnFirstInputDelegate();
        public event OnFirstInputDelegate OnFirstInputEvent;

        public delegate void OnEnemyHitTreeDelegate(float damage);
        public event OnEnemyHitTreeDelegate OnEnemyHitTreeEvent;

        public delegate void OnEnemyDiedDelegate();
        public event OnEnemyDiedDelegate OnEnemyDiedEvent;

        public delegate void OnLastEnemyDiedDelegate();
        public event OnLastEnemyDiedDelegate OnLastEnemyDiedEvent;

        public void OnLastEnemyDied()
        {
            OnLastEnemyDiedEvent?.Invoke();
        }

        public void OnFirstInputIsPressed()
        {
            OnFirstInputEvent?.Invoke();
        }

        public void OnEnemyHitTree(float damage)
        {
            OnEnemyHitTreeEvent?.Invoke(damage);
        }

        public void OnEnemyDied()
        {
            OnEnemyDiedEvent?.Invoke();
        }


    }
}

