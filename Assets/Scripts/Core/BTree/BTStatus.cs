using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BTStatus
{
    Running,
    Success,
    Failure
}

public static class BTStatusExtension
{
    public static BTStatus Invert(this BTStatus status)
    {
        if (status == BTStatus.Success)
        {
            return BTStatus.Failure;
        }
        else if (status == BTStatus.Failure)
        {
            return BTStatus.Success;
        }
        return BTStatus.Running;
    }

    public static BTStatus Fixed(this BTStatus status, BTStatus fix)
    {
        if (status == BTStatus.Running)
        {
            return status;
        }
        else
        {
            return fix;
        }
    }
}
