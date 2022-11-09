/**
 * Writes an input into the console
 * @param msg message
 */
declare function log(msg: any): void;

declare class Color {
    R: number;
    G: number;
    B: number;
    A: number;

    constructor(r: number, g: number, b: number, a: number);
}

declare interface Skin {

}

declare interface Batcher {

}

declare class ScrollPane {
    constructor(table: Table, skin: Skin);

    SetPosition(x: number, y: number): ScrollPane;
    Validate(): ScrollPane;
    GetScrollBarWidth(): number;
    SetSize(width: number, height: number): ScrollPane;
}

declare interface Stage {
    AddElement<T>(element: T): T;
}

declare interface UICanvas {
    Stage: Stage;
}

declare interface Screen {
    Width: number;
    Height: number;
}

declare interface IDrawable {
    LeftWidth;
    RightWidth;
    TopHeight;
    BottomHeight;
    MinWidth;
    MinHeight;


    SetPadding(top: number, bottom: number, left: number, right: number): void;
    Draw(batcher: Batcher, x: number, y: number, width: number, height: number, color: Color): void;
}

declare enum Align {
    Center = 1 << 0,
    Top = 1 << 1,
    Bottom = 1 << 2,
    Left = 1 << 3,
    Right = 1 << 4,

    TopLeft = Top | Left,
    TopRight = Top | Right,
    BottomLeft = Bottom | Left,
    BottomRight = Bottom | Right
}

declare class Cell {
    SetPadTop(value: number): Cell;
    SetPadRight(value: number): Cell;
    SetPadLeft(value: number): Cell;
    SetPadBottom(value: number): Cell;

    SetAlign(align: Align): Cell;
}

declare class PrimitiveDrawable implements IDrawable {
    constructor(color: Color);

    LeftWidth;
    RightWidth;
    TopHeight;
    BottomHeight;
    MinWidth;
    MinHeight;


    SetPadding(top: number, bottom: number, left: number, right: number): void;
    Draw(batcher: Batcher, x: number, y: number, width: number, height: number, color: Color): void;
}

declare class Table {
    Left(): Table;
    Right(): Table;
    Top(): Table;
    Bottom(): Table;

    PadTop(value: number): Table;
    PadLeft(value: number): Table;
    PadRight(value: number): Table;
    PadBottom(value: number): Table;

    SetFillParent(fillParent: boolean): Table;

    Defaults(): Cell;

    SetBackground(drawable: IDrawable);
}

declare var UiCanvas: UICanvas;
declare var Screen: Screen;
declare var Skin: Skin;