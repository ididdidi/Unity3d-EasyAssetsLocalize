using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    [System.Serializable]
    public class AudioReceiver : LocalizationReceiver<AudioClip>
    {        
        public AudioReceiver(LocalizationTag localizationTag) : base(localizationTag)
        {
        }
    }
}