using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace TemplateFx.UI
{
    public class TutorialController : Singleton<TutorialController>
    {
        [SerializeField] private GameObject handIconObject;
        [SerializeField] private RectTransform targetCircleObjectRectTransform;
        [HideInInspector] public GameObject targetObject;
        [SerializeField] private string[] tutorialStrings;
        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] private float upIndex;
        bool isCircleStage;


        private void OnEnable()
        {

            LevelManager.Instance.eventManager.OnFirstInputEvent += EventManager_OnFirstInputEvent;
            GameState.Instance.OnFinishGameEvent += Instance_OnFinishGameEvent;
        }

        private void Instance_OnFinishGameEvent(LevelFinishStatus levelFinish)
        {
            handIconObject.SetActive(false);
            targetCircleObjectRectTransform.gameObject.SetActive(false);
            tutorialText.gameObject.SetActive(false);
            isCircleStage = false;
            this.enabled = false;
        }

        private void EventManager_OnFirstInputEvent()
        {
            handIconObject.SetActive(false);
            targetCircleObjectRectTransform.gameObject.SetActive(true);
            tutorialText.text = tutorialStrings[1];
            isCircleStage = true;
        }



        private void OnDisable()
        {

            LevelManager.Instance.eventManager.OnFirstInputEvent -= EventManager_OnFirstInputEvent;
            GameState.Instance.OnFinishGameEvent -= Instance_OnFinishGameEvent;
        }

        void Start()
        {
            handIconObject.SetActive(true);
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = tutorialStrings[0];
        }


        void Update()
        {
            if (!isCircleStage)
            {
                return;
            }

            Vector3 v3NewPos = targetObject.transform.position + Vector3.up * upIndex;
            v3NewPos = CameraController.Instance.cameraHimself.WorldToViewportPoint(v3NewPos);
            v3NewPos = CameraController.Instance.cameraHimself.ViewportToScreenPoint(v3NewPos);
            targetCircleObjectRectTransform.position = v3NewPos;
        }
    }

}
