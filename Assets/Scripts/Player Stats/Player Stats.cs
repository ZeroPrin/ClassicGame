using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField]
    PlayerStatsUI playerStatsUI;
    [SerializeField]
    PlayerController playerController;

    [Header("Stats")]
    public int HP;
    public int Speed;
    public int Strength;
    public int Wisdom;

    private float extendedSpeedBoostDuration;
    private float extendedStrengthBoostDuration;

    [Header ("Actions")]
    public Action onStatsChanged;

    [Header("Coroutines")]
    private Coroutine speedBoostCoroutine;
    private Coroutine poisonCoroutine;
    private Coroutine strengthBoostCoroutine;

    public void Initialize()
    {
        HP = 100;
        Speed = (int)playerController.moveSpeed;
        Strength = (int)playerController.jumpForce;
        Wisdom = 0;

        extendedStrengthBoostDuration = 0;
        extendedSpeedBoostDuration = 0;

        onStatsChanged?.Invoke();
    }

    #region Speed Effect
    public void ApplySpeedBoost(int amount, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            extendedSpeedBoostDuration += duration;
        }
        else
        {
            extendedSpeedBoostDuration = duration;
            speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(amount));
        }
    }

    private IEnumerator SpeedBoostCoroutine(int amount)
    {
        Speed += amount;
        onStatsChanged?.Invoke();

        while (extendedSpeedBoostDuration > 0)
        {
            yield return new WaitForSeconds(1f);
            extendedSpeedBoostDuration -= 1f;
        }

        Speed -= amount;
        onStatsChanged?.Invoke();
        speedBoostCoroutine = null;
    }
    #endregion

    #region Strength Effect
    public void ApplyStrengthBoost(int amount, float duration)
    {
        if (strengthBoostCoroutine != null)
        {
            extendedStrengthBoostDuration += duration;
        }
        else
        {
            extendedStrengthBoostDuration = duration;
            strengthBoostCoroutine = StartCoroutine(StrengthBoostCoroutine(amount));
        }
    }

    private IEnumerator StrengthBoostCoroutine(int amount)
    {
        Strength += amount;
        onStatsChanged?.Invoke();

        while (extendedStrengthBoostDuration > 0)
        {
            yield return new WaitForSeconds(1f);
            extendedStrengthBoostDuration -= 1f;
        }

        Strength -= amount;
        onStatsChanged?.Invoke();
        strengthBoostCoroutine = null;
    }
    #endregion

    #region Poison Effect
    public void ApplyPoison(int damagePerSecond, float duration)
    {
        if (poisonCoroutine != null)
        {
            StopCoroutine(poisonCoroutine);
        }
        poisonCoroutine = StartCoroutine(PoisonCoroutine(damagePerSecond, duration));
    }

    private IEnumerator PoisonCoroutine(int damagePerSecond, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            HP -= damagePerSecond;

            if (HP < 0)
                HP = 0;

            onStatsChanged?.Invoke();
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
    }
    #endregion

    #region Wisdom effect
    public void IncreaseWisdom(int amount)
    {
        Wisdom += amount;
        onStatsChanged?.Invoke();
    }
    #endregion

    #region Helth Effect
    public void IncreaseHP(int amount)
    {
        HP += amount;

        if (HP > 100)
            HP = 100;

        onStatsChanged?.Invoke();
    }
    #endregion
}
