using UnityEngine;

public interface IParticleVFXBaseScript 
{
    GameObject DestroyVFX { get; }

    void ManualDestroy();
}
