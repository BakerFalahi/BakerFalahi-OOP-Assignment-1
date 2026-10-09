var hotel = new Hotel();
var guest = new Guest(1, "Sara Nabil", "01012345678");
var room = new Room(205, RoomType.Double, 1200);

hotel.AddGuest(guest);
hotel.AddRoom(room);

var reservation = hotel.CreateReservation(1001, guest.GuestId, room.RoomNumber, new DateOnly(2026, 10, 10), new DateOnly(2026, 10, 13));
reservation.Confirm();
reservation.CheckIn();

Console.WriteLine($"{guest.FullName} reserved room {reservation.Room.RoomNumber}");
Console.WriteLine($"Status: {reservation.Status}");
Console.WriteLine($"Total cost: {reservation.TotalCost:C}");

enum RoomType
{
    Single,
    Double,
    Suite
}

enum ReservationStatus
{
    Pending,
    Confirmed,
    CheckedIn,
    CheckedOut,
    Cancelled
}

class Guest
{
    readonly List<Reservation> reservations = [];

    public int GuestId { get; }
    public string FullName { get; }
    public string PhoneNumber { get; }
    public IReadOnlyList<Reservation> Reservations => reservations;

    public Guest(int guestId, string fullName, string phoneNumber)
    {
        GuestId = guestId;
        FullName = Required(fullName, "Full name");
        PhoneNumber = Required(phoneNumber, "Phone number");
    }

    internal void AddReservation(Reservation reservation)
    {
        reservations.Add(reservation);
    }

    static string Required(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{field} is required.");

        return value.Trim();
    }
}

class Room
{
    public int RoomNumber { get; }
    public RoomType RoomType { get; }
    public decimal NightlyRate { get; private set; }
    public bool IsUnderMaintenance { get; private set; }

    public Room(int roomNumber, RoomType roomType, decimal nightlyRate)
    {
        RoomNumber = roomNumber;
        RoomType = roomType;
        ChangeNightlyRate(nightlyRate);
    }

    public void ChangeNightlyRate(decimal nightlyRate)
    {
        if (nightlyRate <= 0)
            throw new ArgumentException("Nightly rate must be positive.");

        NightlyRate = nightlyRate;
    }

    public void StartMaintenance()
    {
        IsUnderMaintenance = true;
    }

    public void EndMaintenance()
    {
        IsUnderMaintenance = false;
    }
}

class Reservation
{
    public int ReservationId { get; }
    public DateOnly CheckInDate { get; }
    public DateOnly CheckOutDate { get; }
    public Room Room { get; }
    public ReservationStatus Status { get; private set; }
    public int Nights => CheckOutDate.DayNumber - CheckInDate.DayNumber;
    public decimal TotalCost => Nights * Room.NightlyRate;

    public Reservation(int reservationId, DateOnly checkInDate, DateOnly checkOutDate, Room room)
    {
        if (checkOutDate <= checkInDate)
            throw new ArgumentException("Check-out date must be after check-in date.");

        if (room.IsUnderMaintenance)
            throw new InvalidOperationException("Cannot reserve a room under maintenance.");

        ReservationId = reservationId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        Room = room;
        Status = ReservationStatus.Pending;
    }

    public bool IsActiveFor(Room room, DateOnly checkIn, DateOnly checkOut)
    {
        if (Room != room)
            return false;

        if (Status is ReservationStatus.Cancelled or ReservationStatus.CheckedOut)
            return false;

        return checkIn < CheckOutDate && checkOut > CheckInDate;
    }

    public void Confirm()
    {
        if (Status != ReservationStatus.Pending)
            throw new InvalidOperationException("Only pending reservations can be confirmed.");

        Status = ReservationStatus.Confirmed;
    }

    public void CheckIn()
    {
        if (Status != ReservationStatus.Confirmed)
            throw new InvalidOperationException("Reservation must be confirmed before check-in.");

        Status = ReservationStatus.CheckedIn;
    }

    public void CheckOut()
    {
        if (Status != ReservationStatus.CheckedIn)
            throw new InvalidOperationException("Only checked-in reservations can be checked out.");

        Status = ReservationStatus.CheckedOut;
    }

    public void Cancel()
    {
        if (Status is not ReservationStatus.Pending and not ReservationStatus.Confirmed)
            throw new InvalidOperationException("Only pending or confirmed reservations can be cancelled.");

        Status = ReservationStatus.Cancelled;
    }
}

class Hotel
{
    readonly List<Guest> guests = [];
    readonly List<Room> rooms = [];
    readonly List<Reservation> reservations = [];

    public void AddGuest(Guest guest)
    {
        guests.Add(guest);
    }

    public void AddRoom(Room room)
    {
        rooms.Add(room);
    }

    public Reservation CreateReservation(int reservationId, int guestId, int roomNumber, DateOnly checkIn, DateOnly checkOut)
    {
        var guest = guests.FirstOrDefault(item => item.GuestId == guestId)
            ?? throw new InvalidOperationException("Guest was not found.");

        var room = rooms.FirstOrDefault(item => item.RoomNumber == roomNumber)
            ?? throw new InvalidOperationException("Room was not found.");

        if (reservations.Any(item => item.IsActiveFor(room, checkIn, checkOut)))
            throw new InvalidOperationException("Room is already booked for these dates.");

        var reservation = new Reservation(reservationId, checkIn, checkOut, room);
        reservations.Add(reservation);
        guest.AddReservation(reservation);
        return reservation;
    }
}
