using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField]
    PlayerStats playerStats;

    [Header ("UI Elements")]
    [SerializeField]
    TextMeshProUGUI hp;
    [SerializeField]
    TextMeshProUGUI speed;
    [SerializeField]
    TextMeshProUGUI strength;
    [SerializeField]
    TextMeshProUGUI wisdom;

    public void Initialize() 
    {
        playerStats.onStatsChanged += SetStats;
    }
    public void SetStats() 
    {
        hp.SetText($"{playerStats.HP}");
        speed.SetText($"{playerStats.Speed}");
        strength.SetText($"{playerStats.Strength}");
        wisdom.SetText($"{playerStats.Wisdom}");
    }
}
