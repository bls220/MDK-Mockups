﻿using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups
{
    public class MockGridProgramRuntimeInfo : IMyGridProgramRuntimeInfo
    {
        public virtual TimeSpan TimeSinceLastRun { get; set; }

        public virtual double LastRunTimeMs { get; set; }

        public virtual int MaxInstructionCount { get; set; }

        public virtual int CurrentInstructionCount { get; set; }

        public virtual int MaxMethodCallCount { get; set; }

        public virtual int CurrentMethodCallCount { get; set; }

        public virtual UpdateFrequency UpdateFrequency { get; set; }

        public int MaxCallChainDepth
        {
            get
            {
                // TODO: check in SE to get this value.
                return 10;
            }
        }

        public int CurrentCallChainDepth { get; } 
    }
}