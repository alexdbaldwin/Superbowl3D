using System;

namespace Timing
{
	public class LapTime
	{
		private int lapTimeMin = 0;
		private float lapTimeSec = 0;
		public LapTime (int minutes, float seconds)
		{
			lapTimeMin = minutes;
			lapTimeSec = seconds;
		}
		
		public float GetLapTimeInSeconds()
		{
			return (lapTimeMin * 60) + lapTimeSec;
		}
		
		public int GetMinutes()
		{
			return lapTimeMin;
		}
		
		public float GetSeconds()
		{
			return lapTimeSec;
		}
		
		public string ToString()
		{
			return lapTimeMin + "." + lapTimeSec.ToString ("F2");
		}
	}
}

