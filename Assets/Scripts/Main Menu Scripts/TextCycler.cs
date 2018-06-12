using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCycler : MonoBehaviour
{
    [SerializeField]
    private float cycleTime = 0.5f;
    private float cycleTimer;
    private TextMeshProUGUI cycleText;

    // Use this for initialization
    void Start()
    {
        cycleText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        cycleTimer += Time.deltaTime;
        if (cycleTimer > cycleTime)
        {
            cycleTimer = 0;
            cycleText.enabled = !cycleText.enabled;
        }
    }
}
