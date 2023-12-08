using System;

namespace Models.Classes;

[Serializable]
public class Car {

    // Properties
    public int Id { get; set; }
    public int Year { get; set; }
    public string VIN { get; set; }
    public string Model { get; set; }
    public string Marka { get; set; }
    public string Color { get; set; }


    // Constructor

    public Car(int id, int year, string model, string marka, string vin, string color) { 

        Id = id;
        Year = year;
        Model = model;
        Marka = marka;
        VIN = vin;
        Color = color;
    }

    // Functions

    public override string ToString() {
        return $"Id: {Id}\nMarka: {Marka}\nModel: {Model}\nYear: {Year}\nVIN: {VIN}\nColor: {Color}\n\n";
    }

    public void CloneFromAnotherInstance(Car car) {

        Year = car.Year;
        VIN = car.VIN;
        Model = car.Model;
        Marka = car.Marka;
        Color = car.Color;
    }
}