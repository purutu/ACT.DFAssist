using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT.DFAssist
{
    public enum GameEvents
    {
        None,
        InstanceEnter, // [0] = instance code
        InstanceLeave, // [0] = instance code

        FateBegin,     // [0] = fate code
        FateProgress,  // [0] = fate code, [1] = progress
        FateEnd,       // [0] = fate code, [1] = status(?)

        MatchBegin,    // [0] = match type(0,1), [1] = roulette code or instance count, [...] = instance
        MatchStatus,   // [0] = instance code, [1] = status, [2] = tank, [3] = healer, [4] = dps
        MatchDone,     // [0] = roulette code, [1] = instance code
        MatchEnd,      // [0] = end reason <MatchEndType>

        MatchCancel,
        MatchCount,    // [0] = count
    }

    public enum MatchResult
    {
        Cancel = 0,
        Enter = 1,
    }

    public enum MatchType
    {
        Roulette = 0,
        Assignment = 1,
    }

    public enum State
    {
        Idle,
        Queued,
        Matched,
    }
}
