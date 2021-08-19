

public static class ConvertStrings 
{
    public static string GetCountryCode(string code)
    {
        string countryCode = code != null ? code : "aa";
        return countryCode.ToLower();
    }

    public static string SubtractFriendRequestKey(string key)
    {
        if (PlayerKeys.FriendRequest.FR.Length <= key.Length)
        {
            int startIndex = 0;
            int length = PlayerKeys.FriendRequest.FR.Length;

            return key.Substring(startIndex, length);
        }
        else return null;
    }

    public static string SubtractPlayfabIdFromFriendRequestKey(string key)
    {
        if (PlayerKeys.FriendRequest.FR.Length <= key.Length)
        {
            int startIndex = PlayerKeys.FriendRequest.FR.Length;
            int length = key.Length - startIndex;

            return key.Substring(startIndex, length);
        }
        else return null;
    }

    public static string SubtractMessageDataFromPlayerInternalData(string key, bool subtractMessageWord)
    {
        if (PlayerKeys.InternalData.MessageKey.Length <= key.Length)
        {
            if (subtractMessageWord)
            {
                int startIndex = 0;
                int length = PlayerKeys.InternalData.MessageKey.Length;
                return key.Substring(startIndex, length);
            }
            else
            {
                int startIndex = PlayerKeys.InternalData.MessageKey.Length;
                int endPoint = key.IndexOf(PlayerKeys.InternalData.MessageEndPoint);
                int length = endPoint - startIndex;
                return key.Substring(startIndex, length);
            }
        }
        else return null;
    }
}
