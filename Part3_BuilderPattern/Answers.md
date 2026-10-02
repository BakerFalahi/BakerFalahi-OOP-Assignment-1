# Answers

## Task 3.1

A 20-parameter constructor is hard to read because the call site becomes a long row of values. If two values have the same type, like two strings or two decimal amounts, the compiler cannot tell when they are accidentally swapped. It also becomes painful to change later because adding one optional field forces every constructor call to be checked again.

The deeper problem is that one class may be trying to hold too many loosely related details. Address data and payment/order data are different ideas. Keeping them as one long list makes validation and maintenance harder.

## Task 3.3

The composed builder is better because each smaller builder owns one clear group of data. `AddressBuilder` only knows how to build and validate an address, and `OrderBuilder` only knows the order and payment details.

This also avoids duplicate address validation because the same `AddressBuilder` is reused for both billing and shipping. The final invoice builder becomes easier to read because it is made from complete parts instead of twenty separate values.
