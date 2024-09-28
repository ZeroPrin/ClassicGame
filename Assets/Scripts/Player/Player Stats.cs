using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStats : IInitializable, IPlayerStatsProvider
{
    [Header("Main Components")]
    private PlayerStatsConfig _playerStatsConfig;

    [Header("Stats")]
    public int HP { get; private set; }
    public int Speed { get; private set; }
    public int Strength { get; private set; }
    public int Wisdom { get; private set; }

    public event Action OnStatsChanged;
    public event Action OnDead;

    [Inject]
    public void Consntruct(PlayerStatsConfig playerStatsConfig) 
    {
        _playerStatsConfig = playerStatsConfig;
    }

    void IInitializable.Initialize()
    {
        HP = 100;
        Speed = (int)_playerStatsConfig.MoveSpeed;
        Strength = (int)_playerStatsConfig.JumpForce;
        Wisdom = 0;

        NotifyStatsChanged();
    }

    private void NotifyStatsChanged()
    {
        OnStatsChanged?.Invoke();
    }

    public void ChangeHP(int value)
    {
        HP = Mathf.Clamp(HP + value, 0, 100);
        NotifyStatsChanged();

        if (HP == 0)
        {
            OnDead.Invoke();
        }
    }

    public void ChangeSpeed(int value)
    {
        Speed = Mathf.Clamp(Speed + value, 1, 100);
        NotifyStatsChanged();
    }

    public void ResetSpeed() 
    {
        Speed = (int)_playerStatsConfig.MoveSpeed;
    }

    public void ChangeStrength(int value)
    {
        Strength = Mathf.Clamp(Strength + value, 1, 100);
        NotifyStatsChanged();
    }

    public void ResetStrength()
    {
        Strength = (int)_playerStatsConfig.JumpForce;
    }

    public void ChangeWisdom(int value)
    {
        Wisdom += value;
        NotifyStatsChanged();
    }
}


















//#region Speed Effect
//public void ApplySpeedBoost(int amount, float duration)
//{
//    if (_speedBoostCoroutine != null)
//    {
//        _extendedSpeedBoostDuration += duration;
//    }
//    else
//    {
//        _extendedSpeedBoostDuration = duration;
//        _speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(amount));
//    }
//}

//private IEnumerator SpeedBoostCoroutine(int amount)
//{
//    Speed += amount;
//    OnStatsChanged?.Invoke();

//    while (_extendedSpeedBoostDuration > 0)
//    {
//        yield return new WaitForSeconds(1f);
//        _extendedSpeedBoostDuration -= 1f;
//    }

//    Speed -= amount;
//    OnStatsChanged?.Invoke();
//    _speedBoostCoroutine = null;
//}
//#endregion

//#region Strength Effect
//public void ApplyStrengthBoost(int amount, float duration)
//{
//    if (_strengthBoostCoroutine != null)
//    {
//        _extendedStrengthBoostDuration += duration;
//    }
//    else
//    {
//        _extendedStrengthBoostDuration = duration;
//        _strengthBoostCoroutine = StartCoroutine(StrengthBoostCoroutine(amount));
//    }
//}

//private IEnumerator StrengthBoostCoroutine(int amount)
//{
//    Strength += amount;
//    OnStatsChanged?.Invoke();

//    while (_extendedStrengthBoostDuration > 0)
//    {
//        yield return new WaitForSeconds(1f);
//        _extendedStrengthBoostDuration -= 1f;
//    }

//    Strength -= amount;
//    OnStatsChanged?.Invoke();
//    _strengthBoostCoroutine = null;
//}
//#endregion

//#region Poison Effect
//public void ApplyPoison(int damagePerSecond, float duration)
//{
//    if (_poisonCoroutine != null)
//    {
//        StopCoroutine(_poisonCoroutine);
//    }
//    _poisonCoroutine = StartCoroutine(PoisonCoroutine(damagePerSecond, duration));
//}

//private IEnumerator PoisonCoroutine(int damagePerSecond, float duration)
//{
//    float elapsedTime = 0;
//    while (elapsedTime < duration)
//    {
//        HP -= damagePerSecond;

//        if (HP < 0)
//            HP = 0;

//        OnStatsChanged?.Invoke();
//        yield return new WaitForSeconds(1f);
//        elapsedTime += 1f;
//    }
//}
//#endregion

//#region Wisdom effect
//public void IncreaseWisdom(int amount)
//{
//    Wisdom += amount;
//    OnStatsChanged?.Invoke();
//}
//#endregion

//#region Helth Effect
//public void IncreaseHP(int amount)
//{
//    HP += amount;

//    if (HP > 100)
//        HP = 100;

//    OnStatsChanged?.Invoke();
//}
//#endregion

