using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] SystemPrefabs;
    public Events.EventGameState OnGameStateChanged;

    public enum GameState
    {
        PREGAME,
        LOADING,
        RUNNING,
        PAUSEDINVENTORY,
        PAUSED,
        GAMEOVER
    }
    private List<GameObject> instancedSystemPrefabs;
    private string _currentLevelName = string.Empty;
    private AsyncOperation ao;
    private List<AsyncOperation> loadOperations;
    GameState currentGameState = GameState.PREGAME;

    public GameState CurrentGameState
    {
        get { return currentGameState; }
        private set { currentGameState = value; }
    }

    private void Start()
    {        
        DontDestroyOnLoad(gameObject);
        loadOperations = new List<AsyncOperation>();
        instancedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void StartGame()
    {
        StateToLoading();
        LoadLevel("01Level1");
    }

    private void Update()
    {
        if (currentGameState==GameState.PREGAME)
        {
            return;
        }

        if (currentGameState == GameState.LOADING)
        {
            UIManager.Instance.loadingScreen.UpdateLoadingSlider(ao.progress);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = currentGameState;
        currentGameState = state;

        switch (currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;                
                break;

            case GameState.LOADING:
                Time.timeScale = 1.0f;                
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;                
                break;

            case GameState.PAUSEDINVENTORY:
                Time.timeScale = 0.0f;                
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;                
                break;

            case GameState.GAMEOVER:
                Time.timeScale = 1.0f;                
                break;

            default:
                break;
        }
        OnGameStateChanged.Invoke(currentGameState, previousGameState);
    }        

    public void LoadLevel(string levelName)
    {
        ao=SceneManager.LoadSceneAsync(levelName,LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.Log("[GameManager] Unable to loal level" + levelName);
            return;
        }        
        ao.completed += OnLoadOperationComplete;
        loadOperations.Add(ao);
        _currentLevelName = levelName;
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (loadOperations.Contains(ao))
        {
            loadOperations.Remove(ao);
            if (loadOperations.Count == 0)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("01Level1"));
                StateToRunning();
                
            }
        }        
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao=SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.Log("[GameManager] Unable to unloal level" + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Boot"));        
        UpdateState(GameState.PREGAME);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for(int i = 0; i < instancedSystemPrefabs.Count; ++i)
        {
            Destroy(instancedSystemPrefabs[i]);
        }
        instancedSystemPrefabs.Clear();
    }    

    public void StateToLoading()
    {
        UpdateState(GameState.LOADING);
    }

    public void StateToPausedInventory()
    {
        
        UpdateState(GameState.PAUSEDINVENTORY);
    }

    public void StateToRunning()
    {        
        UpdateState(GameState.RUNNING);
        
    }

    public void TogglePause()
    {        
        UpdateState(currentGameState==GameState.RUNNING?GameState.PAUSED:GameState.RUNNING);
    }

    public void RestartGame()
    {
        UnloadLevel("01Level1");
        UpdateState(GameState.PREGAME);
    }

    public void StateToGameOver()
    {
        UnloadLevel("01Level1");              
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
