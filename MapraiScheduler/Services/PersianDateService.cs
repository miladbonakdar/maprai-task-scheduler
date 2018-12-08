﻿using MD.PersianDateTime;
using System;

namespace MapraiScheduler.Services
{
    //https://www.codeproject.com/Articles/850359/Persian-Calendar-PersianDateTime-in-Csharp
    public static class PersianDateService
    {
        public static PersianDateTime GetNow(this PersianDateTime p)
        {
            return new PersianDateTime(DateTime.Now);
        }
    }
}