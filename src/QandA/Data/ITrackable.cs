using System;

namespace QandA.Data
{
	public interface ITrackable
	{
		DateTime Created { get; set; }

		DateTime LastUpdated { get; set; }
	}
}