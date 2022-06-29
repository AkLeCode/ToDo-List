using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ChecklistObject : MonoBehaviour
{
    public string itemTitle;
    public bool itemIsChecked;

    private Text itemText;
    private Toggle itemToggle;

    private void Start()
    {
        //on initialise le text de l'item
        itemText = GetComponentInChildren<Text>();
        itemText.text = itemTitle;
        itemToggle = GetComponent<Toggle>();
        itemToggle.SetIsOnWithoutNotify(itemIsChecked);
    }

    public void SetObjectInfos(string _itemTitle, bool _itemIsChecked)
    {
        this.itemTitle = _itemTitle;
        this.itemIsChecked = _itemIsChecked;
    }
}
