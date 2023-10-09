using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{

    public Color TextColor = Color.white;
    public float Speed = 0.4f;
    public float TimeAlive = 0.5f;
    public string Text = "0";
    public float FadeSpeed = 2.0f;

    private Text _Text;

    private void Start()
    {
        _Text = GetComponent<Text>();
        _Text.text = Text;
        _Text.color = TextColor;
    }

    // Update is called once per frame
    void Update()
    {

        TimeAlive -= Time.deltaTime;

        if (TimeAlive <= 0)
        {
            //fade out

            _Text.color = new Color(_Text.color.r,
                                _Text.color.g,
                                _Text.color.b,
                                _Text.color.a - (Time.deltaTime * FadeSpeed));

            if (_Text.color.a <= 0)
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        transform.position = new Vector3(transform.position.x,
                                        transform.position.y + (Speed * Time.deltaTime),
                                           transform.position.z);
    }
}
