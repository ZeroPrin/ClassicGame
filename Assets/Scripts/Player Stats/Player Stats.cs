using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStats : MonoBehaviour
{
    [Header("Main Components")]
    private PlayerStatsUI _playerStatsUI;
    private PlayerController _playerController;

    [Header("Stats")]
    public int HP;
    public int Speed;
    public int Strength;
    public int Wisdom;

    private float _extendedSpeedBoostDuration;
    private float _extendedStrengthBoostDuration;

    [Header ("Actions")]
    public Action OnStatsChanged;

    [Header("Coroutines")]
    private Coroutine _speedBoostCoroutine;
    private Coroutine _poisonCoroutine;
    private Coroutine _strengthBoostCoroutine;

    [Inject]
    public void Consntruct(PlayerStatsUI playerStatsUI, PlayerController playerController) 
    {
        _playerStatsUI = playerStatsUI;
        _playerController = playerController;
    }

    public void Initialize()
    {
        HP = 100;
        Speed = (int)_playerController.MoveSpeed;
        Strength = (int)_playerController.JumpForce;
        Wisdom = 0;

        _extendedStrengthBoostDuration = 0;
        _extendedSpeedBoostDuration = 0;

        OnStatsChanged?.Invoke();
    }

    #region Speed Effect
    public void ApplySpeedBoost(int amount, float duration)
    {
        if (_speedBoostCoroutine != null)
        {
            _extendedSpeedBoostDuration += duration;
        }
        else
        {
            _extendedSpeedBoostDuration = duration;
            _speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(amount));
        }
    }

    private IEnumerator SpeedBoostCoroutine(int amount)
    {
        Speed += amount;
        OnStatsChanged?.Invoke();

        while (_extendedSpeedBoostDuration > 0)
        {
            yield return new WaitForSeconds(1f);
            _extendedSpeedBoostDuration -= 1f;
        }

        Speed -= amount;
        OnStatsChanged?.Invoke();
        _speedBoostCoroutine = null;
    }
    #endregion

    #region Strength Effect
    public void ApplyStrengthBoost(int amount, float duration)
    {
        if (_strengthBoostCoroutine != null)
        {
            _extendedStrengthBoostDuration += duration;
        }
        else
        {
            _extendedStrengthBoostDuration = duration;
            _strengthBoostCoroutine = StartCoroutine(StrengthBoostCoroutine(amount));
        }
    }

    private IEnumerator StrengthBoostCoroutine(int amount)
    {
        Strength += amount;
        OnStatsChanged?.Invoke();

        while (_extendedStrengthBoostDuration > 0)
        {
            yield return new WaitForSeconds(1f);
            _extendedStrengthBoostDuration -= 1f;
        }

        Strength -= amount;
        OnStatsChanged?.Invoke();
        _strengthBoostCoroutine = null;
    }
    #endregion

    #region Poison Effect
    public void ApplyPoison(int damagePerSecond, float duration)
    {
        if (_poisonCoroutine != null)
        {
            StopCoroutine(_poisonCoroutine);
        }
        _poisonCoroutine = StartCoroutine(PoisonCoroutine(damagePerSecond, duration));
    }

    private IEnumerator PoisonCoroutine(int damagePerSecond, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            HP -= damagePerSecond;

            if (HP < 0)
                HP = 0;

            OnStatsChanged?.Invoke();
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
    }
    #endregion

    #region Wisdom effect
    public void IncreaseWisdom(int amount)
    {
        Wisdom += amount;
        OnStatsChanged?.Invoke();
    }
    #endregion

    #region Helth Effect
    public void IncreaseHP(int amount)
    {
        HP += amount;

        if (HP > 100)
            HP = 100;

        OnStatsChanged?.Invoke();
    }
    #endregion
}
