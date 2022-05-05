namespace hangnow_back.Authentications;

public class I18n
{
    private static readonly Dictionary<string, string> EnTranslation = new()
    {
        {"invalid_request", "Invalid authentication request"},
        {"invalid_key", "Invalid key: {0}"},
        {"user_not_found", "User not found"},
        {"invalid_auth_request", "Invalid authentication request"},
    };
    
    private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
    {
        {"en", EnTranslation}
    };
    
    public static string Get(string key, params object[] args)
    {
        var currentDictionary = Translations["en"];

        // ReSharper disable once TailRecursiveCall
        return !currentDictionary.ContainsKey(key) ? Get("invalid_key", key) : string.Format(currentDictionary[key], args);
    }
}