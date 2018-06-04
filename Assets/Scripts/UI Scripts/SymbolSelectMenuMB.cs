using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymbolSelectMenuMB : MonoBehaviour
{
    private int player;
    [SerializeField]
    private GameObject[] symbolImages;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject ResizeBoardMenu;


    private void OnEnable()
    {
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
        SymbolSelectionMsg msg = new SymbolSelectionMsg();
        msg.player = player;
        msg.symbol = symbolImages[index].GetComponent<Image>().sprite;

        Messenger.GetInstance().BroadCastMessage(msg);

        player++;
        SetTitleText();
        symbolImages[index].GetComponent<Image>().color = Color.gray;
        symbolImages[index].GetComponent<Button>().interactable = false;

        if (player > 1)
        {
            gameObject.SetActive(false);
            ResizeBoardMenu.SetActive(true);
        }
    }

    private void SetTitleText()
    {
        titleText.text = string.Format("Player {0}:\nPlease Select A Symbol", player + 1);
    }
}
