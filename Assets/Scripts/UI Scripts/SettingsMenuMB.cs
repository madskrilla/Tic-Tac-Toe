using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuMB : MonoBehaviour
{
    [SerializeField]
    private GameObject boardSelectionMenu;
    [SerializeField]
    private GameObject symbolSelectionMenu;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActivateBoardSelectionMenu()
    {
        boardSelectionMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ActivateSymbolSelectionMenu()
    {
        symbolSelectionMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
