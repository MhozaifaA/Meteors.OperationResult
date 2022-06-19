# [Meteors] OperationResult 6.1.3 
> `version 1.3 net6.0`


*Meteors Operation Result came with new and redesigned to accept more than container for status and data. **OperationResult** is isolated but stuck with kernel of your business logic , without if/else and some corrupted code to handle results.*

Install-Package Meteors.OperationResult -Version 1.0.1

> Soon to lunch with full doc 60%



This lib belongs to the **Meteors**,
Meteorites helps you write less and clean code with the power of design patterns and full support for the most popular programming for perpetual projects

All you need in your project is to use meteorites,
Simplicity is one in all,



### Documentation 

* #### Schemas

  - ##### OperationResult/Base Schema

    ▸ Types/Statuses

    `Unknown, Success, Exist, NotExist,Failed,Forbidden,Exception,Unauthorized`

     ▸ Fields/Props

    `Data, IsSuccess, HasException, FullExceptionMessage, HasCutomStatusCode,Message, OperationResultType, Exception, StatusCode`

    ▸ Methods/Func Helper

    `SetSuccess, SetFailed, SetException, SetContent`

    ▸ Implicit

    `(type) act SetContent, `

    `(Tuple(type, message)) act SetContent,`

    `(Tuple(message,type)) act SetFailed,`

    `(result) act SetSuccess,`

    `((result, message)) act SetSuccess,`

    `(exception) as SetException`

  - ##### _Operation Schema

    ▸ Extensions  (Abstract-Base/Main Class)

    `SetSuccess, SetFailed, SetException, SetContent`

  ​

- ###  How to use

  ```c#
    public class FooUser { UserName, Password }
    public class FooUserDetails { FullName, Age }

    public OperationResult<FooUserDetails> Example(FooUser user)
  ```

  ​

  > Regular way

  ```c#
    {
      try
      {
        OperationResult<FooUserDetails> operation = new ();
       
        if(!IsCurrect(user))
        {
           operation.OperationResultType = OperationResultTypes.Failed;
           operation.Message = $"{user.UserName} faild to access";
          return operation;
        }
        operation.OperationResultType = OperationResultTypes.Success;
        operation.Message = $"Success to access"; //option
        operation.Data = GetDatails(user);
        return operation;
      }
      catch(Exception e)
      {
        operation.Exception = e;
        operation.OperationResultType = OperationResultTypes.Exception;
        return operation;
      }
    }
  ```

  > Method Way

  ```c#
  {
    try
    {
      OperationResult<FooUserDetails> operation = new ();

      if(!IsCurrect(user))
      	return operation.SetFailed($"{user.UserName} faild to access");
      
      return operation.SetSuccess(result);

    }
    catch(Exception e)
    {
      return operation.SetException(e);
    }
  }
  ```
  > Extension Way

  ``` c#
  {
    try
    { 
      if(!IsCurrect(user))
      	return _Operation.SetFailed($"{user.UserName} faild to access");
      
      return _Operation.SetSuccess(result);
    }
    catch(Exception e)
    {
      return _Operation.SetException(e);
    }
  }
  ```

  > Implicit way

  ```c#
  {
    try
    { 
      if(!IsCurrect(user))
      	return ($"{user.UserName} faild to access",OperationResultTypes.Failed);
      
      return result; // mean success
    }
    catch(Exception e)
    {
      return e;
    }
  }
  ```

  You can see benefit when using `Meteors.OperationResult` Ways

  > With Global Exception Handle

  ```c#
   { 
      if(!IsCurrect(user))
      	return ($"{user.UserName} faild to access",OperationResultTypes.Failed);
      return result;
    }
  ```

  ​



TODO
- [x] implicit OperationResult<T>(T result)  write doc and find other way, this cause a lot of issues `1.3`
- move some dynamic option in Extension with not able to use overread to be static controling `1.3 -> 2.0`  , some while effect on prof (Test)
- [x] fix HasCustomeStatusCode cond inside to json result and value/ better not to mapping to json only fix internal value status>0 `1.3`
- [x] fix with not set operation types with = 0 `1.3`
- linq to for in priority funcs to increase 200ns
- [X]enable to retuen data with other success status
- build ToProString enum prof `1.3`
- [ ] implicti and explicti from status types to Status code <-> `1.3` `remove issue - this will not be in lib , can be extension or any spsific not fit with only 5 statuses with all statusCode of http `
- [x] find new name for OperationResultTypes `1.3`
- warrning when use unable object in multi thread like (EF Context)
- Helper to convert from any operation type to other with out take data (this too useful when need to get un-success to return operation from other) 'note: this will work agenst ** enable to retuen data with other success status** , later i well see how to enable two side (smart mapping can be)
- Find more pritty way when return generic "_Operation" with out need to generic only fill *base

**Feature [X] will braking change and effect in some features**
