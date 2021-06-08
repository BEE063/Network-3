using PP_lab1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using ThreadsEdu;
using Buffer = PP_lab1.Buffer;

namespace ConsoleApp
{
    public class FourthThread
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private Semaphore _secondFileSemaphore;
        private BitArray[] _receivedMessage;
        private BitArray[] _sendMessage;
        private BitArray[] _sendReceipt;
        private PostToThirdWT _post;

        public FourthThread(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore,  ref Semaphore secondFileSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
            _secondFileSemaphore = secondFileSemaphore;
        }
        public void FourthThreadMain(Object obj)
        {
            _post = (PostToThirdWT)obj;
            ConsoleHelper.WriteToConsole("4 поток", "Начинаю работу.Жду передачи данных.");
            //1
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleRequest("4 поток", "connect", _receivedMessage);
            _sendReceipt = new BitArray[1];
            Frame.GenerateReceipt(_sendReceipt, true);
            _post(_sendReceipt);

            _sendSemaphore.Release();
            //2
            _receiveSemaphore.WaitOne();

            Buffer buffer = new Buffer();
            ConsoleHelper.WriteToConsole("4 поток", "Данные полученны");
            ConsoleHelper.WriteToConsoleMatrixBitArray("4 поток", _receivedMessage);
            ConsoleHelper.WriteTextMessageToConsole("4 поток: ", _receivedMessage);
            bool check = buffer.CheckSum(_receivedMessage);
            Frame.GenerateReceipt(_sendReceipt, check);
            _post(_sendReceipt);

            _sendSemaphore.Release();
            //3
            _receiveSemaphore.WaitOne();

            if (check == false)
            {
                ConsoleHelper.WriteToConsoleMatrixBitArray("4 поток", _receivedMessage);
                ConsoleHelper.WriteTextMessageToConsole("4 поток переданный текст: ", _receivedMessage);
                Frame.GenerateReceipt(_sendReceipt, buffer.CheckSum(_receivedMessage));
            }

            _sendSemaphore.Release();
            //4
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleRequest("4 поток", "", _receivedMessage);
            ConsoleHelper.WriteToConsole("4 поток", "Подготавливаю данные.");
            FileStream fls;
            string data;
            _secondFileSemaphore.WaitOne();
            fls = new FileStream("C:/Users/Ekate/Downloads/ConsoleApp/2.txt", FileMode.Open);
            StreamReader fstr_in = new StreamReader(fls);
            data = fstr_in.ReadLine();
            fstr_in.Close();
            _secondFileSemaphore.Release();
            _post(Frame.GenerateErrorData(data));

            _sendSemaphore.Release();
            ////5
            _receiveSemaphore.WaitOne();

            ResendData(_post, _receivedMessage);

            _sendSemaphore.Release();
            //6
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleReceipt("4 поток", _receivedMessage);
            ConsoleHelper.WriteToConsoleDisconnect("4 поток", "disconnect", _receivedMessage);
            Frame.GenerateRequest(_sendReceipt, true);
            _post(_sendReceipt);
            ConsoleHelper.WriteToConsole("4 поток", "Заканчиваю работу");

            _sendSemaphore.Release();

        }
        public void ReceiveData(BitArray[] array)
        {
            _receivedMessage = array;
        }
        public void ResendData(PostToThirdWT _postTo, BitArray[] array)
        {
            if (array[0][0] == false || array == null)
            {
                ConsoleHelper.WriteToConsoleReceipt("4 поток", _receivedMessage);
                ConsoleHelper.WriteToConsole("4 поток", "Отправляю повторно");
                FileStream fls;
                string data;
                _secondFileSemaphore.WaitOne();
                fls = new FileStream("C:/Users/Ekate/Downloads/ConsoleApp/2.txt", FileMode.Open);
                StreamReader fstr_in = new StreamReader(fls);
                data = fstr_in.ReadLine();
                fstr_in.Close();
                _secondFileSemaphore.Release();
                _postTo(Frame.GenerateData(data));

            }

        }
    }
}

