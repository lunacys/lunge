﻿using System;

namespace lunge.Library.InputMgr
{
    /// <summary>
    /// Determines which mouse button currently is in use.
    /// </summary>
    [Obsolete("Use Nez instead")]
    public enum MouseButton
    {
        /// <summary>
        /// Left mouse button
        /// </summary>
        Left,
        /// <summary>
        /// Right mouse button
        /// </summary>
        Right,
        /// <summary>
        /// Middle mouse button
        /// </summary>
        Middle,
        /// <summary>
        /// First extra mouse button
        /// </summary>
        X1,
        /// <summary>
        /// Second extra mouse button
        /// </summary>
        X2
    }
}