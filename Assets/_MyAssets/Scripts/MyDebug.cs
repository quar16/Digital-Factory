using System.Collections;
using System.Collections.Generic;

public class MyDebug
{
    public static void Log(params object[] obj)
    {
        string str = obj[0].ToString();

        for (int i = 1; i < obj.Length; i++)
        {
            str += "|";
            str += obj[i];
        }
        UnityEngine.Debug.Log(str);
    }
}
