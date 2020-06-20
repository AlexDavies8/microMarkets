using UnityEngine;

public class LineRendererCopy : MonoBehaviour
{
    [SerializeField] private LineRenderer _source = null;
    [SerializeField] private LineRenderer _destination = null;

    private void Update()
    {
        _destination.positionCount = _source.positionCount;
        Vector3[] positions = new Vector3[_source.positionCount];
        _source.GetPositions(positions);
        _destination.SetPositions(positions);
    }
}
