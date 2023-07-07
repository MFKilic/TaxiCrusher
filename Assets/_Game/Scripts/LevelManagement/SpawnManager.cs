using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using TemplateFx.Player;
using TemplateFx.UI;



namespace TemplateFx
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        [SerializeField] private Transform[] _spawnTransforms;
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private GameObject _carPrefab;
        [SerializeField] private GameObject _obstaclePrefab;
        [SerializeField] private int _enemyCount;

        private int _spawnCount;


        private void OnEnable()
        {
            LevelManager.Instance.eventManager.OnEnemyDiedEvent += EventManager_OnEnemyDiedEvent;
        }

        private void EventManager_OnEnemyDiedEvent()
        {
            _enemyCount--;
            UIManager.Instance.viewPlay.EnemyLeftTextWriter();
            if (_enemyCount == 0)
            {
                LevelManager.Instance.eventManager.OnLastEnemyDied();
                GameState.Instance.OnFinishGame(LevelFinishStatus.WIN);
            }
        }

        private void OnDisable()
        {
            LevelManager.Instance.eventManager.OnEnemyDiedEvent -= EventManager_OnEnemyDiedEvent;
        }

        public int GetEnemyCount()
        {
            return _enemyCount;
        }

        public void SpawnEverything()
        {
            SpawnCar();
            SpawnEnemies();
            SpawnObstacles();
        }

        private void SpawnCar()
        {
            GameObject car = Instantiate(_carPrefab, new Vector3(0, 0.45f, -7.8f), Quaternion.identity, LevelManager.Instance.characterHolderTransform);
            CameraController.Instance.goPlayer = car.GetComponentInChildren<CarController>().gameObject;
            CameraController.Instance.SetOffset();
        }

        private void SpawnObstacles()
        {
            if (LevelManager.Instance.datas.level > 8)
            {
                if (Random.value < 0.8f)
                {
                    _obstaclePrefab.SetActive(true);
                }
                else
                {
                    _obstaclePrefab.SetActive(false);
                }
            }
            else
            {
                _obstaclePrefab.SetActive(false);
            }

        }

        public void SpawnEnemies()
        {

            _spawnCount = (LevelManager.Instance.datas.level + 1);

            if (_spawnCount > 21)
            {
                _spawnCount = 21;
            }

            _enemyCount = _spawnCount;

            _spawnTransforms.Shuffle();

            int enemyListNumber = 0;

            for (int i = 0; i < _spawnCount; i++)
            {
                if (LevelManager.Instance.datas.level > 6)
                {
                    enemyListNumber = Random.Range(0, _enemyPrefabs.Length);
                }
                else
                {
                    enemyListNumber = 0;
                }

              GameObject goEnemy = Instantiate(_enemyPrefabs[enemyListNumber], 
                  _spawnTransforms[i].position, _spawnTransforms[i].rotation, LevelManager.Instance.characterHolderTransform);
                if(LevelManager.Instance.datas.level == 0)
                {
                    TutorialController.Instance.targetObject = goEnemy;
                }

            }

            UIManager.Instance.viewPlay.EnemyLeftTextWriter();

        }

    }


}

