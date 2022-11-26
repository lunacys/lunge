using System;

namespace lunge.Library.Utils.Bitmasking;

/// <summary>
/// Used for Bit Mask Mapping.
/// You can bitwise OR it in order to select multiple edges and/or corners.
/// </summary>
[Flags]
public enum BitMaskMapperDirection
{
    None = 0,
    /// <summary>
    /// Top edge
    /// </summary>
    Top = 1 << 0,
    /// <summary>
    /// Right edge
    /// </summary>
    Right = 1 << 1,
    /// <summary>
    /// Bottom edge
    /// </summary>
    Bottom = 1 << 2,
    /// <summary>
    ///  Left edge
    /// </summary>
    Left = 1 << 3,
    
    /// <summary>
    /// Top-right corner
    /// </summary>
    TopRight = 1 << 4,
    /// <summary>
    /// Bottom-right corner
    /// </summary>
    BottomRight = 1 << 5,
    /// <summary>
    /// Bottom-left corner
    /// </summary>
    BottomLeft = 1 << 6,
    /// <summary>
    /// Top-left corner
    /// </summary>
    TopLeft = 1 << 7
}