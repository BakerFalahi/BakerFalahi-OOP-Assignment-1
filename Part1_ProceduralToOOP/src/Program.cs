var store = new OrderStore();
store.Seed();
new Menu(store).Run();

class Customer
{
    public int Id { get; }
    public string Name { get; }
    public string Phone { get; }

    public Customer(int id, string name, string phone)
    {
        Id = id;
        Name = RequireText(name, "Customer name");
        Phone = RequireText(phone, "Phone");
    }

    static string RequireText(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{field} is required.");

        return value.Trim();
    }

    public override string ToString() => $"{Id}: {Name} ({Phone})";
}

class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; private set; }

    public Product(int id, string name, decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be positive.");

        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Product name is required.") : name.Trim();
        Price = price;
    }

    public override string ToString() => $"{Id}: {Name} - {Price:C}";
}

class OrderLine
{
    public Product Product { get; }
    public int Quantity { get; }
    public decimal LineTotal => Product.Price * Quantity;

    public OrderLine(Product product, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.");

        Product = product;
        Quantity = quantity;
    }
}

class Order
{
    readonly List<OrderLine> lines = [];

    public int Id { get; }
    public Customer Customer { get; }
    public bool IsPaid { get; private set; }
    public IReadOnlyList<OrderLine> Lines => lines;
    public decimal Total => lines.Sum(line => line.LineTotal);

    public Order(int id, Customer customer)
    {
        Id = id;
        Customer = customer;
    }

    public void AddLine(Product product, int quantity)
    {
        lines.Add(new OrderLine(product, quantity));
    }

    public void Pay()
    {
        if (lines.Count == 0)
            throw new InvalidOperationException("Cannot pay for an empty order.");

        IsPaid = true;
    }

    public override string ToString()
    {
        var status = IsPaid ? "Paid" : "Unpaid";
        return $"Order {Id} for {Customer.Name}: {Total:C} ({status})";
    }
}

class OrderStore
{
    readonly List<Customer> customers = [];
    readonly List<Product> products = [];
    readonly List<Order> orders = [];
    int nextCustomerId = 1;
    int nextProductId = 1;
    int nextOrderId = 1;

    public IReadOnlyList<Customer> Customers => customers;
    public IReadOnlyList<Product> Products => products;
    public IReadOnlyList<Order> Orders => orders;

    public Customer AddCustomer(string name, string phone)
    {
        var customer = new Customer(nextCustomerId++, name, phone);
        customers.Add(customer);
        return customer;
    }

    public Product AddProduct(string name, decimal price)
    {
        var product = new Product(nextProductId++, name, price);
        products.Add(product);
        return product;
    }

    public Order PlaceOrder(int customerId, List<(int productId, int quantity)> items)
    {
        var customer = FindCustomer(customerId);
        var order = new Order(nextOrderId++, customer);

        foreach (var item in items)
            order.AddLine(FindProduct(item.productId), item.quantity);

        if (order.Lines.Count == 0)
            throw new InvalidOperationException("Order must contain at least one product.");

        orders.Add(order);
        return order;
    }

    public void PayOrder(int orderId)
    {
        FindOrder(orderId).Pay();
    }

    public decimal TotalSales()
    {
        return orders.Where(order => order.IsPaid).Sum(order => order.Total);
    }

    public Customer FindCustomer(int id)
    {
        return customers.FirstOrDefault(customer => customer.Id == id)
            ?? throw new InvalidOperationException("Customer was not found.");
    }

    public Product FindProduct(int id)
    {
        return products.FirstOrDefault(product => product.Id == id)
            ?? throw new InvalidOperationException("Product was not found.");
    }

    public Order FindOrder(int id)
    {
        return orders.FirstOrDefault(order => order.Id == id)
            ?? throw new InvalidOperationException("Order was not found.");
    }

    public void Seed()
    {
        AddCustomer("Mona Ali", "01000000001");
        AddCustomer("Omar Hassan", "01000000002");
        AddProduct("Keyboard", 650);
        AddProduct("Mouse", 300);
        AddProduct("Monitor", 4200);
    }
}

class Menu
{
    readonly OrderStore store;

    public Menu(OrderStore store)
    {
        this.store = store;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Add customer");
            Console.WriteLine("2. Add product");
            Console.WriteLine("3. Place order");
            Console.WriteLine("4. Pay order");
            Console.WriteLine("5. Show customers");
            Console.WriteLine("6. Show products");
            Console.WriteLine("7. Show orders");
            Console.WriteLine("8. Total sales");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1": AddCustomer(); break;
                    case "2": AddProduct(); break;
                    case "3": PlaceOrder(); break;
                    case "4": PayOrder(); break;
                    case "5": ShowCustomers(); break;
                    case "6": ShowProducts(); break;
                    case "7": ShowOrders(); break;
                    case "8": Console.WriteLine($"Total sales: {store.TotalSales():C}"); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    void AddCustomer()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("Phone: ");
        var phone = Console.ReadLine() ?? "";
        Console.WriteLine($"Added {store.AddCustomer(name, phone)}");
    }

    void AddProduct()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("Price: ");
        var price = ReadDecimal();
        Console.WriteLine($"Added {store.AddProduct(name, price)}");
    }

    void PlaceOrder()
    {
        ShowCustomers();
        Console.Write("Customer ID: ");
        var customerId = ReadInt();
        var items = new List<(int productId, int quantity)>();

        while (true)
        {
            ShowProducts();
            Console.Write("Product ID (0 to finish): ");
            var productId = ReadInt();

            if (productId == 0)
                break;

            Console.Write("Quantity: ");
            items.Add((productId, ReadInt()));
        }

        Console.WriteLine(store.PlaceOrder(customerId, items));
    }

    void PayOrder()
    {
        ShowOrders();
        Console.Write("Order ID: ");
        store.PayOrder(ReadInt());
        Console.WriteLine("Order paid.");
    }

    void ShowCustomers()
    {
        foreach (var customer in store.Customers)
            Console.WriteLine(customer);
    }

    void ShowProducts()
    {
        foreach (var product in store.Products)
            Console.WriteLine(product);
    }

    void ShowOrders()
    {
        foreach (var order in store.Orders)
        {
            Console.WriteLine(order);
            foreach (var line in order.Lines)
                Console.WriteLine($"  {line.Product.Name} x {line.Quantity} = {line.LineTotal:C}");
        }
    }

    static int ReadInt()
    {
        if (!int.TryParse(Console.ReadLine(), out var value))
            throw new ArgumentException("Enter a valid number.");

        return value;
    }

    static decimal ReadDecimal()
    {
        if (!decimal.TryParse(Console.ReadLine(), out var value))
            throw new ArgumentException("Enter a valid amount.");

        return value;
    }
}
