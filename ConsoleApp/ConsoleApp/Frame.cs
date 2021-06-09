using ConsoleApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PP_lab1
{
    public class Frame
    {
        public static BitArray[] GenerateData(byte[] bytes)
        {
            BitArray fileArray = new BitArray(bytes);

            int[] messageIntArray = new int[fileArray.Length];
            for (int i = 0; i < messageIntArray.Length; i++)
            {
                if (fileArray[i] == true)
                {
                    messageIntArray[i] = 1;
                }
                else
                {
                    messageIntArray[i] = 0;
                }
            }
            var messageInt = string.Join(" ", messageIntArray);
            var checkStr = string.Join(" ", Array.ConvertAll(VerticalSum(ToTwoDimensionalArray(messageIntArray, bytes.Length, 8)), x => x.ToString()));

            BitArray bitArray = new BitArray(8);
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (VerticalSum(ToTwoDimensionalArray(messageIntArray, bytes.Length, 8))[i] == 1)
                {
                    bitArray[i] = true;
                }
                else
                {
                    bitArray[i] = false;
                }
            }


            BitArray total = new BitArray(bitArray.Length + fileArray.Length + 17);
            for (int i = 1; i < 7; i++)
            {
                total[i] = true;
            }
            total[0] = false;
            total[7] = false;
            total[8] = true;
            for (int i = 9; i < bitArray.Length + 9; i++)
            {
                total[i] = bitArray[i - 9];
            }
            for (int i = 17; i < fileArray.Length+17; i++)
            {
                total[i] = fileArray[i - bitArray.Length - 9];
            }
            total[fileArray.Length + 16 + 1] = false;
            for (int i = fileArray.Length + 16 + 2; i < fileArray.Length + 16 + 8; i++)
            {
                total[i] = true;
            }
            total[fileArray.Length + 16 + 8] = false;

            double package = total.Length / 56;
            BitArray[] bitArrayMatrix = new BitArray[(int)(Math.Ceiling(package))];

            if (total.Length > 56)
            {
                Packages(total, (int)Math.Ceiling(package));
            }

            return Packages(total, (int)Math.Ceiling(package));
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
        public static BitArray[] Packages(BitArray array, int height)
        {

            BitArray[] bitArray = new BitArray[height];
            for (int i = 0, c = 0; i < height; i++)
            {
                BitArray ba = new BitArray(array.Count / height);
                for (int j = 0; j < array.Count / height; j++, c++)
                {
                    ba.Set(j, array[c]);
                }
                bitArray[i] = ba;
            }
            return bitArray;
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

        public static BitArray[] GenerateErrorData(byte[] bytes)
        {
            BitArray fileArray = new BitArray(bytes);

            int[] messageIntArray = new int[fileArray.Length];
            for (int i = 0; i < messageIntArray.Length; i++)
            {
                if (fileArray[i] == true)
                {
                    messageIntArray[i] = 1;
                }
                else
                {
                    messageIntArray[i] = 0;
                }
            }
            var messageInt = string.Join(" ", messageIntArray);
            var checkStr = string.Join(" ", Array.ConvertAll(VerticalSum(ToTwoDimensionalArray(messageIntArray, bytes.Length, 8)), x => x.ToString()));

            BitArray bitArray = new BitArray(8);
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (VerticalSum(ToTwoDimensionalArray(messageIntArray, bytes.Length, 8))[i] == 1)
                {
                    bitArray[i] = true;
                }
                else
                {
                    bitArray[i] = false;
                }
            }


            BitArray total = new BitArray(bitArray.Length + fileArray.Length + 17);
            for (int i = 1; i < 7; i++)
            {
                total[i] = true;
            }
            total[0] = false;
            total[7] = false;
            total[8] = true;
            for (int i = 9; i < bitArray.Length + 9; i++)
            {
                total[i] = bitArray[i - 9];
            }
            for (int i = 17; i < fileArray.Length + 17; i++)
            {
                total[i] = fileArray[i - bitArray.Length - 9];
            }
            for(int i=23; i<32; i++)
            {
                if (total[i] == true)
                {
                    total[i] = false;
                }
                else
                {
                    total[i] = true;
                }
            }
            total[fileArray.Length + 16 + 1] = false;
            for (int i = fileArray.Length + 16 + 2; i < fileArray.Length + 16 + 8; i++)
            {
                total[i] = false;
            }
            total[fileArray.Length + 16 + 8] = false;

            double package = total.Length / 56;
            BitArray[] bitArrayMatrix = new BitArray[(int)(Math.Ceiling(package))];

            if (total.Length > 56)
            {
                Packages(total, (int)Math.Ceiling(package));
            }

            return Packages(total, (int)Math.Ceiling(package));
        }

        private static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public static void GenerateReceipt(BitArray[] array, bool isValid)
        {
            BitArray bt = new BitArray(1);
            if (isValid == true)
            {
                bt[0] = true;
                array[0] = bt;
            }
            else
            {
                bt[0] = false;
                array[0] = bt;
            }
        }
        public static void GenerateRequest(BitArray[] array, bool isValid)
        {
            if (isValid == true)
            {
                array[0][0] = true;
            }
            else
            {
                array[0][0] = false;
            }
        }


    }
}

