from random import randint

arr = []

n = int(input())

for i in range(1, n + 1):
    arr.append(i)

for i in range(0, n - 5):
    index = randint(i, i + 5)

    arr[i], arr[index] = arr[index], arr[i]

with open("result.txt", "w") as f:
    result = ""
    for i in arr:
        result += str(i) + " "
    
    f.write(result[:-1])