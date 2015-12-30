﻿namespace AngleSharp.Dom.Html
{
    using AngleSharp.Dom.Collections;
    using AngleSharp.Extensions;
    using AngleSharp.Html;
    using System;

    /// <summary>
    /// Represents an HTML output element.
    /// </summary>
    sealed class HtmlOutputElement : HtmlFormControlElement, IHtmlOutputElement
    {
        #region Fields

        String _defaultValue;
        String _value;
        SettableTokenList _for;

        #endregion

        #region ctor

        public HtmlOutputElement(Document owner, String prefix = null)
            : base(owner, TagNames.Output, prefix)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default value of the element, initially the empty
        /// string.
        /// </summary>
        public String DefaultValue
        {
            get { return _defaultValue ?? TextContent; }
            set { _defaultValue = value; }
        }

        /// <summary>
        /// Gets or sets the text content of a node and its descendants.
        /// </summary>
        public override String TextContent
        {
            get { return _value ?? _defaultValue ?? base.TextContent; }
            set { base.TextContent = value; }
        }

        /// <summary>
        /// Gets or sets the value of the contents of the elements.
        /// </summary>
        public String Value
        {
            get { return TextContent; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets the IDs of the labeled control. Reflects the for attribute.
        /// </summary>
        public ISettableTokenList HtmlFor
        {
            get
            { 
                if (_for == null)
                {
                    _for = new SettableTokenList(this.GetOwnAttribute(AttributeNames.For));
                    CreateBindings(_for, AttributeNames.For);
                }

                return _for; 
            }
        }

        /// <summary>
        /// Gets the type of input control (output).
        /// </summary>
        public String Type
        {
            get { return TagNames.Output; }
        }

        #endregion

        #region Helpers

        protected override Boolean CanBeValidated()
        {
            return true;
        }

        /// <summary>
        /// Resets the form control to its initial value.
        /// </summary>
        internal override void Reset()
        {
            _value = null;
        }

        #endregion
    }
}
