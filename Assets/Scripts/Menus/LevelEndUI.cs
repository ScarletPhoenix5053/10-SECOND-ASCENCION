using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class LevelEndUI : MonoBehaviour
    {
        private void Awake()
        {
            if (_scoreDisplay == null)
            {
                Debug.LogWarning("Score display is never set in " + name + ", and" +
                    " it will never be updated.");
            }
        }

        private float _score= 0;
        [SerializeField]
        private Text _scoreDisplay;

        public void OnRetstartButtonPressed()
        {
            SceneLoader.LoadScene(SceneName.LevelHighscore);
        }
        public void OnMenuButtonPressed()
        {
            SceneLoader.LoadScene(SceneName.MenuMain);
        }
        public void DoNewHighscorePrompt()
        {
            throw new NotImplementedException();
        }
        public void OnSaveNewHighscore()
        {
            throw new NotImplementedException();
        }
        public void SetScore(float newScore)
        {
            _score = newScore;       
            
            if (_scoreDisplay != null)
            {
                _scoreDisplay.text = _score.ToString();
            }
        }
    }
}