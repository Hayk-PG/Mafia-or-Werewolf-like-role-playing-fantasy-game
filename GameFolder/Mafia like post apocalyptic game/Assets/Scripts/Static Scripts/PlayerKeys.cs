﻿

public static class PlayerKeys
{
    public struct PlayerPrefabName
    {
        public const string PlayerPrefab = "PlayerPrefab 1";
    }

    #region Savable values keys
    public static string UsernameKey => "Username";
    public static string PasswordKey => "Password";
    public static string EntityId => "EntityId";
    public static string EntityType => "EntityType";
    public static string UserID => "PlayfabID";
    public static string GenderKey => "Gender";
    public static string LocationKey => "Location";
    public static string RegDateKey => "RegDate";
    #endregion

    #region Gender values keys
    public static string Male => "Male"; 
    public static string Female => "Female";
   
    #endregion

    #region Flag key
    public static string Flag => "Flag";
    #endregion

    #region Role index key
    public static string RoleIndex => "RoleIndex";
    #endregion

    #region Statistics
    public struct StatisticKeys
    {
        public static string TotalTimePlayed => "Total time played";
        public static string Rank => "Rank";
        public static string Points => "Points";
        public static string AsSurvivor => "As survivor";
        public static string AsDoctor => "As doctor";
        public static string AsSheriff => "As sheriff";
        public static string AsSoldier => "As soldier";
        public static string AsInfected => "As infected";
        public static string AsWitch => "As lizard";


        public static string WinAsSurvivor => "Win as survivor";
        public static string WinAsInfected => "Win as infected";
        public static string LostAsSurvivor => "Lost as survivor";
        public static string LostAsInfected => "Lost as infected";
    }
    #endregion

    #region FriendRequest
    public class FriendRequest
    {
        /// <summary>
        /// Friend request
        /// </summary>
        public static string FR = "FriendRequest from ";
        /// <summary>
        /// Friend request accepted
        /// </summary>
        public static string FRA = "FriendRequestAccepted";
        /// <summary>
        /// Friend request denied
        /// </summary>
        public static string FRD = "FriendRequestDenied";
        public static string ID = "|-ID-|";
    }
    #endregion

    #region ProfilePicture
    public static string ProfilePicture = "spriteImage";
    #endregion

    #region InternalData
    public struct InternalData
    {
        public static string MessageKey = "Message";
        public static string MessageEndPoint = "*_*_*_";
    }
    #endregion

    #region SetPlayersRoleKeys
    public struct SetPlayersRoleKeys
    {
        public const string AvatarButtonIndex = "AvatarButtonIndex";
        public const string RoleNumber = "RoleNumber";
        public const string TakeAvatarButtonOwnership = "TakeAvatarButtonOwnership";
        public const string RoleName = "RoleName";
        public const string RoomName = "RoomName";
        public const string SetOwnedAvatarButtonSprite = "SetOwnedAvatarButtonSprite";
    }
    #endregion
}
