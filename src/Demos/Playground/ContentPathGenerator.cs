

using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace Nez
{
    /// <summary>
    /// Class that contains the names of all of the files processed by the Pipeline Tool
    /// </summary>
    /// <remarks>
    /// Nez includes a T4 template that will auto-generate the content of this file.
    /// See: https://github.com/prime31/Nez/blob/master/FAQs/ContentManagement.md#auto-generating-content-paths"
    /// </remarks>
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    class Content
    {
		public static class Characters
		{
			public const string _1 = @"Content/characters/1.png";
			public const string _2 = @"Content/characters/2.png";
			public const string _3 = @"Content/characters/3.png";
			public const string _4 = @"Content/characters/4.png";
			public const string _5 = @"Content/characters/5.png";
			public const string _6 = @"Content/characters/6.png";
		}

		public static class Map
		{
			public const string Tilemap = @"Content/map/tilemap.tmx";
			public const string Tileset = @"Content/map/tileset.png";
		}

		public static class Steering
		{
			public const string GrayCircle = @"Content/steering/GrayCircle.png";
			public const string GreenSquare = @"Content/steering/GreenSquare.png";
			public const string PathPiece = @"Content/steering/PathPiece.png";
			public const string RedSquare = @"Content/steering/RedSquare.png";
			public const string Target = @"Content/steering/Target.png";
			public const string TestNinePatch = @"Content/steering/TestNinePatch.png";
			public const string YellowArrow = @"Content/steering/YellowArrow.png";
		}

		public static class Tiles
		{
			public const string Testtileset = @"Content/tiles/test-tileset.png";
			public const string Testtileset4bit = @"Content/tiles/test-tileset-4bit.png";
			public const string Testtileset8bit = @"Content/tiles/test-tileset-8bit.png";
			public const string Wall1tileset = @"Content/tiles/wall-1-tileset.png";
		}

		public const string Bg = @"Users/fdctech/source/lunge/src/Demos/Playground/Content/bg.png";
		public const string Plume = @"Users/fdctech/source/lunge/src/Demos/Playground/Content/plume.tga";
		public const string Spritelight = @"Users/fdctech/source/lunge/src/Demos/Playground/Content/sprite-light.png";

    }
}

