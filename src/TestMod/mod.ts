export class Mod {

    constructor(private myNum: number) { }

    public say() {
        log('we are saying number ' + this.myNum);
    }
}