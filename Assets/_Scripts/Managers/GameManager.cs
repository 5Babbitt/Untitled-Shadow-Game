using UnityEngine;
using FiveBabbittGames;

/// <summary>
/// GameManager
/// </summary>
public class GameManager : Singleton<GameManager>
{
    bool isGameOver;
    public bool IsGameOver => isGameOver;
    
    protected override void Awake()
    {
        base.Awake();

    }

    public void GameOver()
    {
        isGameOver = true;
        SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.EndGame, true);
    }

    public void Escaped()
    {
        isGameOver = false;
        SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.EndGame, true);

    }
}
