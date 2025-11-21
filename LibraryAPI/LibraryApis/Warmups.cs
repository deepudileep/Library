// Warmups.cs
using System;
using System.Text;

namespace LibraryApis.Common;

public static class Warmups
{   
    public static bool IsPowerOfTwo(long n)
    {
        if (n <= 0) return false;
        // bit trick: power of two has single 1-bit
        return (n & (n - 1)) == 0;
    }

    public static string ReverseTitle(string title)
    {
        if (title == null) return null;
        var sb = new StringBuilder(title.Length);
        for (int i = title.Length - 1; i >= 0; i--)
            sb.Append(title[i]);
        return sb.ToString();
    }

    public static string RepeatTitle(string title, int times)
    {
        if (title == null) return null;
        if (times <= 0) return string.Empty;
        var sb = new StringBuilder(title.Length * times);
        for (int i = 0; i < times; i++) sb.Append(title);
        return sb.ToString();
    }

    public static int[] ListOddIdsUpTo100()
    {
        var list = new System.Collections.Generic.List<int>();
        for (int i = 1; i < 100; i += 2) list.Add(i);
        return list.ToArray();
    }
}
