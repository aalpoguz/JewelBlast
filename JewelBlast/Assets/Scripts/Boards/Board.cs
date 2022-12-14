using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject tilePrefab;
    public Jewels[] jewels;
    public Jewels[,] allJewels; // 2 boyutlu dizi
    public float jewelSpeed;
    public MatchController matchController;

    public enum BoardStatus { waiting, moving };

    public BoardStatus currentStatus = BoardStatus.moving;


    public Jewels bomb;
    public float bombChance = 2f;


    private void Awake()
    {
        matchController = Object.FindObjectOfType<MatchController>();
    }

    private void Start()
    {
        allJewels = new Jewels[width, height];



        EditFunction();
    }



    public void EditFunction()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject tileBg = Instantiate(tilePrefab, pos, Quaternion.identity);
                tileBg.transform.parent = this.transform;

                tileBg.name = "Tile_BG - " + x + ", " + y;

                int randomJewels = Random.Range(0, jewels.Length);

                int controlCounter = 0;
                while (isMatchFunction(new Vector2Int(x, y), jewels[randomJewels]) && controlCounter < 100)
                {
                    randomJewels = Random.Range(0, jewels.Length);

                    controlCounter++;
                    if (controlCounter > 0)
                    {
                        print(controlCounter);
                    }
                }


                createJewels(new Vector2Int(x, y), jewels[randomJewels]);

            }
        }
    }

    public void createJewels(Vector2Int pos, Jewels generateJewels)
    {
        if (Random.Range(0f, 100f) < bombChance)
        {
            generateJewels = bomb;
        }

        Jewels jewel = Instantiate(generateJewels, new Vector3(pos.x, pos.y + height, 0f), Quaternion.identity);
        jewel.transform.parent = this.transform;
        jewel.name = "Jewels - " + pos.x + ", " + pos.y;

        allJewels[pos.x, pos.y] = jewel;
        jewel.EditJewel(pos, this);
    }

    bool isMatchFunction(Vector2Int posControl, Jewels controlledJewel)
    {

        if (posControl.x > 1)
        {
            if (allJewels[posControl.x - 1, posControl.y].type == controlledJewel.type && allJewels[posControl.x - 2, posControl.y].type == controlledJewel.type)
            {
                return true;
            }
        }

        if (posControl.y > 1)
        {
            if (allJewels[posControl.x, posControl.y - 1].type == controlledJewel.type && allJewels[posControl.x, posControl.y - 2].type == controlledJewel.type)
            {
                return true;
            }
        }



        return false;
    }

    void MatchedJewelDestroy(Vector2Int pos)
    {
        if (allJewels[pos.x, pos.y] != null)
        {
            if (allJewels[pos.x, pos.y].isMatch)
            {

                if(allJewels[pos.x, pos.y].type == Jewels.JewelType.bomb)
                {
                    SoundManager.instance.blastVoiceEff();
                }
                else
                {
                    SoundManager.instance.jewelVoiceEff();
                }
                

                Instantiate(allJewels[pos.x, pos.y].jewelEffect, new Vector2(pos.x, pos.y), Quaternion.identity);

                Destroy(allJewels[pos.x, pos.y].gameObject);
                allJewels[pos.x, pos.y] = null;
            }
        }
    }

    public void allMatchesDestroy()
    {
        for (int i = 0; i < matchController.FoundedJewelList.Count; i++)
        {
            if (matchController.FoundedJewelList[i] != null)
            {
                UIManager.instance.increaseScore(matchController.FoundedJewelList[i].scoreValue);
                MatchedJewelDestroy(matchController.FoundedJewelList[i].posIndex);
            }
        }

        StartCoroutine(SlideDown());
    }

    IEnumerator SlideDown()
    {
        yield return new WaitForSeconds(0.2f);

        int emptyCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allJewels[x, y] == null)
                {
                    emptyCounter++;
                }
                else if (emptyCounter > 0)
                {
                    allJewels[x, y].posIndex.y -= emptyCounter;
                    allJewels[x, y - emptyCounter] = allJewels[x, y];
                    allJewels[x, y] = null;
                }
            }

            emptyCounter = 0;
        }

        StartCoroutine(FillTheBoardAgain());
    }

    IEnumerator FillTheBoardAgain()
    {
        yield return new WaitForSeconds(.5f);
        FillUpSpaces();

        yield return new WaitForSeconds(.5f);
        matchController.FindMatches();

        if (matchController.FoundedJewelList.Count > 0)
        {
            yield return new WaitForSeconds(.5f);
            allMatchesDestroy();
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            currentStatus = BoardStatus.moving;
        }
    }

    void FillUpSpaces()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allJewels[x, y] == null)
                {
                    int randomJewels = Random.Range(0, jewels.Length);
                    createJewels(new Vector2Int(x, y), jewels[randomJewels]);
                }

            }
        }

        WrongPlacesControl();
    }

    void WrongPlacesControl()
    {
        List<Jewels> foundedJewelList = new List<Jewels>();

        foundedJewelList.AddRange(FindObjectsOfType<Jewels>()); //sahnede el ile olu??turulmu?? olabilen m??cevherleri i??ine almak.

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (foundedJewelList.Contains(allJewels[x, y]))//alljewels xysini i??eriyorsa
                {
                    foundedJewelList.Remove(allJewels[x, y]);
                }
            }
        }

        foreach (Jewels jewels in foundedJewelList)
        {
            Destroy(jewels.gameObject);
        }
    }

    public void BoardMix()
    {
        if (currentStatus != BoardStatus.waiting)
        {
            currentStatus = BoardStatus.waiting;

            List<Jewels> sceneJewelsList = new List<Jewels>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    sceneJewelsList.Add(allJewels[x, y]);
                    allJewels[x, y] = null;


                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int willUseJewel = Random.Range(0, sceneJewelsList.Count);
                    
                    int controlCounter = 0; //while kontrol??

                    while (isMatchFunction(new Vector2Int(x, y), sceneJewelsList[willUseJewel]) && controlCounter<100 && sceneJewelsList.Count>1)
                     {
                        willUseJewel = Random.Range(0, sceneJewelsList.Count);

                        controlCounter++;
                     }

                     sceneJewelsList[willUseJewel].EditJewel(new Vector2Int(x,y), this);
                     allJewels[x,y] = sceneJewelsList[willUseJewel];
                     sceneJewelsList.RemoveAt(willUseJewel);
                }
            }
            StartCoroutine(SlideDown());
        }
    }

}
