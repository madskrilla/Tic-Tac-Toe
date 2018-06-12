using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenuMB : MonoBehaviour
{
    [SerializeField]
    private Animator menuAnimator;

    public virtual void OpenMenu()
    {
        menuAnimator.SetBool("Show", true);
    }

    public virtual void CloseMenu()
    {
        menuAnimator.SetBool("Show", false);
    }

}
