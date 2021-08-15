

using UnityEngine;

public interface IRoomButton 
{
    string Pin { get; set; }
    string RoomButtonName { get; set; }
    string RoomName { get; set; }
    string RoomPlayersCount { get; set; }
    Sprite OpenSprite { get; set; }
    Sprite LockSprite { get; set; }


    void UpdateRoomButton(string roomName, int playersCount, int maxPlayers, bool isOpen, bool isLocked, string pin);

    void UpdateRoomButton(int playersCount, int maxPlayers, bool isOpen);

    void EnableRoomCanvasGroup();

    void EnablePasswordCanvasGroup();

}
