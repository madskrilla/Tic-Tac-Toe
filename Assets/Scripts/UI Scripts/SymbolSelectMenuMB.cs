using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymbolSelectMenuMB : OptionMenuMB
{
    private int player;
    [SerializeField]
    private GameObject[] symbolImages;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject ResizeBoardMenu;
    [SerializeField]
    private RectTransform playerOneIcon;
    [SerializeField]
    private RectTransform playerTwoIcon;
    [SerializeField]
    private float particleTravelTime = 1.0f;
    [SerializeField]
    private ParticleSystem selectionParticles;

    private void Start()
    {
        Messenger.GetInstance().RegisterListener(new OpenSymbolSelectMenuMsg(), OpenMenu);
    }

    private void OpenMenu(Message msg)
    {
        OpenMenu();
    }

    public override void OpenMenu()
    {
        base.OpenMenu();

        player = 0;
        for (int i = 0; i < symbolImages.Length; i++)
        {
            symbolImages[i].GetComponent<Image>().color = Color.white;
            symbolImages[i].GetComponent<Button>().interactable = true;
        }

        SetTitleText();
    }

    public void SelectImage(int index)
    {
        StartCoroutine(MakeSelection(index));
    }

    private IEnumerator MakeSelection(int index)
    {
        symbolImages[index].GetComponent<Image>().color = Color.gray;
        symbolImages[index].GetComponent<Button>().interactable = false;

        selectionParticles.transform.position = symbolImages[index].transform.position;
        Vector3 particleDest = player == 0 ? playerOneIcon.position : playerTwoIcon.position;
        float moveTime = 0f;
        float movePercent;

        selectionParticles.Play();

        while (selectionParticles.transform.position != particleDest)
        {
            movePercent = moveTime / particleTravelTime;
            selectionParticles.transform.position = Vector3.Lerp(selectionParticles.transform.position, particleDest, movePercent);
            moveTime += Time.deltaTime;
            yield return null;
        }

        selectionParticles.Stop();

        SymbolSelectionMsg msg = new SymbolSelectionMsg();
        msg.player = player;
        msg.symbol = symbolImages[index].GetComponent<Image>().sprite;

        Messenger.GetInstance().BroadCastMessage(msg);

        player++;
        SetTitleText();

        if (player > 1)
        {
            CloseMenu();
        }

    }

    private void SetTitleText()
    {
        titleText.text = string.Format("Player {0}:\nPlease Select A Symbol", player + 1);
    }

    private void OnDestroy()
    {
        Messenger.GetInstance().UnregisterListener(new OpenSymbolSelectMenuMsg(), OpenMenu);
    }
}
