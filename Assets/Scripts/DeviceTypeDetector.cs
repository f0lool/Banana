using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DeviceTypeDetector;

public static class DeviceTypeDetector
{
    public enum DeviceType
    {
        Mobile,
        Desktop
    }

    public static DeviceType CheckDeviceType()
    {
        if (Application.isMobilePlatform)
        {
            return DeviceType.Mobile;
        }
        else
        {
            return DeviceType.Desktop;
        }
    }
}
