using System;
using UnityEngine;

[System.Serializable]
public class Element : MonoBehaviour, IElement
{

	[SerializeField] string    m_Category = "Default";
	public           string    Id            => gameObject.name;
	public           string    category      => m_Category;
	public           Transform baseTransform => transform;

	public virtual void Simulate(bool value)
	{
		gameObject.SetActive(value);
	}
	
}


public interface IElement
{

	string    Id            { get; }
	string    category      { get; }
	Transform baseTransform { get; }

	void Simulate(bool value);

}


public interface IRepository
{

	IElement this[string key] { get; }

	void Init();

	IElement[] GetCategoryElements(string category);

	void ForEachCategory(string category, Action<IElement> task);

}