using System;
using System.Collections.Generic;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Base abstract non-generic class for creating Layers.
    /// </summary>
    public abstract class BaseLayer : Core.ICloneable
    {
        /// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
    }

    /// <summary>
    /// Base abstract generic class for creating Layers.
    /// </summary>
    public abstract class BaseLayer<T> : BaseLayer where T : BaseLayer<T>
    {
        /// <summary>
        /// The type of the resource.
        /// </summary>
        protected string m_resourceType;

        /// <summary>
        /// The specific type of the asset. Valid values: upload, private and authenticated. Default: upload.
        /// </summary>
        protected string m_type;

        /// <summary>
        /// The identifier of the uploaded asset. 
        /// </summary>
        protected string m_publicId;

        /// <summary>
        /// The resource format.
        /// </summary>
        protected string m_format;

        /// <summary>
        /// The type of the resource. Valid values: image, raw, and video.
        /// </summary>
        public T ResourceType(string resourceType)
        {
            m_resourceType = resourceType;
            return (T)this;
        }

        /// <summary>
        /// The specific type of the asset. Valid values: upload, private and authenticated. Default: upload.
        /// </summary>
        public T Type(string type)
        {
            this.m_type = type;
            return (T)this;
        }

        /// <summary>
        /// Set the public ID of a previously uploaded PNG image to add as overlay.
        /// </summary>
        /// <param name="publicId">Public ID.</param>
        public T PublicId(string publicId)
        {
            this.m_publicId = publicId.Replace('/', ':');
            return (T)this;
        }

        /// <summary>
        /// Set a 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public T Format(string format)
        {
            this.m_format = format;
            return (T)this;
        }

        /// <summary>
        /// Get an additional parameters for the layer.
        /// </summary>
        public virtual string AdditionalParams()
        {
            return string.Empty;
        }

        /// <summary>
        /// Get this layer represented as string.
        /// </summary>
        /// <returns>The layer represented as string.</returns>
        public override string ToString()
        {
            List<string> components = new List<string>();
            if (!string.IsNullOrEmpty(m_resourceType) && !m_resourceType.Equals("image"))
            {
                components.Add(m_resourceType);
            }

            if (!string.IsNullOrEmpty(m_type) && !m_type.Equals("upload"))
            {
                components.Add(m_type);
            }

            string additionalParams = this.AdditionalParams();

            if (!string.IsNullOrEmpty(additionalParams))
            {
                components.Add(additionalParams);
            }

            if (!string.IsNullOrEmpty(m_publicId))
            {
                components.Add(FormattedPublicId());
            }

            return string.Join(":", components.ToArray());
        }

        private string FormattedPublicId()
        {
            var transientPublicId = m_publicId;

            if (!string.IsNullOrEmpty(m_format))
            {
                transientPublicId = string.Format("{0}.{1}", transientPublicId, m_format);
            }

            return transientPublicId;
        }

        #region ICloneable

        /// <summary>
        /// Create a shallow copy of the current object.
        /// </summary>
        /// <returns>A new instance of the current object.</returns>
        public override object Clone() => MemberwiseClone();

        #endregion
    }
}
