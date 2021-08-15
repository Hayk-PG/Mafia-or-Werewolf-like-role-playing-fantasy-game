using UnityEngine;

public class VFXHolder : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] GameObject[] vfx;

    public GameObject[] VFX => vfx;


    /// <summary>
    /// Instantiate particle effects
    /// </summary>
    /// <param name="position"></param>
    /// <param name="vfxIndex"></param>
    public void CreateVFX(int vfxIndex)
    {
        GameObject vfx = Instantiate(VFX[vfxIndex]);
        vfx.name = VFX[vfxIndex].name;
    }

    /// <summary>
    /// Instantiate particle effects with position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="vfxIndex"></param>
    public void CreateVFX(Vector3 position, int vfxIndex)
    {
        GameObject vfx = Instantiate(VFX[vfxIndex], position, Quaternion.identity);
        vfx.name = VFX[vfxIndex].name;
    }

    /// <summary>
    /// Instantiate particle effects with parent
    /// </summary>
    /// <param name="position"></param>
    /// <param name="vfxIndex"></param>
    public void CreateVFX(Transform parent, int vfxIndex)
    {
        GameObject vfx = Instantiate(VFX[vfxIndex], parent);
        vfx.name = VFX[vfxIndex].name;
    }

    /// <summary>
    /// Instantiate vfx with parent and position
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="position"></param>
    /// <param name="vfxIndex"></param>
    public void CreateVFX(Transform parent, Vector3 position, int siblingIndex, int vfxIndex)
    {
        GameObject vfx = Instantiate(VFX[vfxIndex], parent);
        vfx.transform.position = position;
        vfx.transform.SetSiblingIndex(siblingIndex);
        vfx.name = VFX[vfxIndex].name;
    }
}
