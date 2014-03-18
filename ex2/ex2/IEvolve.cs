using System;

namespace CS.Market
{
    public interface IEvolve
    {
        // Behaviour should only be supported for offset > TimeSpan.Zero
        void Evolve(TimeSpan offset);
    }
}