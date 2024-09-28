using System;
public interface IPlayerStatsProvider
{
    int HP { get; }
    int Speed { get; }
    int Strength { get; }
    int Wisdom { get; }

    event Action OnStatsChanged;
    event Action OnDead;
}
