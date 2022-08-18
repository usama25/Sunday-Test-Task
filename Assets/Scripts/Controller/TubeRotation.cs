using GameAssets.GameSet.GameDevUtils.Managers;
using UnityEngine;



public class TubeRotation : MonoBehaviour
{

	[SerializeField] float speed;
	CenterDependentInput    input;
	float                  rotated;
	float                  oldDigree;


	void Start()
	{
		input = new CenterDependentInput();
	}


	void Update()
	{
		if(GameManager.Instance.GameCurrentState != GameState.Gameplay)
			return;
		
		
		input.Calculate();
		rotated            += speed * input.value;
		transform.rotation =  Quaternion.Euler(0, 0, -rotated);
	}

}