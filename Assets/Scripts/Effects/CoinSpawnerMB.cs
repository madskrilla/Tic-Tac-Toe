using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnerMB : MonoBehaviour
{
    [SerializeField]
    private ObjectPool coinPool;
    [SerializeField]
    private Vector2 initialVelocity = new Vector2();
    [SerializeField]
    private float velocityVariance = 3f;
    [SerializeField]
    private int burstSize = 10;
    private Coroutine activeCO;


    // Use this for initialization
    void Start()
    {
        
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            for (int i = 0; i < burstSize; i++)
            {
                SpawnObject();
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StartCoinShower()
    {
        activeCO = StartCoroutine(SpawnObjects());
    }

    public void StopCoinShower()
    {
        if (activeCO != null)
        {
            StopCoroutine(activeCO); 
        }
    }

    private void SpawnObject()
    {
        GameObject newGO = coinPool.GetObject();
        newGO.transform.position = transform.position;
        newGO.transform.rotation = transform.rotation;

        Vector2 velocity = new Vector2(initialVelocity.x, initialVelocity.y);
        velocity.x += (UnityEngine.Random.Range(-velocityVariance, velocityVariance));
        velocity.y += UnityEngine.Random.Range(-velocityVariance, velocityVariance);
        newGO.GetComponent<PhysicsObjectMB>().AddForce(velocity);
    }
}
