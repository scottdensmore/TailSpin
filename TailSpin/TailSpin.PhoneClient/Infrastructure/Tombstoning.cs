//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


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
