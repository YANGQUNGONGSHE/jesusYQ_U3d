//===================================================
//作    者：王家俊  http://www.unity3d.com  QQ：394916173
//创建时间：2016-08-26 22:26:09
//备    注：String扩展方法
//===================================================

public static class StringUtil 
{
    public static int ToInt(this string str)
    {
        int temp = 0;
        int.TryParse(str, out temp);
        return temp;
    }

    public static float ToFloat(this string str)
    {
        float temp = 0;
        float.TryParse(str, out temp);
        return temp;
    }

    public static bool ToBool(this string str)
    {
        bool temp;
        if (str.Equals("1"))
        {
            temp = true;
        }
        else
        {
            temp = false;
        }
        return temp;
    }
}