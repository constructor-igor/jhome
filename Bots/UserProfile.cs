namespace Microsoft.BotBuilderSamples.Bots
{
    /// <summary>
    /// This is our application state. Just a regular serializable .NET class.
    /// </summary>
    public class UserProfile  
    {  
        public string Name { get; set; }  
        public string DialogState { get; set; }
    } 
}