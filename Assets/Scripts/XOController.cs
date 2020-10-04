using UnityEngine;

public class XOController : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Enabled = Animator.StringToHash("enabled");
    
    public bool IsEnabled { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetActive(bool value)
    {
        _animator.SetBool(Enabled, value);
        IsEnabled = value;
    }
}
