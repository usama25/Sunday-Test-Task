using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace GameAssets.GameSet.GameDevUtils.Managers
{


	[System.Serializable]
	public class GameStateScreens
	{
		public GameState  stateType;
		public GameObject panel;
	}


	public class UIManager : MonoBehaviour
	{
		public Text gpLevelText;
		public Text mmLevelText;
		
	
		[SerializeField] private GameStateScreens[] gameStateScreens;


		public void EnableUIScreen(GameState state, float delay)
		{
			StartCoroutine(EnableScreenWithDelay(state, delay));
		}

		IEnumerator EnableScreenWithDelay(GameState gameState, float delay)
		{
			
			yield return new WaitForSeconds(delay);
			
			foreach (GameStateScreens stateScreen in gameStateScreens)
			{
				stateScreen.panel.SetActive(stateScreen.stateType == gameState);
			}

			UpdateLevel(GameManager.Instance.InfinityCurrentLevel);
		}

		void UpdateLevel(int levelNumber)
		{
			gpLevelText.text = $"Level {levelNumber}";
			mmLevelText.text = $"Level {levelNumber}";

		}

	}
	
	
	
	
}