using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FurnitureType
{
    Refrigerator, Stove, Toilet, Type4, Type5, Type6
}

public enum FurnitureColor
{
    Red, Green, Yellow, Orange
}

public enum CellType
{
    Furniture, Exit, Empty
}

public struct Cell
{
    public CellType cType;
    public FurnitureType fType;
    public FurnitureColor fColor;
    //borders here
}

public class FieldModel : MonoBehaviour
{
    const int height = 10;
    const int width = 10;
    char[,] rawCells;
    public Cell[,] cells { private set; get; }
    //byte[,] walls

    void Awake()
    {
        GenerateRawFieldData(); //char generation for now, reading from file - later
        FillCellsFromRawData();
    }

    void GenerateRawFieldData()
    {
        System.Random rnd = new System.Random();
        rawCells = new char[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                rawCells[i, j] = '0';

        int numberOfColors = Enum.GetNames(typeof(FurnitureColor)).Length;
        int numberOfFurnitures = 3; //can be any, numberOfFurnitures should be >= numberOfColors

        
        for (int m = 0; m < numberOfFurnitures; m++)
            for (int k = 0; k < numberOfColors; k++)
            {
                int rndX, rndY;
                do
                {
                    rndX = rnd.Next(width);
                    rndY = rnd.Next(height);
                }
                while (rawCells[rndX, rndY] != '0');
                rawCells[rndX, rndY] = (char)(65 + (m * numberOfFurnitures) + k);
                //Debug.Log("Raw cell value: " + (65 + (k * numberOfColors) + m).ToString());
            }

        //generate exit
        do
        {
            int rndX = rnd.Next(width);
            int rndY = rnd.Next(height);
            if (rawCells[rndX, rndY] == '0')
                if ((rndX == 0) || (rndY == 0) || (rndX == width - 1) || (rndY == height - 1))
                {
                    rawCells[rndX, rndY] = '*';
                    break;
                }
        }
        while (true);
    }

    void FillCellsFromRawData()
    {
        cells = new Cell[width, height];
        int numberOfFurnitures = Enum.GetNames(typeof(FurnitureType)).Length;
        int numberOfColors = Enum.GetNames(typeof(FurnitureColor)).Length;

        for (int i = 0; i < rawCells.GetLength(0); i++)
            for (int j = 0; j < rawCells.GetLength(1); j++)
                switch (rawCells[i, j])
                {
                    case '0':
                        cells[i, j].cType = CellType.Empty;
                        break;
                    case '*':
                        cells[i, j].cType = CellType.Exit;
                        break;
                    default:
                        cells[i, j].cType = CellType.Furniture;
                        //char values from 65 to 88 (ASCII) - furniture
                        cells[i, j].fType = (FurnitureType)(((int)rawCells[i, j] - 65) / numberOfFurnitures);
                        //cells[i, j].fType = FurnitureType.Refrigerator;
                        cells[i, j].fColor = (FurnitureColor)(((int)rawCells[i, j] - 65) % numberOfColors);
                        break;
                }
    }

    void Update()
    {
        
    }
}
