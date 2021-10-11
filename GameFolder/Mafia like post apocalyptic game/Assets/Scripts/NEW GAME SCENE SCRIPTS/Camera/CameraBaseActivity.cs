using UnityEngine;

public class CameraBaseActivity : MonoBehaviour
{
    protected Camera _Camera { get; set; }
    protected Options _Options { get; set; }
    protected InformPlayerRole _InformPlayerRole { get; set; }
    protected ChatSizeController _ChatSizeController { get; set; }
    protected CardsTabController _CardsTabController { get; set; }
    protected EndTab _EndTab { get; set; }

    protected virtual void Awake()
    {
        _Camera = GetComponent<Camera>();
        _CardsTabController = FindObjectOfType<CardsTabController>();
        _InformPlayerRole = FindObjectOfType<InformPlayerRole>();
        _ChatSizeController = FindObjectOfType<ChatSizeController>();
        _Options = FindObjectOfType<Options>();
        _EndTab = FindObjectOfType<EndTab>();
    }


}
