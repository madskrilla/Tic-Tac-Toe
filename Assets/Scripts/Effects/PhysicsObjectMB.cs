using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObjectMB : MonoBehaviour
{
    private Vector3 velocity = new Vector3();
    private PhysicsController physInst;

    // Use this for initialization
    void Start()
    {
        physInst = GameObject.FindObjectOfType<PhysicsController>() as PhysicsController;
    }

    public void AddForce(Vector3 force)
    {
        velocity += force;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.localPosition += (velocity) * Time.deltaTime;
        transform.position += new Vector3(0, physInst.Gravity) * Time.deltaTime;

        velocity *= 0.99f;
    }
}
