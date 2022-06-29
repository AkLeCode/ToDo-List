using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.JSONSerializeModule;
using System.IO;

public class ToDoListManager : MonoBehaviour
{
    public Transform content;
    public GameObject addPanel;
    public GameObject itemPrefab;
    public Button CreateBtn;

    string filePath;

    private List<ChecklistObject> checklistObjects = new List<ChecklistObject>();
    private InputField[] addInputfield;


    public class CheckListForJson
    {
        public string itemTitle;
        public int itemIndex;
        public bool itemIsChecked;

        public CheckListForJson(string _itemTitle, bool _itemIsChecked)
        {
            this.itemTitle = _itemTitle;
            this.itemIsChecked = _itemIsChecked;
        }
    }


    private void Start()
    {
        filePath = Application.persistentDataPath + "/ToDoList.txt";
        Debug.Log(filePath);
        LoadData();
    }


    //--- Passe entre les mode "Affichage de la liste" et "Ajout à la liste" ---
    //PARAMETRE : _mode -> Index du mode à mettre
    public void SwitchMode(int _mode)
    {

        switch (_mode)
        {
            //mode 0: Affichage de la liste
            case 0:
                addPanel.SetActive(false);
                break;

            //mode 1: Affichage de la fenetre d'ajout
            case 1:
                addPanel.SetActive(true);
                break;
        }

    }


    public void onCreatClicked()
    {
        addInputfield = addPanel.GetComponentsInChildren<InputField>();
        CreateItem(addInputfield[0].text, false);
        addInputfield[0].SetTextWithoutNotify("");
    }


    public void onCancelClicked()
    {
        SwitchMode(0);
        addInputfield[0].SetTextWithoutNotify("");
    }

    //--- Ajoute un item à la liste ---
    //PARAMETRE : _itemTilte -> Le text contenue dans le future item
    void CreateItem(string _itemTitle,bool isOnLoad, bool _itemIsChecked = false)
    {
        GameObject item = Instantiate(itemPrefab);

        item.transform.SetParent(content);
        ChecklistObject itemObject = item.GetComponent<ChecklistObject>();

        itemObject.SetObjectInfos(_itemTitle, _itemIsChecked);
        checklistObjects.Add(itemObject);

        ChecklistObject temp = itemObject;
        itemObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { setItemIsChecked(temp); });
        itemObject.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { RemoveBtnCliched(temp); });
        if(!isOnLoad)
            SaveData();
        SwitchMode(0);
    }


    void setItemIsChecked(ChecklistObject _item)
    {
        _item.itemIsChecked = _item.GetComponent<Toggle>().isOn;
        SaveData();
    }


    //--- Supprime l'item de la liste ---
    //PARAMETRE : _item -> L'item à supprimer
    void RemoveBtnCliched(ChecklistObject _item)
    {
        checklistObjects.Remove(_item);
        Destroy(_item.gameObject);
        SaveData();
    }


    void SaveData()
    {
        string contents = "";

        foreach (ChecklistObject checklistObject in checklistObjects)
        {
            CheckListForJson temp = new CheckListForJson(checklistObject.itemTitle, checklistObject.itemIsChecked);
            contents += JsonUtility.ToJson(temp) + "\n";
        }
        Debug.Log(contents);
        File.WriteAllText(filePath, contents);
    }


    void LoadData()
    {
        if(File.Exists(filePath))
        {
            string contents = File.ReadAllText(filePath);
            string[] splitContents = contents.Split('\n');

            foreach(string content in splitContents)
            {
                CheckListForJson temp = JsonUtility.FromJson<CheckListForJson>(content);
                CreateItem(temp.itemTitle, true, temp.itemIsChecked);
            }  
        }
        else
        {
            Debug.Log("file not found");
        }
    }
}
