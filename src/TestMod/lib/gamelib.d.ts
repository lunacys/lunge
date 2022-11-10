/**
 * Writes an input into the console
 * @param msg message
 */
declare function log(msg: any): void;

/**
 * Imports a file and immediately evaluates it.
 * Every required file implicitly has a "return exports" statement
 */
//declare function require(path: string): any; // NOTE: doesn't need to be called from TypeScript

declare interface Vector2 {
    X: number;
    Y: number;
}

declare interface TestMG {
    GetMousePosition(): Vector2;

    GetTitle(): string;
    SetTitle(value: string);
}

declare var mg: TestMG;