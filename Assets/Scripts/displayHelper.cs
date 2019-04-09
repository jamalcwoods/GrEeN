using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class displayHelper
{
    static public Rect centerRect(Rect r)
    {
        return new Rect(r.position.x - (r.width / 2), r.position.y - (r.height / 2), r.width, r.height);
    }
}
