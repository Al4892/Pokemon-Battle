using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Fighter : MonoBehaviour
{
    [SerializeField]
    private string _name;
    public string Name => _name;
    [SerializeField]
    private Health _health;
    [SerializeField]
    private Animator _characterAnimator;
    [SerializeField]
    private Attacks _attacks;
    public Health Health => _health;
    public Attacks attacks => _attacks;
    public Animator CharacterAnimator => _characterAnimator;
    [SerializeField]
    private UnityEvent _onFighterInitialized;
    [SerializeField]
    private string _winAnimation = "win";
    public string WinAnimationName => _winAnimation;
    [SerializeField]
    private string _winSoundName = "WinSound";
    public string WinSoundName => _winSoundName;
    public void InitializeFighter()
    {
        _onFighterInitialized?.Invoke();
    }

}
