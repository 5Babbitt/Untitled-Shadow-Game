using UnityEngine;
using FiveBabbittGames;
using UnityEngine.SceneManagement;

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
        //await SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.EndGame, true);
        LoadLevel("EndGame");
    }

    public void Escaped()
    {
        isGameOver = false;
        //await SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.EndGame, true);
        LoadLevel("EndGame");

    }

    public async void LoadLevel(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
