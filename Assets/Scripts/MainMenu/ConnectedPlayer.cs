using System;

namespace AssemblyCSharp
{
	public class ConnectedPlayer
	{
		PlayerState currentPlayerState;
		string name = "";
		public ConnectedPlayer (string name)
		{
			this.name = name;
			currentPlayerState = PlayerState.OBSERVER;
		}
		
		public String GetName()
		{
			return name;
		}
		
		public PlayerState GetPlayerState()
		{
			return currentPlayerState;
		}
		
		public void SetPlayerState(PlayerState newPlayerState)
		{
			currentPlayerState = newPlayerState;
		}
	}
	
	public enum PlayerState { PLAYERONE, PLAYERTWO, OBSERVER };
}

