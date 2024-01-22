using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils {
    public static string TimeString(float t) {
        TimeSpan time = TimeSpan.FromSeconds(t);

        return string.Format("{0:D2}:{1:D2}.{2:D3}",
            time.Minutes,
            time.Seconds,
            time.Milliseconds);
    }
}
