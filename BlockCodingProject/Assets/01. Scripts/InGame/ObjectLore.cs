using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectLore : MonoBehaviour, IPointerClickHandler
{
    private Image myImg;
    private string myHeaderName;
    private Image myHeaderIcon;

    [TextArea(5, 10)]
    public string lore;

    private void Awake()
    {
        myImg = GetComponent<Image>();
        myHeaderName = transform.Find("BlockName").transform.Find("Name").GetComponent<Text>().text;
        myHeaderIcon = transform.Find("BlockImage").GetComponent<Image>();
        myHeaderName = myHeaderName.Replace("\n", " ");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.LorePanel(myHeaderIcon.sprite, myImg.color, myHeaderName, lore);
    }
}
