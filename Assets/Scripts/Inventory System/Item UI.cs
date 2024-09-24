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

    [Header ("Object")]
    public GameObject Prefab;
    public int Index;
}
