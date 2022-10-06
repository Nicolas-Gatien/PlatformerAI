using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using TMPro;

public class GenerationManager : MonoBehaviour
{
    [Header("Spawning")]
    public int numOfPlayers;
    public int[] nodesPerLayer;
    public GameObject playerObject;
    public Transform spawnPosition;

    [Header("Generations")]
    public float timeBtwGen;
    float timeBefNextGen;
    public MapGenerator mapGen;

    [Header("Evolution")]
    public float weightChange = 0.2f;
    public float biasChange = 3f;
    [Range(0, 100)]
    public float evolutionChance;
    public int generations;

    public RuntimeAnimatorController bestBrainSprite;
    public int deathPenalty;
    public PlayerStat[] bestPlayers = new PlayerStat[0];
    PlayerFactory factory;

    public TextMeshProUGUI genText;


    private void Start()
    {
        factory = new PlayerFactory(
            nodesPerLayer, 
            playerObject, 
            spawnPosition
            );

        mapGen = FindObjectOfType<MapGenerator>();
        timeBefNextGen = timeBtwGen;
        if (mapGen != null)
        {
            mapGen.Generate();
        }

        SpawnXAgents(numOfPlayers);
    }

    private void Update()
    {
        timeBefNextGen -= Time.deltaTime;

        if (timeBefNextGen <= 0)
        {
            generations++;
            genText.text = "Gen: " + generations;
            SpawnNextGen(GetBrainsForNextGen());

            if (mapGen != null)
            {
                mapGen.Generate();
            }
        }
    }

    // Spawn Agents
    void SpawnXAgents(int x)
    {
        for (int i = 0; i < x; i++)
        {
            factory.SpawnNewPlayer();
        }
    }
    void SpawnNextGen(NeuralNetwork[] brains)
    {
        timeBefNextGen = timeBtwGen;
        foreach (Player agent in GetAllPlayers())
        {
            Destroy(agent.gameObject);
        }

        foreach (NeuralNetwork brain in brains)
        {
            if (brains[0] == brain)
            {
                factory.SpawnBestPlayer(brain, bestBrainSprite);
            }else
            {
                factory.SpawnNewPlayer(brain);
            }
        }
    }

    Player[] GetAllPlayers()
    {
        Player[] players = FindObjectsOfType<Player>();
        return players;
    }
    PlayerStat[] GetStatsOf(Player[] players)
    {
        PlayerStat[] stats = new PlayerStat[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            stats[i] = new PlayerStat(players[i].Score, players[i].brain.Clone());
        }

        return stats;
    }
    PlayerStat[] GetBestPlayers()
    {
        PlayerStat[] players = GetStatsOf(GetAllPlayers());
        PlayerStat[] allPlayers = new PlayerStat[players.Length + bestPlayers.Length];

        allPlayers = players.Concat(bestPlayers).ToArray();
        allPlayers = allPlayers.OrderBy(player => player.score).ToArray();
        Array.Reverse(allPlayers);

        PlayerStat[] finalBestPlayers = new PlayerStat[numOfPlayers];
        for (int i = 0; i < numOfPlayers; i++)
        {
            finalBestPlayers[i] = allPlayers[i];
        }

        if (finalBestPlayers[0].score / 5 > timeBtwGen)
        {
            timeBtwGen = finalBestPlayers[0].score / 5;
        }

        return finalBestPlayers;
    }

    NeuralNetwork[] GetBrainsSortedByScore()
    {
        PlayerStat[] players = GetBestPlayers();
        NeuralNetwork[] brains = new NeuralNetwork[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            brains[i] = players[i].brain.Clone();
        }

        return brains;
    }
    NeuralNetwork[] GetBrainsForNextGen()
    {
        NeuralNetwork[] brains = GetBrainsSortedByScore();

        for (int i = 0; i < brains.Length / 2; i++)
        {
            brains[brains.Length - i - 1] = brains[i].Clone();
            brains[brains.Length - i - 1].EvolveNetwork(weightChange, biasChange, evolutionChance);
        }

        return brains;
    }
}

public class PlayerStat{

    public PlayerStat(float score, NeuralNetwork brain)
    {
        this.score = score;
        this.brain = brain;
    }
    public float score = 0;
    public NeuralNetwork brain;
}