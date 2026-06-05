using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Program
{
    static string filePath = "clients.json";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1. Dodaj klienta");
            Console.WriteLine("2. Pokaż klientów");
            Console.WriteLine("3. Aktualizuj dane klienta");
            Console.WriteLine("4. Usuń klienta");
            Console.WriteLine("5. Wyjdź");
            Console.Write("Wybierz opcję: ");

            ConsoleKey optionKey = Console.ReadKey(true).Key;
            switch (optionKey)
            {
                case ConsoleKey.D1:
                    AddClient();
                    break;
                case ConsoleKey.D2:
                    ShowClients();
                    break;
                case ConsoleKey.D3:
                    UpdateClient();
                    break;
                case ConsoleKey.D4:
                    DeleteClient();
                    break;
                case ConsoleKey.D5:
                    return;
                default:
                    Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                    break;
            }
        }
    }

    static void AddClient()
    {
        List<Client> clients = LoadClients();
        Console.Write("\nPodaj imię klienta: ");
        string name = Console.ReadLine();
        Console.Write("Podaj email klienta: ");
        string email = Console.ReadLine();
        Console.Write("Podaj numer telefonu klienta: ");
        string numberPhoneInput = Console.ReadLine();
        if(!int.TryParse(numberPhoneInput, out int numberPhone))
        {
            Console.WriteLine("Nieprawidłowy numer telefonu.");
            return;
        }
        var client = new Client
        {
            Id = clients.Count > 0 ? clients.Max(c => c.Id) + 1 : 1,
            Name = name,
            Email = email,
            NumberPhone = numberPhone
        };
        clients.Add(client);
        SaveClients(clients);
    }

    static void ShowClients()
    {
        Console.WriteLine("\n=== Lista klientów ===\n");
        List<Client> clients = LoadClients();
        if (clients.Count == 0)
        {
            Console.WriteLine("Brak klientów.");
            return;
        }
        foreach(var client in clients)
        {
            Console.WriteLine($"\nID klienta: {client.Id}\nImię: {client.Name}\nEmail: {client.Email}\nNumer telefonu: {client.NumberPhone}");
        }
    }
    static void UpdateClient()
    {
        List<Client> clients = LoadClients();
        Console.WriteLine("\nPodaj ID klienta do aktualizacji: ");
        string idInput = Console.ReadLine();
        if(!int.TryParse(idInput, out int id))
        {
            Console.WriteLine("Błędne ID");
            return;
        }
        Client client = clients.FirstOrDefault(client => client.Id == id);
        if(client == null)
        {
            Console.WriteLine("Nie znaleziono klienta o podanym ID.");
            return;
        }
        Console.WriteLine($"Edytujesz klienta: {client.Name} | {client.Email} | {client.NumberPhone})");
        Console.Write("Nowe imię: (ENTER aby zostawić bez zmian)");
        string newName = Console.ReadLine();
        Console.Write("Nowy email: (ENTER aby zostawić bez zmian)");
        string newEmail = Console.ReadLine();
        Console.Write("Nowy numer telefonu: (ENTER aby zostawić bez zmian)");
        string newNumberPhoneInput = Console.ReadLine();

        if (!string.IsNullOrEmpty(newName))
        {
            client.Name = newName;
        }
        if(!string.IsNullOrEmpty(newEmail))
        {
            client.Email = newEmail;
        }

        if (!string.IsNullOrEmpty(newNumberPhoneInput))
        {
            if ((int.TryParse(newNumberPhoneInput, out int newNumberPhone)))
            {
                client.NumberPhone = newNumberPhone;
            }
            else
            {
                Console.WriteLine("Nieprawidłowy numer telefonu. Numer telefonu pozostaje bez zmian.");
            }
        }
        SaveClients(clients);
        Console.WriteLine("Zmiany zapisano pomyślnie.");

    }

    static void DeleteClient()
    {
        List<Client> clients = LoadClients();
        Console.WriteLine("Podaj ID klienta którego chcesz usunąć z listy");
        string idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int id)){
            Console.WriteLine("Błędne ID");
            return;
        }
        Client client = clients.FirstOrDefault(client => client.Id == id);
        if (client == null) {
            Console.WriteLine("Nie znaleziono klienta o podanym ID.");
            return;
        }
        Console.WriteLine($"Usuwasz klienta {client.Name} | {client.Email} | {client.NumberPhone}");
        Console.WriteLine("Czy napewno chcesz usunąc tego klienta z listy? (t/n)");
        ConsoleKey key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.T) {
            clients.Remove(client);
            SaveClients(clients);
            Console.WriteLine("Klient został usunięty z listy.");
        }
        else
        {
            Console.WriteLine("Anulowano");
        }

    }
    static List<Client> LoadClients()
    {
        if (!File.Exists(filePath))
        {
            return new List<Client>();
        }
        string json = File.ReadAllText(filePath);
        return string.IsNullOrWhiteSpace(json) ? new List<Client>() : JsonSerializer.Deserialize<List<Client>>(json) ?? new List<Client>(); 
    }

    static void SaveClients(List<Client> clients)
    {
        string json = JsonSerializer.Serialize(clients, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(filePath, json);
    }
}

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int NumberPhone { get; set; }
}