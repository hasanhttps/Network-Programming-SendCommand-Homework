using System.Net;
using System.Text;
using Models.Enums;
using Models.Classes;
using System.Text.Json;
using System.Net.Sockets;

namespace Client {
    internal class Program {
        static void Main() {
            UdpClient udpClient = new UdpClient();
            var connectEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            
            while (true) {

                dynamic request, jsonformat, buffer;
                string marka, model, vin, color, id, year;

                Console.WriteLine("1. Get all cars: ");
                Console.WriteLine("2. Add car: ");
                Console.WriteLine("3. Update car: ");
                Console.WriteLine("4. Delete car: ");

                Console.WriteLine("Please enter the operation id (1-4): ");
                var choose = Console.ReadLine();

                switch (choose) {
                    case "1":
                        request = new Command() { Method = HttpMethods.GET };
                        jsonformat = JsonSerializer.Serialize(request);
                        buffer = Encoding.Default.GetBytes(jsonformat);
                        udpClient.Send(buffer, buffer.Length, connectEP);
                        break;
                    case "2":
                        Console.WriteLine("Please enter the car id: ");
                        id = Console.ReadLine();
                        Console.WriteLine("Please enter the car marka: ");
                        marka = Console.ReadLine();
                        Console.WriteLine("Please enter the car model: ");
                        model = Console.ReadLine();
                        Console.WriteLine("Please enter the car VIN: ");
                        vin = Console.ReadLine();
                        Console.WriteLine("Please enter the car color: ");
                        color = Console.ReadLine();
                        Console.WriteLine("Please enter the car year: ");
                        year = Console.ReadLine();

                        request = new Command() { Method = HttpMethods.POST, Car = new Car(int.Parse(id), int.Parse(year), model, marka, vin, color) };
                        jsonformat = JsonSerializer.Serialize(request);
                        buffer = Encoding.Default.GetBytes(jsonformat);
                        udpClient.Send(buffer, buffer.Length, connectEP);
                        break;
                    case "3":
                        Console.WriteLine("Please enter the car id: ");
                        id = Console.ReadLine();
                        Console.WriteLine("Please enter the car marka: ");
                        marka = Console.ReadLine();
                        Console.WriteLine("Please enter the car model: ");
                        model = Console.ReadLine();
                        Console.WriteLine("Please enter the car VIN: ");
                        vin = Console.ReadLine();
                        Console.WriteLine("Please enter the car color: ");
                        color = Console.ReadLine();
                        Console.WriteLine("Please enter the car year: ");
                        year = Console.ReadLine();

                        request = new Command() { Method = HttpMethods.PUT, Car = new Car(int.Parse(id), int.Parse(year), model, marka, vin, color) };
                        jsonformat = JsonSerializer.Serialize(request);
                        buffer = Encoding.Default.GetBytes(jsonformat);
                        udpClient.Send(buffer, buffer.Length, connectEP);
                        break;
                    case "4":
                        Console.WriteLine("Please enter the car id: ");
                        id = Console.ReadLine();

                        request = new Command() { Method = HttpMethods.DELETE, Id = int.Parse(id) } ;
                        jsonformat = JsonSerializer.Serialize(request);
                        buffer = Encoding.Default.GetBytes(jsonformat);
                        udpClient.Send(buffer, buffer.Length, connectEP);
                        break;
                    default:
                        break;
                }
                var bytes = new List<byte>();
                while (true) {
                    var result = udpClient.Receive(ref connectEP);
                    var len = result.Length;
                    bytes.AddRange(result);
                    if (len != ushort.MaxValue - 29) break;
                }
                var cars = Encoding.Default.GetString(bytes.ToArray());
                List<Car>? Cars = JsonSerializer.Deserialize<List<Car>>(cars);

                Console.Clear();
                foreach (var car in Cars)
                    Console.WriteLine(car);
            }
        }
    }
}