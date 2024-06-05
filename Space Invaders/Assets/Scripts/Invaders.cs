using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public Projectile missilePrefab;
    public float missileAttackRate = 1.0f;
    public int amountKilled { get; private set; }
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float) this.amountKilled / (float) this.totalInvaders;
    private Vector3 _direction = Vector2.right;

    private void Awake()
    {
      // instantiate all invaders
      for (int row = 0; row < this.rows; row++)
      {
        // add offset so invaders are in the middle
        float width = 2.0f * (this.columns - 1);
        float height = 2.0f * (this.rows - 1);
        Vector2 centering = new Vector2(-width / 2, -height/2);
        // set position of each invader
        Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);

        for (int col = 0; col < this.columns; col++)
        {
          Invader invader = Instantiate(this.prefabs[row], this.transform);
          invader.killed += InvaderKilled;
          Vector3 position = rowPosition;
          // columns are x axis
          // rows are y axis
          // 2.0f is hard coded as padding
          // where invader is 1 unit of space and 1 unit of padding
          position.x += col * 2.0f;
          invader.transform.localPosition = position;
        }
      }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    // update called every frame
    private void Update()
    {
      // need to know speed and direction then update position
      this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

      Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
      Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

      // change direction when edge of screen is hit
      foreach (Transform invader in this.transform)
      {
        // check invader is disable or not
        if (!invader.gameObject.activeInHierarchy)
        {
            continue;
        }

        // check left or right edge
        if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f))
        {
            AdvanceRow();
        } else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f))
        {
            AdvanceRow();
        }
      }
    }

    private void AdvanceRow()
    {
      _direction.x *= -1.0f;

      Vector3 position = this.transform.position;
      position.y -= 1.0f;
      this.transform.position = position;
    }

    private void MissileAttack()
    {
      // increase missile attacks are invaders decrease
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            // increases percentage of spawning missle
            if (Random.value < (1.0f / (float) this.amountAlive))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void InvaderKilled()
    {
        this.amountKilled++;
        // reset game state when all invaders killed
        if (this.amountKilled >= this.totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
