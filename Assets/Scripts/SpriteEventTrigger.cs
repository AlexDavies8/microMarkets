using UnityEngine;
using UnityEngine.Events;

public class SpriteEventTrigger : MonoBehaviour
{
    [SerializeField] private int _priority = 0;
    public int Priority => _priority;

    [SerializeField] private Vector2UnityEvent _onMouseEnter = null;
    public Vector2UnityEvent OnMouseEnter => _onMouseEnter;
    [SerializeField] private Vector2UnityEvent _onMouseExit = null;
    public Vector2UnityEvent OnMouseExit => _onMouseExit;
    [SerializeField] private Vector2UnityEvent _onMouseDown = null;
    public Vector2UnityEvent OnMouseDown => _onMouseDown;
    [SerializeField] private Vector2UnityEvent _onMouseStay = null;
    public Vector2UnityEvent OnMouseStay => _onMouseStay;
    [SerializeField] private Vector2UnityEvent _onMouseUp = null;
    public Vector2UnityEvent OnMouseUp => _onMouseUp;

    public void MouseEnter(Vector2 mousePosition) => OnMouseEnter.Invoke(mousePosition);
    public void MouseExit(Vector2 mousePosition) => OnMouseExit.Invoke(mousePosition);
    public void MouseDown(Vector2 mousePosition) => OnMouseDown.Invoke(mousePosition);
    public void MouseStay(Vector2 mousePosition) => OnMouseStay.Invoke(mousePosition);
    public void MouseUp(Vector2 mousePosition) => OnMouseUp.Invoke(mousePosition);
}
