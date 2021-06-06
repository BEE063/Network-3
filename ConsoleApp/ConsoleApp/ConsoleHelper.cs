using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public static class ConsoleHelper
    {
        public static object LockObject = new Object();
        public static void WriteTextMessageToConsole(string info, BitArray array)
        {
            lock (LockObject)
            {
                BitArray bitArray = new BitArray(8);
                for (int i = 0; i < 8; i++)
                {
                    bitArray[i] = array[i];
                }


                BitArray bitMessage = new BitArray(array.Length - 8);
                for (int i = 0; i < array.Length - 8; i++)
                {
                    bitMessage[i] = array[8 + i];
                }
                byte[] bytesMessageBack = BitArrayToByteArray(bitMessage);
                string textMessageBack = System.Text.Encoding.Unicode.GetString(bytesMessageBack);
                Console.WriteLine(info + textMessageBack);

            }
        }

        private static byte[] BitArrayToByteArray(BitArray array)
        {
            byte[] ret = new byte[(array.Length - 1) / 8 + 1];
            array.CopyTo(ret, 0);
            return ret;
        }

        public static void WriteToConsole(string info, string write)
        {
            lock (LockObject)
            {
                Console.WriteLine(info + " : " + write);
            }

        }
        public static void WriteToConsoleReceipt(string info, BitArray array)
        {
            lock (LockObject)
            {
                Console.Write(info + " : ");
                if (array[0] == true)
                    Console.WriteLine("Квитанция от другого потока - true");
                else
                    Console.WriteLine("Квитанция от другого потока - false");
            }
        }
        public static void WriteToConsoleRequest(string info, string type, BitArray array)
        {
            lock (LockObject)
            {
                if (array[0] == true && type == "connect")
                {
                    Console.WriteLine(info + " : Другой поток запрашивает соеденение");
                }
                else
                {
                    Console.WriteLine(info + " : Другой поток разрешает соеденение");
                }

            }
        }
        public static void WriteToConsoleDisconnect(string info, string type, BitArray array)
        {
            lock (LockObject)
            {
                if (array[0] == true && type == "disconnect")
                {
                    Console.WriteLine(info + " : Другой поток запрашивает разрыв подключения");
                }
                else
                {
                    Console.WriteLine(info + " : Другой поток разрешает разрыв подключения");
                }

            }
        }
        public static void WriteToConsoleArray(string info, BitArray array)
        {
            lock (LockObject)
            {
                Console.Write(info + " : ");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == true)
                        Console.Write("1");
                    else
                        Console.Write("0");

                }

                Console.WriteLine();

            }

        }
    }
}
