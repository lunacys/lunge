using System;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Playground.Components.WorldGeneration;

public class WorldGeneratorSettings
{
    public int MaxRooms = 100;
    public int MinUsedArea = 100000000;

    public int WorldWidth = 256;
    public int WorldHeight = 256;

    public int InitialMinRoomWidth = 16;
    public int InitialMinRoomHeight = 16;
    public int InitialMaxRoomWidth = 32;
    public int InitialMaxRoomHeight = 32;
    
    public int NextMinRoomWidth = 8;
    public int NextMinRoomHeight = 8;
    public int NextMaxRoomWidth = 32;
    public int NextMaxRoomHeight = 32;

    public int NextStepRoomGenerationIterations = 4000;
    public int MaxRoomGenIterations = 400000;

    private System.Numerics.Vector2 _roomSpacing = new System.Numerics.Vector2(4, 4);

    public Vector2 RoomSpacing
    {
        get => new Vector2(_roomSpacing.X, _roomSpacing.Y);
        set => _roomSpacing = new System.Numerics.Vector2(value.X, value.Y);
    }

    public float RandomEdgeInclusionChance = 0.125f;

    public int CostEmptySpace = 5;
    public int CostRoom = 1;
    public int CostHallway = 2;

    private Action _generateAction;

    public WorldGeneratorSettings(Action generateAction)
    {
        _generateAction = generateAction;
    }

    public void ImGuiDraw()
    {
        ImGui.Begin("Generator Settings");

        ImGui.SliderInt("Max Rooms", ref MaxRooms, 2, 200);
        ImGui.SliderInt("Min Filled Area", ref MinUsedArea, 2, 200);
        ImGui.Separator();

        ImGui.SliderInt("World Width (tiles)", ref WorldWidth, 16, 512);
        ImGui.SliderInt("World Height (tiles)", ref WorldHeight, 16, 512);
        ImGui.Separator();

        ImGui.SliderInt("Initial Min Room Width (tiles)", ref InitialMinRoomWidth, 1, 32);
        ImGui.SliderInt("Initial Min Room Height (tiles)", ref InitialMinRoomHeight, 1, 32);
        ImGui.SliderInt("Initial Max Room Width (tiles)", ref InitialMaxRoomWidth, 1, 32);
        ImGui.SliderInt("Initial Max Room Height (tiles)", ref InitialMaxRoomHeight, 1, 32);

        ImGui.SliderInt("Next MinRoom Width (tiles)", ref NextMinRoomWidth, 2, 200);
        ImGui.SliderInt("Next MinRoom Height (tiles)", ref NextMinRoomHeight, 2, 200);
        ImGui.SliderInt("Next MaxRoom Width (tiles)", ref NextMaxRoomWidth, 2, 200);
        ImGui.SliderInt("Next MaxRoom Height (tiles)", ref NextMaxRoomHeight, 2, 200);
        ImGui.Separator();

        ImGui.SliderInt("Next Step Room Generation Iterations", ref NextStepRoomGenerationIterations, 2, 200);
        ImGui.SliderInt("Max Room Gen Iterations", ref MaxRoomGenIterations, 2, 200);
        ImGui.Separator();

        ImGui.SliderFloat2("Max Room Spacing (tiles)", ref _roomSpacing, 0, 16);
        ImGui.Separator();
        ImGui.SliderFloat("Random Edge Inclusion Chance", ref RandomEdgeInclusionChance, 0, 1f);
        ImGui.SliderInt("Empty Space Cost", ref CostEmptySpace, 0, 50);
        ImGui.SliderInt("Room Cost", ref CostRoom, 0, 50);
        ImGui.SliderInt("Hallway Cost", ref CostHallway, 0, 50);

        if (ImGui.Button("Generate New World"))
        {
            _generateAction();
        }

        ImGui.End();
    }
}