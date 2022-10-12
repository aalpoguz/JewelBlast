using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchController : MonoBehaviour
{
    Board board;
    public List<Jewels> FoundedJewelList = new List<Jewels>();

    private void Awake()
    {
        board = Object.FindObjectOfType<Board>();
    }

    public void FindMatches()
    {
        FoundedJewelList.Clear();

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Jewels validJewel = board.allJewels[x, y];

                if (validJewel != null)
                {
                    // X line match control
                    if (x > 0 && x < board.width - 1)
                    {
                        Jewels leftJewel = board.allJewels[x - 1, y];
                        Jewels rightJewel = board.allJewels[x + 1, y];
                        if (leftJewel != null && rightJewel != null)
                        {
                            if (leftJewel.type == validJewel.type && rightJewel.type == validJewel.type)
                            {
                                validJewel.isMatch = true;
                                leftJewel.isMatch = true;
                                rightJewel.isMatch = true;
                                FoundedJewelList.Add(validJewel);
                                FoundedJewelList.Add(leftJewel);
                                FoundedJewelList.Add(rightJewel);

                            }
                        }
                    }

                    // Y line match control
                    if (y > 0 && y < board.height - 1)
                    {
                        Jewels downJewel = board.allJewels[x, y - 1];
                        Jewels upJewel = board.allJewels[x, y + 1];

                        if (downJewel != null && upJewel != null)
                        {
                            if (downJewel.type == validJewel.type && upJewel.type == validJewel.type)
                            {
                                validJewel.isMatch = true;
                                downJewel.isMatch = true;
                                upJewel.isMatch = true;

                                FoundedJewelList.Add(validJewel);
                                FoundedJewelList.Add(downJewel);
                                FoundedJewelList.Add(upJewel);
                            }
                        }

                    }
                }
            }
        }//loops done

        if (FoundedJewelList.Count > 0)
        {
            FoundedJewelList = FoundedJewelList.Distinct().ToList();
        }
        FindTheBomb();
    }

    public void FindTheBomb()
    {
        for (int i = 0; i < FoundedJewelList.Count; i++)
        {
            Jewels jewels = FoundedJewelList[i];

            int x = jewels.posIndex.x;
            int y = jewels.posIndex.y;

            if (jewels.posIndex.x > 0)
            {
                if (board.allJewels[x - 1, y] != null)
                {
                    if (board.allJewels[x - 1, y].type == Jewels.JewelType.bomb)
                    {
                        BombSideMarked(new Vector2Int(x-1,y),board.allJewels[x-1,y] );
                    }
                }
            }

            if (jewels.posIndex.x >board.width-1)
            {
                if (board.allJewels[x + 1, y] != null)
                {
                    if (board.allJewels[x + 1, y].type == Jewels.JewelType.bomb)
                    {
                        BombSideMarked(new Vector2Int(x+ 1,y),board.allJewels[x+ 1,y] );
                    }
                }
            }

            if (jewels.posIndex.y > 0)
            {
                if (board.allJewels[x , y- 1] != null)
                {
                    if (board.allJewels[x, y - 1].type == Jewels.JewelType.bomb)
                    {
                        BombSideMarked(new Vector2Int(x,y-1),board.allJewels[x,y-1] );
                    }
                }
            }

            if (jewels.posIndex.y >board.height-1)
            {
                if (board.allJewels[x , y+ 1] != null)
                {
                    if (board.allJewels[x , y+ 1].type == Jewels.JewelType.bomb)
                    {
                        BombSideMarked(new Vector2Int(x,y+ 1),board.allJewels[x,y+ 1] );
                    }
                }
            }
        }
    }

    public void BombSideMarked(Vector2Int bombPos, Jewels bomb)
    {
        for (int x = bombPos.x - bomb.bombVolume; x <= bombPos.x + bomb.bombVolume; x++)
        {
            for (int y = bombPos.y - bomb.bombVolume; y < bombPos.y + bomb.bombVolume; y++)
            {
                if (x >= 0 && x < board.width - 1 && y >= 0 && y < board.height - 1)
                {
                    if(board.allJewels[x,y]!=null)
                    {
                        board.allJewels[x,y].isMatch = true;
                        FoundedJewelList.Add(board.allJewels[x,y]);
                    }// aynı elemanlardan farklı farklı geliyor distinct yapılacak
                }
            }
        }

        if(FoundedJewelList.Count>0)
        {
            FoundedJewelList = FoundedJewelList.Distinct().ToList();
        }
    }
}
