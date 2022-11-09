using lunge.Library.Scripting.TsDeclarations.Api.Common;
using Nez.Systems;

namespace lunge.Library.Scripting.TsDeclarations.Api.Nez;

[TsInterface(typeof(NezContentManager), "ContentManager")]
public class TsNezContentManager
{
    [TsExclude] internal NezContentManager OrigContent { get; set; } = null!;

    internal TsNezContentManager(NezContentManager content)
    {
        OrigContent = content;
    }

    public TsTexture2D LoadTexture(string name, bool premultiplyAlpha) => OrigContent.LoadTexture(name, premultiplyAlpha);
}