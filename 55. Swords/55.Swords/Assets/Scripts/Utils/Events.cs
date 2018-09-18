using UnityEngine.Events;
using UnityEngine;

public class Events 
{ 
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }
    [System.Serializable] public class EventGameObject : UnityEvent<GameObject> { }
}
