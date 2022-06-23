from random import randint

arr = []

n = int(input())

for i in range(n, 0, -1):
    arr.append(i)

with open("result.txt", "w") as f:
    result = ""
    for i in arr:
        result += str(i) + " "
    
    f.write(result[:-1])