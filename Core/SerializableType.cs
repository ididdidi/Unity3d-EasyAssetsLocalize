using System.IO;
using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Class for Class for serializing <see cref="System.Type"/>
    /// </summary>
    [System.Serializable]
    internal class SerializableType : ISerializationCallbackReceiver
    {
        public System.Type type;
        public byte[] data;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">Instance <see cref="System.Type"/></param>
        public SerializableType(System.Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Method for writing serialized data to memory.
        /// </summary>
        /// <param name="writer"><see cref="BinaryWriter"/></param>
        /// <param name="type">Instance <see cref="System.Type"/></param>
        public static void Write(BinaryWriter writer, System.Type type)
        {
            if (type == null)
            {
                writer.Write((byte)0xFF);
                return;
            }
            if (type.IsGenericType)
            {
                var t = type.GetGenericTypeDefinition();
                var p = type.GetGenericArguments();
                writer.Write((byte)p.Length);
                writer.Write(t.AssemblyQualifiedName);
                for (int i = 0; i < p.Length; i++)
                {
                    Write(writer, p[i]);
                }
                return;
            }
            writer.Write((byte)0);
            writer.Write(type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Method for reading serialized data from memory.
        /// </summary>
        /// <param name="reader"><see cref="BinaryWriter"/></param>
        /// <returns></returns>
        public static System.Type Read(BinaryReader reader)
        {
            var paramCount = reader.ReadByte();
            if (paramCount == 0xFF)
                return null;
            var typeName = reader.ReadString();
            var type = System.Type.GetType(typeName);
            if (type == null)
                throw new System.Exception("Can't find type; '" + typeName + "'");
            if (type.IsGenericTypeDefinition && paramCount > 0)
            {
                var p = new System.Type[paramCount];
                for (int i = 0; i < paramCount; i++)
                {
                    p[i] = Read(reader);
                }
                type = type.MakeGenericType(p);
            }
            return type;
        }

        /// <summary>
        /// Method called before serialization.
        /// </summary>
        public void OnBeforeSerialize()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                Write(writer, type);
                data = stream.ToArray();
            }
        }

        /// <summary>
        /// Method called after deserialize.
        /// </summary>
        public void OnAfterDeserialize()
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream)) 
            {
                type = Read(reader);
            }
        }
    }
}