using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UPDClientChat
{
    internal class UDPChat
    {
        IPAddress localAddress;
        string? username;
        int localPort;
        int remotePort;

        public void handlerChat()
        {
            localAddress = IPAddress.Parse("127.0.0.1");
            Console.Write("Введите свое имя: ");
            username = Console.ReadLine();
            Console.Write("Введите порт для приема сообщений: ");
            if (!int.TryParse(Console.ReadLine(), out localPort)) return;
            Console.Write("Введите порт для отправки сообщений: ");
            if (!int.TryParse(Console.ReadLine(), out remotePort)) return;
            Console.WriteLine();

            // запускаем получение сообщений
            Task.Run(ReceiveMessageAsync);
            // запускаем ввод и отправку сообщений
            SendMessageAsync();
        }
        

        // отправка сообщений в группу
        async Task SendMessageAsync()
        {
            using UdpClient sender = new UdpClient();
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
            // отправляем сообщения
            while (true)
            {
                var message = Console.ReadLine(); // сообщение для отправки
                                                  // если введена пустая строка, выходим из цикла и завершаем ввод сообщений
                if (string.IsNullOrWhiteSpace(message)) break;
                // иначе добавляем к сообщению имя пользователя
                message = $"{username}: {message}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                // и отправляем на 127.0.0.1:remotePort
                await sender.SendAsync(data, new IPEndPoint(localAddress, remotePort));
            }
        }
        // отправка сообщений
        async Task ReceiveMessageAsync()
        {
            using UdpClient receiver = new UdpClient(localPort);
            while (true)
            {
                // получаем данные
                var result = await receiver.ReceiveAsync();
                var message = Encoding.UTF8.GetString(result.Buffer);
                // выводим сообщение
                Console.WriteLine(message);
            }
        }
    }
}
