using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
	public Action<ItemBox> OnDropHandler;

	ItemBox _currentItemBox;
	List<Type> FilterType = new List<Type>();

	public void SetItemBox(ItemBox itemBox)
    {
		_currentItemBox = itemBox;
	}

	public bool TryAccept(GameObject item)
	{
		if(FilterType.Count > 0)
        {
            for (int i = 0; i < FilterType.Count; i++)
            {
				object component = item.GetComponent(FilterType[i]);
				if(component != null)
                {
					return true;
				}
			}

			return false;
		}

		return true;
	}

	public ItemBox NeedReplace()
    {
		if(!_currentItemBox.IsEmpty)
        {
			return _currentItemBox;
		}

		return null;
    }

	public void Drop(ItemBox itemBox)
	{
		OnDropHandler?.Invoke(itemBox);
	}

	public void AddFilter(Type filter)
    {
		FilterType.Add(filter);
	}

	public void ResetFilter()
	{
		FilterType.Clear();
	}
}
