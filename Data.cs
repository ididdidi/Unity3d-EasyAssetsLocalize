using UnityEngine;
using UnityEngine.Video;

namespace Localization
{
    [System.Serializable]
    public class Data
    {
        public enum Type { String, Texture, Audio, Video }
        [SerializeField] private Type type;
        [SerializeField] private string stringData;
        [SerializeField] private Texture2D texture;
        [SerializeField] private AudioClip audio;
        [SerializeField] private VideoClip video;

        public object DataObject { 
            get => GetDataObject(); 
            set => SetDataObject(value); 
        }

        public System.Type DataType
        {
            get => GetDataType();
        }

        public Data(Type type, object dataObject = null)
        {
            this.type = type;
            SetDataObject(dataObject);
        }

        private System.Type GetDataType()
        {
            switch (type)
            {
                case Type.String: return typeof(string);
                case Type.Texture: return typeof(Texture2D);
                case Type.Audio: return typeof(AudioClip);
                case Type.Video: return typeof(VideoClip);
                default: throw new System.NotImplementedException($"GetDataObject for: {type}");
            }
        }

        private object GetDataObject()
        {
            switch (type)
            {
                case Type.String: return stringData;
                case Type.Texture: return texture;
                case Type.Audio: return audio;
                case Type.Video: return video;
                default: throw new System.NotImplementedException($"GetDataObject for: {type}");
            }
        }

        private void SetDataObject(object dataObject)
        {
            switch (type)
            {
                case Type.String: stringData = (string)dataObject; break;
                case Type.Texture: texture = (Texture2D)dataObject; break;
                case Type.Audio: audio = (AudioClip)dataObject; break;
                case Type.Video: video = (VideoClip)dataObject; break;
                default: throw new System.NotImplementedException($"GetDataObject for: {type}");
            }
        }
    }
}