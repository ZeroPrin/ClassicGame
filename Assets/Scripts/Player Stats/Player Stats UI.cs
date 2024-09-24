using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerStatsUI : MonoBehaviour
{
    [Header ("UI Elements")]
    [SerializeField]
    TextMeshProUGUI hp;
    [SerializeField]
    TextMeshProUGUI speed;
    [SerializeField]
    TextMeshProUGUI strength;
    [SerializeField]
    TextMeshProUGUI wisdom;

    private PlayerStats _playerStats;

    [Inject]
    public void Construct(PlayerStats playerStats) 
    {
        _playerStats = playerStats;
    }

    public void Initialize() 
    {
        _playerStats.OnStatsChanged += SetStats;
    }
    public void SetStats() 
    {
        hp.SetText($"{_playerStats.HP}");
        speed.SetText($"{_playerStats.Speed}");
        strength.SetText($"{_playerStats.Strength}");
        wisdom.SetText($"{_playerStats.Wisdom}");
    }
}
