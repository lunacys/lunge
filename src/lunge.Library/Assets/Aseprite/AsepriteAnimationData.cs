using Nez.Persistence;

namespace lunge.Library.Assets.Aseprite;

public class AsepriteAnimationData
{
    [DecodeAlias("frames")]
    [JsonInclude] public AsepriteFrame[] Frames { get; set; } = null!;
    [DecodeAlias("meta")]
    [JsonInclude] public AsepriteMetadata Meta { get; set; } = null!;
}

public class AsepriteFrame
{
    [DecodeAlias("filename")]
    [JsonInclude] public string Filename { get; set; } = null!;
    [DecodeAlias("frame")]
    [JsonInclude]
    public AsepriteRectangle Frame { get; set; }
    [DecodeAlias("rotated")]
    [JsonInclude]
    public bool Rotated { get; set; }
    [DecodeAlias("trimmed")]
    [JsonInclude]
    public bool Trimmed { get; set; }
    [DecodeAlias("spriteSourceSize")]
    [JsonInclude]
    public AsepriteRectangle SpriteSourceSize { get; set; }
    [DecodeAlias("sourceSize")]
    [JsonInclude]
    public AsepriteSize SourceSize { get; set; }
    [DecodeAlias("duration")]
    [JsonInclude]
    public int Duration { get; set; }
}

public struct AsepriteRectangle
{
    [DecodeAlias("x")]
    [JsonInclude] public int X { get; set; }
    [DecodeAlias("y")]
    [JsonInclude] public int Y { get; set; }
    [DecodeAlias("w")]
    [JsonInclude] public int W { get; set; }
    [DecodeAlias("h")]
    [JsonInclude] public int H { get; set; }
}

public class AsepriteMetadata
{
    [DecodeAlias("app")]
    [JsonInclude] public string App { get; set; } = null!;
    [DecodeAlias("version")]
    [JsonInclude] public string Version { get; set; } = null!;
    [DecodeAlias("image")]
    [JsonInclude] public string Image { get; set; } = null!;
    [DecodeAlias("format")]
    [JsonInclude] public string Format { get; set; } = null!;
    [DecodeAlias("size")]
    [JsonInclude] public AsepriteSize Size { get; set; }
    [DecodeAlias("scale")]
    [JsonInclude] public string Scale { get; set; } = null!;
    [DecodeAlias("frameTags")]
    [JsonInclude] public AsepriteFrameTag[] FrameTags { get; set; } = null!;
    [DecodeAlias("layers")]
    [JsonInclude] public AsepriteLayer[] Layers { get; set; } = null!;
    [DecodeAlias("slices")]
    [JsonInclude] public AsepriteSlice[] Slices { get; set; } = null!;
}

public struct AsepriteSize
{
    [DecodeAlias("w")]
    [JsonInclude] public int W { get; set; }
    [DecodeAlias("h")]
    [JsonInclude] public int H { get; set; }
}

public class AsepriteFrameTag
{
    [DecodeAlias("name")]
    [JsonInclude] public string Name { get; set; } = null!;
    [DecodeAlias("from")]
    [JsonInclude] public int From { get; set; }
    [DecodeAlias("to")]
    [JsonInclude] public int To { get; set; }
    [DecodeAlias("direction")]
    [JsonInclude] public string Direction { get; set; } = null!;
}

public class AsepriteLayer
{
    [DecodeAlias("name")]
    [JsonInclude] public string Name { get; set; } = null!;
    [DecodeAlias("opacity")]
    [JsonInclude] public int Opacity { get; set; }
    [DecodeAlias("blendMode")]
    [JsonInclude] public string BlendMode { get; set; } = null!;
}

public class AsepriteSlice
{

}