using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class FurthestDistance : MonoBehaviour
{
    public TextMeshProUGUI distText;

    // Update is called once per frame
    void Update()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Player");
        GameObject bestAgent;
        agents = agents.OrderBy(filling => filling.transform.position.x).ToArray();
        bestAgent = agents[agents.Length - 1];

        if (bestAgent.transform.position.x > transform.position.x)
        {
            transform.position = new Vector2(bestAgent.transform.position.x, 0);
        }

        distText.text = (int)transform.position.x + "m";
    }
}
