using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed result of asset destroy.
    /// </summary>
    [DataContract]
    public class DeletionResult : BaseResult
    {
        /// <summary>
        /// Result description.
        /// </summary>
        [DataMember(Name = "result")]
        public string Result { get; protected set; }
    }
}
