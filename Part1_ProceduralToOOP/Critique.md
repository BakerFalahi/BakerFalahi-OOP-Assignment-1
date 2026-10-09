# Critique

The original program puts all customers, products, orders, and order lines in global variables. That makes every function depend on the same shared state, so one change in one place can accidentally affect the whole program. It also makes the code harder to test because there is no easy way to create a clean copy of the system for one test.

The behavior is separated from the data it works on. For example, order logic is handled by free functions instead of belonging to an `Order` class. This means the rules for an order are spread around the file instead of being kept with the order itself.

There are no real domain objects. Customers, products, and orders are just loose pieces of data. This makes it easier to pass the wrong ID, update the wrong list, or forget to keep related data in sync.

Validation is weak because the data can be changed directly from many places. Invalid orders, bad quantities, missing customers, or missing products can appear if one function forgets to check something.

The program is also hard to extend. Adding a new feature, like discounts or order status, would require searching through many functions and global arrays. With classes, the new behavior could be added closer to the data it belongs to.
