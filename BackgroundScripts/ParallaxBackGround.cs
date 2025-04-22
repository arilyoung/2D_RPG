using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float xParallaxEffect;
    [SerializeField] private float yParallaxEffect;

    private float xPosition;
    private float yPosition;
    private float length;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
        yPosition = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void Update()
    {
        float xDistanceToMove = cam.transform.position.x * xParallaxEffect;
        float yDistanceToMove = cam.transform.position.y * yParallaxEffect;
        float distanceMoved = cam.transform.position.x * (1 - xParallaxEffect);

        transform.position = new Vector3(xPosition + xDistanceToMove, yPosition + yDistanceToMove);

        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}
