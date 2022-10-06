using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory
{
    private Layer[] layers;
    private GameObject playerObject;
    private Vector2 spawnPos;

    public PlayerFactory(int[] layers, GameObject playerObject, Transform spawnPos)
    {
        this.layers = new Layer[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = new Layer(layers[i]);
        }

        this.playerObject = playerObject;
        this.spawnPos = spawnPos.position;
    }

    public void SpawnNewPlayer()
    {
        SpawnPlayerWithBrain(GetNewBrain());
    }
    public void SpawnNewPlayer(NeuralNetwork brain)
    {
        SpawnPlayerWithBrain(brain);
    }
    public void SpawnBestPlayer(NeuralNetwork brain, RuntimeAnimatorController controller)
    {
        GameObject player = SpawnPlayerWithBrain(brain);

        SpriteRenderer sprite = player.GetComponentInChildren<SpriteRenderer>();
        Color col = sprite.color;
        col.a = 100f;
        sprite.color = col;
        sprite.sortingOrder = 100;

        Animator anim = player.GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = controller;

    }

    GameObject SpawnPlayerWithBrain(NeuralNetwork brain)
    {
        GameObject player = MonoBehaviour.Instantiate(playerObject, spawnPos, Quaternion.identity);
        player.GetComponent<Player>().brain = brain.Clone();
        return player;
    }
    NeuralNetwork GetNewBrain()
    {
        NeuralNetwork brain = new NeuralNetwork(layers);
        return brain;
    }
}
