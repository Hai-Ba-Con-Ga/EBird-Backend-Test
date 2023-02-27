using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bird.Test;
[TestFixture]
public partial class Test
{
    [Test, Order(1), TestCaseSource("FixedTestCases")]
    public void TestBruteForce_WhenGivenRightArgument_RunWell(int[] weights, int days, int expected)
    {
        var actual = Solution.BruteForce(weights, days);
        Assert.AreEqual(expected, actual);
    }

    [Test, Order(2), TestCaseSource("FixedTestCases")]
    public void TestBinarySearch_WhenGivenRightArgument_RunWell(int[] weights, int days, int expected)
    {
        var actual = Solution.BinarySearch(weights, days);
        Assert.AreEqual(expected, actual);
    }

    [Test, Order(3), Repeat(2), Timeout(20000)]
    public void TestBinarySearch_WhenRandomInput_RunWell()
    {
        // create random testcase.
        var weights = GetRandomList(random, random.Next(25,30), 1, 200);
        var days = random.Next(10,20);

        var expected = Solution.BruteForce(weights, days);
        var actual = Solution.BinarySearch(weights, days);

        Assert.AreEqual(expected, actual);
    }

    
    [Test, Order(4), Repeat(3), Timeout(20000)]
    public void BinarySearch_PerformanceTest_WhenRandomInput()
    {
        // create random testcase.
        var weights = GetRandomList(random, random.Next(25,30), 1, 200);
        var days = random.Next(10,20);

        // create stopwath to calcuate execution time.
        var stopwatch = new Stopwatch();
        var executionTimeBinarySearch = new TimeSpan();
        var executionTimeBruteForce = new TimeSpan();

        
        stopwatch.Start();
        var expected = Solution.BruteForce(weights, days);
        stopwatch.Stop();
        executionTimeBruteForce = stopwatch.Elapsed;


        stopwatch.Restart();
        var actual = Solution.BinarySearch(weights, days);
        executionTimeBinarySearch = stopwatch.Elapsed;
        stopwatch.Stop();


        Assert.That(executionTimeBinarySearch.TotalMilliseconds, Is.LessThan(1));


        Console.WriteLine("weights: " + string.Join(", ", weights) + "\ndays: " + days + "\nexpected: " + expected);
        Console.WriteLine("Execution time of brute force: {0} ms", executionTimeBruteForce.TotalMilliseconds);
        Console.WriteLine("Execution time of binary search: {0} ms\n\n", executionTimeBinarySearch.TotalMilliseconds);
    }


}