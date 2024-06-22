namespace PurchaseManagement.Commons
{
    public static class NavigationParametersTest
    {
        // Use a dictionary to store navigation parameters
        private static readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public static void AddParameter(string key, object value)
        {
            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = value;
            }
            else
            {
                _parameters.Add(key, value);
            }
        }
        public static Dictionary<string, object> GetParameters()
        {
            return _parameters;
        }
        public static object GetParameter(string key)
        {
            if (_parameters.ContainsKey(key))
            {
                return _parameters[key];
            }

            return null;
        }

        public static void Clear()
        {
            _parameters.Clear();
        }
    }
}
