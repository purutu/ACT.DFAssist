namespace ACT.DFAssist
{
	// occur events
	public enum GameEvents
	{
		None,

		InstanceEnter,	// [0] = instance code
		InstanceLeave,	// [0] = instance code

		FateOccur,		// [0] = fate code

		MatchQueue,		// [0] = MatchType, [1] = code, [...] = instances
		MatchDone,		// [0] = roulette code, [1] = instance code
		MatchEnter,     // [0] = instance code
		MatchCancel,
	}

	// match types
	public enum MatchType
	{
		Roulette = 0,
		Assignment = 1,
	}

	// match expression style
	public enum MatchStyle
	{
		Short,			// 5.1 이전
		Long,			// 5.1 부터
		Code
	}
}
