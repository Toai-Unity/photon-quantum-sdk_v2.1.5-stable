using Photon.Deterministic;
using System;

namespace Quantum
{
    partial struct Input
    {
        public FPVector2 Direction
        {
            get
            {
                if (EncodedDirection == default)
                    return default;

                Int32 angle = ((Int32)EncodedDirection - 1) * 2;

                return FPVector2.Rotate(FPVector2.Up, angle * FP.Deg2Rad);
            }
            set
            {
                if (value == default)
                {
                    EncodedDirection = default;
                    return;
                }

                var angle = FPVector2.RadiansSigned(FPVector2.Up, value) * FP.Rad2Deg;

                angle = (((angle + 360) % 360) / 2) + 1;

                EncodedDirection = (Byte)(angle.AsInt);
            }
        }
    }
}
