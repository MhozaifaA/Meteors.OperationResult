using OperationContext;
using Xunit;

namespace OperationResult.Tests
{

    public class UnitTest
    {
        class Fee
        {      
            public Fee(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }
            public string Name { get; set; }
        }
        private OperationResult<Fee> ExecuteFun() => new ();

        [Fact]
        public void ToJsonResult()
        {
           var result = ExecuteFun().ToJsonResult();
        }
    }
}