using System;
using Models.Enums;

namespace Models.Classes;

[Serializable]
public class Command {

    // Properties

    public int Id { get; set; }
    public Car Car { get; set; }
    public HttpMethods Method { get; set; }
}