using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapGenerator : MonoBehaviour
{
    public int numOfJumps;

    public int maxPlatformLength;
    public int maxJumpLength;
    public int maxHeightDifference = 2;

    public Tile landTile;
    public Tilemap colliderMap;

    private Vector3Int previous;


    public void Generate()
    {
        transform.position = Vector2.zero + new Vector2(0.5f, 0.5f);
        colliderMap.ClearAllTiles();

        for (int i = 0; i < numOfJumps; i++)
        {
            int platLength = Random.Range(1, maxPlatformLength + 1);
            for (int j = 0; j < platLength; j++)
            {
                Vector3Int currentCell = colliderMap.WorldToCell(transform.position);

                if (currentCell != previous)
                {
                    colliderMap.SetTile(currentCell, landTile);
                    previous = currentCell;
                }

                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }

            int jumpLength = Random.Range(1, maxJumpLength + 1);
            for (int j = 0; j < jumpLength; j++)
            {
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }

            int heightDif = Random.Range(-maxHeightDifference, maxHeightDifference);
            for (int j = 0; j < heightDif; j++)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + heightDif);
            }
        }
    }
}
