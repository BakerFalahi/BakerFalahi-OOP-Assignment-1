var invoice = new InvoiceBuilder()
    .WithInvoiceId(5001)
    .ForCustomer("Nour Samy", "nour@example.com", "01055555555")
    .WithBillingAddress(address => address
        .WithStreet("15 Nile Street")
        .WithCity("Cairo")
        .WithState("Cairo")
        .WithZipCode("11511")
        .WithCountry("Egypt"))
    .WithShippingAddress(address => address
        .WithStreet("20 Tahrir Square")
        .WithCity("Cairo")
        .WithState("Cairo")
        .WithZipCode("11512")
        .WithCountry("Egypt"))
    .WithOrder(order => order
        .OnDate(new DateOnly(2026, 10, 2))
        .PaidBy("Card")
        .InCurrency("EGP")
        .WithAmounts(1000, 100, 126))
    .Build();

Console.WriteLine($"Invoice {invoice.InvoiceId} for {invoice.CustomerName}: {invoice.Order.TotalAmount} {invoice.Order.Currency}");

record Address(string Street, string City, string State, string ZipCode, string Country);

record OrderInfo(DateOnly OrderDate, string PaymentMethod, string Currency, decimal SubTotal, decimal DiscountAmount, decimal TaxAmount)
{
    public decimal TotalAmount => SubTotal - DiscountAmount + TaxAmount;
}

record Invoice(
    int InvoiceId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    Address BillingAddress,
    Address ShippingAddress,
    OrderInfo Order);

class AddressBuilder
{
    string? street;
    string? city;
    string? state;
    string? zipCode;
    string? country;

    public AddressBuilder WithStreet(string value)
    {
        street = value;
        return this;
    }

    public AddressBuilder WithCity(string value)
    {
        city = value;
        return this;
    }

    public AddressBuilder WithState(string value)
    {
        state = value;
        return this;
    }

    public AddressBuilder WithZipCode(string value)
    {
        zipCode = value;
        return this;
    }

    public AddressBuilder WithCountry(string value)
    {
        country = value;
        return this;
    }

    public Address Build()
    {
        return new Address(
            Required(street, "Street"),
            Required(city, "City"),
            Required(state, "State"),
            Required(zipCode, "Zip code"),
            Required(country, "Country"));
    }

    static string Required(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{field} is required.");

        return value.Trim();
    }
}

class OrderBuilder
{
    DateOnly? orderDate;
    string? paymentMethod;
    string? currency;
    decimal subTotal;
    decimal discountAmount;
    decimal taxAmount;

    public OrderBuilder OnDate(DateOnly value)
    {
        orderDate = value;
        return this;
    }

    public OrderBuilder PaidBy(string value)
    {
        paymentMethod = value;
        return this;
    }

    public OrderBuilder InCurrency(string value)
    {
        currency = value;
        return this;
    }

    public OrderBuilder WithAmounts(decimal subTotal, decimal discountAmount, decimal taxAmount)
    {
        if (subTotal < 0 || discountAmount < 0 || taxAmount < 0)
            throw new ArgumentException("Amounts cannot be negative.");

        this.subTotal = subTotal;
        this.discountAmount = discountAmount;
        this.taxAmount = taxAmount;
        return this;
    }

    public OrderInfo Build()
    {
        return new OrderInfo(
            orderDate ?? throw new InvalidOperationException("Order date is required."),
            Required(paymentMethod, "Payment method"),
            Required(currency, "Currency"),
            subTotal,
            discountAmount,
            taxAmount);
    }

    static string Required(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{field} is required.");

        return value.Trim();
    }
}

class InvoiceBuilder
{
    int? invoiceId;
    string? customerName;
    string? customerEmail;
    string? customerPhone;
    Address? billingAddress;
    Address? shippingAddress;
    OrderInfo? order;

    public InvoiceBuilder WithInvoiceId(int value)
    {
        invoiceId = value;
        return this;
    }

    public InvoiceBuilder ForCustomer(string name, string email, string phone)
    {
        customerName = name;
        customerEmail = email;
        customerPhone = phone;
        return this;
    }

    public InvoiceBuilder WithBillingAddress(Func<AddressBuilder, AddressBuilder> build)
    {
        billingAddress = build(new AddressBuilder()).Build();
        return this;
    }

    public InvoiceBuilder WithShippingAddress(Func<AddressBuilder, AddressBuilder> build)
    {
        shippingAddress = build(new AddressBuilder()).Build();
        return this;
    }

    public InvoiceBuilder WithOrder(Func<OrderBuilder, OrderBuilder> build)
    {
        order = build(new OrderBuilder()).Build();
        return this;
    }

    public Invoice Build()
    {
        return new Invoice(
            invoiceId ?? throw new InvalidOperationException("Invoice ID is required."),
            Required(customerName, "Customer name"),
            Required(customerEmail, "Customer email"),
            Required(customerPhone, "Customer phone"),
            billingAddress ?? throw new InvalidOperationException("Billing address is required."),
            shippingAddress ?? throw new InvalidOperationException("Shipping address is required."),
            order ?? throw new InvalidOperationException("Order details are required."));
    }

    static string Required(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{field} is required.");

        return value.Trim();
    }
}
