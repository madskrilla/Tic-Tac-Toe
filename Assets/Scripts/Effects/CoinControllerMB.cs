using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControllerMB : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 2f;
    private float lifeTimer;

    // Use this for initialization
    void Start()
    {
        lifeTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
            gameObject.SetActive(false);
            lifeTimer = lifeTime;
        }
    }
}
