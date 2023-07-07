using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TemplateFx.UI;


namespace TemplateFx
{

    public class ViewPlay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private TextMeshProUGUI enemyLeftText;
        [SerializeField] private TutorialController tutorialController;
        public SteeringWheel steeringWheel;
        // Start is called before the first frame update
        public void ViewPlayStart()
        {
            if(LevelManager.Instance.datas.level == 0)
            {
                tutorialController.enabled = true;
            }
            textLevel.text = "LEVEL " + (LevelManager.Instance.datas.level + 1);
        }

       

        public void EnemyLeftTextWriter()
        {
            enemyLeftText.text = SpawnManager.Instance.GetEnemyCount() + " ENEMY LEFT";
        }

        public void OnClickRestartButton()
        {
            GameState.Instance.OnPrepareNewGame();
        }



        // Update is called once per frame
        void Update()
        {

        }
    }

}

