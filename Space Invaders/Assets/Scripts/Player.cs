using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public float speed = 5.0f;
    private bool _laserActive;
    private void Update()
    {
        // GetKey allows key to be held down for input
        // side to side movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
          this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
          this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }
        // shooting functionality
        // GetKeyDown prevents infinite shooting
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
          Shoot();
        }
    }

    private void Shoot()
    {
        if (!_laserActive) {
          Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
          projectile.destroyed += LaserDestroyed;
          _laserActive = true;
        }
    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
