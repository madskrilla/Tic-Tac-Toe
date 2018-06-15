using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuMB : MonoBehaviour
{

    private bool musicActive = true;
    private bool soundFXActive = true;

    public void ToggleGameMusic()
    {
        musicActive = !musicActive;
    }

    public void ToggleSoundFX()
    {
        soundFXActive = !soundFXActive;
    }

    public void CloseMenu()
    {
        this.gameObject.SetActive(false);
    }
}
