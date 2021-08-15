using UnityEngine;

public class TimeTickerVFXController : MonoBehaviour
{

    void Update()
    {
        _Destroy();
    }

    #region _Destroy
    void _Destroy()
    {
        if (!PlayerBaseConditions.IsVotesLastSeconds)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    
}
