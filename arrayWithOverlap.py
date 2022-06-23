from random import randint

arr = []

n = int(input())

m = n // 10
k = n % 10

for i in range(1, 10):
    for j in range(m):
        arr.append(m * i)

for i in range(m + k):
    arr.append(m * 10)

with open("result.txt", "w") as f:
    result = ""
    for i in arr:
        result += str(i) + " "
    
    f.write(result[:-1])