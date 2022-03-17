var plik = System.IO.File.ReadLines("distances-pl.txt"); 
string startingCity = "BielskoBiala";

string test = System.Text.RegularExpressions.Regex.Replace(plik.ToList()[0], @"\s+", " "); //zamień milion spacji na jedną, łatwiej na split

string[] cityNames = new string[plik.ToList().Count];
int[][] distancesBetweenCities = new int[plik.ToList().Count][];
for (int i = 0; i < plik.ToList().Count; i++) {
    distancesBetweenCities[i] = new int[test.Split(" ").Length - 1];
}

int index = 0;
foreach (var line in plik) {
    string[] temp = (System.Text.RegularExpressions.Regex.Replace(line, @"\s+", " ")).Split(" ");
    cityNames[index] = temp[0]; //wpisane nazwy miast do osobnej tablicy
    for (int i = 1; i < temp.Length; i++) {
        distancesBetweenCities[index][i - 1] =
            int.Parse(temp[i]); //uzupełniona tablica dystansu pomiędzy miastami
    }
    index++;
}

int[] indexOfVisitedCities = new int[plik.ToList().Count];

//find startingCity
for (int i = 0; i < cityNames.Length; i++) {
    if (cityNames[i] == startingCity) {
        indexOfVisitedCities[0] = i;
        break;
    }
}

Console.WriteLine("Droga z Bielska-Białej:"); //wpisane na sztywno, by ładnie się odmieniało :/
Console.WriteLine("==============================");
Console.WriteLine($"0. {startingCity}");

int globalIndex = 0,
    sumKM = 0;

string getCityNameByKM(int row, int km) {
    int index = distancesBetweenCities[row].ToList().FindIndex(x => x == km);
    return index != -1 ? cityNames[index] : "";
}

for (int i = 0; i < cityNames.Length - 1; i++) {
    int nearest = distancesBetweenCities[indexOfVisitedCities[globalIndex]]
        .Where(x => x != 0) //odrzuć samego siebie
        .Where(x => !indexOfVisitedCities.Take(globalIndex).Any(x2 => cityNames[x2] == getCityNameByKM(indexOfVisitedCities[globalIndex], x))) //odrzuć już odwiedzone
        .Min(),
        nearestIndex = distancesBetweenCities[indexOfVisitedCities[globalIndex]].ToList().FindIndex(x => x == nearest);
    string nearestName = cityNames[nearestIndex];
    Console.WriteLine($"{i + 1}. {nearestName} - {nearest}km");
    indexOfVisitedCities[i + 1] = nearestIndex;
    sumKM += nearest;
    globalIndex++;

    if (globalIndex == cityNames.Length - 1) {
        Console.WriteLine($"{i + 2}. {cityNames[indexOfVisitedCities[0]]} - {distancesBetweenCities[nearestIndex][indexOfVisitedCities[0]]}km");
    }
}

Console.WriteLine("==============================");
Console.WriteLine($"Łączna długość trasy: {sumKM}km");