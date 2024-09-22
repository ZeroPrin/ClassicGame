using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ItemUI : MonoBehaviour
{
    [Header ("UI Elements")]
    public Image Icon;
    public TextMeshProUGUI Count;
    public TextMeshProUGUI Name;
    //[SerializeField]
    //Button button;

    [Header ("Object")]
    public GameObject Prefab;
    public int Index;

    //public void Initialize() 
    //{
    //    button.onClick.AddListener(onButtonClicked);
    //}

    //public void onButtonClicked() 
    //{
    //    Master.HandController.SetObject(Prefab, Index);
    //    Debug.Log("Obj in hand");
    //}
}
