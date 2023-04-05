using UnityEngine;
using UnityEngine.Video;

namespace Localization
{
    [System.Serializable]
    public class Data
    {
        private enum Type { String, Texture, Audio, Video }
        [SerializeField] private Type type;
        [SerializeField] private string stringData;
        [SerializeField] private Texture2D texture;
        [SerializeField] private AudioClip audio;
        [SerializeField] private VideoClip video;

        public object DataObject { 
            get => GetDataObject(); 
            set => SetDataObject(value); 
        }

        public Data(object dataObject)
        {
            SetDataObject(dataObject);
        }

        private object GetDataObject()
        {
            switch (type)
            {
                case Type.String: return stringData;
                case Type.Texture: return texture;
                default: return null;
            }
        }

        private void SetDataObject(object dataObject)
        {
            if (dataObject is string)
            {
                type = Type.String;
                stringData = (string)dataObject;
            }
            else if (dataObject is Texture2D)
            {
                type = Type.Texture;
                texture = (Texture2D)dataObject;
            }
        }
    }
}