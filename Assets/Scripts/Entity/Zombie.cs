
using UnityEngine;


public class Zombie : Entity {
    // Zombie can aware the agent in any distance
   
    private void Start() {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update() {
        RotateTowardTarget();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(other.gameObject); // DoDamage
            Target = null;
            HumanPlaySceneManager.manager.GameOver();
        }
        else if (other.gameObject.CompareTag("Bullet") ){
            Destroy(other.gameObject);
            Destroy(gameObject);
            HumanPlaySceneManager.manager.IncreaseScore(1);
        }
    }

}
