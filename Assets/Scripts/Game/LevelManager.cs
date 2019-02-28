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
    public delegate void LevelEndHandler(float score);
    public static event LevelEndHandler OnLevelEnd;
    #endregion

    #region Unity Messages
    private void Awake()
    {
        OnLevelStart += SetupLevel;        
        OnLevelEnd += UnsubscribeFromEvents;
    }
    private void Update()
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
        OnLevelEnd?.Invoke(0);
    }
    #endregion
    #region Private Methods
    private void SetupLevel()
    {
        Player1.transform.position = new Vector3(Player1Pos.x, Player1.transform.position.y, Player1Pos.z);
    }
    private void UnsubscribeFromEvents(float score)
    {
        OnLevelStart -= SetupLevel;
        OnLevelEnd -= UnsubscribeFromEvents;
    }
    #endregion
}
