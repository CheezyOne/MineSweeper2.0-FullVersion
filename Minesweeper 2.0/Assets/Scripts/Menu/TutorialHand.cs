using UnityEngine;

public class TutorialHand : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _animationClip;

    private void Awake()
    {
        _animator.Play(_animationClip.name);
    }
}