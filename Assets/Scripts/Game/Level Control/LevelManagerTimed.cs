using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class LevelManagerTimed : LevelManager
    {
        protected override void Awake()
        {
            base.Awake();
        }
        protected virtual void Start()
        {
            _timeRemaining = _timeLimit;
        }
        protected override void Update()
        {
            base.Update();

            // Update time: end level when time runs out
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining <= 0) EndLevel();
            if (_timeDisplay != null) _timeDisplay.text = Math.Round(_timeRemaining, 2).ToString();
        }

        public float SecondsRemaining { get { return _timeRemaining; } }

        [SerializeField]
        private float _timeLimit = 10f;
        [SerializeField]
        [ReadOnly]
        private float _timeRemaining;
        [SerializeField]
        private Text _timeDisplay;
    }
}