path = "input.txt"
delimiter = "   "

locationsA = []
locationsB = []

with open(path) as file:
    for line in file:
        locations = line.split(delimiter)
        locationsA.append(int(locations[0]))
        locationsB.append(int(locations[1]))

locationsA.sort()
locationsB.sort()

distance = 0
similarity = 0

for a, b in zip(locationsA, locationsB):
    distance += (abs(a - b))
    similarity += a * locationsB.count(a)

print ("Total distance (part 1): ", distance)
print ("Similarity (part 2): ", similarity)