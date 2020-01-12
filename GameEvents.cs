namespace ACT.DFAssist
{
	// 발생 이벤트 
	public enum GameEvents
	{
		None,

		InstanceEnter,	// [0] = instance code
		InstanceLeave,	// [0] = instance code

		FateOccur,		// [0] = fate code

		MatchQueue,		// [0] = MatchType, [1] = code, [...] = instances
		MatchDone,		// [0] = roulette code, [1] = instance code
		MatchCancel,
	}

	// 매치 타입
	public enum MatchType
	{
		Roulette = 0,
		Assignment = 1,
	}

	// 매치 표현 형태
	public enum MatchStyle
	{
		Short,			// 5.1 이전
		Long,			// 5.1 부터
		Code
	}
}
