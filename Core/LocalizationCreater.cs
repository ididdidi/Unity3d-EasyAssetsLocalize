using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationCreater
    {
        public Localization Create<T>(string name,  T obj, int quantity) where T : Object
        {
            var localization = new Localization(name, System.Linq.Enumerable.Repeat(obj, quantity));
            return localization;
        }
    }
}