using System.Collections.Generic;

namespace ACT.DFAssist
{
    static class DataModel
    {
        public enum EventType
        {
            NONE,
            INSTANCE_ENTER, // [0] = instance code
            INSTANCE_EXIT,  // [0] = instance code

            FATE_BEGIN,     // [0] = fate code
            FATE_PROGRESS,  // [0] = fate code, [1] = progress
            FATE_END,       // [0] = fate code, [1] = status(?)

            // 사용자 요청
            MATCH_BEGIN,    // [0] = match type(0,1), [1] = roulette code or instance count, [...] = instance

            // 매치 상태
            MATCH_PROGRESS, // [0] = instance code, [1] = status, [2] = tank, [3] = healer, [4] = dps

            // 매치됨
            MATCH_ALERT,    // [0] = roulette code, [1] = instance code

            // 매치끗
            MATCH_END,      // [0] = end reason <MatchEndType>
        }

        public class Area
        {
            public string Name { get; set; }
            public IReadOnlyDictionary<int, Fate> Fates { get; set; }
        }

        public class Fate
        {
            public Area Area { get; set; }
            public string Name { get; set; }

            public static explicit operator Fate(string name)
            {
                return new Fate { Name = name };
            }
        }

        public class GameData
        {
            public decimal Version { get; set; }
            public Dictionary<int, Instance> Instances { get; set; }
            public Dictionary<int, Roulette> Roulettes { get; set; }
            public Dictionary<int, Area> Areas { get; set; }
        }

        public class Instance
        {
            public int OldId { get; set; }
            public string Name { get; set; }
            public byte Tank { get; set; }
            public byte Healer { get; set; }
            public byte Dps { get; set; }
            public bool PvP { get; set; }
        }

        public class Roulette
        {
            public string Name { get; set; }

            public static explicit operator Roulette(string name)
            {
                return new Roulette { Name = name };
            }
        }

        public class Language
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }
    }
}
