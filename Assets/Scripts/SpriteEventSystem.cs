using UnityEngine;

public class SpriteEventSystem : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    [SerializeField] private LayerMask _eventLayers = 0;

    Collider2D _underMouse;

    private void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var underMouse = Physics2D.OverlapPoint(mousePosition, _eventLayers.value);

        if (underMouse == null)
        {
            _underMouse?.GetComponent<SpriteEventTrigger>().MouseExit(mousePosition);
            _underMouse = null;
            return;
        }

        var eventTrigger = underMouse.GetComponent<SpriteEventTrigger>();
        if (!eventTrigger) return;

        eventTrigger.MouseStay(mousePosition);

        if (_underMouse != underMouse)
        {
            eventTrigger.MouseEnter(mousePosition);
            _underMouse = underMouse;
        }

        if (Input.GetMouseButtonDown(0))
        {
            eventTrigger.MouseDown(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            eventTrigger.MouseUp(mousePosition);
        }
    }
}
