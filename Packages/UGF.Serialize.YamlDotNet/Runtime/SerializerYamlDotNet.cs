using System;
using System.Threading.Tasks;
using UGF.RuntimeTools.Runtime.Contexts;
using UGF.Serialize.Runtime;
using UGF.Yaml.Runtime;
using Unity.Profiling;
using IYamlSerializer = YamlDotNet.Serialization.ISerializer;
using IYamlDeserializer = YamlDotNet.Serialization.IDeserializer;

namespace UGF.Serialize.YamlDotNet.Runtime
{
    public class SerializerYamlDotNet : SerializerAsync<string>
    {
        public IYamlSerializer Serializer { get; }
        public IYamlDeserializer Deserializer { get; }

        private static ProfilerMarker m_markerSerialize;
        private static ProfilerMarker m_markerDeserialize;

#if ENABLE_PROFILER
        static SerializerYamlDotNet()
        {
            m_markerSerialize = new ProfilerMarker("SerializerYamlDotNet.Serialize");
            m_markerDeserialize = new ProfilerMarker("SerializerYamlDotNet.Deserialize");
        }
#endif

        public SerializerYamlDotNet() : this(YamlUtility.DefaultSerializer, YamlUtility.DefaultDeserializer)
        {
        }

        public SerializerYamlDotNet(IYamlSerializer serializer, IYamlDeserializer deserializer)
        {
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            Deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        protected override object OnSerialize(object target, IContext context)
        {
            return InternalSerialize(target);
        }

        protected override object OnDeserialize(Type targetType, string data, IContext context)
        {
            return InternalDeserialize(targetType, data);
        }

        protected override Task<string> OnSerializeAsync(object target, IContext context)
        {
            return Task.Run(() => InternalSerialize(target));
        }

        protected override Task<object> OnDeserializeAsync(Type targetType, string data, IContext context)
        {
            return Task.Run(() => InternalDeserialize(targetType, data));
        }

        protected virtual string OnSerialize(object target)
        {
            return Serializer.Serialize(target);
        }

        protected virtual object OnDeserialize(Type targetType, string data)
        {
            return Deserializer.Deserialize(data, targetType);
        }

        private string InternalSerialize(object target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            m_markerSerialize.Begin();

            string result = OnSerialize(target);

            m_markerSerialize.End();

            return result;
        }

        private object InternalDeserialize(Type targetType, string data)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            if (data == null) throw new ArgumentNullException(nameof(data));

            m_markerDeserialize.Begin();

            object target = OnDeserialize(targetType, data);

            m_markerDeserialize.End();

            return target;
        }
    }
}
