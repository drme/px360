using System;

namespace ThW.X360.Controller.Windows
{
    public enum X360Keys : int
    {
        None    = 0,
        A       = 1 << 0,
        B       = 1 << 1,
        X       = 1 << 2,
        Y       = 1 << 3,
        LB      = 1 << 4,
        RB      = 1 << 5,
        Back    = 1 << 6,
        Start = 1 << 7,
        Guide = 1 << 8,
        Sync = 1 << 9,
        Up = 1 << 10,
        Down = 1 << 11,
        Left = 1 << 12,
        Right = 1 << 13,
        LS = 1 << 14,
        RS = 1 << 15,
        LSUp = 1 << 16,
        LSDown = 1 << 17,
        LSLeft = 1 << 18,
        LSRight =1 << 19,
        RSUp = 1 << 20,
        RSDown = 1 << 21,
        RSLeft = 1 << 22,
        RSRight =1 << 23,
        LT = 1 << 24,
        RT = 1 << 25
    }
}
