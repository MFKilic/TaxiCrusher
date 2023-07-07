using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace TemplateFx.Enviroment
{
    public class FenceController : MonoBehaviour
    {
        [SerializeField] private Transform _woodPieceTransform;
        [SerializeField] private Rigidbody _woodPieceRigidbody;
        private Vector3 _woodPieceStartPos;
        private bool _woodPieveSetActiveFlag;
       
        void Awake () 
        {
            _woodPieceStartPos = _woodPieceTransform.localPosition;
        }

        private void OnEnable()
        {
            GameState.Instance.OnPrepareNewGameEvent += Instance_OnPrepareNewGameEvent;
        }

        private void Instance_OnPrepareNewGameEvent()
        {
            _woodPieceRigidbody.isKinematic = true;
            _woodPieceTransform.gameObject.SetActive(true);
            _woodPieceTransform.DOLocalJump(_woodPieceStartPos, 1, 1, 1).OnComplete(() => { _woodPieveSetActiveFlag = false; });
            _woodPieceTransform.DOLocalRotate(Vector3.zero,1);
         
        }

        private void OnDisable()
        {
            GameState.Instance.OnPrepareNewGameEvent -= Instance_OnPrepareNewGameEvent;
        }
        // Update is called once per frame
        void Update()
        {
            if(_woodPieveSetActiveFlag)
            {
                return;
            }

            if(_woodPieceTransform.localPosition.y < -20)
            {
                _woodPieceTransform.gameObject.SetActive(false);
                _woodPieveSetActiveFlag = true;
            }
        }
    }
}

