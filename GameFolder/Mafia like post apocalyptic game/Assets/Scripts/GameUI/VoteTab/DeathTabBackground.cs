using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTabBackground : VoteTabBackground
{
    public event Action<bool> OnDeathVFXSActivity;

    public void DeathVFXSetActive()
    {
        OnDeathVFXSActivity?.Invoke(true);
    }

    public void DeathVFXSetInactive()
    {
        OnDeathVFXSActivity?.Invoke(false);
    }

  








}
