
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using Pixelplacement;

namespace TemplateFx
{
    public class CameraController : Singleton<CameraController>
    {
        public GameObject goPlayer;

        public Camera cameraHimself;

        Vector3 v3StartOffset;

        Vector3 v3StartPos;

        public float fFollowSpeed = 5;

        public float fLookSpeed = 5;

        private Tweener camShakeTweener;

        void Awake()
        {
            v3StartPos = transform.position;
        }

        private void OnEnable()
        {
            LevelManager.Instance.eventManager.OnLastEnemyDiedEvent += EventManager_OnLastEnemyDiedEvent;
        }

        private void EventManager_OnLastEnemyDiedEvent()
        {
            cameraHimself.DOFieldOfView(20, 0.5f);
        }

        private void OnDisable()
        {
            LevelManager.Instance.eventManager.OnLastEnemyDiedEvent -= EventManager_OnLastEnemyDiedEvent;
        }



        public void SetOffset()
        {
            transform.position = v3StartPos;
            v3StartOffset = transform.position - goPlayer.transform.position;
            cameraHimself.DOFieldOfView(60, 1);
        }


        void FixedUpdate()
        {

            MoveToCar();

        }

        public void ShakeCamera(float fDelay, float fDuration, float fStrength = 3f, int nVibrato = 10, float fRandomness = 90f, bool bFadeOut = true)
        {
            StartCoroutine(Shake(fDelay, fDuration, fStrength, nVibrato, fRandomness, bFadeOut));
        }

        public IEnumerator Shake(float fDelay, float fDuration, float fStrength = 3f, int nVibrato = 10, float fRandomness = 90f, bool bFadeOut = true)
        {
            if (fDelay > 0)
            {
                yield return new WaitForSeconds(fDelay);
            }
            else
            {
                yield return null;
            }

            camShakeTweener = Camera.main.DOShakePosition(fDuration, fStrength, nVibrato, fRandomness, bFadeOut);
        }




        private void MoveToCar()
        {
            Vector3 _targetPos = goPlayer.transform.position + v3StartOffset;

            transform.position = Vector3.Lerp(transform.position, _targetPos, fFollowSpeed * Time.fixedDeltaTime);

        }

    }







}

