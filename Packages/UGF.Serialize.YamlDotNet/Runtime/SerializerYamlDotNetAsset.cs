using UGF.Serialize.Runtime;
using UnityEngine;

namespace UGF.Serialize.YamlDotNet.Runtime
{
    [CreateAssetMenu(menuName = "Unity Game Framework/Serialize/Serializer YamlDotNet", order = 2000)]
    public class SerializerYamlDotNetAsset : SerializerAsset<string>
    {
        protected override ISerializer<string> OnBuildTyped()
        {
            return new SerializerYamlDotNet();
        }
    }
}
