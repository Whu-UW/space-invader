using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;

    public float animationTime = 1.0f;

    public System.Action killed;

    private SpriteRenderer _spriteRenderer;

    private int _animationFrame;

    private void Awake()
    {
      _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
      InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    // update what frame we are one
    private void AnimateSprite()
    {
      _animationFrame++;

      // check that our current frame doesn't exceed provided sprites
      // loop back to beginning if it does exceed
      if (_animationFrame >= this.animationSprites.Length)
      {
          _animationFrame = 0;
      }

      _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      // check if object is laser
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
