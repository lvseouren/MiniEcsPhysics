using System;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using XFixMath.NET;
using Debug = UnityEngine.Debug;
using Random = System.Random;

internal class SIMD_Test
{
    public class NormalCalc
    {
        public static double Multiply(double[] nums)
        {
            double result = 1.0d;

            for (int i = 0; i < nums.Length; i++)
            {
                result *= nums[i];
            }
            return result;
        }

        public static double AddTotal(double[] nums)
        {
            double result = 0.0d;

            for (int i = 0; i < nums.Length; i++)
            {
                result += nums[i];
            }
            return result;
        }
    }

    public static unsafe double AverageSIMD(double[] nums)
    {
        int vectorSize = Vector<double>.Count;
        var accVector = Vector<double>.Zero;
        int i;
        var array = nums;
        double result = 1.0d;
        fixed (double* p = array)
        {
            for (i = 0; i <= array.Length - vectorSize; i += vectorSize)
            {
                //var v = new Vector<double>(array, i);
                //var v = Unsafe.Read<Vector<double>>(p + i);
                Vector<double> v = new Vector<double> (nums, i );
                accVector = Vector.Add(accVector, v);                
            }
        }
        var tempArray = new double[Vector<double>.Count];
        accVector.CopyTo(tempArray);
        
        for (int j = 0; j < tempArray.Length; j++)
        {
            result = result + tempArray[j];
        }

        for (; i < array.Length; i++)
        {
            result += array[i];
        }

        return result;
    }

    public static string testInfo1;
    public static string testInfo2;
    public static void Test()
    {
        Debug.Log($"是否支持SIMD：{Vector.IsHardwareAccelerated}");
        //生成运算数组
        double[] nums = new double[100000];
        Random random = new Random();
        for (int i = 0; i < nums.Length; i++)
        {
            nums[i] = random.NextDouble() * 2.723;
        }

        //普通连+
        Stopwatch stopwatch = Stopwatch.StartNew();
        double result = 0;
        for (int i = 0; i < 10000; i++)
        {
            result = NormalCalc.AddTotal(nums);
        }
        stopwatch.Stop();
        Debug.Log($"普通连+ 结果={result},\n耗时：{stopwatch.ElapsedMilliseconds}");

        //Vector
        stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < 10000; i++)
        {
            result = AverageSIMD(nums);
        }
        stopwatch.Stop();
        Debug.Log($"Vector连+ 结果={result},\n耗时：{stopwatch.ElapsedMilliseconds}");
    }

    //public static void TestXFix64SIMD()
    //{
    //    int count = 100000;
    //    XFix64Vector4[] vecs = new XFix64Vector4[count];
    //    for (int i = 0; i < count; i++)
    //    {
    //        vecs[i] = MathXFix64.RandomXFix64Vector4();
    //    }

    //    //普通+
    //    Stopwatch stopwatch = Stopwatch.StartNew();
    //    XFix64Vector4 result = default;
    //    for (int i = 0; i < count; i++)
    //    {
    //        result = vecs[i] + vecs[i];
    //    }
    //    stopwatch.Stop();
    //    testInfo1 = ($"XFix64Vector4 普通加法: 结果={result},\n耗时：{stopwatch.ElapsedMilliseconds}");
    //    Debug.Log(testInfo1);

    //    //SIMD+
    //    stopwatch = Stopwatch.StartNew();
        
    //    for (int i = 0; i < count; i++)
    //    {
    //        result = XFix64Vector4.SIMD_ADD(vecs[i], vecs[i]);
    //    }
    //    stopwatch.Stop();
    //    testInfo2 = ($"XFix64Vector4 SIMD加法: 结果={result},\n耗时：{stopwatch.ElapsedMilliseconds}");
    //    Debug.Log(testInfo2);

    //    XFix64Vector4 a = new XFix64Vector4(1);
    //    XFix64Vector4 b = new XFix64Vector4(2);
    //    var c = a + b;
    //    var d = XFix64Vector4.SIMD_ADD(a, b);
    //}
}    

