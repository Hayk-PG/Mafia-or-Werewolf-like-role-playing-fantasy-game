using UnityEngine;

public class VFXCamera : MonoBehaviour
{
    static Camera VfxCamera { get; set; }



    void Awake()
    {
        VfxCamera = GetComponent<Camera>();
    }

    public static void VFXCameraActivity(bool enabled)
    {
        if (VfxCamera.enabled != enabled) VfxCamera.enabled = enabled;
    }
}
