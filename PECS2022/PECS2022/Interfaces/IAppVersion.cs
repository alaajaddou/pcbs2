﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Interfaces
{
    public interface IAppVersion
    {
        string GetVersion();
        int GetBuild();
    }
}
