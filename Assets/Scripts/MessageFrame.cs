using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class MessageFrame : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _timeBetweenLetters = 0.05f;
    [SerializeField]
    private float _timeToHide = 2f;
    [SerializeField]
    private string _showAnimationName = "ShowMessageFrame";
    [SerializeField]
    private string _hideAnimationName = "HideMessageFrame";
    private string _currentText;
    private Coroutine typingCoroutine;
    public static MessageFrame Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowMessage(string message)
    {
        _currentText = message;
        _text.text = "";
        _animator.Play(_showAnimationName, 0, 0f);
        typingCoroutine = StartCoroutine(TypeMessage());
    }
    private IEnumerator TypeMessage()
    {
        for (int i = 0; i < _currentText.Length; i++)
        {
            _text.text += _currentText[i];
            yield return new WaitForSeconds(_timeBetweenLetters);
        }
        yield return new WaitForSeconds(_timeToHide);
        _animator.Play(_hideAnimationName, 0, 0f);
    }
    private void StopCoroutine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

        }
    }
    public void StopMessage()
    {
        StopCoroutine();
        _animator.Play(_hideAnimationName, 0, 0f);
        _text.text = "";
    }
    
}
