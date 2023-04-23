using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite[] _bubbleSprites;



    [SerializeField]
    private float _period = 1.0f; // five seconds for a full cycle of small to large
    private int _spriteIndex = 0;

    private float _spriteTimer = 0.0f;

    [SerializeField]
    private bool _inRange = false;

    [SerializeField]
    private const float PERFECT_TIME_BONUS = 0.25f;

    [SerializeField]
    private const float BAD_TIMINING_PENALTY = -0.25f;

    void Start()
    {
        _spriteTimer = (_period / (_bubbleSprites.Length - 1));
    }

    void Update()
    {
        UpdateSprite();
        
        _inRange = _spriteIndex >= _bubbleSprites.Length - 1;
        //_renderer.color = _inRange ? Color.cyan : Color.white;

    }

    public bool IsInRange()
    {
        return _inRange;
    }

    public float GetTimingBonus () => PERFECT_TIME_BONUS;

    public float GetTimingPenalty () => BAD_TIMINING_PENALTY;

    private void UpdateSprite()
    {
        _spriteTimer -= Time.deltaTime;

        if (_spriteTimer <= 0.0f)
        {
            _spriteTimer = (_period / (_bubbleSprites.Length - 1));

            _spriteIndex += 1;
            if (_spriteIndex > _bubbleSprites.Length - 1)
            {
                _spriteIndex = 0;
            }

            _renderer.sprite = _bubbleSprites[_spriteIndex];
        }
    }
}
