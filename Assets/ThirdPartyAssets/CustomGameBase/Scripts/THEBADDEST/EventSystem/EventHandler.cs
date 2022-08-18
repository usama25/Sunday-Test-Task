using UnityEngine;


namespace THEBADDEST
{


	public class EventHandler
	{

		//InApp Events

		public static EventListener<string>       OnBuyINAPP       = new EventListener<string>();
		public static EventCallBack<string, bool> HasPruchaseINAPP = new EventCallBack<string, bool>();
		public static EventListener<string>       OnInAPPSuccess   = new EventListener<string>();

		public static EventCallBack<string, string> GetPrice        = new EventCallBack<string, string>();
		public static EventListener                 RestoreINAPP    = new EventListener();
		public static EventListener<bool>           RestoreCallBack = new EventListener<bool>();
		
		public static EventListener<string> PlaySound= new EventListener<string>();
		

	}


}