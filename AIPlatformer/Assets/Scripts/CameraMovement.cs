using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector2 movement;
    bool locked;
    public float moveSmoothing;
    [SerializeField] private float moveSpeed;

    [Header("Zooming")]
    public float sensitivity = 1;
    public float minFov = 1;
    public float maxFov = 15;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            locked = !locked;
        }

        float fov = Camera.main.orthographicSize;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.orthographicSize = fov;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!locked)
        {
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            transform.position += new Vector3(movement.x * moveSpeed, movement.y * moveSpeed, 0) * Time.deltaTime;
        }else
        {
            GameObject[] agents = GameObject.FindGameObjectsWithTag("Player");
            GameObject bestAgent;
            agents = agents.OrderBy(filling => filling.transform.position.x).ToArray();

            int i = 1;
            while (agents[agents.Length - i].GetComponent<Player>().isAlive == false)
            {
                i++;
            }

            bestAgent = agents[agents.Length-i];

            transform.position = Vector3.Lerp(transform.position, new Vector3(bestAgent.transform.position.x, bestAgent.transform.position.y, -10), moveSmoothing * Time.deltaTime);
        }

    }
}
