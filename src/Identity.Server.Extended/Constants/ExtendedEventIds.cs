namespace Identity.Server.Extended.Constants;

public static class ExtendedEventIds
{
    private const int start = 6000;
    public const int ClientCreation = start + 1;
    public const int ClientUpdate = start + 2;
    public const int ClientDeletion = start + 3;
}