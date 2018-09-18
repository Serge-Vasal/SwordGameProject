using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Camera _dummyCamera;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private HealthBar healthBar;

    public LoadingScreen loadingScreen;
    public CharacterStats playerStats;

    private int health;
    

    private void Start()
    {        
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);

        _mainMenu.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);

        loadingScreen.gameObject.SetActive(currentState == GameManager.GameState.LOADING);

        healthBar.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);       

        if (previousState == GameManager.GameState.LOADING && currentState == GameManager.GameState.RUNNING)
        {
            SetDummyCameraActive(false);
        }

        if (currentState == GameManager.GameState.PREGAME)
        {
            SetDummyCameraActive(true);
        }
    }    

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }       

        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.StartGame();           
        }

        
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            health = playerStats.GetHealth();
            healthBar.UpdateHealthSlider(health);
        }
    }    

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }  
    
    public void SetMaxHealthSliderBar(int maxHealth)
    {
        healthBar.SetMaxHealthSliderValue(maxHealth);
    }
}
