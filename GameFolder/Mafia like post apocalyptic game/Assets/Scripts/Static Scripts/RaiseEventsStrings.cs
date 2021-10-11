
public static class RaiseEventsStrings 
{
    public static string CreateGameStartVFX { get; set; } = "CreateGameStartVFX";
    public static string PlayTimerTickingSoundFX { get; set; } = "PlayTimerTickingSoundFX";
    public static string PlayerActivity { get; set; } = "PlayerActivity";
    public static string PlayersVotes { get; set; } = "PlayersVotes";
    public static string GameEnd { get; set; } = "GameEnd";

    public static byte GameStartKey { get; set; } = 0;
    public static byte OnEverySecondKey { get; set; } = 1;
    public static byte GameEndKey { get; set; } = 2;
    public static byte StartNewRoundKey { get; set; } = 3;
}
