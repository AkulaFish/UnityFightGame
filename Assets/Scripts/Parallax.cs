using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float multiplier;
    public GameObject camera;
    private Vector3 startPosition;
    public bool isInfinite;
    private float lenght;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 tmp = new Vector3(startPosition.x, startPosition.y);

        startPosition = transform.position;
        if (isInfinite)
        {
            lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(startPosition.x - 2 + (multiplier + camera.transform.position.x), transform.position.y, transform.position.z);

        if (isInfinite)
        {
            float tmp = camera.transform.position.x + (1 - multiplier);

            if (tmp > startPosition.x + lenght)
            {
                startPosition.x += lenght;
            } 
            else if (tmp < startPosition.x - lenght)
            {
                startPosition.x -= lenght;
            }
        }
    }
}
