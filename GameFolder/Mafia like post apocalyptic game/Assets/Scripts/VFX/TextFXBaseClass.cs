using UnityEngine;

public abstract class TextFXBaseClass : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] string animationClip;

    Animator Animator => anim;
    string AnimationClip => animationClip;


    protected void OnEnable()
    {
        Animator.Play(AnimationClip);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

}
