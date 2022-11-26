using System.Collections.Generic;

namespace lunge.Library.Utils.Bitmasking;

/// <summary>
/// Maps image index to bitmask value.
/// Edges and Corners have 16 unique combinations.
/// Both have 256 unique combinations.
/// </summary>
public class BitMaskMapper
{
    private readonly Dictionary<int, int> _map;
    
    public BitMaskMapperDirection Directions { get; }

    public int this[int bitMaskValue] => GetIndex(bitMaskValue);

    public BitMaskMapper(BitMaskMapperDirection directions)
    {
        _map = new Dictionary<int, int>();

        Directions = directions;
    }

    public int GetIndex(int bitMaskValue)
        => _map[bitMaskValue];
}