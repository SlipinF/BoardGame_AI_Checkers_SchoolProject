using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPlayerBoard
{
    public static TileType[,] states = new TileType[Game_plan.maxCol, Game_plan.maxRow] {
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.invalid,TileType.green,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.invalid,TileType.green,TileType.green,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.green,TileType.green,TileType.green,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.green,TileType.green,TileType.green,TileType.green,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.blue,TileType.blue,TileType.blue,TileType.blue, TileType.green,TileType.green,TileType.green,TileType.green,TileType.green,TileType.yellow,TileType.yellow,TileType.yellow,TileType.yellow},
      {TileType.invalid,TileType.blue,TileType.blue,TileType.blue, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.yellow,TileType.yellow,TileType.yellow},
      {TileType.invalid,TileType.blue,TileType.blue,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.yellow,TileType.yellow,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.blue,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.yellow,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.empty,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.purple,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.orange,TileType.invalid},
      {TileType.invalid,TileType.purple,TileType.purple,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.orange,TileType.orange,TileType.invalid},
      {TileType.invalid,TileType.purple,TileType.purple,TileType.purple, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.orange,TileType.orange,TileType.orange},
      {TileType.purple,TileType.purple,TileType.purple,TileType.purple, TileType.red,TileType.red,TileType.red,TileType.red,TileType.red,TileType.orange,TileType.orange,TileType.orange,TileType.orange},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.red,TileType.red,TileType.red,TileType.red,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.red,TileType.red,TileType.red,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.invalid,TileType.red,TileType.red,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.invalid,TileType.red,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      };
}
