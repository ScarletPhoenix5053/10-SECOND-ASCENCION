using UnityEngine;
using UnityEngine.UI;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class LevelManager : MonoBehaviour
    {
        #region Public Vars
        // OBJECTS
        public PlayerController Player1;
        public PlayerController Player2;

        // LEVEL DATA
        public Vector3 Player1Pos;
        public Vector3 Player2Pos;

        // UI ELEMENTS
        [SerializeField]
        private Text _scoreDisplay;
        [SerializeField]
        private string _scoreText = "SCORE: ";
        [ReadOnly]
        private int _score = 0;
        #endregion
        #region Events
        // EVENTS
        public delegate void LevelStartHandler();
        public static event LevelStartHandler OnLevelStart;
        public delegate void LevelEndHandler(int score);
        public static event LevelEndHandler OnLevelEnd;
        #endregion

        #region Unity Messages
        protected virtual void Awake()
        {
            OnLevelStart += SetupLevel;
            OnLevelEnd += UnsubscribeFromEvents;
        }
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Quitting level");
                EndLevel();
            }

            if (_scoreDisplay != null)
            {
                _scoreDisplay.text = _scoreText + GetScore();
            }
        }
        #endregion

        #region Public Methods
        public static void StartLevel()
        {
            OnLevelStart?.Invoke();
        }
        public static void EndLevel()
        {
            OnLevelEnd?.Invoke(FindObjectOfType<LevelManager>().GetScore());
        }
        public virtual int GetScore()
        {
            // Use height of highest player to calculate a score value
            var p1Score = (int)Mathf.Ceil(Player1.transform.position.y * 100);
            var p2Score = (int)Mathf.Ceil(Player2.transform.position.y * 100);
            var highest = Mathf.Max(p1Score, p2Score);

            // Score will not decrease after a player reaches a certain height
            if (highest < _score) highest = _score;
            else _score = highest;

            // Score will never be below 0
            return highest > 0 ? highest : 0;
        }
        #endregion
        #region Private Methods
        protected void SetupLevel()
        {
            Player1.transform.position = new Vector3(Player1Pos.x, Player1.transform.position.y, Player1Pos.z);
        }
        protected void UnsubscribeFromEvents(int score)
        {
            OnLevelStart -= SetupLevel;
            OnLevelEnd -= UnsubscribeFromEvents;
        }
        #endregion
    }
}