using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bird.Test;
[TestFixture]
public partial class Test
{

    static Random random = new Random();
    static int[] GetRandomList(Random random, int count, int minValue, int maxValue)
    {
        var list = new int[count];
        for (int i = 0; i < count; i++)
            list[i] = random.Next(minValue, maxValue + 1);
        return list;
    }

    static IEnumerable<object[]> FixedTestCases()
    {
        yield return new object[] { new int[] { 3, 2, 2, 4, 1, 4 }, 3, 6 };
        yield return new object[] { new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 5, 15 };
        yield return new object[] { new int[] { 6, 2, 8, 5, 10, 8, 5, 7, 9, 11, 5, 4, 3, 7 }, 2, 46 };
        

        // empty weights
        //yield return new object[] { new int[] { }, 4, 3 };

        // wrong days
        //yield return new object[] { new int[] { 3, 6, 2, 4, 1 }, -1, 3 };
    }

    static int lengthOfRandomList = 1;
    static IEnumerable<object[]> RandomTestCases()
    {
        for (int i = 0; i < lengthOfRandomList; i++)
        {
            var list = GetRandomList(random, random.Next(1, 3), 1, 1);
            var days = random.Next(1, 10);
            var expected = Solution.BruteForce(list, days);

            yield return new object[] { list, days, expected };
        }
    }

}
