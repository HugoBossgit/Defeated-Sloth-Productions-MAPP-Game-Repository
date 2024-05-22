using UnityEngine;

public class CutFruit : MonoBehaviour
{
    [SerializeField] private GameObject genericParticle;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cut")
        {
            Vector2 contactPoint = collision.contacts[0].point;
            GameObject particleInstance = Instantiate(genericParticle, contactPoint, Quaternion.identity);

            Destroy(particleInstance, 0.5f);

            Destroy(this.gameObject);
            GameObject scoreText = GameObject.Find("ShowScore");
            scoreText.GetComponent<ShowScore_fruitNinja>().IncrementScore(1);
        }
    }

}