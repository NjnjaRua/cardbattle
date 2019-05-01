using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CardCompare : IComparer<int>
{
    public bool reverse = false;
    public int Compare(int x, int y)
    {
        int result = 0;
        if (x < y) result = 1;
        else if (x > y) result = -1;
        if (reverse)
            result *= -1;
        return result;
    }
}
public static class Util
{
	public static bool QuickCompare(byte[] b1, byte[] b2)
	{
		if (b1 == null || b2 == null || b1.Length != b2.Length)
			return false;
		int len = b1.Length;
		for (int i = 0; i < len; i++) {
			if (b1 [i] != b2 [i])
				return false;
		}

		return true;
	}

    public static System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
    public static string GetMd5Hash(byte[] rawdata)
    {
        byte[] data = md5Hash.ComputeHash(rawdata);
        var stringBuilder = new System.Text.StringBuilder();
        for (int i = 0; i < data.Length; i++)
            stringBuilder.Append(data[i].ToString("x2"));
        return stringBuilder.ToString();
    }

    public static string NumberFormat(long number)
    {
        return number.ToString("N0");
    }

    public static float calculateTextWidth(string message, Text txt)
    {
        Canvas.ForceUpdateCanvases();
        return txt.preferredWidth;
    }

    public static Vector3 GetPostConvert(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
            Debug.DrawLine(ray.origin, hit.point);
        return hit.point;
    }
}