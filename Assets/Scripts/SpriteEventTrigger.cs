using UnityEngine;
using UnityEngine.Events;

public class SpriteEventTrigger : MonoBehaviour
{
    [SerializeField] private Vector2UnityEvent OnMouseEnter = null;
    [SerializeField] private Vector2UnityEvent OnMouseExit = null;
    [SerializeField] private Vector2UnityEvent OnMouseDown = null;
    [SerializeField] private Vector2UnityEvent OnMouseStay = null;
    [SerializeField] private Vector2UnityEvent OnMouseUp = null;

    public void MouseEnter(Vector2 mousePosition) => OnMouseEnter.Invoke(mousePosition);
    public void MouseExit(Vector2 mousePosition) => OnMouseExit.Invoke(mousePosition);
    public void MouseDown(Vector2 mousePosition) => OnMouseDown.Invoke(mousePosition);
    public void MouseStay(Vector2 mousePosition) => OnMouseStay.Invoke(mousePosition);
    public void MouseUp(Vector2 mousePosition) => OnMouseUp.Invoke(mousePosition);
}
