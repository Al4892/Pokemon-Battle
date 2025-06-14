using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private int _numberOffFighters = 2;
    [SerializeField]
    private UnityEvent _onfightersReady;
    [SerializeField]
    private UnityEvent _onBattleFinished;
    [SerializeField]
    private UnityEvent _onBattleStarted;
    private List<Fighter> _Fighters = new List<Fighter>();
    private Coroutine _BattleCouroutine;
    public void AddFighter(Fighter fighter)
    {
        _Fighters.Add(fighter);
        CheckFIghters();
    }
    public void RemoveFighter(Fighter fighter)
    {
        _Fighters.Remove(fighter);
    }
    private void CheckFIghters()
    {
        if (_Fighters.Count < _numberOffFighters)
        {
            return;
        }
        _onfightersReady?.Invoke();
    }
    public void StartBattle()
    {
        _BattleCouroutine = StartCoroutine(BattleCoroutine());
    }
    private IEnumerator BattleCoroutine()
    {
        _onBattleStarted?.Invoke();
        while (_Fighters.Count > 1)
        {
            Fighter attacker = _Fighters[Random.Range(0, _Fighters.Count)];
            Fighter defender = attacker;
            while (defender == attacker)
            {
                defender = _Fighters[Random.Range(0, _Fighters.Count)];
            }
            Attack attack = attacker.attacks.getRandomAttack();
            yield return new WaitForSeconds(attack.attackTime);
            defender.Health.TakeDamage(Random.Range(attack.minDamage, attack.MaxDamage));
            if (defender.Health.CurrentHealth <= 0)
            {
                _Fighters.Remove(defender);
                
            }
            yield return null;
        }
        _onBattleFinished?.Invoke();
    }
}
