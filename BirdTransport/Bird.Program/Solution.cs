
public class Solution
{
    public static int BruteForce(int[] weights, int days, int currentPosition = 0, int currentDay = 0, int maxWeight = 0)
    {

        if (currentPosition >= weights.Length)
        {
            if (currentDay <= days)
                return maxWeight;
            else return Int32.MaxValue;
        }
        if (currentDay >= days)
            return Int32.MaxValue;

        var result = Int32.MaxValue;

        for (int next = currentPosition, sum = 0; next < weights.Length; next++)
        {
            sum += weights[next];
            result = Math.Min(result, BruteForce(weights, days, next + 1, currentDay + 1, Math.Max(maxWeight, sum)));
        }

        if (currentDay == 0 && result == Int32.MaxValue)
            throw new Exception("No solution");
        return result;
    }

    public static int BinarySearch(int[] weights, int days)
    {
        var left = 1;
        var right = weights.Sum() + 1;
        var result = right + 1;

        while (left <= right)
        {
            var mid = left + right >> 1;

            if (checkWithExpectResult(weights, days, mid))
            {
                right = mid - 1;
                result = Math.Min(result, mid);
            }
            else left = mid + 1;
        }

        return result;
    }
    public static bool checkWithExpectResult(int[] weights, int days, int capacity)
    {
        var sum = 0;
        var count = 0;
        foreach (var item in weights)
        {
            if (item > capacity) return false;
            if (sum + item > capacity)
            {
                count++;
                sum = item;
                continue;
            }
            else sum += item;
        }
        if (sum != 0) count++;
        return count <= days;
    }

    static void Main(string[] args)
    {
        var weights = new int[]{3, 2, 2, 4, 1, 4};
        var days = 3;

        weights = new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        days = 5;
        Console.WriteLine(BruteForce(weights, days));
        Console.WriteLine(BinarySearch(weights, days));
    }
}