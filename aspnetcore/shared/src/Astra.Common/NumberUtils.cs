namespace Astra.Common;

public static class NumberUtils
{
    public static decimal Truncate(this decimal value, uint decimalPlaces)
    {
        //整数部分
        var integralValue = Math.Truncate(value);
        //小数部分
        var fraction = value - integralValue;
        //截断
        var factor = (decimal)Math.Pow(10, decimalPlaces);
        var truncatedFraction = Math.Truncate(fraction * factor) / factor;
        var result = integralValue + truncatedFraction;
        return result;
    }

    public static decimal Ceiling(this decimal value, uint decimalPlaces)
    {
        // 计算倍数因子
        var factor = (decimal)Math.Pow(10, decimalPlaces); 
        // 向上取整并恢复原始倍数
        return Math.Ceiling(value * factor) / factor;
    }
}