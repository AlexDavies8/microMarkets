using System.Linq;
using UnityEngine;

public class SpriteEventSystem : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    [SerializeField] private LayerMask _eventLayers = 0;

    SpriteEventTrigger _underMouse;

    private void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var allUnderMouse = Physics2D.OverlapPointAll(mousePosition, _eventLayers.value);

        var underMouse = allUnderMouse.Select(x => x.GetComponent<SpriteEventTrigger>()).OrderBy(x => -x.Priority).FirstOrDefault();

        if (underMouse == null)
        {
            _underMouse?.MouseExit(mousePosition);
            _underMouse = null;
            return;
        }

        if (!underMouse) return;

        underMouse.MouseStay(mousePosition);

        if (_underMouse != underMouse)
        {
            underMouse.MouseEnter(mousePosition);
            _underMouse = underMouse;
        }

        if (Input.GetMouseButtonDown(0))
        {
            underMouse.MouseDown(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            underMouse.MouseUp(mousePosition);
        }
    }
}
