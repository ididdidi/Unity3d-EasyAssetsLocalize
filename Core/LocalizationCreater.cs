using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationCreater
    {
        public LocalizationTag Create<T>(string name,  T obj, int quantity) where T : Object
        {
            var localizationTag = new LocalizationTag(name, System.Linq.Enumerable.Repeat(obj, quantity));
            return localizationTag;
        }
    }
}