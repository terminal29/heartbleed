using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("World Generation")]
    public IWorldTile stoneTile;
    public IWorldTile lavaTile;
    public IWorldTile moveInstructionTile;
    public IWorldTile jumpInstructionTile;
    public GroundGenerator generator;
    private IGeneratorSpec generatorSpec;

    [Header("Player Stats")]
    bool playerExists = false;
    public PlayerController player;
    public HeartController heart;

    [Header("Camera")]
    public CameraController cameraController;

    [Header("UI")]
    public RespawnUI respawnUI;

    // Start is called before the first frame update
    void Start()
    {
        respawnUI.onQuitSelected = () => Application.Quit();
        respawnUI.onRestartSelected = () => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        respawnUI.onDieSelected = () => PlayerDied();
        respawnUI.onRespawnSelected = () => PlayerRespawned();
        respawnUI.Hide();
        CleanRestart();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void CleanRestart()
    {
        Vector2Int worldSize = generator.GetWorldSize();

        generatorSpec = new HoleGroundGenerator(stoneTile, new List<Rect>{
           new Rect(16, worldSize.y-32, 32, 32),
           new Rect(15, worldSize.y-31, 34, 32),
           new Rect(14, worldSize.y-30, 36, 32),
           new Rect(14, worldSize.y-30, 36, 32),
           new Rect(31, worldSize.y-35, 10, 3)
        }, new Vector2Int(18, worldSize.y - 29), new List<HoleGroundGenerator.CustomGenerator>
        {
            (seed, position) =>
            {
                if(new Rect(31, worldSize.y-35, 10, 3).Contains(position))
                {
                    return lavaTile;
                }
                if((position.x == 17 || position.x == 18) && (position.y == worldSize.y - 32 || position.y == worldSize.y - 31))
                {
                    return moveInstructionTile;
                }
                if((position.x == 22 || position.x == 23) && (position.y == worldSize.y - 32 || position.y == worldSize.y - 31))
                {
                    return jumpInstructionTile;
                }
                if((position.x >= 29 && position.x <= 30) && (position.y >= (worldSize.y - 32) && position.y <= (worldSize.y - 31)))
                {
                    return stoneTile;
                }

                return null;
            }
        });

        generator = Instantiate(generator);
        generator.Generate(0, generatorSpec);

        if (!playerExists)
        {
            player = Instantiate(player);
            player.setGameManager(this);
            cameraController.fixture = player.gameObject;
        }
        Vector2Int worldSpawn = generatorSpec.GetSpawn();
        player.Teleport(worldSpawn);
    }

    public void onPlayerDied()
    {
        respawnUI.Show();
    }

    public void PlayerDied()
    {
        if (heart.GetHealth() > 0)
        {
            respawnUI.Hide();
            Vector2Int worldSpawn = generatorSpec.GetSpawn();
            player.Teleport(worldSpawn);
            player.Respawn();
        }
    }

    public void PlayerRespawned()
    {
        if (heart.GetHealth() > 1)
        {
            respawnUI.Hide();
            heart.SetHealth(heart.GetHealth() - 1);
            Vector2Int worldSpawn = generatorSpec.GetSpawn();
            player.Teleport(worldSpawn);
            player.Respawn(); // TODO respawn at checkpoint
        }
    }

    public PlayerController GetPlayer()
    {
        return player;
    }

    public HeartController GetHeart()
    {
        return heart;
    }

    public GroundGenerator GetWorldGenerator()
    {
        return generator;
    }

    public IGeneratorSpec GetGeneratorSpec()
    {
        return generatorSpec;
    }
}
