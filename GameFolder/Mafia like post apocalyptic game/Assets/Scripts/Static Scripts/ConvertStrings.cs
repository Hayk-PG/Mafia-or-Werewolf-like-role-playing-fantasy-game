

public static class ConvertStrings 
{
    public static string GetCountryCode(string code)
    {
        string countryCode = code != null ? code : "aa";
        return countryCode.ToLower();
    }

    public static string SubtractFriendRequestKey(string key)
    {
        int startIndex = 0;
        int length = PlayerKeys.FriendRequest.FR.Length;

        return key.Substring(startIndex, length);
    }

    public static string SubtractPlayfabIdFromFriendRequestKey(string key)
    {
        int startIndex = PlayerKeys.FriendRequest.FR.Length;
        int length = key.Length - startIndex;

        return key.Substring(startIndex, length);
    }
}
