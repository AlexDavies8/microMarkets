using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    public Animator Animator => _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    int x, y;

    public void MoveTo(Vector2 position)
    {
        Vector2 difference = (position - (Vector2)transform.position).normalized;
        _animator.SetFloat("x", difference.x);
        _animator.SetFloat("y", difference.y);

        float fx = difference.x;
        float fy = difference.y;

        x = fx > 0.5f ? 1 : fx < -0.5f ? -1 : 0;
        y = fy > 0.5f ? 1 : fy < -0.5f ? -1 : 0;

        transform.position = position;
    }

    public void UpdateResource(SpriteRenderer resource, int index)
    {
        if (y == 0)
        {
            resource.transform.localPosition = new Vector2(-2 + (index % 3) * 2, (index / 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
        else if (x == 0)
        {
            resource.transform.localPosition = new Vector2(-1 + (index % 2) * 2, -1 + (index / 2) * 2) / 16f;
            resource.sortingOrder = 10 - index / 2;
        }
        else if (x == y)
        {
            resource.transform.localPosition = new Vector2(-2 + (index % 3) * 2, -2 + (index / 3) * 2 + (index % 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
        else
        {
            resource.transform.localPosition = new Vector2(-2 + (index % 3) * 2, 2 + (index / 3) * 2 - (index % 3) * 2) / 16f;
            resource.sortingOrder = 10 - index / 3;
        }
    }
}
