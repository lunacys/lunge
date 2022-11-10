import * as M from "./mod";

export class MyClass {
    public say(msg: string) {
        log("Saying: " + msg);
    }
}

const c = new MyClass();
c.say("sayin sdpofgjdfoihjsefogdfjo");

const mod = new M.Mod(1337);
mod.say();