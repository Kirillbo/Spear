using System;
using UnityEngine;
using System.Collections;
using Tools;

public class PermamentRepeatTimer : Timer
{
    public PermamentRepeatTimer(float permamentTime, float timer, Action method) : base(permamentTime, timer, method)
    {
    }

    public override bool Tick(float deltaTime)
    {
        if (AssessTime(deltaTime))
        {
            if (SecondTime > 0)
            {
                NewTime();
            }
            Repeat();
            return true;
        }

        return false;
    }

    void Repeat()
    {
        _currentTime = 0f;
    }

    void NewTime()
    {
        Time = SecondTime;
        SecondTime = 0;
    }
}
