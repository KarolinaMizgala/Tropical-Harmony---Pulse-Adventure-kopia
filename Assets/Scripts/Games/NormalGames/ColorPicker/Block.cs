using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IPointerDownHandler
{
	private	Image			image;
	private	ColorPickerController	gameController;

	public Color Color
	{
		set => image.color = value;
		get => image.color;
	}

	public void Setup(ColorPickerController gameController)
	{
		image				= GetComponent<Image>();
		this.gameController	= gameController;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		gameController.CheckBlock(Color);
	}
}

