using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Joke.Data
{
    public class AppSetting : ViewModelBase
    {
        // Our settings
        public ApplicationDataContainer localSettings { get; set; }
        StorageFolder localFolder { get; set; }

        //主题
        const string Key_IsDarkTheme = "IsDarkTheme";
        const bool Value_IsDarkTheme = false;

        const string Key_IsSighIn = "IsSighIn";
        const bool Value_IsSighIn = false;

        public AppSetting()
        {
            localSettings = ApplicationData.Current.LocalSettings;
            localFolder = ApplicationData.Current.LocalFolder;
        }

        public bool IsDarkTheme
        {
            get { return GetValueOrDefault<bool>(Key_IsDarkTheme, Value_IsDarkTheme); }
            set { AddOrUpdateValue(Key_IsDarkTheme, value); RaisePropertyChanged(Key_IsDarkTheme); }
        }

        public bool IsSighIn
        {
            get { return GetValueOrDefault<bool>(Key_IsSighIn, Value_IsSighIn); }
            set { AddOrUpdateValue(Key_IsSighIn, value); RaisePropertyChanged(Key_IsSighIn); }
        }











        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (localSettings.Values.ContainsKey(Key))
            {
                // If the value has changed
                if (localSettings.Values[Key] != value)
                {
                    // Store the new value
                    localSettings.Values[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                localSettings.Values.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (localSettings.Values.ContainsKey(Key))
            {
                value = (T)localSettings.Values[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }
    }
}
