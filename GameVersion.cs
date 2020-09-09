namespace ACT.DFAssist
{
	class GameVersion
	{
		public int Index { get; set; }
		public string Name { get; set; }

		//
		public GameVersion()
		{
		}

		//
		public GameVersion(int index, string name)
		{
			Index = index;
			Name = name;
		}

		// 버전 목록
		public static readonly GameVersion[] Versions = new GameVersion[]
		{
			/* 20200909 */ new GameVersion(0, "5.31"),
			/* 20200825 */ new GameVersion(0, "5.30HF"),
			/* 20200811 */ new GameVersion(0, "5.30"),
			/* 20200330 */ new GameVersion(9, "5.25"),
			/* 20200330 */ new GameVersion(8, "5.21HF"),
			/* 20200218 */ new GameVersion(7, "5.2"),
			/* 20191224 */ new GameVersion(6, "5.18"),
			/* 20191210 */ new GameVersion(5, "5.15"),
			/* 20191126 */ new GameVersion(4, "5.11HF"),
			/* 20191111 */ new GameVersion(3, "5.11"),
			/* 20191029 */ new GameVersion(2, "5.1"),
			/* 20190628 */ new GameVersion(1, "<= 5.0"),
		};
	}
}
