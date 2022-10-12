using System;
using System.Collections.Generic;
using UnityEngine.Events;

public static class CSharpCallLuaList
{
    [XLua.CSharpCallLua]
    public static List<Type> csharpCallLuaList = new List<Type>()
    {
        typeof(UnityAction<bool>)
    };
}