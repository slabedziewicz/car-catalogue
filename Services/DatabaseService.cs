using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using Car_Catalogue_Desktop.Models;

namespace Car_Catalogue_Desktop.Services;

public class DatabaseService {
    private readonly string _dbPath;

    public DatabaseService() {
        var projectRoot = GetProjectRoot();
        _dbPath = Path.Combine(projectRoot, "database.sqlite");
        InitializeDatabase();
    }

    private string GetProjectRoot() {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !dir.GetFiles("*.csproj").Any()) {
            dir = dir.Parent;
        }
        return dir?.FullName ?? AppContext.BaseDirectory;
    }

    private void InitializeDatabase() {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Cars (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Brand TEXT NOT NULL,
                Model TEXT NOT NULL,
                Year INTEGER NOT NULL,
                BodyType TEXT,
                TopSpeed INTEGER,
                HorsePower INTEGER,
                Torque INTEGER,
                EngineCapacity REAL,
                ZeroToHundred REAL,
                Weight INTEGER
            );
        ";
        command.ExecuteNonQuery();
    }

    public List<Car> GetAllCars() {
        var cars = new List<Car>();
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Cars";

        using var reader = command.ExecuteReader();
        while (reader.Read()) {
            cars.Add(new Car {
                Id = reader.GetInt32(0),
                Brand = reader.GetString(1),
                Model = reader.GetString(2),
                Year = reader.GetInt32(3),
                BodyType = reader.IsDBNull(4) ? null : reader.GetString(4),
                TopSpeed = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                HorsePower = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                Torque = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                EngineCapacity = reader.IsDBNull(8) ? null : reader.GetDouble(8),
                ZeroToHundred = reader.IsDBNull(9) ? null : reader.GetDouble(9),
                Weight = reader.IsDBNull(10) ? null : reader.GetInt32(10)
            });
        }

        return cars;
    }

    public void AddCar(Car car) {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Cars (Brand, Model, Year, BodyType, TopSpeed, HorsePower, Torque, EngineCapacity, ZeroToHundred, Weight)
            VALUES ($Brand, $Model, $Year, $BodyType, $TopSpeed, $HorsePower, $Torque, $EngineCapacity, $ZeroToHundred, $Weight);
            SELECT last_insert_rowid();
        ";
        
        command.Parameters.AddWithValue("$Brand", car.Brand);
        command.Parameters.AddWithValue("$Model", car.Model);
        command.Parameters.AddWithValue("$Year", car.Year);
        command.Parameters.AddWithValue("$BodyType", car.BodyType ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$TopSpeed", car.TopSpeed ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$HorsePower", car.HorsePower ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$Torque", car.Torque ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$EngineCapacity", car.EngineCapacity ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$ZeroToHundred", car.ZeroToHundred ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$Weight", car.Weight ?? (object)DBNull.Value);

        var result = command.ExecuteScalar();
        long lastId = result != null ? (long)result : 0;
        car.Id = (int)lastId;
    }

    public void UpdateCar(Car car) {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Cars
            SET Brand = $Brand,
                Model = $Model,
                Year = $Year,
                BodyType = $BodyType,
                TopSpeed = $TopSpeed,
                HorsePower = $HorsePower,
                Torque = $Torque,
                EngineCapacity = $EngineCapacity,
                ZeroToHundred = $ZeroToHundred,
                Weight = $Weight
            WHERE Id = $Id
        ";
        
        command.Parameters.AddWithValue("$Id", car.Id);
        command.Parameters.AddWithValue("$Brand", car.Brand);
        command.Parameters.AddWithValue("$Model", car.Model);
        command.Parameters.AddWithValue("$Year", car.Year);
        command.Parameters.AddWithValue("$BodyType", car.BodyType ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$TopSpeed", car.TopSpeed ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$HorsePower", car.HorsePower ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$Torque", car.Torque ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$EngineCapacity", car.EngineCapacity ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$ZeroToHundred", car.ZeroToHundred ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$Weight", car.Weight ?? (object)DBNull.Value);

        command.ExecuteNonQuery();
    }

    public void DeleteCar(int id) {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Cars WHERE Id = $Id";
        command.Parameters.AddWithValue("$Id", id);

        command.ExecuteNonQuery();
    }
}
