using UnityEngine;

public class Projectile : MonoBehaviour
{
    // projectile for both invaders and player
    public Vector3 direction;
    public float speed;
    public System.Action destroyed;

    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.destroyed != null)
        {
            this.destroyed.Invoke();
        }
        // destroy project whenever collision occurs
        Destroy(this.gameObject);
    }
}
