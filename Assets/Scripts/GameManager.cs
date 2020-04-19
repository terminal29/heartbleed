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
    private GroundGenerator generatorInstance;

    [Header("Player Stats")]
    bool playerExists = false;
    public PlayerController player;
    public HeartController heart;

    [Header("Camera")]
    public CameraController cameraController;

    [Header("UI")]
    public RespawnUI respawnUI;

    [Header("Loot")]
    public GameObject coinLoot;
    public int coins = 0;

    [Header("Enemies")]
    public SlimeEnemy slimeEnemyPrefab;


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

    public GameObject GetCoinPrefab()
    {
        return coinLoot;
    }

    public void AddCoins(int coins)
    {
        this.coins += coins;
    }

    Rect InvertRectY(Rect input)
    {
        Vector2Int worldSize = generatorInstance.GetWorldSize();
        return new Rect(input.x, worldSize.y - input.y - input.height, input.width, input.height);
    }

    public void Regenerate()
    {
        if (generatorInstance)
        {
            generatorInstance.Clear();
            DestroyImmediate(generatorInstance);
        }
        generatorInstance = Instantiate(generator);

        Vector2Int worldSize = generatorInstance.GetWorldSize();

        generatorSpec = new HoleGroundGenerator(stoneTile, new List<Rect>{
           new Rect(16, worldSize.y-32, 32, 32),
           new Rect(15, worldSize.y-31, 34, 32),
           new Rect(14, worldSize.y-30, 36, 32),
           new Rect(14, worldSize.y-30, 36, 32),
           InvertRectY(new Rect(42, 32, 5, 6)),
           InvertRectY(new Rect(26, 36, 18, 2)),
           InvertRectY(new Rect(12, 38, 18, 4)),
           InvertRectY(new Rect(12, 38, 2, 10)),
           InvertRectY(new Rect(12, 48, 30, 10)),
           InvertRectY(new Rect(42, 48, 10, 2)),
           InvertRectY(new Rect(52, 48, 2, 14)),
           InvertRectY(new Rect(42, 60, 10, 2)),
           InvertRectY(new Rect(26, 60, 16, 2)),
           InvertRectY(new Rect(12, 60, 16, 8)),
           InvertRectY(new Rect(10, 60, 2, 20)),
           InvertRectY(new Rect(12, 79, 2, 1)),
           InvertRectY(new Rect(14, 79, 20, 10)),
           InvertRectY(new Rect(34, 79, 4, 2)),
           InvertRectY(new Rect(36, 81, 2, 30)),
           InvertRectY(new Rect(10, 93, worldSize.x-20, 29)),
        }, new Vector2Int(18, worldSize.y - 29), new List<HoleGroundGenerator.CustomGenerator>
        {
            (seed, position) =>
            {
                Rect[] boulders = new Rect[]
                {
                    new Rect(14, 40, 2, 2),
                    new Rect(20, 40, 2, 2),
                    new Rect(26, 40, 2, 2),
                    new Rect(12, 50, 2, 8),
                    new Rect(14, 52, 2, 6),
                    new Rect(16, 54, 2, 4),
                    new Rect(18, 56, 2, 2),
                    new Rect(22, 52, 2, 2),
                    new Rect(26, 52, 2, 2),
                    new Rect(30, 52, 2, 2),
                    new Rect(34, 52, 2, 2),
                    new Rect(38, 52, 2, 2),
                    new Rect(42, 52, 2, 2),
                    new Rect(26, 64, 2, 4),
                    new Rect(24, 66, 2, 2),
                    new Rect(20, 67, 2, 1),
                    new Rect(16, 67, 2, 1),
                    new Rect(12, 67, 2, 1),
                    new Rect(14, 82, 2, 7),
                    new Rect(24, 85, 2, 2),
                };

                foreach(Rect boulder in boulders)
                {
                    if (InvertRectY(boulder).Contains(position))
                    {
                        return stoneTile;
                    }
                }

                Rect[] lavaSpots = new Rect[]
                {
                    new Rect(31, 32, 10, 3),
                    new Rect(50, 62, 2, 2),
                    new Rect(46, 62, 2, 2),
                    new Rect(42, 62, 2, 2),
                    new Rect(38, 62, 2, 2),
                    new Rect(34, 62, 2, 2),
                    new Rect(30, 62, 2, 2),
                    new Rect(22, 68, 2, 2),
                    new Rect(18, 68, 2, 2),
                    new Rect(14, 68, 2, 2),
                    new Rect(16, 89, 18, 2)
                };

                foreach(Rect lava in lavaSpots)
                {
                    if (InvertRectY(lava).Contains(position))
                    {
                        return lavaTile;
                    }
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

        generatorInstance.Generate(0, generatorSpec);
    }

    void CleanRestart()
    {
        Regenerate();
        AddAesthetics();
        SpawnMonsters();

        if (!playerExists)
        {
            player = Instantiate(player);
            player.setGameManager(this);
            cameraController.fixture = player.gameObject;
        }
        Vector2Int worldSpawn = generatorSpec.GetSpawn();
        player.Teleport(worldSpawn);
        player.SetAlive(true);
    }


    public Dictionary<PerkType, bool> enabledPerks = new Dictionary<PerkType, bool>{
        {PerkType.BigLight, false },
        {PerkType.QuickReload, true },
        {PerkType.DoubleFire, true },
        {PerkType.DirectFire, true }
        };

    public enum PerkType
    {
        BigLight,
        QuickReload,
        DoubleFire,
        DirectFire
    }

    public Dictionary<PerkType, bool> GetPerks()
    {
        return enabledPerks;
    }

    public void SetPerkStatus(PerkType type, bool status)
    {
        enabledPerks[type] = status;
    }

    public void onPlayerDied()
    {
        respawnUI.Show();
    }

    public void PlayerDied()
    {
        SetPerkStatus(PerkType.BigLight, false);
        SetPerkStatus(PerkType.DirectFire, false);
        SetPerkStatus(PerkType.DoubleFire, false);
        SetPerkStatus(PerkType.QuickReload, false);

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
            player.Respawn(); // TODO respawn at checkpoint
        }
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < 10; i++)
        {
            while (true)
            {
                int x = Random.Range(0, generatorInstance.GetWorldSize().x);
                int y = Random.Range(0, generatorInstance.GetWorldSize().y);

                // Dont spawn on top of the player or right next to them
                if (generatorSpec.GetSpawn().x > x - 5 && generatorSpec.GetSpawn().x < x + 5)
                {
                    continue;
                }
                if (generatorInstance.IsValidMonsterSpawn(new Vector2Int(x, y)))
                {
                    SlimeEnemy slime = Instantiate(slimeEnemyPrefab, new Vector2(x, y), Quaternion.identity);
                    slime.SetLightColor(Random.ColorHSV());
                    break;
                }
            }
        }
    }

    private void AddAesthetics()
    {

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
        return generatorInstance;
    }

    public IGeneratorSpec GetGeneratorSpec()
    {
        return generatorSpec;
    }

    public float GetLootMultiplier()
    {
        return GetHeart().GetMaxHealth() - GetHeart().GetHealth() + 1;
    }
}
