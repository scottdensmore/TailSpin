namespace TailSpin.PhoneClient.Infrastructure
{
    using Microsoft.Phone.Shell;

    public static class Tombstoning
    {
        public static void Save(string key, object value)
        {
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                PhoneApplicationService.Current.State.Remove(key);
            }

            PhoneApplicationService.Current.State.Add(key, value);
        }

        public static T Load<T>(string key)
        {
            object result;

            if (!PhoneApplicationService.Current.State.TryGetValue(key, out result))
            {
                result = default(T);
            }
            else
            {
                PhoneApplicationService.Current.State.Remove(key);
            }

            return (T)result;
        }
    }
}
