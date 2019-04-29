using System.Security.Cryptography;
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
}