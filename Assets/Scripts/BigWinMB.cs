using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWinMB : MonoBehaviour
{
    [SerializeField]
    private CoinSpawnerMB[] coinSpawners;
    [SerializeField]
    private Animator BigWinAnim;


    public void PlayBigWin()
    {
        foreach (var shower in coinSpawners)
        {
            shower.StartCoinShower();
        }

        BigWinAnim.SetBool("Show", true);

        StartCoroutine(RemoveBigWinAnim());
    }

    private IEnumerator RemoveBigWinAnim()
    {
        yield return new WaitForSeconds(3.0f);
        StopBigWin();
    }

    public void StopBigWin()
    {
        foreach (var shower in coinSpawners)
        {
            shower.StopCoinShower();
        }

        BigWinAnim.SetBool("Show", false);
    }

    #region Debug

    public void ActivateCoins()
    {
        foreach (var shower in coinSpawners)
        {
            shower.StartCoinShower();
        }
    }

    public void DeactivateCoins()
    {
        foreach (var shower in coinSpawners)
        {
            shower.StopCoinShower();
        }
    }

    public void ActivateBigWin()
    {
        BigWinAnim.SetBool("Show", true);
    }

    public void DeactivateBigWin()
    {
        BigWinAnim.SetBool("Show", false);
    }
    #endregion
}
