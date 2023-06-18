using UnityEngine;
using UnityEngine.Video;

namespace ResourceLocalization
{
    public static class ReceiverCreator
    {
     //   public static LocalizationReceiver CreateReceiver(this LocalizationTag tag)
     //   {
     //       var data = tag?.Resources[0].Data;
     //       
     //       if (data is string) { return new TextReceiver(tag); }
     //       else if (data is Texture2D) { return new ImageReceiver(tag); }
     //       else if (data is Sprite) { return new ImageReceiver(tag); }
     //       else if (data is AudioClip) { return new AudioReceiver(tag); }
     //       else if (data is VideoClip) { return new VideoReceiver(tag); }
     //       else { throw new System.ArgumentException($"Resource type {data.GetType()} has no receiver assigned"); }
     //   }
    }
}
