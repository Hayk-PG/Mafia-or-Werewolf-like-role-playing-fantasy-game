using UnityEngine;

public class PlayerBadgeButton : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void PlayBadgeAnimation()
    {
        anim.Play("PlayerBadgeAnimation");
    }
}
