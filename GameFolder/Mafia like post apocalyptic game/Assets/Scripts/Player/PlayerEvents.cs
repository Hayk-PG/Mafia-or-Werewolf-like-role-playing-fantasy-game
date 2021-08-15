using System;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public event Action<int, int> OnTakeAvatarButtonOwnership;
    public event Action<int, string> OnSetOwnedAvatarButtonSprite;
    public event Action OnStartPlayerSelfTimer;
    public event Action OnSetTeamsUI;


    void Update()
    {
        if(PlayerBaseConditions._InstanceIsThis())
        {
            InvokeOnTakeAvatarButtonOwnership();
            InvokeOnSetOwnedAvatarButtonSprite();
        }
    }

    void InvokeOnTakeAvatarButtonOwnership()
    {
        if (PlayerComponents.instance.PlayerSerializeView.TakeAvatarButtonOwnership)
        {
            OnTakeAvatarButtonOwnership?.Invoke(PlayerComponents.instance.PlayerSerializeView.RoleNumber, PlayerComponents.instance.PlayerSerializeView.AvatarButtonIndex);
            PlayerComponents.instance.PlayerSerializeView.TakeAvatarButtonOwnership = false;
        }
    }

    /// <summary>
    /// Also invokes OnStartPlayerSelfTimer event
    /// </summary>
    void InvokeOnSetOwnedAvatarButtonSprite()
    {
        if (PlayerComponents.instance.PlayerSerializeView.SetOwnedAvatarButtonSprite)
        {
            OnSetOwnedAvatarButtonSprite?.Invoke(PlayerComponents.instance.PlayerSerializeView.AvatarButtonIndex, PlayerComponents.instance.PlayerSerializeView.RoleName);

            InvokeOnStartPlayerSelfTimer();
            InvokeOnSetTeamsUI();

            PlayerComponents.instance.PlayerSerializeView.SetOwnedAvatarButtonSprite = false;
        }
    }

    void InvokeOnStartPlayerSelfTimer()
    {
        OnStartPlayerSelfTimer?.Invoke();
    }

    void InvokeOnSetTeamsUI()
    {
        OnSetTeamsUI?.Invoke();
    }




}
