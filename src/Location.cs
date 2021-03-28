namespace Beedux
{
    public static class Location
    {
        public static string Reducer(string location, IAction action)
        {
            return action switch
            {
                NewLocationAction a => a.Location,
                _ => location
            };
        }
    }
}
