using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TemplateFx.UI;

namespace TemplateFx.Player
{
    public class CarController : Singleton<CarController>
    {

        [SerializeField] private Transform[] frontWheels;
        [SerializeField] private TrailRenderer[] backWheelsTrailRenderers;
        [SerializeField] private ParticleSystem[] backWheelsParticles;
        [SerializeField] private float _moveSpeed = 50;
        [SerializeField] private float _maxSpeed = 15;
        [SerializeField] private float _drag = 0.98f;
        [SerializeField] private float _steerAngle = 20;
        [SerializeField] private float _traction = 1;
        [SerializeField] private int _verticalSpeed = 0;
        [SerializeField] private ParticleSystem confettiPs;
        [SerializeField] private Rigidbody rb;
        private SteeringWheel _steeringWheel;
        private float _steerInput, _wheelAngle;
        private bool _trailFlag;



        private Vector3 moveForce;

        void Start()
        {
            _steeringWheel = UIManager.Instance.viewPlay.steeringWheel;
        }

        private void OnEnable()
        {
            GameState.Instance.OnPrepareNewGameEvent += Instance_OnPrepareNewGameEvent;
            GameState.Instance.OnFinishGameEvent += Instance_OnFinishGameEvent;
        }

        private void Instance_OnFinishGameEvent(LevelFinishStatus levelFinish)
        {
            if (levelFinish == LevelFinishStatus.WIN)
            {
                confettiPs.Play();
            }
        }

        private void Instance_OnPrepareNewGameEvent()
        {
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }

        private void OnDisable()
        {
            GameState.Instance.OnPrepareNewGameEvent -= Instance_OnPrepareNewGameEvent;
            GameState.Instance.OnFinishGameEvent -= Instance_OnFinishGameEvent;

        }

        public void SetVerticalSpeed(int verticalSpeed = 1)
        {
            _verticalSpeed = verticalSpeed;
        }

        void Update()
        {
            if (!GameState.Instance.IsPlaying())
            {
                return;
            }



            if (Input.GetMouseButtonUp(0))
            {
                _verticalSpeed = 0;
            }

            FallControl();

            AngleCalculations();

            WheelTrails();
        }

        private void FallControl()
        {
            if (transform.position.y < -30)
            {
                rb.isKinematic = true;
                GameState.Instance.OnFinishGame(LevelFinishStatus.LOSE);
            }

        }

        private void AngleCalculations()
        {
            _steerInput = _steeringWheel.GetAngle() / 200f;
            _wheelAngle = _steerInput * _steerAngle;

            foreach (Transform fW in frontWheels)
            {
                fW.localEulerAngles = Vector3.up * _wheelAngle;
            }
        }

        private void WheelTrails()
        {

            if (Mathf.Abs(_wheelAngle) == _steerAngle)
            {
                if (!_trailFlag)
                {
                    foreach (TrailRenderer tRender in backWheelsTrailRenderers)
                    {
                        tRender.emitting = true;
                    }

                    foreach (ParticleSystem ps in backWheelsParticles)
                    {
                        ps.Play();
                    }
                    _trailFlag = true;
                }
            }
            else
            {
                if (_trailFlag)
                {
                    foreach (TrailRenderer tRender in backWheelsTrailRenderers)
                    {
                        tRender.emitting = false;
                    }
                    foreach (ParticleSystem ps in backWheelsParticles)
                    {
                        ps.Stop();
                    }
                    _trailFlag = false;
                }
            }
        }

        void FixedUpdate()
        {
            if (!GameState.Instance.IsPlaying())
            {
                return;
            }

            DriftEngineControl();
        }

        private void DriftEngineControl()
        {
            moveForce += transform.forward * _moveSpeed * _verticalSpeed * Time.fixedDeltaTime;
            transform.position += moveForce * Time.fixedDeltaTime;

            transform.Rotate(Vector3.up * _steerInput * moveForce.magnitude * _steerAngle * Time.fixedDeltaTime);


            moveForce *= _drag;
            moveForce = Vector3.ClampMagnitude(moveForce, _maxSpeed);


            moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, _traction * Time.fixedDeltaTime) * moveForce.magnitude;
        }
    }
}
