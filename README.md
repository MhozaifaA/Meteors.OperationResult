# [Meteors] OperationResult 1.3

*Meteors Operation Result came with new and redesigned to accept more than container for status and data. **OperationResult** is isolated but stuck with kernel of your business logic , with out if/else and some corrupted code to handle results.*

Install-Package Meteors.OperationResult -Version 1.0.1

> Soon to lunch with full doc



This lib belongs to the **Meteors**,
Meteorites helps you write less and clean code with the power of design patterns and full support for the most popular programming for perpetual project

All you need in your project is to use meteorites,
Simplicity is one in all,



### APIs





TODO

- fix with not set operation types with = 0 
- linq to for in priority funcs to increase 200ns
- [X]enable to retuen data with other success status
- build ToProString enum prof
- implicti and explicti from status types to Status code <->
- find new name for OperationResultTypes
- warrning when use unable object in multi thread like (EF Context)
- Helper to convert from any operation type to other with out take data (this too useful when need to get un-success to return operation from other) 'note: this will work agenst ** enable to retuen data with other success status** , later i well see how to enable two side (smart mapping can be)
- Find more pritty way when return generic "_Operation" with out need to generic only fill *base

**Feature [X] will braking change and effect in some features**
