log("Rounding 10,000 random numbers");

for i = 1, 10000
do
    local num = math.random()
    math.floor(num)
end

local file = assert(io.open("ModBenchmarkCases/javascript.js", "r"))

local content = file:read("*all")
file:close()

log(content)