# [Meteors] OperationResult 8.0.1


[![](https://img.shields.io/github/license/MhozaifaA/OperationResult)](https://github.com/MhozaifaA/Meteors.OperationResult/blob/master/LICENSE.md)
![](https://img.shields.io/nuget/dt/Meteors.OperationResult)
[![](https://img.shields.io/nuget/v/Meteors.OperationResult)](https://www.nuget.org/packages/Meteors.OperationResult)
![](https://img.shields.io/badge/Unit%20Test-%E2%80%89254%E2%80%89Passed-green)



*Meteors Operation Result came with new and redesigned to accept more than container for status and data. **OperationResult** is isolated but stuck with kernel of your business logic , without if/else and some corrupted code to handle results.*

Install-Package Meteors.OperationResult -Version 8.0.1


<div style="display: flex;"> 

[nuget](https://www.nuget.org/packages/Meteors.OperationResult/) - [medium](https://medium.com/@botchey.mha/build-powerful-operation-result-structure-with-any-transform-layers-c-c310b790865f) - [Source Code](https://github.com/MhozaifaA/OperationResult)

</div>

<!-- ![Meteor logo pack](https://user-images.githubusercontent.com/48151918/175791394-3913f060-5551-435c-adda-5bc487964f1c.png) -->

<p align="center">
<img width="10%" src="https://user-images.githubusercontent.com/48151918/175791394-3913f060-5551-435c-adda-5bc487964f1c.png" />
</p>

> [!Important]
> Old enum Statues(OperationStatues) become record Statuses. [See New Statues](https://github.com/MhozaifaA/Meteors.OperationResult/blob/develop/src/Enum/Statuses.cs)


### Documentation 

* #### Schemas

  - ##### OperationResult/Base Schema

    ▸ Types/Statuses

    `Unknown, Success, Exist, NotExist, Failed, Forbidden, Exception, Unauthorized`

     ▸ Fields/Props

    `Data, IsSuccess, HasException, FullExceptionMessage, HasCutomStatusCode,Message, OperationResultType, Exception, StatusCode`

    ▸ Methods/Func Helper

    `SetSuccess, SetFailed, SetException, SetContent, Append, Set `

    ▸ Implicit

    `(type) act SetContent, `

    `(Tuple(type, message)) act SetContent,`

    `(Tuple(message,type)) act SetFailed,`

    `(result) act SetSuccess,`

    `((result, message)) act SetSuccess,`

    `(exception) as SetException`

    `ToOperationDynamic`

  - ##### _Operation Schema

    ▸ Extensions  (Abstract-Base/Main Class)

    `SetSuccess, SetFailed, SetException, SetContent, Set`

  - ##### Extension Schema

    ▸ `ToOperationResult<T>(),`
    `WithStatusCode<T>(statuscode:int),`
    `WithStatusCodeAsync<T>(statuscode:int),`
    `ToJsonResult<T>(),`
    `ToJsonResult<T>(body:bool),`
    `ToJsonResultAsync<T>(),`
    `ToJsonResultAsync<T>(body:bool),`
    `Collect<T1.....T7>(T2....T7),`
    `Into<T1.....T7,T>((T1....T7),`
    `CollectAsync<T1.....T7>(T2....T7),`
    `IntoAsync<T1.....T7,T>((T1....T7),`
    ​
    
- ### Use Global

```c#
//in program.cs 
OperationResultOptions.IsBody(bool)
OperationResultOptions.IntoBody(operation=> ...)
OperationResultOptions.SerializerSettings(...)
```

- ###  How to use *- `before get operation`* 

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
           operation.Message = $"{user.UserName} failed to access";
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
      	return operation.SetFailed($"{user.UserName} failed to access");
      
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
      	return _Operation.SetFailed($"{user.UserName} failed to access");
      
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
      	return ($"{user.UserName} failed to access",OperationResultTypes.Failed);
      
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
      	return ($"{user.UserName} failed to access",OperationResultTypes.Failed);
      return result;
    }
  ```

- ### How to use - *`After get operation`*

  - Most OperationResult used with WebAPIs and Responses  Sync/Async :

    ```C#
    private IRepository repository;
    public IActionResult Index()
    {
      return repository.Example()...all controlling ...;
    }

    public async Task<IActionResult> Index()
    {
      return await repository.Example()...all controlling ...;
    }
    ```

    - ToJsonResult()

      ```C#
       return repository.Example().ToJsonResult();
       return await repository.Example().ToJsonResultAsync();

      /*
      success 200: { fullName:"Admin" , age:24 }
      other [statuscode]: "message.."
      exception: ""System.Exception. .. . .. line ... . inner exception ...""
      */
      ```

    - ToJsonResult(true)

      ```C#
      return repository.Example().ToJsonResult(isbody:true);
       return await repository.Example().ToJsonResultAsync(isbody:true);

      /*
      success 200: {
      "Data":{"UserName":null,"Password":null},
      "IsSuccess":true, "HasException":false,
      "FullExceptionMessage":null,"Message":"message..",
      "Status":200,"StatusCode":200
      }

      other [statuscode]: {
      "Data":null,
      "IsSuccess":false,"HasException":false,
      "FullExceptionMessage":null,"Message":"message..",
      "Status":[statuscode],"StatusCode":[statuscode]
      }

      exception: {
      "Data":null,
      "IsSuccess":false,"HasException":true,
      "FullExceptionMessage":"System.Exception. .. . .. line ... . inner exception ...","Message":"message..",
      "Status":500,"StatusCode":[statuscode]
      }
      */
      ```

    - WithStatusCode()

      ```c#
      return repository.Example().WithStatusCode(415).ToJsonResult();
      return await repository.Example().WithStatusCodeAsync(415).ToJsonResultAsync();

      /*
      success 415: "expected body"
      other 415: "expected body"
      exception 415: "expected body"
      */
      ```

    - Collect

      ```c#
      {
           // value: Tuple (operation1,operation3.....)
           Operation1().Collect(Operation2(),Operation3()...) 
            
             // value: Tuple (operation1,operation3.....)
           (await Operation1Async()).Collect(await Operation2Async(),await Operation3Async()...) 
           
           // value:Task Tuple ( operation1,operation3.....)
           Operation1Async().CollectAsync(Operation2Async(),Operation3Async()...) 
       
      }

      ```

    - Into   [see](https://medium.com/@botchey.mha/build-powerful-operation-result-structure-with-any-transform-layers-c-c310b790865f)

      ```C#
      /*
      success[Success,Exit,NotExit] 200:  OperationResult<>(data)
      failed [Failed,Forbidden,Unauthorized] 400: "message.."
      exception 500: ""System.Exception. .. . .. line ... . inner exception ...""
      */
         Operation1().Into(o=>o) 
           
        /*
      success[Success,Exit,NotExit] 200:  OperationResult<Foo>(newdata)
      failed [Failed,Forbidden,Unauthorized] 400: "message.."
      exception 500: ""System.Exception. .. . .. line ... . inner exception ...""
      */
         Operation1().Into(o=> new Foo{  
         Result = o.Data,
           StatusMessage =  o.Status.ToString()
         }) 
           
           /* Async */
           
        Operation1Async().IntoAsync(o=>o) 
           
      ```

    - Collect.Into

      ```C#
      /*
      success[Success,Exit,NotExit] 200:  (perationResult<foo>(o1,o2)
      failed [Failed,Forbidden,Unauthorized] 400: "message.."
      exception 500: ""System.Exception. .. . .. line ... . inner exception ...""
      */
         Operation1().Collect(Operation2()).Into((o1,o2)=> new foo(){
         
           //do what want with  operation o1 
            //do what want with  operation o2
         
         }) 
           
           
           /*
      success[Success,Exit,NotExit] 200:  OperationResult<foo>(o1,o2)
      failed [Failed,Forbidden,Unauthorized] 400: "message1 + message2 + ... message7"
      exception 500: ""System.Exception. .. . .. line ... . inner exception ...""
      */
         Operation1().Collect(Operation2(),...Operation7()).Into((o1,o2....o7)=> new foo(){
         
           //do what want with  operation o1 
            //do what want with  operation o2 ...... o7
         
         }) 
           
           
           success[Success,Exit,NotExit] 200:  (OperationResult<foo>(o1,o2) , "message..","message...")
      failed [Failed,Forbidden,Unauthorized] 400: "message1 + message2 + ... message7",
      exception 500: ""System.Exception. .. . .. line ... . inner exception ...""
      */
         Operation1Async().CollectAsync(Operation2Async(),...Operation7Async()).IntoAsync((o1,o2....o7)=> new foo(){
         
           //do what want with  operation o1 
            //do what want with  operation o2 ...... o7
         
         })     
      ```

    - Collect.Into.ToJsonResult  [see](https://medium.com/@botchey.mha/build-powerful-operation-result-structure-with-any-transform-layers-c-c310b790865f)

      ```C#
      return Operation1().Collect(Operation2(),...Operation7()).Into((o1,....o7)=>{
        new foo(){
          //fill 
        }
      }).ToJsonResult(isbody);
      return Operation1Async().CollectAsync(Operation2Async(),...Operation7Async()).IntoAsync((o1,....o7)=>{
        new foo(){
          Operation1Data = o1.Data,
          :
          Operation7Data = o7.Data
          //fill 
        }
      }).ToJsonResultAsync(isbody);

      /*
      isbody: false

      success[Success,Exit,NotExit] 200:  (OperationResult<foo>(o1,o2..o7)
      failed [Failed,Forbidden,Unauthorized] 400: "message1 + message2 + ... message7"
      exception 500: ""System.Exception. .. . .. line ... . inner exception ...""
      */
      ```


      /*
      isbody: true
    
      success [Success,Exit,NotExit] 200: {
      "Data": { "operation1Data": data1...... "operation7Data": data17} ,
      "IsSuccess":true, "HasException":false,
      "FullExceptionMessage":null,"Message":"message..",
      "Status":200,"StatusCode":200
      }
    
      failed [Failed,Forbidden,Unauthorized] 400: {
      "Data":null,
      "IsSuccess":false,"HasException":false,
      "FullExceptionMessage":null,"Message":"message1 + message2 + ... message7",
      "Status":[statuscode],"StatusCode":[statuscode]
      }
    
      exception: 500 {
      "Data":null,
      "IsSuccess":false,"HasException":true,
      "FullExceptionMessage":"System.Exception. .. . .. line ... . inner exception ...","Message":"message1 + message2 + ... message7",
      "Status":500,"StatusCode":[statuscode]
      }
      */
      ```


## Guide  [Medium](https://medium.com/@botchey.mha/build-powerful-operation-result-structure-with-any-transform-layers-c-c310b790865f)

Collect and Into extensions build to handle multi operations and choice the correct status(**Priority**) with new object.

# **How Priority works**

1- Find Statuses.Exception and change status to exception.

2- Sort OperationResultTypes and join message failed (Failed, Forbidden, Unauthorized) and set status to max failed.

3- Collect message and return Result data with success status.

4- WithStatusCode after done Priority you can control with status.

> **Synchronized and Asynchronized - check **[**Task.WhenAll**](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.whenall?view=net-6.0)



________________________________________________


## Highlighted 💻
- [x] move WithStatusCode extension to Base,
- [x] think if we replace OperationResultbase to -> OperationResult without Base! as abstract (this feature allow to save same concept and add more extensions later)
- [ ] Build interfaces for each prop, that take operation result to make once extension for interface and able to inhrent this extensions (customers build over Meteors).

- [x] Add ctr/method to revice all props as 'Create Instance' (more useing when you have un-know operation-prop take value after plh of condig  ).
        ex:

  ```C# 
    Status status;
    string message = String.Empty();
    :
    Int statuscode...
    :
    
    if(--cond--){ ..//change status } 
    else if( .... cond ---.... }....
    :

    _Operation.Set(status,message,statuscode....); //auto know exactly operationResult
  ```
 - [ ] Global static Isbody, Global static checkin object to serialize , sme to add xtensions for oepration.
 - [ ] Singletone/IEnumrable service inject to control (five 5 services as Status we have for customize).
      ```C#
       readonly ISuccessOperation<>  successOperation; //has custome options and custome global(scop)
       readonly IFailedOperation<>  failedOperation; //has custome options and custome global(scop)
      ```
 - [ ] appsettings attr.
 - [ ] IOptions for (custome default messages, handle statuscodes(->staatus)..,http,.. ).
 - [ ] HttpResponseMessage to OperationResult (support full options).
 - [x] back to implicity (success) 😉 but for limited types (IList<>,ICollection<>,IEnumerable<>,INumber(int,double,...) .Net7.0) not supported (Tuple,Object, dynamic, any not basic) under see (string) 
      ``` C#
      OperationResult<List<Foo>> Get() 
      {  return new ();   };
      ```
 - [x] Stop return null/by default value as Json like as ("", [], {}) 


TODO
- [x] implicit OperationResult<T>(T result)  write doc and find other way, this cause a lot of issues `1.3`
- move some dynamic option in Extension with not able to use overread to be static controling `1.3 -> 2.0`  , some while effect on prof (Test)
- [x] fix HasCustomeStatusCode cond inside to json result and value/ better not to mapping to json only fix internal value status>0 `1.3`
- [x] fix with not set operation types with = 0 `1.3`
- linq to for in priority funcs to increase 200ns
- [X] enable to retuen data with other success status
- [X] build ToProString enum prof `1.3`
- [ ] implicti and explicti from status types to Status code <-> `1.3` `remove issue - this will not be in lib , can be extension or any spsific not fit with only 5 statuses with all statusCode of http `
- [x] find new name for OperationResultTypes `1.3`
- warrning when use unable object in multi thread like (EF Context)
- Helper to convert from any operation type to other with out take data (this too useful when need to get un-success to return operation from other) 'note: this will work agenst ** enable to retuen data with other success status** , later i well see how to enable two side (smart mapping can be)
- Find more pritty way when return generic "_Operation" with out need to generic only fill *base
- write extension methods for Http operation results, this can done by users, but Meteors is some internal using extensions of OperationResult, so they can be public and more what users need
- [x] Support message with SetException with all shape , look like (exception , message).

**Feature [X] will braking change and effect in some features**



> This lib belongs to the **Meteors**,
> Meteorites helps you write less and clean code with the power of design patterns and full support for the most popular programming for perpetual projects
>
> All you need in your project is to use meteorites,
> Simplicity is one in all,

