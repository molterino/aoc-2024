const string path = "input.txt";
const string separator = "   ";

var locationsGroupA = new List<int>();
var locationsGroupB = new List<int>();

try
{
    foreach (var line in File.ReadAllLines(path))
    {
        var locationIds = line.Split(separator);

        locationsGroupA.Add(int.Parse(locationIds[0]));
        locationsGroupB.Add(int.Parse(locationIds[1]));
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error during file processing: {e.Message}");
}

locationsGroupA.Sort();
locationsGroupB.Sort();

int distance = 0;

for (int i = 0; i < locationsGroupA.Count; i++)
{
    distance += Math.Abs(locationsGroupA[i] - locationsGroupB[i]);
}

Console.WriteLine($"Total distance: {distance}");