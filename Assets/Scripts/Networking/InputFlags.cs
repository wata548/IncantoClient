using System;
using BVH;

namespace Networking {
	[Flags]
    public enum InputFlags {
    	None       = 0b0,
        Forward    = 0b1,
        Backward   = 0b10,
        Left       = 0b100,
        Right      = 0b1000,
        Focus      = 0b10000,
        Jump       = 0b100000,
    }
}