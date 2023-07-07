using System.Collections;
using System.Collections.Generic;
using TemplateFx;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace TemplateFx.Enviroment
{
    public class MainObjectHealthController : MonoBehaviour
    {
        [SerializeField] private Transform _healthBarTransform;
        [SerializeField] private Image _healthBarImage;

        private Tween _healthBarTween, _shakeTween, _treeFallingTween;
        private float _healthCount;
        private Vector3 _startScale;


        void Start()
        {
            _startScale = transform.localScale;
        }

        private void OnEnable()
        {
            GameState.Instance.OnPrepareNewGameEvent += Instance_OnPrepareNewGameEvent;
            LevelManager.Instance.eventManager.OnEnemyHitTreeEvent += EventManager_OnEnemyHitTreeEvent;
        }

        private void Instance_OnPrepareNewGameEvent()
        {
            if (_treeFallingTween != null)
            {
                _treeFallingTween.Kill();
            }
            transform.DORotate(new Vector3(0, 90, 0), 1);
            _healthCount = LevelManager.Instance.datas.mainObjectHealthCount;
            _healthBarImage.fillAmount = 0;
            _healthBarImage.DOFillAmount(1, 2);
        }

        private void EventManager_OnEnemyHitTreeEvent(float damage)
        {
            _healthCount -= damage;

            float remapHealthCount = _healthCount.Remap(0f, LevelManager.Instance.datas.mainObjectHealthCount, 0f, 1f);

            transform.localScale = _startScale;

            if (_shakeTween == null)
            {
                _shakeTween = transform.DOShakeScale(0.2f, 0.2f, 1, 1).OnComplete(() => { _shakeTween = null; });
            }


            if (_healthBarTween != null)
            {
                _healthBarTween.Kill();
            }
            _healthBarTween = _healthBarImage.DOFillAmount(remapHealthCount, 0.3f);

            if (_healthCount <= 0)
            {
                _healthCount = 0;
                _treeFallingTween = transform.DORotate(new Vector3(0, 90, 90), 4).OnComplete(() => { _treeFallingTween = null; });
                GameState.Instance.OnFinishGame(LevelFinishStatus.LOSE);
            }
        }

        private void OnDisable()
        {
            GameState.Instance.OnPrepareNewGameEvent -= Instance_OnPrepareNewGameEvent;
            LevelManager.Instance.eventManager.OnEnemyHitTreeEvent -= EventManager_OnEnemyHitTreeEvent;
        }

        void Update()
        {
            _healthBarTransform.LookAt(CameraController.Instance.transform);
        }
    }

}
