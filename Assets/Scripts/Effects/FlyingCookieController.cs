using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCookieController : MonoBehaviour
{
    [SerializeField]
    private float cookieSpeed = 2f;
    [SerializeField]
    private Transform cameraBounds;

    private Vector3 startPosition = new Vector3();

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * cookieSpeed * Time.deltaTime;
        if (transform.position.x > cameraBounds.position.x)
        {
            GetComponentInChildren<ParticleSystem>().Stop();
            transform.position = startPosition;
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
