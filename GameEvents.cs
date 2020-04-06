namespace ACT.DFAssist
{
	// occur events
	public enum GameEvents : int
	{
		None,

		InstanceEnter,  // [0] = instance code
		InstanceLeave,  // [0] = instance code

		FateOccur,      // [0] = fate code

		MatchQueue,     // [0] = MatchType, [1] = code, [...] = instances
		MatchDone,      // [0] = roulette code, [1] = instance code
		MatchInstance,  // [0] = instance code
		MatchCancel,
	}

	// match types
	public enum MatchType : int 
	{
		Roulette = 0,
		Assignment = 1,
	}

	// match expression style
	public enum MatchStyle : int 
	{
		Short,          // 5.1 이전
		Long,           // 5.1 부터
		Code
	}

	// match status
	public enum MatchStatus : int
	{
		Idle,
		Queue,
		Matched,
	}
}
