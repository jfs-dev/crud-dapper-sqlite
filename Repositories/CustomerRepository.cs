using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;

using crud_dapper_sqlite.Models;

namespace crud_dapper_sqlite.Repositories;

public class CustomerRepository(string dbPath) : IDisposable
{
    private readonly IDbConnection _dbConnection = new SqliteConnection($"Data Source={dbPath}");

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _dbConnection.Query<Customer>("SELECT * FROM Customers");
    }

    public Customer? GetCustomerById(int id)
    {
        return _dbConnection.QueryFirstOrDefault<Customer>("SELECT * FROM Customers WHERE Id = @Id", new { Id = id });
    }

    public Customer CreateCustomer(Customer customer)
    {
        customer.Id = _dbConnection.QuerySingle<int>("INSERT INTO Customers (Name, Email) VALUES (@Name, @Email); SELECT LAST_INSERT_ROWID();", customer);
        
        return customer;
    }

    public void UpdateCustomer(Customer customer)
    {
        _dbConnection.Execute("UPDATE Customers SET Name = @Name, Email = @Email WHERE Id = @Id", customer);
    }

    public void DeleteCustomer(int id)
    {
        _dbConnection.Execute("DELETE FROM Customers WHERE Id = @Id", new { Id = id });
    }

    public void Dispose() => _dbConnection.Dispose();
}
