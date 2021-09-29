using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveAxis
{
    public static readonly int HORIZONTAL = 0;
    public static readonly int VERTICAL = 1;
    public static readonly int DIAGONAL = 2;
    public static readonly List<int> AXES = new List<int>(new[]{ HORIZONTAL, VERTICAL, DIAGONAL });
}
