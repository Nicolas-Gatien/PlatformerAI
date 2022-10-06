using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public NeuralNetwork brain;
    private PlayerMovement movement;
    private GenerationManager genManager;

    public bool isAlive;

    public float Score
    {
        get
        {
            if (isAlive)
            {
                return transform.position.x;
            }else
            {
                return transform.position.x - genManager.deathPenalty;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        genManager = FindObjectOfType<GenerationManager>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Node[] outputs = brain.GetOutputs();
        double jumpInput1 = outputs[0].value;
        double moveInput1 = outputs[1].value;
        double moveInput2 = outputs[2].value;

        brain.SetInputs(movement.GetData());

        if (jumpInput1 > 0.75f)
        {
            movement.Jump();
        }

        if (moveInput1 < -0.75f)
        {
            movement.Move(-1f);
        }

        if (moveInput2 > 0.75f)
        {
            movement.Move(1f);
        }
    }
}
