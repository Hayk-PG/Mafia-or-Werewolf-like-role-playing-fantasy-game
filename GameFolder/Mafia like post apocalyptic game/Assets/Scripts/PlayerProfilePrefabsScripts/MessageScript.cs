using UnityEngine;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour
{
    [SerializeField] Button messageButton;
    [SerializeField] Button deleteMessageButton;
    [SerializeField] Text messageSentFromText;

    public string MessageSentFrom
    {
        get => messageSentFromText.text;
        set => messageSentFromText.text = value;
    }    
    public string InternalDataKey { get; set; }
    public string InternalDataValue { get; set; }   //Message


    void Update()
    {
        OnClickButton(messageButton);
        OnClickButton(deleteMessageButton);
    }

    void OnClickButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            if(button == messageButton)
            {
                MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.CanvasGroups[8], true);
                PlayerBaseConditions.PlayerProfile.NotificationMessageText = InternalDataValue;
                PlayerBaseConditions.PlayfabManager.PlayfabInternalData.DeleteData(PlayerBaseConditions.OwnPlayfabId, InternalDataKey, InternalDataValue);
                Destroy(gameObject);
            }

            if(button == deleteMessageButton)
            {
                PlayerBaseConditions.PlayfabManager.PlayfabInternalData.DeleteData(PlayerBaseConditions.OwnPlayfabId, InternalDataKey, InternalDataValue);
                Destroy(gameObject);
            }
        });
    }
}
