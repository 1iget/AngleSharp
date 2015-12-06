﻿namespace AngleSharp.Dom.Html
{
    using AngleSharp.Extensions;
    using AngleSharp.Html;
    using AngleSharp.Network;
    using AngleSharp.Services.Media;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the HTML object element.
    /// </summary>
    sealed class HtmlObjectElement : HtmlFormControlElement, IHtmlObjectElement
    {
        #region Fields

        IObjectInfo _obj;
        IDownload _download;

        #endregion

        #region ctor

        public HtmlObjectElement(Document owner, String prefix = null)
            : base(owner, Tags.Object, prefix, NodeFlags.Scoped)
        {
            RegisterAttributeObserver(AttributeNames.Data, UpdateSource);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the address of the resource.
        /// </summary>
        public String Source
        {
            get { return this.GetUrlAttribute(AttributeNames.Data); }
            set { this.SetOwnAttribute(AttributeNames.Data, value); }
        }

        /// <summary>
        /// Gets or sets the type of the resource. If present, the attribute
        /// must be a valid MIME type.
        /// </summary>
        public String Type
        {
            get { return this.GetOwnAttribute(AttributeNames.Type); }
            set { this.SetOwnAttribute(AttributeNames.Type, value); }
        }

        /// <summary>
        /// Gets or sets an attribute whose presence indicates that the
        /// resource specified by the data attribute is only to be used if the
        /// value of the type attribute and the Content-Type of the
        /// aforementioned resource match.
        /// </summary>
        public Boolean TypeMustMatch
        {
            get { return this.HasOwnAttribute(AttributeNames.TypeMustMatch); }
            set { this.SetOwnAttribute(AttributeNames.TypeMustMatch, value ? String.Empty : null); }
        }

        /// <summary>
        /// Gets or sets the associated image map of the object if the object
        /// element represents an image.
        /// </summary>
        public String UseMap
        {
            get { return this.GetOwnAttribute(AttributeNames.UseMap); }
            set { this.SetOwnAttribute(AttributeNames.UseMap, value); }
        }

        /// <summary>
        /// Gets or sets the width of the object element.
        /// </summary>
        public Int32 DisplayWidth
        {
            get { return this.GetOwnAttribute(AttributeNames.Width).ToInteger(OriginalWidth); }
            set { this.SetOwnAttribute(AttributeNames.Width, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the height of the object element.
        /// </summary>
        public Int32 DisplayHeight
        {
            get { return this.GetOwnAttribute(AttributeNames.Height).ToInteger(OriginalHeight); }
            set { this.SetOwnAttribute(AttributeNames.Height, value.ToString()); }
        }

        /// <summary>
        /// Gets the original width of the object.
        /// </summary>
        public Int32 OriginalWidth
        {
            get { return _obj != null ? _obj.Width : 0; }
        }

        /// <summary>
        /// Gets the original height of the object.
        /// </summary>
        public Int32 OriginalHeight
        {
            get { return _obj != null ? _obj.Height : 0; }
        }

        /// <summary>
        /// Gets the active document of the object element's nested browsing
        /// context, if it has one; otherwise returns null.
        /// </summary>
        public IDocument ContentDocument
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the object element's nested browsing context, if it has one;
        /// otherwise returns null.
        /// </summary>
        public IWindow ContentWindow
        {
            get { return null; }
        }

        #endregion

        #region Methods

        protected override Boolean CanBeValidated()
        {
            return false;
        }

        #endregion

        #region Helpers

        void UpdateSource(String value)
        {
            if (_download != null && !_download.IsCompleted)
            {
                _download.Cancel();
            }

            if (!String.IsNullOrEmpty(value))
            {
                var loader = Owner.Loader;

                if (loader != null)
                {
                    var url = new Url(Source);
                    var request = this.CreateRequestFor(url);
                    var download = loader.DownloadAsync(request);
                    var task = this.ProcessResource<IObjectInfo>(_download, result => _obj = result);
                    _download = download;
                }
            }
        }

        #endregion
    }
}
