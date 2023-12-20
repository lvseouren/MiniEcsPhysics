using System;
using System.IO;

namespace XFixMath.NET
{

    /// <summary>
    /// Represents a Q31.32 fixed-point number.
    /// </summary>
    public partial struct XFix64 : IEquatable<XFix64>, IComparable<XFix64>
    {
        readonly long m_rawValue;

        const ulong MAX_INT = 0x7FFFFFFFFF;  //（SHIFTING == 8）long.MaxValue >> Fix64AL_PLACES   整数部分最大值 549,755,813,887    约5497亿 
        const int MIN_FRA = 7;//（SHIFTING == 8）不强制取四位的情况下小数部分最大位数（十进制），       

        // Precision of this type is 2^-32, that is 2,3283064365386962890625E-10
        public static readonly decimal Precision = (decimal)(new XFix64(1L));//0.00000000023283064365386962890625m;
        public static readonly XFix64 MaxValue = new XFix64(LONG_MAX_VALUE);
        public static readonly XFix64 MinValue = new XFix64(LONG_MIN_VALUE);
        public static readonly XFix64 One = new XFix64(ONE);
        public static readonly XFix64 Zero = new XFix64();
        public static readonly XFix64 MaxValueSqrt = Sqrt(MaxValue);

        public static readonly XFix64 Half = (XFix64)0.5f;
        public static readonly XFix64 Ten = (XFix64)10;
        public static readonly XFix64 Hundred = (XFix64)100;
        public static readonly XFix64 OneHundredThousand = (XFix64)100000;
        public static readonly XFix64 PositiveInfinity = MaxValue;//兼容性考虑，代替float.PositiveInfinity
        /// <summary>
        /// The value of Pi
        /// </summary>
        public static readonly XFix64 Pi = new XFix64(PI);
        public static readonly XFix64 PiOver2 = new XFix64(PI_OVER_2);
        public static readonly XFix64 PiTimes2 = new XFix64(PI_TIMES_2);
        public static readonly XFix64 PiInv = (XFix64)0.3183098861837906715377675267M;
        public static readonly XFix64 PiOver2Inv = (XFix64)0.6366197723675813430755350535M;


        const int SHIFTING = 8; // 小数点 从中点向右移位；作用：当值为正时，扩大整数范围

        static readonly XFix64 LutInterval = (XFix64)(LUT_SIZE - 1) / PiOver2;

        const long LONG_MAX_VALUE = long.MaxValue;
        const long LONG_MIN_VALUE = long.MinValue;
        const int NUM_BITS = 64;
        const int Fix64AL_PLACES = 32 - SHIFTING;
        const long ONE = 1L << Fix64AL_PLACES;
        const long PI_TIMES_2 = 0x6487ED511 >> SHIFTING;
        const long PI = 0x3243F6A88 >> SHIFTING;
        const long PI_OVER_2 = 0x1921FB544 >> SHIFTING;
        const int LUT_SIZE = (int)(PI_OVER_2 >> (15 - SHIFTING));

        public const bool MultiThread = false;

        const uint FRACTION = 0x00000000FFFFFFFF >> SHIFTING; // &FRACTION 取小数部分
        const ulong INTEGER = 0xFFFFFFFFFFFFFFFF - FRACTION; // &INTEGER 取整数部分
        const uint HalfCONST = 0x80000000 >> SHIFTING; // 0.5
        const ulong ULHalfCONST = 0x80000000UL >> SHIFTING; // 0.5



        /// <summary>
        /// Returns a number indicating the sign of a XFix64 number.
        /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
        /// </summary>
        public static int Sign(XFix64 value)
        {
            return
                value.m_rawValue < 0 ? -1 :
                value.m_rawValue > 0 ? 1 :
                0;
        }


        /// <summary>
        /// Returns the absolute value of a XFix64 number.
        /// Note: Abs(XFix64.MinValue) == XFix64.MaxValue.
        /// </summary>
        public static XFix64 Abs(XFix64 value)
        {
            if (value.m_rawValue == LONG_MIN_VALUE) {
                return MaxValue;
            }

            // branchless implementation, see http://www.strchr.com/optimized_abs_function
            var mask = value.m_rawValue >> 63;
            return new XFix64((value.m_rawValue + mask) ^ mask);
        }

        /// <summary>
        /// Returns the absolute value of a XFix64 number.
        /// FastAbs(XFix64.MinValue) is undefined.
        /// 最小值无定义。 未判断最小值溢出
        /// </summary>
        public static XFix64 FastAbs(XFix64 value)
        {
            // branchless implementation, see http://www.strchr.com/optimized_abs_function
            var mask = value.m_rawValue >> 63;
            return new XFix64((value.m_rawValue + mask) ^ mask);
        }


        /// <summary>
        /// Returns the largest integer less than or equal to the specified number.
        /// </summary>
        public static XFix64 Floor(XFix64 value)
        {
            // Just zero out the Fix64al part
            return new XFix64((long)((ulong)value.m_rawValue & INTEGER));
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        public static XFix64 Ceiling(XFix64 value)
        {
            var hasFix64alPart = (value.m_rawValue & FRACTION) != 0;
            return hasFix64alPart ? Floor(value) + One : value;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value.
        /// If the value is halfway between an even and an uneven value, returns the even value.
        /// </summary>
        public static XFix64 Round(XFix64 value)
        {
            var Fix64alPart = value.m_rawValue & FRACTION;
            var integralPart = Floor(value);
            if (Fix64alPart < HalfCONST) {
                return integralPart;
            }
            if (Fix64alPart > HalfCONST) {
                return integralPart + One;
            }
            // if number is halfway between two values, round to the nearest even number
            // this is the method used by System.Math.Round().
            return (integralPart.m_rawValue & ONE) == 0
                       ? integralPart
                       : integralPart + One;
        }

        /// <summary>
        /// Adds x and y. Performs saturating addition, i.e. in case of overflow, 
        /// rounds to MinValue or MaxValue depending on sign of operands.
        /// </summary>
        public static XFix64 operator +(XFix64 x, XFix64 y)
        {
            var xl = x.m_rawValue;
            var yl = y.m_rawValue;
            var sum = xl + yl;
            // if signs of operands are equal and signs of sum and x are different
            if (((~(xl ^ yl) & (xl ^ sum)) & LONG_MIN_VALUE) != 0) {
                sum = xl > 0 ? LONG_MAX_VALUE : LONG_MIN_VALUE;
            }
            return new XFix64(sum);
        }

        /// <summary>
        /// Adds x and y witout performing overflow checking. Should be inlined by the CLR.
        /// </summary>
        public static XFix64 FastAdd(XFix64 x, XFix64 y)
        {
            return new XFix64(x.m_rawValue + y.m_rawValue);
        }

        /// <summary>
        /// Subtracts y from x. Performs saturating substraction, i.e. in case of overflow, 
        /// rounds to MinValue or MaxValue depending on sign of operands.
        /// </summary>
        public static XFix64 operator -(XFix64 x, XFix64 y)
        {
            var xl = x.m_rawValue;
            var yl = y.m_rawValue;
            var diff = xl - yl;
            // if signs of operands are different and signs of sum and x are different
            if ((((xl ^ yl) & (xl ^ diff)) & LONG_MIN_VALUE) != 0) {
                diff = xl < 0 ? LONG_MIN_VALUE : LONG_MAX_VALUE;
            }
            return new XFix64(diff);
        }

        /// <summary>
        /// Subtracts y from x witout performing overflow checking. Should be inlined by the CLR.
        /// 未检测溢出
        /// </summary>
        public static XFix64 FastSub(XFix64 x, XFix64 y)
        {
            return new XFix64(x.m_rawValue - y.m_rawValue);
        }

        static long AddOverflowHelper(long x, long y, ref bool overflow)
        {
            var sum = x + y;
            // x + y overflows if sign(x) ^ sign(y) != sign(sum)
            overflow |= ((x ^ y ^ sum) & LONG_MIN_VALUE) != 0;
            return sum;
        }

        public static XFix64 operator *(XFix64 x, XFix64 y)
        {

            var xl = x.m_rawValue;
            var yl = y.m_rawValue;

            var xlo = (ulong)(xl & FRACTION);
            var xhi = xl >> Fix64AL_PLACES;
            var ylo = (ulong)(yl & FRACTION);
            var yhi = yl >> Fix64AL_PLACES;

            var lolo = xlo * ylo;
            var lohi = (long)xlo * yhi;
            var hilo = xhi * (long)ylo;
            var hihi = xhi * yhi;

            var loResult = lolo >> Fix64AL_PLACES;
            var midResult1 = lohi;
            var midResult2 = hilo;
            var hiResult = hihi << Fix64AL_PLACES;

            bool overflow = false;
            var sum = AddOverflowHelper((long)loResult, midResult1, ref overflow);
            sum = AddOverflowHelper(sum, midResult2, ref overflow);
            sum = AddOverflowHelper(sum, hiResult, ref overflow);

            bool opSignsEqual = ((xl ^ yl) & LONG_MIN_VALUE) == 0;

            // if signs of operands are equal and sign of result is negative,
            // then multiplication overflowed positively
            // the reverse is also true
            if (opSignsEqual) {
                if (sum < 0 || (overflow && xl > 0)) {
                    //XLog.LogError("XFix64 multiplication1 overflowed");
                    return MaxValue;
                }
            }
            else {
                if (sum > 0) {
                    //XLog.LogError("XFix64 multiplication2 overflowed");
                    return MinValue;
                }
            }

            // if the top 32 bits of hihi (unused in the result) are neither all 0s or 1s,
            // then this means the result overflowed.
            //hihi：整数部分乘积（未移位），整数部分所占位数为64 - Fix64AL_PLACES
            var topCarry = hihi >> 64 - Fix64AL_PLACES;
            if (topCarry != 0 && topCarry != -1 /*&& xl != -17 && yl != -17*/) {
                //XLog.LogError("XFix64 multiplication3 overflowed");
                return opSignsEqual ? MaxValue : MinValue;
            }

            // If signs differ, both operands' magnitudes are greater than 1,
            // and the result is greater than the negative operand, then there was negative overflow.
            if (!opSignsEqual) {
                long posOp, negOp;
                if (xl > yl) {
                    posOp = xl;
                    negOp = yl;
                }
                else {
                    posOp = yl;
                    negOp = xl;
                }
                if (sum > negOp && negOp < -ONE && posOp > ONE) {
                    //XLog.LogError("XFix64 multiplication4 overflowed");
                    return MinValue;
                }
            }

            return new XFix64(sum);
        }

        /// <summary>
        /// Performs multiplication without checking for overflow.
        /// 未检测溢出
        /// Useful for performance-critical code where the values are guaranteed not to cause overflow
        /// </summary>
        public static XFix64 FastMul(XFix64 x, XFix64 y)
        {

            var xl = x.m_rawValue;
            var yl = y.m_rawValue;

            var xlo = (ulong)(xl & FRACTION);
            var xhi = xl >> Fix64AL_PLACES;
            var ylo = (ulong)(yl & FRACTION);
            var yhi = yl >> Fix64AL_PLACES;

            var lolo = xlo * ylo;
            var lohi = (long)xlo * yhi;
            var hilo = xhi * (long)ylo;
            var hihi = xhi * yhi;

            //小数乘法
            var loResult = lolo >> Fix64AL_PLACES;
            var midResult1 = lohi;
            var midResult2 = hilo;
            //定点数的整数部分
            var hiResult = hihi << Fix64AL_PLACES;

            var sum = (long)loResult + midResult1 + midResult2 + hiResult;
            return new XFix64(sum);
        }

        //dotnet4.5 以上版本可用 AggressiveInlining
        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)] 
        static int CountLeadingZeroes(ulong x)
        {
            int result = 0;
            while ((x & 0xF000000000000000) == 0) { result += 4; x <<= 4; }
            while ((x & 0x8000000000000000) == 0) { result += 1; x <<= 1; }
            return result;
        }

        public static XFix64 test_div(XFix64 x, XFix64 y)
        {
            if (x == 0) {
                return XFix64.Zero;
            }
            if (y == 0) {
                throw new DivideByZeroException();
            }

            int sign = Sign(x) * Sign(y);
            ulong x_ul = (ulong)x.m_rawValue;
            ulong y_ul = (ulong)y.m_rawValue;
            //(long)((x_ul  / y_ul)<<Fix64AL_PLACES) + 
            ulong d = x_ul / y_ul;
            if (d >= (0xFFFFFFFFFFFFFFFF >> Fix64AL_PLACES)) {
                //XLog.LogError("XFix64 divider overflowed");
                if (sign > 0) {
                    return MaxValue;
                }
                else {
                    return MinValue;
                }
            }
            return new XFix64(((long)(d << Fix64AL_PLACES) + (long)((((x_ul % y_ul) << Fix64AL_PLACES) / y_ul))) * sign);

        }

        /// <summary>
        /// https://blog.veitheller.de/Fixed_Point_Division.html
        /// </summary>
        public static XFix64 operator /(XFix64 x, XFix64 y)
        {
            var xl = x.m_rawValue;
            var yl = y.m_rawValue;

            if (yl == 0) {
                //TODO 多线程下需要特殊处理
                if (MultiThread) {
                    return XFix64.Zero;
                }
                else {
                    throw new DivideByZeroException();
                }
            }

            var remainder = (ulong)(xl >= 0 ? xl : -xl);
            var divider = (ulong)(yl >= 0 ? yl : -yl);
            var quotient = 0UL;
            var bitPos = Fix64AL_PLACES + 1;


            // If the divider is divisible by 2^n, take advantage of it.
            while ((divider & 0xF) == 0 && bitPos >= 4) {
                divider >>= 4;
                bitPos -= 4;
            }

            while (remainder != 0 && bitPos >= 0) {
                int shift = CountLeadingZeroes(remainder);
                if (shift > bitPos) {
                    shift = bitPos;
                }

                remainder <<= shift;
                bitPos -= shift;
                var div = remainder / divider;
                remainder = remainder % divider;
                quotient += div << bitPos;

                //Detect overflow
                if ((div & ~(0xFFFFFFFFFFFFFFFF >> bitPos)) != 0) {
                    //XLog.LogError("XFix64 divider overflowed");
                    return ((xl ^ yl) & LONG_MIN_VALUE) == 0 ? MaxValue : MinValue;
                }

                remainder <<= 1;
                --bitPos;
            }

            // rounding
            ++quotient;
            var result = (long)(quotient >> 1);
            if (((xl ^ yl) & LONG_MIN_VALUE) != 0) {
                result = -result;
            }

            return new XFix64(result);
        }

        public static XFix64 operator %(XFix64 x, XFix64 y)
        {
            return new XFix64(
                x.m_rawValue == LONG_MIN_VALUE & y.m_rawValue == -1 ?
                0 :
                x.m_rawValue % y.m_rawValue);
        }

        /// <summary>
        /// Performs modulo as fast as possible; throws if x == MinValue and y == -1.
        /// Use the operator (%) for a more reliable but slower modulo.
        /// </summary>
        public static XFix64 FastMod(XFix64 x, XFix64 y)
        {
            return new XFix64(x.m_rawValue % y.m_rawValue);
        }

        public static XFix64 operator -(XFix64 x)
        {
            return x.m_rawValue == LONG_MIN_VALUE ? MaxValue : new XFix64(-x.m_rawValue);
        }

        public static bool operator ==(XFix64 x, XFix64 y)
        {
            return x.m_rawValue == y.m_rawValue;
        }

        public static bool operator !=(XFix64 x, XFix64 y)
        {
            return x.m_rawValue != y.m_rawValue;
        }

        public static bool operator >(XFix64 x, XFix64 y)
        {
            return x.m_rawValue > y.m_rawValue;
        }

        public static bool operator <(XFix64 x, XFix64 y)
        {
            return x.m_rawValue < y.m_rawValue;
        }

        public static bool operator >=(XFix64 x, XFix64 y)
        {
            return x.m_rawValue >= y.m_rawValue;
        }

        public static bool operator <=(XFix64 x, XFix64 y)
        {
            return x.m_rawValue <= y.m_rawValue;
        }


        /// <summary>
        /// https://en.wikipedia.org/wiki/Methods_of_computing_square_roots
        /// http://medialab.freaknet.org/martin/src/sqrt/
        /// Returns the square root of a specified number.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was negative.
        /// </exception>
        public static XFix64 Sqrt(XFix64 x)
        {
            var xl = x.m_rawValue;
            if (xl < 0) {
                // We cannot represent infinities like Single and Double, and Sqrt is
                // mathematically undefined for x < 0. So we just throw an exception.
                throw new ArgumentOutOfRangeException("Negative value passed to Sqrt", "x:" + xl);
            }

            var num = (ulong)xl;
            var result = 0UL;

            // second-to-top bit
            var bit = 1UL << (NUM_BITS - 2);

            while (bit > num) {
                bit >>= 2;
            }

            // The main part is executed twice, in order to avoid
            // using 128 bit values in computations.
            for (var i = 0; i < 2; ++i) {
                // First we get the top 48 bits of the answer.
                while (bit != 0) {
                    if (num >= result + bit) {
                        num -= result + bit;
                        result = (result >> 1) + bit;
                    }
                    else {
                        result = result >> 1;
                    }
                    bit >>= 2;
                }

                if (i == 0) {
                    // Then process it again to get the lowest 16 bits.
                    if (num > (1UL << Fix64AL_PLACES) - 1) {
                        // The remainder 'num' is too large to be shifted left
                        // by 32, so we have to add 1 to result manually and
                        // adjust 'num' accordingly.
                        // num = a - (result + 0.5)^2
                        //       = num + result^2 - (result + 0.5)^2
                        //       = num - result - 0.5
                        num -= result;
                        num = (num << Fix64AL_PLACES) - ULHalfCONST;
                        result = (result << Fix64AL_PLACES) + ULHalfCONST;
                    }
                    else {
                        num <<= Fix64AL_PLACES;
                        result <<= Fix64AL_PLACES;
                    }

                    bit = 1UL << (Fix64AL_PLACES - 2);
                }
            }
            // Finally, if next bit would have been 1, round the result upwards.
            if (num > result) {
                ++result;
            }
            return new XFix64((long)result);
        }

        /// <summary>
        /// Returns the Sine of x.
        /// This function has about 9 decimals of accuracy for small values of x.
        /// It may lose accuracy as the value of x grows.
        /// Performance: about 25% slower than Math.Sin() in x64, and 200% slower in x86.
        /// </summary>
        public static XFix64 Sin(XFix64 x)
        {
            bool flipHorizontal, flipVertical;
            var clampedL = ClampSinValue(x.m_rawValue, out flipHorizontal, out flipVertical);
            var clamped = new XFix64(clampedL);

            // Find the two closest values in the LUT and perform linear interpolation
            // This is what kills the performance of this function on x86 - x64 is fine though
            var rawIndex = FastMul(clamped, LutInterval);
            var roundedIndex = Round(rawIndex);
            var indexError = FastSub(rawIndex, roundedIndex);

            var nearestValue = new XFix64(SinLut[flipHorizontal ?
                SinLut.Length - 1 - (int)roundedIndex :
                (int)roundedIndex] >> SHIFTING);
            var secondNearestValue = new XFix64(SinLut[flipHorizontal ?
                SinLut.Length - 1 - (int)roundedIndex - Sign(indexError) :
                (int)roundedIndex + Sign(indexError)] >> SHIFTING);

            var delta = FastMul(indexError, FastAbs(FastSub(nearestValue, secondNearestValue))).m_rawValue;
            var interpolatedValue = nearestValue.m_rawValue + (flipHorizontal ? -delta : delta);
            var finalValue = flipVertical ? -interpolatedValue : interpolatedValue;
            return new XFix64(finalValue);
        }

        /// <summary>
        /// Returns a rough approximation of the Sine of x.
        /// This is at least 3 times faster than Sin() on x86 and slightly faster than Math.Sin(),
        /// however its accuracy is limited to 4-5 decimals, for small enough values of x.
        /// </summary>
        public static XFix64 FastSin(XFix64 x)
        {
            bool flipHorizontal, flipVertical;
            var clampedL = ClampSinValue(x.m_rawValue, out flipHorizontal, out flipVertical);

            // Here we use the fact that the SinLut table has a number of entries
            // equal to (PI_OVER_2 >> 15) to use the angle to index directly into it
            var rawIndex = (uint)(clampedL >> (15 - SHIFTING));
            if (rawIndex >= LUT_SIZE) {
                rawIndex = LUT_SIZE - 1;
            }
            var nearestValue = SinLut[flipHorizontal ?
                SinLut.Length - 1 - (int)rawIndex :
                (int)rawIndex] >> SHIFTING;
            return new XFix64(flipVertical ? -nearestValue : nearestValue);
        }

        /// <summary>
        /// todo:返回cos值对应的angle，即cos(return) = x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static XFix64 Acos(XFix64 x)
        {
            return Zero;
        }

        public static XFix64 Clamp(XFix64 x, XFix64 min, XFix64 max)
        {
            x = x < min ? min : x;
            x = x > max ? max : x;
            return x;
        }

        public static XFix64 Clamp01(XFix64 x)
        {
            return Clamp(x, XFix64.Zero, XFix64.One);
        }

        public static XFix64 Max(XFix64 a, XFix64 b)
        {
            return a > b ? a : b;
        }

        public static XFix64 Min(XFix64 a, XFix64 b)
        {
            return a > b ? b : a;
        }

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)] 
        static long ClampSinValue(long angle, out bool flipHorizontal, out bool flipVertical)
        {
            // Clamp value to 0 - 2*PI using modulo; this is very slow but there's no better way AFAIK
            var clamped2Pi = angle % PI_TIMES_2;
            if (angle < 0) {
                clamped2Pi += PI_TIMES_2;
            }

            // The LUT contains values for 0 - PiOver2; every other value must be obtained by
            // vertical or horizontal mirroring
            flipVertical = clamped2Pi >= PI;
            // obtain (angle % PI) from (angle % 2PI) - much faster than doing another modulo
            var clampedPi = clamped2Pi;
            while (clampedPi >= PI) {
                clampedPi -= PI;
            }
            flipHorizontal = clampedPi >= PI_OVER_2;
            // obtain (angle % PI_OVER_2) from (angle % PI) - much faster than doing another modulo
            var clampedPiOver2 = clampedPi;
            if (clampedPiOver2 >= PI_OVER_2) {
                clampedPiOver2 -= PI_OVER_2;
            }
            return clampedPiOver2;
        }

        /// <summary>
        /// Returns the cosine of x.
        /// See Sin() for more details.
        /// </summary>
        public static XFix64 Cos(XFix64 x)
        {
            var xl = x.m_rawValue;
            var rawAngle = xl + (xl > 0 ? -PI - PI_OVER_2 : PI_OVER_2);
            return Sin(new XFix64(rawAngle));
        }

        /// <summary>
        /// Returns a rough approximation of the cosine of x.
        /// See FastSin for more details.
        /// </summary>
        public static XFix64 FastCos(XFix64 x)
        {
            var xl = x.m_rawValue;
            var rawAngle = xl + (xl > 0 ? -PI - PI_OVER_2 : PI_OVER_2);
            return FastSin(new XFix64(rawAngle));
        }

        /// <summary>
        /// Returns the tangent of x.
        /// </summary>
        /// <remarks>
        /// This function is not well-tested. It may be wildly inaccurate.
        /// </remarks>
        public static XFix64 Tan(XFix64 x)
        {
            var clampedPi = x.m_rawValue % PI;
            var flip = false;
            if (clampedPi < 0) {
                clampedPi = -clampedPi;
                flip = true;
            }
            if (clampedPi > PI_OVER_2) {
                flip = !flip;
                clampedPi = PI_OVER_2 - (clampedPi - PI_OVER_2);
            }

            var clamped = new XFix64(clampedPi);

            // Find the two closest values in the LUT and perform linear interpolation
            var rawIndex = FastMul(clamped, LutInterval);
            var roundedIndex = Round(rawIndex);
            var indexError = FastSub(rawIndex, roundedIndex);

            var nearestValue = new XFix64(TanLut[(int)roundedIndex] >> SHIFTING);
            var secondNearestValue = new XFix64(TanLut[(int)roundedIndex + Sign(indexError)] >> SHIFTING);

            var delta = FastMul(indexError, FastAbs(FastSub(nearestValue, secondNearestValue))).m_rawValue;
            var interpolatedValue = nearestValue.m_rawValue + delta;
            var finalValue = flip ? -interpolatedValue : interpolatedValue;
            return new XFix64(finalValue);
        }

        public static XFix64 Atan2(XFix64 y, XFix64 x)
        {
            var yl = y.m_rawValue;
            var xl = x.m_rawValue;
            if (xl == 0) {
                if (yl > 0) {
                    return PiOver2;
                }
                if (yl == 0) {
                    return Zero;
                }
                return -PiOver2;
            }
            XFix64 atan;
            var z = y / x;

            // Deal with overflow
            if (One + (XFix64)0.28M * z * z == MaxValue) {
                return y < Zero ? -PiOver2 : PiOver2;
            }

            if (Abs(z) < One) {
                atan = z / (One + (XFix64)0.28M * z * z);
                if (xl < 0) {
                    if (yl < 0) {
                        return atan - Pi;
                    }
                    return atan + Pi;
                }
            }
            else {
                atan = PiOver2 - z / (z * z + (XFix64)0.28M);
                if (yl < 0) {
                    return atan - Pi;
                }
            }
            return atan;
        }



        public static explicit operator XFix64(long value)
        {
            return new XFix64(value * ONE);
        }
        public static explicit operator long(XFix64 value)
        {
            return value.m_rawValue >> Fix64AL_PLACES;
        }

        public static explicit operator XFix64(double value)
        {
            return new XFix64((long)(value * ONE));
        }

        public static explicit operator double(XFix64 value)
        {
            return (double)value.m_rawValue / ONE;
        }
        public static explicit operator XFix64(decimal value)
        {
            return new XFix64((long)(value * ONE));
        }
        public static explicit operator decimal(XFix64 value)
        {
            return (decimal)value.m_rawValue / ONE;
        }

        public override bool Equals(object obj)
        {
            return obj is XFix64 && ((XFix64)obj).m_rawValue == m_rawValue;
        }

        public override int GetHashCode()
        {
            return m_rawValue.GetHashCode();
        }

        public bool Equals(XFix64 other)
        {
            return m_rawValue == other.m_rawValue;
        }

        public int CompareTo(XFix64 other)
        {
            return m_rawValue.CompareTo(other.m_rawValue);
        }

        public override string ToString()
        {
            //return $"{((decimal)this).ToString()}:{m_rawValue}";
            return ((decimal)this).ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            // Up to 10 decimal places
            return ((decimal)this).ToString("0.##########", formatProvider);
        }
        public static XFix64 FromRaw(long rawValue)
        {
            return new XFix64(rawValue);
        }

        internal static void GenerateSinLut()
        {
            using (var writer = new StreamWriter("Fix64SinLut.cs")) {
                writer.Write(
@"namespace FixMath.NET {
    partial struct XFix64 {
        public static readonly long[] SinLut = new[] {");
                int lineCounter = 0;
                for (int i = 0; i < LUT_SIZE; ++i) {
                    var angle = i * Math.PI * 0.5 / (LUT_SIZE - 1);
                    if (lineCounter++ % 8 == 0) {
                        writer.WriteLine();
                        writer.Write("            ");
                    }
                    var sin = Math.Sin(angle);
                    var rawValue = ((XFix64)sin).m_rawValue;
                    writer.Write(string.Format("0x{0:X}L, ", rawValue));
                }
                writer.Write(
@"
        };
    }
}");
            }
        }

        internal static void GenerateTanLut()
        {
            using (var writer = new StreamWriter("Fix64TanLut.cs")) {
                writer.Write(
@"namespace FixMath.NET {
    partial struct XFix64 {
        public static readonly long[] TanLut = new[] {");
                int lineCounter = 0;
                for (int i = 0; i < LUT_SIZE; ++i) {
                    var angle = i * Math.PI * 0.5 / (LUT_SIZE - 1);
                    if (lineCounter++ % 8 == 0) {
                        writer.WriteLine();
                        writer.Write("            ");
                    }
                    var tan = Math.Tan(angle);
                    if (tan > (double)MaxValue || tan < 0.0) {
                        tan = (double)MaxValue;
                    }
                    var rawValue = (((decimal)tan > (decimal)MaxValue || tan < 0.0) ? MaxValue : (XFix64)tan).m_rawValue;
                    writer.Write(string.Format("0x{0:X}L, ", rawValue));
                }
                writer.Write(
@"
        };
    }
}");
            }
        }
        public static long theOne()
        {
            return ONE;
        }
        /// <summary>
        /// The underlying integer representation
        /// </summary>
        public long RawValue { get { return m_rawValue; } }

        /// <summary>
        /// This is the constructor from raw value; it can only be used interally.
        /// </summary>
        /// <param name="rawValue"></param>
        XFix64(long rawValue)
        {
            m_rawValue = rawValue;
        }

        public XFix64(int value)
        {
            m_rawValue = value * ONE;
        }

        public XFix64(float value)
        {
            float f = (float)Math.Round(value, 4);
            m_rawValue = (long)(f * ONE);
        }

        public XFix64(double value)
        {
            ///警告！ 不允许使用double
            float f = (float)Math.Round(value, 4);
            m_rawValue = (long)(f * ONE);
        }

        public static XFix64 CreateRawFix64(long value)
        {
            return new XFix64(value);
        }

        #region 各位分数处理
        //public static explicit operator XFix64(CfgF100 value)
        //{
        //    return new XFix64(value);
        //}

        //public static explicit operator XFix64(CfgF1000 value)
        //{
        //    return new XFix64(value);
        //}

        //public static explicit operator XFix64(CfgF10000 value)
        //{
        //    return new XFix64(value);
        //}

        //public static explicit operator XFix64(CfgF100000 value)
        //{
        //    return new XFix64(value);
        //}

        //public static explicit operator XFix64(CfgF1000000 value)
        //{
        //    return new XFix64(value);
        //}

        //public XFix64(CfgF100 value)
        //{
        //    if (value == null) {
        //        m_rawValue = 0;
        //    }
        //    else {
        //        float f = (float)Math.Round(value.Numerator / 100f, 2);
        //        m_rawValue = (long)(f * ONE);
        //    }
        //}

        //public XFix64(CfgF1000 value)
        //{
        //    if (value == null) {
        //        m_rawValue = 0;
        //    }
        //    else {
        //        float f = (float)Math.Round(value.Numerator / 1000f, 3);
        //        m_rawValue = (long)(f * ONE);
        //    }
        //}

        //public XFix64(CfgF10000 value)
        //{
        //    if (value == null) {
        //        m_rawValue = 0;
        //    }
        //    else {
        //        float f = (float)Math.Round(value.Numerator / 10000f, 4);
        //        m_rawValue = (long)(f * ONE);
        //    }
        //}

        //public XFix64(CfgF100000 value)
        //{
        //    if (value == null) {
        //        m_rawValue = 0;
        //    }
        //    else {
        //        float f = (float)Math.Round(value.Numerator / 100000f, 5);
        //        m_rawValue = (long)(f * ONE);
        //    }
        //}

        //public XFix64(CfgF1000000 value)
        //{
        //    if (value == null) {
        //        m_rawValue = 0;
        //    }
        //    else {
        //        float f = (float)Math.Round(value.Numerator / 1000000f, 6);
        //        m_rawValue = (long)(f * ONE);
        //    }
        //}
        #endregion

        #region 重载操作运算符


        public static implicit operator XFix64(float value)
        {
            //return new XFix64((long)(value * ONE));
            float f = (float)Math.Round(value, 4);
            var ret = new XFix64((long)(f * ONE));
            return ret;
        }
        public static implicit operator float(XFix64 value)
        {
            return (float)value.m_rawValue / ONE;
        }

        public static implicit operator int(XFix64 num)
        {
            if (Math.Abs(num.m_rawValue / ONE) > int.MaxValue) {
                //XLog.LogError($"XFix64 converter overflowed, long to int");
            }
            return (int)(num.m_rawValue / ONE);
        }

        public static implicit operator XFix64(int num)
        {
            return new XFix64(num);
        }

        //public static implicit operator LBattleSide(XFix64 num)
        //{
        //    return (LBattleSide)(int)num;
        //}

        public static XFix64 operator +(XFix64 left, float right)
        {
            return left + new XFix64(right);
        }

        public static XFix64 operator -(XFix64 left, float right)
        {
            return left - new XFix64(right);
        }

        public static XFix64 operator *(XFix64 left, float right)
        {
            return left * new XFix64(right);
        }

        public static XFix64 operator /(XFix64 left, float right)
        {
            return left / new XFix64(right);
        }

        public static XFix64 operator +(XFix64 left, int right)
        {
            return left + new XFix64(right);
        }

        public static XFix64 operator -(XFix64 left, int right)
        {
            return left - new XFix64(right);
        }

        public static XFix64 operator *(XFix64 left, int right)
        {
            return left * new XFix64(right);
        }

        public static XFix64 operator /(XFix64 left, int right)
        {
            return left / new XFix64(right);
        }


        #endregion

        #region 加载三角函数
#if SERVER
        public static readonly long[] SinLut = GetSinLut();
        public static readonly long[] TanLut = GetTanLut();
        public static void Clear()
        {
        }
#else
        private static long[] s_SinLut;
        private static long[] s_TanLut;
        public static long[] SinLut
        {
            get {
                if (s_SinLut == null) {
                    s_SinLut = GetSinLut();
                }
                return s_SinLut;
            }
        }
        public static long[] TanLut
        {
            get {
                if (s_TanLut == null) {
                    s_TanLut = GetTanLut();
                }
                return s_TanLut;
            }
        }
        public static void Clear()
        {
            //s_SinLut = null;
            //s_TanLut = null;
        }
#endif
        #endregion
    }
}
