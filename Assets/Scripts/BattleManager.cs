using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private int _numberOffFighters = 2;
    [SerializeField]
    private UnityEvent _onBattleStop;
    [SerializeField]
    private UnityEvent _onBattleFinished;
    [SerializeField]
    private UnityEvent _onBattleStarted;
    private List<Fighter> _Fighters = new List<Fighter>();
    private Coroutine _BattleCouroutine;
    private DamageTarget _damageTarget = new DamageTarget();
    public void AddFighter(Fighter fighter)
    {
        MessageFrame.Instance.ShowMessage($"{fighter.Name} has joined the battle");
        _Fighters.Add(fighter);
        CheckFIghters();
    }
    public void RemoveFighter(Fighter fighter)
    {
        _Fighters.Remove(fighter);
        if (_Fighters.Count < 2)
        {
            if (_BattleCouroutine != null)
            {
                StopCoroutine(_BattleCouroutine);
                _BattleCouroutine = null;
            }
            _onBattleStop?.Invoke();
        }
    }
    private void CheckFIghters()
    {
        if (_Fighters.Count < _numberOffFighters)
        {
            return;
        }

        _onBattleStarted?.Invoke();
    }
    public void StartBattle()
    {
        foreach (Fighter fighter in _Fighters)
        {
            fighter.InitializeFighter();
        }
        _BattleCouroutine = StartCoroutine(BattleCoroutine());
    }
    private IEnumerator BattleCoroutine()
    {

        while (_Fighters.Count > 1)
        {
            Fighter attacker = _Fighters[Random.Range(0, _Fighters.Count)];
            Fighter defender = attacker;
            while (defender == attacker)
            {
                defender = _Fighters[Random.Range(0, _Fighters.Count)];
            }
            attacker.transform.LookAt(defender.transform);
            defender.transform.LookAt(attacker.transform);
            Attack attack = attacker.attacks.getRandomAttack();
            MessageFrame.Instance.ShowMessage($"{attacker.Name} attacks With {attack.attackName}!");
            SoundManager.instance.Play(attack.soundName);
            attacker.CharacterAnimator.Play(attack.animationName);
            GameObject attackParticles = Instantiate(attack.particlesPrefab, attacker.transform.position, Quaternion.identity);
            attackParticles.transform.SetParent(attacker.transform);
            yield return new WaitForSeconds(attack.attackTime);
            float damage = Random.Range(attack.minDamage, attack.MaxDamage);
            GameObject DefenderParticles = Instantiate(attack.hitParticlesPrefab, defender.transform.position, Quaternion.identity);
            DefenderParticles.transform.SetParent(defender.transform);
            _damageTarget.setDamageTarget(damage, defender.transform);
            defender.Health.TakeDamage(_damageTarget);
            if (defender.Health.CurrentHealth <= 0)
            {
                RemoveFighter(defender);

            
            }
            
            yield return new WaitForSeconds(1f);
            
            EndBattle(_Fighters[0]);
        }
        _onBattleFinished?.Invoke();
    }
    private void EndBattle(Fighter winner)
    {
        winner.transform.LookAt(Camera.main.transform);
        MessageFrame.Instance.ShowMessage($"{winner.Name} wins the battle");
        SoundManager.instance.Play(winner.WinSoundName);
        winner.CharacterAnimator.Play(winner.WinAnimationName);
        _onBattleFinished?.Invoke();
    }
}
