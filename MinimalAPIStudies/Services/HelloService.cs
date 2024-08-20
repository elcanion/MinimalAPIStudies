using MinimalAPIStudies.Interfaces;

namespace MinimalAPIStudies.Services
{
    public class HelloService : IHelloService
    {
        public void Hello() { }

        public string Hello(string message)
        {
            return message;
        }
    }
}
