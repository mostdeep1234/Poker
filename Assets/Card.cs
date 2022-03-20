using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    [Header("Score Card")]
    public int ScoreCard;

    [Header("Card ID")]
    public int cardId;

    [Header("Card Label")]
    public string cardLabel;

    [Header("Sprite List")]
    public List<Sprite> cardSprites = new List<Sprite>();

    [Header("Close Card Sprite")]
    public Sprite closeCardSprite;

    [Header("Sprite Renderer")]
    public SpriteRenderer spriteRenderer;

    public Animator animator;

    public bool addingCheckGraph;

    /// <summary>
    /// /////////////////////////////////////////////////////////////////
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        FillTheCardScore();

        animator = this.GetComponent<Animator>();

        StartCoroutine(CardDisable());
    }

    private void Update()
    {
        IsInDeckPos();
    }

    /// <summary>
    /// assign score card
    /// for ther card sets
    /// </summary>
    /// <param name="id"></param>
    public void AssignScoreCard(int id, int lastResult)
    {
        if (id != 2)
        {
            switch (cardLabel)
            {
                case GameControl.SPADES:
                    ScoreCard = id + 4 + lastResult;
                    break;
                case GameControl.HEARTS:
                    ScoreCard = id + 3 + lastResult;
                    break;
                case GameControl.CLUBS:
                    ScoreCard = id + 2 + lastResult;
                    break;
                case GameControl.DIAMONDS:
                    ScoreCard = id + 1 + lastResult;
                    break;
            }
        }
        else
        {
            switch (cardLabel)
            {
                case GameControl.SPADES:
                    ScoreCard = 15 + 4 + lastResult;
                    break;
                case GameControl.HEARTS:
                    ScoreCard = 15 + 3 + lastResult;
                    break;
                case GameControl.CLUBS:
                    ScoreCard = 15 + 2 + lastResult;
                    break;
                case GameControl.DIAMONDS:
                    ScoreCard = 15 + 1 + lastResult;
                    break;
            }
        }

        return;
    }

    IEnumerator CardDisable()
    {
        yield return new WaitUntil(() => this.transform.parent == GameControl.gameControl.player1Pos ||
              this.transform.parent == GameControl.gameControl.player2Pos || this.transform.parent == GameControl.gameControl.player3Pos ||
              this.transform.parent == GameControl.gameControl.player4Pos);

        yield return new WaitUntil(() => this.transform.parent == GameControl.gameControl.cardDeckPos);

        yield return new WaitUntil(() => this.transform.parent == null);

        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void IsInDeckPos ()
    {
        if (this.transform.parent == GameControl.gameControl.cardDeckPos)
            this.GetComponent<SpriteRenderer>().enabled = true;
    }

    /// <summary>
    /// fill the card score
    /// by searching the individual score
    /// </summary>
    public void FillTheCardScore()
    {
        switch (cardId)
        { case 3:
                AssignScoreCard(3, 0);
                break;
            case 4:
                AssignScoreCard(4, 7); // 7
                break;
            case 5:
                AssignScoreCard(5, 15);// 8
                break;
            case 6:
                AssignScoreCard(6, 24);// 9
                break;
            case 7:
                AssignScoreCard(7, 34);// 10
                break;
            case 8:
                AssignScoreCard(8, 45);// 11
                break;
            case 9:
                AssignScoreCard(9, 57); //12
                break;
            case 10:
                AssignScoreCard(10, 70); //13
                break;
            case 11:
                AssignScoreCard(11, 84); //14
                break;
            case 12:
                AssignScoreCard(12, 99); // 15
                break;
            case 13:
                AssignScoreCard(13, 115);//16
                break;
            case 14:
                AssignScoreCard(14, 132); //17
                break;
            //POKER
            case 2:
                AssignScoreCard(2, 150);//18
                break;
        }

        return;
    }

    //////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// hover sending event system
    /// </summary>
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && addingCheckGraph == false)
        {
            //check for the turn card
            if (Player.player.turnCheck && Player.player.throwCard.Count < 5)
            {
                Player.player.throwCard.Add(this);

                animator.enabled = true;

                addingCheckGraph = true;
            }
        }

        else if (Input.GetMouseButtonDown(1) && addingCheckGraph)
        {
            //check the turn of the card
            if (Player.player.turnCheck)
            {
                Player.player.throwCard.Remove(this);

                animator.enabled = false;

                addingCheckGraph = false;

                float r = 255;

                float g = 255;

                float b = 255;

                this.GetComponent<SpriteRenderer>().color = new Color(r, g, b, 255);
            }
        }
    }
}
