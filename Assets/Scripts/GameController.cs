using System.Collections;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Normal,
    Menu,
    UpgradeSelection,
    DeathScreen
}

public class GameController : MonoBehaviour
{   
    public static GameController gameController { get; private set; }

    public int level = 0;
    public GameObject player;
    public GameObject playerCamera;
    public GeneratorBehaviour levelGenerator;

    public void StartGame()
    {
        StartCoroutine(levelGenerator.Generate(10 + level));
        player.transform.position = Vector3.up;
        player.GetComponent<Rigidbody>().MovePosition(Vector3.up);
        playerCamera.GetComponent<CameraContoller>().SetRotation(Quaternion.identity);
        StartCoroutine(gameController.SetGameStateDelayed(GameState.Normal, 1));
    }

    void Awake()
    {
        if(gameController == null)
        {
            gameController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StartGame();
    }

    public GameState gameState = GameState.Normal;

    public GameState GetGameState()
    {
        return gameState;
    }

    public IEnumerator SetGameStateDelayed(GameState gs, int frameDelay)
    {
        int elapsed = 0;
        if(elapsed < frameDelay)
        {
            elapsed++;
            yield return null;            
        }
        SetGameState(gs);
        yield break;
    }

    public void SetGameState(GameState gs)
    {
        gameState = gs;
    }
}