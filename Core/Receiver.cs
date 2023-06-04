using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ResourceLocalization
{
    /// <summary>
    /// Base class for storing references to localization objects.
    /// </summary>
    [System.Serializable]
    public class Receiver
    {
        private static readonly Dictionary<string, System.Type> TypeMatching = new Dictionary<string, System.Type>
        {
            [typeof(string).ToString()] = typeof(Text),
            [typeof(Texture2D).ToString()] = typeof(Image),
            [typeof(AudioClip).ToString()] = typeof(AudioSource),
            [typeof(VideoClip).ToString()] = typeof(VideoPlayer)
        };

        [SerializeField] private string name;
        [SerializeField] private string id;
        [SerializeField] private string resourceType;
        [SerializeField] private Behaviour[] receivers;
        public bool open;
        
        /// <summary>
        /// Identifier of the localization tag in the repository
        /// </summary>
        public string ID { get => id; set => id = value; }
        public string Name { get => name; }
        public Behaviour[] Receivers { get => receivers; set => receivers = value; }

        public System.Type Type
        {
            get
            {
                try
                {
                    return TypeMatching[resourceType];
                }
                catch
                {
                    return typeof(Behaviour);
                }
            }
        }

        public Receiver(LocalizationTag localizationTag)
        {
            var resourceType = localizationTag.Resources[0]?.GetType();
            this.name = $"{localizationTag.Name} ({resourceType.Name})";
            this.id = localizationTag.ID;
            this.resourceType = resourceType.ToString();
        }

        private void SetLocalization(LocalizationTag localizationTag)
        {
            if(Receivers == null) { return; }

        }

        private void SetResource(string resource)
        {
            for (int i = 0; i < Receivers.Length; i++)
            {
                ((Text)Receivers[i]).text = resource;
            }
        }

        private void SetResource(Texture2D resource)
        {
            for (int i = 0; i < Receivers.Length; i++)
            {
                ((Image)Receivers[i]).sprite = Sprite.Create(resource, new Rect(0.0f, 0.0f, resource.width, resource.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }

        private void SetResource(AudioClip resource)
        {
            for (int i = 0; i < Receivers.Length; i++)
            {
                ((AudioSource)Receivers[i]).clip = resource;
            }
        }

        private void SetResource(VideoClip resource)
        {
            for (int i = 0; i < Receivers.Length; i++)
            {
                ((VideoPlayer)Receivers[i]).clip = resource;
            }
        }
    }
}
