<<<<<<< HEAD
# BakerFalahi-OOP-Assignment-1
Assignment repo for assignment/1-5 (OOP Assignment 1)
=======
# Object-Oriented Programming Assignment 1

## Student Information

- **Name:**
- **ID:**

## Overview

This repository contains the complete submission for the first Object-Oriented Programming assignment. The work is organized into four parts that demonstrate core OOP design skills in C#, including refactoring procedural code into classes, applying encapsulation and validation, using the Builder design pattern, and solving an algorithmic problem in C#.

The solution is managed through `OOP_Design_Encapsulation_BuilderPattern.sln` and includes three runnable console projects plus a standalone LeetCode solution.

## Assignment Objectives

- Convert procedural logic into an object-oriented design.
- Encapsulate data and protect domain rules through class methods and validation.
- Model real-world entities using clear responsibilities and relationships.
- Apply the Builder Pattern to construct complex objects in a readable and maintainable way.
- Provide a C# solution for a LeetCode problem with accepted-result evidence.

## Project Structure

```text
.
|-- OOP_Design_Encapsulation_BuilderPattern.sln
|-- README.md
|-- Part1_ProceduralToOOP
|   |-- Critique.md
|   `-- src
|       |-- Part1_ProceduralToOOP.csproj
|       `-- Program.cs
|-- Part2_HotelReservationSystem
|   `-- src
|       |-- Part2_HotelReservationSystem.csproj
|       `-- Program.cs
|-- Part3_BuilderPattern
|   |-- Answers.md
|   `-- src
|       |-- Part3_BuilderPattern.csproj
|       `-- Program.cs
`-- Part4_LeetCode
    |-- accepted_screenshot.png
    |-- Solution.cs
    `-- 1679_MaxNumberOfKSumPairs
        `-- Solution.cs
```

## Part 1: Procedural Code to OOP

Part 1 refactors an order-management program into an object-oriented C# console application. The solution separates the domain into focused classes:

- `Customer` stores customer identity and contact information.
- `Product` stores product information and pricing.
- `OrderLine` represents a product and quantity inside an order.
- `Order` owns order behavior, line totals, payment status, and total calculation.
- `OrderStore` manages collections of customers, products, and orders.
- `Menu` provides the console interaction layer.

The implementation demonstrates encapsulation by keeping internal collections private, exposing read-only views where appropriate, and validating important rules such as positive prices, positive quantities, non-empty orders, and valid input values.

`Part1_ProceduralToOOP/Critique.md` explains the weaknesses of the original procedural version, including shared global state, behavior separated from data, weak validation, and poor extensibility.

## Part 2: Hotel Reservation System

Part 2 implements a small hotel reservation domain model using encapsulation and validation. The main classes and enums are:

- `Guest`
- `Room`
- `Reservation`
- `Hotel`
- `RoomType`
- `ReservationStatus`

The system supports adding guests and rooms, creating reservations, checking room availability, calculating total reservation cost, and moving reservations through valid status transitions such as pending, confirmed, checked in, checked out, and cancelled.

Important business rules are protected inside the model. Examples include rejecting invalid date ranges, preventing reservations for rooms under maintenance, preventing overlapping bookings, and allowing status changes only when the reservation is in the correct state.

## Part 3: Builder Pattern

Part 3 demonstrates the Builder Pattern by constructing an invoice with nested related data. Instead of relying on a long constructor with many parameters, the solution uses composed builders:

- `InvoiceBuilder` builds the full invoice.
- `AddressBuilder` builds reusable billing and shipping address objects.
- `OrderBuilder` builds payment and order amount details.

The builder implementation improves readability, groups related data together, and centralizes validation for required fields and valid amounts. `Part3_BuilderPattern/Answers.md` also explains why a large constructor is difficult to maintain and why composed builders are a better design for complex objects.

## Part 4: LeetCode Solution

Part 4 contains a C# solution for LeetCode problem `1679 - Max Number of K-Sum Pairs`.

The implemented approach sorts the input array and uses a two-pointer technique to count the maximum number of valid pairs whose sum equals `k`. The solution is included in:

- `Part4_LeetCode/Solution.cs`
- `Part4_LeetCode/1679_MaxNumberOfKSumPairs/Solution.cs`

An accepted-result screenshot is included as `Part4_LeetCode/accepted_screenshot.png`.

## Technologies Used

- C#
- .NET 10.0
- JetBrains Rider / Visual Studio compatible solution structure
- Console applications

## Build and Run

Restore and build the full solution:

```bash
dotnet restore OOP_Design_Encapsulation_BuilderPattern.sln
dotnet build OOP_Design_Encapsulation_BuilderPattern.sln
```

Run each console project:

```bash
dotnet run --project Part1_ProceduralToOOP/src/Part1_ProceduralToOOP.csproj
dotnet run --project Part2_HotelReservationSystem/src/Part2_HotelReservationSystem.csproj
dotnet run --project Part3_BuilderPattern/src/Part3_BuilderPattern.csproj
```

## Notes

- Each assignment part is kept in its own folder for clarity.
- The first three parts are runnable C# console projects.
- The LeetCode section is stored separately because it follows the platform's expected `Solution` class format.
- Generated build folders such as `bin` and `obj` are not required for understanding the assignment and can be regenerated by building the solution.
>>>>>>> 77c8ba7 ([S1-A5] OOP Assignment 1)
