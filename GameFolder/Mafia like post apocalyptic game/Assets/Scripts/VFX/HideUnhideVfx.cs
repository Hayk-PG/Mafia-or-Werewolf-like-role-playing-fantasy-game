using UnityEngine;

public class HideUnhideVfx : MonoBehaviour, IHideUnhideVfx
{
    [Header("VFX")]
    [SerializeField] GameObject vfx;


    public void Activity(bool setActive)
    {
        vfx.SetActive(setActive);
    }
}
