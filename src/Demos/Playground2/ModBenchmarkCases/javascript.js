log("Rounding 10,000 random numbers");

for (var i = 0; i <= 10000; i++) {
	var num = Math.random();
	Math.round(num);
}

log("Ack(3,8)")

function Ack(M, N){
    if (M == 0)
        return N + 1
    if (N == 0)
        return Ack(M - 1, 1)
    return Ack(M - 1, Ack(M, (N - 1)))
}

var N = 3;
var M = 8;
Ack(N,M);