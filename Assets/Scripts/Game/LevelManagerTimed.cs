using System;
using UnityEngine;

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
        base.Awake();
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0) EndLevel();
    }

    public float SecondsRemaining { get { return _timeRemaining; } }

    [SerializeField]
    private float _timeLimit = 10f;
    [SerializeField][ReadOnly]
    private float _timeRemaining;   
}
