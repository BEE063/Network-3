using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PP_lab1
{
    public class Buffer
    {
        public BitArray frame { get; set; }
        public BitArray receipt { get; set; }
        public BitArray request { get; set; }
        public BitArray[] _frameArray;
        public Buffer()
        {

        }
        public bool CheckSum(BitArray array)
        {
            BitArray bitArray = new BitArray(8);
            for (int i = 0; i < 8; i++)
            {
                bitArray[i] = array[i];
            }
            int[] messageIntArray = new int[array.Length - 8];
            for (int i = bitArray.Length; i < messageIntArray.Length; i++)
            {
                if (array[i] == true)
                {
                    messageIntArray[i - bitArray.Length] = 1;
                }
                else
                {
                    messageIntArray[i - bitArray.Length] = 0;
                }
            }



            var checkStr = string.Join(" ", Array.ConvertAll(VerticalSum(ToTwoDimensionalArray(messageIntArray, array.Length / 8, 8)), x => x.ToString()));



            string[] checkArray = new string[bitArray.Length];
            for (int i = 0; i < checkArray.Length; i++)
            {
                if (bitArray[i] == true)
                {
                    checkArray[i] = "1";
                }
                else
                {
                    checkArray[i] = "0";
                }
            }

            string checkArrayString = string.Join(" ", checkArray);


            if (checkArrayString == checkStr)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static int[,] ToTwoDimensionalArray(int[] array, int height, int width)
        {
            int[,] a = new int[height, width];
            for (int i = 0, c = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++, c++)
                {
                    if (c == array.Length) return a;
                    a[i, j] = array[c];
                }
            }
            return a;
        }
        public static int[] VerticalSum(int[,] array)
        {
            int[] sumColumn = new int[8];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sumColumn[j] += array[i, j];
                }

            }
            for (int i = 0; i < sumColumn.Length; i++)
            {
                if (sumColumn[i] % 2 == 0)
                {
                    sumColumn[i] = 1;
                }
                else
                {
                    sumColumn[i] = 0;
                }
            }
            return sumColumn;
        }
        private static byte[] BitArrayToByteArray(BitArray array)
        {
            byte[] ret = new byte[(array.Length - 1) / 8 + 1];
            array.CopyTo(ret, 0);
            return ret;
        }
    }
}
