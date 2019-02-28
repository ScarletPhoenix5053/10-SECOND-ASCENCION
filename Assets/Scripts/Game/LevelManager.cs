using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sierra.AGPW.TenSecondAscencion;

public class LevelManager : MonoBehaviour
{
    #region Public Vars
    // OBJECTS
    public PlayerController Player1;
    public PlayerController Player2;

    // LEVEL DATA
    public Vector3 Player1Pos;
    public Vector3 Player2Pos;
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
            Debug.Log("eND");
            EndLevel();
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
    public int GetScore()
    {
        var p1Score = (int)Mathf.Ceil(Player1.transform.position.y * 100);
        var p2Score = (int)Mathf.Ceil(Player2.transform.position.y * 100);
        var highest = Mathf.Max(p1Score, p2Score);

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
