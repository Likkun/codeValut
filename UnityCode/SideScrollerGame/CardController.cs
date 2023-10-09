using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{

    //Event for card flipping.
    public delegate void CardFlipped( bool ShowFront, CARD_FRONT type );
    public static event CardFlipped OnCardFlipped;

    public float x, y, z;
    public int timer;


    public bool isFlipping = false;
    public bool ShowFront = false;
    public bool forceFaceDown = false;

    public Sprite FrontHeart;
    public Sprite FrontKnife;
    public Sprite FrontCry;


    public enum CARD_FRONT{
        HEART = 0,
        KNIFE,
        CRY
	}

    public CARD_FRONT myFront = 0;

    public Sprite CardBack;

	private void OnEnable()
	{
        GameManager.OnClearMatchedCards += HandleClearCardType;
        GameManager.OnAllCardsFaceDown += FaceDown;
	}

	private void OnDisable()
	{
        GameManager.OnClearMatchedCards -= HandleClearCardType;
        GameManager.OnAllCardsFaceDown += FaceDown;
    }

	public void StartFlip()
	{
        if (!isFlipping)
        {
            isFlipping = true;
            StartCoroutine(CalculateFlip());
        }
    }


    public void FaceDown()
	{
        if (ShowFront == true)
        {
            isFlipping = true;
            
            StartCoroutine(CalculateFlip());
        }
    }

    //actually "FLIP" the card images
    public void Flip()
	{
        if( CardBack != null )
		{
			if (!ShowFront)
			{
				//this.GetComponent<Image>().sprite = CardFront;
				switch (myFront)
				{
                    case CARD_FRONT.HEART:
                        this.GetComponent<Image>().sprite = FrontHeart;
                        break;
                    case CARD_FRONT.KNIFE:
                        this.GetComponent<Image>().sprite = FrontKnife;
                        break;
                    case CARD_FRONT.CRY:
                        this.GetComponent<Image>().sprite = FrontCry;
                        break;
                }
            }
			else
			{
                this.GetComponent<Image>().sprite = CardBack;
            }
            ShowFront = !ShowFront;            
		}
	}

    //Coroutine to rotate the card
    IEnumerator CalculateFlip()
	{
        for( int i = 0; i < 180; i++)
		{
            yield return new WaitForSeconds(0.0001f);
            transform.Rotate(new Vector3(x, y, z));

            timer+=1;
            if(timer == -90 || timer == 90)
			{
                Flip();
			}

		}
        yield return new WaitForSeconds(0.25f);
        OnCardFlipped(ShowFront, myFront);
        timer = 0;
        isFlipping = false;
        forceFaceDown = false;

    }

    /// <summary>
    /// Handle an event call to clear cards of a certain type.
    /// Check if this card is that type, if so destroy it.
    /// </summary>
    /// <param name="ClearType">Type of card to clear</param>
    public void HandleClearCardType(CARD_FRONT ClearType)
	{
        if( myFront == ClearType)
		{
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
		}
	}

}
