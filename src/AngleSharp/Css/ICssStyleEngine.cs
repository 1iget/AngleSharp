﻿namespace AngleSharp.Css
{
    using AngleSharp.Css.Dom;
    using System;

    /// <summary>
    /// Specializes the API for a CSS style engine.
    /// </summary>
    public interface ICssStyleEngine : IStylingService
    {
        /// <summary>
        /// Gets the default CSS stylesheet.
        /// </summary>
        ICssStyleSheet Default { get; }

        /// <summary>
        /// Creates a style declaration for the given source.
        /// </summary>
        /// <param name="source">
        /// The source code for the inline style declaration.
        /// </param>
        /// <param name="options">
        /// The options with the parameters for evaluating the style.
        /// </param>
        /// <returns>The created style declaration.</returns>
        ICssStyleDeclaration ParseDeclaration(String source, StyleOptions options);
    }
}
