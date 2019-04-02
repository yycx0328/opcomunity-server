using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//GetInlineItem("asd[[qwq],[asd1[[qq1],[ee1]]]]", 1, '[', ']'); return qwq
//GetInlineItem("asd[[qwq],[asd1[[qq1],[ee1]]]]", 2, '[', ']'); return asd1[[qq1],[ee1]]]
//GetInlineItem("asd[[qwq],[asd1[[qq1],[ee1]]]]", 3, '[', ']'); return [qq1],[ee1]
//GetInlineItem("asd[[qwq],[asd1[[qq1],[ee1]]]]", 4, '[', ']'); return qq1
//GetInlineItem("asd[[qwq],[asd1[[qq1],[ee1]]]]", 5, '[', ']'); return ee1
/// <summary>
/// Arithmetic
/// </summary>
public static class Arithmetic
{
    /// <summary>
    /// GetInlineItem
    /// </summary>
    /// <param name="str">The STR.</param>
    /// <param name="nth">The NTH.</param>
    /// <param name="begin">The begin.</param>
    /// <param name="end">The end.</param>
    /// <returns></returns>
    public static string GetInlineItem(string str, int nth, char begin, char end)
    {
        int ith = 0;
        int idx = 0;

        while (idx >= 0 && ith <= nth)
        {
            idx = str.IndexOf(begin, idx);
            if (idx == -1)
                return null;

            ith++;
            idx++;
        }

        int start = idx;
        int eth = 0;
        char[] anyChars = new char[] { begin, end };
        while (idx >= 0 && eth >= 0)
        {
            idx = str.IndexOfAny(anyChars, idx);
            if (idx == -1)
                return null;

            if (str[idx] == begin)
                eth++;
            else //== end
                eth--;
            idx++;
        }

        return str.Substring(start, idx - start - 1);
    }

    /// <summary>
    /// GetAllInlineItems
    /// </summary>
    /// <param name="str">The STR.</param>
    /// <param name="begin">The begin.</param>
    /// <param name="end">The end.</param>
    /// <returns></returns>
    public static string[] GetAllInlineItems(string str, char begin, char end)
    {
        List<string> list = new List<string>();

        int ith = 0;
        string item = GetInlineItem(str, ith, begin, end);
        while (item != null)
        {
            list.Add(item);
            //list.AddRange(GetAllInlineItems(item, begin, end));
            ith++;
            item = GetInlineItem(str, ith, begin, end);
        }

        return list.ToArray();
    }

    /// <summary>
    /// 对排序数组项进行模糊配置
    /// </summary>
    /// <param name="sortedList">The sorted list.</param>
    /// <param name="value">The value.</param>
    /// <param name="separator">The separator.</param>
    /// <returns>
    /// 	<c>true</c> if [is hazy matched] [the specified sorted list]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsHazyMatched(string[] sortedList, string value, params char[] separator)
    {
        if (sortedList == null || separator == null || value == null)
            return false;

        value = value.Trim();
        int index = Array.BinarySearch<string>(sortedList, value, StringComparer.InvariantCultureIgnoreCase);
        if (index >= 0)
            return true;

        string[] srcParts = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        string[] itemParts;
        foreach (string item in sortedList.Where(p => p.Contains("*")))
        {
            itemParts = item.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (srcParts.Length != itemParts.Length)
                continue;

            for (int i = 0; i < srcParts.Length; i++)
            {
                bool iMatch = itemParts[i] == "*" || itemParts[i].Equals(srcParts[i], StringComparison.InvariantCultureIgnoreCase);
                if (iMatch)
                {
                    if (i == srcParts.Length - 1)
                        return true;
                    else
                        continue;
                }
                else
                {
                    break;
                }
            }
        }
        return false;
    }
}