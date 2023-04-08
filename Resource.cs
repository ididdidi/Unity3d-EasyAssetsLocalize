using UnityEngine;
using UnityEngine.Video;

namespace Localization
{
    [System.Serializable]
    public class Resource
    {
        [SerializeField] private string tag;
        private enum Type { String, Texture, Audio, Video }
        [SerializeField] private Type type;
        [SerializeField] private string stringData;
        [SerializeField] private Texture2D texture;
        [SerializeField] private AudioClip audio;
        [SerializeField] private VideoClip video;

        public string Tag { get => tag; }
        public object Data
        {
            get => GetDataObject();
            set => SetDataObject(value);
        }

        public System.Type ObjectType
        {
            get => GetSystemType();
        }

        public Resource(string tag, object dataObject)
        {
            this.tag = tag;
            this.type = GetDataType(dataObject.GetType());
            SetDataObject(dataObject);
        }

        public Resource(string tag, System.Type type)
        {
            this.tag = tag;
            this.type = GetDataType(type);
        }

        private System.Type GetSystemType()
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

        private Type GetDataType(System.Type type)
        {
            if (type.IsAssignableFrom(typeof(string))) { return Type.String; }
            else if (type.IsAssignableFrom(typeof(Texture2D))) { return Type.Texture; }
            else if (type.IsAssignableFrom(typeof(AudioClip))) { return Type.Audio; }
            else if (type.IsAssignableFrom(typeof(VideoClip))) { return Type.Video; }
            else { throw new System.NotImplementedException($"GetDataObject for: {type}"); }
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
                case Type.String: stringData = (dataObject != null) ? (string)dataObject : ""; break;
                case Type.Texture: texture = (Texture2D)dataObject; break;
                case Type.Audio: audio = (AudioClip)dataObject; break;
                case Type.Video: video = (VideoClip)dataObject; break;
                default: throw new System.NotImplementedException($"GetDataObject for: {type}");
            }
        }
    }
}