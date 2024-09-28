using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerStatsUI : MonoBehaviour
{
    [Header ("UI Elements")]
    [SerializeField] TextMeshProUGUI _hp;
    [SerializeField] TextMeshProUGUI _speed;
    [SerializeField] TextMeshProUGUI _strength;
    [SerializeField] TextMeshProUGUI _wisdom;

    private IPlayerStatsProvider _playerStats;

    [Inject]
    public void Construct(IPlayerStatsProvider playerStats) 
    {
        _playerStats = playerStats;
    }

    public void Awake() 
    {
        _playerStats.OnStatsChanged += SetStats;
    }

    public void SetStats() 
    {
        _hp.SetText($"{_playerStats.HP}");
        _speed.SetText($"{_playerStats.Speed}");
        _strength.SetText($"{_playerStats.Strength}");
        _wisdom.SetText($"{_playerStats.Wisdom}");
    }
}
