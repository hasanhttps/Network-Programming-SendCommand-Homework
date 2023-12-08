using System.Net;
using System.Text;
using Models.Enums;
using Models.Classes;
using System.Net.Sockets;
using static Server.JsonHandling;

namespace Server {
    internal class Program {

        // Properties

        public static List<Car> Cars = new List<Car>();

        // Main

        static void Main() {

            UdpClient udpClient = new UdpClient(27001);
            var remoteEp = new IPEndPoint(IPAddress.Any, 0);

            Cars = ReadData<List<Car>>("Cars")!;

            while (true) {

                var buffer = new byte[1024];
                dynamic chunks;
                var result = udpClient.Receive(ref remoteEp);
                var request = Encoding.Default.GetString(result);
                var command = System.Text.Json.JsonSerializer.Deserialize<Command>(request);

                switch (command!.Method) {

                    case HttpMethods.GET:
                        request = System.Text.Json.JsonSerializer.Serialize(Cars);
                        buffer = Encoding.Default.GetBytes(request);
                        chunks = buffer.Chunk(ushort.MaxValue - 29);

                        foreach (var chunk in chunks) {
                            udpClient.Send(chunk, chunk.Length, remoteEp);
                        }
                        break;
                    case HttpMethods.POST:
                        Add(command.Car);
                        request = System.Text.Json.JsonSerializer.Serialize(Cars);
                        buffer = Encoding.Default.GetBytes(request);
                        chunks = buffer.Chunk(ushort.MaxValue - 29);

                        foreach (var chunk in chunks){
                            udpClient.Send(chunk, chunk.Length, remoteEp);
                        }
                        break;
                    case HttpMethods.PUT:
                        Update(command.Car);
                        request = System.Text.Json.JsonSerializer.Serialize(Cars);
                        buffer = Encoding.Default.GetBytes(request);
                        chunks = buffer.Chunk(ushort.MaxValue - 29);

                        foreach (var chunk in chunks) {
                            udpClient.Send(chunk, chunk.Length, remoteEp);
                        }
                        break;
                    case HttpMethods.DELETE:
                        Delete(command.Id);
                        request = System.Text.Json.JsonSerializer.Serialize(Cars);
                        buffer = Encoding.Default.GetBytes(request);
                        chunks = buffer.Chunk(ushort.MaxValue - 29);

                        foreach (var chunk in chunks) {
                            udpClient.Send(chunk, chunk.Length, remoteEp);
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        // Functions

        Car? GetById(int id) {

            foreach (Car car in Cars) {
                if (car.Id == id) return car;
            }
            return null;
        }

        static List<Car>? GetAll() {
            return Cars;
        }

        static bool Add(Car car) {

            foreach(var item in Cars) {
                if (item.Id == car.Id) return false;
            }
            Cars.Add(car);
            WriteData(Cars, "Cars");
            return true;
        }

        static bool Update(Car car) {

            foreach (var item in Cars) {
                if (item.Id == car.Id) {
                    item.CloneFromAnotherInstance(car);
                    return true;
                }
            }
            WriteData(Cars, "Cars");
            return false;
        }

        static bool Delete(int id) {
            foreach (var item in Cars) {
                if (item.Id == id) {
                    Cars.Remove(item);
                    return true;
                }
            }
            WriteData(Cars, "Cars");
            return false;
        }
    }
}