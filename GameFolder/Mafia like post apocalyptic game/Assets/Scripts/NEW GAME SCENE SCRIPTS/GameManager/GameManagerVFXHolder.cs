using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerVFXHolder : MonoBehaviour
{
    [Serializable] public class VFX
    {
        [SerializeField] internal GameObject[] vfx;

        /// <summary>
        /// 0: GameStartVFX
        /// </summary>
        public GameObject[] Vfx
        {
            get => vfx;
        }
    }

    public VFX _VFX;


    /// <summary>
    /// Instantiate particle effects
    /// </summary>
    /// <param name="position"></param>
    /// <param name="vfxIndex"></param>
    public void CreateVFX(int vfxIndex)
    {
        GameObject vfx = Instantiate(_VFX.Vfx[vfxIndex]);
        vfx.name = _VFX.Vfx[vfxIndex].name;
    }

    /// <summary>
    /// Instantiate particle effects with position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="vfxIndex"></param>
    public void CreateVFX(Vector3 position, int vfxIndex)
    {
        GameObject vfx = Instantiate(_VFX.Vfx[vfxIndex], position, Quaternion.identity);
        vfx.name = _VFX.Vfx[vfxIndex].name;
    }

    /// <summary>
    /// Instantiate particle effects with parent
    /// </summary>
    /// <param name="position"></param>
    /// <param name="vfxIndex"></param>
    public void CreateVFX(Transform parent, int vfxIndex)
    {
        GameObject vfx = Instantiate(_VFX.Vfx[vfxIndex], parent);
        vfx.name = _VFX.Vfx[vfxIndex].name;
    }

    /// <summary>
    /// Instantiate vfx with parent and position
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="position"></param>
    /// <param name="vfxIndex"></param>
    public void CreateVFX(Transform parent, Vector3 position, int siblingIndex, int vfxIndex)
    {
        GameObject vfx = Instantiate(_VFX.Vfx[vfxIndex], parent);
        vfx.transform.position = position;
        vfx.transform.SetSiblingIndex(siblingIndex);
        vfx.name = _VFX.Vfx[vfxIndex].name;
    }
}
