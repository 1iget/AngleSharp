﻿namespace AngleSharp.Dom.Html
{
    using AngleSharp.Extensions;
    using AngleSharp.Html;
    using AngleSharp.Network;
    using AngleSharp.Services.Media;
    using System;

    /// <summary>
    /// Represents the image element.
    /// </summary>
    sealed class HtmlImageElement : HtmlElement, IHtmlImageElement
    {
        #region Fields

        IImageInfo _img;
        IDownload _download;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new image element.
        /// </summary>
        public HtmlImageElement(Document owner, String prefix = null)
            : base(owner, Tags.Img, prefix, NodeFlags.Special | NodeFlags.SelfClosing)
        {
            RegisterAttributeObserver(AttributeNames.Src, UpdateSource);
            RegisterAttributeObserver(AttributeNames.SrcSet, UpdateSource);
            RegisterAttributeObserver(AttributeNames.Sizes, UpdateSource);
            RegisterAttributeObserver(AttributeNames.CrossOrigin, UpdateSource);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the actual used image source.
        /// </summary>
        public String ActualSource
        {
            get { return _img != null && _img.Source != null ? _img.Source.Href : String.Empty; }
        }

        /// <summary>
        /// Gets or sets the image candidates for higher density images.
        /// </summary>
        public String SourceSet
        {
            get { return this.GetOwnAttribute(AttributeNames.SrcSet); }
            set { this.SetOwnAttribute(AttributeNames.SrcSet, value); }
        }

        /// <summary>
        /// Gets or sets the sizes to responsively.
        /// </summary>
        public String Sizes
        {
            get { return this.GetOwnAttribute(AttributeNames.Sizes); }
            set { this.SetOwnAttribute(AttributeNames.Sizes, value); }
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public String Source
        {
            get { return this.GetUrlAttribute(AttributeNames.Src); }
            set { this.SetOwnAttribute(AttributeNames.Src, value); }
        }

        /// <summary>
        /// Gets or sets the alternative text.
        /// </summary>
        public String AlternativeText
        {
            get { return this.GetOwnAttribute(AttributeNames.Alt); }
            set { this.SetOwnAttribute(AttributeNames.Alt, value); }
        }

        /// <summary>
        /// Gets or sets the cross-origin attribute.
        /// </summary>
        public String CrossOrigin
        {
            get { return this.GetOwnAttribute(AttributeNames.CrossOrigin); }
            set { this.SetOwnAttribute(AttributeNames.CrossOrigin, value); }
        }

        /// <summary>
        /// Gets or sets the usemap attribute, which indicates that the image
        /// has an associated image map.
        /// </summary>
        public String UseMap
        {
            get { return this.GetOwnAttribute(AttributeNames.UseMap); }
            set { this.SetOwnAttribute(AttributeNames.UseMap, value); }
        }

        /// <summary>
        /// Gets or sets the displayed width of the image element.
        /// </summary>
        public Int32 DisplayWidth
        {
            get { return this.GetOwnAttribute(AttributeNames.Width).ToInteger(OriginalWidth); }
            set { this.SetOwnAttribute(AttributeNames.Width, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the displayed height of the image element.
        /// </summary>
        public Int32 DisplayHeight
        {
            get { return this.GetOwnAttribute(AttributeNames.Height).ToInteger(OriginalHeight); }
            set { this.SetOwnAttribute(AttributeNames.Height, value.ToString()); }
        }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public Int32 OriginalWidth
        {
            get { return IsCompleted ? _img.Width : 0; }
        }

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        public Int32 OriginalHeight
        {
            get { return IsCompleted ? _img.Height : 0; }
        }

        /// <summary>
        /// Gets if the image is completely available.
        /// </summary>
        public Boolean IsCompleted
        {
            get { return _img != null; }
        }

        /// <summary>
        /// Gets or sets if the image element is a map. The attribute must not
        /// be specified on an element that does not have an ancestor a element
        /// with an href attribute.
        /// </summary>
        public Boolean IsMap
        {
            get { return this.HasOwnAttribute(AttributeNames.IsMap); }
            set { this.SetOwnAttribute(AttributeNames.IsMap, value ? String.Empty : null); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// For more information, see:
        /// http://www.w3.org/html/wg/drafts/html/master/embedded-content.html#update-the-image-data
        /// </summary>
        void GetImage(Url source)
        {
            if (source.IsInvalid)
            {
                source = null;
            }
            else if (_img != null && source.Equals(_img.Source))
            {
                return;
            }

            if (_download != null && !_download.IsCompleted)
            {
                _download.Cancel();
            }

            var document = Owner;

            if (source != null && document != null)
            {
                var loader = document.Loader;

                if (loader != null)
                {
                    var request = this.CreateRequestFor(source);
                    var download = loader.DownloadAsync(request);
                    var task = this.ProcessResource<IImageInfo>(download, result => _img = result);
                    document.DelayLoad(task);
                    _download = download;
                }
            }
        }

        void UpdateSource(String value)
        {
            var candidate = this.GetImageCandidate();
            GetImage(candidate);
        }

        #endregion
    }
}
