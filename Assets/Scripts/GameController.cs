using System.Collections;
using UnityEngine;

/// @enum GameState
/// @brief Defines the states the game might be in, eg. the options menu, the death screen etc.
public enum GameState
{
    Normal,
    Menu,
    UpgradeSelection,
    DeathScreen
}

/// @class GameController
/// @brief A Singleton, global controller for the game
/// 
/// Stores a static reference to one of itself, able to be accessed
public class GameController : MonoBehaviour
{   
    public static GameController gameController { get; private set; }

    public int level = 0;
    public GameObject player;
    public GameObject playerCamera;
    public GeneratorBehaviour levelGenerator;
    public RandCards RandCards;
    public bool intermission;
    public int record = 0;
    public void StartGame()
    {
        level = 0;
        GenerateLevel();
        player.transform.position = Vector3.up;
        player.GetComponent<Rigidbody>().MovePosition(Vector3.up);
        playerCamera.GetComponent<CameraContoller>().SetRotation(Quaternion.identity);
        playerCamera.transform.localPosition = new Vector3(0, .75f, 0);
        StartCoroutine(gameController.SetGameStateDelayed(GameState.Normal, 1));
    }

    public void GenerateLevel()
    {
        if(levelGenerator.gameObject.activeSelf)
            StartCoroutine(levelGenerator.Generate(5 + level));
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

    public void OnPlayerKillEnemy(float lifetimeRestore)
    {
        player.GetComponent<PlayerDamage>().Heal(lifetimeRestore);
        RandCards.KillBoost();
    }
}