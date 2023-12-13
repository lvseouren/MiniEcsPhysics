using System;
using System.Collections.Generic;
using XFixMath.NET;

/// <summary>
/// 底层常量定义
/// </summary>
public static class XDefine
{

    /// <summary>
    /// 10毫米/厘米
    /// </summary>
    public const int UNIT_MM_PER_CM = 10;
    /// <summary>
    /// 0.1厘米/毫米
    /// </summary>
    public const float UNIT_CM_PER_MM = 0.1f;

    /// <summary>
    /// 100厘米/米
    /// </summary>
    public const int UNIT_CM_PER_METRE = 100;
    /// <summary>
    /// 0.01米/厘米
    /// </summary>
    public const float UNIT_METRE_PER_CM = 0.01f;

    /// <summary>
    /// 100【厘弧度】/弧度
    /// </summary>
    public const int UNIT_CR_PER_RADIAN = 100;

    /// <summary>
    /// 0.001米/毫米
    /// </summary>
    public const float UNIT_METRE_PER_MM = 0.001f;

    /// <summary>
    ////1000毫米/米
    /// </summary>
    public const int UNIT_MM_PER_METRE = 1000;

    /// <summary>
    /// 1000毫秒/秒
    /// </summary>
    public const int UNIT_MS_PER_SECOND = 1000;

#if SERVER
    /// <summary>
    /// x毫秒/帧
    /// </summary>
    public const int UNIT_MS_PER_FRAME = 100;
    /// <summary>
    /// x秒/帧
    /// </summary>
    //public const float UNIT_SECOND_PER_FRAME = 0.1f;
    public static readonly Fix64 UNIT_SECOND_PER_FRAME = Fix64.One / Fix64.Ten;
#else
    /// <summary>
    /// x毫秒/逻辑帧
    /// </summary>
    public const int UNIT_MS_PER_FRAME = 100;
    /// <summary>
    /// x秒/逻辑帧 0.1
    /// </summary>
    public static readonly XFix64 UNIT_SECOND_PER_FRAME = XFix64.One / XFix64.Ten;
#endif
    /// <summary>
    /// （动态变化）x毫秒/渲染帧，跟unity的协程调用保持一致
    /// </summary>
    public static int DYNAMIC_MS_PER_RENDER_FRAME = 33;
    /// <summary>
    /// x渲染帧/秒
    /// </summary>
    public static int RENDER_FRAME_RATE = -1;

    /// <summary>
    /// 0.001秒/毫秒
    /// </summary>
    public const float UNIT_SECOND_PER_MS = 0.001f;
    /// <summary>
    /// 百分比分母
    /// </summary>
    public const int UNIT_HUNDRED_PER_HUNDRED = 100;
    /// <summary>
    /// 万分比分母
    /// </summary>
    public const int TEN_THOUSANDTH_RATIO = 10000;
    /// <summary>
    /// 百分之一
    /// </summary>
    public const float UNIT_ONE_PERCENT = 0.01f;
    /// <summary>
    /// 万分之一
    /// </summary>
    public const float UNIT_ONE_TEN_THOUSANDS = 0.0001f;
    /// <summary>
    /// 声音百分比分母
    /// </summary>
    public const float SOUND_VOLUME = 100;
    /// <summary>
    /// 战斗闪避率分母
    /// </summary>
    public const int BATTLE_DODGE_MAX = 100;
    /// <summary>
    /// 影子高度
    /// </summary>
    public const float BATTLE_SHADOW_HIGHT = 0.1f;
    /// <summary>
    /// 摄像机振幅
    /// </summary>
    public const int BATTLE_SHAKE_WIDTH = 15;
    /// <summary>
    /// 战斗天赋数值转换比例
    /// </summary>
    public const float BATTLE_SCIENCE_NUM_RATE = 0.0001f;
}
